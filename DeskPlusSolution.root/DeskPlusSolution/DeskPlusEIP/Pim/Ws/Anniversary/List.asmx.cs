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
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;
using System.Collections;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Anniversary
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSPim
    {
        public List()
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Anniversary.List).Assembly;	//어셈블리 선언
            #endregion
            this.GetResourceManager();
        }

        #region Get

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAnniversaryInfoList(string p)
        {
            List<object> listobj = new List<object>();
            try
            {
                string companyID = string.Empty;
                string sType = string.Empty;
                string sOpenType = string.Empty;
                string sSearchKey = string.Empty;
                string sSearchValue = string.Empty;

                string sStartTime = string.Empty;
                string sEndTime = string.Empty;

                int iPageSize = 0;
                int iPageSizePageNo = 0;
                string sOrder = string.Empty;
                string sCountry = string.Empty;
                int totalCount = 0;

                SetDecryptString(p);
                companyID = GetDecryptParam("companyID", "0");
                sType = GetDecryptParam("type", "");
                sOpenType = GetDecryptParam("opentype", "0");
                sSearchKey = GetDecryptParam("SearchKey", "");
                sSearchValue = GetDecryptParam("SearchValue", "").ToXSS();
                iPageSize = GetDecryptParam("PageSize", "0").ToNumber<int>();
                iPageSizePageNo = GetDecryptParam("PageSizePageNo", "0").ToNumber<int>();
                totalCount = GetDecryptParam("TotalCount", "0").ToNumber<int>();

                sOrder = GetDecryptParam("Order", " ScheduleID DESC ");
                sStartTime = GetDecryptParam("StartTime", "");

                sEndTime = GetDecryptParam("EndTime", "");

                string rtnSetPaging = SetPaging(totalCount, iPageSize, iPageSizePageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                int iPage = 0;
                int iRecordPage = 0;

                if (iPageSizePageNo == 1)
                {
                    iPage = 0;
                    iRecordPage = iPageSize;
                }
                else
                {
                    iRecordPage = (iPageSizePageNo) * iPageSize;
                    iPage = ((iPageSizePageNo - 1) * iPageSize) ;

                }

                BE.ScheduleInfo info = new ScheduleInfo();
                info.CompanyID = companyID.ToNumber<int>();
                info.Type = "'J'";//ConvertComma(sType);
                info.OpenType = ConvertComma(sOpenType);
                info.OpenType = sOpenType;
                info.SearchKey = sSearchKey;
                info.SearchValue = sSearchValue;
                info.stringStartTime = sStartTime;
                info.stringENDTime = sEndTime;

                info.Pagesize = iRecordPage;//setPageingStr[0].ToNumber<int>();
                info.PageSizePageNo = iPage; //setPageingStr[1].ToNumber<int>();
                info.Order = sOrder;
                info.Country = "Ko";
                info.UserID = CurrentUserInfo.UserID;

                DAL.Schedule bll = new DAL.Schedule();
                DataTableReader dtr = bll.GetAnniversaryProc(info);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {

                            BE.DayInfo data = new DayInfo();
                            data.ScheduleID = dtr["ScheduleID"].ToString();
                            data.Title = dtr["Title"].ToString();
                            data.DateInfo = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString();//.ToString("yyyy-MM-dd");
                            data.EndTime = DateTime.Parse(dtr["EndTime"].ToString()).ToShortDateString();//.ToString("yyyy-MM-dd");
                            data.WriteUserID = dtr["UserID"].ToString().ToNumber<int>();
                            
                            switch (dtr["OpenType"].ToString())
                            {
                                case "-1": // 개인일정
                                    data.CategoryDesc = GetResource("MYSCHEDULE");
                                    break;
                                case "0":// 개인기념일
                                    data.CategoryDesc = GetResource("schedule2");
                                    break;
                                case "1"://전체 공유일정
                                    data.CategoryDesc = GetResource("ShareSchedule");
                                    break;
                                case "2"://전체 공유 기념일
                                    data.CategoryDesc = GetResource("AllAnniversary");
                                    break;
                                case "3"://전체 공유 절기
                                    data.CategoryDesc = GetResource("holy5");
                                    break;

                            }
                            if (dtr["lunar"].ToString().Equals("N"))
                            {
                                data.DateInfo += "(" + GetResource("sunday") + ")";
                            }
                            else
                            {
                                data.DateInfo += "(" + GetResource("lunalday") + ")";
                            }

                            data.WriteName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                          dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                          dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang
                          );
                            listobj.Add(data);
                        }
                    }
                }


                return listobj;

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAnniversaryInfoList", exc);
                return listobj;
            }
        }


         
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetAnniversaryInfoListCnt(string p)
        {
            try
            {
                string companyID = string.Empty;
                string sType = string.Empty;
                string sOpenType = string.Empty;
                string sSearchKey = string.Empty;
                string sSearchValue = string.Empty;

                string sStartTime = string.Empty;
                string sEndTime = string.Empty;

                SetDecryptString(p);
                companyID = GetDecryptParam("companyID", "0");
                sType = GetDecryptParam("type", "");
                sOpenType = GetDecryptParam("opentype", "0");
                sSearchKey = GetDecryptParam("SearchKey", "");
                sSearchValue = GetDecryptParam("SearchValue", "");


                sStartTime = GetDecryptParam("StartTime", "");
                sEndTime = GetDecryptParam("EndTime", "");

                BE.ScheduleInfo info = new ScheduleInfo();
                info.CompanyID = companyID.ToNumber<int>();
                info.Type = ConvertComma(sType);
                info.OpenType = sOpenType;
                info.SearchKey = sSearchKey;
                info.SearchValue = sSearchValue;
                info.stringStartTime = sStartTime;
                info.stringENDTime = sEndTime;
                info.UserID = CurrentUserInfo.UserID;

                DAL.Schedule bll = new DAL.Schedule();

                return bll.GetAnniversaryProcCnt(info);

            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAnniversaryInfoListCnt", exc);
                return 0;
            }

        }
        #endregion

        #region Set
        #endregion

        #region utill
        private string ConvertComma(string sValue)
        {
            string rtMsg = string.Empty;

            string[] arV = sValue.Split(',');


            for (int i = 0; i < arV.Length; i++)
            {
                rtMsg += "'" + arV[i] + "'";
                if (i != (arV.Length - 1))
                {
                    rtMsg += ",";
                }
            }


            return rtMsg;
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
}
