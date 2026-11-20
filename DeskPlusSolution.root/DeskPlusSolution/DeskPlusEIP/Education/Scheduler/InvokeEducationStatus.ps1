#requires -Version 3.0
<#
매일 00시에 진행상태와 D-Day를 갱신합니다. sqlcmd는 필요하지 않습니다.
사이트의 WebConnectionString.config에 있는 SQLConnSolution 설정을 사용합니다.
-ValidateOnly는 DB 연결과 프로시저/권한만 확인하며 실제 갱신은 실행하지 않습니다.
메신저 알람 발송 스크립트와는 별도로 등록합니다.
#>
[CmdletBinding()]
param(
    [string]$ConfigPath,

    [switch]$ValidateOnly,

    [ValidateRange(30, 3600)]
    [int]$TimeoutSeconds = 600
)

$ErrorActionPreference = 'Stop'
$statusConnection = $null
$statusStage = '설정 파일 확인'
$statusExitCode = 1

function Read-EducationStatusConnectionString {
    param([string]$Path)

    if ([string]::IsNullOrWhiteSpace($Path) -or
        ($Path -notmatch '^[A-Za-z]:[\\/]' -and
            $Path -notmatch '^\\\\[^\\/]+\\[^\\/]+\\') -or
        -not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw 'STATUS_CONFIG_NOT_FOUND'
    }

    $configFile = Get-Item -LiteralPath $Path
    if ($configFile -isnot [IO.FileInfo]) { throw 'STATUS_CONFIG_NOT_FOUND' }

    $xmlSettings = New-Object Xml.XmlReaderSettings
    $xmlSettings.DtdProcessing = [Xml.DtdProcessing]::Prohibit
    $xmlSettings.XmlResolver = $null
    $xmlSettings.MaxCharactersInDocument = 1048576
    $xmlReader = $null
    try {
        $xmlReader = [Xml.XmlReader]::Create($configFile.FullName, $xmlSettings)
        $configDocument = New-Object Xml.XmlDocument
        $configDocument.XmlResolver = $null
        $configDocument.Load($xmlReader)
    }
    finally {
        if ($null -ne $xmlReader) { $xmlReader.Dispose() }
    }

    if ($configDocument.DocumentElement.Name -ne 'connectionStrings' -or
        $configDocument.DocumentElement.NamespaceURI.Length -ne 0) {
        throw 'STATUS_CONFIG_INVALID'
    }

    $connectionSettings = $configDocument.SelectNodes('/connectionStrings/add[@name="SQLConnSolution"]')
    if ($connectionSettings.Count -ne 1) { throw 'STATUS_CONFIG_INVALID' }
    $connectionSetting = $connectionSettings.Item(0)
    $providerName = $connectionSetting.GetAttribute('providerName')
    if ($providerName.Length -ne 0 -and
        -not [string]::Equals($providerName, 'System.Data.SqlClient', [StringComparison]::OrdinalIgnoreCase)) {
        throw 'STATUS_CONFIG_INVALID'
    }
    $connectionString = $connectionSetting.GetAttribute('connectionString')
    if ([string]::IsNullOrWhiteSpace($connectionString)) { throw 'STATUS_CONFIG_INVALID' }

    $connectionBuilder = New-Object System.Data.SqlClient.SqlConnectionStringBuilder
    $connectionBuilder.set_ConnectionString($connectionString)
    if ([string]::IsNullOrWhiteSpace($connectionBuilder.get_DataSource()) -or
        [string]::IsNullOrWhiteSpace($connectionBuilder.get_InitialCatalog())) {
        throw 'STATUS_CONFIG_INVALID'
    }
    # PowerShell의 IDictionary 속성 해석 대신 실제 .NET 접근자를 사용합니다.
    $connectionBuilder.set_ApplicationName('EducationStatusScheduler')
    $connectionBuilder.set_PersistSecurityInfo($false)
    $connectionBuilder.set_ConnectTimeout(30)
    return $connectionBuilder.get_ConnectionString()
}

function Assert-EducationStatusProcedure {
    param($Connection)

    $metadataCommand = $null
    $metadataReader = $null
    try {
        $metadataCommand = $Connection.CreateCommand()
        $metadataCommand.CommandTimeout = 30
        $metadataCommand.CommandType = [System.Data.CommandType]::Text
        # 읽기 전용 메타데이터 조회이며 갱신 프로시저는 실행하지 않습니다.
        $metadataCommand.CommandText = @'
SELECT
    CASE WHEN OBJECT_ID(N'dbo.DP_EDU_List_UpdScheduler', N'P') IS NULL THEN 0 ELSE 1 END AS ProcedureExists,
    ISNULL(HAS_PERMS_BY_NAME(N'dbo.DP_EDU_List_UpdScheduler', N'OBJECT', N'EXECUTE'), 0) AS CanExecute,
    (SELECT COUNT(*) FROM sys.parameters
     WHERE object_id = OBJECT_ID(N'dbo.DP_EDU_List_UpdScheduler', N'P')) AS ParameterCount;
'@
        $metadataReader = $metadataCommand.ExecuteReader()
        if (-not $metadataReader.Read()) { throw 'STATUS_PROCEDURE_CHECK_FAILED' }
        if ([Convert]::ToInt32($metadataReader['ProcedureExists']) -ne 1) {
            throw 'STATUS_PROCEDURE_NOT_FOUND'
        }
        if ([Convert]::ToInt32($metadataReader['CanExecute']) -ne 1) {
            throw 'STATUS_PROCEDURE_PERMISSION'
        }
        if ([Convert]::ToInt32($metadataReader['ParameterCount']) -ne 0) {
            throw 'STATUS_PROCEDURE_PARAMETERS'
        }
    }
    finally {
        if ($null -ne $metadataReader) { $metadataReader.Dispose() }
        if ($null -ne $metadataCommand) { $metadataCommand.Dispose() }
    }
}

