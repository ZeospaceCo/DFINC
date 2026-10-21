using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.PMS.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
namespace Zeospace.DeskPlus.PMS.Web.Ws.Board
{
    /// <summary>
    /// ShortReplyMod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyMod : Base.BaseWSPMS
    {

        #region MOD()

        #region 댓글

        /// <summary>
        /// 짧은댓글 Mod
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModShortReple(string p)
        {
            string projectID = string.Empty;
            string menuID = string.Empty;
            string contents = string.Empty;
            string boardShortReplyID = string.Empty;
            string boardID = string.Empty;
            //contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);

            SetDecryptString(p);
            projectID = GetDecryptParam("projectID", "0");
            menuID = GetDecryptParam("menuID", "0");
            contents = GetDecryptParam("contents", "");
            boardID = GetDecryptParam("boardID", "");
            boardShortReplyID = GetDecryptParam("boardShortReplyID", "0");

            BE.BoardShortReplyInfo info = new BE.BoardShortReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ProjectID = projectID.ToNumber<int>();
            info.MenuID = menuID.ToNumber<int>();
            info.BoardID = boardID.ToNumber<int>();
            info.BoardShortReplyID = boardShortReplyID.ToNumber<int>();
            info.Contents = contents;
            info.WriteUserID = CurrentUserInfo.UserID;

            DAL.BoardShortReply biz = new DAL.BoardShortReply();
            return biz.Mod(info);
        }

        #endregion

        #endregion
    }
}
