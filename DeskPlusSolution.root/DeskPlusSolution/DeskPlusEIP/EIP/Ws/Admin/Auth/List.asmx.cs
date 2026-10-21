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
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSEIP
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : Role User 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRoleUserPageList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int roleID = GetDecryptParam("roleID", "0").ToNumber<int>();
            string totalCount = GetDecryptParam("totalCount", "0");
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");
            string pageSize = GetDecryptParam("pageSize", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
            string order = GetDecryptParam("order", "");

            string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');


            DAL.UserRole dal = new DAL.UserRole();
            DataTableReader dtr = dal.GetUserRolePageList(companyID, roleID, setPageingStr[0], setPageingStr[1],
                searchField, searchValue, order);

            try
            {
                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.EIP.BE.UserRoleInfo info = new UserRoleInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            info.RoleID = dtr["RoleID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Types = dtr["Types"].ToString();
                            info.RoleName = dtr["RoleName"].ToString();
                            info.RoleCode = dtr["RoleCode"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetRoleUserPageList : " + ex);
                return null;
            }
        }

        /// <summary>
        /// role user 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetRoleUserPageCount(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int roleID = GetDecryptParam("roleID", "0").ToNumber<int>();
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.UserRole dal = new DAL.UserRole();
                return dal.GetUserRolePageCount(companyID, roleID, searchField, searchValue);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetRoleUserPageCount : " + ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// role user 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelRoleUser(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int roleID = GetDecryptParam("roleID", "0").ToNumber<int>();
            int userID = GetDecryptParam("userID", "0").ToNumber<int>();
            string types = GetDecryptParam("types", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    Zeospace.DeskPlus.EIP.BE.UserRoleInfo info = new BE.UserRoleInfo();
                    info.CompanyID = companyID;
                    info.RoleID = roleID;
                    info.UserID = userID;
                    info.Types = types;

                    Zeospace.DeskPlus.EIP.DAL.UserRole dal = new DAL.UserRole();
                    dal.Del(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelSubsideUser : " + ex);
                return false;
            }
        }
    }
}
