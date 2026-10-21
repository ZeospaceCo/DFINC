using System.Web.Services;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
{
    /// <summary>
    /// ShortReply의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class ShortReply : Base.BaseWSBoard
    {
        #region ADD()

        #region 댓글

        /// <summary>
        /// 댓글 ADD
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddShortReply(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string contents = GetDecryptParam("contents", "");
            //contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            BE.ShortReplyInfo info = new BE.ShortReplyInfo();

            info.CompanyID = boaComID;
            info.CategoryID = categoryID.ToNumber<int>();
            info.ListID = listID.ToNumber<int>();
            info.ShortReplyID = 0;
            info.ParentID = 0;
            info.GroupID = 0;
            info.ListOrder = 0;
            info.Depth = 0;
            info.Content = Server.UrlDecode(contents);
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;

            BLL.ShortReply dal = new BLL.ShortReply();
            dal.Add(info, CurrentUserInfo.PreferredLanguage);

            return 1;
        }

        /// <summary>
        /// 대댓글 ADD
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddShortReplyReplys(string p)
        {

            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string shortReplyID = GetDecryptParam("shortReplyID", "0");
            string contents = GetDecryptParam("contents", "");
            //contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            BE.ShortReplyInfo info = new BE.ShortReplyInfo();
            DAL.ShortReply dalShort = new DAL.ShortReply();

            BE.ShortReplyInfo shortinfo = dalShort.GetShortReplyListID(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>(), shortReplyID.ToNumber<int>());

            info.CompanyID = boaComID;
            info.CategoryID = categoryID.ToNumber<int>();
            info.ListID = listID.ToNumber<int>();
            info.ShortReplyID = shortReplyID.ToNumber<int>();
            info.ParentID = shortReplyID.ToNumber<int>();
            info.GroupID = shortinfo.GroupID;
            info.ListOrder = shortinfo.ListOrder + 1;
            info.Depth = shortinfo.Depth + 1;
            info.Content = Server.UrlDecode(contents);
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;

            BLL.ShortReply dal = new BLL.ShortReply();
            dal.Add(info, CurrentUserInfo.PreferredLanguage);

            return 1;
        }

        #endregion 댓글

        #endregion ADD()
    }
}