using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class TabTaskMain : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        public string TabPortletTitle = "";
        public string TabPortletFirst = "";
        public string ParamValueList = "";
        public string UrlValueList = "";

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.TabTaskMain).Assembly;	//어셈블리 선언
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

        /// <summary>
        /// 
        /// </summary>
        private void LoadTabPortlet()
        {
            /*
            Zeospace.DeskPlus.EIP.DAL.PortletTab ptDal = new EIP.DAL.PortletTab();
            List<Zeospace.DeskPlus.EIP.BE.PortletTabInfo> alTabPortlet = ptDal.GetPortletTab(CurrentUserInfo.CompanyID, PortletID.ToNumber<int>());

            string targetPortletList = string.Empty;
            foreach (Zeospace.DeskPlus.EIP.BE.PortletTabInfo info in alTabPortlet)
            {
                targetPortletList += info.TargetPortletID + ",";
            }
            */

            string targetPortletList = TargetPortletID;

            if (!string.IsNullOrEmpty(targetPortletList)) targetPortletList = targetPortletList.TrimEnd(',');

            if (targetPortletList.Length > 0)
            {
                Zeospace.DeskPlus.EIP.DAL.Portlet dal = new EIP.DAL.Portlet();
                List<Zeospace.DeskPlus.EIP.BE.PortletInfo> alPortletList = dal.GetPortletMyRoleList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                    targetPortletList, string.Join(",", CurrentUserInfo.RoleIDs), PortletID.ToNumber<int>());
                

                StringBuilder sb = new StringBuilder();
                StringBuilder pvSb = new StringBuilder();
                StringBuilder urlSb = new StringBuilder();
                DAL.Task taskdal = new DAL.Task();

                if (EtcParam.Length > 0) TabPortletFirst = EtcParam;

                string jsEvent = "LoadSelTab_" + TabPortletID + "_" + PortletID;
                for (int i = 0; i < alPortletList.Count; i++)
                {
                    if (alPortletList[i].TPortletID.ToString() == PortletID)
                    {                       
                        var cnt = 0;
                        if (TabPortletFirst.Length == 0) TabPortletFirst = alPortletList[i].PortletID.ToString();

                        if (alPortletList[i].UIURL.IndexOf("GetRequest") != -1)
                        {
                            cnt = taskdal.GetRequestCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "");
                        }
                        else if (alPortletList[i].UIURL.IndexOf("GetReceive") != -1)
                        {
                            cnt = taskdal.GetReceiveCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "");
                        }
                        else if (alPortletList[i].UIURL.IndexOf("GetReference") != -1)
                        {
                            cnt = taskdal.GetReferenceCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "");
                        }
                        else if (alPortletList[i].UIURL.IndexOf("GetForward") != -1)
                        {
                            cnt = taskdal.GetForwardCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "");
                        }

                        //var firstTabCss = (TabPortletFirst == alPortletList[i].PortletID.ToString()) ? "class='on hovercol'" : "";
                        var firstTabCss = (TabPortletFirst == alPortletList[i].PortletID.ToString()) ? "btn-secondary" : "";

                        if (cnt > 0)
                        {
                            sb.Append(string.Format(@"<span><a id='{0}' class=""btn btn-sm {4} "" onclick='{1}({2})'>{3} <span class=""badge number badge-light"">{5}</span></a></span>",
                                "seltemptab_" + TabPortletID + "_" + PortletID + "_" + alPortletList[i].PortletID.ToString(),
                                jsEvent,
                                alPortletList[i].PortletID,
                                this.GetLangName(alPortletList[i].NameBase, alPortletList[i].NameKo, alPortletList[i].NameEn,
                                    alPortletList[i].NameJa, alPortletList[i].NameFr, alPortletList[i].NameZhcn, alPortletList[i].NameZhtw, PreLang),
                                    firstTabCss,
                                    cnt
                                    ));
                        }
                        else
                        {
                            sb.Append(string.Format(@"<span><a id='{0}' class=""btn btn-sm {4} "" onclick='{1}({2})'>{3}</a></span>",
                                "seltemptab_" + TabPortletID + "_" + PortletID + "_" + alPortletList[i].PortletID.ToString(),
                                jsEvent,
                                alPortletList[i].PortletID,
                                this.GetLangName(alPortletList[i].NameBase, alPortletList[i].NameKo, alPortletList[i].NameEn,
                                    alPortletList[i].NameJa, alPortletList[i].NameFr, alPortletList[i].NameZhcn, alPortletList[i].NameZhtw, PreLang),
                                    firstTabCss
                                    ));
                        }

                        pvSb.Append(alPortletList[i].PortletID + ":" + alPortletList[i].ParamValue);
                        if (i != alPortletList.Count - 1) pvSb.Append(",");

                        // PortletID, UIURL
                        urlSb.Append(alPortletList[i].PortletID + ":" + alPortletList[i].UIURL);
                        if (i != alPortletList.Count - 1) urlSb.Append(",");
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