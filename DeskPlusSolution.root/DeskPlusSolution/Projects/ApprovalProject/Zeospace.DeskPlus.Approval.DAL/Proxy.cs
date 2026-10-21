using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.DAL
{
    public class Proxy : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private BE.ProxyInfo GetProxyInfo(IDataReader _reader)
        {
            return new ProxyInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                ProxyUserID = (_reader["ProxyUserID"] == DBNull.Value) ? -1 : _reader["ProxyUserID"].ToString().ToNumber<int>(),
                ProxyTitleID = (_reader["ProxyTitleID"] == DBNull.Value) ? -1 : _reader["ProxyTitleID"].ToString().ToNumber<int>(),
                ProxyDeptID = (_reader["ProxyDeptID"] == DBNull.Value) ? -1 : _reader["ProxyDeptID"].ToString().ToNumber<int>()
            };
        }

        /// <summary>
        /// Gets the attach list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ProxyInfo> GetProxyInfoList(IDataReader reader)
        {
            List<ProxyInfo> list = new List<ProxyInfo>();
            while (reader.Read())
                list.Add(GetProxyInfo(reader));
            return list;
        }

        #endregion DataReader

        #region GET()

        #region GetProxyList() - 대결의뢰자 리스트 가져오기

        /// <summary>
        /// 작성자   : 이문호
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 대결의뢰자 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetProxyList(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_SelByUserID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GetProxyList() - 대결의뢰자 리스트 가져오기

        #region GetPageList()

        /// <summary>
        /// 작성자   : 이문호
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 대결자 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _userID, int _deptID, string _pageSize,
            string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_SelList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GetPageList()

        #region GetPageListCount

        /// <summary>
        /// 작성자   : 이문호
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 페이징 총 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_userID">The _userID</param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _userID, int _deptID)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_SelListCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        #endregion GetPageListCount

        #region GetPageListAdmin()

        /// <summary>
        /// 작성자   : 이문호
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 대결자 리스트 가져오기 (관리자)
        /// 수정일자 :
        /// 수정내용 : 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userIDList"></param>
        /// <param name="_deptIDList"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <returns></returns>
        public DataTableReader GetPageListAdmin(int _companyID, string _userIDList, string _deptIDList, string _searchStartDate, string _searchEndDate, string _searchKey, string _searchValue,
            string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_SelListAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 1000).Value = _userIDList.ToNull().ToXSS();
                cmd.Parameters.Add("@DeptID", SqlDbType.VarChar, 1000).Value = _deptIDList.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 19).Value = _searchStartDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 19).Value = _searchEndDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GetPageListAdmin()

        #region GetPageListCountAdmin

        /// <summary>
        /// 작성자   : 이문호
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 페이징 총 카운트 (관리자)
        /// 수정일자 :
        /// 수정내용 : 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userIDList"></param>
        /// <param name="_deptIDList"></param>
        /// <param name="_searchStartDate"></param>
        /// <param name="_searchEndDate"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public int GetPageListCountAdmin(int _companyID, string _userIDList, string _deptIDList, string _searchStartDate, string _searchEndDate, string _searchKey, string _searchValue)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_SelListCntAdmin", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 1000).Value = _userIDList.ToNull().ToXSS();
                cmd.Parameters.Add("@DeptID", SqlDbType.VarChar, 1000).Value = _deptIDList.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 19).Value = _searchStartDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 19).Value = _searchEndDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        #endregion GetPageListCountAdmin

        #region GetProxyCount

        /// <summary>
        /// 작성자   : 이문호
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 대결중인 사람 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_userID">The _userID</param>
        /// <returns></returns>
        public int GetProxyCount(int _companyID, int _userID, int _deptID)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_ProxyCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        #endregion GetProxyCount

        #region 대결자리스트(부서변경사용자)

        /// <summary>
        /// 작성자   :
        /// 작성일자 :
        /// 작성목적 :
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_userID">The _userID</param>
        /// <returns></returns>
        public int GetModifyCount(int _companyID, int _userID, int _deptID)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_SelModifyListCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   :
        /// 작성일자 :
        /// 작성목적 :
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader GetModifyList(int _companyID, int _userID, int _deptID, string _pageSize,
            string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_SelModifyList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion 대결자리스트(부서변경사용자)

        #region GetProxyUser (지정한 대결자 등록되어있는지 조회)

        /// <summary>
        /// 작성자   : 이문호
        /// 작성일자 : 2012-05-09
        /// 작성목적 : 지정한 대결자 등록되어있는지 조회
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_proxyUserID"></param>
        /// <param name="_proxyDeptID"></param>
        /// <param name="_proxyID"></param>
        /// <returns></returns>
        public DataTableReader GetProxyUser(int _companyID, int _userID, int _deptID, int _proxyUserID, int _proxyDeptID, int _proxyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_SelByProxyUser", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@ProxyUserID", SqlDbType.Int).Value = _proxyUserID;
                cmd.Parameters.Add("@ProxyDeptID", SqlDbType.Int).Value = _proxyDeptID;
                cmd.Parameters.Add("@ProxyID", SqlDbType.Int).Value = _proxyID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GetProxyUser (지정한 대결자 등록되어있는지 조회)       

        #region GetProxySelByDate (대결자 중복 기간 체크)

        /// <summary>
        /// 작성목적 : 대결자 중복 기간 체크 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_startDateStr"></param>
        /// <param name="_endDateStr"></param>
        /// <param name="_proxyID"></param>
        /// <returns></returns>
        public DataTableReader GetProxySelByDate(int _companyID, int _userID, int _deptID, string _startDateStr, string _endDateStr, int _proxyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_SelByDate", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@ProxyStartDateStr", SqlDbType.VarChar, 20).Value = _startDateStr;
                cmd.Parameters.Add("@ProxyEndDateStr", SqlDbType.VarChar, 20).Value = _endDateStr;
                cmd.Parameters.Add("@ProxyID", SqlDbType.Int).Value = _proxyID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GetProxySelByDate (대결자 중복 기간 체크)

        #region GetProxyInfoByProxyID (특정 대결자 정보 가져오기)

        /// <summary>
        /// 작성목적 : 특정 대결자 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_proxyID"></param>
        /// <returns></returns>
        public DataTableReader GetProxyInfoByProxyID(int _companyID, int _proxyID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_SelByProxyID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ProxyID", SqlDbType.Int).Value = _proxyID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GetProxyInfoByProxyID (특정 대결자 정보 가져오기)       

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 대결자 추가
        /// </summary>
        /// <returns></returns>
        public bool Add(BE.ProxyInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Proxy_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@ProxyUserID", SqlDbType.Int).Value = _info.ProxyUserID;
                    cmd.Parameters.Add("@ProxyDeptID", SqlDbType.Int).Value = _info.ProxyDeptID;
                    cmd.Parameters.Add("@ProxyStartDate", SqlDbType.DateTime).Value = _info.ProxyStartDate;
                    cmd.Parameters.Add("@ProxyEndDate", SqlDbType.DateTime).Value = _info.ProxyEndDate;
                    cmd.Parameters.Add("@AllDay", SqlDbType.Char).Value = _info.AllDay;
                    cmd.Parameters.Add("@ProxyType", SqlDbType.NVarChar, 100).Value = _info.ProxyType;
                    cmd.Parameters.Add("@ProxyContent", SqlDbType.NVarChar, 500).Value = _info.ProxyContent;
                    cmd.Parameters.Add("@IsCall", SqlDbType.Char).Value = _info.IsCall;
                    cmd.Parameters.Add("@IsProxy", SqlDbType.Char).Value = _info.IsProxy;
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
                throw;
            }
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 대결자 수정
        /// </summary>
        /// <returns></returns>
        public bool Mod(BE.ProxyInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Proxy_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProxyID", SqlDbType.Int).Value = _info.ProxyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@ProxyUserID", SqlDbType.Int).Value = _info.ProxyUserID;
                    cmd.Parameters.Add("@ProxyDeptID", SqlDbType.Int).Value = _info.ProxyDeptID;
                    cmd.Parameters.Add("@ProxyStartDate", SqlDbType.DateTime).Value = _info.ProxyStartDate;
                    cmd.Parameters.Add("@ProxyEndDate", SqlDbType.DateTime).Value = _info.ProxyEndDate;
                    cmd.Parameters.Add("@AllDay", SqlDbType.Char).Value = _info.AllDay;
                    cmd.Parameters.Add("@ProxyType", SqlDbType.NVarChar, 100).Value = _info.ProxyType;
                    cmd.Parameters.Add("@ProxyContent", SqlDbType.NVarChar, 500).Value = _info.ProxyContent;
                    cmd.Parameters.Add("@IsCall", SqlDbType.Char).Value = _info.IsCall;
                    cmd.Parameters.Add("@IsProxy", SqlDbType.Char).Value = _info.IsProxy;
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
                throw;
            }
        }

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 작성자 : 이문호
        /// 목적 : 대결자 삭제하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Del(BE.ProxyInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Proxy_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProxyID", SqlDbType.Int).Value = _info.ProxyID;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   :
        /// 작성일자 :
        /// 작성목적 :
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">컴퍼니아이디</param>
        /// <param name="_userID">The _userID</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID, int _deptID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_APP_Proxy_DelModifyList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();
                ExecuteNonQuery(cmd);
            }
            return true;
        }

        #endregion DEL()

        #region ProxyAction()

        /// <summary>
        /// 대결지정 (대결시작, 대결중지)
        /// </summary>
        /// <returns></returns>
        public bool ProxyAction(BE.ProxyInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_APP_Proxy_Assign", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProxyID", SqlDbType.Int).Value = _info.ProxyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@ProxyUserID", SqlDbType.Int).Value = _info.ProxyUserID;
                    cmd.Parameters.Add("@ProxyDeptID", SqlDbType.Int).Value = _info.ProxyDeptID;
                    cmd.Parameters.Add("@IsProxy", SqlDbType.Char).Value = _info.IsProxy;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }
                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion ProxyAction()
    }
}