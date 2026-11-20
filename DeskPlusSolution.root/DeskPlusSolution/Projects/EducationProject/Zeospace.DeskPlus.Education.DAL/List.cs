using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Education.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Zeospace.DeskPlus.Education.DAL
{
    public class List : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {      
        #region ADD

        /// <summary>
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ListInfo _info)
        {
            try
            {
                _info.ContentText = _info.ContentText.ToEditorXSS();
                _info.ContentText = Regex.Replace(_info.ContentText, "<img", "<ximg", RegexOptions.IgnoreCase);

                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EDU_List_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = _info.Title.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NText).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@ContentText", SqlDbType.NText).Value = _info.ContentText.ToNull();
                    cmd.Parameters.Add("@ItemID", SqlDbType.NVarChar, 10).Value = _info.ItemID.ToNull().ToXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate.ToNull();
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate.ToNull();
                    cmd.Parameters.Add("@PopUpYN", SqlDbType.Char, 1).Value = _info.PopUpYN.ToNull();
                    cmd.Parameters.Add("@FileYN", SqlDbType.Char, 1).Value = _info.FileYN.ToNull();
                    cmd.Parameters.Add("@AlarmDay", SqlDbType.Int).Value = _info.AlarmDay;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;                    

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }              

        #endregion 

        #region MOD()

        /// <summary>
        /// 교육 정보와 대상자, 제거할 일반 첨부파일의 DB 기록을 함께 수정합니다.
        /// 기존 대상자의 수료 정보와 수료증은 보존합니다.
        /// </summary>
        public bool Modify(ListInfo _info)
        {
            int failureReason;
            return Modify(_info, out failureReason);
        }

        public bool Modify(ListInfo _info, out int failureReason)
        {
            failureReason = 0;
            try
            {
                XElement targets = new XElement("Targets");
                string[] selectedTargets = _info.ToUserList.Split(',');
                for (int i = 0; i < selectedTargets.Length; i++)
                {
                    string[] target = selectedTargets[i].Split('_');
                    targets.Add(new XElement("Target",
                        new XAttribute("IsGroup", target[0]),
                        new XAttribute("UserID", Int32.Parse(target[1])),
                        new XAttribute("DeptID", Int32.Parse(target[2])),
                        new XAttribute("TOOrders", i + 1)));
                }

                XElement attachments = new XElement("Attachments");
                if (!String.IsNullOrEmpty(_info.DeleteAttachIDs))
                {
                    foreach (string attachID in _info.DeleteAttachIDs.Split(','))
                        attachments.Add(new XElement("Attachment", new XAttribute("AttachID", Int32.Parse(attachID))));
                }

                string contentText = (_info.ContentText ?? String.Empty).ToEditorXSS();
                contentText = Regex.Replace(contentText, "<img", "<ximg", RegexOptions.IgnoreCase);

                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EDU_List_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = _info.Title.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NText).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@ContentText", SqlDbType.NText).Value = contentText.ToNull();
                    cmd.Parameters.Add("@ItemID", SqlDbType.NVarChar, 10).Value = _info.ItemID.ToNull().ToXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.Char, 8).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.Char, 8).Value = _info.EndDate;
                    cmd.Parameters.Add("@PopUpYN", SqlDbType.Char, 1).Value = _info.PopUpYN;
                    cmd.Parameters.Add("@FileYN", SqlDbType.Char, 1).Value = _info.FileYN.ToNull();
                    cmd.Parameters.Add("@AlarmDay", SqlDbType.Int).Value = _info.AlarmDay;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@Targets", SqlDbType.Xml).Value = targets.ToString(SaveOptions.DisableFormatting);
                    cmd.Parameters.Add("@DeleteAttachments", SqlDbType.Xml).Value = attachments.ToString(SaveOptions.DisableFormatting);
                    SqlParameter modified = cmd.Parameters.Add("@Modified", SqlDbType.Bit);
                    modified.Direction = ParameterDirection.Output;
                    SqlParameter reason = cmd.Parameters.Add("@FailureReason", SqlDbType.TinyInt);
                    reason.Direction = ParameterDirection.Output;
                    cn.Open();
                    ExecuteNonQuery(cmd);
                    if (reason.Value != null && reason.Value != DBNull.Value)
                        failureReason = Convert.ToInt32(reason.Value);
                    return modified.Value != DBNull.Value && Convert.ToBoolean(modified.Value);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 0: 없는 교육, 1: 수정/전체삭제 가능, 2: 수정 가능하나 완료자가 있어 전체삭제 불가.
        /// 화면 확인용이며 실제 삭제 시에는 잠금 후 다시 확인합니다.
        /// </summary>
        public int GetModifyState(int _companyID, int _listID)
        {
            if (_companyID <= 0 || _listID <= 0) return 0;

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                const string query = @"SELECT CASE
    WHEN NOT EXISTS (SELECT 1 FROM dbo.DP_EDU_List WHERE CompanyID = @CompanyID AND ListID = @ListID) THEN 0
    WHEN EXISTS (SELECT 1 FROM dbo.DP_EDU_TOUser WHERE CompanyID = @CompanyID AND ListID = @ListID AND Status = '3') THEN 2
    ELSE 1 END";
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();
                return Convert.ToInt32(ExecuteScalar(cmd));
            }
        }

        /// <summary>
        /// 수정 알람 여부를 판단할 교육 정보, 선택 대상, 실제 대상자, 일반 첨부파일을 조회합니다.
        /// 반드시 수정 작업과 같은 TransactionScope 안에서 수정 전후에 호출해야 합니다.
        /// 교육 행을 먼저 잠그며 팝업/알람 설정과 정렬 순서, 수정 일시 등은 비교에서 제외합니다.
        /// </summary>
        public DataSet GetModificationAlarmSnapshot(int companyID, int listID)
        {
            if (companyID <= 0 || listID <= 0)
                throw new ArgumentException("교육 수정 알람 비교 대상이 올바르지 않습니다.");

            const string query = @"SELECT Title, ItemID, StartDate, EndDate,
    CASE WHEN FileYN = 'N' THEN 'N' ELSE 'Y' END AS FileYN, Contents, PopUpYN, AlarmDay
FROM dbo.DP_EDU_List WITH (UPDLOCK, HOLDLOCK)
WHERE CompanyID = @CompanyID AND ListID = @ListID;

SELECT IsGroup, UserID, DeptID
FROM dbo.DP_EDU_ListTO WITH (HOLDLOCK)
WHERE CompanyID = @CompanyID AND ListID = @ListID
ORDER BY IsGroup, UserID, DeptID;

SELECT UserID
FROM dbo.DP_EDU_TOUser WITH (HOLDLOCK)
WHERE CompanyID = @CompanyID AND ListID = @ListID
ORDER BY UserID;

SELECT AttachID
FROM dbo.DP_EDU_Attach WITH (HOLDLOCK)
WHERE CompanyID = @CompanyID AND ListID = @ListID
ORDER BY AttachID;";

            using (var cn = new SqlConnection(ConnSolution(companyID)))
            using (var cmd = new SqlCommand(query, cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = listID;
                cn.Open();
                return ExecuteDataSet(cmd, "ModificationAlarm");
            }
        }

        #endregion

        #region DEL()

        /// <summary>
        /// 해당 교육과 대상자, 첨부파일의 DB 기록을 전체 삭제합니다.
        /// 완료자가 있으면 삭제하지 않으며 확인부터 삭제까지 잠금을 유지합니다.
        /// 실제 첨부파일은 삭제하지 않습니다.
        /// </summary>
        public bool Del(int _companyID, int _listID)
        {
            if (_companyID <= 0 || _listID <= 0) return false;

            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();
                    using (SqlTransaction transaction = cn.BeginTransaction())
                    {
                        // 교육을 먼저 잠근 뒤 전체 대상자의 완료 여부를 확인합니다.
                        // 사전 확인 이후 사용자가 완료하더라도 확인과 삭제 사이에 상태가 바뀌지 않습니다.
                        const string query = @"IF NOT EXISTS (
    SELECT 1 FROM dbo.DP_EDU_List WITH (UPDLOCK, HOLDLOCK)
    WHERE CompanyID = @CompanyID AND ListID = @ListID)
    SELECT CAST(0 AS bit);
ELSE IF EXISTS (
    SELECT 1 FROM dbo.DP_EDU_TOUser WITH (UPDLOCK, HOLDLOCK)
    WHERE CompanyID = @CompanyID AND ListID = @ListID AND Status = '3')
    SELECT CAST(0 AS bit);
ELSE
    SELECT CAST(1 AS bit);";
                        using (var check = new SqlCommand(query, cn, transaction))
                        {
                            check.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            check.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                            if (!Convert.ToBoolean(ExecuteScalar(check))) return false;
                        }

                        using (var cmd = new SqlCommand("DP_EDU_List_Del", cn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                            object result = ExecuteScalar(cmd);
                            if (result == null || result == DBNull.Value || !Convert.ToBoolean(result)) return false;
                        }
                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion

        #region GET

        /// <summary>
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, string _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = "Select * FROM DP_EDU_List WHERE CompanyID = @CompanyID AND ListID =@ListID";
                query = string.Format(query, _listID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, string _listID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"SELECT V.*, E.CompleteUserID,
CU.NameBase AS CompleteNameBase, CU.NameKo AS CompleteNameKo,
CU.NameEn AS CompleteNameEn, CU.NameJa AS CompleteNameJa,
CU.NameFr AS CompleteNameFr, CU.NameZhcn AS CompleteNameZhcn, CU.NameZhtw AS CompleteNameZhtw
FROM dbo.VW_EDU_TOUser AS V
INNER JOIN dbo.DP_EDU_TOUser AS E
    ON E.CompanyID = V.CompanyID AND E.ListID = V.ListID AND E.UserID = V.UserID
LEFT JOIN dbo.DP_ACC_User AS CU ON CU.CompanyID = E.CompanyID AND CU.UserID = E.CompleteUserID
WHERE V.CompanyID = @CompanyID AND V.ListID = @ListID AND V.UserID = @UserID";
                query = string.Format(query, _listID);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchItem"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchComplete"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="searchDateField"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _userID, string _pageSize, string _pageSizePageNo, string _searchItem, string _searchStatus, 
            string _searchComplete, string _searchField, string _searchValue, string searchDateField, string _searchStartDate, string _searchEndDate)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_EDU_List_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchItem", SqlDbType.NVarChar, 10).Value = _searchItem;
                cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                cmd.Parameters.Add("@SearchComplete", SqlDbType.Char, 1).Value = _searchComplete;
                cmd.Parameters.Add("@searchDateField", SqlDbType.VarChar, 50).Value = searchDateField;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;                
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchItem"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchComplete"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="searchDateField"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _userID, string _searchItem, string _searchStatus, string _searchComplete,string _searchField, string _searchValue, 
            string searchDateField, string _searchStartDate, string _searchEndDate)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_EDU_List_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchItem", SqlDbType.NVarChar, 10).Value = _searchItem;
                cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                cmd.Parameters.Add("@SearchComplete", SqlDbType.Char, 1).Value = _searchComplete;
                cmd.Parameters.Add("@searchDateField", SqlDbType.VarChar, 50).Value = searchDateField;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 50).Value = _searchField;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;                
                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;

        }

        /// <summary>
        /// 현재 사용자가 조회할 수 있는 교육의 상태별 건수를 반환합니다.
        /// 목록의 검색조건을 적용하지 않고 전체 교육을 집계합니다.
        /// </summary>
        public DataTableReader GetStatusSummary(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                const string query = @"
SELECT
    COUNT(*) AS TotalCount,
    ISNULL(SUM(CASE WHEN Status = '1' THEN 1 ELSE 0 END), 0) AS WaitCount,
    ISNULL(SUM(CASE WHEN Status = '2' THEN 1 ELSE 0 END), 0) AS ProgressCount,
    ISNULL(SUM(CASE WHEN Status = '3' THEN 1 ELSE 0 END), 0) AS CompleteCount,
    ISNULL(SUM(CASE WHEN Status = '4' THEN 1 ELSE 0 END), 0) AS DelayCount
FROM VW_EDU_TOUser
WHERE CompanyID = @CompanyID
  AND UserID = @UserID";

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 현재 사용자가 교육 기간 내에 표시할 수 있는 팝업을 조회합니다.
        /// 교육 ID가 0이면 전체, 양수이면 해당 교육만 조회합니다.
        /// </summary>
        public DataTableReader GetList(int _companyID, int _userID, string _nowDate, int _listID = 0)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            using (var cmd = new SqlCommand("DP_EDU_List_SelPopUp", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@NowDate", SqlDbType.Char, 8).Value = _nowDate.ToNull();
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 검색한 교육의 상태별 건수와 과정별 완료 현황을 조회합니다.
        /// </summary>
        public DataSet GetList(int _companyID, string _searchStartDate, string _searchEndDate,
            string _searchItem, string _searchTarget, string _targetIDs)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            using (var cmd = CreateCommand(cn, _companyID, _searchStartDate, _searchEndDate,
                _searchItem, _searchTarget, _targetIDs, "S", "", 0, 20, 0))
            {
                cn.Open();
                return ExecuteDataSet(cmd, "DashBoard");
            }
        }

        private static SqlCommand CreateCommand(SqlConnection cn, int companyID, string startDate,
            string endDate, string item, string target, string targetIDs, string mode,
            string status, int listID, int pageSize, long pageOffset)
        {
            var cmd = new SqlCommand("DP_EDU_List_SelDashBoard", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
            cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = startDate.ToNull().Trim();
            cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = endDate.ToNull().Trim();
            cmd.Parameters.Add("@SearchItem", SqlDbType.NVarChar, 10).Value = item.ToNull().Trim();
            cmd.Parameters.Add("@SearchTarget", SqlDbType.Char, 1).Value = (target ?? "A").Trim();
            cmd.Parameters.Add("@TargetIDs", SqlDbType.NVarChar, -1).Value = targetIDs.ToNull().Trim();
            cmd.Parameters.Add("@Mode", SqlDbType.Char, 1).Value = mode;
            cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = status.ToNull().Trim();
            cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = listID;
            cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
            cmd.Parameters.Add("@PageSizePageNo", SqlDbType.BigInt).Value = pageOffset;
            return cmd;
        }


        #endregion 
    }
}
