using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.PMS.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.PMS.Web.Ws.Board
{
    /// <summary>
    /// ReadList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ReadList : Base.BaseWSPMS
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

        #endregion


        #region 조회자

        /// <summary>
        /// Gets the page list.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListViewer(string p)
        {
            string ProjectID = string.Empty;
            string MenuID = string.Empty;
            string totalCount = string.Empty;
            string BoardID = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;

            SetDecryptString(p);
            ProjectID = GetDecryptParam("projectID", "0");
            MenuID = GetDecryptParam("menuID", "0");
            BoardID = GetDecryptParam("boardID", "0");

            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

          
            //string[] setPageingStr = rtnSetPaging.Split(',');
            BE.BoardViewerInfo listInfo = new BoardViewerInfo();
            DAL.BoardViewer list = new Zeospace.DeskPlus.PMS.DAL.BoardViewer();

            listInfo.CompanyID = CurrentUserInfo.CompanyID;
            listInfo.ProjectID = ProjectID.ToNumber<int>();
            listInfo.MenuID = MenuID.ToNumber<int>();
            listInfo.BoardID = BoardID.ToNumber<int>();
            listInfo.PageSize = setPageingStr[0].ToNumber<int>();
            listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();

            DataTableReader dtr = list.GetPageList(listInfo);

            ObjList listobj = new ObjList();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.PMS.BE.BoardViewerInfo info = new BE.BoardViewerInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ProjectID = dtr["ProjectID"].ToString().ToNumber<int>();
                        info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                        info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();

                        #region 부서 다국어 설정
                        info.DeptBase = this.GetLangName(
                                dtr["DeptBase"].ToString().ToReverseXSS().ToNull(), dtr["DeptKo"].ToString().ToReverseXSS().ToNull(),
                                dtr["DeptEn"].ToString().ToReverseXSS().ToNull(), dtr["DeptJa"].ToString().ToReverseXSS().ToNull(),
                                dtr["DeptFr"].ToString().ToReverseXSS().ToNull(), dtr["DeptZhcn"].ToString().ToReverseXSS().ToNull(),
                                dtr["DeptZhtw"].ToString().ToReverseXSS().ToNull(), PreLang);
                        #endregion

                        #region 직급 다국어 설정
                        info.TitleBase = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 직책 다국어 설정
                        info.DutyBase = this.GetLangName(
                                dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        info.NameBase = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);

                        DateTime dt;

                        if (DateTime.TryParse(dtr["CRUDDate"].ToString(), out dt))
                            info.CRUDDateStr = DateHelpers.GetCultureDate(dt, PreLang).Substring(0, 10);
                        else
                            info.CRUDDateStr = string.Empty;

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// Gets the list count.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListViewerCount(string p)
        {
            SetDecryptString(p);
            string ProjectID = GetDecryptParam("projectID", "0");
            string MenuID = GetDecryptParam("menuID", "0");
            string BoardID = GetDecryptParam("boardID", "0");

            //

            BE.BoardViewerInfo listInfo = new BoardViewerInfo();
            DAL.BoardViewer list = new DAL.BoardViewer();

            listInfo.CompanyID = CurrentUserInfo.CompanyID;
            listInfo.ProjectID = ProjectID.ToNumber<int>();
            listInfo.MenuID = MenuID.ToNumber<int>();
            listInfo.BoardID = BoardID.ToNumber<int>();

            return list.GetPageCount(listInfo);
        }

        #endregion
    }
}
