namespace Zeospace.DeskPlus.Education.BE
{
    /// <summary>
    /// 스케줄 실행 결과입니다. SentCount는 Teams에서 발송을 확인하고 이력에 기록한 건수입니다.
    /// </summary>
    public class ScheduledAlarmResult
    {
        public int TargetCount { get; set; }
        public int SentCount { get; set; }
        public int SkippedCount { get; set; }
        public int FailedCount { get; set; }
        public int CleanedHistoryCount { get; set; }
        public bool HistoryCleanupFailed { get; set; }
    }
}
