using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Account.BE;

//using Zeospace.DeskPlus.EIP.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.Web.Package
{
    public partial class Menu : Base.BasePageEIP
    {
        #region 변수설정

        public string TopMenu = "";
        public string UserDeptName = "";
        public int BaseDeptCnt = 0;
        public string MenuImage;

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Package.Menu).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
                LoadImage();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadTopMenu();
            TopNavigator();

            //lbTodayDate.Text = GetBaseDate(System.DateTime.Now).ToString("yyyyMMdd hh:mm");
            //lbTodayDate.Text = DateHelper.GetCultureDateChar8(GetBaseDate(System.DateTime.Now).ToString("yyyyMMdd"), PreLang);
            lbTodayDate.Text = DateHelper.GetCultureDateChar8(System.DateTime.Now.ToString("yyyyMMdd"), PreLangReal);// +" " + DateTime.Now.ToString("HH:mm");
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
                for (int i = 0; i < dv.Count; i++)
                {
                    if (dv[i]["ParentMenuID"].ToString().ToNull() == "")
                    {
                        string[] parameters = bizMDL.GetMenuParameter(CurrentUserInfo.CompanyID,
                            dv[i]["MenuID"].ToString().ToNumber<int>(),
                            CurrentUserInfo.UserID, ldapInfo[0]);

                        string url = dv[i]["TargetURL"].ToString();
                        string param = String.Empty;
                        string dcParam = String.Empty;

                        if (!string.IsNullOrEmpty(url)) //TargetURL이 없으면 하위 메뉴의 첫번째를 클릭하게 한다.
                        {
                            if (parameters[0].Length > 0 || parameters[1].Length > 0 || !string.IsNullOrEmpty(dv[i]["DirectCall"].ToString())) url = url + "?";

                            if (!string.IsNullOrEmpty(dv[i]["DirectCall"].ToString()))
                            {
                                //home
                                if (dv[i]["DirectCall"].ToString().ToLower() == "home")
                                {
                                    dcParam = "tabID=" + GetFirstTabID();
                                }
                                else
                                {
                                    dcParam = "dcparm=" + dv[i]["DirectCall"].ToString();
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
                            sb.Append(string.Format(@"<li style=""padding-left:0px !important; margin-left:0px;""><a id=""mn_{1}"" onClick=""javascript:Menu.CallTopMenu('{0}','{1}','{2}')"">{3}</a></li>",
                                url,
                                dv[i]["MenuID"].ToString(),
                                dv[i]["OpenUrl"].ToString(),
                                name));
                            useFirstStyle = false;
                            firstMenuID = dv[i]["MenuID"].ToString();
                        }
                        else
                        {
                            sb.Append(string.Format(@"<li><a id=""mn_{1}"" onClick=""javascript:Menu.CallTopMenu('{0}','{1}','{2}')"">{3}</a></li>",
                                url,
                                dv[i]["MenuID"].ToString(),
                                dv[i]["OpenUrl"].ToString(),
                                name));
                        }

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
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Main[LoadTopMenu] : " + ex);
            }

            //DirectCall 이 없으면 처음 메뉴로 할당한다. (그래야 하위메뉴까지 출력된다.) - SHIMPARK 201304017
            if (DirectCall.Length == 0) DirectCallMenuID = firstMenuID + "|";

            TopMenu = sb.ToString();
        }

        /// <summary>
        /// 상단 네비게이션
        /// </summary>
        private void TopNavigator()
        {
            SignOutLink.NavigateUrl = "/DeskPlusEIP/Site/Auth/LogOut.aspx";
            SignOutLink.Visible = true;

            if (CurrentUserInfo.IsInRole("admin") || CurrentUserInfo.IsSysAdmin)  // administrator
            {
                LiAdminLink.Visible = true;//슈퍼관리자 li tag
                AdminLink.NavigateUrl = "/DeskPlusEIP/EIP/Admin/Default.aspx";
                AdminLink.Visible = true; //슈퍼관리자 버튼
                
                lnkAdmin.Visible = true;
                lnkAdmin.NavigateUrl = "/DeskPlusEIP/EIP/Admin/Default.aspx";
                lnkAdmin.Text = "Admin";
                lnkAdmin.Attributes.Add("style", "text-decoration:none");
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
                SignOutLink.Text = resMng.GetString("LogOut");
                AdminLink.Text = resMng.GetString("SuperAdmin");
            }
        }

        /// <summary>
        /// 다법인별 이미지 처리
        /// </summary>
        private void LoadImage()
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");

            //기본 이미지
            MenuImage = this.BaseImgPath + "/Portal/logo.gif";

            //회사별 이미지
            string ciMenuImage = baseFileDirectory + CurrentUserInfo.CompanyID + "/logo.gif";

            #region Menu 이미지

            if (System.IO.File.Exists(Server.MapPath(ciMenuImage)))
            {
                MenuImage = ciMenuImage;
            }
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", MenuImage);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            MenuImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

            #endregion Menu 이미지
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

        #endregion Property (프로퍼티)
    }
}