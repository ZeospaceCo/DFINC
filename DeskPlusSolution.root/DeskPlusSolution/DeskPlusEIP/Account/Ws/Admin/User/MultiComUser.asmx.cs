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
    /// MultiComUser의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MultiComUser : Base.BaseWSAccount
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
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
            int userID = GetDecryptParam("userID", "").ToNumber<int>();
            int multiCompanyID = GetDecryptParam("multiCompanyID", "0").ToNumber<int>();
            string groupGUID = GetDecryptParam("groupGUID", "");
            string userList = GetDecryptParam("userList", "");
            string deptList = GetDecryptParam("deptList", "");            

            //group값이 없을 경우 하나 생성해 준다.
            if (groupGUID.Length == 0) groupGUID = System.Guid.NewGuid().ToString();

            BLL.UserCompany bizUserCom = new BLL.UserCompany();

            if (userList.Length > 0 && deptList.Length > 0)
            {
                string[] userStr = userList.Split(',');
                string[] deptStr = deptList.Split(',');

                /*
                 * 호출회사 + USERID  로 GroupGUID 를 얻는다.
                 * 타회사 + GroupGUID 호출로 타회사의 UserID 를 얻는다.
                 * 타회사 + 타회사 UserID 를 통해 기존부서 및 겸직부서의 정보를 oldUserDeptList 에 담는다.
                 */

                List<BE.UserDeptInfo> newUserDeptList = new List<BE.UserDeptInfo>();
                for (int i = 0; i < deptStr.Length; i++)
                {
                    BE.UserDeptInfo be = new BE.UserDeptInfo();
                    be.CompanyID = multiCompanyID;
                    be.UserID = userStr[i].ToNumber<int>();
                    be.DeptID = deptStr[i].ToNumber<int>();                    
                    be.WriteUserID = CurrentUserInfo.UserID;

                    newUserDeptList.Add(be);
                }

                
                bizUserCom.Add(companyID, userID, CurrentUserInfo.UserID, groupGUID, multiCompanyID, newUserDeptList);

                Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                bizMenu.ClearCacheMenuUser(companyID);
                //사용자별 메뉴 정보 - 캐쉬설정
                bizMenu.SetCacheMenuListUser(companyID);


                #region 메일 업체 전송 (메일의 자동화 완성을 위해)

                DAL.User dalUser = new DAL.User();
                List<UserInfo> alUserList = dalUser.GetUserInfo(companyID, userID, false, false);
                if (alUserList.Count > 0)
                {
                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory comBiz = new DeskPlus.Common.BLL.MailZettaFactory();
                    comBiz.ChangeDeptUser(alUserList[0].MailID + "@" + alUserList[0].MailDomain, alUserList[0].DeptID);
                }

                #endregion
            }
            else
            {
                //전부 삭제했을 경우, 매칭 테이블 삭제한다.
                DAL.UserCompany dalCom = new DAL.UserCompany();
                dalCom.Del(multiCompanyID, groupGUID);
            }

            return true;

        }
    }
}
