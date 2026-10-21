using System;
using System.Collections;
using System.Configuration;
using System.Data;
using Zeospace.DeskPlus.Component.SMS;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using Zeospace.DeskPlus.Account.BE;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.Component.SMS
{
    /// <summary>
    /// SMS 솔류션 분기 클래스
    /// </summary>
    public class ControlSendSMS
    {
        private const string nameSpace = "Zeospace.DeskPlus.Component.SMS";

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : SMS 솔류션 분기 클래스
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-14
        /// </summary>
        /// <param name="_smsInfo">The _SMS info.</param>
        /// <returns></returns>
        public bool Transfer(Zeospace.DeskPlus.Component.SMS.SMSInfo _smsInfo)
        {
            string linkedCompany = AppSettingHelper.GetAppSetting(AppSettingType.SMS, "SMSCompany", "SMSNotice");
            string nameSpaceClass = nameSpace + "." + linkedCompany;

            Zeospace.DeskPlus.Component.SMS.ISMS iSMS = null;

            //업체명
            switch (linkedCompany)
            {
                case "SMSNotice":
                    iSMS = (Zeospace.DeskPlus.Component.SMS.SMSNotice)System.Reflection.Assembly.Load(nameSpace).CreateInstance(nameSpaceClass);
                    break;
                case "Surem":
                    iSMS = (Zeospace.DeskPlus.Component.SMS.Surem)System.Reflection.Assembly.Load(nameSpace).CreateInstance(nameSpaceClass);
                    break;                
                case "SuremDll":
                    iSMS = (Zeospace.DeskPlus.Component.SMS.SuremDll)System.Reflection.Assembly.Load(nameSpace).CreateInstance(nameSpaceClass);
                    break;
                case "SUREMSG":
                    iSMS = (Zeospace.DeskPlus.Component.SMS.SUREMSG)System.Reflection.Assembly.Load(nameSpace).CreateInstance(nameSpaceClass);
                    break;
            }

            bool rtn = false;

            if (iSMS != null)
            {
                if (iSMS.Transfer(_smsInfo))
                {
                    rtn = true;
                }
            }

            return rtn;
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 다수의 사용자 SMS 발송하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-01-31
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_subject">The _subject.</param>
        /// <param name="_userStr">The _user STR.</param>
        /// <param name="_useSendAllUsers">_useSendAllUsers : 전체사용자발송시 : 1</param>
        /// <returns></returns>
        public bool SendSMS(int _companyID, int _userID, string _subject, string[] _userStr, bool _useSendAllUsers)
        {
            #region SMS 발송           

            if(_subject.Length > 0)
            {                
                Zeospace.DeskPlus.Account.DAL.User dalUser = new Account.DAL.User();
                
                if(_useSendAllUsers)
                {
                    SendSMS(_companyID, _userID, _subject, "", _useSendAllUsers);
                }
                else
                {
                    for (int i=0;i<_userStr.Length;i++)                
                    {
                        if(_userStr[i] != "")
                        {
                            List<UserInfo> alList = dalUser.GetUserInfo(_companyID, _userStr[i].ToNumber<int>(), true, true);
                            SendSMS(_companyID, _userID, _subject, alList[0].MobilePhone, _useSendAllUsers);
                        }
                    }
                }
            }

            return true;

            #endregion
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 개별 사용자 SMS 발송하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-01-31
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_subject">The _subject.</param>
        /// <param name="_receiverMobile">The _receiver mobile.</param>
        /// <param name="_useSendAllUsers">_useSendAllUsers : 전체사용자발송시 : 1</param>
        /// <returns></returns>
        private bool SendSMS(int _companyID, int _userID, string _subject, string _receiverMobile, bool _useSendAllUsers)
        {
            try
            {
                SMSInfo smsInfo = new SMSInfo();
                smsInfo.CompanyID       = _companyID;
                smsInfo.UserID          = _userID;
                smsInfo.InterfaceID     = "";
                smsInfo.Gubun           = "";
                smsInfo.Content         = _subject;
                smsInfo.ReceivePhone    = "";
                smsInfo.Status          = "0"; //즉시
                smsInfo.UseSendAllUsers = _useSendAllUsers;

                string receiveNumber = _receiverMobile;
                if(receiveNumber.Length > 0)
                {
                    receiveNumber = receiveNumber.Replace("~", "");
                    receiveNumber = receiveNumber.Replace(".", "");
                    receiveNumber = receiveNumber.Replace(",", "");
                    receiveNumber = receiveNumber.Replace(")", "");
                    receiveNumber = receiveNumber.Replace("(", "");
                    receiveNumber = receiveNumber.Replace(" ", "");
                }

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("receiveNumber : " + receiveNumber, AppSettingType.SMS);
                                
                //모든 사용자에게 발송
                if(_useSendAllUsers)
                {
                    smsInfo.ReceivePhone1 = "";
                    smsInfo.ReceivePhone2 = "";
                    smsInfo.ReceivePhone3 = "";

                    Zeospace.DeskPlus.Component.SMS.ControlSendSMS sendSMS = new Zeospace.DeskPlus.Component.SMS.ControlSendSMS();
                    sendSMS.Transfer(smsInfo);
                }
                else if (receiveNumber.Trim().Length > 0)
                {
                    receiveNumber = receiveNumber.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");

                    if (receiveNumber.Trim().Length == 10)
                    {
                        smsInfo.ReceivePhone1 = receiveNumber.Trim().Substring(0, 3);
                        smsInfo.ReceivePhone2 = receiveNumber.Trim().Substring(3, 3);
                        smsInfo.ReceivePhone3 = receiveNumber.Trim().Substring(6, 4);
                    }
                    else if (receiveNumber.Trim().Length == 11)
                    {
                        smsInfo.ReceivePhone1 = receiveNumber.Trim().Substring(0, 3);
                        smsInfo.ReceivePhone2 = receiveNumber.Trim().Substring(3, 4);
                        smsInfo.ReceivePhone3 = receiveNumber.Trim().Substring(7, 4);
                    }
                    else
                    {
                        smsInfo.ReceivePhone1 = receiveNumber.Trim().Substring(0, 3);
                        smsInfo.ReceivePhone2 = receiveNumber.Trim().Substring(3, 3);
                        smsInfo.ReceivePhone3 = receiveNumber.Trim().Substring(6, receiveNumber.Trim().Length - 6);
                    }

                    //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("smsInfo.ReceivePhone1 : " + smsInfo.ReceivePhone1, AppSettingType.SMS);
                    //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("smsInfo.ReceivePhone2 : " + smsInfo.ReceivePhone2, AppSettingType.SMS);
                    //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("smsInfo.ReceivePhone3 : " + smsInfo.ReceivePhone3, AppSettingType.SMS);                
                    
                    
                    Transfer(smsInfo);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendSMS(int _companyID, int _userID, int _title, string _receiverMobile) : " + ex);                
                return false;
            }
        }
    }
}
