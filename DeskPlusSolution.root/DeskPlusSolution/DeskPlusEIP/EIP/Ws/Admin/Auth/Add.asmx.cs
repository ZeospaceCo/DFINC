using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Interop.ZeoLicEp;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Auth
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSEIP
    {

        /// <summary>
        /// 권한 코드 카운트
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetRoleCountByRoleCode(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                string roleCode = GetDecryptParam("roleCode", "");
                string gubun = GetDecryptParam("gubun", "");

                Zeospace.DeskPlus.EIP.DAL.Role dal = new DAL.Role();
                List<RoleInfo> al = dal.GetRole(companyID, roleCode, gubun);

                return al.Count();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetRoleCountByRoleCode : " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// codeid 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetSysCodeCountByCodeID(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                string parentCodeID = GetDecryptParam("parentCodeID", "0");
                string codeID = GetDecryptParam("codeID", "");

                Zeospace.DeskPlus.Common.DAL.SYSCode biz = new DeskPlus.Common.DAL.SYSCode();
                return biz.GetCountByCodeID(companyID, parentCodeID, codeID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSysCodeCountByCodeID : " + ex.Message);
                return 0;
            }
        }


        /// <summary>
        /// ROLE 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddRole(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string roleCode = GetDecryptParam("roleCode", "");
            string roleName = GetDecryptParam("roleName", "");
            string roleDesc = GetDecryptParam("roleDesc", "");
            string gubun = GetDecryptParam("gubun", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    Zeospace.DeskPlus.EIP.BE.RoleInfo info = new BE.RoleInfo();
                    info.CompanyID = companyID;
                    info.RoleCode = roleCode;
                    info.RoleName = roleName;
                    info.RoleDesc = roleDesc;
                    info.Gubun = gubun;

                    Zeospace.DeskPlus.EIP.DAL.Role dal = new DAL.Role();
                    dal.Add(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddRole : " + ex);
                return false;
            }
        }


        /// <summary>
        /// ROLE 수정하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModRole(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int roleID = GetDecryptParam("roleID", "0").ToNumber<int>();
            string roleCode = GetDecryptParam("roleCode", "");
            string roleName = GetDecryptParam("roleName", "");
            string roleDesc = GetDecryptParam("roleDesc", "");
            string gubun = GetDecryptParam("gubun", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    Zeospace.DeskPlus.EIP.BE.RoleInfo info = new BE.RoleInfo();
                    info.CompanyID = companyID;
                    info.RoleID = roleID;
                    info.RoleCode = roleCode;
                    info.RoleName = roleName;
                    info.RoleDesc = roleDesc;
                    info.Gubun = gubun;

                    Zeospace.DeskPlus.EIP.DAL.Role dal = new DAL.Role();
                    dal.Mod(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModRole : " + ex);
                return false;
            }
        }
    }
}
