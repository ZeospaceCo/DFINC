using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.BLL
{
    public class UserApply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 커뮤니티 성격, 회원 가입 방식에 따른 가입
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_cmnName"></param>
        /// <param name="_preLang"></param>
        /// <param name="_logInID"></param>
        /// <param name="_type"></param>
        /// <param name="_joinType"></param>
        /// <param name="_usermail"></param>
        /// <returns></returns>
        public bool Add(UserApplyInfo _info, string _cmnName, string _preLang, string _logInID, int _type, int _joinType, string _usermail)
        {
            try
            {
                DAL.User userdal = new DAL.User();                
                DAL.UserGroupMem dalGroupMem = new DAL.UserGroupMem();
                DAL.UserApply dalUserApply = new DAL.UserApply();

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    #region 1. UserApply 테이블 추가(1: 공개(승인필요), 2: 비공개), UserGroupMem, UserInfo 테이블 추가 (1: 공개(바로 가입), 3: 퀴즈형)

                    if (_type == 1)
                    {
                        if (_joinType == 1)
                        {
                            //UserGroupMemInfo
                            if (!dalGroupMem.Add(_info.CompanyID, _info.CommunityID, _info.UserID)) return false;

                            //UserInfo
                            BE.UserInfo userInfo = new UserInfo();

                            userInfo.CompanyID = _info.CompanyID;
                            userInfo.CommunityID = _info.CommunityID;
                            userInfo.UserID = _info.UserID;
                            userInfo.Description = _info.Description;
                            userInfo.Grade = 9;
                            userInfo.Status = 1;

                            if (!userdal.Add(userInfo)) return false;                            
                        }
                        else
                        {
                            if (!dalUserApply.Add(_info)) return false;
                        }
                    }
                    else if (_type == 2)
                    {
                        if (!dalUserApply.Add(_info)) return false;
                    }
                    else if (_type == 3)
                    {
                        //UserGroupMemInfo
                        if (!dalGroupMem.Add(_info.CompanyID, _info.CommunityID, _info.UserID)) return false;

                        //UserInfo
                        BE.UserInfo userInfo = new UserInfo();

                        userInfo.CompanyID = _info.CompanyID;
                        userInfo.CommunityID = _info.CommunityID;
                        userInfo.UserID = _info.UserID;
                        userInfo.Description = _info.Description;
                        userInfo.Grade = 9;
                        userInfo.Status = 1;

                        if (!userdal.Add(userInfo)) return false; 
                    }
                    else
                    {
                        return false;
                    }

                    #endregion
                   
                    ts.Complete();
                }

                SendAlarm(_info, _cmnName, _preLang, _logInID, _type, _joinType, _usermail);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del()", ex);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-17
        /// 작성목적 : 가입신청 관리에서 승인처리
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userIDList"></param>
        /// <param name="_communityID"></param>
        /// <param name="_preLang"></param>
        /// <param name="_adminName"></param>
        /// <param name="_adminUserID"></param>
        /// <param name="_loginID"></param>
        /// <param name="_usermail"></param>
        /// <returns></returns>
        public bool Add(int _companyID, string _userIDList, int _communityID, string _preLang, string _adminName, int _adminUserID, string _loginID, string _usermail)
        {
            try
            {
                DAL.User dalUser = new DAL.User();
                DAL.UserApply dalUserApply = new DAL.UserApply();
                DAL.UserGroupMem dalGroupMem = new DAL.UserGroupMem();

                SendAlarm(_companyID, _userIDList, _communityID, _preLang, _adminName, _adminUserID, _loginID, _usermail);

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (string userID in _userIDList.Split(','))
                    {
                        #region UserInfo 테이블에 추가, UserGroupMem 테이블에 추가, UserApply 테이블 삭제, 알람 발송

                        // 1. UserInfo 테이블에 추가
                        if (!dalUser.Add(_companyID, _communityID, userID.ToNumber<int>())) return false;

                        // 2. UserGroupMem 테이블에 추가
                        if (!dalGroupMem.Add(_companyID, _communityID, userID.ToNumber<int>())) return false;

                        // 3.  UserApply 테이블 삭제  
                        if (!dalUserApply.Del(_companyID, _communityID, userID.ToNumber<int>())) return false;

                        #endregion
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

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-08
        /// 작성목적 : 가입 신청한 커뮤니티 취소하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <param name="_userID"></param>
        /// <param name="_preLang"></param>
        /// <param name="_logInID"></param>
        /// <param name="_usermail"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _communityID, int _userID, string _preLang, string _logInID, string _usermail)
        {
            try
            {
                SendAlarm(_companyID, _communityID, _userID, _preLang, _logInID, _usermail);

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.UserApply dal = new DAL.UserApply();
                   
                    // 2. UserApply 테이블 삭제                    
                    if (!dal.Del(_companyID, _communityID, _userID))
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
        /// 작성자 : shine
        /// 작성일자 : 2012-05-17
        /// 작성목적 : 커뮤니티신청관리의 승인불가
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userIDList">The _user ID list.</param>
        /// <param name="_communityID">The _community ID.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public bool Del(int _companyID, string _userIDList, int _communityID, string _preLang, string _adminName, int _adminUserID, string _loginID, string _usermail)
        {
            try
            {
                DAL.UserApply dalUserApply = new DAL.UserApply();
                BLL.Alarm bll = new Alarm();
                Common.BLL.SMS smsbll = new Common.BLL.SMS();

                SendAlarmDeNY(_companyID, _userIDList, _communityID, _preLang, _adminName, _adminUserID, _loginID, _usermail);

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (string userID in _userIDList.Split(','))
                    {
                        #region UserApply 테이블 삭제, 알람발송
                       
                        // 2. UserApply 테이블 삭제
                        if (!dalUserApply.Del(_companyID, _communityID, userID.ToNumber<int>())) return false;

                        #endregion
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
        /// 작성목적 : 커뮤니티 성격, 회원 가입 방식에 따른 가입 알람 발송
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarm(UserApplyInfo _info, string _cmnName, string _preLang, string _logInID, int _type, int _joinType, string _usermail)
        {
            BLL.Alarm bll = new Alarm();
            DAL.User userdal = new DAL.User();                

            DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "AlarmCommunity");

            string userIDMESSENGER = "";    //메신져
            string userIDMOBILE = "";       //모바일
            string userIDMAIL = "";         //메일
            string userIDSMS = "";          //SMS            

            if ((_type == 1 && _joinType == 2) || _type == 2)
            {
                BE.AlarmInfo alarmInfo = new BE.AlarmInfo();

                alarmInfo.CompanyID = _info.CompanyID;
                alarmInfo.UserID = _info.UserID;
                alarmInfo.UserName = _info.UserName;
                alarmInfo.Name = _cmnName;
                alarmInfo.Description = _info.Description;

                DataTableReader dtrUser = userdal.GetGradeList(_info.CompanyID, _info.CommunityID, 2); //커뮤니티지기 Grade : 2

                if (dtrUser != null)
                {
                    if (dtrUser.HasRows)
                    {
                        while (dtrUser.Read())
                        {
                            string AlarmCommunity = dtrUser["AlarmCommunity"].ToString();
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
                                userIDMESSENGER += dtrUser["UserID"].ToString() + ",";
                            }

                            userIDMOBILE += dtrUser["UserID"].ToString() + ",";

                            if (AlarmMail.Equals("1"))
                            {
                                userIDMAIL += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                            }

                            if (AlarmSMS.Equals("1"))
                            {
                                userIDSMS += dtrUser["MobilePhone"].ToString() + ",";
                            }

                        }
                    }
                }

                foreach (DataRowView dr in dvAlarm)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MAIL":
                            if (userIDMAIL.Length > 0)
                            {
                                bll.SendMail(alarmInfo, _usermail, userIDMAIL, "Mail", "COMMUNITYJOIN", _preLang);
                            }
                            break;

                        case "MESSENGER":
                            if (userIDMESSENGER.Length > 0)
                            {
                                foreach (string userID in userIDMESSENGER.Split(','))
                                {
                                    if (userID != "")
                                    {
                                        bll.SendAlarm(alarmInfo, userID.ToNumber<int>(), "Messenger", "COMMUNITYJOIN", _preLang);
                                    }
                                }
                            }
                            break;

                        case "SMS":
                            if (userIDSMS.Length > 0)
                            {
                                string[] message = bll.GetAlarmMessage(alarmInfo, "SMS", "COMMUNITYJOIN", _preLang);
                                string smsContent = message[0];

                                foreach (string mobile in userIDSMS.Split(','))
                                {
                                    if (mobile != "")
                                    {
                                        Common.BLL.SMS smsbll = new Common.BLL.SMS();
                                        smsbll.SendSMS(alarmInfo.CompanyID, alarmInfo.UserID, _logInID, smsContent, false, mobile);
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
                                bll.SendMobile(alarmInfo, userID.ToNumber<int>(), "Messenger", "COMMUNITYJOIN", _preLang);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 승인처리 알람 발송
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarm(int _companyID, string _userIDList, int _communityID, string _preLang, string _adminName, int _adminUserID, string _loginID, string _usermail)
        {
            BLL.Alarm bll = new Alarm();
            DAL.User dalUser = new DAL.User();
            
            DataView dvAlarm = AppSettingHelper.GetSysCode(_companyID, "AlarmCommunity");

            string userIDMESSENGER = "";    //메신져
            string userIDMOBILE = "";       //모바일
            string userIDMAIL = "";         //메일
            string userIDSMS = "";          //SMS     

            foreach (string userID in _userIDList.Split(','))
            {
                BE.AlarmInfo alarmInfo = new BE.AlarmInfo();
                alarmInfo.CompanyID = _companyID;
                alarmInfo.UserID = _adminUserID;
                alarmInfo.UserName = _adminName;
                alarmInfo.Description = "";

                DataTableReader getUserDtr = dalUser.GetUserInfo(_companyID, _communityID, userID.ToNumber<int>());
                if (getUserDtr != null)
                {
                    if (getUserDtr.HasRows)
                    {
                        while (getUserDtr.Read())
                        {
                            string AlarmCommunity = getUserDtr["AlarmCommunity"].ToString();
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

                            alarmInfo.Name = getUserDtr["CMNName"].ToString();

                            if (AlarmMessenger.Equals("1"))
                            {
                                userIDMESSENGER += userID + ",";
                            }

                            userIDMOBILE += userID + ",";

                            if (AlarmMail.Equals("1"))
                            {
                                userIDMAIL += getUserDtr["MailID"].ToString() + "@" + getUserDtr["MailDomain"].ToString() + ",";
                            }

                            if (AlarmSMS.Equals("1"))
                            {
                                userIDSMS += getUserDtr["MobilePhone"].ToString() + ",";
                            }

                        }
                    }
                }

                foreach (DataRowView dr in dvAlarm)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MAIL":
                            if (userIDMAIL.Length > 0)
                            {
                                bll.SendMail(alarmInfo, _usermail, userIDMAIL, "Mail", "COMMUNITYAPPROVE", _preLang);
                            }
                            break;

                        case "MESSENGER":
                            if (userIDMESSENGER.Length > 0)
                            {
                                foreach (string rcvUserID in userIDMESSENGER.Split(','))
                                {
                                    if (rcvUserID != "")
                                    {
                                        bll.SendAlarm(alarmInfo, rcvUserID.ToNumber<int>(), "Messenger", "COMMUNITYAPPROVE", _preLang);
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
                                        smsbll.SendSMS(alarmInfo.CompanyID, alarmInfo.UserID, _loginID, smsContent, false, mobile);
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
                        foreach (string rcvUserID in userIDMOBILE.Split(','))
                        {
                            if (rcvUserID != "")
                            {
                                bll.SendMobile(alarmInfo, rcvUserID.ToNumber<int>(), "Messenger", "COMMUNITYREJECT", _preLang);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 가입 신청한 커뮤니티 취소하기
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarm(int _companyID, int _communityID, int _userID, string _preLang, string _logInID, string _usermail)
        {
            DAL.UserApply dal = new DAL.UserApply();

            DataView dvAlarm = AppSettingHelper.GetSysCode(_companyID, "AlarmCommunity");

            string userIDMESSENGER = "";    //메신져
            string userIDMOBILE = "";       //모바일
            string userIDMAIL = "";         //메일
            string userIDSMS = "";          //SMS     

            DataTableReader dtr = dal.GetList(_companyID, _communityID, _userID);

            BE.AlarmInfo alarmInfo = new BE.AlarmInfo();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        alarmInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        alarmInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        alarmInfo.UserName = dtr["UserName"].ToString();
                        alarmInfo.Name = dtr["CMNName"].ToString();
                        alarmInfo.Description = dtr["Description"].ToString();
                    }
                }
            }

            DAL.User userdal = new DAL.User();
            DataTableReader dtrUser = userdal.GetGradeList(_companyID, _communityID, 2); //커뮤니티지기 Grade : 2

            if (dtrUser != null)
            {
                if (dtrUser.HasRows)
                {
                    while (dtrUser.Read())
                    {
                        string AlarmCommunity = dtrUser["AlarmCommunity"].ToString();
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
                            userIDMESSENGER += dtrUser["UserID"].ToString() + ",";
                        }

                        userIDMOBILE += dtrUser["UserID"].ToString() + ",";

                        if (AlarmMail.Equals("1"))
                        {
                            userIDMAIL += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                        }

                        if (AlarmSMS.Equals("1"))
                        {
                            userIDSMS += dtrUser["MobilePhone"].ToString() + ",";
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
                            bll.SendMail(alarmInfo, _usermail, userIDMAIL, "Mail", "COMMUNITYJOINCANCEL", _preLang);
                        }
                        break;

                    case "MESSENGER":
                        if (userIDMESSENGER.Length > 0)
                        {
                            foreach (string rcvUserID in userIDMESSENGER.Split(','))
                            {
                                if (rcvUserID != "")
                                {
                                    bll.SendAlarm(alarmInfo, rcvUserID.ToNumber<int>(), "Messenger", "COMMUNITYJOINCANCEL", _preLang);
                                }
                            }
                        }
                        break;

                    case "SMS":
                        if (userIDSMS.Length > 0)
                        {
                            string[] message = bll.GetAlarmMessage(alarmInfo, "SMS", "COMMUNITYJOINCANCEL", _preLang);
                            string smsContent = message[0];

                            foreach (string mobile in userIDSMS.Split(','))
                            {
                                if (mobile != "")
                                {
                                    Common.BLL.SMS smsbll = new Common.BLL.SMS();
                                    smsbll.SendSMS(alarmInfo.CompanyID, alarmInfo.UserID, _logInID, smsContent, false, mobile);
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
                    foreach (string rcvUserID in userIDMOBILE.Split(','))
                    {
                        if (rcvUserID != "")
                        {
                            bll.SendMobile(alarmInfo, rcvUserID.ToNumber<int>(), "Messenger", "COMMUNITYJOINCANCEL", _preLang);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 승인불가 알람 발송
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarmDeNY(int _companyID, string _userIDList, int _communityID, string _preLang, string _adminName, int _adminUserID, string _loginID, string _usermail)
        {
            BLL.Alarm bll = new Alarm();
            DAL.User dalUser = new DAL.User();

            DataView dvAlarm = AppSettingHelper.GetSysCode(_companyID, "AlarmCommunity");

            string userIDMESSENGER = "";    //메신져
            string userIDMOBILE = "";       //모바일
            string userIDMAIL = "";         //메일
            string userIDSMS = "";          //SMS     

            foreach (string userID in _userIDList.Split(','))
            {
                BE.AlarmInfo alarmInfo = new BE.AlarmInfo();
                alarmInfo.CompanyID = _companyID;
                alarmInfo.UserID = _adminUserID;
                alarmInfo.UserName = _adminName;
                alarmInfo.Description = "";

                DataTableReader getUserDtr = dalUser.GetUserInfo(_companyID, _communityID, userID.ToNumber<int>());
                if (getUserDtr != null)
                {
                    if (getUserDtr.HasRows)
                    {
                        while (getUserDtr.Read())
                        {
                            string AlarmCommunity = getUserDtr["AlarmCommunity"].ToString();
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

                            alarmInfo.Name = getUserDtr["CMNName"].ToString();

                            if (AlarmMessenger.Equals("1"))
                            {
                                userIDMESSENGER += userID + ",";
                            }

                            userIDMOBILE += userID + ",";

                            if (AlarmMail.Equals("1"))
                            {
                                userIDMAIL += getUserDtr["MailID"].ToString() + "@" + getUserDtr["MailDomain"].ToString() + ",";
                            }

                            if (AlarmSMS.Equals("1"))
                            {
                                userIDSMS += getUserDtr["MobilePhone"].ToString() + ",";
                            }

                        }
                    }
                }

                foreach (DataRowView dr in dvAlarm)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MAIL":
                            if (userIDMAIL.Length > 0)
                            {
                                bll.SendMail(alarmInfo, _usermail, userIDMAIL, "Mail", "COMMUNITYDENY", _preLang);
                            }
                            break;

                        case "MESSENGER":
                            if (userIDMESSENGER.Length > 0)
                            {
                                foreach (string rcvUserID in userIDMESSENGER.Split(','))
                                {
                                    if (rcvUserID != "")
                                    {
                                        bll.SendAlarm(alarmInfo, rcvUserID.ToNumber<int>(), "Messenger", "COMMUNITYDENY", _preLang);
                                    }
                                }
                            }
                            break;

                        case "SMS":
                            if (userIDSMS.Length > 0)
                            {
                                string[] message = bll.GetAlarmMessage(alarmInfo, "SMS", "COMMUNITYDENY", _preLang);
                                string smsContent = message[0];

                                foreach (string mobile in userIDSMS.Split(','))
                                {
                                    if (mobile != "")
                                    {
                                        Common.BLL.SMS smsbll = new Common.BLL.SMS();
                                        smsbll.SendSMS(alarmInfo.CompanyID, alarmInfo.UserID, _loginID, smsContent, false, mobile);
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
                        foreach (string rcvUserID in userIDMOBILE.Split(','))
                        {
                            if (rcvUserID != "")
                            {
                                bll.SendMobile(alarmInfo, rcvUserID.ToNumber<int>(), "Messenger", "COMMUNITYDENY", _preLang);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
