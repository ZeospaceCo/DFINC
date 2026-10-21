using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Interop.ZeoLicEp;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Static.Attach
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSEIP
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
        /// 작성일자 : 
        /// 작성목적 : 첨부파일 다운로드 리스트 카운트 가져오기
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

                #region IP값 세팅

                if (searchField == "IPAddress")
                {
                    string[] remoteAddrStr = searchValue.Split('.');
                    string remoteAddr = searchValue == "" ? "" : UtilHelper.GetIPAddress(remoteAddrStr);
                    searchValue = remoteAddr;
                }
                #endregion

                Zeospace.DeskPlus.Common.DAL.DownAttach biz = new DownAttach();

                icnt = biz.GetPageListCount(CurrentUserInfo.CompanyID, startDate, endDate, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 첨부파일 다운로드 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetStaticAttach(string p)
        {
            try
            {

                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");

                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                string order = GetDecryptParam("order", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                Zeospace.DeskPlus.Common.DAL.DownAttach biz = new DownAttach();

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

                #region IP값 세팅

                if (searchField == "IPAddress")
                {
                    string[] remoteAddrStr = searchValue.Split('.');
                    string remoteAddr = searchValue == "" ? "" : UtilHelper.GetIPAddress(remoteAddrStr);
                    searchValue = remoteAddr;
                }
                #endregion

                DataTableReader dtr = biz.GetAttachPageList(CurrentUserInfo.CompanyID,
                        setPageingStr[0], setPageingStr[1],
                        startDate, endDate,
                        searchField, searchValue, order);


                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Common.BE.DownAttachInfo info = new DownAttachInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            info.Gubun = dtr["Gubun"].ToString();
                            info.IPAddress = dtr["IPAddress"].ToString();
                            info.WriteDateStr = Convert.ToDateTime(dtr["WriteDate"]).ToShortDateString() + " "
                                + Convert.ToDateTime(dtr["WriteDate"]).ToShortTimeString();//등록일;
                            
                            
                            info.FileName = dtr["FileName"].ToString();
                            info.FilePath = dtr["FilePath"].ToString();

                            if (info.IPAddress.Trim().Length == 12)
                            {
                                string IP1 = string.Empty;
                                string IP2 = string.Empty;
                                string IP3 = string.Empty;
                                string IP4 = string.Empty;

                                IP1 = info.IPAddress.Substring(0, 3).ToNumber<int>().ToString();
                                IP2 = info.IPAddress.Substring(3, 3).ToNumber<int>().ToString();
                                IP3 = info.IPAddress.Substring(6, 3).ToNumber<int>().ToString();
                                IP4 = info.IPAddress.Substring(9, 3).ToNumber<int>().ToString();

                                info.IPAddress = IP1 + "." + IP2 + "." + IP3 + "." + IP4;
                            }

                            #region 이름 다국어 설정
                            info.UserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleName = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

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
    }
}
