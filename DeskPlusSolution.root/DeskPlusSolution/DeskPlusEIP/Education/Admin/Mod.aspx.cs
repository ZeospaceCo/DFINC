using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Education.Web.Admin
{
    public partial class Mod : Base.BasePageEducationAdmin
    {
        #region 변수설정

        private string eventArgs = String.Empty;
        public bool HasEducation { get; private set; }
        public string ReturnViewUrl { get; private set; }
        public string EducationToday
        {
            get { return BLL.List.GetRegistrationToday().ToString("yyyyMMdd", CultureInfo.InvariantCulture); }
        }
        public int OriginalStartDateValue
        {
            get { return GetViewStateInt("OriginalStartDate", "0"); }
        }
        public int OriginalEndDateValue
        {
            get { return GetViewStateInt("OriginalEndDate", "0"); }
        }
        public int OriginalAlarmDayValue
        {
            get { return OriginalAlarmDay; }
        }

        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (!AccessAuth)
            {
                ShowUnavailable(403, "수정 권한이 없습니다.");
                return;
            }

            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Admin.Mod).Assembly;
            SetLanguage();

            int requestedListID;
            if (!Int32.TryParse(GetDecryptParam("listID", ""), NumberStyles.None,
                CultureInfo.InvariantCulture, out requestedListID) || requestedListID <= 0
                || (IsPostBack && OriginalListID != requestedListID))
            {
                ShowUnavailable(404, "조회할 수 없는 내용입니다.");
                return;
            }

            ListID = requestedListID;
            ReturnViewUrl = GetReturnViewUrl(false);
            int modifyState = new DAL.List().GetModifyState(CurrentUserInfo.CompanyID, ListID);
            if (modifyState != 1 && modifyState != 2)
            {
                ShowUnavailable(404, "조회할 수 없는 내용입니다.");
                return;
            }
            HasEducation = true;
            if (!IsPostBack)
            {
                OriginalListID = ListID;
                SwfUploadGUID = Guid.NewGuid().ToString();
                LoadData();
                if (!HasEducation) return;

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Education/"
                    + DateTime.Now.ToString("yyyyMM", CultureInfo.InvariantCulture) + "/editor/" + Guid.NewGuid().ToString();
                NonActiveX = "T";
            }
            else
            {
                tbxStartDate.Text = Request.Form[tbxStartDate.UniqueID] ?? tbxStartDate.Text;
                tbxEndDate.Text = Request.Form[tbxEndDate.UniqueID] ?? tbxEndDate.Text;
                LoadPostedTargets();
                LoadExistingAttachments();
            }

            if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath;
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            using (DataTableReader reader = new DAL.List().GetList(CurrentUserInfo.CompanyID, ListID.ToString(CultureInfo.InvariantCulture)))
            {
                if (reader == null || !reader.Read())
                {
                    ShowUnavailable(404, "조회할 수 없는 내용입니다.");
                    return;
                }

                OriginalItemID = reader["ItemID"].ToString();
                LoadSelectionItems(OriginalItemID);
                tbxTitle.Text = reader["Title"].ToString().ToReverseXSS();
                OriginalStartDate = reader["StartDate"].ToString().Trim();
                OriginalEndDate = reader["EndDate"].ToString().Trim();
                tbxStartDate.Text = DateHelper.GetCultureDateChar8(OriginalStartDate, PreLangReal);
                tbxEndDate.Text = DateHelper.GetCultureDateChar8(OriginalEndDate, PreLangReal);
                cbPopUp.Checked = reader["PopUpYN"].ToString() == "Y";

                int alarmDay;
                OriginalAlarmDay = Int32.TryParse(reader["AlarmDay"].ToString(), out alarmDay) ? alarmDay : 0;
                ddlAlarmDay.Items.Clear();
                ddlAlarmDay.Items.Add(new ListItem("미알람", "0"));
                for (int day = 1; day <= 7; day++)
                    ddlAlarmDay.Items.Add(new ListItem(day.ToString(CultureInfo.InvariantCulture), day.ToString(CultureInfo.InvariantCulture)));
                ddlAlarmDay.Items.Add(new ListItem("10", "10"));
                ddlAlarmDay.Items.Add(new ListItem("15", "15"));
                ddlAlarmDay.Items.Add(new ListItem("20", "20"));
                ddlAlarmDay.Items.Add(new ListItem("25", "25"));
                ddlAlarmDay.Items.Add(new ListItem("30", "30"));
                if (ddlAlarmDay.Items.FindByValue(OriginalAlarmDay.ToString(CultureInfo.InvariantCulture)) == null && OriginalAlarmDay >= 0)
                    ddlAlarmDay.Items.Add(new ListItem(OriginalAlarmDay.ToString(CultureInfo.InvariantCulture), OriginalAlarmDay.ToString(CultureInfo.InvariantCulture)));
                if (ddlAlarmDay.Items.FindByValue(OriginalAlarmDay.ToString(CultureInfo.InvariantCulture)) != null)
                    ddlAlarmDay.SelectedValue = OriginalAlarmDay.ToString(CultureInfo.InvariantCulture);

                ddlFileYN.SelectedValue = String.Equals(reader["FileYN"].ToString().Trim(), "N", StringComparison.OrdinalIgnoreCase) ? "N" : "Y";

                string contents = reader["Contents"].ToString().ToEditorXSS();
                tbHiddenHTML_contents.Value = contents;
                tbHiddenText_contents.Text = reader["ContentText"].ToString().ToReverseXSS();
                tbHiddenMime_contents.Text = Utility.GetEditorHtmlHeader(PreLang, "") + contents + "</body></html>";
            }

            LoadTargets();
            LoadExistingAttachments();
        }

        private void LoadSelectionItems(string currentItemID)
        {
            ddlItem.Items.Clear();
            DataView codes = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");
            foreach (DataRowView code in codes)
            {
                string codeID = code["CodeID"].ToString();
                if (code["ActiveCode"].ToString() != "1" && codeID != currentItemID) continue;
                string name = GetLangName(code["NameBase"].ToString(), code["NameKo"].ToString(),
                    code["NameEn"].ToString(), code["NameJa"].ToString(), code["NameFr"].ToString(),
                    code["NameZhcn"].ToString(), code["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                ddlItem.Items.Add(new ListItem(name, codeID));
            }

            if (ddlItem.Items.FindByValue(currentItemID) == null && !String.IsNullOrEmpty(currentItemID))
                ddlItem.Items.Add(new ListItem(currentItemID, currentItemID));
            if (ddlItem.Items.FindByValue(currentItemID) != null) ddlItem.SelectedValue = currentItemID;
        }

        private void LoadTargets()
        {
            List<BE.ListTOInfo> targets = new DAL.ListTO().GetListTo(CurrentUserInfo.CompanyID, ListID);
            Dictionary<string, string> labels = new Dictionary<string, string>();
            List<string> selections = new List<string>();
            StringBuilder html = new StringBuilder();
            foreach (BE.ListTOInfo target in targets)
            {
                if (target == null || target.DeptID <= 0
                    || (target.IsGroup != "G" && target.IsGroup != "M")
                    || (target.IsGroup == "M" && target.UserID <= 0)) continue;
                string selection = target.IsGroup + "_" + (target.IsGroup == "G" ? "0" : target.UserID.ToString(CultureInfo.InvariantCulture))
                    + "_" + target.DeptID.ToString(CultureInfo.InvariantCulture);
                string name = GetLangName(target.NameBase, target.NameKo, target.NameEn, target.NameJa,
                    target.NameFr, target.NameZhcn, target.NameZhtw, PreLang).ToReverseXSS();
                if (String.IsNullOrWhiteSpace(name)) name = target.IsGroup == "G" ? "부서 " + target.DeptID : "사용자 " + target.UserID;
                if (target.IsGroup == "M")
                {
                    string department = GetLangName(target.DeptBase, target.DeptKo, target.DeptEn, target.DeptJa,
                        target.DeptFr, target.DeptZhcn, target.DeptZhtw, PreLang).ToReverseXSS();
                    string title = GetLangName(target.TitleBase, target.TitleKo, target.TitleEn, target.TitleJa,
                        target.TitleFr, target.TitleZhcn, target.TitleZhtw, PreLang).ToReverseXSS();
                    name = department + "/" + title + "/" + name;
                }
                if (labels.ContainsKey(selection)) continue;
                labels.Add(selection, name);
                selections.Add(selection);
                html.Append(TargetHtml(selection, name));
            }
            ltlReceiveTargets.Text = html.ToString();
            hdReceiveID.Value = String.Join(",", selections.ToArray());
            hdReceiveLabels.Value = new JavaScriptSerializer().Serialize(labels);
        }

        // 이름은 재표시 용도일 뿐이며 실제 대상자는 ID를 기준으로 DB에서 검증합니다.
        private void LoadPostedTargets()
        {
            Dictionary<string, string> labels = new Dictionary<string, string>();
            try
            {
                if (!String.IsNullOrEmpty(hdReceiveLabels.Value) && hdReceiveLabels.Value.Length <= 65536)
                    labels = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(hdReceiveLabels.Value) ?? labels;
            }
            catch (ArgumentException) { }
            catch (InvalidOperationException) { }

            StringBuilder html = new StringBuilder();
            string selections = hdReceiveID.Value ?? "";
            if (selections.Length > 65536) return;
            foreach (string selection in selections.Split(','))
            {
                if (!IsValidTarget(selection)) continue;
                string label;
                if (!labels.TryGetValue(selection, out label) || String.IsNullOrWhiteSpace(label)) label = selection;
                if (label.Length > 500) label = label.Substring(0, 500);
                html.Append(TargetHtml(selection, label));
            }
            ltlReceiveTargets.Text = html.ToString();
        }

        private static string TargetHtml(string selection, string label)
        {
            string[] parts = selection.Split('_');
            string elementID = parts[0] + "_" + (parts[0] == "G" ? parts[2] : parts[1]);
            string safeLabel = HttpUtility.HtmlEncode(label);
            return String.Format(CultureInfo.InvariantCulture,
                "<a href=\"#\" action=\"del\" id=\"{0}\" type=\"{1}\" oid=\"{2}\" title=\"{3}\" style=\"cursor:pointer;white-space:nowrap;line-height:14px;margin-right:5px\">{3},</a>",
                elementID, parts[0], selection, safeLabel);
        }

        private static bool IsValidTarget(string selection)
        {
            string[] parts = (selection ?? "").Split('_');
            int userID;
            int deptID;
            return parts.Length == 3
                && Int32.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out userID)
                && Int32.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out deptID)
                && deptID > 0 && ((parts[0] == "M" && userID > 0) || (parts[0] == "G" && userID == 0));
        }

        private void LoadExistingAttachments()
        {
            List<BE.AttachInfo> attachments = new DAL.Attach().GetAttachList(CurrentUserInfo.CompanyID, ListID);
            List<string> originalAttachIDs = new List<string>();
            StringBuilder html = new StringBuilder("<ul class=\"table_typeT ul_typeT\">");
            foreach (BE.AttachInfo attachment in attachments)
            {
                string attachID = attachment.AttachID.ToString(CultureInfo.InvariantCulture);
                originalAttachIDs.Add(attachID);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                string storedFileName = String.IsNullOrEmpty(attachment.FileGUID) ? attachment.FileName : attachment.FileGUID;
                parameters.Add("filePath", attachment.FilePath + storedFileName);
                parameters.Add("fileName", attachment.FileName.Replace("&", "&amp;"));
                string url = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p="
                    + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(parameters)));
                string fileLengthKB = attachment.FileLength < 0 ? ""
                    : String.Format("{0:n1}", Convert.ToDecimal(attachment.FileLength) / 1024) + " KB";
                html.AppendFormat(CultureInfo.InvariantCulture,
                    "<li id=\"existing_attach_{0}\"><span><a href=\"{1}\" target=\"_blank\" rel=\"noopener\">{2}</a></span> "
                    + "<span class=\"mount\">{3}</span> "
                    + "<a href=\"#\" data-remove-attach=\"{0}\" style=\"display:inline-block;\" title=\"공지첨부 삭제\"><i class=\"fas fa-times-circle\"></i></a></li>",
                    attachID,
                    HttpUtility.HtmlEncode(url), HttpUtility.HtmlEncode(attachment.FileName), HttpUtility.HtmlEncode(fileLengthKB));
            }
            if (!IsPostBack) OriginalAttachIDs = String.Join(",", originalAttachIDs.ToArray());
            html.Append("</ul>");
            ltlExistingFiles.Text = html.ToString();
        }

        private void SetLanguage()
        {
            GetResourceManager();
            cbtnMod.Text = "수정";
            cbtnList.Text = "취소";
            cbtnReceive.Text = "선택";
        }

        #endregion

        #region Method (함수)

        private Dictionary<string, string> GetReturnParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string[] fields = { "returnPage", "dashBoardSearch", "currentPage", "pageSize", "searchItem", "searchComplete", "searchStatus", "searchConfirm", "searchListID",
                "searchDateField", "searchDateValue", "searchField", "searchValue", "startDate", "endDate",
                "searchTarget", "targetIDs", "targetNames", "searchUserField", "searchUserValue",
                "searchTitle", "searchEmployment", "startYM", "endYM", "userID", "courseStatus" };
            foreach (string field in fields) parameters.Add(field, GetDecryptParam(field, ""));
            parameters["listID"] = ListID.ToString(CultureInfo.InvariantCulture);
            return parameters;
        }

        private string GetReturnViewUrl(bool checkTargetUser)
        {
            Dictionary<string, string> parameters = GetReturnParameters();
            int targetUserID;
            if (!Int32.TryParse(parameters["userID"], NumberStyles.None, CultureInfo.InvariantCulture, out targetUserID) || targetUserID <= 0)
                parameters.Remove("userID");
            else if (checkTargetUser)
            {
                using (DataTableReader reader = new DAL.TOUser().GetTargetUser(CurrentUserInfo.CompanyID, ListID, targetUserID))
                    if (reader == null || !reader.Read()) parameters.Remove("userID");
            }
            return "/DeskPlusEIP/Education/Admin/View.aspx?p="
                + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(parameters)));
        }

        private void ShowUnavailable(int statusCode, string message)
        {
            HasEducation = false;
            form1.Visible = false;
            Response.StatusCode = statusCode;
            Response.TrySkipIisCustomErrors = true;
            ltlError.Text = "<div class=\"layout_Tbox\">" + HttpUtility.HtmlEncode(message) + "</div>";
        }

        private bool TryGetDates(out string startValue, out string endValue)
        {
            startValue = "";
            endValue = "";
            try
            {
                startValue = DateHelper.GetCultureDate(Request.Form[tbxStartDate.UniqueID] ?? tbxStartDate.Text, "yyyyMMdd", PreLangReal);
                endValue = DateHelper.GetCultureDate(Request.Form[tbxEndDate.UniqueID] ?? tbxEndDate.Text, "yyyyMMdd", PreLangReal);
            }
            catch (ArgumentException) { return false; }
            catch (FormatException) { return false; }

            DateTime startDate;
            DateTime endDate;
            return DateTime.TryParseExact(startValue, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate)
                && DateTime.TryParseExact(endValue, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate)
                && startDate <= endDate;
        }

        private bool IsValidItem()
        {
            string selectedID = ddlItem.SelectedValue;
            if (String.IsNullOrEmpty(selectedID) || selectedID.Length > 10) return false;
            if (selectedID == OriginalItemID) return true;
            DataView codes = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");
            foreach (DataRowView code in codes)
                if (code["CodeID"].ToString() == selectedID && code["ActiveCode"].ToString() == "1") return true;
            return false;
        }

        private void Modification()
        {
            if (!AccessAuth || !HasEducation || OriginalListID != ListID) return;

            // 완료자 유무와 관계없이 현재 회사에 남아 있는 내역만 수정합니다.
            int modifyState = new DAL.List().GetModifyState(CurrentUserInfo.CompanyID, ListID);
            if (modifyState != 1 && modifyState != 2) { ShowUnavailable(404, "조회할 수 없는 내역입니다."); return; }

            string startDate;
            string endDate;
            int alarmDay;
            string title = tbxTitle.Text.Trim();
            string contentText = tbHiddenText_contents.Text.Trim();
            string contents = tbHiddenHTML_contents.Value ?? "";
            string selections = (hdReceiveID.Value ?? "").Trim();
            if (title.Length == 0 || title.Length > 100 || !TryGetDates(out startDate, out endDate)
                || !IsValidItem()
                || selections.Length == 0 || selections.Length > 65536
                || (contentText.Length == 0 && contents.IndexOf("<img", StringComparison.OrdinalIgnoreCase) < 0))
            {
                Utility.ShowMessageBox(Page, "invalidModify", "수정할 정보와 대상자, 기간을 확인해 주세요.", "BtnCheck = 'N';");
                return;
            }

            if (!Int32.TryParse(ddlAlarmDay.SelectedValue, NumberStyles.None, CultureInfo.InvariantCulture, out alarmDay))
                alarmDay = -1;
            // 기존 일정을 그대로 둔 대상자 추가 등의 수정은 지난 재알림 예정일 때문에 막지 않습니다.
            // 서버 업무 계층에서도 현재 저장된 일정과 비교해 다시 검증합니다.
            bool unchangedAlarmSchedule = String.Equals(startDate, OriginalStartDate, StringComparison.Ordinal)
                && String.Equals(endDate, OriginalEndDate, StringComparison.Ordinal) && alarmDay == OriginalAlarmDay;
            string alarmValidationMessage = !BE.ListInfo.IsSupportedAlarmDay(alarmDay)
                ? "알람 예정일은 종료일 기준 유효한 일자만 선택 가능합니다."
                : unchangedAlarmSchedule ? String.Empty : BLL.List.GetRegistrationAlarmValidationMessage(
                    startDate, endDate, alarmDay, BLL.List.GetRegistrationToday());
            if (!String.IsNullOrEmpty(alarmValidationMessage))
            {
                Utility.ShowMessageBox(Page, "invalidAlarmDate", alarmValidationMessage,
                    "BtnCheck = 'N'; $('#ddlAlarmDay').focus();");
                return;
            }

            foreach (string selection in selections.Split(','))
                if (!IsValidTarget(selection))
                {
                    Utility.ShowMessageBox(Page, "invalidTarget", "대상자를 다시 선택해 주세요.", "BtnCheck = 'N';");
                    return;
                }

            Guid uploadGUID;
            if (!Guid.TryParse(SwfUploadGUID, out uploadGUID))
                throw new InvalidOperationException("Invalid education upload session.");

            BE.ListInfo info = new BE.ListInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ListID = ListID;
            info.Title = title;
            info.Contents = contents;
            info.ContentText = contentText;
            info.ItemID = ddlItem.SelectedValue;
            info.StartDate = startDate;
            info.EndDate = endDate;
            info.PopUpYN = cbPopUp.Checked ? "Y" : "N";
            info.AlarmDay = alarmDay;
            info.FileYN = ddlFileYN.SelectedValue;
            info.WriteUserID = CurrentUserInfo.UserID;
            info.ToUserList = selections;
            info.DeleteAttachIDs = String.Empty;
            info.OriginalAttachIDs = OriginalAttachIDs;
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            info.HiddenSpoolPath = filePath + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID
                + "/Spool/" + uploadGUID.ToString() + "/";
            info.SenderName = CurrentUserInfo.Name;
            info.PreLang = PreLang;

            string failureMessage;
            if (new BLL.List().Modify(info, out failureMessage))
            {
                ReturnViewUrl = GetReturnViewUrl(true);
                Utility.ShowMessageBox(Page, "modifySuccess", "수정되었습니다.",
                    "window.location.href='" + HttpUtility.JavaScriptStringEncode(ReturnViewUrl) + "';");
                return;
            }
            Utility.ShowMessageBox(Page, "modifyFail", String.IsNullOrEmpty(failureMessage)
                ? "수정하는데 실패했습니다. 입력 내용을 확인해 주세요." : failureMessage, "BtnCheck = 'N';");
        }

        #endregion

        #region Property (프로퍼티)

        private string OriginalAttachIDs
        {
            get { return ViewState["OriginalAttachIDs"] as string; }
            set { ViewState["OriginalAttachIDs"] = value; }
        }

        private int OriginalListID
        {
            get { return GetViewStateInt("OriginalListID", "0"); }
            set { ViewState["OriginalListID"] = value; }
        }

        private string OriginalItemID
        {
            get { return GetViewStateString("OriginalItemID", ""); }
            set { ViewState["OriginalItemID"] = value; }
        }

        private int OriginalAlarmDay
        {
            get { return GetViewStateInt("OriginalAlarmDay", "0"); }
            set { ViewState["OriginalAlarmDay"] = value; }
        }

        private string OriginalStartDate
        {
            get { return GetViewStateString("OriginalStartDate", ""); }
            set { ViewState["OriginalStartDate"] = value; }
        }

        private string OriginalEndDate
        {
            get { return GetViewStateString("OriginalEndDate", ""); }
            set { ViewState["OriginalEndDate"] = value; }
        }

        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }

        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }

        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        #endregion

        #region Event Handler (이벤트핸들러)

        protected void lbtnMod_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AccessAuth || !HasEducation) return;
                if (eventArgs == "MOD") Modification();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Education.Admin.Mod.lbtnMod_Click", ex);
                Utility.ShowMessageBox(Page, "modifyError", "수정하는데 실패했습니다.", "BtnCheck = 'N';");
            }
        }

        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion
    }
}

