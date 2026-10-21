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

namespace Zeospace.DeskPlus.Pim.Web.Ws.Schedule
{
    /// <summary>
    /// ShortReplyMod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyMod : Base.BaseWSPim
    {

        #region 댓글 수정

        /// <summary>
        /// 할일 댓글 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModShortReple(string p)
        {
            SetDecryptString(p);
            string scheduleID = GetDecryptParam("scheduleID", "0");
            string replyID = GetDecryptParam("replyID", "0");
            string contents = GetDecryptParam("contents", "");
            string security = GetDecryptParam("security", "0");

            BE.ScheduleReplyInfo info = new ScheduleReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ScheduleID = scheduleID.ToNumber<int>();
            info.ReplyID = replyID.ToNumber<int>();
            info.Content = contents.ToReverseXSS();
            info.UserID = CurrentUserInfo.UserID;
            info.IsSecurity = security;

            DAL.ScheduleReply dal = new ScheduleReply();
            return dal.Mod(info);
        }

        #endregion
    }
}
