using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

namespace Zeospace.DeskPlus.Pim.Web.Admin.ShareUser
{
    public partial class List : Base.BasePagePimAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int UserID = 0;
        public int CompanyID = 0;

        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Admin.ShareUser.List).Assembly;	//어셈블리 선언
            #endregion

            UserID  = CurrentUserInfo.UserID;
            CompanyID = CurrentUserInfo.CompanyID;

            if (!IsPostBack)
            {
                SetLanguage();
                SettingControl();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        private void LoadData()
        {
        }

        /// <summary>
        /// 작성자  : 
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = this.GetResource("ADD");
                cbtnSearch.Text = this.GetResource("SEARCHTEXT");
                CbtnMySchedule.Text = GetResource("MYSCHEDULE");
            }
        }


        private void SettingControl()
        {
            ListItem listkind1 = new ListItem(GetResource("Name"), "N");
            ddlSearchField.Items.Add(listkind1);

            
        }
        #endregion

        #region (함수)
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}