using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
{
    /// <summary>
    /// Rate의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Rate : Base.BaseWSBoard
    {
        #region ADD()

        #region 댓글

        /// <summary>
        /// 작성자 : 심재운
        /// 투표 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddRating(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string rating = GetDecryptParam("rating", "");
            string contents = GetDecryptParam("contents", "");

            rating = rating.Replace("rating", "");

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            BE.RatingInfo info = new BE.RatingInfo();
            info.CompanyID = boaComID;
            info.CategoryID = categoryID.ToNumber<int>();
            info.ListID = listID.ToNumber<int>();
            info.RatingID = 0;
            info.Rating = rating.ToNumber<int>();
            info.WriteUserID = CurrentUserInfo.UserID;
            //info.WriteUserName = CurrentUserInfo.Name;
            info.Comments = contents;

            DAL.Ratings biz = new DAL.Ratings();
            return biz.Add(info);
        }

        #endregion 댓글

        #endregion ADD()
    }
}