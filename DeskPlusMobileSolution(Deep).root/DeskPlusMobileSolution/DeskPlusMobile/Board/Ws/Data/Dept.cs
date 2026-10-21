using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.Data
{
    public class Dept
    {
        /// <summary>
        /// Gets the dept user tree.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetDeptUserTree(int _companyID)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int)};
            parms[0].Value = _companyID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_ACC_Dept_SelDeptUser", "DeptUsers", parms);
        }



        /// <summary>
        /// 조직도 위,아래 재귀값 가져오기 (1: UP, 2:DOWN)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_upDown">The _up down.</param>
        /// <returns></returns>
        public DataSet GetDeptTree(int _companyID, int _userID, int _upDown)
        {
            SqlParameter[] parms = new SqlParameter[] {
                    new SqlParameter("@CompanyID", SqlDbType.Int)
                    ,new SqlParameter("@UserID", SqlDbType.Int)
                    ,new SqlParameter("@UpDown", SqlDbType.Int)
            };

            parms[0].Value = _companyID;
            parms[1].Value = _userID;
            parms[2].Value = _upDown;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_ACC_Dept_SelByDeptUpDown", "GetDeptTreeUPDOWN", parms);
        }

        /// <summary>
        /// 조직도를 위한 메소드
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetDeptTree(int _companyID)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int)};


            parms[0].Value = _companyID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_ACC_Dept_SelDeptTree", "Depts", parms);
        }
    }
}