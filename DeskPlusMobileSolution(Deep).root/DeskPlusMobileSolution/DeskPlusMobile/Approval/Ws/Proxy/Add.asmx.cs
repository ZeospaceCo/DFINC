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
using Zeospace.DeskPlusMobile.Web.Approval.Ws.BE;
using HtmlAgilityPack;
using System.Configuration;
using Zeospace.DeskPlusMobile.Web.Account.BE;
using Zeospace.DeskPlusMobile.Web.Helper.Security;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Collections;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.Proxy
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Helper.MobileBaseService
    {
        /// <summary>
        /// 휴가원에서 대결자 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddProxyUser(string p)
        {
            try
            {
                string rlt = "-1";

                SetDecryptString(p);

                string companyid = GetDecryptParam("companyid", "0");
                string userid = GetDecryptParam("userid", "0");
                string deptid = GetDecryptParam("deptid", "0");
                string proxyuserid = GetDecryptParam("proxyuserid", "0");
                string proxydeptid = GetDecryptParam("proxydeptid", "0");
                string proxystartdate = GetDecryptParam("proxystartdate", "");
                string proxyenddate = GetDecryptParam("proxyenddate", "");
                string WkItemCd = GetDecryptParam("WkItemCd", "");
                string reason = GetDecryptParam("reason", "");

                BE.AccountInfo userinfo = new BE.AccountInfo();

                List<BE.AccountInfo> list = GetUserInfo(companyid.ToNumber<int>(), userid.ToNumber<int>(), true, false);
                foreach (BE.AccountInfo info in list)
                {
                    if (deptid.ToNumber<int>() == info.DeptID)
                    {
                        userinfo.CompanyID = info.CompanyID;
                        userinfo.UserID = info.UserID;
                        userinfo.DeptID = info.DeptID;
                        userinfo.Name = info.Name;
                        userinfo.DeptName = info.DeptName;
                        userinfo.TitleName = info.TitleName;
                        userinfo.MailID = info.MailID;
                        userinfo.MailDomain = info.MailDomain;
                        userinfo.MobilePhone = info.MobilePhone;
                        userinfo.PreferredLanguage = info.PreferredLanguage;
                    }
                }


                BE.ProxyInfo be = new ProxyInfo();
                be.CompanyID = companyid.ToNumber<int>();
                be.UserID = userid.ToNumber<int>();
                be.DeptID = deptid.ToNumber<int>();
                be.ProxyUserID = proxyuserid.ToNumber<int>();
                be.ProxyDeptID = proxydeptid.ToNumber<int>();
                be.IsProxy = "N";

                // 부재기간
                be.ProxyStartDate = Convert.ToDateTime(proxystartdate);
                be.ProxyEndDate = Convert.ToDateTime(proxyenddate);
                be.AllDay = "N"; // 종일 체크 여부
                be.ProxyType = WkItemCd; // 부재유형
                be.ProxyContent = reason;  // 부재내용
                be.IsCall = "Y"; //대결 예약 대기
                be.WriteUserID = userid.ToNumber<int>();

                string proxyPreferredLanguage = "";
                List<BE.AccountInfo> ulist = GetUserInfo(be.CompanyID, be.ProxyUserID, true, true);
                foreach (BE.AccountInfo pinfo in ulist)
                {
                    proxyPreferredLanguage = pinfo.PreferredLanguage;
                }

                if (proxyPreferredLanguage == "")
                {
                    proxyPreferredLanguage = "ko";
                }

                DataView dv = XmlDataBaseHelper.GetSysCode(be.CompanyID, "AbsenceType");
                dv.RowFilter = "ParentCodeID = 'AbsenceType' AND CodeID='" + be.ProxyType + "'";

                foreach (DataRowView dr in dv)
                {
                    be.ProxyTypeStr = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(),
                        dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),
                        dr["NameZhTw"].ToString(), proxyPreferredLanguage);
                }

                be.ProxyDateStr = DateHelper.GetCultureDate(be.ProxyStartDate, proxyPreferredLanguage, true) + " ~ " + DateHelper.GetCultureDate(be.ProxyEndDate, proxyPreferredLanguage, true);

                if (Addition(userinfo, be))
                {
                    rlt = "0";
                }

                return rlt;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddProxyUser()", ex);
                return "-1";
            }
        }

        #region Addition

        /// <summary>
        /// 대결자 추가
        /// </summary>
        /// <param name="_userInfo"></param>
        /// <param name="_proxyInfo"></param>
        /// <param name="_proxyDate"></param>
        /// <returns></returns>
        public bool Addition(BE.AccountInfo _userInfo, BE.ProxyInfo _proxyInfo)
        {
            if (_proxyInfo == null) return false;

            try
            {
                bool rtnval = false;
                rtnval = AddProxy(_proxyInfo);

                if (rtnval)
                {
                    // 알람처리
                    List<BE.AccountInfo> list = GetUserInfo(_proxyInfo.CompanyID, _proxyInfo.ProxyUserID, true, false);
                    foreach (BE.AccountInfo info in list)
                    {
                        if (_proxyInfo.ProxyDeptID == info.DeptID)
                        {
                            SendAlarm(_userInfo, info, "START", _proxyInfo.ProxyDateStr, _proxyInfo.ProxyTypeStr, _proxyInfo.ProxyContent);
                        }
                    }
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Addition()", ex);
                return false;
            }
        }

        #endregion ADD        

        #region AddProxy
        public bool AddProxy(BE.ProxyInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)  
                                               ,new SqlParameter("@DeptID", SqlDbType.Int)  
                                               ,new SqlParameter("@ProxyUserID", SqlDbType.Int)
                                               ,new SqlParameter("@ProxyDeptID", SqlDbType.Int)
                                               ,new SqlParameter("@ProxyStartDate", SqlDbType.DateTime)
                                               ,new SqlParameter("@ProxyEndDate", SqlDbType.DateTime)
                                               ,new SqlParameter("@AllDay", SqlDbType.Char)
                                               ,new SqlParameter("@ProxyType", SqlDbType.NVarChar, 100)
                                               ,new SqlParameter("@ProxyContent", SqlDbType.NVarChar, 500)
                                               ,new SqlParameter("@IsCall", SqlDbType.Char)
                                               ,new SqlParameter("@IsProxy", SqlDbType.Char)
                                               ,new SqlParameter("@WriteUserID", SqlDbType.Int)
                };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.UserID;
                parms[2].Value = _info.DeptID;
                parms[3].Value = _info.ProxyUserID;
                parms[4].Value = _info.ProxyDeptID;
                parms[5].Value = _info.ProxyStartDate;
                parms[6].Value = _info.ProxyEndDate;
                parms[7].Value = _info.AllDay;
                parms[8].Value = _info.ProxyType;
                parms[9].Value = _info.ProxyContent;
                parms[10].Value = _info.IsCall;
                parms[11].Value = _info.IsProxy;
                parms[12].Value = _info.WriteUserID;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "INS_PROC_Proxy");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms);

                return true;
            }
            catch (SqlException ex)
            {
                AppException.WriteException("AddProxy()", ex);
                return false;
            }
        }
        #endregion

        #region GetUserInfo
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 사용자 정보 가져오기
        /// 작성일자 : 
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public List<BE.AccountInfo> GetUserInfo(int _companyID, int _userID, bool _activeUser, bool _baseDept)
        {
            try
            {
                string activeUser = (_activeUser) ? "'1'" : "'0','1'";
                string baseDept = (_baseDept) ? "'1'" : "'0','1'";

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_UserInfo");
                sql = string.Format(sql, activeUser, baseDept, _companyID, _userID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);

                List<BE.AccountInfo> list = new List<AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new AccountInfo();

                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();
                    info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                    info.MailID = dr["MailID"].ToString();
                    info.MailDomain = dr["MailDomain"].ToString();
                    info.Mail = dr["Mail"].ToString();
                    info.MobilePhone = dr["MobilePhone"].ToString();
                    info.PreferredLanguage = dr["PreferredLanguage"].ToString();

                    #region 이름 다국어 처리
                    info.Name = this.GetLangName(
                                dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 부서명 다국어 처리
                    info.DeptName = this.GetLangName(
                                dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                                dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                                dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                                dr["DeptZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직급명 다국어 처리
                    info.TitleName = this.GetLangName(
                            dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                            dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                            dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                            dr["TitleZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    #region 직책명 다국어 처리
                    info.DutyName = this.GetLangName(
                            dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                            dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                            dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                            dr["DutyZhtw"].ToString().ToNull(), PreLang);
                    #endregion

                    info.AlarmApproval = dr["AlarmApproval"].ToString();

                    if (info.AlarmApproval.IndexOf("1-").Equals(0))
                    {
                        info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                        info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                        info.AlarmMobile = (info.AlarmApproval.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                        info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                    }
                    else
                    {
                        info.AlarmMessenger = "0";
                        info.AlarmMail = "0";
                        info.AlarmMobile = "0";
                        info.AlarmSMS = "0";
                    }

                    list.Add(info);
                }

                return list;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetUserInfo()", ex);
                return null;
            }
        }
        #endregion

        #region 알람 - 알람발송

        /// <summary>
        /// 작성자   : 
        /// 작성목적 : 알람
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_documentID"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_state"></param>
        private void SendAlarm(BE.AccountInfo _sendinfo, BE.AccountInfo _targetinfo, string _state, string _proxyDate, string _proxyTypeStr, string _proxyContent)
        {
            try
            {
                if (_targetinfo == null || _state.Equals("")) return;

                DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmApproval");
                if (dvApp.Count.Equals(0)) return;

                string xmlPath = Server.MapPath("/DeskPlusMobile/XmlData/Proxy/" + ((_targetinfo.PreferredLanguage == "") ? "ko" : _targetinfo.PreferredLanguage) + ".xml");
                XmlDocument contXml = new XmlDocument();
                contXml.Load(xmlPath);
                XmlNode rootNode = contXml.DocumentElement;
                XmlNode subNode = null;

                foreach (DataRowView dr in dvApp)
                {
                    string method = dr["CodeID"].ToString().ToUpper();

                    subNode = rootNode.SelectSingleNode("/ALARM/" + method);            // MAIL, MESSENGER, SMS
                    string[] rtnval = GetAlarmCont(subNode, _sendinfo, _targetinfo, _state, _proxyDate, _proxyTypeStr, _proxyContent);

                    switch (method)
                    {
                        case "MAIL":
                            #region 메일발송
                            if (_targetinfo.AlarmMail.Equals("1"))
                            {
                                CreateMHT(_sendinfo.MailID + "@" + _sendinfo.MailDomain, _sendinfo.Name, _targetinfo.Mail, "", "", rtnval[0], rtnval[1]);
                            }
                            #endregion
                            break;
                        case "MESSENGER":
                            #region 메신저발송
                            if (_targetinfo.AlarmMessenger.Equals("1"))
                            {
                                Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
                                alarmInfo.CompanyID = _sendinfo.CompanyID;
                                alarmInfo.SenderID = _sendinfo.UserID;
                                alarmInfo.SenderName = _sendinfo.Name;
                                alarmInfo.SenderMail = _sendinfo.MailID + "@" + _sendinfo.MailDomain;
                                alarmInfo.SenderPhone = _sendinfo.MobilePhone;
                                alarmInfo.ReceiverID = _targetinfo.UserID;
                                alarmInfo.ReceiverName = _targetinfo.Name;
                                alarmInfo.ReceiverMail = _targetinfo.Mail;
                                alarmInfo.ReceiverPhone = _targetinfo.MobilePhone;
                                alarmInfo.Status = "";
                                alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.Subject = rtnval[0];
                                alarmInfo.Content = rtnval[1];
                                alarmInfo.TargetUrl = "";
                                alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.WriteDate = System.DateTime.Now;
                                alarmInfo.AlarmType = 1;        // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                                alarmInfo.KindSubject = "Approval";
                                alarmInfo.ViewOption = "01";
                                alarmInfo.Store = 1;
                                alarmInfo.UserState = 0;

                                Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();
                                alarm.Add(alarmInfo);
                            }
                            #endregion
                            break;
                        case "SMS":
                            
                            break;
                    }
                }

                #region MOBILE발송
                // 모바일사용의 경우, 무조건 입력
                subNode = rootNode.SelectSingleNode("/ALARM/MOBILE");            // MAIL, MESSENGER, SMS
                string[] rtnCont = GetAlarmCont(subNode, _sendinfo, _targetinfo, _state, _proxyDate, _proxyTypeStr, _proxyContent);

                Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfom = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
                alarmInfom.CompanyID = _sendinfo.CompanyID;
                alarmInfom.SenderID = _sendinfo.UserID;
                alarmInfom.SenderName = _sendinfo.Name;
                alarmInfom.SenderMail = _sendinfo.MailID + "@" + _sendinfo.MailDomain;
                alarmInfom.SenderPhone = _sendinfo.MobilePhone;
                alarmInfom.ReceiverID = _targetinfo.UserID;
                alarmInfom.ReceiverName = _targetinfo.Name;
                alarmInfom.ReceiverMail = _targetinfo.Mail;
                alarmInfom.ReceiverPhone = _targetinfo.MobilePhone;
                alarmInfom.Status = "";
                alarmInfom.SendDate = System.DateTime.Now.AddMinutes(1);
                alarmInfom.Subject = rtnCont[0];
                alarmInfom.Content = rtnCont[1];
                alarmInfom.TargetUrl = "";
                alarmInfom.StartDate = System.DateTime.Now.AddMinutes(1);
                alarmInfom.EndDate = System.DateTime.Now.AddMinutes(1);
                alarmInfom.WriteDate = System.DateTime.Now;
                alarmInfom.AlarmType = 4;        // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                alarmInfom.KindSubject = "Approval";
                alarmInfom.ViewOption = "01";
                alarmInfom.Store = 1;
                alarmInfom.UserState = 0;

                Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarmm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();
                alarmm.Add(alarmInfom);
                #endregion
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SendAlarm()", ex);
            }
        }

        #region GetAlarmCont
        /// <summary>
        /// 작성목적 : 알람내용(제목,내용)
        /// </summary> 
        /// </summary>
        /// <param name="_node"></param>
        /// <param name="_sendinfo"></param>
        /// <param name="_targetinfo"></param>
        /// <param name="_state"></param>
        /// <param name="_proxyDate"></param>
        /// <param name="_proxyTypeStr"></param>
        /// <param name="_proxyContent"></param>
        /// <returns></returns>
        private string[] GetAlarmCont(XmlNode _node, BE.AccountInfo _sendinfo, BE.AccountInfo _targetinfo, string _state, string _proxyDate, string _proxyTypeStr, string _proxyContent)
        {
            if (_node == null || _targetinfo == null || _state.Equals("")) return null;

            try
            {
                string[] rtnval = new string[2];

                string subj = _node.SelectSingleNode("SUBJ/" + _state).InnerText;
                string act = _node.SelectSingleNode("ACTION/" + _state).InnerText;
                string cont = _node.SelectSingleNode("CONTENT").InnerText;

                cont = cont.Replace("$SENDDEPTNM$", _sendinfo.DeptName);
                cont = cont.Replace("$SENDTITLENM$", _sendinfo.TitleName);
                cont = cont.Replace("$SENDUSERNM$", _sendinfo.Name);
                cont = cont.Replace("$TARGETDEPTNM$", _targetinfo.DeptName);
                cont = cont.Replace("$TARGETTITLENM$", _targetinfo.TitleName);
                cont = cont.Replace("$TARGETUSERNM$", _targetinfo.Name);
                cont = cont.Replace("$ACTION$", act);
                cont = cont.Replace("$PROXYDATE$", _proxyDate);
                cont = cont.Replace("$PROXYTYPE$", _proxyTypeStr);
                cont = cont.Replace("$PROXYCONTENT$", _proxyContent);

                rtnval[0] = subj;
                rtnval[1] = cont;

                return rtnval;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmCont()", ex);
                return null;
            }
        }
        #endregion

        #region CreateMHT
        /// <summary>
        /// 작성자   : 
        /// 작성목적 : 메일 발송
        /// 작성일자 : 2012-09-10
        /// </summary>
        /// <param name="_to"></param>
        /// <param name="_from"></param>
        /// <param name="_subject"></param>
        /// <param name="_contentHTML"></param>
        public void CreateMHT(string _sendMail, string _sendName, string _to, string _cc, string _bcc, string _subject, string _body)
        {
            MailDataInfo mailInfo = null;

            mailInfo = new MailDataInfo();
            mailInfo.From = _sendName + "<" + _sendMail + ">";
            //mailInfo.To.Add(_to, _to);

            //받는 사람
            foreach (string tmp in _to.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailInfo.To.Contains(tmp)) mailInfo.To.Add(tmp, tmp);
                }
            }

            //참조
            foreach (string tmp in _cc.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailInfo.Cc.Contains(tmp)) mailInfo.Cc.Add(tmp, tmp);
                }
            }

            //숨은 참조
            foreach (string tmp in _bcc.TrimEnd(';').Split(';'))
            {
                if (tmp.Length > 0)
                {
                    if (!mailInfo.Bcc.Contains(tmp)) mailInfo.Bcc.Add(tmp, tmp);
                }
            }

            mailInfo.Subject = _subject;
            mailInfo.Contents = _body;

            Helper.AppException.WriteAssert(AppSettingType.SQLApproval, "Approval:CreateMHT:" + _to + "," + _sendMail + "," + _subject + "," + _body);

            Zeospace.DeskPlusMobile.Web.Helper.SendMail sendMail = new SendMail();
            sendMail.Send(mailInfo, false, false, "");
        }
        #endregion        

        #endregion

    }
}
