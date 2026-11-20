using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using Zeospace.DeskPlus.Education.BE;

namespace Zeospace.DeskPlus.Education.DAL
{
    public class TOAttach : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 완료 저장 트랜잭션 안에서 현재 대상자와 관리자 확인·대기 상태를 확인합니다.
        /// </summary>
        private static void EnsureCanComplete(SqlConnection cn, SqlTransaction transaction,
            int companyID, int listID, int userID)
        {
            // 수정/삭제/완료 취소와 같은 순서로 교육을 먼저, 대상자를 다음에 잠급니다.
            const string query = @"DECLARE @StartDate char(8), @Status char(1), @ConfirmYN char(1);
SELECT @StartDate = StartDate
FROM dbo.DP_EDU_List WITH (UPDLOCK, HOLDLOCK)
WHERE CompanyID = @CompanyID AND ListID = @ListID;
IF @StartDate IS NULL OR LEN(@StartDate) <> 8
    OR @StartDate COLLATE Latin1_General_BIN LIKE '%[^0-9]%' OR ISDATE(@StartDate) = 0
    SELECT 0;
ELSE
BEGIN
    SELECT @Status = Status, @ConfirmYN = ConfirmYN
    FROM dbo.DP_EDU_TOUser WITH (UPDLOCK, HOLDLOCK)
    WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID;
    IF @@ROWCOUNT = 0
        SELECT 0;
    ELSE IF ISNULL(@ConfirmYN, 'N') = 'Y'
        SELECT 3;
    ELSE IF @Status = '1' OR @StartDate > @Today
        SELECT 2;
    ELSE
        SELECT 1;
END;";
            using (var cmd = new SqlCommand(query, cn, transaction))
            {
                AddTargetParameters(cmd, companyID, listID, userID);
                DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time")).Date;
                cmd.Parameters.Add("@Today", SqlDbType.Char, 8).Value = today.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                int state = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);
                if (state == 3) throw new ArgumentException("관리자 확인이 완료된 교육은 완료 처리할 수 없습니다.");
                if (state == 2) throw new ArgumentException(ListInfo.WaitingCompletionMessage);
                if (state != 1) throw new ArgumentException("완료 처리할 교육 대상자를 확인할 수 없습니다.");
            }
        }

        public List<TOAttachInfo> GetList(int companyID, int listID, int userID)
        {
            const string query = @"SELECT A.CompanyID, A.ListID, A.UserID, A.FilePath, A.FileGUID,
A.FileName, A.FileType, A.FileLength, A.WriteDate, A.WriteUserID
FROM dbo.DP_EDU_TOAttach AS A
INNER JOIN dbo.DP_EDU_TOUser AS U
    ON U.CompanyID = A.CompanyID AND U.ListID = A.ListID AND U.UserID = A.UserID
INNER JOIN dbo.DP_EDU_List AS L
    ON L.CompanyID = U.CompanyID AND L.ListID = U.ListID
WHERE A.CompanyID = @CompanyID AND A.ListID = @ListID AND A.UserID = @UserID
ORDER BY A.WriteDate, A.FileGUID";

            using (var cn = new SqlConnection(ConnSolution(companyID)))
            using (var cmd = new SqlCommand(query, cn))
            {
                AddTargetParameters(cmd, companyID, listID, userID);
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var list = new List<TOAttachInfo>();
                    while (reader.Read()) list.Add(GetAttach(reader));
                    return list;
                }
            }
        }

        public Dictionary<int, int> GetFileCounts(int companyID, int userID)
        {
            const string query = @"SELECT A.ListID, COUNT(*) AS FileCount
FROM dbo.DP_EDU_TOAttach AS A
INNER JOIN dbo.DP_EDU_TOUser AS U
    ON U.CompanyID = A.CompanyID AND U.ListID = A.ListID AND U.UserID = A.UserID
INNER JOIN dbo.DP_EDU_List AS L
    ON L.CompanyID = U.CompanyID AND L.ListID = U.ListID
WHERE A.CompanyID = @CompanyID AND A.UserID = @UserID
GROUP BY A.ListID";

            using (var cn = new SqlConnection(ConnSolution(companyID)))
            using (var cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var counts = new Dictionary<int, int>();
                    while (reader.Read()) counts.Add((int)reader["ListID"], (int)reader["FileCount"]);
                    return counts;
                }
            }
        }

        public string GetYearMonth(int companyID, int listID)
        {
            const string query = @"SELECT CONVERT(char(6), WriteDate, 112)
FROM dbo.DP_EDU_List
WHERE CompanyID = @CompanyID AND ListID = @ListID";

            using (var cn = new SqlConnection(ConnSolution(companyID)))
            using (var cmd = new SqlCommand(query, cn))
            {
                AddListParameters(cmd, companyID, listID);
                cn.Open();
                object yearMonth = cmd.ExecuteScalar();
                return yearMonth == null || yearMonth == DBNull.Value ? null : Convert.ToString(yearMonth);
            }
        }

        public List<TOAttachInfo> GetList(int companyID, int listID)
        {
            const string query = @"SELECT A.CompanyID, A.ListID, A.UserID, A.FilePath, A.FileGUID,
A.FileName, A.FileType, A.FileLength, A.WriteDate, A.WriteUserID,
N.NameBase, N.NameKo, N.NameEn, N.NameJa, N.NameFr, N.NameZhcn, N.NameZhtw
FROM dbo.DP_EDU_TOAttach AS A
INNER JOIN dbo.DP_EDU_TOUser AS U
    ON U.CompanyID = A.CompanyID AND U.ListID = A.ListID AND U.UserID = A.UserID
INNER JOIN dbo.DP_EDU_List AS L
    ON L.CompanyID = U.CompanyID AND L.ListID = U.ListID
OUTER APPLY (
    SELECT TOP 1 UI.NameBase, UI.NameKo, UI.NameEn, UI.NameJa, UI.NameFr, UI.NameZhcn, UI.NameZhtw
    FROM dbo.VW_UserInfo AS UI
    WHERE UI.CompanyID = A.CompanyID AND UI.UserID = A.UserID
    ORDER BY UI.BaseDept DESC, UI.DeptOrder, UI.DeptID
) AS N
WHERE A.CompanyID = @CompanyID AND A.ListID = @ListID
ORDER BY A.WriteDate, A.UserID, A.FileGUID";

            using (var cn = new SqlConnection(ConnSolution(companyID)))
            using (var cmd = new SqlCommand(query, cn))
            {
                AddListParameters(cmd, companyID, listID);
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    var list = new List<TOAttachInfo>();
                    while (reader.Read())
                    {
                        TOAttachInfo info = GetAttach(reader);
                        info.NameBase = reader["NameBase"].ToString();
                        info.NameKo = reader["NameKo"].ToString();
                        info.NameEn = reader["NameEn"].ToString();
                        info.NameJa = reader["NameJa"].ToString();
                        info.NameFr = reader["NameFr"].ToString();
                        info.NameZhcn = reader["NameZhcn"].ToString();
                        info.NameZhtw = reader["NameZhtw"].ToString();
                        info.UserName = String.IsNullOrEmpty(info.NameKo) ? info.NameBase : info.NameKo;
                        list.Add(info);
                    }
                    return list;
                }
            }
        }

        public void EnsureCanSaveAttachments(int companyID, int listID, int userID)
        {
            if (companyID <= 0 || listID <= 0 || userID <= 0)
                throw new ArgumentException("첨부파일을 등록할 교육 대상자 정보가 올바르지 않습니다.");
            using (var cn = new SqlConnection(ConnSolution(companyID)))
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    LockAttachmentTarget(cn, transaction, companyID, listID, userID);
                    EnsureCanRegisterAttachments(cn, transaction, companyID, listID, userID);
                    transaction.Commit();
                }
            }
        }

        private static void LockAttachmentTarget(SqlConnection cn, SqlTransaction transaction,
            int companyID, int listID, int userID)
        {
            const string query = @"IF NOT EXISTS (
    SELECT 1 FROM dbo.DP_EDU_List WITH (UPDLOCK, HOLDLOCK)
    WHERE CompanyID = @CompanyID AND ListID = @ListID)
    SELECT 0;
