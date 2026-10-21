using System;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
{
    /// <summary>
    /// RateList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class RateList : Base.BaseWSBoard
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

        #region 투표

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 투표 접근 가능 여부
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        private bool AccessRating(int _categoryID)
        {
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleBoard = CurrentUserInfo.IsInRole("board");//board admin
            bool admin = false;
            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleBoard) admin = true;

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(_categoryID);

            BLL.Category biz = new BLL.Category();
            return biz.GetAccessRating(boaComID, _categoryID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID, admin);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 투표 페이징 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageRating(string p)
        {
            string categoryID = string.Empty;
            string listID = string.Empty;
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;

            SetDecryptString(p);
            categoryID = GetDecryptParam("categoryID", "0");
            listID = GetDecryptParam("listID", "0");
            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            ObjList listobj = new ObjList();
            if (AccessRating(categoryID.ToNumber<int>()) == false) return listobj;

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.Ratings biz = new DAL.Ratings();
            DataTableReader dtr = biz.GetPageList(boaComID, categoryID.ToNumber<int>(),
                listID.ToNumber<int>(), setPageingStr[0], setPageingStr[1]);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Board.BE.RatingInfo info = new BE.RatingInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.RatingID = dtr["RatingID"].ToString().ToNumber<int>();
                        info.Rating = dtr["Rating"].ToString().ToNumber<int>();
                        info.Comments = dtr["Comments"].ToString();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        info.DeptCodeID = dtr["DeptCodeID"].ToString().ToNumber<int>();

                        #region 부서 다국어 설정

                        info.DeptName = this.GetLangName(
                                dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                        #endregion 부서 다국어 설정

                        info.UserName = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);

                        DateTime dt;

                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                            info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.WriteDateStr = string.Empty;

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 투표 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetRatingCount(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            if (AccessRating(categoryID.ToNumber<int>()) == false) return 0;

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            DAL.Ratings list = new DAL.Ratings();
            return list.GetPageListCount(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>());
        }

        #endregion 투표

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
            string categoryID = string.Empty;
            string listID = string.Empty;
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;

            SetDecryptString(p);
            categoryID = GetDecryptParam("categoryID", "0");
            listID = GetDecryptParam("listID", "0");
            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.ListViewers biz = new DAL.ListViewers();
            DataTableReader dtr = biz.GetPageList(boaComID, categoryID.ToNumber<int>(),
                listID.ToNumber<int>(), setPageingStr[0], setPageingStr[1]);

            ObjList listobj = new ObjList();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Board.BE.ListViewersInfo info = new BE.ListViewersInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();

                        #region 부서 다국어 설정

                        info.DeptName = this.GetLangName(
                                dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                        #endregion 부서 다국어 설정

                        #region 직급 다국어 설정

                        info.TitleName = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                        #endregion 직급 다국어 설정

                        #region 직책 다국어 설정

                        info.DutyName = this.GetLangName(
                                dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                dtr["DutyZhtw"].ToString().ToNull(), PreLang);

                        #endregion 직책 다국어 설정

                        info.UserName = this.GetLangName(
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
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            DAL.ListViewers list = new DAL.ListViewers();
            return list.GetPageListCount(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>());
        }

        #endregion 조회자
    }
}