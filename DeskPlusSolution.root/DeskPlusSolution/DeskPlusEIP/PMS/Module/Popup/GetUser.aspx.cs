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

namespace Zeospace.DeskPlus.PMS.Web.Module.Popup
{
    public partial class GetUser : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Module.Popup.GetUser).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }


            this.lbxList.Attributes.Add("onChange", "lbxListOnChange()");


        }

        

        #endregion


        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            #region 사용자정보가져오기
            //string loginID = HttpContext.Current.User.Identity.Name;
            //if (loginID == "") return;

            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id     
            UserID = CurrentUserInfo.UserID; //userid
            #endregion

            DAL.ProjectUser bll = new Zeospace.DeskPlus.PMS.DAL.ProjectUser();
            List<BE.ProjectUserInfo> al = bll.GetProjectUserByProjectID(CompanyID, ProjectID);

            if (al != null)
            {
                lbxList.ClearSelection();
                string userName = "";
                int userID = 0;

                foreach (BE.ProjectUserInfo be in al)
                {
                    //userName = Component.WebComponent.StringHelper.GetNameDBPreLang(PreLang, be.UserName);
                    userName = this.GetLangName(
                                    be.NameBase,
                                    be.NameKo, be.NameEn, be.NameJa,
                                    be.NameFr, be.NameZhcn, be.NameZhtw, PreLang).ToReverseXSS();

                    userID = be.UserID;

                    lbxList.Items.Add(new ListItem(userName, userID.ToString()));
                }
            }
        }

        #endregion


        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnSubmit.Text = resMng.GetString("Confirm");
                cbtnCancel.Text = resMng.GetString("Close");

                ltlReceiveMan.Text = resMng.GetString("ChargerUserDept_Title");
                ltlReceiveManSelect.Text = resMng.GetString("ChargerUserDept_Remark");
                lblSelectedReceiptor.Text = resMng.GetString("ChargerUserDept_SelectedReceiptor");
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
            get { return GetViewStateInt("CompanyID", "1"); }
            set { ViewState["CompanyID"] = value; }
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
        #endregion


        #region Event Handler (이벤트 핸들러)

        #endregion
    }
}
