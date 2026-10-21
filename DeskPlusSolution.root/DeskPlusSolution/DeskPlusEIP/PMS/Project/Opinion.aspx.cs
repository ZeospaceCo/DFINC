using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.PMS.Web.Project
{
    public partial class Opinion : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Project.Opinion).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
            
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 데이터 초기화
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-02-03
        /// </summary>
        private void LoadData()
        {
            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //if (loginID == "") return;

            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id       
            UserID = CurrentUserInfo.UserID;
            #endregion

            DAL.Project bll = new Zeospace.DeskPlus.PMS.DAL.Project();
            var al = bll.GetProject(CompanyID, ProjectID);

            foreach (var info in al)
            {
                string content = info.Opinion.Replace("\r\n", "<br />");
                content = content.Replace("  ", "&nbsp;&nbsp;");
                lblContent.Text = content;
            }
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = resMng.GetString("Close"); //닫기
            }
        }
        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get
            {
                return GetViewStateInt("CompanyID", "0");
            }
            set
            {
                ViewState["CompanyID"] = value;
            }
        }


        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get
            {
                return GetViewStateInt("UserID", "0");
            }
            set
            {
                ViewState["UserID"] = value;
            }
        }


        /// <summary>
        /// Gets the project ID.
        /// </summary>
        /// <value>The project ID.</value>
        private new int ProjectID
        {
            get { return GetDecryptParam("projectID", "0").ToNumber<int>(); }
        }




        #endregion

        #region Event Handler (이벤트 핸들러)

        

        #endregion
    }
}
