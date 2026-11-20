using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.Education.Web.Ws.Content
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSEducation
    {
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public BE.CompletionCancelResult CancelComplete(string p)
        {
            Base.CompletionCancelRequest.Validate(Context.Request);
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0 || CurrentUserInfo.UserID <= 0)
                throw new HttpException(403, "로그인한 사용자의 내역만 완료 취소할 수 있습니다.");

            SetDecryptString(p);
            int listID = Base.CompletionCancelRequest.ReadPositiveID(GetDecryptParam("listID", ""),
                "완료 취소할 정보가 올바르지 않습니다.");
            // 다른 사용자 ID를 요청해도 취소 대상은 현재 로그인 사용자로만 고정합니다.
            return new BLL.TOAttach().CancelComplete(CurrentUserInfo.CompanyID, listID, CurrentUserInfo.UserID);
        }
    }
}
