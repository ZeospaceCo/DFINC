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

namespace Zeospace.DeskPlus.Address.Web.Ws.Admin.Folder
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
        /// 즐겨찾기의 선택한 그룹의 하위 그룹 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCommonCategoryPCount(string p)
        {
            SetDecryptString(p);
            int CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            DAL.CommonCategory category = new DAL.CommonCategory();
            return category.GetCommonCategoryPCount(CurrentUserInfo.CompanyID, CategoryID);
        }
    }
}
