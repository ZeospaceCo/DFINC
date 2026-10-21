using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Code
{
    /// <summary>
    /// MoveUpdown의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MoveUpdown : Base.BaseWSEIP
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : syscode 순서 정렬 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderSysCode(string p)
        {
            SetDecryptString(p);

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                string sortList = GetDecryptParam("sortList", ""); //카테고리값            
                sortList = sortList.TrimEnd(',');

                Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
                return dal.ModOrder(CurrentUserInfo.CompanyID, sortList);
            }

            return false;

        }
    }
}
