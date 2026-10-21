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
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlusMobile.Web.Mail.Ws
{
    /// <summary>
    /// GetMail의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetMail : Helper.MobileBaseService
    {
        /// <summary>
        /// 
        /// 작성목적 : 안 읽은 편지 카운트 가져오기
        /// 작성일자 : 2011-11-11
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetMailTot()
        {
            if (WebMailID == "") return "!NULL";

            string tmpCount = string.Empty;
            string returnCode = string.Empty;
            string method = string.Empty;
            string param = string.Empty;
            string sXmlData = string.Empty;
            string paramMailCount = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailCount");

            try
            {
                method = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailCount");                

                #region 받은편지함
                param = string.Format(paramMailCount, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, "1", "4", "all", "");                

                sXmlData = UrlSendHelper.UsePost(mailWebServiceUrl, param);                                
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sXmlData);
                
                returnCode = xml.SelectSingleNode("/data/returnCode").InnerText;

                if (returnCode == "0")
                {
                    //성공                    
                    tmpCount += int.Parse(xml.SelectSingleNode("/data/result").InnerText).ToString("#,##0") + "^";
                }
                else
                {
                    //실패
                }
                #endregion

                #region 휴지통
                param = string.Format(paramMailCount, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, "6", "4", "all", "");                 
                sXmlData = UrlSendHelper.UsePost(mailWebServiceUrl, param);

                xml.LoadXml(sXmlData);                
                returnCode = xml.SelectSingleNode("/data/returnCode").InnerText;

                if (returnCode == "0")
                {
                    //성공                    
                    tmpCount += int.Parse(xml.SelectSingleNode("/data/result").InnerText).ToString("#,##0") + "^";
                }
                else
                {
                    //실패
                }
                #endregion

                #region 개인함

                param = string.Format(paramMailCount, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, "person", "4", "all", ""); 
                
                sXmlData = UrlSendHelper.UsePost(mailWebServiceUrl, param);
                xml.LoadXml(sXmlData);
                
                returnCode = xml.SelectSingleNode("/data/returnCode").InnerText;

                if (returnCode == "0")
                {
                    //성공                    
                    tmpCount += int.Parse(xml.SelectSingleNode("/data/result").InnerText).ToString("#,##0") + "^";   //실서버
                }
                else
                {
                    //실패
                }
                #endregion

                return tmpCount;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetMailTot", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// 작성목적 : 메일 리스트 가져오기
        /// 작성일자 : 2011-11-11
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMailList(string p)
        {
            SetDecryptString(p);
            string code = GetDecryptParam("Code", "0");
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "10");
            string searchValue = GetDecryptParam("MSv", "");
            string param = string.Empty;
            string paramMailList = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailList");

            try
            {
                int pageSum = pageNo.ToNumber<int>() * pageSize.ToNumber<int>();

                string method = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailList");
                //param = string.Format(paramMailList, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, code, "0", pageSize, pageSum, "all", Server.UrlEncode(searchValue));
                //param = string.Format(paramMailList, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, code, "0", pageSize, pageSum, "all", Helper.Security.AES.Base64.Endode(searchValue));
                param = string.Format(paramMailList, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, code, "0", pageSize, pageSum, "all", Helper.Security.AES.Base64Convert.Encode(searchValue));
                
                Helper.AppException.WriteException("mailWebServiceUrl: " + mailWebServiceUrl);
                Helper.AppException.WriteException("param: " + param);
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
                
                foreach(XmlNode node in xmlList)
                {
                    BE.ListInfo info = new BE.ListInfo();

                    info.RowNum = 0;
                    info.mail_sn = node["uid"].InnerText;
                    info.mail_subject = node["subject"].InnerText;
                    string[] ArrTo = node["to"].InnerText.Split(',');

                    string tmpTo = ArrTo[0];

                    if (ArrTo.Length > 1)
                    {
                        tmpTo = tmpTo + " (" + ArrTo.Length + ")";
                    }
                    info.to_header = tmpTo;

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

                    info.state_code = "";
                    info.create_time = node["sendDate"].InnerText;
                    info.mailbox_code = code;
                    info.read_flag = node["isRead"].InnerText;
                    

                    //deepMail 용 추가 필드 정의 
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
                Helper.AppException.WriteException("GetMailList", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// 작성목적 : 개인 문서함 정보 가져오기
        /// 작성일자 : 2011-07-09
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMailBoxList()
        {
            string paramMailBoxList = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailBoxList"); 
            string param = string.Empty;

            try
            {                
                param = string.Format(paramMailBoxList, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword); //act/domain/id/pw

                string sXmlData = UrlSendHelper.UsePost(mailWebServiceUrl, param);   // 실서버

                XmlNodeList xmlList = null;
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sXmlData);

                string returnCode = xml.SelectSingleNode("/data/returnCode").InnerText;

                if(returnCode == "0")
                {
                    //성공
                }
                else
                {
                    //실패
                }

                xmlList = xml.SelectNodes("/data/result/item");
                List<object> listobj = new List<object>();
                
                
                int tmpLevels;
                string tmpMailCount;
                foreach (XmlNode node in xmlList)
                {
                    if (node["depth"].InnerText == "0")
                    {
                        BE.MailBoxInfo info = new BE.MailBoxInfo();
                        info.MailCount = int.Parse(node["unseenCount"].InnerText);
                        info.mailbox_code = node["uid"].InnerText;                        

                        tmpMailCount = " (0)";

                        if (info.MailCount == 0)
                        {
                            tmpMailCount = " (0)";
                        }
                        else
                        {
                            tmpMailCount = " (" + node["unseenCount"].InnerText + ")";
                        }

                        info.mailbox_name = node["name"].InnerText + tmpMailCount;
                        //string tmpCode = node["puid"].InnerText;
                        listobj.Add(info);
                    }
                    else
                    {
                        tmpLevels = int.Parse(node["depth"].InnerText) + 1;

                        BE.MailBoxInfo childinfo = new BE.MailBoxInfo();
                        childinfo.MailCount = int.Parse(node["unseenCount"].InnerText);
                        childinfo.levels = tmpLevels.ToString(); ;
                        childinfo.mailbox_code = node["uid"].InnerText;                        

                        tmpMailCount = " (0)";

                        if (childinfo.MailCount == 0)
                        {
                            tmpMailCount = " (0)";
                        }
                        else
                        {
                            tmpMailCount = " (" + node["unseenCount"].InnerText + ")";
                        }

                        string tmpNbsp = "";
                        for (int i = 2; i < tmpLevels; i++)
                        {
                            tmpNbsp += "&nbsp;&nbsp;&nbsp;";
                        }

                        childinfo.mailbox_name = tmpNbsp + "└ " + node["name"].InnerText + tmpMailCount;
                        listobj.Add(childinfo);

                    }

                }

                return listobj;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetMailBoxList", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// 작성목적 : 휴지통 리스트 가져오기
        /// 작성일자 : 2011-11-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetTrashList(string p)
        {
            SetDecryptString(p);
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string param = string.Empty;
            string paramMailList = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailList");
            try
            {
                int pageSum = pageNo.ToNumber<int>() * pageSize.ToNumber<int>();

                param = string.Format(paramMailList, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, "6", "0", pageSize, pageSum, "all", "");
                
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
                    BE.ListInfo info = new BE.ListInfo();

                    info.RowNum = 0;
                    info.mail_sn = node["uid"].InnerText;
                    info.mail_subject = node["subject"].InnerText;
                    info.to_header = "";

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

                    info.state_code = "";
                    info.create_time = node["sendDate"].InnerText;
                    info.mailbox_code = "";
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

                    var mailAddress = UserInfo.MailID + "@" + UserInfo.MailDomain;
                    info.type = "receive";
                    if (info.cc.IndexOf(mailAddress) >= 0 || info.cc.IndexOf(UserInfo.Name) >= 0)
                    {
                        info.type = "refer";
                    }
                    else if (info.bcc.IndexOf(mailAddress) > 0 || info.cc.IndexOf(UserInfo.Name) > 0)
                    {
                        info.type = "refer2";
                    }

                    listobj.Add(info);                    
                }                

                return listobj;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetTrashList", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// 작성목적 : 메일 내용 가져오기
        /// 작성일자 : 2011-11-11
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetContent(string p)
        {
            SetDecryptString(p);
            string code = GetDecryptParam("Code", "0");
            string mail_sn = GetDecryptParam("Mail_sn", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_QRY_ZTMESSAGE_MESSAGE_SN");
            sqlQuery = String.Format(sqlQuery, WebMailID, code, mail_sn);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_MAIL, CommandType.Text, sqlQuery, "GetContent", null);

            DataTable dt = ds.Tables[0];


            DataColumn dc1 = new DataColumn("bodyNoStyle", typeof(string));
            HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
            string mailContent = ds.Tables[0].Rows[0]["body"].ToString().Trim();

            mailContent = Regex.Replace(mailContent, "<script", "<xscript", RegexOptions.IgnoreCase);
            mailContent = Regex.Replace(mailContent, "</script>", "</xscript>", RegexOptions.IgnoreCase);
            mailContent = Regex.Replace(mailContent, "<form", "<xform", RegexOptions.IgnoreCase);
            mailContent = Regex.Replace(mailContent, "</form>", "</xform>", RegexOptions.IgnoreCase);
            mailContent = Regex.Replace(mailContent, "<v:imagedata", "<IMG", RegexOptions.IgnoreCase);
            mailContent = Regex.Replace(mailContent, "</v:imagedata>", "", RegexOptions.IgnoreCase);

            //메일 본문안의 Style 제거
            mailContent = Regex.Replace(mailContent, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
            mailContent = Regex.Replace(mailContent, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
            mailContent = Regex.Replace(mailContent, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

            hdoc.LoadHtml(mailContent);
            var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출
            if (bodyNode != null)
            {
                foreach (HtmlNode post in hdoc.DocumentNode.SelectNodes("//body"))
                {
                    dc1.DefaultValue += post.InnerHtml;
                }
            }
            else
            {
                dc1.DefaultValue = mailContent;
            }

            dt.Columns.Add(dc1);


            return StringHelper.GetJSONString(dt);
        }

        /// <summary>
        /// 
        /// 작성목적 : 메일 내용 앞뒤 가져오기
        /// 작성일자 : 2011-11-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetBeforeNext(string p)
        {
            SetDecryptString(p);
            string code = GetDecryptParam("Code", "0");
            string mail_sn = GetDecryptParam("Mail_sn", "0");
            string gubun = GetDecryptParam("Gubun", "0");
            
            try
            {

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_PROC_ZTMESSAGE");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@user_sn", SqlDbType.Int),
                                                            new SqlParameter("@Mailbox_code", SqlDbType.Char,4),
                                                            new SqlParameter("@mail_sn", SqlDbType.Int),
                                                            new SqlParameter("@Gubun", SqlDbType.VarChar,10)
                };

                parms[0].Value = WebMailID;
                parms[1].Value = code;
                parms[2].Value = mail_sn;
                parms[3].Value = gubun;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_MAIL, CommandType.StoredProcedure, proc, parms);

                return Convert.ToInt32(obj);                
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Mail_GetBeforeNext: " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 
        /// 작성목적 : 임시 저장 메일 가져오기
        /// 작성일자 : 2011-07-11
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDraft(string p)
        {
            SetDecryptString(p);
            string mailID = GetDecryptParam("MailID", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_QRY_MESSAGE");
            sqlQuery = String.Format(sqlQuery, CompanyID, UserID, mailID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetDraft", null);

            return StringHelper.GetJSONString(ds.Tables[0]);
        }

        /// <summary>
        /// 
        /// 작성목적 : 휴지통 메일 내용 앞뒤 가져오기
        /// 작성일자 : 2011-11-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetBeforeNextTrash(string p)
        {
            SetDecryptString(p);
            string mail_sn = GetDecryptParam("Mail_sn", "0");
            string gubun = GetDecryptParam("Gubun", "0");

            try
            {
                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_PROC_ZTMESSAGE_TRASH");

                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@user_sn", SqlDbType.Int),
                                                            new SqlParameter("@mail_sn", SqlDbType.Int),
                                                            new SqlParameter("@Gubun", SqlDbType.VarChar,10)
                };

                parms[0].Value = WebMailID;
                parms[1].Value = mail_sn;
                parms[2].Value = gubun;

                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_MAIL, CommandType.StoredProcedure, proc, parms);

                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Mail_GetBeforeNextTrash: " + errMsg);
                throw;
            }
        }


        /// <summary>
        /// 
        /// 작성목적 : 메일 첨부파일 가져오기
        /// 작성일자 : 2011-11-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFile(string p)
        {
            SetDecryptString(p);
            string mail_sn = GetDecryptParam("Mail_sn", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_QRY_ZTMESSAGE_ATTACHFILE");
            sqlQuery = String.Format(sqlQuery, mail_sn);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_MAIL, CommandType.Text, sqlQuery, "GetAttachFile", null);

            return StringHelper.GetJSONString(ds.Tables[0]);
        }

        /// <summary>
        /// 
        /// 작성목적 : 메일 받는 사람, 참조 가져오기
        /// 작성일자 : 2011-11-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetHearder(string p)
        {
            SetDecryptString(p);
            string mail_sn = GetDecryptParam("Mail_sn", "0");

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_QRY_ZTMESSAGE_RECIPIENT");
            sqlQuery = String.Format(sqlQuery, mail_sn);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_MAIL, CommandType.Text, sqlQuery, "GetHearder", null);

            return StringHelper.GetJSONString(ds.Tables[0]);
        }

        
        /// <summary>
        /// 
        /// 작성목적 : 메일 내용 가져오기
        /// 작성일자 : 2011-11-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetTotalContent(string p)
        {
            SetDecryptString(p);
            string code = GetDecryptParam("Code", "0");            
            string mail_sn = GetDecryptParam("Mail_sn", "0");
            string searchValue = GetDecryptParam("MSv", "");
            
            string mailWebServiceUrl = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailWebServiceUrl");
            string paramMailRead = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailRead");
            
            string param = string.Format(paramMailRead, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, code, mail_sn, "all", Helper.Security.AES.Base64Convert.Encode(searchValue));

            Helper.AppException.WriteException("mailWebServiceUrl: " + mailWebServiceUrl);
            Helper.AppException.WriteException("param: " + param);
            string sXmlData = UrlSendHelper.UsePost(mailWebServiceUrl, param);

            XmlNode xmlNode;
            XmlNodeList xmlAttachList;
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(sXmlData);

            string returnCode = xml.SelectSingleNode("/data/returnCode").InnerText;

            if(returnCode == "0")
            {
                //성공
            }
            else
            {
                //실패
            }

            xmlNode = xml.SelectSingleNode("/data/result/item");

            //본문내용 처리
            List<object> listobj = new List<object>();
            BE.ContentInfo info = new BE.ContentInfo();

            info.uid = xmlNode["uid"].InnerText;
            info.prevUid = xmlNode["prevUid"].InnerText;
            info.nextUid = xmlNode["nextUid"].InnerText;
            info.subject = xmlNode["subject"].InnerText;
            info.from = xmlNode["from"].InnerText;
            info.to = xmlNode["to"].InnerText;
            info.cc = xmlNode["cc"].InnerText;
            info.bcc = xmlNode["bcc"].InnerText;
            info.sendDate = xmlNode["sendDate"].InnerText;
            info.recvDate = xmlNode["recvDate"].InnerText;
            info.flag = xmlNode["flag"].InnerText;
            info.content = xmlNode["content"].InnerText;

            xmlAttachList = xmlNode.SelectNodes("attachList/attachFile");
            BE.ContentInfo.AttachList[] attachListTmp = new BE.ContentInfo.AttachList[xmlAttachList.Count];

            int tmpIndex = 0;
            foreach (XmlNode node in xmlAttachList)
            {
                //첨부파일 처리
                attachListTmp[tmpIndex].name = node["name"].InnerText;
                attachListTmp[tmpIndex].size = node["size"].InnerText;
                attachListTmp[tmpIndex].downloadUrl = node["downloadUrl"].InnerText;
                tmpIndex++;
            }

            info.attachLists = attachListTmp;

            listobj.Add(info);            

            return listobj;
        }

        /// <summary>
        /// 
        /// 작성목적 : 발송승인자 가져오기
        /// 작성일자 : 2013-03-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetRequest()
        {
            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_QRY_REQUEST");
                sqlQuery = String.Format(sqlQuery, WebMailID);

                return Convert.ToInt32(SQLHelper.ExecuteScalar(SQLHelper.CONN_MAIL, CommandType.Text, sqlQuery, null));

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetRequest: " + errMsg);
                return -1;
            }
        }

        /// <summary>
        /// 
        /// 작성목적 : 자동완성
        /// 작성일자 : 2015-08-05
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAutoCompleteData(string p)
        {
            try
            {
                SetDecryptString(p);
                string txt = GetDecryptParam("txt", "0");

                string before = txt.Trim();
                string tmpPrefix = txt.Trim();
                string[] ArrPrefix = txt.Split(',');

                if (ArrPrefix.Length > 1)
                {
                    tmpPrefix = ArrPrefix[ArrPrefix.Length - 1].Trim();
                    int index = txt.LastIndexOf(",");
                    before = txt.Substring(0, index + 1);
                }
                else
                {
                    before = "";
                }

                List<object> listobj = new List<object>();

                if (tmpPrefix.Trim() != "")
                {
                    string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_PROC_USER");

                    SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                     };
                    parm[0].Value = CompanyID;
                    parm[1].Value = UserID;
                    parm[2].Value = tmpPrefix;

                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BE.SearchInfo info = new BE.SearchInfo();
  
                        info.Name = "\"" + dr["mName"].ToString() + "\"<" + dr["Mail"].ToString() + ">,";  
                        info.MailValue = before + info.Name;

                        listobj.Add(info);

                    }
                }

                return listobj;

            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetAutoCompleteData", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 게시판 첨부파일 가져오기
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSpoolList(string p)
        {
            SetDecryptString(p);
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "0");

            try
            {
                //string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/MailMobile/" + swfUploadGUID + "/");
                string uploadPath = Server.MapPath("/DeskPlusMobile/MailAttach/" + CompanyID + "/Users/" + UserID + "/" + swfUploadGUID + "/");

                List<object> listobj = new List<object>();

                DirectoryInfo DirInfo = new DirectoryInfo(uploadPath);

                var files = new DirectoryInfo(uploadPath).EnumerateFiles().OrderBy(f => f.LastWriteTime);

                string sourceFile = ""; 
                string fileTag = "";
                string fileTagStr = "";
                string BasefilePath = "";

                foreach (var item in files)
                {
                    if (item.Name != "Thumbs.db")
                    {
                        
                        sourceFile = uploadPath + item.Name;
                        FileInfo sourceFileInfo = new FileInfo(sourceFile);

                        BE.AttachInfo info = new BE.AttachInfo();
                        info.CompanyID = CompanyID;
                        info.FileName = sourceFileInfo.Name;
                        info.FileType = sourceFileInfo.Extension;
                        //BasefilePath = "/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/MailMobile/" + swfUploadGUID + "/" + sourceFileInfo.Name;
                        BasefilePath = "/DeskPlusMobile/MailAttach/" + CompanyID + "/Users/" + UserID + "/" + swfUploadGUID + "/" + sourceFileInfo.Name;

                        fileTag = "<img src='{0}' width='30px' height='30px' />";
                        fileTagStr = string.Format(fileTag, BasefilePath);

                        info.FilePath = fileTagStr;

                        listobj.Add(info);
                        
                    }
                }


                return listobj;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetSpoolList", ex);
                throw;
            }
        }
    }
}
