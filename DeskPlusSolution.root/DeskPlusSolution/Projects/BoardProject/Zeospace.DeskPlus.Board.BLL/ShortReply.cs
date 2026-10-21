using System;
using System.Collections.Generic;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;
using System.Data;

namespace Zeospace.DeskPlus.Board.BLL
{
    public class ShortReply : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : 심재운
        /// 댓글 저장
        /// </summary>
        /// <param name="_shortinfo">The _shortinfo.</param>
        /// <returns></returns>
        public bool Add(BE.ShortReplyInfo _shortinfo, string _preLang)
        {
            if (_shortinfo == null) return false;

            try
            {
                //using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                //{
                    //짧은 댓글 저장
                    DAL.ShortReply dalSr = new DAL.ShortReply();
                    dalSr.Add(_shortinfo);

                    BLL.Lists listBiz = new Lists();
                    DAL.Lists listDal = new DAL.Lists();

                    var list = listDal.GetList(_shortinfo.CompanyID, _shortinfo.CategoryID, _shortinfo.ListID);

                    int recvUserID = 0;
                    string recvName = "";
                    string recvTitle = "";

                    // 코드관리로 처리
                    DataView dvAlarm = AppSettingHelper.GetSysCode(_shortinfo.CompanyID, "AlarmBoard");

                    string AlarmSNS = string.Empty;
                    string AlarmMail = string.Empty;
                    string AlarmMessenger = string.Empty;
                    string AlarmMobile = string.Empty;
                    string AlarmSMS = string.Empty;

                    //원본글 작성자에게 알람 발송하기
                    foreach (var listInfo in list)
                    {
                        recvUserID = listInfo.WriteUserID; //받을 사람 (원본글 작성자 아이디)
                        recvName = UtilHelper.GetLangName(
                            listInfo.NameBase.ToReverseXSS(), listInfo.NameKo.ToReverseXSS(), listInfo.NameEn.ToReverseXSS(), listInfo.NameJa.ToReverseXSS(),
                            listInfo.NameFr.ToReverseXSS(), listInfo.NameZhcn.ToReverseXSS(), listInfo.NameZhtw.ToReverseXSS(), _preLang);

                        recvTitle = listInfo.Title.ToReverseXSS(); //제목

                        listInfo.WriteUserID = _shortinfo.UserID; //보내는 사용자 ID
                        listInfo.WriteUserName = _shortinfo.UserName;//보내는 사람
                        listInfo.AlarmContent = _shortinfo.Content; //알람내용                       

                        Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
                        List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(listInfo.CompanyID, recvUserID);

                        if (listUser != null)
                        {
                            foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo infoUser in listUser)
                            {
                                AlarmSNS = infoUser.AlarmSNS;
                            }
                        }

                        if (AlarmSNS.IndexOf("1-").Equals(0))
                        {
                            AlarmMail = (AlarmSNS.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            AlarmMessenger = (AlarmSNS.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            AlarmMobile = (AlarmSNS.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                            AlarmSMS = (AlarmSNS.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            AlarmMessenger = "0";
                            AlarmMail = "0";
                            AlarmMobile = "0";
                            AlarmSMS = "0";
                        }

                        foreach (DataRowView dr in dvAlarm)
                        {
                            string method = dr["CodeID"].ToString().ToUpper();
                            switch (method)
                            {
                                case "MESSENGER":
                                    // 메신저 알림
                                    if (AlarmMessenger.Equals("1"))
                                    {
                                        listBiz.SendAlarm(listInfo, listInfo.ListID, recvUserID, 1);
                                    }
                                    break;
                            }
                        }

                        // 모바일사용의 경우, 무조건 입력
                        if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                        {
                            listBiz.SendAlarm(listInfo, listInfo.ListID, recvUserID, 4);
                        }  
                    }

                    /*
                    //짧은댓글 기재자들에게 알람 발송하기                   
                    var dtr = dalSr.GetShortReplyList(_shortinfo.CompanyID, _shortinfo.CategoryID, _shortinfo.ListID);
                    //짧은댓글 작성자 아이디 정보
                    List<int> userList = new List<int>();
                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                //짧은댓글 작성자와 원본작성자와 동일하면 보내지 않는다.
                                if (recvUserID != dtr["UserID"].ToString().ToNumber<int>())
                                {
                                    //다중 댓글 중복자에게 보내지 않도록 한다. 1통만 보내자.~~~~
                                    if (!userList.Exists(c => c == dtr["UserID"].ToString().ToNumber<int>()))
                                    {
                                        BE.ListInfo listInfo = new ListInfo();

                                        listInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                                        listInfo.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                                        listInfo.ListID = dtr["ListID"].ToString().ToNumber<int>();

                                        listInfo.Title = recvTitle; //알람 제목
                                        listInfo.AlarmContent = _shortinfo.Content; //알람내용
                                        listInfo.WriteUserID = recvUserID; //보내는 사용자 ID (게시판 작성자)
                                        listInfo.WriteUserName = recvName;//보내는 사람   (게시판 작성자 이름)                                        

                                        Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
                                        List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> listUser = dal.GetUserEnv(listInfo.CompanyID, dtr["UserID"].ToString().ToNumber<int>());

                                        if (listUser != null)
                                        {
                                            foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo infoUser in listUser)
                                            {
                                                AlarmSNS = infoUser.AlarmSNS;
                                            }
                                        }

                                        if (AlarmSNS.IndexOf("1-").Equals(0))
                                        {
                                            AlarmMail = (AlarmSNS.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                            AlarmMessenger = (AlarmSNS.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                            AlarmMobile = (AlarmSNS.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                            AlarmSMS = (AlarmSNS.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                                        }
                                        else
                                        {
                                            AlarmMessenger = "0";
                                            AlarmMail = "0";
                                            AlarmMobile = "0";
                                            AlarmSMS = "0";
                                        }

                                        foreach (DataRowView dr in dvAlarm)
                                        {
                                            string method = dr["CodeID"].ToString().ToUpper();
                                            switch (method)
                                            {
                                                case "MESSENGER":
                                                    // 메신저 알림
                                                    if (AlarmMessenger.Equals("1"))
                                                    {
                                                        listBiz.SendAlarm(listInfo, listInfo.ListID, dtr["UserID"].ToString().ToNumber<int>(), 1);
                                                    }
                                                    break;
                                            }
                                        }

                                        // 모바일사용의 경우, 무조건 입력
                                        if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                                        {
                                            listBiz.SendAlarm(listInfo, listInfo.ListID, dtr["UserID"].ToString().ToNumber<int>(), 4);
                                        } 

                                        userList.Add(dtr["UserID"].ToString().ToNumber<int>());
                                    }
                                }
                            }
                        }
                    }
                    */

                    //ts.Complete();
                //}

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                throw;
            }
        }

        #endregion ADD()
    }
}