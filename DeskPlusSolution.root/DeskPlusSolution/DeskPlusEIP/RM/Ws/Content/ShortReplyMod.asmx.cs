using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.RM.Web.Ws.Content
{
    /// <summary>
    /// ShortReplyMod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyMod : Base.BaseWSRM
    {
        #region MOD()

        /// <summary>
        /// 댓글 Mod
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModShortReple(string p)
        {
            SetDecryptString(p);
            string reservationID = GetDecryptParam("reservationID", "0");
            string parentReservationID = GetDecryptParam("parentReservationID", "0");
            string shortReplyID = GetDecryptParam("shortReplyID", "0");
            string contents = GetDecryptParam("contents", "");

            BE.ShortReplyInfo info = new BE.ShortReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ReservationID = reservationID.ToNumber<int>();
            info.ShortReplyID = shortReplyID.ToNumber<int>();
            info.Content = Server.UrlDecode(contents).ToReverseXSS();
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;

            BLL.ShortReply dal = new BLL.ShortReply();
            return dal.Mod(info, parentReservationID.ToNumber<int>(), CurrentUserInfo.PreferredLanguage);
        }

        #endregion 
    }
}
