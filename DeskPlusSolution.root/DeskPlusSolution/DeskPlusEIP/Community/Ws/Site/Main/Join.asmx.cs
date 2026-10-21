using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Main
{
    /// <summary>
    /// Join의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Join : Base.BaseWSCommunity
    {
        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-23
        /// 작성목적 : 공개 커뮤니티가입하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddUserApply(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                string description = GetDecryptParam("description", "");
                string communityName = GetDecryptParam("communityName", "");
                int type = GetDecryptParam("type", "0").ToNumber<int>();
                int joinType = GetDecryptParam("joinType", "1").ToNumber<int>();

                BE.UserApplyInfo info = new UserApplyInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CommunityID = communityID;
                info.UserID = CurrentUserInfo.UserID;
                info.UserName = CurrentUserInfo.Name;
                info.Description = description;

                string usermail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
                BLL.UserApply bll = new BLL.UserApply();
                return bll.Add(info, communityName, PreLang, CurrentUserInfo.LogInID, type, joinType, usermail);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        #endregion
    }
}
