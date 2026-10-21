using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Web.Services;
using System.Xml;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Reflection;
using System.Configuration;
using System.Web.Script.Serialization;

namespace Zeospace.DeskPlusMobile.Web.Portal.Ws
{
    /// <summary>
    /// GetPortal의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class GetPortal : Helper.MobileBaseService
    {
        private DataSet dsGetDeptUserTree;
        private string authority = "";

        #region 메일

        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 메일 최신 글
        /// 작성일자 : 2011-06-03
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMailList()
        {
            try
            {
                string mailWebServiceUrl = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailWebServiceUrl");
                string paramMailList = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailList");
                string param = string.Format(paramMailList, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, "basic", "0", "5", "0", "all", "");                

                string sXmlData = UrlSendHelper.UsePost(mailWebServiceUrl, param);

                XmlNodeList xmlList = null;
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sXmlData);

                string returnCode = xml.SelectSingleNode("/data/returnCode").InnerText;

                if (returnCode == "0")
                {
                    //성공
                }
                else
                {
                    //실패
                }

                xmlList = xml.SelectNodes("/data/result/item");

                List<object> listobj = new List<object>();

                foreach (XmlNode node in xmlList)
                {
                    BE.MailInfo info = new BE.MailInfo();

                    string tmpFrom = node["from"].InnerText;
                    string tmpFromName = "";

                    if (tmpFromName == "")
                    {
                        info.from_header = tmpFrom;
                    }
                    else
                    {
                        info.from_header = tmpFromName;
                    }

                    info.create_time = node["sendDate"].InnerText;
                    info.mail_subject = node["subject"].InnerText;
                    //info.today = (DateTime.Now.CompareTo(Convert.ToDateTime(dr["date_header"])) == 0) ? "Y" : "N";
                    info.today = "N";
                    info.mail_sn = node["uid"].InnerText;
                    info.state_code = "";
                    info.read_flag = node["isRead"].InnerText;

                    // deepMail 용 추가 필드 정의
                    info.uid = node["uid"].InnerText;
                    info.to = node["to"].InnerText;
                    info.cc = node["cc"].InnerText;
                    info.bcc = node["bcc"].InnerText;
                    info.recvDate = node["recvDate"].InnerText;
                    info.flag = node["flag"].InnerText;
                    info.isAttach = node["isAttach"].InnerText;
                    info.isRead = node["isRead"].InnerText;

                    #region 날짜 비교(New Img)

                    DateTime writeDate = Convert.ToDateTime(node["sendDate"].InnerText);
                    DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan gap = todayDate.Subtract(writeDate);
                    int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();
                    info.today = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";

                    #endregion 날짜 비교(New Img)

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetMailList(potal)", ex);
                throw;
            }
        }

        #endregion 메일

        #region 일정관리

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
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.Ws.GetPortal).Assembly;	//어셈블리 선언
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
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_SCHEDULEETC");
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
                            if ((dv[i]["ReplyCnt"].ToString() == "0") || (dv[i]["ReplyCnt"].ToString() == "") || (dv[i]["ReplyCnt"].ToString() == null))
                            {
                                info.ReplyCnt = "";
                            }
                            else
                            {
                                info.ReplyCnt = "[" + dv[i]["ReplyCnt"].ToString() + "]";
                            }

                            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_QRY_Status");
                            sqlQuery = String.Format(sqlQuery, CompanyID);
                            DataSet ds1 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetStatus", null);

                            info.Status = "";
                            if (ds1 != null)
                            {
                                foreach (DataRow dr in ds1.Tables[0].Rows)
                                {
                                    if (dr["StatusID"].ToString() != "0" && dr["StatusID"].ToString() == dv[i]["StatusID"].ToString())
                                    {
                                        string statusName = this.GetLangName(
                                                dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                                dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                                dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                                dr["NameZhtw"].ToString().ToNull(), PreLang);

                                        info.Status = "<span class='bg-success text-white'" + (dv[i]["Colors"].ToString().Length > 0 ? "style='background-color:" + dv[i]["Colors"] + "!important;'" : "") + ">&nbsp;" + statusName + "&nbsp;</span>";
                                    }
                                }
                            }

