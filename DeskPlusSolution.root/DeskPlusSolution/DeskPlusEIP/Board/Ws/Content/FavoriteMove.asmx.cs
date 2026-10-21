using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
{
    /// <summary>
    /// FavoriteMove의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class FavoriteMove : Base.BaseWSBoard
    {
        #region MOD()

        #region 즐겨찾기

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 상위폴더 수정
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveFavorite(string p)
        {
            SetDecryptString(p);
            int favoriteID = GetDecryptParam("favoriteID", "0").ToNumber<int>(); //원본 카테고리
            int parentFavoriteID = GetDecryptParam("parentFavoriteID", "0").ToNumber<int>();//북사할 카테고리

            if (favoriteID > 0)
            {
                DAL.Favorite dal = new DAL.Favorite();
                return dal.ModParentFavorite(CurrentUserInfo.CompanyID, favoriteID, parentFavoriteID,
                    CurrentUserInfo.UserID);
            }
            else
            {
                return false;
            }
        }

        #endregion 즐겨찾기

        #endregion MOD()
    }
}