function Invoke-EducationStatusProcedure {
    param($Connection, [int]$Timeout)

    $updateCommand = $null
    $updateReader = $null
    try {
        $updateCommand = $Connection.CreateCommand()
        $updateCommand.CommandType = [System.Data.CommandType]::StoredProcedure
        # 호출 대상은 고정하며 요청값/임의 SQL/다른 프로시저 이름을 받지 않습니다.
        $updateCommand.CommandText = 'dbo.DP_EDU_List_UpdScheduler'
        $updateCommand.CommandTimeout = $Timeout
        $updateReader = $updateCommand.ExecuteReader()
        if (-not $updateReader.Read()) { throw 'STATUS_RESULT_INVALID' }

        $updatedCount = 0
        $updatedListCount = 0
        if (-not [int]::TryParse([string]$updateReader['UpdatedCount'], [ref]$updatedCount) -or
            -not [int]::TryParse([string]$updateReader['UpdatedListCount'], [ref]$updatedListCount) -or
            $updatedCount -lt 0 -or $updatedListCount -lt 0) {
            throw 'STATUS_RESULT_INVALID'
        }
        if ($updateReader.Read() -or $updateReader.NextResult()) { throw 'STATUS_RESULT_INVALID' }

        return [pscustomobject]@{
            UpdatedCount = $updatedCount
            UpdatedListCount = $updatedListCount
        }
    }
    finally {
        if ($null -ne $updateReader) { $updateReader.Dispose() }
        if ($null -ne $updateCommand) { $updateCommand.Dispose() }
    }
}

try {
    Add-Type -AssemblyName System.Data
    if ([string]::IsNullOrWhiteSpace($ConfigPath)) {
        # D:\DeskPlusSolution\DeskPlusScheduler에 복사했을 때의 기본 사이트 경로입니다.
        $ConfigPath = Join-Path (Split-Path -Parent $PSScriptRoot) 'DeskPlusEIP\WebConnectionString.config'
    }
    $statusConnectionString = Read-EducationStatusConnectionString -Path $ConfigPath

    $statusStage = 'DB 연결'
    $statusConnection = New-Object System.Data.SqlClient.SqlConnection($statusConnectionString)
    $statusConnection.Open()
    $statusConnectionString = $null

    $statusStage = '프로시저 및 실행 권한 확인'
    Assert-EducationStatusProcedure -Connection $statusConnection

    if ($ValidateOnly) {
        Write-Output '상태 갱신 사전 점검 완료: 설정, DB 연결, 프로시저 및 실행 권한을 확인했습니다. 데이터 변경과 알람 발송은 실행하지 않았습니다.'
    }
    else {
        $statusStage = '진행상태 및 D-Day 갱신'
        $statusResult = Invoke-EducationStatusProcedure -Connection $statusConnection -Timeout $TimeoutSeconds
        Write-Output ('상태 갱신 완료: 대상자 진행상태 {0}건, 교육 D-Day {1}건' -f
            $statusResult.UpdatedCount, $statusResult.UpdatedListCount)
    }
    $statusExitCode = 0
}
catch {
    # 연결 문자열, 계정/비밀번호 및 원본 예외 내용은 출력하지 않습니다.
    $statusFailure = '설정 또는 서버 처리 결과를 확인해 주세요.'
    switch ($_.Exception.Message) {
        'STATUS_CONFIG_NOT_FOUND' { $statusFailure = '설정 파일을 찾을 수 없습니다. -ConfigPath에 WebConnectionString.config의 전체 경로를 지정해 주세요.' }
        'STATUS_CONFIG_INVALID' { $statusFailure = 'WebConnectionString.config의 SQLConnSolution 설정을 확인해 주세요.' }
        'STATUS_PROCEDURE_NOT_FOUND' { $statusFailure = 'DP_EDU_List_UpdScheduler 프로시저의 배포 여부 및 실행 계정 권한을 확인해 주세요.' }
        'STATUS_PROCEDURE_PERMISSION' { $statusFailure = 'DP_EDU_List_UpdScheduler 프로시저 실행 권한을 확인해 주세요.' }
        'STATUS_PROCEDURE_PARAMETERS' { $statusFailure = '매개변수 없는 DP_EDU_List_UpdScheduler 프로시저가 필요합니다. 배포 버전을 확인해 주세요.' }
        'STATUS_RESULT_INVALID' { $statusFailure = '프로시저의 반환 건수를 확인할 수 없습니다. DB 변경 여부를 확인한 후 재시도해 주세요.' }
    }
    $statusException = $_.Exception
    while ($null -ne $statusException) {
        if ($statusException -is [System.Data.SqlClient.SqlException]) {
            $statusFailure = 'SQL 오류 번호 {0}. DB 연결, 실행 권한 및 프로시저 상태를 확인해 주세요.' -f $statusException.Number
            break
        }
        $statusException = $statusException.InnerException
    }
    Write-Output ('상태 갱신 실패: {0}. {1}' -f $statusStage, $statusFailure)
}
finally {
    $statusConnectionString = $null
    if ($null -ne $statusConnection) {
        try { $statusConnection.Dispose() }
        catch {
            Write-Output '상태 갱신 실패: DB 연결 종료에 실패했습니다. 처리 결과를 확인해 주세요.'
            $statusExitCode = 1
        }
    }
}

exit $statusExitCode
