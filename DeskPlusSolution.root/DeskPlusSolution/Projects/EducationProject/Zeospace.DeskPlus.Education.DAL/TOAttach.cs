using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Xml;
using Zeospace.DeskPlus.Education.BE;

namespace Zeospace.DeskPlus.Education.DAL
{
    public class TOAttach : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public TOAttachInfo Get(int companyID, int listID, int userID)
        {
            return Get(companyID, listID, userID, "");
        }

        public TOAttachInfo Get(int companyID, int listID, int userID, string fileGUID)
        {
            const string query = @"SELECT A.CompanyID, A.ListID, A.UserID, A.FilePath, A.FileGUID,
A.FileName, A.FileType, A.FileLength, A.WriteDate
FROM dbo.DP_EDU_TOAttach AS A
INNER JOIN dbo.DP_EDU_TOUser AS U
    ON U.CompanyID = A.CompanyID AND U.ListID = A.ListID AND U.UserID = A.UserID
INNER JOIN dbo.DP_EDU_List AS L
    ON L.CompanyID = U.CompanyID AND L.ListID = U.ListID
WHERE A.CompanyID = @CompanyID AND A.ListID = @ListID AND A.UserID = @UserID
    AND (@FileGUID = N'' OR A.FileGUID = @FileGUID)";

            using (var cn = new SqlConnection(ConnSolution(companyID)))
            using (var cmd = new SqlCommand(query, cn))
            {
                AddTargetParameters(cmd, companyID, listID, userID);
                cmd.Parameters.Add("@FileGUID", SqlDbType.NVarChar, 200).Value = fileGUID ?? "";
                cn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    TOAttachInfo info = GetAttach(reader);
                    // 여러 파일이 있으면 다운로드할 파일을 명시해야 합니다.
                    return reader.Read() ? null : info;
                }
            }
        }

        public List<TOAttachInfo> GetList(int companyID, int listID, int userID)
        {
            const string query = @"SELECT A.CompanyID, A.ListID, A.UserID, A.FilePath, A.FileGUID,
A.FileName, A.FileType, A.FileLength, A.WriteDate
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
A.FileName, A.FileType, A.FileLength, A.WriteDate,
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

        public bool Add(List<TOAttachInfo> files)
        {
            if (files == null) throw new ArgumentNullException("files");
            if (files.Count == 0) throw new ArgumentException("수료증 파일을 첨부해 주세요.");
            TOAttachInfo target = files[0];
            if (target == null) throw new ArgumentException("수료증 파일 정보가 없습니다.");
            var document = new XmlDocument();
            XmlElement root = document.CreateElement("Files");
            document.AppendChild(root);
            foreach (TOAttachInfo file in files)
            {
                if (file == null || file.CompanyID != target.CompanyID || file.ListID != target.ListID || file.UserID != target.UserID)
                    throw new ArgumentException("수료증 대상자 정보가 일치하지 않습니다.");
                if (file.FileLength <= 0) throw new ArgumentException("수료증 파일이 비어 있습니다.");
                XmlElement item = document.CreateElement("File");
                item.SetAttribute("FilePath", file.FilePath);
                item.SetAttribute("FileGUID", file.FileGUID);
                item.SetAttribute("FileName", file.FileName);
                item.SetAttribute("FileType", file.FileType);
                item.SetAttribute("FileLength", file.FileLength.ToString(CultureInfo.InvariantCulture));
                root.AppendChild(item);
            }

            using (var cn = new SqlConnection(ConnSolution(target.CompanyID)))
            using (var cmd = new SqlCommand("dbo.DP_EDU_TOUser_Complete", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddTargetParameters(cmd, target.CompanyID, target.ListID, target.UserID);
                cmd.Parameters.Add("@Files", SqlDbType.Xml).Value = document.OuterXml;
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        private static TOAttachInfo GetAttach(IDataReader reader)
        {
            return new TOAttachInfo
            {
                CompanyID = (int)reader["CompanyID"],
                ListID = (int)reader["ListID"],
                UserID = (int)reader["UserID"],
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
