using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.DAL
{
    public class Repeat : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        public Repeat()
        {
        }

        private BE.RepeatScheduleInfo GetRepeat(IDataReader _reader)
        {
            return new BE.RepeatScheduleInfo(
                (_reader["CompanyID"] == DBNull.Value) ? -1 : (int)_reader["CompanyID"],
                (_reader["ScheduleID"] == DBNull.Value) ? -1 : (int)_reader["ScheduleID"],
                _reader["RepeatType"].ToString(),
                _reader["SelectType"].ToString(),
                (_reader["IntervalCnt"] == DBNull.Value) ? -1 : (int)_reader["IntervalCnt"],
                _reader["Option1"].ToString(),
                _reader["Option2"].ToString(),
                _reader["Option3"].ToString(),
                _reader["Option4"].ToString(),
                _reader["Option5"].ToString(),
                _reader["Option6"].ToString(),
                _reader["Option7"].ToString(),
                (_reader["MaxReptCnt"] == DBNull.Value) ? -1 : (int)_reader["MaxReptCnt"],
                (_reader["EndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["EndDate"]
            );
        }

        /// <summary>
        /// Gets the repeat schedule list.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected List<RepeatScheduleInfo> GetRepeatScheduleList(IDataReader reader)
        {
            List<RepeatScheduleInfo> list = new List<RepeatScheduleInfo>();
            while (reader.Read())
                list.Add(GetRepeat(reader));
            return list;
        }

        #region Get

        public List<BE.RepeatScheduleInfo> GetRepeatSchedule(int companyid, int scheduleID)
        {
            using (var cn = new SqlConnection(ConnSolution(companyid)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Repeat_Sel");

                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyid;
                cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = scheduleID;

                cn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    return GetRepeatScheduleList(reader);
                }
            }
        }

        #endregion Get

        #region Add

        public void AddRepeatSchedule(RepeatScheduleInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_RepeatSchedule_Ins", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _info.ScheduleID;
                    cmd.Parameters.Add("@RepeatType", SqlDbType.Char, 1).Value = _info.RepeatType;
                    cmd.Parameters.Add("@SelectType", SqlDbType.Char, 1).Value = _info.SelectType;
                    cmd.Parameters.Add("@IntervalCnt", SqlDbType.Int).Value = _info.IntervalCnt;

                    cmd.Parameters.Add("@Option1", SqlDbType.VarChar, 2).Value = _info.Option1;
                    cmd.Parameters.Add("@Option2", SqlDbType.VarChar, 2).Value = _info.Option2;
                    cmd.Parameters.Add("@Option3", SqlDbType.VarChar, 2).Value = _info.Option3;
                    cmd.Parameters.Add("@Option4", SqlDbType.VarChar, 2).Value = _info.Option4;
                    cmd.Parameters.Add("@Option5", SqlDbType.VarChar, 2).Value = _info.Option5;

                    cmd.Parameters.Add("@Option6", SqlDbType.VarChar, 2).Value = _info.Option6;
                    cmd.Parameters.Add("@Option7", SqlDbType.VarChar, 2).Value = _info.Option7;

                    cmd.Parameters.Add("@MaxReptCnt", SqlDbType.Int).Value = _info.MaxReptCnt;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
            }
        }

        #endregion Add

        #region DEL

        public void DelRepeatSchedule(RepeatScheduleInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_RepeatSchedule_Del", cn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _info.ScheduleID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
            }
        }

        #endregion DEL
    }
}