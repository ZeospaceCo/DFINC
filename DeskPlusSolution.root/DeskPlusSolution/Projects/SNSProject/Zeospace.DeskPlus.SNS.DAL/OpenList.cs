using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.SNS.DAL
{
    public class OpenList : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-05-13
        /// 작성목적 : 공유대상자 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _userID, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_SNS_OpenList_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-05-13
        /// 작성목적 : 공유대상자 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _userID, string _pageSize, string _pageSizePageNo, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_SNS_OpenList_SelPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : famo23
        /// 작성일자 : 2013-05-16
        /// 작성목적 : 나의 담벼락을 보는 사람 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetOpenPageListCount(int _companyID, int _userID, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_SNS_OpenList_SelOpenPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-05-16
        /// 작성목적 : 나의 담벼락을 보는 사람 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataTableReader GetOpenPageList(int _companyID, int _userID, string _pageSize, string _pageSizePageNo, string _searchValue)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_SNS_OpenList_SelOpenPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : famo23
        /// 작성일자 : 2013-06-19
        /// 작성목적 : 공개대상 관리
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_UserID"></param>
        /// <returns></returns>
        public DataSet GetOpenList(int _companyID, int _UserID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.SNS, "QRY_SNS_OpenList_Sel");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;

                cn.Open();
                return ExecuteDataSet(cmd, "SHARE");
            }
        }

        #endregion GET()

        #region DEL

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-06-19
        /// 작성목적 : 공개대상 삭제
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int DelOpenList(BE.OpenListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_SNS_OpenList_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;

                    cn.Open();

                    return ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        #endregion DEL

        #region ADD

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-06-19
        /// 작성목적 : 공개대상 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int AddOpenList(BE.OpenListInfo _info)
        {
            try
            {
                int rtnVal = 0;
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_SNS_OpenList_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@OpenUserID", SqlDbType.Int).Value = _info.OpenUserID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.Int).Value = _info.GroupID;

                    cn.Open();

                    rtnVal = ExecuteNonQuery(cmd);
                }

                return rtnVal;
            }
            catch (SqlException exc)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(exc);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }
        }

        #endregion ADD
    }
}