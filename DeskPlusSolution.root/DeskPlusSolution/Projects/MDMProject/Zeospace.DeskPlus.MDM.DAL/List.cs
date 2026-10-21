using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.MDM.BE;

namespace Zeospace.DeskPlus.MDM.DAL
{

    public class List : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-01-16
        /// 작성목적 : 모바일 기기 승인요청, 승인거부 리스트 갯수
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_activeMobile"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_activeUser"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _activeMobile, string _searchField, string _searchValue, string _startDate, string _endDate, bool _activeUser)
        {
            int rtCnt = 0;
            string activeUser = (_activeUser) ? "1" : "0";
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_List_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ActiveMobile", SqlDbType.Char, 1).Value = _activeMobile.ToNull().ToXSS();
                cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = activeUser.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _startDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _endDate.ToNull().ToXSS();

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-01-16
        /// 작성목적 : 모바일 기기 승인요청, 승인거부 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_activeMobile"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <param name="_order"></param>
        /// <param name="_activeUser"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _activeMobile, string _pageSize, string _pageSizePageNo,
            string _searchField, string _searchValue, string _startDate, string _endDate, string _order, bool _activeUser)
        {
            string activeUser = (_activeUser) ? "1" : "0";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_List_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ActiveMobile", SqlDbType.Char, 1).Value = _activeMobile.ToNull().ToXSS();
                cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = activeUser.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _startDate.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _endDate.ToNull().ToXSS();
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order.ToNull().ToXSS(); ;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 모바일 기기 사용자 리스트 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptIDList"></param>
        /// <param name="_searchKey"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_activeUser"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, string _deptIDList, string _searchKey, string _searchValue, bool _activeUser)
        {
            string activeUser = (_activeUser) ? "1" : "0";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_List_SelUserPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.VarChar, 1000).Value = _deptIDList.ToNull().ToXSS();
                cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = activeUser.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchKey.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();

                cn.Open();

                return Convert.ToInt32(ExecuteScalar(cmd));
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-01-16
        /// 작성목적 : 모바일 기기 승인요청, 승인거부 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptIDList"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <param name="_searchField"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_order"></param>
        /// <param name="_activeUser"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, string _deptIDList, string _pageSize, string _pageSizePageNo,
            string _searchField, string _searchValue, string _order, bool _activeUser)
        {
            string activeUser = (_activeUser) ? "1" : "0";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_List_SelUserPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.VarChar, 1000).Value = _deptIDList.ToNull().ToXSS();
                cmd.Parameters.Add("@ActiveUser", SqlDbType.Char, 1).Value = activeUser.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField.ToNull().ToXSS();
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue.ToNull().ToXSS();
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
                cmd.Parameters.Add("@Order", SqlDbType.NVarChar, 100).Value = _order.ToNull().ToXSS(); ;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-02-15
        /// 작성목적 : 사용자별 모바일 단말기정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>        
        /// <returns></returns>
        public DataTableReader GetListUUID(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.MDM, "QRY_MDM_List_Sel");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-08
        /// 작성목적 : 사용자별 모바일 단말기정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>        
        /// <returns></returns>
        public DataTableReader GetListUUIDAll(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.MDM, "QRY_MDM_List_SelAll");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-02-15
        /// 작성목적 : 모바일 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataTableReader GetListInfo(int _companyID, int _userID, int _deptID, int _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_List_Sel", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-05
        /// 작성목적 : 선택한 모바일 리스트 사용자 정보 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public DataTableReader GetUserList(int _companyID, string _listID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_List_SelByListID", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 4000).Value = _listID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 승인요청 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>        
        /// <returns></returns>
        public bool Del(int _companyID, string _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 4000).Value = _listID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-18
        /// 작성목적 : 모바일 사용자 정보 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _userID, int _listID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_DelByListID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }



        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 거부
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_activeMobile"></param>
        /// <param name="_userID"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        public bool Mod(int _companyID, string _listID, string _activeMobile, int _userID, string _name)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.VarChar, 4000).Value = _listID;
                    cmd.Parameters.Add("@ActiveMobile", SqlDbType.Char, 1).Value = _activeMobile.ToNull().ToXSS();
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _name.ToNull().ToXSS();
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-01-07
        /// 작성목적 : 승인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_userID"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        public bool ModApprove(int _companyID, int _listID, int _userID, string _name)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_UpdApprove", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _listID;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _name.ToNull().ToXSS();
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-18
        /// 작성목적 : 모바일 사용자 정보 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_UpdByListID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@ActiveMobile", SqlDbType.Char, 1).Value = _info.ActiveMobile.ToNull().ToXSS();
                    cmd.Parameters.Add("@MailAlarm", SqlDbType.Char, 1).Value = _info.MailAlarm.ToNull().ToXSS();
                    cmd.Parameters.Add("@EtcAlarm", SqlDbType.VarChar, 300).Value = _info.EtcAlarm.ToNull().ToXSS();
                    cmd.Parameters.Add("@LoginType", SqlDbType.Char, 1).Value = _info.LoginType.ToNull().ToXSS();
                    cmd.Parameters.Add("@AccessRight", SqlDbType.Char, 1).Value = _info.AccessRight.ToNull().ToXSS();
                    cmd.Parameters.Add("@Memo", SqlDbType.NVarChar, 4000).Value = _info.Memo.ToNull().ToXSS();
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _info.ModifyUserName.ToNull().ToXSS();
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-02-26
        /// 작성목적 : 접속불가처리
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_accessRight"></param>
        /// <param name="_modifyuserID"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        public bool ModUser(int _companyID, string _userID, string _accessRight, int _modifyuserID, string _name)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_UpdByUID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 4000).Value = _userID;
                    cmd.Parameters.Add("@AccessRight", SqlDbType.Char, 1).Value = _accessRight.ToNull().ToXSS();
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _modifyuserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _name.ToNull().ToXSS();
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-06
        /// 작성목적 : 설정값변경
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModAll(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_UpdAll", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@MailAlarm", SqlDbType.Char, 1).Value = _info.MailAlarm;
                    cmd.Parameters.Add("@EtcAlarm", SqlDbType.VarChar, 300).Value = _info.EtcAlarm;
                    cmd.Parameters.Add("@LoginType", SqlDbType.Char, 1).Value = _info.LoginType;
                    cmd.Parameters.Add("@AccessRight", SqlDbType.Char, 1).Value = _info.AccessRight;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _info.ModifyUserName.ToNull().ToXSS();
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-18
        /// 작성목적 : 모바일 사용자 정보 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModUser(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_UpdByUser", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@ListID", SqlDbType.Int).Value = _info.ListID;
                    cmd.Parameters.Add("@MailAlarm", SqlDbType.Char, 1).Value = _info.MailAlarm.ToNull().ToXSS();
                    cmd.Parameters.Add("@EtcAlarm", SqlDbType.VarChar, 300).Value = _info.EtcAlarm.ToNull().ToXSS();
                    cmd.Parameters.Add("@LoginType", SqlDbType.Char, 1).Value = _info.LoginType.ToNull().ToXSS();
                    cmd.Parameters.Add("@Memo", SqlDbType.NVarChar, 4000).Value = _info.Memo.ToNull().ToXSS();
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _info.ModifyUserName.ToNull().ToXSS();
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }


        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-04-25
        /// 작성목적 : 알람 설정 변경
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModAlarm(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_UpdAlarm", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@MailAlarm", SqlDbType.Char, 1).Value = _info.MailAlarm;
                    cmd.Parameters.Add("@EtcAlarm", SqlDbType.VarChar, 300).Value = _info.EtcAlarm;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _info.ModifyUserName.ToNull().ToXSS();
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-04-25
        /// 작성목적 : 로그인 요청정보 변경
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModLogin(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_UpdLogin", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@LoginType", SqlDbType.Char, 1).Value = _info.LoginType;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _info.ModifyUserName.ToNull().ToXSS();
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        /// <summary>
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-04-25
        /// 작성목적 : 모바일 접근 권한 변경
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModAuth(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_UpdAccess", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@AccessRight", SqlDbType.Char, 1).Value = _info.AccessRight;
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _info.ModifyUserName.ToNull().ToXSS();
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-02-18
        /// 작성목적 : 사용자 환경정보 저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(ListInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_List_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@AccessRight", SqlDbType.Char, 1).Value = _info.AccessRight.ToNull().ToXSS();
                    cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
                    cmd.Parameters.Add("@ModifyUserName", SqlDbType.NVarChar, 50).Value = _info.ModifyUserName.ToNull().ToXSS();
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }
        }

        #endregion
    }
}
