using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Education.Web.Content
{
    public partial class View : Base.BasePageEducation
    {
        public bool HasEducation { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            cbtnCancel.Text = "목록";
            cbtnComplete.Text = "수료증 등록 및 완료";
            cbtnComplete.Visible = false;
            ltlCertificateFile.Text = "";
            LoadData();
            if (HasEducation) LoadCertificate();
        }

        private void LoadData()
        {
            using (DataTableReader reader = new DAL.List().GetList(CurrentUserInfo.CompanyID, ListID.ToString(), CurrentUserInfo.UserID))
            {
                if (reader == null || !reader.Read())
                {
                    Response.StatusCode = 404;
                    Response.TrySkipIisCustomErrors = true;
                    lbTitle.Text = "조회할 수 없는 교육입니다.";
                    return;
                }
                HasEducation = true;
                Status = reader["Status"].ToString();
                lbTitle.Text = HttpUtility.HtmlEncode(reader["Title"].ToString().ToReverseXSS());
                ltlDate.Text = DateHelper.GetCultureDateChar8(reader["StartDate"].ToString(), PreLangReal)
                    + " ~ " + DateHelper.GetCultureDateChar8(reader["EndDate"].ToString(), PreLangReal);
                lbContent.Text = reader["Contents"].ToString().ToEditorXSS();
                switch (Status)
                {
                    case "1": ltlStatus.Text = "대기"; break;
                    case "2": ltlStatus.Text = "진행중"; break;
                    case "3": ltlStatus.Text = "완료"; break;
                    case "4": ltlStatus.Text = "지연"; break;
                }
                ltlComplete.Text = Status == "3" ? "완료" : "미완료";
                string completeDate = reader["CompleteDate"].ToString();
                ltlCompleteDate.Text = completeDate.Length == 8
                    ? DateHelper.GetCultureDateChar8(completeDate, PreLangReal) : "-";
                cbtnComplete.Visible = Status != "3";
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
            ltlCertificateFile.Text = "<ul class=\"table_typeT ul_typeT\"><li></li></ul>";
            List<BE.TOAttachInfo> certificates = new DAL.TOAttach().GetList(CurrentUserInfo.CompanyID, ListID, CurrentUserInfo.UserID);
            if (certificates.Count == 0) return;
            StringBuilder files = new StringBuilder("<ul class=\"table_typeT ul_typeT\">");
            foreach (BE.TOAttachInfo certificate in certificates)
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("listID", ListID.ToString());
                parameters.Add("fileGUID", certificate.FileGUID);
                string downloadUrl = "CertificateDownload.aspx?p="
                    + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(parameters)));
                string fileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(certificate.FileLength) / 1024) + " KB";
                files.AppendFormat(@"<li>
                    <a href=""{0}"" target=""_blank"" rel=""noopener"">{1}</a>
                    <span class=""mount"">{2}</span></li>",
                    HttpUtility.HtmlEncode(downloadUrl), HttpUtility.HtmlEncode(certificate.FileName), fileLengthKB);
            }
            files.Append("</ul>");
            ltlCertificateFile.Text = files.ToString();
            cbtnComplete.Visible = false;
        }

        public string Status
        {
            get { return GetViewStateString("Status", ""); }
            set { ViewState["Status"] = value; }
        }
    }
}
