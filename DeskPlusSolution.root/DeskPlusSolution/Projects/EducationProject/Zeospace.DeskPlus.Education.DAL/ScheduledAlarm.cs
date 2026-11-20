using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Zeospace.DeskPlus.Education.DAL
{
    /// <summary>
    /// 교육 종료일 전 자동 알람의 대상 조회 및 Teams 발송 이력을 처리합니다.
    /// </summary>
    public class ScheduledAlarm : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        private const int CompanyID = 1;

        public DataTableReader GetTargets(DateTime today)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("DP_EDU_Alarm_SelTargets", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Today", SqlDbType.Char, 8).Value = DateText(today);
                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        public enum DeliveryClaim
        {
            Skip = 0,
            Started = 1,
            Pending = 2
        }

        public DeliveryClaim TryStartTeamsDelivery(int listID, int userID, DateTime alarmDate, DateTime today,
            string expectedTitle, string expectedEndDate, int expectedAlarmDay)
        {
            const string query = @"
SET NOCOUNT ON;
INSERT dbo.DP_EDU_AlarmTeamsDelivery (CompanyID, ListID, UserID, AlarmDate, EndDate, AttemptDate)
SELECT @CompanyID, @ListID, @UserID, @AlarmDate, @EndDate, @Today
WHERE EXISTS (
    SELECT 1
    FROM dbo.DP_EDU_TOUser AS E
    INNER JOIN dbo.DP_EDU_List AS L ON L.CompanyID = E.CompanyID AND L.ListID = E.ListID
    INNER JOIN dbo.VW_UserInfo AS U ON U.CompanyID = E.CompanyID AND U.UserID = E.UserID
    WHERE E.CompanyID = @CompanyID AND E.ListID = @ListID AND E.UserID = @UserID
      AND ISNULL(E.Status, '') <> '3' AND U.ActiveCompany = 1 AND U.ActiveUser = 1
      AND L.Title = @Title AND L.EndDate = @EndDate AND L.EndDate >= @Today
      AND L.AlarmDay = @AlarmDay
)
AND NOT EXISTS (
    SELECT 1 FROM dbo.DP_EDU_AlarmTeamsDelivery WITH (UPDLOCK, HOLDLOCK)
    WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID AND AlarmDate = @AlarmDate
);
IF @@ROWCOUNT = 1
    SELECT 1;
ELSE IF EXISTS (
    SELECT 1 FROM dbo.DP_EDU_AlarmTeamsDelivery
    WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID
      AND AlarmDate = @AlarmDate AND SentAtUtc IS NULL
)
    SELECT 2;
ELSE
    SELECT 0;";

            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                AddDeliveryKey(cmd, listID, userID, alarmDate);
                cmd.Parameters.Add("@Today", SqlDbType.Char, 8).Value = DateText(today);
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = expectedTitle;
                cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = expectedEndDate;
                cmd.Parameters.Add("@AlarmDay", SqlDbType.Int).Value = expectedAlarmDay;
                cn.Open();
                return (DeliveryClaim)(int)cmd.ExecuteScalar();
            }
        }

        public void CompleteTeamsDelivery(int listID, int userID, DateTime alarmDate)
        {
            const string query = @"
UPDATE dbo.DP_EDU_AlarmTeamsDelivery SET SentAtUtc = SYSUTCDATETIME()
WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID
  AND AlarmDate = @AlarmDate AND SentAtUtc IS NULL;";
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                AddDeliveryKey(cmd, listID, userID, alarmDate);
                cn.Open();
                if (cmd.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Pending Teams delivery was not found.");
            }
        }

        private static void AddDeliveryKey(SqlCommand cmd, int listID, int userID, DateTime alarmDate)
        {
            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = CompanyID;
            cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = listID;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
            cmd.Parameters.Add("@AlarmDate", SqlDbType.Char, 8).Value = DateText(alarmDate);
        }

        /// <summary>성공 이력은 변경하지 않으며, 재시도 가능한 실패만 기록합니다.</summary>
        public void RecordFailure(int listID, int userID, DateTime alarmDate, DateTime today, string errorCode,
            bool releaseTeamsClaim)
        {
            if (listID <= 0 || userID <= 0)
                throw new ArgumentException("Invalid education reminder recipient.");
            // 예외 메시지나 접속 정보 대신 사전에 정한 짧은 코드만 저장합니다.
            if (errorCode != "QUEUE_FAILED" && errorCode != "MESSAGE_FAILED" && errorCode != "PREPARE_FAILED")
                errorCode = "QUEUE_FAILED";

            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("DP_EDU_Alarm_RecordFailure", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Today", SqlDbType.Char, 8).Value = DateText(today);
                cmd.Parameters.Add("@AlarmDate", SqlDbType.Char, 8).Value = DateText(alarmDate);
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = listID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@ErrorCode", SqlDbType.VarChar, 30).Value = errorCode;
                cn.Open();
                // 기존 프로시저가 자체 트랜잭션을 관리하므로 단독으로 실행합니다.
                ExecuteNonQuery(cmd);
                if (releaseTeamsClaim)
                {
                    const string query = @"
DELETE FROM dbo.DP_EDU_AlarmTeamsDelivery
WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID
  AND AlarmDate = @AlarmDate AND SentAtUtc IS NULL;";
                    using (SqlCommand release = new SqlCommand(query, cn))
                    {
                        AddDeliveryKey(release, listID, userID, alarmDate);
                        if (release.ExecuteNonQuery() != 1)
                            throw new InvalidOperationException("Pending Teams delivery was not found.");
                    }
                }
            }
        }

        /// <summary>교육 종료 후 7일이 지난 오래된 이력만 정리합니다.</summary>
        public int CleanupHistory(DateTime today)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("DP_EDU_Alarm_CleanupHistory", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Today", SqlDbType.Char, 8).Value = DateText(today);
                SqlParameter deletedCount = cmd.Parameters.Add("@DeletedCount", SqlDbType.Int);
                deletedCount.Direction = ParameterDirection.Output;
                cn.Open();
                ExecuteNonQuery(cmd);
                if (deletedCount.Value == null || deletedCount.Value == DBNull.Value)
                    throw new InvalidOperationException("Reminder history cleanup returned no result.");
                const string query = @"
DELETE D
FROM dbo.DP_EDU_AlarmTeamsDelivery AS D
LEFT JOIN dbo.DP_EDU_List AS L ON L.CompanyID = D.CompanyID AND L.ListID = D.ListID
WHERE D.CompanyID = @CompanyID AND D.EndDate < @Cutoff
  AND (L.EndDate IS NULL OR L.EndDate < @Cutoff)
  AND D.AttemptDate < @Cutoff
  AND (D.SentAtUtc IS NULL OR CONVERT(char(8), DATEADD(hour, 9, D.SentAtUtc), 112) < @Cutoff);";
                using (SqlCommand cleanup = new SqlCommand(query, cn))
                {
                    cleanup.Parameters.Add("@CompanyID", SqlDbType.Int).Value = CompanyID;
                    cleanup.Parameters.Add("@Cutoff", SqlDbType.Char, 8).Value = DateText(today.AddDays(-7));
                    return Convert.ToInt32(deletedCount.Value, CultureInfo.InvariantCulture) + cleanup.ExecuteNonQuery();
                }
            }
        }

        private static string DateText(DateTime date)
        {
            return date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
        }

        private static SqlConnection CreateConnection()
        {
            SqlConnectionStringBuilder solution = new SqlConnectionStringBuilder(ConnSolution(CompanyID));
            solution.Enlist = false;
            return new SqlConnection(solution.ConnectionString);
        }
    }
}
