using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// ShortReplyMod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyMod : Base.BaseWSReport
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

            BE.ShortReplyInfo info = new BE.ShortReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ListID = listID.ToNumber<int>();
            info.Content = contents.ToReverseXSS();
            info.ShortReplyID = shortReplyID.ToNumber<int>();
            info.UserID = CurrentUserInfo.UserID;

            DAL.ShortReply biz = new DAL.ShortReply();
            return biz.Mod(info);
        }

        #endregion 댓글

        #endregion MOD()
    }
}