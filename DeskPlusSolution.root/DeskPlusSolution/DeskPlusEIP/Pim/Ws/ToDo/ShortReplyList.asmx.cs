using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;
using ObjList = System.Collections.Generic.List<System.Object>;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Pim.Web.Ws.ToDo
{
    /// <summary>
    /// ShortReplyList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyList : Base.BaseWSPim
    {
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
        /// 용도 : 일정 페이징 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            string toDoID = string.Empty;
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;
            string searchTerm = string.Empty;
            string searchTermValue = string.Empty;
            string searchField = string.Empty;
            string searchValue = string.Empty;
            string order = string.Empty;

            SetDecryptString(p);
            toDoID = GetDecryptParam("toDoID", "0");
            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            searchTerm = GetDecryptParam("searchTerm", "");
            searchTermValue = GetDecryptParam("searchTermValue", "");

            searchField = GetDecryptParam("searchField", "");
            searchValue = GetDecryptParam("searchValue", "");
            order = GetDecryptParam("order", "");

            ObjList listobj = new ObjList();

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
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

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            DAL.ToDoReply biz = new DAL.ToDoReply();
            DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, toDoID.ToNumber<int>(),
                setPageingStr[0], setPageingStr[1],startDate, endDate,
                    searchField, searchValue, order, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";

                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Pim.BE.ToDoReplyInfo info = new BE.ToDoReplyInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ToDoID = dtr["ToDoID"].ToString().ToNumber<int>();
                        info.ReplyID = dtr["ReplyID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.TUserID = dtr["TUserID"].ToString().ToNumber<int>();
                        info.IsSecurity = dtr["IsSecurity"].ToString();

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);


                        info.UserName = name;
                        info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        info.View = "N";

                        // 원본글 작성자와 댓글러가 같은지 
                        if (CurrentUserInfo.UserID == info.TUserID)
                        {
                            info.View = "Y";
                        }
                        else if (CurrentUserInfo.UserID == info.UserID && info.IsSecurity == "1")
                        {
                            info.View = "Y";
                        }
                        else if (info.IsSecurity == "0")
                        {
                            info.View = "Y";
                        }

                        #region 직급
                        info.TitleName = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(), PreLang);
                        #endregion

                        #region 사진 이미지 설정
                        string photoName = dtr["PhotoImage"].ToString();
                        string BaseImgPath = "";
                        string imgTag = "";
                        string encParams = "";
                        bool response = false;

                        Dictionary<string, string> paraDic = null;

                        if (!string.IsNullOrEmpty(photoName) && photoName.Length > 0)
                        {
                            BaseImgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + photoName;
                            BaseImgPath = string.Format(BaseImgPath, CurrentUserInfo.CompanyID, info.UserID);

                            if (System.IO.File.Exists(Server.MapPath(BaseImgPath))) response = true;
                        }

                        if (response == false) BaseImgPath = baseImageDirectory + "C/Img/Icon/ico_noimg.gif";

                        if (System.IO.File.Exists(Server.MapPath(BaseImgPath)))
                        {
                            #region 스크린샷
                            paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", BaseImgPath);
                            paraDic.Add("thumnail", "Y");
                            paraDic.Add("thumWidth", "52");
                            paraDic.Add("thumHeight", "52");
                            encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                            BaseImgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            #endregion

                            imgTag = "<img src='{0}' width='52' />";
                            imgTag = string.Format(imgTag, BaseImgPath);
                        }

                        info.PhotoImage = imgTag;

                        #endregion


                        info.InputDate = Convert.ToDateTime(dtr["InputDate"]).ToShortDateString() + " " + Convert.ToDateTime(dtr["InputDate"]).ToShortTimeString();//등록일

                                               

                        #region 날짜 비교(New Img)
                        DateTime writeDate = Convert.ToDateTime(dtr["InputDate"].ToString());
                        DateTime todayDate = System.DateTime.Now;
                        TimeSpan gap = todayDate.Subtract(writeDate);
                        
                        info.ShowNew = (gap.Days <= 0) ? "Y" : "N";
                        #endregion


                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 일정 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCount(string p)
        {
            SetDecryptString(p);

            string toDoID = GetDecryptParam("toDoID", "0");
            string searchTerm = GetDecryptParam("searchTerm", "");
            string searchTermValue = GetDecryptParam("searchTermValue", "");
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");

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

            DAL.ToDoReply biz = new DAL.ToDoReply();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, toDoID.ToNumber<int>(), 
                startDate, endDate, searchField, searchValue, CurrentUserInfo.UserID);
        }
    }
}
