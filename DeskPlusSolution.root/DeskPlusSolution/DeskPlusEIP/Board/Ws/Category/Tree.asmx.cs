using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.Web.Ws.Category
{
    /// <summary>
    /// Tree의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Tree : Base.BaseWSBoard
    {
        #region 사용자 카테고리 정보

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

            int categoryID = GetDecryptParam("catID", "0").ToNumber<Int32>();
            string types = GetDecryptParam("types", "BL");//BL : 게시판 TL : 임시저장 IL : 중요표시

            DAL.CategoryUser dal = new DAL.CategoryUser();
            List<CategoryUserInfo> al = dal.GetList(CurrentUserInfo.CompanyID, categoryID,
                CurrentUserInfo.UserID, types);

            foreach (BE.CategoryUserInfo info in al)
            {
                return info.ListType + ":" + info.PageSize;
            }

            return "0:10";
        }

        #endregion 사용자 카테고리 정보
    }
}