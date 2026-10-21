using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;

namespace Zeospace.DeskPlus.Coop.Web.Ws.Admin.Category
{
    /// <summary>
    /// MoveSubUpdown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MoveSubUpdown : Base.BaseWSCoop
    {

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 소분류 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderItemSub(string p)
        {
            SetDecryptString(p);
            string itemID = GetDecryptParam("itemID", "0");
            string sortItemSubList = GetDecryptParam("sortItemSubList", ""); //소분류 아이디값            
            sortItemSubList = sortItemSubList.TrimEnd(',');

            DAL.ItemSub dal = new DAL.ItemSub();
            return dal.ModOrderItemSub(CurrentUserInfo.CompanyID, itemID.ToNumber<int>(), sortItemSubList);
        }
    }
}
