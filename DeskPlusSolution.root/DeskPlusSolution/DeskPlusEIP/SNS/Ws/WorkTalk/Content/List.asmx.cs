using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.SNS.Web.Ws.WorkTalk.Content
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSWorkTalk
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
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-10
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.SNS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.SNS.Web.Ws.WorkTalk.Content.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion


            try
            {
                SetDecryptString(p);
                string menuID = GetDecryptParam("menuID", "2");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string order = GetDecryptParam("order", "");

                string preLang = CurrentUserInfo.PreferredLanguage;

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');
                string titleOpenType = String.Empty;
                string styleOpenType = String.Empty;

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

                DAL.WorkTalk dal = new DAL.WorkTalk();

                DataTableReader dtr = null;

                if (menuID == "2")
                {
                    //수신톡
                    dtr = dal.GetRecvPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                        searchField, searchValue, startDate, endDate, order);
                }
                else if (menuID == "3")
                {
                    //발신톡
                    dtr = dal.GetSendPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                        searchField, searchValue, startDate, endDate, order);
                }
                else if (menuID == "4")
                {
                    //결재톡
                    dtr = dal.GetAppPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                        searchField, searchValue, startDate, endDate, order);
                }
                else
                {
                    //수신톡
                    dtr = dal.GetRecvPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                        searchField, searchValue, startDate, endDate, order);
                } 

                List<object> listobj = new List<object>();
                
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.WorkTalkInfo info = new BE.WorkTalkInfo();

                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>(); //원본글작성자
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();//댓글작성자
                            info.Gubun = dtr["Gubun"].ToString();
                            info.GubunTitle = "";
                            info.OptionTop = dtr["OptionTop"].ToString().ToNumber<int>();
                            info.AccountTag = dtr["AccountTag"].ToString();


                            switch (info.Gubun)
                            {
                                case "PIM_SCHE":
                                    switch (info.AccountTag)
                                    {
                                        case "0":
                                            titleOpenType = this.GetResource("pim1");
                                            //styleOpenType = "<span class='m_pim_private'></span>&nbsp;";
                                            break;
                                        case "1":
                                            titleOpenType = this.GetResource("pim3");
                                            //styleOpenType = "<span class='m_pim_share'></span>&nbsp;";
                                            break;
                                        case "2":
                                            titleOpenType = this.GetResource("pim4");
                                            //styleOpenType = "<span class='m_pim_dept'></span>&nbsp;";
                                            break;
                                        case "3":
                                            titleOpenType = this.GetResource("pim2");
                                            //styleOpenType = "<span class='m_pim_tot'></span>&nbsp;";
                                            break;
                                    }
                                    info.GubunTitle = titleOpenType; //this.GetResource("Schedule");
                                    //info.StyleType = styleOpenType;
                                    break;
                                case "PIM_TODO":
                                    switch (info.AccountTag)
                                    {
                                        case "0":
                                            titleOpenType = this.GetResource("TodoShare1");
                                            //styleOpenType = "<span class='m_pim_private'></span>&nbsp;";
                                            break;
                                        case "1":
                                            titleOpenType = this.GetResource("TodoShare3");
                                            //styleOpenType = "<span class='m_pim_share'></span>&nbsp;";
                                            break;
                                        case "2":
                                            titleOpenType = this.GetResource("TodoShare4");
                                            //styleOpenType = "<span class='m_pim_dept'></span>&nbsp;";
                                            break;
                                        case "3":
                                            titleOpenType = this.GetResource("TodoShare2");
                                            //styleOpenType = "<span class='m_pim_tot'></span>&nbsp;";
                                            break;
                                    }
                                    info.GubunTitle = titleOpenType; //this.GetResource("Schedule");
                                    //info.StyleType = styleOpenType;
                                    break;
                                case "Board":
                                    info.GubunTitle = this.GetResource("Board");
                                    if (info.OptionTop == 1) info.GubunTitle = this.GetResource("Notice");//공지글
                                    break;
                                case "APP":
                                    switch (dtr["Section"].ToString())
                                    {
                                        //구분(1:상신.결재/2:첨언추가,3:전달, 4:업무톡)

                                        case "1":
                                            titleOpenType = this.GetResource("ShortReplySec1");    //결재 -> 첨언
                                            break;
                                        case "2":
                                            titleOpenType = this.GetResource("ShortReplySec2");    //의견추가 -> 추가첨언
                                            break;
                                        case "3":
                                            titleOpenType = this.GetResource("ShortReplySec3");    //전달
                                            break;
                                        case "4":
                                            titleOpenType = this.GetResource("ShortReplySec4");    //업무톡 -> 댓글
                                            break;
                                        default:
                                            titleOpenType = this.GetResource("ShortReplySec");     //기타
                                            break;
                                    }
                                    info.GubunTitle = titleOpenType;

                                    if (dtr["SectionCount"].ToString().ToNumber<int>() > 1)
                                    {
                                        info.SectionCount = "+";
                                    }
                                    else
                                    {
                                        info.SectionCount = "";
                                    }

                                    switch (dtr["ApprovalState"].ToString())
                                    {
                                        //D : 보류, W: 대기, I: 진행, R: 반려, C: 완료
                                        case "D":
                                            //styleOpenType = "<span class='m_a_d3'></span>&nbsp;";
                                            info.StateStr = this.GetResource("AppReplySec1");    
                                            break;
                                        case "W":
                                            //styleOpenType = "<span class='m_a_r3'></span>&nbsp;";
                                            info.StateStr = this.GetResource("AppReplySec2");    
                                            break;
                                        case "I":
                                            //styleOpenType = "<span class='m_a_i3'></span>&nbsp;";
                                            info.StateStr = this.GetResource("AppReplySec3");    
                                            break;
                                        case "R":
                                            //styleOpenType = "<span class='m_a_re3'></span>&nbsp;";
                                            info.StateStr = this.GetResource("AppReplySec4");    
                                            break;
                                        case "C":
                                            //styleOpenType = "<span class='m_a_e3'></span>&nbsp;";
                                            info.StateStr = this.GetResource("AppReplySec5");    
                                            break;
                                        default:
                                            //styleOpenType = "<span class='m_a_i3'></span>&nbsp;";
                                            info.StateStr = this.GetResource("AppReplySec3");     
                                            break;
                                    }

                                    //info.StyleType = styleOpenType;
                                    break;                              

                            }

                            info.Title = dtr["Title"].ToString();
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();                            
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();

                            info.ReplyUserCount = dtr["ReplyUserCount"].ToString().ToNumber<int>();

                            if (info.ReplyUserCount > 1)
                            {
                                info.ReplyUserStr = "+";
                            }
                            else
                            {
                                info.ReplyUserStr = "";
                            }                           


                            #region 이름 다국어 설정
                            //댓글작성자 이름
                            info.Name = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 다국어 경로 다국어 설정
                            info.PathName = this.GetLangName(
                                dtr["PathNameKo"].ToString(),
                                dtr["PathNameKo"].ToString(), dtr["PathNameEn"].ToString(), dtr["PathNameJa"].ToString(),
                                dtr["PathNameFr"].ToString(), dtr["PathNameZhCn"].ToString(), dtr["PathNameZhTw"].ToString(), PreLang);
                            #endregion

                            if (info.PathName.Length > 0) info.PathName = info.PathName.Substring(1, info.PathName.Length - 1);

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.RegDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.RegDate = string.Empty;


                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            //하루만
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N";
                            #endregion


                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();

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
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-16        
        /// 작성목적 : 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string menuID = GetDecryptParam("menuID", "2");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");

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

                int icnt = 0;

                DAL.WorkTalk dal = new DAL.WorkTalk();

                if (menuID == "2")
                {
                    //수신톡
                    icnt = dal.GetRecvPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchField, searchValue, startDate, endDate);
                }
                else if (menuID == "3")
                {
                    //발신톡
                    icnt = dal.GetSendPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchField, searchValue, startDate, endDate);
                }
                else if (menuID == "4")
                {
                    //결재톡
                    icnt = dal.GetAppPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchField, searchValue, startDate, endDate);
                }
                else
                {
                    //수신톡
                    icnt = dal.GetRecvPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchField, searchValue, startDate, endDate);
                }

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-14
        /// 작성목적 : 폴더별 사용자 정보 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModFolderUser(string p)
        {
            try
            {
                SetDecryptString(p);

                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "R");

                BE.FolderUserInfo info = new BE.FolderUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.Types = types;
                info.PageSize = pageSize.ToNumber<int>();                

                DAL.FolderUser dal = new DAL.FolderUser();
                dal.AddMod(info);

                return true;
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
