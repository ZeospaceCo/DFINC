using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package.Approval
{
    /// <summary>
    /// Tab의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Tab : Base.BaseWSPortlet
    {
        #region 변수

        #endregion

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

        #region 결재대상문서

        /// <summary>
        /// 결재대상 문서 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetApproveList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = "";
                string proxyUserId = "0"; //대결자 아이디 제외

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                int isBundle = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBundle", "0").ToNumber<int>();

                Zeospace.DeskPlus.Approval.DAL.DocumentProcess biz = new DeskPlus.Approval.DAL.DocumentProcess();
                Zeospace.DeskPlus.Approval.DAL.AttachFile dalAttach = new DeskPlus.Approval.DAL.AttachFile();

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


                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");

                // 대결의뢰자 선택시 userID 대신 proxyUserID사용
                DataTableReader dtr = null;
                if (proxyUserId.ToNumber<int>() > 0)
                {
                    if (version.ToLower() == "3.3")
                    {
                        dtr = biz.GetPageListApprove33(CurrentUserInfo.CompanyID,
                            proxyUserId.ToNumber<int>(), isBundle, setPageingStr[0], setPageingStr[1],
                            startDate, endDate, searchField, searchValue, order);
                    }
                    else
                    {
                        dtr = biz.GetPageListApproveInErp(CurrentUserInfo.CompanyID,
                            proxyUserId.ToNumber<int>(), isBundle, setPageingStr[0], setPageingStr[1],
                            startDate, endDate, searchField, searchValue, order);
                    }
                }
                else
                {
                    if (version.ToLower() == "3.3")
                    {
                        dtr = biz.GetPageListApprove33(CurrentUserInfo.CompanyID,
                            CurrentUserInfo.UserID, isBundle, setPageingStr[0], setPageingStr[1],
                            startDate, endDate, searchField, searchValue, order);
                    }
                    else
                    {
                        dtr = biz.GetPageListApproveInErp(CurrentUserInfo.CompanyID,
                            CurrentUserInfo.UserID, isBundle, setPageingStr[0], setPageingStr[1],
                            startDate, endDate, searchField, searchValue, order);
                    }
                }

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.DocumentProcessInfo info = new DocumentProcessInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            //info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            //info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            //info.Number3 = dtr["Number3"].ToString();
                            info.ApprovalState = dtr["ApprovalState"].ToString();
                            //info.NextUserID = dtr["NextUserID"].ToString().ToNumber<int>();

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion


                            #region 주석

                            /*
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

                            #region 다음결재자 직급 다국어 설정
                            string NextTitleBase = "";
                            string NextTitleKo = "";
                            string NextTitleEn = "";
                            string NextTitleJa = "";
                            string NextTitleFr = "";
                            string NextTitleZhcn = "";
                            string NextTitleZhtw = "";

                            string[] arrNextTitle = dtr["NextTitle"].ToString().ToNull().Split('$');

                            if (dtr["NextTitle"].ToString().ToNull() != "")
                            {
                                NextTitleBase = arrNextTitle[0].ToString();
                                NextTitleKo = arrNextTitle[1].ToString();
                                NextTitleEn = arrNextTitle[2].ToString();
                                NextTitleJa = arrNextTitle[3].ToString();
                                NextTitleFr = arrNextTitle[4].ToString();
                                NextTitleZhcn = arrNextTitle[5].ToString();
                                NextTitleZhtw = arrNextTitle[6].ToString();
                            }

                            info.NextTitleInfo = this.GetLangName(
                                    NextTitleBase, NextTitleKo, NextTitleEn, NextTitleJa,
                                    NextTitleFr, NextTitleZhcn, NextTitleZhtw, PreLang);
                            #endregion

                            #region 다음결재자 이름 다국어 설정
                            string NextUserBase = "";
                            string NextUserKo = "";
                            string NextUserEn = "";
                            string NextUserJa = "";
                            string NextUserFr = "";
                            string NextUserZhcn = "";
                            string NextUserZhtw = "";

                            string[] arrNextUser = dtr["NextUser"].ToString().ToNull().Split('$');

                            if (dtr["NextUser"].ToString().ToNull() != "")
                            {
                                NextUserBase = arrNextUser[0].ToString();
                                NextUserKo = arrNextUser[1].ToString();
                                NextUserEn = arrNextUser[2].ToString();
                                NextUserJa = arrNextUser[3].ToString();
                                NextUserFr = arrNextUser[4].ToString();
                                NextUserZhcn = arrNextUser[5].ToString();
                                NextUserZhtw = arrNextUser[6].ToString();

                            }

                            info.NextUserInfo = this.GetLangName(
                                   NextUserBase, NextUserKo, NextUserEn, NextUserJa,
                                   NextUserFr, NextUserZhcn, NextUserZhtw, PreLang);
                            #endregion

                            #region 문서양식명 다국어 설정
                            info.FormBase = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(), dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(), dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(), dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(), PreLang);
                            #endregion
                            */
                            #endregion

                            info.UserID = dtr["TargetUserID"].ToString().ToNumber<int>();

                            // 결재페이지 이동 파라미터
                            info.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                            info.TargetUserID = dtr["TargetUserID"].ToString().ToNumber<int>();
                            info.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                            info.ApprovalTag = dtr["ApprovalTag"].ToString();
                            info.MyTurn = dtr["MyTurn"].ToString();
                            
                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;

                            info.Subject = dtr["Subject"].ToString();//제목                            

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteTime"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int ApprovalNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= ApprovalNewShowDay) ? "Y" : "N";
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
                return new List<object>();
                //throw;
            }
        }

        /// <summary>
        /// 결재대상 문서 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetApproveListCnt(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string proxyUserId = "0"; //대결자 아이디 제외

                int icnt = 0;
                int isBundle = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBundle", "0").ToNumber<int>();
                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");

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

                Zeospace.DeskPlus.Approval.DAL.DocumentProcess biz = new DeskPlus.Approval.DAL.DocumentProcess();

                // 대결의뢰자 선택시 userID 대신 proxyUserID사용
                if (proxyUserId.ToNumber<int>() > 0)
                {
                    if (version.ToLower() == "3.3")
                    {
                        icnt = biz.GetPageCountApprove33(CurrentUserInfo.CompanyID,
                            proxyUserId.ToNumber<int>(), isBundle, startDate, endDate,
                            searchField, searchValue);
                    }
                    else
                    {
                        icnt = biz.GetPageCountApproveInErp(CurrentUserInfo.CompanyID,
                            proxyUserId.ToNumber<int>(), isBundle, startDate, endDate,
                            searchField, searchValue);
                    }
                }
                else
                {
                    if (version.ToLower() == "3.3")
                    {
                        icnt = biz.GetPageCountApprove33(CurrentUserInfo.CompanyID,
                            CurrentUserInfo.UserID, isBundle, startDate, endDate,
                            searchField, searchValue);
                    }
                    else
                    {
                        icnt = biz.GetPageCountApproveInErp(CurrentUserInfo.CompanyID,
                            CurrentUserInfo.UserID, isBundle, startDate, endDate,
                            searchField, searchValue);
                    }

                }

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return 0;
                //throw;
            }
        }

        #endregion

        #region 상신중인문서

        /// <summary>
        /// 상신문서 문서 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetReportList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = "";

                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                Zeospace.DeskPlus.Approval.DAL.DocumentProcess biz = new DeskPlus.Approval.DAL.DocumentProcess();
                Zeospace.DeskPlus.Approval.DAL.AttachFile dalAttach = new DeskPlus.Approval.DAL.AttachFile();

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


                DataTableReader dtr = null;

                if (version.ToLower() == "3.3")
                {
                    dtr = biz.GetPageListProcess33(CurrentUserInfo.CompanyID,
                        CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                        startDate, endDate,
                        searchField, searchValue, order);
                }
                else
                {
                    dtr = biz.GetPageListProcessInErp(CurrentUserInfo.CompanyID,
                        CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                        startDate, endDate,
                        searchField, searchValue, order);
                }

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.DocumentProcessInfo info = new DocumentProcessInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            //info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            //info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            //info.Number3 = dtr["Number3"].ToString();
                            info.ApprovalState = dtr["ApprovalState"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            //info.NextUserID = dtr["NextUserID"].ToString().ToNumber<int>();
                            //info.MyTurn = dtr["MyTurn"].ToString();

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            /*
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

                            #region 다음결재자 직급 다국어 설정
                            string NextTitleBase = "";
                            string NextTitleKo = "";
                            string NextTitleEn = "";
                            string NextTitleJa = "";
                            string NextTitleFr = "";
                            string NextTitleZhcn = "";
                            string NextTitleZhtw = "";

                            string[] arrNextTitle = dtr["NextTitle"].ToString().ToNull().Split('$');

                            if (dtr["NextTitle"].ToString().ToNull() != "")
                            {
                                NextTitleBase = arrNextTitle[0].ToString();
                                NextTitleKo = arrNextTitle[1].ToString();
                                NextTitleEn = arrNextTitle[2].ToString();
                                NextTitleJa = arrNextTitle[3].ToString();
                                NextTitleFr = arrNextTitle[4].ToString();
                                NextTitleZhcn = arrNextTitle[5].ToString();
                                NextTitleZhtw = arrNextTitle[6].ToString();
                            }

                            info.NextTitleInfo = this.GetLangName(
                                    NextTitleBase, NextTitleKo, NextTitleEn, NextTitleJa,
                                    NextTitleFr, NextTitleZhcn, NextTitleZhtw, PreLang);
                            #endregion

                            #region 다음결재자 이름 다국어 설정
                            string NextUserBase = "";
                            string NextUserKo = "";
                            string NextUserEn = "";
                            string NextUserJa = "";
                            string NextUserFr = "";
                            string NextUserZhcn = "";
                            string NextUserZhtw = "";

                            string[] arrNextUser = dtr["NextUser"].ToString().ToNull().Split('$');

                            if (dtr["NextUser"].ToString().ToNull() != "")
                            {
                                NextUserBase = arrNextUser[0].ToString();
                                NextUserKo = arrNextUser[1].ToString();
                                NextUserEn = arrNextUser[2].ToString();
                                NextUserJa = arrNextUser[3].ToString();
                                NextUserFr = arrNextUser[4].ToString();
                                NextUserZhcn = arrNextUser[5].ToString();
                                NextUserZhtw = arrNextUser[6].ToString();

                            }

                            info.NextUserInfo = this.GetLangName(
                                   NextUserBase, NextUserKo, NextUserEn, NextUserJa,
                                   NextUserFr, NextUserZhcn, NextUserZhtw, PreLang);
                            #endregion

                            #region 문서양식명 다국어 설정
                            info.FormBase = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(), dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(), dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(), dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.AccountTag = dtr["AccountTag"].ToString().ToNull();
                            info.NextSequence = dtr["NextSequence"].ToString().ToNumber<int>();
                            */

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;


                            //info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            //info.Filecount = dalAttach.GetAttachFileCntByPath(info.CompanyID, info.ChildDocID);

                            info.Subject = dtr["Subject"].ToString();//제목                            

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteTime"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
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
                return new List<object>();
                //throw;
            }
        }

        /// <summary>
        /// 상신문서 문서 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetReportListCnt(string p)
        {

            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");

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

                Zeospace.DeskPlus.Approval.DAL.DocumentProcess biz = new DeskPlus.Approval.DAL.DocumentProcess();

                if (version.ToLower() == "3.3")
                {
                    icnt = biz.GetPageCountProcess33(CurrentUserInfo.CompanyID,
                        CurrentUserInfo.UserID, startDate, endDate,
                        searchField, searchValue);
                }
                else
                {
                    icnt = biz.GetPageCountProcessInErp(CurrentUserInfo.CompanyID,
                        CurrentUserInfo.UserID, startDate, endDate,
                        searchField, searchValue);
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

        #region 참조문서

        /// <summary>
        /// 상신문서 문서 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCarbonList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = "";

                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                Zeospace.DeskPlus.Approval.DAL.CarbonCopyUser biz = new DeskPlus.Approval.DAL.CarbonCopyUser();
                Zeospace.DeskPlus.Approval.DAL.AttachFile dalAttach = new DeskPlus.Approval.DAL.AttachFile();

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

                DataTableReader dtr = null;

                if (version.ToLower() == "3.3")
                {
                    //dtr = biz.GetPageListProcess33(CurrentUserInfo.CompanyID,
                    //    CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                    //    startDate, endDate,
                    //    searchField, searchValue, order);
                }
                else
                {
                    dtr = biz.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, 0,
                        setPageingStr[0], setPageingStr[1], startDate, endDate, searchField, searchValue, order);
                }

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.DocumentProcessInfo info = new DocumentProcessInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ApprovalState = dtr["ApprovalState"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;

                            info.Subject = dtr["Subject"].ToString();//제목                            

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteTime"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
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
                return new List<object>();
                //throw;
            }
        }

        /// <summary>
        /// 상신문서 문서 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCarbonListCnt(string p)
        {

            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");

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

                Zeospace.DeskPlus.Approval.DAL.CarbonCopyUser biz = new DeskPlus.Approval.DAL.CarbonCopyUser();

                if (version.ToLower() == "3.3")
                {
                    //icnt = biz.GetPageCountProcess33(CurrentUserInfo.CompanyID,
                    //    CurrentUserInfo.UserID, startDate, endDate,
                    //    searchField, searchValue);
                }
                else
                {
                    icnt = biz.GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, 0, startDate, endDate, searchField, searchValue);
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

        #region 전달문서

        /// <summary>
        /// 상신문서 문서 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetForwardList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = "";

                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                Zeospace.DeskPlus.Approval.DAL.ForwardUser biz = new DeskPlus.Approval.DAL.ForwardUser();
                Zeospace.DeskPlus.Approval.DAL.AttachFile dalAttach = new DeskPlus.Approval.DAL.AttachFile();

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

                DataTableReader dtr = null;

                if (version.ToLower() == "3.3")
                {
                    //dtr = biz.GetPageListProcess33(CurrentUserInfo.CompanyID,
                    //    CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                    //    startDate, endDate,
                    //    searchField, searchValue, order);
                }
                else
                {
                    dtr = biz.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, setPageingStr[0], setPageingStr[1], startDate, endDate, searchField, searchValue, order);
                }

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.DocumentProcessInfo info = new DocumentProcessInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ApprovalState = dtr["ApprovalState"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;

                            info.Subject = dtr["Subject"].ToString();//제목                            

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteTime"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
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
                return new List<object>();
                //throw;
            }
        }

        /// <summary>
        /// 상신문서 문서 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetForwardListCnt(string p)
        {

            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");

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

                Zeospace.DeskPlus.Approval.DAL.ForwardUser biz = new DeskPlus.Approval.DAL.ForwardUser();

                if (version.ToLower() == "3.3")
                {
                    //icnt = biz.GetPageCountProcess33(CurrentUserInfo.CompanyID,
                    //    CurrentUserInfo.UserID, startDate, endDate,
                    //    searchField, searchValue);
                }
                else
                {
                    icnt = biz.GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, startDate, endDate, searchField, searchValue);
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

        #region 회람문서

        /// <summary>
        /// 상신문서 문서 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCirList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = "";

                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                Zeospace.DeskPlus.Approval.DAL.CirculationDocument biz = new DeskPlus.Approval.DAL.CirculationDocument();
                Zeospace.DeskPlus.Approval.DAL.AttachFile dalAttach = new DeskPlus.Approval.DAL.AttachFile();

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

                DataTableReader dtr = null;

                if (version.ToLower() == "3.3")
                {
                    //dtr = biz.GetPageListProcess33(CurrentUserInfo.CompanyID,
                    //    CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                    //    startDate, endDate,
                    //    searchField, searchValue, order);
                }
                else
                {
                    dtr = biz.GetPageListRecv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, setPageingStr[0], setPageingStr[1], startDate, endDate, searchField, searchValue, order);
                }

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.CirculationDocumentInfo info = new CirculationDocumentInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CirculationDocumentID = dtr["CirculationDocumentID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ApprovalState = dtr["ApprovalState"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;

                            info.Subject = dtr["Subject"].ToString();//제목                            

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteTime"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
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
                return new List<object>();
                //throw;
            }
        }

        /// <summary>
        /// 상신문서 문서 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCirListCnt(string p)
        {

            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                //전자결재 버전
                string version = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalVersion", "4.0");

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

                Zeospace.DeskPlus.Approval.DAL.CirculationDocument biz = new DeskPlus.Approval.DAL.CirculationDocument();

                if (version.ToLower() == "3.3")
                {
                    //icnt = biz.GetPageCountProcess33(CurrentUserInfo.CompanyID,
                    //    CurrentUserInfo.UserID, startDate, endDate,
                    //    searchField, searchValue);
                }
                else
                {
                    icnt = biz.GetPageCountRecv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, startDate, endDate, searchField, searchValue);
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
    }
}
