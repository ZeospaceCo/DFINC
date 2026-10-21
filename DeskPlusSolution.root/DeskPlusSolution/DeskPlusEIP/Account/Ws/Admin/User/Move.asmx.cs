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
        /// 용도 : 다중 사용자들 기본부서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModBaseDept(string p)
        {
            try
            {

                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int deptID = GetDecryptParam("deptID", "0").ToNumber<int>();
                string userList = GetDecryptParam("userList", "0");

                if (companyID == 0 || deptID == 0 || userList.Length == 0) return false;

                Zeospace.DeskPlus.Account.DAL.User dal = new DAL.User();
                dal.ModBaseDept(companyID, deptID, userList);

                //제타 6.5 자동완성기능 부서변경API
                //ep관리자 > 사용자 관리 > 리스트에서 체크박스로 체크 후 부서변경 클릭시...
                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                if (mailProduct.ToUpper() == "ZETTA65")
                {
                    string email = "";

                    Zeospace.DeskPlus.Account.DAL.User dalUser = new DAL.User();
                    List<Zeospace.DeskPlus.Account.BE.UserInfo> be = dalUser.GetUserInfo(companyID, userList.ToNumber<int>(), true, true);

                    foreach (Zeospace.DeskPlus.Account.BE.UserInfo userinfo in be)
                    {
                        Zeospace.DeskPlus.Common.BLL.MailZettaFactory biz = new DeskPlus.Common.BLL.MailZettaFactory();
                        string sendParams = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "Zetta65_ModDeptUser", "");

                        email = userinfo.Mail;

                        sendParams = string.Format(sendParams, email, deptID);

                        biz.SendPost(sendParams);

                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("사용자부서변경 sendParams =" + sendParams, AppSettingType.MAIL);
                    }


                }


                Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                bizMenu.ClearCacheMenuUser(companyID);
                //사용자별 메뉴 정보 - 캐쉬설정
                bizMenu.SetCacheMenuListUser(companyID);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModBaseDept : " + ex.Message);
                return false;
            }
        }
    }
}
