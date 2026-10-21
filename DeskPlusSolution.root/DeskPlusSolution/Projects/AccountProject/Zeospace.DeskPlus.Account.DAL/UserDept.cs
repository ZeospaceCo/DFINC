using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserDept : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
         *
            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
            cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
            cmd.Parameters.Add("@CodeTitleID", SqlDbType.Int).Value = _info.CodeTitleID;
            cmd.Parameters.Add("@CodeDutyID", SqlDbType.Int).Value = _info.CodeDutyID;
            cmd.Parameters.Add("@BaseDept", SqlDbType.Char,1).Value = _info.BaseDept.ToNull().ToXSS();
            cmd.Parameters.Add("@UserDeptOrder", SqlDbType.Int).Value = _info.UserDeptOrder;
            cmd.Parameters.Add("@MailID", SqlDbType.VarChar,20).Value = _info.MailID.ToNull().ToXSS();
            cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar,30).Value = _info.MailDomain.ToNull().ToXSS();
            cmd.Parameters.Add("@MailPassword", SqlDbType.NVarChar,30).Value = _info.MailPassword.ToNull().ToXSS();
            cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
            cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _info.WriteDate;
            cmd.Parameters.Add("@ModifyUserID", SqlDbType.Int).Value = _info.ModifyUserID;
            cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = _info.ModifyDate;
         */

        #region DataReader

        private UserDeptInfo GetUserDept(IDataReader _reader)
        {
            return new UserDeptInfo(
            (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
            (_reader["UserID"] == DBNull.Value) ? -1 : (int)_reader["UserID"],
            (_reader["DeptID"] == DBNull.Value) ? -1 : (int)_reader["DeptID"],
            (_reader["CodeTitleID"] == DBNull.Value) ? -1 : (int)_reader["CodeTitleID"],
            (_reader["CodeDutyID"] == DBNull.Value) ? -1 : (int)_reader["CodeDutyID"],
            _reader["BaseDept"].ToString(),
            (_reader["UserDeptOrder"] == DBNull.Value) ? -1 : (int)_reader["UserDeptOrder"],
            _reader["MailID"].ToString(),
            _reader["MailDomain"].ToString(),
            _reader["MailPassword"].ToString(),
            (_reader["WriteUserID"] == DBNull.Value) ? -1 : (int)_reader["WriteUserID"],
            (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
            (_reader["ModifyUserID"] == DBNull.Value) ? -1 : (int)_reader["ModifyUserID"],
            (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"]
            );
        }

        protected List<UserDeptInfo> GetUserDeptList(IDataReader reader)
        {
            List<UserDeptInfo> list = new List<UserDeptInfo>();
            while (reader.Read())
                list.Add(GetUserDept(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사용자의 부서 추가하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool AddUserDept(BE.UserDeptInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserDept_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@CodeTitleID", SqlDbType.Int).Value = _info.CodeTitleID;
                    cmd.Parameters.Add("@CodeDutyID", SqlDbType.Int).Value = _info.CodeDutyID;
                    cmd.Parameters.Add("@BaseDept", SqlDbType.Char, 1).Value = _info.BaseDept;
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
        /// 용도 : 겸직 부서 설정하기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool ModBaseDept(BE.UserDeptInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserDept_UpdBaseDept", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@CodeTitleID", SqlDbType.Int).Value = _info.CodeTitleID;
                    cmd.Parameters.Add("@CodeDutyID", SqlDbType.Int).Value = _info.CodeDutyID;

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
        /// 용도 : 사용자 부서 삭제하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public bool DelUserDept(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserDept_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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
        /// 용도 : 순서변경
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_list">The _list.</param>
        /// <returns></returns>
        public bool ModOrder(int _companyID, string _list)
        {
            try
            {
                if (_list.Length > 0)
                {
                    string[] listStr = _list.Split(',');
                    int userID, deptID;

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < listStr.Length; i++)
                        {
                            userID = listStr[i].ToString().Split('|')[0].ToString().ToNumber<int>();
                            deptID = listStr[i].ToString().Split('|')[1].ToString().ToNumber<int>();

                            var cmd = new SqlCommand("DP_ACC_UserDept_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                            cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = deptID;
                            cmd.Parameters.Add("@UserDeptOrder", SqlDbType.Int).Value = (listStr.Length - i);

                            ExecuteNonQuery(cmd);
                        }
                    }
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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 부서와 사용자 가져오기 2015.10.14
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public DataSet GetDeptUserTreeByOwnerID(int _companyID, int _deptID, bool _active)
        {
            string active = (_active) ? "'1'" : "'0','1'";
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserDept_SelByDeptID");
                query = string.Format(query, active);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;

                cn.Open();

                return ExecuteDataSet(cmd, "GetDeptUserTreeByOwnerID");
            }
        }

        /// <summary>
        /// 조직도에서 부서 및 사용자 이름 검색 (통합관리자)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public DataSet GetDeptUserByName(string _name, bool _active)
        {
            string active = (_active) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution()))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserDept_SelSearchNameTot");
                query = string.Format(query, active);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = _name;

                cn.Open();

                return ExecuteDataSet(cmd, "GetDeptUserByName");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 조직도에서 부서 및 사용자 이름 검색 2015.10.14
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public DataSet GetDeptUserByName(int _companyID, string _name, bool _active)
        {
            string active = (_active) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserDept_SelSearchName");
                query = string.Format(query, active);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = _name;

                cn.Open();

                return ExecuteDataSet(cmd, "GetDeptUserByName");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 사용자 부서 가져오기(트리용) - 2015.10.14
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public DataSet GetDeptUserTree(int _companyID, int _deptID, bool _active)
        {
            string active = (_active) ? "1" : "0,1";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_ACC_UserDept_SelGetTree", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                if (_deptID == 0)
                {
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = System.DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                }

                cmd.Parameters.Add("@Active", SqlDbType.VarChar, 3).Value = active;

                cn.Open();

                return ExecuteDataSet(cmd, "DPGetDeptUserTree");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 해당 부서의 사용자 카운트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_active">if set to <c>true</c> [_active].</param>
        /// <returns></returns>
        public int GetCountByDeptID(int _companyID, int _deptID, bool _active)
        {
            string active = (_active) ? "'1'" : "'0','1'";

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserDept_SelCntByDeptID");
                query = string.Format(query, active);

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cn.Open();

                return ExecuteScalar(cmd).ToString().ToNumber<int>();
            }
        }

        #endregion 가져오기
    }
}