using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.Statistics
{
    /// <summary>
    /// ListMonth의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListMonth : Base.BaseWSApproval
    {
        #region GET

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
        /// 공통-월별처리
        /// </summary>
        /// <param name="_statelist"></param>
        /// <returns></returns>
        private List<BE.MonthInfo> SetMonthInfo(List<BE.StateInfo> _statelist)
        {
            if (_statelist == null) return null;

            List<BE.MonthInfo> arrlist = new List<BE.MonthInfo>();
            BE.MonthInfo cInfo = new BE.MonthInfo();
            BE.MonthInfo wInfo = new BE.MonthInfo();
            BE.MonthInfo rInfo = new BE.MonthInfo();
            BE.MonthInfo dInfo = new BE.MonthInfo();
            BE.MonthInfo sumInfo = new BE.MonthInfo();

            foreach (BE.StateInfo info in _statelist)
            {
                #region 월 체크

                /* 
                   기안(진행) [0] - W,I,P
                   승인(완료) [1] - C
                   반려 [2] - R
                   보류 [3] - D,H 
                 */


                if (info.Month.Equals(1))
                {
                    cInfo.January = info.C;
                    wInfo.January = info.W + info.I + info.P;
                    rInfo.January = info.R;
                    dInfo.January = info.D + info.H;
                }
                if (info.Month.Equals(2))
                {
                    cInfo.February = info.C;
                    wInfo.February = info.W + info.I + info.P;
                    rInfo.February = info.R;
                    dInfo.February = info.D + info.H;
                }
                if (info.Month.Equals(3))
                {
                    cInfo.March = info.C;
                    wInfo.March = info.W + info.I + info.P;
                    rInfo.March = info.R;
                    dInfo.March = info.D + info.H;
                }
                if (info.Month.Equals(4))
                {
                    cInfo.April = info.C;
                    wInfo.April = info.W + info.I + info.P;
                    rInfo.April = info.R;
                    dInfo.April = info.D + info.H;
                }
                if (info.Month.Equals(5))
                {
                    cInfo.May = info.C;
                    wInfo.May = info.W + info.I + info.P;
                    rInfo.May = info.R;
                    dInfo.May = info.D + info.H;
                }
                if (info.Month.Equals(6))
                {
                    cInfo.June = info.C;
                    wInfo.June = info.W + info.I + info.P;
                    rInfo.June = info.R;
                    dInfo.June = info.D + info.H;
                }
                if (info.Month.Equals(7))
                {
                    cInfo.July = info.C;
                    wInfo.July = info.W + info.I + info.P;
                    rInfo.July = info.R;
                    dInfo.July = info.D + info.H;
                }
                if (info.Month.Equals(8))
                {
                    cInfo.Auguest = info.C;
                    wInfo.Auguest = info.W + info.I + info.P;
                    rInfo.Auguest = info.R;
                    dInfo.Auguest = info.D + info.H;
                }
                if (info.Month.Equals(9))
                {
                    cInfo.September = info.C;
                    wInfo.September = info.W + info.I + info.P;
                    rInfo.September = info.R;
                    dInfo.September = info.D + info.H;
                }
                if (info.Month.Equals(10))
                {
                    cInfo.October = info.C;
                    wInfo.October = info.W + info.I + info.P;
                    rInfo.October = info.R;
                    dInfo.October = info.D + info.H;
                }
                if (info.Month.Equals(11))
                {
                    cInfo.November = info.C;
                    wInfo.November = info.W + info.I + info.P;
                    rInfo.November = info.R;
                    dInfo.November = info.D + info.H;
                }
                if (info.Month.Equals(12))
                {
                    cInfo.December = info.C;
                    wInfo.December = info.W + info.I + info.P;
                    rInfo.December = info.R;
                    dInfo.December = info.D + info.H;
                }
                #endregion
            }

            sumInfo.January = cInfo.January + wInfo.January + rInfo.January + dInfo.January;
            sumInfo.February = cInfo.February + wInfo.February + rInfo.February + dInfo.February;
            sumInfo.March = cInfo.March + wInfo.March + rInfo.March + dInfo.March;
            sumInfo.April = cInfo.April + wInfo.April + rInfo.April + dInfo.April;
            sumInfo.May = cInfo.May + wInfo.May + rInfo.May + dInfo.May;
            sumInfo.June = cInfo.June + wInfo.June + rInfo.June + dInfo.June;
            sumInfo.July = cInfo.July + wInfo.July + rInfo.July + dInfo.July;
            sumInfo.Auguest = cInfo.Auguest + wInfo.Auguest + rInfo.Auguest + dInfo.Auguest;
            sumInfo.September = cInfo.September + wInfo.September + rInfo.September + dInfo.September;
            sumInfo.October = cInfo.October + wInfo.October + rInfo.October + dInfo.October;
            sumInfo.November = cInfo.November + wInfo.November + rInfo.November + dInfo.November;
            sumInfo.December = cInfo.December + wInfo.December + rInfo.December + dInfo.December;

            arrlist.Add(cInfo);
            arrlist.Add(wInfo);
            arrlist.Add(rInfo);
            arrlist.Add(dInfo);
            arrlist.Add(sumInfo);

            return arrlist;
        }

        /// <summary>
        /// 공통-월별현황리스트
        /// </summary>
        /// <param name="_dtMain"></param>
        /// <param name="_dtSub"></param>
        /// <returns></returns>
        private List<object> GetItems(DataTable _dtMain, DataTable _dtSub, bool _isUser)
        {
            if (_dtMain == null || _dtSub == null) return null;
            //if (_dtMain.Rows.Count.Equals(0) || _dtSub.Rows.Count.Equals(0)) return null;

            // 서브리스트 처리
            List<BE.StateInfo> listStat = new List<BE.StateInfo>();
            foreach (DataRow dr in _dtSub.Rows)
            {
                BE.StateInfo info = new BE.StateInfo();
                info.KeyID = dr["KeyID"].ToString().ToNumber<int>();
                info.Years = dr["Years"].ToString().ToNumber<int>();
                info.Month = dr["Months"].ToString().ToNumber<int>();
                info.C = dr["C"].ToString().ToNumber<int>();
                info.W = dr["W"].ToString().ToNumber<int>();
                info.I = dr["I"].ToString().ToNumber<int>();
                info.R = dr["R"].ToString().ToNumber<int>();
                info.H = dr["H"].ToString().ToNumber<int>();
                info.D = dr["D"].ToString().ToNumber<int>();
                info.P = dr["P"].ToString().ToNumber<int>();

                listStat.Add(info);
            }

            List<object> uobj = new List<object>();
            foreach (DataRow dr in _dtMain.Rows)
            {
                BE.StatisticsInfo oInfo = new BE.StatisticsInfo();

                oInfo.Row = dr["RowID"].ToString();
                oInfo.KeyID = dr["KeyID"].ToString().ToNumber<int>();
                oInfo.Cnt = dr["Cnt"].ToString().ToNumber<int>();
                oInfo.NameBase = (dr["NameBase"].ToString().Equals("")) ? dr["NameBaseOrg"].ToString() : GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                oInfo.UserID = (_isUser) ? oInfo.KeyID : 0;

                List<BE.StateInfo> subInfo = listStat.Where(x => x.KeyID.Equals(oInfo.KeyID)).ToList<BE.StateInfo>();
                oInfo.ArrMonthInfo = SetMonthInfo(subInfo);    //상태&월별처리

                uobj.Add(oInfo);
            }
            return uobj;
        }

        #region 월별현황-양식별
        /// <summary>
        /// 월별현황-양식별
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFormList(string p)
        {
            if (IsAdmin == false) return null;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Admin.Statistics.ListMonth).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            int totalItemCount = GetDecryptParam("totalItemCount", "0").ToNumber<int>();
            int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            int pageSizePageNo = GetDecryptParam("pageSizePageNo", "0").ToNumber<int>();
            int years = GetDecryptParam("years", DateTime.Today.Year.ToString()).ToNumber<int>();
            if (years < 1901) return null;
            
            string sDate = years.ToString() + "-01-01";         // 2012-01-01
            string eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            string srhField = GetDecryptParam("searchField", "");
            string srhValue = GetDecryptParam("searchValue", "");

            string rtnSetPaging = SetPaging(totalItemCount, pageSize, pageSizePageNo);
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.Statistics dal = new DAL.Statistics();
            DataSet dsList = dal.GetMonthFormList(CurrentUserInfo.CompanyID, setPageingStr[0].ToNumber<int>(), setPageingStr[1].ToNumber<int>(), sDate, eDate, srhField, srhValue);
            DataTable dtMain = dsList.Tables[0];    // 양식별
            DataTable dtSub = dsList.Tables[1];     // 년월건수

            List<object> uobj = this.GetItems(dtMain, dtSub, false);
            return uobj;
        }

        /// <summary>
        /// 월별현황-양식별
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetFormListCnt(string p)
        {
            if (IsAdmin == false) return 0;

            SetDecryptString(p);

            int years = GetDecryptParam("years", DateTime.Today.Year.ToString()).ToNumber<int>();
            if (years < 1901) return 0;

            string sDate = years.ToString() + "-01-01";         // 2012-01-01
            string eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            string srhField = GetDecryptParam("searchField", "");
            string srhValue = GetDecryptParam("searchValue", "");

            DAL.Statistics dal = new DAL.Statistics();
            return dal.GetMonthFormListCnt(CurrentUserInfo.CompanyID, sDate, eDate, srhField, srhValue);
        }
        #endregion

        #region 월별현황-부서별
        /// <summary>
        /// 월별현황-부서별
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetDeptList(string p)
        {
            if (IsAdmin == false) return null;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Admin.Statistics.ListMonth).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            int totalItemCount = GetDecryptParam("totalItemCount", "0").ToNumber<int>();
            int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            int pageSizePageNo = GetDecryptParam("pageSizePageNo", "0").ToNumber<int>();
            int years = GetDecryptParam("years", DateTime.Today.Year.ToString()).ToNumber<int>();
            if (years < 1901) return null;
            
            string sDate = years.ToString() + "-01-01";         // 2012-01-01
            string eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            string srhField = GetDecryptParam("searchField", "");
            string srhValue = GetDecryptParam("searchValue", "");

            string rtnSetPaging = SetPaging(totalItemCount, pageSize, pageSizePageNo);
            string[] setPageingStr = rtnSetPaging.Split(',');
            
            DAL.Statistics dal = new DAL.Statistics();
            DataSet dsList = dal.GetMonthDeptList(CurrentUserInfo.CompanyID, setPageingStr[0].ToNumber<int>(), setPageingStr[1].ToNumber<int>(), sDate, eDate, srhField, srhValue);
            DataTable dtMain = dsList.Tables[0];    // 양식별
            DataTable dtSub = dsList.Tables[1];     // 년월건수

            List<object> uobj = this.GetItems(dtMain, dtSub, false);
            return uobj;
            
        }

        /// <summary>
        /// 월별현황-부서별
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetDeptListCnt(string p)
        {
            if (IsAdmin == false) return 0;

            SetDecryptString(p);

            int years = GetDecryptParam("years", DateTime.Today.Year.ToString()).ToNumber<int>();
            if (years < 1901) return 0;

            string sDate = years.ToString() + "-01-01";         // 2012-01-01
            string eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            string srhField = GetDecryptParam("searchField", "");
            string srhValue = GetDecryptParam("searchValue", "");

            DAL.Statistics dal = new DAL.Statistics();
            return dal.GetMonthDeptListCnt(CurrentUserInfo.CompanyID, sDate, eDate, srhField, srhValue);
        }
        #endregion

        #region 월별현황-개인별 (작성자)
        /// <summary>
        /// 월별현황-개인별 (작성자)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetUserList(string p)
        {
            if (IsAdmin == false) return null;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Admin.Statistics.ListMonth).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            int totalItemCount = GetDecryptParam("totalItemCount", "0").ToNumber<int>();
            int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            int pageSizePageNo = GetDecryptParam("pageSizePageNo", "0").ToNumber<int>();
            int years = GetDecryptParam("years", DateTime.Today.Year.ToString()).ToNumber<int>();
            if (years < 1901) return null;

            string sDate = years.ToString() + "-01-01";         // 2012-01-01
            string eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            string srhField = GetDecryptParam("searchField", "");
            string srhValue = GetDecryptParam("searchValue", "");

            string rtnSetPaging = SetPaging(totalItemCount, pageSize, pageSizePageNo);
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.Statistics dal = new DAL.Statistics();
            DataSet dsList = dal.GetMonthUserList(CurrentUserInfo.CompanyID, setPageingStr[0].ToNumber<int>(), setPageingStr[1].ToNumber<int>(), sDate, eDate, srhField, srhValue);
            DataTable dtMain = dsList.Tables[0];    // 양식별
            DataTable dtSub = dsList.Tables[1];     // 년월건수

            List<object> uobj = this.GetItems(dtMain, dtSub, true);
            return uobj;
        }

        /// <summary>
        /// 월별현황-개인별 (작성자)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetUserListCnt(string p)
        {
            if (IsAdmin == false) return 0;

            SetDecryptString(p);

            int years = GetDecryptParam("years", DateTime.Today.Year.ToString()).ToNumber<int>();
            if (years < 1901) return 0;

            string sDate = years.ToString() + "-01-01";         // 2012-01-01
            string eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            string srhField = GetDecryptParam("searchField", "");
            string srhValue = GetDecryptParam("searchValue", "");

            DAL.Statistics dal = new DAL.Statistics();
            return dal.GetMonthUserListCnt(CurrentUserInfo.CompanyID, sDate, eDate, srhField, srhValue);
        }
        #endregion

        #endregion
    }
}
