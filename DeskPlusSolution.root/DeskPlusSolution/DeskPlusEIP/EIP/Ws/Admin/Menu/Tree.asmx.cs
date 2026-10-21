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

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Menu
{
    /// <summary>
    /// Tree의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Tree : Base.BaseWSEIP
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 메뉴 및 권한, IP 들 초기화하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ResetMenuList(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int userID = GetDecryptParam("userID", "0").ToNumber<int>();

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                //전체 메뉴, 데이터연동, IP 초기화 하기
                Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new Zeospace.DeskPlus.EIP.BLL.Menu();
                bizMenu.ResetMenuAuth(companyID);

                HttpContext context = HttpContext.Current;
                string[] remoteAddrStr = context.Request.UserHostAddress.Split('.');
                string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

                //사용자 메뉴별 ROLE 및 IP 재설정                
                bizMenu.GetMenuListUser(companyID, userID, remoteAddr.ToDouble<double>());

                return true;
            }

            return false;
        }


        /// <summary>
        /// 메뉴 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string DelMenu(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.Menu dal = new DAL.Menu();
                    DataSet ds = dal.GetMenuByPID(companyID, menuID.ToString());
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0) return "0";
                    }

                    dal.Del(companyID, menuID);

                    return "1";
                }

                return "-1";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelMenu : " + ex);
                return "-2";
            }
        }

        /// <summary>
        /// 용도 : 메뉴아이콘 설정 및 메뉴 및 권한, IP 들 초기화하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool IconSetting(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int userID = GetDecryptParam("userID", "0").ToNumber<int>();
            string iconYN = GetDecryptParam("iconYN", "N");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                DAL.Menu dal = new DAL.Menu();
                dal.ModMenuIcon(companyID, iconYN);

                //전체 메뉴, 데이터연동, IP 초기화 하기
                Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new Zeospace.DeskPlus.EIP.BLL.Menu();
                bizMenu.ResetMenuAuth(companyID);

                HttpContext context = HttpContext.Current;
                string[] remoteAddrStr = context.Request.UserHostAddress.Split('.');
                string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

                //사용자 메뉴별 ROLE 및 IP 재설정                
                bizMenu.GetMenuListUser(companyID, userID, remoteAddr.ToDouble<double>());

                return true;
            }

            return false;
        }
    }
}
