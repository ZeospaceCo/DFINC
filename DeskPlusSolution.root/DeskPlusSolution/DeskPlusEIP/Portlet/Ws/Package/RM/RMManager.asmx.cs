using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Portlet.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.RM
{
    /// <summary>
    /// RMManager의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class RMManager : Base.BaseWSPortlet
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
        /// 작성자   : famo23
        /// 작성일자 : 2017-03-03
        /// 작성목적 :  승인대기 리스트 (자원담당자용)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRmMaList(string p)
        {
            try
            {
                SetDecryptString(p);

                int pageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
                int pageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
                int totalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalCount, pageSize, pageSizePageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                List<object> list = new List<object>();

                DAL.RM dal = new DAL.RM();
                DataTableReader dtr = dal.GetRMList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1]);

                int row = 0;
                string tmpListID = "";
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ReserveInfo info = new ReserveInfo();

                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                            info.EndDate = Convert.ToDateTime(dtr["EndDate"].ToString());
                            info.StartTime = info.StartDate.Month.ToString().PadLeft(2, '0') + "." + info.StartDate.Day.ToString().PadLeft(2, '0') + " " + info.StartDate.Hour.ToString().PadLeft(2, '0') + ":" + info.StartDate.Minute.ToString().PadLeft(2, '0');
                            info.EndTime = info.EndDate.Month.ToString().PadLeft(2, '0') + "." + info.EndDate.Day.ToString().PadLeft(2, '0') + " " + info.EndDate.Hour.ToString().PadLeft(2, '0') + ":" + info.EndDate.Minute.ToString().PadLeft(2, '0');
                            info.Subject = dtr["Subject"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.WriteUserName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                                            dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                            dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                            if (tmpListID != dtr["ListID"].ToString())
                            {
                                info.Row = 1;
                                row = 1;
                                tmpListID = dtr["ListID"].ToString();
                            }
                            else
                            {
                                row += row + 1;
                                info.Row = row;
                            }

                            list.Add(info);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }
    }
}

