using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Zeospace.DeskPlus.Community.BE;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using System.Xml;
using System.Collections;

namespace Zeospace.DeskPlus.Community.BLL
{
    public class ComApply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 커뮤니티 신청 등록
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_preLang"></param>
        /// <param name="_logInID"></param>
        /// <param name="_usermail"></param>
        /// <returns></returns>
        public int Add(ComApplyInfo _info, string _preLang, string _logInID, string _usermail)
        {
            try
            {
                int applyID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. ComApply 테이블 추가
                    DAL.ComApply dal = new DAL.ComApply();
                    applyID = dal.Add(_info);
                    if (applyID < 1)
                    {
                        return -1;
                    }
                   
                    ts.Complete();                    
                }

                SendAlarm(_info, _preLang, _logInID, _usermail);

                return applyID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add()", ex);
                return -1;
            }
        }

        #endregion 

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-08
        /// 작성목적 : 커뮤니티 신청 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_applyID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _applyID, string _preLang, string _logInID, string _usermail)
        {
            try
            {
                SendAlarm(_companyID, _applyID, _preLang, _logInID, _usermail);

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.ComApply dal = new DAL.ComApply();
                 
                    if (!dal.Del(_companyID, _applyID))
                    {
                        return false;
                    }
                    
                    ts.Complete();
                }                

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del()", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-23
        /// 작성목적 : 커뮤니티 개설 승인 불가
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_applyID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, string _applyIDList, string _preLang, string _name, int _userID, string _logInID, string _usermail)
        {
            try
            {
                SendAlarm(_companyID, _applyIDList, _preLang, _name, _userID, _logInID, _usermail);

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.ComApply dal = new DAL.ComApply();

                    foreach (string applyID in _applyIDList.Split(','))
                    {
                        // 2.ComApply 테이블 삭제                    
                        if (!dal.Del(_companyID, applyID.ToNumber<int>()))
                        {
                            return false;
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del()", ex);
                return false;
            }
        }       

        #endregion 

        #region  알람 발송
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarm(ComApplyInfo _info, string _preLang, string _logInID, string _usermail)
        {
            DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "AlarmCommunity");

            string userIDMESSENGER = "";    //메신져
            string userIDMOBILE = "";       //모바일
            string userIDMAIL = "";         //메일
            string userIDSMS = "";          //SMS

            Zeospace.DeskPlus.EIP.DAL.UserRole eipdal = new Zeospace.DeskPlus.EIP.DAL.UserRole();
            DataTableReader dtrRole = eipdal.GetUserRoleCodeList(_info.CompanyID, "community");

            if (dtrRole != null)
            {
                if (dtrRole.HasRows)
                {
                    while (dtrRole.Read())
                    {
                        string AlarmCommunity = dtrRole["AlarmCommunity"].ToString();
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

                        if (AlarmCommunity.IndexOf("1-").Equals(0))
                        {
                            AlarmMail = (AlarmCommunity.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            AlarmMessenger = (AlarmCommunity.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            AlarmMobile = (AlarmCommunity.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                            AlarmSMS = (AlarmCommunity.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            AlarmMessenger = "0";
                            AlarmMail = "0";
                            AlarmMobile = "0";
                            AlarmSMS = "0";
                        }

                        if (AlarmMessenger.Equals("1"))
                        {
                            userIDMESSENGER += dtrRole["UserID"].ToString() + ",";
                        }

                        userIDMOBILE += dtrRole["UserID"].ToString() + ",";                                                                                

                        if (AlarmMail.Equals("1"))
                        {
                            userIDMAIL += dtrRole["MailID"].ToString() + "@" + dtrRole["MailDomain"].ToString() + ",";
                        }

                        if (AlarmSMS.Equals("1"))
                        {
                            userIDSMS += dtrRole["MobilePhone"].ToString() + ",";
                        }
                    }
                }

                BLL.Alarm bll = new Alarm();

                BE.AlarmInfo alarmInfo = new BE.AlarmInfo();
                alarmInfo.CompanyID = _info.CompanyID;
                alarmInfo.UserID = _info.WriteUserID;
                alarmInfo.UserName = _info.WriteUserName;
                alarmInfo.Name = _info.Name;
                alarmInfo.Description = _info.Description;


                foreach (DataRowView dr in dvAlarm)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MAIL":
                            if (userIDMAIL.Length > 0)
                            {
                                bll.SendMail(alarmInfo, _usermail, userIDMAIL, "Mail", "COMMUNITYADD", _preLang);
                            }
                            break;

                        case "MESSENGER":
                            if (userIDMESSENGER.Length > 0)
                            {
                                foreach (string userID in userIDMESSENGER.Split(','))
                                {
                                    if (userID != "")
                                    {
                                        bll.SendAlarm(alarmInfo, userID.ToNumber<int>(), "Messenger", "COMMUNITYADD", _preLang);
                                    }
                                }
                            }
                            break;

                        case "SMS":
                            if (userIDSMS.Length > 0)
                            {
                                string[] message = bll.GetAlarmMessage(alarmInfo, "SMS", "COMMUNITYADD", _preLang);
                                string smsContent = message[0];

                                foreach (string mobile in userIDSMS.Split(','))
                                {
                                    if (mobile != "")
                                    {
                                        Common.BLL.SMS smsbll = new Common.BLL.SMS();
                                        smsbll.SendSMS(_info.CompanyID, _info.WriteUserID, _logInID, smsContent, false, mobile);
                                    }
                                }
                            }
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력
                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                {
                    if (userIDMOBILE.Length > 0)
                    {
                        foreach (string userID in userIDMOBILE.Split(','))
                        {
                            if (userID != "")
                            {
                                bll.SendMobile(alarmInfo, userID.ToNumber<int>(), "Messenger", "COMMUNITYADD", _preLang);
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 커뮤니티 삭제
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarm(int _companyID, int _applyID, string _preLang, string _logInID, string _usermail)
        {
            DataView dvAlarm = AppSettingHelper.GetSysCode(_companyID, "AlarmCommunity");

            string userIDMESSENGER = "";    //메신져
            string userIDMOBILE = "";       //모바일
            string userIDMAIL = "";         //메일
            string userIDSMS = "";          //SMS

            Zeospace.DeskPlus.EIP.DAL.UserRole eipdal = new Zeospace.DeskPlus.EIP.DAL.UserRole();
            DataTableReader dtrRole = eipdal.GetUserRoleCodeList(_companyID, "community");

            if (dtrRole != null)
            {
                if (dtrRole.HasRows)
                {
                    while (dtrRole.Read())
                    {
                        string AlarmCommunity = dtrRole["AlarmCommunity"].ToString();
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

                        if (AlarmCommunity.IndexOf("1-").Equals(0))
                        {
                            AlarmMail = (AlarmCommunity.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            AlarmMessenger = (AlarmCommunity.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            AlarmMobile = (AlarmCommunity.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                            AlarmSMS = (AlarmCommunity.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            AlarmMessenger = "0";
                            AlarmMail = "0";
                            AlarmMobile = "0";
                            AlarmSMS = "0";
                        }

                        if (AlarmMessenger.Equals("1"))
                        {
                            userIDMESSENGER += dtrRole["UserID"].ToString() + ",";
                        }

                        if (AlarmMobile.Equals("1"))
                        {
                            userIDMOBILE += dtrRole["UserID"].ToString() + ",";
                        }                                                                                               

                        if (AlarmMail.Equals("1"))
                        {
                            userIDMAIL += dtrRole["MailID"].ToString() + "@" + dtrRole["MailDomain"].ToString() + ",";
                        }

                        if (AlarmSMS.Equals("1"))
                        {
                            userIDSMS += dtrRole["MobilePhone"].ToString() + ",";
                        }
                    }
                }

                DAL.ComApply dal = new DAL.ComApply();
                DataTableReader dtr = dal.GetListInfo(_companyID, _applyID);

                BE.AlarmInfo alarmInfo = new BE.AlarmInfo();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            alarmInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            alarmInfo.UserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            alarmInfo.UserName = dtr["WriteUserName"].ToString();
                            alarmInfo.Name = dtr["Name"].ToString();
                            alarmInfo.Description = dtr["Description"].ToString();
                        }
                    }
                }

                BLL.Alarm bll = new Alarm();

                foreach (DataRowView dr in dvAlarm)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MAIL":
                            if (userIDMAIL.Length > 0)
                            {
                                bll.SendMail(alarmInfo, _usermail, userIDMAIL, "Mail", "COMMUNITYCANCEL", _preLang);
                            }
                            break;

                        case "MESSENGER":
                            if (userIDMESSENGER.Length > 0)
                            {
                                foreach (string userID in userIDMESSENGER.Split(','))
                                {
                                    if (userID != "")
                                    {
                                        bll.SendAlarm(alarmInfo, userID.ToNumber<int>(), "Messenger", "COMMUNITYCANCEL", _preLang);
                                    }
                                }
                            }
                            break;

                        case "SMS":
                            if (userIDSMS.Length > 0)
                            {
                                string[] message = bll.GetAlarmMessage(alarmInfo, "SMS", "COMMUNITYCANCEL", _preLang);
                                string smsContent = message[0];

                                foreach (string mobile in userIDSMS.Split(','))
                                {
                                    if (mobile != "")
                                    {
                                        Common.BLL.SMS smsbll = new Common.BLL.SMS();
                                        smsbll.SendSMS(_companyID, alarmInfo.UserID, _logInID, smsContent, false, mobile);
                                    }
                                }
                            }
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력
                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                {
                    if (userIDMOBILE.Length > 0)
                    {
                        foreach (string userID in userIDMOBILE.Split(','))
                        {
                            if (userID != "")
                            {
                                bll.SendMobile(alarmInfo, userID.ToNumber<int>(), "Messenger", "COMMUNITYCANCEL", _preLang);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 커뮤니티 신청불가
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarm(int _companyID, string _applyIDList, string _preLang, string _name, int _userID, string _logInID, string _usermail)
        {
            DAL.ComApply dal = new DAL.ComApply();

            //  1. 알람 발송
            DataView dvAlarm = AppSettingHelper.GetSysCode(_companyID, "AlarmCommunity");

            string userIDMESSENGER = "";    //메신져
            string userIDMOBILE = "";       //모바일
            string userIDMAIL = "";         //메일
            string userIDSMS = "";          //SMS

            ArrayList Receiver = new ArrayList();

            Zeospace.DeskPlus.EIP.DAL.UserRole eipdal = new Zeospace.DeskPlus.EIP.DAL.UserRole();
            DataTableReader dtrRole = eipdal.GetUserRoleCodeList(_companyID, "community");

            if (dtrRole != null)
            {
                if (dtrRole.HasRows)
                {
                    while (dtrRole.Read())
                    {
                        string AlarmCommunity = dtrRole["AlarmCommunity"].ToString();
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

                        if (AlarmCommunity.IndexOf("1-").Equals(0))
                        {
                            AlarmMail = (AlarmCommunity.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            AlarmMessenger = (AlarmCommunity.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            AlarmMobile = (AlarmCommunity.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                            AlarmSMS = (AlarmCommunity.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            AlarmMessenger = "0";
                            AlarmMail = "0";
                            AlarmMobile = "0";
                            AlarmSMS = "0";
                        }

                        Receiver.Add(dtrRole["UserID"].ToString());

                        if (AlarmMessenger.Equals("1"))
                        {
                            userIDMESSENGER += dtrRole["UserID"].ToString() + ",";
                        }
                        
                        if (AlarmMobile.Equals("1"))
                        {
                            userIDMOBILE += dtrRole["UserID"].ToString() + ",";
                        }                                                      

                        if (AlarmMail.Equals("1"))
                        {
                            userIDMAIL += dtrRole["MailID"].ToString() + "@" + dtrRole["MailDomain"].ToString() + ",";
                        }

                        if (AlarmSMS.Equals("1"))
                        {
                            userIDSMS += dtrRole["MobilePhone"].ToString() + ",";
                        }
                    }
                }         

                foreach (string applyID in _applyIDList.Split(','))
                {
                    BE.AlarmInfo alarmInfo = new BE.AlarmInfo();

                    DataTableReader dtr = dal.GetInfo(_companyID, applyID.ToNumber<int>());                    

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                string AlarmCommunity = dtrRole["AlarmCommunity"].ToString();
                                string AlarmMail = string.Empty;
                                string AlarmMessenger = string.Empty;
                                string AlarmMobile = string.Empty;
                                string AlarmSMS = string.Empty;

                                if (AlarmCommunity.IndexOf("1-").Equals(0))
                                {
                                    AlarmMail = (AlarmCommunity.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                    AlarmMessenger = (AlarmCommunity.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                    AlarmMobile = (AlarmCommunity.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                    AlarmSMS = (AlarmCommunity.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                                }
                                else
                                {
                                    AlarmMessenger = "0";
                                    AlarmMail = "0";
                                    AlarmMobile = "0";
                                    AlarmSMS = "0";
                                }

                                alarmInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                                alarmInfo.UserID = _userID;
                                alarmInfo.UserName = _name;
                                alarmInfo.Name = dtr["CMNName"].ToString();
                                alarmInfo.Description = dtr["Description"].ToString();

                                if (!Receiver.Contains(dtr["WriteUserID"].ToString()))
                                {
                                    if (AlarmMessenger.Equals("1"))
                                    {
                                        userIDMESSENGER += dtrRole["WriteUserID"].ToString() + ",";
                                    }

                                    if (AlarmMobile.Equals("1"))
                                    {
                                        userIDMOBILE += dtrRole["WriteUserID"].ToString() + ",";
                                    }                                                                                          

                                    if (AlarmMail.Equals("1"))
                                    {
                                        userIDMAIL += dtrRole["MailID"].ToString() + "@" + dtrRole["MailDomain"].ToString() + ",";
                                    }

                                    if (AlarmSMS.Equals("1"))
                                    {
                                        userIDSMS += dtrRole["MobilePhone"].ToString() + ",";
                                    }
                                }                                
                            }
                        }
                    }

                    BLL.Alarm bll = new Alarm();

                    foreach (DataRowView dr in dvAlarm)
                    {
                        string method = dr["CodeID"].ToString().ToUpper();
                        switch (method)
                        {
                            case "MAIL":
                                if (userIDMAIL.Length > 0)
                                {
                                    bll.SendMail(alarmInfo, _usermail, userIDMAIL, "Mail", "COMMUNITYREJECT", _preLang);
                                }
                                break;

                            case "MESSENGER":
                                if (userIDMESSENGER.Length > 0)
                                {
                                    foreach (string userID in userIDMESSENGER.Split(','))
                                    {
                                        if (userID != "")
                                        {
                                            bll.SendAlarm(alarmInfo, userID.ToNumber<int>(), "Messenger", "COMMUNITYREJECT", _preLang);
                                        }
                                    }
                                }
                                break;

                            case "SMS":
                                if (userIDSMS.Length > 0)
                                {
                                    string[] message = bll.GetAlarmMessage(alarmInfo, "SMS", "COMMUNITYREJECT", _preLang);
                                    string smsContent = message[0];

                                    foreach (string mobile in userIDSMS.Split(','))
                                    {
                                        if (mobile != "")
                                        {
                                            Common.BLL.SMS smsbll = new Common.BLL.SMS();
                                            smsbll.SendSMS(_companyID, alarmInfo.UserID, _logInID, smsContent, false, mobile);
                                        }
                                    }
                                }
                                break;
                        }
                    }

                    // 모바일사용의 경우, 무조건 입력
                    if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                    {
                        if (userIDMOBILE.Length > 0)
                        {
                            foreach (string userID in userIDMOBILE.Split(','))
                            {
                                if (userID != "")
                                {
                                    bll.SendMobile(alarmInfo, userID.ToNumber<int>(), "Messenger", "COMMUNITYREJECT", _preLang);
                                }
                            }
                        }
                    }
                }                
            }
        }

        #endregion
    }
}