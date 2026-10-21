using System;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws.Admin.Template
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSBoard
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

        #region 템플릿

        /// <summary>
        /// 작성자 : 심재운
        /// 템플릿 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetTemplateList(string p)
        {
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;
            string searchField = string.Empty;
            string searchValue = string.Empty;
            string order = string.Empty;

            SetDecryptString(p);

            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
            searchField = GetDecryptParam("searchField", "");
            searchValue = GetDecryptParam("searchValue", "");
            order = GetDecryptParam("order", "");

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.Templates biz = new DAL.Templates();
            DataTableReader dtr = biz.GetTemplatePageList(CurrentUserInfo.CompanyID, setPageingStr[0], setPageingStr[1], searchField, searchValue, order);

            ObjList listobj = new ObjList();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.TemplatesInfo info = new BE.TemplatesInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.TemplateID = dtr["TemplateID"].ToString().ToNumber<int>();
                        info.Title = dtr["Title"].ToString();
                        info.Content = dtr["Content"].ToString();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                        DateTime dt;

                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                        {
                            info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        }
                        else
                        {
                            info.WriteDateStr = string.Empty;
                        }


                        int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;
                        info.ROW = listNum.ToString();

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 템플릿 총 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetTemplateCount(string p)
        {
            SetDecryptString(p);
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");

            int icnt = 0;
            //return icnt;
            DAL.Templates biz = new DAL.Templates();

            icnt = biz.GetTemplatePageListCount(CurrentUserInfo.CompanyID, searchField, searchValue);

            return icnt;
        }

        #endregion 템플릿
    }
}