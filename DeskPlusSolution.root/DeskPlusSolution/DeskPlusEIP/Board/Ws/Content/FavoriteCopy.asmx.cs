using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
{
    /// <summary>
    /// FavoriteCopy의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class FavoriteCopy : Base.BaseWSBoard
    {
        #region ADD()

        #region 즐겨찾기

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 게시판 폴더를 즐겨찾기에 넣기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddFavoriteCategory(string p)
        {
            SetDecryptString(p);

            string parentFavoriteID = GetDecryptParam("parentFavoriteID", "0");
            string categoryID = GetDecryptParam("categoryID", "0");

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            DAL.Favorite dal = new DAL.Favorite();
            return dal.AddFavoriteCategory(boaComID, CurrentUserInfo.UserID,
                categoryID.ToNumber<int>(), parentFavoriteID.ToNumber<int>());
        }

        #endregion 즐겨찾기

        #endregion ADD()
    }
}