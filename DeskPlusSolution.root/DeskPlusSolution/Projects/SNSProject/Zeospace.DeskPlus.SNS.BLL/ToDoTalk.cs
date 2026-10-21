using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.SNS.BE;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.SNS.BLL
{
    public class ToDoTalk : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 할일 댓글 작성
        /// </summary>
        /// <param name="_shortinfo">The _shortinfo.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public bool Add(BE.ToDoTalkInfo _shortinfo, string _preLang, string _preTitle)
        {
            if (_shortinfo == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 짧은댓글 저장
                    DAL.ToDoTalk replyDal = new DAL.ToDoTalk();
                    replyDal.Add(_shortinfo);                    

                    ts.Complete();
                }

                SendAlarm(_shortinfo, _preLang, _preTitle);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ToDoTalk.Add : " + ex);
                return false;
            }
        }

        #endregion ADD()

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
        public void SendAlarm(BE.ToDoTalkInfo _shortinfo, string _preLang, string _preTitle)
        {
            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_shortinfo.CompanyID, "AlarmSNS");
            if (dvAlarm.Count.Equals(0)) return;

            Zeospace.DeskPlus.Pim.DAL.ToDo todoDal = new Zeospace.DeskPlus.Pim.DAL.ToDo();
            DataTableReader dtr = todoDal.GetTodoID(_shortinfo.CompanyID, _shortinfo.ToDoID);

            int recvUserID = 0;
            string recvName = "";
            string recvTitle = "";
            string subject = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        string AlarmSNS = dtr["AlarmSNS"].ToString();
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

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

                        //원본 작성자 아이디
                        recvUserID = dtr["UserID"].ToString().ToNumber<int>();

                        //원본 작성자 이름
                        recvName = UtilHelper.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            _preLang);

                        recvTitle = dtr["Title"].ToString().ToReverseXSS(); //제목

                        // 알람제목
                        subject = "[" + _preTitle.Split(',')[0] + "]" + recvTitle + _preTitle.Split(',')[1];

                        //원본작성자와 짧은댓글 작성자가 동일하면 보내지 않는다.
                        if (recvUserID != _shortinfo.UserID)
                        {
                            // 알람발송(원본글)                            
                            foreach (DataRowView dr in dvAlarm)
                            {
                                string method = dr["CodeID"].ToString().ToUpper();
                                switch (method)
                                {
                                    case "MESSENGER":
                                        if (AlarmMessenger.Equals("1"))
                                        {
                                            SendMessenger(_shortinfo, recvUserID, subject);
                                        }
                                        break;
                                }
                            }

                            // 모바일사용의 경우, 무조건 입력
                            if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                            {
                                SendMobile(_shortinfo, recvUserID, subject);
                            }  
                        }
                    }
                }
            }

            //짧은댓글 기재자들에게 알람 발송하기
            DAL.ToDoTalk replyDal = new DAL.ToDoTalk();
            var dtrUsers = replyDal.GetShortReplyList(_shortinfo.CompanyID, _shortinfo.ToDoID);
            //짧은댓글 작성자 아이디 정보
            List<int> userList = new List<int>();
            if (dtrUsers != null)
            {
                if (dtrUsers.HasRows)
                {
                    while (dtrUsers.Read())
                    {
                        //짧은댓글 작성자와 원본작성자와 동일하면 보내지 않는다.
                        if (recvUserID != dtrUsers["UserID"].ToString().ToNumber<int>() && _shortinfo.UserID != dtrUsers["UserID"].ToString().ToNumber<int>())
                        {
                            //다중 댓글 중복자에게 보내지 않도록 한다. 1통만 보내자.~~~~
                            if (!userList.Exists(c => c == dtrUsers["UserID"].ToString().ToNumber<int>()))
                            {
                                Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
                                List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(_shortinfo.CompanyID, dtrUsers["UserID"].ToString().ToNumber<int>());

                                string AlarmSNS = string.Empty;
                                string AlarmMail = string.Empty;
                                string AlarmMessenger = string.Empty;
                                string AlarmMobile = string.Empty;
                                string AlarmSMS = string.Empty;

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
                                
                                BE.ToDoTalkInfo replyInfo = new ToDoTalkInfo();

                                replyInfo.CompanyID = dtrUsers["CompanyID"].ToString().ToNumber<int>();
                                replyInfo.ToDoID = dtrUsers["ToDoID"].ToString().ToNumber<int>();

                                replyInfo.AlarmContent = _shortinfo.Content; //알람내용
                                replyInfo.Content = _shortinfo.Content;
                                replyInfo.UserID = recvUserID;  //보내는 사용자 ID (작성자)
                                replyInfo.UserName = recvName;   //보내는 사람   (작성자 이름)

                                foreach (DataRowView dr in dvAlarm)
                                {
                                    string method = dr["CodeID"].ToString().ToUpper();
                                    switch (method)
                                    {
                                        case "MESSENGER":
                                            if (AlarmMessenger.Equals("1"))
                                            {
                                                SendMessenger(replyInfo, dtrUsers["UserID"].ToString().ToNumber<int>(), subject);
                                            }
                                            break;
                                    }
                                }

                                // 모바일사용의 경우, 무조건 입력
                                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                                {
                                    SendMobile(replyInfo, dtrUsers["UserID"].ToString().ToNumber<int>(), subject);
                                } 
                                
                                userList.Add(dtrUsers["UserID"].ToString().ToNumber<int>());
                            }
                        }
                    }
                }
            }
        }      

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 메신져 보내기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-04-28
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <returns></returns>
        public bool SendMessenger(BE.ToDoTalkInfo _replyInfo, int _receiverID, string _subject)
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
            //alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            alarmTargetURL = baseHttp + fullDomain + "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims";

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
        /// 메서드명   :
        /// 작성목적   : 모바일 보내기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-04-28
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <param name="_listID">The _list ID.</param>
        /// <param name="_receiverID">The _receiver ID.</param>
        /// <returns></returns>
        public bool SendMobile(BE.ToDoTalkInfo _replyInfo, int _receiverID, string _subject)
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
            //alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            alarmTargetURL = baseHttp + fullDomain + "/DeskPlusEIP/EIP/DeskPlusEPDefault.aspx?dcparm=webpims";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("ToDoID", _replyInfo.ToDoID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            string alarmMobileURL = "/DeskPlusMobile/Pims/AlarmView.aspx?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = alarmMobileURL;
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