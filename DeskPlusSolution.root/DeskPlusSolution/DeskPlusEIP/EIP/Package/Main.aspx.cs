using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Portlet.Web.Base;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.EIP.Web.Package
{
    public partial class Main : Base.BasePageEIP
    {
        #region 변수설정

        public int TabID = 0;
        public string TabList = "";
        public string TabLayout = "1";
        public bool PortletAuth = false;
        public string IsAdmin = "0";

        //게시판 메인팝업
        public string MainPopupBoardList = "";
        public string PopupWidth = "";
        public string PopupHeight = "";

        //팝업
        public SortedDictionary<string, string> PopupContent;

        // TODO 팝업
        public string TodoPopup = "";
        #endregion 변수설정

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Package.Main).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                PopupContent = new SortedDictionary<string, string>();

                LoadParam();
                SetLanguage();
                LoadData();

                //Script에서 처리하던 사항을 Behind에서 처리 ByFreeman 20120707
                LoadTabPortletLayout();
                LoadTabPortletList();

                //메인팝업
                BoardMainPopupList();
                //팝업 & TODO 팝업
                GetPopUp();
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

        #endregion Page Process (페이지 처리)

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
                    sb.Append(@"<li class=""on hovercol"">");
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

                #endregion 이름 다국어 설정

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
            if (File.Exists(file))
            {
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

            string[] remoteAddrStr = Request.UserHostAddress.Split('.');
            double remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr).ToDouble<double>();
            string roleUserList = string.Join(",", CurrentUserInfo.RoleIDs);

            try
            {
                DAL.TabPortlet dal = new DAL.TabPortlet();
                BLL.PortletIP bizPortletIP = new BLL.PortletIP();

                //나의 role 권한으로 볼 수 잇는 포틀릿 정보를 가져온다.
                List<TabPortletInfo> al = dal.GetTabPortlet(CurrentUserInfo.CompanyID, TabID, CurrentUserInfo.UserID, roleUserList);
                DAL.MenuDataLink dalMenu = new DAL.MenuDataLink();
                BLL.MenuDataLink bllMenu = new BLL.MenuDataLink();
                List<BE.LDAPInfo> alLdap = dalMenu.GetLdap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                List<BE.PortletIPInfo> portletIPList = bizPortletIP.SetCachePortletIP(CurrentUserInfo.CompanyID);

                string uiUrl = "";

                foreach (TabPortletInfo info in al)
                {
                    try
                    {
                        #region 포틀릿  IP값이 존재할 경우

                        var ipPortletIPLists = (from entry in portletIPList
                                                where entry.PortletID == info.PortletID
                                                select entry);

                        //### IP값이 존재할 경우
                        if (ipPortletIPLists.Count() > 0)
                        {
                            double stratIP = 0;
                            double endIP = 0;
                            bool ipAccess = false; //우선 막아준다.

                            foreach (BE.PortletIPInfo ipInfo in ipPortletIPLists)
                            {
                                stratIP = ipInfo.StartIP.ToDouble<double>();
                                endIP = ipInfo.EndIP.ToDouble<double>();

                                AppException.WriteAssert("LoadTabPortletList : stratIP - " + stratIP + " / endIP - " + endIP + " / remoteAddr - " + remoteAddr, AppSettingType.EIP);

                                //[차단] ip 값이 존재하면 접근 불가며, 차단처리하고 빠져나온다.
                                if (ipInfo.IPAccess == "1") //0:허용; 1:차단
                                {
                                    if (stratIP <= remoteAddr && endIP >= remoteAddr)
                                    {
                                        ipAccess = false; break;
                                    }
                                }

                                //[허용] ip값이 존재할 경우, 차단 값이 존재할 수있으니 계속 루프로 확인한다.
                                if (ipInfo.IPAccess == "0") //0:허용; 1:차단
                                {
                                    if (stratIP <= remoteAddr && endIP >= remoteAddr) ipAccess = true;
                                }
                            }

                            if (!ipAccess) continue; //IP 으로 인해 차단당할 경우, 숨긴다.
                        }

                        #endregion 포틀릿  IP값이 존재할 경우

                        uiUrl = info.UIURL;

                        string paramValue = info.ParamValue.ToNull();
                        //id$Mail$OL^pass$UserPassword$OL^go$dec$OL
                        if (paramValue.Length > 0)
                        {
                            info.ParamValue = "";
                            string ldapValue = "";
                            foreach (string tmpStr in paramValue.Split('^'))
                            {
                                //id$Mail$OL
                                if (tmpStr.Split('$')[2] == "OL")
                                {
                                    //Mail
                                    ldapValue = bllMenu.GetLinkValue(alLdap[0], tmpStr.Split('$')[1]);
                                    info.ParamValue += tmpStr.Split('$')[0] + "$" + ldapValue + "$" + tmpStr.Split('$')[2] + "^";
                                }
                                else //OU
                                {
                                    info.ParamValue += tmpStr.Split('$')[0] + "$" + tmpStr.Split('$')[1] + "$" + tmpStr.Split('$')[2] + "^";
                                }
                            }

                            info.ParamValue = info.ParamValue.TrimEnd('^');
                        }

                        StringBuilder sb = new StringBuilder();

                        sb.Append("tabPortletID=" + info.TabPortletID);
                        sb.Append("&portletID=" + info.PortletID);
                        sb.Append("&categoryID=" + info.CategoryID);
                        sb.Append("&nameBase=" + info.NameBase);
                        sb.Append("&nameKo=" + info.NameKo);
                        sb.Append("&nameEn=" + info.NameEn);
                        sb.Append("&nameJa=" + info.NameJa);
                        sb.Append("&nameFr=" + info.NameFr);
                        sb.Append("&nameZhcn=" + info.NameZhcn);
                        sb.Append("&nameZhtw=" + info.PathNameZhtw);
                        sb.Append("&portletCode=" + info.PortletCode);
                        sb.Append("&portletType=" + info.PortletType);
                        sb.Append("&uiType=" + info.UIType);
                        sb.Append("&uiTemplate=" + info.UITemplate);
                        sb.Append("&uiURL=" + uiUrl);
                        sb.Append("&dataType=" + info.DataType);
                        sb.Append("&dataURL=" + info.DataURL);
                        sb.Append("&linkType=" + info.LinkType);
                        sb.Append("&linkPopupSize=" + info.LinkPopupSize);
                        sb.Append("&linkURL=" + info.LinkURL);
                        sb.Append("&isPesonalization=" + info.IsPesonalization);
                        sb.Append("&activePortlet=" + info.ActivePortlet);
                        sb.Append("&sendMethod=" + info.SendMethod);
                        sb.Append("&description=" + info.Description);
                        sb.Append("&photoName=" + info.PhotoName);
                        sb.Append("&isAdmin=" + info.IsAdmin);
                        sb.Append("&userID=" + info.UserID);
                        sb.Append("&isMinimize=" + info.IsMinimize);
                        sb.Append("&pageSize=" + info.PageSize);
                        sb.Append("&etcParam=" + info.EtcParam);
                        sb.Append("&paramValue=" + info.ParamValue);
                        sb.Append("&columnID=" + info.ColumnID);

                        string encP = Utility.EncryptString(sb.ToString());

                        BaseControlPortlet c = (BaseControlPortlet)Page.LoadControl(uiUrl);
                        c.P = encP;

                        Control parent = ServerFindControl.FindControlAll(placeholderLayout, info.ColumnID);

                        if (parent != null)
                            parent.Controls.Add(c); //ByFreeman 20140415
                        else
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("url " + uiUrl, AppSettingType.EIP);
                    }
                    catch (Exception ex)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("url " + uiUrl + " / Main[LoadPortlet] : " + ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTabPortlet : " + ex);
                throw;
            }
        }


        /// <summary>
        /// 게시판 메인 팝업
        /// 심재운
        /// </summary>
        private void BoardMainPopupList()
        {
            //string ct = string.Empty;
            string pID = string.Empty;

            Zeospace.DeskPlus.Board.DAL.ListEtc biz = new Board.DAL.ListEtc();
            Zeospace.DeskPlus.Board.BLL.Category bizCat = new Board.BLL.Category();
            NameValueCollection nvcAuth = new NameValueCollection();

            string nowDate = string.Empty;
            nowDate = DateTime.Now.ToString("yyyyMMdd");

            DataSet ds = biz.GetMainPopUpList(CurrentUserInfo.CompanyID, nowDate);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string categoryID = dr["CategoryID"].ToString();
                    string listID = dr["ListID"].ToString();
                    string optionUseDept = dr["OptionUseDept"].ToString();
                    string popupSize = dr["Etc7"].ToString();

                    //읽기 이상 존재하는지 체크
                    nvcAuth = bizCat.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, Convert.ToInt32(categoryID), optionUseDept, false, BoardAuth.Read, true, false);

                    if (nvcAuth.AllKeys.Length > 0)
                    {
                        pID = categoryID + "_" + listID;

                        //ct = "window.open('/DeskPlusEIP/BoardPopUp.aspx?popid=" + pID + "', 'pop" + pID + "', 'menubar=no, scrollbars=no, status=no, toolbar=no, resizable=no, width=1000, height=750');";
                        //BoardPopup.Add(pID, ct);
                        PopupWidth = popupSize.Split(',')[0];
                        PopupHeight = popupSize.Split(',')[1];
                        MainPopupBoardList = "/DeskPlusEIP/Portlet/BoardPopup/BoardMainPopUp.aspx?popid=" + pID + "&height=" + PopupHeight + "," + pID;
                        
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 팝업
        /// </summary>
        private void GetPopUp()
        {

            string ct = string.Empty;
            string pID = string.Empty;

            Zeospace.DeskPlus.Board.DAL.ListEtc biz = new Board.DAL.ListEtc();
            Zeospace.DeskPlus.Board.BLL.Category bizCat = new Board.BLL.Category();
            NameValueCollection nvcAuth = new NameValueCollection();

            string nowDate = string.Empty;
            nowDate = DateTime.Now.ToString("yyyyMMdd");

            DataSet ds = biz.GetPopUpList(CurrentUserInfo.CompanyID, nowDate);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string categoryID = dr["CategoryID"].ToString();
                    string listID = dr["ListID"].ToString();
                    string optionUseDept = dr["OptionUseDept"].ToString();

                    //읽기 이상 존재하는지 체크
                    nvcAuth = bizCat.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, Convert.ToInt32(categoryID), optionUseDept, false, BoardAuth.Read, true, false);

                    if (nvcAuth.AllKeys.Length > 0)
                    {
                        pID = categoryID + "_" + listID;

                        ct = "window.open('/DeskPlusEIP/Portlet/BoardPopup/BoardPopUp.aspx?popid=" + pID + "', 'pop" + pID + "', 'menubar=no, scrollbars=no, status=no, toolbar=no, resizable=no, width=1000, height=750');";
                        PopupContent.Add(pID, ct);
                    }
                }
            }

            /*****************************************************************************
             * [Todo팝업]
             * Main팝업은 하나만 가능.
             * 게시판 메인팝업을 사용하는 경우, Todo팝업은 일반팝업으로 처리.
             * 게시판 메인팝업을 사용하지 않는 경우, Todo팝업은 메인팝업으로 처리
             ****************************************************************************/
            string isTodo = "N";
            Zeospace.DeskPlus.Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();
            List<UserEnvironmentInfo> list = env.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (list != null)
            {
                foreach (UserEnvironmentInfo info in list)
                {
                    isTodo = (info.IsTodo.Equals(""))? "N" : info.IsTodo;
                }
            }
            if (isTodo.Equals("Y"))
            {
                //  Todo_UserID (cookie key)
                pID = "TODO_" + CurrentUserInfo.UserID;

                if (!MainPopupBoardList.Equals(""))
                {
                    //일반팝업
                    ct = "window.open('/DeskPlusEIP/Portlet/TodoPopup/TodoList.aspx?popid=" + pID + "', 'pop" + pID + "', 'menubar=no, scrollbars=no, status=no, toolbar=no, resizable=no, width=1000, height=510');";
                    PopupContent.Add(pID, ct);
                }
                else
                {
                    //메인팝업
                    TodoPopup = "/DeskPlusEIP/Portlet/TodoPopup/TodoList.aspx?popid=" + pID + "," + pID;
                }
            }
        }
        
        #endregion Method (함수)

        #region Property (프로퍼티)

        public string MailServerIP
        {
            get
            {
                return AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "");
            }
        }

        #endregion Property (프로퍼티)
    }
}