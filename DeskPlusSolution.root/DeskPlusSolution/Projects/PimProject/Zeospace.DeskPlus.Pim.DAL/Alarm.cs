using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.Pim.DAL
{
    public class Alarm : Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-02-05
        /// 작성목적 : 기본 알람 설정가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataTableReader GetSetting(int _companyID, int _userID)
        {
            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Alarm_Sel");
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;                

                return ExecuteDataTableReader(cmd);
            }
        }


        #endregion 

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-02-05
        /// 작성목적 : 기본 알람으로 설정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(UserAlarmInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Alarm_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@Kind", SqlDbType.Char, 1).Value = _info.Kind;
                    cmd.Parameters.Add("@Time", SqlDbType.Int).Value = _info.Time;                   

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

        #endregion 

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2021-03-09
        /// 작성목적 : 알람 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_scheduleID"></param>
        /// <param name="_displayID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _scheduleID, int _displayID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_Common_DelPim", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _scheduleID;
                    cmd.Parameters.Add("@DisplayID", SqlDbType.Int).Value = _displayID;

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
        /// 작성자 : famo23
        /// 작성일자 : 2021-03-09
        /// 작성목적 : 알람 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_scheduleID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _scheduleID, string _startDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_Common_DelPimDate", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _scheduleID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.VarChar, 8).Value = _startDate;

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
        /// 작성자 : famo23
        /// 작성일자 : 2021-03-09
        /// 작성목적 : 알람 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_scheduleID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _scheduleID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnAlarm(_companyID)))
                {
                    var cmd = new SqlCommand("SQL_DP_ALARM_Common_DelPimAll", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _scheduleID;

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


        #endregion 
    }
}
