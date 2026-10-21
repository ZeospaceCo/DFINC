using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Admin.Category
{
    /// <summary>
    /// MoveUpdown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class MoveUpdown : Base.BaseWSBoard
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시판 폴더 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderCategory(string p)
        {
            SetDecryptString(p);
            string sortCatList = GetDecryptParam("sortCatList", ""); //카테고리값
            sortCatList = sortCatList.TrimEnd(',');

            if (sortCatList.Length == 0) return false;

            //여러개중 하나를 샘플로 가져가자. (통합게시판 여부)
            int companyID = BLL.Category.GetCompanyCategoryInfo(sortCatList.Split(',')[0].ToNumber<int>());
            DAL.Category dal = new DAL.Category();
            return dal.ModOrderCategory(companyID, sortCatList);
        }
    }
}