using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Component.SMS;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.SMSNew
{
    /// <summary>
    /// SendSMS의 요약 설명입니다.
    /// </summary>
    /// <remarks></remarks>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class SendSMS : Base.BaseWSPortlet
    {
        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 생성자 생성
        /// </summary>
        /// <remarks></remarks>
        public SendSMS()
        {
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 :sms 내용 저장하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SaveContent(string p)
        {
            try
            {
                SetDecryptString(p);

                int id = GetDecryptParam("id", "0").ToNumber<int>();
                string gubun = GetDecryptParam("gubun", "");
                string content = GetDecryptParam("content", "");

                content = Server.UrlDecode(content);

                Component.SMS.SMSHelper dal = new SMSHelper();

                if (gubun.ToLower() == "insert")
                {
                    if (content.Length > 0)
                    {
                        Component.SMS.SMSInfo smsInfo = new Component.SMS.SMSInfo();
                        smsInfo.CompanyID = CurrentUserInfo.CompanyID;
                        smsInfo.Content = content;
                        smsInfo.SMSNoticeTemplateID = 0;
                        smsInfo.WriteUserID = CurrentUserInfo.UserID;
                        smsInfo.WriteUserName = CurrentUserInfo.Name;

                        dal.AddSMSTemplateNotice(smsInfo);
                    }
                }
                else if (gubun.ToLower() == "delete")
                {
                    if (id > 0)
                    {
                        Component.SMS.SMSInfo smsInfo = new Component.SMS.SMSInfo();
                        smsInfo.CompanyID = CurrentUserInfo.CompanyID;
                        smsInfo.Content = content;
                        smsInfo.SMSNoticeTemplateID = id;
                        smsInfo.WriteUserID = CurrentUserInfo.UserID;
                        smsInfo.WriteUserName = CurrentUserInfo.Name;

                        dal.DelSMSTemplateNotice(smsInfo);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 :최신전화번호 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelRecentContent(string p)
        {
            try
            {
                SetDecryptString(p);

                string id = Server.UrlDecode(GetDecryptParam("id", "0"));

                Component.SMS.SMSHelper dal = new SMSHelper();

                if (id.Length > 0)
                {
                    id = id.TrimEnd('#');

                    Component.SMS.RecentPhoneDAL bll = new RecentPhoneDAL();
                    Component.SMS.SuremRecentPhoneInfo be = new SuremRecentPhoneInfo();

                    be.CompanyID = CurrentUserInfo.CompanyID;
                    be.UserID = CurrentUserInfo.UserID;

                    if (id.IndexOf('#') != -1)
                    {
                        string[] idStr = id.Split('#');
                        for (int i = 0; i < idStr.Length; i++)
                        {
                            if (idStr[i].ToString() != "")
                            {
                                be.SuremRecentPhoneID = Convert.ToInt32(idStr[i]);
                                bll.Del(be);
                            }
                        }
                    }
                    else
                    {
                        be.SuremRecentPhoneID = Convert.ToInt32(id);
                        bll.Del(be);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 날짜를 다국어에 맞게 년월일 콤마로 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCulDate(string p)
        {
            try
            {
                SetDecryptString(p);

                string yyyy = "";
                string mm = "";
                string dd = "";

                string dateVal = Server.UrlDecode(GetDecryptParam("dateVal", ""));
                if (dateVal.Length > 0)
                {
                    string date = DateHelpers.GetCultureDate(dateVal, "yyyyMMdd", PreLangReal);
                    yyyy = date.Substring(0, 4);
                    mm = date.Substring(4, 2);
                    dd = date.Substring(6, 2);
                }

                return yyyy + "," + mm + "," + dd;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return ",,";
            }
        }

        /// <summary>
        /// SMS 전송하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddSMS(string p)
        {
            try
            {
                SetDecryptString(p);

                string iMsg = Server.UrlDecode(GetDecryptParam("msg", ""));
                string status = GetDecryptParam("status", "0"); //상태(즉시:0,예약:1)
                string yyyymmdd = GetDecryptParam("yyyymmdd", ""); //년월일
                string hh = GetDecryptParam("hh", ""); //시
                string mm = GetDecryptParam("mm", ""); //분

                string replyNumber = GetDecryptParam("replyNumber", ""); //회신번호
                string receiveNumber = GetDecryptParam("receiveNumber", ""); //발송번호 (전화번호:이름;전화번호:이름;)

                List<string> getSMSContent = UtilHelper.GetSMSContent(iMsg, 79);

                foreach (string smsContent in getSMSContent)
                {
                    #region SMS 발송하기

                    Zeospace.DeskPlus.Component.SMS.SMSInfo smsInfo = new Zeospace.DeskPlus.Component.SMS.SMSInfo();
                    smsInfo.CompanyID = CurrentUserInfo.CompanyID;
                    smsInfo.LoginID = CurrentUserInfo.LogInID;
                    smsInfo.UserID = CurrentUserInfo.UserID;
                    smsInfo.WriteUserName = CurrentUserInfo.Name;
                    smsInfo.InterfaceID = "";
                    smsInfo.Gubun = "";
                    smsInfo.Content = smsContent;

                    smsInfo.Status = status;
                    smsInfo.SendPhone = CurrentUserInfo.MobilePhone;
                    smsInfo.Subject = "";
                    smsInfo.TargetUrl = "";

                    //##### 중요!!! 회사하나로 SMS 비용한다, 계열사 일경우 부서명 가져와야 한다
                    smsInfo.CompanyName = CurrentUserInfo.CompanyID.ToString();
                    smsInfo.DeptID = CurrentUserInfo.DeptID;
                    smsInfo.DeptName = CurrentUserInfo.DeptName;

                    if (smsInfo.Status == "1" && yyyymmdd.Length > 0) //예약일경우
                    {
                        smsInfo.SendDate = Convert.ToDateTime(yyyymmdd.Trim());

                        if (hh.Trim().Length > 0 && mm.Trim().Length > 0)
                        {
                            smsInfo.SendDate = smsInfo.SendDate.AddHours(Convert.ToInt32(hh));
                            smsInfo.SendDate = smsInfo.SendDate.AddMinutes(Convert.ToInt32(mm));

                            if (hh.Length == 1) hh = "0" + hh;
                            if (mm.Length == 1) mm = "0" + mm;

                            smsInfo.SendTime = hh + mm + "00";
                        }
                    }
                    else
                    {
                        smsInfo.SendDate = System.DateTime.Now.AddMinutes(1);//즉시발송(약간 딜레이가 있어서 1분 추가하였음)
                        smsInfo.SendTime = "000000";
                    }

                    smsInfo.StartDate = System.DateTime.MaxValue;//미사용
                    smsInfo.EndDate = System.DateTime.MaxValue;//미사용

                    replyNumber = replyNumber.TrimEnd(',');
                    replyNumber = replyNumber.Replace(" ", "");
                    replyNumber = System.Text.RegularExpressions.Regex.Replace(replyNumber.Trim(), @"[-~.,)(]{1}", "");

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

                    #region 최근번호 입력하기

                    Component.SMS.SuremRecentPhoneInfo recentInfo = new SuremRecentPhoneInfo();
                    recentInfo.CompanyID = CurrentUserInfo.CompanyID;
                    recentInfo.UserID = CurrentUserInfo.UserID;
                    recentInfo.Name = CurrentUserInfo.Name;

                    #endregion 최근번호 입력하기

                    #region 수신자 전화번호

                    receiveNumber = receiveNumber.TrimEnd(';');
                    receiveNumber = System.Text.RegularExpressions.Regex.Replace(receiveNumber.Trim(), @"[-~.)(]{1}", "");
                    receiveNumber.Replace(" ", "");

                    foreach (string rcvNum in receiveNumber.Split(','))
                    {
                        if (rcvNum.IndexOf(':') != -1)
                        {
                            //전화번호:이름
                            smsInfo.ReceivePhone = rcvNum.Split(':')[0];//수신자 전화번호
                            smsInfo.ReceiverName = rcvNum.Split(':')[1];//수신자 이름
                        }
                        else
                        {
                            smsInfo.ReceivePhone = rcvNum;//수신자 전화번호
                            smsInfo.ReceiverName = ""; //수신자 이름
                        }

                        //수신자 전화번호가 존재하면 저장한다.
                        if (smsInfo.ReceivePhone.Length > 0)
                            Add(smsInfo, recentInfo);
                    }

                    #endregion 수신자 전화번호

                    #endregion SMS 발송하기
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_smsInfo">The _SMS info.</param>
        /// <param name="_recentInfo">최신전화번호</param>
        /// <param name="_receiveNumber">수신자전화번호</param>
        /// <remarks></remarks>
        private void Add(Zeospace.DeskPlus.Component.SMS.SMSInfo _smsInfo, Component.SMS.SuremRecentPhoneInfo _recentInfo)
        {
            #region 수신자 전화번호 (3단 분리작업)

            _smsInfo.ReceivePhone = _smsInfo.ReceivePhone.Trim();

            if (_smsInfo.ReceivePhone.Length > 0)
            {
                _smsInfo.ReceivePhone = System.Text.RegularExpressions.Regex.Replace(_smsInfo.ReceivePhone, @"[-~.,)(]{1}", "");
                _smsInfo.ReceivePhone = _smsInfo.ReceivePhone.Replace(" ", "");

                //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("_receiveNumber " + _receiveNumber, AppSettingType.Site);

                switch (_smsInfo.ReceivePhone.Trim().Length)
                {
                    case 10:
                        _smsInfo.ReceivePhone1 = _smsInfo.ReceivePhone.Substring(0, 3);
                        _smsInfo.ReceivePhone2 = _smsInfo.ReceivePhone.Substring(3, 3);
                        _smsInfo.ReceivePhone3 = _smsInfo.ReceivePhone.Substring(6, 4);
                        break;

                    case 11:
                        _smsInfo.ReceivePhone1 = _smsInfo.ReceivePhone.Substring(0, 3);
                        _smsInfo.ReceivePhone2 = _smsInfo.ReceivePhone.Substring(3, 4);
                        _smsInfo.ReceivePhone3 = _smsInfo.ReceivePhone.Substring(7, 4);
                        break;

                    default:
                        _smsInfo.ReceivePhone1 = _smsInfo.ReceivePhone.Substring(0, 3);
                        _smsInfo.ReceivePhone2 = _smsInfo.ReceivePhone.Substring(3, 3);
                        _smsInfo.ReceivePhone3 = _smsInfo.ReceivePhone.Substring(6, _smsInfo.ReceivePhone.Length - 6);
                        break;
                }

                _recentInfo.PhoneNumber1 = _smsInfo.ReceivePhone1;
                _recentInfo.PhoneNumber2 = _smsInfo.ReceivePhone2;
                _recentInfo.PhoneNumber3 = _smsInfo.ReceivePhone3;
            }

            #endregion 수신자 전화번호 (3단 분리작업)

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