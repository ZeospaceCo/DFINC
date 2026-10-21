using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web.Admin.Config
{
    public partial class Default : Base.BasePageAddressAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
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
            ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Admin.Config.Default).Assembly;	//어셈블리 선언
            #endregion
            if (!IsPostBack)
            {
                LoadData();
                SetLanguage();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            string attachSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, Helpers.AppSettingType.ADDRESS, "AttachLimitSize", "");
            string listCount = GetEnvKeyValue(CurrentUserInfo.CompanyID, Helpers.AppSettingType.ADDRESS, "ListCount", "");
            string useActive = GetEnvKeyValue(CurrentUserInfo.CompanyID, Helpers.AppSettingType.ADDRESS, "UseActive", "");

            ddlListCount.SelectedValue = listCount;
            ddlUseActiveX.SelectedValue = useActive;
            ctbFileLength.Text = attachSize;
        }

        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnModClassInfo.Text = resMng.GetString("BtnSave");

                // ActiveX 사용
                ddlUseActiveX.Items.Clear();
                ddlUseActiveX.Items.Add(new ListItem(resMng.GetString("Use"), "Y"));
                ddlUseActiveX.Items.Add(new ListItem(resMng.GetString("Unuse"), "N"));
                ddlUseActiveX.SelectedValue = "N";
            }
        }

        #endregion



        #region (함수)

        /// <summary>
        /// 작성자  : 심재운
        /// Mods the config.
        /// </summary>
        private void ModConfig()
        {
            string attachSize = ctbFileLength.Text;
            string listCount = ddlListCount.SelectedValue;
            string useActive = ddlUseActiveX.SelectedValue;

            Zeospace.DeskPlus.Common.BE.EnvInfo info = new DeskPlus.Common.BE.EnvInfo();
            Zeospace.DeskPlus.Common.DAL.Env biz = new DeskPlus.Common.DAL.Env();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Gubun = AppSettingHelper.GetDescription(AppSettingType.ADDRESS);
            info.KeyID = "AttachLimitSize";
            info.KeyValue = attachSize;
            info.WriteUserID = CurrentUserInfo.UserID;
            biz.Mod(info);

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Gubun = AppSettingHelper.GetDescription(AppSettingType.ADDRESS);
            info.KeyID = "ListCount";
            info.KeyValue = listCount;
            info.WriteUserID = CurrentUserInfo.UserID;
            biz.Mod(info);

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Gubun = AppSettingHelper.GetDescription(AppSettingType.ADDRESS);
            info.KeyID = "UseActive";
            info.KeyValue = useActive;
            info.WriteUserID = CurrentUserInfo.UserID;
            biz.Mod(info);

            //address 의 application session 삭제
            AppSettingHelper.ClearConfigApplication(AppSettingType.ADDRESS);

            Helpers.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("Complete"), "");
        }

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)       

        /// <summary>
        /// 수정하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            ModConfig();
        }

        #endregion
    }
}