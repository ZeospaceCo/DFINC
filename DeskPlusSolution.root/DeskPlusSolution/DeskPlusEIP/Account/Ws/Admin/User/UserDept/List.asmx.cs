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

namespace Zeospace.DeskPlus.Account.Web.Ws.Admin.User.UserDept
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSAccount
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사용자의 부서 할당하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddUserDept(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
                int userID = GetDecryptParam("userID", "").ToNumber<int>();
                string deptList = GetDecryptParam("deptList", "");
                string titleDutyList = GetDecryptParam("titleDutyList", "");

                if (deptList.Length > 0)
                {
                    string[] deptStr = deptList.Split(',');
                    string[] titleDutyStr = titleDutyList.Split('$');

                    List<BE.UserDeptInfo> al = new List<BE.UserDeptInfo>();

                    for (int i = 0; i < deptStr.Length; i++)
                    {
                        BE.UserDeptInfo be = new BE.UserDeptInfo();
                        be.CompanyID = companyID;
                        be.UserID = userID;
                        be.DeptID = deptStr[i].ToNumber<int>();
                        be.CodeTitleID = titleDutyStr[i].Split(',')[0].ToNumber<int>();
                        be.CodeDutyID = titleDutyStr[i].Split(',')[1].ToNumber<int>();
                        be.BaseDept = titleDutyStr[i].Split(',')[2];
                        be.WriteUserID = CurrentUserInfo.UserID;

                        al.Add(be);
                    }

                    BLL.UserDept userDeptBiz = new BLL.UserDept();
                    userDeptBiz.AddUserDept(al);

                    Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                    //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                    bizMenu.ClearCacheMenuUser(companyID);
                    //사용자별 메뉴 정보 - 캐쉬설정
                    bizMenu.SetCacheMenuListUser(companyID);


                    #region 메일 업체 전송 (메일의 자동화 완성을 위해)
                    string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");

                    if (mailProduct.ToUpper() == "ZETTA65")
                    {
                        DAL.User dalUser = new DAL.User();
                        List<UserInfo> userList = dalUser.GetUserInfo(companyID, userID, false, false);
                        if (userList.Count > 0)
                        {
                            Zeospace.DeskPlus.Common.BLL.MailZettaFactory comBiz = new DeskPlus.Common.BLL.MailZettaFactory();
                            comBiz.ChangeDeptUser(userList[0].MailID + "@" + userList[0].MailDomain, userList[0].DeptID);
                        }
                    }

                    #endregion

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddUserDept : " + ex.Message);
                return false;
            }

        }
    }
}
