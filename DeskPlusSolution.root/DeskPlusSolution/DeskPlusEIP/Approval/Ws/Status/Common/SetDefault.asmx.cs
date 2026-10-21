using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Status.Common
{
    /// <summary>
    /// SetDefault의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetDefault : Base.BaseWSApproval
    {

        /// <summary>
        /// 초기메뉴로 설정(저장)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public void SetDefaultMenu(string p)
        {           
            try
            {
                SetDecryptString(p);
                string folderStatusID = GetDecryptParam("folderStatusID", "");
              
                Account.DAL.UserEnvironment dal = new Account.DAL.UserEnvironment();
                dal.ApprovalStatusMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderStatusID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
