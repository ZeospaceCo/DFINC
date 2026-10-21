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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.EIP.Web.Menu
{
    public partial class Menu : Base.BasePageEIP
    {

        #region 변수설정

        public string TopMenu = "";        
        public string UserDeptName = "";
        public int BaseDeptCnt = 0;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Menu.Menu).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();               
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

            #endregion


            DataView dv = biz.GetMenuUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, remoteAddr.ToDouble<double>());
            StringBuilder sb = new StringBuilder();
            BLL.MenuDataLink bizMDL = new BLL.MenuDataLink();
            DAL.MenuDataLink dal = new DAL.MenuDataLink();
            List<BE.LDAPInfo> ldapInfo = dal.GetLdap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            
            for (int i = 0; i < dv.Count; i++)
            {
                //최상위 메뉴만 출력하기
                if (dv[i]["ParentMenuID"].ToString().ToNull() == "")
                {
                    string[] parameters = bizMDL.GetMenuParameter(CurrentUserInfo.CompanyID, dv[i]["MenuID"].ToString().ToNumber<int>(), CurrentUserInfo.UserID, ldapInfo[0]);
                    string url = dv[i]["TargetURL"].ToString();
                    string param = String.Empty;
                    string dcParam = String.Empty;

                    if (!string.IsNullOrEmpty(url)) //TargetURL이 없으면 하위 메뉴의 첫번째를 클릭하게 한다.
                    {
                        if (parameters[0].Length > 0 || parameters[1].Length > 0 || !string.IsNullOrEmpty(dv[i]["DirectCall"].ToString())) url = url + "?";

                        if (!string.IsNullOrEmpty(dv[i]["DirectCall"].ToString()))
                        {
                            dcParam = "dcparm=" + dv[i]["DirectCall"].ToString();
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
                        dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(), dv[i]["NameZhTw"].ToString(), PreLang);
                    #endregion

                    sb.Append(string.Format(@"<li><a id=""mn_{1}"" onClick=""javascript:Menu.CallTopMenu('{0}','{1}','{2}')"">{3}</a></li>",
                        url,
                        dv[i]["MenuID"].ToString(),
                        dv[i]["OpenUrl"].ToString(),
                        name));

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
            }            

            Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
            List<UserInfo> alUser = dal.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, false);

            StringBuilder sbUSer = new StringBuilder();
            foreach(UserInfo info in alUser)    
                sbUSer.Append(string.Format(@"<li><a href=""#info"" onclick=""Menu.SetBaseDept({0},{1},{2})"">{3} {4} {5}</a></li>", 
                    info.DeptID, info.CodeTitleID, info.CodeDutyID,
                    this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhCn, info.DeptZhTw, PreLang),
                    this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhCn, info.TitleZhTw, PreLang),
                    CurrentUserInfo.Name));

            UserDeptName = sbUSer.ToString();
            BaseDeptCnt = alUser.Count;
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            DirectCall = GetParm("d", ""); //DirectCall 은 암호화를 하지 않는다
            TargetParam = GetParm("t", "").Replace("|","=").Replace(";","&"); //추가 파라미터는 암호화를 하지 않는다
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




        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

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
        #endregion
    }
}