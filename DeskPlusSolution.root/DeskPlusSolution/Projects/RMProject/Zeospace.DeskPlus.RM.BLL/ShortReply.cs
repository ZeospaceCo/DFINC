using System;
using System.Collections.Generic;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.RM.BE;
using System.Data;
using System.Xml;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.BLL
{
    public class ShortReply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 댓글 저장
        /// </summary>
        /// <param name="_shortinfo"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool Add(BE.ShortReplyInfo _shortinfo, int _parentReservationID, string _preLang)
        {
            if (_shortinfo == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //댓글 저장
                    DAL.ShortReply dal = new DAL.ShortReply();
                    dal.Add(_shortinfo);                    

                    ts.Complete();
                }

                #region 알람발송
                SendAlarm(_shortinfo, _parentReservationID, _preLang);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 댓글 수정
        /// </summary>
        /// <param name="_shortinfo"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool Mod(BE.ShortReplyInfo _shortinfo, int _parentReservationID, string _preLang)
        {
            if (_shortinfo == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //댓글 수정
                    DAL.ShortReply dal = new DAL.ShortReply();
                    dal.Mod(_shortinfo);                    

                    ts.Complete();
                }

                #region 알람발송
                SendAlarm(_shortinfo, _parentReservationID, _preLang);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return false;
            }
        }

        #endregion ADD()

        #region 알람

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarm(BE.ShortReplyInfo _shortinfo, int _parentReservationID, string _preLang)
        {

            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_shortinfo.CompanyID, "AlarmResource");
            if (dvAlarm.Count.Equals(0)) return;

            //알람
            DAL.Reverse reDal = new DAL.Reverse();
            DataTableReader dtr = reDal.GetList(_shortinfo.CompanyID, _shortinfo.ReservationID);

            int recvUserID = 0;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //원본글 작성자에게 알람 발송하기
                        recvUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        _shortinfo.Title = dtr["Subject"].ToString();

                        string AlarmRM = dtr["AlarmRM"].ToString();
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

                        if (AlarmRM.IndexOf("1-").Equals(0))
                        {
                            AlarmMail = (AlarmRM.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            AlarmMessenger = (AlarmRM.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            AlarmMobile = (AlarmRM.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                            AlarmSMS = (AlarmRM.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            AlarmMessenger = "0";
                            AlarmMail = "0";
                            AlarmMobile = "0";
                            AlarmSMS = "0";
                        }

                        if (_shortinfo.UserID != recvUserID)
                        {
                            foreach (DataRowView dr in dvAlarm)
                            {
                                string method = dr["CodeID"].ToString().ToUpper();
                                switch (method)
                                { 
                                    case "MESSENGER":
                                        if (AlarmMessenger.Equals("1"))
                                        {
                                            SendMessenger(_shortinfo, recvUserID, _preLang);
                                        }
                                        break;
                                }
                            }

                            // 모바일사용의 경우, 무조건 입력
                            if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                            {
                                SendMobile(_shortinfo, recvUserID, _preLang);                                     
                            }  
                        }
                    }
                }
            }

            //짧은댓글 기재자들에게 알람 발송하기
            DAL.ShortReply dal = new DAL.ShortReply();
            var dtrShort = dal.GetShortReplyList(_shortinfo.CompanyID, _shortinfo.ReservationID);
            List<int> userList = new List<int>();
            if (dtrShort != null)
            {
                if (dtrShort.HasRows)
                {
                    while (dtrShort.Read())
                    {
                        string AlarmRM = dtrShort["AlarmRM"].ToString();
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

                        if (AlarmRM.IndexOf("1-").Equals(0))
                        {
                            AlarmMail = (AlarmRM.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            AlarmMessenger = (AlarmRM.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            AlarmMobile = (AlarmRM.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                            AlarmSMS = (AlarmRM.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            AlarmMessenger = "0";
                            AlarmMail = "0";
                            AlarmMobile = "0";
                            AlarmSMS = "0";
                        }

                        //짧은댓글 작성자와 원본작성자와 동일하면 보내지 않는다.
                        if ((recvUserID != dtrShort["UserID"].ToString().ToNumber<int>()) && (_shortinfo.UserID != dtrShort["UserID"].ToString().ToNumber<int>()))
                        {
                            //다중 댓글 중복자에게 보내지 않도록 한다. 1통만 보내자.~~~~
                            if (!userList.Exists(c => c == dtrShort["UserID"].ToString().ToNumber<int>()))
                            {                                   
                                foreach (DataRowView dr in dvAlarm)
                                {
                                    string method = dr["CodeID"].ToString().ToUpper();
                                    switch (method)
                                    {
                                        case "MESSENGER":
                                            if (AlarmMessenger.Equals("1"))
                                            {
                                                SendMessenger(_shortinfo, dtrShort["UserID"].ToString().ToNumber<int>(), _preLang);
                                            }
                                            break;
                                    }
                                }

                                // 모바일사용의 경우, 무조건 입력
                                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                                {
                                    SendMobile(_shortinfo, dtrShort["UserID"].ToString().ToNumber<int>(), _preLang);                                    
                                }  

                                userList.Add(dtrShort["UserID"].ToString().ToNumber<int>());
                            }
                        }
                    }
                }
            }


            //공유자에게 알람 발송
            DAL.ShareUser suDal = new DAL.ShareUser();
            DataTableReader dtrShareUser = suDal.GetShareUserListByID(_shortinfo.CompanyID, _parentReservationID);

            if (dtrShareUser != null)
            {
                if (dtrShareUser.HasRows)
                {
                    while (dtrShareUser.Read())
                    {
                        string AlarmRM = dtrShareUser["AlarmRM"].ToString();
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

                        if (AlarmRM.IndexOf("1-").Equals(0))
                        {
                            AlarmMail = (AlarmRM.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            AlarmMessenger = (AlarmRM.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            AlarmMobile = (AlarmRM.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                            AlarmSMS = (AlarmRM.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            AlarmMessenger = "0";
                            AlarmMail = "0";
                            AlarmMobile = "0";
                            AlarmSMS = "0";
                        }

                        if (_shortinfo.UserID != dtrShareUser["UserID"].ToString().ToNumber<int>())
                        {
                            //공유자와 원본작성자, 댓글작성자와 동일하면 보내지 않는다.
                            if (recvUserID != dtrShareUser["UserID"].ToString().ToNumber<int>() && !userList.Contains(dtrShareUser["UserID"].ToString().ToNumber<int>()))
                            {
                                foreach (DataRowView dr in dvAlarm)
                                {
                                    string method = dr["CodeID"].ToString().ToUpper();
                                    switch (method)
                                    {
                                        case "MESSENGER":
                                            if (AlarmMessenger.Equals("1"))
                                            {
                                                SendMessenger(_shortinfo, dtrShareUser["UserID"].ToString().ToNumber<int>(), _preLang);
                                            }
                                            break;
                                    }
                                }

                                // 모바일사용의 경우, 무조건 입력
                                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                                {
                                    SendMobile(_shortinfo, dtrShareUser["UserID"].ToString().ToNumber<int>(), _preLang);                                  
                                }                                  
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 메신저 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool SendMessenger(BE.ShortReplyInfo _info, int _receiverID, string _preLang)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            AlarmInfo alarmInfo = new Common.BE.AlarmInfo();
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
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.RESOURCEMANAGER, "AlarmViewTargetURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("reservationID", _info.ReservationID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "ResourceManager"; //제목에 안들어가며, 구분자값임
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-10-20
        /// 작성목적 : 메신저 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public bool SendMobile(BE.ShortReplyInfo _info, int _receiverID, string _preLang)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            AlarmInfo alarmInfo = new Common.BE.AlarmInfo();
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
            string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.RESOURCEMANAGER, "AlarmViewTargetURL", "");
            alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("reservationID", _info.ReservationID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = "/DeskPlusMobile/RM/AlarmView.aspx?p=" + encparam;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "ResourceManager"; //제목에 안들어가며, 구분자값임
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        } 
        #endregion 알람
    }
}