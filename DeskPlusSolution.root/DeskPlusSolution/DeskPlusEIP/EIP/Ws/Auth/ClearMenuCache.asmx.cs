using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Auth
{
    /// <summary>
    /// ClearMenuCache의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ClearMenuCache : System.Web.Services.WebService
    {
        /// <summary>
        /// 인사 추가 후, 메뉴별 사용자 케쉬 삭제하기
        /// </summary>
        /// <param name="companID"></param>
        /// <returns></returns>
        [WebMethod]
        public bool ResetMenuCache(int companID)
        {
            try
            {
                Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                bizMenu.ClearCacheMenuUser(companID);
                //사용자별 메뉴 정보 - 캐쉬설정
                bizMenu.SetCacheMenuListUser(companID);


                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ResetMenuCache : " + ex);
                throw;
            }
        }
    }
}
