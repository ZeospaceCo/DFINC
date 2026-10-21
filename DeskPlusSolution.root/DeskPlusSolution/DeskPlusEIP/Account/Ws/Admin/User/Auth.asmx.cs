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

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.User
{
    /// <summary>
    /// Auth의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Auth : Base.BaseWSAccount
    {

        /// <summary>
        /// 목적 : 메일 중복 체크
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<EIP.BE.UserRoleInfo> GetMyUserRoleList(string p)
        {
            try
            {

                this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Ws.Admin.User.Auth).Assembly;	//어셈블리 선언

                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>(); //회사 아이디
                int userID = GetDecryptParam("userID", "0").ToNumber<int>(); //사용자 아이디
                int deptID = GetDecryptParam("deptID", "0").ToNumber<int>(); //부서 아이디            

                List<EIP.BE.UserRoleInfo> al = new List<EIP.BE.UserRoleInfo>();

                if (companyID > 0 && userID > 0 && deptID > 0)
                {
                    Zeospace.DeskPlus.EIP.DAL.UserRole dal = new EIP.DAL.UserRole();
                    DataTableReader dtr = dal.GetUserRoleByUser(companyID, deptID, userID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                EIP.BE.UserRoleInfo info = new EIP.BE.UserRoleInfo();
                                info.CompanyID = dtr["RoleName"].ToString().ToNumber<int>();
                                info.RoleID = dtr["RoleID"].ToString().ToNumber<int>();
                                info.RoleName = dtr["RoleName"].ToString();
                                info.NameBase = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                    dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                                info.Types = dtr["Types"].ToString();

                                if (info.Types.Equals("GG")) info.Types = GetResource("Private_AccountInfo_ltUserDept"); // "부서";

                                if (info.Types.Equals("GM")) info.Types = GetResource("User"); //"사용자";

                                al.Add(info);
                            }
                        }
                    }
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMyUserRoleList : " + ex.Message);
                return null;
            }
        }
    }
}
