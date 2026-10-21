using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Common.BLL
{
    public class SMS
    {
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 다수의 사용자 SMS 발송하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-01-31
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_subject">The _subject.</param>
        /// <param name="_useSendAllUsers">_useSendAllUsers : 전체사용자발송시 : 1</param>
        /// <param name="_mobilePhone">The _mobile phone.</param>
        /// <returns></returns>
        public bool SendSMS(int _companyID, int _userID, string _loginID, string _subject,
            bool _useSendAllUsers, string _mobilePhone)
        {
            #region SMS 발송

            if (_subject.Length > 0)
            {
                if (_useSendAllUsers)
                {
                    SendSMS(_companyID, _userID, _loginID, _subject, "", _useSendAllUsers);
                }
                else
                {
                    SendSMS(_companyID, _userID, _loginID, _subject, _mobilePhone, _useSendAllUsers);
                }
            }

            return true;

            #endregion SMS 발송
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 개별 사용자 SMS 발송하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-01-31
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_subject">The _subject.</param>
        /// <param name="_receiverMobile">The _receiver mobile.</param>
        /// <param name="_useSendAllUsers">_useSendAllUsers : 전체사용자발송시 : 1</param>
        /// <returns></returns>
        private bool SendSMS(int _companyID, int _userID, string _loginID, string _subject,
            string _receiverMobile, bool _useSendAllUsers)
        {
            try
            {
                Zeospace.DeskPlus.Component.SMS.SMSInfo smsInfo = new Component.SMS.SMSInfo();
                smsInfo.CompanyID = _companyID;
                smsInfo.UserID = _userID;
                smsInfo.LoginID = _loginID;
                smsInfo.InterfaceID = "";
                smsInfo.Gubun = "";
                smsInfo.Content = _subject;
                smsInfo.ReceivePhone = "";
                smsInfo.Status = "0"; //즉시
                smsInfo.UseSendAllUsers = _useSendAllUsers;//모든 사용자에게 발송여부
                smsInfo.ReceivePhone = _receiverMobile.ToNull().Trim(); //수신자 전화번호

                Zeospace.DeskPlus.Account.DAL.User userDal = new Account.DAL.User();
                var userInfo = userDal.GetUserInfo(_companyID, _userID, true, true);

                foreach (var uinfo in userInfo)
                {
                    string sendUserMP = GetMobilePhone(uinfo.MobilePhone);
                    if (sendUserMP.Length > 0)
                    {
                        //발신자 전화번호
                        smsInfo.ReplyPhone1 = sendUserMP.Split(',')[0];
                        smsInfo.ReplyPhone2 = sendUserMP.Split(',')[1];
                        smsInfo.ReplyPhone3 = sendUserMP.Split(',')[2];
                    }

                    smsInfo.WriteUserName = uinfo.NameBase; //발신자 이름
                }

                //수신자 전화번호
                string recvUserMP = GetMobilePhone(smsInfo.ReceivePhone);
                if (recvUserMP.Length > 0)
                {
                    smsInfo.ReceivePhone1 = recvUserMP.Split(',')[0];
                    smsInfo.ReceivePhone2 = recvUserMP.Split(',')[1];
                    smsInfo.ReceivePhone3 = recvUserMP.Split(',')[2];
                }

                if (smsInfo.UseSendAllUsers == true || smsInfo.ReceivePhone1.Length > 0)
                {
                    Component.SMS.ControlSendSMS sendSMS = new Component.SMS.ControlSendSMS();
                    sendSMS.Transfer(smsInfo);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendSMS(int _companyID, int _userID, int _title, string _receiverMobile) : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 송수신 전화번호 3단 만들기
        /// </summary>
        /// <param name="_mobilePhone">The _mobile phone.</param>
        /// <returns></returns>
        private string GetMobilePhone(string _mobilePhone)
        {
            _mobilePhone = _mobilePhone.Trim();
            if (_mobilePhone.Length == 0) return "";

            string mobile1 = "";
            string mobile2 = "";
            string mobile3 = "";

            if (_mobilePhone.Length > 7)
            {
                _mobilePhone = System.Text.RegularExpressions.Regex.Replace(_mobilePhone, @"[-~.,)(]{1}", "");
                _mobilePhone = _mobilePhone.Replace(" ", "");

                switch (_mobilePhone.Length)
                {
                    case 10:
                        mobile1 = _mobilePhone.Substring(0, 3);
                        mobile2 = _mobilePhone.Substring(3, 3);
                        mobile3 = _mobilePhone.Substring(6, 4);
                        break;

                    case 11:
                        mobile1 = _mobilePhone.Substring(0, 3);
                        mobile2 = _mobilePhone.Substring(3, 4);
                        mobile3 = _mobilePhone.Substring(7, 4);
                        break;

                    default:
                        mobile1 = _mobilePhone.Substring(0, 3);
                        mobile2 = _mobilePhone.Substring(3, 3);
                        mobile3 = _mobilePhone.Substring(6, _mobilePhone.Length - 6);
                        break;
                }

                return mobile1 + "," + mobile2 + "," + mobile3;
            }

            return "";
        }
    }
}