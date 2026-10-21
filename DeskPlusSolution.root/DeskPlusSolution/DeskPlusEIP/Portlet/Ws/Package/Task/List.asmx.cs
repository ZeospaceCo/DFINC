using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Portlet.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.Task
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
        /// 
        /// </summary>
        /// <param name="_statusLast"></param>
        /// <returns></returns>
        private string GetStatusName(string _statusLast)
        {

            string resourceNm = "";
            if (!this.resMng.Equals(null))
            {
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
                    case "6":
                        resourceNm = resMng.GetString("Delay");
                        break;
                }
            }

            return resourceNm;
        }        
                
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 요청한 협업
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<CommonEntityWS> GetRequest(string p)
        {
            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Task.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            //총카운트 제한하기
            //int totCnt = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TabTaskMain", "PORTLET", PreLang).ToNumber<int>();
            //if (totCnt < TotalCount) TotalCount = totCnt;

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');

            try
            {
                var list = new List<CommonEntityWS>();

                /*
                var session = Session["TASK_REQESTList"];//요청
                //전체이며, 검색어가 없으면 = 전체용이다.
                if (session != null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1)
                {
                    list = (List<CommonEntityWS>)Session["TASK_REQESTList"];
                    return list;
                }
                */

                DAL.Task dal = new DAL.Task();
                DataTableReader dtr = dal.GetRequest(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], SearchStatus, SearchField, SearchValue);


                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            var info = new CommonEntityWS();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();

                            info.StatusLastName = GetStatusName(dtr["StatusLast"].ToString());
                            info.StatusLast = dtr["StatusLast"].ToString();

                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.RequestDate = dtr["RequestDate"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            #region 날짜 비교(New Img)
                            DateTime WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(WriteDate);
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N"; //하루만
                            #endregion

                            list.Add(info);
                        }
                    }
                }

                //전체이며, 검색어가 없으면 = 전체용이다. 세션값이 없으면 담는다.
                //if (session == null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1) Session["TASK_REQESTList"] = list;

                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetRequest : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 요청 받은 협업
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<CommonEntityWS> GetReceive(string p)
        {
            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Task.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            //총카운트 제한하기
            //int totCnt = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TabTaskMain", "PORTLET", PreLang).ToNumber<int>();
            //if (totCnt < TotalCount) TotalCount = totCnt;

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');

            try
            {
                var list = new List<CommonEntityWS>();

                /*
                var session = Session["TASK_TOList"];
                //전체이며, 검색어가 없으면 = 전체용이다.
                if (session != null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1)
                {
                    list = (List<CommonEntityWS>)Session["TASK_TOList"];
                    return list;
                }
                */

                DAL.Task dal = new DAL.Task();
                DataTableReader dtr = dal.GetReceive(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], SearchStatus, SearchField, SearchValue);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            var info = new CommonEntityWS();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();

                            info.StatusLastName = GetStatusName(dtr["StatusLast"].ToString());
                            info.StatusLast = dtr["StatusLast"].ToString();

                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.RequestDate = dtr["RequestDate"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            #region 날짜 비교(New Img)
                            DateTime WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(WriteDate);
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N"; //하루만
                            #endregion

                            list.Add(info);
                        }
                    }
                }

                //전체이며, 검색어가 없으면 = 전체용이다. 세션값이 없으면 담는다.
                //if (session == null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1) Session["TASK_TOList"] = list;
                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetReceive : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 참조 받은 협업
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<CommonEntityWS> GetReference(string p)
        {
            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Task.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            //총카운트 제한하기
            //int totCnt = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TabTaskMain", "PORTLET", PreLang).ToNumber<int>();
            //if (totCnt < TotalCount) TotalCount = totCnt;

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');

            try
            {
                var list = new List<CommonEntityWS>();

                /*
                var session = Session["TASK_CCList"];
                //전체이며, 검색어가 없으면 = 전체용이다.
                if (session != null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1)
                {
                    list = (List<CommonEntityWS>)Session["TASK_CCList"];
                    return list;
                }
                */

                DAL.Task dal = new DAL.Task();
                DataTableReader dtr = dal.GetReference(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], SearchStatus, SearchField, SearchValue);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            var info = new CommonEntityWS();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();

                            info.StatusLastName = GetStatusName(dtr["StatusLast"].ToString());
                            info.StatusLast = dtr["StatusLast"].ToString();

                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.RequestDate = dtr["RequestDate"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            #region 날짜 비교(New Img)
                            DateTime WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(WriteDate);
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N"; //하루만
                            #endregion

                            list.Add(info);
                        }
                    }
                }

                //전체이며, 검색어가 없으면 = 전체용이다. 세션값이 없으면 담는다.
                //if (session == null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1) Session["TASK_CCList"] = list;
                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetReference : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 전달 받은 협업
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<CommonEntityWS> GetForward(string p)
        {

            SetDecryptString(p);

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Task.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();
            SearchStatus = GetDecryptParam("searchStatus", "");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");

            //총카운트 제한하기
            //int totCnt = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TabTaskMain", "PORTLET", PreLang).ToNumber<int>();
            //if (totCnt < TotalCount) TotalCount = totCnt;

            string rtnSetPaging = SetPaging();
            string[] setPageingStr = rtnSetPaging.Split(',');

            try
            {
                var list = new List<CommonEntityWS>();

                /*
                var session = Session["Task_FORWARDList"];
                //전체이며, 검색어가 없으면 = 전체용이다.
                if (session != null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1)
                {
                    list = (List<CommonEntityWS>)Session["Task_FORWARDList"];
                    return list;
                }
                */

                DAL.Task dal = new DAL.Task();
                DataTableReader dtr = dal.GetForward(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], SearchStatus, SearchField, SearchValue);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            var info = new CommonEntityWS();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ListOrder = dtr["ListOrder"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();

                            info.StatusLastName = GetStatusName(dtr["StatusLast"].ToString());
                            info.StatusLast = dtr["StatusLast"].ToString();

                            info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                            info.RequestDate = dtr["RequestDate"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.Viewing = dtr["Viewing"].ToString();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;

                            #region 날짜 비교(New Img)
                            DateTime WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(WriteDate);
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N"; //하루만
                            #endregion

                            list.Add(info);
                        }
                    }
                }
                //전체이며, 검색어가 없으면 = 전체용이다. 세션값이 없으면 담는다.
                //if (session == null && SearchStatus.Length == 0 && SearchValue.Length == 0 && PageSizePageNo == 1) Session["Task_FORWARDList"] = list;
                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetForward : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 리스트 갯수
        /// </summary>
        /// <param name="p"></param>
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
                var list = new List<CommonEntityWS>();
                DAL.Task dal = new DAL.Task();
                var response = 0;

                switch (gubun.ToUpper())
                {
                    case "REQUEST":
                        response = dal.GetRequestCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchStatus, SearchField, SearchValue);
                        break;
                    case "TO":
                        response = dal.GetReceiveCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchStatus, SearchField, SearchValue);
                        break;
                    case "CC":
                        response = dal.GetReferenceCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchStatus, SearchField, SearchValue);
                        break;
                    case "FORWARD":
                        response = dal.GetForwardCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchStatus, SearchField, SearchValue);
                        break;
                }               

                //총카운트 제한하기
                //int totCnt = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "TabTaskMain", "PORTLET", PreLang).ToNumber<int>();
                //if (totCnt < response) response = totCnt;

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetCount : " + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-06
        /// 작성목적 : 세션 초기화
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ClearSession()
        {
            try
            {
                Session["TASK_Getcount"] = null; //카운트
                Session["TASK_REQESTList"] = null;//요청
                Session["TASK_TOList"] = null;//수신
                Session["TASK_CCList"] = null;//참조
                Session["TASK_FORWARDList"] = null;//전달

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
    /// 
    /// </summary>
    public class CommonEntityWS
    {
        public CommonEntityWS() { }
        public int CompanyID { get; set; }
        public int ListID { get; set; }
        public int GroupID { get; set; }
        public int ListOrder { get; set; }
        public string Title { get; set; }
        public string StatusLast { get; set; }
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
        public string HeaderTitle { get; set; }
        public string ShowNew { get; set; }
        public string Viewing { get; set; }
    }

}
