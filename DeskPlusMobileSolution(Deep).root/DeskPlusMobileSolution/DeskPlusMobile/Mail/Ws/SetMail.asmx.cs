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
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlusMobile.Web.Mail.Ws
{
    /// <summary>
    /// SetMail의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetMail : Helper.MobileBaseService
    {

        /// <summary>
        /// 
        /// 작성목적 : 메일 삭제하기
        /// 작성일자 : 2011-11-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string Del(string p)
        {
            SetDecryptString(p);
            string code = GetDecryptParam("Code", "0");
            string mail_sn = GetDecryptParam("Mail_sn", "0").TrimEnd(',');            
            
            try
            {
                string mailWebServiceUrl = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailWebServiceUrl");                
                string paramMailDelete = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailDelete");
                string param = string.Format(paramMailDelete, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, code, mail_sn);  //act/domain/id/pw/mbox/uid
                
                string sXmlData = UrlSendHelper.UsePostDev(mailWebServiceUrl, param);
                                
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

                return "OK";

            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("Mail-Del()", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// 작성목적 : 메일 복원하기
        /// 작성일자 : 2011-11-14
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string Restore(string p)
        {
            SetDecryptString(p);
            string Mail_sn = GetDecryptParam("Mail_sn", "0").TrimEnd(',');            

            try
            {
                string mailWebServiceUrl = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailWebServiceUrl");                
                string paramMailMove = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailMove");

                //메일 복원은 무조건 받은메일함으로 복구
                string param = string.Format(paramMailMove, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, "6", Mail_sn, "1"); //act/domain/id/pw/mbox/uid/targetMbox                
                string sXmlData = UrlSendHelper.UsePostDev(mailWebServiceUrl, param);

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

                return "OK";

            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("Mail-Restore()", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// 작성목적 : 메일 읽은 표시하기
        /// 작성일자 : 2011-11-11
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string Mod(string p)
        {
            SetDecryptString(p);
            string code = GetDecryptParam("Code", "0");
            string mail_sn = GetDecryptParam("Mail_sn", "0");    

            try
            {
                string mailWebServiceUrl = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailWebServiceUrl");                
                string paramMailStatus = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailStatus");
                string param = string.Format(paramMailStatus, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, code, mail_sn);  //act/domain/id/pw/mbox/uid
                
                string sXmlData = UrlSendHelper.UsePostDev(mailWebServiceUrl, param);
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
                
                return "OK";

            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("Mail-Mod()", ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// 작성목적 : 임시저장하기
        /// 작성일자 : 2011-07-11
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetDraft(string p)
        {
            SetDecryptString(p);

            string mailID = GetDecryptParam("MailID", "0");
            string to = GetDecryptParam("To", "");
            string cc = GetDecryptParam("Cc", "");
            string bcc = GetDecryptParam("Bcc", "");
            string subject = GetDecryptParam("Subject", "");
            string body = GetDecryptParam("Body", "");

            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            try
            {
                string tmpDate = System.DateTime.Now.Year.ToString() + "/" + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + "/" + System.DateTime.Now.Day.ToString().PadLeft(2, '0') + " " + System.DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + System.DateTime.Now.Minute.ToString().PadLeft(2, '0');
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "INS_PROC_MESSAGE");

                SqlParameter[] parm = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
                                               new SqlParameter("@MailID", SqlDbType.Int),
											   new SqlParameter("@UserID", SqlDbType.Int),
											   new SqlParameter("@Subject", SqlDbType.NVarChar, 250),
                                               new SqlParameter("@To_Header", SqlDbType.NText), 
                                               new SqlParameter("@Cc_Header", SqlDbType.NText),
											   new SqlParameter("@Bcc_Header", SqlDbType.NText),											   
											   new SqlParameter("@Body", SqlDbType.NText), 
											   new SqlParameter("@WriteDate", SqlDbType.NVarChar,80),
                                               new SqlParameter("@IsUseYN", SqlDbType.Char,1),
                                               new SqlParameter("@MailID2", SqlDbType.Int),
										   };
                parm[0].Value = CompanyID;
                parm[1].Value = 0;
                parm[2].Value = UserID;
                parm[3].Value = Server.UrlDecode(subject);
                parm[4].Value = to;
                parm[5].Value = cc;
                parm[6].Value = bcc;
                parm[7].Value = Server.UrlDecode(body);
                parm[8].Value = tmpDate;
                parm[9].Value = "Y";
                parm[10].Value = (mailID.Trim() == "") ? 0 : mailID.ToNumber<int>();

                parm[1].Direction = ParameterDirection.Output;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm);

                return parm[1].Value.ToString();
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("SetDraft()", ex);
                throw;
            }
        }

        public bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }

        /// <summary>
        /// 
        /// 작성목적 : 메일 쓰기
        /// 작성일자 : 2015-06-01
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SendMail(string p)
        {
            try
            {
                string rtn = "1";

                SetDecryptString(p);

                string tohearder = GetDecryptParam("To", "").Replace("\n", "");
                string cchearder = GetDecryptParam("Cc", "").Replace("\n", "");
                string bcchearder = GetDecryptParam("Bcc", "").Replace("\n", "");
                string subject = GetDecryptParam("Subject", "");
                string body = GetDecryptParam("Body", "");
                string forward = GetDecryptParam("Forward", "");
                string code = GetDecryptParam("Code", "1");
                string swfUploadGUID = GetDecryptParam("SwfUploadGUID", "");

                string strTo = String.Empty;
                string strCc = String.Empty;
                string strBcc = String.Empty;

                MailDataInfo mailDInfo = new MailDataInfo();

                string[] ArrTo = tohearder.TrimEnd(',').Split(',');

                if (ArrTo.Length > 0)
                {
                    for (int i = 0; i < ArrTo.Length; i++)
                    {
                        if ((ArrTo[i].IndexOf("<") < 0) && (ArrTo[i].IndexOf(">") < 0) && (ArrTo[i].IndexOf("@") < 0))
                        {
                            string tmpUser = ArrTo[i].Trim().Replace(",", "").Replace("\"", "");

                            if (tmpUser != "")
                            {
                                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_PROC_USERNAME");

                                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                            };
                                parm[0].Value = CompanyID;
                                parm[1].Value = UserID;
                                parm[2].Value = tmpUser;

                                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);

                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 1)
                                    {
                                        return "3^" + ArrTo[i];
                                    }
                                    else
                                    {
                                        foreach (DataRow dr in ds.Tables[0].Rows)
                                        {
                                            if (!mailDInfo.To.Contains(dr["Mail"].ToString()))
                                            {
                                                mailDInfo.To.Add(dr["Mail"].ToString(), dr["Mail"].ToString());
                                                strTo += dr["Mail"].ToString() + ",";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    return "4^" + ArrTo[i];
                                }
                            }
                        }
                        else
                        {
                            if ((ArrTo[i].IndexOf("<") > -1) && (ArrTo[i].IndexOf(">") > -1))
                            {
                                int startIdx = ArrTo[i].IndexOf("<");
                                int endIdx = ArrTo[i].IndexOf(">");
                                string tmpTo = ArrTo[i].Substring(startIdx + 1, endIdx - startIdx - 1);
                                if (IsValidEmail(tmpTo))
                                {
                                    if (!mailDInfo.To.Contains(tmpTo))
                                    {
                                        mailDInfo.To.Add(tmpTo, tmpTo);
                                        strTo += tmpTo + ",";
                                    }
                                }
                                else
                                {
                                    return "5^" + ArrTo[i];
                                }
                            }
                            else if ((ArrTo[i].IndexOf("<") > -1) && (ArrTo[i].IndexOf(">") < 0))
                            {
                                int startIdx = ArrTo[i].IndexOf("<");
                                string tmpTo = ArrTo[i].Substring(startIdx + 1, ArrTo[i].Length - 1);
                                if (IsValidEmail(tmpTo))
                                {
                                    if (!mailDInfo.To.Contains(tmpTo))
                                    {
                                        mailDInfo.To.Add(tmpTo, tmpTo);
                                        strTo += tmpTo + ",";
                                    }
                                }
                                else
                                {
                                    return "5^" + ArrTo[i];
                                }
                            }
                            else
                            {
                                string tmpTo = ArrTo[i].Replace(">", "");
                                if (IsValidEmail(tmpTo))
                                {
                                    if (!mailDInfo.To.Contains(tmpTo))
                                    {
                                        mailDInfo.To.Add(tmpTo, tmpTo);
                                        strTo += tmpTo + ",";
                                    }
                                }
                                else
                                {
                                    return "5^" + ArrTo[i];
                                }
                            }
                        }
                    }
                }

                string[] ArrCc = cchearder.TrimEnd(',').Split(',');

                if (ArrCc.Length > 0)
                {
                    for (int i = 0; i < ArrCc.Length; i++)
                    {
                        if ((ArrCc[i].IndexOf("<") < 0) && (ArrCc[i].IndexOf(">") < 0) && (ArrCc[i].IndexOf("@") < 0))
                        {
                            string tmpUser = ArrCc[i].Trim().Replace(",", "").Replace("\"", "");

                            if (tmpUser != "")
                            {
                                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_PROC_USERNAME");

                                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                            };
                                parm[0].Value = CompanyID;
                                parm[1].Value = UserID;
                                parm[2].Value = tmpUser;

                                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);


                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 1)
                                    {
                                        return "3^" + ArrCc[i];
                                    }
                                    else
                                    {
                                        foreach (DataRow dr in ds.Tables[0].Rows)
                                        {
                                            if (!mailDInfo.Cc.Contains(dr["Mail"].ToString()))
                                            {
                                                mailDInfo.Cc.Add(dr["Mail"].ToString(), dr["Mail"].ToString());
                                                strCc += dr["Mail"].ToString() + ",";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    return "4^" + ArrCc[i];
                                }
                            }
                        }
                        else
                        {
                            if ((ArrCc[i].IndexOf("<") > -1) && (ArrCc[i].IndexOf(">") > -1))
                            {
                                int startIdx = ArrCc[i].IndexOf("<");
                                int endIdx = ArrCc[i].IndexOf(">");
                                string tmpCc = ArrCc[i].Substring(startIdx + 1, endIdx - startIdx - 1);
                                if (IsValidEmail(tmpCc))
                                {
                                    if (!mailDInfo.Cc.Contains(tmpCc))
                                    {
                                        mailDInfo.Cc.Add(tmpCc, tmpCc);
                                        strCc += tmpCc + ",";
                                    }
                                }
                                else
                                {
                                    return "5^" + ArrCc[i];
                                }
                            }
                            else if ((ArrCc[i].IndexOf("<") > -1) && (ArrCc[i].IndexOf(">") < 0))
                            {
                                int startIdx = ArrCc[i].IndexOf("<");
                                string tmpCc = ArrCc[i].Substring(startIdx + 1, ArrCc[i].Length - 1);
                                if (IsValidEmail(tmpCc))
                                {
                                    if (!mailDInfo.Cc.Contains(tmpCc))
                                    {
                                        mailDInfo.Cc.Add(tmpCc, tmpCc);
                                        strCc += tmpCc + ",";
                                    }
                                }
                                else
                                {
                                    return "5^" + ArrCc[i];
                                }
                            }
                            else
                            {
                                string tmpCc = ArrCc[i].Replace(">", "");
                                if (IsValidEmail(tmpCc))
                                {
                                    if (!mailDInfo.Cc.Contains(tmpCc))
                                    {
                                        mailDInfo.Cc.Add(tmpCc, tmpCc);
                                        strCc += tmpCc + ",";
                                    }
                                }
                                else
                                {
                                    return "5^" + ArrCc[i];
                                }
                            }
                        }
                    }
                }

                string[] ArrBcc = bcchearder.TrimEnd(',').Split(',');

                if (ArrBcc.Length > 0)
                {
                    for (int i = 0; i < ArrBcc.Length; i++)
                    {
                        if ((ArrBcc[i].IndexOf("<") < 0) && (ArrBcc[i].IndexOf(">") < 0) && (ArrBcc[i].IndexOf("@") < 0))
                        {
                            string tmpUser = ArrBcc[i].Trim().Replace(",", "").Replace("\"", "");

                            if (tmpUser != "")
                            {
                                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMail, "SEL_PROC_USERNAME");

                                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@SearchValue", SqlDbType.NVarChar, 500)
                            };
                                parm[0].Value = CompanyID;
                                parm[1].Value = UserID;
                                parm[2].Value = tmpUser;

                                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "dsList", parm);


                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 1)
                                    {
                                        return "3^" + ArrBcc[i];
                                    }
                                    else
                                    {
                                        foreach (DataRow dr in ds.Tables[0].Rows)
                                        {
                                            if (!mailDInfo.Bcc.Contains(dr["Mail"].ToString()))
                                            {
                                                mailDInfo.Bcc.Add(dr["Mail"].ToString(), dr["Mail"].ToString());
                                                strBcc += dr["Mail"].ToString() + ",";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    return "4^" + ArrBcc[i];
                                }
                            }
                        }
                        else
                        {
                            if ((ArrBcc[i].IndexOf("<") > -1) && (ArrBcc[i].IndexOf(">") > -1))
                            {
                                int startIdx = ArrBcc[i].IndexOf("<");
                                int endIdx = ArrBcc[i].IndexOf(">");
                                string tmpBcc = ArrBcc[i].Substring(startIdx + 1, endIdx - startIdx - 1);
                                if (IsValidEmail(tmpBcc))
                                {
                                    if (!mailDInfo.Bcc.Contains(tmpBcc))
                                    {
                                        mailDInfo.Bcc.Add(tmpBcc, tmpBcc);
                                        strBcc += tmpBcc + ",";
                                    }
                                }
                                else
                                {
                                    return "5^" + ArrBcc[i];
                                }
                            }
                            else if ((ArrBcc[i].IndexOf("<") > -1) && (ArrBcc[i].IndexOf(">") < 0))
                            {
                                int startIdx = ArrBcc[i].IndexOf("<");
                                string tmpBcc = ArrBcc[i].Substring(startIdx + 1, ArrBcc[i].Length - 1);
                                if (IsValidEmail(tmpBcc))
                                {
                                    if (!mailDInfo.Bcc.Contains(tmpBcc))
                                    {
                                        mailDInfo.Bcc.Add(tmpBcc, tmpBcc);
                                        strBcc += tmpBcc + ",";
                                    }
                                }
                                else
                                {
                                    return "5^" + ArrBcc[i];
                                }
                            }
                            else
                            {
                                string tmpBcc = ArrBcc[i].Replace(">", "");
                                if (IsValidEmail(tmpBcc))
                                {
                                    if (!mailDInfo.Bcc.Contains(tmpBcc))
                                    {
                                        mailDInfo.Bcc.Add(tmpBcc, tmpBcc);
                                        strBcc += tmpBcc + ",";
                                    }
                                }
                                else
                                {
                                    return "5^" + ArrBcc[i];
                                }
                            }
                        }
                    }
                }

                strTo = strTo.TrimEnd(',');
                strCc = strCc.TrimEnd(',');
                strBcc = strBcc.TrimEnd(',');

                UserInfoHelper userInfo = new UserInfoHelper(HttpContext.Current.User.Identity.Name);

                string strSubject = Server.UrlDecode(subject);

                string strBody = Server.UrlDecode(body);

                Helper.AppException.WriteAssert(AppSettingType.SQLMail, "Mail:SendMail subject " + strSubject);
                Helper.AppException.WriteAssert(AppSettingType.SQLMail, "Mail:SendMail strTo " + strTo);
                Helper.AppException.WriteAssert(AppSettingType.SQLMail, "Mail:SendMail strCc : " + strCc);
                Helper.AppException.WriteAssert(AppSettingType.SQLMail, "Mail:SendMail strBcc : " + strBcc);

                string mailWebServiceUrl = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailWebServiceUrl");
                string method = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailSend");
                string paramMailSend = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailSend");

                string tmpAttachs = string.Empty;
                //tmpAttachs = "D:\\DEEPSoft\\Postian\\attachs\\" + CompanyID + "\\Users\\" + UserID + "\\" + swfUploadGUID;
                tmpAttachs = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailAttachPath");
                tmpAttachs = tmpAttachs + CompanyID + "\\Users\\" + UserID + "\\" + swfUploadGUID;
                //tmpAttachs = "D:/DEEPSoft/Postian/attachs/" + CompanyID + "/Users/" + UserID + "/" + swfUploadGUID;

                //본문 인코딩 해야 함. modify shimpark
                string param = string.Format(paramMailSend, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, strTo,
                    strCc, strBcc, Server.UrlEncode(strSubject), code, forward, Server.UrlEncode(strBody), tmpAttachs);  //act/domain/id/pw/to/cc/bcc/subject/mbox/forward/content

                Helper.AppException.WriteAssert(AppSettingType.SQLMail, "Mail:SendMail param : " + mailWebServiceUrl + "?" + param);

                string sXmlData = UrlSendHelper.UsePostDev(mailWebServiceUrl, param);

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(sXmlData);

                string returnCode = xml.SelectSingleNode("/data/returnCode").InnerText;
                //string returnMsg = xml.SelectSingleNode("/data/returnMsg").InnerText;

                Helper.AppException.WriteAssert(AppSettingType.SQLMail, "Mail:SendMail returnCode : " + returnCode);

                if (returnCode == "0")
                {
                    //성공
                    rtn = "0";
                }
                else
                {
                    //실패
                    rtn = "1";
                }

                return rtn;
            }
            catch(Exception ex)
            {
                Helper.AppException.WriteException("SendMail()", ex);
                return "1";
            }
            

        }       

        #region 메일쓰기 자동완성 적용 전
        /*
        
        /// <summary>
        /// 
        /// 작성목적 : 메일 쓰기
        /// 작성일자 : 2011-07-11
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string SendMail(string p)
        {
            string rtn = "1";

            SetDecryptString(p);

            string to = GetDecryptParam("To", "").Replace("\n", "");
            string cc = GetDecryptParam("Cc", "").Replace("\n", "");
            string bcc = GetDecryptParam("Bcc", "").Replace("\n", "");
            string subject = GetDecryptParam("Subject", "");
            string body = GetDecryptParam("Body", "");
            string attachList = GetDecryptParam("AttachList", "");
            string attachNameList = GetDecryptParam("AttachNameList", "");
            string attachRealNameList = GetDecryptParam("AttachRealNameList", "");

            string forward = GetDecryptParam("Forward", "");

            UserInfoHelper userInfo = new UserInfoHelper(HttpContext.Current.User.Identity.Name);

            //받는 사람
            string strTo = to.Replace(";", ",");

            //참조
            string strCc = cc.Replace(";", ",");

            //숨은 참조
            string strBcc = bcc.Replace(";", ",");

            string strSubject = Server.UrlDecode(subject);

            string strBody = Server.UrlDecode(body);

            Helper.AppException.WriteAssert(AppSettingType.SQLMail, "Mail:SendMail" + to + "," + subject);
            Helper.AppException.WriteAssert(AppSettingType.SQLMail, "Mail:SendMail strBody : " + strBody);

            string mailWebServiceUrl = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailWebServiceUrl");
            string method = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "MailSend");
            string paramMailSend = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Mail, "ParamMailSend");

            //본문 인코딩 해야 함. modify shimpark
            string param = string.Format(paramMailSend, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, strTo,
                strCc, strBcc, strSubject, "1", forward, Server.UrlEncode(strBody));  //act/domain/id/pw/to/cc/bcc/subject/mbox/forward/content

            Helper.AppException.WriteAssert(AppSettingType.SQLMail, "Mail:SendMail param : " + param);

            string sXmlData = UrlSendHelper.UsePostDev(mailWebServiceUrl, param);

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(sXmlData);

            string returnCode = xml.SelectSingleNode("/data/returnCode").InnerText;

            if (returnCode == "0")
            {
                //성공
                rtn = "0";
            }
            else
            {
                //실패
                rtn = "1";
            }

            return rtn;
        }       
         
        */
        #endregion 

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : Spool 폴더 파일삭제
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSpoolFileList(string p)
        {
            SetDecryptString(p);
            string fileName = GetDecryptParam("fileName", "");
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");

            try
            {
                // 파일 삭제
                //string filePath = "/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/MailMobile/" + swfUploadGUID + "/" + fileName;
                string filePath = "/DeskPlusMobile/MailAttach/" + CompanyID + "/Users/" + UserID + "/" + swfUploadGUID + "/" + fileName;

                if (File.Exists(Server.MapPath(filePath)))
                {
                    System.IO.File.Delete(Server.MapPath(filePath));
                }

                return true;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("DelSpoolFileList()", ex);
                return false;
            }
        }
    }
}
