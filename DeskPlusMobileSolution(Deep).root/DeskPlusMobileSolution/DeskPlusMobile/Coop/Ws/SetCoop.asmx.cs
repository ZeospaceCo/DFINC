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
using Zeospace.DeskPlusMobile.Web.Coop.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Coop.Ws
{
    /// <summary>
    /// SetCoop의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetCoop : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 답변하기
        /// 작성일자 : 2015-04-03
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool AddReply(string p)
        {
            SetDecryptString(p);
            string bPage = GetDecryptParam("BPage", "0");
            string listID = GetDecryptParam("ListID", "");
            string status = GetDecryptParam("Status", "0");            
            string content = GetDecryptParam("Content", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Coop.Ws.SetCoop).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "INS_PROC_Coop_List");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@GroupID", SqlDbType.Int),
                                                            new SqlParameter("@ListOrder", SqlDbType.Int),
                                                            new SqlParameter("@Depth", SqlDbType.Int),
                                                            new SqlParameter("@Title", SqlDbType.NVarChar,100),
                                                            new SqlParameter("@Contents", SqlDbType.NText),
                                                            new SqlParameter("@MobileContent", SqlDbType.NVarChar,4000),
                                                            new SqlParameter("@Status", SqlDbType.Char,1),
                                                            new SqlParameter("@IsAlarm", SqlDbType.Char,1),
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int)
                };
                parms[0].Value = CompanyID;
                parms[1].Value = listID;
                parms[2].Value = listID;
                parms[3].Value = 0;
                parms[4].Value = 1;
                parms[5].Value = "";
                parms[6].Value = Server.UrlDecode(content).ToReverseXSS(); 
                parms[7].Value = Server.UrlDecode(content).ToReverseXSS(); 
                parms[8].Value = status;
                parms[9].Value = "0";
                parms[10].Value = UserID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                if (Convert.ToInt32(obj) > 0)
                {
                    // 알람처리  
                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCoopInfo");
                    sql = string.Format(sql, CompanyID.ToString(), listID);
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                    string originTitle = ds.Tables[0].Rows[0]["Title"].ToString();
                    int originWriteUserID = ds.Tables[0].Rows[0]["WriteUserID"].ToString().ToNumber<int>();
                    string alarmContent = "[" + resMng.GetString("ReplyTitle") + "] " + originTitle;                    
                                    
                    if (bPage == "request")
                    {
                        // 1. 요청자 답글 알람                        
                        AlarmReplyRequest(CompanyID, listID.ToNumber<int>(), UserID, UserInfo.Name, alarmContent);
                    }
                    else if (bPage == "receive")
                    {
                        // 2. 수신자 답글 알람
                        AlarmReplyTO(CompanyID, listID.ToNumber<int>(), originWriteUserID, UserID, UserInfo.Name, alarmContent);
                    }
                    else
                    {
                        // 3. 참조자 답급 알람
                        AlarmReplyCC(CompanyID, listID.ToNumber<int>(), originWriteUserID, UserID, UserInfo.Name, alarmContent);
                    }                  

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
        /// 작성목적 : 완료취소
        /// 작성일자 : 2015-04-03
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool CompCancel(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("ListID", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Coop.Ws.SetCoop).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCoopInfo");
                sql = string.Format(sql, CompanyID.ToString(), listID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                string originTitle = ds.Tables[0].Rows[0]["Title"].ToString();

                string msg = resMng.GetString("Title") + " : '" + originTitle + "' " + UserInfo.Name + " " + resMng.GetString("MsgCancel");
                string msgcontent = @"<HTML>
<HEAD>
<META content='text/html; charset=utf-8' http-equiv=Content-Type>
<STYLE>p {margin-top:2px;margin-bottom:2px;}</STYLE>
<META name=GENERATOR content='TAGFREE Active Designer v2.5'>
</HEAD>
<BODY>" + msg + "</BODY></HTML>";

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "INS_PROC_Coop_Cancel");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@Depth", SqlDbType.Int),
                                                            new SqlParameter("@Contents", SqlDbType.NText),
                                                            new SqlParameter("@MobileContent", SqlDbType.NVarChar,4000),
                                                            new SqlParameter("@Status", SqlDbType.Char,1),
                                                            new SqlParameter("@IsAlarm", SqlDbType.Char,1),
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int)
                };
                parms[0].Value = CompanyID;
                parms[1].Value = listID;
                parms[2].Value = 1;
                parms[3].Value = msgcontent;
                parms[4].Value = msg;
                parms[5].Value = "3";
                parms[6].Value = "1";
                parms[7].Value = UserID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                if (Convert.ToInt32(obj) > 0)
                {
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
                Helper.AppException.WriteException("CompCancel: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 완료하기
        /// 작성일자 : 2015-04-03
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool AddComplete(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("ListID", "");            
            string startDate = GetDecryptParam("StartDate", "");
            string endDate = GetDecryptParam("EndDate", "");
            string difficulty = GetDecryptParam("Difficulty", "");
            string content = GetDecryptParam("Content", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Coop.Ws.SetCoop).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCoopInfo");
                sql = string.Format(sql, CompanyID.ToString(), listID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                string originTitle = ds.Tables[0].Rows[0]["Title"].ToString();
                int originWriteUserID = ds.Tables[0].Rows[0]["WriteUserID"].ToString().ToNumber<int>();

                string msg = resMng.GetString("Title") + " : '" + originTitle + "' " + UserInfo.Name + " " + resMng.GetString("MsgComplete");
                string msgcontent = @"<HTML>
<HEAD>
<META content='text/html; charset=utf-8' http-equiv=Content-Type>
<STYLE>p {margin-top:2px;margin-bottom:2px;}</STYLE>
<META name=GENERATOR content='TAGFREE Active Designer v2.5'>
</HEAD>
<BODY>" + msg + "</BODY></HTML>";

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "INS_PROC_Coop_Complete");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@Depth", SqlDbType.Int),
                                                            new SqlParameter("@Contents", SqlDbType.NText),
                                                            new SqlParameter("@MobileContent", SqlDbType.NVarChar,4000),
                                                            new SqlParameter("@MsgContents", SqlDbType.NText),
                                                            new SqlParameter("@MsgMobileContent", SqlDbType.NVarChar,4000),
                                                            new SqlParameter("@Status", SqlDbType.Char,1),
                                                            new SqlParameter("@IsAlarm", SqlDbType.Char,1),
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int),
                                                            new SqlParameter("@StartDate", SqlDbType.Char,8),
                                                            new SqlParameter("@EndDate", SqlDbType.Char,8),
                                                            new SqlParameter("@Difficultys", SqlDbType.Char,1),
                                                            new SqlParameter("@CompleteDate", SqlDbType.Char,8)
                };
                parms[0].Value = CompanyID;
                parms[1].Value = listID;
                parms[2].Value = 1;
                parms[3].Value = Server.UrlDecode(content).ToReverseXSS();
                parms[4].Value = Server.UrlDecode(content).ToReverseXSS();
                parms[5].Value = msgcontent;
                parms[6].Value = msg;
                parms[7].Value = "4";
                parms[8].Value = "1";
                parms[9].Value = UserID;
                parms[10].Value = DateHelper.GetCultureDate(startDate, "yyyyMMdd", PreLangReal);
                parms[11].Value = DateHelper.GetCultureDate(endDate, "yyyyMMdd", PreLangReal);
                parms[12].Value = difficulty;
                parms[13].Value = DateHelper.GetCultureDate(System.DateTime.Now.ToShortDateString(), "yyyyMMdd", PreLangReal);

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                if (Convert.ToInt32(obj) > 0)
                {
                    // 알람처리  
                    string alarmContent = "[" + resMng.GetString("CStatus4") + "] " + originTitle;
                    AlarmComplete(CompanyID, listID.ToNumber<int>(), originWriteUserID, UserID, UserInfo.Name, alarmContent);

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
        /// 작성목적 : 접수하기
        /// 작성일자 : 2015-04-03
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool AddAccept(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("ListID", "");
            string startDate = GetDecryptParam("StartDate", "");
            string endDate = GetDecryptParam("EndDate", "");
            string difficulty = GetDecryptParam("Difficulty", "");
            string content = GetDecryptParam("Content", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Coop.Ws.SetCoop).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string status = "2";
                string tmpStartDate = DateHelper.GetCultureDate(startDate, "yyyyMMdd", PreLangReal);
                string tmpEndDate = DateHelper.GetCultureDate(endDate, "yyyyMMdd", PreLangReal);
                //시작일자가 금일날짜보다 작을 경우, 처리중으로 변경
                int ymd = System.DateTime.Now.ToString("yyyyMMdd").ToNumber<int>();
                if (tmpStartDate.ToNumber<int>() <= ymd && tmpEndDate.ToNumber<int>() >= ymd)
                {
                    status = "3";
                }

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCoopInfo");
                sql = string.Format(sql, CompanyID.ToString(), listID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                string originTitle = ds.Tables[0].Rows[0]["Title"].ToString();
                int originWriteUserID = ds.Tables[0].Rows[0]["WriteUserID"].ToString().ToNumber<int>();

                string msg = resMng.GetString("Title") + " : '" + originTitle + "' " + UserInfo.Name + " " + resMng.GetString("MsgAccept");
                string msgcontent = @"<HTML>
<HEAD>
<META content='text/html; charset=utf-8' http-equiv=Content-Type>
<STYLE>p {margin-top:2px;margin-bottom:2px;}</STYLE>
<META name=GENERATOR content='TAGFREE Active Designer v2.5'>
</HEAD>
<BODY>" + msg + "</BODY></HTML>";

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "INS_PROC_Coop_Accept");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@Depth", SqlDbType.Int),
                                                            new SqlParameter("@Contents", SqlDbType.NText),
                                                            new SqlParameter("@MobileContent", SqlDbType.NVarChar,4000),
                                                            new SqlParameter("@MsgContents", SqlDbType.NText),
                                                            new SqlParameter("@MsgMobileContent", SqlDbType.NVarChar,4000),
                                                            new SqlParameter("@Status", SqlDbType.Char,1),
                                                            new SqlParameter("@IsAlarm", SqlDbType.Char,1),
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int),
                                                            new SqlParameter("@StartDate", SqlDbType.Char,8),
                                                            new SqlParameter("@EndDate", SqlDbType.Char,8),
                                                            new SqlParameter("@Difficultys", SqlDbType.Char,1),
                                                            new SqlParameter("@CompleteDate", SqlDbType.Char,8)
                };
                parms[0].Value = CompanyID;
                parms[1].Value = listID;
                parms[2].Value = 1;
                parms[3].Value = Server.UrlDecode(content).ToReverseXSS();
                parms[4].Value = Server.UrlDecode(content).ToReverseXSS();
                parms[5].Value = msgcontent;
                parms[6].Value = msg;
                parms[7].Value = status;
                parms[8].Value = "1";
                parms[9].Value = UserID;
                parms[10].Value = DateHelper.GetCultureDate(startDate, "yyyyMMdd", PreLangReal);
                parms[11].Value = DateHelper.GetCultureDate(endDate, "yyyyMMdd", PreLangReal);
                parms[12].Value = difficulty;
                parms[13].Value = "";

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                if (Convert.ToInt32(obj) > 0)
                {
                    // 알람처리                     
                    string alarmContent = "[" + resMng.GetString("CoopStatus2") + "] " + originTitle;
                    AlarmAccept(CompanyID, listID.ToNumber<int>(), originWriteUserID, UserID, UserInfo.Name, alarmContent);

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
        /// 작성목적 : 반려하기
        /// 작성일자 : 2015-04-03
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool Reject(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("ListID", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Coop.Ws.SetCoop).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCoopInfo");
                sql = string.Format(sql, CompanyID.ToString(), listID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                string originTitle = ds.Tables[0].Rows[0]["Title"].ToString();
                int originWriteUserID = ds.Tables[0].Rows[0]["WriteUserID"].ToString().ToNumber<int>();

                string msg = resMng.GetString("Title") + " : '" + originTitle + "' " + UserInfo.Name + " " + resMng.GetString("MsgReject");
                string msgcontent = @"<HTML>
<HEAD>
<META content='text/html; charset=utf-8' http-equiv=Content-Type>
<STYLE>p {margin-top:2px;margin-bottom:2px;}</STYLE>
<META name=GENERATOR content='TAGFREE Active Designer v2.5'>
</HEAD>
<BODY>" + msg + "</BODY></HTML>";

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "INS_PROC_Coop_Reject");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@ListID", SqlDbType.Int),
                                                            new SqlParameter("@Depth", SqlDbType.Int),
                                                            new SqlParameter("@Contents", SqlDbType.NText),
                                                            new SqlParameter("@MobileContent", SqlDbType.NVarChar,4000),
                                                            new SqlParameter("@Status", SqlDbType.Char,1),
                                                            new SqlParameter("@IsAlarm", SqlDbType.Char,1),
                                                            new SqlParameter("@WriteUserID", SqlDbType.Int)
                };
                parms[0].Value = CompanyID;
                parms[1].Value = listID;
                parms[2].Value = 1;
                parms[3].Value = msgcontent;
                parms[4].Value = msg;
                parms[5].Value = "5";
                parms[6].Value = "1";
                parms[7].Value = UserID;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, parms);

                if (Convert.ToInt32(obj) > 0)
                {
                    // 알람처리  
                    string alarmContent = "[" + resMng.GetString("CoopStatus5") + "] " + originTitle;

                    AlarmReject(CompanyID, listID.ToNumber<int>(), originWriteUserID, UserID, UserInfo.Name, alarmContent);
                                        
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
                Helper.AppException.WriteException("CompCancel: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 요청자 답글 = [수신자 + 참조자]
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_replyUserID">The _reply user ID.</param>
        /// <param name="_replyUserName">Name of the _reply user.</param>
        /// <param name="_content">The _content.</param>
        /// <returns></returns>
        public bool AlarmReplyRequest(int _companyID, int _listID, int _replyUserID, string _replyUserName, string _content)
        {
            if (_replyUserID == 0) return false;

            //수신자
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectTO");
            sql = string.Format(sql, _companyID, _listID);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsTo", null);

            string tmpTOUser = "";

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tmpTOUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpTOUser = tmpTOUser.TrimEnd(',');

            List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
            IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisTO)
            {
                SendAlarm(_companyID, _replyUserID, _replyUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
            }
            

            //참조자
            string sqlcc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCC");
            sqlcc = string.Format(sqlcc, _companyID, _listID);
            DataSet dscc = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlcc, "dsCc", null);

            string tmpCCUser = "";
            if (dscc != null)
            {
                foreach (DataRow dr in dscc.Tables[0].Rows)
                {
                    tmpCCUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpCCUser = tmpCCUser.TrimEnd(',');

            List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
            IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisCC)
            {
                if (userID.Length > 0)
                {
                    SendAlarm(_companyID, _replyUserID, _replyUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }
            

            return true;
        }


        /// <summary>
        /// 수신자 답글 = [요청자 + 수신자(답글작성자제외) + 참조자]
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_replyUserID">The _reply user ID.</param>
        /// <param name="_replyUserName">Name of the _reply user.</param>
        /// <param name="_content">The _content.</param>
        /// <returns></returns>
        public bool AlarmReplyTO(int _companyID, int _listID, int _writeUserID, int _replyUserID, string _replyUserName, string _content)
        {
            if (_writeUserID == 0 || _replyUserID == 0) return false;

            //요청자
            SendAlarm(_companyID, _replyUserID, _replyUserName, _content, _content, _listID, _listID, _writeUserID, 0);

            //수신자
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectTO");
            sql = string.Format(sql, _companyID, _listID);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsTo", null);

            string tmpTOUser = "";

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tmpTOUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpTOUser = tmpTOUser.TrimEnd(',');

            List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
            IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisTO)
            {
                //답글작성자제외
                if (_replyUserID != userID.ToNumber<int>() && userID.Length > 0)
                {
                    SendAlarm(_companyID, _replyUserID, _replyUserName,
                            _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            //참조자
            string sqlcc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCC");
            sqlcc = string.Format(sqlcc, _companyID, _listID);
            DataSet dscc = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlcc, "dsCc", null);

            string tmpCCUser = "";
            if (dscc != null)
            {
                foreach (DataRow dr in dscc.Tables[0].Rows)
                {
                    tmpCCUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpCCUser = tmpCCUser.TrimEnd(',');

            List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
            IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisCC)
            {
                if (userID.Length > 0)
                {
                    SendAlarm(_companyID, _replyUserID, _replyUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            return true;
        }

        /// <summary>
        /// 참조자 답글 = [요청자 + 수신자 + 참조자(작성자제외)]
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_replyUserID">The _reply user ID.</param>
        /// <param name="_replyUserName">Name of the _reply user.</param>
        /// <param name="_content">The _content.</param>
        /// <returns></returns>
        public bool AlarmReplyCC(int _companyID, int _listID, int _writeUserID, int _replyUserID, string _replyUserName, string _content)
        {
            if (_writeUserID == 0 || _replyUserID == 0) return false;

            //요청자
            SendAlarm(_companyID, _replyUserID, _replyUserName, _content, _content, _listID, _listID, _writeUserID, 0);

            //수신자
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectTO");
            sql = string.Format(sql, _companyID, _listID);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsTo", null);

            string tmpTOUser = "";

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tmpTOUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpTOUser = tmpTOUser.TrimEnd(',');

            List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
            IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisTO)
            {
                SendAlarm(_companyID, _replyUserID, _replyUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
            }

            //참조자
            string sqlcc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCC");
            sqlcc = string.Format(sqlcc, _companyID, _listID);
            DataSet dscc = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlcc, "dsCc", null);

            string tmpCCUser = "";
            if (dscc != null)
            {
                foreach (DataRow dr in dscc.Tables[0].Rows)
                {
                    tmpCCUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpCCUser = tmpCCUser.TrimEnd(',');

            List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
            IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisCC)
            {
                //참조자 중, 답변 작성자는 제외한다.
                if (_replyUserID != userID.ToNumber<int>() && userID.Length > 0)
                {
                    SendAlarm(_companyID, _replyUserID, _replyUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            return true;
        }


        /// <summary>
        /// 완료 = 요청자 + 수신자(완료자 제외) + 참조자
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_acceptUserID">The _accept user ID.</param>
        /// <param name="_acceptUserName">Name of the _accept user.</param>
        /// <param name="_content">The _content.</param>
        public bool AlarmComplete(int _companyID, int _listID, int _writeUserID, int _completeUserID, string _completeUserName, string _content)
        {
            if (_writeUserID == 0 || _completeUserID == 0) return false;

            //요청자
            SendAlarm(_companyID, _completeUserID, _completeUserName, _content, _content, _listID, _listID, _writeUserID, 0);

            //수신자
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectTO");
            sql = string.Format(sql, _companyID, _listID);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsTo", null);

            string tmpTOUser = "";

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tmpTOUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpTOUser = tmpTOUser.TrimEnd(',');

            List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
            IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisTO)
            {
                //완료자는 제외한다.
                if (_completeUserID != userID.ToNumber<int>() && userID.Length > 0)
                {
                    SendAlarm(_companyID, _completeUserID, _completeUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            //참조자
            string sqlcc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCC");
            sqlcc = string.Format(sqlcc, _companyID, _listID);
            DataSet dscc = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlcc, "dsCc", null);

            string tmpCCUser = "";
            if (dscc != null)
            {
                foreach (DataRow dr in dscc.Tables[0].Rows)
                {
                    tmpCCUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpCCUser = tmpCCUser.TrimEnd(',');

            List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
            IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisCC)
            {
                if (userID.Length > 0)
                {
                    SendAlarm(_companyID, _completeUserID, _completeUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            return true;
        }


        /// <summary>
        /// 접수 = 요청자 + 수신자(접수자 제외) + 참조자
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_acceptUserID">The _accept user ID.</param>
        /// <param name="_content">The _content.</param>
        public bool AlarmAccept(int _companyID, int _listID, int _writeUserID, int _acceptUserID, string _acceptUserName, string _content)
        {
            if (_writeUserID == 0 || _acceptUserID == 0) return false;
           
            //요청자
            SendAlarm(_companyID, _acceptUserID, _acceptUserName, _content, _content, _listID, _listID, _writeUserID, 0);

            //수신자
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectTO");
            sql = string.Format(sql, _companyID, _listID);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsTo", null);

            string tmpTOUser = "";

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tmpTOUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpTOUser = tmpTOUser.TrimEnd(',');

            List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
            IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisTO)
            {
                //완료자는 제외한다.
                if (_acceptUserID != userID.ToNumber<int>() && userID.Length > 0)
                {
                    SendAlarm(_companyID, _acceptUserID, _acceptUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            //참조자
            string sqlcc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCC");
            sqlcc = string.Format(sqlcc, _companyID, _listID);
            DataSet dscc = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlcc, "dsCc", null);

            string tmpCCUser = "";
            if (dscc != null)
            {
                foreach (DataRow dr in dscc.Tables[0].Rows)
                {
                    tmpCCUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpCCUser = tmpCCUser.TrimEnd(',');

            List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
            IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisCC)
            {
                if (userID.Length > 0)
                {
                    SendAlarm(_companyID, _acceptUserID, _acceptUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }


            return true;
        }

        /// <summary>
        /// 반려 =  요청자 + 수신자(반려자 제외) + 참조자
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_rejectUserID">The _reject user ID.</param>
        /// <param name="_content">The _content.</param>
        public bool AlarmReject(int _companyID, int _listID, int _writeUserID, int _rejectUserID, string _rejectUserName, string _content)
        {
            if (_writeUserID == 0 || _rejectUserID == 0) return false;

            //요청자
            SendAlarm(_companyID, _rejectUserID, _rejectUserName, _content, _content, _listID, _listID, _writeUserID, 0);
            
            //수신자
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectTO");
            sql = string.Format(sql, _companyID, _listID);
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsTo", null);

            string tmpTOUser = "";

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tmpTOUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpTOUser = tmpTOUser.TrimEnd(',');

            List<string> userTOList = tmpTOUser.Split(',').ToList<string>();
            IEnumerable<string> userDisTO = userTOList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisTO)
            {
                //반려자는 제외한다.
                if (_rejectUserID != userID.ToNumber<int>() && userID.Length > 0)
                {
                    SendAlarm(_companyID, _rejectUserID, _rejectUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            //참조자
            string sqlcc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCoop, "SEL_QRY_SelectCC");
            sqlcc = string.Format(sqlcc, _companyID, _listID);
            DataSet dscc = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlcc, "dsCc", null);

            string tmpCCUser = "";
            if (dscc != null)
            {
                foreach (DataRow dr in dscc.Tables[0].Rows)
                {
                    tmpCCUser += dr["UserID"].ToString() + ",";
                }
            }
            tmpCCUser = tmpCCUser.TrimEnd(',');

            List<string> userCCList = tmpCCUser.Split(',').ToList<string>();
            IEnumerable<string> userDisCC = userCCList.Distinct(); //사용자 중복제거

            foreach (string userID in userDisCC)
            {
                if (userID.Length > 0)
                {
                    SendAlarm(_companyID, _rejectUserID, _rejectUserName,
                        _content, _content, _listID, _listID, userID.ToNumber<int>(), 0);
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_senderUserID"></param>
        /// <param name="_senderName"></param>
        /// <param name="_subject"></param>
        /// <param name="_contents"></param>
        /// <param name="_listID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_forwardListID"></param>
        private void SendAlarm(int _companyID, int _senderUserID, string _senderName, string _subject,
            string _contents, int _listID, int _groupID, int _receiverID, int _forwardListID)
        {
            
            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
            alarmInfo.CompanyID = _companyID;
            alarmInfo.SenderID = _senderUserID;
            alarmInfo.SenderName = _senderName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _subject.ToReverseXSS();
            alarmInfo.Content = _contents.ToReverseXSS();

            string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string alarmTargetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Coop, "AlarmTargetURL");

            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("listID", _listID.ToString());
            paraDic.Add("groupID", _groupID.ToString());
            if (_forwardListID > 0) paraDic.Add("forwardListID", _forwardListID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Coop";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm
            = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();
            alarm.Add(alarmInfo);

        }
    }
}
