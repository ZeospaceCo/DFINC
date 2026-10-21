using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Data;
using Zeospace.DeskPlus.Search.BLL;
using System.Collections;

namespace Zeospace.DeskPlus.Search.Web.Ws.Admin.Index
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

        #region GET()
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
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-16
        /// 작성목적 : 리스트 가져오기
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

                int userID = CurrentUserInfo.UserID;
                int deptID = CurrentUserInfo.DeptID;

                if (searchValue == "") userID = 0;

                List<object> listobj = new List<object>();
                switch (searchField.ToUpper())
                {
                    case "APPROVAL":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Approval.Search.GetPageList(CurrentUserInfo.CompanyID, userID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "BOARD":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Board.Search.GetPageList(CurrentUserInfo.CompanyID, userID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "EDM":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.EDM.Search.GetPageList(CurrentUserInfo.CompanyID, userID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "ADDRESS":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Address.Search.GetPageList(CurrentUserInfo.CompanyID, userID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "WEBHARD":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.WebHard.Search.GetPageList(CurrentUserInfo.CompanyID, userID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "COOP":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Coop.Search.GetPageList(CurrentUserInfo.CompanyID, userID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "TASK":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.TAS.Search.GetPageList(CurrentUserInfo.CompanyID, userID, deptID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "PIM":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Pim.Search.GetPageList(CurrentUserInfo.CompanyID, userID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
                        break;
                    case "RM":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.RM.Search.GetPageList(CurrentUserInfo.CompanyID, userID, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>(), searchField, searchValue, startDate, endDate, order));
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
                //string searchGroup = GetDecryptParam("searchGroup", "");

                //if (searchValue.Length == 0) return 0;
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

                int userID = CurrentUserInfo.UserID;
                int deptID = CurrentUserInfo.DeptID;

                if (searchValue == "") userID = 0;
                switch (searchField.ToUpper())
                {
                    case "APPROVAL":
                        pageCount = Zeospace.DeskPlus.Search.BLL.Approval.Search.GetPageListCount(CurrentUserInfo.CompanyID, userID, searchValue, startDate, endDate);
                        break;
                    case "BOARD":
                        pageCount = Zeospace.DeskPlus.Search.BLL.Board.Search.GetPageListCount(CurrentUserInfo.CompanyID, userID, searchValue, startDate, endDate);
                        break;
                    case "EDM":
                        pageCount = Zeospace.DeskPlus.Search.BLL.EDM.Search.GetPageListCount(CurrentUserInfo.CompanyID, userID, searchValue, startDate, endDate);
                        break;
                    case "ADDRESS":
                        pageCount = Zeospace.DeskPlus.Search.BLL.Address.Search.GetPageListCount(CurrentUserInfo.CompanyID, userID, searchValue, startDate, endDate);
                        break;
                    case "WEBHARD":
                        pageCount = Zeospace.DeskPlus.Search.BLL.WebHard.Search.GetPageListCount(CurrentUserInfo.CompanyID, userID, searchValue, startDate, endDate);
                        break;
                    case "COOP":
                        pageCount = Zeospace.DeskPlus.Search.BLL.Coop.Search.GetPageListCount(CurrentUserInfo.CompanyID, userID, searchValue, startDate, endDate);
                        break;
                    case "TASK":
                        pageCount = Zeospace.DeskPlus.Search.BLL.TAS.Search.GetPageListCount(CurrentUserInfo.CompanyID, userID, deptID, searchValue, startDate, endDate);
                        break;
                    case "PIM":
                        pageCount = Zeospace.DeskPlus.Search.BLL.Pim.Search.GetPageListCount(CurrentUserInfo.CompanyID, userID, searchValue, startDate, endDate);
                        break;
                    case "RM":
                        pageCount = Zeospace.DeskPlus.Search.BLL.RM.Search.GetPageListCount(CurrentUserInfo.CompanyID, userID, searchValue, startDate, endDate);
                        break;
                    default:

                        break;

                }

                return pageCount;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion 


        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-11-28
        /// 작성목적 : 모든 레코드 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAllRecord(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Search.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Search.Web.Ws.Admin.Index.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion


            SetDecryptString(p);
            string func = GetDecryptParam("f", "");

            string[] funcArr = func.Split(',');

            List<object> listobj = new List<object>();

            for (int i = 0; i < funcArr.Length; i++)
            {
                switch (funcArr[i].ToUpper())
                {
                    case "BOARD":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Board.Search.GetAllIndexRecords());
                        break;
                    case "APPROVAL":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Approval.Search.GetAllIndexRecords());
                        break;
                    case "EDM":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.EDM.Search.GetAllIndexRecords());
                        break;
                    case "ADDRESS":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.EDM.Search.GetAllIndexRecords());
                        break;
                    case "WEBHARD":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.EDM.Search.GetAllIndexRecords());
                        break;
                    case "COOP":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Coop.Search.GetAllIndexRecords());
                        break;
                    case "TASK":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.TAS.Search.GetAllIndexRecords());
                        break;
                    case "PIM":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.Pim.Search.GetAllIndexRecords());
                        break;
                    case "RM":
                        listobj.AddRange(Zeospace.DeskPlus.Search.BLL.RM.Search.GetAllIndexRecords());
                        break;
                    default:
                        break;
                }
            }

            return listobj;
        }
    }
}
