using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Web;
using System.Globalization;

namespace Zeospace.DeskPlus.Education.Web.Admin
{
    public partial class View : Base.BasePageEducationAdmin
    {
        public bool HasEducation { get; private set; }
        public bool IsAdminConfirmed { get; private set; }
        public bool IsWaitingForCompletion { get; private set; }
        public string ReturnListUrl { get; private set; }
        public string ReturnAfterDeleteUrl { get; private set; }
        public string ModifyUrl { get; private set; }
        public string AdminConfirmationJson = "null";
        public int TargetUserID { get { return targetUserID; } }
        public bool IsComplete { get { return Status == "3"; } }
        private bool certificateRequired = true;
        public bool CertificateRequired { get { return certificateRequired; } }
        public int SavedCertificateCount { get; private set; }

        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            private set { ViewState["SwfUploadGUID"] = value; }
        }

        #region 변수설정
        private string eventArgs = String.Empty;
        private int targetUserID;
        private string targetUserName = String.Empty;
        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            phUserStatus.Visible = false;
            pnlCertificates.Visible = false;
            phCertificateUpload.Visible = false;
            cbtnComplete.Visible = false;
            cbtnCancelComplete.Visible = false;
            cbtnTargetList.Visible = false;
            cbtnSendAlarm.Visible = false;
            cbtnSendAllAlarm.Visible = false;
            cbtnSendDelayedAlarm.Visible = false;
            cbtnSendIncompleteMail.Visible = false;
            cbtnSendDelayedMail.Visible = false;
            cbtnDelete.Visible = false;
            cbtnMod.Visible = false;
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0 || CurrentUserInfo.UserID <= 0
                || !AccessAuth)
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;
                form1.Visible = false;
                return;
            }

            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Admin.View).Assembly;	
            GetResourceManager();
            #endregion 다국어 리소스 로드

            SetLanguage();
            LoadReturnUrl();
            LoadData();
            if (HasEducation && LoadTargetUser())
            {
                if (IsPostBack && !MatchesCompletionContext())
                {
                    ShowTargetError();
                    return;
                }
                if (!LoadAdminConfirmation())
                {
                    ShowTargetError();
                    return;
                }
                if (!IsPostBack)
                {
                    SwfUploadGUID = Guid.NewGuid().ToString("D");
                    ViewState["CompletionCompanyID"] = CurrentUserInfo.CompanyID;
                    ViewState["CompletionActorUserID"] = CurrentUserInfo.UserID;
                    ViewState["CompletionListID"] = ListID;
                    ViewState["CompletionTargetUserID"] = TargetUserID;
                }
                cbtnComplete.Visible = !IsComplete && !IsAdminConfirmed;
                phCertificateUpload.Visible = !IsComplete && !IsAdminConfirmed;
                cbtnCancelComplete.Visible = IsComplete && !IsAdminConfirmed;
                cbtnSendAlarm.Visible = !IsComplete;
                LoadCertificates();
                Base.CompletionNotice.Show(this, CurrentUserInfo.CompanyID, ListID,
                    CurrentUserInfo.UserID, TargetUserID);
            }
            cbtnTargetList.Visible = HasEducation;
            cbtnSendAllAlarm.Visible = HasEducation && TargetUserID == 0;
            cbtnSendDelayedAlarm.Visible = HasEducation && TargetUserID == 0;
            cbtnSendIncompleteMail.Visible = HasEducation && TargetUserID == 0;
            cbtnSendDelayedMail.Visible = HasEducation && TargetUserID == 0;
            int modifyState = HasEducation
                ? new DAL.List().GetModifyState(CurrentUserInfo.CompanyID, ListID) : 0;
            cbtnDelete.Visible = modifyState == 1;
            cbtnMod.Visible = modifyState == 1 || modifyState == 2;
            if (HasEducation) LoadModifyUrl();
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            DAL.List dalList = new DAL.List();

            using (DataTableReader dtr = dalList.GetList(CurrentUserInfo.CompanyID, ListID.ToString()))
            {
                if (dtr == null || !dtr.HasRows)
                {
                    Response.StatusCode = 404;
                    Response.TrySkipIisCustomErrors = true;
                    lbTitle.Text = "조회할 수 없는 내역입니다.";
                    return;
                }

                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        HasEducation = true;
                        lbTitle.Text = HttpUtility.HtmlEncode(dtr["Title"].ToString().ToReverseXSS());
                        ltlDate.Text = DateHelper.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal) + "~" + DateHelper.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);

                        DateTime writeDate;
                        ltlWriteDate.Text = DateTime.TryParse(Convert.ToString(dtr["WriteDate"], CultureInfo.InvariantCulture),
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out writeDate)
                            ? writeDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "";
                        string writeUserName = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                            dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                            dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                        string writeUserText = String.IsNullOrWhiteSpace(writeUserName) ? "" : HttpUtility.HtmlEncode(writeUserName);
                        ltlWriteUserName.Text = writeUserText;
                        int writeUserID;
                        if (!String.IsNullOrWhiteSpace(writeUserName)
                            && Int32.TryParse(dtr["WriteUserID"].ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out writeUserID)
                            && writeUserID > 0)
                        {
                            ltlWriteUserName.Text = String.Format(CultureInfo.InvariantCulture,
                                @"<a href=""#"" onclick=""AccountInfo('{0}');return false;"" style=""cursor:pointer"">{1}</a>",
                                writeUserID, writeUserText);
                        }

                        if (dtr["PopUpYN"].ToString() == "Y")
                        {
                            ltlPopUpYN.Text = "Y";
                        }
                        else
                        {
                            ltlPopUpYN.Text = "N";
                        }

                        certificateRequired = !String.Equals(dtr["FileYN"].ToString().Trim(), "N", StringComparison.OrdinalIgnoreCase);
                        ltlFileYN.Text = CertificateRequired ? "Y" : "N";

                        if (dtr["AlarmDay"].ToString() == "0")
                        {
                            ltlAlarmDay.Text = "미알람";
                        }
                        else
                        {
                            ltlAlarmDay.Text = dtr["AlarmDay"].ToString() + "일전 미완료자 재알람 발송";
                        }

                        lbContent.Text = dtr["Contents"].ToString().ToEditorXSS();         

                        DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");

                        for (int i = 0; i < dv.Count; i++)
                        {
                            if (dv[i]["CodeID"].ToString() == dtr["ItemID"].ToString())
                            {
                                ltlItem.Text = HttpUtility.HtmlEncode(this.GetLangName(dv[i]["NameBase"].ToString(), dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                                                dv[i]["NameFr"].ToString(), dv[i]["NameZhcn"].ToString(), dv[i]["NameZhtw"].ToString(), PreLang).ToReverseXSS());
                            }
                        }
                    }
                }
            }

            #region 수신자

            DAL.ListTO dalListTo = new DAL.ListTO();
            var listTo = dalListTo.GetListTo(CurrentUserInfo.CompanyID, ListID);
            string toList = "";
            foreach (var info in listTo)
            {
                if (info == null || (info.IsGroup != "M" && info.IsGroup != "G")) continue;

                string tName = HttpUtility.HtmlEncode(this.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                               info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang).ToReverseXSS());

                if (info.IsGroup == "M")
                {
                    string tDeptName = HttpUtility.HtmlEncode(this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn,
                                info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang).ToReverseXSS());

                    string tTitle = HttpUtility.HtmlEncode(this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn,
                                info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang).ToReverseXSS());

                    toList += string.Format(@"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>, ", info.UserID, tDeptName + " " + tTitle + " " + tName);
                }
                else
                {
                    toList += tName + ", ";
                }
            }
            this.ltlReceive.Text = toList.Trim().TrimEnd(',');

            #endregion                  
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        private void LoadModifyUrl()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string[] fields = { "returnPage", "dashBoardSearch", "currentPage", "pageSize", "searchItem", "searchComplete", "searchStatus", "searchConfirm", "searchListID",
                "searchDateField", "searchDateValue", "startDate", "endDate", "searchTarget", "targetIDs",
                "targetNames", "searchUserField", "searchUserValue", "searchTitle", "searchEmployment",
                "startYM", "endYM", "searchField", "searchValue", "courseStatus" };
            foreach (string field in fields) parameters.Add(field, GetDecryptParam(field, ""));
            parameters.Add("listID", ListID.ToString(CultureInfo.InvariantCulture));
            if (TargetUserID > 0) parameters.Add("userID", TargetUserID.ToString(CultureInfo.InvariantCulture));
            ModifyUrl = "/DeskPlusEIP/Education/Admin/Mod.aspx?p="
                + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(parameters)));
        }

        private void LoadReturnUrl()
        {
            if (GetDecryptParam("returnPage", "") == "DashBoard")
            {
                DashBoardSearch search;
                try
                {
                    search = DashBoardSearch.Parse(GetDecryptParam, CurrentUserInfo.CompanyID, false);
                }
                catch (ArgumentException ex)
                {
                    throw new HttpException(400, ex.Message);
                }
                Dictionary<string, string> dashBoardParameters = new Dictionary<string, string>();
                dashBoardParameters.Add("startYM", search.StartYM);
                dashBoardParameters.Add("endYM", search.EndYM);
                dashBoardParameters.Add("searchTarget", search.Target);
                dashBoardParameters.Add("targetIDs", search.TargetIDs);
                dashBoardParameters.Add("targetNames", search.TargetNames);
                dashBoardParameters.Add("searchItem", search.ItemID);
                dashBoardParameters.Add("courseStatus", search.CourseStatus);
                dashBoardParameters.Add("searchTitle", search.SearchTitle);
                ReturnListUrl = "/DeskPlusEIP/Education/Admin/DashBoard.aspx?p="
                    + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(dashBoardParameters)));
                ReturnAfterDeleteUrl = ReturnListUrl;
                return;
            }
            if (GetDecryptParam("returnPage", "") != "List") return;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string[] fields = { "dashBoardSearch", "currentPage", "pageSize", "searchItem", "searchComplete", "searchStatus", "searchConfirm", "searchListID", "searchDateField",
                "startDate", "endDate", "searchTarget", "targetIDs", "targetNames", "searchUserField", "searchUserValue",
                "searchTitle", "searchEmployment" };
            foreach (string field in fields) parameters.Add(field, GetDecryptParam(field, ""));

            ReturnListUrl = "/DeskPlusEIP/Education/Admin/List.aspx?p="
                + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(parameters)));

            // 삭제한 조건으로 다시 조회하지 않도록 삭제 성공 전용 복귀 URL을 만듭니다.
            int searchListID;
            if (Int32.TryParse((parameters["searchListID"] ?? "").Trim(), NumberStyles.None,
                CultureInfo.InvariantCulture, out searchListID) && searchListID > 0 && searchListID == ListID)
            {
                parameters["searchListID"] = "";
                parameters["searchTitle"] = "";
            }
            parameters["currentPage"] = "1";
            ReturnAfterDeleteUrl = "/DeskPlusEIP/Education/Admin/List.aspx?p="
                + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(parameters)));
        }

        private bool LoadAdminConfirmation()
        {
            AdminConfirmationJson = "null";
            using (DataTableReader reader = new DAL.TOUser().GetConfirmation(CurrentUserInfo.CompanyID,
                ListID, TargetUserID))
            {
                if (reader == null || !reader.Read()) return false;
                BE.AdminListInfo confirmation = Base.AdminConfirmation.Read(reader, PreLang);
                IsAdminConfirmed = confirmation.IsConfirmed;
                AdminConfirmationJson = new System.Web.Script.Serialization.JavaScriptSerializer()
                    .Serialize(confirmation)
                    .Replace("<", "\\u003c").Replace(">", "\\u003e").Replace("&", "\\u0026")
                    .Replace("\u2028", "\\u2028").Replace("\u2029", "\\u2029");
                return true;
            }
        }

        private bool LoadTargetUser()
        {
            phUserStatus.Visible = false;
            pnlCertificates.Visible = false;
            targetUserID = 0;
            targetUserName = String.Empty;
            Status = String.Empty;
            ltlCompleteUserName.Text = "";
            string userID = GetDecryptParam("userID", "");
            if (String.IsNullOrEmpty(userID)) return false;
            if (!Int32.TryParse(userID, NumberStyles.None, CultureInfo.InvariantCulture, out targetUserID)
                || targetUserID <= 0)
            {
                ShowTargetError();
                return false;
            }

            using (DataTableReader dtr = new DAL.TOUser().GetTargetUser(CurrentUserInfo.CompanyID, ListID, targetUserID))
            {
                if (dtr == null || !dtr.Read())
                {
                    ShowTargetError();
                    return false;
                }

                targetUserName = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                    dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                    dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                string logInID = dtr["LogInID"].ToString().ToReverseXSS();
                if (String.IsNullOrWhiteSpace(targetUserName)) targetUserName = "사용자 " + targetUserID;
                if (!String.IsNullOrWhiteSpace(logInID)) targetUserName += " (" + logInID + ")";
                ltlTargetUser.Text = targetUserName;
                Status = dtr["Status"].ToString();
                IsWaitingForCompletion = BE.ListInfo.IsWaitingForCompletion(Status,
                    dtr["StartDate"].ToString(), BLL.List.GetRegistrationToday());
                switch (Status)
                {
                    case "1": ltlStatus.Text = "대기"; break;
                    case "2": ltlStatus.Text = "진행"; break;
                    case "3": ltlStatus.Text = "완료"; break;
                    case "4": ltlStatus.Text = "지연"; break;
                    default: ltlStatus.Text = "-"; break;
                }
                ltlComplete.Text = Status == "3" ? "완료" : "미완료";
                int completeUserID;
                if (Status == "3"
                    && Int32.TryParse(dtr["CompleteUserID"].ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out completeUserID)
                    && completeUserID > 0)
                {
                    string completeUserName = GetLangName(dtr["CompleteNameBase"].ToString(), dtr["CompleteNameKo"].ToString(),
                        dtr["CompleteNameEn"].ToString(), dtr["CompleteNameJa"].ToString(), dtr["CompleteNameFr"].ToString(),
                        dtr["CompleteNameZhcn"].ToString(), dtr["CompleteNameZhtw"].ToString(), PreLang).ToReverseXSS();
                    if (!String.IsNullOrWhiteSpace(completeUserName))
                    {
                        ltlCompleteUserName.Text = String.Format(CultureInfo.InvariantCulture,
                            @"<a href=""#"" onclick=""AccountInfo('{0}');return false;"" style=""cursor:pointer"">{1}</a>",
                            completeUserID, HttpUtility.HtmlEncode(completeUserName));
                    }
                }
                DateTime completeDate;
                ltlCompleteDate.Text = DateTime.TryParseExact(dtr["CompleteDate"].ToString(), "yyyyMMdd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out completeDate)
                    ? completeDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "";
                phUserStatus.Visible = true;
                return true;
            }
        }

        private void ShowTargetError()
        {
            HasEducation = false;
            targetUserID = 0;
            phUserStatus.Visible = false;
            pnlCertificates.Visible = false;
            phCertificateUpload.Visible = false;
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            lbTitle.Text = "조회할 수 없는 대상자입니다.";
        }

        private void LoadCertificates()
        {
            pnlCertificates.Visible = true;
            ltlCertificateFile.Text = "";
            List<BE.TOAttachInfo> certificates = new DAL.TOAttach().GetList(CurrentUserInfo.CompanyID, ListID, targetUserID);
            SavedCertificateCount = certificates.Count;
            if (certificates.Count == 0) return;
            StringBuilder files = new StringBuilder("<ul class=\"table_typeT ul_typeT\">");
            foreach (BE.TOAttachInfo certificate in certificates)
            {
                string downloadUrl = CertificateDownloadUrl(certificate.FilePath, certificate.FileGUID, certificate.FileName);
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

        protected string CertificateDownloadUrl(string filePath, string fileGUID, string fileName)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("filePath", filePath + fileGUID);
            parameters.Add("fileName", fileName.Replace("&", "&amp;"));
            return "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p="
                + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(parameters)));
        }

        private bool MatchesCompletionContext()
        {
            Guid uploadGUID;
            return CurrentUserInfo != null && TargetUserID > 0
                && Object.Equals(ViewState["CompletionCompanyID"], CurrentUserInfo.CompanyID)
                && Object.Equals(ViewState["CompletionActorUserID"], CurrentUserInfo.UserID)
                && Object.Equals(ViewState["CompletionListID"], ListID)
                && Object.Equals(ViewState["CompletionTargetUserID"], TargetUserID)
                && Guid.TryParseExact(SwfUploadGUID, "D", out uploadGUID);
        }

        private void ShowCompletionMessage(string message)
        {
            lblCompleteMessage.Text = HttpUtility.HtmlEncode(message);
            lblCompleteMessage.Visible = true;
        }

        protected void lbtnComplete_Click(object sender, EventArgs e)
        {
            if (!IsPostBack || !AccessAuth || !HasEducation || !MatchesCompletionContext()) return;
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

                // 저장된 첨부는 유지하고, 서버에서 확인한 대상자의 완료 상태만 변경합니다.
                // BLL/DAL이 현재 대상자, 대기 상태와 첨부 필수 설정을 저장 직전에 다시 확인합니다.
                if (!new BLL.TOAttach().CompleteSaved(CurrentUserInfo.CompanyID, ListID, TargetUserID,
                    CurrentUserInfo.UserID))
                {
                    ShowCompletionMessage("완료 처리에 실패했습니다. 첨부파일과 현재 상태를 확인한 후 다시 시도해 주세요.");
                    return;
                }

                Base.CompletionNotice.Redirect(this, "/DeskPlusEIP/Education/Admin/View.aspx",
                    CurrentUserInfo.CompanyID, ListID, CurrentUserInfo.UserID, TargetUserID);
            }
            catch (ArgumentException ex)
            {
                ShowCompletionMessage(ex.Message);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Education.Admin.View.Complete", ex);
                ShowCompletionMessage("완료 처리에 실패했습니다. 첨부파일 목록을 확인한 후 다시 시도해 주세요.");
            }
        }

        protected void lbtnDeleteAttachment_Click(object sender, EventArgs e)
        {
            if (!IsPostBack || !AccessAuth || !HasEducation || !MatchesCompletionContext()) return;
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
                // 관리자 권한과 서명된 ViewState로 확인한 대상자의 첨부만 삭제합니다.
                // 완료 여부는 BLL/DAL에서도 같은 대상자 범위로 다시 검사합니다.
                if (new BLL.TOAttach().DeleteAttachment(CurrentUserInfo.CompanyID, ListID,
                    TargetUserID, fileGUID))
                {
                    Response.Redirect("/DeskPlusEIP/Education/Admin/View.aspx" + Request.Url.Query, false);
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Education.Admin.View.DeleteAttachment", ex);
                ShowCompletionMessage("첨부파일 삭제에 실패했습니다. 첨부파일 목록을 확인한 후 다시 시도해 주세요.");
            }
            LoadCertificates();
        }

        private string GetSpoolPath()
        {
            // 대상자 폴더가 아니라 로그인한 관리자의 업로드 세션만 읽습니다.
            string basePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            if (String.IsNullOrWhiteSpace(basePath))
                throw new InvalidOperationException("첨부파일 저장 경로가 설정되지 않았습니다.");
            return basePath.TrimEnd('/') + "/" + CurrentUserInfo.CompanyID
                + "/Users/" + CurrentUserInfo.UserID + "/Spool/" + SwfUploadGUID + "/";
        }

        protected void lbtnSaveAttachments_Click(object sender, EventArgs e)
        {
            if (!IsPostBack || !AccessAuth || !HasEducation || !MatchesCompletionContext()) return;
            if (IsAdminConfirmed)
            {
                ShowCompletionMessage("관리자 확인이 완료된 내역은 첨부파일을 등록할 수 없습니다.");
                return;
            }
            if (IsComplete)
            {
                ShowCompletionMessage("완료된 내역은 첨부파일을 등록할 수 없습니다.");
                return;
            }
            try
            {
                // 관리자 권한/대상자는 현재 조회 결과와 서명된 ViewState로 다시 확인합니다.
                if (!new BLL.TOAttach().SaveAttachments(CurrentUserInfo.CompanyID, ListID, TargetUserID,
                    CurrentUserInfo.UserID, GetSpoolPath()))
                {
                    ShowCompletionMessage("첨부파일 등록에 실패했습니다. 첨부파일 목록을 확인한 후 다시 시도해 주세요.");
                    return;
                }
                Response.Redirect("/DeskPlusEIP/Education/Admin/View.aspx" + Request.Url.Query, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (ArgumentException ex)
            {
                ShowCompletionMessage(ex.Message);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Education.Admin.View.SaveAttachments", ex);
                ShowCompletionMessage("첨부파일 등록에 실패했습니다. 첨부파일 목록을 확인한 후 다시 시도해 주세요.");
            }
        }

        /// <summary>
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnComplete.Text = "완료";
                cbtnTargetList.Text = "대상자리스트";
                cbtnSendAlarm.Text = "알람발송";
                cbtnSendAllAlarm.Text = "미완료알람발송";
                cbtnSendDelayedAlarm.Text = "지연알람발송";
                cbtnSendIncompleteMail.Text = "미완료메일발송";
                cbtnSendDelayedMail.Text = "지연메일발송";
                cbtnDelete.Text = "삭제";
                cbtnMod.Text = "수정";
                cbtnCancel.Text = "목록";     
            }
        }

        #endregion Method (함수)

        #region Property (프로퍼티)      

        /// <summary>
        /// Status
        /// </summary>
        public string Status
        {
            get { return GetViewStateString("Status", ""); }
            set { ViewState["Status"] = value; }
        }

        #endregion Property (프로퍼티)
      
    }
}
