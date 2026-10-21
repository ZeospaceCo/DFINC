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
        public List<PortletDataLinkInfo> GetPortletDataLinkList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();

            try
            {
                DAL.PortletDataLink dal = new DAL.PortletDataLink();
                List<BE.PortletDataLinkInfo> al = dal.GetDataLink(companyID, portletID);

                foreach (BE.PortletDataLinkInfo info in al)
                {
                    //암호화 (1 : md5, 2: ASE(Rindal, 3:기타입력)
                    if (info.EncriptType != "3") info.EncriptValue = "";
                    info.EncriptType = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, info.EncriptType, "ENCRIPTION", CurrentUserInfo.PreferredLanguage);
                    if (info.EncriptType == "") info.EncriptType = "사용안함";


                    //파라미터 연결 대상( U : UI / D : Data / L : Link)
                    if (info.TargetType == "D")
                    {
                        info.TargetDataType = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, info.TargetDataType, "TargetDataType", CurrentUserInfo.PreferredLanguage);
                    }
                    else
                    {
                        info.TargetDataType = "";
                    }

                    info.TargetType = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, info.TargetType, "TargetType", CurrentUserInfo.PreferredLanguage);
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPortletDataLinkList : " + ex);
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
        /// 데이터연동 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortletDataLink(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletDataLinkID = GetDecryptParam("portletDataLinkID", "0").ToNumber<int>();

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    Zeospace.DeskPlus.EIP.DAL.PortletDataLink dal = new DAL.PortletDataLink();
                    dal.Del(companyID, portletDataLinkID);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortletDataLink : " + ex);
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetCountByPortletCode(Mod) : " + ex);
                return 0;
            }
        }
    }
}
