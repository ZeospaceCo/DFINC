using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class FaxManager : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_faxID"></param>
        /// <returns></returns>
        public DataTableReader GetList(int _companyID, int _faxID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_FaxManager_SelByFaxID");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@FaxID", SqlDbType.Int).Value = _faxID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #region 페이징

        /// <summary>
        /// 리스트 가져오기
        /// shimpark
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_isActive"></param>
        /// <param name="_gubun"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _searchKey, string _searchValue, bool _isActive, string _gubun, string _order)
        {
            string active = (_isActive) ? "1" : "0,1";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COM_FaxManager_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;
                cmd.Parameters.Add("@IsActive", SqlDbType.VarChar, 50).Value = active;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 개수 가져오기
        /// shimpark
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_isActive"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public int GetPageCount(int _companyID,
            string _searchKey, string _searchValue, bool _isActive, string _gubun)
        {
            string active = (_isActive) ? "1" : "0,1";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COM_FaxManager_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;
                cmd.Parameters.Add("@IsActive", SqlDbType.VarChar, 50).Value = active;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                object obj = ExecuteScalar(cmd);
                return obj.ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 발송 수신자 명단 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_order"></param>
        /// <param name="_guid"></param>
        /// <returns></returns>
        public DataTableReader GetSendRecvPageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _order, string _guid)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COM_FaxManager_SelSendRecvPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@GUID", SqlDbType.VarChar, 32).Value = _guid;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 발송 수신자 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_guid"></param>
        /// <returns></returns>
        public int GetSendRecvPageCount(int _companyID, string _guid)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COM_FaxManager_SelSendRecvPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@GUID", SqlDbType.VarChar, 32).Value = _guid;

                cn.Open();

                object obj = ExecuteScalar(cmd);
                return obj.ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 발송대장 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <param name="_branchID"></param>
        /// <returns></returns>
        public DataTableReader GetSendPageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate,
            string _searchKey, string _searchValue, string _order, int _branchID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COM_FaxManager_SelSendPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = _branchID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 발송대장 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_branchID"></param>
        /// <returns></returns>
        public int GetSendPageCount(int _companyID,
            string _searchStartDate, string _searchEndDate,
            string _searchKey, string _searchValue, int _branchID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COM_FaxManager_SelSendPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = _branchID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                object obj = ExecuteScalar(cmd);
                return obj.ToString().ToNumber<int>();
            }
        }

        /// <summary>
        /// 수신된 fax 리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <param name="_faxID"></param>
        /// <returns></returns>
        public DataTableReader GetRecvPageList(int _companyID, string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchKey, string _searchValue,
            string _order, int _faxID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COM_FaxManager_SelRecvPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FaxID", SqlDbType.Int).Value = _faxID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 수신팩스 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_faxID"></param>
        /// <returns></returns>
        public int GetRecvPageCnt(int _companyID,
            string _searchStartDate, string _searchEndDate, string _searchKey, string _searchValue, int _faxID)
        {
            int rtCnt = 0;

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_COM_FaxManager_SelRecvPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FaxID", SqlDbType.Int).Value = _faxID;
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        #endregion 페이징

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(BE.FaxManagerInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COM_FaxManager_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    //cmd.Parameters.Add("@FaxID", SqlDbType.Int).Value = _info.FaxID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@FaxNumber", SqlDbType.VarChar, 100).Value = _info.FaxNumber.ToNull();
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = _info.BranchID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _info.Gubun.ToNull();
                    cmd.Parameters.Add("@IsActive", SqlDbType.Int).Value = _info.IsActive;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(BE.FaxManagerInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_COM_FaxManager_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@FaxID", SqlDbType.Int).Value = _info.FaxID;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull();
                    cmd.Parameters.Add("@FaxNumber", SqlDbType.VarChar, 100).Value = _info.FaxNumber.ToNull();
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = _info.BranchID;
                    cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _info.Gubun.ToNull();
                    cmd.Parameters.Add("@IsActive", SqlDbType.Int).Value = _info.IsActive;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.ModifyUserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_faxID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _faxID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COM_FaxManager_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FaxID", SqlDbType.Int).Value = _faxID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_faxID"></param>
        /// <returns></returns>
        public bool ModActive(int _companyID, int _faxID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_COM_FaxManager_UpdActive", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@FaxID", SqlDbType.Int).Value = _faxID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion 추가/수정/삭제
    }
}