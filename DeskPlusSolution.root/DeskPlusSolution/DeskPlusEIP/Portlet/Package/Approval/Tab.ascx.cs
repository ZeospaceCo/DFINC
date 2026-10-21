using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text;

namespace Zeospace.DeskPlus.Portlet.Web.Package.Approval
{
    public partial class Tab : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {

        public string TabPortletTitle = "";
        public string TabPortletFirst = "";
        public string ParamValueList = "";
        public string UrlValueList = "";
        public string AppVersion = "";
        public string ApprovalStatusMenuUrl = "/DeskPlusEIP/Approval/Status/Default.aspx";
        
        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.Approval.Tab).Assembly;	//어셈블리 선언
            #endregion


            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        public void LoadData()
        {
            AppVersion = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");
            SetApprovalStatusMenuUrl();
            LoadTabPortlet();

        }
        #endregion

        #region (함수)

        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }

        private void SetApprovalStatusMenuUrl()
        {
            Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
            List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = dal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (list != null && list.Count > 0 && "NEW".Equals(list[0].ApprovalStatusMenuType))
            {
                ApprovalStatusMenuUrl = "/DeskPlusEIP/Approval/Status/DefaultAll.aspx";
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 탭포틀릿의 포틀릿 타이틀 출력하기
        /// </summary>
        private void LoadTabPortlet()
        {
            /*
            Zeospace.DeskPlus.EIP.DAL.PortletTab ptDal = new EIP.DAL.PortletTab();
            List<Zeospace.DeskPlus.EIP.BE.PortletTabInfo> alTabPortlet = ptDal.GetPortletTab(CurrentUserInfo.CompanyID, PortletID.ToNumber<int>());

            string targetPortletList = string.Empty;
            foreach(Zeospace.DeskPlus.EIP.BE.PortletTabInfo info in alTabPortlet)
            {
                targetPortletList += info.TargetPortletID + ",";
            }
            */

            string targetPortletList = TargetPortletID;

            if(!string.IsNullOrEmpty(targetPortletList)) targetPortletList = targetPortletList.TrimEnd(',');

            if (targetPortletList.Length > 0)
            {
                Zeospace.DeskPlus.EIP.DAL.Portlet dal = new EIP.DAL.Portlet();
                List<Zeospace.DeskPlus.EIP.BE.PortletInfo> alPortletList =  dal.GetPortletMyRoleList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                    targetPortletList, string.Join(",", CurrentUserInfo.RoleIDs), PortletID.ToNumber<int>());

                StringBuilder sb = new StringBuilder();
                StringBuilder pvSb = new StringBuilder();
                StringBuilder urlSb = new StringBuilder();
                
                string jsEvent = "LoadSelTab_" + TabPortletID + "_" + PortletID;

                //여러 탭포틀릿에서 동일한 포틀릿을 선택할 경우, 두개 이상의 포틀릿 리스트가 나오므로 중복되지 않게 나오도록 한다.
                string portletCheck = ",";

                for(int i = 0; i < alPortletList.Count; i++) 
                {
                    if (alPortletList[i].TPortletID.ToString() == PortletID)
                    {
                        //중복값 체크 하기
                        if (portletCheck.IndexOf("," + alPortletList[i].PortletID.ToString() + ",") == -1)
                        {
                            portletCheck += alPortletList[i].PortletID.ToString() + ",";

                            if (TabPortletFirst.Length == 0) TabPortletFirst = alPortletList[i].PortletID.ToString();

                            sb.Append(string.Format(@"<span><a id='{0}' class=""btn btn-sm "" onclick='{1}({2})'>{3}</a></span>",
                                "selapprovaltab_" + TabPortletID + "_" + PortletID + "_" + alPortletList[i].PortletID.ToString(),
                                jsEvent,
                                alPortletList[i].PortletID,
                                this.GetLangName(alPortletList[i].NameBase, alPortletList[i].NameKo, alPortletList[i].NameEn,
                                    alPortletList[i].NameJa, alPortletList[i].NameFr, alPortletList[i].NameZhcn, alPortletList[i].NameZhtw, PreLang)));

                            //if (i != alPortletList.Count - 1) sb.Append("<li>|</li>");

                            pvSb.Append(alPortletList[i].PortletID + ":" + alPortletList[i].ParamValue);
                            if (i != alPortletList.Count - 1) pvSb.Append(",");

                            // PortletID, UIURL
                            urlSb.Append(alPortletList[i].PortletID + ":" + alPortletList[i].UIURL);
                            if (i != alPortletList.Count - 1) urlSb.Append(",");
                        }
                    }
                }                

                TabPortletTitle = sb.ToString();
                ParamValueList = pvSb.ToString();
                UrlValueList = urlSb.ToString();
            }
        }

        #endregion

        #region (프로퍼티)
        
        #endregion
    }
}
