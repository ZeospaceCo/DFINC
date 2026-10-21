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

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Portlet
{
    /// <summary>
    /// ModTab의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ModTab : Base.BaseWSEIP
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 탭 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<PortletTabInfo> GetPortletTabList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();

            try
            {
                DAL.PortletTab dal = new DAL.PortletTab();
                List<PortletTabInfo> al = dal.GetPortletTab(companyID, portletID);

                foreach (PortletTabInfo info in al)
                {
                    info.PortletNameBase = this.GetLangName(
                        info.PortletNameBase, 
                        info.PortletNameKo, 
                        info.PortletNameEn, 
                        info.PortletNameJa, 
                        info.PortletNameFr, 
                        info.PortletNameZhcn, 
                        info.PortletNameZhtw, PreLang);

                    info.NameBase = this.GetLangName(
                        info.NameBase,
                        info.NameKo,
                        info.NameEn,
                        info.NameJa,
                        info.NameFr,
                        info.NameZhcn,
                        info.NameZhtw, PreLang);
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPortletTabList : " + ex);
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
        public List<PortletRoleInfo> GetPortletRoleList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();

            try
            {
                DAL.PortletRole dal = new DAL.PortletRole();
                return dal.GetPortletRole(companyID, portletID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPortletRoleList : " + ex);
                return null;
            }
        }


        /// <summary>
        /// 포틀릿 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortlet(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string portletList = GetDecryptParam("portletList", "").TrimEnd(',');

            if (portletList.Length == 0) return false;

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    foreach (string id in portletList.Split(','))
                    {
                        DAL.Portlet dal = new DAL.Portlet();
                        dal.Del(companyID, categoryID, id.ToNumber<int>());
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortlet : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 포틀릿 탭 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortletTab(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
                int portletTabID = GetDecryptParam("portletTabID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.PortletTab dal = new DAL.PortletTab();
                    dal.Del(companyID, portletID, portletTabID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortletTab : " + ex);
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
        public bool DelPortletRole(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int portletRoleID = GetDecryptParam("portletRoleID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.PortletRole dal = new DAL.PortletRole();
                    dal.Del(companyID, portletRoleID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortletRole : " + ex);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 탭 이름 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModPortletTabName(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            int portletTabID = GetDecryptParam("portletTabID", "0").ToNumber<int>();

            string nameKo = GetDecryptParam("nameko", "");
            string nameEn = GetDecryptParam("nameEn", "");
            string nameJa = GetDecryptParam("nameJa", "");
            string nameFr = GetDecryptParam("nameFr", "");
            string nameZhcn = GetDecryptParam("nameZhcn", "");
            string nameZhtw = GetDecryptParam("nameZhtw", "");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                BE.PortletTabInfo info = new BE.PortletTabInfo();
                info.CompanyID = companyID;
                info.PortletID = portletID;
                info.PortletTabID = portletTabID;
                info.NameKo = nameKo;
                info.NameEn = nameEn;
                info.NameJa = nameJa;
                info.NameFr = nameFr;
                info.NameZhcn = nameZhcn;
                info.NameZhtw = nameZhtw;

                DAL.PortletTab dal = new DAL.PortletTab();
                dal.ModTabName(info);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 중복된 포틀릿코드 값이 있는지 확인
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCountByPortletCode(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            string portletCode = GetDecryptParam("portletCode", "");


            try
            {
                DAL.Portlet dal = new DAL.Portlet();

                return dal.GetPotletCodeCount(companyID, portletID, portletCode);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetCountByPortletCode(ModTab) : " + ex);
                return 0;
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
        public List<PortletIPInfo> GetPortletIPList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();

            try
            {
                DAL.PortletIP dal = new DAL.PortletIP();
                List<BE.PortletIPInfo> al = dal.GetPortletIP(companyID, portletID);

                foreach (PortletIPInfo info in al)
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPortletIPList : " + ex);
                return null;
            }
        }


        /// <summary>
        /// ip 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortletIP(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.PortletIP dal = new DAL.PortletIP();
                    dal.Del(companyID, portletID);

                    //포틀릿 ip 권한 캐쉬 설정하기
                    BLL.PortletIP biz = new BLL.PortletIP();
                    biz.ClearCachePortletIP(companyID);
                    biz.SetCachePortletIP(companyID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortletIP : " + ex);
                return false;
            }
        }
    }
}
