using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.Education.Web.Ws.Admin
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSEducation
    {
        #region 공지첨부 삭제

        /// <summary>
        /// 수정 가능한 첨부 기록과 실제 파일을 즉시 삭제합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAttachFile(string p)
        {
            Base.CompletionCancelRequest.Validate(Context.Request);
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0 || CurrentUserInfo.UserID <= 0
                || (!CurrentUserInfo.IsSysAdmin && !CurrentUserInfo.IsInRole("admin")
                    && !CurrentUserInfo.IsInRole("Education")))
                throw new HttpException(403, "공지첨부을 삭제할 권한이 없습니다.");

            SetDecryptString(p);
            int listID = Base.CompletionCancelRequest.ReadPositiveID(GetDecryptParam("listID", ""),
                "삭제할 정보가 올바르지 않습니다.");
            int attachID = Base.CompletionCancelRequest.ReadPositiveID(GetDecryptParam("attachID", ""),
                "삭제할 공지첨부 정보가 올바르지 않습니다.");

            DAL.List list = new DAL.List();
            int modifyState = list.GetModifyState(CurrentUserInfo.CompanyID, listID);
            if (modifyState != 1 && modifyState != 2) throw new HttpException(404, "조회할 수 없는 내역입니다.");
            return new BLL.Attach().DelForModify(CurrentUserInfo.CompanyID, listID, attachID);
        }

        #endregion 공지첨부 삭제
    }
}
