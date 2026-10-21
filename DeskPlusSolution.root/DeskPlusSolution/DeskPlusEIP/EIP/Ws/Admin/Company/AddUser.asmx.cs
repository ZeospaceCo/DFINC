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
    /// AddUser의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddUser : Base.BaseWSEIP
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
            string name = GetDecryptParam("name", "");
            string loginID = GetDecryptParam("loginID", "");
            string pwd = GetDecryptParam("pwd", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin)
                {
                    Zeospace.DeskPlus.Account.BE.UserInfo info = new Account.BE.UserInfo();
                    info.CompanyID = companyID;                    
                    info.NameBase = name;
                    info.LogInID = loginID;
                    info.Password = pwd;
                    info.ActiveUser = "1";
                    info.WriteUserID = CurrentUserInfo.UserID;
                    //사용자 추가
                    Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
                    int userID = dal.Add(info);

                    //사용자 부서,직급,직책 추가
                    Zeospace.DeskPlus.Account.DAL.UserDept udDal= new Account.DAL.UserDept();
                    Zeospace.DeskPlus.Account.BE.UserDeptInfo udInfo = new Account.BE.UserDeptInfo();
                    udInfo.CompanyID = companyID;
                    udInfo.UserID = userID;
                    udInfo.DeptID = deptID;
                    udInfo.CodeTitleID = 0;
                    udInfo.CodeDutyID = 0;
                    udInfo.BaseDept = "1";
                    udInfo.WriteUserID = CurrentUserInfo.UserID;

                    udDal.AddUserDept(udInfo);

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
