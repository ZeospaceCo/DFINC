using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Report.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.DAL
{
    public class Pim : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {

        /// <summary>
        /// 심재운
        /// 목적 :  날짜별 나의 일정 및 할일 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_endDate">The _end date.</param>
        /// <param name="_openType">Type of the _open.</param>
        /// <param name="_userList">The _user list.</param>
        /// <returns></returns>
        public DataTableReader GetPimMySche(int _companyID, string _userID, int _deptID, DateTime _startDate, DateTime _endDate,
            string _openType, string _userList)
        {
            /*
             *exec DP_REPORT_PIM_SelMySche
            @UserID=58,
            @CompanyID=1,
            @StartDate='2012-12-21 00:00:00:000',
            @EndDate='2012-12-21 00:00:00:000',
            @OPENTYPE='1,2',
            @USERLIST='58,',
            @DeptID=38
            */

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                
                var cmd = new SqlCommand("DP_REPORT_PIM_SelMySche", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 100).Value = _userID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _startDate;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _endDate;
                cmd.Parameters.Add("@OPENTYPE", SqlDbType.VarChar, 100).Value = _openType;
                cmd.Parameters.Add("@USERLIST", SqlDbType.VarChar, 1000).Value = _userList;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
    }
}
