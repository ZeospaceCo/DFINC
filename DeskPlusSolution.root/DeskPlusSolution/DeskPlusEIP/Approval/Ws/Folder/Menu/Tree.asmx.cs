using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Menu
{
    /// <summary>
    /// Tree의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Tree : Base.BaseWSApproval
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {

            SetDecryptString(p);
            string folderID = GetDecryptParam("folderID", "");
            string gubun = GetDecryptParam("gubun", "");

            bool response = false;

            try
            {
                if (folderID.Length > 0 && gubun == "Dept_Common") //부서별공용폴더 삭제
                {
                    if (folderID.ToNumber<int>() > 0)
                    {
                        DAL.PublicFolder dal = new DAL.PublicFolder();
                        dal.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, folderID.ToNumber<int>());

                        response = true;
                    }
                }
                else if (folderID.Length > 0 && gubun == "Private_Folder") //개인문서함 폴더 삭제
                {
                    if (folderID.IndexOf("PrivateFolder_") != -1) folderID = folderID.Split('_')[1];

                    if (folderID.ToNumber<int>() > 0)
                    {
                        DAL.PrivateFolder dal = new DAL.PrivateFolder();
                        dal.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID.ToNumber<int>());

                        response = true;
                    }
                }
                else if (folderID.Length > 0 && gubun == "Approval_Folder") //완결문서함 폴더 삭제
                {
                    if (folderID.IndexOf("ApprovalFolder_") != -1) folderID = folderID.Split('_')[1];

                    if (folderID.ToNumber<int>() > 0)
                    {
                        DAL.ApprovalFolder dal = new DAL.ApprovalFolder();
                        dal.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID.ToNumber<int>());

                        response = true;
                    }
                }
                else if (folderID.Length > 0 && gubun == "Arbitrary_Folder") //전결문서함 폴더 삭제
                {
                    if (folderID.IndexOf("ArbitraryFolder_") != -1) folderID = folderID.Split('_')[1];

                    if (folderID.ToNumber<int>() > 0)
                    {
                        DAL.ArbitraryFolder dal = new DAL.ArbitraryFolder();
                        dal.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID.ToNumber<int>());

                        response = true;
                    }
                }
                else if (folderID.Length > 0 && gubun == "Reject_Folder") //반려문서함 폴더 삭제
                {
                    if (folderID.IndexOf("RejectFolder_") != -1) folderID = folderID.Split('_')[1];

                    if (folderID.ToNumber<int>() > 0)
                    {
                        DAL.ApprovalFolderR dal = new DAL.ApprovalFolderR();
                        dal.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID.ToNumber<int>());

                        response = true;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 수발자 여부
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetAccountUser(string p)
        {   
            try
            {
                DAL.Account dal = new DAL.Account();
                int response = dal.GetAccountUser(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, "R", CurrentUserInfo.UserID);
                return (response == 0) ? false : true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAccountUser : " + ex.Message);
                throw;
            }
        }
    }
}
