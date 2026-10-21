using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Account.BE;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Collections;

namespace Zeospace.DeskPlusMobile.Web.Account.Data
{
    public class UserDept : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        /// <summary>
        /// Gets the user dept.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public ArrayList GetUserDept(int _companyID, int _userID)
        {
            SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                              ,new SqlParameter("@UserID", SqlDbType.Int)  
            };

            parms[0].Value = _companyID;
            parms[1].Value = _userID;

            UserDeptInfo userDeptInfo = null;
            ArrayList alUserDept = null;
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_PROC_ACCOUNT_USERDEPTS");
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms))
            {
                alUserDept = new ArrayList();

                while (sdr.Read())
                {

                    userDeptInfo = new UserDeptInfo();
                    userDeptInfo.CompanyID = _companyID;
                    userDeptInfo.DeptID = (sdr.GetValue(0) == DBNull.Value) ? -1 : sdr.GetInt32(0);
                    userDeptInfo.DeptName = this.GetLangName(sdr.GetValue(1).ToString(), sdr.GetValue(2).ToString(), sdr.GetValue(3).ToString(), sdr.GetValue(4).ToString(), sdr.GetValue(5).ToString(),
                                         sdr.GetValue(6).ToString(), sdr.GetValue(7).ToString(), PreLang);
                    userDeptInfo.CodeTitleID = (sdr.GetValue(8) == DBNull.Value) ? -1 : sdr.GetInt32(8);
                    userDeptInfo.TitleName = this.GetLangName(sdr.GetValue(9).ToString(), sdr.GetValue(10).ToString(), sdr.GetValue(11).ToString(), sdr.GetValue(12).ToString(), sdr.GetValue(13).ToString(),
                                         sdr.GetValue(14).ToString(), sdr.GetValue(15).ToString(), PreLang);
                    userDeptInfo.CodeDutyID = (sdr.GetValue(16) == DBNull.Value) ? -1 : sdr.GetInt32(16);
                    userDeptInfo.DutyName = this.GetLangName(sdr.GetValue(17).ToString(), sdr.GetValue(18).ToString(), sdr.GetValue(19).ToString(), sdr.GetValue(20).ToString(), sdr.GetValue(21).ToString(),
                                         sdr.GetValue(22).ToString(), sdr.GetValue(23).ToString(), PreLang);
                    userDeptInfo.BaseDept = (sdr.GetValue(24) == DBNull.Value) ? "1" : sdr.GetString(24);
                    userDeptInfo.UserDeptOrder = (sdr.GetValue(25) == DBNull.Value) ? 1 : sdr.GetInt32(25);
                    userDeptInfo.MailID = (sdr.GetValue(26) == DBNull.Value) ? "" : sdr.GetString(26);
                    userDeptInfo.MailDomain = (sdr.GetValue(27) == DBNull.Value) ? "" : sdr.GetString(27);
                    userDeptInfo.MailPassword = (sdr.GetValue(28) == DBNull.Value) ? "" : sdr.GetString(28);

                    alUserDept.Add(userDeptInfo);
                }
            }

            return alUserDept;
        }

        /// <summary>
        /// 부서별 사용자 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public DataSet GetUserListByDept(int _companyID, string _deptID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_USERDEPT");
                proc = string.Format(proc, _companyID, _deptID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserListByDept", null);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetUserListByDept: " + errMsg);
                throw;
            }
        }
    }
}