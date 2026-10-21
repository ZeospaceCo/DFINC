using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
{
    /// <summary>
    /// ShortReplyMod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyMod : Base.BaseWSBoard
    {
        #region MOD()

        #region 댓글

        /// <summary>
        /// 댓글 Mod
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModShortReple(string p)
        {
            //contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);

            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string contents = GetDecryptParam("contents", "");
            string shortReplyID = GetDecryptParam("shortReplyID", "0");

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            BE.ShortReplyInfo info = new BE.ShortReplyInfo
            {
                CompanyID = boaComID,
                CategoryID = categoryID.ToNumber<int>(),
                ListID = listID.ToNumber<int>(),
                Content = Server.UrlDecode(contents),
                ShortReplyID = shortReplyID.ToNumber<int>(),
                UserID = CurrentUserInfo.UserID
            };

            DAL.ShortReply biz = new DAL.ShortReply();
            return biz.Mod(info);
        }

        #endregion 댓글

        #endregion MOD()
    }
}