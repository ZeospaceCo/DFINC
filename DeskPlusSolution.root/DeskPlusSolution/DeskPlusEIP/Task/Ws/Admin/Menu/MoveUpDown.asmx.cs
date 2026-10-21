using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.Task.Web.Ws.Admin.Menu
{
    /// <summary>
    /// MoveUpDown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MoveUpDown : Base.BaseWSTask
    {
        #region Mod
        /// <summary>
        /// 대분류 순서 변경
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ItemOrder(string p)
        {
            SetDecryptString(p);
              string sortItemList = GetDecryptParam("sortItemList", ""); //대분류 아이디값            
                sortItemList = sortItemList.TrimEnd(',');
           

            DAL.Item item = new DAL.Item();

            return item.ItemOrder(CurrentUserInfo.CompanyID, sortItemList); 


        }
        #endregion
       
    }
}
