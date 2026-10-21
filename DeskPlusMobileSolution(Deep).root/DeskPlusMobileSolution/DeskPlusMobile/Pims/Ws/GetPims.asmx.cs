using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Reflection;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.Pims.Ws
{
    /// <summary>
    /// GetPims의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetPims : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정관리 스케줄 가지고 오기
        /// 작성일자 : 2011-06-16
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetScheduleList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Pims.Ws.GetPims).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            SetDecryptString(p);

            string tmpDate = GetDecryptParam("TmpDate", "0");
            string userID = GetDecryptParam("UserID", "0");
            string isOpen = GetDecryptParam("IsOpen", "");

            string pimsType = "";
            if (UserEnv != null)
            {
                pimsType = UserEnv;
            }
            else
            {
                pimsType = "0|0|0|0|";
            }

            string[] pimTypeArr = pimsType.Split('|');

            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@TmpDate", SqlDbType.VarChar,10)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@IsOpen", SqlDbType.Char,1)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = tmpDate;
                parm[2].Value = userID;
                parm[3].Value = isOpen;

                if (isOpen != "Y")
                {
                    if (pimsType.IndexOf("1") > -1)
                    {
                        if (pimTypeArr[0] == "1")
                        {
                            //개인일정
                            string proc0 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC0");
                            DataSet ds0 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc0, "Schedule", parm);
                            ds.Merge(ds0);
                        }

                        if (pimTypeArr[1] == "1")
                        {
                            //부서일정
                            string proc1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC1");
                            DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc1, "Schedule", parm);
                            ds.Merge(ds1);
                        }

                        if (pimTypeArr[2] == "1")
                        {
                            //공유일정
                            string proc2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC2");
                            DataSet ds2 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc2, "Schedule", parm);
                            ds.Merge(ds2);
                        }

                        if (pimTypeArr[3] == "1")
                        {
                            //전체일정
                            string proc3 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC3");
                            DataSet ds3 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc3, "Schedule", parm);
                            ds.Merge(ds3);
                        }
                    }
                    else
                    {
                        string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC");
                        ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "Schedule", parm);
                    }
                }
                else
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETCSHARE");
                    ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "Schedule", parm);
                }

                var sqlAccQuery = "WITH TmpTBL AS  ";
                sqlAccQuery += "(SELECT Row_Number() over (ORDER BY titleorder, dutyorder,userdeptorder DESC, NameBase, NameKo,NameEn,NameJa,NameFr,NameZhCn,NameZhTw) AS RowNum ";
                sqlAccQuery += " ,KeyID as OwnerID";
                sqlAccQuery += " FROM VW_ADD_Tree WITH(NOLOCK) WHERE Companyid = " + CompanyID + " AND Type IN ('GM')   )  SELECT MIN(RowNum) RowNum , OwnerID FROM TmpTBL GROUP BY OwnerID";

                DataSet dsAccount = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlAccQuery, "SEL_QRY_USER", null);

                var query = from c in ds.Tables[0].AsEnumerable()
                            join b in dsAccount.Tables[0].AsEnumerable()
                            on c.Field<int>("UserID") equals b.Field<int>("OwnerID")
                            orderby c.Field<int>("UserID").ToString() == userID ? 0 : 1, b.Field<int>("OwnerID")
                            select c;

                DataTable dtAccount = query.CopyToDataTable();

                DataView dv = dtAccount.DefaultView;

                //DataView dv = ds.Tables[0].DefaultView;
                //dv.Sort = "StartTime, ScheduleID";

                List<object> listobj = new List<object>();

                int tmpScheduleID = 0;

                if (dv.Table.Rows.Count > 0)
                {
                    for (int i = 0; i < dv.Table.Rows.Count; i++)
                    {
                        if (tmpScheduleID != Convert.ToInt32(dv[i]["ScheduleID"].ToString()))
                        {
                            BE.ScheduleInfo info = new BE.ScheduleInfo();

                            info.ScheduleID = Convert.ToInt32(dv[i]["ScheduleID"].ToString());
                            tmpScheduleID = info.ScheduleID;
                            info.Title = dv[i]["Title"].ToString();
                            info.StartTime = Convert.ToDateTime(dv[i]["StartTime"].ToString()).ToShortTimeString();
                            info.EndTime = Convert.ToDateTime(dv[i]["EndTime"].ToString()).ToShortTimeString();
                            info.UserID = Convert.ToInt32(dv[i]["UserID"].ToString());
                            info.OpenType = Convert.ToInt32(dv[i]["OpenType"].ToString());

                            switch (info.OpenType)
                            {
                                case 0:
                                    info.OpenTypeStr = resMng.GetString("SOpenType0");
                                    break;
                                case 1:
                                    info.OpenTypeStr = resMng.GetString("SOpenType1");
                                    break;
                                case 2:
                                    info.OpenTypeStr = resMng.GetString("SOpenType2");
                                    break;
                                case 3:
                                    info.OpenTypeStr = resMng.GetString("SOpenType3");
                                    break;
                                default:
                                    info.OpenTypeStr = "";
                                    break;
                            }

                            info.DisplayID = Convert.ToInt32(dv[i]["DisplayID"].ToString());

                            string Name = this.GetLangName(dv[i]["NameBase"].ToString(), dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(),
                                   dv[i]["NameJa"].ToString(), dv[i]["NameFr"].ToString(), dv[i]["NameZhcn"].ToString(), dv[i]["NameZhtw"].ToString(), PreLang);

                            if (Name != "")
                            {
                                if (info.UserID != userID.ToNumber<int>())
                                {
                                    string TitleName = this.GetLangName(dv[i]["TitleBase"].ToString(), dv[i]["TitleKo"].ToString(), dv[i]["TitleEn"].ToString(),
                                           dv[i]["TitleJa"].ToString(), dv[i]["TitleFr"].ToString(), dv[i]["TitleZhcn"].ToString(), dv[i]["TitleZhtw"].ToString(), PreLang);
                                    string DeptName = this.GetLangName(dv[i]["DeptBase"].ToString(), dv[i]["DeptKo"].ToString(), dv[i]["DeptEn"].ToString(),
                                               dv[i]["DeptJa"].ToString(), dv[i]["DeptFr"].ToString(), dv[i]["DeptZhcn"].ToString(), dv[i]["DeptZhtw"].ToString(), PreLang);

                                    info.UserInfo = Name + "/" + TitleName + "/" + DeptName;
                                }
                                else
                                {
                                    info.UserInfo = "";
                                }
                            }
                            else
                            {
                                info.UserInfo = "";
                            }

                            //댓글 카운트
                            if ((dv[i]["ReplyCnt"].ToString() == "0") || (dv[i]["ReplyCnt"].ToString() == "") || (dv[i]["ReplyCnt"].ToString() == null))
                            {
                                info.ReplyCnt = "";
                            }
                            else
                            {
                                info.ReplyCnt = "[" + dv[i]["ReplyCnt"].ToString() + "]";
                            }

                            var status = GetStatus(dv[i]["StatusID"].ToString().ToNumber<int>());
                            info.Status = "";
                            if (dv[i]["StatusID"].ToString() != "0" && status.Length > 0)
                            {
                                info.Status = "<span class='bg-success text-white'" + (dv[i]["Colors"].ToString().Length > 0 ? "style='background-color:" + dv[i]["Colors"] + "!important;'" : "") + ">&nbsp;" + status + "&nbsp;</span>";
                            }

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetScheduleList()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정관리 스케줄 가지고 오기
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetScheduleContent(string p)
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);
            string scheduleID = GetDecryptParam("ScheduleID", "0");
            string displayID = GetDecryptParam("DisplayID", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_SCHEDULE_SCHEDULEID");
            sqlQuery = String.Format(sqlQuery, CompanyID, scheduleID, displayID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetContent", null);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataColumn dc0 = new DataColumn("bodyNoStyle", typeof(string));
                    DataColumn dc1 = new DataColumn("StartShortTime", typeof(string));
                    DataColumn dc2 = new DataColumn("EndShortTime", typeof(string));
                    DataColumn dc3 = new DataColumn("Status", typeof(string));
                    HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                    string scheduleContent = ds.Tables[0].Rows[0]["Content"].ToString().Trim().ToEditorXSS();

                    /*
                    scheduleContent = Regex.Replace(scheduleContent, "<script", "<xscript", RegexOptions.IgnoreCase);
                    scheduleContent = Regex.Replace(scheduleContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
                    scheduleContent = Regex.Replace(scheduleContent, "<form", "<xform", RegexOptions.IgnoreCase);
                    scheduleContent = Regex.Replace(scheduleContent, "</form>", "</xform>", RegexOptions.IgnoreCase);
                    scheduleContent = Regex.Replace(scheduleContent, "<v:imagedata", "<IMG", RegexOptions.IgnoreCase);
                    scheduleContent = Regex.Replace(scheduleContent, "</v:imagedata>", "", RegexOptions.IgnoreCase);

                    //본문안의 Style 제거
                    scheduleContent = Regex.Replace(scheduleContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    scheduleContent = Regex.Replace(scheduleContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    scheduleContent = Regex.Replace(scheduleContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);
                    */

                    hdoc.LoadHtml(scheduleContent);
                    var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

                    if (bodyNode != null)
                    {
                        dc0.DefaultValue = bodyNode.InnerHtml;
                    }
                    else
                    {
                        dc0.DefaultValue = scheduleContent;
                    }
                    dt.Columns.Add(dc0);

                    dc1.DefaultValue = ds.Tables[0].Rows[0]["StartTime"].ToString();
                    dt.Columns.Add(dc1);

                    dc2.DefaultValue = ds.Tables[0].Rows[0]["EndTime"].ToString();
                    dt.Columns.Add(dc2);

                    var status = GetStatus(ds.Tables[0].Rows[0]["StatusID"].ToString().ToNumber<int>());
                    dc3.DefaultValue = "";
                    if (ds.Tables[0].Rows[0]["StatusID"].ToString() != "0" && status.Length > 0)
                    {
                        dc3.DefaultValue = "<span class='bg-success text-white'" + (ds.Tables[0].Rows[0]["Colors"].ToString().Length > 0 ? "style='background-color:" + ds.Tables[0].Rows[0]["Colors"] + "!important;'" : "") + ">&nbsp;" + status + "&nbsp;</span>";
                    }

                    dt.Columns.Add(dc3);

                    return StringHelper.GetJSONString(dt);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

            //return StringHelper.GetJSONString(ds.Tables[0]);
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정분류 가지고 오기
        /// 작성일자 : 2012-11-05
        /// </summary>
        /// <returns></returns>      
        private string GetStatus(int _statusID)
        {
            string rtnVal = String.Empty;

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_StatusID");
            sqlQuery = String.Format(sqlQuery, CompanyID, _statusID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetStatusID", null);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    rtnVal = this.GetLangName(
                                        dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                        dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                        dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                        dr["NameZhtw"].ToString().ToNull(), PreLang);
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 일정관리 스케줄 가지고 오기 수정시 본문에 HTML 제거
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetScheduleContentStripHTML(string p)
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);
            string scheduleID = GetDecryptParam("ScheduleID", "0");
            string displayID = GetDecryptParam("DisplayID", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_SCHEDULE_SCHEDULEID");
            sqlQuery = String.Format(sqlQuery, CompanyID, scheduleID, displayID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetContent", null);

            //모바일에서는 Body 태그의 Text 값만을 사용
            DataTable dt = ds.Tables[0];
            DataColumn dc0 = new DataColumn("bodyNoStyle", typeof(string));
            DataColumn dc1 = new DataColumn("StartShortTime", typeof(string));
            DataColumn dc2 = new DataColumn("EndShortTime", typeof(string));
            DataColumn dc3 = new DataColumn("Subject", typeof(string));

            HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();

            string scheduleContent = ds.Tables[0].Rows[0]["Content"].ToString().Trim().ToReverseXSS();

            /*
            scheduleContent = Regex.Replace(scheduleContent, "<script", "<xscript", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "<form", "<xform", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "</form>", "</xform>", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "<v:imagedata", "<IMG", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "</v:imagedata>", "", RegexOptions.IgnoreCase);

            //메일 본문안의 Style 제거
            scheduleContent = Regex.Replace(scheduleContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);
            */

            hdoc.LoadHtml(scheduleContent);
            var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

            if (bodyNode != null)
            {
                dc0.DefaultValue = bodyNode.InnerText.Replace("\r\n", "");
            }
            else
            {
                dc0.DefaultValue = scheduleContent;
            }
            dt.Columns.Add(dc0);

            dc1.DefaultValue = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartTime"].ToString()).ToShortTimeString();
            dt.Columns.Add(dc1);

            dc2.DefaultValue = Convert.ToDateTime(ds.Tables[0].Rows[0]["EndTime"].ToString()).ToShortTimeString();
            dt.Columns.Add(dc2);

            dc3.DefaultValue = ds.Tables[0].Rows[0]["Title"].ToString().Trim().ToReverseXSS();
            dt.Columns.Add(dc3);

            return StringHelper.GetJSONString(dt);
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정관리 할일 가지고 오기
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetToDoList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Pims.Ws.GetPims).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            SetDecryptString(p);

            string year = GetDecryptParam("Year", "0");
            string month = GetDecryptParam("Month", "0");
            string day = GetDecryptParam("Day", "0");
            string userID = GetDecryptParam("UserID", "0");
            string isOpen = GetDecryptParam("IsOpen", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Pims.Ws.GetPims).Assembly;	//어셈블리 선언            
            this.GetResourceManager();

            string pimsType = "";
            if (UserEnv != null)
            {
                pimsType = UserEnv;
            }
            else
            {
                pimsType = "0|0|0|0|";
            }

            string[] pimTypeArr = pimsType.Split('|');

            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@year", SqlDbType.Int)
                                            ,new SqlParameter("@month", SqlDbType.Int)
                                            ,new SqlParameter("@day", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@IsOpen", SqlDbType.Char,1)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = year;
                parm[2].Value = month;
                parm[3].Value = day;
                parm[4].Value = userID;
                parm[5].Value = isOpen;

                if (isOpen != "Y")
                {
                    if (pimsType.IndexOf("1") > -1)
                    {
                        if (pimTypeArr[0] == "1")
                        {
                            //개인할일
                            string proc0 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC0");
                            DataSet ds0 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc0, "ToDo", parm);
                            ds.Merge(ds0);
                        }

                        if (pimTypeArr[1] == "1")
                        {
                            //부서할일
                            string proc1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC1");
                            DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc1, "ToDo", parm);
                            ds.Merge(ds1);
                        }

                        if (pimTypeArr[2] == "1")
                        {
                            //공유할일
                            string proc2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC2");
                            DataSet ds2 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc2, "ToDo", parm);
                            ds.Merge(ds2);
                        }

                        if (pimTypeArr[3] == "1")
                        {
                            //전체할일
                            string proc3 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC3");
                            DataSet ds3 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc3, "ToDo", parm);
                            ds.Merge(ds3);
                        }
                    }
                    else
                    {
                        string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC");
                        ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "ToDo", parm);
                    }
                }
                else
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETCSHARE");
                    ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "ToDo", parm);
                }

                var sqlAccQuery = "WITH TmpTBL AS  ";
                sqlAccQuery += "(SELECT Row_Number() over (ORDER BY titleorder, dutyorder,userdeptorder DESC, NameBase, NameKo,NameEn,NameJa,NameFr,NameZhCn,NameZhTw) AS RowNum ";
                sqlAccQuery += " ,KeyID as OwnerID";
                sqlAccQuery += " FROM VW_ADD_Tree WITH(NOLOCK) WHERE Companyid = " + CompanyID + " AND Type IN ('GM')   )  SELECT MIN(RowNum) RowNum , OwnerID FROM TmpTBL GROUP BY OwnerID";

                DataSet dsAccount = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlAccQuery, "SEL_QRY_USER", null);

                var query = from c in ds.Tables[0].AsEnumerable()
                            join b in dsAccount.Tables[0].AsEnumerable()
                            on c.Field<int>("UserID") equals b.Field<int>("OwnerID")
                            orderby c.Field<int>("UserID").ToString() == userID ? 0 : 1, b.Field<int>("OwnerID")
                            select c;

                DataTable dtAccount = query.CopyToDataTable();

                List<object> listobj = new List<object>();
                foreach (DataRow dr in dtAccount.Rows)
                {
                    BE.ToDoInfo info = new BE.ToDoInfo();

                    info.ToDoID = Convert.ToInt32(dr["ToDoID"].ToString());
                    info.UserID = Convert.ToInt32(dr["UserID"].ToString());
                    info.OpenType = Convert.ToInt32(dr["OpenType"].ToString());

                    switch (info.OpenType)
                    {
                        case 0:
                            info.OpenTypeStr = resMng.GetString("TOpenType0");
                            break;
                        case 1:
                            info.OpenTypeStr = resMng.GetString("TOpenType1");
                            break;
                        case 2:
                            info.OpenTypeStr = resMng.GetString("TOpenType2");
                            break;
                        case 3:
                            info.OpenTypeStr = resMng.GetString("TOpenType3");
                            break;
                        default:
                            info.OpenTypeStr = "";
                            break;
                    }

                    if (dr["Status"].ToString() == "W")
                    {
                        info.Status = resMng.GetString("Status1");
                    }
                    else if (dr["Status"].ToString() == "I")
                    {
                        info.Status = resMng.GetString("Status2");
                    }
                    else if (dr["Status"].ToString() == "E")
                    {
                        info.Status = resMng.GetString("Status3");
                    }
                    else if (dr["Status"].ToString() == "C")
                    {
                        info.Status = resMng.GetString("Status4");
                    }
                    else if (dr["Status"].ToString() == "T")
                    {
                        info.Status = resMng.GetString("Status5");
                    }
                    else if (dr["Status"].ToString() == "D")
                    {
                        info.Status = resMng.GetString("Status6");
                    }

                    if (dr["Priority"].ToString() == "1")
                    {
                        info.Priority = resMng.GetString("Priority1");
                    }
                    else if (dr["Priority"].ToString() == "2")
                    {
                        info.Priority = resMng.GetString("Priority2");
                    }
                    else if (dr["Priority"].ToString() == "3")
                    {
                        info.Priority = resMng.GetString("Priority3");
                    }

                    info.Title = dr["Title"].ToString();

                    if (dr["ScheduleYN"].ToString() == "Y")
                    {
                        info.ScheduleYN = resMng.GetString("ScheduleToDo");
                    }
                    else
                    {
                        info.ScheduleYN = "";
                    }

                    string Name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                   dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                    if (Name != "")
                    {
                        if (info.UserID != userID.ToNumber<int>())
                        {
                            string TitleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                                   dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                            string DeptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                                       dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                            info.UserInfo = Name + "/" + TitleName + "/" + DeptName;
                        }
                        else
                        {
                            info.UserInfo = "";
                        }
                    }
                    else
                    {
                        info.UserInfo = "";
                    }

                    //댓글 카운트
                    if ((dr["ReplyCnt"].ToString() == "0") || (dr["ReplyCnt"].ToString() == "") || (dr["ReplyCnt"].ToString() == null))
                    {
                        info.ReplyCnt = "";
                    }
                    else
                    {
                        info.ReplyCnt = "[" + dr["ReplyCnt"].ToString() + "]";
                    }

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetToDoList()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정관리 할일 내용 가져오기
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetToDoContent(string p)
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);
            string toDoID = GetDecryptParam("ToDoID", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_TODO_TODOID");
            sqlQuery = String.Format(sqlQuery, CompanyID, toDoID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetToDoContent", null);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataColumn dc0 = new DataColumn("bodyNoStyle", typeof(string));
                    DataColumn dc1 = new DataColumn("StartShortTime", typeof(string));

                    HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                    string toDoContent = ds.Tables[0].Rows[0]["Content"].ToString().Trim().ToEditorXSS();

                    /*
                    toDoContent = Regex.Replace(toDoContent, "<script", "<xscript", RegexOptions.IgnoreCase);
                    toDoContent = Regex.Replace(toDoContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
                    toDoContent = Regex.Replace(toDoContent, "<form", "<xform", RegexOptions.IgnoreCase);
                    toDoContent = Regex.Replace(toDoContent, "</form>", "</xform>", RegexOptions.IgnoreCase);
                    toDoContent = Regex.Replace(toDoContent, "<v:imagedata", "<IMG", RegexOptions.IgnoreCase);
                    toDoContent = Regex.Replace(toDoContent, "</v:imagedata>", "", RegexOptions.IgnoreCase);

                    //메일 본문안의 Style 제거
                    toDoContent = Regex.Replace(toDoContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    toDoContent = Regex.Replace(toDoContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    toDoContent = Regex.Replace(toDoContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);
                    */

                    hdoc.LoadHtml(toDoContent);
                    var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출
                    if (bodyNode != null)
                    {
                        dc0.DefaultValue = bodyNode.InnerHtml;
                    }
                    else
                    {
                        dc0.DefaultValue = toDoContent;
                    }
                    dt.Columns.Add(dc0);

                    dc1.DefaultValue = DateHelpers.GetCultureDate(Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()), UserInfo.PreferredLanguage).Substring(0, 10);
                    dt.Columns.Add(dc1);

                    return StringHelper.GetJSONString(dt);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 일정관리 할일 가지고 오기 수정시 본문에 HTML 제거
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetToDoContentStripHTML(string p)
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);
            string toDoID = GetDecryptParam("ToDoID", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_TODO_TODOID");
            sqlQuery = String.Format(sqlQuery, CompanyID, toDoID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetToDoContent", null);

            //모바일에서는 Body 태그의 Text 값만을 사용
            DataTable dt = ds.Tables[0];
            DataColumn dc0 = new DataColumn("bodyNoStyle", typeof(string));
            DataColumn dc1 = new DataColumn("Subject", typeof(string));
            HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();

            string scheduleContent = ds.Tables[0].Rows[0]["Content"].ToString().Trim().ToReverseXSS();

            /*
            scheduleContent = Regex.Replace(scheduleContent, "<script", "<xscript", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "<form", "<xform", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "</form>", "</xform>", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "<v:imagedata", "<IMG", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "</v:imagedata>", "", RegexOptions.IgnoreCase);

            //메일 본문안의 Style 제거
            scheduleContent = Regex.Replace(scheduleContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
            scheduleContent = Regex.Replace(scheduleContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);
            */

            hdoc.LoadHtml(scheduleContent);
            var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

            if (bodyNode != null)
            {
                dc0.DefaultValue = bodyNode.InnerText.Replace("\r\n", "");
            }
            else
            {
                dc0.DefaultValue = scheduleContent;
            }
            dt.Columns.Add(dc0);

            dc1.DefaultValue = ds.Tables[0].Rows[0]["Title"].ToString().Trim().ToReverseXSS();
            dt.Columns.Add(dc1);

            return StringHelper.GetJSONString(dt);
        }

        /// <summary>
        /// 작성자   : 이중호
        /// 작성목적 : 일정관리 스케줄 할일 가지고 오기
        /// 작성일자 : 2019-04-30
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetScheduleToDoList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.Ws.GetPortal).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);

            string userID = GetDecryptParam("UserID", "0");
            string isOpen = GetDecryptParam("IsOpen", "");
            string year = GetDecryptParam("Year", "0");
            string month = GetDecryptParam("Month", "0");
            string day = GetDecryptParam("Day", "0");

            string tmpDate = year + "-" + month + "-" + day;
            if (year == "0" || month == "0" || day == "0") tmpDate = "0";

            string pimsType = "";
            if (UserEnv != null)
            {
                pimsType = UserEnv;
            }
            else
            {
                pimsType = "0|0|0|0|";
            }

            string[] pimTypeArr = pimsType.Split('|');

            try
            {
                DataSet dsSch = new DataSet();
                SqlParameter[] parm1 = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@TmpDate", SqlDbType.VarChar,10)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@IsOpen", SqlDbType.Char,1)
                };
                parm1[0].Value = CompanyID;
                parm1[1].Value = tmpDate;
                parm1[2].Value = userID;
                parm1[3].Value = isOpen;

                if (isOpen != "Y")
                {
                    if (pimsType.IndexOf("1") > -1)
                    {
                        if (pimTypeArr[0] == "1")
                        {
                            //개인일정
                            string proc0 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC0");
                            DataSet ds0 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc0, "Schedule", parm1);
                            dsSch.Merge(ds0);
                        }

                        if (pimTypeArr[1] == "1")
                        {
                            //부서일정
                            string proc1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC1");
                            DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc1, "Schedule", parm1);
                            dsSch.Merge(ds1);
                        }

                        if (pimTypeArr[2] == "1")
                        {
                            //공유일정
                            string proc2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC2");
                            DataSet ds2 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc2, "Schedule", parm1);
                            dsSch.Merge(ds2);
                        }

                        if (pimTypeArr[3] == "1")
                        {
                            //전체일정
                            string proc3 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC3");
                            DataSet ds3 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc3, "Schedule", parm1);
                            dsSch.Merge(ds3);
                        }
                    }
                    else
                    {
                        string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC");
                        dsSch = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "Schedule", parm1);
                    }
                }
                else
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC");
                    dsSch = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "Schedule", parm1);
                }

                DataView dv1 = dsSch.Tables[0].DefaultView;

                List<BE.ScheduleInfo> listobj1 = new List<BE.ScheduleInfo>();

                int tmpScheduleID = 0;

                if (dv1.Table.Rows.Count > 0)
                {
                    for (int i = 0; i < dv1.Table.Rows.Count; i++)
                    {
                        if (tmpScheduleID != Convert.ToInt32(dv1[i]["ScheduleID"].ToString()))
                        {
                            BE.ScheduleInfo info = new BE.ScheduleInfo();

                            info.ScheduleID = Convert.ToInt32(dv1[i]["ScheduleID"].ToString());
                            tmpScheduleID = info.ScheduleID;
                            info.Title = dv1[i]["Title"].ToString();
                            info.StartTime = Convert.ToDateTime(dv1[i]["StartTime"].ToString()).ToShortTimeString();
                            info.EndTime = Convert.ToDateTime(dv1[i]["EndTime"].ToString()).ToShortTimeString();
                            info.UserID = Convert.ToInt32(dv1[i]["UserID"].ToString());
                            info.OpenType = Convert.ToInt32(dv1[i]["OpenType"].ToString());

                            switch (info.OpenType)
                            {
                                case 0:
                                    info.OpenTypeStr = resMng.GetString("SOpenType0");
                                    break;

                                case 1:
                                    info.OpenTypeStr = resMng.GetString("SOpenType1");
                                    break;

                                case 2:
                                    info.OpenTypeStr = resMng.GetString("SOpenType2");
                                    break;

                                case 3:
                                    info.OpenTypeStr = resMng.GetString("SOpenType3");
                                    break;

                                default:
                                    info.OpenTypeStr = "";
                                    break;
                            }

                            info.DisplayID = Convert.ToInt32(dv1[i]["DisplayID"].ToString());

                            string Name = this.GetLangName(dv1[i]["NameBase"].ToString(), dv1[i]["NameKo"].ToString(), dv1[i]["NameEn"].ToString(),
                                   dv1[i]["NameJa"].ToString(), dv1[i]["NameFr"].ToString(), dv1[i]["NameZhcn"].ToString(), dv1[i]["NameZhtw"].ToString(), PreLang);

                            if (Name != "")
                            {
                                if (info.UserID != userID.ToNumber<int>())
                                {
                                    string TitleName = this.GetLangName(dv1[i]["TitleBase"].ToString(), dv1[i]["TitleKo"].ToString(), dv1[i]["TitleEn"].ToString(),
                                           dv1[i]["TitleJa"].ToString(), dv1[i]["TitleFr"].ToString(), dv1[i]["TitleZhcn"].ToString(), dv1[i]["TitleZhtw"].ToString(), PreLang);
                                    string DeptName = this.GetLangName(dv1[i]["DeptBase"].ToString(), dv1[i]["DeptKo"].ToString(), dv1[i]["DeptEn"].ToString(),
                                               dv1[i]["DeptJa"].ToString(), dv1[i]["DeptFr"].ToString(), dv1[i]["DeptZhcn"].ToString(), dv1[i]["DeptZhtw"].ToString(), PreLang);

                                    info.UserInfo = Name + " " + TitleName + " " + DeptName;
                                }
                                else
                                {
                                    info.UserInfo = "";
                                }
                            }
                            else
                            {
                                info.UserInfo = "";
                            }

                            //댓글 카운트
                            if ((dv1[i]["ReplyCnt"].ToString() == "0") || (dv1[i]["ReplyCnt"].ToString() == "") || (dv1[i]["ReplyCnt"].ToString() == null))
                            {
                                info.ReplyCnt = "";
                            }
                            else
                            {
                                info.ReplyCnt = "[" + dv1[i]["ReplyCnt"].ToString() + "]";
                            }

                            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_Status");
                            sqlQuery = String.Format(sqlQuery, CompanyID);
                            DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetStatus", null);

                            info.Status = "";
                            if (ds1 != null)
                            {
                                foreach (DataRow dr in ds1.Tables[0].Rows)
                                {
                                    if (dr["StatusID"].ToString() != "0" && dr["StatusID"].ToString() == dv1[i]["StatusID"].ToString())
                                    {
                                        string statusName = this.GetLangName(
                                                dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                                dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                                dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                                dr["NameZhtw"].ToString().ToNull(), PreLang);

                                        info.Status = "<span class='bg-success text-white'" + (dv1[i]["Colors"].ToString().Length > 0 ? "style='background-color:" + dv1[i]["Colors"] + "!important;'" : "") + ">&nbsp;" + statusName + "&nbsp;</span>";
                                    }
                                }
                            }

                            listobj1.Add(info);
                        }
                    }
                }

                DataSet dsToDo = new DataSet();
                SqlParameter[] parm2 = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@year", SqlDbType.Int)
                                            ,new SqlParameter("@month", SqlDbType.Int)
                                            ,new SqlParameter("@day", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@IsOpen", SqlDbType.Char,1)
                };
                parm2[0].Value = CompanyID;
                parm2[1].Value = year;
                parm2[2].Value = month;
                parm2[3].Value = day;
                parm2[4].Value = userID;
                parm2[5].Value = isOpen;

                if (isOpen != "Y")
                {
                    if (pimsType.IndexOf("1") > -1)
                    {
                        if (pimTypeArr[0] == "1")
                        {
                            //개인할일
                            string proc0 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC0");
                            DataSet ds0 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc0, "ToDo", parm2);
                            dsToDo.Merge(ds0);
                        }

                        if (pimTypeArr[1] == "1")
                        {
                            //부서할일
                            string proc1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC1");
                            DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc1, "ToDo", parm2);
                            dsToDo.Merge(ds1);
                        }

                        if (pimTypeArr[2] == "1")
                        {
                            //공유할일
                            string proc2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC2");
                            DataSet ds2 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc2, "ToDo", parm2);
                            dsToDo.Merge(ds2);
                        }

                        if (pimTypeArr[3] == "1")
                        {
                            //전체할일
                            string proc3 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC3");
                            DataSet ds3 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc3, "ToDo", parm2);
                            dsToDo.Merge(ds3);
                        }
                    }
                    else
                    {
                        string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC");
                        dsToDo = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "ToDo", parm2);
                    }
                }
                else
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC");
                    dsToDo = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "ToDo", parm2);
                }

                List<BE.ToDoInfo> listobj2 = new List<BE.ToDoInfo>();

                foreach (DataRow dr in dsToDo.Tables[0].Rows)
                {
                    BE.ToDoInfo info = new BE.ToDoInfo();

                    info.ToDoID = Convert.ToInt32(dr["ToDoID"].ToString());
                    info.UserID = Convert.ToInt32(dr["UserID"].ToString());
                    info.OpenType = Convert.ToInt32(dr["OpenType"].ToString());

                    switch (info.OpenType)
                    {
                        case 0:
                            info.OpenTypeStr = resMng.GetString("TOpenType0");
                            break;

                        case 1:
                            info.OpenTypeStr = resMng.GetString("TOpenType1");
                            break;

                        case 2:
                            info.OpenTypeStr = resMng.GetString("TOpenType2");
                            break;

                        case 3:
                            info.OpenTypeStr = resMng.GetString("TOpenType3");
                            break;

                        default:
                            info.OpenTypeStr = "";
                            break;
                    }

                    if (dr["Status"].ToString() == "W")
                    {
                        info.Status = resMng.GetString("Status1");
                    }
                    else if (dr["Status"].ToString() == "I")
                    {
                        info.Status = resMng.GetString("Status2");
                    }
                    else if (dr["Status"].ToString() == "E")
                    {
                        info.Status = resMng.GetString("Status3");
                    }
                    else if (dr["Status"].ToString() == "C")
                    {
                        info.Status = resMng.GetString("Status4");
                    }
                    else if (dr["Status"].ToString() == "T")
                    {
                        info.Status = resMng.GetString("Status5");
                    }
                    else if (dr["Status"].ToString() == "D")
                    {
                        info.Status = resMng.GetString("Status6");
                    }

                    if (dr["Priority"].ToString() == "1")
                    {
                        info.Priority = resMng.GetString("Priority1");
                    }
                    else if (dr["Priority"].ToString() == "2")
                    {
                        info.Priority = resMng.GetString("Priority2");
                    }
                    else if (dr["Priority"].ToString() == "3")
                    {
                        info.Priority = resMng.GetString("Priority3");
                    }

                    info.Title = dr["Title"].ToString();

                    if (dr["ScheduleYN"].ToString() == "Y")
                    {
                        info.ScheduleYN = resMng.GetString("ScheduleToDo");
                    }
                    else
                    {
                        info.ScheduleYN = "";
                    }

                    string Name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                   dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                    if (Name != "")
                    {
                        if (info.UserID != userID.ToNumber<int>())
                        {
                            string TitleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                                   dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                            string DeptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                                       dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                            info.UserInfo = Name + " " + TitleName + " " + DeptName;
                        }
                        else
                        {
                            info.UserInfo = "";
                        }
                    }
                    else
                    {
                        info.UserInfo = "";
                    }

                    //댓글 카운트
                    if ((dr["ReplyCnt"].ToString() == "0") || (dr["ReplyCnt"].ToString() == "") || (dr["ReplyCnt"].ToString() == null))
                    {
                        info.ReplyCnt = "";
                    }
                    else
                    {
                        info.ReplyCnt = "[" + dr["ReplyCnt"].ToString() + "]";
                    }

                    listobj2.Add(info);
                }

                DataTable dtSch = ToDataTable(listobj1);
                DataTable dtToDo = ToDataTable(listobj2);

                DataTable rtnDt = new DataTable();
                dtSch.Merge(dtToDo);
                rtnDt = dtSch.Copy();

                var sqlAccQuery1 = "WITH TmpTBL AS  ";
                sqlAccQuery1 += "(SELECT Row_Number() over (ORDER BY titleorder, dutyorder,userdeptorder DESC, NameBase, NameKo,NameEn,NameJa,NameFr,NameZhCn,NameZhTw) AS RowNum ";
                sqlAccQuery1 += " ,KeyID as OwnerID";
                sqlAccQuery1 += " FROM VW_ADD_Tree WITH(NOLOCK) WHERE Companyid = " + CompanyID + " AND Type IN ('GM')   )  SELECT MIN(RowNum) RowNum , OwnerID FROM TmpTBL GROUP BY OwnerID";

                DataSet dsAccount1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlAccQuery1, "SEL_QRY_USER", null);

                var query1 = from c in rtnDt.AsEnumerable()
                             join b in dsAccount1.Tables[0].AsEnumerable()
                             on c.Field<string>("UserID") equals b.Field<int>("OwnerID").ToString()
                             orderby c.Field<string>("UserID").ToString() == userID ? 0 : 1, b.Field<Int64>("RowNum"), c.Field<string>("ToDoID"), c.Field<string>("StartTime"), c.Field<string>("ScheduleID")
                             select c;

                DataTable allList = new DataTable();

                if (query1.Count() > 0)
                {
                    allList = query1.CopyToDataTable();
                }

                allList.TableName = "rtnList";
                DataSet dsRtn = new DataSet();
                dsRtn.Tables.Add(allList.Copy());

                return DataTableToJSONWithJavaScriptSerializer(dsRtn);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Portal-GetScheduleToDoList()", ex);
                throw;
            }
        }

        public DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public string DataTableToJSONWithJavaScriptSerializer(DataSet dataset)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

            Dictionary<string, object> ssvalue = new Dictionary<string, object>();

            foreach (DataTable table in dataset.Tables)
            {
                List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
                Dictionary<string, object> childRow;

                string tablename = table.TableName;
                foreach (DataRow row in table.Rows)
                {
                    childRow = new Dictionary<string, object>();
                    foreach (DataColumn col in table.Columns)
                    {
                        childRow.Add(col.ColumnName, row[col]);
                    }
                    parentRow.Add(childRow);
                }

                ssvalue.Add(tablename, parentRow);
            }

            return jsSerializer.Serialize(ssvalue);
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 달력 가지고 오기
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCalendar(string p)
        {
            //if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);

            int year = Convert.ToInt32(GetDecryptParam("Year", DateTime.Now.Year.ToString()));
            int month = Convert.ToInt32(GetDecryptParam("Month", DateTime.Now.Month.ToString()));
            int day = Convert.ToInt32(GetDecryptParam("Day", DateTime.Now.Day.ToString()));
            string userID = GetDecryptParam("UserID", "0");
            string isOpen = GetDecryptParam("IsOpen", "");
            int maxPrevDay = 0; // 이전달의 Max 날짜
            int maxDay = 0; //현재달의 Max 날짜
            int startDay = 0; //현재달의 시작 요일
            int realDay = 1; // 실제 날짜
            int nextDay = 1; // 다음날 시작 일자
            int countWeek = 0;

            try
            {
                DateTime dt = new DateTime(year, month, 1);

                startDay = GetCountOfWeek(dt.DayOfWeek); //해당 월의 시작 요일

                DateTime dtReal = new DateTime(year, month, 1); //해당 월 
                DateTime dtPrev = dtReal.AddMonths(-1);  //이전달 Max 날짜 계산
                DateTime dtNext = dtReal.AddMonths(1);  //다음달 날짜 계산

                maxPrevDay = GetDayCountOfMonth(dtPrev.Year, dtPrev.Month); //이전달의 Max 날짜
                maxDay = GetDayCountOfMonth(year, month); //해당 월의 날짜갯수

                List<object> listobj = new List<object>();

                BE.CalendarInfo info;


                for (int i = 0; i < 42; i++)
                {
                    info = new BE.CalendarInfo();

                    //요일 표시
                    info.Week = countWeek.ToString();
                    countWeek += 1;
                    if (countWeek % 7 == 0) countWeek = 0;

                    if (i < startDay) //지난달 표시
                    {
                        info.Year = dtPrev.Year.ToString();
                        info.Month = dtPrev.Month.ToString();
                        info.Day = (maxPrevDay + 1 - startDay + i).ToString();  //해당월의 날짜가 들어간다
                        info.Class = " dim";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " dim_hol";
                        }
                    }
                    else if (i >= startDay && i < maxDay + startDay) // 현재달 표시
                    {
                        info.Year = year.ToString();
                        info.Month = month.ToString();
                        info.Day = realDay.ToString();  //해당월의 날짜가 들어간다
                        realDay += 1;

                        info.Class = "";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " hol";
                        }

                        //오늘 날짜 Style 표시
                        if ((info.Day == DateTime.Now.Day.ToString()) && (info.Month == DateTime.Now.Month.ToString()) && (info.Year == DateTime.Now.Year.ToString()))
                        {
                            info.Class = " busy today";
                        }
                    }
                    else //다음달 표시
                    {
                        info.Year = dtNext.Year.ToString();
                        info.Month = dtNext.Month.ToString();
                        info.Day = nextDay.ToString();  //빈칸들
                        nextDay += 1;

                        info.Class = " dim";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " dim_hol";
                        }
                    }

                    info.Month = (Convert.ToInt32(info.Month) < 10) ? "0" + info.Month : info.Month;
                    info.Day = (Convert.ToInt32(info.Day) < 10) ? "0" + info.Day : info.Day;

                    string tmpDate = info.Year + "-" + info.Month + "-" + info.Day;
                    info.Func = "List.getList('" + tmpDate + "')";
                    info.DoCount = "";
                    info.Index = tmpDate;

                    listobj.Add(info);
                }

                /*
                //공휴일 표시
                string[] arSolar = XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "Solar").Split(','); // 양력
                string[] arLunar = XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "Lunar").Split(','); // 음력                

                string targetDay = String.Empty; //공휴일
                string beforeDay = String.Empty; //공휴일 앞으로 몇일 쉴지
                string afterDay = String.Empty;  //공휴일 뒤로 몇일 쉴지
                List<string> Holidayobj = new List<string>();
                //양력 표시
                foreach (string _s in arSolar)
                {
                    if (_s.Length > 0)
                    {
                        if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                        {
                            targetDay = _s.Split('|')[0];
                            beforeDay = _s.Split('|')[1];
                            afterDay = _s.Split('|')[2];
                        }
                        else
                        {
                            targetDay = _s;
                        }

                        DateTime sDate = DateTime.Parse(year + "-" + targetDay);
                        Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }

                //음력 표시
                foreach (string _s in arLunar)
                {
                    if (_s.Length > 0)
                    {
                        if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                        {
                            targetDay = _s.Split('|')[0];
                            beforeDay = _s.Split('|')[1];
                            afterDay = _s.Split('|')[2];
                        }
                        else
                        {
                            targetDay = _s;
                        }

                        DateTime sDate = DateTime.Parse(year + "-" + targetDay);
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                        Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));


                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }

                */

                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj2 = new List<object>();

                if (context.Cache[CacheName + CompanyID] == null)
                {
                    SetCacheHolidayList(CompanyID, DateTime.Now.ToString("yyyy"));
                }

                listobj2 = (List<object>)context.Cache[CacheName + CompanyID];

                List<string> Holidayobj = new List<string>();
                string tempDate = string.Empty;

                foreach (BE.HolidayRInfo _be in listobj2)
                {
                    string SolarLunar = _be.SolarLunar;                               //양음력구분
                    string HOLIDAY = _be.HoliDay;                                     //일자
                    string TITLE = _be.Title;                                         //제목
                    string HolidayBetween = _be.HolidayBetween;                       //연휴구분
                    int HolidayBetweenDay = _be.HolidayBetweenDay;                    //연휴일자
                    string IsReplace = _be.IsReplace;                                 //대체휴가구분
                    int IsReplaceDay = 0;                                             //대체휴가일자
                    string IsHolidayKind = _be.IsHolidayKind;                         //공휴일구분    

                    DateTime sDate = DateTime.Parse(HOLIDAY);

                    //양력음력구분
                    if (SolarLunar != "0")
                    {
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                    }

                    //대체휴일 확인
                    if (IsReplace == "1")
                    {
                        for (int i = 0; i <= HolidayBetweenDay; i++)
                        {
                            if (HolidayBetween == "1" || HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    if (i > 0)
                                    {
                                        IsReplaceDay = IsReplaceDay + 1;
                                    }
                                }
                            }

                            if (HolidayBetween == "2" || HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }
                    }

                    //대체휴일 + 토 확인
                    if (IsReplace == "2")
                    {
                        for (int i = 0; i <= HolidayBetweenDay; i++)
                        {
                            if (HolidayBetween == "0")
                            {
                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }
                                else if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }

                            if (HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }
                    }

                    //일자 저장
                    Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));

                    //비교 데이터 저장
                    if (IsHolidayKind == "0")
                    {
                        tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                    }

                    if (HolidayBetween == "1")
                    {
                        //이전일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }
                    else if (HolidayBetween == "2")
                    {
                        //다음일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }
                    else if (HolidayBetween == "3")
                    {
                        //이전다음일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "1")
                            {
                                tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                            }

                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "1")
                            {
                                tempDate = tempDate + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }

                    if (IsReplaceDay > 0)
                    {
                        int IsReplaceDayStart = HolidayBetweenDay + 1;
                        int IsReplaceDayEnd = IsReplaceDay + HolidayBetweenDay;

                        for (int i = IsReplaceDayStart; i <= IsReplaceDayEnd; i++)
                        {
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                        }
                    }
                }

                string Holiday = string.Join(",", Holidayobj);

                string pimMonth = year.ToString() + "-" + month.ToString() + "-01";
                DataTable dtMonth = GetMonthList(userID, isOpen, pimMonth);

                //달력에 일정 및 공휴일 표시 
                foreach (BE.CalendarInfo calInfo in listobj)
                {
                    //일정이 존재 할 경우 스타일 표시
                    //DataRow[] dr = dtMonth.Select("YearTime='" + calInfo.Year + "' AND MonthTime='" + Convert.ToInt32(calInfo.Month) + "' AND DayTime='" + Convert.ToInt32(calInfo.Day) + "'");

                    // 공휴일 
                    string tmpMD = calInfo.Month.PadLeft(2, '0') + "-" + calInfo.Day.PadLeft(2, '0');
                    string tmpHol = String.Empty;
                    foreach (string hol in Holiday.Split(','))
                    {
                        tmpHol = hol.Replace(year.ToString() + "-", "");
                        if (tmpMD == tmpHol)
                        {
                            if (calInfo.Month.ToNumber<int>() == month)
                            {
                                calInfo.Class = " hol";
                            }
                            else
                            {
                                calInfo.Class = " dim_hol";
                            }
                        }
                    }

                    string tmpDate = calInfo.Year + "-" + calInfo.Month + "-" + calInfo.Day;
                    DataRow[] dr = dtMonth.Select("StartDate <= '" + tmpDate + " 23:59:59.000' AND '" + tmpDate + " 00:00:00.000' <= EndDate");

                    if (dr.Length > 0)
                    {
                        if (Convert.ToInt32(calInfo.Month) == month)
                        {
                            if (calInfo.Class.IndexOf("hol") > 0)
                            {
                                calInfo.Class = " busy hol";
                            }
                            else
                            {
                                calInfo.Class = "busy";
                            }

                            //일요일 Style 표시
                            if (calInfo.Week == "0")
                            {
                                calInfo.Class = " busy hol";
                            }
                        }
                        else
                        {
                            if (calInfo.Class.IndexOf("hol") > 0)
                            {
                                calInfo.Class = " busy dim_hol";
                            }
                            else
                            {
                                calInfo.Class = "dim busy";
                            }

                            //일요일 Style 표시
                            if (calInfo.Week == "0")
                            {
                                calInfo.Class = " busy dim_hol";
                            }
                        }

                        calInfo.DoCount = dr.Length.ToString();
                    }


                    //오늘 날짜 Style 표시
                    if ((Convert.ToInt32(calInfo.Day) == DateTime.Now.Day) && (Convert.ToInt32(calInfo.Month) == DateTime.Now.Month) && (Convert.ToInt32(calInfo.Year) == DateTime.Now.Year))
                    {
                        if (calInfo.Class.IndexOf("hol") > 0)
                        {
                            calInfo.Class = " busy holtoday";
                        }
                        else
                        {
                            calInfo.Class = " busy today";
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetCalendar()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 휴일 캐쉬에 담기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public void SetCacheHolidayList(int _companyID, string _UserSelectedYear)
        {
            try
            {
                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj = new List<object>();

                if (context.Cache[CacheName + _companyID] == null)
                {
                    SqlParameter[] parm = new SqlParameter[]{
                                new SqlParameter("@CompanyID", SqlDbType.Int)
                            ,new SqlParameter("@SelectYear", SqlDbType.Int)
                    };
                    parm[0].Value = CompanyID;
                    parm[1].Value = _UserSelectedYear.ToNumber<int>();

                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_HolidayR_SelByYear");
                    sql = string.Format(sql, _companyID, _UserSelectedYear.ToNumber<int>());
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsYear", parm);

                    foreach (DataRow dtr in ds.Tables[0].Rows)
                    {
                        BE.HolidayRInfo _data = new BE.HolidayRInfo();

                        _data.SolarLunar = dtr["SolarLunar"].ToString();                               //양음력구분
                        _data.HoliDay = dtr["HOLIDAY"].ToString();                                     //일자
                        _data.Title = dtr["TITLE"].ToString();                                         //제목
                        _data.HolidayBetween = dtr["HolidayBetween"].ToString();                       //연휴구분
                        _data.HolidayBetweenDay = dtr["HolidayBetweenDay"].ToString().ToNumber<int>(); //연휴일자
                        _data.IsReplace = dtr["IsReplace"].ToString();                                 //대체휴가구분
                        _data.IsReplaceDay = 0;                                                        //대체휴가일자
                        _data.IsHolidayKind = dtr["IsHolidayKind"].ToString();                         //공휴일구분        

                        listobj.Add(_data);
                    }

                    HttpContext ctx = HttpContext.Current;
                    ctx.Cache.Insert(CacheName + _companyID, listobj, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetCacheHolidayList : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 달력 가지고 오기 (아무 것도 없는 달력)
        /// 작성일자 : 2011-07-15
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBasicCalendar(string p)
        {
            SetDecryptString(p);

            int year = Convert.ToInt32(GetDecryptParam("Year", DateTime.Now.Year.ToString()));
            int month = Convert.ToInt32(GetDecryptParam("Month", DateTime.Now.Month.ToString()));
            int maxPrevDay = 0; // 이전달의 Max 날짜
            int maxDay = 0; //현재달의 Max 날짜
            int startDay = 0; //현재달의 시작 요일
            int realDay = 1; // 실제 날짜
            int nextDay = 1; // 다음날 시작 일자
            int countWeek = 0;

            try
            {
                DateTime dt = new DateTime(year, month, 1);

                startDay = GetCountOfWeek(dt.DayOfWeek); //해당 월의 시작 요일

                DateTime dtReal = new DateTime(year, month, 1); //해당 월 
                DateTime dtPrev = dtReal.AddMonths(-1);  //이전달 Max 날짜 계산
                DateTime dtNext = dtReal.AddMonths(1);  //다음달 날짜 계산

                maxPrevDay = GetDayCountOfMonth(dtPrev.Year, dtPrev.Month); //이전달의 Max 날짜
                maxDay = GetDayCountOfMonth(year, month); //해당 월의 날짜갯수

                List<object> listobj = new List<object>();

                BE.CalendarInfo info;


                for (int i = 0; i < 42; i++)
                {
                    info = new BE.CalendarInfo();

                    //요일 표시
                    info.Week = countWeek.ToString();
                    countWeek += 1;
                    if (countWeek % 7 == 0) countWeek = 0;

                    if (i < startDay) //지난달 표시
                    {
                        info.Year = dtPrev.Year.ToString();
                        info.Month = dtPrev.Month.ToString();
                        info.Day = (maxPrevDay + 1 - startDay + i).ToString();  //해당월의 날짜가 들어간다
                        info.Class = " dim";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " dim_hol";
                        }
                    }
                    else if (i >= startDay && i < maxDay + startDay) // 현재달 표시
                    {
                        info.Year = year.ToString();
                        info.Month = month.ToString();
                        info.Day = realDay.ToString();  //해당월의 날짜가 들어간다
                        realDay += 1;

                        info.Class = "";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " hol";
                        }

                    }
                    else //다음달 표시
                    {
                        info.Year = dtNext.Year.ToString();
                        info.Month = dtNext.Month.ToString();
                        info.Day = nextDay.ToString();  //빈칸들
                        nextDay += 1;

                        info.Class = " dim";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " dim_hol";
                        }
                    }

                    info.Month = (Convert.ToInt32(info.Month) < 10) ? "0" + info.Month : info.Month;
                    info.Day = (Convert.ToInt32(info.Day) < 10) ? "0" + info.Day : info.Day;

                    string tmpDate = info.Year + "-" + info.Month + "-" + info.Day;
                    info.Func = "setList('" + tmpDate + "')";
                    info.DoCount = "";
                    info.Index = tmpDate;

                    listobj.Add(info);
                }

                /*
                //공휴일 표시
                string[] arSolar = XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "Solar").Split(','); // 양력
                string[] arLunar = XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "Lunar").Split(','); // 음력  

                string targetDay = String.Empty; //공휴일
                string beforeDay = String.Empty; //공휴일 앞으로 몇일 쉴지
                string afterDay = String.Empty;  //공휴일 뒤로 몇일 쉴지
                List<string> Holidayobj = new List<string>();
                //양력 표시
                foreach (string _s in arSolar)
                {
                    if (_s.Length > 0)
                    {
                        if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                        {
                            targetDay = _s.Split('|')[0];
                            beforeDay = _s.Split('|')[1];
                            afterDay = _s.Split('|')[2];
                        }
                        else
                        {
                            targetDay = _s;
                        }

                        DateTime sDate = DateTime.Parse(year + "-" + targetDay);
                        Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }

                //음력 표시
                foreach (string _s in arLunar)
                {
                    if (_s.Length > 0)
                    {
                        if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                        {
                            targetDay = _s.Split('|')[0];
                            beforeDay = _s.Split('|')[1];
                            afterDay = _s.Split('|')[2];
                        }
                        else
                        {
                            targetDay = _s;
                        }

                        DateTime sDate = DateTime.Parse(year + "-" + targetDay);
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                        Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));


                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }                
                */

                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj2 = new List<object>();

                if (context.Cache[CacheName + CompanyID] == null)
                {
                    SetCacheHolidayList(CompanyID, DateTime.Now.ToString("yyyy"));
                }

                listobj2 = (List<object>)context.Cache[CacheName + CompanyID];

                List<string> Holidayobj = new List<string>();
                string tempDate = string.Empty;

                foreach (BE.HolidayRInfo _be in listobj2)
                {
                    string SolarLunar = _be.SolarLunar;                               //양음력구분
                    string HOLIDAY = _be.HoliDay;                                     //일자
                    string TITLE = _be.Title;                                         //제목
                    string HolidayBetween = _be.HolidayBetween;                       //연휴구분
                    int HolidayBetweenDay = _be.HolidayBetweenDay;                    //연휴일자
                    string IsReplace = _be.IsReplace;                                 //대체휴가구분
                    int IsReplaceDay = 0;                                             //대체휴가일자
                    string IsHolidayKind = _be.IsHolidayKind;                         //공휴일구분    

                    DateTime sDate = DateTime.Parse(HOLIDAY);

                    //양력음력구분
                    if (SolarLunar != "0")
                    {
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                    }

                    //대체휴일 확인
                    if (IsReplace == "1")
                    {
                        for (int i = 0; i <= HolidayBetweenDay; i++)
                        {
                            if (HolidayBetween == "1" || HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    if (i > 0)
                                    {
                                        IsReplaceDay = IsReplaceDay + 1;
                                    }
                                }
                            }

                            if (HolidayBetween == "2" || HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }
                    }

                    //대체휴일 + 토 확인
                    if (IsReplace == "2")
                    {
                        for (int i = 0; i <= HolidayBetweenDay; i++)
                        {
                            if (HolidayBetween == "0")
                            {
                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }
                                else if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }

                            if (HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }
                    }

                    //일자 저장
                    Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));

                    //비교 데이터 저장
                    if (IsHolidayKind == "0")
                    {
                        tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                    }

                    if (HolidayBetween == "1")
                    {
                        //이전일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }
                    else if (HolidayBetween == "2")
                    {
                        //다음일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }
                    else if (HolidayBetween == "3")
                    {
                        //이전다음일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "1")
                            {
                                tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                            }

                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "1")
                            {
                                tempDate = tempDate + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }

                    if (IsReplaceDay > 0)
                    {
                        int IsReplaceDayStart = HolidayBetweenDay + 1;
                        int IsReplaceDayEnd = IsReplaceDay + HolidayBetweenDay;

                        for (int i = IsReplaceDayStart; i <= IsReplaceDayEnd; i++)
                        {
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                        }
                    }
                }

                string Holiday = string.Join(",", Holidayobj);

                //달력에 일정 및 공휴일 표시 
                foreach (BE.CalendarInfo calInfo in listobj)
                {
                    // 공휴일 
                    string tmpMD = calInfo.Month.PadLeft(2, '0') + "-" + calInfo.Day.PadLeft(2, '0');
                    string tmpHol = String.Empty;
                    foreach (string hol in Holiday.Split(','))
                    {
                        tmpHol = hol.Replace(year.ToString() + "-", "");
                        if (tmpMD == tmpHol)
                        {
                            if (calInfo.Month.ToNumber<int>() == month)
                            {
                                calInfo.Class = " hol";
                            }
                            else
                            {
                                calInfo.Class = " dim_hol";
                            }
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBasicCalendar()", ex);
                throw;
            }
        }

        /// <summary>
        ///  요일을 숫자료 변환
        /// </summary>
        /// <param name="_dow"></param>
        /// <returns></returns>
        private int GetCountOfWeek(DayOfWeek _dow)
        {
            int rtn = 0;

            switch (_dow)
            {
                case DayOfWeek.Sunday: rtn = 0; break;
                case DayOfWeek.Monday: rtn = 1; break;
                case DayOfWeek.Tuesday: rtn = 2; break;
                case DayOfWeek.Wednesday: rtn = 3; break;
                case DayOfWeek.Thursday: rtn = 4; break;
                case DayOfWeek.Friday: rtn = 5; break;
                case DayOfWeek.Saturday: rtn = 6; break;
            }

            return rtn;
        }

        /// <summary>
        /// 월별 날짜수 계산
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private int GetDayCountOfMonth(int year, int month)
        {
            int leapYear = 0;
            int rtn = 0;

            //윤년 체크
            if ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0)) leapYear = 1;

            switch (month)
            {
                case 1: rtn = 31; break;
                case 2: rtn = 28 + leapYear; break;
                case 3: rtn = 31; break;
                case 4: rtn = 30; break;
                case 5: rtn = 31; break;
                case 6: rtn = 30; break;
                case 7: rtn = 31; break;
                case 8: rtn = 31; break;
                case 9: rtn = 30; break;
                case 10: rtn = 31; break;
                case 11: rtn = 30; break;
                case 12: rtn = 31; break;
                default: rtn = -1; break;
            }
            return rtn;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 월별 일정 가져오기
        /// 작성일자 : 2011-06-24
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        private DataTable GetMonthList(string _userID, string _isOpen, string _pimsMonth)
        {
            string pimsType = "";
            if (UserEnv != null)
            {
                pimsType = UserEnv;
            }
            else
            {
                pimsType = "0|0|0|0|";
            }

            string[] pimTypeArr = pimsType.Split('|');

            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@IsOpen", SqlDbType.Char,1)
                                            ,new SqlParameter("@PimsMonth", SqlDbType.VarChar,10)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = _userID;
                parm[2].Value = _isOpen;
                parm[3].Value = _pimsMonth;

                if (_isOpen != "Y")
                {
                    if (pimsType.IndexOf("1") > -1)
                    {
                        if (pimTypeArr[0] == "1")
                        {
                            //개인일정    
                            string proc0 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC0_MONTH");
                            DataSet ds0 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc0, "dsMonthList", parm);
                            ds.Merge(ds0);

                            //개인할일
                            string proc4 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC0_MONTH");
                            DataSet ds4 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc4, "dsMonthList", parm);
                            ds.Merge(ds4);
                        }

                        if (pimTypeArr[1] == "1")
                        {
                            //부서일정
                            string proc1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC1_MONTH");
                            DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc1, "dsMonthList", parm);
                            ds.Merge(ds1);

                            //부서할일
                            string proc5 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC1_MONTH");
                            DataSet ds5 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc5, "dsMonthList", parm);
                            ds.Merge(ds5);
                        }

                        if (pimTypeArr[2] == "1")
                        {
                            //공유일정
                            string proc2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC2_MONTH");
                            DataSet ds2 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc2, "dsMonthList", parm);
                            ds.Merge(ds2);

                            //공유할일
                            string proc6 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC2_MONTH");
                            DataSet ds6 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc6, "dsMonthList", parm);
                            ds.Merge(ds6);
                        }

                        if (pimTypeArr[3] == "1")
                        {
                            //전체일정
                            string proc3 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC3_MONTH");
                            DataSet ds3 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc3, "dsMonthList", parm);
                            ds.Merge(ds3);

                            //전체할일
                            string proc7 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC3_MONTH");
                            DataSet ds7 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc7, "dsMonthList", parm);
                            ds.Merge(ds7);
                        }

                    }
                    else
                    {
                        string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC_MONTH");
                        ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsMonthList", parm);

                        // 할일 추가 
                        string proc1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC_MONTH");
                        DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc1, "dsMonthList", parm);
                        ds.Merge(ds1);
                    }
                }
                else
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC_MONTHSHARE");
                    ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsMonthList", parm);

                    // 할일 추가 
                    string proc1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC_MONTHSHARE");
                    DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc1, "dsMonthList", parm);
                    ds.Merge(ds1);
                }

                /*
                var groupbyfilter = from dr in ds.Tables[0].AsEnumerable()
                                    group dr by new { YearTime = dr["YearTime"], MonthTime = dr["MonthTime"], DayTime = dr["DayTime"] };

                DataTable dt = ds.Tables[0].Clone();

                foreach (var x in groupbyfilter)
                    x.CopyToDataTable(dt, LoadOption.OverwriteChanges);                 

                return dt;
                */

                /*
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "ScheduleID";
  
                DataTable dt = new DataTable();
                DataColumn dc0 = new DataColumn("YearTime", typeof(int));
                DataColumn dc1 = new DataColumn("MonthTime", typeof(int));
                DataColumn dc2 = new DataColumn("DayTime", typeof(int));
                DataColumn dc3 = new DataColumn("ScheduleID", typeof(int));  
   
                dt.Columns.Add(dc0);
                dt.Columns.Add(dc1);
                dt.Columns.Add(dc2);
                dt.Columns.Add(dc3);

                int tmpScheduleID = 0;
                DataRow row;
                if (dv.Table.Rows.Count > 0)
                {
                    for (int i = 0; i < dv.Table.Rows.Count; i++)
                    {
                        if (tmpScheduleID != Convert.ToInt32(dv[i]["ScheduleID"].ToString()))
                        {
                            row = dt.NewRow();
                            row["YearTime"] = Convert.ToInt32(dv[i]["YearTime"].ToString());
                            row["MonthTime"] = Convert.ToInt32(dv[i]["MonthTime"].ToString());
                            row["DayTime"] = Convert.ToInt32(dv[i]["DayTime"].ToString());
                            row["ScheduleID"] = Convert.ToInt32(dv[i]["ScheduleID"].ToString());
                            tmpScheduleID = Convert.ToInt32(dv[i]["ScheduleID"].ToString());

                            dt.Rows.Add(row);                                                       
                        }
                    }
                }
                
                return dt;
                */

                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetMonthList()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정관리 공유자 가지고 오기
        /// 작성일자 : 2011-06-24
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShareList(string p)
        {
            SetDecryptString(p);
            string userID = GetDecryptParam("UserID", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_SCHESHAREUSERS");
            sqlQuery = String.Format(sqlQuery, CompanyID, userID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "Share", null);

            List<object> listobj = new List<object>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BE.ShareListInfo info = new BE.ShareListInfo();

                info.OpenID = Convert.ToInt32(dr["OpenID"].ToString());

                info.Name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                  dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                info.TitleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                           dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                info.DeptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                           dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                listobj.Add(info);
            }

            return listobj;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 첨부파일정보 가져오기
        /// 작성일자 : 2012-04-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFile(string p)
        {
            SetDecryptString(p);
            string scheduleID = GetDecryptParam("ScheduleID", "0");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            if (String.IsNullOrEmpty(scheduleID)) { return "!NULL"; }

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_AttList");
                sql = string.Format(sql, CompanyID.ToString(), scheduleID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsAtt", null);
                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAttachFile('" + scheduleID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이중호
        /// 작성목적 : 문서 스트리밍 정보 가져오기
        /// 작성일자 : 2018-04-13
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDSFile(string p)
        {
            SetDecryptString(p);
            string scheduleID = GetDecryptParam("ScheduleID", "0");
            string attachType = GetDecryptParam("TYPE", "");

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.TableName = "dsDS";
            ds.Tables.Add(dt);

            if (CompanyID <= 0 || UserID <= 0) return StringHelper.GetJSONString(ds.Tables[0]);

            if (String.IsNullOrEmpty(scheduleID)) return StringHelper.GetJSONString(ds.Tables[0]);

            try
            {
                var docStreamYN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "DocStreamYN");

                if (docStreamYN == "Y")
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_DSList");
                    SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@ScheduleID", SqlDbType.Int)
                                            ,new SqlParameter("@AttachType", SqlDbType.NVarChar, 100)
                };
                    parm[0].Value = CompanyID;
                    parm[1].Value = scheduleID;
                    parm[2].Value = attachType;
                    ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsDS", parm);

                }

                return StringHelper.GetJSONString(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetDSFile('" + scheduleID + "')", ex);
                throw;
            }
        }

        /*
        /// <summary>
        /// 음력을 양력으로 변환
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="isLeapMonth"></param>
        /// <returns></returns>
        private DateTime LunarToSolar(int year, int month, int day, bool isLeapMonth)
        {
            EastAsianLunisolarCalendar kLunar = new KoreanLunisolarCalendar();
            int leapMonth = 0;

            // 윤달이 존재하는지 체크
            if (kLunar.GetMonthsInYear(year) > 12)
            {
                leapMonth = kLunar.GetLeapMonth(year);

                if (isLeapMonth)
                {
                    month++;
                }
                else if (month > leapMonth - 1)
                {
                    month++;
                }
            }


            DateTime solDate = kLunar.ToDateTime(year, month, day, 0, 0, 0, 0);

            return solDate;
        }
        */

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 일정 댓글 정보 가져오기
        /// 작성일자 : 2012-11-07
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetScheduleReply(string p)
        {
            SetDecryptString(p);
            string scheduleID = GetDecryptParam("ScheduleID", "0");

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_ScheduleReply");
                sql = string.Format(sql, CompanyID.ToString(), scheduleID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsSR", null);

                List<object> listobj = new List<object>();

                string name = "";
                string deptName = "";
                string titleName = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ScheduleReplyInfo info = new BE.ScheduleReplyInfo();

                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.ScheduleID = dr["ScheduleID"].ToString().ToNumber<int>();
                    info.ReplyID = dr["ReplyID"].ToString().ToNumber<int>();
                    info.Content = dr["Content"].ToString().Replace("\n", "<br>");
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();
                    info.SUserID = dr["SUserID"].ToString().ToNumber<int>();
                    info.IsSecurity = dr["IsSecurity"].ToString();

                    name = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(),
                        PreLang);

                    deptName = this.GetLangName(
                       dr["DeptBase"].ToString(),
                       dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(),
                       dr["DeptFr"].ToString(), dr["DeptZhCn"].ToString(), dr["DeptZhTw"].ToString(),
                       PreLang);

                    titleName = this.GetLangName(
                       dr["TitleBase"].ToString(),
                       dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(),
                       dr["TitleFr"].ToString(), dr["TitleZhCn"].ToString(), dr["TitleZhTw"].ToString(),
                       PreLang);

                    info.UserName = deptName + " " + titleName + " " + name;

                    info.View = "N";

                    // 원본글 작성자와 댓글러가 같은지
                    if (UserID == info.SUserID)
                    {
                        info.View = "Y";
                    }
                    else if (UserID == info.UserID && info.IsSecurity == "1")
                    {
                        info.View = "Y";
                    }
                    else if (info.IsSecurity == "0")
                    {
                        info.View = "Y";
                    }

                    DateTime dt;

                    if (DateTime.TryParse(dr["InputDate"].ToString(), out dt))
                        info.InputDate = Convert.ToDateTime(dr["InputDate"]).ToShortDateString() + " " + Convert.ToDateTime(dr["InputDate"]).ToShortTimeString();
                    else
                        info.InputDate = string.Empty;

                    #region 날짜 비교(New Img)
                    DateTime writeDate = Convert.ToDateTime(dr["InputDate"].ToString());
                    DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan gap = todayDate.Subtract(writeDate);
                    int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                    info.ShowNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";
                    #endregion

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetScheduleReply('" + scheduleID + "')", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 할일 댓글 정보 가져오기
        /// 작성일자 : 2012-11-07
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetToDoReply(string p)
        {
            SetDecryptString(p);
            string toDoID = GetDecryptParam("ToDoID", "0");

            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_ToDoReply");
                sql = string.Format(sql, CompanyID.ToString(), toDoID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsTR", null);

                List<object> listobj = new List<object>();

                string name = "";
                string deptName = "";
                string titleName = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ToDoReplyInfo info = new BE.ToDoReplyInfo();

                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.ToDoID = dr["ToDoID"].ToString().ToNumber<int>();
                    info.ReplyID = dr["ReplyID"].ToString().ToNumber<int>();
                    info.Content = dr["Content"].ToString().Replace("\n", "<br>");
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();
                    info.TUserID = dr["TUserID"].ToString().ToNumber<int>();
                    info.IsSecurity = dr["IsSecurity"].ToString();

                    name = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(),
                        PreLang);

                    deptName = this.GetLangName(
                       dr["DeptBase"].ToString(),
                       dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(),
                       dr["DeptFr"].ToString(), dr["DeptZhCn"].ToString(), dr["DeptZhTw"].ToString(),
                       PreLang);

                    titleName = this.GetLangName(
                       dr["TitleBase"].ToString(),
                       dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(),
                       dr["TitleFr"].ToString(), dr["TitleZhCn"].ToString(), dr["TitleZhTw"].ToString(),
                       PreLang);

                    info.UserName = deptName + " " + titleName + " " + name;

                    info.View = "N";

                    // 원본글 작성자와 댓글러가 같은지
                    if (UserID == info.TUserID)
                    {
                        info.View = "Y";
                    }
                    else if (UserID == info.UserID && info.IsSecurity == "1")
                    {
                        info.View = "Y";
                    }
                    else if (info.IsSecurity == "0")
                    {
                        info.View = "Y";
                    }

                    DateTime dt;

                    if (DateTime.TryParse(dr["InputDate"].ToString(), out dt))
                        info.InputDate = Convert.ToDateTime(dr["InputDate"]).ToShortDateString() + " " + Convert.ToDateTime(dr["InputDate"]).ToShortTimeString();
                    else
                        info.InputDate = string.Empty;

                    #region 날짜 비교(New Img)
                    DateTime writeDate = Convert.ToDateTime(dr["InputDate"].ToString());
                    DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan gap = todayDate.Subtract(writeDate);
                    int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                    info.ShowNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";
                    #endregion

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetToDoReply('" + toDoID + "')", ex);
                throw;
            }
        }
    }
}
