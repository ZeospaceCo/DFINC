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
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Pim.BLL
{
    public class ToDoReply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET()
        #endregion

        #region ADD()

        /// <summary>
        /// 할일 댓글 작성
        /// </summary>
        /// <param name="_shortinfo">The _shortinfo.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public bool Add(BE.ToDoReplyInfo _shortinfo, string _preLang, string _preTitle)
        {
            if (_shortinfo == null) return false;

            try
            {
                //using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                //{
                    // 짧은댓글 저장
                    DAL.ToDoReply replyDal = new DAL.ToDoReply();
                    replyDal.Add(_shortinfo);

                    DAL.ToDo todoDal = new DAL.ToDo();
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
                                    SendAlarm(_shortinfo, recvUserID, subject);
                                }                                
                            }
                        }
                    }

                    //짧은댓글 기재자들에게 알람 발송하기
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
                                        //BE.ToDoInfo todoInfo = new ToDoInfo();
                                        BE.ToDoReplyInfo replyInfo = new ToDoReplyInfo();

                                        replyInfo.CompanyID = dtrUsers["CompanyID"].ToString().ToNumber<int>();
                                        replyInfo.ToDoID = dtrUsers["ToDoID"].ToString().ToNumber<int>();

                                        replyInfo.AlarmContent = _shortinfo.Content; //알람내용
                                        replyInfo.Content = _shortinfo.Content;
                                        replyInfo.UserID = recvUserID;  //보내는 사용자 ID (작성자)
                                        replyInfo.UserName = recvName;   //보내는 사람   (작성자 이름)

                                        SendAlarm(replyInfo, dtrUsers["UserID"].ToString().ToNumber<int>(), subject);
                                        userList.Add(dtrUsers["UserID"].ToString().ToNumber<int>());
                                    }
                                }
                            }
                        }
                    }

                    //ts.Complete();
                //}

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ToDoReply.Add : " + ex);
                return false;
            }
        }

        #endregion

        #region DEL()
        #endregion

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
        public void SendAlarm(BE.ToDoReplyInfo _replyInfo, int _receiverID, string _subject)
        {
            #region 로직
            /*
             */
            #endregion

            #region 소스

            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_replyInfo.CompanyID, "AlarmPims");

            string AlarmPims = string.Empty;
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
                    AlarmPims = infoUser.AlarmPims;
                }
            }

            if (AlarmPims.IndexOf("1-").Equals(0))
            {
                AlarmMail = (AlarmPims.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                AlarmMessenger = (AlarmPims.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                AlarmMobile = (AlarmPims.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                AlarmSMS = (AlarmPims.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
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
        private bool SendMessenger(BE.ToDoReplyInfo _replyInfo, int _receiverID, string _subject)
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
            alarmInfo.KindSubject = "Pim";
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
        private bool SendMobile(BE.ToDoReplyInfo _replyInfo, int _receiverID, string _subject)
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
            alarmInfo.KindSubject = "Pim";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        #endregion
    }
}
