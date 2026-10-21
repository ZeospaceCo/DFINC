using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Community.BE;
using System.Transactions;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;
using System.Web;
using System.Collections;

namespace Zeospace.DeskPlus.Community.BLL
{    
    public class Com : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 커뮤니티 등록
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ComInfo _info)
        {
            try
            {
                int communityID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Com 테이블 추가
                    DAL.Com dal = new DAL.Com();
                    communityID = dal.Add(_info);
                    if (communityID < 1)
                    {
                        return -1;
                    }

                    // 2. ComDetail 테이블 추가 
                    BLL.ComDetail bllComDetail = new ComDetail();
                    if (!bllComDetail.Add(_info.CompanyID, communityID))
                    {
                        return -1;
                    }

                    // 3. UserGroup 테이블 추가 (커뮤니티지기, 운영자..세팅)
                    BLL.UserGroup bllUserGroup = new UserGroup();
                    int iUserGroupAdmin = bllUserGroup.Add(_info.CompanyID, communityID);

                    if (iUserGroupAdmin < 1)
                    {
                        return -1;
                    }

                    // 4. UserPointConfig 테이블 추가 
                    BLL.PointConfig bllPointConfig = new PointConfig();
                    if (!bllPointConfig.Add(_info, communityID))
                    {
                        return -1;
                    }

                    // 5. UserInfo 테이블 추가
                    BE.UserInfo userInfo = new UserInfo();
                    userInfo.CompanyID = _info.CompanyID;
                    userInfo.CommunityID = communityID;
                    userInfo.UserID = _info.WriteUserID;
                    userInfo.Description = "";
                    userInfo.Grade = 2;
                    userInfo.Status = 1;

                    DAL.User dalUser = new DAL.User();
                    if (!dalUser.Add(userInfo))
                    {
                        return -1;
                    }

                    // 6. UserGroupMem 테이블 추가
                    BE.UserGroupMemInfo userGroupMemInfo = new BE.UserGroupMemInfo();
                    userGroupMemInfo.CompanyID = _info.CompanyID;
                    userGroupMemInfo.CommunityID = communityID;
                    userGroupMemInfo.GroupID = iUserGroupAdmin;
                    userGroupMemInfo.UserID = _info.WriteUserID;

                    DAL.UserGroupMem dalUserGroupMem = new DAL.UserGroupMem();
                    if (!dalUserGroupMem.Add(userGroupMemInfo))
                    {
                        return -1;
                    }

                    // 7. Menu , MenuAuth 테이블 추가
                    BLL.Menu bllMenu = new Menu();
                    int iMenuID = bllMenu.Add(_info.CompanyID, communityID);

                    if (iMenuID < 1)
                    {
                        return -1;
                    }

                    // 8. MenuDisplay 테이블 추가 
                    BE.MenuDisplayInfo menuDisplayInfo = new BE.MenuDisplayInfo();
                    menuDisplayInfo.CompanyID = _info.CompanyID;
                    menuDisplayInfo.CommunityID = communityID;
                    menuDisplayInfo.MenuID = iMenuID;

                    DAL.MenuDisplay dalMenuDisplay = new DAL.MenuDisplay();
                    if (!dalMenuDisplay.Add(menuDisplayInfo))
                    {
                        return -1;
                    }

                    // 9. 게시판 폴더 추가
                    HttpContext context = HttpContext.Current;
                    string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    filePath = filePath + "{0}/Community/{1}";
                    string folderVirtualPath = string.Format(filePath, _info.CompanyID, communityID);

                    string folderPath = context.Server.MapPath(folderVirtualPath);
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    folderPath = folderVirtualPath + "/Board/";
                    string folderBoard = context.Server.MapPath(folderPath);
                    if (!Directory.Exists(folderBoard)) Directory.CreateDirectory(folderBoard);                   

                    ts.Complete();
                }                
                return communityID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add()", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 커뮤니티 등록
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_name"></param>
        /// <param name="_userID"></param>
        /// <param name="_applyID"></param>
        /// <param name="_preLang"></param>
        /// <param name="_logInID"></param>
        /// <param name="_usermail"></param>
        /// <returns></returns>
        public int Add(ComInfo _info, string _name, int _userID, int _applyID, string _preLang, string _logInID, string _usermail)
        {
            try
            {
                int communityID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Com 테이블 추가
                    DAL.Com dal = new DAL.Com();    
                    communityID = dal.Add(_info);
                    if (communityID < 1)
                    {
                        return -1;
                    }

                    // 2. ComDetail 테이블 추가 
                    BLL.ComDetail bllComDetail = new ComDetail();
                    if (!bllComDetail.Add(_info.CompanyID, communityID))
                    {
                        return -1;
                    }                    

                    // 3. UserGroup 테이블 추가 (커뮤니티지기, 운영자..세팅)
                    BLL.UserGroup bllUserGroup = new UserGroup();
                    int iUserGroupAdmin = bllUserGroup.Add(_info.CompanyID, communityID);

                    if (iUserGroupAdmin < 1)
                    {
                        return -1;
                    }
                        
                    // 4. UserPointConfig 테이블 추가 
                    BLL.PointConfig bllPointConfig = new PointConfig();
                    if (!bllPointConfig.Add(_info, communityID))
                    {
                        return -1;
                    }

                    // 5. UserInfo 테이블 추가
                    BE.UserInfo userInfo = new UserInfo();
                    userInfo.CompanyID = _info.CompanyID;
                    userInfo.CommunityID = communityID;
                    userInfo.UserID = _info.WriteUserID;
                    userInfo.Description = "";
                    userInfo.Grade = 2;
                    userInfo.Status = 1;                    

                    DAL.User dalUser = new DAL.User();
                    if (!dalUser.Add(userInfo))
                    {
                        return -1;
                    }

                    // 6. UserGroupMem 테이블 추가
                    BE.UserGroupMemInfo userGroupMemInfo = new BE.UserGroupMemInfo();
                    userGroupMemInfo.CompanyID = _info.CompanyID;
                    userGroupMemInfo.CommunityID = communityID;
                    userGroupMemInfo.GroupID = iUserGroupAdmin;
                    userGroupMemInfo.UserID = _info.WriteUserID;

                    DAL.UserGroupMem dalUserGroupMem = new DAL.UserGroupMem();
                    if (!dalUserGroupMem.Add(userGroupMemInfo))
                    {
                        return -1;
                    }

                    // 7. Menu , MenuAuth 테이블 추가
                    BLL.Menu bllMenu = new Menu();
                    int iMenuID = bllMenu.Add(_info.CompanyID, communityID);

                    if (iMenuID < 1)
                    {
                        return -1;
                    }

                    // 8. MenuDisplay 테이블 추가 
                    BE.MenuDisplayInfo menuDisplayInfo = new BE.MenuDisplayInfo();
                    menuDisplayInfo.CompanyID = _info.CompanyID;
                    menuDisplayInfo.CommunityID = communityID;
                    menuDisplayInfo.MenuID = iMenuID;

                    DAL.MenuDisplay dalMenuDisplay = new DAL.MenuDisplay();
                    if (!dalMenuDisplay.Add(menuDisplayInfo))
                    {
                        return -1;
                    }        

                    // 9. 게시판 폴더 추가
                    HttpContext context = HttpContext.Current;
                    string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    filePath = filePath + "{0}/Community/{1}";
                    string folderVirtualPath = string.Format(filePath, _info.CompanyID, communityID);

                    string folderPath = context.Server.MapPath(folderVirtualPath);
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    folderPath = folderVirtualPath + "/Board/";
                    string folderBoard = context.Server.MapPath(folderPath);
                    if (!Directory.Exists(folderBoard)) Directory.CreateDirectory(folderBoard);
                                                            
                    ts.Complete();                    
                }

                SendAlarm(_info, _name, _userID, _applyID, _preLang, _logInID, _usermail);
                
                return communityID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add(user)", ex);
                return -1;
            }
        }
        #endregion 

        #region  알람 발송
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 알람 발송
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_preLang"></param>
        /// <returns></returns>
        public void SendAlarm(ComInfo _info, string _name, int _userID, int _applyID, string _preLang, string _logInID, string _usermail)
        {
            DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "AlarmCommunity");

            string userIDMESSENGER = "";    //메신져
            string userIDMOBILE = "";       //모바일
            string userIDMAIL = "";         //메일
            string userIDSMS = "";          //SMS

            ArrayList Receiver = new ArrayList();

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

                        Receiver.Add(dtrRole["UserID"].ToString());

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

                DAL.ComApply dalComApply = new DAL.ComApply();
                DataTableReader dtr = dalComApply.GetInfo(_info.CompanyID, _applyID);

                BE.AlarmInfo alarmInfo = new BE.AlarmInfo();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            string AlarmCommunity = dtr["AlarmCommunity"].ToString();
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
                                    userIDMESSENGER += dtr["WriteUserID"].ToString().ToNumber<int>() + ",";
                                }

                                userIDMOBILE += dtr["WriteUserID"].ToString().ToNumber<int>() + ",";                                                                             

                                if (AlarmMail.Equals("1"))
                                {
                                    userIDMAIL += dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString() + ",";
                                }

                                if (AlarmSMS.Equals("1"))
                                {
                                    userIDSMS += dtr["MobilePhone"].ToString() + ",";
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
                                bll.SendMail(alarmInfo, _usermail, userIDMAIL, "Mail", "COMMUNITYCONFIRM", _preLang);
                            }
                            break;

                        case "MESSENGER":
                            if (userIDMESSENGER.Length > 0)
                            {
                                foreach (string userID in userIDMESSENGER.Split(','))
                                {
                                    if (userID != "")
                                    {
                                        bll.SendAlarm(alarmInfo, userID.ToNumber<int>(), "Messenger", "COMMUNITYCONFIRM", _preLang);
                                    }
                                }
                            }
                            break;

                        case "SMS":
                            if (userIDSMS.Length > 0)
                            {
                                string[] message = bll.GetAlarmMessage(alarmInfo, "SMS", "COMMUNITYCONFIRM", _preLang);
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
                                bll.SendMobile(alarmInfo, userID.ToNumber<int>(), "Messenger", "COMMUNITYCONFIRM", _preLang);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
