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

namespace Zeospace.DeskPlus.Board.Web.Ws.Temp
{
    /// <summary>
    /// GalleryList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class GalleryList : Base.BaseWSBoard
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

        #region 임시저정

        /// <summary>
        /// 작성자 : 심재운
        /// 임시저장 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)]
        public string GetMyTemplateList(string p)
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
                        //info.Content = dtr["Content"].ToString();

                        //info.BlogContent = dtr["MobileContent"].ToString();

                        //if (info.BlogContent.Length > 200)
                        //    info.BlogContent = info.BlogContent.Substring(0, 195) + "..";

                        info.WriteUserID = CurrentUserInfo.UserID;
                        info.WriteUserName = CurrentUserInfo.Name;
                        info.Filecount = dtr["Filecount"].ToString().ToNumber<int>();
                        info.WriteDateStr = "";

                        #region 스크린샷

                        info.ScreenShot = dtr["ScreenShot"].ToString();
                        if (info.ScreenShot.ToNull().Length == 0) info.ScreenShot = this.BaseImgPath + "/Icon/ico_noimg2.gif";
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", info.ScreenShot);
                        paraDic.Add("thumnail", "Y");
                        paraDic.Add("thumWidth", "160");
                        paraDic.Add("thumHeight", "160");
                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                        info.ScreenShot = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                        #endregion 스크린샷

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

            #endregion 날짜

            int icnt = 0;
            //return icnt;
            DAL.MyTemplates biz = new DAL.MyTemplates();

            icnt = biz.GetMyTemplatePageCount(CurrentUserInfo.CompanyID, startDate, endDate, searchField, searchValue, CurrentUserInfo.UserID);

            return icnt;
        }

        #endregion 임시저정

        #region MOD()

        #region 카테고리별 사용자정보

        /// <summary>
        /// 용도 : 카테고리별 사용자정보 설정하기
        /// 작성자 : 심재운
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
                string types = GetDecryptParam("types", "BL"); //BL : 게시판 TL : 임시저장 IL : 중요표시

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
    }
}