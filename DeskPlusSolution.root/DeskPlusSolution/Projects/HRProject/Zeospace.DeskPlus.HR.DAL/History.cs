using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.HR.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Zeospace.DeskPlus.HR.DAL
{
    public class History : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET

        /// <summary>
        /// 히스토리 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_attID"></param>
        /// <returns></returns>
        public DataTableReader GetHistoryList(int _companyID, int _attID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_HR_Attendance_History_SelPageList", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = ConfigurationManager.AppSettings["SQLCommandTimeout"].ToNumber<int>();

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@AttID", SqlDbType.Int).Value = _attID;                       

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GET()
    }
}
