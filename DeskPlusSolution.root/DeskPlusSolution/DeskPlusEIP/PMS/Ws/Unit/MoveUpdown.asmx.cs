using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.PMS.Web.Ws.Unit
{
    /// <summary>
    /// MoveUpdown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MoveUpdown : Base.BaseWSPMS
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModDeptOrder(string p)
        {
            SetDecryptString(p);
            string projectID = GetDecryptParam("projectID", "0");
            string sortCatList = GetDecryptParam("sortCatList", "");
            sortCatList = sortCatList.TrimEnd(',');

            DAL.Unit dal = new DAL.Unit();
            return dal.ModOrder(CurrentUserInfo.CompanyID, projectID.ToNumber<int>(), sortCatList);

        }
    }
}
