using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Education.Web.Ws.Content
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSEducation
    {
        #region GET()
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_totalCnt"></param>
        /// <param name="_pageSize"></param>
        /// <param name="_pageNo"></param>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            int recsInLastPage = _totalCnt % _pageSize;

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
                string searchItem = GetDecryptParam("searchItem", "");
                string searchStatus = GetDecryptParam("searchStatus", "");
                string searchComplete = GetDecryptParam("searchComplete", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchDateField = GetDecryptParam("searchDateField", "");
                string searchDateValue = GetDecryptParam("searchDateValue", "");       

                string preLang = CurrentUserInfo.PreferredLanguage;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                #region 날짜

                string startDate = "";
                string endDate = "";

                if (searchDateValue != "")
                {
                    startDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                DAL.List dal = new DAL.List();

                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                    setPageingStr[0], setPageingStr[1], searchItem, searchStatus, searchComplete, searchField, searchValue, searchDateField, startDate, endDate);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.ItemName = dtr["ItemName"].ToString();
                            info.DateStr = DateHelpers.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal) +" ~ " + 
                                DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                            info.FileCount = dtr["FileCount"].ToString().ToNumber<int>();
                            
                            if (dtr["ExcessDay"].ToString().ToNumber<int>() > 0)
                            {
                                info.ExcessDay = "-" + dtr["ExcessDay"].ToString();
                            }
                            else if (dtr["ExcessDay"].ToString().ToNumber<int>() < 0)
                            {
                                info.ExcessDay = dtr["ExcessDay"].ToString().Replace("-", "+");
                            }
                            else
                            {
                                info.ExcessDay = "0";
                            }

                            if (dtr["Status"].ToString() == "1")
                            {
                                info.StatusStr = "대기";
                            }
                            else if (dtr["Status"].ToString() == "2")
                            {
                                info.StatusStr = "진행중";
                            }
                            else if (dtr["Status"].ToString() == "3")
                            {
                                info.StatusStr = "완료";
                            }
                            else if (dtr["Status"].ToString() == "4")
                            {
                                info.StatusStr = "지연";
                            }

                            if (dtr["Status"].ToString() == "3")
                            {
                                info.CompleteYN = "완료";
                            }
                            else
                            {
                                info.CompleteYN = "미완료";
                            }                            

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
                string searchItem = GetDecryptParam("searchItem", "");
                string searchStatus = GetDecryptParam("searchStatus", "");
                string searchComplete = GetDecryptParam("searchComplete", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchDateField = GetDecryptParam("searchDateField", "");
                string searchDateValue = GetDecryptParam("searchDateValue", "");

                string startDate = "";
                string endDate = "";

                if (searchDateValue != "")
                {
                    startDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                int icnt = 0;

                DAL.List dal = new DAL.List();
                icnt = dal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchItem,searchStatus, searchComplete, 
                    searchField, searchValue, searchDateField, startDate, endDate);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 검색조건과 관계없이 현재 사용자의 전체 교육 진행 현황을 조회합니다.
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public StatusSummaryInfo GetStatusSummary(string p)
        {
            try
            {
                SetDecryptString(p);

                StatusSummaryInfo info = new StatusSummaryInfo();
                DAL.List dal = new DAL.List();

                using (DataTableReader dtr = dal.GetStatusSummary(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID))
                {
                    if (dtr != null && dtr.Read())
                    {
                        info.TotalCount = dtr["TotalCount"].ToString().ToNumber<int>();
                        info.WaitCount = dtr["WaitCount"].ToString().ToNumber<int>();
                        info.ProgressCount = dtr["ProgressCount"].ToString().ToNumber<int>();
                        info.CompleteCount = dtr["CompleteCount"].ToString().ToNumber<int>();
                        info.DelayCount = dtr["DelayCount"].ToString().ToNumber<int>();
                    }
                }

                return info;
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
    }

    [Serializable]
    public class StatusSummaryInfo
    {
        public int TotalCount { get; set; }
        public int WaitCount { get; set; }
        public int ProgressCount { get; set; }
        public int CompleteCount { get; set; }
        public int DelayCount { get; set; }
    }
}
