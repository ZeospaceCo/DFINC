using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
{
    /// <summary>
    /// ListInterested의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListInterested : Base.BaseWSBoard
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

        /// <summary>
        /// 작성목적   : 작성자 이름설정하기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-24
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
        /// 작성목적   : 게시판 제목 스타일 입히기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-24
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
        /// 작성목적   : 옵션설정에 맞게 옵션 스타일 적용하기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-24
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

        #endregion 리스트

        #region 관심대상자

        /// <summary>
        /// 작성목적   : 관심대상자 카운트 가져오기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-24
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetInterestedListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string categoryID = GetDecryptParam("categoryID", "0");
                string interestedUserID = GetDecryptParam("interestedUserID", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (string.Compare(searchTerm, "6M", true) == 0 || string.Compare(searchTerm, "1Y", true) == 0 || string.Compare(searchTerm, "2Y", true) == 0 || string.Compare(searchTerm, "3Y", true) == 0)
                {
                    if (string.Compare(searchTerm, "6M", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("6M", PreLangReal);
                    else if (string.Compare(searchTerm, "1Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("1Y", PreLangReal);
                    else if (string.Compare(searchTerm, "2Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("2Y", PreLangReal);
                    else if (string.Compare(searchTerm, "3Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("3Y", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (string.Compare(searchTerm, "PE", true) == 0)
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                DAL.InterestedUser dal = new DAL.InterestedUser();
                return dal.GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID, interestedUserID.ToNumber<int>(), startDate, endDate,
                    searchField, searchValue);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetImpoListCount : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성목적   : 관심대상자 리스트 가져오기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-24
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)]
        public string GetInterestedPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string categoryID = GetDecryptParam("categoryID", "0");
                string interestedUserID = GetDecryptParam("interestedUserID", "0");
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

                if (string.Compare(searchTerm, "6M", true) == 0 || string.Compare(searchTerm, "1Y", true) == 0 || string.Compare(searchTerm, "2Y", true) == 0 || string.Compare(searchTerm, "3Y", true) == 0)
                {
                    if (string.Compare(searchTerm, "6M", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("6M", PreLangReal);
                    else if (string.Compare(searchTerm, "1Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("1Y", PreLangReal);
                    else if (string.Compare(searchTerm, "2Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("2Y", PreLangReal);
                    else if (string.Compare(searchTerm, "3Y", true) == 0)
                        getDate = DateHelpers.GetCultureStartEndDatePeriod("3Y", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (string.Compare(searchTerm, "PE", true) == 0)
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                DAL.InterestedUser biz = new DAL.InterestedUser();
                DataTableReader dtr = biz.GetPageList(
                    CurrentUserInfo.CompanyID,
                    CurrentUserInfo.DeptID,
                    CurrentUserInfo.UserID, interestedUserID.ToNumber<int>(), startDate, endDate,
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

                            info.BlogContent = dtr["MobileContent"].ToString();

                            if (info.BlogContent.Length > 200)
                                info.BlogContent = info.BlogContent.Substring(0, 195) + "..";

                            info.OwnerDeptID = dtr["OwnerDeptID"].ToString().ToNumber<int>();
                            info.OptionModify = dtr["OptionModify"].ToString();
                            info.OptionTop = dtr["OptionTop"].ToString();
                            info.OptionTopDay = dtr["OptionTopDay"].ToString().ToNumber<int>();

                            info.ReadCount = dtr["ReadCount"].ToString().ToNumber<int>();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.ImgCount = dtr["ImgCount"].ToString().ToNumber<int>();
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
                            info.ROW = (dtr["OptionTop"].ToString() == "1") ? "<span class=\"badge notice badge-warning\"><span class=\"s_notice\"></span></span>" : listNum.ToString();

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

                            if (dtr["ReadCheck"].ToString().ToNumber<int>() == 0) info.Title = "<b>" + info.Title + "</b>";

                            info.Title = (info.HeaderName.Length > 0) ? info.HeaderName + " " + info.Title : info.Title;

                            #region 스크린샷

                            info.ScreenShot = dtr["ScreenShot"].ToString();
                            if (info.ScreenShot.ToNull().Length == 0) info.ScreenShot = this.BaseImgPath + "/Icon/ico_noimg2.gif";
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", info.ScreenShot);
                            paraDic.Add("thumnail", "Y");
                            paraDic.Add("thumWidth", "94");
                            paraDic.Add("thumHeight", "124");
                            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                            info.ScreenShot = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                            #endregion 스크린샷

                            #region 날짜 비교(New Img)

                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";

                            #endregion 날짜 비교(New Img)

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

        #endregion 관심대상자

        #region 첨부파일

        /// <summary>
        /// 작성목적   : 첨부파일 정보 가져오기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-24
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
                string encFilePath = "";

                foreach (BE.AttachFileInfo _attachInfo in attachInfo)
                {
                    encFilePath = (_attachInfo.FileGUID != "") ? _attachInfo.FilePath + _attachInfo.FileGUID : _attachInfo.FilePath + _attachInfo.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", encFilePath);
                    paraDic.Add("fileName", _attachInfo.FileName);
                    encFilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    totalFileInfo += _attachInfo.FileName + ":" + encFilePath + "|"; // Key < 파일경로파일명
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

        #region MOD()

        #region 카테고리별 사용자정보

        /// <summary>
        /// 작성목적   : 카테고리별 사용자정보 설정하기
        /// 작성자     : 이지원
        /// 최초작성일 : 2017-03-24
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCategoryUser(string p)
        {
            try
            {
                SetDecryptString(p);

                string categoryID = GetDecryptParam("catID", "0");
                string listType = GetDecryptParam("listType", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "IP"); //BL : 게시판 TL : 임시저장 IL : 중요표시

                BE.CategoryUserInfo info = new CategoryUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CategoryID = categoryID.ToNumber<int>();
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
                info.ListType = listType;
                info.Types = types;

                DAL.CategoryUser dal = new DAL.CategoryUser();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 카테고리별 사용자정보

        #endregion MOD()

        #region DEL()
        #endregion DEL()
    }
}
