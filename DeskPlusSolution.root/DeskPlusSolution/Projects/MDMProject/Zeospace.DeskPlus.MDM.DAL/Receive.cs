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
    public class Receive : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()
        
        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2020-06-01
        /// 작성목적 : 수신자 검색
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchValue"></param>
        /// <returns></returns>
        public DataSet GetUserDept(int _companyID, string _searchValue)
        {
            using (var cn = new SqlConnection(ConnSolution()))
            {
                var cmd = new SqlCommand("DP_ACC_UserEtc_SelName", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                cn.Open();

                return ExecuteDataSet(cmd, "GetUserDept");
            }
        }

        /// <summary>
        /// 부서 아이디로 사용자 정보 가져오기 (하위부서까지)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public DataSet GetUserInfoByDept(int _companyID, string _deptID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.MDM, "QRY_MDM_ReceiveEtc_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetUserInfoByDept");
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2020-06-02
        /// 작성목적 : 수신자 내역 카운트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendID"></param>
        /// <returns></returns>
        public int GetPageListCount(int _companyID, int _sendID)
        {
            int rtCnt = 0;

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_Receive_SelPageCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SendID", SqlDbType.Int).Value = _sendID;

                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
            }

            return rtCnt;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2020-06-02
        /// 작성목적 : 수신자 내역 리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendID"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageSizePageNo"></param>
        /// <returns></returns>
        public DataTableReader GetPageList(int _companyID, int _sendID, string _pageSize, string _pageSizePageNo)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_MDM_Receive_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SendID", SqlDbType.Int).Value = _sendID;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-06-02
        /// 작성목적 : 수신자 등록  
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_isGroup"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _sendID, int _userID, int _deptID, string _isGroup)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_Receive_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SendID", SqlDbType.Int).Value = _sendID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@IsGroup", SqlDbType.Char, 1).Value = _isGroup;

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
        /// 작성자 : famo23
        /// 작성일자 : 2020-06-16
        /// 작성목적 : 전사 수신자 등록
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_sendID"></param>
        /// <returns></returns>
        public bool AddAll(int _companyID, int _sendID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_MDM_Receive_InsAll", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@SendID", SqlDbType.Int).Value = _sendID;

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

        #endregion 
    }
}
