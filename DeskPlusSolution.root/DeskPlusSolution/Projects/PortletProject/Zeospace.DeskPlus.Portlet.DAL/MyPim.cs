using System;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.DAL
{
    public class MyPim : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public MyPim()
        {
        }

        #region Get

        public DataTableReader GetSchedule(int _CompanyID, int _UserID, DateTime _start, DateTime _End)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Schedule_Sel");
                    ///query = string.Format(query, _listCount, _searchVal);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@START", SqlDbType.DateTime).Value = _start;
                    cmd.Parameters.Add("@END", SqlDbType.DateTime).Value = _End;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        public DataTableReader GetTodo(int _CompanyID, int _UserID, DateTime _start)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Todo_Sel");
                    ///query = string.Format(query, _listCount, _searchVal);

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _start;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 날짜별(+3개월, -3개월) 할일, 일정 리스트 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_startYearMonth">The _start year month.</param>
        /// <param name="_endYearMonth">The _end year month.</param>
        /// <returns></returns>
        public DataTableReader GetTodoShcList(int _companyID, int _userID, string _startYearMonth, string _endYearMonth)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_PT_ToDo_SelUserID", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                cmd.Parameters.Add("@StartYearMonth", SqlDbType.Char, 6).Value = _startYearMonth;
                cmd.Parameters.Add("@EndYearMonth", SqlDbType.Char, 6).Value = _endYearMonth;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        /// <summary>
        /// 작성자  : famo23
        /// 작성일자 : 2017-02-10
        /// 작성목적 : 해당 날짜의 일정가져오기
        /// </summary>
        /// <param name="_CompanyID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_DeptID"></param>
        /// <param name="_date"></param>
        /// <returns></returns>
        public DataTableReader GetScheduleList(int _CompanyID, int _UserID, int _DeptID, string _date)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PT_TodoPopup_PimScheduleList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@TmpDate", SqlDbType.DateTime).Value = _date;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _DeptID;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// 작성자  : famo23
        /// 작성일자 : 2017-02-13
        /// 작성목적 : 해당 날짜의 할일가져오기
        /// </summary>
        /// <param name="_CompanyID"></param>
        /// <param name="_UserID"></param>
        /// <param name="_date"></param>
        /// <returns></returns>
        public DataTableReader GetTodoList(int _CompanyID, int _UserID, string _date)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_Todo_SelDate");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@TmpDate", SqlDbType.DateTime).Value = _date;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion Get
    }
}
