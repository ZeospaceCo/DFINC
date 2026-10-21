using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.Web.Package
{
    public partial class NMenu : Base.BasePageEIP
    {
        #region 변수설정

        public string TopMenu = "";
        public string UserDeptName = "";
        public int BaseDeptCnt = 0;
        public string MenuImage;
        public string ApprovalStatusNewMenuDirectCall = "false";

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Package.NMenu).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
                LoadPhoto();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadCompany();
            LoadTopMenu();
            TopNavigator();
        }

        /// <summary>
        /// 회사정보 
        /// </summary>
        private void LoadCompany()
        {
            string MenuImageCnt = "";
            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = new List<DeskPlus.Common.BE.CompanyInfo>();
            Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
            al = dal.GetCompany(CurrentUserInfo.CompanyID, false);

            if (al.Count > 0)
            {
                foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo info in al)
                {
                    MenuImageCnt = info.Logo;
                }
            }

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");

            //기본 이미지
            MenuImage = this.BaseImgPath + "/Portal/logo.gif";

            //회사별 이미지
            string ciMenuImage = baseFileDirectory + CurrentUserInfo.CompanyID + "/logo.png";

            if (System.IO.File.Exists(Server.MapPath(ciMenuImage)))
            {
                MenuImage = ciMenuImage + "?v=" + MenuImageCnt;
            }

            /*
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", MenuImage);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            MenuImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
            */

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 나의 권한 시작 텝 아이디 정보
        /// </summary>
        private int GetFirstTabID()
        {
            DAL.Tab dal = new DAL.Tab();
            List<TabInfo> al = dal.GetTabByRole(CurrentUserInfo.CompanyID, string.Join(",", CurrentUserInfo.RoleIDs), CurrentUserInfo.UserID);

            if (al.Count > 1) return 0; //ByFreeman 20161129 여러개의 탭일경우 시작탭으로 설정된 사항 우선 적용 

            int firstTabID = 0;
            foreach (TabInfo info in al)
            {
                firstTabID = info.TabID;
                break;
            }

            return firstTabID;
        }

        /// <summary>
        /// 상단 메뉴
        /// </summary>
        private void LoadTopMenu()
        {
            #region IP 체크

            string[] remoteAddrStr = Request.UserHostAddress.Split('.');
            string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

            BLL.Menu biz = new BLL.Menu();
            List<string> alMenuUser = biz.GetMenuListUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, remoteAddr.ToDouble<double>());

            if (alMenuUser == null)
            {
                string url = @"top.location='{0}';";
                url = string.Format(url, "/DeskPlusEIP/Site/Auth/Logout.aspx");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "NOMENU", GetResource("AccessFail"), "");
                return;
            }

            #endregion IP 체크

            //** 사용자가 볼수 있는 메뉴 정보 가져오기
            DataView dv = biz.GetMenuUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, remoteAddr.ToDouble<double>());
            if (dv == null)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Main[LoadTopMenu] GetMenuUser Is Null : " + CurrentUserInfo.UserID);
                Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                bizMenu.ClearCacheMenuUser(CurrentUserInfo.CompanyID);
                //사용자별 메뉴 정보 - 캐쉬설정
                bizMenu.SetCacheMenuListUser(CurrentUserInfo.CompanyID);

                dv = biz.GetMenuUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, remoteAddr.ToDouble<double>());
            }

            StringBuilder sb = new StringBuilder();
            BLL.MenuDataLink bizMDL = new BLL.MenuDataLink();
            DAL.MenuDataLink dal = new DAL.MenuDataLink();
            List<BE.LDAPInfo> ldapInfo = dal.GetLdap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            bool useFirstStyle = true;
            string firstMenuID = "";

            try
            {
                if (dv != null)
                {
                    for (int i = 0; i < dv.Count; i++)
                    {
                        if (dv[i]["ParentMenuID"].ToString().ToNull() == "")
                        {
                            string[] parameters = bizMDL.GetMenuParameter(CurrentUserInfo.CompanyID,
                                dv[i]["MenuID"].ToString().ToNumber<int>(),
                                CurrentUserInfo.UserID, ldapInfo[0]);

                            string directCall = dv[i]["DirectCall"].ToString();
                            string url = GetApprovalStatusMenuUrl(dv[i]["TargetURL"].ToString(), directCall);
                            string param = String.Empty;
                            string dcParam = String.Empty;

                            if (!string.IsNullOrEmpty(url)) //TargetURL이 없으면 하위 메뉴의 첫번째를 클릭하게 한다.
                            {
                                if (parameters[0].Length > 0 || parameters[1].Length > 0 || !string.IsNullOrEmpty(directCall)) url = url + "?";

                                if (!string.IsNullOrEmpty(directCall))
                                {
                                    //home
                                    if (directCall.ToLower() == "home")
                                    {
                                        dcParam = "tabID=" + GetFirstTabID();
                                    }
                                    else
                                    {
                                        dcParam = "dcparm=" + directCall;
                                    }
                                }

                                if (!string.IsNullOrEmpty(parameters[0])) param = parameters[0];   // 메뉴 호출시 암호화된 파라미터
                                if (!string.IsNullOrEmpty(dcParam))  //dcparam이 존재하면 추가(호출 메뉴를 알기 위해서)
                                {
                                    if (!string.IsNullOrEmpty(param)) param += "&";
                                    param += dcParam;
                                }

                                if (!string.IsNullOrEmpty(param)) param = "p=" + GetEncryptParam(param);

                                if (!string.IsNullOrEmpty(parameters[1]))  // 메뉴 호출시 암호화되지 않은 파라미터
                                {
                                    if (!string.IsNullOrEmpty(param)) param += "&";
                                    param = param + parameters[1];
                                }

                                url = url + param;
                            }

                            #region 다국어 이름 설정

                            string name = this.GetLangName(
                                dv[i]["NameBase"].ToString(),
                                dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                                dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(), dv[i]["NameZhTw"].ToString(), PreLang).ToReverseXSS();

                            #endregion 다국어 이름 설정

                            if (useFirstStyle)
                            {
                                useFirstStyle = false;
                                firstMenuID = dv[i]["MenuID"].ToString();
                            }

                            if (dv[i]["IconYN"].ToString() == "Y")
                            {
                                if (dv[i]["DirectCall"].ToString() == "home")
                                {
                                    sb.Append(string.Format(@"<li><a id=""mn_{1}"" href=""#"" class=""dropdown-item"" onClick=""javascript:Menu.CallTopMenu('{0}','{1}','{2}')"" title=""{3}"">
                                        <i class=""{4}"" ></i></a>
                                        <ul class=""dropdown-item-submenu"" id=""umenu_{1}""><li class=""titleN"">
                                        <a id=""hmn_{1}"" href=""#"" onClick=""javascript:Menu.SettingFaMenu()""><i class=""fas fa-cog""></i></a></li>",
                                            url,
                                            dv[i]["MenuID"].ToString(),
                                            dv[i]["OpenUrl"].ToString(),
                                            name,
                                            dv[i]["ClassName"].ToString()));
                                }
                                else
                                {
                                    sb.Append(string.Format(@"<li><a id=""mn_{1}"" href=""#"" class=""dropdown-item"" onClick=""javascript:Menu.CallTopMenu('{0}','{1}','{2}')"" title=""{3}"">
                                        <i class=""{4}"" ></i></a>
                                        <ul class=""dropdown-item-submenu"" id=""umenu_{1}""><li class=""titleN"">{3}</li>",
                                            url,
                                            dv[i]["MenuID"].ToString(),
                                            dv[i]["OpenUrl"].ToString(),
                                            name,
                                            dv[i]["ClassName"].ToString()));
                                }
                            }
                            else
                            {
                                if (dv[i]["DirectCall"].ToString() == "home")
                                {
                                    sb.Append(string.Format(@"<li><a id=""mn_{1}"" href=""#"" class=""dropdown-item"" onClick=""javascript:Menu.CallTopMenu('{0}','{1}','{2}')"" title=""{3}"">
                                        {3}</a>
                                        <ul class=""dropdown-item-submenu"" id=""umenu_{1}""><li class=""titleN"">
                                        <a id=""hmn_{1}"" href=""#"" onClick=""javascript:Menu.SettingFaMenu()""><i class=""fas fa-cog""></i></a></li>",
                                            url,
                                            dv[i]["MenuID"].ToString(),
                                            dv[i]["OpenUrl"].ToString(),
                                            name));
                                }
                                else
                                {
                                    sb.Append(string.Format(@"<li><a id=""mn_{1}"" href=""#"" class=""dropdown-item"" onClick=""javascript:Menu.CallTopMenu('{0}','{1}','{2}')"" title=""{3}"">
                                        {3}</a>
                                        <ul class=""dropdown-item-submenu"" id=""umenu_{1}""><li class=""titleN"">{3}</li>",
                                            url,
                                            dv[i]["MenuID"].ToString(),
                                            dv[i]["OpenUrl"].ToString(),
                                            name));
                                }
                            }

                            if (dv[i]["DirectCall"].ToString() == "home")
                            {
                                HomeID = dv[i]["MenuID"].ToString();
                                sb.Append(GetSubMenu(dv, dv[i]["MenuID"].ToString().ToNumber<int>(), remoteAddr, "Y"));
                            }
                            else
                            {
                                sb.Append(GetSubMenu(dv, dv[i]["MenuID"].ToString().ToNumber<int>(), remoteAddr, "N"));
                            }

                            sb.Append("</ul></li>");

                            //Direct Call ByFreeman 20120116
                            if (!string.IsNullOrEmpty(DirectCall) && dv[i]["DirectCall"].ToString().ToLower() == DirectCall.ToLower())
                            {
                                DirectCallMenuID = dv[i]["MenuID"].ToString() + "|";
                            }
                        }
                        else
                        {
                            //Direct Call ByFreeman 20120116
                            if (!string.IsNullOrEmpty(DirectCall) && dv[i]["DirectCall"].ToString().ToLower() == DirectCall.ToLower())
                            {
                                DirectCallMenuID = dv[i]["ParentMenuID"].ToString() + "|" + dv[i]["MenuID"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Main[LoadTopMenu] : " + ex);
            }

            //DirectCall 이 없으면 처음 메뉴로 할당한다. (그래야 하위메뉴까지 출력된다.) - SHIMPARK 201304017
            //if (DirectCall.Length == 0) DirectCallMenuID = firstMenuID + "|";

            //if (DirectCall.Length == 0)
            if (DirectCallMenuID.Length == 0)
            {
                DirectCallMenuID = firstMenuID + "|";
                DirectCall = "FirstMenu";
            }

            if (DirectCall == "homeFa") DirectCallMenuID = firstMenuID + "|";

            TopMenu = sb.ToString();
        }

        /// <summary>
        /// 하단 메뉴 
        /// </summary>
        private string GetSubMenu(DataView _dv, int _menuID, string _remoteAddr, string _homeYN)
        {
            string rtn = "";

            BLL.Menu biz = new BLL.Menu();
            BLL.MenuDataLink bizMDL = new BLL.MenuDataLink();
            DAL.MenuDataLink dal = new DAL.MenuDataLink();
            List<BE.LDAPInfo> ldapInfo = dal.GetLdap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            DataTable dt = _dv.ToTable();
            DataView dvSub = dt.DefaultView;
            dvSub.RowFilter = "ParentMenuID = " + _menuID;

            if (dvSub != null)
            {
                for (int i = 0; i < dvSub.Count; i++)
                {
                    if (dvSub[i]["ParentMenuID"].ToString().ToNull().ToNumber<int>() == _menuID)
                    {
                        string[] parameters = bizMDL.GetMenuParameter(CurrentUserInfo.CompanyID, dvSub[i]["MenuID"].ToString().ToNumber<int>(), CurrentUserInfo.UserID, ldapInfo[0]);
                        string directCall = dvSub[i]["DirectCall"].ToString();
                        string url = GetApprovalStatusMenuUrl(dvSub[i]["TargetURL"].ToString(), directCall);
                        string param = String.Empty;
                        string dcParam = String.Empty;

                        if (!string.IsNullOrEmpty(url)) //TargetURL이 없으면 하위 메뉴의 첫번째를 클릭하게 한다.
                        {
                            if (parameters[0].Length > 0 || parameters[1].Length > 0 || !string.IsNullOrEmpty(directCall)) url = url + "?";

                            if (!string.IsNullOrEmpty(directCall))
                            {
                                dcParam = "dcparm=" + directCall;
                            }

                            if (!string.IsNullOrEmpty(parameters[0])) param = parameters[0];   // 메뉴 호출시 암호화된 파라미터
                            if (!string.IsNullOrEmpty(dcParam))  //dcparam이 존재하면 추가(호출 메뉴를 알기 위해서)
                            {
                                if (!string.IsNullOrEmpty(param)) param += "&";
                                param += dcParam;
                            }

                            if (!string.IsNullOrEmpty(param)) param = "p=" + GetEncryptParam(param);

                            if (!string.IsNullOrEmpty(parameters[1]))  // 메뉴 호출시 암호화되지 않은 파라미터 
                            {
                                if (!string.IsNullOrEmpty(param)) param += "&";
                                param = param + parameters[1];
                            }

                            url = url + param;
                        }

                        #region 다국어 이름 설정
                        string name = this.GetLangName(
                            dvSub[i]["NameBase"].ToString(),
                            dvSub[i]["NameKo"].ToString(), dvSub[i]["NameEn"].ToString(), dvSub[i]["NameJa"].ToString(),
                            dvSub[i]["NameFr"].ToString(), dvSub[i]["NameZhCn"].ToString(), dvSub[i]["NameZhTw"].ToString(), PreLang);
                        #endregion

                        rtn += string.Format(@"<li><a id=""smn_{1}"" href=""#"" onClick=""javascript:Menu.CallSubMenu('{0}','{1}','{2}')"">{3}</a></li>",
                                url,
                                dvSub[i]["MenuID"].ToString(),
                                dvSub[i]["OpenUrl"].ToString(),
                                name);
                    }
                }
            }

            if (_homeYN == "Y")
            {
                DataView dfa = biz.GetFaMenuUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, _remoteAddr.ToDouble<double>());

                if (dfa != null)
                {
                    for (int i = 0; i < dfa.Count; i++)
                    {
                        string[] parameters = bizMDL.GetMenuParameter(CurrentUserInfo.CompanyID, dfa[i]["MenuID"].ToString().ToNumber<int>(), CurrentUserInfo.UserID, ldapInfo[0]);
                        string directCall = dfa[i]["DirectCall"].ToString();
                        string url = GetApprovalStatusMenuUrl(dfa[i]["TargetURL"].ToString(), directCall);
                        string param = String.Empty;
                        string dcParam = String.Empty;

                        if (!string.IsNullOrEmpty(url)) //TargetURL이 없으면 하위 메뉴의 첫번째를 클릭하게 한다.
                        {
                            if (parameters[0].Length > 0 || parameters[1].Length > 0 || !string.IsNullOrEmpty(directCall)) url = url + "?";

                            if (!string.IsNullOrEmpty(directCall))
                            {
                                dcParam = "dcparm=" + directCall;
                            }

                            if (!string.IsNullOrEmpty(parameters[0])) param = parameters[0];   // 메뉴 호출시 암호화된 파라미터
                            if (!string.IsNullOrEmpty(dcParam))  //dcparam이 존재하면 추가(호출 메뉴를 알기 위해서)
                            {
                                if (!string.IsNullOrEmpty(param)) param += "&";
                                param += dcParam;
                            }

                            if (!string.IsNullOrEmpty(param)) param = "p=" + GetEncryptParam(param);

                            if (!string.IsNullOrEmpty(parameters[1]))  // 메뉴 호출시 암호화되지 않은 파라미터 
                            {
                                if (!string.IsNullOrEmpty(param)) param += "&";
                                param = param + parameters[1];
                            }

                            url = url + param;
                        }

                        #region 다국어 이름 설정
                        string name = this.GetLangName(
                            dfa[i]["NameBase"].ToString(),
                            dfa[i]["NameKo"].ToString(), dfa[i]["NameEn"].ToString(), dfa[i]["NameJa"].ToString(),
                            dfa[i]["NameFr"].ToString(), dfa[i]["NameZhCn"].ToString(), dfa[i]["NameZhTw"].ToString(), PreLang);
                        #endregion

                        rtn += string.Format(@"<li id=""lifa_{1}""><a id=""smn_{1}"" href=""#"" onClick=""javascript:Menu.CallSubMenu('{0}','{1}','{2}')"">{3}</a>  
                                                <button type=""button"" class=""btn btn-link text-white"" style=""padding:0px 2px 2px 0px;"" onClick=""javascript:Menu.DelFaMenu('{1}')"">                                              
                                                <a href=""#""><i class=""fas fa-times dropbtn""></i></a></button></li>",
                                url,
                                dfa[i]["MenuID"].ToString(),
                                dfa[i]["OpenUrl"].ToString(),
                                name);
                    }
                }

            }

            return rtn;
        }

        /// <summary>
        /// 상단 네비게이션
        /// </summary>
        private void TopNavigator()
        {
            if (CurrentUserInfo.IsInRole("admin") || CurrentUserInfo.IsSysAdmin)  // administrator
            {
                lnkAdmin.Visible = true;
                lnkAdmin.NavigateUrl = "/DeskPlusEIP/EIP/Admin/Default.aspx";
                lnkAdmin.Text = "Admin";
                //lnkAdmin.Attributes.Add("style", "text-decoration:none");
                sadmin.Visible = true;
            }

            Zeospace.DeskPlus.Account.DAL.UserCompany userComBiz = new Account.DAL.UserCompany();
            List<Zeospace.DeskPlus.Account.BE.UserCompanyInfo> userComList = userComBiz.GetUserCompany(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            string groupGUID = "";
            if (userComList.Count > 0) groupGUID = userComList[0].GroupGUID;

            Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();

            int cnt = 0;
            StringBuilder sbUSer = new StringBuilder();

            //타회사정보가 존재할 경우
            if (groupGUID.Length > 0)
            {
                userComList = null;
                userComList = userComBiz.GetUserCompanyByGUID(groupGUID);
                //타회사 겸직 정보가 존재할 경우
                if (userComList.Count > 0)
                {
                    foreach (var comInfo in userComList)
                    {
                        List<UserInfo> alUser = dal.GetUserInfo(comInfo.CompanyID, comInfo.UserID, true, false);

                        foreach (var info in alUser)
                            sbUSer.Append(string.Format(@"<li><a href=""#info"" onclick=""Menu.SetBaseDept({0},{1},{2},{3},{4},'{5}')"">{6} {7} {8} {9}</a></li>",
                                info.CompanyID, info.UserID, info.DeptID, info.CodeTitleID, info.CodeDutyID, info.LogInID,
                                this.GetLangName(info.ComBase, info.ComKo, info.ComEn, info.ComJa, info.ComFr, info.ComZhcn, info.ComZhtw, PreLang).ToReverseXSS(),
                                this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhCn, info.DeptZhTw, PreLang).ToReverseXSS(),
                                this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhCn, info.TitleZhTw, PreLang).ToReverseXSS(),
                                this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhCn, info.NameZhTw, PreLang).ToReverseXSS()));

                        cnt += alUser.Count;
                    }
                }
            }
            else
            {
                //타회사 겸직 정보가 없다면, 쿠키정보의 인사정보로 처리한다.
                List<UserInfo> alUser = dal.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, false);

                foreach (var info in alUser)
                    sbUSer.Append(string.Format(@"<li><a href=""#info"" onclick=""Menu.SetBaseDept({0},{1},{2},{3},{4},'{5}')"">{6} {7} {8} {9}</a></li>",
                                info.CompanyID, info.UserID, info.DeptID, info.CodeTitleID, info.CodeDutyID, info.LogInID,
                                this.GetLangName(info.ComBase, info.ComKo, info.ComEn, info.ComJa, info.ComFr, info.ComZhcn, info.ComZhtw, PreLang).ToReverseXSS(),
                                this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhCn, info.DeptZhTw, PreLang).ToReverseXSS(),
                                this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhCn, info.TitleZhTw, PreLang).ToReverseXSS(),
                                CurrentUserInfo.Name));

                cnt += alUser.Count;
            }

            UserDeptName = sbUSer.ToString();
            BaseDeptCnt = cnt;
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            DirectCall = GetParm("d", ""); //DirectCall 은 암호화를 하지 않는다
            TargetParam = GetParm("t", "").Replace("|", "=").Replace(";", "&"); //추가 파라미터는 암호화를 하지 않는다
            TargetParam = GetApprovalStatusMenuTargetParam(DirectCall, TargetParam);
            ApprovalStatusNewMenuDirectCall = IsApprovalStatusDirectCall(DirectCall) && IsApprovalStatusNewMenu() ? "true" : "false";
        }

        /// <summary>
        /// 전자결재 신메뉴 사용자의 DirectCall URL 보정
        /// </summary>
        private string GetApprovalStatusMenuUrl(string _url, string _directCall)
        {
            return _url;
        }

        /// <summary>
        /// 전자결재 신메뉴 사용자의 DirectCall 추가 파라미터 보정
        /// </summary>
        private string GetApprovalStatusMenuTargetParam(string _directCall, string _targetParam)
        {
            if (!IsApprovalStatusDirectCall(_directCall)) return _targetParam;
            if (!IsApprovalStatusNewMenu()) return _targetParam;

            if (_directCall.ToNull().Equals("approvalComplete", StringComparison.OrdinalIgnoreCase))
            {
                if (GetTargetParamValue(_targetParam, "folderStatusID").Length > 0) return _targetParam;

                string cabinetID = GetTargetParamValue(_targetParam, "cabinetID");
                return "folderStatusID=" + GetApprovalCompleteFolderStatusID(cabinetID);
            }

            return _targetParam;
        }

        private bool IsApprovalStatusDirectCall(string _directCall)
        {
            return _directCall.ToNull().Equals("approvalStatus", StringComparison.OrdinalIgnoreCase)
                || _directCall.ToNull().Equals("approvalComplete", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsApprovalStatusDirectCallRequest(string _directCall)
        {
            return IsApprovalStatusDirectCall(_directCall)
                && DirectCall.ToNull().Equals(_directCall.ToNull(), StringComparison.OrdinalIgnoreCase);
        }

        private bool IsApprovalStatusNewMenu()
        {
            string menuType = GetViewStateString("ApprovalStatusMenuType", "");
            if (menuType.Length == 0)
            {
                menuType = "OLD";

                Account.DAL.UserEnvironment dal = new Account.DAL.UserEnvironment();
                List<UserEnvironmentInfo> list = dal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                if (list != null && list.Count > 0)
                {
                    menuType = list[0].ApprovalStatusMenuType.ToNull();
                }

                ViewState["ApprovalStatusMenuType"] = menuType;
            }

            return menuType.Equals("NEW", StringComparison.OrdinalIgnoreCase);
        }

        private string GetTargetParamValue(string _targetParam, string _key)
        {
            if (_targetParam.ToNull().Length == 0) return "";

            string[] paramList = _targetParam.Split('&');
            foreach (string param in paramList)
            {
                string[] keyValue = param.Split('=');
                if (keyValue.Length > 1 && keyValue[0].Equals(_key, StringComparison.OrdinalIgnoreCase))
                {
                    return keyValue[1];
                }
            }

            return "";
        }

        private string GetApprovalCompleteFolderStatusID(string _cabinetID)
        {
            switch (_cabinetID.ToNull().ToUpper())
            {
                case "PRIVATE":
                    return "PrivateCompleteAll";
                case "APPROVAL":
                    return "ApproveCompleteAll";
                case "REJECT":
                    return "ApproveRejectAll";
                case "ARBITRARY":
                    return "ArbitraryAll";
                case "CARBONCOPY":
                case "CARBONCOPY_DOC":
                    return "CarboncopyAll";
                case "CARBONCOPY_FORWARD":
                    return "CarboncopyForwardAll";
                case "CARBONCOPY_CIRRECEIPT":
                    return "CarboncopyCirReceiptAll";
                case "CARBONCOPY_CIRSEND":
                    return "CarboncopyCirSendAll";
                case "DEPT":
                    return "DeptCompleteAll";
                default:
                    return "ApproveCompleteAll";
            }
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 사진 이미지 가져오기
        /// </summary>
        private void LoadPhoto()
        {
            string photo = "";
            string accessDate = "";
            string accessTime = "";

            Account.DAL.User dal = new Account.DAL.User();
            DataTableReader dtr = dal.GetMyInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        photo = dtr["PhotoImage"].ToString();

                        //접속시간 보여주기
                        if (dtr["Access"] != null)
                        {
                            #region 최종접속일
                            accessDate = Convert.ToDateTime(dtr["Access"]).ToShortDateString();
                            accessTime = Convert.ToDateTime(dtr["Access"]).ToShortTimeString();
                            DateTime dt;

                            if (DateTime.TryParse(accessDate, out dt))
                                accessDate = DateHelper.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);

                            accessDate = accessDate.Replace("-", ".").Replace("/", ".");
                            accessTime = accessTime.Replace(resMng.GetString("AM"), "AM").Replace(resMng.GetString("PM"), "PM");
                            #endregion
                        }
                    }
                }
            }

            lbTodayDate.Text = GetResource("LastAccessDate") + " " + accessDate + " " + accessTime;

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            string images = this.BaseImgPath + "/Icon/ico_noimg.gif";

            if (System.IO.File.Exists(Server.MapPath(baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Picture/" + photo)))
            {
                images = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Picture/" + photo;
            }

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", images);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

            PhotoImage = "<img src='" + images + "' />";
        }

        #endregion Method (함수)

        #region Property (프로퍼티)

        /// <summary>
        /// 작성자 : Freeman
        /// 작성일자 : 2012-1-16
        /// 작성목적 : Direct Call
        /// </summary>
        public string DirectCall
        {
            get { return GetViewStateString("DirectCall", ""); }
            set { ViewState["DirectCall"] = value; }
        }

        /// <summary>
        /// 작성자 : Freeman
        /// 작성일자 : 2012-1-16
        /// 작성목적 : 추가 파라미터
        /// </summary>
        public string TargetParam
        {
            get { return GetViewStateString("TargetParam", ""); }
            set { ViewState["TargetParam"] = value; }
        }

        /// <summary>
        /// 작성자 : Freeman
        /// 작성일자 : 2012-1-16
        /// 작성목적 : Direct Call Menu ID  (ParentMenuID | SubMenuID)
        /// </summary>
        public string DirectCallMenuID
        {
            get { return GetViewStateString("DirectCallMenuID", ""); }
            set { ViewState["DirectCallMenuID"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PhotoImage
        {
            get { return GetViewStateString("PhotoImage", ""); }
            set { ViewState["PhotoImage"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string HomeID
        {
            get { return GetViewStateString("HomeID", ""); }
            set { ViewState["HomeID"] = value; }
        }

        #endregion Property (프로퍼티)
    }
}
