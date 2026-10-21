using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Preference.Setting
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSApproval
    {
        /// <summary>
        /// 전자결재 : 승인 시, 비밀번호 사용유무 설정하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Mod(string p)
        {
            try
            {
                SetDecryptString(p);

                string ispwd = GetDecryptParam("ispwd", "N");
                string approvalStatusMenuType = GetDecryptParam("approvalStatusMenuType", "OLD");

                Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Account.DAL.UserEnvironment();
                bool rtnval = dal.ModIsApprovePwd(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, ispwd);
                rtnval = rtnval && dal.ModApprovalStatusMenuType(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, approvalStatusMenuType);
                
                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
