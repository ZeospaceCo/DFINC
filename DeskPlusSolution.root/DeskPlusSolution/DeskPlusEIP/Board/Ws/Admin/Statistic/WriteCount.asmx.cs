using System;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws.Admin.Statistic
{
    /// <summary>
    /// WriteCount의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class WriteCount : Base.BaseWSBoard
    {
        #region 리스트

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

        #endregion 리스트

        #region 통계

        /// <summary>
        /// 작성자 : 다니엘
        /// 게시물조회
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_myTemplateID">The _list ID.</param>
        /// <param name="_title">The _title.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetWriteCount(string p)
        {
            string CategoryID = string.Empty;
            string StartDate = string.Empty;
            string EndDate = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;
            string order = string.Empty;
            string totalCount = string.Empty;

            SetDecryptString(p);

            CategoryID = GetDecryptParam("CategoryID", "");
            StartDate = GetDecryptParam("StartDate", "");
            EndDate = GetDecryptParam("EndDate", "");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
            order = GetDecryptParam("order", "");
            totalCount = GetDecryptParam("totalCount", "0");

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.ListViewers biz = new DAL.ListViewers();
            DataTableReader dtr = biz.GetListWriteList(CurrentUserInfo.CompanyID, CategoryID, StartDate, EndDate, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>() - 1, order);
            ObjList listobj = new ObjList();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ListInfo info = new BE.ListInfo();

                        info.ROW = dtr["ROW"].ToString();
                        info.ReplyCount = dtr["CNT"].ToString().ToNumber<int>();
                        info.DeptName = dtr["deftname"].ToString();
                        info.WriteUserName = dtr["WriteUserName"].ToString();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : 김광일
        /// 문서조회 총 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetWriteCountCount(string p)
        {
            SetDecryptString(p);
            string CategoryID = string.Empty;
            string StartDate = string.Empty;
            string EndDate = string.Empty;

            CategoryID = GetDecryptParam("CategoryID", "");
            StartDate = GetDecryptParam("StartDate", "");
            EndDate = GetDecryptParam("EndDate", "");

            int icnt = 0;
            //return icnt;
            DAL.ListViewers biz = new DAL.ListViewers();

            icnt = biz.GetListWriteListCount(CurrentUserInfo.CompanyID, CategoryID, StartDate, EndDate);

            return icnt;
        }

        #endregion 통계
    }
}