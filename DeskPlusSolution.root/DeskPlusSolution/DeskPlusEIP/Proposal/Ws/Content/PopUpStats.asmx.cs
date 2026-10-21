using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Proposal.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Proposal.Web.Ws.Content
{
    /// <summary>
    /// PopUpStats의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class PopUpStats : Base.BaseWSProposal
    {
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
        /// 작성자 : shine
        /// 통계 보기 총 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageCount(string p)
        {
            SetDecryptString(p);

            string judgeGubun = GetDecryptParam("judgeGubun", "0");
            string startYears = GetDecryptParam("startYears", System.DateTime.Now.Year.ToString());
            string startMonths = GetDecryptParam("startMonths", System.DateTime.Now.Month.ToString());
            string endYears = GetDecryptParam("endYears", System.DateTime.Now.Year.ToString());
            string endMonths = GetDecryptParam("endMonths", System.DateTime.Now.Month.ToString());
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");

            string startYearMonth = startMonths.Length == 1 ? startYears + "0" + startMonths : startYears + startMonths;
            string endYearMonth = endMonths.Length == 1 ? endYears + "0" + endMonths : endYears + endMonths;

            DAL.List biz = new DAL.List();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, judgeGubun, startYearMonth, endYearMonth, searchField, searchValue);

        }

        /// <summary>
        /// 작성자 : shine
        /// 전체 제안 보기 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.StatusList> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
                int pageSizePageNo = GetDecryptParam("pageSizePageNo", "1").ToNumber<int>();
                int totalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

                string judgeGubun = GetDecryptParam("judgeGubun", "0");
                string startYears = GetDecryptParam("startYears", System.DateTime.Now.Year.ToString());
                string startMonths = GetDecryptParam("startMonths", System.DateTime.Now.Month.ToString());
                string endYears = GetDecryptParam("endYears", System.DateTime.Now.Year.ToString());
                string endMonths = GetDecryptParam("endMonths", System.DateTime.Now.Month.ToString());
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string gubun = GetDecryptParam("gubun", "");

                int companyID = CurrentUserInfo.CompanyID;
                string startYearMonth = startMonths.Length == 1 ? startYears + "0" + startMonths : startYears + startMonths;
                string endYearMonth = endMonths.Length == 1 ? endYears + "0" + endMonths : endYears + endMonths;

                string rtnSetPaging = SetPaging(totalCount, pageSize, pageSizePageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.List biz = new DAL.List();
                List<Ws.Content.StatusList> alList = new List<StatusList>();

                DataTableReader dtr = biz.GetPageList(companyID, setPageingStr[0], setPageingStr[1], 
                    judgeGubun, startYearMonth, endYearMonth, searchField, searchValue);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.Content.StatusList info = new StatusList();

                            string years = string.Empty;

                            info.ParentDeptName = dtr["ParentDeptName"].ToString();
                            info.DeptName = dtr["DeptName"].ToString();
                            info.Status = GetStatus(dtr["Status"].ToString());
                            info.Grade = dtr["LastGrade"].ToString();
                            info.NoCnt = dtr["NoCnt"].ToString();
                            alList.Add(info);
                        }
                    }
                }
                return alList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }
    }

    [Serializable]
    public class StatusList
    {
        public StatusList() { }

        public string ParentDeptName { get; set; }
        public string DeptName { get; set; }
        public string Grade { get; set; }
        public string Status { get; set; }
        public string NoCnt { get; set; }
    }
}
