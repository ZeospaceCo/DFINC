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
    /// ShortReply의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReply : Base.BaseWSPMS
    {

        #region ADD()

        #region 댓글

        /// <summary>
        /// 짧은댓글 ADD
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddShortReply(string p)
        {
            SetDecryptString(p);

            int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
            
            string contents = GetDecryptParam("contents", "");
            //contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);

            BE.BoardShortReplyInfo info = new BE.BoardShortReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ProjectID = projectID;
            info.MenuID = menuID;
            info.BoardID = boardID;
            info.Contents = Server.UrlDecode(contents).ToReverseXSS();
            info.WriteUserID = CurrentUserInfo.UserID;            

            DAL.BoardShortReply dal = new DAL.BoardShortReply();
            return dal.Add(info);
        }

        #endregion

        #endregion
    }
}
