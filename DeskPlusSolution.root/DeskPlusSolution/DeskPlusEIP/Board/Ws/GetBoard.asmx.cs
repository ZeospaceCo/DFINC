using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws
{
    /// <summary>
    /// GetBoard의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class GetBoard : Base.BaseWSBoard
    {
        #region GET()

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

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 작성자 이름설정하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_titleName">Name of the _title.</param>
        /// <param name="_dutyName">Name of the _duty.</param>
        /// <returns></returns>
        private string GetWriteUserName(string _writeUserName, string _deptName,
            string _titleName, string _dutyName)
        {
            string nameFieldFormat = _writeUserName;
            string applyCategory = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "VWNameFieldCategory", "0");
            nameFieldFormat = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "VWNameField", "WriteName");
            nameFieldFormat = (nameFieldFormat.IndexOf("WriteName") != -1) ? nameFieldFormat.Replace("WriteName", _writeUserName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("DeptName") != -1) ? nameFieldFormat.Replace("DeptName", _deptName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("TitleName") != -1) ? nameFieldFormat.Replace("TitleName", _titleName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("DutyName") != -1) ? nameFieldFormat.Replace("DutyName", _dutyName) : nameFieldFormat;

            //이름 보이기 (이름:WriteName, 부서:Dept, 직급:Title, 직책:Duty)
            return nameFieldFormat;
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

                #endregion 내가 볼 수 있는 하위 카테고리

                return myAuthCatList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 게시판 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)]
        public string GetPageList(string p)
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
                string optionUseDept = GetDecryptParam("optionUseDept", "");//부서별게시판여부
                string optionUseView = GetDecryptParam("optionUseView", "");//보기용 여부
                string auth = GetDecryptParam("auth", ""); //관리자 : 1; 일반인 : 0

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                bool isChild = (searchGroup == "2") ? true : false;//하위검색
                isChild = (optionUseView == "1") ? true : isChild; //보기용이면 하위검색가능

                DAL.Lists biz = new DAL.Lists();

                string myAuthCatList = "";

                if (categoryID != "0" && categoryID.Length > 0)
                {
                    if (auth == "1")
                    {
                        myAuthCatList = categoryID.ToString();

                        if (isChild)
                        {
                            BLL.Category catBiz = new BLL.Category();
                            myAuthCatList = catBiz.GetChildCategory(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>());
                        }
                    }
                    else
                    {
                        myAuthCatList = GetMyAuthCatList(categoryID.ToNumber<int>(), isChild);
                    }
                }
                else
                {
                    myAuthCatList = "-1"; //카테고리 값이 0 넘어오면 -1 로 해서 리스트 안나오게 함
                }

                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (string.Compare(searchTerm, "week", true) == 0 || string.Compare(searchTerm, "month", true) == 0)
                {
                    if (string.Compare(searchTerm, "week", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (string.Compare(searchTerm, "month", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (string.Compare(searchTerm, "period", true) == 0)
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion 날짜

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID,
                    myAuthCatList,
                    setPageingStr[0], setPageingStr[1],
                    startDate, endDate,
                    searchField, searchValue, order,
                    optionUseDept, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Board.BE.ListInfo info = new BE.ListInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();

                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Depth = dtr["Depth"].ToString().ToNumber<int>();
                            //info.Content = dtr["Content"].ToString();
                            //info.MobileContent = dtr["MobileContent"].ToString();

                            info.BlogContent = dtr["MobileContent"].ToString().ToReverseXSS();

                            if (info.BlogContent.Length > 200)
                                info.BlogContent = info.BlogContent.Substring(0, 195) + "..";

                            info.OwnerDeptID = dtr["OwnerDeptID"].ToString().ToNumber<int>();
                            info.OptionModify = dtr["OptionModify"].ToString();
                            info.OptionTop = dtr["OptionTop"].ToString();
                            info.OptionTopDay = dtr["OptionTopDay"].ToString().ToNumber<int>();

                            info.ReadCount = dtr["ReadCount"].ToString().ToNumber<int>();
                            //info.DeptName = dtr["DeptName"].ToString();
                            //info.TitleName = dtr["TitleName"].ToString();
                            //info.DutyName = dtr["DutyName"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.ImgCount = dtr["ImgCount"].ToString().ToNumber<int>();
                            info.HeaderID = dtr["HeaderID"].ToString().ToNumber<int>();
                            info.ImpoCnt = dtr["ImpoCnt"].ToString().ToNumber<int>();

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
                                #region 이름 다국어 설정

                                info.WriteUserName = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);

                                #endregion 이름 다국어 설정

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

                                #region 다국어 경로 다국어 설정

                                info.PathName = this.GetLangName(
                                    dtr["PathNameKo"].ToString(),
                                    dtr["PathNameKo"].ToString(), dtr["PathNameEn"].ToString(), dtr["PathNameJa"].ToString(),
                                    dtr["PathNameFr"].ToString(), dtr["PathNameZhCn"].ToString(), dtr["PathNameZhTw"].ToString(), PreLang);

                                #endregion 다국어 경로 다국어 설정

                                info.WriteUserName = GetWriteUserName(
                                    info.WriteUserName,
                                    info.DeptName,
                                    info.TitleName,
                                    info.DutyName);
                            }

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            info.ModifyUserID = dtr["ModifyUserID"].ToString().ToNumber<int>();
                            info.ModifyUserName = dtr["ModifyUserName"].ToString();

                            if (DateTime.TryParse(dtr["ModifyDate"].ToString(), out dt))
                                info.ModifyDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.ModifyDate = string.Empty;

                            info.PassWord = dtr["PassWord"].ToString();
                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            //info.ROW = (dtr["OptionTop"].ToString() == "1") ? "<img src='" + this.BaseImgPath + "/Lang/" + this.PreLang + "/not.gif' alt=''>" : listNum.ToString();

                            #region 다국어 카테고리 이름 설정

                            info.CA_Title = this.GetLangName(
                                dtr["CNameBase"].ToString(),
                                dtr["CNameKo"].ToString(), dtr["CNameEn"].ToString(), dtr["CNameJa"].ToString(),
                                dtr["CNameFr"].ToString(), dtr["CNameZhCn"].ToString(), dtr["CNameZhTw"].ToString(), PreLang);

                            #endregion 다국어 카테고리 이름 설정

                            info.Filecount = dtr["Filecount"].ToString().ToNumber<int>();
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();

                            info.Title = dtr["Title"].ToString().ToReverseXSS();//리스트용

                            info.TitleNoHtml = info.Title; //툴팁용
                            info.TitleShort = info.Title;//갤러리용

                            if (info.TitleShort.Length > 15)
                                info.TitleShort = info.TitleShort.Substring(0, 12) + "..";

                            info.TitleShort = GetTitle(info, info.TitleShort, dtr["Etc2"].ToString());
                            info.Title = GetTitle(info, info.Title, dtr["Etc2"].ToString());

                            //조회하지 않으면 굵게 처리
                            if (dtr["ReadCheck"].ToString().ToNumber<int>() == 0) info.Title = "<b>" + info.Title + "</b>";

                            //말머리
                            info.Title = (info.HeaderName.Length > 0) ?
                                info.HeaderName + " " + info.Title : info.Title;

                            info.ScreenShot = dtr["ScreenShot"].ToString();
                            if (info.ScreenShot.ToNull().Length == 0)
                            {
                                info.ScreenShot = this.BaseImgPath + "/Icon/ico_noimg2.gif";
                            }

                            listobj.Add(info);
                        }
                    }
                }

                return JsonConvert.SerializeObject(listobj);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 게시판 제목 스타일 입히기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_title">The _title.</param>
        /// <param name="_etc2">The _etc2.</param>
        /// <returns></returns>
        private string GetTitle(BE.ListInfo _info, string _title, string _etc2)
        {
            string title = SetTitleOptionEtc(_info.Title, _etc2);
            string arrowURl = "<img src='" + this.BaseImgPath + "/Icon/ico_arrow.gif' title='' />";

            string space = "";
            for (int i = 0; i < _info.Depth; i++) space += "&nbsp;&nbsp;";

            //공지글일 경우
            if (_info.OptionTop.ToNumber<int>() > 0) title = "<b>" + title + "</b>";
            //답변글 일 경우
            if (_info.Depth > 0) title = arrowURl + " " + title;
            if (space.Length > 0) title = space + title;

            return title;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 옵션설정에 맞게 옵션 스타일 적용하기
        /// </summary>
        /// <param name="_title">The _title.</param>
        /// <param name="_etc2">The _etc2.</param>
        /// <returns></returns>
        private string SetTitleOptionEtc(string _title, string _etc2)
        {
            try
            {
                if (_etc2.ToNull().Length > 0)
                {
                    string[] etcStr = _etc2.Split(',');
                    StringBuilder sb = new StringBuilder();
                    if (etcStr[2].ToString() != "0") sb.Append("<b>");
                    if (etcStr[0].ToString() != "") sb.Append("<font color='" + etcStr[0].ToString() + "'>");
                    if (etcStr[1].ToString() != "") sb.Append("<font size='" + etcStr[1].ToString() + "'>");
                    sb.Append(_title);
                    if (etcStr[0].ToString() != "") sb.Append("</font>");
                    if (etcStr[1].ToString() != "") sb.Append("</font>");
                    if (etcStr[2].ToString() != "0") sb.Append("</b>");

                    _title = sb.ToString();

                    return _title;
                }

                return _title;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
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
                string optionUseDept = GetDecryptParam("optionUseDept", "");//부서별게시판여부
                string optionUseView = GetDecryptParam("optionUseView", "");//보기용 여부
                string auth = GetDecryptParam("auth", ""); //관리자 : 1; 일반인 : 0

                int icnt = 0;

                bool isChild = (searchGroup == "2") ? true : false;//하위검색
                isChild = (optionUseView == "1") ? true : isChild; //보기용이면 하위검색가능

                string myAuthCatList = "";

                if (categoryID != "0" && categoryID.Length > 0)
                {
                    if (auth == "1")
                    {
                        myAuthCatList = categoryID.ToString();

                        if (isChild)
                        {
                            BLL.Category catBiz = new BLL.Category();
                            myAuthCatList = catBiz.GetChildCategory(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>());
                        }
                    }
                    else
                    {
                        myAuthCatList = GetMyAuthCatList(categoryID.ToNumber<int>(), isChild);
                    }
                }
                else
                {
                    myAuthCatList = "-1"; //카테고리 값이 0 넘어오면 -1 로 해서 리스트 안나오게 함
                }

                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (string.Compare(searchTerm, "week", true) == 0 || string.Compare(searchTerm, "month", true) == 0)
                {
                    if (string.Compare(searchTerm, "week", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (string.Compare(searchTerm, "month", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (string.Compare(searchTerm, "period", true) == 0)
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion 날짜

                DAL.Lists biz = new DAL.Lists();
                icnt = biz.GetPageListCount(CurrentUserInfo.CompanyID,
                    myAuthCatList,
                    startDate, endDate,
                    searchField, searchValue, optionUseDept, CurrentUserInfo.DeptID);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 특정 카테고리의 리스트 개수 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCategoryListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string categoryID = GetDecryptParam("categoryID", "0");

                DAL.Lists biz = new DAL.Lists();
                int icnt = biz.GetCountList(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>());

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 리스트

        #region 첨부파일

        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetAttachFileList(string p)
        {
            try
            {
                SetDecryptString(p);
                string categoryID = GetDecryptParam("categoryID", "0");
                string listID = GetDecryptParam("listID", "0");
                string screenShot = GetDecryptParam("screenShot", "");

                ObjList listobj = new ObjList();

                DAL.AttachFiles dal = new DAL.AttachFiles();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>());

                string fileName = (screenShot.Length > 0) ? System.IO.Path.GetFileName(Server.MapPath(screenShot)).ToLower().Replace("thum_", "") : ""; //파일명

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (BE.AttachFileInfo _attachInfo in attachInfo)
                {
                    Zeospace.DeskPlus.Board.BE.AttachFileInfo info = new BE.AttachFileInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.CategoryID = _attachInfo.CategoryID;
                    info.ListID = _attachInfo.ListID;
                    info.AttachFileID = _attachInfo.AttachFileID;
                    info.FilePath = _attachInfo.FilePath;
                    info.FileName = _attachInfo.FileName;
                    info.FileType = _attachInfo.FileType.Replace(".", "");
                    info.FileLength = _attachInfo.FileLength;
                    info.FileGUID = _attachInfo.FileGUID;
                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    //스크린샷 파일과 동일 여부 찾기
                    info.ScreenShot = (fileName == info.FileName.ToLower() || fileName == info.FileGUID.ToLower()) ? "1" : "0";

                    string fileOk = "0";
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (_attachInfo.FileType.ToLower() == allowedExtensions[i])
                        {
                            fileOk = "1";
                            break;
                        }
                    }

                    info.ImageType = fileOk;

                    listobj.Add(info);
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
        /// 작성자   : shimpark
        /// 작성목적 : 첨부파일 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFileListMenu(string p)
        {
            try
            {
                int categoryID = 0;
                int listID = 0;

                SetDecryptString(p);

                categoryID = GetDecryptParam("categoryID", "0").ToNumber<Int32>();
                listID = GetDecryptParam("listID", "0").ToNumber<Int32>();

                string response = "0";

                DAL.AttachFiles dal = new DAL.AttachFiles();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, categoryID, listID);

                string totalFileInfo = "";

                foreach (BE.AttachFileInfo _attachInfo in attachInfo)
                {
                    if (_attachInfo.FileGUID != "")
                    {
                        totalFileInfo += _attachInfo.FileName + ":" + _attachInfo.FilePath + _attachInfo.FileGUID + "|"; // Key < 파일경로파일명
                    }
                    else
                    {
                        totalFileInfo += _attachInfo.FileName + ":" + _attachInfo.FilePath + _attachInfo.FileName + "|"; // Key < 파일경로파일명
                    }
                }

                totalFileInfo = totalFileInfo.TrimEnd('|');

                if (totalFileInfo.Length > 0)
                {
                    response = totalFileInfo.Replace("&", "&amp;");		// 첨부파일 정보 가져오기
                }

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 첨부파일

        #region 댓글

        /// <summary>
        /// 댓글 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetShortReplyList(string p)
        {
            string categoryID = string.Empty;
            string listID = string.Empty;
            SetDecryptString(p);
            categoryID = GetDecryptParam("categoryID", "0");
            listID = GetDecryptParam("listID", "0");

            ObjList listobj = new ObjList();

            DAL.ShortReply sr = new DAL.ShortReply();

            DataTableReader dtr = sr.GetShortReplyList(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>());
            if (dtr != null)
            {
                string name = "";

                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Board.BE.ShortReplyInfo info = new BE.ShortReplyInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.ShortReplyID = dtr["ShortReplyID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);

                        info.UserName = name;
                        info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        DateTime dt;

                        if (DateTime.TryParse(dtr["InputDate"].ToString(), out dt))
                            info.InputDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.InputDate = string.Empty;

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        #endregion 댓글

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

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.ListViewers biz = new DAL.ListViewers();
            DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(),
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

            DAL.ListViewers list = new DAL.ListViewers();
            return list.GetPageListCount(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>());
        }

        #endregion 조회자

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

            BLL.Category biz = new BLL.Category();
            return biz.GetAccessRating(CurrentUserInfo.CompanyID, _categoryID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID, admin);
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

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.Ratings biz = new DAL.Ratings();
            DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(),
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

            DAL.Ratings list = new DAL.Ratings();
            return list.GetPageListCount(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>());
        }

        #endregion 투표

        #region 임시저장

        /// <summary>
        /// 작성자 : 심재운
        /// 임시저장 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetMyTemplateList(string p)
        {
            SetDecryptString(p);

            string totalCount = GetDecryptParam("totalCount", "0");
            string pageSize = GetDecryptParam("pageSize", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            string searchTerm = GetDecryptParam("searchTerm", "");
            string searchTermValue = GetDecryptParam("searchTermValue", "");

            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");
            string order = GetDecryptParam("order", "");

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            #region 날짜

            string startDate = "";
            string endDate = "";
            string getDate = "";

            if (string.Compare(searchTerm, "week", true) == 0 || string.Compare(searchTerm, "month", true) == 0)
            {
                if (string.Compare(searchTerm, "week", true) == 0)
                    getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                else if (string.Compare(searchTerm, "month", true) == 0)
                    getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (string.Compare(searchTerm, "period", true) == 0)
            {
                startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            #endregion 날짜

            DAL.MyTemplates biz = new DAL.MyTemplates();
            DataTableReader dtr = biz.GetMyTemplatePageList(CurrentUserInfo.CompanyID, setPageingStr[0], setPageingStr[1],
                startDate, endDate, searchField, searchValue, order, CurrentUserInfo.UserID);

            ObjList listobj = new ObjList();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.MyTemplateInfo info = new BE.MyTemplateInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.MyTemplateID = dtr["MyTemplateID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString();
                        info.WriteUserID = CurrentUserInfo.UserID;
                        info.WriteUserName = CurrentUserInfo.Name;
                        info.Filecount = dtr["Filecount"].ToString().ToNumber<int>();
                        info.WriteDateStr = "";
                        info.BlogContent = dtr["MobileContent"].ToString();

                        if (info.BlogContent.Length > 200)
                            info.BlogContent = info.BlogContent.Substring(0, 195) + "..";

                        info.ScreenShot = dtr["ScreenShot"].ToString();
                        if (info.ScreenShot.ToNull() == "")
                        {
                            info.ScreenShot = this.BaseImgPath + "/Icon/ico_noimg2.gif";
                        }

                        DateTime dt;
                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                        {
                            info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        }

                        int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;
                        info.ROW = listNum;
                        info.Filecount = dtr["Filecount"].ToString().ToNumber<int>();
                        info.Title = dtr["Title"].ToString();
                        info.TitleNoHtml = dtr["Title"].ToString();

                        info.TitleShort = info.TitleNoHtml;
                        if (info.TitleNoHtml.Length > 15)
                            info.TitleShort = info.TitleNoHtml.Substring(0, 12) + "..";

                        info.Title = SetMyTitle(info.CategoryID, info.MyTemplateID, info.TitleShort);

                        info.Title = dtr["Title"].ToString();
                        info.Title = SetMyTitle(info.CategoryID, info.MyTemplateID, info.Title);

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 임시저장 총 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetMyTemplateCount(string p)
        {
            SetDecryptString(p);

            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert(p, AppSettingType.BOARD);

            string searchTerm = GetDecryptParam("searchTerm", "");
            string searchTermValue = GetDecryptParam("searchTermValue", "");
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");

            #region 날짜

            string startDate = "";
            string endDate = "";
            string getDate = "";

            if (string.Compare(searchTerm, "week", true) == 0 || string.Compare(searchTerm, "month", true) == 0)
            {
                if (string.Compare(searchTerm, "week", true) == 0)
                    getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                else if (string.Compare(searchTerm, "month", true) == 0)
                    getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (string.Compare(searchTerm, "period", true) == 0)
            {
                startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            #endregion 날짜

            int icnt = 0;
            //return icnt;
            DAL.MyTemplates biz = new DAL.MyTemplates();

            icnt = biz.GetMyTemplatePageCount(CurrentUserInfo.CompanyID, startDate, endDate, searchField, searchValue, CurrentUserInfo.UserID);

            return icnt;
        }

        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetMyAttachFileList(string p)
        {
            SetDecryptString(p);
            string categoryID = categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string screenShot = GetDecryptParam("screenShot", "");

            ObjList listobj = new ObjList();

            DAL.MyAttachFiles dal = new DAL.MyAttachFiles();
            List<BE.MyAttachFileInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>());

            string fileName = (screenShot.Length > 0) ? System.IO.Path.GetFileName(Server.MapPath(screenShot)).ToLower().Replace("thum_", "") : ""; //파일명
            string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

            foreach (BE.MyAttachFileInfo _attachInfo in attachInfo)
            {
                Zeospace.DeskPlus.Board.BE.MyAttachFileInfo info = new BE.MyAttachFileInfo();

                info.CompanyID = _attachInfo.CompanyID;
                info.CategoryID = _attachInfo.CategoryID;
                info.MyTemplateID = _attachInfo.MyTemplateID;
                info.AttachFileID = _attachInfo.AttachFileID;
                info.FilePath = _attachInfo.FilePath;
                info.FileName = _attachInfo.FileName;
                info.FileType = _attachInfo.FileType.Replace(".", "");
                info.FileLength = _attachInfo.FileLength;
                info.FileGUID = _attachInfo.FileGUID;
                info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                //스크린샷 파일과 동일 여부 찾기
                info.ScreenShot = (fileName == info.FileName.ToLower() || fileName == info.FileGUID.ToLower()) ? "1" : "0";

                string fileOk = "0";
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (_attachInfo.FileType.ToLower() == allowedExtensions[i])
                    {
                        fileOk = "1";
                        break;
                    }
                }

                info.ImageType = fileOk;

                listobj.Add(info);
            }

            return listobj;
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 임시글 첨부파일 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetMyAttachFileListMenu(string p)
        {
            int categoryID = 0;
            int listID = 0;

            SetDecryptString(p);

            categoryID = GetDecryptParam("categoryID", "0").ToNumber<Int32>();
            listID = GetDecryptParam("listID", "0").ToNumber<Int32>();

            string response = "0";

            DAL.MyAttachFiles dal = new DAL.MyAttachFiles();
            List<BE.MyAttachFileInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, categoryID, listID);

            string totalFileInfo = "";

            foreach (BE.MyAttachFileInfo _attachInfo in attachInfo)
            {
                if (_attachInfo.FileGUID != "")
                {
                    totalFileInfo += _attachInfo.FileName + ":" + _attachInfo.FilePath + _attachInfo.FileGUID + "|"; // Key < 파일경로파일명
                }
                else
                {
                    totalFileInfo += _attachInfo.FileName + ":" + _attachInfo.FilePath + _attachInfo.FileName + "|"; // Key < 파일경로파일명
                }
            }

            totalFileInfo = totalFileInfo.TrimEnd('|');

            if (totalFileInfo.Length > 0)
            {
                response = totalFileInfo.Replace("&", "&amp;");		// 첨부파일 정보 가져오기
            }

            return response;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 옵션설정에 맞게 옵션 스타일 적용하기
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_myTemplateID">The _list ID.</param>
        /// <param name="_title">The _title.</param>
        /// <returns></returns>
        private string SetMyTitle(int _categoryID, int _myTemplateID, string _title)
        {
            DAL.MyTemplateEtc dalEtc = new DAL.MyTemplateEtc();
            List<BE.MyTemplateEtcInfo> al = dalEtc.GetList(CurrentUserInfo.CompanyID, _categoryID, _myTemplateID);

            if (al.Count > 0)
            {
                foreach (BE.MyTemplateEtcInfo info in al)
                {
                    string[] etcStr = info.Etc2.Split(',');
                    StringBuilder sb = new StringBuilder();
                    if (etcStr[2].ToString() != "0") sb.Append("<b>");
                    if (etcStr[0].ToString() != "") sb.Append("<font color='" + etcStr[0].ToString() + "'>");
                    if (etcStr[1].ToString() != "") sb.Append("<font size='" + etcStr[1].ToString() + "'>");
                    sb.Append(_title);
                    if (etcStr[0].ToString() != "") sb.Append("</font>");
                    if (etcStr[1].ToString() != "") sb.Append("</font>");
                    if (etcStr[2].ToString() != "0") sb.Append("</b>");

                    _title = sb.ToString();
                }
            }

            return _title;
        }

        #endregion 임시저장

        #region 사용자 카테고리 정보

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 카테고리 사용자별 정보 가져오기(ListType:PageSize)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCategoryUser(string p)
        {
            SetDecryptString(p);

            int categoryID = GetDecryptParam("catID", "0").ToNumber<Int32>();
            string types = GetDecryptParam("types", "BL");//BL : 게시판 TL : 임시저장 IL : 중요표시

            DAL.CategoryUser dal = new DAL.CategoryUser();
            List<CategoryUserInfo> al = dal.GetList(CurrentUserInfo.CompanyID, categoryID,
                CurrentUserInfo.UserID, types);

            foreach (BE.CategoryUserInfo info in al)
            {
                return info.ListType + ":" + info.PageSize;
            }

            return "0:10";
        }

        #endregion 사용자 카테고리 정보

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
        public ObjList GetReadCount(string p)
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
            DataTableReader dtr = biz.GetListViewList(CurrentUserInfo.CompanyID, CategoryID, StartDate, EndDate, pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>() - 1, order);
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

                        #region 다국어 이름 설정

                        string pathname = this.GetLangName(
                            dtr["PathNameKo"].ToString(),
                            dtr["PathNameKo"].ToString(), dtr["PathNameEn"].ToString(), dtr["PathNameJa"].ToString(),
                            dtr["PathNameFr"].ToString(), dtr["PathNameZhCn"].ToString(), dtr["PathNameZhTw"].ToString(), PreLang);

                        #endregion 다국어 이름 설정

                        info.TitleName = pathname;
                        info.Title = dtr["TITLE"].ToString();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.WriteDate = dtr["WriteDate"].ToString();
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
        public int GetReadCountCount(string p)
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

            icnt = biz.GetListViewListCount(CurrentUserInfo.CompanyID, CategoryID, StartDate, EndDate);

            return icnt;
        }

        ///////////////////////

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

        #region 중요표시

        /// <summary>
        /// 중요표시 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetImpoListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string categoryID = GetDecryptParam("categoryID", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (string.Compare(searchTerm, "week", true) == 0 || string.Compare(searchTerm, "month", true) == 0)
                {
                    if (string.Compare(searchTerm, "week", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (string.Compare(searchTerm, "month", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (string.Compare(searchTerm, "period", true) == 0)
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                DAL.ImpoUser dal = new DAL.ImpoUser();
                return dal.GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, startDate, endDate,
                    searchField, searchValue);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetImpoListCount : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 중요표시 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetImpoPageList(string p)
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
                string order = GetDecryptParam("order", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (string.Compare(searchTerm, "week", true) == 0 || string.Compare(searchTerm, "month", true) == 0)
                {
                    if (string.Compare(searchTerm, "week", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (string.Compare(searchTerm, "month", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (string.Compare(searchTerm, "period", true) == 0)
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                DAL.ImpoUser biz = new DAL.ImpoUser();
                DataTableReader dtr = biz.GetPageList(
                    CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, startDate, endDate,
                    setPageingStr[0], setPageingStr[1],
                    searchField, searchValue, order);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Board.BE.ListInfo info = new BE.ListInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();

                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Depth = dtr["Depth"].ToString().ToNumber<int>();
                            info.Content = dtr["Content"].ToString();

                            info.MobileContent = dtr["MobileContent"].ToString();

                            info.BlogContent = dtr["MobileContent"].ToString();

                            if (info.BlogContent.Length > 200)
                                info.BlogContent = info.BlogContent.Substring(0, 195) + "..";

                            info.OwnerDeptID = dtr["OwnerDeptID"].ToString().ToNumber<int>();
                            info.OptionModify = dtr["OptionModify"].ToString();
                            info.OptionTop = dtr["OptionTop"].ToString();
                            info.OptionTopDay = dtr["OptionTopDay"].ToString().ToNumber<int>();

                            info.ReadCount = dtr["ReadCount"].ToString().ToNumber<int>();
                            //info.DeptName = dtr["DeptName"].ToString();
                            //info.TitleName = dtr["TitleName"].ToString();
                            //info.DutyName = dtr["DutyName"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.ImgCount = dtr["ImgCount"].ToString().ToNumber<int>();
                            info.HeaderID = dtr["HeaderID"].ToString().ToNumber<int>();
                            info.ImpoCnt = dtr["ImpoCnt"].ToString().ToNumber<int>();

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
                                #region 이름 다국어 설정

                                info.WriteUserName = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);

                                #endregion 이름 다국어 설정

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

                                #region 다국어 경로 다국어 설정

                                info.PathName = this.GetLangName(
                                    dtr["PathNameKo"].ToString(),
                                    dtr["PathNameKo"].ToString(), dtr["PathNameEn"].ToString(), dtr["PathNameJa"].ToString(),
                                    dtr["PathNameFr"].ToString(), dtr["PathNameZhCn"].ToString(), dtr["PathNameZhTw"].ToString(), PreLang);

                                #endregion 다국어 경로 다국어 설정

                                info.WriteUserName = GetWriteUserName(
                                    info.WriteUserName,
                                    info.DeptName,
                                    info.TitleName,
                                    info.DutyName);
                            }

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            info.ModifyUserID = dtr["ModifyUserID"].ToString().ToNumber<int>();
                            info.ModifyUserName = dtr["ModifyUserName"].ToString();

                            if (DateTime.TryParse(dtr["ModifyDate"].ToString(), out dt))
                                info.ModifyDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.ModifyDate = string.Empty;

                            info.PassWord = dtr["PassWord"].ToString();
                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            //info.ROW = (dtr["OptionTop"].ToString() == "1") ? "<img src='" + this.BaseImgPath + "/Lang/" + this.PreLang + "/not.gif' alt=''>" : listNum.ToString();

                            #region 다국어 카테고리 이름 설정

                            info.CA_Title = this.GetLangName(
                                dtr["CNameBase"].ToString(),
                                dtr["CNameKo"].ToString(), dtr["CNameEn"].ToString(), dtr["CNameJa"].ToString(),
                                dtr["CNameFr"].ToString(), dtr["CNameZhCn"].ToString(), dtr["CNameZhTw"].ToString(), PreLang);

                            #endregion 다국어 카테고리 이름 설정

                            info.Filecount = dtr["Filecount"].ToString().ToNumber<int>();
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();
                            info.TitleNoHtml = dtr["Title"].ToString();

                            info.TitleShort = info.TitleNoHtml;
                            if (info.TitleNoHtml.Length > 15)
                                info.TitleShort = info.TitleNoHtml.Substring(0, 12) + "..";

                            info.TitleShort = GetTitle(info, info.TitleShort, dtr["Etc2"].ToString());

                            info.Title = dtr["Title"].ToString();
                            info.Title = GetTitle(info, info.Title, dtr["Etc2"].ToString());

                            //조회하지 않으면 굵게 처리
                            if (dtr["ReadCheck"].ToString().ToNumber<int>() == 0) info.Title = "<b>" + info.Title + "</b>";

                            //말머리
                            info.Title = (info.HeaderName.Length > 0) ? info.HeaderName + " " + info.Title : info.Title;

                            info.ScreenShot = dtr["ScreenShot"].ToString();
                            if (info.ScreenShot.ToNull().Length == 0)
                            {
                                info.ScreenShot = this.BaseImgPath + "/Icon/ico_noimg2.gif";
                            }

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

        #endregion 중요표시

        #region 말머리

        /// <summary>
        /// 말머리 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetHeaderList(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

                DAL.Header dal = new DAL.Header();
                List<BE.HeaderInfo> al = dal.GetHeaderLists(CurrentUserInfo.CompanyID, categoryID);
                ObjList listobj = new ObjList();

                foreach (var info in al)
                {
                    BE.HeaderInfo be = new HeaderInfo();
                    be.CompanyID = CurrentUserInfo.CompanyID;
                    be.HeaderID = info.HeaderID;
                    be.CategoryID = info.CategoryID;
                    be.Orders = info.Orders;

                    be.NameBase = this.GetLangName(
                            info.NameBase, info.NameKo, info.NameEn, info.NameJa,
                            info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);

                    be.UseActive = info.UseActive;
                    be.WriteUserID = info.WriteUserID;
                    be.WriteDate = info.WriteDate;

                    listobj.Add(be);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetHeaderList : " + ex.Message);
                throw;
            }
        }

        #endregion 말머리

        /// <summary>
        /// 말머리 리스트 가져오기
        /// json 으로 데이터 반환하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)]
        public string GetHeaderListS(string p)
        {
            try
            {
                int categoryID = 4;

                DAL.Header dal = new DAL.Header();
                List<BE.HeaderInfo> listobj = dal.GetHeaderLists(CurrentUserInfo.CompanyID, categoryID);

                foreach (var info in listobj)
                {
                    info.ModifyDate = System.DateTime.MinValue.ToUniversalTime();
                }

                return JsonConvert.SerializeObject(listobj);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetHeaderList : " + ex.Message);
                throw;
            }
        }

        #endregion GET()
    }

}