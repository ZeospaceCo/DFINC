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
    /// MoveUpdown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MoveUpdown : Base.BaseWSCoop
    {

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 대분류 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderItem(string p)
        {
            SetDecryptString(p);
            string sortItemList = GetDecryptParam("sortItemList", ""); //대분류 아이디값            
            sortItemList = sortItemList.TrimEnd(',');

            DAL.Item dal = new DAL.Item();
            return dal.ModOrderItem(CurrentUserInfo.CompanyID, sortItemList);
        }
    }
}
