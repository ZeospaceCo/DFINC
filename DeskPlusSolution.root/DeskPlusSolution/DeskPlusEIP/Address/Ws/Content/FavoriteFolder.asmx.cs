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
using Zeospace.DeskPlus.Address.BE;

namespace Zeospace.DeskPlus.Address.Web.Ws.Content
{
    /// <summary>
    /// FavoriteFolder의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class FavoriteFolder : Base.BaseWSAddress
    {

        /// <summary>
        /// 즐겨찾기 그룹 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>        
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddFavoCategory(string p)
        {
            try
            {
                SetDecryptString(p);
                int ParentcategoryID = GetDecryptParam("parentCategoryID", "0").ToNumber<int>();
                int FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                string Types = GetDecryptParam("types", "");

                if (ParentcategoryID < 0 || FolderID < 0 || Types == "")
                    return false;

                BE.FavorCategoryInfo info = new BE.FavorCategoryInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.ParentCategoryID = ParentcategoryID;
                info.UserID = CurrentUserInfo.UserID;
                info.FolderID = FolderID;
                info.Types = Types;

                DAL.FavorCategory dal = new DAL.FavorCategory();

                if (dal.Add(info) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddFavoCategory : " + ex.Message);
                throw;
            }
        }
    }
}
