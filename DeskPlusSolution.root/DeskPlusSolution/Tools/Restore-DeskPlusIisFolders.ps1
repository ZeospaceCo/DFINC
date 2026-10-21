[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [switch]$Apply,
    [string]$BackupDirectory = (Join-Path ([IO.Path]::GetTempPath()) 'DFINC-IIS'),
    [string]$ResultPath
)

$ErrorActionPreference = 'Stop'
$webRoot = [IO.Path]::GetFullPath((Join-Path (Split-Path $PSScriptRoot -Parent) 'DeskPlusEIP')).TrimEnd('\')
$adminAssembly = Join-Path $env:windir 'System32\inetsrv\Microsoft.Web.Administration.dll'
$manager = $null
$result = @{ Success = $false; Converted = 0 }
try {
    Add-Type -Path $adminAssembly
    $manager = [Microsoft.Web.Administration.ServerManager]::new()
    if ($manager.Sites.Count -eq 0) {
        throw 'IIS sites could not be read. Run this script in an administrator PowerShell.'
    }
    $plan = @()
    foreach ($site in $manager.Sites) {
        $parent = $site.Applications | Where-Object {
            $_.Path -ieq '/DeskPlusEIP' -and
            [IO.Path]::GetFullPath([Environment]::ExpandEnvironmentVariables($_.VirtualDirectories['/'].PhysicalPath)).TrimEnd('\') -ieq $webRoot
        }
        if ($null -eq $parent) { continue }

        foreach ($application in @($site.Applications)) {
            if (-not $application.Path.StartsWith($parent.Path + '/', [StringComparison]::OrdinalIgnoreCase)) { continue }

            $relative = $application.Path.Substring($parent.Path.Length + 1).Replace('/', '\')
            $expectedPath = [IO.Path]::GetFullPath((Join-Path $webRoot $relative)).TrimEnd('\')
            if (-not $expectedPath.StartsWith($webRoot + '\', [StringComparison]::OrdinalIgnoreCase)) {
                throw "Application path leaves the DeskPlusEIP folder: $($application.Path)"
            }
            $physicalPath = [IO.Path]::GetFullPath([Environment]::ExpandEnvironmentVariables($application.VirtualDirectories['/'].PhysicalPath)).TrimEnd('\')
            if ($physicalPath -ine $expectedPath) {
                throw "Application has a custom physical mapping; review it first: $($site.Name)$($application.Path)"
            }
            if ($application.VirtualDirectories.Count -ne 1) {
                throw "Application has additional virtual directories; review them first: $($site.Name)$($application.Path)"
            }
            if (-not (Test-Path -LiteralPath $expectedPath -PathType Container)) {
                throw "Physical folder is missing: $expectedPath"
            }
            $plan += [PSCustomObject]@{
                Site = $site.Name
                Application = $application.Path
                PhysicalPath = $physicalPath
                SiteObject = $site
                ApplicationObject = $application
            }
        }
    }

    if ($plan.Count -eq 0) {
        $result.Success = $true
        Write-Output 'No child applications need conversion; DeskPlusEIP already uses folders.'
        return
    }
    $plan | Select-Object Site, Application, PhysicalPath | Format-Table -AutoSize
    if (-not $Apply) {
        Write-Output "Preview only: $($plan.Count) child applications. Run with -Apply in an administrator PowerShell to convert them to folders."
        return
    }
    if (-not $PSCmdlet.ShouldProcess($webRoot, "Convert $($plan.Count) IIS child applications to ordinary folders")) { return }

    $backupPath = Join-Path $BackupDirectory ((Get-Date -Format 'yyyyMMdd-HHmmss') + '-' + [Guid]::NewGuid().ToString('N').Substring(0, 8))
    New-Item -ItemType Directory -Path $backupPath -Force | Out-Null
    Copy-Item -LiteralPath (Join-Path $env:windir 'System32\inetsrv\config\applicationHost.config') -Destination (Join-Path $backupPath 'applicationHost.config')
    $plan | Select-Object Site, Application, PhysicalPath | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath (Join-Path $backupPath 'converted-applications.json') -Encoding UTF8

    foreach ($entry in $plan) {
        $entry.SiteObject.Applications.Remove($entry.ApplicationObject)
    }
    $manager.CommitChanges()
    $result.Success = $true
    $result.Converted = $plan.Count
    $result.Backup = $backupPath
    Write-Output "Converted $($plan.Count) child applications to folders. Backup: $backupPath"
}
catch {
    $result.Error = $_.Exception.Message
    throw
}
finally {
    if ($null -ne $manager) { $manager.Dispose() }
    if ($ResultPath) {
        $result | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath $ResultPath -Encoding UTF8
    }
}
