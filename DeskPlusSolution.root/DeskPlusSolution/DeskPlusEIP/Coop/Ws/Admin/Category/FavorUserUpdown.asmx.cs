using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.Web.Ws.Admin.Category
{
    /// <summary>
    /// FavorUser의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class FavorUserUpdown : Base.BaseWSCoop
    {

        /// <summary>
        /// 작성자 : shimpark
        /// 용도 : 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrder(string p)
        {
            SetDecryptString(p);

            string sortItemList = GetDecryptParam("sortItemList", ""); //대분류 아이디값            
            sortItemList = sortItemList.TrimEnd(',');

            if (sortItemList.Length == 0) return true;

            DAL.ItemFavUser dal = new DAL.ItemFavUser();
            return dal.ModOrder(CurrentUserInfo.CompanyID, sortItemList);
        }


        /// <summary>
        /// 작성자 : shimpark
        /// 용도 : 자주 지정자 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            SetDecryptString(p);

            int itemSubID = GetDecryptParam("itemSubID", "0").ToNumber<int>();
            string userList = GetDecryptParam("userList", "");

            userList = userList.TrimEnd(',');

            if (userList.Length == 0) return true;

            DAL.ItemFavUser dal = new DAL.ItemFavUser();
            BE.ItemFavUserInfo be = new BE.ItemFavUserInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ItemSubID = itemSubID;
            be.WriteUserID = CurrentUserInfo.UserID;

            return dal.Add(be, userList);
        }
    }
}
