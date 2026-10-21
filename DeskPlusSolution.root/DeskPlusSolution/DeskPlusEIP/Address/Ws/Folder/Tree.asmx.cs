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

namespace Zeospace.DeskPlus.Address.Web.Ws.Folder
{
    /// <summary>
    /// Tree의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Tree : Base.BaseWSAddress
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 카테고리 사용자별 정보 가져오기(ListType:PageSize)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCategoryUser(string p)
        {
            SetDecryptString(p);

            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<Int32>();
            string types = GetDecryptParam("types", "");

            BE.CategoryUserInfo info = new CategoryUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.CategoryID = categoryID;
            info.Types = types;

            DAL.CategoryUser dal = new DAL.CategoryUser();
            List<CategoryUserInfo> al = dal.GetCategoryUserLists(info);

            foreach (BE.CategoryUserInfo userInfo in al)
            {
                return userInfo.ListType + ":" + userInfo.PageSize;
            }

            return "1:10";
        }


        /// <summary>
        /// 작성자 : shine
        /// 개인주소록의 선택한 그룹의 하위 그룹 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPrivateCategoryCount(string p)
        {
            SetDecryptString(p);
            int CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            DAL.PrivateCategory category = new DAL.PrivateCategory();
            return category.GetPrivateCategoryPCount(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID);
        }


        /// <summary>
        /// 작성자 : shine
        /// 즐겨찾기의 선택한 그룹의 하위 그룹 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetFavorCategoryCount(string p)
        {
            SetDecryptString(p);
            int CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            DAL.FavorCategory category = new DAL.FavorCategory();
            return category.GetFavoCategoryPCount(CurrentUserInfo.CompanyID, CategoryID, CurrentUserInfo.UserID);
        }
    }
}
