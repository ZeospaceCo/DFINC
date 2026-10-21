using System;
using System.Collections.Generic;
using System.Transactions;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

using System.Data;

namespace Zeospace.DeskPlus.SNS.BLL
{
    public class ApprovalTalk : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_preTitle"></param>
        /// <param name="_isComp"></param>
        /// <returns></returns>
        public bool Add(BE.ApprovalTalkInfo _info, string _preTitle, string _isComp)
        {
            if (_info == null) return false;

            try
            {
                bool rtnval = false;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.ApprovalTalk dal = new DAL.ApprovalTalk();
                    rtnval = dal.Add(_info);
                    if (!rtnval) return false;

                    ts.Complete();
                }

                if (rtnval)
                {
                    SendAlarm( _info, _preTitle, _isComp);
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ApprovalTalk.Add : " + ex);
                return false;
            }
        }

        #endregion ADD



        #region 알람

        // <summary>
        /// Sends the alarm.
        /// </summary>
        /// <param name="_replyInfo">The _reply info.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_recvTitle">The _recv title.</param>
        /// <param name="_subject">The _subject.</param>
        /// <returns></returns>
        public void SendAlarm(BE.ApprovalTalkInfo _info, string _preTitle, string _isComp)
        {
            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "AlarmSNS");
            if (dvAlarm.Count.Equals(0)) return;

            Zeospace.DeskPlus.Approval.DAL.Sequence seqdal = new Zeospace.DeskPlus.Approval.DAL.Sequence();

            if (_isComp.Equals("1"))
            {
                // 완결된 문서
                List<Zeospace.DeskPlus.Approval.BE.UserInfo> list = seqdal.GetAlarmComplete(_info.CompanyID, _info.DocumentID, _info.GroupID, _info.AccountTag, 1000);
                foreach (Zeospace.DeskPlus.Approval.BE.UserInfo info in list)
                {
                    if (!_info.UserID.Equals(info.UserID))
                    {
                        string AlarmSNS = string.Empty;
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

                        Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
                        List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(info.CompanyID, info.UserID);

                        if (listUser != null)
                        {
                            foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo infoUser in listUser)
                            {
                                AlarmSNS = infoUser.AlarmSNS;
                            }
                        }

                        if (AlarmSNS.IndexOf("1-").Equals(0))
                        {
                            AlarmMail = (AlarmSNS.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            AlarmMessenger = (AlarmSNS.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            AlarmMobile = (AlarmSNS.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                            AlarmSMS = (AlarmSNS.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            AlarmMessenger = "0";
                            AlarmMail = "0";
                            AlarmMobile = "0";
                            AlarmSMS = "0";
                        }

                        foreach (DataRowView dr in dvAlarm)
                        {
                            string method = dr["CodeID"].ToString().ToUpper();
                            switch (method)
                            {
                                case "MESSENGER":
                                    if (AlarmMessenger.Equals("1"))
                                    {
                                        SendMessenger(_info, info.UserID, _preTitle);
                                    }
                                    break;
                            }
                        }

                        // 모바일사용의 경우, 무조건 입력
                        if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                        {
                            SendMobile(_info, info.UserID, _preTitle);
                        }                          
                    }
                }
            }
            else
            {
                // 진행중인 문서
                List<UserInfo> list = seqdal.GetAlarmApproving(_info.CompanyID, _info.DocumentID, _info.GroupID);
                foreach (Zeospace.DeskPlus.Approval.BE.UserInfo info in list)
                {
                    if (!_info.UserID.Equals(info.UserID))
                    {
                        string AlarmSNS = string.Empty;
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

                        Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
                        List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(info.CompanyID, info.UserID);

                        if (listUser != null)
                        {
                            foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo infoUser in listUser)
                            {
                                AlarmSNS = infoUser.AlarmSNS;
                            }
                        }

                        if (AlarmSNS.IndexOf("1-").Equals(0))
                        {
                            AlarmMail = (AlarmSNS.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            AlarmMessenger = (AlarmSNS.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            AlarmMobile = (AlarmSNS.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                            AlarmSMS = (AlarmSNS.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            AlarmMessenger = "0";
                            AlarmMail = "0";
                            AlarmMobile = "0";
                            AlarmSMS = "0";
                        }

                        foreach (DataRowView dr in dvAlarm)
                        {
                            string method = dr["CodeID"].ToString().ToUpper();
                            switch (method)
                            {
                                case "MESSENGER":
                                    if (AlarmMessenger.Equals("1"))
                                    {
                                        SendMessenger(_info, info.UserID, _preTitle);
                                    }
                                    break;
                            }
                        }

                        // 모바일사용의 경우, 무조건 입력
                        if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                        {
                            SendMobile(_info, info.UserID, _preTitle);
                        }  
                    }
                }
            }
        }

        /// <summary>
        /// Sends the alarm.
        /// </summary>
        /// <param name="_replyInfo">The _reply info.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <param name="_startDate">The _start date.</param>
        /// <param name="_recvTitle">The _recv title.</param>
        /// <param name="_subject">The _subject.</param>
        /// <returns></returns>
        public bool SendMessenger(BE.ApprovalTalkInfo _info, int _receiverID, string _subject)
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

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = baseHttp + fullDomain + "/DeskplusEIP/EIP/DeskPlusEPDefault.aspx";

            #endregion URL 처리

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "SNS";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-28
        /// 작성목적 : 모바일 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool SendMobile(BE.ApprovalTalkInfo _info, int _receiverID, string _subject)
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

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = baseHttp + fullDomain + "/DeskplusEIP/EIP/DeskPlusEPDefault.aspx";

            #endregion URL 처리

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "SNS";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }
        #endregion 알람
    }
}