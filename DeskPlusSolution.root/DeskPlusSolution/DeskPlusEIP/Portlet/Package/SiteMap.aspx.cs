using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class SiteMap : Base.BasePagePortlet
    {
        public string TopMenu;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.SiteMap).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadTopMenu();
            }
        }


        /// <summary>
        /// 상단 메뉴
        /// </summary>
        private void LoadTopMenu()
        {
            #region IP 체크

            string[] remoteAddrStr = Request.UserHostAddress.Split('.');
            string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

            Zeospace.DeskPlus.EIP.BLL.Menu biz = new EIP.BLL.Menu();
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
            Zeospace.DeskPlus.EIP.BLL.MenuDataLink bizMDL = new EIP.BLL.MenuDataLink();
            Zeospace.DeskPlus.EIP.DAL.MenuDataLink dal = new EIP.DAL.MenuDataLink();
            List<Zeospace.DeskPlus.EIP.BE.LDAPInfo> ldapInfo = dal.GetLdap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            int menuID = 0;
            int colNum = 1;
            int topMenu = 0;
            
            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["ParentMenuID"].ToString().ToNull() == "")
                {
                    menuID = dv[i]["MenuID"].ToString().ToNumber<int>();
                    string[] parameters = bizMDL.GetMenuParameter(CurrentUserInfo.CompanyID, menuID, CurrentUserInfo.UserID, ldapInfo[0]);
                    string url = dv[i]["TargetURL"].ToString().ToNull();
                    string param = String.Empty;
                    string dcParam = String.Empty;

                    if (url.Length > 0) //TargetURL이 없으면 하위 메뉴의 첫번째를 클릭하게 한다.
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
                    else
                    {
                        url = "#";
                    }

                    #region 다국어 이름 설정
                    string name = this.GetLangName(
                        dv[i]["NameBase"].ToString(),
                        dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                        dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(), dv[i]["NameZhTw"].ToString(), PreLang).ToReverseXSS();
                    #endregion

                    if (topMenu == 0 || topMenu % 3 == 0)
                    {
                        sb.Append(string.Format(@"<div class=""column column{0}"">", colNum));
                        colNum++;
                    }

                    sb.Append(string.Format(@"<div class=""services""><h3><a href=""{1}"">{0}</a></h3>", name, url));

                    sb.Append("<ul>");
                    var al = GetSubMenuList(menuID);

                    foreach (var be in al)
                    {
                        sb.Append(string.Format(@"<li class=""chevron""><a href=""{1}"">{0}</a></li>", be.MenuName, be.TargetURL));
                    }

                    sb.Append("</ul>");
                    sb.Append("</div>");

                    if (topMenu % 3 == 0)
                    {
                        sb.Append("</div>");
                    }

                    //if (useFirstStyle)
                    //{
                    //    sb.Append(string.Format(@"<li style=""padding-left:0px !important; margin-left:0px;""><a id=""mn_{1}"" onClick=""javascript:Menu.CallTopMenu('{0}','{1}','{2}')"">{3}</a></li>",
                    //        url,
                    //        dv[i]["MenuID"].ToString(),
                    //        dv[i]["OpenUrl"].ToString(),
                    //        name));
                    //    useFirstStyle = false;
                    //}
                    //else
                    //{
                    //    sb.Append(string.Format(@"<li><a id=""mn_{1}"" onClick=""javascript:Menu.CallTopMenu('{0}','{1}','{2}')"">{3}</a></li>",
                    //        url,
                    //        dv[i]["MenuID"].ToString(),
                    //        dv[i]["OpenUrl"].ToString(),
                    //        name));

                    //}
                    

                }
            }

            TopMenu = sb.ToString();
        }

        /// <summary>
        /// 하위 메뉴
        /// </summary>
        /// <returns></returns>
        public List<Zeospace.DeskPlus.EIP.BE.MenuInfo> GetSubMenuList(int _menuID)
        {  
            List<Zeospace.DeskPlus.EIP.BE.MenuInfo> al = new List<MenuInfo>();
            if (_menuID == 0) return al;

            HttpContext context = HttpContext.Current;
            string[] remoteAddrStr = context.Request.UserHostAddress.Split('.');
            string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

            Zeospace.DeskPlus.EIP.BLL.Menu biz = new EIP.BLL.Menu();
            DataView dv = biz.GetMenuUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, remoteAddr.ToDouble<double>());
            if (dv == null) return al;

            Zeospace.DeskPlus.EIP.BLL.MenuDataLink bizMDL = new EIP.BLL.MenuDataLink();
            Zeospace.DeskPlus.EIP.DAL.MenuDataLink dal = new Zeospace.DeskPlus.EIP.DAL.MenuDataLink();
            List<Zeospace.DeskPlus.EIP.BE.LDAPInfo> ldapInfo = dal.GetLdap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["ParentMenuID"].ToString().ToNull().ToNumber<int>() == _menuID)
                {
                    string[] parameters = bizMDL.GetMenuParameter(CurrentUserInfo.CompanyID, dv[i]["MenuID"].ToString().ToNumber<int>(), CurrentUserInfo.UserID, ldapInfo[0]);
                    string url = dv[i]["TargetURL"].ToString();
                    string param = String.Empty;
                    string dcParam = String.Empty;
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

                    #region 다국어 이름 설정
                    string name = this.GetLangName(
                        dv[i]["NameBase"].ToString(),
                        dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                        dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(), dv[i]["NameZhTw"].ToString(), PreLang);
                    #endregion

                    Zeospace.DeskPlus.EIP.BE.MenuInfo info = new MenuInfo();
                    info.CompanyID = dv[i]["CompanyID"].ToString().ToNumber<int>();
                    info.MenuID = dv[i]["MenuID"].ToString().ToNumber<int>();
                    info.MenuOrder = dv[i]["MenuOrder"].ToString().ToNumber<int>();
                    info.MenuName = name.ToReverseXSS();
                    info.TargetURL = url;
                    info.DirectCall = dv[i]["DirectCall"].ToString();
                    info.OpenURL = dv[i]["OpenURL"].ToString();
                    al.Add(info);
                }
            }

            return al;
        }
    }
}