using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Statistics.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Statistics.Web.Ws.Content.Login
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSStatistics
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

        #region 로그인 성공 관련
        /// <summary>
        /// 작성자 : seo
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

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                DataTableReader dtr = lg.GetPageList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.LoginInfo info = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            #region 개설자 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>();

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
        /// 작성자 : seo
        /// 헤더 토탈 정보
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListLoginTotal(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0;
                listInfo.PageSizePageNo = 0;

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = "";

                DataTableReader dtr = lg.GetTotalLoginList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.LoginInfo info = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>();

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
        /// 통계 로그인 통계 성공 카운트
        /// 작성자 : seo
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetLSListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                int icnt = 0;

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0; //Int32.Parse(recordPerPage);
                listInfo.PageSizePageNo = 0;// Int32.Parse(currentPage);

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                icnt = lg.GetPageLSCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region 로그인 실패 관련
        /// <summary>
        /// 작성자 : seo
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageFList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                DataTableReader dtr = lg.GetPageFList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.LoginInfo info = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            #region 개설자 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>();

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
        /// 작성자 : seo
        /// 헤더 토탈 정보
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageFListLoginTotal(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0;
                listInfo.PageSizePageNo = 0;

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = "";

                DataTableReader dtr = lg.GetTotalLoginFList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.LoginInfo info = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>();

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
        /// 통계 로그인 통계 성공 카운트
        /// 작성자 : seo
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetLSFListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                int icnt = 0;

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0; //Int32.Parse(recordPerPage);
                listInfo.PageSizePageNo = 0;// Int32.Parse(currentPage);

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                icnt = lg.GetPageLSFCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region 모바일 로그인 성공 관련
        /// <summary>
        /// 작성자 : seo
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageMList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                DataTableReader dtr = lg.GetPageMList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.LoginInfo info = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            #region 개설자 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>();

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
        /// 작성자 : seo
        /// 헤더 토탈 정보
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageMListLoginTotal(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0;
                listInfo.PageSizePageNo = 0;

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = "";

                DataTableReader dtr = lg.GetTotalLoginMList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.LoginInfo info = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>();

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
        /// 통계 로그인 통계 성공 카운트
        /// 작성자 : seo
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetLSMListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                int icnt = 0;

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0; //Int32.Parse(recordPerPage);
                listInfo.PageSizePageNo = 0;// Int32.Parse(currentPage);

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                icnt = lg.GetPageLSMCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region 모바일 로그인 실패 관련
        /// <summary>
        /// 작성자 : seo
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageMFList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                DataTableReader dtr = lg.GetPageMFList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.LoginInfo info = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            #region 개설자 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>();

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
        /// 작성자 : seo
        /// 헤더 토탈 정보
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageMFListLoginTotal(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0;
                listInfo.PageSizePageNo = 0;

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = "";

                DataTableReader dtr = lg.GetTotalLoginMFList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.LoginInfo info = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>();

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
        /// 통계 로그인 통계 성공 카운트
        /// 작성자 : seo
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetLSMFListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string recordPerPage = GetDecryptParam("pageSize", "0");
                string currentPage = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string TargetYear = GetDecryptParam("targetYear", "");

                int icnt = 0;

                BE.LoginInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.LoginInfo();
                DAL.Login lg = new Zeospace.DeskPlus.Statistics.DAL.Login();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0; //Int32.Parse(recordPerPage);
                listInfo.PageSizePageNo = 0;// Int32.Parse(currentPage);

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                icnt = lg.GetPageLSMFCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

    }
    #endregion
}
