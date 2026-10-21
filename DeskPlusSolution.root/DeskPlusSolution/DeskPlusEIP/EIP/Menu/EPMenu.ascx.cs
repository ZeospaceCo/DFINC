using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.BLL;
using Zeospace.DeskPlus.EIP.BE;
//using Zeospace.DeskPlus.EIP.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Menu
{
    public partial class EPMenu : Base.BaseControlEIP
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region Global Variable (전역 변수)
        protected String ASLink = "";
        protected String OrganTreeLink = "";
        //protected String MyInfoLink = "";
        //protected String SignOutLink = "";
        //protected String AdminLink = "";
        //protected String PersonalLink = "";
        //protected String PersonalOutLink = "";
        protected String NewWinLink = "";
        protected String SMSLink = "";
        
        #endregion

        #region EPContents SQL 상수
        private const string EPCONTENTS_GETBYUNITID_ORACLE = "SQL_DP_WBNT_PContents_GetBynit";
        private const string EPCONTENTS_GETBYUNITID = "SQL_DP_Portlet_EPContents_GetByUnitID";
        private const string PARM_UNITID_ORACLE = "v_UnitID";
        private const string PARM_UNITID = "@UnitID";
        private const string PARM_CV_1 = "cv_1";
        #endregion

        #region Page Process (페이지 처리)
        private void Page_Load(object sender, System.EventArgs e)
        {
            //if (AppSettingHelper.GetAppSetting(AppSettingType.Base, "DefaultLanguage") != "")
            //{
            //    defaultLang = AppSettingHelper.GetAppSetting(AppSettingType.Base, "DefaultLanguage").ToString();
            //}

            //int targetMenuID = 0;

            ////Current 는 현재요청에 대한 HttpContext 의 인스턴스를 반환
            ////요청에 관한 다양한 정보을 얻을 수 있다.
            ////Items 는 현재 페이지 요청안에서 데이터를 공유할 수 있는 유용한 수단을 제공한다.
            ////데이터를 저장하게 되면 현재 요청 중, 어떤 클래스에서도 저장된 데이터를 참고할 수 있다.
            ////따라서 데이터 공유를 하기 위해 사용하고 있다. written by Shimpark
            //EIPPortalInfo eipPortal = (EIPPortalInfo)HttpContext.Current.Items["EIPPortalInfo"];


            //MyInfoLink.Visible = false;
            //SignOutLink.Visible = false;
            //AdminLink.Visible = false;
            //PersonalLink.Visible = false;
            //PersonalOutLink.Visible = false;

            //try
            //{
            //    string loginID = HttpContext.Current.User.Identity.Name;

            //    //쿠키에서 개인 정보를 가져옴
            //    UserInfoHelper userInfo = new UserInfoHelper(loginID);

            //    UserID = userInfo.UserID;

            //    //오늘 날짜 세팅
            //    LoadCalendar();

            //    //메일 로그아웃 페이지 설정
            //    SetMailLogOut();

            //1. 사용자 인증 여부
            if (Request.IsAuthenticated == true)
            {
                MyInfoLink.NavigateUrl = "javascript:OpenUserInfo('/DeskPlusEIP/Account/Admin/UserRegister.aspx?userID=" + CurrentUserInfo.UserID + "')";
                MyInfoLink.Visible = true;

                SignOutLink.NavigateUrl = "/DeskPlusEIP/Site/Auth/LogOut.aspx";
                SignOutLink.Visible = true;


                //MyInfoLink = "<a href=\"javascript:DeskPlusWin('/DeskPlusEIP/Account/Private/MyInfo.aspx','MyInfo',800,527);\" style=\"COLOR: black\"><img src=\"" + this.ImagePath + "/EIP/top_but_myinfo.gif\" border=\"0\"></a>";
                //SignOutLink = "<a href=\"" + Request.ApplicationPath + "/Site/SiteBase/Base/Authentication/LogOut.aspx\" style=\"COLOR: black\"><img src=\"" + this.ImagePath + "/EIP/top_but_logout.gif\" border=\"0\"></a>";

                //1-1. administrator 권한 일경우
                if (CurrentUserInfo.IsInRole("admin") || CurrentUserInfo.IsSysAdmin)  // administrator
                {
                    //1-2 epAdmin 이미지 및 경로 설정
                    AdminLink.NavigateUrl = "/DeskPlusEIP/EIP/Admin/Default.aspx";
                    AdminLink.Visible = true;
                    LiAdminLink.Visible = true;
                    //AdminLink = "<a href=\"" + Request.ApplicationPath + "/EIP/Admin/Default.aspx\" style=\"COLOR: black\"><img src=\"" + this.ImagePath + "/EIP/top_but_ep.gif\" border=\"0\"></a>";
                }
            }

            //    //Menu 초기화

            //    //해당 메뉴가 개인화가 가능하지 체크(activeMenu 는 BE)
            //    if (eipPortal.activeMenu.IsPersonalization)
            //    {
            //        //개인화버튼(개인화 들어갔다나오면 edit 로 파라미터 생성됨)
            //        if (Request.Params["personal"] == null)
            //        {
            //            PersonalLink.NavigateUrl = "/DeskPlusEIP/EIP/Modules/Personal/PersonalAdmin.aspx?personal=mod&parm=" + eipPortal.activeMenu.MenuID + "&p=" + eipPortal.activeMenu.PageID;
            //            PersonalLink.Visible = true;
            //            LiPersonalLink.Visible = true;
            //            //PersonalLink = "<a href=\"" + Request.ApplicationPath + "/EIP/Modules/Personal/PersonalAdmin.aspx?personal=mod&parm=" + eipPortal.activeMenu.MenuID + "&p=" + eipPortal.activeMenu.PageID + "\" style=\"COLOR: white\"><img src=\"" + ImagePath + "/EIP/top_but_admin.gif\" border=\"0\"></a>";
            //        }
            //        else //개인화 나오기버튼
            //        {
            //            PersonalOutLink.NavigateUrl = "/DeskPlusEIP/EIP/EPDefault.aspx?parm=" + eipPortal.activeMenu.MenuID;
            //            PersonalOutLink.Visible = true;
            //            LiPersonalOutLink.Visible = true;
            //            //PersonalOutLink = "<a href=\"" + Request.ApplicationPath + "/EIP/EPDefault.aspx?parm=" + eipPortal.activeMenu.MenuID + "\" style=\"COLOR: white\"><img src=\"" + ImagePath + "/EIP/top_but_mypage.gif\" border=\"0\"></a>";
            //        }
            //    }

            //    //사용자 인증여부
            //    if (Request.IsAuthenticated == true)
            //    {
            //        //본문 새창 띄우기
            //        if (eipPortal.activeMenu.PageUnitArr != null)
            //        {
            //            if ((eipPortal.activeMenu.PageUnitArr.Count == 1) && (((EIPPortletInfo)eipPortal.activeMenu.PageUnitArr[0]).PortletType == "PortletContents"))
            //            {
            //                NewWinLink = MakeFuncArg();
            //            }
            //        }
            //    }

            //    currentMenuID = eipPortal.activeMenu.MenuID;

            //    ArrayList authorizedMenus = new ArrayList();
            //    ArrayList authorizedSubMenus = new ArrayList();

            //    int selectMenuIndex = -1;
            //    int selectSubMenuIndex = -1;
            //    int selectRealSubMenuIndex = -1;
            //    bool isIPAccess = false;

            //    for (int i = 0; i < eipPortal.eipMenus.Count; i++)
            //    {
            //        Zeospace.DeskPlus.EIP.BE.EIPMenuListInfo menu = (Zeospace.DeskPlus.EIP.BE.EIPMenuListInfo)eipPortal.eipMenus[i];

            //        // IP 체크가 허용 되는지 확인
            //        // 확인 되었으면 isIPAccess = true;
            //        if (menu.IPAddress.Length > 0)
            //        {
            //            // IP 체크여부에 따라 isIPAccess 값 변경
            //            try
            //            {
            //                if (menu.IPAddress.IndexOf("~") != -1)
            //                {
            //                    string[] ipAddressStr = menu.IPAddress.Split('~');
            //                    string[] fromAddressStr = ipAddressStr[0].ToString().Split('.');
            //                    string[] toAddressStr = ipAddressStr[1].ToString().Split('.');

            //                    string iPAddress1 = Component.WebComponent.StringHelper.SetFrontDigit(3, fromAddressStr[0].ToString(), '0');
            //                    string iPAddress2 = Component.WebComponent.StringHelper.SetFrontDigit(3, fromAddressStr[1].ToString(), '0');
            //                    string iPAddress3 = Component.WebComponent.StringHelper.SetFrontDigit(3, fromAddressStr[2].ToString(), '0');
            //                    string iPAddress4 = Component.WebComponent.StringHelper.SetFrontDigit(3, fromAddressStr[3].ToString(), '0');

            //                    string iPAddress5 = Component.WebComponent.StringHelper.SetFrontDigit(3, toAddressStr[0].ToString(), '0');
            //                    string iPAddress6 = Component.WebComponent.StringHelper.SetFrontDigit(3, toAddressStr[1].ToString(), '0');
            //                    string iPAddress7 = Component.WebComponent.StringHelper.SetFrontDigit(3, toAddressStr[2].ToString(), '0');
            //                    string iPAddress8 = Component.WebComponent.StringHelper.SetFrontDigit(3, toAddressStr[3].ToString(), '0');

            //                    double fromIPAddress = Convert.ToDouble(iPAddress1 + iPAddress2 + iPAddress3 + iPAddress4);
            //                    double toIPAddress = Convert.ToDouble(iPAddress5 + iPAddress6 + iPAddress7 + iPAddress8);

            //                    double clientIPAddress = Convert.ToDouble(getClientIPAddress());

            //                    if (clientIPAddress >= fromIPAddress && clientIPAddress <= toIPAddress)
            //                    {
            //                        //접근 가능
            //                        isIPAccess = true;
            //                    }
            //                    else
            //                    {
            //                        //접근 불가능
            //                        isIPAccess = false;
            //                    }
            //                }
            //                else
            //                {
            //                    //접근 가능
            //                    isIPAccess = true;
            //                }
            //            }
            //            catch
            //            {
            //                //접근 가능
            //                isIPAccess = true;
            //            }
            //        }
            //        else  // IP 값이 없을 경우, 무조건 접근 가능
            //        {
            //            //접근 가능
            //            isIPAccess = true;
            //        }

            //        if (userInfo.IsInRole(menu.RoleIDList))
            //        {
            //            //						isInRole = true; // 권한이 하나라도 있다면
            //            if (menu.ParentID == 0 && isIPAccess) // 부모 메뉴 위치 지정
            //            {
            //                authorizedMenus.Add(menu);
            //                selectMenuIndex++;
            //            }
            //            else // 자식 메뉴 위치 지정
            //            {
            //                if (eipPortal.activeMenu.ParentID == 0 && menu.ParentID == eipPortal.activeMenu.MenuID && isIPAccess)
            //                {
            //                    if (eipPortal.activeMenu.PageID != -1)
            //                    {
            //                        authorizedSubMenus.Add(menu);
            //                        selectSubMenuIndex++;
            //                    }
            //                }
            //                else
            //                {
            //                    if (menu.ParentID == eipPortal.activeMenu.ParentID && isIPAccess)
            //                    {
            //                        authorizedSubMenus.Add(menu);
            //                        selectSubMenuIndex++;
            //                    }
            //                }


            //                // menu.ParentID == 0 이면 메인 메뉴 ByFreeman 20071220
            //                // ActiveMenu의 ParentID == 0 이면 메인 메뉴이므로 하위 메뉴의 페이지를 선택하게 해야 한다
            //                // 부모 메뉴를 클릭할때와 서브 메뉴를 클릭할때 조건을 나눠야 한다
            //                //if (eipPortal.activeMenu.PageID == menu.PageID && menu.ParentID == eipPortal.activeMenu.ParentID)
            //                if (eipPortal.activeMenu.ParentID == 0) //부모 메뉴
            //                {
            //                    if (eipPortal.activeMenu.PageID == menu.PageID && menu.ParentID == eipPortal.activeMenu.MenuID)
            //                    {
            //                        selectRealSubMenuIndex = selectSubMenuIndex;
            //                    }

            //                }
            //                else
            //                {
            //                    if (eipPortal.activeMenu.PageID == menu.PageID && menu.ParentID == eipPortal.activeMenu.ParentID)
            //                    {
            //                        selectRealSubMenuIndex = selectSubMenuIndex;
            //                    }
            //                }
            //            }



            //            if (menu.MenuID == currentMenuID && isIPAccess)
            //            {
            //                dlMenuList.SelectedIndex = selectMenuIndex;
            //                //dlSubMenuList.SelectedIndex = selectRealSubMenuIndex;
            //            }
            //        }

            //        isIPAccess = false;
            //    }

            //    #region 메뉴 선택
            //    //dlMenuList.SelectedIndex = selectRealMenuIndex;

            //    dlSubMenuList.SelectedIndex = selectRealSubMenuIndex;

            //    if (selectRealSubMenuIndex > -1)
            //    {
            //        for (int j = 0; j < authorizedMenus.Count; j++)
            //        {
            //            if (eipPortal.activeMenu.ParentID == ((Zeospace.DeskPlus.EIP.BE.EIPMenuListInfo)authorizedMenus[j]).MenuID)
            //            {
            //                dlMenuList.SelectedIndex = j;
            //            }
            //        }
            //    }
            //    #endregion
            //    //상위메뉴
            //    if (dlMenuList.SelectedIndex == -1)
            //    {
            //        dlMenuList.SelectedIndex = 0;
            //        if (authorizedMenus.Count > 0)
            //        {
            //            targetMenuID = ((Zeospace.DeskPlus.EIP.BE.EIPMenuListInfo)authorizedMenus[0]).MenuID;
            //        }
            //    }

            //    //하위메뉴
            //    if (dlSubMenuList.SelectedIndex == -1)
            //    {
            //        dlSubMenuList.SelectedIndex = 0;
            //        if (authorizedSubMenus.Count > 0)
            //        {
            //            targetMenuID = ((Zeospace.DeskPlus.EIP.BE.EIPMenuListInfo)authorizedSubMenus[0]).MenuID;
            //        }
            //    }

            //    if (currentMenuID != 0)
            //    {
            //        dlMenuList.DataSource = authorizedMenus;
            //        dlSubMenuList.DataSource = authorizedSubMenus;

            //        dlMenuList.DataBind();
            //        dlSubMenuList.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("EIPMenu_OLD.ascx : ", ex);
            //}

            //if (currentMenuID == 0)
            //{
            //    if (targetMenuID == 0)
            //    {
            //        FormsAuthentication.SignOut();
            //        Response.Redirect(Request.ApplicationPath);
            //        //Response.Redirect("/DeskPlusEIP/EIP/EPDefault.aspx");
            //        //Response.Redirect("/DeskPlusEIP/Html/ErrorMessage/AccessDenied.aspx");
            //    }
            //    else
            //    {
            //        Response.Redirect("/DeskPlusEIP/EIP/EPDefault.aspx?parm=" + targetMenuID);
            //    }
            //}

            ////			if (!isInRole)
            ////			{
            ////				System.Text.StringBuilder sb = new System.Text.StringBuilder();
            ////				sb.Append("<script language='javascript'>\n");
            ////				sb.Append("alert('" + this.GetResource("Modules_Notice_NoAuthority") + "');\n");
            ////				sb.Append("</script>\n\n");
            ////
            ////				if(!Page.ClientScript.IsClientScriptBlockRegistered("MessageDisplay"))
            ////				{
            ////					Page.ClientScript.RegisterStartupScript(this.GetType(),"MessageDisplay", sb.ToString());				
            ////				}
            ////				FormsAuthentication.SignOut();
            ////			}
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 미니달력 설정
        /// </summary>
        private void LoadCalendar()
        {
            //if (Component.WebComponent.StringHelper.GetDBPreLang(PreLang) == 2)
            //{
            //    lbTodayDate.Text = DateTime.Now.DayOfWeek.ToString() + "." + DateTime.Now.ToString("dd") + "." + DateTime.Now.ToString("MM") + "." + DateTime.Now.ToString("yyyy");
            //}
            //else
            //{
            //    lbTodayDate.Text = DateTime.Now.ToString("yyyy") + "." + DateTime.Now.ToString("MM") + "." + DateTime.Now.ToString("dd");
            //}
        }

        /// <summary>
        /// 접속한 ip address 가져오기
        /// </summary>
        /// <returns></returns>
        private string getClientIPAddress()
        {
            //int loop1, loop2;
            //NameValueCollection coll;

            //// Load ServerVariable collection into NameValueCollection object.
            //coll = Request.ServerVariables;
            //// Get names of all keys into a string array. 
            //String[] arr1 = coll.AllKeys;
            //string remoteAddr = "";
            //for (loop1 = 0; loop1 < arr1.Length; loop1++)
            //{
            //    if (arr1[loop1].ToString() == "REMOTE_ADDR")
            //    {
            //        String[] arr2 = coll.GetValues(arr1[loop1]);
            //        for (loop2 = 0; loop2 < arr2.Length; loop2++)
            //        {
            //            remoteAddr = arr2[loop2].ToString();
            //        }
            //    }
            //}


            string remoteAddr = Request.UserHostAddress;
            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("UserHostAddress : " + remoteAddr, AppSettingType.Site);

            //if (remoteAddr.Length > 0)
            //{
            //    string[] remoteAddrStr = remoteAddr.Split('.');

            //    string iPAddress1 = Component.WebComponent.StringHelper.SetFrontDigit(3, remoteAddrStr[0].ToString(), '0');
            //    string iPAddress2 = Component.WebComponent.StringHelper.SetFrontDigit(3, remoteAddrStr[1].ToString(), '0');
            //    string iPAddress3 = Component.WebComponent.StringHelper.SetFrontDigit(3, remoteAddrStr[2].ToString(), '0');
            //    string iPAddress4 = Component.WebComponent.StringHelper.SetFrontDigit(3, remoteAddrStr[3].ToString(), '0');

            //    remoteAddr = iPAddress1 + iPAddress2 + iPAddress3 + iPAddress4;

            //    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("remoteAddr : " + remoteAddr, AppSettingType.Site);
            //}

            return remoteAddr;
        }


        private string MakeFuncArg()
        {
            //EIPPortalInfo eipPortal = (EIPPortalInfo)HttpContext.Current.Items["EIPPortalInfo"];
            //string targetURL = "";
            //string targetParams = "";
            //string newWinOpenArg = "";
            //string param = "";

            //string siteName = ConfigurationManager.AppSettings["SiteName"].ToString();

            //if (ConfigurationManager.AppSettings["EIP"] == "DALORACLE")
            //{
            //    OracleParameter[] parm = new OracleParameter[] { new OracleParameter(PARM_UNITID_ORACLE, OracleType.Number),
            //                                                 new OracleParameter(PARM_CV_1, OracleType.Cursor) };

            //    parm[0].Value = Convert.ToInt32(((EIPPortletInfo)eipPortal.activeMenu.PageUnitArr[0]).UnitID);
            //    parm[1].Direction = ParameterDirection.Output;


            //    using (OracleDataReader dr = OracleHelper.ExecuteReader(OracleHelper.CONN_SOLUTION, CommandType.StoredProcedure, EPCONTENTS_GETBYUNITID_ORACLE, parm))
            //    {
            //        while (dr.Read())
            //        {
            //            targetURL = dr["TargetURL"].ToString();

            //            if (targetURL.IndexOf("$SiteName$") > 0)
            //            {
            //                targetURL = targetURL.Replace("$SiteName$", siteName);
            //            }

            //            //targetURL = dr["TargetURL"].ToString();
            //            targetParams = dr["TargetParams"].ToString();
            //        }
            //    }
            //}
            //else
            //{
            //    SqlParameter[] parm = new SqlParameter[] { new SqlParameter(PARM_UNITID, SqlDbType.Int) };
            //    parm[0].Value = Convert.ToInt32(((EIPPortletInfo)eipPortal.activeMenu.PageUnitArr[0]).UnitID);


            //    using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, EPCONTENTS_GETBYUNITID, parm))
            //    {
            //        while (dr.Read())
            //        {
            //            targetURL = dr["TargetURL"].ToString();

            //            if (targetURL.IndexOf("$SiteName$") > 0)
            //            {
            //                targetURL = targetURL.Replace("$SiteName$", siteName);
            //            }

            //            //targetURL = dr["TargetURL"].ToString();
            //            targetParams = dr["TargetParams"].ToString();
            //        }
            //    }
            //}

            //if (targetURL.ToString().ToLower() == "/comframe/linkpage.asp")
            //{
            //    Hashtable paramHash = MakeParameter(targetParams);

            //    string getValue = "";

            //    foreach (DictionaryEntry ph in paramHash)
            //    {
            //        if (param.Length > 0) param += "&";
            //        param += ph.Key + "=" + ph.Value;

            //        if (ph.Key.ToString().ToLower() == "getkey")
            //        {
            //            getValue = ph.Value.ToString();
            //        }
            //    }

            //    newWinOpenArg = "<a href=\"javascript:openDeskPlusWin('" + getValue + "','" + param + "');\" style=\"COLOR: white\"><img src=\"" + this.ImagePath + "/Site/Menu/top_but_myinfo.gif\" border=\"0\" style=\"position:relative;top:2px;\"></a>";
            //}
            //else
            //{
            //    newWinOpenArg = "<a href=\"javascript:openContentWin('" + ((EIPPortletInfo)eipPortal.activeMenu.PageUnitArr[0]).UnitID + "','" + ((EIPPortletInfo)eipPortal.activeMenu.PageUnitArr[0]).PortletName + "','" + ((EIPPortletInfo)eipPortal.activeMenu.PageUnitArr[0]).DesignWidth + "','" + ((EIPPortletInfo)eipPortal.activeMenu.PageUnitArr[0]).DesignHeight + "');\" style=\"COLOR: white\"><img src=\"" + this.ImagePath + "/Site/Menu/top_but_myinfo.gif\" border=\"0\" style=\"position:relative;top:2px;\"></a>";
            //}

            ////return newWinOpenArg;

            return "";
        }

        /// <summary>
        /// Contents를 생성할때 넘겨줄 Parameter 정보를 생성
        /// </summary>
        public Hashtable MakeParameter(string _targetParams)
        {
            Hashtable paramHash = new Hashtable();

            char[] delimStr1 = new char[] { '&' };
            char[] delimStr2 = new char[] { '=' };

            string[] param1Arr = _targetParams.Split(delimStr1);
            

            //for (int i = 0; i < param1Arr.Length; i++)
            //{
            //    paramTmp = param1Arr[i];
            //    param2Arr = paramTmp.Split(delimStr2);
            //    paramName = param2Arr[0].ToString();		//이름
            //    if (param2Arr.Length > 1) paramValueStr = param2Arr[1].ToString();		//값

            //    if (paramValueStr.StartsWith("#"))
            //    {
            //        isEnc = paramValueStr.Substring(1, 1);
            //        paramValue = paramValueStr.Substring(3, paramValueStr.Length - 3);

            //        //SSO 관련 데이터 가져오기
            //        SSO eipSSO = new SSO();
            //        SSOInfo ssoInfo = eipSSO.GetSSO(Context.User.Identity.Name);
            //        switch (paramValueStr.Substring(2, 1).ToUpper())
            //        {
            //            case "L":
            //                switch (paramValue)		//사용자 정보(21개)
            //                {
            //                    case "userid":
            //                        paramValue = ssoInfo.UserID.ToString();
            //                        break;
            //                    case "displayName":
            //                        paramValue = ssoInfo.DisplayName;
            //                        break;
            //                    case "employeeNumber":
            //                        paramValue = ssoInfo.EmployeeNumber;
            //                        break;
            //                    case "departmentNumber":
            //                        paramValue = ssoInfo.DepartmentNumber;
            //                        break;
            //                    case "mail":
            //                        paramValue = ssoInfo.Mail;
            //                        break;
            //                    case "userPassword":
            //                        paramValue = ssoInfo.UserPassword;
            //                        break;
            //                    case "sn":
            //                        paramValue = ssoInfo.SN;
            //                        break;
            //                    case "givenName":
            //                        paramValue = ssoInfo.GivenName;
            //                        break;
            //                    case "initials":
            //                        paramValue = ssoInfo.Initials;
            //                        break;
            //                    case "title":
            //                        paramValue = ssoInfo.Title;
            //                        break;
            //                    case "telephoneNumber":
            //                        paramValue = ssoInfo.TelephoneNumber;
            //                        break;
            //                    case "facsimileTelephoneNumber":
            //                        paramValue = ssoInfo.FacsimileTelephoneNumber;
            //                        break;
            //                    case "mobile":
            //                        paramValue = ssoInfo.Mobile;
            //                        break;
            //                    case "roomNumber":
            //                        paramValue = ssoInfo.RoomNumber;
            //                        break;
            //                    case "carLicense":
            //                        paramValue = ssoInfo.CarLicense;
            //                        break;
            //                    case "o":
            //                        paramValue = ssoInfo.O;
            //                        break;
            //                    case "ou":
            //                        paramValue = ssoInfo.OU;
            //                        break;
            //                    case "employeeType":
            //                        paramValue = ssoInfo.EmployeeNumber;
            //                        break;
            //                    case "preferredLanguage":
            //                        paramValue = ssoInfo.PreferredLanguage;
            //                        break;
            //                    case "labeledURI":
            //                        paramValue = ssoInfo.LabeledURI;
            //                        break;
            //                    case "description":
            //                        paramValue = ssoInfo.Description;
            //                        break;
            //                }
            //                break;
            //            case "U":

            //                break;
            //        }

            //        if (isEnc == "1") // 암호화 할것
            //        {
            //            paramValue = Zeospace.DeskPlus.Component.Security.Protector.Enc(paramValue);
            //        }

            //        paramHash.Add(paramName, paramValue);
            //    }
            //}

            return paramHash;
        }


        public string GetMenuName(string _menuName, string _menuEnglishName)
        {
            string titleName = String.Empty;

            //if (Component.WebComponent.StringHelper.GetDBPreLang(PreLang) == 2)
            //{
            //    titleName = _menuEnglishName;
            //}
            //else
            //{
            //    titleName = _menuName;
            //}

            //if (titleName == "")
            //{
            //    return "No Title";
            //}
            //else
            //{
                return titleName;
            //}
        }

        /// <summary>
        /// 메일을 종류를 체크하여 메일쪽 로그아웃 페이지 호출
        /// </summary>
        private void SetMailLogOut()
        {
            //string mailType = ConfigurationManager.AppSettings["MAIL"].ToString();
            //if (!String.IsNullOrEmpty(mailType))
            //{
            //    if (mailType.ToLower().Equals("zettamail"))
            //    {
            //        MailServerIP = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.Mail, "MailServerIP", "");
            //    }
            //}
        }
        #endregion

        #region Property (프로퍼티)
        public string MessengerID
        {
            get
            {
                string messengerID = HttpContext.Current.User.Identity.Name;
                messengerID = Convert.ToBase64String(System.Text.Encoding.GetEncoding("euc-kr").GetBytes(messengerID));
                //messengerID = System.Text.Encoding.GetEncoding("euc-kr").GetString(System.Convert.FromBase64String(messengerID));
                return messengerID;
            }
        }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get { return GetViewStateInt("UserID", "0"); }
            set { ViewState["UserID"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DptList
        {
            get { return this.GetViewStateString("DptList", ""); }
            set { ViewState["DptList"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DptList_Now
        {
            get { return this.GetViewStateString("DptList_Now", ""); }
            set { ViewState["DptList_Now"] = value; }
        }

        public string MailServerIP
        {
            get { return this.GetViewStateString("MailServerIP", ""); }
            set { ViewState["MailServerIP"] = value; }
        }

        /// <summary>
        /// 작성자   : Freeman
        /// 작성일자 : 2011-01-13
        /// 작성목적 : 바탕화면 바로가기에 사용할 FullDomain 정보
        /// 수정일자 : 
        /// 수정내용 : 
        /// </summary>
        //public string FullDomain
        //{
        //    get
        //    {
        //        return Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.Base, "FullDomain");
        //    }
        //}

        /// <summary>
        /// Gets my role.
        /// </summary>
        /// <value></value>
        public string MyRole
        {
            get
            {
                string roles = "";

                if (Request.IsAuthenticated == true)
                {
                    string loginID = HttpContext.Current.User.Identity.Name;
                    UserInfoHelper userInfo = new UserInfoHelper(loginID);
                    ArrayList roleArr = userInfo.RolesList;
                    for (int i = 0; i < roleArr.Count; i++)
                    {
                        roles += roleArr[i].ToString() + "/";
                    }

                    return "/" + roles;
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region Web Form 디자이너에서 생성한 코드
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		디자이너 지원에 필요한 메서드입니다. 이 메서드의 내용을
        ///		코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion

    }
}
