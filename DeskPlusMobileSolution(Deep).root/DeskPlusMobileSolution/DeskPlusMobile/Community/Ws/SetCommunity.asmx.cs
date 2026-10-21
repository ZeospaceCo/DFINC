using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.IO;
using Zeospace.DeskPlusMobile.Web.Community.Ws.BE;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;

namespace Zeospace.DeskPlusMobile.Web.Community.Ws
{
    /// <summary>
    /// SetCommunity의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetCommunity : Helper.MobileBaseService
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 방문카운트, 포인트 추가
        /// 작성일자 : 2012-07-09
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddVisitPoint(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("CommunityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("MenuID", "0").ToNumber<int>();

                if (communityID < 0 || menuID < 0) return false;

                SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@CommunityID", SqlDbType.Int),
											   new SqlParameter("@UserID", SqlDbType.Int)
										   };
                parms[0].Value = CompanyID;
                parms[1].Value = communityID;
                parms[2].Value = UserID;

                string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLCommunity, "INS_PROC_USERVISITETC");
                SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(CompanyID), CommandType.StoredProcedure, proc, "AddVisitPoint", parms);
                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddVisitPoint", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 댓글, 포인트 등록
        /// 작성일자 : 2012-07-12
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddShortReplyPoint(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("CommunityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("MenuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("BoardID", "0").ToNumber<int>();
                string content = GetDecryptParam("Content", "");

                if (communityID < 0 || menuID < 0 || boardID < 0) return false;

                Ws.BE.BoaShortReplyInfo info = new BE.BoaShortReplyInfo();

                info.CompanyID = CompanyID;
                info.CommunityID = communityID;
                info.MenuID = menuID;
                info.BoardID = boardID;
                info.UserID = UserID;
                info.Content = Server.UrlDecode(content).ToReverseXSS();
                info.UserName = UserInfo.Name;

                // 댓글, 포인트 추가 
                Ws.Data.BoaShortReply dal = new Data.BoaShortReply();
                if (!dal.Add(info))
                {
                    return false;
                }

                SendAlarm(CompanyID, communityID, menuID, boardID, info);

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AddShortReplyPoint", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성목적   : 알람 보내기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-05-18
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_recvUserID">The _recv user ID.</param>
        public void SendAlarm(int _CompanyID, int _communityID, int _menuID, int _boardID, Ws.BE.BoaShortReplyInfo _info)
        {

            // 알람-코드관리로 처리
            DataView dvApp = XmlDataBaseHelper.GetSysCode(CompanyID, "AlarmCommunity");
            if (dvApp.Count > 0)
            {

                foreach (DataRowView dr in dvApp)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MESSENGER":
                            // 알람 발송
                            Ws.Data.Boa boaDal = new Data.Boa();
                            DataSet ds = boaDal.GetBoardList(CompanyID, _communityID, _menuID, _boardID);

                            int recvUserID = 0;

                            //원본글 작성자에게 알람 발송하기
                            if (ds != null)
                            {
                                foreach (DataRow dr2 in ds.Tables[0].Rows)
                                {
                                    recvUserID = dr2["WriteUserID"].ToString().ToNumber<int>();     //받을 사람 (원본글 작성자 아이디)                        
                                    _info.Title = dr2["Title"].ToString().ToReverseXSS();             //제목

                                    string AlarmCommunity = dr2["AlarmCommunity"].ToString();
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
                                        SendAlarmUser(_info, recvUserID, 1);
                                    }

                                    SendAlarmUser(_info, recvUserID, 4);
                                }
                            }

                            //댓글 기재자들에게 알람 발송하기
                            Ws.Data.BoaShortReply replyDal = new Data.BoaShortReply();
                            DataSet dsReply = replyDal.GetShortReply(CompanyID, _communityID, _menuID, _boardID);
                            List<int> userList = new List<int>();

                            if (dsReply != null)
                            {
                                foreach (DataRow dr2 in dsReply.Tables[0].Rows)
                                {
                                    string AlarmCommunity = dr2["AlarmCommunity"].ToString();
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

                                    //댓글 작성자와 원본작성자와 동일하면 보내지 않는다.
                                    if (recvUserID != dr2["UserID"].ToString().ToNumber<int>())
                                    {
                                        //다중 댓글 중복자에게 보내지 않도록 한다. 1통만 보내자.~~~~
                                        if (!userList.Exists(c => c == dr2["UserID"].ToString().ToNumber<int>()))
                                        {
                                            if (AlarmMessenger.Equals("1"))
                                            {
                                                SendAlarmUser(_info, dr2["UserID"].ToString().ToNumber<int>(), 1);
                                            }

                                            SendAlarmUser(_info, dr2["UserID"].ToString().ToNumber<int>(), 4);
                                            userList.Add(dr2["UserID"].ToString().ToNumber<int>());
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 작성목적   : 알람 보내기
        /// 작성자     : famo23
        /// 최초작성일 : 2012-07-13
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_recvUserID">The _recv user ID.</param>
        public bool SendAlarmUser(BoaShortReplyInfo _info, int _receiverID, int _AlarmType)
        {
            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();

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

            string httpType = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            string fullDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "FULLDOMAIN");
            string alarmTargetURL = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "AlarmBoardURL");

            alarmTargetURL = string.Format(alarmTargetURL, httpType, fullDomain);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("communityID", _info.CommunityID.ToString());
            paraDic.Add("menuID", _info.MenuID.ToString());
            paraDic.Add("boardID", _info.BoardID.ToString());
            paraDic.Add("alarmPopup", "Y");

            HttpContext context = HttpContext.Current;
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            alarmTargetURL = alarmTargetURL + "?p=" + encparam;

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = "/DeskPlusMobile/Community/AlarmView.aspx?p=" + encparam;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = _AlarmType;
            alarmInfo.KindSubject = "Community";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm biz = new Messenger.BLL.Alarm();
            return biz.Add(alarmInfo);
        }
    }
}
