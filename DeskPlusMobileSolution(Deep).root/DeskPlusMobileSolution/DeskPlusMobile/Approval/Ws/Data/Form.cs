using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.Data
{
    public class Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <returns></returns>
        public DataSet GetForm(int _companyID, int _formID)
        {
            SqlParameter[] parms = new SqlParameter[] {
							new SqlParameter("@CompanyID", SqlDbType.Int),
                            new SqlParameter("@FormID", SqlDbType.Int)
            };
            parms[0].Value = _companyID;
            parms[1].Value = _formID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DP_APP_Form_Sel", "GetForm", parms);
        }


    }
}