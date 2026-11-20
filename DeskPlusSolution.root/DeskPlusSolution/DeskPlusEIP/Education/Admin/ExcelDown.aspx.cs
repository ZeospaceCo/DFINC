using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Education.Web.Admin
{
    public partial class ExcelDown : Base.BasePageEducationAdmin
    {
        #region 변수설정
        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Admin.ExcelDown).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            // 관리자 권한이 없는 요청은 사용자 정보를 다운로드할 수 없습니다.
            if (!AccessAuth)
            {
                Response.StatusCode = 403;
                Response.SuppressContent = true;
                return;
            }

            #region 검색조건

            BE.AdminListInfo search = new BE.AdminListInfo();
            search.ItemID = GetDecryptParam("searchItem", "").Trim();
            search.Complete = GetDecryptParam("searchComplete", "").Trim();
            search.SearchStatus = GetDecryptParam("searchStatus", "").Trim();
            search.SearchConfirm = GetDecryptParam("searchConfirm", "").Trim();
            string searchListID = GetDecryptParam("searchListID", "");
            int parsedSearchListID;
            if (!String.IsNullOrEmpty(searchListID))
            {
                if (!Int32.TryParse(searchListID, NumberStyles.None, CultureInfo.InvariantCulture,
                    out parsedSearchListID) || parsedSearchListID < 0)
                    throw new ArgumentException("검색조건이 올바르지 않습니다.");
                search.SearchListID = parsedSearchListID;
            }
            search.DateField = GetDecryptParam("searchDateField", "StartDate").Trim();
            search.TargetType = GetDecryptParam("searchTarget", "A").Trim();
            search.TargetIDs = GetDecryptParam("targetIDs", "").Trim();
            search.UserField = GetDecryptParam("searchUserField", "Name").Trim();
            search.UserValue = GetDecryptParam("searchUserValue", "").Trim();
            search.Title = GetDecryptParam("searchTitle", "").Trim();
            search.Employment = GetDecryptParam("searchEmployment", "").Trim();

            if ((search.DateField != "StartDate" && search.DateField != "EndDate")
                || (search.TargetType != "A" && search.TargetType != "G")
                || (search.UserField != "Name" && search.UserField != "LogInID")
                || (search.Complete != "" && search.Complete != "3" && search.Complete != "0")
                || (search.SearchStatus != "" && search.SearchStatus != "1" && search.SearchStatus != "2"
                    && search.SearchStatus != "3" && search.SearchStatus != "4")
                || (search.SearchConfirm != "" && search.SearchConfirm != "Y" && search.SearchConfirm != "N")
                || (search.Employment != "" && search.Employment != "1" && search.Employment != "0"))
                throw new ArgumentException("검색조건을 확인해 주세요.");

            if (search.ItemID.Length > 10 || search.Complete.Length > 1
                || search.SearchStatus.Length > 1
                || search.SearchConfirm.Length > 1
                || search.TargetIDs.Length > 11000 || search.UserValue.Length > 100
                || search.Title.Length > 100 || search.Employment.Length > 1)
                throw new ArgumentException("검색조건의 입력 길이를 확인해 주세요.");

            if (search.TargetType == "A") search.TargetIDs = "";

            #endregion 검색조건

            #region 날짜

            string startDate = GetDecryptParam("startDate", "").Trim();
            string endDate = GetDecryptParam("endDate", "").Trim();
            string searchDateValue = GetDecryptParam("searchDateValue", "").Trim();

            if (searchDateValue != "")
            {
                string[] searchDates = searchDateValue.Split(',');
                if (searchDates.Length != 2 || searchDates[0].Trim() == "" || searchDates[1].Trim() == "")
                    throw new ArgumentException("기간의 시작일과 종료일을 모두 입력해 주세요.");
                startDate = searchDates[0].Trim();
                endDate = searchDates[1].Trim();
            }

            if ((startDate != "" && endDate == "") || (startDate == "" && endDate != ""))
                throw new ArgumentException("기간의 시작일과 종료일을 모두 입력해 주세요.");

            search.StartDate = startDate == "" ? "" : DateHelpers.GetCultureDate(startDate, "yyyyMMdd", PreLangReal);
            search.EndDate = endDate == "" ? "" : DateHelpers.GetCultureDate(endDate, "yyyyMMdd", PreLangReal);

            if (startDate != "")
            {
                DateTime parsedStartDate;
                DateTime parsedEndDate;
                if (!DateTime.TryParseExact(search.StartDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate)
                    || !DateTime.TryParseExact(search.EndDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate)
                    || parsedStartDate > parsedEndDate)
                    throw new ArgumentException("날짜 형식 또는 조회 기간을 확인해 주세요.");
            }

            #endregion 날짜

            StringBuilder sb = new StringBuilder();

            #region 엑셀 상단 기술

            sb.Append(@"
                <!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">
                <html><head><title>사용자 리스트</title><meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
                <style type=""text/css"">td{mso-number-format:""\@"";}</style>
                </head><body>");

            sb.Append("<table style='BORDER-RIGHT-WIDTH: 0px; WIDTH: 100%; BORDER-COLLAPSE: collapse; BORDER-TOP-WIDTH: 0px; BORDER-BOTTOM-WIDTH: 0px; BORDER-LEFT-WIDTH: 0px' id='tableHeader' border='1' cellSpacing='0' cellPadding='0'>");
            sb.Append("<tr>");
            sb.Append("<td align='center'>분류</td>");
            sb.Append("<td align='center'>사업장</td>");
            sb.Append("<td align='center'>부서</td>");
            sb.Append("<td align='center'>아이디</td>");
            sb.Append("<td align='center'>성명</td>");
            sb.Append("<td align='center'>재직구분</td>");
            sb.Append("<td align='center'>과정명</td>");
            sb.Append("<td align='center'>기간</td>");
            sb.Append("<td align='center'>진행상태</td>");
            sb.Append("<td align='center'>공지</td>");
            sb.Append("<td align='center'>첨부</td>");
            sb.Append("<td align='center'>완료여부</td>");
            sb.Append("<td align='center'>완료자</td>");
            sb.Append("<td align='center'>완료일</td>");
            sb.Append("<td align='center'>확인여부</td>");
            sb.Append("<td align='center'>확인자</td>");
            sb.Append("<td align='center'>확인일</td>");
            sb.Append("</tr>");

            #endregion 엑셀 상단 기술

            DAL.TOUser biz = new DAL.TOUser();

            // 페이지 크기 0으로 현재 검색조건에 해당하는 전체 결과를 조회합니다.
            using (DataTableReader dtr = biz.GetAdminList(CurrentUserInfo.CompanyID, search, 1, 0).CreateDataReader())
            {
                if (dtr != null && dtr.HasRows)
                {
                    Dictionary<string, string> itemNames = new Dictionary<string, string>();
                    DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");
                    foreach (DataRowView code in dv)
                    {
                        itemNames[code["CodeID"].ToString()] = this.GetLangName(
                            code["NameBase"].ToString(), code["NameKo"].ToString(), code["NameEn"].ToString(),
                            code["NameJa"].ToString(), code["NameFr"].ToString(), code["NameZhcn"].ToString(),
                            code["NameZhtw"].ToString(), PreLang);
                    }

                    while (dtr.Read())
                    {
                        string itemID = dtr["ItemID"].ToString();
                        string itemName;
                        if (!itemNames.TryGetValue(itemID, out itemName)) itemName = itemID;

                        bool isComplete = dtr["Status"].ToString() == "3";
                        // 그리드와 동일하게 조회 프로시저가 계산한 진행상태를 표시합니다.
                        string statusName;
                        switch (dtr["ProgressStatus"].ToString())
                        {
                            case "1": statusName = "대기"; break;
                            case "2": statusName = "진행"; break;
                            case "3": statusName = "완료"; break;
                            case "4": statusName = "지연"; break;
                            default: statusName = ""; break;
                        }
                        string completeUserName = "";
                        if (isComplete && dtr["CompleteUserID"].ToString().ToNumber<int>() > 0)
                        {
                            completeUserName = this.GetLangName(
                                dtr["CompleteNameBase"].ToString(), dtr["CompleteNameKo"].ToString(), dtr["CompleteNameEn"].ToString(),
                                dtr["CompleteNameJa"].ToString(), dtr["CompleteNameFr"].ToString(), dtr["CompleteNameZhcn"].ToString(),
                                dtr["CompleteNameZhtw"].ToString(), PreLang);
                            if (String.IsNullOrWhiteSpace(completeUserName)) completeUserName = "";
                        }
                        string completeDate = isComplete
                            ? DateHelpers.GetCultureDateChar8(dtr["CompleteDate"].ToString(), PreLangReal) : "";
                        BE.AdminListInfo confirmation = Base.AdminConfirmation.Read(dtr, PreLang);

                        sb.Append("<tr>");
                        sb.Append("<td align='center'>" + ExcelText(itemName) + "</td>");
                        sb.Append("<td align='center'>" + ExcelText(this.GetLangName(
                            dtr["WorkplaceBase"].ToString(), dtr["WorkplaceKo"].ToString(), dtr["WorkplaceEn"].ToString(),
                            dtr["WorkplaceJa"].ToString(), dtr["WorkplaceFr"].ToString(), dtr["WorkplaceZhcn"].ToString(),
                            dtr["WorkplaceZhtw"].ToString(), PreLang)) + "</td>");
                        sb.Append("<td align='center'>" + ExcelText(this.GetLangName(
                            dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(),
                            dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(),
                            dtr["DeptZhtw"].ToString(), PreLang)) + "</td>");
                        sb.Append("<td align='center'>" + ExcelText(dtr["LogInID"].ToString()) + "</td>");
                        sb.Append("<td align='center'>" + ExcelText(this.GetLangName(
                            dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(),
                            dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(),
                            dtr["NameZhtw"].ToString(), PreLang)) + "</td>");
                        sb.Append("<td align='center'>" + (dtr["ActiveUser"].ToString() == "1" ? "재직" : "퇴직") + "</td>");
                        sb.Append("<td align='center'>" + ExcelText(dtr["Title"].ToString()) + "</td>");
                        sb.Append("<td align='center'>" + ExcelText(
                            DateHelpers.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal) + " ~ "
                            + DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal)) + "</td>");
                        sb.Append("<td align='center'>" + ExcelText(statusName) + "</td>");
                        sb.Append("<td align='center'>" + dtr["FileCount"].ToString().ToNumber<int>() + "</td>");
                        sb.Append("<td align='center'>" + dtr["CertificateCount"].ToString().ToNumber<int>() + "</td>");
                        sb.Append("<td align='center'>" + (isComplete ? "완료" : "미완료") + "</td>");
                        sb.Append("<td align='center'>" + ExcelText(completeUserName) + "</td>");
                        sb.Append("<td align='center'>" + ExcelText(completeDate) + "</td>");
                        sb.Append("<td align='center'>" + (confirmation.IsConfirmed ? "확인" : "미확인") + "</td>");
                        // 공통 확인 정보는 이미 역변환된 원문이므로 한 번 더 역변환하지 않습니다.
                        sb.Append("<td align='center'>" + ExcelText(confirmation.ConfirmUserName, false) + "</td>");
                        sb.Append("<td align='center'>" + ExcelText(confirmation.ConfirmDateStr) + "</td>");
                        sb.Append("</tr>");
                    }
                }
            }

            sb.Append("</table>");
            sb.Append("</body></html>");

            Response.Clear();
            Response.ClearHeaders();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.ContentEncoding = Encoding.UTF8;
            Response.Charset = "utf-8";
            Response.AddHeader("content-disposition", "attachment;filename=Education_Users_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sb.ToString());
            Response.End();
        }

        /// <summary>
        /// HTML을 인코딩하고 텍스트를 엑셀 수식으로 실행하지 않도록 처리합니다.
        /// </summary>
        private string ExcelText(string value, bool reverseXss = true)
        {
            value = reverseXss ? (value ?? "").ToReverseXSS() : value ?? "";
            string text = value.TrimStart();
            if (text.Length > 0 && (text[0] == '=' || text[0] == '+' || text[0] == '-' || text[0] == '@'))
                value = "'" + value;

            return HttpUtility.HtmlEncode(value);
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

    }
}
