using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Account.BE;
using System.IO;
using Zeospace.DeskPlus.Portlet.Web.Base;

namespace Zeospace.DeskPlus.EIP.Web.Portal
{
    public partial class Main : Base.BasePageEIP
    {
        #region 변수설정

        public int TabID = 0;
        public string TabList = "";
        public string TabLayout = "1";
        public bool PortletAuth = false;
        public string IsAdmin = "0";

        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Portal.Main).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();

                //Script에서 처리하던 사항을 Behind에서 처리 ByFreeman 20120707
                LoadTabPortletLayout();
                LoadTabPortletList();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            if (TabID == 0)
            {
                Account.DAL.UserEnvironment dal = new Account.DAL.UserEnvironment();
                List<UserEnvironmentInfo> al = dal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                foreach (UserEnvironmentInfo info in al)
                {
                    TabID = info.BaseTab;
                }
            }

            int firstTab = LoadTabList();
            TabID = (TabID <= 0) ? firstTab : TabID;
            LoadTabLayout();
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 작성자  : 심재운
        /// 레이아웃
        /// </summary>
        private void LoadTabLayout()
        {
            DAL.Tab dal = new DAL.Tab();
            List<TabInfo> al = dal.GetTab(CurrentUserInfo.CompanyID, TabID, true);
            
            foreach (TabInfo info in al)
            {
                TabLayout = info.TabLayout.ToString();
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 탭리스트
        /// </summary>
        private int LoadTabList()
        {
            DAL.Tab dal = new DAL.Tab();
            List<TabInfo> al = dal.GetTabByRole(CurrentUserInfo.CompanyID, string.Join(",", CurrentUserInfo.RoleIDs), CurrentUserInfo.UserID);

            StringBuilder sb = new StringBuilder();
            string name = string.Empty;
            int i = 1;
            int firstTabID = 0;            

            foreach (TabInfo info in al)
            {
                if ((TabID <= 0 && i == 1) || (TabID == info.TabID))
                {
                    sb.Append(@"<li class=""on"">");
                    firstTabID = info.TabID;
                    //관리자 탭이며, 관리자일 경우 포틀릿 권한 가능
                    if (info.IsAdmin == "1" && CurrentUserInfo.IsInRole("admin")) PortletAuth = true;
                    //사용자 탭이며, 본인이 추가한 포틀릿이 맞으면 권한 가능
                    if (info.IsAdmin == "0" && info.WriteUserID == CurrentUserInfo.UserID) PortletAuth = true;

                    IsAdmin = info.IsAdmin;//관리자 탭여부 확인
                    
                }
                else sb.Append(@"<li>");

                #region 이름 다국어 설정
                name = this.GetLangName(
                        info.NameBase.ToString().ToNull(), info.NameKo.ToString().ToNull(),
                        info.NameEn.ToString().ToNull(), info.NameJa.ToString().ToNull(),
                        info.NameFr.ToString().ToNull(), info.NameZhcn.ToString().ToNull(),
                        info.NameZhtw.ToString().ToNull(), PreLang);
                #endregion


                sb.Append(string.Format(@"<a alt='' title='' onclick='Main.GoTab({0})' ", info.TabID));
                sb.Append(string.Format(@"><span title='{0}'>", name));
                sb.Append(name);
                sb.Append("</span></a></li>");

                i += 1;

                
            }

            TabList = sb.ToString();

            return firstTabID;
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            TabID = GetDecryptParam("tabID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSimpleAdd.Text = resMng.GetString("Cancel");
            }
        }

        /// <summary>
        /// 레이아웃 로드
        /// </summary>
        private void LoadTabPortletLayout()
        {
            string layoutPath = "/DeskPlusEIP/EIP/Jt/Portal/Layout/JTLayout" + TabLayout + ".htm";

            string file = Server.MapPath(layoutPath); 
            StreamReader sr; 
            
            FileInfo fi = new FileInfo(file); 
            string layoutHTML = ""; 
            if (File.Exists(file)) { 
                sr = File.OpenText(file);
                layoutHTML += sr.ReadToEnd(); 
                sr.Close(); 
            }

            Control layoutControl = ParseControl(layoutHTML);
            placeholderLayout.Controls.Add(layoutControl);
        }

        /// <summary>
        /// 포틀릿 로드
        /// </summary>
        private void LoadTabPortletList()
        {
            if (TabID == 0) return;
            string roleUserList = string.Join(",", CurrentUserInfo.RoleIDs);

            try
            {
                DAL.TabPortlet dal = new DAL.TabPortlet();
                List<TabPortletInfo> al = dal.GetTabPortlet(CurrentUserInfo.CompanyID, TabID, CurrentUserInfo.UserID, roleUserList);
                DAL.MenuDataLink dalMenu = new DAL.MenuDataLink();
                BLL.MenuDataLink bllMenu = new BLL.MenuDataLink();
                List<BE.LDAPInfo> alLdap = dalMenu.GetLdap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                foreach (TabPortletInfo info in al)
                {
                    try
                    {
                        string paramValue = info.ParamValue.ToNull();
                        //id$Mail$OL^pass$UserPassword$OL^go$dec$OL
                        if (paramValue.Length > 0)
                        {
                            info.ParamValue = "";

                            foreach (string tmpStr in paramValue.Split('^'))
                            {
                                //id$Mail$OL
                                if (tmpStr.Split('$')[2] == "OL")
                                {
                                    //Mail
                                    string ldapValue = bllMenu.GetLinkValue(alLdap[0], tmpStr.Split('$')[1]);
                                    info.ParamValue += tmpStr.Split('$')[0] + "$" + ldapValue + "$" + tmpStr.Split('$')[2] + "^";

                                }
                                else //OU
                                {
                                    info.ParamValue += tmpStr.Split('$')[0] + "$" + tmpStr.Split('$')[1] + "$" + tmpStr.Split('$')[2] + "^";
                                }
                            }

                            info.ParamValue = info.ParamValue.TrimEnd('^');
                        }

                        string p = "";
                        p += "tabPortletID=" + info.TabPortletID;
                        p += "&portletID=" + info.PortletID;
                        p += "&categoryID=" + info.CategoryID;
                        p += "&nameBase=" + info.NameBase;
                        p += "&nameKo=" + info.NameKo;
                        p += "&nameEn=" + info.NameEn;
                        p += "&nameJa=" + info.NameJa;
                        p += "&nameFr=" + info.NameFr;
                        p += "&nameZhcn=" + info.NameZhcn;
                        p += "&nameZhtw=" + info.PathNameZhtw;
                        p += "&portletCode=" + info.PortletCode;
                        p += "&portletType=" + info.PortletType;
                        p += "&uiType=" + info.UIType;
                        p += "&uiTemplate=" + info.UITemplate;
                        p += "&uiURL=" + info.UIURL;
                        p += "&dataType=" + info.DataType;
                        p += "&dataURL=" + info.DataURL;
                        p += "&linkType=" + info.LinkType;
                        p += "&linkPopupSize=" + info.LinkPopupSize;
                        p += "&linkURL=" + info.LinkURL;
                        p += "&isPesonalization=" + info.IsPesonalization;
                        p += "&activePortlet=" + info.ActivePortlet;
                        p += "&sendMethod=" + info.SendMethod;
                        p += "&description=" + info.Description;
                        p += "&photoName=" + info.PhotoName;
                        p += "&isAdmin=" + info.IsAdmin;
                        p += "&userID=" + info.UserID;
                        p += "&isMinimize=" + info.IsMinimize;
                        p += "&pageSize=" + info.PageSize;
                        p += "&etcParam=" + info.EtcParam;
                        p += "&paramValue=" + info.ParamValue;

                        string encP = Utility.EncryptString(p);

                        BaseControlPortlet c = (BaseControlPortlet)Page.LoadControl(info.UIURL);
                        c.P = encP;


                        Control parent = ServerFindControl.FindControlAll(placeholderLayout, info.ColumnID);

                        parent.Controls.Add(c);

                    }
                    catch (Exception ex)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Main[LoadPortlet] : " + ex);
                    }
                }


            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTabPortlet : " + ex);
                throw;
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}