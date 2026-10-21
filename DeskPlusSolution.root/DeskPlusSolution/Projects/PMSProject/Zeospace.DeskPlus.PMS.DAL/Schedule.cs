using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.DAL
{
    public class Schedule : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ScheduleInfo GetSchedule(IDataReader _reader)
        {
            return new ScheduleInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                ProjectID = (_reader["ProjectID"] == DBNull.Value) ? -1 : _reader["ProjectID"].ToString().ToNumber<int>(),
                ScheduleID = (_reader["ScheduleID"] == DBNull.Value) ? -1 : _reader["ScheduleID"].ToString().ToNumber<int>(),
                ParentScheduleID = (_reader["ParentScheduleID"] == DBNull.Value) ? -1 : _reader["ParentScheduleID"].ToString().ToNumber<int>(),
                Subjects = _reader["Subjects"].ToString().ToNull(),
                Contents = _reader["Contents"].ToString().ToNull().ToEditorXSS(),
                StartDate = (_reader["StartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["StartDate"],
                EndDate = (_reader["EndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["EndDate"],
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                ManyName = _reader["ManyName"].ToString().ToNull(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteDate = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                PimsID = (_reader["PimsID"] == DBNull.Value) ? -1 : _reader["PimsID"].ToString().ToNumber<int>(),

                #region ####  부서,직급,직책 다국어 시작 #######

                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),
                TitleBase = _reader["TitleBase"].ToString().ToNull(),
                TitleKo = _reader["TitleKo"].ToString().ToNull(),
                TitleEn = _reader["TitleEn"].ToString().ToNull(),
                TitleJa = _reader["TitleJa"].ToString().ToNull(),
                TitleFr = _reader["TitleFr"].ToString().ToNull(),
                TitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TitleZhtw = _reader["TitleZhtw"].ToString().ToNull(),
                DutyBase = _reader["DutyBase"].ToString().ToNull(),
                DutyKo = _reader["DutyKo"].ToString().ToNull(),
                DutyEn = _reader["DutyEn"].ToString().ToNull(),
                DutyJa = _reader["DutyJa"].ToString().ToNull(),
                DutyFr = _reader["DutyFr"].ToString().ToNull(),
                DutyZhcn = _reader["DutyZhcn"].ToString().ToNull(),
                DutyZhtw = _reader["DutyZhtw"].ToString().ToNull()

                #endregion ####  부서,직급,직책 다국어 시작 #######
            };
        }

        protected List<ScheduleInfo> GetScheduleList(IDataReader reader)
        {
            List<ScheduleInfo> list = new List<ScheduleInfo>();
            while (reader.Read())
                list.Add(GetSchedule(reader));
            return list;
        }

        #endregion DataReader

        #region ADD()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 추가하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-09
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(ScheduleInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Schedule_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    //cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _info.ScheduleID;
                    cmd.Parameters.Add("@ParentScheduleID", SqlDbType.Int).Value = _info.ParentScheduleID;
                    cmd.Parameters.Add("@Subjects", SqlDbType.NVarChar, 100).Value = _info.Subjects.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NText).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@AddPims", SqlDbType.Int).Value = _info.PimsID;

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : list 수정
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-10-02
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(ScheduleInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Schedule_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _info.ProjectID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _info.ScheduleID;
                    cmd.Parameters.Add("@ParentScheduleID", SqlDbType.Int).Value = _info.ParentScheduleID;
                    cmd.Parameters.Add("@Subjects", SqlDbType.NVarChar, 100).Value = _info.Subjects.ToNull().ToXSS();
                    cmd.Parameters.Add("@Contents", SqlDbType.NText).Value = _info.Contents.ToNull().ToEditorXSS();
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _info.StartDate;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = _info.EndDate;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;
                    cmd.Parameters.Add("@AddPims", SqlDbType.Int).Value = _info.PimsID;

                    cn.Open();
                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 삭제
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-12-22
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_scheduleID">The _schedule ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _projectID, int _scheduleID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Schedule_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _scheduleID;

                    cn.Open();
                    ExecuteNonQuery(cmd);

                    return true;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion DEL()

        #region GET()

        /// <summary>
        /// Gets the schedule month list.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <returns></returns>
        public DataSet GetScheduleMonthList(int _companyID, int _projectID, DateTime _startDate)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PMS_Schedule_SelByMonthList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = _startDate;

                    return ExecuteDataSet(cmd, "GetScheduleMonthList");
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
        /// 프로젝트 기본 일정 정보 반환하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_projectID"></param>
        /// <param name="_scheduleID"></param>
        /// <returns></returns>
        public List<ScheduleInfo> GetSchedule(int _companyID, int _projectID, int _scheduleID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Schedule_SeltByID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = _scheduleID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetScheduleList(reader);
                    }
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
        /// Gets the schedule by parent schedule ID.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_projectID">The _project ID.</param>
        /// <param name="_parentScheduleID">The _parent schedule ID.</param>
        /// <returns></returns>
        public DataTableReader GetScheduleByParentScheduleID(int _companyID, int _projectID, int _parentScheduleID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PMS, "QRY_PMS_Schedule_SeltByPScheduleID");
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@ProjectID", SqlDbType.Int).Value = _projectID;
                    cmd.Parameters.Add("@ParentScheduleID", SqlDbType.Int).Value = _parentScheduleID;

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
        ///  스케줄 가져오기
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public DataTableReader GetSheduleID(BE.ScheduleInfo para)
        {
            using (var cn = new SqlConnection(ConnSolution(para.CompanyID)))
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PIM, "QRY_PMS_Schedule_SelectID");
                var cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = para.CompanyID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = para.WriteUserID;
                cmd.Parameters.Add("@ScheduleID", SqlDbType.Int).Value = para.ScheduleID;
                cn.Open();
                return ExecuteDataTableReader(cmd);
            }
        }

        #endregion GET()
    }
}