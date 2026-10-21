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
    public class ShortReply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-07-01
        /// 작성목적 : 답글 추가시 알람 발송
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public bool Add(BoaShortReplyInfo _info, string _preLang)
        {
            if (_info == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 답글 추가
                    DAL.BoaShortReply replayDal = new DAL.BoaShortReply();
                    replayDal.Add(_info);
                    
                    ts.Complete();
                }

                SendAlarm(_info, _preLang);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return false;
            }
        }

        #endregion

        #region 알람발송
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-04-27
        /// 작성목적 : 알람발송
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_scheduleID"></param>
        /// <returns></returns>
        public void SendAlarm(BoaShortReplyInfo _info, string _preLang)
        {
            DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "AlarmCommunity");

            // 알람발송
            BLL.Alarm listBiz = new Alarm();
            DAL.Boa boaDal = new DAL.Boa();

            DataTableReader dtrList = boaDal.GetList(_info.CompanyID, _info.CommunityID, _info.MenuID, _info.BoardID);

            int recvUserID = 0;

            //원본글 작성자에게 알람 발송하기
            if (dtrList != null)
            {
                if (dtrList.HasRows)
                {
                    while (dtrList.Read())
                    {

                        string AlarmCommunity = dtrList["AlarmCommunity"].ToString();
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

                        recvUserID = dtrList["WriteUserID"].ToString().ToNumber<int>();     //받을 사람 (원본글 작성자 아이디)                                                              
                        _info.Title = dtrList["Title"].ToString().ToReverseXSS();             //제목

                        foreach (DataRowView dr in dvAlarm)
                        {
                            string method = dr["CodeID"].ToString().ToUpper();
                            switch (method)
                            {
                                case "MESSENGER":
                                    if (AlarmMessenger.Equals("1"))
                                    {
                                        listBiz.SendAlarmUser(_info, recvUserID);
                                    }      
                                    break;
                            }
                        }

                        // 모바일사용의 경우, 무조건 입력
                        if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                        {
                             listBiz.SendMobileUser(_info, recvUserID);                                                                           
                        }
                    }
                }
            }

            DAL.BoaShortReply replayDal = new DAL.BoaShortReply();

            //짧은댓글 기재자들에게 알람 발송하기
            DataTableReader dtrReplay = replayDal.GetShortReplyList(_info.CompanyID, _info.CommunityID, _info.MenuID, _info.BoardID);
            List<int> userList = new List<int>();

            if (dtrReplay != null)
            {
                if (dtrReplay.HasRows)
                {
                    while (dtrReplay.Read())
                    {

                        string AlarmCommunity = dtrList["AlarmCommunity"].ToString();
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

                        //짧은댓글 작성자와 원본작성자와 동일하면 보내지 않는다.
                        if (recvUserID != dtrReplay["UserID"].ToString().ToNumber<int>())
                        {
                            //다중 댓글 중복자에게 보내지 않도록 한다. 1통만 보내자.~~~~
                            if (!userList.Exists(c => c == dtrReplay["UserID"].ToString().ToNumber<int>()))
                            {

                                foreach (DataRowView dr in dvAlarm)
                                {
                                    string method = dr["CodeID"].ToString().ToUpper();
                                    switch (method)
                                    {
                                        case "MESSENGER":
                                            if (AlarmMessenger.Equals("1"))
                                            {
                                                listBiz.SendAlarmUser(_info, dtrReplay["UserID"].ToString().ToNumber<int>());
                                            }
                                            break;
                                    }
                                }

                                // 모바일사용의 경우, 무조건 입력
                                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                                {
                                    listBiz.SendMobileUser(_info, dtrReplay["UserID"].ToString().ToNumber<int>());
                                }
                                
                                userList.Add(dtrReplay["UserID"].ToString().ToNumber<int>());
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
