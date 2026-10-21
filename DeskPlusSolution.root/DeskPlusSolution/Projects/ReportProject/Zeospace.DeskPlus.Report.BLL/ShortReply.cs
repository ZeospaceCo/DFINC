using System;
using System.Collections.Generic;
using System.Transactions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Report.BE;

namespace Zeospace.DeskPlus.Report.BLL
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
        public bool Add(BE.ShortReplyInfo _shortinfo, string _preLang, string _preTitle)
        {
            if (_shortinfo == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //짧은 댓글 저장
                    DAL.ShortReply dalSr = new DAL.ShortReply();
                    dalSr.Add(_shortinfo);

                    BLL.List listBiz = new List();
                    DAL.List listDal = new DAL.List();

                    var list = listDal.GetList(_shortinfo.CompanyID, _shortinfo.ListID.ToString());

                    int recvUserID = 0;
                    string recvName = "";
                    string recvTitle = "";

                    //원본글 작성자에게 알람 발송하기
                    foreach (var listInfo in list)
                    {
                        recvUserID = listInfo.WriteUserID; //받을 사람 (원본글 작성자 아이디)
                        recvName = UtilHelper.GetLangName(
                            listInfo.NameBase.ToReverseXSS(), listInfo.NameKo.ToReverseXSS(), listInfo.NameEn.ToReverseXSS(), listInfo.NameJa.ToReverseXSS(),
                            listInfo.NameFr.ToReverseXSS(), listInfo.NameZhcn.ToReverseXSS(), listInfo.NameZhtw.ToReverseXSS(), _preLang);
                        // 제목
                        listInfo.Title = _preTitle;
                        recvTitle = listInfo.Title; //제목

                        listInfo.WriteUserID = _shortinfo.UserID; //보내는 사용자 ID
                        listInfo.WriteUserName = _shortinfo.UserName;//보내는 사람
                        listInfo.AlarmContent = _shortinfo.Content; //알람내용

                        if (listInfo.ListID > 0 && recvUserID > 0 && _preTitle.Length > 0)
                        {
                            listBiz.SendAlarm(listInfo, recvUserID);
                        }
                    }

                    //짧은댓글 기재자들에게 알람 발송하기
                    var dtr = dalSr.GetShortReplyList(_shortinfo.CompanyID, _shortinfo.ListID);
                    //짧은댓글 작성자 아이디 정보
                    List<int> userList = new List<int>();

                    var alarmRcvUserID = 0;

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
                                        listInfo.ListID = dtr["ListID"].ToString().ToNumber<int>();

                                        listInfo.Title = recvTitle; //알람 제목
                                        listInfo.AlarmContent = _shortinfo.Content; //알람내용
                                        listInfo.WriteUserID = recvUserID; //보내는 사용자 ID (게시판 작성자)
                                        listInfo.WriteUserName = recvName;//보내는 사람   (게시판 작성자 이름)

                                        alarmRcvUserID = dtr["UserID"].ToString().ToNumber<int>();

                                        if (listInfo.ListID > 0 && alarmRcvUserID > 0 && _preTitle.Length > 0)
                                        {
                                            listBiz.SendAlarm(listInfo, alarmRcvUserID);
                                        }

                                        userList.Add(dtr["UserID"].ToString().ToNumber<int>());
                                    }
                                }
                            }
                        }
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return false;
            }
        }

        #endregion ADD()
    }
}