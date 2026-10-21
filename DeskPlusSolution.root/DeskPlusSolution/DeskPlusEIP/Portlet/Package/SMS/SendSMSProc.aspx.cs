using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Component.SMS;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Package.SMS
{
    public partial class SendSMSProc : Base.BasePagePortlet
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {

            int companyID = CurrentUserInfo.CompanyID;
            int userID = CurrentUserInfo.UserID;
            string iMsg = Server.UrlDecode(GetParm("iMsg", ""));
            string receiveNumber = GetParm("receiveNumber", "");
            string replyNumber = GetParm("replyNumber", "");
            string yyyymmdd = GetParm("yyyymmdd", "");
            string hms = GetParm("hms", "");
            string calltype_val = GetParm("calltype_val", "0");
            string companyName = GetParm("deptName", "");//회사선택값 (SMS 비용)



            Zeospace.DeskPlus.Component.SMS.SMSInfo smsInfo = new Zeospace.DeskPlus.Component.SMS.SMSInfo();
            smsInfo.CompanyID = companyID;
            smsInfo.UserID = userID;
            smsInfo.WriteUserName = CurrentUserInfo.Name;
            smsInfo.InterfaceID = "";
            smsInfo.Gubun = "";
            smsInfo.Content = Server.UrlDecode(iMsg);

            smsInfo.Status = calltype_val.Trim();
            smsInfo.SendPhone = CurrentUserInfo.MobilePhone;
            smsInfo.Subject = "";
            smsInfo.TargetUrl = "";

            //##### 중요!!! 회사하나로 SMS 비용한다, 계열사 일경우 부서명 가져와야 한다          
            smsInfo.CompanyName = companyName;
            smsInfo.DeptID = CurrentUserInfo.DeptID;
            smsInfo.DeptName = CurrentUserInfo.DeptName;

            if (calltype_val.Trim() == "1") //예약일경우
            {
                smsInfo.SendDate = Convert.ToDateTime(yyyymmdd.Trim());

                if (hms.Trim().Length > 0)
                {
                    smsInfo.SendDate = smsInfo.SendDate.AddHours(Convert.ToInt32(hms.Substring(0, 2)));
                    smsInfo.SendDate = smsInfo.SendDate.AddMinutes(Convert.ToInt32(hms.Substring(2, 2)));
                }

                smsInfo.SendTime = hms.Trim();
            }
            else
            {
                smsInfo.SendDate = System.DateTime.Now;
                smsInfo.SendTime = "";
            }

            smsInfo.StartDate = System.DateTime.MaxValue;
            smsInfo.EndDate = System.DateTime.MaxValue;

            replyNumber = replyNumber.TrimEnd(',');



            if (replyNumber.Trim().Length > 0)
            {
                if (replyNumber.Trim().Length == 10)
                {
                    smsInfo.ReplyPhone1 = replyNumber.Trim().Substring(0, 3);
                    smsInfo.ReplyPhone2 = replyNumber.Trim().Substring(3, 3);
                    smsInfo.ReplyPhone3 = replyNumber.Trim().Substring(6, 4);
                }
                else if (replyNumber.Trim().Length == 11)
                {
                    smsInfo.ReplyPhone1 = replyNumber.Trim().Substring(0, 3);
                    smsInfo.ReplyPhone2 = replyNumber.Trim().Substring(3, 4);
                    smsInfo.ReplyPhone3 = replyNumber.Trim().Substring(7, 4);
                }
                else
                {
                    smsInfo.ReplyPhone1 = replyNumber.Trim().Substring(0, 3);
                    smsInfo.ReplyPhone2 = replyNumber.Trim().Substring(3, 3);
                    smsInfo.ReplyPhone3 = replyNumber.Trim().Substring(6, replyNumber.Trim().Length - 6);
                }
            }

            smsInfo.UseSendAllUsers = false;

            //최근번호 입력하기
            Component.SMS.SuremRecentPhoneInfo recentInfo = new SuremRecentPhoneInfo();
            Component.SMS.RecentPhoneDAL recentDll = new RecentPhoneDAL();
            recentInfo.CompanyID = companyID;
            recentInfo.UserID = userID;
            recentInfo.Name = CurrentUserInfo.Name;


            receiveNumber = receiveNumber.TrimEnd(';');

            if (receiveNumber.IndexOf(',') != -1)
            {
                string[] receiveNumberStr = receiveNumber.Split(',');
                string tmpReceiveNumber = string.Empty;
                for (int i = 0; i < receiveNumberStr.Length; i++)
                {
                    tmpReceiveNumber = receiveNumberStr[i].ToString();

                    if (tmpReceiveNumber.IndexOf(':') != -1)
                    {
                        string[] tmpReceiveNumberStr = tmpReceiveNumber.Split(':');
                        smsInfo.ReceivePhone = tmpReceiveNumberStr[1].ToString().TrimEnd(',');
                    }
                    else
                    {
                        smsInfo.ReceivePhone = tmpReceiveNumber.TrimEnd(',');
                    }

                    if (smsInfo.ReceivePhone.Length > 0)
                    {
                        Add(smsInfo, recentInfo, smsInfo.ReceivePhone);
                    }
                }
            }
            else
            {
                Add(smsInfo, recentInfo, receiveNumber.TrimEnd(','));
            }

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<SCRIPT>self.location.href='SendSMS.aspx'</SCRIPT>");
        }


        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_smsInfo">The _SMS info.</param>
        /// <param name="_receiveNumber">The _receive number.</param>
        private void Add(Zeospace.DeskPlus.Component.SMS.SMSInfo _smsInfo, Component.SMS.SuremRecentPhoneInfo _recentInfo, string _receiveNumber)
        {
            if (_receiveNumber.Trim().Length > 0)
            {
                _receiveNumber = _receiveNumber.Replace("~", "");
                _receiveNumber = _receiveNumber.Replace(".", "");
                _receiveNumber = _receiveNumber.Replace(",", "");
                _receiveNumber = _receiveNumber.Replace(")", "");
                _receiveNumber = _receiveNumber.Replace("(", "");
                _receiveNumber = _receiveNumber.Replace(" ", "");

                _smsInfo.ReceivePhone = _receiveNumber;

                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_receiveNumber " + _receiveNumber, AppSettingType.Site);

                if (_receiveNumber.Trim().Length == 10)
                {
                    _smsInfo.ReceivePhone1 = _receiveNumber.Trim().Substring(0, 3);
                    _smsInfo.ReceivePhone2 = _receiveNumber.Trim().Substring(3, 3);
                    _smsInfo.ReceivePhone3 = _receiveNumber.Trim().Substring(6, 4);
                }
                else if (_receiveNumber.Trim().Length == 11)
                {
                    _smsInfo.ReceivePhone1 = _receiveNumber.Trim().Substring(0, 3);
                    _smsInfo.ReceivePhone2 = _receiveNumber.Trim().Substring(3, 4);
                    _smsInfo.ReceivePhone3 = _receiveNumber.Trim().Substring(7, 4);
                }
                else
                {
                    _smsInfo.ReceivePhone1 = _receiveNumber.Trim().Substring(0, 3);
                    _smsInfo.ReceivePhone2 = _receiveNumber.Trim().Substring(3, 3);
                    _smsInfo.ReceivePhone3 = _receiveNumber.Trim().Substring(6, _receiveNumber.Trim().Length - 6);
                }

                _recentInfo.PhoneNumber1 = _smsInfo.ReceivePhone1;
                _recentInfo.PhoneNumber2 = _smsInfo.ReceivePhone2;
                _recentInfo.PhoneNumber3 = _smsInfo.ReceivePhone3;


            }

            Component.SMS.ControlSendSMS sendSMS = new ControlSendSMS();
            if (sendSMS.Transfer(_smsInfo))
            {
                //최근전화번호 저장
                Component.SMS.RecentPhoneDAL bll = new RecentPhoneDAL();
                bll.Add(_recentInfo);
            }
        }
    }
}