ELSE IF NOT EXISTS (
    SELECT 1 FROM dbo.DP_EDU_TOUser WITH (UPDLOCK, HOLDLOCK)
    WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID)
    SELECT 0;
ELSE SELECT 1;";
            using (var cmd = new SqlCommand(query, cn, transaction))
            {
                AddTargetParameters(cmd, companyID, listID, userID);
                if (Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture) != 1)
                    throw new ArgumentException("첨부파일을 등록할 교육 대상자를 확인할 수 없습니다.");
            }
        }

        private static void EnsureCanRegisterAttachments(SqlConnection cn, SqlTransaction transaction,
            int companyID, int listID, int userID)
        {
            // 사전 검사와 실제 등록 모두 같은 대상자 잠금 안에서 현재 상태를 확인합니다.
            const string query = @"SELECT CASE WHEN ISNULL(ConfirmYN, 'N') = 'Y' THEN 3
    WHEN Status = '3' THEN 2 ELSE 1 END
FROM dbo.DP_EDU_TOUser WITH (UPDLOCK, HOLDLOCK)
WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID;";
            using (var cmd = new SqlCommand(query, cn, transaction))
            {
                AddTargetParameters(cmd, companyID, listID, userID);
                object result = cmd.ExecuteScalar();
                int state = result == null || result == DBNull.Value
                    ? 0 : Convert.ToInt32(result, CultureInfo.InvariantCulture);
                if (state == 3)
                    throw new ArgumentException("관리자 확인이 완료된 교육은 첨부파일을 등록할 수 없습니다.");
                if (state == 2)
                    throw new ArgumentException("완료된 교육은 첨부파일을 등록할 수 없습니다.");
                if (state != 1)
                    throw new ArgumentException("첨부파일을 등록할 교육 대상자를 확인할 수 없습니다.");
            }
        }

        /// <summary>미완료·관리자 미확인 대상자의 첨부만 추가하며 완료 정보는 변경하지 않습니다.</summary>
        public bool SaveAttachments(int companyID, int listID, int userID,
            List<TOAttachInfo> files, int writeUserID)
        {
            if (companyID <= 0 || listID <= 0 || userID <= 0 || writeUserID <= 0)
                throw new ArgumentException("첨부파일 등록 정보가 올바르지 않습니다.");
            if (files == null) throw new ArgumentNullException("files");
            if (files.Count == 0) return false;
            foreach (TOAttachInfo file in files)
            {
                if (file == null || file.CompanyID != companyID || file.ListID != listID || file.UserID != userID)
                    throw new ArgumentException("첨부파일 대상자 정보가 일치하지 않습니다.");
                if (file.FileLength <= 0 || String.IsNullOrEmpty(file.FilePath) || file.FilePath.Length > 1000
                    || String.IsNullOrEmpty(file.FileGUID) || file.FileGUID.Length > 200
                    || String.IsNullOrEmpty(file.FileName) || file.FileName.Length > 500
                    || file.FileType == null || file.FileType.Length > 50)
                    throw new ArgumentException("첨부파일 정보가 올바르지 않습니다.");
            }
            const string query = @"IF NOT EXISTS (
    SELECT 1 FROM dbo.DP_EDU_TOAttach WITH (UPDLOCK, HOLDLOCK)
    WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID AND FileGUID = @FileGUID)
