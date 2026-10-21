using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Community.BE;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Xml;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Web;

namespace Zeospace.DeskPlus.Community.BLL
{
    public class Alarm : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region 알람

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 메신저 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_type"></param>
        /// <param name="_method"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool SendAlarm(BE.AlarmInfo _info, int _receiverID, string _type, string _method, string _preLang)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            Common.BE.AlarmInfo alarmInfo = new Common.BE.AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.UserID;
            alarmInfo.SenderName = _info.UserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            string[] message = GetAlarmMessage(_info, _type, _method, _preLang);

            alarmInfo.Subject = message[0].ToReverseXSS();
            alarmInfo.Content = message[1].ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "AlarmTargetURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Community"; //제목에 안들어가며, 구분자값임
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 모바일 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_type"></param>
        /// <param name="_method"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool SendMobile(BE.AlarmInfo _info, int _receiverID, string _type, string _method, string _preLang)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            Common.BE.AlarmInfo alarmInfo = new Common.BE.AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.UserID;
            alarmInfo.SenderName = _info.UserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            string[] message = GetAlarmMessage(_info, _type, _method, _preLang);

            alarmInfo.Subject = message[0].ToReverseXSS();
            alarmInfo.Content = message[1].ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "AlarmTargetURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "Community"; //제목에 안들어가며, 구분자값임
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-07-06
        /// 작성목적 : 메신저 알람 (일정관리)
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_scheduleID"></param>
        /// <returns></returns>
        public bool SendAlarm(BE.ScheduleInfo _info, int _receiverID, int _scheduleID)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            Common.BE.AlarmInfo alarmInfo = new Common.BE.AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.WriteUserID;
            alarmInfo.SenderName = _info.WriteUserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";

