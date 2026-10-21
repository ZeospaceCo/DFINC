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
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.PMS.Web.Ws.Project
{
    /// <summary>
    /// Main의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Main : Base.BaseWSPMS
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
        /// 게시판 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetBoardPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string currentPage = GetDecryptParam("currentPage", "0");

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                int ProjectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();

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

                BE.BoardInfo listInfo = new Zeospace.DeskPlus.PMS.BE.BoardInfo();
                DAL.Board list = new Zeospace.DeskPlus.PMS.DAL.Board();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.ProjectID = ProjectID;
                listInfo.MenuID = MenuID;
                listInfo.SearchField = "";
                listInfo.SearchValue = "";
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();
                listInfo.SearchStartDate = "";
                listInfo.SearchEndDate = "";

                DataTableReader dtr = list.GetPageList(listInfo);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.BoardInfo info = new Zeospace.DeskPlus.PMS.BE.BoardInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ProjectID = dtr["ProjectID"].ToString().ToNumber<int>();
                            info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                            info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Depth = dtr["Depth"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            //info.Contents = dtr["Contents"].ToString();
                            //info.MobileContent = dtr["MobileContent"].ToString();
                            info.ReadCount = dtr["ReadCount"].ToString().ToNumber<int>();
                            info.Gubun = dtr["Gubun"].ToString().ToNumber<int>();
                            info.DeptCodeID = dtr["DeptCodeID"].ToString().ToNumber<int>();
                            info.NameBase = this.GetLangName(
                                                dtr["NameBase"].ToString(),
                                                dtr["NameKo"].ToString(),
                                                dtr["NameEn"].ToString(),
                                                dtr["NameJa"].ToString(),
                                                dtr["NameFr"].ToString(),
                                                dtr["NameZhCn"].ToString(),
                                                dtr["NameZhTw"].ToString(), PreLang);

                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            DateTime dt;

                            //작성일
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDateStr = string.Empty;

                            //info.WriteDate = dtr["WriteDate"].ToString();
                            //info.WriteDateYMD = dtr["WriteDateYMD"].ToString();


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
        /// 게시판 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetBoardListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                int ProjectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int icnt = 0;

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

                BE.BoardInfo listInfo = new Zeospace.DeskPlus.PMS.BE.BoardInfo();
                DAL.Board list = new Zeospace.DeskPlus.PMS.DAL.Board();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.ProjectID = ProjectID;
                listInfo.MenuID = MenuID;
                listInfo.SearchField = "";
                listInfo.SearchValue = "";
                listInfo.SearchStartDate = "";
                listInfo.SearchEndDate = "";

                icnt = list.GetPageCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 단위 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetUnitPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string currentPage = GetDecryptParam("currentPage", "0");

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                int ProjectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int weightTotal = GetDecryptParam("weightTotal", "0").ToNumber<int>();

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

                BE.UnitInfo listInfo = new Zeospace.DeskPlus.PMS.BE.UnitInfo();
                DAL.Unit list = new Zeospace.DeskPlus.PMS.DAL.Unit();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.ProjectID = ProjectID;
                listInfo.Status = 0;
                listInfo.SearchField = "";
                listInfo.SearchValue = "";
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

                DataTableReader dtr = list.GetPageList(listInfo);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.UnitInfo info = new Zeospace.DeskPlus.PMS.BE.UnitInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ProjectID = dtr["ProjectID"].ToString().ToNumber<int>();
                            info.UnitID = dtr["UnitID"].ToString().ToNumber<int>();
                            info.Code = dtr["Code"].ToString();
                            info.Subjects = dtr["Subjects"].ToString();
                            //info.Contents = dtr["Contents"].ToString();
                            info.Process = dtr["Process"].ToString().ToNumber<int>();
                            info.PlanMoney = dtr["PlanMoney"].ToString().ToNumber<int>();
                            info.ExecuteMoney = dtr["ExecuteMoney"].ToString().ToNumber<int>();
                            info.StatusStr = RecStatus(dtr["Status"].ToString().ToNumber<int>());
                            info.ChargeDeptID = dtr["ChargeDeptID"].ToString();
                            info.RelateDeptID = dtr["RelateDeptID"].ToString();
                            
                            info.PlanDay = dtr["PlanDay"].ToString().ToNumber<int>(); ;
                            info.WeightPercent = dtr["WeightPercent"].ToString().ToNumber<int>();
                                                     

                            info.NameBase = this.GetLangName(
                                                dtr["NameBase"].ToString(),
                                                dtr["NameKo"].ToString(),
                                                dtr["NameEn"].ToString(),
                                                dtr["NameJa"].ToString(),
                                                dtr["NameFr"].ToString(),
                                                dtr["NameZhCn"].ToString(),
                                                dtr["NameZhTw"].ToString(), PreLang);

                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            //info.WriteDate = dtr["WriteDate"].ToString();

                            DateTime dt;


                            if (DateTime.TryParse(dtr["PlanStartDate"].ToString(), out dt))
                                info.PlanStartDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.PlanStartDateStr = string.Empty;

                            if (DateTime.TryParse(dtr["PlanEndDate"].ToString(), out dt))
                                info.PlanEndDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.PlanEndDateStr = string.Empty;

                            if (DateTime.TryParse(dtr["StartDate"].ToString(), out dt))
                                info.StartDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.StartDateStr = string.Empty;

                            if (DateTime.TryParse(dtr["EndDate"].ToString(), out dt))
                                info.EndDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.EndDateStr = string.Empty;

                            info.ProcessPercent = RecProcessPercent(info.WeightPercent, info.Process, weightTotal);
                            
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
        /// 단위 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetUnitListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                int ProjectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int icnt = 0;

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

                BE.UnitInfo listInfo = new Zeospace.DeskPlus.PMS.BE.UnitInfo();
                DAL.Unit list = new Zeospace.DeskPlus.PMS.DAL.Unit();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.ProjectID = ProjectID;
                listInfo.Status = 0;
                listInfo.SearchField = "";
                listInfo.SearchValue = "";

                icnt = list.GetPageCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 상태값 추출하기
        /// </summary>
        /// <param name="_status"></param>
        /// <returns></returns>
        private string RecStatus(int _status)
        {
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Ws.Project.List).Assembly;	//어셈블리 선언

            if (_status == 0) return "";
            string response = "";

            switch (_status)
            {
                case 1:
                    response = GetResource("List_Status1");
                    break;
                case 2:
                    response = GetResource("List_Status2");
                    break;
                case 3:
                    response = GetResource("List_Status3");
                    break;
                case 4:
                    response = GetResource("List_Status4");
                    break;
                case 5:
                    response = GetResource("List_Status5");
                    break;
            }


            return response;
        }


        /// <summary>
        /// Recs the process percent.
        /// </summary>
        /// <param name="_weightPercent">The _weight percent.</param>
        /// <param name="_process">The _process.</param>
        /// <param name="_weightTotal">The _weight total.</param>
        /// <returns></returns>
        private int RecProcessPercent(int _weightPercent, int _process, int _weightTotal)
        {
            if (_weightPercent == 0 || _process == 0)
            {
                return 0;
            }
            else
            {
                int weightPercent = _weightPercent;//가중치
                int process = _process;//진행률

                double realProcess = Convert.ToDouble(weightPercent) / Convert.ToDouble(_weightTotal) * process;

                return Convert.ToInt32(realProcess);
            }
        }

        #endregion        
    }
}
