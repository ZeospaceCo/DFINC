using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;


namespace Zeospace.DeskPlusMobile.Web.RM.Ws
{
    /// <summary>
    /// SetRM의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetRM : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 자원예약
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            SetDecryptString(p);

            string subject = GetDecryptParam("subject", "");
            string content = GetDecryptParam("content", "");
            string startDate = GetDecryptParam("startDate", "");
            string endDate = GetDecryptParam("endDate", "");
            string isGuard = GetDecryptParam("isGuard", "");
            string sTime = GetDecryptParam("sTime", "");
            string eTime = GetDecryptParam("eTime", "");
            string listID = GetDecryptParam("listID", "0");
            string qty = GetDecryptParam("qty", "0");
            string authority = GetDecryptParam("authority", "0");
            try
            {
                int reservationID = 0;

                Ws.BE.ReserveInfo info = new BE.ReserveInfo();

                info.CompanyID = CompanyID;
                info.Subject = Server.UrlDecode(subject).ToXSS();
                info.UserImg = "FF0000";
                info.Content = Server.UrlDecode(content).ToXSS().ToEditorXSS();
                info.IsGuard = isGuard;

                info.DeptID = UserInfo.DeptID;
                info.WriteUserID = UserID;
                info.WriteUserName = UserInfo.Name;
                info.WriteDateYM = FileHelper.GetYearMonth();
                info.WriteDate = System.DateTime.Now;

                info.ModifyUserID = UserID;
                info.ModifyUserName = UserInfo.Name;
                info.ModifyDate = System.DateTime.Now;


                if (authority == "1")
                {
                    //자원 담당자의 승인 필요 O
                    info.Usable = "0";
                    //본인이 담당자의 경우에는 승인 필요X
                    DataTableReader dtr = GetListUser(CompanyID, listID.ToNumber<int>());

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                if (UserID == dtr["ManagerUserID"].ToString().ToNumber<int>())
                                {
                                    info.Usable = "1";
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //자원 담당자의 승인 필요 X
                    info.Usable = "1";
                }

                DateTime dtStart = Convert.ToDateTime(startDate);
                DateTime dtEnd = Convert.ToDateTime(endDate);

                info.StartDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, (Convert.ToDateTime(sTime)).Hour, (Convert.ToDateTime(sTime)).Minute, 0);
                info.EndDate = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, (Convert.ToDateTime(eTime)).Hour, (Convert.ToDateTime(eTime)).Minute, 0);

                TimeSpan gap = info.EndDate.Subtract(info.StartDate);
                info.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();

                info.ParentReservationID = 0;

                info.ListID = listID.ToNumber<int>();
                info.Qty = qty.ToNumber<int>();

                BLL.Reserve bll = new BLL.Reserve();
                reservationID = bll.Add(info);


                #region 알람발송

