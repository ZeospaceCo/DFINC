using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Portlet.Web.Base
{
    public class BaseControlPortlet : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseControl
    {
        private string p;
        private string decryString = string.Empty;//복고화 값
        public bool IsAuth;

        /// <summary>
        /// 스킨정보
        /// </summary>
        public string Skin { get; set; }
        /// <summary>
        /// 이미지 경로 프로퍼티
        /// </summary>
        public string ImgPath { get; set; }
        /// <summary>
        /// css 경로 프로퍼티
        /// </summary>
        public string CssPath { get; set; }

        #region 포틀릿 속성
        public string TabPortletID;
        public string PortletID;
        public string CategoryID;
        public string Title;
        public string PortletCode;
        public string PortletType;
        public string UIType;
        public string UITemplate;
        public string UIURL;
        public string DataType;
        public string DataURL;
        public string LinkType;
        public string LinkPopupSize;
        public string LinkURL;
        public string IsPesonalization;
        public string ActivePortlet;
        public string SendMethod;
        public string Description;
        public string PhotoName;
        public string IsAdmin;
        public int UserID;
        public string IsMinimize;
        public int PageSize;
        public string EtcParam;
        public string ParamValue;
        public string ColumnID;
        public string TargetPortletID;
        #endregion



        /// <summary>
        /// 
        /// </summary>
        public BaseControlPortlet()
        {
            this.Load += new System.EventHandler(this.BaseControlPortlet_Load);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseControlPortlet_Load(object sender, System.EventArgs e)
        {
            SetDecryptString();
            GetPortletParam();

            #region 페이지 사이즈 설정

            int adminPageSize = 0;
            if (ParamValue.Length > 0)
            {
                foreach (string keyValue in ParamValue.Split('^'))
                {
                    if (keyValue.IndexOf('$') != -1)
                    {
                        string pKey = keyValue.Split('$')[0];
                        string pValue = keyValue.Split('$')[1];

                        if (pKey.ToLower() == "pagesize") adminPageSize = pValue.ToNumber<int>();
                    }
                }
            }

            //사용자가 설정한 pageSize 가 없으면 관리자가 설정한 pageSize 로 간다.
            if (adminPageSize > 0) PageSize = adminPageSize;
            if (PageSize == 0) PageSize = 5;

            #endregion

            IsAuth = false;

            if (IsAdmin == "0" && UserID == CurrentUserInfo.UserID) IsAuth = true;//개인이 추가한 포틀릿일 경우
            if (IsAdmin == "1" && CurrentUserInfo.IsInRole("admin")) IsAuth = true;//관리자가 추가한 포틀릿일 경우

            SetSkin();
        }

        #region 함수

        /// <summary>
        /// 스킨 설정하기
        /// </summary>
        private void SetSkin()
        {
            try
            {
                // 이미지파일 경로 설정하기
                // ex) DeskPlusEIP/Design/[LANGUAGE]/[TYPE]/images 
                string skin = Account.BLL.User.GetSkin(CurrentUserInfo.CompanyID, CurrentUserInfo.LogInID);

                CssPath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssDirectory", "/DeskPlusEIP/Design/");
                ImgPath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                if (!CssPath.EndsWith("/")) CssPath += "/";
                if (!ImgPath.EndsWith("/")) ImgPath += "/";

                CssPath = CssPath + skin + "/Css";
                ImgPath = ImgPath + skin + "/Img";

            }
            catch (Exception ex)
            {
                CssPath = "/DeskPlusEIP/Design/C/Css";
                ImgPath = "/DeskPlusEIP/Design/C/Img";

                //로그에 남기기				
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetSkin : " + Request.Url.ToString() + " : " + ex.Message);
            }
        }


        /// <summary>
        /// 작성자 : shimpark
        /// 암호화 문자열 반환하기
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected new string GetEncryptParam(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                return Utility.EncryptString(Server.UrlEncode(data));
            }
            catch
            {
                return "!NULL";
            }
        }

        /// <summary>
        /// 파리미터 암호화값 전역변수에 할당하기
        /// </summary>
        /// <param name="_encParam"></param>
        /// <returns></returns>
        protected new void SetDecryptString()
        {
            string param = P;
            if (!string.IsNullOrEmpty(param) && param != "!NULL")
            {
                param = Server.UrlDecode(param);//디코딩
                decryString = Utility.DecryptString(param);
                decryString = Server.UrlDecode(decryString);//디코딩
            }
        }


        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : code behind 단에서 복고화 하여 파라미터값 가져오기
        /// </summary>
        /// <param name="_keyName">Name of the _key.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected new string GetDecryptParam(string _keyName, string _errorReturn)
        {
            if (!string.IsNullOrEmpty(decryString))
            {
                return UtilHelper.GetParmforParameter(decryString, _keyName, _errorReturn);
            }

            return _errorReturn;

        }

        /// <summary>
        /// 포틀릿 파라미터값 변수 할당하기
        /// </summary>
        private void GetPortletParam()
        {
            TabPortletID = GetDecryptParam("tabPortletID", "0");//
            PortletID = GetDecryptParam("portletID", "0");//
            CategoryID = GetDecryptParam("categoryID", "0");//

            Title = this.GetLangName(GetDecryptParam("nameBase", ""), GetDecryptParam("nameKo", ""), GetDecryptParam("nameEn", ""), GetDecryptParam("nameJa", ""),
                GetDecryptParam("nameFr", ""), GetDecryptParam("nameZhcn", ""), GetDecryptParam("nameZhtw", ""), CurrentUserInfo.PreferredLanguage);//



            UITemplate = GetDecryptParam("uiTemplate", "");//            
            DataType = GetDecryptParam("dataType", "R");//
            DataURL = GetDecryptParam("dataURL", ""); //            
            LinkPopupSize = GetDecryptParam("linkPopupSize", "");//
            LinkURL = GetDecryptParam("linkURL", ""); //            
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
            IsMinimize = GetDecryptParam("isMinimize", "0"); //
            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();//
            EtcParam = GetDecryptParam("etcParam", "");
            ParamValue = GetDecryptParam("paramValue", "");//
            TargetPortletID = GetDecryptParam("targetPortletID", "");//

            #region 미사용
            PortletCode = GetDecryptParam("portletCode", "");//?
            PortletType = GetDecryptParam("portletType", "");//?
            UIType = GetDecryptParam("uiType", "");//?
            UIURL = GetDecryptParam("uiURL", ""); //?
            LinkType = GetDecryptParam("linkType", ""); //?
            IsPesonalization = GetDecryptParam("isPesonalization", "");//?
            ActivePortlet = GetDecryptParam("activePortlet", ""); //?
            SendMethod = GetDecryptParam("sendMethod", "");//?
            Description = GetDecryptParam("description", "");//?
            PhotoName = GetDecryptParam("photoName", "");//?
            IsAdmin = GetDecryptParam("isAdmin", "");//?
            ColumnID = GetDecryptParam("columnID", "col1");//?            
            #endregion

        }

        #endregion

        #region (프로퍼티)
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string P
        {
            get
            {
                return p;
            }
            set
            {
                p = value;
            }
        }



        #endregion
    }
}