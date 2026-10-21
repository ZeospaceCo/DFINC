using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Account.Data
{
    public class Dept
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2011-07-12
        /// 작성목적 : 사용자 정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetDeptUserTree(int _companyID)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int)
            };

            parms[0].Value = _companyID;

            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_PROC_ACC_DEPT");
            return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "DeptUsers", parms);
        }

        /// <summary>
        /// 전사 부서정보 가져오기 2015.10.15
        /// </summary>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public DataSet GetDept(int _deptID)
        {
            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_DEPT_ALL");
            proc = string.Format(proc, _deptID);
            DataSet ds = new DataSet();
            ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetDept", null);

            return ds;
        }

        /// <summary>
        /// 부서정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_activeDept"></param>
        /// <returns></returns>
        public DataSet GetDept(int _companyID, bool _activeDept)
        {
            string activeDept = (_activeDept) ? "'1'" : "'0','1'";

            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_DeptCompanyID");
            proc = string.Format(proc, _companyID, activeDept);
            DataSet ds = new DataSet();
            ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "dsDept", null);

            return ds;
        }

        /// <summary>
        /// 전사 부서정보 가져오기 2015.10.15
        /// </summary>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public DataSet GetDept(int _companyID, int _deptID)
        {
            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_DEPT");
            proc = string.Format(proc, _companyID, _deptID);
            DataSet ds = new DataSet();
            ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetDept", null);

            return ds;
        }

    }
}