                if (reservationID > 0 && info.Usable == "0")
                {
                    SendAlarm(reservationID, "ADD");

                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Add", ex);
                return false;
            }
        }


        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 자원예약 댓글등록
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddReply(string p)
        {
            SetDecryptString(p);

            string reservationID = GetDecryptParam("ReservationID", "0");
            string parentReservationID = GetDecryptParam("ParentReservationID", "0");
            string contents = GetDecryptParam("Content", "");

            try
            {
                BE.ShortReplyInfo info = new BE.ShortReplyInfo();

                info.CompanyID = CompanyID;
                info.ReservationID = reservationID.ToNumber<int>();
                info.Content = Server.UrlDecode(contents).ToReverseXSS();
                info.UserID = UserID;
                info.UserName = UserInfo.Name;

                Data.ShortReply dal = new Data.ShortReply();
                dal.Add(info);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddReply", ex);
                return false;
            }
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddReject(string p)
        {
            SetDecryptString(p);

            string reservationID = GetDecryptParam("ReservationID", "0");
            string parentReservationID = GetDecryptParam("ParentReservationID", "0");
            string contents = GetDecryptParam("Content", "");

            bool rtnVal = false;
            try
            {
                BE.RejectInfo info = new BE.RejectInfo();

                info.CompanyID = CompanyID;
                info.ReservationID = reservationID.ToNumber<int>();
                info.Content = Server.UrlDecode(contents).ToReverseXSS();
                info.UserID = UserID;
                info.UserName = UserInfo.Name;

                Data.Reject dal = new Data.Reject();

                rtnVal = dal.Add(info);
                if (rtnVal)
                {
                    //거부처리
                    Data.Reserve rDal = new Data.Reserve();
                    rtnVal = rDal.ModUsable(CompanyID, reservationID.ToNumber<int>(), "2");

                    if (rtnVal)
                    {
                        SendAlarm(reservationID.ToNumber<int>(), "REJECT");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddReply", ex);
                return false;
            }
        }


        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 승인or거부
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModUsable(string p)
        {
            SetDecryptString(p);

            string reservationID = GetDecryptParam("reservationID", "0");
            string usable = GetDecryptParam("usable", "");
            bool rtnVal = false;
            try
            {
                Data.Reserve dal = new Data.Reserve();
                rtnVal = dal.ModUsable(CompanyID, reservationID.ToNumber<int>(), usable);

                string section = (usable == "1") ? "APPROVE" : "REJECT";
                if (rtnVal)
                {
                    SendAlarm(reservationID.ToNumber<int>(), section);
                }
                return rtnVal;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ModUsable", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : 방영환
        /// 작성목적 : 자원예약삭제
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            SetDecryptString(p);

            string reservationID = GetDecryptParam("reservationID", "0");
            string parentReservationID = GetDecryptParam("parentReservationID", "0");
            string writeYM = GetDecryptParam("writeYM", "");
            string repeatSel = GetDecryptParam("repeatSel", "");
            string startDate = GetDecryptParam("startDate", "");
            try
            {
                BLL.Reserve bll = new BLL.Reserve();

                if (repeatSel == "1")
                {
                    //이번건만 삭제
                    bll.Del(CompanyID, reservationID.ToNumber<int>(), parentReservationID.ToNumber<int>(), writeYM);
                }
                else if (repeatSel == "2")
                {
                    DateTime tmpDate = Convert.ToDateTime(startDate);
                    string tmpStartDate = tmpDate.ToShortDateString().Replace("-", "");
                    bll.Del(CompanyID, reservationID.ToNumber<int>(), parentReservationID.ToNumber<int>(), writeYM, tmpStartDate);
                }
                else if (repeatSel == "3")
                {
                    bll.DelAll(CompanyID, parentReservationID.ToNumber<int>());
                }
                else
                {
                    //반복일정이 아닌경우
                    bll.Del(CompanyID, reservationID.ToNumber<int>());
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Del", ex);
                return false;
            }
        }

        /// <summary>
        /// 자원담당자 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <returns></returns>
        private DataTableReader GetListUser(int _companyID, int _listID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_QRY_ListUser");

                sql = string.Format(sql, _companyID, _listID);

                return SQLHelper.ExecuteDataTableReader(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsInfo", null);
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAccountInfo()", ex);
                return null;
            }
        }


        /// <summary>
        /// 자원예약 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        private BE.ReserveInfo GetReserve(int _companyID, int _reservationID)
        {
            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_QRY_SelReserve");
            sqlQuery = String.Format(sqlQuery, _companyID, _reservationID);

            DataTableReader dtr = SQLHelper.ExecuteDataTableReader(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetReserve", null);

            BE.ReserveInfo info = null;
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        info = new BE.ReserveInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                        info.ParentReservationID = dtr["ParentReservationID"].ToString().ToNumber<int>();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        info.WriteUserName = dtr["WriteUserName"].ToString();
                        info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                        info.Subject = dtr["Subject"].ToString().ToReverseXSS();
                        info.Title = dtr["Title"].ToString();
                    }
                }
            }

            return info;
        }

        #region 알람 - 알람발송

        /// <summary>
        ///  자원예약, 승인 or 거부시 알람 발송.
        /// </summary>
        /// <param name="_reservationID"></param>
        /// <param name="_usable"></param>
        private void SendAlarm(int _reservationID, string _section)
        {
            /*
            1. 자원등록시 : 자원 등록시에 해당 자원이 승인이 필요한 경우에만 담당자에게 알람 발송.
            2. 승인 or 거부시 : 예약자에게 알람 발송. 예약자가 자원담당자와 동일시에는 발송하지 않는다.
             * 자원예약 거부시 : 예약자, 공유자에게 알람발송.
             * 자원예약 승인시 : 예약자, 공유자에게 알람발송.
             * 자원예약 등록시 : 담당자에게 알람발송.             
            */
            //담당자 : 자원예약 등록시 승인이 필요한 경우에만.
            //자원예약자 : 승인 or 거부시
            //공유자 : 승인 or 거부시
            // 등록 : ADD, 승인 : APPROVE, 거부 : REJECT

            //자원예약정보 가져오기
            BE.ReserveInfo info = new BE.ReserveInfo();
            info = GetReserve(CompanyID, _reservationID);

            //알람발송자 정보
            BE.AccountInfo sendUserInfo = new BE.AccountInfo();
            sendUserInfo.CompanyID = UserInfo.CompanyID;
            sendUserInfo.UserID = UserInfo.UserID;
            sendUserInfo.Name = UserInfo.Name;
            sendUserInfo.DeptName = UserInfo.DeptName;
            sendUserInfo.TitleName = UserInfo.TitleName;
            sendUserInfo.DutyName = UserInfo.DutyName;
            sendUserInfo.Mail = UserInfo.MailID + "@" + UserInfo.MailDomain;
            sendUserInfo.MobilePhone = UserInfo.MobilePhone;
            sendUserInfo.PreLang = PreLang;

            //알람대상자 가져오기
            List<BE.AccountInfo> receiveUserlist = GetAlarmTarget(CompanyID, _reservationID, UserID, _section);

            //코드 관리로 처리
            DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmResource");
            if (dvApp.Count > 0)
            {
                foreach (DataRowView dr in dvApp)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MAIL":
                            SendMail(info, sendUserInfo, receiveUserlist, _section);
                            break;
                        case "MESSENGER":
                            SendMessenger(info, sendUserInfo, receiveUserlist, _section);
                            break;
                    }
                }
            }

            //모바일은 무조건 알람발송
            SendMobile(info, sendUserInfo, receiveUserlist, _section);
        }


        /// <summary>
        /// 자원예약, 승인, 거부 알람대상자 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_section"></param>
        /// <returns></returns>
        private List<BE.AccountInfo> GetAlarmTarget(int _companyID, int _reservationID, int _userID, string _section)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@ReservationID", SqlDbType.Int)
                                            ,new SqlParameter("@UserID", SqlDbType.Int)
                                            ,new SqlParameter("@Section", SqlDbType.VarChar, 10)                                            
                    };
                parm[0].Value = _companyID;
                parm[1].Value = _reservationID;
                parm[2].Value = _userID;
                parm[3].Value = _section;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_PROC_Reserve_AlarmUser");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, proc, "GetAlarmTarget", parm);

                List<BE.AccountInfo> list = new List<BE.AccountInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.AccountInfo info = new BE.AccountInfo();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.UserID = dr["UserID"].ToString().ToNumber<int>();
                    info.Name = dr["NameBase"].ToString().ToNull();

                    if (!dr["MailID"].ToString().Equals("") && !dr["MailDomain"].ToString().Equals(""))
                    {
                        info.Mail = dr["MailID"].ToString() + "@" + dr["MailDomain"].ToString();
                    }
                    else
                    {
                        info.Mail = "";
                    }
                    info.MobilePhone = dr["MobilePhone"].ToString().ToNull();

                    // 개인알람정보추가 2017-04-14
                    info.PreferredLanguage = dr["PreLang"].ToString();
                    info.PreLang = dr["PreLang"].ToString();
                    info.AlarmApproval = dr["AlarmRM"].ToString();
                    if (info.AlarmApproval.IndexOf("1-").Equals(0))
                    {
                        info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                        info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                        info.AlarmMobile = (info.AlarmApproval.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                        info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                    }
                    else
                    {
                        info.AlarmMessenger = "0";
                        info.AlarmMail = "0";
                        info.AlarmMobile = "0";
                        info.AlarmSMS = "0";
                    }

                    list.Add(info);
                }

                return list;

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmTarget('" + _reservationID + "')", ex);
                return null;
            }
        }


        /// <summary>
        /// 메신저 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_section"></param>
        private void SendMessenger(BE.ReserveInfo _info, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _section)
        {
            string[] arrCont = null;

            foreach (BE.AccountInfo info in _receiveUserlist)
            {
                if (info.AlarmMessenger.Equals("1"))
                {
                    //알람내용 가져오기
                    arrCont = GetAlarmCont(_info, info.PreLang, "MESSENGER", _section, "");
                    if (arrCont == null) return;

                    Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
                    alarmInfo.CompanyID = _info.CompanyID;
                    alarmInfo.SenderID = _sendUserInfo.UserID;
                    alarmInfo.SenderName = _sendUserInfo.Name;
                    alarmInfo.SenderMail = "";
                    alarmInfo.SenderPhone = "";
                    alarmInfo.ReceiverID = info.UserID;
                    alarmInfo.ReceiverName = "";
                    alarmInfo.ReceiverMail = "";
                    alarmInfo.ReceiverPhone = "";
                    alarmInfo.Status = "";
                    alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.Subject = arrCont[0];
                    alarmInfo.Content = arrCont[1];


                    string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
                    string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
                    string targetURL = "";
                    if (_section == "ADD")
                    {
                        targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.RM, "AlarmTargetURL");
                        targetURL = string.Format(targetURL, baseHttp, fullDomain);
                        targetURL = targetURL + "$t=rmBasePage|AdminReserve";
                    }
                    else
                    {
                        targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.RM, "AlarmViewTargetURL");
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("reservationID", _info.ReservationID.ToString());

                        HttpContext context = HttpContext.Current;
                        string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                        targetURL = targetURL + "?p=" + encparam;
                    }

                    alarmInfo.TargetUrl = targetURL;
                    alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                    alarmInfo.WriteDate = System.DateTime.Now;
                    alarmInfo.AlarmType = 1;        // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                    alarmInfo.KindSubject = "ResourceManager";
                    alarmInfo.ViewOption = "01";
                    alarmInfo.Store = 1;
                    alarmInfo.UserState = 0;

                    Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();
                    alarm.Add(alarmInfo);
                }
            }
        }

        /// <summary>
        /// 모바일 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_section"></param>
        private void SendMobile(BE.ReserveInfo _info, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _section)
        {
            string[] arrCont = null;

            foreach (BE.AccountInfo info in _receiveUserlist)
            {
                //알람내용 가져오기
                arrCont = GetAlarmCont(_info, info.PreLang, "MESSENGER", _section, "");
                if (arrCont == null) return;

                Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
                alarmInfo.CompanyID = _info.CompanyID;
                alarmInfo.SenderID = _sendUserInfo.UserID;
                alarmInfo.SenderName = _sendUserInfo.Name;
                alarmInfo.SenderMail = "";
                alarmInfo.SenderPhone = "";
                alarmInfo.ReceiverID = info.UserID;
                alarmInfo.ReceiverName = "";
                alarmInfo.ReceiverMail = "";
                alarmInfo.ReceiverPhone = "";
                alarmInfo.Status = "";
                alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.Subject = arrCont[0];
                alarmInfo.Content = arrCont[1];


                string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
                string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
                string targetURL = "";
                string targetMobileUrl = "";
                if (_section == "ADD")
                {
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.RM, "AlarmTargetURL");
                    targetURL = string.Format(targetURL, baseHttp, fullDomain);
                    targetURL = targetURL + "$t=rmBasePage|AdminReserve";

                    targetMobileUrl = "/DeskPlusMobile/RM/UsableList.aspx?";
                }
                else
                {
                    targetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.RM, "AlarmViewTargetURL");
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("reservationID", _info.ReservationID.ToString());

                    HttpContext context = HttpContext.Current;
                    string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                    targetURL = targetURL + "?p=" + encparam;

                    targetMobileUrl = "/DeskPlusMobile/RM/AlarmView.aspx?p=" + encparam;
                }

                alarmInfo.TargetUrl = targetURL;
                alarmInfo.TargetMobileUrl = targetMobileUrl;
                alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
                alarmInfo.WriteDate = System.DateTime.Now;
                alarmInfo.AlarmType = 4;        // 알람형식 : 1=메신저 2=메일 3=SMS 4=모바일
                alarmInfo.KindSubject = "ResourceManager";
                alarmInfo.ViewOption = "01";
                alarmInfo.Store = 1;
                alarmInfo.UserState = 0;

                Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();
                alarm.Add(alarmInfo);
            }
        }

        /// <summary>
        /// 메일 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_sendUserInfo"></param>
        /// <param name="_receiveUserlist"></param>
        /// <param name="_section"></param>
        private void SendMail(BE.ReserveInfo _info, BE.AccountInfo _sendUserInfo, List<BE.AccountInfo> _receiveUserlist, string _section)
        {
            // 받는 사람
            Hashtable htMailTo = new Hashtable();

            foreach (BE.AccountInfo info in _receiveUserlist)
            {
                if (info.AlarmMail.Equals("1"))
                {
                    if (!htMailTo.ContainsKey(info.Mail))
                    {
                        htMailTo.Add(info.Mail, info.Mail);
                    }
                }
            }

            string linkURL = string.Empty;

            string baseHttp = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            linkURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.RM, "AlarmViewTotalTargetURL");
            linkURL = string.Format(linkURL, baseHttp, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("companyID", _info.CompanyID.ToString());
            paraDic.Add("reservationID", _info.ReservationID.ToString());

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            linkURL = linkURL + "?p=" + encparam;

            MailDataInfo mailInfo = new MailDataInfo();
            mailInfo.To = htMailTo;
            mailInfo.From = _sendUserInfo.Mail;

            string[] arrAlarm = GetAlarmCont(_info, _sendUserInfo.PreLang, "MAIL", _section, linkURL);
            if (arrAlarm == null) return;

            mailInfo.Subject = arrAlarm[0];
            mailInfo.Contents = arrAlarm[1];

            Zeospace.DeskPlusMobile.Web.Helper.SendMail sendMail = new SendMail();
            sendMail.Send(mailInfo, false, false, "");
        }

        /// <summary>
        /// 알람내용 가져오기
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_preLang"></param>
        /// <param name="_gubun"></param>
        /// <param name="_section"></param>
        /// <param name="_linkURL"></param>
        /// <returns></returns>
        private string[] GetAlarmCont(BE.ReserveInfo _info, string _preLang, string _gubun, string _section, string _linkURL)
        {
            try
            {
                string xmlPath = Server.MapPath("/DeskPlusMobile/XmlData/RM/" + PreLang + ".xml");
                XmlDocument contXml = new XmlDocument();
                contXml.Load(xmlPath);
                XmlNode rootNode = contXml.DocumentElement;

                string[] arrRtnval = new string[2];

                arrRtnval[0] = rootNode.SelectSingleNode("/ALARM/" + _gubun + "/SUBJ/" + _section).InnerText + _info.Subject.ToReverseXSS();
                arrRtnval[1] = (rootNode.SelectSingleNode("/ALARM/" + _gubun + "/ContentHTML") != null) ? System.Web.HttpUtility.HtmlDecode(rootNode.SelectSingleNode("/ALARM/" + _gubun + "/ContentHTML").InnerText) : "";
                arrRtnval[1] = arrRtnval[1].Replace("$SECTION$", rootNode.SelectSingleNode("/ALARM/" + _gubun + "/SECTION/" + _section).InnerText);          //문서제목
                arrRtnval[1] = arrRtnval[1].Replace("$ListName$", _info.Title);          //자원명
                arrRtnval[1] = arrRtnval[1].Replace("$SUBJECT$", _info.Subject.ToReverseXSS());          //문서제목
                arrRtnval[1] = arrRtnval[1].Replace("$Name$", _info.WriteUserName);          //예약자
                arrRtnval[1] = arrRtnval[1].Replace("$StartDate$", _info.StartDate.ToString("yyyy-MM-dd")); //시작년월일

                if (_linkURL != "")
                {
                    string linkURL = rootNode.SelectSingleNode("/ALARM/" + _gubun + "/PAGELINK").InnerText.Replace("$LINKURL$", _linkURL);
                    arrRtnval[1] = arrRtnval[1] + linkURL;
                }

                return arrRtnval;   // [0] : Subj / [1] : Content
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetAlarmCont()", ex);
                return null;
            }
        }

        #endregion
    }
}
