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
                BLL.MenuDataLink bizDataLink = new BLL.MenuDataLink();
                BLL.MenuIP bizMenuIP = new BLL.MenuIP();
                BLL.Menu bizMenu = new BLL.Menu();

                #region 캐쉬 삭제
                //데이터연동 정보 - 캐쉬 초기화
                bizDataLink.ClearCacheMenuDataLink(companyID);

                //메뉴별 IP 정보 - 캐쉬 초기화
                bizMenuIP.ClearCacheMenuIP(companyID);

                //메뉴 - 캐쉬 초기화
                bizMenu.ClearCacheMenu(companyID);

                //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                bizMenu.ClearCacheMenuUser(companyID); 
                #endregion

                #region 캐쉬 설정

                //데이터연동 정보 - 캐쉬설정
                bizDataLink.SetCacheMenuDataLink(companyID);

                //메뉴별 IP 정보 - 캐쉬설정
                bizMenuIP.SetCacheMenuIP(companyID);

                //메뉴 - 캐쉬설정
                bizMenu.SetCacheMenuList(companyID);

                //사용자별 메뉴 정보 - 캐쉬설정
                bizMenu.SetCacheMenuListUser(companyID);

                #endregion

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
        /// 작성자 : 심재운
        /// 용도 : role 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelRole(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int roleID = GetDecryptParam("roleID", "0").ToNumber<int>();
            string gubun = GetDecryptParam("gubun", "");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                Zeospace.DeskPlus.EIP.DAL.Role dal = new DAL.Role();
                return dal.Del(companyID, roleID, gubun);
            }

            return false;
        }
    }
}
