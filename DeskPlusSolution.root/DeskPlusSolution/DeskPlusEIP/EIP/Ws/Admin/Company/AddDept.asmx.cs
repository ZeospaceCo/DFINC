using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Company
{
    /// <summary>
    /// AddDept의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddDept : Base.BaseWSEIP
    {

        /// <summary>
        /// 회사 부서 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            string deptName = GetDecryptParam("deptName", "");            
            string shortName = GetDecryptParam("shortName", "");
            string deptCode = GetDecryptParam("deptCode", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin)
                {
                    Zeospace.DeskPlus.Account.BE.DeptInfo info = new Account.BE.DeptInfo();
                    info.CompanyID = companyID;
                    info.DeptID = 0;
                    info.ParentDeptID = deptID;
                    info.NameBase = deptName;
                    info.ShortName = shortName;
                    info.DeptCode = deptCode;
                    info.ActiveDept = "1";

                    Zeospace.DeskPlus.Account.DAL.Dept dal = new Account.DAL.Dept();
                    dal.Add(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return false;
            }
        } 
    }
}
