using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Admin.Category
{
    /// <summary>
    /// Move의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Move : Base.BaseWSBoard
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시판 폴더 이동
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveCategory(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>(); //원본 카테고리
            int toCategoryID = GetDecryptParam("toCategoryID", "0").ToNumber<int>();//북사할 카테고리

            if (categoryID > 0)
            {
                int companyID = BLL.Category.GetCompanyCategoryInfo(categoryID);
                DAL.Category dal = new DAL.Category();
                return dal.ModMoveCategory(companyID, categoryID, toCategoryID);
            }
            else
            {
                return false;
            }
        }
    }
}