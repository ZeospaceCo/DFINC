using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Schedule
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSPim
    {
        #region 댓글

        #region 가져오기
        /// <summary>
        /// 일정 댓글 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetShortReplyList(string p)
        {
            string scheduleID = string.Empty;
            SetDecryptString(p);
            scheduleID = GetDecryptParam("scheduleID", "0");

            ObjList listobj = new ObjList();

            DAL.ScheduleReply sr = new ScheduleReply();

            DataTableReader dtr = sr.GetShortReplyList(CurrentUserInfo.CompanyID, scheduleID.ToNumber<int>());
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ScheduleReplyInfo info = new ScheduleReplyInfo();

                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.ScheduleID = scheduleID.ToNumber<int>();
                        info.ReplyID = dtr["ReplyID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.SUserID = dtr["SUserID"].ToString().ToNumber<int>();
                        info.IsSecurity = dtr["IsSecurity"].ToString();

                        info.UserName = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);
                        info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        info.View = "N";

                        // 원본글 작성자와 댓글러가 같은지 
                        if (CurrentUserInfo.UserID == info.SUserID)
                        {
                            info.View = "Y";
                        }
                        else if (CurrentUserInfo.UserID == info.UserID && info.IsSecurity == "1")
                        {
                            info.View = "Y";
                        }
                        else if (info.IsSecurity == "0")
                        {
                            info.View = "Y";
                        }

                        DateTime dt;


                        if (DateTime.TryParse(dtr["InputDate"].ToString(), out dt))
                            info.InputDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.InputDate = string.Empty;

                        #region 날짜 비교(New Img)
                        DateTime writeDate = Convert.ToDateTime(dtr["InputDate"].ToString());
                        DateTime todayDate = System.DateTime.Now;
                        TimeSpan gap = todayDate.Subtract(writeDate);
                        
                        info.ShowNew = (gap.Days <= 0) ? "Y" : "N";
                        #endregion


                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }

        #endregion

        #region 삭제

        /// <summary>
        /// 댓글 DEL
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShortReply(string p)
        {
            SetDecryptString(p);
            string scheduleID = GetDecryptParam("scheduleID", "0");
            string replyID = GetDecryptParam("replyID", "0");

            DAL.ScheduleReply biz = new ScheduleReply();
            BE.ScheduleReplyInfo info = biz.GetShortReplyID(CurrentUserInfo.CompanyID, scheduleID.ToNumber<int>(), replyID.ToNumber<int>());

            if (info.UserID == CurrentUserInfo.UserID)
            {
                return biz.Del(CurrentUserInfo.CompanyID, scheduleID.ToNumber<int>(), replyID.ToNumber<int>());
            }
            else
            {
                return false;
            }
        }

        #endregion
        #endregion

    }
}
