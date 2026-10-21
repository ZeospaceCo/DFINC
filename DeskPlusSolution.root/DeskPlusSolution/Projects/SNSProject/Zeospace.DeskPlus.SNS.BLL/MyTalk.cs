using System;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Common.BE;
using System.IO;
using System.Web;

using System.Data;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.SNS.BLL
{
    public class MyTalk : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2013-06-19
        /// 작성목적 : 마이톡 쓰기
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_preLang"></param>
        /// <param name="_preTitle"></param>
        /// <param name="_chek"></param>
        /// <param name="_swfUploadGUID"></param>
        /// <returns></returns>
        public bool Add(BE.MyTalkInfo _info, string _preLang, string _preTitle, bool _chek, string _swfUploadGUID)
        {
            if (_info == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // DB 처리
                    DAL.MyTalk dal = new DAL.MyTalk();
                    int rGroupID = dal.Add(_info);

                    if (rGroupID > 0)
                    {
                        if (_swfUploadGUID != "")
                        {
                            // 파일 처리
                            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
                            string hiddenSpoolPath = string.Format(filePath, _info.CompanyID, _info.UserID, _swfUploadGUID);

                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(hiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.Attach bll = new BLL.Attach();

                                if (!bll.Add(_info, rGroupID, hiddenSpoolPath))
                                {
                                    return false;
                                }
                            }
                        }

                        if (!DocStream(_info.CompanyID, rGroupID))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }                  

                    ts.Complete();
                }
            
                if (_chek == false)
                {
                    SendAlarm(_info, _preLang, _preTitle, _chek, _swfUploadGUID);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return false;
            }
        }

        public bool DocStream(int _companyID, int _rGroupID)
        {   
            var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.SNS, "DocStreamYN", "N");

            if (docStreamYN == "Y")
            {
                DAL.Attach dalFile = new DAL.Attach();
                DocStream ds = new DocStream();
                try
                {
                    var newDSList = dalFile.GetList(_companyID, _rGroupID).Tables[0];
                    for (int i = 0; i < newDSList.Rows.Count; i++)
                    {
                        ds.AddDocStream(newDSList.Rows[i]["CompanyID"].ToString().ToNumber<int>(), newDSList.Rows[i]["AttachID"].ToString().ToNumber<int>(), "SNS_MYTALK");
                        ds.UpdateFileID(newDSList.Rows[i]["CompanyID"].ToString().ToNumber<int>(), newDSList.Rows[i]["AttachID"].ToString().ToNumber<int>(), "SNS_MYTALK");
                    }
                }
                catch (Exception ex)
                {
                    var DSList = dalFile.GetList(_companyID, _rGroupID).Tables[0];
                    for (int i = 0; i < DSList.Rows.Count; i++)
                    {
                        ds.DelDocStream(DSList.Rows[i]["CompanyID"].ToString().ToNumber<int>(), DSList.Rows[i]["AttachID"].ToString().ToNumber<int>(), "SNS_MYTALK");
                    }
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SNS_MYTALK_Addition_DS : " + ex.Message);
                    return false;
                }
            }

            return true;
        }

        #endregion ADD()

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
        public void SendAlarm(BE.MyTalkInfo _info, string _preLang, string _preTitle, bool _chek, string _swfUploadGUID)
        {
            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "AlarmSNS");
            if (dvAlarm.Count.Equals(0)) return;

            int recvUserID = 0;
            string subject = "";

            string AlarmSNS = string.Empty;
            string AlarmMail = string.Empty;
            string AlarmMessenger = string.Empty;
            string AlarmMobile = string.Empty;
            string AlarmSMS = string.Empty;

            recvUserID = _info.RUserID;
            subject = "[" + _preTitle.Split(',')[0] + "]" + _info.MyDeptName + " " + _info.UserName + _preTitle.Split(',')[1];

            Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
            List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(_info.CompanyID, _info.RUserID);

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
                            SendMessenger(_info, recvUserID, subject);
                        }
                        break;
                }
            }

            // 모바일사용의 경우, 무조건 입력
            if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
            {
                SendMobile(_info, recvUserID, subject);
            }  
        }

        public bool SendMessenger(BE.MyTalkInfo _replyInfo, int _receiverID, string _subject)
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
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-28
        /// 작성목적 : 모바일 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool SendMobile(BE.MyTalkInfo _replyInfo, int _receiverID, string _subject)
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
        #endregion 알람
    }
}