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

namespace Zeospace.DeskPlus.Approval.Web.Ws.Status
{
    /// <summary>
    /// ListProcessAll의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListProcessAll : Base.BaseWSApproval
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
        /// 작성자 : 이문호
        /// 결재상황조회 - 상신한 문서 리스트 가져오기
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
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.DocumentProcessAll biz = new DAL.DocumentProcessAll();
                DAL.AttachFile dalAttach = new DAL.AttachFile();

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

                DataTableReader dtr = biz.GetPageListProcessAll(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                    startDate, endDate,
                    searchField, searchValue, order);

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.DocumentProcessInfo info = new BE.DocumentProcessInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            info.Number3 = dtr["Number3"].ToString();
                            info.ApprovalState = dtr["ApprovalState"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.NextUserID = dtr["NextUserID"].ToString().ToNumber<int>();
                            info.MyTurn = dtr["MyTurn"].ToString();

                            #region 이름 다국어 설정
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
                            info.ApprovalTag = dtr["MyTurnYTag"].ToString();
                            info.LastTag = dtr["LastTag"].ToString();
                            info.IsLegacy = dtr["IsLegacy"].ToString();             //커스텀연동정보
                            info.CustomFileName = dtr["CustomFileName"].ToString(); //커스텀연동정보

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;


                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.Filecount = dalAttach.GetAttachFileCntByPath(info.CompanyID, info.ChildDocID);

                            info.Subject = dtr["Subject"].ToString();//제목                            

                            info.IsGuard = dtr["IsGuard"].ToString();//문서종류

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteTime"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
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
                throw;
            }
        }



        /// <summary>
        /// 결재상황조회 상신한 문서 카운트
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

                DAL.DocumentProcessAll biz = new DAL.DocumentProcessAll();
                icnt = biz.GetPageCountProcessAll(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, startDate, endDate,
                    searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이문호
        /// 결재상황조회 - 상신한 문서 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListSlip(string p)
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
                string order = GetDecryptParam("order", "");

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.DocumentProcess biz = new DAL.DocumentProcess();
                DAL.AttachFile dalAttach = new DAL.AttachFile();

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

                DataTableReader dtr = biz.GetPageListProcessSlip(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                    startDate, endDate,
                    searchField, searchValue, order);

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.DocumentProcessInfo info = new BE.DocumentProcessInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            info.Number3 = dtr["Number3"].ToString();
                            info.ApprovalState = dtr["ApprovalState"].ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.NextUserID = dtr["NextUserID"].ToString().ToNumber<int>();
                            info.MyTurn = dtr["MyTurn"].ToString();

                            #region 이름 다국어 설정
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
                            info.ApprovalTag = dtr["MyTurnYTag"].ToString();
                            info.LastTag = dtr["LastTag"].ToString();
                            info.IsLegacy = dtr["IsLegacy"].ToString();             //커스텀연동정보
                            info.CustomFileName = dtr["CustomFileName"].ToString(); //커스텀연동정보

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;


                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.Filecount = dalAttach.GetAttachFileCntByPath(info.CompanyID, info.ChildDocID);

                            info.Subject = dtr["Subject"].ToString();//제목                            

                            info.IsGuard = dtr["IsGuard"].ToString();//문서종류

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteTime"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
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
                throw;
            }
        }



        /// <summary>
        /// 결재상황조회 상신한 문서 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCountSlip(string p)
        {
            try
            {
                SetDecryptString(p);
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

                DAL.DocumentProcess biz = new DAL.DocumentProcess();
                icnt = biz.GetPageCountProcessSlip(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, startDate, endDate,
                    searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region 첨부파일

        /// <summary>
        /// 작성자   : 이문호
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
                SetDecryptString(p);

                int childDocID = GetDecryptParam("childDocID", "0").ToNumber<Int32>();


                string response = "0";

                DAL.AttachFile dal = new DAL.AttachFile();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileListsByPath(CurrentUserInfo.CompanyID, childDocID);

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

        #endregion

        #region 프로세스(상신보류/상신보류해제...)
        /// <summary>
        /// 상신보류(H) / 상신보류해제(W)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ApproveH(string p)
        {
            try
            {
                SetDecryptString(p);

                int docID = GetDecryptParam("docID", "0").ToNumber<int>();
                string accountTag = GetDecryptParam("accountTag", "");
                string approvalState = GetDecryptParam("state", "");
                int groupID = GetDecryptParam("groupID", "0").ToNumber<int>();

                bool rtnval = false;

                BLL.DocumentProcess dpBll = new BLL.DocumentProcess();

                if (approvalState.ToUpper().Equals("H"))
                {
                    //상신보류
                    BE.UserInfo sendInfo = GetSendUserInfo("H", "");
                    rtnval = dpBll.ApproveH(CurrentUserInfo.CompanyID, docID, groupID, accountTag, sendInfo);
                }
                else
                {
                    //상신보류해제
                    BE.UserInfo sendInfo = GetSendUserInfo("HC", "");
                    rtnval = dpBll.ApproveHCancel(CurrentUserInfo.CompanyID, docID, groupID, accountTag, sendInfo);
                }

                if (rtnval)
                {
                    return "TRUE";
                }
                else
                {
                    return "FALSE";
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "FALSE";
            }
        }

        /// <summary>
        /// 상신취소/문서폐기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ApproveDelete(string p)
        {
            try
            {
                SetDecryptString(p);

                int groupID = GetDecryptParam("groupID", "0").ToNumber<int>();
                int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();
                string accountTag = GetDecryptParam("accountTag", "0");

                BE.UserInfo sendInfo = GetSendUserInfo("C", "");
                BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
                if (dpBll.DeleteDoc(CurrentUserInfo.CompanyID, groupID, documentID, sendInfo, accountTag.Substring(0, 2)))
                {
                    return "TRUE";
                }
                else
                {
                    return "FALSE";
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "FALSE";
            }
        }

        /// <summary>
        /// 수신회수용 본문내용 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetContentHTML(string p)
        {
            try
            {
                SetDecryptString(p);

                int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();

                string ContentHTML = "";

                DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                List<BE.DocumentProcessInfo> list = dpDal.GetDocument(CurrentUserInfo.CompanyID, documentID);
                foreach (BE.DocumentProcessInfo info in list)
                {
                    ContentHTML = info.ContentHTML;
                }

                return ContentHTML;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "!NULL";
            }
        }

        /// <summary>
        /// 수신회수
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string CancelSusin(string p)
        {
            try
            {
                SetDecryptString(p);

                int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();
                string accountTag = GetDecryptParam("accountTag", "");
                string contentHTML = GetDecryptParam("contentHTML", "");
                contentHTML = Server.UrlDecode(contentHTML);

                if (accountTag.IndexOf("ZQ") > -1 && contentHTML == "") return "FALSE";

                BE.UserInfo sendInfo = GetSendUserInfo("C", "");
                BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
                if (dpBll.CancelSusinDoc(CurrentUserInfo.CompanyID, documentID, contentHTML))
                {
                    return "TRUE";
                }
                else
                {
                    return "FALSE";
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "FALSE";
            }
        }
        #endregion

        #region ModViewing
        /// <summary>
        /// 해당 결재 결재페이지 접근가능여부 확인
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetViewing(string p)
        {
            try
            {
                SetDecryptString(p);

                int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();
                int sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
                string approvalTag = GetDecryptParam("approvalTag", "");
                int userID = GetDecryptParam("targetUserID", "0").ToNumber<int>();

                BLL.Sequence seqBll = new BLL.Sequence();
                BE.SequenceInfo info = seqBll.GetViewing(CurrentUserInfo.CompanyID, documentID, sequence, approvalTag, userID);

                string rtnval = "";
                if (info != null)
                {
                    rtnval = (info.IsViewing.Equals("N")) ? "OK" : GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang) + "/" + GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang) + "/" + GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region 페이지 사이즈 저장
        /// <summary>
        /// 용도 : 문서함별 사용자별 페이지사이즈 설정하기
        /// 작성자 : 이문호
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SavePageSize(string p)
        {
            try
            {
                SetDecryptString(p);

                string folderID = GetDecryptParam("folderID", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "Process"); //BL : 게시판 TL : 임시저장 IL : 중요표시

                BE.FolderPageSizeInfo info = new FolderPageSizeInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.FolderID = folderID;
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
                info.Types = types;

                DAL.FolderPageSize dal = new DAL.FolderPageSize();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 용도 : 상신취소 연동파일 확인
        /// 작성자 : 
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCustomFileName(string p)
        {
            try
            {
                SetDecryptString(p);

                string customPath = "";
                string fileName = GetDecryptParam("fileName", "");
                if (fileName.Split('.').Length > 1)
                {
                    string[] fileArr = fileName.Split('.');
                    fileName = fileArr[0] + "_Cancel." + fileArr[1];

                    customPath = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "CustomScriptRoot", "");
                    if (!customPath.Equals(""))
                    {
                        customPath += fileName;       //  /DeskPlusEIP/Legacy/aaa.aspx

                        if (!File.Exists(Server.MapPath(customPath)))
                        {
                            customPath = "";
                        }
                    }
                    else
                    {
                        customPath = "";
                    }
                }
                return customPath;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "";
            }

        }
        #endregion
    }
}
