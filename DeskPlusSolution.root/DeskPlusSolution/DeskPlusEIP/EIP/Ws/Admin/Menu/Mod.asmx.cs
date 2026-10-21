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
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSEIP
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 데이터연동 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<MenuDataLinkInfo> GetMenuDataLinkList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

            try
            {
                DAL.MenuDataLink dal = new DAL.MenuDataLink();
                List<BE.MenuDataLinkInfo> al = dal.GetDataLink(companyID, menuID);

                foreach (BE.MenuDataLinkInfo info in al)
                {
                    //암호화 (1 : md5, 2: ASE(Rindal, 3:기타입력)
                    if (info.EncriptType != "3") info.EncriptValue = "";
                    info.EncriptType = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, info.EncriptType, "ENCRIPTION", CurrentUserInfo.PreferredLanguage);
                    if (info.EncriptType == "") info.EncriptType = "사용안함";
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMenuDataLinkList : " + ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 권한접근제어 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<MenuRoleInfo> GetMenuRoleList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

            try
            {
                DAL.MenuRole dal = new DAL.MenuRole();
                return dal.GetMenuRole(companyID, menuID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMenuRoleList : " + ex);
                return null;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : IP 접근제어 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<MenuIPInfo> GetMenuIPList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

            try
            {
                DAL.MenuIP dal = new DAL.MenuIP();
                List<BE.MenuIPInfo> al = dal.GetMenuIP(companyID, menuID);

                foreach (MenuIPInfo info in al)
                {
                    if (info.StartIP.Trim().Length > 0)
                    {
                        info.StartIP1 = info.StartIP.Substring(0, 3).ToNumber<int>().ToString();
                        info.StartIP2 = info.StartIP.Substring(3, 3).ToNumber<int>().ToString();
                        info.StartIP3 = info.StartIP.Substring(6, 3).ToNumber<int>().ToString();
                        info.StartIP4 = info.StartIP.Substring(9, 3).ToNumber<int>().ToString();
                    }

                    if (info.EndIP.Trim().Length > 0)
                    {
                        info.EndIP1 = info.EndIP.Substring(0, 3).ToNumber<int>().ToString();
                        info.EndIP2 = info.EndIP.Substring(3, 3).ToNumber<int>().ToString();
                        info.EndIP3 = info.EndIP.Substring(6, 3).ToNumber<int>().ToString();
                        info.EndIP4 = info.EndIP.Substring(9, 3).ToNumber<int>().ToString();
                    }

                    if (info.IPAccess == "0")
                    {
                        info.IPAccess = "접속허용";
                    }
                    else if (info.IPAccess == "1")
                    {
                        info.IPAccess = "접속차단";
                    }
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetMenuIPList : " + ex);
                return null;
            }
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
        /// 데이터연동 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMenuDataLink(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuDataLinkID = GetDecryptParam("menuDataLinkID", "0").ToNumber<int>();

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    Zeospace.DeskPlus.EIP.DAL.MenuDataLink dal = new DAL.MenuDataLink();
                    dal.Del(companyID, menuDataLinkID);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelMenuDataLink : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 메뉴 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMenuRole(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int menuRoleID = GetDecryptParam("menuRoleID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.MenuRole dal = new DAL.MenuRole();
                    dal.Del(companyID, menuRoleID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelMenuRole : " + ex);
                return false;
            }
        }


        /// <summary>
        /// ip 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMenuIP(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int menuIPID = GetDecryptParam("menuIPID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.MenuIP dal = new DAL.MenuIP();
                    dal.Del(companyID, menuIPID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelMenuIP : " + ex);
                return false;
            }
        }
    }
}
