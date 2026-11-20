using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Education.Web.Content
{
    public partial class ViewPopupTotal : Base.BasePageEducation
    {
        #region 변수설정

        public bool HasEducation { get; private set; }
        public bool IsAdminConfirmed { get; private set; }
        public bool IsWaitingForCompletion { get; private set; }
        private bool certificateRequired = true;
        public bool CertificateRequired { get { return certificateRequired; } }
        public int SavedCertificateCount { get; private set; }

        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            private set { ViewState["SwfUploadGUID"] = value; }
        }

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            HasEducation = false;
            Status = "";
            phEducation.Visible = false;
            phNotFound.Visible = true;
            cbtnComplete.Visible = false;
            cbtnCancelComplete.Visible = false;
            phCertificateUpload.Visible = false;
            ltlCompleteUserName.Text = "";
            ltlCertificateFile.Text = "";

            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0
                || CurrentUserInfo.UserID <= 0 || ListID <= 0
                || (IsPostBack && !MatchesCompletionContext()))
            {
                SetNotFound();
                return;
            }

            LoadData();
            if (!HasEducation) return;
            if (!LoadAdminConfirmation())
            {
                HasEducation = false;
                SetNotFound();
                return;
            }

            if (!IsPostBack)
            {
                SwfUploadGUID = Guid.NewGuid().ToString("D");
                ViewState["CompletionCompanyID"] = CurrentUserInfo.CompanyID;
                ViewState["CompletionUserID"] = CurrentUserInfo.UserID;
                ViewState["CompletionListID"] = ListID;
            }

            phEducation.Visible = true;
            phNotFound.Visible = false;
            cbtnComplete.Visible = Status != "3" && !IsAdminConfirmed;
            phCertificateUpload.Visible = Status != "3" && !IsAdminConfirmed;
            cbtnCancelComplete.Visible = Status == "3" && !IsAdminConfirmed;
            LoadCertificate();
            Base.CompletionNotice.Show(this, CurrentUserInfo.CompanyID, ListID,
                CurrentUserInfo.UserID, CurrentUserInfo.UserID);
        }

        #endregion Page Process (페이지 처리)

        #region 데이터 처리

        private bool LoadAdminConfirmation()
        {
            using (DataTableReader reader = new DAL.TOUser().GetConfirmation(CurrentUserInfo.CompanyID,
                ListID, CurrentUserInfo.UserID))
            {
                if (reader == null || !reader.Read()) return false;
                IsAdminConfirmed = String.Equals(reader["ConfirmYN"].ToString(), "Y", StringComparison.Ordinal);
                return true;
            }
        }

        private void LoadData()
        {
            // 알림에는 내역 ID만 있으므로 현재 회사와 로그인한 사용자로 대상을 확인합니다.
            using (DataTableReader reader = new DAL.List().GetList(CurrentUserInfo.CompanyID,
                ListID.ToString(CultureInfo.InvariantCulture), CurrentUserInfo.UserID))
            {
                if (reader == null || !reader.Read())
                {
                    SetNotFound();
                    return;
                }

                HasEducation = true;
                Status = reader["Status"].ToString();
                IsWaitingForCompletion = BE.ListInfo.IsWaitingForCompletion(Status,
                    reader["StartDate"].ToString(), BLL.List.GetRegistrationToday());
                lbTitle.Text = HttpUtility.HtmlEncode(reader["Title"].ToString().ToReverseXSS());
                ltlDate.Text = DateHelper.GetCultureDateChar8(reader["StartDate"].ToString(), PreLangReal)
                    + " ~ " + DateHelper.GetCultureDateChar8(reader["EndDate"].ToString(), PreLangReal);
                lbContent.Text = reader["Contents"].ToString().ToEditorXSS();
                certificateRequired = !String.Equals(reader["FileYN"].ToString().Trim(), "N", StringComparison.OrdinalIgnoreCase);
                ltlFileYN.Text = CertificateRequired ? "Y" : "N";
                switch (Status)
                {
                    case "1": ltlStatus.Text = "대기"; break;
                    case "2": ltlStatus.Text = "진행"; break;
                    case "3": ltlStatus.Text = "완료"; break;
                    case "4": ltlStatus.Text = "지연"; break;
                }
                ltlComplete.Text = Status == "3" ? "완료" : "미완료";
                string completeDate = reader["CompleteDate"].ToString();
                ltlCompleteDate.Text = completeDate.Length == 8
                    ? DateHelper.GetCultureDateChar8(completeDate, PreLangReal) : "";

                int completeUserID;
                if (Status == "3" && Int32.TryParse(reader["CompleteUserID"].ToString(), NumberStyles.None,
                    CultureInfo.InvariantCulture, out completeUserID) && completeUserID > 0)
                {
                    string completeUserName = GetLangName(reader["CompleteNameBase"].ToString(), reader["CompleteNameKo"].ToString(),
                        reader["CompleteNameEn"].ToString(), reader["CompleteNameJa"].ToString(), reader["CompleteNameFr"].ToString(),
                        reader["CompleteNameZhcn"].ToString(), reader["CompleteNameZhtw"].ToString(), PreLang).ToReverseXSS();
                    if (!String.IsNullOrWhiteSpace(completeUserName))
                    {
                        ltlCompleteUserName.Text = String.Format(CultureInfo.InvariantCulture,
                            @"<a href=""#"" onclick=""AccountInfo('{0}');return false;"" style=""cursor:pointer"">{1}</a>",
                            completeUserID, HttpUtility.HtmlEncode(completeUserName));
                    }
                }

                DataView items = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");
                foreach (DataRowView item in items)
                {
                    if (item["CodeID"].ToString() != reader["ItemID"].ToString()) continue;
                    ltlItem.Text = HttpUtility.HtmlEncode(GetLangName(item["NameBase"].ToString(), item["NameKo"].ToString(),
                        item["NameEn"].ToString(), item["NameJa"].ToString(), item["NameFr"].ToString(),
                        item["NameZhcn"].ToString(), item["NameZhtw"].ToString(), PreLang).ToReverseXSS());
                    break;
                }
            }
        }

        private void LoadCertificate()
        {
            ltlCertificateFile.Text = "";
            List<BE.TOAttachInfo> certificates = new DAL.TOAttach().GetList(CurrentUserInfo.CompanyID, ListID, CurrentUserInfo.UserID);
            SavedCertificateCount = certificates.Count;
            if (certificates.Count == 0) return;

            StringBuilder files = new StringBuilder("<ul class=\"table_typeT ul_typeT\">");
            foreach (BE.TOAttachInfo certificate in certificates)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("filePath", certificate.FilePath + certificate.FileGUID);
                parameters.Add("fileName", certificate.FileName.Replace("&", "&amp;"));
                string downloadUrl = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p="
                    + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(parameters)));
                string fileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(certificate.FileLength) / 1024) + " KB";
                files.AppendFormat(@"<li>
                    <a href=""{0}"" target=""_blank"" rel=""noopener"">{1}</a>
                    <span class=""certificate-file-size"">{2}</span>",
                    HttpUtility.HtmlEncode(downloadUrl), HttpUtility.HtmlEncode(certificate.FileName), fileLengthKB);
                if (Status != "3" && !IsAdminConfirmed)
                {
                    files.AppendFormat(@"<a href=""#"" class=""certificate-saved-file-delete"" data-file-guid=""{0}"" title=""파일 삭제"" aria-label=""첨부파일 삭제""><i class=""fas fa-times-circle"" aria-hidden=""true""></i></a>",
                        HttpUtility.HtmlEncode(certificate.FileGUID));
                }
                files.Append("</li>");
            }
            files.Append("</ul>");
            ltlCertificateFile.Text = files.ToString();
        }

        private void SetNotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
        }

        private bool MatchesCompletionContext()
        {
            Guid uploadGUID;
            return Object.Equals(ViewState["CompletionCompanyID"], CurrentUserInfo.CompanyID)
                && Object.Equals(ViewState["CompletionUserID"], CurrentUserInfo.UserID)
                && Object.Equals(ViewState["CompletionListID"], ListID)
                && Guid.TryParseExact(SwfUploadGUID, "D", out uploadGUID);
        }

        private void ShowCompletionMessage(string message)
        {
            lblCompleteMessage.Text = HttpUtility.HtmlEncode(message);
            lblCompleteMessage.Visible = true;
        }

        protected void lbtnComplete_Click(object sender, EventArgs e)
        {
            if (!IsPostBack || !HasEducation || !MatchesCompletionContext()) return;
            if (IsAdminConfirmed)
            {
                ShowCompletionMessage("관리자 확인이 완료된 내역은 완료 처리할 수 없습니다.");
                return;
            }
            if (Status == "3")
            {
                ShowCompletionMessage("이미 완료한 내역입니다.");
                return;
            }
            if (IsWaitingForCompletion)
            {
                ShowCompletionMessage(BE.ListInfo.WaitingCompletionMessage);
                return;
            }

            try
            {
                if (CertificateRequired && SavedCertificateCount == 0)
                {
                    ShowCompletionMessage("첨부파일을 1개 이상 등록해 주세요.");
                    return;
                }

                // 등록된 첨부파일을 유지하며 완료 처리만 별도로 수행합니다.
                // 저장 직전의 대상자, 대기 상태와 첨부 필수 설정은 BLL/DAL에서도 다시 확인합니다.
                if (!new BLL.TOAttach().CompleteSaved(CurrentUserInfo.CompanyID, ListID, CurrentUserInfo.UserID,
                    CurrentUserInfo.UserID))
                {
                    ShowCompletionMessage("완료 처리에 실패했습니다. 첨부파일과 현재 상태를 확인한 후 다시 시도해 주세요.");
                    return;
                }

                // 기존 메신저 팝업 주소를 유지하고 GET으로 갱신하여 중복 등록을 방지합니다.
                Base.CompletionNotice.Redirect(this, "/DeskPlusEIP/Education/Content/ViewPopupTotal.aspx",
                    CurrentUserInfo.CompanyID, ListID, CurrentUserInfo.UserID, CurrentUserInfo.UserID);
            }
            catch (ArgumentException ex)
            {
                ShowCompletionMessage(ex.Message);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Education.Content.ViewPopupTotal.Complete", ex);
                ShowCompletionMessage("완료 처리에 실패했습니다. 첨부파일 목록을 확인한 후 다시 시도해 주세요.");
            }
        }

        protected void lbtnDeleteAttachment_Click(object sender, EventArgs e)
        {
            if (!IsPostBack || !HasEducation || !MatchesCompletionContext()) return;
            if (IsAdminConfirmed)
            {
                ShowCompletionMessage("관리자 확인이 완료된 내역의 첨부파일은 삭제할 수 없습니다.");
                return;
            }
            if (Status == "3")
            {
                ShowCompletionMessage("완료된 내역의 첨부파일은 삭제할 수 없습니다.");
                return;
            }
            string fileGUID = hfDeleteAttachmentGuid.Value;
            if (String.IsNullOrEmpty(fileGUID) || fileGUID.Length > 200 || fileGUID == "." || fileGUID == ".."
                || fileGUID.IndexOfAny(new char[] { '/', '\\', '\0' }) >= 0)
            {
                ShowCompletionMessage("삭제할 첨부파일을 다시 선택해 주세요.");
                return;
            }

            try
            {
                // 팝업에서도 현재 로그인한 대상자의 첨부만 개별 삭제합니다.
                // 진행 상태와 완료일은 그대로 유지합니다.
                if (new BLL.TOAttach().DeleteAttachment(CurrentUserInfo.CompanyID, ListID,
                    CurrentUserInfo.UserID, fileGUID))
                {
                    Response.Redirect("/DeskPlusEIP/Education/Content/ViewPopupTotal.aspx" + Request.Url.Query, false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }
                ShowCompletionMessage("삭제할 첨부파일을 찾을 수 없습니다. 첨부파일 목록을 다시 확인해 주세요.");
            }
            catch (ArgumentException ex)
            {
                ShowCompletionMessage(ex.Message);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Education.Content.ViewPopupTotal.DeleteAttachment", ex);
                ShowCompletionMessage("첨부파일 삭제에 실패했습니다. 첨부파일 목록을 확인한 후 다시 시도해 주세요.");
            }
            LoadCertificate();
        }

        private string GetSpoolPath()
        {
            string basePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            if (String.IsNullOrWhiteSpace(basePath))
                throw new InvalidOperationException("첨부파일 저장 경로가 설정되지 않았습니다.");
            return basePath.TrimEnd('/') + "/" + CurrentUserInfo.CompanyID
                + "/Users/" + CurrentUserInfo.UserID + "/Spool/" + SwfUploadGUID + "/";
        }

        protected void lbtnSaveAttachments_Click(object sender, EventArgs e)
        {
            if (!IsPostBack || !HasEducation || !MatchesCompletionContext()) return;
            if (IsAdminConfirmed)
            {
                ShowCompletionMessage("관리자 확인이 완료된 내역은 첨부파일을 등록할 수 없습니다.");
                return;
            }
            if (Status == "3")
            {
                ShowCompletionMessage("완료된 내역은 첨부파일을 등록할 수 없습니다.");
                return;
            }
            try
            {
                // 업로드 세션은 서명된 ViewState로 확인하며 완료와 분리해 등록합니다.
                if (!new BLL.TOAttach().SaveAttachments(CurrentUserInfo.CompanyID, ListID, CurrentUserInfo.UserID,
                    CurrentUserInfo.UserID, GetSpoolPath()))
                {
                    ShowCompletionMessage("첨부파일 등록에 실패했습니다. 첨부파일 목록을 확인한 후 다시 시도해 주세요.");
                    return;
                }
                Response.Redirect("/DeskPlusEIP/Education/Content/ViewPopupTotal.aspx" + Request.Url.Query, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (ArgumentException ex)
            {
                ShowCompletionMessage(ex.Message);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Education.Content.ViewPopupTotal.SaveAttachments", ex);
                ShowCompletionMessage("첨부파일 등록에 실패했습니다. 첨부파일 목록을 확인한 후 다시 시도해 주세요.");
            }
        }

        #endregion 데이터 처리

        public string Status
        {
            get { return GetViewStateString("Status", ""); }
            set { ViewState["Status"] = value; }
        }
    }
}
