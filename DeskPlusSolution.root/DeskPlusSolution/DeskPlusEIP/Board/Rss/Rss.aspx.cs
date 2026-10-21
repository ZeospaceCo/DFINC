using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;
using System.Data;
using Zeospace.DeskPlus.Board.BE;
using System.Text;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Rss
{
    public partial class Rss : Base.BasePageBoard
    {
        #region 변수설정        
        private int PageSize;

        private string OptionUseDept;//부서별게시판여부
        private string OptionUseView;//보기용 여부
        private string Auth; //관리자 : 1; 일반인 : 0

        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParam();

            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleBoard = CurrentUserInfo.IsInRole("board");//board admin
            Auth = (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleBoard) ? "1" : "0"; //관리자 : 1; 일반인 : 0

            LoadData();
        }

        /// <summary>
        /// 데이터 초기화
        /// </summary>
        private void LoadData()
        {
            Response.Clear();
            Response.ContentType = "application/rss+xml";

            DAL.Category dalCat = new DAL.Category();
            DataSet ds = dalCat.GetCategory(CurrentUserInfo.CompanyID, CategoryID);

            Rss20DataInfo feedInfo = new Rss20DataInfo();

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OptionUseDept = dr["OptionUseDept"].ToString();
                    OptionUseView = dr["OptionUseView"].ToString();

                    string title = this.GetLangName(
                                    dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                    dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                    dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                    dr["NameZhtw"].ToString().ToNull(), PreLang);

                    feedInfo.Title = title;
                    feedInfo.Description = title;

                    string baseHttp = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                    string fullDomain = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                    feedInfo.LinkUrl = baseHttp + fullDomain;

                    feedInfo.RssID = "BOARD_" + dr["CategoryID"].ToString();
                    feedInfo.LastUpdatedTime = Convert.ToDateTime(dr["WriteDate"]);
                }
            }



            List<Rss20DataInfo> itemInfo = GetPageList();

            SyndicationFeed syndicationFeed =
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Rss20Helper.SyndicationHelper(feedInfo, itemInfo);

            if (syndicationFeed != null && syndicationFeed.Items.Count() > 0)
            {
                Rss20FeedFormatter formatter = new Rss20FeedFormatter(syndicationFeed);
                XmlWriter writer = XmlWriter.Create(Response.Output, null);
                formatter.WriteTo(writer);
                writer.Flush();
            }
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터
        /// </summary>
        private void LoadParam()
        {
            string decryString = "";
            string param = GetParm("rss", "!NULL");
            if (!string.IsNullOrEmpty(param) && param != "!NULL")
            {
                param = Server.UrlDecode(param);//디코딩
                decryString = Utility.DecryptString(param);
                decryString = Server.UrlDecode(decryString);//디코딩
            }

            foreach (string _qry in decryString.Split('&'))
            {
                string key = _qry.Split('=')[0];
                string keyValue = _qry.Split('=')[1];

                switch (key)
                {
                    case "categoryID":
                        CategoryID = keyValue.ToNumber<int>();
                        break;
                    case "pageSize":
                        PageSize = keyValue.ToNumber<int>();
                        break;
                }
            }
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSimpleAdd.Text = resMng.GetString("Cancel");
            }
        }

        #region 리스트

        /// <summary>
        /// 게시판 카운트 가져오기
        /// </summary>
        /// <returns></returns>
        private int GetListCount()
        {
            try
            {
                int icnt = 0;
                bool isChild = (OptionUseView == "1") ? true : false; //보기용이면 하위검색가능
                string myAuthCatList = "";

                if (CategoryID > 0)
                {
                    if (Auth == "1")
                    {
                        myAuthCatList = CategoryID.ToString();

                        if (isChild)
                        {
                            BLL.Category catBiz = new BLL.Category();
                            myAuthCatList = catBiz.GetChildCategory(CurrentUserInfo.CompanyID, CategoryID);
                        }
                    }
                    else
                    {
                        myAuthCatList = GetMyAuthCatList(CategoryID, isChild);
                    }
                }
                else
                {
                    myAuthCatList = "-1"; //카테고리 값이 0 넘어오면 -1 로 해서 리스트 안나오게 함
                }

                DAL.Lists biz = new DAL.Lists();
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
        /// GetPageList
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private List<Rss20DataInfo> GetPageList()
        {
            try
            {
                int totalCount = GetListCount();
                string pageSizePageNo = "1";

                string rtnSetPaging = SetPaging(totalCount, PageSize, pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                bool isChild = (OptionUseView == "1") ? true : false; //보기용이면 하위검색가능

                DAL.Lists biz = new DAL.Lists();

                string myAuthCatList = "";

                if (CategoryID > 0)
                {
                    if (Auth == "1")
                    {
                        myAuthCatList = CategoryID.ToString();

                        if (isChild)
                        {
                            BLL.Category catBiz = new BLL.Category();
                            myAuthCatList = catBiz.GetChildCategory(CurrentUserInfo.CompanyID, CategoryID);
                        }
                    }
                    else
                    {
                        myAuthCatList = GetMyAuthCatList(CategoryID, isChild);
                    }
                }
                else
                {
                    myAuthCatList = "-1"; //카테고리 값이 0 넘어오면 -1 로 해서 리스트 안나오게 함
                }


                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID,
                    myAuthCatList,
                    setPageingStr[0], setPageingStr[1],
                    "", "",
                    "", "", "",
                    OptionUseDept, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
                List<Rss20DataInfo> listobj = new List<Rss20DataInfo>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        string baseHttp = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                        string fullDomain = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");

                        while (dtr.Read())
                        {
                            Rss20DataInfo info = new Rss20DataInfo();
                            info.RssID = "BOARD_" + dtr["ListID"].ToString();
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

                            info.Description = dtr["MobileContent"].ToString();
                            if (info.Description.Length > 200)
                                info.Description = info.Description.Substring(0, 195) + "..";

                            info.LastUpdatedTime = Convert.ToDateTime(dtr["WriteDate"]);

                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("catID", dtr["CategoryID"].ToString());
                            paraDic.Add("categoryID", dtr["CategoryID"].ToString());
                            paraDic.Add("listID", dtr["ListID"].ToString());

                            info.LinkUrl = baseHttp + fullDomain + "/DeskPlusEIP/Board/Content/ViewPopup.aspx?p=" + GetEncryptParam(Utility.GetParameterDic(paraDic));

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
                DAL.Category dal = new DAL.Category();
                DataSet ds = dal.GetCategory(CurrentUserInfo.CompanyID, _categoryID);

                string optionUseView = "";
                string optionUseDept = "";

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        optionUseView = dr["OptionUseView"].ToString(); //보기용
                        optionUseDept = dr["OptionUseDept"].ToString(); //부서게시판 여부용
                    }
                }

                #region 내가 볼 수 있는 하위 카테고리
                NameValueCollection nvc = new NameValueCollection();
                string myAuthCatList = "";// _categoryID.ToString() + ",";
                BLL.Category biz = new BLL.Category();

                //보기용은 하위카테고리까지 가져옴
                if (optionUseView == "1")
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, _categoryID, optionUseDept, true, BE.BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) myAuthCatList += s + ",";
                }
                else
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, _categoryID, optionUseDept, _useChild, BE.BoardAuth.Read, true, false);

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

        #endregion        

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion        
    }
}