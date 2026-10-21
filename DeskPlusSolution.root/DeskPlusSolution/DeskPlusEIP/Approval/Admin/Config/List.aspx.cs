using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.Config
{
    public partial class List : Zeospace.DeskPlus.Approval.Web.Base.BasePageApprovalAdmin
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
            ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Config.List).Assembly;	//어셈블리 선언
            #endregion
            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
                
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            string attachSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, Helpers.AppSettingType.APPROVAL, "AttachLimitSize", "");
            string listCount = GetEnvKeyValue(CurrentUserInfo.CompanyID, Helpers.AppSettingType.APPROVAL, "ListCount", "");
            string carbonUserViewOption = GetEnvKeyValue(CurrentUserInfo.CompanyID, Helpers.AppSettingType.APPROVAL, "CarbonUserViewOption", "0");
            string reportUserCarbonViewOption = GetEnvKeyValue(CurrentUserInfo.CompanyID, Helpers.AppSettingType.APPROVAL, "ReportUserCarbonViewOption", "0");

            ddlListCount.SelectedValue = listCount;
            ctbFileLength.Text = attachSize;

            rblCarbonUserViewOption.SelectedValue = carbonUserViewOption;
            rblReportUserCarbonViewOption.SelectedValue = reportUserCarbonViewOption;
        }

        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnModClassInfo.Text = resMng.GetString("Btn_Save"); //"확인";

                //참조자 열람 옵션
                rblCarbonUserViewOption.Items.Clear();
                rblCarbonUserViewOption.Items.Add(new ListItem(resMng.GetString("CarbonUserViewOptionItem1"), "1"));
                rblCarbonUserViewOption.Items.Add(new ListItem(resMng.GetString("CarbonUserViewOptionItem2"), "0"));

                //상신자 참조열람 옵션
                rblReportUserCarbonViewOption.Items.Clear();
                rblReportUserCarbonViewOption.Items.Add(new ListItem(resMng.GetString("Use"), "1"));
                rblReportUserCarbonViewOption.Items.Add(new ListItem(resMng.GetString("NotUse"), "0"));
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
            string carbonUserViewOption = rblCarbonUserViewOption.SelectedValue;
            string reportUserCarbonViewOption = rblReportUserCarbonViewOption.SelectedValue;

            Zeospace.DeskPlus.Common.BE.EnvInfo info = new DeskPlus.Common.BE.EnvInfo();
            Zeospace.DeskPlus.Common.DAL.Env biz = new DeskPlus.Common.DAL.Env();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Gubun = AppSettingHelper.GetDescription(AppSettingType.APPROVAL);
            info.KeyID = "AttachLimitSize";
            info.KeyValue = attachSize;
            info.WriteUserID = CurrentUserInfo.UserID;
            biz.Mod(info);

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Gubun = AppSettingHelper.GetDescription(AppSettingType.APPROVAL);
            info.KeyID = "ListCount";
            info.KeyValue = listCount;
            info.WriteUserID = CurrentUserInfo.UserID;
            biz.Mod(info);

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Gubun = AppSettingHelper.GetDescription(AppSettingType.APPROVAL);
            info.KeyID = "CarbonUserViewOption";
            info.KeyValue = carbonUserViewOption;
            info.WriteUserID = CurrentUserInfo.UserID;
            biz.Mod(info);

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Gubun = AppSettingHelper.GetDescription(AppSettingType.APPROVAL);
            info.KeyID = "ReportUserCarbonViewOption";
            info.KeyValue = reportUserCarbonViewOption;
            info.WriteUserID = CurrentUserInfo.UserID;
            biz.Mod(info);

            /*
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Gubun = AppSettingHelper.GetDescription(AppSettingType.APPROVAL);
            info.KeyID = "UseActive";
            info.KeyValue = useActive;
            info.WriteUserID = CurrentUserInfo.UserID;
            biz.Mod(info);
            */

            //approval 의 application session 삭제
            AppSettingHelper.ClearConfigApplication(AppSettingType.APPROVAL);

            Helpers.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_SUCCESS"), "");
        }

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : 심재운
        /// 환경정보 수정하기
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            ModConfig();
        }

        #endregion
    }
}