using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;

namespace Zeospace.DeskPlusMobile.Web.Pims.Ws
{
    /// <summary>
    /// SetPims의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetPims : Helper.MobileBaseService
    {
        #region DEL()
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정 삭제하기
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string DelSchedule(string p)
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);
            string scheduleID = GetDecryptParam("ScheduleID", "0");
            string displayID = GetDecryptParam("DisplayID", "0");
            

            try
            {
                // DB 삭제     
                /*
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "DEL_PROC_SCHEDULE");

                SqlParameter[] parm = new SqlParameter[] {
										       new SqlParameter("@CompanyID", SqlDbType.Int),
										       new SqlParameter("@ScheduleID", SqlDbType.Int),
									       };
                parm[0].Value = CompanyID;
                parm[1].Value = scheduleID;
                */

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "DEL_PROC_SCHEDULESolo");

                SqlParameter[] parm = new SqlParameter[] {
										       new SqlParameter("@CompanyID", SqlDbType.Int),										       
                                               new SqlParameter("@DisplayID", SqlDbType.Int),
                                               new SqlParameter("@UserID", SqlDbType.Int)
									       };
                parm[0].Value = CompanyID;
                parm[1].Value = displayID;
                parm[2].Value = UserID;                

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);

                // 알람 DB 삭제 
                string sqlQuery1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "DEL_PROC_SCHEDULEALARM");

                SqlParameter[] parm1 = new SqlParameter[] {
										       new SqlParameter("@CompanyID", SqlDbType.Int),
										       new SqlParameter("@ScheduleID", SqlDbType.Int),
                                               new SqlParameter("@DisplayID", SqlDbType.Int)
									       };
                parm1[0].Value = CompanyID;
                parm1[1].Value = scheduleID;
                parm1[2].Value = displayID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, sqlQuery1, parm1);

                /* 삭제해야 하는데 가비지로 남김
                // 첨부파일 삭제
                string targetLocalPath = Server.MapPath(FilePath + "/" + CompanyID + "/Users/" + UserID + "/Pims/" + scheduleID);
                if (targetLocalPath != "" && Convert.ToInt32(scheduleID) != 0)
                {
                    if (System.IO.Directory.Exists(targetLocalPath)) System.IO.Directory.Delete(targetLocalPath, true);
                }
                */

                return "OK";
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("DelSchedule()", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 할일삭제하기
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string DelToDo(string p)
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);
            string toDoID = GetDecryptParam("ToDoID", "0");

            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "DEL_PROC_TODO");

                SqlParameter[] parm = new SqlParameter[] {
										       new SqlParameter("@CompanyID", SqlDbType.Int),
										       new SqlParameter("@ToDoID", SqlDbType.Int),
									       };
                parm[0].Value = CompanyID;
                parm[1].Value = toDoID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);

                return "OK";
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("DelToDo()", ex);
                throw;
            }
        }

        #endregion

        #region ADD()
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정 저장하기
        /// 작성일자 : 2011-06-24
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddSchedule(string p)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Pims.Ws.SetPims).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);

            string companyID = GetDecryptParam("CompanyID", "");
            string title = GetDecryptParam("Title", "");
            string content = GetDecryptParam("Content", "");
            string startDate = GetDecryptParam("StartDate", "");
            string endDate = GetDecryptParam("EndDate", "");
            string startTime = GetDecryptParam("StartTime", "");
            string endTime = GetDecryptParam("EndTime", "");
            string userID = GetDecryptParam("UserID", "0");
            string openType = GetDecryptParam("OpenType", "");
            string deptID = GetDecryptParam("DeptID", "");
            string statusID = GetDecryptParam("StatusID", "0");
            string alarmKind = GetDecryptParam("AlarmKind", "");
            int alarmTime = GetDecryptParam("AlarmTime", "0").ToNumber<int>();
            string alarmMail = GetDecryptParam("AlarmMail", "");
            string alert = "";

            try
            {
                if (!alarmKind.Equals(""))
                {
                    alert = "Y";

                    if (!alarmKind.Equals("E"))
                    {
                        alarmMail = "";
                    }
                    else
                    {
                        alarmTime = 0;
                    }
                }
                else
                {
                    alert = "N";
                    alarmTime = 0;
                    alarmMail = "";
                }

                DateTime dtStart = Convert.ToDateTime(startDate);
                DateTime dtEnd = Convert.ToDateTime(endDate);

                DateTime tmpStartTime = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, Convert.ToDateTime(startTime).Hour, Convert.ToDateTime(startTime).Minute, 0);
                DateTime tmpEndTime = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, Convert.ToDateTime(endTime).Hour, Convert.ToDateTime(endTime).Minute, 0);

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "INS_PROC_SCHEDULE");

                SqlParameter[] parm = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@Type", SqlDbType.Char,1),
											   new SqlParameter("@Title", SqlDbType.NVarChar, 100),
                                               new SqlParameter("@Content", SqlDbType.NText),
											   new SqlParameter("@StartTime", SqlDbType.DateTime, 8),
											   new SqlParameter("@EndTime", SqlDbType.DateTime, 8),
											   new SqlParameter("@Repeat", SqlDbType.Char, 1), 
											   new SqlParameter("@IsOpen", SqlDbType.Char, 1), 
											   new SqlParameter("@UserID", SqlDbType.Int),
                                               new SqlParameter("@DeptID", SqlDbType.Int),
									           new SqlParameter("@OpenType", SqlDbType.TinyInt),                                               
                                               new SqlParameter("@StatusID", SqlDbType.Int),
                                               new SqlParameter("@Alert", SqlDbType.Char, 1), 
                                               new SqlParameter("@AlertType", SqlDbType.Char, 1), 
                                               new SqlParameter("@AlertTime", SqlDbType.Int), 
                                               new SqlParameter("@AlertTargetAddress", SqlDbType.NVarChar, 200)
										   };
                parm[0].Value = companyID;
                parm[1].Value = "I";
                parm[2].Value = Server.UrlDecode(title).ToXSS();
                parm[3].Value = Server.UrlDecode(content).ToXSS().ToEditorXSS();
                parm[4].Value = tmpStartTime;
                parm[5].Value = tmpEndTime;
                parm[6].Value = "N";
                parm[7].Value = "Y";
                parm[8].Value = userID;
                parm[9].Value = deptID;
                parm[10].Value = openType;
                parm[11].Value = statusID;
                parm[12].Value = alert;
                parm[13].Value = alarmKind;
                parm[14].Value = alarmTime;
                parm[15].Value = alarmMail;

                //SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, "dsPim", parm);

                string retunVal = ds.Tables["dsPim"].Rows[0][0].ToString();
                string[] arrVal = retunVal.Split('^');

                if (alert == "Y")
                {
                    BE.ScheduleInfo info = new BE.ScheduleInfo();
                    info.CompanyID = companyID.ToNumber<int>();
                    info.ScheduleID = arrVal[0].ToString().ToNumber<int>();
                    info.Title = Server.UrlDecode(title);
                    info.Content = Server.UrlDecode(content);
                    info.AlertType = alarmKind;
                    info.AlertTime = alarmTime;
                    info.AlertTargetAddress = alarmMail;

                    Dictionary<string, string> paraDical = new Dictionary<string, string>();

                    paraDical.Add("DisplayID", arrVal[1].ToString());
                    paraDical.Add("CompanyID", companyID);
                    paraDical.Add("ScheduleID", arrVal[0].ToString());
                    info.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                    info.SenderMail = UserInfo.MailID + "@" + UserInfo.MailDomain;

                    if (alarmKind == "M" || alarmKind == "B")
                        SendAlarm(info, UserID, arrVal[1].ToNumber<int>(), tmpStartTime, tmpEndTime);

                    //매일알람을 선택했을경우
                    if ((alarmKind == "E") && (info.AlertTargetAddress != ""))
                    {
                        info.Title = "[" + resMng.GetString("PimAlarm") + "]" + tmpStartTime + "~" + tmpEndTime + " " + info.Title;
                        SendAlarm(info, 0, 0, tmpStartTime, tmpEndTime);
                    }                                    
                }

                return "OK";
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("AddSchedule()", ex);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 할일 저장하기
        /// 작성일자 : 2011-06-24
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddToDo(string p)
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);

            string companyID = GetDecryptParam("CompanyID", "");
            string title = GetDecryptParam("Title", "");
            string content = GetDecryptParam("Content", "");
            string startDate = GetDecryptParam("StartDate", "");
            string status = GetDecryptParam("Status", "");
            string priority = GetDecryptParam("Priority", "");
            string userID = GetDecryptParam("UserID", "0");
            string openType = GetDecryptParam("OpenType", "");
            string deptID = GetDecryptParam("DeptID", "");

            try
            {
                DateTime dtStart = Convert.ToDateTime(startDate);
                DateTime tmpStartDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day);
                DateTime tmpEndDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day);

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "INS_PROC_TODO");

                SqlParameter[] parm = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@Type", SqlDbType.Char,1),
                                               new SqlParameter("@Status", SqlDbType.Char,1),
                                               new SqlParameter("@Priority", SqlDbType.VarChar,2),
											   new SqlParameter("@Title", SqlDbType.NVarChar, 100),
                                               new SqlParameter("@Content", SqlDbType.NText),
                                               new SqlParameter("@StartDate", SqlDbType.DateTime, 8),
											   new SqlParameter("@EndDate", SqlDbType.DateTime, 8),
											   new SqlParameter("@ScheduleYN", SqlDbType.Char, 1), 
											   new SqlParameter("@IsOpen", SqlDbType.Char, 1), 
											   new SqlParameter("@UserID", SqlDbType.Int),
                                               new SqlParameter("@DeptID", SqlDbType.Int),
                                               new SqlParameter("@OpenType", SqlDbType.TinyInt)       
										   };
                parm[0].Value = companyID;
                parm[1].Value = "I";
                parm[2].Value = status;
                parm[3].Value = priority;
                parm[4].Value = Server.UrlDecode(title).ToXSS();
                parm[5].Value = Server.UrlDecode(content).ToXSS().ToEditorXSS();
                parm[6].Value = tmpStartDate;
                parm[7].Value = tmpEndDate;
                parm[8].Value = "N";
                parm[9].Value = "Y";
                parm[10].Value = userID;
                parm[11].Value = deptID;
                parm[12].Value = openType;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);
                return "OK";
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("AddToDo()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정 댓글 등록
        /// 작성일자 : 2012-11-07
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddScheduleReply(string p)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Pims.Ws.SetPims).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            SetDecryptString(p);
            string scheduleID = GetDecryptParam("ScheduleID", "0");
            string displayID = GetDecryptParam("DisplayID", "0");
            string content = GetDecryptParam("Content", "");
            string isSecurity = GetDecryptParam("IsSecurity", "0");

            try
            {
                if (CompanyID <= 0 || UserID <= 0) return false;

                BE.ScheduleReplyInfo info = new BE.ScheduleReplyInfo();

                info.CompanyID = CompanyID;
                info.Content = Server.UrlDecode(content).ToReverseXSS();
                info.ScheduleID = scheduleID.ToNumber<int>();
                info.DisplayID = displayID.ToNumber<int>();
                info.UserID = UserID;
                info.UserName = UserInfo.Name;
                info.IsSecurity = isSecurity;

                if (!ScheduleReplyAdd(info))
                {
                    return false;
                }

                // 알람발송 
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_ScheduleID");
                sql = string.Format(sql, CompanyID.ToString(), scheduleID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsSchedule", null);

                int recvUserID = 0;
                string recvName = "";
                string recvTitle = "";
                string subject = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //원본 작성자 아이디
                    recvUserID = dr["UserID"].ToString().ToNumber<int>();

                    //원본 작성자 이름
                    recvName = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(),
                        PreLang);

                    recvTitle = dr["Title"].ToString().ToReverseXSS(); //제목

                    // 알람제목
                    subject = "[" + resMng.GetString("Schedule") + "]" + recvTitle + resMng.GetString("InsertReply");

                    //원본작성자와 짧은댓글 작성자가 동일하면 보내지 않는다.
                    if (recvUserID != info.UserID)
                    {
                        // 알람발송(원본글)
                        SendAlarm(info, recvUserID, subject);
                    }
                }

                //짧은댓글 기재자들에게 알람 발송하기
                string sql1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_ScheduleReply");
                sql1 = string.Format(sql1, CompanyID.ToString(), scheduleID);
                DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql1, "dsSR", null);

                //짧은댓글 작성자 아이디 정보
                List<int> userList = new List<int>();
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    //원본작성자와 짧은댓글 작성자가 동일하면 보내지 않는다.                    
                    if (recvUserID != dr["UserID"].ToString().ToNumber<int>() && info.UserID != dr["UserID"].ToString().ToNumber<int>())
                    {
                        //다중 댓글 중복자에게 보내지 않도록 한다. 1통만 보내자.~~~~
                        if (!userList.Exists(c => c == dr["UserID"].ToString().ToNumber<int>()))
                        {
                            BE.ScheduleReplyInfo replyInfo = new BE.ScheduleReplyInfo();

                            replyInfo.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                            replyInfo.ScheduleID = dr["ScheduleID"].ToString().ToNumber<int>();
                            replyInfo.DisplayID = displayID.ToNumber<int>();

                            replyInfo.AlarmContent = info.Content; //알람내용
                            replyInfo.Content = info.Content;
                            replyInfo.UserID = recvUserID;  //보내는 사용자 ID (작성자)
                            replyInfo.UserName = recvName;   //보내는 사람   (작성자 이름)

                            SendAlarm(replyInfo, dr["UserID"].ToString().ToNumber<int>(), subject);
                            userList.Add(dr["UserID"].ToString().ToNumber<int>());
                        }
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddScheduleReply", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정 댓글 추가
        /// 작성일자 : 2012-11-07
        /// </summary>
        /// <param name="_scheduleReplyInfo"></param>
        /// <returns></returns>
        public bool ScheduleReplyAdd(BE.ScheduleReplyInfo _scheduleReplyInfo)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@ScheduleID", SqlDbType.Int)
                                               ,new SqlParameter("@Content", SqlDbType.NVarChar,200)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@IsSecurity", SqlDbType.Char, 1)                                             
                };

                parms[0].Value = _scheduleReplyInfo.CompanyID;
                parms[1].Value = _scheduleReplyInfo.ScheduleID;
                parms[2].Value = _scheduleReplyInfo.Content;
                parms[3].Value = _scheduleReplyInfo.UserID;
                parms[4].Value = _scheduleReplyInfo.IsSecurity;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "INS_PROC_ScheduleReply");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsScheduleReply", parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("ScheduleReplyAdd(" + _scheduleReplyInfo.ScheduleID.ToString() + "," + _scheduleReplyInfo.UserID + ")", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 할일 댓글 등록
        /// 작성일자 : 2012-11-08
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddToDoReply(string p)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Pims.Ws.SetPims).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            SetDecryptString(p);
            string toDoID = GetDecryptParam("ToDoID", "0");
            string content = GetDecryptParam("Content", "");
            string isSecurity = GetDecryptParam("IsSecurity", "0");

            try
            {
                if (CompanyID <= 0 || UserID <= 0) return false;

                BE.ToDoReplyInfo info = new BE.ToDoReplyInfo();

                info.CompanyID = CompanyID;
                info.Content = Server.UrlDecode(content).ToReverseXSS();
                info.ToDoID = toDoID.ToNumber<int>();
                info.UserID = UserID;
                info.UserName = UserInfo.Name;
                info.IsSecurity = isSecurity;

                if (!ToDoReplyAdd(info))
                {
                    return false;
                }

                // 알람발송 
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_ToDoID");
                sql = string.Format(sql, CompanyID.ToString(), toDoID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsToDo", null);

                int recvUserID = 0;
                string recvName = "";
                string recvTitle = "";
                string subject = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //원본 작성자 아이디
                    recvUserID = dr["UserID"].ToString().ToNumber<int>();

                    //원본 작성자 이름
                    recvName = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(),
                        PreLang);

                    recvTitle = dr["Title"].ToString().ToReverseXSS(); //제목

                    // 알람제목
                    subject = "[" + resMng.GetString("ToDo") + "]" + recvTitle + resMng.GetString("InsertReply");

                    //원본작성자와 짧은댓글 작성자가 동일하면 보내지 않는다.
                    if (recvUserID != info.UserID)
                    {
                        // 알람발송(원본글)
                        SendAlarm(info, recvUserID, subject);
                    }
                }

                //짧은댓글 기재자들에게 알람 발송하기
                string sql1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_ToDoReply");
                sql1 = string.Format(sql1, CompanyID.ToString(), toDoID);
                DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql1, "dsTD", null);

                //짧은댓글 작성자 아이디 정보
                List<int> userList = new List<int>();
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    //원본작성자와 짧은댓글 작성자가 동일하면 보내지 않는다.                    
                    if (recvUserID != dr["UserID"].ToString().ToNumber<int>() && info.UserID != dr["UserID"].ToString().ToNumber<int>())
                    {
                        //다중 댓글 중복자에게 보내지 않도록 한다. 1통만 보내자.~~~~
                        if (!userList.Exists(c => c == dr["UserID"].ToString().ToNumber<int>()))
                        {
                            BE.ToDoReplyInfo replyInfo = new BE.ToDoReplyInfo();

                            replyInfo.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                            replyInfo.ToDoID = dr["ToDoID"].ToString().ToNumber<int>();

                            replyInfo.AlarmContent = info.Content; //알람내용
                            replyInfo.Content = info.Content;
                            replyInfo.UserID = recvUserID;  //보내는 사용자 ID (작성자)
                            replyInfo.UserName = recvName;   //보내는 사람   (작성자 이름)

                            SendAlarm(replyInfo, dr["UserID"].ToString().ToNumber<int>(), subject);
                            userList.Add(dr["UserID"].ToString().ToNumber<int>());
                        }
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddToDoReply", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 할일 댓글 추가
        /// 작성일자 : 2012-11-07
        /// </summary>
        /// <param name="_toDoReplyInfo"></param>
        /// <returns></returns>
        public bool ToDoReplyAdd(BE.ToDoReplyInfo _toDoReplyInfo)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@ToDoID", SqlDbType.Int)
                                               ,new SqlParameter("@Content", SqlDbType.NVarChar,200)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@IsSecurity", SqlDbType.Char, 1)                                             
                };

                parms[0].Value = _toDoReplyInfo.CompanyID;
                parms[1].Value = _toDoReplyInfo.ToDoID;
                parms[2].Value = _toDoReplyInfo.Content;
                parms[3].Value = _toDoReplyInfo.UserID;
                parms[4].Value = _toDoReplyInfo.IsSecurity;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "INS_PROC_ToDoReply");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsToDoReply", parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("ToDoReplyAdd(" + _toDoReplyInfo.ToDoID.ToString() + "," + _toDoReplyInfo.UserID + ")", ex);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-11-07
        /// 작성목적 : 일정 댓글 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_subject"></param>
        /// <returns></returns>
        public void SendAlarm(BE.ScheduleReplyInfo _info, int _receiverID, string _subject)
        {
            try
            {
                DataView dvTask = XmlDataBaseHelper.GetSysCode(_info.CompanyID, "AlarmTask");

                string AlarmTask = string.Empty;
                string AlarmMail = string.Empty;
                string AlarmMessenger = string.Empty;
                string AlarmMobile = string.Empty;
                string AlarmSMS = string.Empty;

                DataSet dsUser = GetUserInfo(_info.CompanyID, _receiverID);

                if (dsUser != null)
                {
                    foreach (DataRow dr in dsUser.Tables[0].Rows)
                    {
                        AlarmTask = dr["AlarmTask"].ToString();
                    }
                }

                if (AlarmTask.IndexOf("1-").Equals(0))
                {
                    AlarmMail = (AlarmTask.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                    AlarmMessenger = (AlarmTask.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                    AlarmMobile = (AlarmTask.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                    AlarmSMS = (AlarmTask.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                }
                else
                {
                    AlarmMessenger = "0";
                    AlarmMail = "0";
                    AlarmMobile = "0";
                    AlarmSMS = "0";
                }

                foreach (DataRowView dr in dvTask)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MESSENGER":
                            if (AlarmMessenger.Equals("1"))
                            {
                                SendMessenger(_info.CompanyID, _info.UserID, _info.UserName, _info.Content, _receiverID, _subject);
                            }
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력                
                SendMobile(_info.CompanyID, _info.UserID, _info.UserName, _info.Content, _receiverID, _subject, _info.ScheduleID, _info.DisplayID);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SendAlarm()", ex);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-11-08
        /// 작성목적 : 할일 댓글 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_subject"></param>
        /// <returns></returns>
        public void SendAlarm(BE.ToDoReplyInfo _info, int _receiverID, string _subject)
        {
            try
            {
                DataView dvTask = XmlDataBaseHelper.GetSysCode(_info.CompanyID, "AlarmTask");

                string AlarmTask = string.Empty;
                string AlarmMail = string.Empty;
                string AlarmMessenger = string.Empty;
                string AlarmMobile = string.Empty;
                string AlarmSMS = string.Empty;

                DataSet dsUser = GetUserInfo(_info.CompanyID, _receiverID);

                if (dsUser != null)
                {
                    foreach (DataRow dr in dsUser.Tables[0].Rows)
                    {
                        AlarmTask = dr["AlarmTask"].ToString();
                    }
                }

                if (AlarmTask.IndexOf("1-").Equals(0))
                {
                    AlarmMail = (AlarmTask.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                    AlarmMessenger = (AlarmTask.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                    AlarmMobile = (AlarmTask.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                    AlarmSMS = (AlarmTask.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                }
                else
                {
                    AlarmMessenger = "0";
                    AlarmMail = "0";
                    AlarmMobile = "0";
                    AlarmSMS = "0";
                }

                foreach (DataRowView dr in dvTask)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MESSENGER":
                            if (AlarmMessenger.Equals("1"))
                            {
                                SendMessenger(_info.CompanyID, _info.UserID, _info.UserName, _info.Content, _receiverID, _subject);
                            }
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력                
                SendMobile(_info.CompanyID, _info.UserID, _info.UserName, _info.Content, _receiverID, _subject, _info.ToDoID);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SendAlarm()", ex);
            }

        }

        /// <summary>
        /// 메신저
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_senderID"></param>
        /// <param name="_senderName"></param>
        /// <param name="_content"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_subject"></param>
        private void SendMessenger(int _companyID, int _senderID, string _senderName, string _content, int _receiverID, string _subject)
        {
            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();

            alarmInfo.CompanyID = _companyID;
            alarmInfo.SenderID = _senderID;
            alarmInfo.SenderName = _senderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _subject;
            alarmInfo.Content = _content.ToReverseXSS();

            string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string alarmTargetURL = httpType + fullDomain + "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims";

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Pim";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm biz = new Messenger.BLL.Alarm();
            biz.Add(alarmInfo);
        }

        /// <summary>
        /// 모바일 - 일정
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_senderID"></param>
        /// <param name="_senderName"></param>
        /// <param name="_content"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_subject"></param>
        /// <param name="_scheduleID"></param>
        /// <param name="_displayID"></param>
        private void SendMobile(int _companyID, int _senderID, string _senderName, string _content, int _receiverID, string _subject, int _scheduleID, int _displayID)
        {
            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();

            alarmInfo.CompanyID = _companyID;
            alarmInfo.SenderID = _senderID;
            alarmInfo.SenderName = _senderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _subject;
            alarmInfo.Content = _content.ToReverseXSS();

            string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string alarmTargetURL = httpType + fullDomain + "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("ScheduleID", _scheduleID.ToString());
            paraDic.Add("DisplayID", _displayID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            string alarmMobileURL = "/DeskPlusMobile/Pims/AlarmView.aspx?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = alarmMobileURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "Pim";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm biz = new Messenger.BLL.Alarm();
            biz.Add(alarmInfo);
        }

        /// <summary>
        /// 모바일 - 할일
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_senderID"></param>
        /// <param name="_senderName"></param>
        /// <param name="_content"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_subject"></param>
        /// <param name="_toDoID"></param>
        private void SendMobile(int _companyID, int _senderID, string _senderName, string _content, int _receiverID, string _subject, int _toDoID)
        {
            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();

            alarmInfo.CompanyID = _companyID;
            alarmInfo.SenderID = _senderID;
            alarmInfo.SenderName = _senderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _subject;
            alarmInfo.Content = _content.ToReverseXSS();

            string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string alarmTargetURL = httpType + fullDomain + "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("ToDoID", _toDoID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            string alarmMobileURL = "/DeskPlusMobile/Pims/AlarmView.aspx?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = alarmMobileURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "Pim";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm biz = new Messenger.BLL.Alarm();
            biz.Add(alarmInfo);
        }

        /// <summary>
        /// 알람 
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_displayID"></param>
        /// <param name="_startTime"></param>
        /// <param name="_endTime"></param>
        public void SendAlarm(BE.ScheduleInfo _info, int _receiverID, int _displayID, DateTime _startTime, DateTime _endTime)
        {
            if (_info.AlertType == "M") //메신저     
            {
                string AlarmPims = string.Empty;
                string AlarmMessenger = string.Empty;

                if (_receiverID > 0)
                {
                    string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_PimAlarm");
                    sqlQuery = String.Format(sqlQuery, _info.CompanyID, _receiverID);

                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "dsAlarm", null);

                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AlarmPims = dr["AlarmPims"].ToString();
                        }
                    }          

                    if (AlarmPims.IndexOf("1-").Equals(0))
                    {
                        AlarmMessenger = (AlarmPims.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                    }
                    else
                    {
                        AlarmMessenger = "0";
                    }
                }

                if (AlarmMessenger.Equals("1"))
                {
                    SendMessenger(_info, _receiverID, _displayID,  _startTime,  _endTime);
                }
            }
            else if (_info.AlertType == "E") //메일        
            {
                SendMail(_info);
            }
            else if (_info.AlertType == "B") //모바일    
            {
                SendMobile(_info, _receiverID, _displayID, _startTime, _endTime);
            }
        }

        /// <summary>
        /// 메신저
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_scheduleID"></param>
        /// <param name="_displayID"></param>
        private void SendMessenger(BE.ScheduleInfo _info, int _receiverID, int _displayID, DateTime _startTime,  DateTime _endTime)
        {
            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();

            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = UserInfo.UserID;
            alarmInfo.SenderName = UserInfo.Name;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";

            alarmInfo.SendDate = _startTime.AddMinutes((_info.AlertTime * -1));
            alarmInfo.Subject = _startTime + "~" + _endTime + " " + _info.Title.ToReverseXSS();
            alarmInfo.Content = "";

            string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");

            string alarmTargetURL = httpType + fullDomain + "/DeskPlusEIP/Pim/Schedule/ViewPopup.aspx";
            alarmTargetURL = alarmTargetURL + "?" + "P=" + _info.Parameterstring;
            alarmInfo.TargetUrl = alarmTargetURL;

            alarmInfo.StartDate = _startTime.AddMinutes(-_info.AlertTime);
            alarmInfo.EndDate = _startTime.AddMinutes(-_info.AlertTime);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Pim";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;
            alarmInfo.ScheduleID = _info.ScheduleID;
            alarmInfo.DisplayID = _displayID;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm biz = new Messenger.BLL.Alarm();
            biz.Add(alarmInfo);
        }

        /// <summary>
        /// 모바일
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_scheduleID"></param>
        /// <param name="_displayID"></param>
        private void SendMobile(BE.ScheduleInfo _info, int _receiverID, int _displayID, DateTime _startTime, DateTime _endTime)
        {
            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();

            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = UserInfo.UserID;
            alarmInfo.SenderName = UserInfo.Name;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";

            alarmInfo.SendDate = _startTime.AddMinutes((_info.AlertTime * -1));
            alarmInfo.Subject = _startTime + "~" + _endTime + " " + _info.Title.ToReverseXSS();
            alarmInfo.Content = "";

            string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");

            string alarmTargetURL = httpType + fullDomain + "/DeskPlusEIP/Pim/Schedule/ViewPopup.aspx";
            alarmTargetURL = alarmTargetURL + "?" + "P=" + _info.Parameterstring;
            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = "/DeskPlusMobile/Pims/AlarmView.aspx?p=" + _info.Parameterstring;

            alarmInfo.StartDate = _startTime.AddMinutes(-_info.AlertTime);
            alarmInfo.EndDate = _startTime.AddMinutes(-_info.AlertTime);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "Pim";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;
            alarmInfo.ScheduleID = _info.ScheduleID;
            alarmInfo.DisplayID = _displayID;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm biz = new Messenger.BLL.Alarm();
            biz.Add(alarmInfo);
        }

        /// <summary>
        /// E-MAIL
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="attach"></param>
        private void SendMail(BE.ScheduleInfo _info)
        {
            MailDataInfo mailInfo = null;

            mailInfo = new MailDataInfo();
            mailInfo.From = UserInfo.Name + "<" + _info.SenderMail + ">";
            //mailInfo.To.Add(_to, _to);

            //받는 사람
            foreach (string tmp in _info.AlertTargetAddress.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailInfo.To.Contains(tmp)) mailInfo.To.Add(tmp, tmp);
                }
            }

            mailInfo.Subject = _info.Title;
            mailInfo.Contents = _info.Content;

            Helper.AppException.WriteAssert(AppSettingType.SQLApproval, "Pim:CreateMHT:" + _info.AlertTargetAddress + "," + _info.SenderMail + "," + _info.Title + "," + _info.Content);

            Zeospace.DeskPlusMobile.Web.Helper.SendMail sendMail = new SendMail();
            sendMail.Send(mailInfo, false, false, "");
        }
     
        #endregion

        #region 알람대상자 정보
        /// <summary>
        /// 알람대상자 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        private DataSet GetUserInfo(int _companyID, int _userID)
        {
            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_ACCOUNTDEPT");
                proc = string.Format(proc, _companyID, _userID);

                return SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserInfo", null);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetUserInfo()", ex);
                throw;
            }
        }
        #endregion

        #region MOD()
        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 일정 수정하기
        /// 작성일자 : 2011-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ModSchedule(string p)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Pims.Ws.SetPims).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);

            string companyID = GetDecryptParam("CompanyID", "");
            string scheduleID = GetDecryptParam("ScheduleID", "");
            string title = GetDecryptParam("Title", "");
            string content = GetDecryptParam("Content", "");
            string startDate = GetDecryptParam("StartDate", "");
            string endDate = GetDecryptParam("EndDate", "");
            string startTime = GetDecryptParam("StartTime", "");
            string endTime = GetDecryptParam("EndTime", "");
            string userID = GetDecryptParam("UserID", "0");
            string openType = GetDecryptParam("OpenType", "");
            string displayID = GetDecryptParam("DisplayID", "0");
            string statusID = GetDecryptParam("StatusID", "0");
            string alarmKind = GetDecryptParam("AlarmKind", "");
            int alarmTime = GetDecryptParam("AlarmTime", "0").ToNumber<int>();
            string alarmMail = GetDecryptParam("AlarmMail", "");
            string alert = "";

            try
            {
                if (!alarmKind.Equals(""))
                {
                    alert = "Y";

                    if (!alarmKind.Equals("E"))
                    {
                        alarmMail = "";
                    }
                    else
                    {
                        alarmTime = 0;
                    }
                }
                else
                {
                    alert = "N";
                    alarmTime = 0;
                    alarmMail = "";
                }

                // 알람 DB 삭제 
                string sqlQuery1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "DEL_PROC_SCHEDULEALARM");

                SqlParameter[] parm1 = new SqlParameter[] {
										       new SqlParameter("@CompanyID", SqlDbType.Int),
										       new SqlParameter("@ScheduleID", SqlDbType.Int),
                                               new SqlParameter("@DisplayID", SqlDbType.Int)
									       };
                parm1[0].Value = CompanyID;
                parm1[1].Value = scheduleID;
                parm1[2].Value = displayID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_ALARM, CommandType.StoredProcedure, sqlQuery1, parm1);

                // 스케줄 수정
                string[] arrSDate = startDate.Split('-');
                string[] arrEDate = endDate.Split('-');

                DateTime dtStart = Convert.ToDateTime(startDate);
                DateTime dtEnd = Convert.ToDateTime(endDate);

                DateTime tmpStartTime = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, Convert.ToDateTime(startTime).Hour, Convert.ToDateTime(startTime).Minute, 0);
                DateTime tmpEndTime = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, Convert.ToDateTime(endTime).Hour, Convert.ToDateTime(endTime).Minute, 0);

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "UPD_PROC_SCHEDULE");

                SqlParameter[] parm = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@ScheduleID", SqlDbType.Int),
											   new SqlParameter("@Title", SqlDbType.NVarChar, 100),
                                               new SqlParameter("@Content", SqlDbType.NText),
											   new SqlParameter("@StartTime", SqlDbType.DateTime, 8),
											   new SqlParameter("@EndTime", SqlDbType.DateTime, 8),
									           new SqlParameter("@OpenType", SqlDbType.TinyInt),
                                               new SqlParameter("@DisplayID", SqlDbType.Int),
                                               new SqlParameter("@StatusID", SqlDbType.Int),
                                               new SqlParameter("@Alert", SqlDbType.Char, 1), 
                                               new SqlParameter("@AlertType", SqlDbType.Char, 1), 
                                               new SqlParameter("@AlertTime", SqlDbType.Int), 
                                               new SqlParameter("@AlertTargetAddress", SqlDbType.NVarChar, 200)
										   };
                parm[0].Value = companyID;
                parm[1].Value = scheduleID;
                parm[2].Value = Server.UrlDecode(title).ToXSS();
                parm[3].Value = Server.UrlDecode(content).ToXSS().ToEditorXSS();
                parm[4].Value = tmpStartTime;
                parm[5].Value = tmpEndTime;
                parm[6].Value = openType;
                parm[7].Value = displayID;
                parm[8].Value = statusID;
                parm[9].Value = alert;
                parm[10].Value = alarmKind;
                parm[11].Value = alarmTime;
                parm[12].Value = alarmMail;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);

                if (alert == "Y")
                {
                    BE.ScheduleInfo info = new BE.ScheduleInfo();
                    info.CompanyID = companyID.ToNumber<int>();
                    info.ScheduleID = scheduleID.ToNumber<int>();
                    info.Title = Server.UrlDecode(title);
                    info.Content = Server.UrlDecode(content);
                    info.AlertType = alarmKind;
                    info.AlertTime = alarmTime;
                    info.AlertTargetAddress = alarmMail;

                    Dictionary<string, string> paraDical = new Dictionary<string, string>();

                    paraDical.Add("DisplayID", displayID);
                    paraDical.Add("CompanyID", companyID);
                    paraDical.Add("ScheduleID", scheduleID);
                    info.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                    info.SenderMail = UserInfo.MailID + "@" + UserInfo.MailDomain;

                    if (alarmKind == "M" || alarmKind == "B")
                        SendAlarm(info, UserID, displayID.ToNumber<int>(), tmpStartTime, tmpEndTime);

                    //매일알람을 선택했을경우
                    if ((alarmKind == "E") && (info.AlertTargetAddress != ""))
                    {
                        info.Title = "[" + resMng.GetString("PimAlarm") + "]" + tmpStartTime + "~" + tmpEndTime + " " + info.Title;
                        SendAlarm(info, 0, 0, tmpStartTime, tmpEndTime);
                    }
                }

                return "OK";
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("ModSchedule()", ex);
                throw;
            }
        }



        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 할일 수정하기
        /// 작성일자 : 2011-06-24
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ModToDo(string p)
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);

            string companyID = GetDecryptParam("CompanyID", "");
            string toDoID = GetDecryptParam("ToDoID", "");
            string startDate = GetDecryptParam("StartDate", "");
            string title = GetDecryptParam("Title", "");
            string content = GetDecryptParam("Content", "");
            string status = GetDecryptParam("Status", "");
            string priority = GetDecryptParam("Priority", "");
            string userID = GetDecryptParam("UserID", "0");
            string openType = GetDecryptParam("OpenType", "");

            try
            {
                DateTime dtStart = Convert.ToDateTime(startDate);
                DateTime tmpStartDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day);
                DateTime tmpEndDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day);

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "UPD_PROC_TODO");

                SqlParameter[] parm = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
                                               new SqlParameter("@ToDoID", SqlDbType.Int),
                                               new SqlParameter("@Status", SqlDbType.Char,1),
                                               new SqlParameter("@Priority", SqlDbType.VarChar,2),
											   new SqlParameter("@Title", SqlDbType.NVarChar, 100),
                                               new SqlParameter("@Content", SqlDbType.NText),
                                               new SqlParameter("@StartDate", SqlDbType.DateTime, 8),
									           new SqlParameter("@EndDate", SqlDbType.DateTime, 8),
                                               new SqlParameter("@OpenType", SqlDbType.TinyInt)  
										   };
                parm[0].Value = companyID;
                parm[1].Value = toDoID;
                parm[2].Value = status;
                parm[3].Value = priority;
                parm[4].Value = Server.UrlDecode(title).ToXSS();
                parm[5].Value = Server.UrlDecode(content).ToXSS().ToEditorXSS();
                parm[6].Value = tmpStartDate;
                parm[7].Value = tmpEndDate;
                parm[8].Value = openType;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);
                return "OK";
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("ModToDo()", ex);
                throw;
            }
        }

        #endregion
    }
}
