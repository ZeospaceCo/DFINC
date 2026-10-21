using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Common.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;

namespace Zeospace.DeskPlus.Common.Web.Ws.WebFax
{
    /// <summary>
    /// SendRecvList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SendRecvList : Base.BaseWSCommon
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
        /// 작성자 : 심재운
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.FaxSendRecvInfo> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");//pagesize
                string currentPage = GetDecryptParam("currentPage", "0");//pageno
                string order = GetDecryptParam("order", "");
                string guid = GetDecryptParam("guid", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.FaxManager dal = new DAL.FaxManager();

                DataTableReader dtr = dal.GetSendRecvPageList(CurrentUserInfo.CompanyID,
                    setPageingStr[0], setPageingStr[1], order, guid);

                List<BE.FaxSendRecvInfo> listobj = new List<FaxSendRecvInfo>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.FaxSendRecvInfo info = new FaxSendRecvInfo();

                            info.Row = dtr["ROW"].ToString().ToNumber<int>();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["RequestTime"].ToString(), out dt))
                                info.RequestTime = DateHelpers.GetCultureDate(dt, PreLangReal) + " " + dt.ToString("HH:mm:ss");
                            else
                                info.RequestTime = string.Empty;

                            if (DateTime.TryParse(dtr["EndTime"].ToString(), out dt))
                                info.EndTime = DateHelpers.GetCultureDate(dt, PreLangReal) + " " + dt.ToString("HH:mm:ss");
                            else
                                info.EndTime = string.Empty;

                            info.NationCode = dtr["NationCode"].ToString();
                            info.FaxNum = dtr["FaxNum"].ToString();
                            info.RecvCompany = dtr["RecvCompany"].ToString();
                            info.RecvName = dtr["RecvName"].ToString();

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
        /// 작성자 : 심재운
        /// 총개수 가져오기
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

                string guid = GetDecryptParam("guid", "");

                DAL.FaxManager dal = new DAL.FaxManager();

                int cnt = dal.GetSendRecvPageCount(CurrentUserInfo.CompanyID,guid);

                return cnt;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

    }
}
