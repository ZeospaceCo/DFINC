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

namespace Zeospace.DeskPlus.Statistics.Web.Ws.Content.Attach
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
        CommonLib commLib = new DeskPlus.Base.ApplicationFrameworks.Helper.CommonLib();
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

        private string GetTagInfo(string _approvalTag)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Statistics.Web.Resource.GlobalString.string";          //리소스명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Statistics.Web.Ws.Content.Attach.List).Assembly;	//어셈블리 선언
            
            this.GetResourceManager();            
            #endregion

            string rtnval = "";
            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("1.................." + _approvalTag);
            switch (_approvalTag)
            {
                case "Approval":
                    rtnval = this.GetResource("Approval");                    
                    break;
                case "Opinion":
                    rtnval = this.GetResource("ModuleNM");
                    break;
                case "WebHard":
                    rtnval = this.GetResource("WebHard");
                    break;
                case "ResourceManager":
                    rtnval = this.GetResource("ResourceManager");
                    break;
                case "PMS":
                    rtnval = this.GetResource("PMS");
                    break;
                case "Pims":
                    rtnval = this.GetResource("Pims");
                    break;
                case "Community":
                    rtnval = this.GetResource("Community");
                    break;
                case "Board":
                    rtnval = this.GetResource("Board");
                    break;
                case "Coop":
                    rtnval = this.GetResource("Coop");
                    break;                
                default:
                    rtnval = this.GetResource("Etc") + "(" + _approvalTag + ")";
                    break;
            }
            return rtnval;
        }

        #region 모듈별 통계

        /// <summary>
        /// 작성자 : 방영환
        /// 모듈별통계 리스트 가져오기
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                DataTableReader dtr = attach.GetPageMList(listInfo);

                ObjList listobj = new ObjList();

                if(dtr != null)
                {
                    if(dtr.HasRows)
                    {
                        while(dtr.Read())
                        {
                            BE.AttachInfo info = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();


                            info.SearchGubun = GetTagInfo(dtr["SearchGubun"].ToString());

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>().ToString();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>().ToString();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>().ToString();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>().ToString();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>().ToString();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>().ToString();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>().ToString();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>().ToString();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>().ToString();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>().ToString();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>().ToString();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>().ToString();

                            listobj.Add(info);                            
                        }
                    }
                }

                return listobj;
            }
            catch(Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 방영환
        /// 모듈별통계 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetMListCount(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();
                

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0; //Int32.Parse(recordPerPage);
                listInfo.PageSizePageNo = 0;// Int32.Parse(currentPage);

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                icnt = attach.GetPageMCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : 방영환
        /// 모듈별통계 리스트 소계
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageMListTotal(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0;
                listInfo.PageSizePageNo = 0;

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = "";

                DataTableReader dtr = attach.GetTotalPageMList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>().ToString("#,##0");

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

        #endregion

        #region 사용자별 통계

        /// <summary>
        /// 작성자 : 방영환
        /// 사용자별통계 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageUList(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                DataTableReader dtr = attach.GetPageUList(listInfo);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();

                            //info.SearchGubun = dtr["SearchGubun"].ToString();
                            info.SearchGubun = this.GetLangName(
                             dtr["NameBase"].ToString(),
                             dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                             dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>().ToString();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>().ToString();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>().ToString();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>().ToString();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>().ToString();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>().ToString();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>().ToString();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>().ToString();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>().ToString();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>().ToString();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>().ToString();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>().ToString();
                            
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
        /// 작성자 : 방영환
        /// 사용자별통계 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetUListCount(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();


                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0; //Int32.Parse(recordPerPage);
                listInfo.PageSizePageNo = 0;// Int32.Parse(currentPage);

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                icnt = attach.GetPageUCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 방영환
        /// 사용자별통계 리스트 소계
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageUListTotal(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0;
                listInfo.PageSizePageNo = 0;

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = "";

                DataTableReader dtr = attach.GetTotalPageUList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>().ToString("#,##0");

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

        #endregion

        #region 부서별 통계

        /// <summary>
        /// 작성자 : 방영환
        /// 부서별통계 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageDList(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                DataTableReader dtr = attach.GetPageDList(listInfo);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();

                            //info.SearchGubun = dtr["SearchGubun"].ToString();
                            info.SearchGubun = this.GetLangName(
                             dtr["DeptBase"].ToString(),
                             dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                             dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(), PreLang);

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>().ToString();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>().ToString();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>().ToString();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>().ToString();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>().ToString();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>().ToString();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>().ToString();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>().ToString();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>().ToString();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>().ToString();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>().ToString();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>().ToString();

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
        /// 작성자 : 방영환
        /// 부서별통계 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetDListCount(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();


                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0; //Int32.Parse(recordPerPage);
                listInfo.PageSizePageNo = 0;// Int32.Parse(currentPage);

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                icnt = attach.GetPageDCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 방영환
        /// 부서별통계 리스트 소계
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageDListTotal(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0;
                listInfo.PageSizePageNo = 0;

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = "";

                DataTableReader dtr = attach.GetTotalPageDList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>().ToString("#,##0");

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

        #endregion

        #region 파일별 통계

        /// <summary>
        /// 작성자 : 방영환
        /// 파일별통계 리스트 가져오기
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                DataTableReader dtr = attach.GetPageFList(listInfo);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();

                            info.SearchGubun = dtr["SearchGubun"].ToString();

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>().ToString();
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>().ToString();
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>().ToString();
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>().ToString();
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>().ToString();
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>().ToString();
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>().ToString();
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>().ToString();
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>().ToString();
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>().ToString();
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>().ToString();
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>().ToString();

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
        /// 작성자 : 방영환
        /// 파일별통계 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetFListCount(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();


                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0; //Int32.Parse(recordPerPage);
                listInfo.PageSizePageNo = 0;// Int32.Parse(currentPage);

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                icnt = attach.GetPageFCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 방영환
        /// 파일별통계 리스트 소계
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageFListTotal(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0;
                listInfo.PageSizePageNo = 0;

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = "";

                DataTableReader dtr = attach.GetTotalPageFList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();

                            info.CntMonth1 = dtr["CntMonth1"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth2 = dtr["CntMonth2"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth3 = dtr["CntMonth3"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth4 = dtr["CntMonth4"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth5 = dtr["CntMonth5"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth6 = dtr["CntMonth6"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth7 = dtr["CntMonth7"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth8 = dtr["CntMonth8"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth9 = dtr["CntMonth9"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth10 = dtr["CntMonth10"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth11 = dtr["CntMonth11"].ToString().ToNumber<int>().ToString("#,##0");
                            info.CntMonth12 = dtr["CntMonth12"].ToString().ToNumber<int>().ToString("#,##0");

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
        
        #endregion


        #region 모듈별 사용량 통계

        /// <summary>
        /// 작성자 : 방영환
        /// 모듈별사용량 통계 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageMFLList(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                DataTableReader dtr = attach.GetPageMFLList(listInfo);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();


                            info.SearchGubun = GetTagInfo(dtr["SearchGubun"].ToString());
                            info.CntMonth1 = GetSize(dtr["CntMonth1"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth2 = GetSize(dtr["CntMonth2"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth3 = GetSize(dtr["CntMonth3"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth4 = GetSize(dtr["CntMonth4"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth5 = GetSize(dtr["CntMonth5"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth6 = GetSize(dtr["CntMonth6"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth7 = GetSize(dtr["CntMonth7"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth8 = GetSize(dtr["CntMonth8"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth9 = GetSize(dtr["CntMonth9"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth10 = GetSize(dtr["CntMonth10"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth11 = GetSize(dtr["CntMonth11"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth12 = GetSize(dtr["CntMonth12"].ToString(), 0).ToNumber<int>().ToString("#,##0");

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
        /// 작성자 : 방영환
        /// 모듈별사용량 통계 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetMFLListCount(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();


                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0; //Int32.Parse(recordPerPage);
                listInfo.PageSizePageNo = 0;// Int32.Parse(currentPage);

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = order;

                icnt = attach.GetPageMFLCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : 방영환
        /// 모듈별사용량 통계 리스트 소계
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageMFLListTotal(string p)
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

                BE.AttachInfo listInfo = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();
                DAL.Attach attach = new Zeospace.DeskPlus.Statistics.DAL.Attach();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.TargetYear = TargetYear;
                listInfo.PageSize = 0;
                listInfo.PageSizePageNo = 0;

                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = "";  // 추후 기간 검색이 필요 할시 
                listInfo.SearchEndDate = ""; // 추후 기간 검색이 필요 할시 
                listInfo.Order = "";

                DataTableReader dtr = attach.GetTotalPageMFLList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new Zeospace.DeskPlus.Statistics.BE.AttachInfo();

                            info.CntMonth1 = GetSize(dtr["CntMonth1"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth2 = GetSize(dtr["CntMonth2"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth3 = GetSize(dtr["CntMonth3"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth4 = GetSize(dtr["CntMonth4"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth5 = GetSize(dtr["CntMonth5"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth6 = GetSize(dtr["CntMonth6"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth7 = GetSize(dtr["CntMonth7"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth8 = GetSize(dtr["CntMonth8"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth9 = GetSize(dtr["CntMonth9"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth10 = GetSize(dtr["CntMonth10"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth11 = GetSize(dtr["CntMonth11"].ToString(), 0).ToNumber<int>().ToString("#,##0");
                            info.CntMonth12 = GetSize(dtr["CntMonth12"].ToString(), 0).ToNumber<int>().ToString("#,##0");

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
        /// byte를 megabyte로 변경
        /// </summary>
        /// <param name="pSize"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        public string GetSize(string pSize, int pi)
        {
            double dSize = double.Parse(pSize);
            string returnStr = string.Empty;

            dSize /= 1024.0;
            dSize /= 1024.0;

            if (pi == 1)
                returnStr = string.Format("{0:F1}", dSize);
            else if (pi == 2)
                returnStr = string.Format("{0:F2}", dSize);
            else if (pi == 3)
                returnStr = string.Format("{0:F3}", dSize);
            else
                returnStr = string.Format("{0}", Convert.ToInt32(dSize));

            return returnStr;
        }

        #endregion

    }
}