            alarmInfo.SendDate = _info.StartDate.AddMinutes((_info.AlertTime * -1));
            alarmInfo.Subject = _info.Title.ToReverseXSS();
            alarmInfo.Content = _info.AlarmContent.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "AlarmScheduleURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("communityID", _info.CommunityID.ToString());
            paraDic.Add("menuID", _info.MenuID.ToString());
            paraDic.Add("scheduleID", _scheduleID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;

            alarmInfo.StartDate = _info.StartDate.AddMinutes(-_info.AlertTime);
            alarmInfo.EndDate = _info.StartDate.AddMinutes(-_info.AlertTime);

            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Community";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 모바일 알람 (일정관리)
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_scheduleID"></param>
        /// <returns></returns>
        public bool SendMobile(BE.ScheduleInfo _info, int _receiverID, int _scheduleID)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            Common.BE.AlarmInfo alarmInfo = new Common.BE.AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.WriteUserID;
            alarmInfo.SenderName = _info.WriteUserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";

            alarmInfo.SendDate = _info.StartDate.AddMinutes((_info.AlertTime * -1));
            alarmInfo.Subject = _info.Title.ToReverseXSS();
            alarmInfo.Content = _info.AlarmContent.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "AlarmScheduleURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("communityID", _info.CommunityID.ToString());
            paraDic.Add("menuID", _info.MenuID.ToString());
            paraDic.Add("scheduleID", _scheduleID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;

            alarmInfo.StartDate = _info.StartDate.AddMinutes(-_info.AlertTime);
            alarmInfo.EndDate = _info.StartDate.AddMinutes(-_info.AlertTime);

            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "Community";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성목적   : 알람 보내기 (게시판 댓글)
        /// 작성자     : shine
        /// 최초작성일 : 2012-07-01
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_recvUserID">The _recv user ID.</param>
        public bool SendAlarmUser(BoaShortReplyInfo _info, int _receiverID)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            Zeospace.DeskPlus.Common.BE.AlarmInfo alarmInfo = new Common.BE.AlarmInfo();

            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.UserID;
            alarmInfo.SenderName = _info.UserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _info.Title.ToReverseXSS();
            alarmInfo.Content = _info.Content.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "AlarmBoardURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("communityID", _info.CommunityID.ToString());
            paraDic.Add("menuID", _info.MenuID.ToString());
            paraDic.Add("boardID", _info.BoardID.ToString());
            if (_info.ListType.ToString() != "")
            {
                paraDic.Add("listType", _info.ListType.ToString());
            }
            paraDic.Add("alarmPopup", "Y");

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Community";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성목적   : 모바일 알람 보내기 (게시판 댓글)
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-04-27
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_recvUserID">The _recv user ID.</param>
        public bool SendMobileUser(BoaShortReplyInfo _info, int _receiverID)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            Zeospace.DeskPlus.Common.BE.AlarmInfo alarmInfo = new Common.BE.AlarmInfo();

            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.UserID;
            alarmInfo.SenderName = _info.UserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _info.Title.ToReverseXSS();
            alarmInfo.Content = _info.Content.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "AlarmBoardURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("communityID", _info.CommunityID.ToString());
            paraDic.Add("menuID", _info.MenuID.ToString());
            paraDic.Add("boardID", _info.BoardID.ToString());
            if (_info.ListType.ToString() != "")
            {
                paraDic.Add("listType", _info.ListType.ToString());
            }
            paraDic.Add("alarmPopup", "Y");

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = "/DeskPlusMobile/Community/AlarmView.aspx?p=" + encparam;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "Community";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 알람 내용
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_type"></param>
        /// <param name="_method"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public string[] GetAlarmMessage(BE.AlarmInfo _info, string _type, string _method, string _preLang)
        {
            string[] messageArr = new string[2];

            try
            {
                XmlDocument confirmXml = null;
                string title = String.Empty;
                string contHtml = String.Empty;

                string xmlPath = String.Empty;

                //개설 신청
                switch (_method)
                {
                    case "COMMUNITYADD":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람                                
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Add_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Add_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Add_SMS.xml");
                                break;
                        }
                        break;
                    case "COMMUNITYCANCEL":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람                                
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Cancel_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Cancel_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Cancel_SMS.xml");
                                break;
                        }
                        break;
                    case "COMMUNITYJOIN":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Join_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Join_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Join_SMS.xml");
                                break;
                        }
                        break;
                    case "COMMUNITYJOINCANCEL":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람                                
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_JoinCancel_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_JoinCancel_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_JoinCancel_SMS.xml");
                                break;
                        }
                        break;
                    case "COMMUNITYAPPROVE":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Approve_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Approve_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Approve_SMS.xml");
                                break;
                        }
                        break;
                    case "COMMUNITYDENY":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Deny_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Deny_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Deny_SMS.xml");
                                break;
                        }
                        break;
                    case "COMMUNITYREJECT":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람                                
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Reject_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Reject_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Reject_SMS.xml");
                                break;
                        }
                        break;
                    case "COMMUNITYCONFIRM":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람                                
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Confirm_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Confirm_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Confirm_SMS.xml");
                                break;
                        }
                        break;
                    case "COMMUNITYWITHDRAW":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람                                
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_WithDraw_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_WithDraw_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_WithDraw_SMS.xml");
                                break;
                        }
                        break;
                    case "COMMUNITYDROP":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람                                
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Drop_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Drop_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Drop_SMS.xml");
                                break;
                        }
                        break;
                    case "COMMUNITYRESTORE":
                        switch (_type)
                        {
                            case "Messenger": //메신저알람                                
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Restore_Messenger.xml");
                                break;
                            case "Mail": //메일
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Restore_Mail.xml");
                                break;
                            case "SMS": //SMS
                                xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/Common/App_Data/" + _preLang + "/Community_Restore_SMS.xml");
                                break;
                        }
                        break;
                }

                confirmXml = new XmlDocument();
                confirmXml.Load(xmlPath);
                XmlNode tmpXmlNode = confirmXml.DocumentElement;

                //XML 의 타이틀 템플릿에 기본 정보(커뮤니티 명 , 신청자 명)를 넣는다
                title = (tmpXmlNode.SelectSingleNode("/ContentInfo/Title") != null) ? System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ContentInfo/Title").InnerText) : "";
                title = title.Replace("$Title$", _info.Name);     //커뮤니티 명
                title = title.Replace("$Name$", _info.UserName);  //신청자 명
                messageArr[0] = title;

                //XML 의 본문 템플릿에 기본 정보(커뮤니티 명 , 신청자 명)를 넣는다
                contHtml = (tmpXmlNode.SelectSingleNode("/ContentInfo/ContentHTML") != null) ? System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ContentInfo/ContentHTML").InnerText) : "";
                contHtml = contHtml.Replace("$Title$", _info.Name);     //커뮤니티 명
                contHtml = contHtml.Replace("$Name$", _info.UserName);  //신청자 명
                contHtml = contHtml.Replace("$Content$", _info.Description);  //내용

                messageArr[1] = contHtml;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAlarmMessage()", ex);

            }

            return messageArr;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-06-19
        /// 작성목적 : 메일 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_sendMail"></param>
        /// <param name="_mailReceiver"></param>
        /// <param name="_type"></param>
        /// <param name="_method"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool SendMail(BE.AlarmInfo _info, string _sendMail, string _mailReceiver, string _type, string _method, string _preLang)
        {
            // 받는 사람
            Hashtable htMailTo = new Hashtable();

            _mailReceiver = _mailReceiver.TrimEnd(',');

            foreach (string mail in _mailReceiver.Split(','))
            {
                htMailTo.Add(mail, mail);
            }

            string[] message = GetAlarmMessage(_info, _type, _method, _preLang);

            MailDataInfo mailInfo = new MailDataInfo();
            mailInfo.To = htMailTo;
            mailInfo.From = _info.UserName + "<" + _sendMail + ">";
            mailInfo.Subject = message[0];
            mailInfo.Contents = message[1];
    
            SendMail sendMail = new SendMail();
            return sendMail.Send(mailInfo);           
        }

        #endregion 
    }
}
