using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Portlet.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.Coop
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSPortlet
    {
        #region 변수
        private int PageSize;
        private int PageSizePageNo;
        private int TotalCount;
        private string SearchStatus;
        private string SearchField;
        private string SearchValue;
        
        #endregion        

        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <returns></returns>
        private string SetPaging()
        {
            //페이지 총 개수

            int recsInLastPage = TotalCount % PageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = PageSize;

            int sizeOfPage = PageSize;
            int pageIndex = PageSizePageNo - 1;
            int pageCount = Convert.ToInt32(((TotalCount - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = PageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 말머리
        /// </summary>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        private string GetHeaderTitle(string _gubun)
        {
            string resourceNm = "";

            if (!this.resMng.Equals(null))
            {
                switch (_gubun.ToUpper())
                {
                    case "COOP":
                        resourceNm = resMng.GetString("COOP");//협업
                        break;
                    case "RM":
                        resourceNm = resMng.GetString("ResourceMng");//자원
                        break;
                    case "REPORT":
                        resourceNm = resMng.GetString("Report");//일지
                        break;
                }
            }

            return resourceNm;
        }

        /// <summary>
        /// 상태값
        /// 1:대기;2:접수;3:처리중;4:완료;5:반려
        /// </summary>
        /// <param name="_statusLast">The _status last.</param>
        /// <returns></returns>
        private string GetStatusName(string _statusLast)
        {
            

            string resourceNm = "";
            if (!this.resMng.Equals(null))
            {
                //1:대기;2:접수;3:처리중;4:완료;5:반려
                switch (_statusLast)
                {
                    case "1":
                        resourceNm = resMng.GetString("COOPStandby");
                        break;
                    case "2":
                        resourceNm = resMng.GetString("COOPAccept");
                        break;
                    case "3":
                        resourceNm = resMng.GetString("COOPProcess");
                        break;
                    case "4":
                        resourceNm = resMng.GetString("COOPComplete");
                        break;
                    case "5":
                        resourceNm = resMng.GetString("COOPReject");
                        break;
                }
            }

            return resourceNm;
        }        

        /// <summary>
        /// 요청업무
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<RequestEntityWS> GetRequestList(string p)
        {
            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Coop.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            //총카운트 제한하기
            int totCnt = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TabCoopMain", "PORTLET", PreLang).ToNumber<int>();
            if (totCnt < TotalCount) TotalCount = totCnt;

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');                        

            try
            {
                var list = new List<RequestEntityWS>();

                var session = Session["Coop_REQESTList"];//요청
                //전체이며, 검색어가 없으면 = 전체용이다.
                if (session != null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1)
                {
                    list = (List<RequestEntityWS>)Session["Coop_REQESTList"];
                    return list;
                }

                DAL.MyCoop dal = new DAL.MyCoop();                
                DataTableReader dtr = dal.GetRequestList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], SearchStatus, SearchField, SearchValue);
                

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            var info = new RequestEntityWS();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.Gubun = dtr["Gubun"].ToString().ToNull();
                            info.HeaderTitle = GetHeaderTitle(info.Gubun);
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();                            
                            
                            info.StatusLastName = GetStatusName(dtr["StatusLast"].ToString());
                            info.StatusLast = dtr["StatusLast"].ToString();
                            if (info.StatusLast == "5") info.StatusLast = "4";//반려는 완료 막대그래프와 동일하게 처리함

                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.TitleBase = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.RequestDate = dtr["RequestDate"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            //자원 ID
                            info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                            //자원 CategoryID
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            if (info.Gubun == "REPORT")
                            {
                                if (info.Title.IndexOf('~') != -1)
                                {
                                    var startDate = DateHelpers.GetCultureDateChar8(info.Title.Split('~')[0], CurrentUserInfo.PreferredLanguage);
                                    var endDate = DateHelpers.GetCultureDateChar8(info.Title.Split('~')[1], CurrentUserInfo.PreferredLanguage);

                                    info.Title = startDate + "~" + endDate + " " + info.NameBase + " " + info.TitleBase + " " + GetResource("MyCoopConfirmRequest");
                                }
                            }

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion

                            list.Add(info);
                        }
                    }
                }

                //전체이며, 검색어가 없으면 = 전체용이다. 세션값이 없으면 담는다.
                if (session == null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1) Session["Coop_REQESTList"] = list;

                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetRequestList : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 수신업무
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<TOEntityWS> GetTOList(string p)
        {
            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Coop.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            //총카운트 제한하기
            int totCnt = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TabCoopMain", "PORTLET", PreLang).ToNumber<int>();
            if (totCnt < TotalCount) TotalCount = totCnt;

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');

            try
            {
                var list = new List<TOEntityWS>();

                var session = Session["Coop_TOList"];
                //전체이며, 검색어가 없으면 = 전체용이다.
                if (session != null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1)
                {
                    list = (List<TOEntityWS>)Session["Coop_TOList"];
                    return list;
                }


                DAL.MyCoop dal = new DAL.MyCoop();
                DataTableReader dtr = dal.GetTOList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], SearchStatus, SearchField, SearchValue);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            var info = new TOEntityWS();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.Gubun = dtr["Gubun"].ToString().ToNull();
                            info.HeaderTitle = GetHeaderTitle(info.Gubun);
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();

                            info.StatusLastName = GetStatusName(dtr["StatusLast"].ToString());
                            info.StatusLast = dtr["StatusLast"].ToString();
                            if (info.StatusLast == "5") info.StatusLast = "4";//반려는 완료 막대그래프와 동일하게 처리함

                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.TitleBase = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.RequestDate = dtr["RequestDate"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            //자원 ID
                            info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                            //자원 CategoryID
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion

                            if (info.Gubun == "REPORT")
                            {
                                if (info.Title.IndexOf('~') != -1)
                                {
                                    var startDate = DateHelpers.GetCultureDateChar8(info.Title.Split('~')[0], CurrentUserInfo.PreferredLanguage);
                                    var endDate = DateHelpers.GetCultureDateChar8(info.Title.Split('~')[1], CurrentUserInfo.PreferredLanguage);

                                    info.Title = startDate + "~" + endDate + " " + info.NameBase + " " + info.TitleBase + " " + GetResource("MyCoopConfirmRequest");
                                }
                            }

                            list.Add(info);
                        }
                    }
                }

                //전체이며, 검색어가 없으면 = 전체용이다. 세션값이 없으면 담는다.
                if (session == null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1) Session["Coop_TOList"] = list;
                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTOList : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 참조업무
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<CCEntityWS> GetCCList(string p)
        {
            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Coop.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            //총카운트 제한하기
            int totCnt = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TabCoopMain", "PORTLET", PreLang).ToNumber<int>();
            if (totCnt < TotalCount) TotalCount = totCnt;

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');

            try
            {
                var list = new List<CCEntityWS>();

                var session = Session["Coop_CCList"];
                //전체이며, 검색어가 없으면 = 전체용이다.
                if (session != null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1)
                {
                    list = (List<CCEntityWS>)Session["Coop_CCList"];
                    return list;
                }

                DAL.MyCoop dal = new DAL.MyCoop();
                DataTableReader dtr = dal.GetCCList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], SearchStatus, SearchField, SearchValue);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            var info = new CCEntityWS();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.Gubun = dtr["Gubun"].ToString().ToNull();
                            info.HeaderTitle = GetHeaderTitle(info.Gubun);
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();

                            info.StatusLastName = GetStatusName(dtr["StatusLast"].ToString());
                            info.StatusLast = dtr["StatusLast"].ToString();
                            if (info.StatusLast == "5") info.StatusLast = "4";//반려는 완료 막대그래프와 동일하게 처리함

                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.TitleBase = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.RequestDate = dtr["RequestDate"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion

                            if (info.Gubun == "REPORT")
                            {
                                if (info.Title.IndexOf('~') != -1)
                                {
                                    var startDate = DateHelpers.GetCultureDateChar8(info.Title.Split('~')[0], CurrentUserInfo.PreferredLanguage);
                                    var endDate = DateHelpers.GetCultureDateChar8(info.Title.Split('~')[1], CurrentUserInfo.PreferredLanguage);

                                    info.Title = startDate + "~" + endDate + " " + info.NameBase + " " + info.TitleBase + " " + GetResource("MyCoopConfirmRequest");
                                }
                            }

                            list.Add(info);
                        }
                    }
                }

                //전체이며, 검색어가 없으면 = 전체용이다. 세션값이 없으면 담는다.
                if (session == null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1) Session["Coop_CCList"] = list;
                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetCCList : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 전달업무
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<ForwardEntityWS> GetForwardList(string p)
        {
            
            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Coop.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            //총카운트 제한하기
            int totCnt = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TabCoopMain", "PORTLET", PreLang).ToNumber<int>();
            if (totCnt < TotalCount) TotalCount = totCnt;

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');

            try
            {
                var list = new List<ForwardEntityWS>();

                var session = Session["Coop_FORWARDList"];
                //전체이며, 검색어가 없으면 = 전체용이다.
                if (session != null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1)
                {
                    list = (List<ForwardEntityWS>)Session["Coop_FORWARDList"];
                    return list;
                }


                DAL.MyCoop dal = new DAL.MyCoop();
                DataTableReader dtr = dal.GetForwardList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], SearchStatus, SearchField, SearchValue);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            var info = new ForwardEntityWS();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.Gubun = dtr["Gubun"].ToString().ToNull();
                            info.HeaderTitle = GetHeaderTitle(info.Gubun);
                            info.ForwardListID = dtr["ForwardListID"].ToString().ToNumber<int>();
                            info.ForwardGroupID = dtr["ForwardGroupID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();

                            info.StatusLastName = GetStatusName(dtr["StatusLast"].ToString());
                            info.StatusLast = dtr["StatusLast"].ToString();
                            if (info.StatusLast == "5") info.StatusLast = "4";//반려는 완료 막대그래프와 동일하게 처리함

                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.TitleBase = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.RequestDate = dtr["RequestDate"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            #endregion

                            if (info.Gubun == "REPORT")
                            {
                                if (info.Title.IndexOf('~') != -1)
                                {
                                    var startDate = DateHelpers.GetCultureDateChar8(info.Title.Split('~')[0], CurrentUserInfo.PreferredLanguage);
                                    var endDate = DateHelpers.GetCultureDateChar8(info.Title.Split('~')[1], CurrentUserInfo.PreferredLanguage);

                                    info.Title = startDate + "~" + endDate + " " + info.NameBase + " " + info.TitleBase + " " + GetResource("MyCoopConfirmRequest");
                                }
                            }

                            list.Add(info);
                        }
                    }
                }
                //전체이며, 검색어가 없으면 = 전체용이다. 세션값이 없으면 담는다.
                if (session == null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1) Session["Coop_FORWARDList"] = list;
                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetForwardList : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 전달업무
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCount(string p)
        {
            SetDecryptString(p);

            var gubun = GetDecryptParam("gubun", "");
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            try
            {
                var list = new List<ForwardEntityWS>();
                DAL.MyCoop dal = new DAL.MyCoop();
                var response = 0;                             

                #region 세션정의
                var session = Session["Coop_Getcount"];//각 기능별 전체 카운트
                if (session == null)
                {
                    if (SearchStatus.Length == 0 && SearchValue.Length == 0)
                    {
                        var cnt1 = dal.GetRequestTotCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "");
                        var cnt2 = dal.GetTOTotCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "");
                        var cnt3 = dal.GetCCTotCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "");
                        var cnt4 = dal.GetForwardTotCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "");

                        Session["Coop_Getcount"] = cnt1 + "," + cnt2 + "," + cnt3 + "," + cnt4;
                    }
                }
                #endregion

                //전체 카운트는 session 에서 가져온다.
                if (Session["Coop_Getcount"] != null && SearchStatus.Length == 0 && SearchValue.Length == 0)
                {
                    var objStr = Session["Coop_Getcount"].ToString().Split(',');
                    switch (gubun.ToUpper())
                    {
                        case "REQUEST":
                            response = objStr[0].ToNumber<int>();
                            break;
                        case "TO":
                            response = objStr[1].ToNumber<int>();
                            break;
                        case "CC":
                            response = objStr[2].ToNumber<int>();
                            break;
                        case "FORWARD":
                            response = objStr[3].ToNumber<int>();
                            break;
                    }
                }
                else
                {
                    //검색 카운트는 db 에서 쿼리한다.
                    switch (gubun.ToUpper())
                    {
                        case "REQUEST":
                            response = dal.GetRequestTotCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchStatus, SearchField, SearchValue);
                            break;
                        case "TO":
                            response = dal.GetTOTotCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchStatus, SearchField, SearchValue);
                            break;
                        case "CC":
                            response = dal.GetCCTotCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchStatus, SearchField, SearchValue);
                            break;
                        case "FORWARD":
                            response = dal.GetForwardTotCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchStatus, SearchField, SearchValue);
                            break;
                    }
                }

                //총카운트 제한하기
                int totCnt = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TabCoopMain", "PORTLET", PreLang).ToNumber<int>();   
                if (totCnt < response) response = totCnt;

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetCount : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 세션 초기화
        /// 작성자 : 심재운
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ClearSession()
        {
            try
            {
                Session["Coop_Getcount"] = null; //카운트
                Session["Coop_REQESTList"] = null;//요청
                Session["Coop_TOList"] = null;//수신
                Session["Coop_CCList"] = null;//참조
                Session["Coop_FORWARDList"] = null;//전달

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ClearSession : " + ex.Message);
                throw;
            }
        }
    }

    /// <summary>
    /// 공용
    /// 작성자 : 심재운
    /// </summary>
    public class CommonEntityWS
    {
        public CommonEntityWS() { }

        public string Gubun { get; set; }
        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public int GroupID { get; set; }
        public int ListOrder { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// 1:대기;2:접수;3:처리중;4:완료;5:반려
        /// </summary>
        public string StatusLast { get; set; }

        /// <summary>
        /// 상태명
        /// </summary>
        public string StatusLastName { get; set; }

        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }

        public string TitleBase { get; set; }

        public string RequestDate { get; set; }
        public int WriteUserID { get; set; }
        public string WriteDate { get; set; }

        /// <summary>
        /// 말머리
        /// </summary>
        public string HeaderTitle { get; set; }
        public string ShowNew { get; set; }
    }

    /// <summary>
    /// 요청
    /// 작성자 : 심재운
    /// </summary>
    [Serializable]
    public class RequestEntityWS : CommonEntityWS
    {
        public RequestEntityWS() { }
        /// <summary>
        /// 자원 id
        /// </summary>
        public int ReservationID { get; set; }
        /// <summary>
        /// 자원카테고리
        /// </summary>
        public int CategoryID { get; set; }
        
    }

    /// <summary>
    /// 수신
    /// 작성자 : 심재운
    /// </summary>
    [Serializable]
    public class TOEntityWS : CommonEntityWS
    {
        public TOEntityWS() { }

        /// <summary>
        /// 자원 id
        /// </summary>
        public int ReservationID { get; set; }
        /// <summary>
        /// 자원 카테고리
        /// </summary>
        public int CategoryID { get; set; }
    }

    /// <summary>
    /// 참조
    /// 작성자 : 심재운
    /// </summary>
    [Serializable]
    public class CCEntityWS : CommonEntityWS
    {
        public CCEntityWS() { }
    }

    /// <summary>
    /// 전달
    /// 작성자 : 심재운
    /// </summary>
    [Serializable]
    public class ForwardEntityWS : CommonEntityWS
    {
        public ForwardEntityWS() { }

        public int ForwardListID { get; set; }
        public int ForwardGroupID { get; set; }
    }

}
