using System;
using System.Collections.Generic;
using System.Transactions;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class ShortReply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET

        #endregion GET

        #region ADD

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_preTitle"></param>
        /// <param name="_isComp"></param>
        /// <returns></returns>
        public bool Add(BE.ShortReplyInfo _info, string _preTitle, string _isComp)
        {
            if (_info == null) return false;

            try
            {
                DAL.ShortReply dal = new DAL.ShortReply();
                bool rtnval = dal.Add(_info);
                if (!rtnval) return false;

                // 알람발송 (알람발송은 Transaction에 포함되지 않음)
                if (rtnval)
                {
                    DAL.Sequence seqdal = new DAL.Sequence();

                    if (_isComp.Equals("1"))
                    {
                        // 완결된 문서
                        List<UserInfo> list = seqdal.GetAlarmComplete(_info.CompanyID, _info.DocumentID, _info.GroupID, _info.AccountTag, 1000);
                        foreach (BE.UserInfo info in list)
                        {
                            if (!_info.UserID.Equals(info.UserID))
                            {
                                SendAlarm(_info, info.UserID, _preTitle);
                            }
                        }
                    }
                    else
                    {
                        // 진행중인 문서
                        List<UserInfo> list = seqdal.GetAlarmApproving(_info.CompanyID, _info.DocumentID, _info.GroupID);
                        foreach (BE.UserInfo info in list)
                        {
                            if (!_info.UserID.Equals(info.UserID))
                            {
                                SendAlarm(_info, info.UserID, _preTitle);
                            }
                        }
                    }
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ShortReply.Add : " + ex);
                return false;
            }
        }

        #endregion ADD

        #region MOD

        #endregion MOD

        #region DEL

        #endregion DEL

        #region 알람

        /// <summary>
        /// Sends the alarm.
        /// </summary>
        /// <param name="_replyInfo">The _reply info.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_recvTitle">The _recv title.</param>
        /// <param name="_subject">The _subject.</param>
        /// <returns></returns>
        public bool SendAlarm(BE.ShortReplyInfo _info, int _receiverID, string _subject)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();

            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.UserID;         //보내는 사용자 ID
            alarmInfo.SenderName = _info.NameBase;     //보내는 사람
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _subject;
            alarmInfo.Content = _info.Content.ToReverseXSS();

            #region URL 처리

            //string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            //string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            //string alarmTargetURL = baseHttp + fullDomain + "/DeskplusEIP/Approval/Common/Popup/ViewTot.aspx";

            //Dictionary<string, string> paraDic = new Dictionary<string, string>();
            //paraDic.Add("companyID", _info.CompanyID.ToString());
            //paraDic.Add("documentID", _info.DocumentID.ToString());

            //HttpContext context = HttpContext.Current;
            //string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            //alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = baseHttp + fullDomain + "/DeskplusEIP/EIP/DeskPlusEPDefault.aspx";

            #endregion URL 처리

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "Approval";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        #endregion 알람
    }
}