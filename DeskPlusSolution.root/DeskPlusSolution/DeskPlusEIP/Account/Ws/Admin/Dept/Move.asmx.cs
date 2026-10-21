using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.BE;
namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.Dept
{
    /// <summary>
    /// Move의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Move : Base.BaseWSAccount
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 상위 부서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModParentDeptID(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            int parentDeptID = GetDecryptParam("parentDeptID", "0").ToNumber<int>();

            Zeospace.DeskPlus.Account.DAL.Dept dal = new DAL.Dept();
            return dal.ModParentDeptID(companyID, deptID, parentDeptID);
        }


        /// <summary>        
        /// 작성자 : 심재운
        /// 용도 : 부서 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModDeptOrder(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>(); //카테고리값            
            string sortCatList = GetDecryptParam("sortCatList", ""); //카테고리값            
            sortCatList = sortCatList.TrimEnd(',');

            Zeospace.DeskPlus.Account.DAL.Dept dal = new DAL.Dept();
            return dal.ModOrder(companyID, sortCatList);
        }
    }
}
