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
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSPMS
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
        /// 작성자 : 심재운
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                string totalCount = GetDecryptParam("totalItemCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string currentPage = GetDecryptParam("currentPage", "0");

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");


                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), currentPage.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');


                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (searchTerm == "week" || searchTerm == "month")
                {
                    if (searchTerm == "week")
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                BE.BoardInfo listInfo = new Zeospace.DeskPlus.PMS.BE.BoardInfo();
                DAL.Board list = new Zeospace.DeskPlus.PMS.DAL.Board();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.ProjectID = projectID;
                listInfo.MenuID = menuID;
                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.PageSize = setPageingStr[0].ToNumber<int>();
                listInfo.PageSizePageNo = setPageingStr[1].ToNumber<int>();
                listInfo.SearchStartDate = startDate;
                listInfo.SearchEndDate = endDate;

                DataTableReader dtr = list.GetPageList(listInfo);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.BoardInfo info = new BoardInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();

                            info.ProjectID = dtr["ProjectID"].ToString().ToNumber<int>();
                            info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                            info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();

                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Depth = dtr["Depth"].ToString().ToNumber<int>();

                            //작성자 아이디
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            //제목
                            info.Title = dtr["Title"].ToString();

                            info.Filecount = dtr["Filecount"].ToString().ToNumber<int>();
                            info.ReplyCount = dtr["ShortReplyCount"].ToString().ToNumber<int>();
                            info.ReadCount = dtr["ReadCount"].ToString().ToNumber<int>();

                            #region 개설자 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptBase = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
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

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDateStr = string.Empty;

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.PMS, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion

                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;
                            info.Row = listNum.ToString();

                            #region 답글일 경우 제목에 화살표 추가

                            string space = "";
                            for (int i = 0; i < info.Depth; i++) space += "&nbsp;&nbsp;";

                            if (info.Depth > 0) info.Title = "<img src='" + this.BaseImgPath + "/Icon/ico_arrow.gif' title='' />" + " " + info.Title;
                            if (space.Length > 0) info.Title = space + info.Title;

                            #endregion

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
        /// 프로젝트 카운트 가져오기
        /// 작성자 : 심재운
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

                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int icnt = 0;

                #region 날짜

                string startDate = "";
                string endDate = "";
                string getDate = "";

                if (searchTerm == "week" || searchTerm == "month")
                {
                    if (searchTerm == "week")
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                BE.BoardInfo listInfo = new Zeospace.DeskPlus.PMS.BE.BoardInfo();
                DAL.Board list = new Zeospace.DeskPlus.PMS.DAL.Board();

                listInfo.CompanyID = CurrentUserInfo.CompanyID;
                listInfo.ProjectID = projectID;
                listInfo.MenuID = menuID;
                listInfo.SearchField = searchField;
                listInfo.SearchValue = searchValue;
                listInfo.SearchStartDate = startDate;
                listInfo.SearchEndDate = endDate;

                icnt = list.GetPageCount(listInfo);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일   : 2012-05-23
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

                int projectID = 0;
                int menuID = 0;
                int boardID = 0;

                SetDecryptString(p);

                projectID = GetDecryptParam("projectID", "0").ToNumber<Int32>();
                menuID = GetDecryptParam("menuID", "0").ToNumber<Int32>();
                boardID = GetDecryptParam("boardID", "0").ToNumber<Int32>();

                string response = "0";

                DAL.BoardAttach dal = new DAL.BoardAttach();
                List<BE.BoardAttachInfo> attachInfo = dal.GetAttachFile(CurrentUserInfo.CompanyID, projectID, menuID, boardID);

                string totalFileInfo = "";
                string encFilePath = "";

                foreach (BE.BoardAttachInfo _attachInfo in attachInfo)
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
        #endregion        
    }
}
