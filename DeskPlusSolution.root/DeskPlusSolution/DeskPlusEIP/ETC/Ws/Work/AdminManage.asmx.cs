using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.ETC.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.ETC.Web.Ws.Work
{
    /// <summary>
    /// AdminManage의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AdminManage : Base.BaseWSETC
    {
        #region 리스트
        /// <summary>
        /// 작성자 : ellepooky
        /// 용도 : 야근/특근 지급 승인
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]

        public bool ModWorkPay(string p)
        {
            SetDecryptString(p);
            string comList = GetDecryptParam("reportList", "");

            Ws.Work.PageList info = new PageList();

            if (comList.Length > 0)
            {
                DAL.WorkReport dal = new DAL.WorkReport();
                foreach (string reportID in comList.Split(','))
                {
                    dal.ModWorkReportPay(reportID.ToNumber<int>());
                }
            }

            return true;
        }

                /// <summary>
        /// 작성자 : ellepooky
        /// 용도 : 야근/특근 지급 취소
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]

        public bool ModWorkPayCancel(string p)
        {
            SetDecryptString(p);
            string comList = GetDecryptParam("reportList", "");

            Ws.Work.PageList info = new PageList();

            if (comList.Length > 0)
            {
                DAL.WorkReport dal = new DAL.WorkReport();
                foreach (string reportID in comList.Split(','))
                {
                    dal.ModWorkReportPayCancel(reportID.ToNumber<int>());
                }
            }

            return true;
        }

        /// <summary>
        /// 결재건수
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetUserCount(string p)
        {
            try
            {
                SetDecryptString(p);

                int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
                int pageSizePageNo = GetDecryptParam("pageSizePageNo", "1").ToNumber<int>();
                int totalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

                string activeUser = GetDecryptParam("activeUser", "1");
                string deptName = GetDecryptParam("deptName", "");
                string titleName = GetDecryptParam("titleName", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string gubun = GetDecryptParam("gubun", "0");

                string rtnSetPaging = SetPaging(totalCount, pageSize, pageSizePageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                string startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                string endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);

                // 데이타처리 - 건수
                DAL.WorkReport dal = new DAL.WorkReport();
                DataTableReader dtr = dal.GetAmountSum(CurrentUserInfo.CompanyID, setPageingStr[0], setPageingStr[1],
                    activeUser, startDate, endDate, deptName, titleName, searchField, searchValue, gubun.ToNumber<int>());

                ObjList obj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            CountInfo info = new CountInfo();

                            if (dtr["CAT"].ToString() == "PA")
                            {
                                info.CAT = dtr["CAT"].ToString();
                                info.CntPA = dtr["PaySum"].ToString();
                            }
                            if (dtr["CAT"].ToString() == "AA")
                            {
                                info.CAT = dtr["CAT"].ToString();
                                info.CntAA = dtr["PaySum"].ToString();
                            }

                            obj.Add(info);
                        }
                    }
                }

                return obj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

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
        /// 작성자 : ellepooky
        /// 휴가 내역 관리 검색 페이징 카운트(관리자)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageCount(string p)
        {
            SetDecryptString(p);

            string activeUser = GetDecryptParam("activeUser", "1");
            string deptName = GetDecryptParam("deptName", "");
            string titleName = GetDecryptParam("titleName", "");
            string searchTermValue = GetDecryptParam("searchTermValue", "");
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");
            string gubun = GetDecryptParam("gubun", "0");

            string startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
            string endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);

            DAL.WorkReport biz = new DAL.WorkReport();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, activeUser, startDate, endDate,
                deptName, titleName, searchField, searchValue, gubun.ToNumber<int>());
        }

        /// <summary>
        /// 작성자 : ellepooky
        /// 야근 / 특근 내역 관리 검색 페이징 리스트(관리자)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Work.PageList> GetPageList(string p)
        {
            try
            {

                SetDecryptString(p);

                int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
                int pageSizePageNo = GetDecryptParam("pageSizePageNo", "1").ToNumber<int>();
                int totalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

                string ReportID = GetDecryptParam("reportID", "1");
                string activeUser = GetDecryptParam("activeUser", "1");
                string deptName = GetDecryptParam("deptName", "");
                string titleName = GetDecryptParam("titleName", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string gubun = GetDecryptParam("gubun", "0");
                string order = GetDecryptParam("order", "");

                string rtnSetPaging = SetPaging(totalCount, pageSize, pageSizePageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                string startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                string endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);

                DAL.WorkReport biz = new DAL.WorkReport();
                List<Ws.Work.PageList> alList = new List<PageList>();

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, setPageingStr[0], setPageingStr[1],
                    activeUser, startDate, endDate, deptName, titleName, searchField, searchValue, gubun.ToNumber<int>(), order);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.Work.PageList info = new PageList();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ROW = dtr["ROW"].ToString().ToNumber<int>();

                            info.ReportID = dtr["ReportID"].ToString().ToNumber<int>();

                            info.EmployeeNumber = dtr["EmployeeNumber"].ToString();
                            info.DeptName = GetLangName(
                                        dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                        dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                        dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                        dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.EmployeeNumber = dtr["EmployeeNumber"].ToString();
                            info.Name = GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            info.TitleName = GetLangName(
                                        dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                        dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                        dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                        dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                            #region 시작일
                            DateTime dtS;
                            if (DateTime.TryParse(dtr["StartDate"].ToString(), out dtS))
                                info.StartDate = DateHelpers.GetCultureDate(dtS, PreLangReal).Substring(0, 10);
                            else
                                info.StartDate = string.Empty;
                            #endregion

                            #region 종료일
                            DateTime dtE;
                            if (DateTime.TryParse(dtr["EndDate"].ToString(), out dtE))
                                info.EndDate = DateHelpers.GetCultureDate(dtE, PreLangReal).Substring(0, 10);
                            else
                                info.EndDate = string.Empty;
                            #endregion

                            // 고객명
                            info.CustomerName = dtr["CustomerName"].ToString();
                            // 기간
                            info.Duration = dtr["Duration"].ToString();
                            // 시간
                            info.Hour = dtr["Hour"].ToString().ToNumber<int>();
                            // 구분
                            info.Gubun = dtr["Gubun"].ToString();
                            // 업무내역
                            info.Reason = dtr["Reason"].ToString();
                            // 지급비용
                            info.ExtraPay =  String.Format("{0:#,0}", int.Parse(dtr["ExtraPay"].ToString()));
                            // 지급여부
                            info.IsPay = dtr["IsPay"].ToString();
                            // 교통비여부
                            info.IsTransportPay = dtr["IsTransportPay"].ToString();
                            // 문서번호
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();

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

        #endregion

        #region BE

        /// <summary>
        /// 건수 
        /// </summary>
        [Serializable]
        public class CountInfo
        {
            public CountInfo() { }

            public string CAT { get; set; }
            public string CntPA { get; set; }
            public string CntAA { get; set; }
        }
        #endregion
    }

}
