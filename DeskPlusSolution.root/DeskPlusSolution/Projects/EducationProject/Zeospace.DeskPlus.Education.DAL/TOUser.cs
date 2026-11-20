using System;
using System.Data;
using Zeospace.DeskPlus.Education.BE;
using System.Data.SqlClient;
using System.Globalization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Education.DAL
{
    public class TOUser : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region ADD()

        /// <summary>
        /// 선택한 개인/부서의 등록시점 구성원을 개인별 교육 대상으로 확정합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_writeUserID"></param>
        /// <returns></returns>
        public int Add(int _companyID, int _listID, int _writeUserID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EDU_TOUser_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _writeUserID;

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

        #region 완료 취소

        /// <summary>
        /// 첨부파일을 유지하고 해당 대상자의 완료 상태만 취소합니다.
        /// </summary>
        public bool CancelComplete(int _companyID, int _listID, int _userID)
        {
            if (_companyID <= 0 || _listID <= 0 || _userID <= 0) return false;

            const string query = @"
DECLARE @Cancelled bit, @TargetListID int, @TargetUserID int;
DECLARE @Today char(8), @StartDate char(8), @EndDate char(8), @RestoredStatus char(1);
SET @Cancelled = 0;

SELECT @TargetListID = L.ListID, @StartDate = L.StartDate, @EndDate = L.EndDate
FROM dbo.DP_EDU_List AS L WITH (UPDLOCK, HOLDLOCK)
WHERE L.CompanyID = @CompanyID AND L.ListID = @ListID;

IF @TargetListID IS NOT NULL
BEGIN
    SELECT @TargetUserID = U.UserID
    FROM dbo.DP_EDU_TOUser AS U WITH (UPDLOCK, HOLDLOCK)
    WHERE U.CompanyID = @CompanyID AND U.ListID = @ListID
      AND U.UserID = @UserID AND U.Status = '3' AND ISNULL(U.ConfirmYN, 'N') <> 'Y';
END;

IF @TargetUserID IS NOT NULL
BEGIN
    IF @StartDate IS NULL OR @EndDate IS NULL
        OR LEN(@StartDate) <> 8 OR LEN(@EndDate) <> 8
        OR @StartDate COLLATE Latin1_General_BIN LIKE '%[^0-9]%'
        OR @EndDate COLLATE Latin1_General_BIN LIKE '%[^0-9]%'
        OR ISDATE(@StartDate) <> 1 OR ISDATE(@EndDate) <> 1
        OR @StartDate > @EndDate
    BEGIN
        RAISERROR(N'교육 기간 정보가 올바르지 않아 완료를 취소할 수 없습니다.', 16, 1);
        RETURN;
    END;

    SET @Today = CONVERT(char(8), GETDATE(), 112);
    SET @RestoredStatus = CASE WHEN @Today < @StartDate THEN '1'
        WHEN @Today > @EndDate THEN '4' ELSE '2' END;

    UPDATE dbo.DP_EDU_TOUser
    SET Status = @RestoredStatus, CompleteDate = NULL, CompleteUserID = NULL
    WHERE CompanyID = @CompanyID AND ListID = @ListID AND UserID = @UserID AND Status = '3'
      AND ISNULL(ConfirmYN, 'N') <> 'Y';
    IF @@ROWCOUNT = 1 SET @Cancelled = 1;
END;

SELECT @Cancelled;";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                using (var cmd = new SqlCommand(query, cn, transaction))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        throw new InvalidOperationException("교육 완료 취소 결과를 확인할 수 없습니다.");
                    bool cancelled = Convert.ToBoolean(result, CultureInfo.InvariantCulture);
                    transaction.Commit();
                    return cancelled;
                }
            }
        }

        #endregion

        #region 관리자 확인

        /// <summary>
        /// 교육 완료 여부와 독립된 관리자의 확인 상태를 조회합니다.
        /// </summary>
        public DataTableReader GetConfirmation(int _companyID, int _listID, int _userID)
        {
            ValidateConfirmationTarget(_companyID, _listID, _userID);

            const string query = @"
SELECT E.CompanyID, E.ListID, E.UserID, E.ConfirmYN, E.ConfirmUserID, E.ConfirmDate,
       CU.NameBase AS ConfirmNameBase, CU.NameKo AS ConfirmNameKo,
       CU.NameEn AS ConfirmNameEn, CU.NameJa AS ConfirmNameJa,
       CU.NameFr AS ConfirmNameFr, CU.NameZhcn AS ConfirmNameZhcn, CU.NameZhtw AS ConfirmNameZhtw
FROM dbo.DP_EDU_TOUser AS E
INNER JOIN dbo.DP_EDU_List AS L ON L.CompanyID = E.CompanyID AND L.ListID = E.ListID
LEFT JOIN dbo.DP_ACC_User AS CU ON CU.CompanyID = E.CompanyID AND CU.UserID = E.ConfirmUserID
WHERE E.CompanyID = @CompanyID AND E.ListID = @ListID AND E.UserID = @UserID";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            using (var cmd = new SqlCommand(query, cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 관리자의 확인 여부만 저장하고 서버에 저장된 최종 확인 정보를 반환합니다.
        /// 동일한 확인 요청은 최초 확인자와 확인일을 유지합니다.
        /// </summary>
        public DataTableReader SetConfirmation(int _companyID, int _listID, int _userID,
            string _confirmYN, int _confirmUserID)
        {
            ValidateConfirmationTarget(_companyID, _listID, _userID);
            _confirmYN = ValidateSearchConfirm(_confirmYN);
            if (_confirmYN == "") throw new ArgumentException("확인 여부가 올바르지 않습니다.", "confirmYN");
            if (_confirmUserID <= 0) throw new ArgumentOutOfRangeException("confirmUserID");

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            using (var cmd = new SqlCommand("DP_EDU_TOUser_SetConfirmation", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@ConfirmYN", SqlDbType.Char, 1).Value = _confirmYN;
                cmd.Parameters.Add("@ConfirmUserID", SqlDbType.Int).Value = _confirmUserID;
                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        private static void ValidateConfirmationTarget(int companyID, int listID, int userID)
        {
            if (companyID <= 0) throw new ArgumentOutOfRangeException("companyID");
            if (listID <= 0) throw new ArgumentOutOfRangeException("listID");
            if (userID <= 0) throw new ArgumentOutOfRangeException("userID");
        }

        #endregion

        #region GET()

        /// <summary>
        /// 해당 교육 대상자의 최신 개인별 상태를 조회합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetTargetUser(int _companyID, int _listID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"
SELECT E.UserID, E.Status, E.CompleteDate, U.LogInID, U.NameBase, U.NameKo, U.NameEn, U.NameJa,
       U.NameFr, U.NameZhcn, U.NameZhtw, E.CompleteUserID, L.FileYN, L.StartDate,
       CU.NameBase AS CompleteNameBase, CU.NameKo AS CompleteNameKo,
       CU.NameEn AS CompleteNameEn, CU.NameJa AS CompleteNameJa,
       CU.NameFr AS CompleteNameFr, CU.NameZhcn AS CompleteNameZhcn, CU.NameZhtw AS CompleteNameZhtw
FROM dbo.DP_EDU_TOUser AS E
INNER JOIN dbo.DP_EDU_List AS L ON L.CompanyID = E.CompanyID AND L.ListID = E.ListID
LEFT JOIN dbo.DP_ACC_User AS U ON U.CompanyID = E.CompanyID AND U.UserID = E.UserID
LEFT JOIN dbo.DP_ACC_User AS CU ON CU.CompanyID = E.CompanyID AND CU.UserID = E.CompleteUserID
WHERE E.CompanyID = @CompanyID AND E.ListID = @ListID AND E.UserID = @UserID";

                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
        }

        /// <summary>
        /// 미완료 재직 대상자의 알람 수신자를 조회합니다. 사용자 ID가 0이면 전체 대상자입니다.
        /// 진행상태가 지정되면 저장된 상태 대신 교육기간을 기준으로 현재 상태를 판정합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStatus">빈 값은 모든 미완료, 2는 진행, 4는 지연입니다.</param>
        /// <returns></returns>
        public DataTableReader GetAlarmTargets(int _companyID, int _listID, int _userID, string _searchStatus = "")
        {
            if (_userID < 0)
                throw new ArgumentOutOfRangeException("userID");
            _searchStatus = (_searchStatus ?? "").Trim();
            if (_searchStatus != "" && _searchStatus != "2" && _searchStatus != "4")
                throw new ArgumentException("알람 발송 진행상태가 올바르지 않습니다.", "searchStatus");

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"
DECLARE @Today char(8);
SET @Today = CONVERT(char(8), GETDATE(), 112);
SELECT DISTINCT E.UserID, L.Title
FROM dbo.DP_EDU_TOUser AS E
INNER JOIN dbo.DP_EDU_List AS L ON L.CompanyID = E.CompanyID AND L.ListID = E.ListID
WHERE E.CompanyID = @CompanyID AND E.ListID = @ListID
  AND ISNULL(E.Status, '') <> '3'
  AND (@UserID = 0 OR E.UserID = @UserID)
  AND (@SearchStatus = ''
       OR CASE WHEN E.Status = '3' THEN '3'
               WHEN @Today < L.StartDate THEN '1'
               WHEN @Today > L.EndDate THEN '4'
               ELSE '2' END = @SearchStatus)
  AND EXISTS (
      SELECT 1
      FROM dbo.VW_UserInfo AS U
      WHERE U.CompanyID = E.CompanyID AND U.UserID = E.UserID
        AND U.ActiveCompany = 1 AND U.ActiveUser = 1
  )
ORDER BY E.UserID";

                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
        }

        /// <summary>
        /// 메일 보기 링크의 대상자가 현재 회사의 교육에 등록된 활성 사용자인지 확인합니다.
        /// 완료 이후에도 조회할 수 있지만 삭제된 대상자와 비활성 계정은 허용하지 않습니다.
        /// </summary>
        public bool CanReadMailView(int companyID, int listID, int userID)
        {
            if (companyID <= 0 || listID <= 0 || userID <= 0) return false;
            const string query = @"SELECT CASE WHEN EXISTS (
    SELECT 1
    FROM dbo.DP_EDU_TOUser AS E
    INNER JOIN dbo.DP_EDU_List AS L ON L.CompanyID = E.CompanyID AND L.ListID = E.ListID
    WHERE E.CompanyID = @CompanyID AND E.ListID = @ListID AND E.UserID = @UserID
      AND EXISTS (
          SELECT 1 FROM dbo.VW_UserInfo AS U
          WHERE U.CompanyID = E.CompanyID AND U.UserID = E.UserID
            AND U.ActiveCompany = 1 AND U.ActiveUser = 1
      )
) THEN 1 ELSE 0 END";
            using (var cn = new SqlConnection(ConnSolution(companyID)))
            using (var cmd = new SqlCommand(query, cn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = listID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        /// <summary>
        /// 진행 중인 미완료자 또는 지연 대상자의 메일 계정 정보를 조회합니다.
        /// 메일 주소가 없는 대상자도 반환하여 발송 결과에서 누락 여부를 확인할 수 있습니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_progressStatus">2는 진행, 4는 지연입니다.</param>
        /// <returns></returns>
        public DataTableReader GetMailTargets(int _companyID, int _listID, string _progressStatus)
        {
            if (_companyID <= 0)
                throw new ArgumentOutOfRangeException("companyID");
            if (_listID <= 0)
                throw new ArgumentOutOfRangeException("listID");
            _progressStatus = (_progressStatus ?? "").Trim();
            if (_progressStatus != "2" && _progressStatus != "4")
                throw new ArgumentException("메일 발송 진행상태가 올바르지 않습니다.", "progressStatus");

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"
DECLARE @Today char(8);
SET @Today = CONVERT(char(8), GETDATE(), 112);
SELECT DISTINCT E.UserID, U.MailID, U.MailDomain,
       U.NameBase, U.NameKo, U.NameEn, U.NameJa,
       U.NameFr, U.NameZhcn, U.NameZhtw
FROM dbo.DP_EDU_TOUser AS E
INNER JOIN dbo.DP_EDU_List AS L ON L.CompanyID = E.CompanyID AND L.ListID = E.ListID
INNER JOIN dbo.DP_ACC_User AS U ON U.CompanyID = E.CompanyID AND U.UserID = E.UserID
WHERE E.CompanyID = @CompanyID AND E.ListID = @ListID
  AND ISNULL(E.Status, '') <> '3'
  AND CASE WHEN E.Status = '3' THEN '3'
           WHEN @Today < L.StartDate THEN '1'
           WHEN @Today > L.EndDate THEN '4'
           ELSE '2' END = @ProgressStatus
  AND EXISTS (
      SELECT 1
      FROM dbo.VW_UserInfo AS A
      WHERE A.CompanyID = E.CompanyID AND A.UserID = E.UserID
        AND A.ActiveCompany = 1 AND A.ActiveUser = 1
  )
ORDER BY E.UserID";

                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@ProgressStatus", SqlDbType.Char, 1).Value = _progressStatus;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
        }

        /// <summary>
        /// 해당 교육에 등록된 전체 대상자 수를 조회합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public int GetTargetListCount(int _companyID, int _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = "SELECT COUNT(*)" + AdminFrom
                    + "WHERE E.CompanyID = @CompanyID AND E.ListID = @ListID";
                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cn.Open();

                    return Convert.ToInt32(ExecuteScalar(cmd), CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        /// 등록시점 대상자 정보와 함께 저장된 최상위 사업장을 페이징하여 조회합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_outputCount"></param>
        /// <param name="_pageOffset"></param>
        /// <returns></returns>
        public DataTableReader GetTargetPageList(int _companyID, int _listID, int _outputCount, int _pageOffset)
        {
            if (_outputCount < 1 || _outputCount > 100)
                throw new ArgumentOutOfRangeException("outputCount");
            if (_pageOffset < 0)
                throw new ArgumentOutOfRangeException("pageOffset");

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = @"
DECLARE @Today char(8);
SET @Today = CONVERT(char(8), GETDATE(), 112);
;WITH TargetRows AS (
SELECT E.UserID, E.CompleteDate,
       CASE WHEN E.Status = '3' THEN '3'
            WHEN @Today < L.StartDate THEN '1'
            WHEN @Today > L.EndDate THEN '4'
            ELSE '2' END AS ProgressStatus,
       E.WorkplaceDeptID, E.WorkplaceBase, E.WorkplaceKo,
       E.WorkplaceEn, E.WorkplaceJa, E.WorkplaceFr,
       E.WorkplaceZhcn, E.WorkplaceZhtw,
       ISNULL(DC.DeptBase, D.NameBase) AS DeptBase,
       ISNULL(DC.DeptKo, D.NameKo) AS DeptKo,
       ISNULL(DC.DeptEn, D.NameEn) AS DeptEn,
       ISNULL(DC.DeptJa, D.NameJa) AS DeptJa,
       ISNULL(DC.DeptFr, D.NameFr) AS DeptFr,
       ISNULL(DC.DeptZhcn, D.NameZhcn) AS DeptZhcn,
       ISNULL(DC.DeptZhtw, D.NameZhtw) AS DeptZhtw,
       DC.TitleBase, DC.TitleKo, DC.TitleEn, DC.TitleJa,
       DC.TitleFr, DC.TitleZhcn, DC.TitleZhtw,
       U.NameBase, U.NameKo, U.NameEn, U.NameJa,
       U.NameFr, U.NameZhcn, U.NameZhtw,
       ROW_NUMBER() OVER (ORDER BY ISNULL(DC.DeptBase, D.NameBase), U.NameBase, E.UserID) AS RowNumber"
                    + AdminFrom + @"WHERE E.CompanyID = @CompanyID AND E.ListID = @ListID
)
SELECT * FROM TargetRows
WHERE RowNumber BETWEEN @StartRow AND @EndRow
ORDER BY RowNumber";

                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@StartRow", SqlDbType.BigInt).Value = (long)_pageOffset + 1;
                    cmd.Parameters.Add("@EndRow", SqlDbType.BigInt).Value = (long)_pageOffset + _outputCount;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
        }

        /// <summary>
        /// SetPaging에서 계산한 출력 건수와 시작 위치로 사용자별 교육 목록을 조회합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchItem"></param>
        /// <param name="_searchComplete"></param>
        /// <param name="_searchDateField"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchTarget"></param>
        /// <param name="_targetIDs"></param>
        /// <param name="_searchUserField"></param>
        /// <param name="_searchUserValue"></param>
        /// <param name="_searchTitle"></param>
        /// <param name="_searchEmployment"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchListID">0이면 전체 교육, 양수이면 해당 교육만 조회합니다.</param>
        /// <param name="_searchConfirm">빈 값은 전체, Y는 확인, N은 미확인입니다.</param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _searchItem, string _searchComplete, string _searchDateField,
            string _searchStartDate, string _searchEndDate, string _searchTarget, string _targetIDs,
            string _searchUserField, string _searchUserValue, string _searchTitle, string _searchEmployment,
            string _searchStatus = "", int _searchListID = 0, string _searchConfirm = "")
        {
            _searchStatus = ValidateSearchStatus(_searchStatus);
            _searchConfirm = ValidateSearchConfirm(_searchConfirm);
            if (_searchListID < 0) throw new ArgumentOutOfRangeException("searchListID");
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_EDU_TOUser_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchItem", SqlDbType.NVarChar, 10).Value = _searchItem.ToNull().Trim();
                cmd.Parameters.Add("@SearchComplete", SqlDbType.Char, 1).Value = _searchComplete.ToNull().Trim();
                cmd.Parameters.Add("@SearchDateField", SqlDbType.VarChar, 50).Value = (_searchDateField ?? "StartDate").Trim();
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate.ToNull().Trim();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate.ToNull().Trim();
                cmd.Parameters.Add("@SearchTarget", SqlDbType.Char, 1).Value = (_searchTarget ?? "A").Trim();
                cmd.Parameters.Add("@TargetIDs", SqlDbType.NVarChar, -1).Value = _targetIDs.ToNull().Trim();
                cmd.Parameters.Add("@SearchUserField", SqlDbType.VarChar, 50).Value = (_searchUserField ?? "Name").Trim();
                cmd.Parameters.Add("@SearchUserValue", SqlDbType.NVarChar, 1000).Value = _searchUserValue.ToNull().Trim().ToXSS();
                cmd.Parameters.Add("@SearchTitle", SqlDbType.NVarChar, 1000).Value = _searchTitle.ToNull().Trim().ToXSS();
                cmd.Parameters.Add("@SearchEmployment", SqlDbType.Char, 1).Value = _searchEmployment.ToNull().Trim();
                cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                cmd.Parameters.Add("@SearchListID", SqlDbType.Int).Value = _searchListID;
                cmd.Parameters.Add("@SearchConfirm", SqlDbType.Char, 1).Value = _searchConfirm;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.BigInt).Value = _pageSizePageNo;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 사용자별 교육 목록과 같은 검색조건으로 전체 건수를 조회합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchItem"></param>
        /// <param name="_searchComplete"></param>
        /// <param name="_searchDateField"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchTarget"></param>
        /// <param name="_targetIDs"></param>
        /// <param name="_searchUserField"></param>
        /// <param name="_searchUserValue"></param>
        /// <param name="_searchTitle"></param>
        /// <param name="_searchEmployment"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchListID">0이면 전체 교육, 양수이면 해당 교육만 조회합니다.</param>
        /// <param name="_searchConfirm">빈 값은 전체, Y는 확인, N은 미확인입니다.</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _searchItem, string _searchComplete,
            string _searchDateField, string _searchStartDate, string _searchEndDate,
            string _searchTarget, string _targetIDs, string _searchUserField,
            string _searchUserValue, string _searchTitle, string _searchEmployment, string _searchStatus = "",
            int _searchListID = 0, string _searchConfirm = "")
        {
            _searchStatus = ValidateSearchStatus(_searchStatus);
            _searchConfirm = ValidateSearchConfirm(_searchConfirm);
            if (_searchListID < 0) throw new ArgumentOutOfRangeException("searchListID");
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_EDU_TOUser_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchItem", SqlDbType.NVarChar, 10).Value = _searchItem.ToNull().Trim();
                cmd.Parameters.Add("@SearchComplete", SqlDbType.Char, 1).Value = _searchComplete.ToNull().Trim();
                cmd.Parameters.Add("@SearchDateField", SqlDbType.VarChar, 50).Value = (_searchDateField ?? "StartDate").Trim();
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate.ToNull().Trim();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate.ToNull().Trim();
                cmd.Parameters.Add("@SearchTarget", SqlDbType.Char, 1).Value = (_searchTarget ?? "A").Trim();
                cmd.Parameters.Add("@TargetIDs", SqlDbType.NVarChar, -1).Value = _targetIDs.ToNull().Trim();
                cmd.Parameters.Add("@SearchUserField", SqlDbType.VarChar, 50).Value = (_searchUserField ?? "Name").Trim();
                cmd.Parameters.Add("@SearchUserValue", SqlDbType.NVarChar, 1000).Value = _searchUserValue.ToNull().Trim().ToXSS();
                cmd.Parameters.Add("@SearchTitle", SqlDbType.NVarChar, 1000).Value = _searchTitle.ToNull().Trim().ToXSS();
                cmd.Parameters.Add("@SearchEmployment", SqlDbType.Char, 1).Value = _searchEmployment.ToNull().Trim();
                cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                cmd.Parameters.Add("@SearchListID", SqlDbType.Int).Value = _searchListID;
                cmd.Parameters.Add("@SearchConfirm", SqlDbType.Char, 1).Value = _searchConfirm;
                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 관리자의 사용자별 교육 목록을 조회합니다. 페이지 크기가 0이면 전체 검색 결과를 반환합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_search"></param>
        /// <param name="_pageIndex"></param>
        /// <param name="_pageSize"></param>
        /// <returns></returns>
        public DataTable GetAdminList(int _companyID, AdminListInfo _search, int _pageIndex, int _pageSize)
        {
            if (_search == null) throw new ArgumentNullException("search");
            if (_pageIndex < 1) _pageIndex = 1;
            if (_pageSize < 0 || _pageSize > 250) throw new ArgumentOutOfRangeException("pageSize");

            using (DataTableReader reader = GetPageList(_companyID,
                _pageSize.ToString(CultureInfo.InvariantCulture),
                (((long)_pageIndex - 1) * _pageSize).ToString(CultureInfo.InvariantCulture),
                _search.ItemID, _search.Complete, _search.DateField, _search.StartDate, _search.EndDate,
                _search.TargetType, _search.TargetIDs, _search.UserField, _search.UserValue, _search.Title, _search.Employment,
                _search.SearchStatus, _search.SearchListID, _search.SearchConfirm))
            {
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        /// <summary>
        /// 사용자별 교육 목록과 같은 검색조건으로 전체 건수를 조회합니다.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_search"></param>
        /// <returns></returns>
        public int GetAdminListCount(int _companyID, AdminListInfo _search)
        {
            if (_search == null) throw new ArgumentNullException("search");
            return GetPageListCount(_companyID, _search.ItemID, _search.Complete, _search.DateField,
                _search.StartDate, _search.EndDate, _search.TargetType, _search.TargetIDs,
                _search.UserField, _search.UserValue, _search.Title, _search.Employment, _search.SearchStatus,
                _search.SearchListID, _search.SearchConfirm);
        }

        private static string ValidateSearchConfirm(string confirm)
        {
            confirm = (confirm ?? "").Trim();
            if (confirm != "" && confirm != "Y" && confirm != "N")
                throw new ArgumentException("확인 여부 검색조건이 올바르지 않습니다.", "confirm");
            return confirm;
        }

        private static string ValidateSearchStatus(string status)
        {
            status = (status ?? "").Trim();
            if (status != "" && status != "1" && status != "2" && status != "3" && status != "4")
                throw new ArgumentException("진행상태 검색조건이 올바르지 않습니다.", "status");
            return status;
        }

        #endregion

        #region Query

        // 사용자 계정은 직접 조인하여 겸직 부서 수만큼 목록이 중복되지 않도록 합니다.
        private const string AdminFrom = @"
FROM dbo.DP_EDU_TOUser AS E
INNER JOIN dbo.DP_EDU_List AS L ON L.CompanyID = E.CompanyID AND L.ListID = E.ListID
LEFT JOIN dbo.DP_ACC_User AS U ON U.CompanyID = E.CompanyID AND U.UserID = E.UserID
LEFT JOIN dbo.DP_COM_DeptCode AS DC ON DC.CompanyID = E.CompanyID AND DC.DeptCodeID = E.DeptCodeID
LEFT JOIN dbo.DP_ACC_Dept AS D ON D.CompanyID = E.CompanyID AND D.DeptID = E.DeptID
";

        #endregion
    }
}
