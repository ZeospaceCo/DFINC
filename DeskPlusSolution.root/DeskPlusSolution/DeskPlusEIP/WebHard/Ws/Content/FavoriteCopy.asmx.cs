using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Content
{
    /// <summary>
    /// FavoriteCopy의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class FavoriteCopy : Base.BaseWSWebHard
    {

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-21
        /// 작성목적 : 즐겨 찾기 추가 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddFavoriteFolder(string p)
        {
            SetDecryptString(p);

            string parentFavoriteID = GetDecryptParam("parentFavoriteID", "0");
            string folderID = GetDecryptParam("folderID", "0");
            string types = GetDecryptParam("types", "0");

            DAL.Favorite dal = new DAL.Favorite();
            return dal.AddFavoriteFolder(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                folderID.ToNumber<int>(), parentFavoriteID.ToNumber<int>(), types);

        }

        #endregion 
    }
}
