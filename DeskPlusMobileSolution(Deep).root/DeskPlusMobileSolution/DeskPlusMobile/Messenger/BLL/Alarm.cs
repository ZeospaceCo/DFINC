using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Configuration;
using System.Data;
using Zeospace.DeskPlusMobile.Web.Messenger.BE;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;

namespace Zeospace.DeskPlusMobile.Web.Messenger.BLL
{
    public class Alarm
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-13
        /// 작성목적 : 알람 발송
        /// </summary>
        /// <param name="_alarmInfo"></param>
        /// <param name="_alarmType"></param>
        /// <returns></returns>
        public bool Add(AlarmInfo _alarmInfo)
        {
            if (_alarmInfo == null)
            {
                return false;
            }
            else
            {
                string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
                string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
                string mobileDOMAIN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "MobileDOMAIN");

                Data.Alarm ialarm = new Zeospace.DeskPlusMobile.Web.Messenger.Data.Alarm();
                string tmpInfo = ialarm.GetLoginIDPwd(_alarmInfo.CompanyID, _alarmInfo.ReceiverID);

                string receiverLoginID = (tmpInfo.Split('^')[1] != null) ? tmpInfo.Split('^')[0] : "";
                string receiverPwd = (tmpInfo.Split('^')[0] != null) ? tmpInfo.Split('^')[1] : "";

                _alarmInfo.TargetUrl = httpType + fullDomain + "/DeskPlusEIP/Site/Auth/LoginOutSide.aspx?cID=" + _alarmInfo.CompanyID
                    + "&uID=" + receiverLoginID + "&uPW=" + receiverPwd + "&tURL=" + _alarmInfo.TargetUrl;

                _alarmInfo.RandomStr = System.Guid.NewGuid().ToString().Substring(0, 7);

                if (!String.IsNullOrEmpty(_alarmInfo.TargetMobileUrl))
                {
                    _alarmInfo.TargetMobileUrl = httpType + mobileDOMAIN + "/DeskPlusMobile/Portal/LoginOutSide.aspx?cID=" + _alarmInfo.CompanyID
                       + "&uID=" + receiverLoginID + "&uPW=" + receiverPwd + "&ran=" + _alarmInfo.RandomStr + "&tURL=" + _alarmInfo.TargetMobileUrl;
                }

                _alarmInfo.AlarmUrl = "";

                if (String.IsNullOrEmpty(_alarmInfo.MsgSendID.ToString()))
                {
                    _alarmInfo.MsgSendID = 0;
                }

                if (String.IsNullOrEmpty(_alarmInfo.ScheduleID.ToString()))
                {
                    _alarmInfo.ScheduleID = 0;
                }

                if (String.IsNullOrEmpty(_alarmInfo.DisplayID.ToString()))
                {
                    _alarmInfo.DisplayID = 0;
                }

                bool response = ialarm.Add(_alarmInfo);

                // 업무 구분과 관계없이 모든 메신저 알람을 Teams에도 발송합니다.
                if (_alarmInfo.AlarmType == 1
                    && TeamsMSG.IsEnabled)
                {
                    try
                    {
                        DataSet users = new Account.Data.User().GetUserCompany(_alarmInfo.CompanyID, _alarmInfo.ReceiverID);
                        string recipient = GetTeamsRecipient(users);
                        string content = (_alarmInfo.Content ?? string.Empty)
                            + "<ul style='font-size:9pt;line-height:20px;'><li>알림 일시 : "
                            + DateTime.Now + "</li></ul>";
                        TeamsMSG.SendChatMessage(recipient, _alarmInfo.Subject, content, _alarmInfo.TargetUrl);
                    }
                    catch (Exception ex)
                    {
                        Helper.AppException.WriteException("Teams 메신저 알람 발송 실패. CompanyID="
                            + _alarmInfo.CompanyID + ", ReceiverID=" + _alarmInfo.ReceiverID, ex);
                    }
                }

                return response;
            }
        }

        internal static string GetTeamsRecipient(DataSet users)
        {
            if (users == null || users.Tables.Count == 0 || users.Tables[0].Rows.Count == 0)
                throw new InvalidOperationException("DB에서 Teams 채팅 수신자를 찾을 수 없습니다.");

            DataRow receiver = users.Tables[0].Rows[0];
            string mail = GetMailField(receiver, "Mail");
            if (string.IsNullOrWhiteSpace(mail))
            {
                string mailId = GetMailField(receiver, "MailID");
                string mailDomain = GetMailField(receiver, "MailDomain");
                if (!string.IsNullOrWhiteSpace(mailId) && !string.IsNullOrWhiteSpace(mailDomain))
                    mail = mailId + "@" + mailDomain;
            }
            if (string.IsNullOrWhiteSpace(mail))
                throw new InvalidOperationException("DB에 Teams 채팅 수신자의 이메일이 등록되어 있지 않습니다.");
            return mail;
        }

        private static string GetMailField(DataRow receiver, string column)
        {
            return receiver.Table.Columns.Contains(column)
                ? Convert.ToString(receiver[column]).Trim() : string.Empty;
        }
    }
}