                            listobj.Add(info);

                            if (listobj.Count >= 5) break; // 최근 5건만 표시한다
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Portal-GetScheduleList()", ex);
                throw;
            }
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
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.Ws.GetPortal).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);

            string year = GetDecryptParam("Year", "0");
            string month = GetDecryptParam("Month", "0");
            string day = GetDecryptParam("Day", "0");
            string userID = GetDecryptParam("UserID", "0");
            string isOpen = GetDecryptParam("IsOpen", "");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.Ws.GetPortal).Assembly;	//어셈블리 선언
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
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_TODOETC");
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

                    listobj.Add(info);

                    if (listobj.Count >= 5) break; // 최근 5건만 표시한다
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Portal-GetToDoList()", ex);
                throw;
            }
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

        #endregion 일정관리

        #region 게시판

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 게시판 최신글
        /// 작성일자 : 2011-07-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBoardList(string p)
        {
            List<object> listobj = new List<object>();

            int titleStyle = 1;

            SetDecryptString(p);
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string searchValue = GetDecryptParam("SearchValue", "");
            string pCatID = GetDecryptParam("PCatID", "0");

            if (pCatID == "0") return listobj;

            try
            {
                object[] rowValues = new object[10];

                //통합게시판 회사아이디
                int boardComID = Board.Ws.BLL.Category.GetCompanyCategoryInfo(pCatID.ToNumber<int>());

                Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category bll = new Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category();
                Zeospace.DeskPlusMobile.Web.Board.Ws.BE.CategoryInfo be = bll.GetCategoryOptionEtc(boardComID, Convert.ToInt32(pCatID), UserID, UserInfo.DeptID);

                if (be.Auth != "")
                {
                    Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List listBiz = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List();

                    string isAuthority = "";
                    string targetCategory = "";

                    //보기용이라면~
                    if (be.OptionUseView.Equals("1"))
                    {
                        string childCatList = bll.GetChildCategory(boardComID, pCatID.ToNumber<int>());

                        if (childCatList.Length > 0)
                        {
                            foreach (string catID in childCatList.Split(','))
                            {
                                isAuthority = bll.GetCategoryAuthority(boardComID, catID.ToNumber<int>(), UserID, UserInfo.DeptID, "1", false);

                                //해당 카테고리 게시판을 볼 수 있는지 확인
                                if (isAuthority != "")
                                {
                                    targetCategory += catID + ",";
                                }
                            }

                            targetCategory = targetCategory.TrimEnd(',');
                        }
                    }
                    else
                    {
                        //일반 카테고리라면..
                        isAuthority = bll.GetCategoryAuthority(boardComID, pCatID.ToNumber<int>(), UserID, UserInfo.DeptID, "1", false);
                        if (isAuthority != "") targetCategory = pCatID;
                    }

                    this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
                    this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.Ws.GetPortal).Assembly;	//어셈블리 선언
                    this.GetResourceManager();

                    DataSet dsList = listBiz.GetBoardList(boardComID, targetCategory, pageNo, pageSize, be.OptionUseDept, UserInfo.DeptID.ToString(), UserID, searchValue);

                    if (dsList != null)
                    {
                        foreach (DataRow dr in dsList.Tables[0].Rows)
                        {
                            if (listobj.Count >= 5) break; // 최근 5건만 표시한다

                            BE.BoardInfo info = new BE.BoardInfo();

                            info.CompanyID = Convert.ToInt32(dr["CompanyID"].ToString());
                            info.CategoryID = Convert.ToInt32(dr["CategoryID"].ToString());
                            info.ListID = Convert.ToInt32(dr["ListID"].ToString());
                            info.Depth = dr["Depth"].ToString().ToNumber<int>();
                            info.Title = dr["Title"].ToString();
                            info.Title = GetTitle(info, info.Title);

                            //말머리
                            string headerName = this.GetLangName(
                                    dr["HeaderBase"].ToString().ToNull(), dr["HeaderKo"].ToString().ToNull(),
                                    dr["HeaderEn"].ToString().ToNull(), dr["HeaderJa"].ToString().ToNull(),
                                    dr["HeaderFr"].ToString().ToNull(), dr["HeaderZhcn"].ToString().ToNull(),
                                    dr["HeaderZhtw"].ToString().ToNull(), PreLang);
                            info.HeaderName = (headerName.Length > 0) ? "[" + headerName + "]" : "";

                            info.Title = (info.HeaderName.Length > 0) ?
                                info.HeaderName + " " + info.Title : info.Title;
                            info.ReplyCount = (dr["ReplyCount"].ToString() == "0") ? "" : "[" + dr["ReplyCount"].ToString() + "]";
                            info.ViewCount = Convert.ToInt32(dr["ViewCount"]);

                            if (dr["OptionAnonymity"].ToString() == "1") //익명
                            {
                                string AnonymityName = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "AnonymityName");
                                info.WriteUserInfo = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
                            }
                            else
                            {
                                string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                                dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                                string titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                                                dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                                string deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                                                dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                                info.WriteUserInfo = deptName + " " + titleName + " " + name;
                            }

                            info.WriteDateAll = Convert.ToDateTime(dr["WriteDate"]).ToString();
                            info.TitlStyle = titleStyle.ToString();
                            info.OptionTop = (dr["OptionTop"].ToString() == "1") ? resMng.GetString("OptionTop") : "";

                            info.CategoryName = this.GetLangName(dr["CNameBase"].ToString(), dr["CNameKo"].ToString(), dr["CNameEn"].ToString(),
                                                dr["CNameJa"].ToString(), dr["CNameFr"].ToString(), dr["CNameZhCn"].ToString(), dr["CNameZhTw"].ToString(), PreLang);

                            #region 날짜 비교(New Img)

                            DateTime writeDate = Convert.ToDateTime(dr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                            info.ShowNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";

                            #endregion 날짜 비교(New Img)

                            titleStyle += 1;

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                //Helper.AppException.WriteException("Portal-GetBoardList()", ex);
                throw;
            }
        }

        /// <summary>
        /// 게시판 제목
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_title">The _title.</param>
        /// <returns></returns>
        private string GetTitle(BE.BoardInfo _info, string _title)
        {
            string title = _title;
            string arrowURl = "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_arrow.gif' title='' />";

            string space = "";
            for (int i = 0; i < _info.Depth; i++) space += "&nbsp;&nbsp;";

            //답변글 일 경우
            if (_info.Depth > 0) title = arrowURl + " " + title;
            if (space.Length > 0) title = space + title;

            return title;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2011-07-12
        /// 작성목적 : 게시판 권한테이블에서 회사,부서,개인 아이디값 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        private string GetMyAutority(int _categoryID)
        {
            authority = "";

            //1. 게시판 카테고리에 해당된 회사,부서,개인 아이디값 가져오기
            Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Authority data = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Authority();
            DataSet ds = new DataSet();
            ds = data.GetAuthority(UserInfo.CompanyID, _categoryID);

            DataView dv = ds.Tables[0].DefaultView;

            string deptMemberIDs;
            string authValue = "";
            //string tempReadWrite = "";
            string[] tempReadWriteStr = null;
            bool authcheck = false;
            int order = 0;

            StringBuilder tempReadWriteSb = new StringBuilder();

            /*
             *
             * 1. 개인권한이 1순위
             * 2. 부서권한이 2순위
             * 3. 회사권한이 3순위
             */

            for (int i = 0; i < dv.Count; i++)
            {
                authcheck = false;

                //1. 개인아이디값이 권한테이블에 있는지 체크
                if (dv[i]["Type"].ToString().Equals("M"))
                {
                    if (UserID.ToString().Equals(dv[i]["OwnerID"].ToString()))
                    {
                        order = 1;
                        authcheck = true;
                        authValue = dv[i]["Authority"].ToString();
                    }
                }

                //2. 개인아이디가 없으면, 부서아이디로 권한을 체크한다.
                if (authcheck == false)
                {
                    if (dv[i]["Type"].ToString() == "G" && dv[i]["OwnerID"].ToString() != "0") //부서로 권한이 할당되어 있는 경우, 재귀함수로 하위디렉토리 검색해서 개인아이디가 있는지 확인
                    {
                        //해당 부서의 값으로 하위 디렉토리를 재귀함수 돌릴 경우, 부서아이디와 맴버아이이가 콤마로 분류되어 있음(무지하게 많음)
                        //부서아이디를 넣어서 맴버를 뽑아온다.
                        deptMemberIDs = GetAuthorityDeptMemeber(Convert.ToInt32(dv[i]["OwnerID"])).ToString();

                        if (deptMemberIDs.Length > 0)
                        {
                            deptMemberIDs = "," + deptMemberIDs + ",";
                            if (deptMemberIDs.IndexOf("," + UserID.ToString() + ",") > 0)
                            {
                                order = 2;
                                //나의 아이디값이 들어 있다면
                                authcheck = true;
                                authValue = dv[i]["Authority"].ToString();
                            }
                        }
                    }
                }

                //3. 둘다 없으면 , 회사아이디로 찾는다.
                if (authcheck == false)
                {
                    //회사 아이디는 0으로 저장되어 있음
                    if (dv[i]["Type"].ToString() == "G" && dv[i]["OwnerID"].ToString() == "0")
                    {
                        order = 3;
                        authcheck = true;
                        authValue = dv[i]["Authority"].ToString();
                    }
                }

                if (authcheck)
                {
                    //임시 저장 = 권한값 + 권한순서
                    tempReadWriteSb.Append(order + ":" + authValue + ",");
                }
            }

            tempReadWriteStr = tempReadWriteSb.ToString().TrimEnd(',').Split(',');

            StringBuilder sb = new StringBuilder();

            //권한값이 있을 경우
            if (tempReadWriteSb.ToString().Length > 0)
            {
                //hash table 보다 느리지만, 동일한 키값이 포함될 수 있기 때문에 사용함!!!
                NameValueCollection nvc = new NameValueCollection();
                nvc.Clear();

                //1:읽기, 2:쓰기, 3: 읽기+꼬리말쓰기, 4:읽기+쓰기+꼬리말쓰기
                for (int i = 0; i < tempReadWriteStr.Length; i++)
                {
                    // 0 : 권한값, 1 : 권한순서
                    string[] modeOrderStr = tempReadWriteStr[i].ToString().Split(':');
                    nvc.Add(modeOrderStr[0].ToString(), modeOrderStr[1].ToString());
                }

                //1순위 (사람)
                if (nvc["1"] != "" && nvc["1"] != null)
                {
                    if (nvc["1"].IndexOf(",") > 0) //사람이 다중일 경우 콤마로 나옴
                    {
                        string[] nvcStr = nvc["1"].Split(',');
                        sb.Append("true/" + nvcStr[0].ToString());
                    }
                    else
                    {
                        sb.Append("true/" + nvc["1"]);
                    }
                } //2순위 (부서)
                else if (nvc["2"] != "" && nvc["2"] != null)
                {
                    if (nvc["2"].IndexOf(",") > 0)
                    {
                        string[] nvcStr = nvc["2"].Split(',');
                        sb.Append("true/" + nvcStr[0].ToString());
                    }
                    else
                    {
                        sb.Append("true/" + nvc["2"]);
                    }
                } //3순위 (회사)
                else if (nvc["3"] != "" && nvc["3"] != null)
                {
                    if (nvc["3"].IndexOf(",") > 0)
                    {
                        string[] nvcStr = nvc["3"].Split(',');
                        sb.Append("true/" + nvcStr[0].ToString());
                    }
                    else
                    {
                        sb.Append("true/" + nvc["3"]);
                    }
                }
            }

            if (sb.ToString() == "")
            {
                sb.Append("false/");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 해당 카테고리 트리에 바인딩할 권한이 있는지 체크
        /// 작성자     : famo23
        /// 최초작성일 : 2011-07-12
        /// </summary>
        /// <param name="dbRow">The db row.</param>
        /// <param name="isAuthority">The is authority.</param>
        /// <returns></returns>
        private bool CheckCategoryAuth(DataRow dbRow, string isAuthority)
        {
            bool response = false;
            //권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
            //string isAuthority = IsAutority(Convert.ToInt32(dbRow["CategoryID"]));

            //일반 게시판일 경우
            if (dbRow["OptionUseDept"].ToString().Equals("0"))
            {
                //권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한
                string[] isAuthorityStr = isAuthority.Split('/');

                //권한이 있을 경우
                if (Convert.ToBoolean(isAuthorityStr[0].ToString()))
                {
                    //읽기 권한 - 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
                    if (Convert.ToInt32(isAuthorityStr[1].ToString()) > 0)
                    {
                        response = true;
                    }
                }
            }
            else //부서별 게시판일 경우 : 권한 상관이 볼 수 있음
            {
                response = true;
            }

            return response;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2011-07-12
        /// 작성목적 : Gets the authority dept memeber.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_ownerID">The _owner ID.</param>
        /// <returns></returns>
        private string GetAuthorityDeptMemeber(int _ownerID)
        {
            authority = ""; //하위부서 및 사람들 가져옴, 재호출시 clear 해야 함

            DataSet ds = new DataSet();

            if (dsGetDeptUserTree != null)
            {
                ds = dsGetDeptUserTree;
            }
            else
            {
                Account.Data.Dept dept = new Zeospace.DeskPlusMobile.Web.Account.Data.Dept();
                ds = dept.GetDeptUserTree(UserInfo.CompanyID);
                ds.Relations.Add("DeptRelation", ds.Tables[0].Columns["OwnerID"], ds.Tables[0].Columns["ParentOwnerID"], false);
                //ds.WriteXml("C:/wmpub/love.xml");

                dsGetDeptUserTree = ds;
            }

            //dataset of tvData 을 foreach 함
            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                //나의 부서와 맞은 하나의 부서의 하위부서들을 재귀함수로 돌려서 viewstate 에 담는다.
                //부서는 G다음에 아이디값, 맴버는 아이디만 표기
                if (dbRow["OwnerID"].ToString() == "G" + _ownerID.ToString())
                {
                    authority += dbRow["OwnerID"].ToString();
                    PopulateSubNode(dbRow);
                }
            }

            return authority.ToString();
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2011-07-12
        /// 작성목적 : 하위 부서 재귀함수
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        private void PopulateSubNode(DataRow _dbRow)
        {
            //자식값을 가져오기
            foreach (DataRow childRow in _dbRow.GetChildRows("DeptRelation"))
            {
                authority += "," + childRow["OwnerID"].ToString();
                //자식값을 재귀함수 이용해서 추출해 오기
                PopulateSubNode(childRow);
            }
        }

        #endregion 게시판

        #region 전자결재

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 전자결재  최신 글
        /// 작성일자 : 2011-07-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetApprovalList(string p)
        {
            SetDecryptString(p);
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string targetUID = GetDecryptParam("TargetUID", "0");

            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.Ws.GetPortal).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_SelectApprove");

                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@PageNo", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@PageSize", SqlDbType.VarChar, 10)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                };
                parm[0].Value = CompanyID;
                parm[1].Value = targetUID;
                parm[2].Value = pageNo;
                parm[3].Value = pageSize;
                parm[4].Value = "";

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);

                List<object> listobj = new List<object>();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.ApprovalInfo info = new BE.ApprovalInfo();

                    info.RowNum = Convert.ToInt32(dr["RowNum"].ToString());
                    info.DocumentID = Convert.ToInt32(dr["DocumentID"].ToString());

                    #region 이름 다국어 설정

                    info.NameBase = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);

                    #endregion 이름 다국어 설정

                    #region 부서 다국어 설정

                    info.DeptBase = this.GetLangName(
                            dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                            dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                            dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                            dr["DeptZhtw"].ToString().ToNull(), PreLang);

                    #endregion 부서 다국어 설정

                    #region 직급 다국어 설정

                    info.TitleBase = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);

                    #endregion 직급 다국어 설정

                    info.Subject = dr["Subject"].ToString();
                    info.WriteTime = Convert.ToDateTime(dr["WriteTime"].ToString()).ToString();
                    info.ApprovalState = dr["ApprovalState"].ToString();
                    info.ApprovalTag = dr["ApprovalTag"].ToString();
                    info.AccountTag = dr["AccountTag"].ToString();
                    info.Number2 = dr["Number2"].ToString();
                    info.Number3 = dr["Number3"].ToString();
                    info.Sequence = Convert.ToInt32(dr["Sequence"].ToString());
                    info.ApprovalType = dr["ApprovalType"].ToString();

                    if (info.ApprovalState == "D")
                    {
                        info.StateStr = resMng.GetString("State2");
                    }
                    else if (info.ApprovalState == "H")
                    {
                        info.StateStr = resMng.GetString("State3");
                    }
                    else if (info.ApprovalState == "I")
                    {
                        info.StateStr = resMng.GetString("State4");
                    }
                    else if (info.ApprovalState == "R")
                    {
                        info.StateStr = resMng.GetString("State5");
                    }
                    else if (info.ApprovalState == "W")
                    {
                        info.StateStr = resMng.GetString("State6");
                    }
                    else if (info.ApprovalState == "T")
                    {
                        info.StateStr = resMng.GetString("State7");
                    }
                    else if (info.ApprovalState == "P")
                    {
                        info.StateStr = resMng.GetString("State8");
                    }
                    else
                    {
                        info.StateStr = resMng.GetString("State1");
                    }

                    info.IsMobile = dr["IsMobile"].ToString();
                    info.IsGuard = dr["IsGuard"].ToString();

                    #region 날짜 비교(New Img)

                    DateTime writeDate = Convert.ToDateTime(dr["WriteTime"].ToString());
                    DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan gap = todayDate.Subtract(writeDate);
                    int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                    info.ShowNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";

                    #endregion 날짜 비교(New Img)

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetList('" + targetUID + "','" + pageNo + "','" + pageSize + ")", ex);
                throw;
            }
        }

        #endregion 전자결재

        #region 겸직 변경
        /// <summary>
        /// 겸직 변경
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetBaseDept(string p)
        {
            SetDecryptString(p);

            int companyID = GetDecryptParam("CompanyID", "").ToNumber<int>();
            int userID = GetDecryptParam("UserID", "").ToNumber<int>();
            int deptID = GetDecryptParam("DeptID", "").ToNumber<int>();
            int titleID = GetDecryptParam("CodeTitleID", "").ToNumber<int>();
            int dutyID = GetDecryptParam("CodeDutyID", "").ToNumber<int>();


            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "UPD_PROC_BASEDEPT");

                SqlParameter[] parm = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@UserID", SqlDbType.Int),
											   new SqlParameter("@DeptID", SqlDbType.Int),
                                               new SqlParameter("@CodeTitleID", SqlDbType.Int),
											   new SqlParameter("@CodeDutyID", SqlDbType.Int)
										   };
                parm[0].Value = companyID;
                parm[1].Value = userID;
                parm[2].Value = deptID;
                parm[3].Value = titleID;
                parm[4].Value = dutyID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);
                return "OK";
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("ModSchedule()", ex);
                throw;
            }
        }
        #endregion
    }
}
