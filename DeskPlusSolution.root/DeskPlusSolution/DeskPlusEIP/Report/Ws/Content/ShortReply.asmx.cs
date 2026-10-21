using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// ShortReply의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class ShortReply : Base.BaseWSReport
    {
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
            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Ws.Content.ShortReply).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);
            string listID = GetDecryptParam("listID", "0");
            string contents = GetDecryptParam("contents", "");
            //contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);

            string preTitle = "[" + GetResource("Reply") + "]" + CurrentUserInfo.DeptName + " " + CurrentUserInfo.Name + GetResource("ReplyScript");

            BE.ShortReplyInfo info = new BE.ShortReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ListID = listID.ToNumber<int>();
            info.Content = Server.UrlDecode(contents).ToReverseXSS();
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;

            BLL.ShortReply dal = new BLL.ShortReply();
            dal.Add(info, CurrentUserInfo.PreferredLanguage, preTitle);

            return 1;
        }

        #endregion 댓글
    }
}