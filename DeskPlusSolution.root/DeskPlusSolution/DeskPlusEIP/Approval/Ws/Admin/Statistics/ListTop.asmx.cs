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
    /// ListTop의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListTop : Base.BaseWSApproval
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
        /// 자리수처리 (소숫점 2자리이하, 버림)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public string FormatPercent(string s, int len)
        {
            string[] sa = s.Split('.');
            if (sa.Length.Equals(2))
            {
                if (sa[1].Length > len)
                {
                    return sa[0] + "." + sa[1].Substring(0, len);
                }
                else
                {
                    return s;
                }
            }
            else
            {
                return s;
            }
        }

        #region 조건별현황 (양식별)
        /// <summary>
        /// 조건별현황 (양식별)
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Admin.Statistics.ListTop).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            int totalItemCount = GetDecryptParam("totalItemCount", "0").ToNumber<int>();
            int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            int pageSizePageNo = GetDecryptParam("pageSizePageNo", "0").ToNumber<int>();

            int years = GetDecryptParam("years", "0").ToNumber<int>();
            string sDate = GetDecryptParam("sDate", "");
            string eDate = GetDecryptParam("eDate", "");
            if (sDate.Equals("") || eDate.Equals(""))
            {
                sDate = years.ToString() + "-01-01";         // 2012-01-01
                eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            }
            string rtnSetPaging = SetPaging(totalItemCount, pageSize, pageSizePageNo);
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.Statistics dal = new DAL.Statistics();
            DataTableReader dtr = dal.GetTopFormList(CurrentUserInfo.CompanyID, setPageingStr[0].ToNumber<int>(), setPageingStr[1].ToNumber<int>(), sDate, eDate);

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i=1;
                    while (dtr.Read())
                    {
                        BE.StatisticsInfo oInfo = new BE.StatisticsInfo();

                        oInfo.UserID = 0;
                        oInfo.TotCnt = dtr["TotCnt"].ToString().ToNumber<int>();
                        oInfo.Cnt = dtr["Cnt"].ToString().ToNumber<int>();
                        oInfo.NameBase = (dtr["NameBase"].ToString().Equals("")) ? dtr["NameBaseOrg"].ToString() : GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                        oInfo.Percentage = FormatPercent(dtr["Per"].ToString(), 2);
                        oInfo.ClassInfo = (i % 2).Equals(0) ? "0" : "1";
                        i++;

                        uobj.Add(oInfo);
                        
                    }
                }
            }
            return uobj;
        }

        /// <summary>
        /// 조건별현황 (양식별)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetFormListCnt(string p)
        {
            if (IsAdmin == false) return 0;

            SetDecryptString(p);

            int years = GetDecryptParam("years", "0").ToNumber<int>();
            string sDate = GetDecryptParam("sDate", "");
            string eDate = GetDecryptParam("eDate", "");
            if (sDate.Equals("") || eDate.Equals(""))
            {
                sDate = years.ToString() + "-01-01";         // 2012-01-01
                eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            }

            DAL.Statistics dal = new DAL.Statistics();
            return dal.GetTopFormListCnt(CurrentUserInfo.CompanyID, sDate, eDate);
        }
        #endregion

        #region 조건별현황 (부서별)
        /// <summary>
        /// 조건별현황 (부서별)
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Admin.Statistics.ListTop).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            int totalItemCount = GetDecryptParam("totalItemCount", "0").ToNumber<int>();
            int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            int pageSizePageNo = GetDecryptParam("pageSizePageNo", "0").ToNumber<int>();

            int years = GetDecryptParam("years", "0").ToNumber<int>();
            string sDate = GetDecryptParam("sDate", "");
            string eDate = GetDecryptParam("eDate", "");
            if (sDate.Equals("") || eDate.Equals(""))
            {
                sDate = years.ToString() + "-01-01";         // 2012-01-01
                eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            }
            string rtnSetPaging = SetPaging(totalItemCount, pageSize, pageSizePageNo);
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.Statistics dal = new DAL.Statistics();
            DataTableReader dtr = dal.GetTopDeptList(CurrentUserInfo.CompanyID, setPageingStr[0].ToNumber<int>(), setPageingStr[1].ToNumber<int>(), sDate, eDate);

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        BE.StatisticsInfo oInfo = new BE.StatisticsInfo();

                        oInfo.UserID = 0;
                        oInfo.TotCnt = dtr["TotCnt"].ToString().ToNumber<int>();
                        oInfo.Cnt = dtr["Cnt"].ToString().ToNumber<int>();
                        oInfo.NameBase = (dtr["NameBase"].ToString().Equals("")) ? dtr["NameBaseOrg"].ToString() : GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                        oInfo.Percentage = FormatPercent(dtr["Per"].ToString(), 2);
                        oInfo.ClassInfo = (i % 2).Equals(0) ? "0" : "1";
                        i++;

                        uobj.Add(oInfo);
                    }
                }
            }
            return uobj;
        }

        /// <summary>
        /// 조건별현황 (부서별)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetDeptListCnt(string p)
        {
            if (IsAdmin == false) return 0;

            SetDecryptString(p);

            int years = GetDecryptParam("years", "0").ToNumber<int>();
            string sDate = GetDecryptParam("sDate", "");
            string eDate = GetDecryptParam("eDate", "");
            if (sDate.Equals("") || eDate.Equals(""))
            {
                sDate = years.ToString() + "-01-01";         // 2012-01-01
                eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            }

            DAL.Statistics dal = new DAL.Statistics();
            return dal.GetTopDeptListCnt(CurrentUserInfo.CompanyID, sDate, eDate);
        }
        #endregion

        #region 조건별현황 (사용자별)
        /// <summary>
        /// 조건별현황 (사용자별)
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Admin.Statistics.ListTop).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            int totalItemCount = GetDecryptParam("totalItemCount", "0").ToNumber<int>();
            int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            int pageSizePageNo = GetDecryptParam("pageSizePageNo", "0").ToNumber<int>();

            int years = GetDecryptParam("years", "0").ToNumber<int>();
            string sDate = GetDecryptParam("sDate", "");
            string eDate = GetDecryptParam("eDate", "");
            if (sDate.Equals("") || eDate.Equals(""))
            {
                sDate = years.ToString() + "-01-01";         // 2012-01-01
                eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            }
            string rtnSetPaging = SetPaging(totalItemCount, pageSize, pageSizePageNo);
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.Statistics dal = new DAL.Statistics();
            DataTableReader dtr = dal.GetTopUserList(CurrentUserInfo.CompanyID, setPageingStr[0].ToNumber<int>(), setPageingStr[1].ToNumber<int>(), sDate, eDate);

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    int i = 1;
                    while (dtr.Read())
                    {
                        BE.StatisticsInfo oInfo = new BE.StatisticsInfo();

                        oInfo.UserID = dtr["KeyID"].ToString().ToNumber<int>();
                        oInfo.TotCnt = dtr["TotCnt"].ToString().ToNumber<int>();
                        oInfo.Cnt = dtr["Cnt"].ToString().ToNumber<int>();
                        oInfo.NameBase = (dtr["NameBase"].ToString().Equals("")) ? dtr["NameBaseOrg"].ToString() : GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                        oInfo.Percentage = FormatPercent(dtr["Per"].ToString(), 2);
                        oInfo.ClassInfo = (i % 2).Equals(0) ? "0" : "1";
                        i++;

                        uobj.Add(oInfo);
                    }
                }
            }
            return uobj;
        }

        /// <summary>
        /// 조건별현황 (사용자별)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetUserListCnt(string p)
        {
            if (IsAdmin == false) return 0;

            SetDecryptString(p);

            int years = GetDecryptParam("years", "0").ToNumber<int>();
            string sDate = GetDecryptParam("sDate", "");
            string eDate = GetDecryptParam("eDate", "");
            if (sDate.Equals("") || eDate.Equals(""))
            {
                sDate = years.ToString() + "-01-01";         // 2012-01-01
                eDate = (years + 1).ToString() + "-01-01";   // 2013-01-01
            }

            DAL.Statistics dal = new DAL.Statistics();
            return dal.GetTopUserListCnt(CurrentUserInfo.CompanyID, sDate, eDate);
        }
        #endregion

        #endregion
    }
}
