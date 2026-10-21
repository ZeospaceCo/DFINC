using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;
using System.Text;

namespace Zeospace.DeskPlus.MDM.Web.Ws.Send
{
    /// <summary>
    /// GetTargetURL의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetTargetURL : Base.BaseWSMDM
    {
        #region GET()

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
        /// 게시판 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string categoryID = GetDecryptParam("categoryID", "0");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");

                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchGroup = GetDecryptParam("searchGroup", "");
                string order = GetDecryptParam("order", "");
                string optionUseDept = "0";//관리자는 부서별 게시판을 전체 보기 위함이므로 "0" 처리한다.
                string optionUseView = "";

                //통합게시판 회사아이디
                int boardComID = Zeospace.DeskPlus.Board.BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

                Zeospace.DeskPlus.Board.DAL.Category dal = new Zeospace.DeskPlus.Board.DAL.Category();
                DataSet ds = dal.GetCategory(boardComID, categoryID.ToNumber<int>());

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        optionUseView = dr["OptionUseView"].ToString();
                    }
                }

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                bool isChild = (searchGroup == "2") ? true : false;//하위검색
                isChild = (optionUseView == "1") ? true : isChild; //보기용이면 하위검색가능

                string myAuthCatList = categoryID.ToString();

                if (isChild)
                {
                    Zeospace.DeskPlus.Board.BLL.Category catBiz = new Zeospace.DeskPlus.Board.BLL.Category();
                    myAuthCatList = catBiz.GetChildCategory(boardComID, categoryID.ToNumber<int>());
                }

                if (myAuthCatList.Length == 0) myAuthCatList = "-1";

                Zeospace.DeskPlus.Board.DAL.Lists biz = new Zeospace.DeskPlus.Board.DAL.Lists();

                DataTableReader dtr = biz.GetPageList(boardComID, myAuthCatList, setPageingStr[0], setPageingStr[1], "", "",
                    searchField, searchValue, order, optionUseDept, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Board.BE.ListInfo info = new Zeospace.DeskPlus.Board.BE.ListInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();

                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.HeaderID = dtr["HeaderID"].ToString().ToNumber<int>();

                            #region 말머리

                            string headerName = this.GetLangName(
                                    dtr["HeaderBase"].ToString().ToNull(), dtr["HeaderKo"].ToString().ToNull(),
                                    dtr["HeaderEn"].ToString().ToNull(), dtr["HeaderJa"].ToString().ToNull(),
                                    dtr["HeaderFr"].ToString().ToNull(), dtr["HeaderZhcn"].ToString().ToNull(),
                                    dtr["HeaderZhtw"].ToString().ToNull(), PreLang);

                            info.HeaderName = (headerName.Length > 0) ? "[" + headerName + "]" : "";

                            #endregion 말머리

                            if (dtr["OptionAnonymity"].ToString() == "1") //익명
                            {
                                string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", "익명,NoName");
                                info.WriteUserName = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
                                info.WriteUserID = 0;
                            }
                            else
                            {
                                info.WriteUserName = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            }

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.ROW = (dtr["OptionTop"].ToString() == "1") ? "<span class=\"badge notice badge-warning\"><span class=\"s_notice\"></span></span>" : listNum.ToString();

                            info.Title = dtr["Title"].ToString();//리스트용
                            info.Title = GetTitle(info, info.Title);

                            //말머리
                            info.Title = (info.HeaderName.Length > 0) ?
                                info.HeaderName + " " + info.Title : info.Title;

                            info.TitleShort = info.Title.ToReverseXSS().Replace("\"", "zeo1").Replace("'","zeo2");


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
        /// 게시판 제목 
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_title"></param>
        /// <returns></returns>
        private string GetTitle(Zeospace.DeskPlus.Board.BE.ListInfo _info, string _title)
        {
            string title = _info.Title;
            string arrowURl = "<img src='" + this.BaseImgPath + "/Icon/ico_arrow.gif' title='' />";

            string space = "";
            for (int i = 0; i < _info.Depth; i++) space += "&nbsp;&nbsp;";

            //답변글 일 경우
            if (_info.Depth > 0) title = arrowURl + " " + title;
            if (space.Length > 0) title = space + title;

            return title;
        }

        /// <summary>
        /// Gets the list count.
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
                string categoryID = GetDecryptParam("categoryID", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchGroup = GetDecryptParam("searchGroup", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string optionUseDept = "0";//관리자는 부서별 게시판을 전체 보기 위함이므로 "0" 처리한다.
                string optionUseView = "";

                int icnt = 0;

                //통합게시판 회사아이디
                int boardComID = Zeospace.DeskPlus.Board.BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

                Zeospace.DeskPlus.Board.DAL.Category dal = new Zeospace.DeskPlus.Board.DAL.Category();
                DataSet ds = dal.GetCategory(boardComID, categoryID.ToNumber<int>());

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        optionUseView = dr["OptionUseView"].ToString();
                    }
                }

                bool isChild = (searchGroup == "2") ? true : false;//하위검색
                isChild = (optionUseView == "1") ? true : isChild; //보기용이면 하위검색가능

                string myAuthCatList = categoryID.ToString();

                if (isChild)
                {
                    Zeospace.DeskPlus.Board.BLL.Category catBiz = new Zeospace.DeskPlus.Board.BLL.Category();
                    myAuthCatList = catBiz.GetChildCategory(boardComID, categoryID.ToNumber<int>());
                }

                if (myAuthCatList.Length == 0) myAuthCatList = "-1";

                Zeospace.DeskPlus.Board.DAL.Lists biz = new Zeospace.DeskPlus.Board.DAL.Lists();
                icnt = biz.GetPageListCount(boardComID, myAuthCatList, "", "", searchField, searchValue, optionUseDept, CurrentUserInfo.DeptID);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion
    }
}
