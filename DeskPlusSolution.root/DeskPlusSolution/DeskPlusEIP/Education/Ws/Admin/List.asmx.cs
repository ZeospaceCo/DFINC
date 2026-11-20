using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Education.Web.Ws.Admin
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSEducation
    {
        #region GET()

        /// <summary>
        /// 조회할 건수와 시작 위치를 계산합니다.
        /// </summary>
        /// <param name="_totalCnt"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            if (_pageSize < 1 || _pageSize > 250) _pageSize = 20;
            if (_totalCnt <= 0) return "0,0";

            int recsInLastPage = _totalCnt % _pageSize;
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageCount = ((_totalCnt - 1) / _pageSize) + 1;
            int pageIndex = Math.Min(Math.Max(_pageNo, 1), pageCount) - 1;

            // 마지막 페이지는 남아 있는 건수만 조회합니다.
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;

            int pageNoPageSize = _pageSize * pageIndex;
            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 관리자 사용자 목록을 조회합니다.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "20");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", GetDecryptParam("currentPage", "1"));
                string searchItem = (GetDecryptParam("searchItem", "") ?? "").Trim();
                string searchComplete = (GetDecryptParam("searchComplete", "") ?? "").Trim();
                string searchStatus = (GetDecryptParam("searchStatus", "") ?? "").Trim();
                string searchConfirm = (GetDecryptParam("searchConfirm", "") ?? "").Trim();
                int searchListID = ParseSearchListID(GetDecryptParam("searchListID", ""));
                string searchDateField = (GetDecryptParam("searchDateField", "StartDate") ?? "StartDate").Trim();
                string searchDateValue = GetDecryptParam("searchDateValue", "");
                string searchTarget = (GetDecryptParam("searchTarget", "A") ?? "A").Trim();
                string targetIDs = (GetDecryptParam("targetIDs", "") ?? "").Trim();
                string searchUserField = (GetDecryptParam("searchUserField", "Name") ?? "Name").Trim();
                string searchUserValue = (GetDecryptParam("searchUserValue", "") ?? "").Trim();
                string searchTitle = (GetDecryptParam("searchTitle", "") ?? "").Trim();
                string searchEmployment = (GetDecryptParam("searchEmployment", "") ?? "").Trim();

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                #region 날짜

                string startDate = "";
                string endDate = "";
                // 이전에 전달하던 시작일/종료일 파라미터도 함께 지원합니다.
                if (searchDateValue == "")
                {
                    string searchStartDate = GetDecryptParam("startDate", "");
                    string searchEndDate = GetDecryptParam("endDate", "");
                    if (searchStartDate != "" || searchEndDate != "")
                        searchDateValue = searchStartDate + "," + searchEndDate;
                }
                if (searchDateValue != "")
                {
                    string[] searchDates = searchDateValue.Split(',');
                    if (searchDates.Length != 2 || searchDates[0] == "" || searchDates[1] == "")
                        throw new ArgumentException("기간의 시작일과 종료일을 모두 입력해 주세요.");
                    startDate = DateHelpers.GetCultureDate(searchDates[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchDates[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                startDate = (startDate ?? "").Trim();
                endDate = (endDate ?? "").Trim();
                ValidateSearch(searchItem, searchComplete, searchDateField, startDate, endDate,
                    searchTarget, targetIDs, searchUserField, searchUserValue, searchTitle, searchEmployment, searchStatus, searchConfirm);
                if (searchTarget == "A") targetIDs = "";

                DAL.TOUser dal = new DAL.TOUser();
                List<object> listobj = new List<object>();
                if (setPageingStr[0] == "0") return listobj;

                using (DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID,
                    setPageingStr[0], setPageingStr[1], searchItem, searchComplete, searchDateField, startDate, endDate,
                    searchTarget, targetIDs, searchUserField, searchUserValue, searchTitle, searchEmployment, searchStatus, searchListID, searchConfirm))
                {
                    if (dtr != null && dtr.HasRows)
                    {
                        // 분류 코드는 목록 전체에 대해 한 번만 조회합니다.
                        Dictionary<string, string> itemNames = new Dictionary<string, string>();
                        DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");
                        foreach (DataRowView code in dv)
                        {
                            itemNames[code["CodeID"].ToString()] = UtilHelper.GetLangName(
                                code["NameBase"].ToString(), code["NameKo"].ToString(), code["NameEn"].ToString(),
                                code["NameJa"].ToString(), code["NameFr"].ToString(), code["NameZhcn"].ToString(),
                                code["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                        }

                        while (dtr.Read())
                        {
                            BE.AdminListInfo info = new BE.AdminListInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            string itemID = dtr["ItemID"].ToString();
                            string itemName;
                            info.ItemName = itemNames.TryGetValue(itemID, out itemName) ? itemName : itemID;
                            info.WorkplaceName = UtilHelper.GetLangName(dtr["WorkplaceBase"].ToString(), dtr["WorkplaceKo"].ToString(),
                                dtr["WorkplaceEn"].ToString(), dtr["WorkplaceJa"].ToString(), dtr["WorkplaceFr"].ToString(),
                                dtr["WorkplaceZhcn"].ToString(), dtr["WorkplaceZhtw"].ToString(), PreLang).ToReverseXSS();
                            info.DeptName = UtilHelper.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(),
                                dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(),
                                dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang).ToReverseXSS();
                            info.LogInID = dtr["LogInID"].ToString().ToReverseXSS();
                            info.Name = UtilHelper.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                            info.Title = dtr["Title"].ToString().ToReverseXSS();
                            info.DateStr = DateHelpers.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal)
                                + " ~ " + DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                            info.EmploymentStr = dtr["ActiveUser"].ToString() == "1" ? "재직" : "퇴직";
                            info.IsComplete = dtr["Status"].ToString() == "3";
                            // 검색과 동일하게 완료 여부와 현재 기간으로 계산한 상태를 표시합니다.
                            switch (dtr["ProgressStatus"].ToString())
                            {
                                case "1": info.StatusStr = "대기"; break;
                                case "2": info.StatusStr = "진행"; break;
                                case "3": info.StatusStr = "완료"; break;
                                case "4": info.StatusStr = "지연"; break;
                                default: info.StatusStr = ""; break;
                            }
                            info.CompleteYN = info.IsComplete ? "완료" : "미완료";
                            info.CompleteDateStr = DateHelpers.GetCultureDateChar8(dtr["CompleteDate"].ToString(), PreLangReal);
                            int completeUserID = dtr["CompleteUserID"].ToString().ToNumber<int>();
                            info.CompleteUserID = info.IsComplete && completeUserID > 0 ? completeUserID : 0;
                            info.CompleteUserName = "";
                            if (info.CompleteUserID > 0)
                            {
                                // 실제 완료 처리자 이름만 표시하고 대상자 이름으로 대체하지 않습니다.
                                info.CompleteUserName = UtilHelper.GetLangName(dtr["CompleteNameBase"].ToString(), dtr["CompleteNameKo"].ToString(),
                                    dtr["CompleteNameEn"].ToString(), dtr["CompleteNameJa"].ToString(), dtr["CompleteNameFr"].ToString(),
                                    dtr["CompleteNameZhcn"].ToString(), dtr["CompleteNameZhtw"].ToString(), PreLang).ToReverseXSS();
                                if (String.IsNullOrWhiteSpace(info.CompleteUserName)) info.CompleteUserName = "";
                            }
                            info.WriteDateStr = dtr["WriteDate"] == DBNull.Value ? ""
                                : Convert.ToDateTime(dtr["WriteDate"]).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.WriteUserName = UtilHelper.GetLangName(dtr["WriteNameBase"].ToString(), dtr["WriteNameKo"].ToString(),
                                dtr["WriteNameEn"].ToString(), dtr["WriteNameJa"].ToString(), dtr["WriteNameFr"].ToString(),
                                dtr["WriteNameZhcn"].ToString(), dtr["WriteNameZhtw"].ToString(), PreLang).ToReverseXSS();
                            info.FileCount = dtr["FileCount"].ToString().ToNumber<int>();
                            info.CertificateCount = dtr["CertificateCount"].ToString().ToNumber<int>();
                            Base.AdminConfirmation.Apply(info, dtr, PreLang);
                            listobj.Add(info);
                        }
                    }
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 관리자 사용자 목록의 전체 건수를 조회합니다.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchItem = (GetDecryptParam("searchItem", "") ?? "").Trim();
                string searchComplete = (GetDecryptParam("searchComplete", "") ?? "").Trim();
                string searchStatus = (GetDecryptParam("searchStatus", "") ?? "").Trim();
                string searchConfirm = (GetDecryptParam("searchConfirm", "") ?? "").Trim();
                int searchListID = ParseSearchListID(GetDecryptParam("searchListID", ""));
                string searchDateField = (GetDecryptParam("searchDateField", "StartDate") ?? "StartDate").Trim();
                string searchDateValue = GetDecryptParam("searchDateValue", "");
                string searchTarget = (GetDecryptParam("searchTarget", "A") ?? "A").Trim();
                string targetIDs = (GetDecryptParam("targetIDs", "") ?? "").Trim();
                string searchUserField = (GetDecryptParam("searchUserField", "Name") ?? "Name").Trim();
                string searchUserValue = (GetDecryptParam("searchUserValue", "") ?? "").Trim();
                string searchTitle = (GetDecryptParam("searchTitle", "") ?? "").Trim();
                string searchEmployment = (GetDecryptParam("searchEmployment", "") ?? "").Trim();

                #region 날짜

                string startDate = "";
                string endDate = "";
                if (searchDateValue == "")
                {
                    string searchStartDate = GetDecryptParam("startDate", "");
                    string searchEndDate = GetDecryptParam("endDate", "");
                    if (searchStartDate != "" || searchEndDate != "")
                        searchDateValue = searchStartDate + "," + searchEndDate;
                }
                if (searchDateValue != "")
                {
                    string[] searchDates = searchDateValue.Split(',');
                    if (searchDates.Length != 2 || searchDates[0] == "" || searchDates[1] == "")
                        throw new ArgumentException("기간의 시작일과 종료일을 모두 입력해 주세요.");
                    startDate = DateHelpers.GetCultureDate(searchDates[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchDates[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                startDate = (startDate ?? "").Trim();
                endDate = (endDate ?? "").Trim();
                ValidateSearch(searchItem, searchComplete, searchDateField, startDate, endDate,
                    searchTarget, targetIDs, searchUserField, searchUserValue, searchTitle, searchEmployment, searchStatus, searchConfirm);
                if (searchTarget == "A") targetIDs = "";

                DAL.TOUser dal = new DAL.TOUser();
                return dal.GetPageListCount(CurrentUserInfo.CompanyID, searchItem, searchComplete, searchDateField, startDate, endDate,
                    searchTarget, targetIDs, searchUserField, searchUserValue, searchTitle, searchEmployment, searchStatus, searchListID, searchConfirm);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 선택한 대상자의 첨부파일 파일을 조회합니다.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCertificateFileList(string p)
        {
            try
            {
                SetDecryptString(p);
                int listID;
                int userID;
                List<object> files = new List<object>();
                if (!Int32.TryParse(GetDecryptParam("listID", "0"), out listID) || listID <= 0
                    || !Int32.TryParse(GetDecryptParam("userID", "0"), out userID) || userID <= 0)
                    return files;

                List<BE.TOAttachInfo> certificates = new DAL.TOAttach().GetList(
                    CurrentUserInfo.CompanyID, listID, userID);
                foreach (BE.TOAttachInfo certificate in certificates)
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    parameters.Add("filePath", certificate.FilePath + certificate.FileGUID);
                    parameters.Add("fileName", certificate.FileName.Replace("&", "&amp;"));
                    files.Add(new
                    {
                        FileName = certificate.FileName,
                        FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(certificate.FileLength) / 1024) + " KB",
                        DownloadUrl = "/DeskPlusEIP/Common/DownLoad/DownLoad.aspx?p="
                            + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(parameters)))
                    });
                }
                return files;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region MOD()

        /// <summary>
        /// 완료/첨부 상태와 별개로 대상자에 대한 관리자 확인을 저장합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public BE.AdminListInfo SetConfirmation(string p)
        {
            Base.CompletionCancelRequest.Validate(Context.Request);
            if (CurrentUserInfo == null || CurrentUserInfo.CompanyID <= 0 || CurrentUserInfo.UserID <= 0
                || (!CurrentUserInfo.IsSysAdmin && !CurrentUserInfo.IsInRole("admin")
                    && !CurrentUserInfo.IsInRole("Education")))
                throw new HttpException(403, "확인을 처리할 권한이 없습니다.");

            SetDecryptString(p);
            int listID = Base.CompletionCancelRequest.ReadPositiveID(GetDecryptParam("listID", ""),
                "확인할 정보가 올바르지 않습니다.");
            int userID = Base.CompletionCancelRequest.ReadPositiveID(GetDecryptParam("userID", ""),
                "확인할 대상자 정보가 올바르지 않습니다.");
            string confirmYN = GetDecryptParam("confirmYN", "");
            if (confirmYN != "Y" && confirmYN != "N")
                throw new HttpException(400, "확인 여부가 올바르지 않습니다.");

            // 확인자는 요청값이 아닌 로그인한 관리자로 기록합니다.
            using (DataTableReader reader = new DAL.TOUser().SetConfirmation(CurrentUserInfo.CompanyID,
                listID, userID, confirmYN, CurrentUserInfo.UserID))
            {
                if (reader == null || !reader.Read())
                    throw new HttpException(404, "확인할 대상자를 찾을 수 없습니다.");
                return Base.AdminConfirmation.Read(reader, PreLang);
            }
        }

        /// <summary>
        /// 관리자 사용자 목록의 페이지당 표시 건수를 저장합니다.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModListUser(string p)
        {
            try
            {
                SetDecryptString(p);

                string isType = GetDecryptParam("isType", "");
                string pageSize = GetDecryptParam("pageSize", "0");

                BE.ListUserInfo info = new BE.ListUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.IsType = isType;
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();

                DAL.ListUser dal = new DAL.ListUser();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region Method

        private static int ParseSearchListID(string value)
        {
            if (String.IsNullOrEmpty(value)) return 0;
            int listID;
            if (!Int32.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out listID) || listID < 0)
                throw new ArgumentException("검색조건이 올바르지 않습니다.");
            return listID;
        }

        /// <summary>
        /// 목록과 건수 조회에서 사용하는 검색조건을 확인합니다.
        /// </summary>
        private static void ValidateSearch(string _searchItem, string _searchComplete,
            string _searchDateField, string _searchStartDate, string _searchEndDate,
            string _searchTarget, string _targetIDs, string _searchUserField,
            string _searchUserValue, string _searchTitle, string _searchEmployment, string _searchStatus, string _searchConfirm)
        {
            ValidateSearchLength(_searchItem, 10, "분류");
            ValidateSearchLength(_searchComplete, 1, "완료여부");
            ValidateSearchLength(_searchStatus, 1, "진행상태");
            ValidateSearchLength(_searchConfirm, 1, "확인여부");
            ValidateSearchLength(_searchDateField, 20, "기간 검색항목");
            ValidateSearchLength(_searchStartDate, 8, "시작일");
            ValidateSearchLength(_searchEndDate, 8, "종료일");
            ValidateSearchLength(_searchTarget, 1, "대상");
            ValidateSearchLength(_targetIDs, 11000, "선택 대상");
            ValidateSearchLength(_searchUserField, 20, "사용자 검색항목");
            ValidateSearchLength(_searchUserValue, 100, "사용자 검색어");
            ValidateSearchLength(_searchTitle, 100, "과정명 검색어");
            ValidateSearchLength(_searchEmployment, 1, "재직여부");

            if (_searchDateField != "StartDate" && _searchDateField != "EndDate")
                throw new ArgumentException("기간 검색항목이 올바르지 않습니다.");
            if (_searchUserField != "Name" && _searchUserField != "LogInID")
                throw new ArgumentException("사용자 검색항목이 올바르지 않습니다.");
            if (_searchTarget != "A" && _searchTarget != "G")
                throw new ArgumentException("대상 검색조건이 올바르지 않습니다.");
            if (_searchComplete != "" && _searchComplete != "3" && _searchComplete != "0")
                throw new ArgumentException("완료여부 검색조건이 올바르지 않습니다.");
            if (_searchStatus != "" && _searchStatus != "1" && _searchStatus != "2"
                && _searchStatus != "3" && _searchStatus != "4")
                throw new ArgumentException("진행상태 검색조건이 올바르지 않습니다.");
            if (_searchEmployment != "" && _searchEmployment != "1" && _searchEmployment != "0")
                throw new ArgumentException("재직여부 검색조건이 올바르지 않습니다.");
            if (_searchConfirm != "" && _searchConfirm != "Y" && _searchConfirm != "N")
                throw new ArgumentException("확인여부 검색조건이 올바르지 않습니다.");

            if (_searchStartDate != "" || _searchEndDate != "")
            {
                DateTime startDate;
                DateTime endDate;
                if (!DateTime.TryParseExact(_searchStartDate, "yyyyMMdd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out startDate)
                    || !DateTime.TryParseExact(_searchEndDate, "yyyyMMdd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out endDate)
                    || startDate > endDate)
                    throw new ArgumentException("기간 검색조건이 올바르지 않습니다.");
            }

            if (_searchTarget == "G")
            {
                string[] tokens = _targetIDs.Split(',');
                if (tokens.Length != 1)
                    throw new ArgumentException("부서는 한 개만 선택해 주세요.");
                foreach (string token in tokens)
                {
                    int targetID;
                    if (!Int32.TryParse(token, NumberStyles.None, CultureInfo.InvariantCulture,
                        out targetID) || targetID <= 0)
                        throw new ArgumentException("선택 대상 정보가 올바르지 않습니다.");
                }
            }
        }

        private static void ValidateSearchLength(string _value, int _maxLength, string _fieldName)
        {
            if (_value.Length > _maxLength)
                throw new ArgumentException(_fieldName + "의 입력 길이를 줄여 주세요.");
        }

        #endregion

    }
}
