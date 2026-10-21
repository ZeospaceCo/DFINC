using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.DAL
{
    public class Task : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public Task()
        {
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 요청한 협업
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetRequest(int _companyID, int _userID, string _pageSize, string _pageSizePageNo, string _searchStatus, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TASK_SelReqList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize.ToNumber<int>();
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo.ToNumber<int>();
                    cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                    cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 100).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 요청받은 협업
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetReceive(int _companyID, int _userID, string _pageSize, string _pageSizePageNo, string _searchStatus, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TASK_SelTOList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize.ToNumber<int>();
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo.ToNumber<int>();
                    cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                    cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 100).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 참조받은 협업
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetReference(int _companyID, int _userID, string _pageSize, string _pageSizePageNo, string _searchStatus, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TASK_SelCCList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize.ToNumber<int>();
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo.ToNumber<int>();
                    cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                    cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 100).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 전달받은 협업
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetForward(int _companyID, int _userID, string _pageSize, string _pageSizePageNo, string _searchStatus, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TASK_SelForwardList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize.ToNumber<int>();
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo.ToNumber<int>();
                    cmd.Parameters.Add("@SearchStatus", SqlDbType.Char, 1).Value = _searchStatus;
                    cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 100).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 요청한 협업 갯수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetRequestCnt(int _companyID, int _userID, string _searchStatus, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TASK_SelReqCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SearchStatus", SqlDbType.VarChar, 10).Value = _searchStatus;
                    cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 100).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 요청받은 협업 갯수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetReceiveCnt(int _companyID, int _userID, string _searchStatus, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TASK_SelTOCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SearchStatus", SqlDbType.VarChar, 10).Value = _searchStatus;
                    cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 100).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 참조 받은 협업 갯수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetReferenceCnt(int _companyID, int _userID, string _searchStatus, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TASK_SelCCCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SearchStatus", SqlDbType.VarChar, 10).Value = _searchStatus;
                    cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 100).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 전달 받은 협업 갯수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchStatus"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetForwardCnt(int _companyID, int _userID, string _searchStatus, string _searchField, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TASK_SelForwardCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@SearchStatus", SqlDbType.VarChar, 10).Value = _searchStatus;
                    cmd.Parameters.Add("@SearchField", SqlDbType.VarChar, 100).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

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

        #region [TODO팝업]

        /// <summary>
        /// [TODO팝업] 업무협업 - 건수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_section"></param>
        /// <returns></returns>
        public int GetTasCnt(int _companyID, int _userID, string _section)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TodoPopup_TASCnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _section;
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

        /// <summary>
        /// [TODO팝업] 업무협업 - 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_section"></param>
        /// <returns></returns>
        public DataTableReader GetForward(int _companyID, int _userID, int _pageSize, string _section)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TodoPopup_TASList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@Section", SqlDbType.Char, 1).Value = _section;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
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
    }
}