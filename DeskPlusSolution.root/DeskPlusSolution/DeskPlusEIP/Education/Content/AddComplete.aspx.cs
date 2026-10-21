using System;
using System.Data;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Education.Web.Content
{
    public partial class AddComplete : Base.BasePageEducation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (!IsPostBack)
            {
                SwfUploadGUID = Guid.NewGuid().ToString();
                ViewState["CompletionListID"] = ListID;
            }
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            try
            {
                string basePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string spoolPath = basePath.TrimEnd('/') + "/" + CurrentUserInfo.CompanyID
                    + "/Users/" + CurrentUserInfo.UserID + "/Spool/" + SwfUploadGUID + "/";
                bool completed = new BLL.TOAttach().Add(CurrentUserInfo.CompanyID, ListID, CurrentUserInfo.UserID, spoolPath);
                if (!completed)
                {
                    lblMessage.Text = "완료 처리에 실패했습니다. 첨부파일을 확인한 후 다시 시도해 주세요. 이미 완료한 경우 상세 화면을 다시 열어 주세요.";
                    return;
                }
                btnComplete.Visible = false;
                cbtnSwfFileAdd.Visible = false;

                ClientScript.RegisterStartupScript(GetType(), "EducationComplete", @"
                    if (parent.View && parent.View.CompleteSaved) {
                        parent.View.CompleteSaved();
                    } else if (window.opener && window.opener.View && window.opener.View.CompleteSaved) {
                        window.opener.View.CompleteSaved();
                        window.close();
                    } else if (parent.disablePopup) {
                        parent.disablePopup();
                    } else {
                        window.close();
                    }", true);
            }
            catch (ArgumentException ex)
            {
                lblMessage.Text = HttpUtility.HtmlEncode(ex.Message);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Education.AddComplete", ex);
                lblMessage.Text = "완료 처리에 실패했습니다. 첨부파일 목록을 확인한 후 다시 등록해 주세요.";
            }
        }

        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }
    }
}
