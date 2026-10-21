using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Apps.Board
{
    /// <summary>
    /// ShortReplyMod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyMod : Base.BaseWSCommunity
    {
        #region MOD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-29
        /// 작성목적 : 짧은 답글 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModShortReple(string p)
        {
            try
            {
                SetDecryptString(p);

                int shortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
                string contents = GetDecryptParam("contents", "");

                BE.BoaShortReplyInfo info = new BoaShortReplyInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CommunityID = communityID;
                info.MenuID = menuID;
                info.BoardID = boardID;
                info.ShortReplyID = shortReplyID;
                info.UserID = CurrentUserInfo.UserID;
                info.Content = Server.UrlDecode(contents).ToReverseXSS();

                DAL.BoaShortReply dal = new DAL.BoaShortReply();
                return dal.Mod(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion
    }
}
