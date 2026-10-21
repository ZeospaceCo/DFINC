using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.IO;
using System.Xml;
using System.Text;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using Zeospace.DeskPlusMobile.Web.Task.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Task.Ws
{
    /// <summary>
    /// SetTask의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetTask : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 답변하기
        /// 작성일자 : 2015-05-14
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddReply(string p)
        {
            SetDecryptString(p);
            string bType = GetDecryptParam("BType", "0");
            string listID = GetDecryptParam("ListID", "");
            string status = GetDecryptParam("Status", "0");
            string content = GetDecryptParam("Content", "");

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "INS_PROC_TAS_List");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@GroupID", SqlDbType.Int),                                                                                                                        
                                                            new SqlParameter("@Title", SqlDbType.NVarChar,100),
                                                            new SqlParameter("@Contents", SqlDbType.NText),
                                                            new SqlParameter("@ContentText", SqlDbType.NText),
                                                            new SqlParameter("@Status", SqlDbType.Char,1),
                                                            new SqlParameter("@IsMsg", SqlDbType.Char,1),
                                                            new SqlParameter("@IsType", SqlDbType.Char,2),
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int)
                };
                parms[0].Value = CompanyID;
                parms[1].Value = listID;
                parms[2].Value = listID;
                parms[3].Value = "";
                parms[4].Value = Server.UrlDecode(content).ToReverseXSS();
                parms[5].Value = Server.UrlDecode(content).ToReverseXSS();
                parms[6].Value = status;
                parms[7].Value = "0";
                parms[8].Value = bType;
                parms[9].Value = UserID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                if (Convert.ToInt32(obj) > 0)
                {
                    // 알람처리  
                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_QRY_TaskInfo");
                    sql = string.Format(sql, CompanyID.ToString(), listID);
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                    BE.ListInfo info = new ListInfo();
                    info.CompanyID = CompanyID;
                    info.ListID = listID.ToNumber<int>();
                    info.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                    info.RequestUserID = ds.Tables[0].Rows[0]["WriteUserID"].ToString().ToNumber<int>();
                    info.WriteUserID = UserID;
                    info.SenderName = UserInfo.Name;
                    info.PreLang = PreLang;

                    BLL.Alarm alarmBiz = new BLL.Alarm();
                    alarmBiz.AlarmEtc(info, "REPLY");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("AddReply: " + errMsg);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 접수하기
        /// 작성일자 : 2015-05-14
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddAccept(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("ListID", "");
            string startDate = GetDecryptParam("StartDate", "");
            string endDate = GetDecryptParam("EndDate", "");
            string content = GetDecryptParam("Content", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Task.Ws.SetTask).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string IsMsg = "1";

                if (content.Trim() != "")
                {
                    IsMsg = "0";
                }

                string status = "2";
                int ymd = System.DateTime.Now.ToString("yyyyMMdd").ToNumber<int>();
                int tmpStartDate = DateHelper.GetCultureDate(startDate, "yyyyMMdd", PreLangReal).ToNumber<int>();
                int tmpEndDate = DateHelper.GetCultureDate(endDate, "yyyyMMdd", PreLangReal).ToNumber<int>();

                if (tmpEndDate >= ymd)
                {
                    if (tmpStartDate <= ymd)
                    {
                        status = "3";
                    }
                }
                else
                {
                    status = "6"; //지연
                }

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "INS_PROC_TAS_Accept");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),                                                            
                                                            new SqlParameter("@Contents", SqlDbType.NText),
                                                            new SqlParameter("@MsgContent", SqlDbType.NVarChar,100),                                                            
                                                            new SqlParameter("@Status", SqlDbType.Char,1),
                                                            new SqlParameter("@IsMsg", SqlDbType.Char,1),
                                                            new SqlParameter("@IsType", SqlDbType.Char,2),
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int),
                                                            new SqlParameter("@StartDate", SqlDbType.Char,8),
                                                            new SqlParameter("@EndDate", SqlDbType.Char,8)
                };
                parms[0].Value = CompanyID;
                parms[1].Value = listID;
                parms[2].Value = Server.UrlDecode(content).ToReverseXSS();
                parms[3].Value = resMng.GetString("TaskAccept");
                parms[4].Value = status;
                parms[5].Value = IsMsg;
                parms[6].Value = "TO";
                parms[7].Value = UserID;
                parms[8].Value = tmpStartDate;
                parms[9].Value = tmpEndDate;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                if (Convert.ToInt32(obj) > 0)
                {
                    // 알람처리  
                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_QRY_TaskInfo");
                    sql = string.Format(sql, CompanyID.ToString(), listID);
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                    BE.ListInfo info = new ListInfo();
                    info.CompanyID = CompanyID;
                    info.ListID = listID.ToNumber<int>();
                    info.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                    info.RequestUserID = ds.Tables[0].Rows[0]["WriteUserID"].ToString().ToNumber<int>();
                    info.WriteUserID = UserID;
                    info.SenderName = UserInfo.Name;
                    info.PreLang = PreLang;

                    BLL.Alarm alarmBiz = new BLL.Alarm();
                    alarmBiz.AlarmEtc(info, "ACCEPT");

                    return status;
                }
                else
                {
                    return "";
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("AddAccept: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 완료하기
        /// 작성일자 : 2015-05-14
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddComplete(string p)
        {
            SetDecryptString(p);
            string bType = GetDecryptParam("BType", "0");
            string listID = GetDecryptParam("ListID", "");
            string startDate = GetDecryptParam("StartDate", "");
            string endDate = GetDecryptParam("EndDate", "");
            string content = GetDecryptParam("Content", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Task.Ws.SetTask).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string IsMsg = "1";

                if (content.Trim() != "")
                {
                    IsMsg = "0";
                }

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "INS_PROC_TAS_Complete");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),                                                            
                                                            new SqlParameter("@Contents", SqlDbType.NText),
                                                            new SqlParameter("@MsgContent", SqlDbType.NVarChar,100),                                                            
                                                            new SqlParameter("@IsMsg", SqlDbType.Char,1),
                                                            new SqlParameter("@IsType", SqlDbType.Char,2),
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int),
                                                            new SqlParameter("@StartDate", SqlDbType.Char,8),
                                                            new SqlParameter("@EndDate", SqlDbType.Char,8),
                                                            new SqlParameter("@CompleteDate", SqlDbType.Char,8),
                };
                parms[0].Value = CompanyID;
                parms[1].Value = listID;
                parms[2].Value = Server.UrlDecode(content).ToReverseXSS();
                parms[3].Value = resMng.GetString("TaskComplete");
                parms[4].Value = IsMsg;
                parms[5].Value = bType;
                parms[6].Value = UserID;
                parms[7].Value = DateHelper.GetCultureDate(startDate, "yyyyMMdd", PreLangReal);
                parms[8].Value = DateHelper.GetCultureDate(endDate, "yyyyMMdd", PreLangReal);
                parms[9].Value = DateHelper.GetCultureDate(System.DateTime.Now.ToShortDateString(), "yyyyMMdd", PreLangReal);

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                if (Convert.ToInt32(obj) > 0)
                {
                    // 알람처리  
                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_QRY_TaskInfo");
                    sql = string.Format(sql, CompanyID.ToString(), listID);
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                    BE.ListInfo info = new ListInfo();
                    info.CompanyID = CompanyID;
                    info.ListID = listID.ToNumber<int>();
                    info.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                    info.RequestUserID = ds.Tables[0].Rows[0]["WriteUserID"].ToString().ToNumber<int>();
                    info.WriteUserID = UserID;
                    info.SenderName = UserInfo.Name;
                    info.PreLang = PreLang;

                    BLL.Alarm alarmBiz = new BLL.Alarm();
                    alarmBiz.AlarmEtc(info, "COMPLETE");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("AddComplete: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 완료취소
        /// 작성일자 : 2015-05-14
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string CompCancel(string p)
        {
            SetDecryptString(p);
            string bType = GetDecryptParam("BType", "0");
            string listID = GetDecryptParam("ListID", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Task.Ws.SetTask).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                // 알람처리  
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_QRY_TaskInfo");
                sql = string.Format(sql, CompanyID.ToString(), listID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                string status = "3";
                int ymd = System.DateTime.Now.ToString("yyyyMMdd").ToNumber<int>();
                int endDate = ds.Tables[0].Rows[0]["EndDate"].ToString().ToNumber<int>();
                if (endDate < ymd)
                {
                    status = "6"; //지연
                }

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "INS_PROC_TAS_Cancel");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@Contents", SqlDbType.NText),                                                            
                                                            new SqlParameter("@Status", SqlDbType.Char,1),
                                                            new SqlParameter("@IsType", SqlDbType.Char,2),
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int)
                };
                parms[0].Value = CompanyID;
                parms[1].Value = listID;
                parms[2].Value = resMng.GetString("TaskCancel");
                parms[3].Value = status;
                parms[4].Value = bType;
                parms[5].Value = UserID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                if (Convert.ToInt32(obj) > 0)
                {
                    BE.ListInfo info = new ListInfo();
                    info.CompanyID = CompanyID;
                    info.ListID = listID.ToNumber<int>();
                    info.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                    info.RequestUserID = ds.Tables[0].Rows[0]["WriteUserID"].ToString().ToNumber<int>();
                    info.WriteUserID = UserID;
                    info.SenderName = UserInfo.Name;
                    info.PreLang = PreLang;

                    BLL.Alarm alarmBiz = new BLL.Alarm();
                    alarmBiz.AlarmEtc(info, "CANCEL");


                    return status;
                }
                else
                {
                    return "";
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("CompCancel: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 재검토요청
        /// 작성일자 : 2015-05-14
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Reject(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("ListID", "");
            string content = GetDecryptParam("Content", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Task.Ws.SetTask).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string IsMsg = "1";

                if (content.Trim() != "")
                {
                    IsMsg = "0";
                }

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "INS_PROC_TAS_Reject");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),                                                            
                                                            new SqlParameter("@Contents", SqlDbType.NText),
                                                            new SqlParameter("@MsgContent", SqlDbType.NVarChar,100),                                                            
                                                            new SqlParameter("@IsMsg", SqlDbType.Char,1),                                                            
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int)
                };
                parms[0].Value = CompanyID;
                parms[1].Value = listID;
                parms[2].Value = Server.UrlDecode(content).ToReverseXSS();
                parms[3].Value = resMng.GetString("TaskReject");
                parms[4].Value = IsMsg;
                parms[5].Value = UserID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                if (Convert.ToInt32(obj) > 0)
                {
                    // 알람처리  
                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLTask, "SEL_QRY_TaskInfo");
                    sql = string.Format(sql, CompanyID.ToString(), listID);
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                    BE.ListInfo info = new ListInfo();
                    info.CompanyID = CompanyID;
                    info.ListID = listID.ToNumber<int>();
                    info.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                    info.RequestUserID = ds.Tables[0].Rows[0]["WriteUserID"].ToString().ToNumber<int>();
                    info.WriteUserID = UserID;
                    info.SenderName = UserInfo.Name;
                    info.PreLang = PreLang;

                    BLL.Alarm alarmBiz = new BLL.Alarm();
                    alarmBiz.AlarmEtc(info, "REJECT");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Reject: " + errMsg);
                throw;
            }
        }
    }
}
