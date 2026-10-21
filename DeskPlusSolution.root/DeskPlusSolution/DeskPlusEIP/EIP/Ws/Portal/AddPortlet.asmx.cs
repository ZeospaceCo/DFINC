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

namespace Zeospace.DeskPlus.EIP.Web.Ws.Portal
{
    /// <summary>
    /// AddPortlet의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddPortlet : Base.BaseWSEIP
    {
        private int CategoryID = 0;
        private string TitleName = "";//포틀릿명
        private string Modules = "";//모듈
        private string DataUrl = "";//데이터경로
        private string IsAdmin = "";//관리자 : 1, 사용자 0
        private string Template = "";//리스트 list, 블로그 blog, 갤러리 gallery

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            try
            {
                SetDecryptString(p);                
                CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();//포틀릿카테고리
                TitleName = GetDecryptParam("titleName", "");//포틀릿명
                Modules = GetDecryptParam("modules", "");//모듈
                DataUrl = GetDecryptParam("dataUrl", "");//데이터경로(파라미터)                
                IsAdmin = GetDecryptParam("isAdmin", "");//관리자 : 1, 사용자 0
                Template = GetDecryptParam("template", "");//리스트 list, 블로그 blog, 갤러리 gallery

                DataUrl = Server.UrlDecode(DataUrl);

                bool response = false;                
                
                switch (Modules.ToLower())
                {
                    case "board" :
                        response = AddBoard();
                        break;
                }

                return response;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 게시판 저장하기
        /// </summary>
        /// <returns></returns>
        private bool AddBoard()
        {
            string uiURL = "";
            switch (Template.ToLower())
            {
               case "list":
                    uiURL = "/DeskPlusEIP/Portlet/Package/Board/List.ascx";
                    break;

                case "blog":
                    uiURL = "/DeskPlusEIP/Portlet/Package/Board/Blog.ascx";
                    break;

                case "gallery":
                    //uiURL = "/DeskPlusEIP/Portlet/Package/Board/Album.ascx";
                    uiURL = "/DeskPlusEIP/Portlet/Package/Board/NoticeImage.ascx";
                    break;
            }

            #region 포틀릿 추가
            BE.PortletInfo info = new BE.PortletInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = CategoryID;

            info.NameBase = TitleName.Trim();
            info.NameKo = "";
            info.NameEn = "";
            info.NameJa = "";
            info.NameFr = "";
            info.NameZhcn = "";
            info.NameZhtw = "";

            info.PortletCode = "";
            //포틀릿종류 ( T : 탭포틀릿 / P : 일반 포틀릿)
            info.PortletType = "P";
            //UI 형식 (T : 템플릿/ L: 내부페이지 Layer/ F : IFrame)
            info.UIType = "L";
            info.UITemplate = "";
            info.UIURL = uiURL;
            info.DataType = "E";
            info.DataURL = "";
            info.LinkType = "P";
            info.LinkPopupSize = "|";
            info.LinkURL = "";
            info.IsPesonalization = "1";
            info.ActivePortlet = "1";
            info.SendMethod = "0";
            info.Description = TitleName.Trim();
            info.PhotoName = "";
            info.IsAdmin = IsAdmin; //사용자
            info.WriteUserID = CurrentUserInfo.UserID;
            
            DAL.Portlet dal = new DAL.Portlet();
            int portletID = dal.Add(info);
            #endregion

            #region 데이터 링크 저장하기
            if (DataUrl.Length > 0)
            {
                Zeospace.DeskPlus.EIP.DAL.PortletDataLink pdlDal = new Zeospace.DeskPlus.EIP.DAL.PortletDataLink();

                foreach (string keyValue in DataUrl.Split('^'))
                {
                    string pKey = keyValue.Split('=')[0];
                    string pValue = keyValue.Split('=')[1];

                    Zeospace.DeskPlus.EIP.BE.PortletDataLinkInfo pdlInfo = new Zeospace.DeskPlus.EIP.BE.PortletDataLinkInfo();
                    pdlInfo.CompanyID = CurrentUserInfo.CompanyID;
                    pdlInfo.PortletID = portletID;
                    pdlInfo.TargetType = "U";
                    pdlInfo.TargetDataType = "E";
                    pdlInfo.LinkName = pKey;
                    pdlInfo.LinkValue = pValue;
                    pdlInfo.LinkType = "OU";

                    pdlDal.Add(pdlInfo);
                }
            }
            #endregion

            #region 권한 할당
            //공용권한은 기본값으로 user 권한을 준다.
            if (portletID > 0 && IsAdmin == "1")
            {
                Zeospace.DeskPlus.EIP.DAL.Role dalRole = new DAL.Role();
                List<RoleInfo> alRole = dalRole.GetRole(CurrentUserInfo.CompanyID, "user", "1");
                int roleID = 0;
                foreach (RoleInfo infoRole in alRole) roleID = infoRole.RoleID;

                Zeospace.DeskPlus.EIP.BE.PortletRoleInfo prInfo = new PortletRoleInfo();
                prInfo.CompanyID = CurrentUserInfo.CompanyID;
                prInfo.PortletID = portletID;
                prInfo.RoleID = roleID;
                Zeospace.DeskPlus.EIP.DAL.PortletRole dalportRole = new DAL.PortletRole();
                dalportRole.Add(prInfo);
            }
            #endregion

            return true;
        }
    }
}
