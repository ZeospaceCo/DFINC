using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Portlet.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Globalization;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// ToSchList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ToSchList : Base.BaseWSPortlet
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
        /// 작성자   : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 오늘의 일정 리스트 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Package.ToSchListPageList> GetToSchList(string p)
        {
            try
            {
                SetDecryptString(p);

                int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
                int pageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
                int totalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

                int companyID = CurrentUserInfo.CompanyID;
                int userID = CurrentUserInfo.UserID;
                DateTime start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00"));


                string rtnSetPaging = SetPaging(totalCount, pageSize, pageSizePageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.ToSchList dal = new DAL.ToSchList();
                List<Ws.Package.ToSchListPageList> alList = new List<ToSchListPageList>();

                DataTableReader dtr = dal.GetToSchList(companyID, setPageingStr[0], setPageingStr[1], userID, start);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.Package.ToSchListPageList info = new ToSchListPageList();
                            // 제목
                            info.Title = dtr["Title"].ToString();
                            // 게시범위
                            info.OpenType = dtr["OpenType"].ToString().ToNumber<int>();

                            DateTime dt;

                            if (DateTime.TryParse(dtr["StartTime"].ToString(), out dt))
                                info.StartTimeStr = dt.GetDateTimeFormats()[99];
                            else
                                info.StartTimeStr = string.Empty;

                            info.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();

                            info.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();

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
    }

    [Serializable]
    public class ToSchListPageList
    {
        public ToSchListPageList() { }

        public int CompanyID { get; set; }
        public int OpenType { get; set; }
        public string Title { get; set; }
        public string StartTimeStr { get; set; }
        public int ScheduleID { get; set; }
        public int DisplayID { get; set; }
    }
}
