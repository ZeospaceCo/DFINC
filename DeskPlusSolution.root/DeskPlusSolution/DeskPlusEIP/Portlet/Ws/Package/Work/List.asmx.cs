using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Portlet.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.Work
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
        #endregion

        /// <summary>
        /// 받은댓글
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShortRecvList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Work.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            
            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            //PageSizePageNo = GetDecryptParam("pageSizeNo", "0").ToNumber<int>();
            PageSizePageNo = 0;
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

            string titleOpenType = String.Empty;
            string styleOpenType = String.Empty;

            try
            {
                List<object> list = new List<object>();

                DAL.Work dal = new DAL.Work();
                DataTableReader dtr = dal.GetWorkShortRcvPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, PageSize.ToString(), PageSizePageNo.ToString(), "", "", "", "", "");

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.WorkInfo info = new WorkInfo();

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
                                            styleOpenType = "<span class='m_pim_private'></span>&nbsp;";
                                            break;
                                        case "1":
                                            titleOpenType = this.GetResource("pim3");
                                            styleOpenType = "<span class='m_pim_share'></span>&nbsp;";
                                            break;
                                        case "2":
                                            titleOpenType = this.GetResource("pim4");
                                            styleOpenType = "<span class='m_pim_dept'></span>&nbsp;";
                                            break;
                                        case "3":
                                            titleOpenType = this.GetResource("pim2");
                                            styleOpenType = "<span class='m_pim_tot'></span>&nbsp;";
                                            break;
                                    }
                                    info.PathName = titleOpenType;
                                    info.GubunTitle = this.GetResource("Schedule"); 
                                    info.StyleType = styleOpenType;
                                    break;
                                case "PIM_TODO":
                                     switch (info.AccountTag)
                                    {
                                        case "0":
                                            titleOpenType = this.GetResource("TodoShare1");
                                            styleOpenType = "<span class='m_pim_private'></span>&nbsp;";
                                            break;
                                        case "1":
                                            titleOpenType = this.GetResource("TodoShare3");
                                            styleOpenType = "<span class='m_pim_share'></span>&nbsp;";
                                            break;
                                        case "2":
                                            titleOpenType = this.GetResource("TodoShare4");
                                            styleOpenType = "<span class='m_pim_dept'></span>&nbsp;";
                                            break;
                                        case "3":
                                            titleOpenType = this.GetResource("TodoShare2");
                                            styleOpenType = "<span class='m_pim_tot'></span>&nbsp;";
                                            break;
                                    }
                                    info.PathName = titleOpenType;
                                    info.GubunTitle = this.GetResource("ToDo"); 
                                    info.StyleType = styleOpenType;
                                    break;
                                case "Board":

                                    #region 다국어 경로 다국어 설정
                                    info.PathName = this.GetLangName(
                                        dtr["PathNameKo"].ToString(),
                                        dtr["PathNameKo"].ToString(), dtr["PathNameEn"].ToString(), dtr["PathNameJa"].ToString(),
                                        dtr["PathNameFr"].ToString(), dtr["PathNameZhCn"].ToString(), dtr["PathNameZhTw"].ToString(), PreLang);
                                    #endregion

                                    if (info.PathName.Length > 0) info.PathName = info.PathName.Substring(1, info.PathName.Length - 1);

                                    info.GubunTitle = this.GetResource("Board");
                                    if (info.OptionTop == 1) info.GubunTitle = this.GetResource("Notice");//공지글
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


                            info.AccountTag = dtr["AccountTag"].ToString();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();

                            list.Add(info);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetShortRecvList : " + ex.Message);
                throw;
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShortSendList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Work.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            //PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            PageSizePageNo = 0;
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

            string titleOpenType = String.Empty;
            string styleOpenType = String.Empty;

            try
            {
                List<object> list = new List<object>();

                DAL.Work dal = new DAL.Work();
                DataTableReader dtr = dal.GetWorkShortSendPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, PageSize.ToString(), PageSizePageNo.ToString(), "", "", "", "", "");

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.WorkInfo info = new WorkInfo();

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
                                            styleOpenType = "<span class='m_pim_private'></span>&nbsp;";
                                            break;
                                        case "1":
                                            titleOpenType = this.GetResource("pim3");
                                            styleOpenType = "<span class='m_pim_share'></span>&nbsp;";
                                            break;
                                        case "2":
                                            titleOpenType = this.GetResource("pim4");
                                            styleOpenType = "<span class='m_pim_dept'></span>&nbsp;";
                                            break;
                                        case "3":
                                            titleOpenType = this.GetResource("pim2");
                                            styleOpenType = "<span class='m_pim_tot'></span>&nbsp;";
                                            break;
                                    }
                                    info.PathName = titleOpenType;
                                    info.GubunTitle = this.GetResource("Schedule"); 
                                    info.StyleType = styleOpenType;
                                    break;
                                case "PIM_TODO":
                                     switch (info.AccountTag)
                                    {
                                        case "0":
                                            titleOpenType = this.GetResource("TodoShare1");
                                            styleOpenType = "<span class='m_pim_private'></span>&nbsp;";
                                            break;
                                        case "1":
                                            titleOpenType = this.GetResource("TodoShare3");
                                            styleOpenType = "<span class='m_pim_share'></span>&nbsp;";
                                            break;
                                        case "2":
                                            titleOpenType = this.GetResource("TodoShare4");
                                            styleOpenType = "<span class='m_pim_dept'></span>&nbsp;";
                                            break;
                                        case "3":
                                            titleOpenType = this.GetResource("TodoShare2");
                                            styleOpenType = "<span class='m_pim_tot'></span>&nbsp;";
                                            break;
                                    }
                                    info.PathName = titleOpenType;
                                    info.GubunTitle = this.GetResource("ToDo"); 
                                    info.StyleType = styleOpenType;
                                    break;
                                case "Board":

                                    #region 다국어 경로 다국어 설정
                                    info.PathName = this.GetLangName(
                                        dtr["PathNameKo"].ToString(),
                                        dtr["PathNameKo"].ToString(), dtr["PathNameEn"].ToString(), dtr["PathNameJa"].ToString(),
                                        dtr["PathNameFr"].ToString(), dtr["PathNameZhCn"].ToString(), dtr["PathNameZhTw"].ToString(), PreLang);
                                    #endregion

                                    if (info.PathName.Length > 0) info.PathName = info.PathName.Substring(1, info.PathName.Length - 1);

                                    info.GubunTitle = this.GetResource("Board");
                                    if (info.OptionTop == 1) info.GubunTitle = this.GetResource("Notice");//공지글
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
                            //원본글작성자이름
                            info.Name = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion



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


                            list.Add(info);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetShortSendList : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 결재톡
        /// 작성자 : 심재운  WITH
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShortAppList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Work.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);


            PageSize = GetDecryptParam("pageSize", "0").ToNumber<int>();
            //PageSizePageNo = GetDecryptParam("pageSizeNo", "1").ToNumber<int>();
            PageSizePageNo = 0;
            TotalCount = GetDecryptParam("totalCount", "0").ToNumber<int>();

            string titleOpenType = String.Empty;
            string styleOpenType = String.Empty;

            try
            {
                List<object> list = new List<object>();

                DAL.Work dal = new DAL.Work();
                DataTableReader dtr = dal.GetWorkShortAppPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, PageSize.ToString(), PageSizePageNo.ToString(), "", "", "", "", "");

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.WorkInfo info = new WorkInfo();

                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>(); //원본글작성자
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();//댓글작성자
                            //info.Gubun = dtr["Gubun"].ToString();
 
                            switch (dtr["Section"].ToString())
                            {
                                //구분(1:상신.결재/2:첨언추가,3:전달, 4:업무톡)

                                case "1":
                                    titleOpenType = this.GetResource("ShortReplySec1");    //결재 -> 첨언
                                    break;
                                case "2":
                                    titleOpenType = this.GetResource("ShortReplySec1"); //this.GetResource("ShortReplySec2");    //의견추가 -> 추가첨언
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
                                    styleOpenType = "<span class='m_a_d3'></span>&nbsp;";
                                    info.StateStr = this.GetResource("AppReplySec1");
                                    break;
                                case "W":
                                    styleOpenType = "<span class='m_a_r3'></span>&nbsp;";
                                    info.StateStr = this.GetResource("AppReplySec2");
                                    break;
                                case "I":
                                    styleOpenType = "<span class='m_a_i3'></span>&nbsp;";
                                    info.StateStr = this.GetResource("AppReplySec3");
                                    break;
                                case "R":
                                    styleOpenType = "<span class='m_a_re3'></span>&nbsp;";
                                    info.StateStr = this.GetResource("AppReplySec4");
                                    break;
                                case "C":
                                    styleOpenType = "<span class='m_a_e3'></span>&nbsp;";
                                    info.StateStr = this.GetResource("AppReplySec5");
                                    break;
                                default:
                                    styleOpenType = "<span class='m_a_i3'></span>&nbsp;";
                                    info.StateStr = this.GetResource("AppReplySec3");
                                    break;
                            }

                            info.StyleType = styleOpenType;

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
                            info.Name = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

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


                            info.AccountTag = dtr["AccountTag"].ToString();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();

                            list.Add(info);
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetShortAppList : " + ex.Message);
                throw;
            }
        }

    }
}
