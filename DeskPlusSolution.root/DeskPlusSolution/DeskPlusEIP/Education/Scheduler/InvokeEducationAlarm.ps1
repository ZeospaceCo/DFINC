<#
재알람 페이지를 Windows 작업 스케줄러에서 호출합니다.
로그인이나 호출키 없이 네트워크로 접근 가능한 도메인 또는 IP 주소로 호출합니다.
실제 메신저 알람이 발송되므로 배포 설정을 완료한 뒤 운영자가 실행합니다.
#>
[CmdletBinding()]
param(
    [string]$Url = 'http://ep.dfinc.co.kr/DeskPlusEIP/Education/Scheduler/Alarm.aspx',

    [switch]$ValidateOnly,

    [ValidateRange(30, 3600)]
    [int]$TimeoutSeconds = 660
)

$ErrorActionPreference = 'Stop'
$alarmResponse = $null
$alarmReader = $null
$alarmStage = '설정 확인'

try {
    $alarmUri = $null
    if (-not [Uri]::TryCreate($Url, [UriKind]::Absolute, [ref]$alarmUri) -or
        ($alarmUri.Scheme -ne 'http' -and $alarmUri.Scheme -ne 'https') -or
        $alarmUri.UserInfo.Length -ne 0 -or
        $alarmUri.Query.Length -ne 0 -or
        $alarmUri.Fragment.Length -ne 0 -or
        -not [string]::Equals($alarmUri.AbsolutePath, '/DeskPlusEIP/Education/Scheduler/Alarm.aspx', [StringComparison]::OrdinalIgnoreCase)) {
        throw 'INVALID_SCHEDULER_URL'
    }

    $alarmStage = '호출 주소 확인'
    # 주소 형식과 DNS 해석만 확인하며 호출 위치나 서버 IP는 제한하지 않습니다.
    $alarmHost = $alarmUri.DnsSafeHost.Trim('[', ']')
    $alarmAddresses = @()
    $alarmAddress = $null
    if ([Net.IPAddress]::TryParse($alarmHost, [ref]$alarmAddress)) {
        $alarmAddresses = @($alarmAddress)
    }
    else {
        $alarmAddresses = @([Net.Dns]::GetHostAddresses($alarmHost))
    }

    if ($alarmAddresses.Count -eq 0) { throw 'UNRESOLVED_SCHEDULER_URL' }

    if ($ValidateOnly) {
        Write-Output '알람 호출 주소 및 DNS 검증 완료. 페이지 접속, 알람 발송 및 이력 정리는 실행하지 않았습니다.'
        exit 0
    }

    $alarmStage = '페이지 호출'
    $alarmRequest = [Net.HttpWebRequest]::Create($alarmUri)
    $alarmRequest.Method = 'POST'
    $alarmRequest.AllowAutoRedirect = $false
    $alarmRequest.Proxy = $null
    $alarmRequest.UseDefaultCredentials = $false
    $alarmRequest.Timeout = $TimeoutSeconds * 1000
    $alarmRequest.ReadWriteTimeout = $TimeoutSeconds * 1000
    $alarmRequest.ContentLength = 0
    $alarmRequest.UserAgent = 'EducationAlarmScheduler/1.0'

    $alarmResponse = $alarmRequest.GetResponse()
    if ([int]$alarmResponse.StatusCode -ne 200) { throw 'UNEXPECTED_SCHEDULER_STATUS' }
    $alarmReader = New-Object IO.StreamReader($alarmResponse.GetResponseStream(), [Text.Encoding]::UTF8)
    $alarmResult = $alarmReader.ReadToEnd() | ConvertFrom-Json

    if ($null -eq $alarmResult -or $alarmResult.success -isnot [bool] -or $alarmResult.success -ne $true) {
        throw 'UNEXPECTED_SCHEDULER_RESPONSE'
    }

    foreach ($alarmField in @('targetCount', 'sentCount', 'skippedCount', 'failedCount', 'cleanedHistoryCount')) {
        $alarmCount = 0
        if (-not [int]::TryParse([string]$alarmResult.$alarmField, [ref]$alarmCount) -or $alarmCount -lt 0) {
            throw 'UNEXPECTED_SCHEDULER_RESPONSE'
        }
    }
    if ($alarmResult.historyCleanupFailed -isnot [bool]) { throw 'UNEXPECTED_SCHEDULER_RESPONSE' }
    if ([int]$alarmResult.failedCount -ne 0) { throw 'SCHEDULER_DELIVERY_FAILED' }
    if ($alarmResult.historyCleanupFailed) { throw 'SCHEDULER_HISTORY_CLEANUP_FAILED' }

    Write-Output ('알람 완료: 대상 {0}, 발송 {1}, 제외 {2}, 실패 {3}, 이력정리 {4}' -f
        $alarmResult.targetCount, $alarmResult.sentCount, $alarmResult.skippedCount, $alarmResult.failedCount, $alarmResult.cleanedHistoryCount)
    exit 0
}
catch [Net.WebException] {
    $alarmFailureResponse = $_.Exception.Response
    if ($null -ne $alarmFailureResponse) {
        try {
            Write-Output ('알람 호출 실패: HTTP {0}. 작업 스케줄러의 재시도 및 발송 이력을 확인해 주세요.' -f [int]$alarmFailureResponse.StatusCode)
        }
        finally {
            $alarmFailureResponse.Close()
        }
    }
    else {
        Write-Output '알람 호출 실패: 서버 연결 또는 응답 시간을 확인해 주세요.'
    }
    exit 1
}
catch {
    # 원본 예외에는 요청 정보가 포함될 수 있어 고정 문구만 출력합니다.
    Write-Output ('알람 실패: {0}. 설정 또는 서버 처리 결과를 확인해 주세요.' -f $alarmStage)
    exit 1
}
finally {
    if ($null -ne $alarmReader) { $alarmReader.Dispose() }
    if ($null -ne $alarmResponse) { $alarmResponse.Close() }
}
