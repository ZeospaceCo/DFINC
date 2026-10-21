using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Board.BE;
using Zeospace.DeskPlus.Board.BLL;
using Zeospace.DeskPlus.Board.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.Board
{
    /// <summary>
    /// TabB의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Tab : Base.BaseWSPortlet
    {

        #region 변수
        private int CategoryID;
        private int PageSize;
        private int PageSizePageNo;
        private int TotalCount;

        private string OptionUseDept;//부서별게시판여부
        private string OptionUseView;//보기용 여부
        private string Auth; //관리자 : 1; 일반인 : 0
        #endregion


        /// <summary>
        /// 게시판 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBoardList(string p)
        {
            SetDecryptString(p);
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

            //bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            //bool isRoleBoard = CurrentUserInfo.IsInRole("board");//board admin
            //Auth = (isRoleAdmin || isRoleBoard) ? "1" : "0"; //관리자 : 1; 일반인 : 0

            Auth = "0"; //관리자 : 1; 일반인 : 0

            try
            {
                List<object> obj = new List<object>();
                Zeospace.DeskPlus.Board.DAL.Category dalCat = new Zeospace.DeskPlus.Board.DAL.Category();
                DataSet ds = dalCat.GetCategory(CurrentUserInfo.CompanyID, CategoryID);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0) return obj;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        OptionUseDept = dr["OptionUseDept"].ToString();
                        OptionUseView = dr["OptionUseView"].ToString();
                    }
                }

                
                List<Zeospace.DeskPlus.Board.BE.ListInfo> al = GetBoardPageList();
                foreach (Zeospace.DeskPlus.Board.BE.ListInfo info in al)
                {
                    obj.Add(info);
                }

                return obj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetBoardList] : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 게시판 총 카운트
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetBoardTotCount(string p)
        {
            SetDecryptString(p);
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleBoard = CurrentUserInfo.IsInRole("board");//board admin
            Auth = (isRoleAdmin || isRoleBoard) ? "1" : "0"; //관리자 : 1; 일반인 : 0

            try
            {

                Zeospace.DeskPlus.Board.DAL.Category dalCat = new Zeospace.DeskPlus.Board.DAL.Category();
                DataSet ds = dalCat.GetCategory(CurrentUserInfo.CompanyID, CategoryID);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0) return 0;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        OptionUseDept = dr["OptionUseDept"].ToString();
                        OptionUseView = dr["OptionUseView"].ToString();
                    }
                }

                return GetBoardListCount();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetBoardList] : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 게시판 카운트 가져오기
        /// </summary>
        /// <returns></returns>        
        private int GetBoardListCount()
        {
            try
            {
                int icnt = 0;
                bool isChild = (OptionUseView == "1") ? true : false; //보기용이면 하위검색가능
                string myAuthCatList = "";

                if (CategoryID > 0)
                {
                    //관리자 일경우
                    if (Auth == "1")
                    {
                        myAuthCatList = CategoryID.ToString();

                        if (isChild)
                        {
                            Zeospace.DeskPlus.Board.BLL.Category catBiz = new Zeospace.DeskPlus.Board.BLL.Category();
                            myAuthCatList = catBiz.GetChildCategory(CurrentUserInfo.CompanyID, CategoryID);
                        }
                    }
                    else
                    {
                        myAuthCatList = GetMyAuthCatList(CategoryID, isChild);
                    }
                }

                if (myAuthCatList.Length == 0) myAuthCatList = "-1";

                Zeospace.DeskPlus.Board.DAL.Lists biz = new Zeospace.DeskPlus.Board.DAL.Lists();
                icnt = biz.GetPageListCount(CurrentUserInfo.CompanyID,
                    myAuthCatList,
                    "", "",
                    "", "", OptionUseDept, CurrentUserInfo.DeptID);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 페이지 리스트
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private List<Zeospace.DeskPlus.Board.BE.ListInfo> GetBoardPageList()
        {
            try
            {
                int totalCount = TotalCount;// GetBoardListCount();
                //int maxPageSize = totalCount / PageSize + 1;

                //if (PageSizePageNo > maxPageSize) PageSizePageNo = maxPageSize;
                //string pageSizePageNo = "2";

                string rtnSetPaging = SetPaging(totalCount, PageSize, PageSizePageNo);
                string[] setPageingStr = rtnSetPaging.Split(',');

                bool isChild = (OptionUseView == "1") ? true : false; //보기용이면 하위검색가능

                Zeospace.DeskPlus.Board.DAL.Lists biz = new Zeospace.DeskPlus.Board.DAL.Lists();

                string myAuthCatList = "";

                if (CategoryID > 0)
                {
                    //관리자 일경우
                    if (Auth == "1")
                    {
                        myAuthCatList = CategoryID.ToString();

                        if (isChild)
                        {
                            Zeospace.DeskPlus.Board.BLL.Category catBiz = new Zeospace.DeskPlus.Board.BLL.Category();
                            myAuthCatList = catBiz.GetChildCategory(CurrentUserInfo.CompanyID, CategoryID);
                        }
                    }
                    else
                    {
                        myAuthCatList = GetMyAuthCatList(CategoryID, isChild);
                    }
                }

                if (myAuthCatList.Length == 0) myAuthCatList = "-1";


                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID,
                    myAuthCatList,
                    setPageingStr[0], setPageingStr[1],
                    "", "",
                    "", "", "",
                    OptionUseDept, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
                List<Zeospace.DeskPlus.Board.BE.ListInfo> listobj = new List<Zeospace.DeskPlus.Board.BE.ListInfo>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        string baseHttp = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                        string fullDomain = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");

                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Board.BE.ListInfo info = new Zeospace.DeskPlus.Board.BE.ListInfo();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();//리스트용

                            #region 말머리
                            string headerName = this.GetLangName(
                                    dtr["HeaderBase"].ToString().ToNull(), dtr["HeaderKo"].ToString().ToNull(),
                                    dtr["HeaderEn"].ToString().ToNull(), dtr["HeaderJa"].ToString().ToNull(),
                                    dtr["HeaderFr"].ToString().ToNull(), dtr["HeaderZhcn"].ToString().ToNull(),
                                    dtr["HeaderZhtw"].ToString().ToNull(), PreLang);

                            headerName = (headerName.Length > 0) ? "[" + headerName + "]" : "";

                            //말머리 있으면 넣기
                            info.Title = (headerName.Length > 0) ?
                                headerName + " " + info.Title : info.Title;
                            #endregion

                            info.MobileContent = dtr["MobileContent"].ToString();
                            if (info.MobileContent.Length > 200)
                                info.MobileContent = info.MobileContent.Substring(0, 195) + "..";

                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("catID", dtr["CategoryID"].ToString());
                            paraDic.Add("categoryID", dtr["CategoryID"].ToString());
                            paraDic.Add("listID", dtr["ListID"].ToString());
                            info.HrefUrl = baseHttp + fullDomain + "/DeskPlusEIP/Board/Content/ViewPopup.aspx?p=" + GetEncryptParam(Utility.GetParameterDic(paraDic));

                            #region 이름 다국어 설정
                            info.WriteUserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelper.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

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
        /// 권한별 카테고리 리스트 가져오기
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_useChild">if set to <c>true</c> [_use child].</param>
        /// <returns></returns>
        private string GetMyAuthCatList(int _categoryID, bool _useChild)
        {
            try
            {
                #region 내가 볼 수 있는 하위 카테고리
                NameValueCollection nvc = new NameValueCollection();
                string myAuthCatList = "";// _categoryID.ToString() + ",";
                Zeospace.DeskPlus.Board.BLL.Category biz = new Zeospace.DeskPlus.Board.BLL.Category();

                //보기용은 하위카테고리까지 가져옴
                if (OptionUseView == "1")
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, _categoryID, OptionUseDept, true, BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) myAuthCatList += s + ",";
                }
                else
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, _categoryID, OptionUseDept, _useChild, BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) myAuthCatList += s + ",";
                }

                myAuthCatList = myAuthCatList.TrimEnd(',');
                #endregion

                return myAuthCatList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
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
    }
}
