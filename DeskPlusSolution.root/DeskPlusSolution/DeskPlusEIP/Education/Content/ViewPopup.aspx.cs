using System;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Education.Web.Content
{
    /// <summary>유효한 메일 링크에 포함된 한 사용자의 조회만 합니다.</summary>
    public partial class ViewPopup : System.Web.UI.Page
    {
        private string nonce;
        private string contentDocument = String.Empty;
        private bool suppressRendering;

        public string BaseCssPath
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssPath", "/DeskPlusEIP/Design/C/Css"); }
        }

        public string CssPath
        {
            get { return BaseCssPath; }
        }

        public string CspNonce
        {
            get
            {
                if (nonce == null)
                {
                    byte[] random = new byte[18];
                    using (RandomNumberGenerator generator = RandomNumberGenerator.Create()) generator.GetBytes(random);
                    nonce = Convert.ToBase64String(random);
                }
                return nonce;
            }
        }

        public string ContentDocumentAttribute { get { return HttpUtility.HtmlAttributeEncode(contentDocument); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetReadOnlyHeaders();
            phEducation.Visible = false;
            phNotFound.Visible = true;
            if (!String.Equals(Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                suppressRendering = true;
                Response.StatusCode = 405;
                Response.TrySkipIisCustomErrors = true;
                Response.AddHeader("Allow", "GET");
                return;
            }

            BLL.MailViewScope scope = null;
            try
            {
                // 결재의 비인증 팝업과 동일하게 파일은 포탈 로그인 후에만 확인합니다.
                foreach (string parameter in new[] { "download", "attachID", "fileGUID", "filePath", "fileName" })
                    if (Request.QueryString.GetValues(parameter) != null)
                    {
                        SetNotFound();
                        return;
                    }
                string token = GetSingleQueryValue("p");
                if (!BLL.MailViewLink.TryRead(token, out scope)
                    || !new DAL.TOUser().CanReadMailView(scope.CompanyID, scope.ListID, scope.UserID))
                {
                    SetNotFound();
                    return;
                }
                if (!LoadEducation(scope))
                {
                    SetNotFound();
                    return;
                }

                LoadAttachmentCounts(scope);
                phEducation.Visible = true;
                phNotFound.Visible = false;
            }
            catch (Exception exception)
            {
                LogReadFailure(scope, "READ", exception);
                SetNotFound();
            }
        }

        private void SetReadOnlyHeaders()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.AddHeader("Referrer-Policy", "no-referrer");
            Response.AddHeader("X-Content-Type-Options", "nosniff");
            Response.AddHeader("X-Frame-Options", "DENY");
            Response.AddHeader("X-Robots-Tag", "noindex, nofollow, noarchive");
            Response.AddHeader("Content-Security-Policy", "default-src 'none'; script-src 'nonce-" + CspNonce
                + "'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; font-src 'self' data:; "
                + "frame-src 'self' about:; connect-src 'none'; object-src 'none'; form-action 'none'; "
                + "base-uri 'none'; frame-ancestors 'none'");
        }

        private string GetSingleQueryValue(string name)
        {
            string[] values = Request.QueryString.GetValues(name);
            return values != null && values.Length == 1 ? values[0] : null;
        }

        private bool LoadEducation(BLL.MailViewScope scope)
        {
            using (DataTableReader reader = new DAL.List().GetList(scope.CompanyID,
                scope.ListID.ToString(CultureInfo.InvariantCulture), scope.UserID))
            {
                if (reader == null || !reader.Read()) return false;
                lbTitle.Text = Encode(reader["Title"].ToString());
                ltlDate.Text = Encode(FormatDate(reader["StartDate"].ToString()) + " ~ "
                    + FormatDate(reader["EndDate"].ToString()));
                ltlFileYN.Text = String.Equals(reader["FileYN"].ToString().Trim(), "N", StringComparison.OrdinalIgnoreCase)
                    ? "N" : "Y";
                string status = reader["Status"].ToString();
                ltlComplete.Text = status == "3" ? "완료" : "미완료";
                ltlStatus.Text = status == "1" ? "대기" : status == "2" ? "진행" : status == "3" ? "완료"
                    : status == "4" ? "지연" : "";
                ltlCompleteUserName.Text = status == "3" ? Encode(PreferredName(reader, "CompleteName")) : "";
                // 이 Literal만 마크업의 Mode=Encode를 사용합니다.
                ltlCompleteDate.Text = status == "3" ? FormatDate(reader["CompleteDate"].ToString()) : "";
                ltlItem.Text = String.Empty;
                foreach (DataRowView item in AppSettingHelper.GetSysCode(scope.CompanyID, "EducationItem"))
                    if (item["CodeID"].ToString() == reader["ItemID"].ToString())
                    {
                        string name = item["NameKo"].ToString();
                        ltlItem.Text = Encode(String.IsNullOrWhiteSpace(name) ? item["NameBase"].ToString() : name);
                        break;
                    }
                SetContentDocument(reader["Contents"].ToString().ToEditorXSS());
                return true;
            }
        }

        private void SetContentDocument(string contents)
        {
            const string policy = "default-src 'none'; script-src 'none'; img-src data:; style-src 'unsafe-inline'; "
                + "connect-src 'none'; object-src 'none'; frame-src 'none'; form-action 'none'; base-uri 'none'";
            contentDocument = "<!DOCTYPE html><html lang=\"ko\"><head><meta charset=\"utf-8\" />"
                + "<meta name=\"referrer\" content=\"no-referrer\" /><meta http-equiv=\"Content-Security-Policy\" content=\""
                + HttpUtility.HtmlAttributeEncode(policy) + "\" /><style>body{margin:0;font-family:'Malgun Gothic',sans-serif;"
                + "font-size:12px;overflow-wrap:break-word}img{max-width:100%;height:auto}table{max-width:100%}</style>"
                + "</head><body>" + contents + "</body></html>";
        }

        private void LoadAttachmentCounts(BLL.MailViewScope scope)
        {
            int noticeCount = 0;
            foreach (BE.AttachInfo file in new DAL.Attach().GetAttachList(scope.CompanyID, scope.ListID))
            {
                if (file != null && file.CompanyID == scope.CompanyID && file.ListID == scope.ListID && file.AttachID > 0)
                    noticeCount++;
            }
            int certificateCount = 0;
            foreach (BE.TOAttachInfo file in new DAL.TOAttach().GetList(scope.CompanyID, scope.ListID, scope.UserID))
            {
                if (file != null && file.CompanyID == scope.CompanyID && file.ListID == scope.ListID && file.UserID == scope.UserID)
                    certificateCount++;
            }
            ltlNoticeFileCount.Text = noticeCount.ToString(CultureInfo.InvariantCulture);
            ltlCertificateFileCount.Text = certificateCount.ToString(CultureInfo.InvariantCulture);
        }

        private static string PreferredName(IDataRecord reader, string prefix)
        {
            string name = reader[prefix + "Ko"].ToString();
            return String.IsNullOrWhiteSpace(name) ? reader[prefix + "Base"].ToString() : name;
        }

        private static string Encode(string value)
        {
            return HttpUtility.HtmlEncode((value ?? String.Empty).ToReverseXSS());
        }

        private static string FormatDate(string value)
        {
            DateTime date;
            return DateTime.TryParseExact(value, new string[] { "yyyyMMdd", "yyyy-MM-dd" }, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date) ? date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "";
        }

        private void SetNotFound()
        {
            phEducation.Visible = false;
            phNotFound.Visible = true;
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!suppressRendering) base.Render(writer);
        }

        private static void LogReadFailure(BLL.MailViewScope scope, string phase, Exception exception)
        {
            try
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(String.Format(
                    CultureInfo.InvariantCulture, "Education.MailView CompanyID={0}, ListID={1}, UserID={2}, Phase={3}, ErrorType={4}",
                    scope == null ? 0 : scope.CompanyID, scope == null ? 0 : scope.ListID,
                    scope == null ? 0 : scope.UserID, phase, exception.GetType().Name));
            }
            catch (Exception) { }
        }
    }
}
