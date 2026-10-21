using System;
using System.Collections.Generic;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Transactions;
using System.Xml;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class Proxy : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BasePage
    {
        public struct ProxyUser
        {
            public string proxyUserInfo; public string proxyUser;
        }

        #region GET

        /// <summary>
        /// 작성자 : 이문호
        /// 작성일자 : 2012-06-25
        /// 작성목적 : 대결의뢰자 목록 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public List<ProxyUser> GetProxyList(int _companyID, int _userID, string _preLang)
        {
            try
            {
                DAL.Proxy dal = new DAL.Proxy();
                DataTableReader dtr = dal.GetProxyList(_companyID, _userID);

                List<ProxyUser> pUser = new List<ProxyUser>();
                ProxyUser proxyUser = new ProxyUser();

                foreach (var listinfo in dtr)
                {
                    string user = dtr["UserID"].ToString();

                    string name = GetLangName(
                            dtr["NameBase"].ToString().ToReverseXSS(),
                            dtr["NameKo"].ToString().ToReverseXSS(),
                            dtr["NameEn"].ToString().ToReverseXSS(), dtr["NameJa"].ToString().ToReverseXSS(),
                            dtr["NameFr"].ToString().ToReverseXSS(), dtr["NameZhcn"].ToString().ToReverseXSS(),
                            dtr["NameZhtw"].ToString().ToReverseXSS(), _preLang);

                    string dept = GetLangName(
                            dtr["deptBase"].ToString().ToReverseXSS(),
                            dtr["deptKo"].ToString().ToReverseXSS(),
                            dtr["deptEn"].ToString().ToReverseXSS(), dtr["deptJa"].ToString().ToReverseXSS(),
                            dtr["deptFr"].ToString().ToReverseXSS(), dtr["deptZhcn"].ToString().ToReverseXSS(),
                            dtr["deptZhtw"].ToString().ToReverseXSS(), _preLang);

                    string title = GetLangName(
                            dtr["titleBase"].ToString().ToReverseXSS(),
                            dtr["titleKo"].ToString().ToReverseXSS(),
                            dtr["titleEn"].ToString().ToReverseXSS(), dtr["titleJa"].ToString().ToReverseXSS(),
                            dtr["titleFr"].ToString().ToReverseXSS(), dtr["titleZhcn"].ToString().ToReverseXSS(),
                            dtr["titleZhtw"].ToString().ToReverseXSS(), _preLang);

                    proxyUser.proxyUserInfo = dept + " " + title + " " + name;
                    proxyUser.proxyUser = user;

                    pUser.Add(proxyUser);
                }

                return pUser;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion GET

        #region ADD

        /// <summary>
        /// 대결자 추가
        /// </summary>
        /// <param name="_userInfo"></param>
        /// <param name="_proxyInfo"></param>
        /// <param name="_proxyDate"></param>
        /// <returns></returns>
        public bool Add(BE.UserInfo _userInfo, BE.ProxyInfo _proxyInfo)
        {
            if (_proxyInfo == null) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Proxy DAL = new Zeospace.DeskPlus.Approval.DAL.Proxy();

                    rtnval = DAL.Add(_proxyInfo);

                    ts.Complete();
                }

                if (rtnval)
                {
                    // 알람처리
                    Account.DAL.User dal = new Account.DAL.User();
                    List<Account.BE.UserInfo> list = dal.GetUserInfo(_proxyInfo.CompanyID, _proxyInfo.ProxyUserID, true, false);
                    foreach (Account.BE.UserInfo info in list)
                    {
                        if (_proxyInfo.ProxyDeptID == info.DeptID)
                        {
                            if (info.AlarmApproval.IndexOf("1-").Equals(0))
                            {
                                info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                info.AlarmMessenger = "0";
                                info.AlarmMail = "0";
                                info.AlarmSMS = "0";
                            }

                            #region 부서명 다국어 처리
                            info.DeptName = UtilHelper.GetLangName(
                                    info.DeptBase.ToNull(), info.DeptKo.ToNull(),
                                    info.DeptEn.ToNull(), info.DeptJa.ToNull(),
                                    info.DeptFr.ToNull(), info.DeptZhCn.ToNull(),
                                    info.DeptZhTw.ToNull(), info.PreferredLanguage);
                            #endregion

                            #region 직급명 다국어 처리
                            info.TitleName = UtilHelper.GetLangName(
                                            info.TitleBase.ToNull(), info.TitleKo.ToNull(),
                                            info.TitleEn.ToNull(), info.TitleJa.ToNull(),
                                            info.TitleFr.ToNull(), info.TitleZhCn.ToNull(),
                                            info.TitleZhTw.ToNull(), info.PreferredLanguage);
                            #endregion

                            #region 사용자명 다국어 처리
                            info.Name = UtilHelper.GetLangName(
                                            info.NameBase.ToNull(), info.NameKo.ToNull(),
                                            info.NameEn.ToNull(), info.NameJa.ToNull(),
                                            info.NameFr.ToNull(), info.NameZhCn.ToNull(),
                                            info.NameZhTw.ToNull(), info.PreferredLanguage);
                            #endregion

                            SendAlarm(_userInfo, info, "START", _proxyInfo.ProxyDateStr, _proxyInfo.ProxyTypeStr, _proxyInfo.ProxyContent);
                        }
                    }
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Proxy[Add] : " + ex);
                return false;
            }
        }

        #endregion ADD

        #region MOD

        /// <summary>
        /// 대결자 수정
        /// </summary>
        /// <param name="_userInfo"></param>
        /// <param name="_proxyInfo"></param>
        /// <param name="_proxyDate"></param>
        /// <returns></returns>
        public bool Mod(BE.UserInfo _userInfo, BE.ProxyInfo _proxyInfo)
        {
            if (_proxyInfo == null) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Proxy DAL = new Zeospace.DeskPlus.Approval.DAL.Proxy();

                    rtnval = DAL.Mod(_proxyInfo);

                    ts.Complete();
                }

                if (rtnval)
                {
                    // 알람처리
                    Account.DAL.User dal = new Account.DAL.User();
                    List<Account.BE.UserInfo> list = dal.GetUserInfo(_proxyInfo.CompanyID, _proxyInfo.ProxyUserID, true, false);
                    foreach (Account.BE.UserInfo info in list)
                    {
                        if (_proxyInfo.ProxyDeptID == info.DeptID)
                        {
                            if (info.AlarmApproval.IndexOf("1-").Equals(0))
                            {
                                info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                info.AlarmMessenger = "0";
                                info.AlarmMail = "0";
                                info.AlarmSMS = "0";
                            }

                            #region 부서명 다국어 처리
                            info.DeptName = UtilHelper.GetLangName(
                                    info.DeptBase.ToNull(), info.DeptKo.ToNull(),
                                    info.DeptEn.ToNull(), info.DeptJa.ToNull(),
                                    info.DeptFr.ToNull(), info.DeptZhCn.ToNull(),
                                    info.DeptZhTw.ToNull(), info.PreferredLanguage);
                            #endregion

                            #region 직급명 다국어 처리
                            info.TitleName = UtilHelper.GetLangName(
                                            info.TitleBase.ToNull(), info.TitleKo.ToNull(),
                                            info.TitleEn.ToNull(), info.TitleJa.ToNull(),
                                            info.TitleFr.ToNull(), info.TitleZhCn.ToNull(),
                                            info.TitleZhTw.ToNull(), info.PreferredLanguage);
                            #endregion

                            #region 사용자명 다국어 처리
                            info.Name = UtilHelper.GetLangName(
                                            info.NameBase.ToNull(), info.NameKo.ToNull(),
                                            info.NameEn.ToNull(), info.NameJa.ToNull(),
                                            info.NameFr.ToNull(), info.NameZhCn.ToNull(),
                                            info.NameZhTw.ToNull(), info.PreferredLanguage);
                            #endregion

                            SendAlarm(_userInfo, info, "START", _proxyInfo.ProxyDateStr, _proxyInfo.ProxyTypeStr, _proxyInfo.ProxyContent);
                        }
                    }
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Proxy[Mod] : " + ex);
                return false;
            }
        }

        #endregion MOD

        #region DEL

        /// <summary>
        /// 대결자 삭제
        /// </summary>
        /// <param name="_userInfo"></param>
        /// <param name="_proxyInfo"></param>
        /// <param name="_proxyDate"></param>
        /// <returns></returns>
        public bool Del(BE.UserInfo _userInfo, BE.ProxyInfo _proxyInfo)
        {
            if (_proxyInfo == null) return false;

            try
            {
                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Proxy DAL = new Zeospace.DeskPlus.Approval.DAL.Proxy();

                    rtnval = DAL.Del(_proxyInfo);

                    ts.Complete();
                }

                if (rtnval && _proxyInfo.IsCall == "Y") //종료된건은 알람 미처리
                {
                    // 알람처리
                    Account.DAL.User dal = new Account.DAL.User();
                    List<Account.BE.UserInfo> list = dal.GetUserInfo(_proxyInfo.CompanyID, _proxyInfo.ProxyUserID, true, false);                    

                    foreach (Account.BE.UserInfo info in list)
                    {
                        if (_proxyInfo.ProxyDeptID == info.DeptID)
                        {
                            if (info.AlarmApproval.IndexOf("1-").Equals(0))
                            {
                                info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                info.AlarmMessenger = "0";
                                info.AlarmMail = "0";
                                info.AlarmSMS = "0";
                            }

                            #region 부서명 다국어 처리
                            info.DeptName = UtilHelper.GetLangName(
                                    info.DeptBase.ToNull(), info.DeptKo.ToNull(),
                                    info.DeptEn.ToNull(), info.DeptJa.ToNull(),
                                    info.DeptFr.ToNull(), info.DeptZhCn.ToNull(),
                                    info.DeptZhTw.ToNull(), info.PreferredLanguage);
                            #endregion

                            #region 직급명 다국어 처리
                            info.TitleName = UtilHelper.GetLangName(
                                            info.TitleBase.ToNull(), info.TitleKo.ToNull(),
                                            info.TitleEn.ToNull(), info.TitleJa.ToNull(),
                                            info.TitleFr.ToNull(), info.TitleZhCn.ToNull(),
                                            info.TitleZhTw.ToNull(), info.PreferredLanguage);
                            #endregion

                            #region 사용자명 다국어 처리
                            info.Name = UtilHelper.GetLangName(
                                            info.NameBase.ToNull(), info.NameKo.ToNull(),
                                            info.NameEn.ToNull(), info.NameJa.ToNull(),
                                            info.NameFr.ToNull(), info.NameZhCn.ToNull(),
                                            info.NameZhTw.ToNull(), info.PreferredLanguage);
                            #endregion

                            SendAlarm(_userInfo, info, "END", _proxyInfo.ProxyDateStr, _proxyInfo.ProxyTypeStr, _proxyInfo.ProxyContent);
                        }
                    }
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Proxy[Del] : " + ex);
                return false;
            }
        }

        #endregion DEL

        #region 알람발송 (메일/메신저/SMS)
        private void SendAlarm(Account.BE.UserInfo _sendinfo, Account.BE.UserInfo _targetinfo, string _state, string _proxyDate, string _proxyTypeStr, string _proxyContent)
        {
            #region 소스

            try
            {
                if (_targetinfo == null || _state.Equals("")) return;

                // 코드관리로 처리
                DataView dvApp = AppSettingHelper.GetSysCode(_sendinfo.CompanyID, "AlarmApproval");
                if (dvApp.Count.Equals(0)) return;

                string xmlpath = FileHelper.GetServerMapPath("/DeskPlusEIP/Approval/App_Data/Proxy/" + ((_targetinfo.PreferredLanguage == "") ? "ko" : _targetinfo.PreferredLanguage) + ".xml");
                XmlDocument contXml = new XmlDocument();
                contXml.Load(xmlpath);
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
                                Zeospace.DeskPlus.Common.BLL.MailZettaFactory mailBll = new MailZettaFactory();
                                mailBll.CreateMHT(_sendinfo.MailID + "@" + _sendinfo.MailDomain, _sendinfo.Name, _targetinfo.Mail, "", "", rtnval[0], rtnval[1], null, "", false);
                            }
                            #endregion
                            break;

                        case "MESSENGER":
                            #region 메신저발송
                            if (_targetinfo.AlarmMessenger.Equals("1"))
                            {
                                AlarmInfo alarmInfo = new AlarmInfo();
                                alarmInfo.CompanyID = _sendinfo.CompanyID;
                                alarmInfo.SenderID = _sendinfo.UserID;
                                alarmInfo.SenderName = _sendinfo.Name;
                                alarmInfo.SenderMail = _sendinfo.MailID + "@" + _sendinfo.MailDomain;
                                alarmInfo.SenderPhone = _sendinfo.MobilePhone;
                                alarmInfo.ReceiverID = _targetinfo.UserID;
                                alarmInfo.ReceiverName = _targetinfo.NameBase;
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
                                alarmInfo.AlarmType = 1;            // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                                alarmInfo.KindSubject = "Approval";
                                alarmInfo.ViewOption = "01";
                                alarmInfo.Store = 1;
                                alarmInfo.UserState = 0;

                                Zeospace.DeskPlus.Common.BLL.Alarm alarmBll = new Alarm();
                                alarmBll.Add(alarmInfo, AlarmEnum.Cool.ToString());
                            }
                            #endregion
                            break;

                        case "SMS":
                            #region SMS발송
                            if (_targetinfo.AlarmSMS.Equals("1"))
                            {
                                Zeospace.DeskPlus.Component.SMS.SMSInfo smsInfo = new Zeospace.DeskPlus.Component.SMS.SMSInfo();

                                // 발송자 & 기타
                                smsInfo.CompanyID = _sendinfo.CompanyID;
                                smsInfo.LoginID = _sendinfo.LogInID;
                                smsInfo.UserID = _sendinfo.UserID;
                                smsInfo.WriteUserName = _sendinfo.Name;
                                smsInfo.InterfaceID = "";
                                smsInfo.Gubun = "";
                                smsInfo.Subject = rtnval[0];
                                smsInfo.Content = rtnval[1];
                                smsInfo.Status = "0";               //상태(즉시:0,예약:1)
                                smsInfo.SendPhone = _sendinfo.MobilePhone;
                                smsInfo.TargetUrl = "";

                                //##### 중요!!! 회사하나로 SMS 비용한다, 계열사 일경우 부서명 가져와야 한다
                                smsInfo.CompanyName = _sendinfo.CompanyID.ToString();
                                smsInfo.DeptID = _sendinfo.DeptID;
                                smsInfo.DeptName = _sendinfo.DeptName;

                                smsInfo.SendDate = System.DateTime.Now.AddMinutes(1); //즉시발송(약간 딜레이가 있어서 1분 추가하였음)
                                smsInfo.SendTime = "000000";
                                smsInfo.StartDate = System.DateTime.MaxValue; //미사용
                                smsInfo.EndDate = System.DateTime.MaxValue;   //미사용
                                smsInfo.UseSendAllUsers = false;

                                // 회신번호 처리
                                string mpinfo = _sendinfo.MobilePhone.Trim().Replace(" ", "");
                                mpinfo = System.Text.RegularExpressions.Regex.Replace(mpinfo, @"[-~.,)(]{1}", "");

                                if (mpinfo.Length.Equals(10))
                                {
                                    smsInfo.ReplyPhone1 = mpinfo.Substring(0, 3);
                                    smsInfo.ReplyPhone2 = mpinfo.Substring(3, 3);
                                    smsInfo.ReplyPhone3 = mpinfo.Substring(6, 4);
                                }
                                else if (mpinfo.Length.Equals(11))
                                {
                                    smsInfo.ReplyPhone1 = mpinfo.Substring(0, 3);
                                    smsInfo.ReplyPhone2 = mpinfo.Substring(3, 4);
                                    smsInfo.ReplyPhone3 = mpinfo.Substring(7, 4);
                                }
                                else
                                {
                                    if (mpinfo.Equals(""))
                                    {
                                        smsInfo.ReplyPhone1 = "";
                                        smsInfo.ReplyPhone2 = "";
                                        smsInfo.ReplyPhone3 = "";
                                    }
                                    else
                                    {
                                        smsInfo.ReplyPhone1 = mpinfo.Substring(0, 3);
                                        smsInfo.ReplyPhone2 = mpinfo.Substring(3, 3);
                                        smsInfo.ReplyPhone3 = mpinfo.Substring(6, mpinfo.Length - 6);
                                    }
                                }

                                // 수신자
                                _targetinfo.MobilePhone = _targetinfo.MobilePhone.Trim().Replace(" ", "");
                                _targetinfo.MobilePhone = System.Text.RegularExpressions.Regex.Replace(_targetinfo.MobilePhone, @"[-~.,)(]{1}", "");

                                smsInfo.ReceivePhone = _targetinfo.MobilePhone;

                                switch (_targetinfo.MobilePhone.Trim().Length)
                                {
                                    case 10:
                                        smsInfo.ReceivePhone1 = smsInfo.ReceivePhone.Substring(0, 3);
                                        smsInfo.ReceivePhone2 = smsInfo.ReceivePhone.Substring(3, 3);
                                        smsInfo.ReceivePhone3 = smsInfo.ReceivePhone.Substring(6, 4);
                                        break;

                                    case 11:
                                        smsInfo.ReceivePhone1 = smsInfo.ReceivePhone.Substring(0, 3);
                                        smsInfo.ReceivePhone2 = smsInfo.ReceivePhone.Substring(3, 4);
                                        smsInfo.ReceivePhone3 = smsInfo.ReceivePhone.Substring(7, 4);
                                        break;

                                    default:
                                        if (smsInfo.ReceivePhone.Equals(""))
                                        {
                                            smsInfo.ReceivePhone1 = "";
                                            smsInfo.ReceivePhone2 = "";
                                            smsInfo.ReceivePhone3 = "";
                                        }
                                        else
                                        {
                                            smsInfo.ReceivePhone1 = smsInfo.ReceivePhone.Substring(0, 3);
                                            smsInfo.ReceivePhone2 = smsInfo.ReceivePhone.Substring(3, 3);
                                            smsInfo.ReceivePhone3 = smsInfo.ReceivePhone.Substring(6, smsInfo.ReceivePhone.Length - 6);
                                        }
                                        break;
                                }

                                Component.SMS.ControlSendSMS sendSMS = new Component.SMS.ControlSendSMS();
                                sendSMS.Transfer(smsInfo);
                            }
                            #endregion
                            break;

                        case "MOBILE":
                            #region MOBILE발송
                            //if (_targetinfo.AlarmMobile.Equals("1"))
                            //{
                                
                            //}
                            #endregion
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력
                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                {
                    subNode = rootNode.SelectSingleNode("/ALARM/MOBILE");            // MAIL, MESSENGER, SMS
                    string[] rtnval = GetAlarmCont(subNode, _sendinfo, _targetinfo, _state, _proxyDate, _proxyTypeStr, _proxyContent);

                    AlarmInfo alarmInfo = new AlarmInfo();
                    alarmInfo.CompanyID = _sendinfo.CompanyID;
                    alarmInfo.SenderID = _sendinfo.UserID;
                    alarmInfo.SenderName = _sendinfo.Name;
                    alarmInfo.SenderMail = _sendinfo.MailID + "@" + _sendinfo.MailDomain;
                    alarmInfo.SenderPhone = _sendinfo.MobilePhone;
                    alarmInfo.ReceiverID = _targetinfo.UserID;
                    alarmInfo.ReceiverName = _targetinfo.NameBase;
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
                    alarmInfo.AlarmType = 4;            // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                    alarmInfo.KindSubject = "Approval";
                    alarmInfo.ViewOption = "01";
                    alarmInfo.Store = 1;
                    alarmInfo.UserState = 0;

                    Zeospace.DeskPlus.Common.BLL.Alarm alarmBll = new Alarm();
                    alarmBll.Add(alarmInfo, AlarmEnum.Cool.ToString());
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.Proxy[SendAlarm]", ex);
            }

            #endregion 소스
        }

        private string[] GetAlarmCont(XmlNode _node, Account.BE.UserInfo _sendinfo, Account.BE.UserInfo _targetinfo, string _state, string _proxyDate, string _proxyTypeStr, string _proxyContent)
        {
            if (_node == null || _targetinfo == null || _state.Equals("")) return null;

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
        #endregion 알람발송 (메일/메신저/SMS)
    }
}