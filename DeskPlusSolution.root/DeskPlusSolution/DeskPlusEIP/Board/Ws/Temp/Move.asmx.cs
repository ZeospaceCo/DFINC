using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Temp
{
    /// <summary>
    /// Move의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Move : Base.BaseWSBoard
    {
        #region ADD()

        #region 임시글 이동하기

        /// <summary>
        /// 임시글을 복사하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveMyTemp(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");

            BLL.MyTemplate bll = new BLL.MyTemplate();
            bll.Move(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>(),
                CurrentUserInfo.DeptID, CurrentUserInfo.UserID, CurrentUserInfo.LogInID,
                CurrentUserInfo.Name, CurrentUserInfo.MobilePhone, PreLang);

            return true;
        }

        #endregion 임시글 이동하기

        #endregion ADD()
    }
}