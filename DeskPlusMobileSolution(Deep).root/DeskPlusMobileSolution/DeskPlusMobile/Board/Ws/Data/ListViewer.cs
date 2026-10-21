using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.Data
{
    public class ListViewer
    {
        /// <summary>
        /// 조회자 추가, 수정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_listID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptCodeID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _categoryID, int _listID, int _userID)
        {
            SqlParameter[] parms = new SqlParameter[] {
			   new SqlParameter("@CompanyID", SqlDbType.Int),
			   new SqlParameter("@CategoryID", SqlDbType.Int),
			   new SqlParameter("@ListID", SqlDbType.Int),
			   new SqlParameter("@UserID", SqlDbType.Int)
		    };

            parms[0].Value = _companyID;
            parms[1].Value = _categoryID;
            parms[2].Value = _listID;
            parms[3].Value = _userID;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DP_BOA_ListViewer_Ins", parms);
                return true;

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                throw;
            }
        }
    }
}