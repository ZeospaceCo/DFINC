using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.Task.Web.Ws.Admin.Menu
{
    /// <summary>
    /// MoveSubUpdown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MoveSubUpdown : Base.BaseWSTask
    {
        #region Mod
        /// <summary>
        /// 소분류 순서 변경
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ItemSubOrder(string p)
        {
            SetDecryptString(p);
            string sortItemSubList = GetDecryptParam("sortItemSubList", ""); //소분류 아이디값            
            sortItemSubList = sortItemSubList.TrimEnd(',');

            DAL.ItemSub itemSub = new DAL.ItemSub();

            return itemSub.ItemSubUpdown(CurrentUserInfo.CompanyID, sortItemSubList);


        }
        #endregion

    }
}
