#Requires -Version 5.1
#Requires -PSEdition Desktop
#Requires -RunAsAdministrator
[CmdletBinding()]
param(
    [string]$ConfigPath,
    [string[]]$IisIdentity = @('IIS APPPOOL\DeskPlusEIP')
)

$ErrorActionPreference = 'Stop'
if ([string]::IsNullOrWhiteSpace($ConfigPath)) { $ConfigPath = Join-Path (Split-Path -Parent $PSScriptRoot) 'DeskPlusEIP\WebAppSettings.config' }
Add-Type -AssemblyName System.Security
[xml]$settingsXml = Get-Content -LiteralPath $ConfigPath
function Read-Setting([string]$Name) {
    $entry = $settingsXml.SelectSingleNode("/appSettings/add[@key='$Name']")
    if (-not $entry -or [string]::IsNullOrWhiteSpace($entry.value)) { throw "Missing setting: $Name" }
    return $entry.value.Trim()
}
$tenantId = ([guid](Read-Setting 'TeamsTenantId')).ToString('D')
$clientId = ([guid](Read-Setting 'TeamsClientId')).ToString('D')
$sender = Read-Setting 'TeamsSenderUserPrincipalName'
$tokenPath = [Environment]::ExpandEnvironmentVariables((Read-Setting 'TeamsChatTokenPath'))
if (-not [IO.Path]::IsPathRooted($tokenPath)) { throw 'TeamsChatTokenPath must be an absolute path.' }
$tokenPath = [IO.Path]::GetFullPath($tokenPath)
$tokenDirectory = [IO.Path]::GetDirectoryName($tokenPath)
$solutionRoot = [IO.Path]::GetFullPath((Split-Path -Parent $PSScriptRoot)).TrimEnd('\') + '\'
if ($tokenDirectory.StartsWith($solutionRoot, [StringComparison]::OrdinalIgnoreCase)) {
    throw 'The token directory must be outside the source and web site directories.'
}
if (-not [IO.Directory]::GetParent($tokenDirectory)) { throw 'A drive root cannot be used as the token directory.' }
if (Test-Path -LiteralPath $tokenDirectory) {
    $tokenName = [IO.Path]::GetFileName($tokenPath)
    foreach ($entry in (Get-ChildItem -LiteralPath $tokenDirectory -Force)) {
        if ($entry.PSIsContainer -or ($entry.Name -notin @($tokenName, "$tokenName.lock", "$tokenName.reauth-mail.sent") -and $entry.Name -notmatch ('^' + [regex]::Escape($tokenName) + '\.[0-9a-f]{32}\.tmp$'))) {
            throw 'The token directory contains unrelated files. Configure a dedicated empty directory before continuing.'
        }
    }
}
# Resolve identities before starting login. No account password is read or stored.
$principals = @([Security.Principal.WindowsIdentity]::GetCurrent().User,
    [Security.Principal.SecurityIdentifier]'S-1-5-18', [Security.Principal.SecurityIdentifier]'S-1-5-32-544')
foreach ($identity in $IisIdentity) {
    $account = New-Object Security.Principal.NTAccount($identity)
    $principals += $account.Translate([Security.Principal.SecurityIdentifier])
}
$source = Join-Path $solutionRoot 'Projects\CommonProject\Zeospace.DeskPlus.Common.BLL\TeamsChatTokenStore.cs'
if (-not (Test-Path -LiteralPath $source)) { $source = Join-Path $PSScriptRoot 'Support\TeamsChatTokenStore.cs' }
if (-not ('Zeospace.DeskPlus.Common.BLL.TeamsChatTokenStore' -as [type])) {
    Add-Type -Path $source -ReferencedAssemblies System.Security.dll
}
$context = [Zeospace.DeskPlus.Common.BLL.TeamsChatTokenStore]::CreateContext($tenantId, $clientId, $sender)
$scope = 'https://graph.microsoft.com/User.Read https://graph.microsoft.com/User.ReadBasic.All https://graph.microsoft.com/Chat.Create https://graph.microsoft.com/ChatMessage.Send offline_access'
$authority = "https://login.microsoftonline.com/$tenantId/oauth2/v2.0"

function Enable-TeamsChat([string]$Path) {
    # Reload current settings so changes made while Microsoft login was in progress are retained.
    $current = New-Object Xml.XmlDocument
    $current.PreserveWhitespace = $true
    $current.Load($Path)
    $entries = @($current.SelectNodes('/appSettings/add[@key="TeamsChatEnabled"]'))
    if ($entries.Count -gt 1) { throw 'Duplicate TeamsChatEnabled settings.' }
    if ($entries.Count -eq 0) {
        $entry = $current.CreateElement('add')
        $entry.SetAttribute('key', 'TeamsChatEnabled')
        [void]$current.DocumentElement.AppendChild($entry)
    } else { $entry = $entries[0] }
    $entry.SetAttribute('value', 'true')
    $temporary = $Path + '.' + [guid]::NewGuid().ToString('N') + '.config'
    try { $current.Save($temporary); [IO.File]::Replace($temporary, $Path, [NullString]::Value) }
    finally { if ([IO.File]::Exists($temporary)) { [IO.File]::Delete($temporary) } }
}
function Invoke-OAuth([string]$Endpoint, [hashtable]$Form) {
    try {
        return Invoke-RestMethod -Method Post -Uri $Endpoint -ContentType 'application/x-www-form-urlencoded' -Body $Form -TimeoutSec 30
    }
    catch {
        # Do not propagate a raw response or request body containing authentication material.
        $failure = $null
        try {
            if ($_.ErrorDetails.Message) { $failure = $_.ErrorDetails.Message | ConvertFrom-Json }
            elseif ($_.Exception.Response) {
                $reader = New-Object IO.StreamReader($_.Exception.Response.GetResponseStream())
                try { $failure = $reader.ReadToEnd() | ConvertFrom-Json } finally { $reader.Dispose() }
            }
        } catch { }
        if ($failure -and $failure.error -is [string] -and $failure.error -match '^[a-zA-Z0-9_]+$') {
            return [pscustomobject]@{ error = $failure.error }
        }
        throw 'Microsoft authentication request failed. Check connectivity, public client flow settings, and delegated permissions.'
    }
}

$device = Invoke-OAuth "$authority/devicecode" @{ client_id = $clientId; scope = $scope }
if ($device.error) { throw "Microsoft device login failed: $($device.error)" }
if (-not $device.device_code -or -not $device.user_code -or -not $device.verification_uri -or [int]$device.expires_in -le 0) {
    throw 'Microsoft returned an incomplete device login response.'
}
$verificationUri = [uri]$device.verification_uri
if ($verificationUri.Scheme -ne 'https' -or $verificationUri.Host -notin @('microsoft.com', 'www.microsoft.com', 'login.microsoftonline.com', 'login.microsoft.com')) {
    throw ('Unexpected Microsoft login address. Scheme: {0}; Host: {1}. No token has been saved.' -f $verificationUri.Scheme, $verificationUri.Host)
}
Write-Host "Open $verificationUri in your browser, enter the code below, and sign in as $sender."
Write-Host $device.user_code
Write-Host 'Only use this code for the login you just started. No chat message will be sent by this script.'
$deadline = [DateTime]::UtcNow.AddSeconds([int]$device.expires_in)
$interval = [Math]::Max(5, [int]$device.interval)
$tokens = $null
while ([DateTime]::UtcNow -lt $deadline) {
    Start-Sleep -Seconds $interval
    $reply = Invoke-OAuth "$authority/token" @{
        client_id = $clientId; device_code = $device.device_code; grant_type = 'urn:ietf:params:oauth:grant-type:device_code'
    }
    if ($reply.error -eq 'authorization_pending') { continue }
    if ($reply.error -eq 'slow_down') { $interval += 5; continue }
    if ($reply.error) { throw "Microsoft login stopped: $($reply.error). Restart this script when ready." }
    $tokens = $reply
    break
}
if (-not $tokens -or -not $tokens.access_token -or -not $tokens.refresh_token) { throw 'Login expired or no refresh token was issued.' }
if ($tokens.token_type -ne 'Bearer') { throw 'Microsoft did not return a Bearer token.' }
$granted = @($tokens.scope -split ' ' | ForEach-Object { $_ -replace '^https://graph\.microsoft\.com/', '' })
foreach ($permission in @('User.Read', 'User.ReadBasic.All', 'Chat.Create', 'ChatMessage.Send')) {
    if ($permission -notin $granted) { throw "Missing delegated permission: $permission. Grant consent and reconnect." }
}
try {
    $me = Invoke-RestMethod -Uri 'https://graph.microsoft.com/v1.0/me?$select=id,userPrincipalName,displayName' -Headers @{ Authorization = 'Bearer ' + $tokens.access_token } -TimeoutSec 30
}
catch { throw 'Unable to verify the signed-in sender using Microsoft Graph /me. No token has been saved.' }
if (-not [string]::Equals($me.userPrincipalName, $sender, [StringComparison]::OrdinalIgnoreCase)) {
    throw "The signed-in account does not match $sender. No token has been saved. Run this script again with the sender account."
}

# Restrict this dedicated directory to SYSTEM, administrators, the setup user, and the specified IIS identity.
if (-not (Test-Path -LiteralPath $tokenDirectory)) { New-Item -ItemType Directory -Path $tokenDirectory | Out-Null }
$directoryItem = Get-Item -LiteralPath $tokenDirectory
if ($directoryItem.Attributes -band [IO.FileAttributes]::ReparsePoint) { throw 'The token directory must not be a junction or symbolic link.' }
$acl = New-Object Security.AccessControl.DirectorySecurity
$acl.SetAccessRuleProtection($true, $false)
$inheritance = [Security.AccessControl.InheritanceFlags]'ContainerInherit, ObjectInherit'
for ($i = 0; $i -lt $principals.Count; $i++) {
    $rights = if ($i -lt 3) { [Security.AccessControl.FileSystemRights]::FullControl } else { [Security.AccessControl.FileSystemRights]::Modify }
    $rule = New-Object Security.AccessControl.FileSystemAccessRule($principals[$i], $rights, $inheritance, [Security.AccessControl.PropagationFlags]::None, [Security.AccessControl.AccessControlType]::Allow)
    $acl.AddAccessRule($rule)
}
Set-Acl -LiteralPath $tokenDirectory -AclObject $acl
foreach ($existing in @($tokenPath, "$tokenPath.lock", "$tokenPath.reauth-mail.sent")) {
    if (Test-Path -LiteralPath $existing) {
        $fileAcl = New-Object Security.AccessControl.FileSecurity
        $fileAcl.SetAccessRuleProtection($false, $false)
        Set-Acl -LiteralPath $existing -AclObject $fileAcl
    }
}
$tokenLock = [Zeospace.DeskPlus.Common.BLL.TeamsChatTokenStore]::AcquireLock($tokenPath)
try {
    [Zeospace.DeskPlus.Common.BLL.TeamsChatTokenStore]::Save($tokenPath, $context, $tokens.refresh_token)
    # Clear the notification marker only after a successful interactive reconnection.
    if ([IO.File]::Exists("$tokenPath.reauth-mail.sent")) { [IO.File]::Delete("$tokenPath.reauth-mail.sent") }
}
finally { $tokenLock.Dispose() }
Enable-TeamsChat $ConfigPath
$tokens = $null
$reply = $null
$device = $null
Write-Host "Connected: $($me.displayName) <$($me.userPrincipalName)>"
Write-Host "Encrypted refresh token saved on this server: $tokenPath"
Write-Host 'The application can now send 1:1 chats. If tenant policy requires login again, rerun this script.'
