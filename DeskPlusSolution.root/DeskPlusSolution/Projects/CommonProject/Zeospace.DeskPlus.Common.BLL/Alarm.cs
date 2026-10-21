using System;
using System.Runtime.CompilerServices;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Common.BLL
{
    public class Alarm
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 알람 호출
        /// </summary>
        /// <param name="_alarmInfo">The _alarm info.</param>
        /// <param name="_alarmType">Type of the _alarm.</param>
        /// <returns></returns>
        public bool Add(AlarmInfo _alarmInfo, string _alarmType)
        {
            if (_alarmInfo == null)
            {
                return false;
            }
            else
            {
                bool response = false;
                string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                string mobileDOMAIN = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "MobileDOMAIN", "");

                DAL.AlarmCommon bizComm = new DAL.AlarmCommon();
                string tmpInfo = bizComm.GetLoginIDPwd(_alarmInfo.CompanyID, _alarmInfo.ReceiverID);
                string receiverLoginID = (tmpInfo.Split('^')[1] != null) ? tmpInfo.Split('^')[0] : "";
                string receiverPwd = (tmpInfo.Split('^')[0] != null) ? tmpInfo.Split('^')[1] : "";

                _alarmInfo.TargetUrl = baseHttp + fullDomain + "/DeskPlusEIP/Site/Auth/LoginOutSide.aspx?cID=" + _alarmInfo.CompanyID
                    + "&uID=" + receiverLoginID + "&uPW=" + receiverPwd + "&tURL=" + _alarmInfo.TargetUrl;

                _alarmInfo.RandomStr = System.Guid.NewGuid().ToString().Substring(0, 7);

                if (!String.IsNullOrEmpty(_alarmInfo.TargetMobileUrl))
                {
                    _alarmInfo.TargetMobileUrl = baseHttp + mobileDOMAIN + "/DeskPlusMobile/Portal/LoginOutSide.aspx?cID=" + _alarmInfo.CompanyID
                       + "&uID=" + receiverLoginID + "&uPW=" + receiverPwd + "&ran=" + _alarmInfo.RandomStr + "&tURL=" + _alarmInfo.TargetMobileUrl;
                }

                if (!String.IsNullOrEmpty(_alarmInfo.AlarmUrl))
                {
                    _alarmInfo.AlarmUrl = baseHttp + mobileDOMAIN + "/DeskPlusMobile/Portal/LoginOutSide.aspx?cID=" + _alarmInfo.CompanyID
                       + "&uID=" + receiverLoginID + "&uPW=" + receiverPwd + "&tURL=" + _alarmInfo.AlarmUrl;
                }
                else
                {
                    _alarmInfo.AlarmUrl = "";
                }

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

                if (_alarmType == AlarmEnum.Cool.ToString())
                {
                    DAL.AlarmCool biz = new DAL.AlarmCool();
                    response = bizComm.Add(_alarmInfo);
                }
                else if (_alarmType == AlarmEnum.SuSoft.ToString())
                {
                    DAL.AlarmSuSoft biz = new DAL.AlarmSuSoft();
                    response = bizComm.Add(_alarmInfo);
                }
                else if (_alarmType == AlarmEnum.Zetta.ToString())
                {
                    DAL.AlarmZetta biz = new DAL.AlarmZetta();
                    response = bizComm.Add(_alarmInfo);
                }

                if (TeamsMSG.IsEnabled && _alarmInfo.AlarmType == 1)
                {
                    try
                    {

                        _alarmInfo.Content += "<ul style = 'font-size:9pt;line-height:20px;'>";
                        _alarmInfo.Content += "<li>결재 일시 : " + DateTime.Now + "</li>";
                        _alarmInfo.Content += "</ul>";

                        // Teams 수신자는 알림의 회사/수신자 ID로 DB에서 조회한 이메일을 사용합니다.
                        var users = new Account.DAL.User().GetUserInfo(_alarmInfo.CompanyID, _alarmInfo.ReceiverID, true, true);
                        if (users == null || users.Count == 0)
                            throw new InvalidOperationException("DB에서 Teams 채팅 수신자를 찾을 수 없습니다. CompanyID="
                                + _alarmInfo.CompanyID + ", ReceiverID=" + _alarmInfo.ReceiverID);

                        var receiver = users[0];
                        string teamsRecipient = (receiver.Mail ?? string.Empty).Trim();
                        if (string.IsNullOrWhiteSpace(teamsRecipient)
                            && !string.IsNullOrWhiteSpace(receiver.MailID)
                            && !string.IsNullOrWhiteSpace(receiver.MailDomain))
                            teamsRecipient = receiver.MailID.Trim() + "@" + receiver.MailDomain.Trim();

                        if (string.IsNullOrWhiteSpace(teamsRecipient))
                            throw new InvalidOperationException("DB에 Teams 채팅 수신자의 이메일이 등록되어 있지 않습니다. ReceiverID="
                                + _alarmInfo.ReceiverID);

                        TeamsMSG.SendChatMessage(teamsRecipient, _alarmInfo.Subject, _alarmInfo.Content, _alarmInfo.TargetUrl);
                    }
                    catch (Exception ex)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Teams 채팅 수신자 확인 실패: " + ex.Message);
                    }
                }

                return response;
            }
        }
    }
}
