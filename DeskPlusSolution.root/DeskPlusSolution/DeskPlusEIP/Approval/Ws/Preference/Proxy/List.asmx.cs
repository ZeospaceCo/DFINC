using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Preference.Proxy
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSApproval
    {

        /// <summary>
        /// 작성자 : 이문호
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }


        /// <summary>
        /// 작성자 : 이문호
        /// 대결자 페이징 총개수
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageListCnt(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            try
            {
                DAL.Proxy list = new DAL.Proxy();
                return list.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetFavoriteListCnt : " + ex);
                return -1;
            }

        }


        /// <summary>
        /// 작성자 : 이문호
        /// 대결자 페이징 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string pageSize = GetDecryptParam("pageSize", "0"); //페이지에 보여지는 목록수
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0"); //페이지 넘버

                HttpContext context = HttpContext.Current;
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.Proxy biz = new DAL.Proxy();

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, setPageingStr[0], setPageingStr[1]);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.ProxyInfo info = new BE.ProxyInfo();

                            //info.ApprovalUserSeq = dtr["ROW"].ToString().ToNumber<int>();
                            info.ProxyID = dtr["ProxyID"].ToString().ToNumber<int>();
                            info.ProxyUserID = dtr["ProxyUserID"].ToString().ToNumber<int>();
                            info.ProxyDeptID = dtr["ProxyDeptID"].ToString().ToNumber<int>();

                            info.ProxyBaseDept = dtr["BaseDept"].ToString().ToNumber<int>();
                            info.ProxyMail = dtr["Mail"].ToString();
                            info.ProxyMobilePhone = dtr["MobilePhone"].ToString();
                            info.ProxyOfficePhone = dtr["OfficePhone"].ToString();
                            info.ProxyState = dtr["IsProxy"].ToString();

                            #region 사진 이미지 설정
                            string ProxyPhotoName = dtr["PhotoImage"].ToString();
                            info.ProxyPhotoName = "";
                            string imgPath = "";
                            string imgTag = "";
                            string encParams = "";
                            Dictionary<string, string> paraDic = null;

                            if (!string.IsNullOrEmpty(ProxyPhotoName) && ProxyPhotoName.Length > 0)
                            {
                                imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + ProxyPhotoName;
                                imgPath = string.Format(imgPath, CurrentUserInfo.CompanyID, info.ProxyUserID);

                                if (System.IO.File.Exists(context.Server.MapPath(imgPath)))
                                {
                                    #region 스크린샷
                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("filePath", imgPath);
                                    encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                    imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                    #endregion

                                    imgTag = "<img src='{0}' width='52' height='52' />";
                                    imgTag = string.Format(imgTag, imgPath);
                                    info.ProxyPhotoName = imgTag;
                                }
                            }

                            if (info.ProxyPhotoName.Length == 0)
                            {
                                #region 스크린샷
                                paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", baseImageDirectory + "C/Img/Icon/ico_noimg.gif");
                                encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                #endregion

                                info.ProxyPhotoName = "<img src='" + imgPath + "' width='52' height='52' alt='' title='' />";
                            }

                            #endregion

                            #region 직급 다국어 설정
                            info.ProxyTitleName = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이름 다국어 설정
                            info.ProxyUserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.ProxyDeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion


                            info.ProxyDateStr = DateTime.Parse(dtr["ProxyStartDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["ProxyStartDate"].ToString()).ToShortTimeString() + " ~ " +
                                    DateTime.Parse(dtr["ProxyEndDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["ProxyEndDate"].ToString()).ToShortTimeString();

                            info.AllDay = dtr["AllDay"].ToString();

                            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AbsenceType");
                            dv.RowFilter = "ParentCodeID = 'AbsenceType' AND CodeID='" + dtr["ProxyType"].ToString() + "'";                            

                            foreach (DataRowView dr in dv)
                            {
                                info.ProxyType = this.GetLangName(
                                    dr["NameBase"].ToString(),
                                    dr["NameKo"].ToString(),
                                    dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                                    dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),
                                    dr["NameZhTw"].ToString(), PreLang);
                            }

                            info.IsCall = dtr["IsCall"].ToString();
                            info.Gubun = dtr["Gubun"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : 이문호
        /// 대결 시작, 중지
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ProxyAction(string p)
        {
            try
            {
                SetDecryptString(p);
                BE.ProxyInfo be = new ProxyInfo();

                be.CompanyID = CurrentUserInfo.CompanyID;
                be.UserID = CurrentUserInfo.UserID;
                be.DeptID = CurrentUserInfo.DeptID;
                be.ProxyID = GetDecryptParam("proxyID", "0").ToNumber<int>(); //페이지에 보여지는 목록수
                be.ProxyUserID = GetDecryptParam("ProxyUserID", "0").ToNumber<int>();
                be.ProxyDeptID = GetDecryptParam("ProxyDeptID", "0").ToNumber<int>(); //페이지 넘버
                be.IsProxy = GetDecryptParam("isProxy", "N"); //페이지 넘버

                DAL.Proxy biz = new DAL.Proxy();

                bool rtnval = biz.ProxyAction(be);
                if (rtnval)
                {
                    // 알람처리
                    string state = (be.IsProxy.Equals("N")) ? "END" : "START";

                    Account.DAL.User dal = new Account.DAL.User();
                    List<Account.BE.UserInfo> list = dal.GetUserInfo(CurrentUserInfo.CompanyID, be.ProxyUserID, true, true);
                    foreach (Account.BE.UserInfo info in list)
                    {
                        SendAlarm(info, state);
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : 이문호
        /// 대결자 지정상태 (IsProxy - Y) 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetProxyCnt(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            try
            {
                DAL.Proxy list = new DAL.Proxy();
                return list.GetProxyCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetProxyCount : " + ex);
                return -1;
            }

        }

        /// <summary>
        /// 작성자 : 이문호
        /// 대결자 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelProxyUser(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);
            string proxyIDs = GetDecryptParam("proxyID", "0");

            try
            {
                if (proxyIDs.Length > 0)
                {
                    Account.DAL.User userdal = new Account.DAL.User();
                    Zeospace.DeskPlus.Approval.BLL.Proxy pBll = new BLL.Proxy();
                    Zeospace.DeskPlus.Approval.DAL.Proxy dal = new DAL.Proxy();

                    BE.UserInfo userinfo = new BE.UserInfo();
                    userinfo.CompanyID = CurrentUserInfo.CompanyID;
                    userinfo.LogInID = CurrentUserInfo.LogInID;
                    userinfo.UserID = CurrentUserInfo.UserID;
                    userinfo.DeptID = CurrentUserInfo.DeptID;
                    userinfo.Name = CurrentUserInfo.Name;
                    userinfo.DeptName = CurrentUserInfo.DeptName;
                    userinfo.TitleName = CurrentUserInfo.TitleName;
                    userinfo.MailID = CurrentUserInfo.MailID;
                    userinfo.MailDomain = CurrentUserInfo.MailDomain;
                    userinfo.MobilePhone = CurrentUserInfo.MobilePhone;
                    userinfo.PreferredLanguage = CurrentUserInfo.PreferredLanguage;
                    
                    foreach (string proxyID in proxyIDs.Split(','))
                    {
                        string proxyUserID = "";
                        string proxyDeptID = "";
                        string proxyDateStr = "";
                        string proxyContent = "";
                        string proxyTypeStr = "";
                        string isCall = "";
                        DataTableReader dtr = dal.GetProxyInfoByProxyID(CurrentUserInfo.CompanyID, proxyID.ToNumber<int>());
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    proxyUserID = dtr["proxyUserID"].ToString();
                                    proxyDeptID = dtr["proxyDeptID"].ToString();

                                    proxyContent = dtr["ProxyContent"].ToString();

                                    string proxyPreferredLanguage = "";
                                    List<Account.BE.UserInfo> ulist = userdal.GetUserInfo(CurrentUserInfo.CompanyID, proxyUserID.ToNumber<int>(), true, true);
                                    foreach (Account.BE.UserInfo pinfo in ulist)
                                    {
                                        proxyPreferredLanguage = pinfo.PreferredLanguage;
                                    }

                                    if (proxyPreferredLanguage == "")
                                    {
                                        proxyPreferredLanguage = "ko";
                                    }

                                    DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AbsenceType");
                                    dv.RowFilter = "ParentCodeID = 'AbsenceType' AND CodeID='" + dtr["ProxyType"].ToString() + "'";

                                    foreach (DataRowView dr in dv)
                                    {
                                        proxyTypeStr = this.GetLangName(
                                            dr["NameBase"].ToString(),
                                            dr["NameKo"].ToString(),
                                            dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                                            dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),
                                            dr["NameZhTw"].ToString(), proxyPreferredLanguage);
                                    }

                                    proxyDateStr = DateHelper.GetCultureDate(DateTime.Parse(dtr["ProxyStartDate"].ToString()), proxyPreferredLanguage, true) + " ~ " + DateHelper.GetCultureDate(DateTime.Parse(dtr["ProxyEndDate"].ToString()), proxyPreferredLanguage, true);

                                    isCall = dtr["IsCall"].ToString();
                                }
                            }
                        }

                        BE.ProxyInfo info = new BE.ProxyInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.ProxyID = proxyID.ToNumber<int>();
                        info.ProxyUserID = proxyUserID.ToNumber<int>();
                        info.ProxyDeptID = proxyDeptID.ToNumber<int>();
                        info.ProxyContent = proxyContent;
                        info.ProxyTypeStr = proxyTypeStr;
                        info.ProxyDateStr = proxyDateStr;
                        info.IsCall = isCall;

                        pBll.Del(userinfo, info);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelProxyUser : " + ex);
                throw;
            }
        }

        #region 알람발송 (메일/메신저/SMS)
        private void SendAlarm(Account.BE.UserInfo _targetinfo, string _state)
        {
            #region 소스

            try
            {
                if (_targetinfo == null || _state.Equals("")) return;

                string alarmMethod = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMethodProxy", "");
                if (alarmMethod.Equals("")) return;

                string xmlpath = FileHelper.GetServerMapPath("/DeskPlusEIP/Approval/App_Data/Proxy/" + CurrentUserInfo.PreferredLanguage + ".xml");
                XmlDocument contXml = new XmlDocument();
                contXml.Load(xmlpath);
                XmlNode rootNode = contXml.DocumentElement;
                XmlNode subNode = null;

                foreach (string method in alarmMethod.Split(','))
                {
                    subNode = rootNode.SelectSingleNode("/ALARM/" + method);            // MAIL, MESSENGER, SMS
                    string[] rtnval = GetAlarmCont(subNode, _targetinfo, _state);

                    switch (method)
                    {
                        case "MAIL":

                            #region 메일발송
                            if (_targetinfo.AlarmMail.Equals("1"))
                            {
                                Zeospace.DeskPlus.Common.BLL.MailZettaFactory mailBll = new MailZettaFactory();
                                mailBll.CreateMHT(CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain, CurrentUserInfo.Name, _targetinfo.Mail, "", "", rtnval[0], rtnval[1], null, "", false);
                            }
                            #endregion

                            break;

                        case "MESSENGER":

                            #region 메신저발송
                            if (_targetinfo.AlarmMessenger.Equals("1"))
                            {
                                AlarmInfo alarmInfo = new AlarmInfo();
                                alarmInfo.CompanyID = CurrentUserInfo.CompanyID;
                                alarmInfo.SenderID = CurrentUserInfo.UserID;
                                alarmInfo.SenderName = CurrentUserInfo.Name;
                                alarmInfo.SenderMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
                                alarmInfo.SenderPhone = CurrentUserInfo.MobilePhone;
                                alarmInfo.ReceiverID = _targetinfo.UserID;
                                alarmInfo.ReceiverName = _targetinfo.NameBase;
                                alarmInfo.ReceiverMail = _targetinfo.Mail;
                                alarmInfo.ReceiverPhone = _targetinfo.MobilePhone;
                                alarmInfo.Status = "";
                                alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.Subject = rtnval[0];
                                alarmInfo.Content = rtnval[1];
                                alarmInfo.TargetUrl = "";
                                alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                                alarmInfo.WriteDate = System.DateTime.Now;
                                alarmInfo.AlarmType = 1;
                                alarmInfo.KindSubject = "Approval";
                                alarmInfo.ViewOption = "01";
                                alarmInfo.Store = 1;
                                alarmInfo.UserState = 0;

                                Zeospace.DeskPlus.Common.BLL.Alarm alarmBll = new Alarm();
                                alarmBll.Add(alarmInfo, AlarmEnum.Cool.ToString());
                            }
                            #endregion

                            break;

                        case "SMS":

                            #region SMS발송
                            if (_targetinfo.AlarmSMS.Equals("1"))
                            {
                                Zeospace.DeskPlus.Component.SMS.SMSInfo smsInfo = new Zeospace.DeskPlus.Component.SMS.SMSInfo();

                                // 발송자 & 기타
                                smsInfo.CompanyID = CurrentUserInfo.CompanyID;
                                smsInfo.LoginID = CurrentUserInfo.LogInID;
                                smsInfo.UserID = CurrentUserInfo.UserID;
                                smsInfo.WriteUserName = CurrentUserInfo.Name;
                                smsInfo.InterfaceID = "";
                                smsInfo.Gubun = "";
                                smsInfo.Subject = rtnval[0];
                                smsInfo.Content = rtnval[1];
                                smsInfo.Status = "0";               //상태(즉시:0,예약:1)
                                smsInfo.SendPhone = CurrentUserInfo.MobilePhone;
                                smsInfo.TargetUrl = "";

                                //##### 중요!!! 회사하나로 SMS 비용한다, 계열사 일경우 부서명 가져와야 한다
                                smsInfo.CompanyName = CurrentUserInfo.CompanyID.ToString();
                                smsInfo.DeptID = CurrentUserInfo.DeptID;
                                smsInfo.DeptName = CurrentUserInfo.DeptName;

                                smsInfo.SendDate = System.DateTime.Now.AddMinutes(1); //즉시발송(약간 딜레이가 있어서 1분 추가하였음)
                                smsInfo.SendTime = "000000";
                                smsInfo.StartDate = System.DateTime.MaxValue; //미사용
                                smsInfo.EndDate = System.DateTime.MaxValue;   //미사용
                                smsInfo.UseSendAllUsers = false;

                                // 회신번호 처리
                                string mpinfo = CurrentUserInfo.MobilePhone.Trim().Replace(" ", "");
                                mpinfo = System.Text.RegularExpressions.Regex.Replace(mpinfo, @"[-~.,)(]{1}", "");

                                if (mpinfo.Length.Equals(10))
                                {
                                    smsInfo.ReplyPhone1 = mpinfo.Substring(0, 3);
                                    smsInfo.ReplyPhone2 = mpinfo.Substring(3, 3);
                                    smsInfo.ReplyPhone3 = mpinfo.Substring(6, 4);
                                }
                                else if (mpinfo.Length.Equals(11))
                                {
                                    smsInfo.ReplyPhone1 = mpinfo.Substring(0, 3);
                                    smsInfo.ReplyPhone2 = mpinfo.Substring(3, 4);
                                    smsInfo.ReplyPhone3 = mpinfo.Substring(7, 4);
                                }
                                else
                                {
                                    if (mpinfo.Equals(""))
                                    {
                                        smsInfo.ReplyPhone1 = "";
                                        smsInfo.ReplyPhone2 = "";
                                        smsInfo.ReplyPhone3 = "";
                                    }
                                    else
                                    {
                                        smsInfo.ReplyPhone1 = mpinfo.Substring(0, 3);
                                        smsInfo.ReplyPhone2 = mpinfo.Substring(3, 3);
                                        smsInfo.ReplyPhone3 = mpinfo.Substring(6, mpinfo.Length - 6);
                                    }
                                }

                                // 수신자
                                _targetinfo.MobilePhone = _targetinfo.MobilePhone.Trim().Replace(" ", "");
                                _targetinfo.MobilePhone = System.Text.RegularExpressions.Regex.Replace(_targetinfo.MobilePhone, @"[-~.,)(]{1}", "");

                                smsInfo.ReceivePhone = _targetinfo.MobilePhone;

                                switch (_targetinfo.MobilePhone.Trim().Length)
                                {
                                    case 10:
                                        smsInfo.ReceivePhone1 = smsInfo.ReceivePhone.Substring(0, 3);
                                        smsInfo.ReceivePhone2 = smsInfo.ReceivePhone.Substring(3, 3);
                                        smsInfo.ReceivePhone3 = smsInfo.ReceivePhone.Substring(6, 4);
                                        break;

                                    case 11:
                                        smsInfo.ReceivePhone1 = smsInfo.ReceivePhone.Substring(0, 3);
                                        smsInfo.ReceivePhone2 = smsInfo.ReceivePhone.Substring(3, 4);
                                        smsInfo.ReceivePhone3 = smsInfo.ReceivePhone.Substring(7, 4);
                                        break;

                                    default:
                                        if (smsInfo.ReceivePhone.Equals(""))
                                        {
                                            smsInfo.ReceivePhone1 = "";
                                            smsInfo.ReceivePhone2 = "";
                                            smsInfo.ReceivePhone3 = "";
                                        }
                                        else
                                        {
                                            smsInfo.ReceivePhone1 = smsInfo.ReceivePhone.Substring(0, 3);
                                            smsInfo.ReceivePhone2 = smsInfo.ReceivePhone.Substring(3, 3);
                                            smsInfo.ReceivePhone3 = smsInfo.ReceivePhone.Substring(6, smsInfo.ReceivePhone.Length - 6);
                                        }
                                        break;
                                }

                                Component.SMS.ControlSendSMS sendSMS = new Component.SMS.ControlSendSMS();
                                sendSMS.Transfer(smsInfo);
                            }
                            #endregion

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.Web.Ws.Preference.Proxy.List[SendAlarm]", ex);
            }

            #endregion 소스
        }

        private string[] GetAlarmCont(XmlNode _node, Account.BE.UserInfo _targetinfo, string _state)
        {
            if (_node == null || _targetinfo == null || _state.Equals("")) return null;

            string[] rtnval = new string[2];

            string subj = _node.SelectSingleNode("SUBJ/" + _state).InnerText;
            string act = _node.SelectSingleNode("ACTION/" + _state).InnerText;
            string cont = _node.SelectSingleNode("CONTENT").InnerText;

            cont = cont.Replace("$SENDDEPTNM$", CurrentUserInfo.DeptName);
            cont = cont.Replace("$SENDTITLENM$", CurrentUserInfo.TitleName);
            cont = cont.Replace("$SENDUSERNM$", CurrentUserInfo.Name);
            cont = cont.Replace("$TARGETDEPTNM$", _targetinfo.DeptBase);
            cont = cont.Replace("$TARGETTITLENM$", _targetinfo.TitleBase);
            cont = cont.Replace("$TARGETUSERNM$", _targetinfo.NameBase);
            cont = cont.Replace("$ACTION$", act);

            rtnval[0] = subj;
            rtnval[1] = cont;

            return rtnval;
        }
        #endregion 알람발송 (메일/메신저/SMS)

    }
}
