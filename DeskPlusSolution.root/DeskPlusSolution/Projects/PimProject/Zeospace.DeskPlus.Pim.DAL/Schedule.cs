using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;
using System.Data.SqlClient;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.DAL
{
    public class Schedule : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /*
         * cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
        cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _info.ScheduleID;
        cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _info.ToDoID;
        cmd.Parameters.Add("@Type", SqlDbType.Char,1).Value = _info.Type.ToNull();
        cmd.Parameters.Add("@Title", SqlDbType.NVarChar,100).Value = _info.Title.ToNull();
        cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _info.Content.ToNull();
        cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = _info.StartTime;
        cmd.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = _info.EndTime;
        cmd.Parameters.Add("@Repeat", SqlDbType.Char,1).Value = _info.Repeat.ToNull();
        cmd.Parameters.Add("@Alert", SqlDbType.Char,1).Value = _info.Alert.ToNull();
        cmd.Parameters.Add("@AlertType", SqlDbType.Char,1).Value = _info.AlertType.ToNull();
        cmd.Parameters.Add("@AlertTime", SqlDbType.Int).Value = _info.AlertTime;
        cmd.Parameters.Add("@AlertTargetAddress", SqlDbType.NVarChar,50).Value = _info.AlertTargetAddress.ToNull();
        cmd.Parameters.Add("@IsOpen", SqlDbType.Char,1).Value = _info.IsOpen.ToNull();
        cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
        cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
        cmd.Parameters.Add("@WriteDate", SqlDbType.DateTime).Value = _info.WriteDate;
        cmd.Parameters.Add("@OpenType", SqlDbType.Int).Value = _info.OpenType;
        cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _info.ReservationID;
        cmd.Parameters.Add("@lunar", SqlDbType.Char,1).Value = _info.lunar.ToNull();

        */


        /// <summary>
        /// Gets the schedule.
        /// </summary>
        /// <param name="_reader">The _reader.</param>
        /// <returns></returns>
        private BE.ScheduleInfo GetSchedule(IDataReader _reader)
        {
            try
            {
                return new ScheduleInfo
                {
                    CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                    ScheduleID = (_reader["ScheduleID"] == DBNull.Value) ? -1 : _reader["ScheduleID"].ToString().ToNumber<int>(),
                    ToDoID = (_reader["ToDoID"] == DBNull.Value) ? -1 : _reader["ToDoID"].ToString().ToNumber<int>(),
                    Type = _reader["Type"].ToString().ToNull(),
                    Title = _reader["Title"].ToString().ToNull(),
                    Content = _reader["Content"].ToString().ToNull(),
                    StartTime = (_reader["StartTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["StartTime"],
                    EndTime = (_reader["EndTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["EndTime"],
                    Repeat = _reader["Repeat"].ToString().ToNull(),
                    Alert = _reader["Alert"].ToString().ToNull(),
                    AlertType = _reader["AlertType"].ToString().ToNull(),
                    AlertTime = (_reader["AlertTime"] == DBNull.Value) ? -1 : _reader["AlertTime"].ToString().ToNumber<int>(),
                    AlertTargetAddress = _reader["AlertTargetAddress"].ToString().ToNull(),
                    IsOpen = _reader["IsOpen"].ToString().ToNull(),
                    UserID = (_reader["UserID"] == DBNull.Value) ? -1 : _reader["UserID"].ToString().ToNumber<int>(),
                    DeptID = (_reader["DeptID"] == DBNull.Value) ? -1 : _reader["DeptID"].ToString().ToNumber<int>(),
                    WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                    OpenType = _reader["OpenType"].ToString().ToNull(),
                    ReservationID = (_reader["ReservationID"] == DBNull.Value) ? -1 : _reader["ReservationID"].ToString().ToNumber<int>(),
                    Lunar = _reader["lunar"].ToString().ToNull(),
                    UserName = _reader["Name"].ToString().ToNull(),
                    StatusID = (_reader["StatusID"] == DBNull.Value) ? -1 : _reader["StatusID"].ToString().ToNumber<int>(),
                    ReplyCnt = (_reader["ReplyCnt"] == DBNull.Value) ? -1 : _reader["ReplyCnt"].ToString().ToNumber<int>(),
                    StatusColors = _reader["Colors"].ToString().ToNull()

                };
            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSchedule", exc);
                throw exc;
            }
        }

        /// <summary>
        /// Gets the schedule list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<ScheduleInfo> GetScheduleList(IDataReader reader)
        {
            List<ScheduleInfo> list = new List<ScheduleInfo>();
            while (reader.Read())
                list.Add(GetSchedule(reader));
            return list;
        }

        #region GET() 

        #region 일간
        /// <summary>
        /// 일간나의일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataDayME(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListDay_Me", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;
                
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }


             
        }
        
        /// <summary>
        /// 일간개인일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataDayPerson(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListDay_Person", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }


        /// <summary>
        /// 일간부서일정 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataDayDept(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListDay_Dept", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;                
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = para.DeptID;
                
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }


             
        }

        /// <summary>
        /// 일간공유일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataDayShare(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListDay_Share", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                //cmd.Parameters.Add("@PimsType", SqlDbType.Int).Value = para.OpenType;
                cmd.Parameters.Add("@deptid", SqlDbType.Int).Value = para.DeptID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }

        /// <summary>
        /// 일간전체일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataDayAll(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListDay_All", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                //cmd.Parameters.Add("@PimsType", SqlDbType.Int).Value = para.OpenType;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;


                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }

        #endregion 

        #region 주간
        /// <summary>
        /// 주간나의일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataWeekME(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListWeek_Me", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }

        /// <summary>
        /// 주간개인일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataWeekPerson(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListWeek_Person", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }


        /// <summary>
        /// 주간부서일정 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataWeekDept(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListWeek_Dept", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = para.DeptID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }

        /// <summary>
        /// 주간공유일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataWeekShare(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListWeek_Share", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@deptid", SqlDbType.Int).Value = para.DeptID;
                //cmd.Parameters.Add("@PimsType", SqlDbType.Int).Value = para.OpenType;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;


                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }

        /// <summary>
        /// 주간전체일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataWeekAll(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListWeek_All", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                //cmd.Parameters.Add("@PimsType", SqlDbType.Int).Value = para.OpenType;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;


                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }

        #endregion 

        #region 월간

        /// <summary>
        /// 월간나의일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataMonthME(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListMonth_Me", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

             

        }

        /// <summary>
        /// 월간개인일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataMonthPerson(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListMonth_Person", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }


        /// <summary>
        /// 월간부서일정 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataMonthDept(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListMonth_Dept", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = para.DeptID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }

        /// <summary>
        /// 월간공유일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataMonthShare(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListMonth_Share", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                
                cmd.Parameters.Add("@deptid", SqlDbType.Int).Value = para.DeptID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;


                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }

        /// <summary>
        /// 월간전체일정
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataMonthAll(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListMonth_All", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                //cmd.Parameters.Add("@PimsType", SqlDbType.Int).Value = para.OpenType;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;


                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }

        #endregion 


        #region  기념일 가져오기
        /// <summary>
        /// 기념일 포틀릿 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleMenu(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_List_Menu", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }



        }



        /// <summary>
        /// 월간 기념일
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleMon(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_List_MON", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-20
        /// 작성목적 : 기념일 갯수 가져오기 
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleMonCount(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_List_MON_CNT", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = para.StartTime;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }


        #endregion 


        /// <summary>
        /// 가져오기
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<BE.ScheduleInfo> GetSheduleData(BE.ScheduleInfo  para)
        {
             
                using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Schedule_Select");

                    if (!para.Type.Equals(""))
                        query += " AND TYPE IN ( " + para.Type  +" )";
                    if (!para.OpenType.Equals("0"))
                        query += " AND OpenType IN ( " + para.OpenType + " )";

                    if (para.WhereQuery.Length > 0)
                        query += " AND " + para.WhereQuery;


                    var cmd = new SqlCommand(query, cn);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                    

                    //cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetScheduleList(reader);
                    }
                }
        }

        /// <summary>
        /// 갯수 가져오기
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public int GetSheduleDataCnt(BE.ScheduleInfo para)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Schedule_SelectCnt");

                if (!para.Type.Equals(""))
                    query += " AND TYPE IN ( " + para.Type + " )";
                if (!para.OpenType.Equals("0"))
                    query += " AND OpenType IN ( " + para.OpenType + " )";

                if (para.WhereQuery.Length > 0)
                    query += " AND " + para.WhereQuery;


                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;

        }

        /// <summary>
        /// 보기
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<BE.ScheduleInfo> GetSheduleDataID(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Schedule_Select");

                query += " AND   ScheduleID = "+ para.ScheduleID.ToString();
                
                var cmd = new SqlCommand(query, cn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;


                //cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;

                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetScheduleList(reader);
                }
            }
        }

        /// <summary>
        /// 보기
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleSingleID(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Schedule_SelID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@ScheduleID ", SqlDbType.Int).Value = para.ScheduleID;
                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 스케줄 건 하나 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_scheduleID">The _schedule ID.</param>
        /// <returns></returns>
        public DataTableReader GetShedule(int _companyID, int _scheduleID)
        {

            using (var cn = new SqlConnection(ConnSolution(_companyID)))
            {

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Schedule_SelByScheduleID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@ScheduleID ", SqlDbType.Int).Value = _scheduleID;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        

        /// <summary>
        /// 뷰에서 가져오기
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleDataViewID(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                 
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_ScheduleVWID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@DisplayID", SqlDbType.Int).Value = para.DisplayID;                
                cn.Open();
                return  ExecuteDataTableReader(cmd);                 
            }
        }


        /// <summary>
        ///  스케줄 가져오기
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleID(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Schedule_SelectID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = para.ScheduleID;
                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        
        /// <summary>
        /// 스케쥴 프로시져
        /// </summary>
        public DataTableReader GetScheduleProc(BE.ScheduleInfo para)
        {
            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar, 100).Value = para.Type;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = para.SearchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = para.SearchValue;

                cmd.Parameters.Add("@StartTime", SqlDbType.NVarChar, 20).Value = para.stringStartTime;
                cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar, 20).Value = para.stringENDTime;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = para.Pagesize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value =para.PageSizePageNo;

                cmd.Parameters.Add("@Order", SqlDbType.VarChar,100).Value = para.Order;
                cmd.Parameters.Add("@COUNTRY", SqlDbType.VarChar, 10).Value = para.Country;
                cmd.Parameters.Add("@OpenType", SqlDbType.VarChar, 20).Value = para.OpenType; 
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        /// <summary>
        /// 전체스케줄
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>        
        public DataTableReader GetAllScheduleProc(BE.ScheduleInfo para)
        {
            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_ScheduleAll_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;


                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime, 20).Value = para.stringStartTime;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = para.stringENDTime;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }


        public int GetScheduleAllProcCnt(BE.ScheduleInfo para)
        {
            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_ScheduleAll_ListCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar, 100).Value = para.Type;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = para.SearchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = para.SearchValue;

                cmd.Parameters.Add("@StartTime", SqlDbType.NVarChar, 20).Value = para.stringStartTime;
                cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar, 20).Value = para.stringENDTime;
                cmd.Parameters.Add("@OpenType", SqlDbType.VarChar, 20).Value = para.OpenType;
                cmd.Parameters.Add("@UserIDList", SqlDbType.VarChar, 500).Value = para.UserIDList;


                cn.Open();
                return int.Parse(ExecuteScalar(cmd).ToString());


            }

        }


        public int GetScheduleProcCnt(BE.ScheduleInfo para)
        {
            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Schedule_ListCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar, 100).Value = para.Type;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = para.SearchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = para.SearchValue;

                cmd.Parameters.Add("@StartTime", SqlDbType.NVarChar, 20).Value = para.stringStartTime;
                cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar, 20).Value = para.stringENDTime;
                cmd.Parameters.Add("@OpenType", SqlDbType.VarChar, 20).Value = para.OpenType; 

                

                cn.Open();
                return  int.Parse(ExecuteScalar(cmd).ToString());

                
            }

        }

        #region 기념일

        public DataTableReader GetAnniversaryProc(BE.ScheduleInfo para)
        {
            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Anniversary_List", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar, 100).Value = para.Type;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = para.SearchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = para.SearchValue;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartTime", SqlDbType.NVarChar, 20).Value = para.stringStartTime;
                cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar, 20).Value = para.stringENDTime;
                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = para.Pagesize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = para.PageSizePageNo;

                cmd.Parameters.Add("@Order", SqlDbType.VarChar, 100).Value = para.Order;
                cmd.Parameters.Add("@COUNTRY", SqlDbType.VarChar, 10).Value = para.Country;
                cmd.Parameters.Add("@OpenType", SqlDbType.VarChar, 20).Value = para.OpenType;
                cn.Open();

                return ExecuteDataTableReader(cmd);
            }

        }

        public int GetAnniversaryProcCnt(BE.ScheduleInfo para)
        {
            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_Anniversary_ListCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@Type", SqlDbType.VarChar, 100).Value = para.Type;
                cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = para.SearchKey;
                cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = para.SearchValue;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.UserID;
                cmd.Parameters.Add("@StartTime", SqlDbType.NVarChar, 20).Value = para.stringStartTime;
                cmd.Parameters.Add("@EndTime", SqlDbType.NVarChar, 20).Value = para.stringENDTime;
                cmd.Parameters.Add("@OpenType", SqlDbType.VarChar, 20).Value = para.OpenType;



                cn.Open();
                return int.Parse(ExecuteScalar(cmd).ToString());


            }

        }
        #endregion 

        /// <summary>
        /// 공휴일 가져오기
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<BE.ScheduleInfo> GetSheduleHolyDay(BE.ScheduleInfo para)
        {

            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Holy_Select");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                //cmd.Parameters.Add("@Type", SqlDbType.VarChar,100).Value = para.Type;
                

                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = para.StartTime;
                cn.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetScheduleList(reader);
                }
            }
        }

        public DataTableReader  GetSheduleHolyDayTable(BE.ScheduleInfo para)
        {
            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Holy_Select");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = para.StartTime;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        /// <summary>
        /// 공휴일 카운트
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public int GetSheduleHolyDayCnt(BE.ScheduleInfo para)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Holy_SelectCnt");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = para.StartTime;
                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;

        }


        #region 전체검색

        public DataTableReader GetAllSearch(BE.ScheduleInfo info)
        {
            using (var cn = new SqlConnection(ConnSolution(info.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TotalListSearch", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = info.Pagesize;
                cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = info.PageSizePageNo;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = info.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = info.CompanyID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = info.StartTime;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = info.EndTime;
                cmd.Parameters.Add("@TITLE", SqlDbType.NVarChar, 200).Value = info.Title.ToXSS();
                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar,200).Value = info.UserName.ToXSS();
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = info.DeptID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        public int GetAllSearchCnt(BE.ScheduleInfo info)
        {
            int rtCnt = 0;
            using (var cn = new SqlConnection(ConnSolution(info.CompanyID)))
            {
                

                var cmd = new SqlCommand("DP_PIM_TotalListSearchCnt", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = info.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = info.CompanyID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = info.StartTime;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = info.EndTime;
                cmd.Parameters.Add("@TITLE", SqlDbType.NVarChar, 200).Value = info.Title.ToXSS();
                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 200).Value = info.UserName.ToXSS();
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = info.DeptID;
                cn.Open();

                rtCnt = int.Parse(ExecuteScalar(cmd).ToString());

            }

            return rtCnt;

        }

        public DataTableReader GetSearchExcel(BE.ScheduleInfo info)
        {
            using (var cn = new SqlConnection(ConnSolution(info.CompanyID)))
            {
                var cmd = new SqlCommand("DP_PIM_TotalSearchExcel", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = info.UserID;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = info.CompanyID;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = info.StartTime;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = info.EndTime;
                cmd.Parameters.Add("@TITLE", SqlDbType.NVarChar, 200).Value = info.Title.ToXSS();
                cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 200).Value = info.UserName.ToXSS();
                cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = info.DeptID;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion 

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-07-25
        /// 작성목적 : 년 바인딩
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public DataTableReader GetYear(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PIM_Schedule_SelYear");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;                    
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }
        #endregion

        #region ADD()
        /// <summary>
        /// Adds the list.
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public int Add(ScheduleInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Schedule_Ins", cn);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _listinfo.ToDoID;
                    cmd.Parameters.Add("@Type", SqlDbType.Char,1).Value = _listinfo.Type;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = _listinfo.Title.ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _listinfo.Content.ToEditorXSS();

                    cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = _listinfo.StartTime;
                    cmd.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = _listinfo.EndTime;
                    cmd.Parameters.Add("@Repeat", SqlDbType.Char, 1).Value = _listinfo.Repeat;
                    cmd.Parameters.Add("@Alert", SqlDbType.Char, 1).Value = _listinfo.Alert;
                    cmd.Parameters.Add("@AlertType", SqlDbType.Char, 1).Value = _listinfo.AlertType;


                    cmd.Parameters.Add("@AlertTime", SqlDbType.Int).Value = _listinfo.AlertTime;
                    cmd.Parameters.Add("@AlertTargetAddress", SqlDbType.NVarChar, 200).Value = _listinfo.AlertTargetAddress.ToXSS();
                    cmd.Parameters.Add("@IsOpen", SqlDbType.Char, 1).Value = _listinfo.IsOpen;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _listinfo.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _listinfo.DeptID;

                    cmd.Parameters.Add("@OpenType", SqlDbType.TinyInt).Value = _listinfo.OpenType.ToNumber<int>();
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _listinfo.ReservationID;
                    cmd.Parameters.Add("@lunar", SqlDbType.Char,1).Value = _listinfo.Lunar;

                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = _listinfo.StatusID;
                    
                    
                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return -1;
            }

        }


      



        #endregion 
 
        #region MOD() 

        /// <summary>
        /// Mods the specified _listinfo.
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <returns></returns>
        public bool Mod(ScheduleInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Schedule_UPD", cn);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _listinfo.ScheduleID;
                    cmd.Parameters.Add("@ToDoID", SqlDbType.Int).Value = _listinfo.ToDoID;
                    cmd.Parameters.Add("@Type", SqlDbType.Char, 1).Value = _listinfo.Type;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = _listinfo.Title.ToXSS();
                    cmd.Parameters.Add("@Content", SqlDbType.NText).Value = _listinfo.Content.ToEditorXSS();

                    cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = _listinfo.StartTime;
                    cmd.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = _listinfo.EndTime;
                    cmd.Parameters.Add("@Repeat", SqlDbType.Char, 1).Value = _listinfo.Repeat;
                    cmd.Parameters.Add("@Alert", SqlDbType.Char, 1).Value = _listinfo.Alert;
                    cmd.Parameters.Add("@AlertType", SqlDbType.Char, 1).Value = _listinfo.AlertType;


                    cmd.Parameters.Add("@AlertTime", SqlDbType.Int).Value = _listinfo.AlertTime;
                    cmd.Parameters.Add("@AlertTargetAddress", SqlDbType.NVarChar, 200).Value = _listinfo.AlertTargetAddress.ToXSS();
                    cmd.Parameters.Add("@IsOpen", SqlDbType.Char, 1).Value = _listinfo.IsOpen;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _listinfo.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _listinfo.DeptID;

                    cmd.Parameters.Add("@OpenType", SqlDbType.TinyInt).Value = _listinfo.OpenType;
                    cmd.Parameters.Add("@ReservationID", SqlDbType.Int).Value = _listinfo.ReservationID;
                    cmd.Parameters.Add("@lunar", SqlDbType.Char, 1).Value = _listinfo.Lunar;
                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = _listinfo.StatusID;

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }

        }

        public bool ModSimple(ScheduleInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Schedule_SimpleUPD", cn);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _listinfo.ScheduleID;
                    cmd.Parameters.Add("@OpenType", SqlDbType.Int).Value = _listinfo.OpenType;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar,200).Value = _listinfo.Title.ToXSS();
                    cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = _listinfo.StartTime;
                    cmd.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = _listinfo.EndTime;
                    cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = _listinfo.StatusID;

                    cmd.Parameters.Add("@Alert", SqlDbType.Char, 1).Value = _listinfo.Alert;
                    cmd.Parameters.Add("@AlertType", SqlDbType.Char, 1).Value = _listinfo.AlertType;
                    cmd.Parameters.Add("@AlertTime", SqlDbType.Int).Value = _listinfo.AlertTime;
                    cmd.Parameters.Add("@AlertTargetAddress", SqlDbType.NVarChar, 200).Value = _listinfo.AlertTargetAddress.ToXSS();

                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
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

        public bool Del(ScheduleInfo _listinfo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_listinfo.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Schedule_DEL", cn);


                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _listinfo.CompanyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _listinfo.ScheduleID;
                     
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return false;
            }

        }




        #endregion 

        #region 전체검색

        public DataTableReader GetTotalSearchList(int _PageSize, int _PageSizePageNo, int _UserID, int _CompanyID, DateTime _StartDate, DateTime _EndDATE, string _title, int _deptid)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_TOTALSEARCH_0", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _PageSizePageNo;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _EndDATE;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = _title.ToXSS();
                    cmd.Parameters.Add("@DeptID", SqlDbType.NVarChar, 200).Value = _deptid;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTotalSearchList", exc);
                return null;
            }
        }

        public DataTableReader GetTotalSearchListUserList(int _PageSize, int _PageSizePageNo, int _UserID, int _CompanyID, DateTime _StartDate, DateTime _EndDATE, string _UserList)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_TOTALSEARCH_USERLIST", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _PageSizePageNo;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _EndDATE;
                    cmd.Parameters.Add("@USERLIST", SqlDbType.VarChar,200).Value = _UserList;
                    
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTotalSearchListUserList", exc);
                return null;
            }
        }

        public int GetTotalSearchListCnt( int _UserID, int _CompanyID, DateTime _StartDate, DateTime _EndDATE,string _title,int _deptid)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_TOTALSEARCH_CNT_0", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _EndDATE;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = _title.ToXSS();
                    cmd.Parameters.Add("@DeptID", SqlDbType.NVarChar, 200).Value = _deptid;
                    cn.Open();
                    return int.Parse(ExecuteScalar(cmd).ToString());
                    
                }

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTotalSearchList", exc);
                return 0;
            }
        }
        
        public int GetTotalSearchListUserListCnt(int _UserID, int _CompanyID, DateTime _StartDate, DateTime _EndDATE, string _UserList)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_TOTALSEARCH_USERLIST_CNT", cn);
                    cmd.CommandType = CommandType.StoredProcedure; 
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _EndDATE;
                    cmd.Parameters.Add("@USERLIST", SqlDbType.VarChar, 200).Value = _UserList;
                    cn.Open();
                    return int.Parse(ExecuteScalar(cmd).ToString());

                }

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTotalSearchList", exc);
                return 0;
            }
        }

        public DataTableReader GetScheduleStatic(int _CompanyID, int _UserID, int _DeptID, DateTime _StartTime)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_Schedule_PersonStatic", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                      
                    
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _StartTime;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _DeptID;
                    
                    
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetScheduleStatic", exc);
                return null;
            }

        }
        //////////////////
        public int GetTotalSearchListCntCurrent(int _UserID, int _CompanyID, DateTime _StartDate, DateTime _EndDATE, string _title, int _CurrentUserid, int _DeptID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_TOTALSEARCH_CURRENT_CNT", cn);
                    cmd.CommandType = CommandType.StoredProcedure; 
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _EndDATE;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = _title.ToXSS();
                    cmd.Parameters.Add("@CurrentUser", SqlDbType.Int).Value = _CurrentUserid;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _DeptID;
                    cn.Open();
                    return int.Parse(ExecuteScalar(cmd).ToString());

                }

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTotalSearchList", exc);
                return 0;
            }
        }

        public DataTableReader GetTotalSearchListCurrent(int _PageSize, int _PageSizePageNo, int _UserID, int _CompanyID, DateTime _StartDate, DateTime _EndDATE, string _title,int _CurrentUserid,int _DeptID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_TOTALSEARCH_CURRENT", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _PageSizePageNo;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _EndDATE;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = _title.ToXSS();
                    cmd.Parameters.Add("@CurrentUser", SqlDbType.Int).Value = _CurrentUserid;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _DeptID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTotalSearchList", exc);
                return null;
            }
        }
        #endregion

        #region 유저일정 검색
        public DataTableReader GetUserListSearch(int _PageSize, int _PageSizePageNo, int _UserID, int _CompanyID, DateTime _StartDate, DateTime _EndDATE,string _OpenType, string _UserList,string _Title)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_USERSchedule", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _PageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _PageSizePageNo;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _StartDate.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _EndDATE.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@OPENTYPE", SqlDbType.VarChar, 100).Value = _OpenType;
                    cmd.Parameters.Add("@USERLIST", SqlDbType.VarChar, 100).Value = _UserList;
                    cmd.Parameters.Add("@TITLE", SqlDbType.VarChar, 200).Value = _Title.ToXSS();

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetUserListSearch", exc);
                return null;
            }
        }

        public int GetUserListSearchCnt(int _UserID, int _CompanyID, DateTime _StartDate, DateTime _EndDATE, string _OpenType, string _UserList, string _Title)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_USERSchedule_Cnt", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _EndDATE;
                    cmd.Parameters.Add("@OPENTYPE", SqlDbType.VarChar, 100).Value = _OpenType;
                    cmd.Parameters.Add("@USERLIST", SqlDbType.VarChar, 100).Value = _UserList;
                    cmd.Parameters.Add("@TITLE", SqlDbType.VarChar, 200).Value = _Title.ToXSS();
                    cn.Open();
                    return int.Parse(ExecuteScalar(cmd).ToString());

                }

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTotalSearchList", exc);
                return 0;
            }
        }
        
        #endregion

        #region 전체 사용자 일정
        public DataTableReader GetAllUserListSearch( int _UserID, int _CompanyID, DateTime _StartDate, DateTime _EndDATE, string _OpenType, string _UserList, string _Title,string _name,int _DeptID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PIM_AllUSERSchedule", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _UserID;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _CompanyID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _StartDate.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _EndDATE.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@OPENTYPE", SqlDbType.VarChar, 100).Value = _OpenType;
                    cmd.Parameters.Add("@USERLIST", SqlDbType.VarChar, 1000).Value = _UserList;
                    cmd.Parameters.Add("@TITLE", SqlDbType.VarChar, 200).Value = _Title.ToXSS();
                    cmd.Parameters.Add("@NAME", SqlDbType.NVarChar, 200).Value = _name;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _DeptID;
                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetUserListSearch", exc);
                return null;
            }
        }

        #endregion
    }
}
