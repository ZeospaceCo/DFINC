using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.PMS.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.PMS.Web.Ws.Template
{
    /// <summary>
    /// JList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class JList : Base.BaseWSPMS
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
        /// 작성자 : 심재운
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalItemCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string currentPage = GetDecryptParam("currentPage", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                string order = GetDecryptParam("order", "");
                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
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

                BE.TemplateInfo listInfo = new Zeospace.DeskPlus.PMS.BE.TemplateInfo();
                DAL.Template list = new Zeospace.DeskPlus.PMS.DAL.Template();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.ProjectID = projectID;
                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();
                listInfo.SearchStartDate = startDate;
                listInfo.SearchEndDate = endDate;

                DataTableReader dtr = list.GetPageList(listInfo);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.TemplateInfo info = new TemplateInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ProjectID = dtr["ProjectID"].ToString().ToNumber<int>();
                            info.TemplateID = dtr["TemplateID"].ToString().ToNumber<int>();
                            info.TemplateNum = dtr["TemplateNum"].ToString().ToNumber<int>();
                            info.Subjects = dtr["Subjects"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            #region 개설자 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion
                            
                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDateStr = string.Empty;

                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;
                            info.Row = listNum.ToString();


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
        /// 사용자 정보 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int status = GetDecryptParam("status", "0").ToNumber<int>();

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

                BE.TemplateInfo listInfo = new Zeospace.DeskPlus.PMS.BE.TemplateInfo();
                DAL.Template list = new Zeospace.DeskPlus.PMS.DAL.Template();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.ProjectID = projectID;
                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = startDate;
                listInfo.SearchEndDate = endDate;

                int cnt = list.GetPageCount(listInfo);

                return cnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion 
    }
}
