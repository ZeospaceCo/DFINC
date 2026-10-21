using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Data;

namespace Zeospace.DeskPlus.Search.Web.Ws
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSSearch
    {

        #region 리스트

        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 작성자 이름설정하기
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_titleName">Name of the _title.</param>
        /// <param name="_dutyName">Name of the _duty.</param>
        /// <returns></returns>
        private string GetWriteUserName(string _writeUserName, string _deptName,
            string _titleName, string _dutyName)
        {
            string nameFieldFormat = _writeUserName;
            string applyCategory = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "VWNameFieldCategory", "0");
            nameFieldFormat = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "VWNameField", "WriteName");
            nameFieldFormat = (nameFieldFormat.IndexOf("WriteName") != -1) ? nameFieldFormat.Replace("WriteName", _writeUserName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("DeptName") != -1) ? nameFieldFormat.Replace("DeptName", _deptName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("TitleName") != -1) ? nameFieldFormat.Replace("TitleName", _titleName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("DutyName") != -1) ? nameFieldFormat.Replace("DutyName", _dutyName) : nameFieldFormat;

            //이름 보이기 (이름:WriteName, 부서:Dept, 직급:Title, 직책:Duty)
            return nameFieldFormat;

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 게시판 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string moduleName = GetDecryptParam("moduleName", "");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string order = GetDecryptParam("order", "");

                string preLang = CurrentUserInfo.PreferredLanguage;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (searchTerm == "week" || searchTerm == "month")
                {
                    if (searchTerm == "week")
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                List<object> listobj = new List<object>();
                switch (moduleName.ToUpper())
                {
                    case "APPROVAL":
                        //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Approval 0 : " + System.DateTime.Now, AppSettingType.SEARCH);
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Approval.Search.GetPageListH(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "BOARD":
                        //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Board 0 : " + System.DateTime.Now, AppSettingType.SEARCH);
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Board.Search.GetPageListH(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "EDM":
                        //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("EDM 0 : " + System.DateTime.Now, AppSettingType.SEARCH);
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.EDM.Search.GetPageListH(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "ADDRESS":
                        //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Address 0 : " + System.DateTime.Now, AppSettingType.SEARCH);
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Address.Search.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "WEBHARD":
                        //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Webhard 0 : " + System.DateTime.Now, AppSettingType.SEARCH);
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.WebHard.Search.GetPageListH(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "COOP":
                        //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Coop 0 : " + System.DateTime.Now, AppSettingType.SEARCH);
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Coop.Search.GetPageListH(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "TASK":
                        //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Coop 0 : " + System.DateTime.Now, AppSettingType.SEARCH);
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.TAS.Search.GetPageListH(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "PIM":
                        //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Pim 0 : " + System.DateTime.Now, AppSettingType.SEARCH);
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Pim.Search.GetPageListH(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "RM":
                        //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Pim 0 : " + System.DateTime.Now, AppSettingType.SEARCH);
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.RM.Search.GetPageListH(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    default:

                        break;

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
        /// 작성자 : 
        /// 검색 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string moduleName = GetDecryptParam("moduleName", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");

                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int pageCount = 0;

                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (searchTerm == "week" || searchTerm == "month")
                {
                    if (searchTerm == "week")
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion


                switch (moduleName.ToUpper())
                {
                    case "APPROVAL":
                        pageCount = Zeospace.DeskPlus.Search.BLL.Approval.Search.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue, startDate, endDate);
                        break;
                    case "BOARD":
                        pageCount = Zeospace.DeskPlus.Search.BLL.Board.Search.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue, startDate, endDate);
                        break;
                    case "EDM":
                        pageCount = Zeospace.DeskPlus.Search.BLL.EDM.Search.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue, startDate, endDate);
                        break;
                    case "ADDRESS":
                        pageCount = Zeospace.DeskPlus.Search.BLL.Address.Search.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue, startDate, endDate);
                        break;
                    case "WEBHARD":
                        pageCount = Zeospace.DeskPlus.Search.BLL.WebHard.Search.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue, startDate, endDate);
                        break;
                    case "COOP":
                        pageCount = Zeospace.DeskPlus.Search.BLL.Coop.Search.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue, startDate, endDate);
                        break;
                    case "TASK":
                        pageCount = Zeospace.DeskPlus.Search.BLL.TAS.Search.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, searchValue, startDate, endDate);
                        break;
                    case "PIM":
                        pageCount = Zeospace.DeskPlus.Search.BLL.Pim.Search.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue, startDate, endDate);
                        break;
                    case "RM":
                        pageCount = Zeospace.DeskPlus.Search.BLL.RM.Search.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue, startDate, endDate);
                        break;
                    default:

                        break;

                }

                return pageCount;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return 0;
            }
        }
        #endregion
    }
}
