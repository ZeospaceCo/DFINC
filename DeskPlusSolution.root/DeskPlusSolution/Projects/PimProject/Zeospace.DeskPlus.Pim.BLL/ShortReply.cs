using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;

using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Pim.BLL
{
    public class ShortReply
    {
        #region ADD()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 쪽지작성
        /// </summary>
        /// <param name="_shortinfo">The _shortinfo.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <param name="_preTitle">The _pre title.</param>
        /// <param name="_chek">if set to <c>true</c> [_chek].</param>
        /// <returns></returns>
        public bool Add(BE.ShortReplyInfo _shortinfo, string _preLang, string _preTitle, bool _chek)
        {
            if (_shortinfo == null) return false;

            try
            {
                //using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                //{
                    DAL.ShortReply replyDal = new DAL.ShortReply();
                    BE.ShortReplyInfo replyInfo = new ShortReplyInfo();

                    replyDal.Add(_shortinfo);

                    // 다른 사람이 댓글을 추가했을 경우에만 알람 발송
                    if (_chek == false)
                    {
                        int recvUserID = 0;
                        string subject = "";

                        // 받는 사람
                        recvUserID = _shortinfo.RUserID;
                        // 제목
                        subject = "[" + _preTitle.Split(',')[0] + "]" + _shortinfo.MyDeptName + " " + _shortinfo.UserName + _preTitle.Split(',')[1];

                        // 알람발송(원본글)
                        SendAlarm(_shortinfo, recvUserID, subject);
                    }

                    //ts.Complete();
                //}
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ShortReply.Add : " + ex);
                return false;
            }
        }
        #endregion

        #region 알람

        public void SendAlarm(BE.ShortReplyInfo _replyInfo, int _receiverID, string _subject)
        {
            #region 로직
            /*
             */
            #endregion

            #region 소스

            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_replyInfo.CompanyID, "AlarmSNS");

            string AlarmSNS = string.Empty;
            string AlarmMail = string.Empty;
            string AlarmMessenger = string.Empty;
            string AlarmMobile = string.Empty;
            string AlarmSMS = string.Empty;

            Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
            List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(_replyInfo.CompanyID, _receiverID);

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
                            SendMessenger(_replyInfo, _receiverID, _subject);
                        }
                        break;
                }
            }

            // 모바일사용의 경우, 무조건 입력
            if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
            {
                SendMobile(_replyInfo, _receiverID, _subject);
            }

            #endregion

            
        }

        /// <summary>
        /// 메신저
        /// </summary>
        /// <param name="_replyInfo"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_subject"></param>
        /// <returns></returns>
        private bool SendMessenger(BE.ShortReplyInfo _replyInfo, int _receiverID, string _subject)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();

            alarmInfo.CompanyID = _replyInfo.CompanyID;
            alarmInfo.SenderID = _replyInfo.UserID;         //보내는 사용자 ID
            alarmInfo.SenderName = _replyInfo.UserName;     //보내는 사람 
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _subject;
            alarmInfo.Content = _replyInfo.Content.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "AlarmTargetURL", "");

            //alarmTargetURL = baseHttp + fullDomain + "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims";
            alarmTargetURL = baseHttp + fullDomain + "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=SNSMyTalk";

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
        /// 모바일
        /// </summary>
        /// <param name="_replyInfo"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_subject"></param>
        /// <returns></returns>
        private bool SendMobile(BE.ShortReplyInfo _replyInfo, int _receiverID, string _subject)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();

            AlarmInfo alarmInfo = new AlarmInfo();

            alarmInfo.CompanyID = _replyInfo.CompanyID;
            alarmInfo.SenderID = _replyInfo.UserID;         //보내는 사용자 ID
            alarmInfo.SenderName = _replyInfo.UserName;     //보내는 사람 
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.Subject = _subject;
            alarmInfo.Content = _replyInfo.Content.ToReverseXSS();

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "AlarmTargetURL", "");

            //alarmTargetURL = baseHttp + fullDomain + "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims";
            alarmTargetURL = baseHttp + fullDomain + "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=SNSMyTalk";

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

        #endregion
    }
}
