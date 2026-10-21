using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.Data
{
    public class AttachFiles
    {
        /// <summary>
        /// Gets the short reply.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <returns></returns>
        public DataSet GetAttachFiles(int _companyID, int _categoryID, int _listID)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@CategoryID", SqlDbType.Int),
											   new SqlParameter("@ListID", SqlDbType.Int)
										   };
            parms[0].Value = _companyID;
            parms[1].Value = _categoryID;
            parms[2].Value = _listID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_Attach_Sel", "GetAttachFiles", parms);
        }

        /// <summary>
        /// Gets the short reply.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// /// <param name="_attachType">The _attachType ID.</param>
        /// <returns></returns>
        public DataSet GetDSFiles(int _companyID,int _categoryID, int _listID, string _attachType)
        {
            SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
                                               new SqlParameter("@CategoryID", SqlDbType.Int),
											   new SqlParameter("@ListID", SqlDbType.Int),
                                               new SqlParameter("@AttachType", SqlDbType.NVarChar, 50),
										   };
            parms[0].Value = _companyID;
            parms[1].Value = _categoryID;
            parms[2].Value = _listID;
            parms[3].Value = _attachType;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_DS_Sel", "GetDSFiles", parms);
        }
    }
}