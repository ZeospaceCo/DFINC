using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Admin.General.Common
{
    /// <summary>
    /// GetGrade의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetGrade : Base.BaseWSCommunity
    {

        #region MOD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-31
        /// 작성목적 : 회원등급명 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModGrade(string p)
        {
            try
            {
                SetDecryptString(p);

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int groupID = GetDecryptParam("groupID", "0").ToNumber<int>();
                string nameBase = GetDecryptParam("nameBase", "");
                string nameKo = GetDecryptParam("nameKo", "");
                string nameEn = GetDecryptParam("nameEn", "");
                string nameJa = GetDecryptParam("nameJa", "");
                string nameFr = GetDecryptParam("nameFr", "");
                string nameZhcn = GetDecryptParam("nameZhcn", "");
                string nameZhtw = GetDecryptParam("nameZhtw", "");

                DAL.UserGroup dal = new DAL.UserGroup();
                dal.Mod(CurrentUserInfo.CompanyID, communityID, groupID, nameBase, nameKo, nameEn, nameJa, nameFr, nameZhcn, nameZhtw);
                return true;
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
