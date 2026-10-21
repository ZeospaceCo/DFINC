using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.PMS.Web.Base
{
    public class BaseControlPMS : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseControl
    {   

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePagePMS"/> class.
        /// </summary>
        public BaseControlPMS()
		{
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Base.BaseControlPMS).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            this.Load += new System.EventHandler(this.BaseControlPMS_Load);
            this.Error += new System.EventHandler(this.BaseControlPMS_Error);
		}

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BaseControlPMS_Error(object sender, System.EventArgs e)
        {
            base.ErrorUserEnvironment();
        }

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 권한 체크
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-16
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BaseControlPMS_Load(object sender, System.EventArgs e)
        {
            SetSkin();            
        }

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
        /// 메서드명   :
        /// 작성목적   : ErrorMSG
        /// 작성자     : 유신종
        /// 최초작성일 : 2009-02-04
        /// </summary>
        /// <param name="_msg">The _MSG.</param>
        private void ErrorMSG(string _msg)
        {
            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.ClientMessage.Alert(_msg);
            //Response.Write("<script language='javascript'>");
            //Response.Write("history.back();");
            //Response.Write("</script>");
            //Response.End();

            string script = "history.back();";
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", _msg, script);
        }


        #region Property (프로퍼티)
        /// <summary>
        /// ProjectID
        /// </summary>
        /// <value>The project ID.</value>
        public int ProjectID
        {
            get {  return GetDecryptParam("projectID", "0").ToNumber<int>(); }
        }
        /// <summary>
        /// MenuID
        /// </summary>
        /// <value>The menu ID.</value>
        public int MenuID
        {
            get { return GetDecryptParam("menuID", "0").ToNumber<int>(); }
        }

        /// <summary>
        /// MenuType
        /// </summary>
        /// <value>The type of the menu.</value>
        public int MenuType
        {
            get { return GetDecryptParam("menuType", "0").ToNumber<int>(); }
        }

        /// <summary>
        /// Gets the proj param.
        /// </summary>
        /// <value>The proj param.</value>
        public string ProjParam
        {
            get
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("projectID", ProjectID.ToString());
                paraDic.Add("menuID", MenuID.ToString());
                paraDic.Add("menuType", MenuType.ToString());
                paraDic.Add("rqProjectID", RqProjectID.ToString());

                string url = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                return "p=" + url;
            }
        }

        /// <summary>
        /// 요청 프로젝트 아이디
        /// </summary>
        public int RqProjectID
        {
            get
            {
                return GetDecryptParam("rqProjectID", "0").ToNumber<int>();
            }
        }

        // <summary>
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
        #endregion
    }
}
