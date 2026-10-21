using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Account.Web.Ws.MyInfo
{
    /// <summary>
    /// LoginInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class LoginInfo : Base.BaseWSAccount
    {
        #region GET()
        /// <summary>
        /// 페이지 가져오기
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
                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("userID", "0");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");

                string rtnSetPaging = SetPaging(Convert.ToInt32(totalCount), Convert.ToInt32(pageSize), Convert.ToInt32(pageSizePageNo));
                string[] setPageingStr = rtnSetPaging.Split(',');

                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (string.Compare(searchTerm, "6M", true) == 0 || string.Compare(searchTerm, "1Y", true) == 0 || string.Compare(searchTerm, "2Y", true) == 0 || string.Compare(searchTerm, "3Y", true) == 0)
                {
                    if (string.Compare(searchTerm, "6M", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("6M", PreLangReal);
                    else if (string.Compare(searchTerm, "1Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("1Y", PreLangReal);
                    else if (string.Compare(searchTerm, "2Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("2Y", PreLangReal);
                    else if (string.Compare(searchTerm, "3Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("3Y", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (string.Compare(searchTerm, "PE", true) == 0)
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion 날짜

                DAL.User dal = new DAL.User();
                DataTableReader dtr = dal.GetLoginInfoList(Convert.ToInt32(companyID), Convert.ToInt32(userID), startDate, endDate, Convert.ToInt32(setPageingStr[0]), Convert.ToInt32(setPageingStr[1]));

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Info info = new Info();
                            info.Row = dtr["Row"].ToString();
                            info.StartDate = Convert.ToDateTime(dtr["StartTime"]).ToString("G");
                            info.Name = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            info.LoginType = dtr["gubun"].ToString();
                            if (info.LoginType.ToString().ToUpper() == "M")
                            {
                                info.LoginType = this.GetLangName(
                                    "모바일버전", "모바일버전",
                                    "Mobile Version", "Mobile Version",
                                    "Mobile Version", "Mobile Version",
                                    "Mobile Version", PreLang);
                            }
                            else
                            {
                                info.LoginType = this.GetLangName(
                                    "PC버전", "PC버전",
                                    "PC Version", "PC Version",
                                    "PC Version", "PC Version",
                                    "PC Version", PreLang);
                            }
                            info.LoginIP = dtr["IPAddress"].ToString();

                            if (info.LoginIP.Trim().Length == 12)
                            {
                                string IP1 = string.Empty;
                                string IP2 = string.Empty;
                                string IP3 = string.Empty;
                                string IP4 = string.Empty;

                                IP1 = info.LoginIP.Substring(0, 3).ToNumber<int>().ToString();
                                IP2 = info.LoginIP.Substring(3, 3).ToNumber<int>().ToString();
                                IP3 = info.LoginIP.Substring(6, 3).ToNumber<int>().ToString();
                                IP4 = info.LoginIP.Substring(9, 3).ToNumber<int>().ToString();

                                info.LoginIP = IP1 + "." + IP2 + "." + IP3 + "." + IP4;
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
        /// 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            int cnt = 0;
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string userID = GetDecryptParam("userID", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");


                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (string.Compare(searchTerm, "6M", true) == 0 || string.Compare(searchTerm, "1Y", true) == 0 || string.Compare(searchTerm, "2Y", true) == 0 || string.Compare(searchTerm, "3Y", true) == 0)
                {
                    if (string.Compare(searchTerm, "6M", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("6M", PreLangReal);
                    else if (string.Compare(searchTerm, "1Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("1Y", PreLangReal);
                    else if (string.Compare(searchTerm, "2Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("2Y", PreLangReal);
                    else if (string.Compare(searchTerm, "3Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("3Y", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (string.Compare(searchTerm, "PE", true) == 0)
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion 날짜

                DAL.User dal = new DAL.User();
                cnt = dal.GetLoginInfoCount(Convert.ToInt32(companyID), Convert.ToInt32(userID), startDate, endDate);

                return cnt;
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
        #endregion
    }

    public class Info
    {
        public string Row { get; set; }
        public string StartDate { get; set; }
        public string LoginIP { get; set; }
        public string LoginType { get; set; }
        public string Name { get; set; }

    }
}