INSERT dbo.DP_EDU_TOAttach
    (CompanyID, ListID, UserID, FilePath, FileGUID, FileName, FileType, FileLength, WriteDate, WriteUserID)
VALUES
    (@CompanyID, @ListID, @UserID, @FilePath, @FileGUID, @FileName, @FileType, @FileLength, GETDATE(), @WriteUserID);";
            using (var cn = new SqlConnection(ConnSolution(companyID)))
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    LockAttachmentTarget(cn, transaction, companyID, listID, userID);
                    EnsureCanRegisterAttachments(cn, transaction, companyID, listID, userID);
                    foreach (TOAttachInfo file in files)
                    {
                        using (var cmd = new SqlCommand(query, cn, transaction))
                        {
                            AddTargetParameters(cmd, companyID, listID, userID);
                            cmd.Parameters.Add("@FilePath", SqlDbType.NVarChar, 1000).Value = file.FilePath;
                            cmd.Parameters.Add("@FileGUID", SqlDbType.NVarChar, 200).Value = file.FileGUID;
                            cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 500).Value = file.FileName;
                            cmd.Parameters.Add("@FileType", SqlDbType.VarChar, 50).Value = file.FileType;
                            cmd.Parameters.Add("@FileLength", SqlDbType.BigInt).Value = file.FileLength;
                            cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = writeUserID;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    return true;
                }
            }
        }

        /// <summary>미완료·관리자 미확인 대상자의 첨부만 삭제하며 교육 상태와 완료일은 변경하지 않습니다.</summary>
        public bool DeleteAttachment(int companyID, int listID, int userID, string fileGUID,
            out List<TOAttachInfo> deletedFiles)
        {
            deletedFiles = new List<TOAttachInfo>();
            if (companyID <= 0 || listID <= 0 || userID <= 0
                || String.IsNullOrWhiteSpace(fileGUID) || fileGUID.Length > 200
                || fileGUID != fileGUID.Trim() || fileGUID == "." || fileGUID == ".."
                || fileGUID.EndsWith(".", StringComparison.Ordinal)
                || fileGUID.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                || fileGUID.IndexOf('/') >= 0 || fileGUID.IndexOf('\\') >= 0
                || !String.Equals(Path.GetFileName(fileGUID), fileGUID, StringComparison.Ordinal))
                throw new ArgumentException("삭제할 첨부파일 정보가 올바르지 않습니다.");

            // FileGUID가 중복 등록된 경우에도 현재 대상자의 같은 파일만 모두 제거합니다.
            // 다른 대상자나 일반 첨부가 같은 실제 파일을 참조하면 물리 파일은 유지합니다.
            const string query = @"DECLARE @Deleted TABLE (FilePath nvarchar(1000), FileGUID nvarchar(200));
DELETE FROM dbo.DP_EDU_TOAttach WITH (UPDLOCK, HOLDLOCK)
OUTPUT DELETED.FilePath, DELETED.FileGUID INTO @Deleted (FilePath, FileGUID)
WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID AND FileGUID = @FileGUID;
SELECT D.FilePath, D.FileGUID,
    CAST(CASE WHEN EXISTS (
        SELECT 1 FROM dbo.DP_EDU_TOAttach AS A WITH (UPDLOCK, HOLDLOCK)
        WHERE LOWER(REPLACE(LTRIM(RTRIM(A.FilePath)), N'\', N'/'))
                = LOWER(REPLACE(LTRIM(RTRIM(D.FilePath)), N'\', N'/'))
            AND LOWER(LTRIM(RTRIM(CASE WHEN ISNULL(A.FileGUID, '') = '' THEN A.FileName ELSE A.FileGUID END)))
                = LOWER(LTRIM(RTRIM(D.FileGUID)))
    ) OR EXISTS (
        SELECT 1 FROM dbo.DP_EDU_Attach AS A WITH (UPDLOCK, HOLDLOCK)
        WHERE LOWER(REPLACE(LTRIM(RTRIM(A.FilePath)), N'\', N'/'))
                = LOWER(REPLACE(LTRIM(RTRIM(D.FilePath)), N'\', N'/'))
            AND LOWER(LTRIM(RTRIM(CASE WHEN ISNULL(A.FileGUID, '') = '' THEN A.FileName ELSE A.FileGUID END)))
                = LOWER(LTRIM(RTRIM(D.FileGUID)))
    ) THEN 1 ELSE 0 END AS bit) AS SharedFile
FROM @Deleted AS D;";
            using (var cn = new SqlConnection(ConnSolution(companyID)))
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    LockAttachmentTarget(cn, transaction, companyID, listID, userID);
                    const string statusQuery = @"SELECT CASE WHEN ISNULL(ConfirmYN, 'N') = 'Y' THEN 3
    WHEN Status = '3' THEN 2 ELSE 1 END
FROM dbo.DP_EDU_TOUser WITH (UPDLOCK, HOLDLOCK)
WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID;";
                    using (var statusCommand = new SqlCommand(statusQuery, cn, transaction))
                    {
                        AddTargetParameters(statusCommand, companyID, listID, userID);
                        int state = Convert.ToInt32(statusCommand.ExecuteScalar(), CultureInfo.InvariantCulture);
                        if (state == 3)
                            throw new ArgumentException("관리자 확인이 완료된 교육의 첨부파일은 삭제할 수 없습니다.");
                        if (state == 2)
                            throw new ArgumentException("완료된 교육의 첨부파일은 삭제할 수 없습니다.");
                        if (state != 1)
                            throw new ArgumentException("첨부파일을 삭제할 교육 대상자를 확인할 수 없습니다.");
                    }
                    using (var cmd = new SqlCommand(query, cn, transaction))
                    {
                        AddTargetParameters(cmd, companyID, listID, userID);
                        cmd.Parameters.Add("@FileGUID", SqlDbType.NVarChar, 200).Value = fileGUID;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                deletedFiles.Add(new TOAttachInfo
                                {
                                    CompanyID = companyID,
                                    ListID = listID,
                                    UserID = userID,
                                    FilePath = reader["FilePath"].ToString(),
                                    FileGUID = reader["FileGUID"].ToString().Trim(),
                                    SharedFile = Convert.ToBoolean(reader["SharedFile"])
                                });
                            }
                        }
                    }
                    transaction.Commit();
                    return deletedFiles.Count > 0;
                }
            }
        }

        /// <summary>DB에 등록된 첨부만 검사하고 완료 정보만 갱신합니다.</summary>
        public bool CompleteSaved(int companyID, int listID, int userID, int completeUserID)
        {
            if (companyID <= 0 || listID <= 0 || userID <= 0 || completeUserID <= 0)
                throw new ArgumentException("완료 처리할 교육 대상자 정보가 올바르지 않습니다.");
            const string query = @"IF EXISTS (
    SELECT 1 FROM dbo.DP_EDU_TOUser
    WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID AND Status = '3')
    SELECT 0;
ELSE IF EXISTS (
    SELECT 1 FROM dbo.DP_EDU_List
    WHERE CompanyID = @CompanyID AND ListID = @ListID AND UPPER(ISNULL(FileYN, 'Y')) <> 'N')
    AND NOT EXISTS (
        SELECT 1 FROM dbo.DP_EDU_TOAttach WITH (UPDLOCK, HOLDLOCK)
        WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID AND FileLength > 0)
    SELECT 2;
ELSE
BEGIN
    UPDATE dbo.DP_EDU_TOUser
    SET Status = '3', CompleteDate = CONVERT(char(8), GETDATE(), 112), CompleteUserID = @CompleteUserID
    WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID
      AND ISNULL(ConfirmYN, 'N') <> 'Y';
    SELECT CASE WHEN @@ROWCOUNT = 1 THEN 1 ELSE 0 END;
END;";
            using (var cn = new SqlConnection(ConnSolution(companyID)))
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    EnsureCanComplete(cn, transaction, companyID, listID, userID);
                    using (var cmd = new SqlCommand(query, cn, transaction))
                    {
                        AddTargetParameters(cmd, companyID, listID, userID);
                        cmd.Parameters.Add("@CompleteUserID", SqlDbType.Int).Value = completeUserID;
                        int result = Convert.ToInt32(cmd.ExecuteScalar(), CultureInfo.InvariantCulture);
                        if (result == 2) throw new ArgumentException("첨부파일을 1개 이상 등록해 주세요.");
                        transaction.Commit();
                        return result == 1;
                    }
                }
            }
        }

        private static TOAttachInfo GetAttach(IDataReader reader)
        {
            return new TOAttachInfo
            {
                CompanyID = (int)reader["CompanyID"],
                ListID = (int)reader["ListID"],
                UserID = (int)reader["UserID"],
                WriteUserID = reader["WriteUserID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["WriteUserID"]),
                FilePath = reader["FilePath"].ToString(),
                FileGUID = reader["FileGUID"].ToString().Trim(),
                FileName = reader["FileName"].ToString(),
                FileType = reader["FileType"].ToString(),
                FileLength = Convert.ToInt64(reader["FileLength"]),
                WriteDate = (DateTime)reader["WriteDate"]
            };
        }

        private static void AddListParameters(SqlCommand cmd, int companyID, int listID)
        {
            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
            cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = listID;
        }

        private static void AddTargetParameters(SqlCommand cmd, int companyID, int listID, int userID)
        {
            AddListParameters(cmd, companyID, listID);
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
        }
    }
}
