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
        /// 목적 : Portlet 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPortletPageList(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Portal.ModTabOrder).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string totalCount = GetDecryptParam("totalCount", "0");
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");
            string pageSize = GetDecryptParam("pageSize", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
            string order = GetDecryptParam("order", "");

            string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');


            DAL.Portlet dal = new DAL.Portlet();
            DataTableReader dtr = dal.GetPageList(companyID, categoryID, setPageingStr[0], setPageingStr[1],
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
                            Zeospace.DeskPlus.EIP.BE.PortletInfo info = new PortletInfo();

                            info.CatNameBase = this.GetLangName(
                                        dtr["CatNameBase"].ToString().ToNull(), dtr["CatNameKo"].ToString().ToNull(),
                                        dtr["CatNameEn"].ToString().ToNull(), dtr["CatNameJa"].ToString().ToNull(),
                                        dtr["CatNameFr"].ToString().ToNull(), dtr["CatNameZhcn"].ToString().ToNull(),
                                        dtr["CatNameZhtw"].ToString().ToNull(), PreLang);

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>(); ;
                            info.PortletID = dtr["PortletID"].ToString().ToNumber<int>(); ;
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>(); ;
                            info.NameBase = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            //포틀릿종류 ( T : 탭포틀릿 / P : 일반 포틀릿)
                            info.PortletType = dtr["PortletType"].ToString();
                            info.PortletTypeName = AppSettingHelper.GetSysCode(companyID, info.PortletType, "PortletType", CurrentUserInfo.PreferredLanguage);

                            //UI 형식 (T : 템플릿/ L: 내부페이지Layer/ F : IFrame)                            
                            info.UIType = AppSettingHelper.GetSysCode(companyID, dtr["UIType"].ToString(), "UIType", CurrentUserInfo.PreferredLanguage);

                            //UI 템플릿 형식 ( L : 리스트 / B : 블러그 / A : 앨범)
                            info.UITemplate = AppSettingHelper.GetSysCode(companyID, dtr["UITemplate"].ToString(), "UIType", CurrentUserInfo.PreferredLanguage);
                            info.UIURL = dtr["UIURL"].ToString();

                            //Data 호출 형식 (R : RSS 호출 / S : SQL Storeprocedure Name)                            
                            info.DataType = AppSettingHelper.GetSysCode(companyID, dtr["DataType"].ToString(), "TargetDataType", CurrentUserInfo.PreferredLanguage);

                            info.DataURL = dtr["DataURL"].ToString();

                            //More Link 형식 (P : 팝업창 /  N : 새창 / C : 하단 프레임)
                            info.LinkType = AppSettingHelper.GetSysCode(companyID, dtr["LinkType"].ToString(), "LinkType", CurrentUserInfo.PreferredLanguage);

                            info.LinkPopupSize = dtr["LinkPopupSize"].ToString();
                            info.LinkURL = dtr["LinkURL"].ToString();
                            //개인화허용유무(1 : 허용 / 0 : 불가)
                            info.IsPesonalization = dtr["IsPesonalization"].ToString();
                            switch (info.IsPesonalization)
                            {
                                case "0":
                                    info.IsPesonalization = this.GetResource("No");
                                    break;
                                case "1":
                                    info.IsPesonalization = this.GetResource("Yes");
                                    break;
                            }

                            //활성화여부(1 : 허용 / 0 : 불가)
                            info.ActivePortlet = dtr["ActivePortlet"].ToString();
                            switch (info.ActivePortlet)
                            {
                                case "0":
                                    info.ActivePortlet = this.GetResource("No");
                                    break;
                                case "1":
                                    info.ActivePortlet = this.GetResource("Yes");
                                    break;
                            }

                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();


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
        /// Portlet 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPortletPageCount(string p)
        {
            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.Portlet dal = new DAL.Portlet();
                return dal.GetPageListCount(companyID, categoryID, searchField, searchValue);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPortletPageCount : " + ex.Message);
                return 0;
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
        /// 작성자 : 심재운
        /// 용도 : 포틀릿 활성,비활성화 처리
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ActivePortlet(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string portletList = GetDecryptParam("portletList", "");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {

                if (portletList.Length > 0)
                {
                    DAL.Portlet dal = new DAL.Portlet();

                    foreach (string _portletID in portletList.Split(','))
                    {
                        dal.ModActive(companyID, categoryID, _portletID.ToNumber<int>());
                    }
                }

                return true;
            }

            return false;
        }
    }
}
