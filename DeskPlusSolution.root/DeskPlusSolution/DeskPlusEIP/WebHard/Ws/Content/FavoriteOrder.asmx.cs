using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Content
{
    /// <summary>
    /// FavoriteOrder의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class FavoriteOrder : Base.BaseWSWebHard
    {

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-27
        /// 작성목적 : 즐겨찾기 폴더 순서 변경하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderFavorite(string p)
        {
            SetDecryptString(p);
            string sortFolList = GetDecryptParam("sortFolList", ""); //카테고리값            
            sortFolList = sortFolList.TrimEnd(',');

            DAL.Favorite dal = new DAL.Favorite();
            return dal.ModOrderFavorite(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, sortFolList);

        }

        #endregion 
    }
}
