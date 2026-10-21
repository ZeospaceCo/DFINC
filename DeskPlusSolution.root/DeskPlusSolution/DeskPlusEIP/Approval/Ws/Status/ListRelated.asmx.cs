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
using HtmlAgilityPack;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Status
{
    /// <summary>
    /// ListReport의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListRelated : Base.BaseWSApproval
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
        /// 결재상황조회 - 관계있는 문서 리스트 가져오기
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


                DataTableReader dtr = biz.GetPageListRelated(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, CurrentUserInfo.DeptID, setPageingStr[0], setPageingStr[1],
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

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.NextUserID = dtr["NextUserID"].ToString().ToNumber<int>();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            info.Number3 = dtr["Number3"].ToString();
                            info.ApprovalState = dtr["ApprovalState"].ToString();

                            info.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                            info.ApprovalTag = dtr["ApprovalTag"].ToString();

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


                            #region 결재취소 가능여부 체크(Y,N)
                            //info.IsCancelApproval = biz.GetIsCancelApproval(info.CompanyID, info.DocumentID, info.Sequence, info.ApprovalTag);
                            info.IsCancelApproval = dtr["IsCancelApproval"].ToString();     //결재취소 가능여부 체크(Y,N)
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
        /// 결재상황조회 관계있는 문서 카운트
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

                DAL.DocumentProcess biz = new DAL.DocumentProcess();
                icnt = biz.GetPageCountRelated(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, CurrentUserInfo.DeptID, startDate, endDate,
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
        /// 결재상황조회 - 관계있는 문서 리스트 가져오기
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


                DataTableReader dtr = biz.GetPageListRelatedSlip(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, CurrentUserInfo.DeptID, setPageingStr[0], setPageingStr[1],
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

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.NextUserID = dtr["NextUserID"].ToString().ToNumber<int>();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            info.Number3 = dtr["Number3"].ToString();
                            info.ApprovalState = dtr["ApprovalState"].ToString();

                            info.Sequence = dtr["Sequence"].ToString().ToNumber<int>();
                            info.ApprovalTag = dtr["ApprovalTag"].ToString();

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


                            #region 결재취소 가능여부 체크(Y,N)
                            //info.IsCancelApproval = biz.GetIsCancelApproval(info.CompanyID, info.DocumentID, info.Sequence, info.ApprovalTag);
                            info.IsCancelApproval = dtr["IsCancelApproval"].ToString();     //결재취소 가능여부 체크(Y,N)
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
        /// 결재상황조회 관계있는 문서 카운트
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
                icnt = biz.GetPageCountRelatedSlip(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, CurrentUserInfo.DeptID, startDate, endDate,
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

        #region 결재 취소

        /// <summary>
        /// 작성자   : 이문호
        /// 작성목적 : 결재 취소
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string CancelApproval(string p)
        {
            try
            {
                SetDecryptString(p);
                int documentID = GetDecryptParam("documentID", "0").ToNumber<Int32>();
                int sequence = GetDecryptParam("sequence", "0").ToNumber<Int32>();
                string approvalTag = GetDecryptParam("approvalTag", "");

                string rtnval = "Y";

                #region 결재취소 가능여부 체크


                //해당문서가 결재취소를 할 수 있는 상태인지 체크
                DAL.DocumentProcess dal = new DAL.DocumentProcess();
                string ApproveYN = dal.GetIsCancelApproval(CurrentUserInfo.CompanyID, documentID, sequence, approvalTag);
                if (ApproveYN != "Y")
                {
                    //결재취소 불가
                    rtnval = "N";
                }

                //병렬사용자중에 결재 한 사람이 있는지 체크
                string ZAApproveYN = dal.GetIsCancelZA(CurrentUserInfo.CompanyID, documentID, sequence, approvalTag);

                #endregion

                if (rtnval != "N")
                {
                    #region 문서내용 처리 - 인장취소
                    BE.DocumentProcessInfo dpInfo = null;

                    #region 문서정보가져오기
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                    List<BE.DocumentProcessInfo> list = dpDal.GetDocument(CurrentUserInfo.CompanyID, documentID);

                    foreach (BE.DocumentProcessInfo info in list)
                    {
                        dpInfo = new BE.DocumentProcessInfo();
                        dpInfo.CompanyID = info.CompanyID;
                        dpInfo.DocumentID = info.DocumentID;
                        dpInfo.ParentDocumentID = info.ParentDocumentID;
                        dpInfo.Number1 = info.Number1;
                        dpInfo.Number2 = info.Number2;
                        dpInfo.Number3 = info.Number3;
                        dpInfo.WriteTime = info.WriteTime;
                        dpInfo.WriteTimeYMD = info.WriteTimeYMD;
                        dpInfo.EmployeeNumber = info.EmployeeNumber;
                        dpInfo.DeptCodeID = info.DeptCodeID;

                        dpInfo.DeptBase = info.DeptBase;
                        dpInfo.DeptKo = info.DeptKo;
                        dpInfo.DeptEn = info.DeptEn;
                        dpInfo.DeptJa = info.DeptJa;
                        dpInfo.DeptFr = info.DeptFr;
                        dpInfo.DeptZhcn = info.DeptZhcn;
                        dpInfo.DeptZhtw = info.DeptZhtw;
                        dpInfo.TitleBase = info.TitleBase;
                        dpInfo.TitleKo = info.TitleKo;
                        dpInfo.TitleEn = info.TitleEn;
                        dpInfo.TitleJa = info.TitleJa;
                        dpInfo.TitleFr = info.TitleFr;
                        dpInfo.TitleZhcn = info.TitleZhcn;
                        dpInfo.TitleZhtw = info.TitleZhtw;
                        dpInfo.DutyBase = info.DutyBase;
                        dpInfo.DutyKo = info.DutyKo;
                        dpInfo.DutyEn = info.DutyEn;
                        dpInfo.DutyJa = info.DutyJa;
                        dpInfo.DutyFr = info.DutyFr;
                        dpInfo.DutyZhcn = info.DutyZhcn;
                        dpInfo.DutyZhtw = info.DutyZhtw;

                        dpInfo.UserID = info.UserID;
                        dpInfo.NameBase = info.NameBase;
                        dpInfo.NameKo = info.NameKo;
                        dpInfo.NameEn = info.NameEn;
                        dpInfo.NameJa = info.NameJa;
                        dpInfo.NameFr = info.NameFr;
                        dpInfo.NameZhcn = info.NameZhcn;
                        dpInfo.NameZhtw = info.NameZhtw;
                        dpInfo.ManyName = info.ManyName;
                        dpInfo.LifeTime = info.LifeTime;
                        dpInfo.AccessTitleID = info.AccessTitleID;
                        dpInfo.AccountTag = info.AccountTag;
                        dpInfo.Subject = info.Subject;
                        dpInfo.IsConfirm = info.IsConfirm;
                        dpInfo.IsLegacy = info.IsLegacy;
                        dpInfo.CustomFileName = info.CustomFileName;
                        dpInfo.IsAfterSubmitCall = info.IsAfterSubmitCall;
                        dpInfo.IsDRM = info.IsDRM;
                        dpInfo.IsGuard = info.IsGuard;
                        dpInfo.IsLinkedEtc = info.IsLinkedEtc;
                        dpInfo.IsUseForce = info.IsUseForce;
                        dpInfo.IsUseFlow = info.IsUseFlow;
                        dpInfo.FormID = info.FormID;
                        dpInfo.FormBase = info.FormBase;
                        dpInfo.FormKo = info.FormKo;
                        dpInfo.FormEn = info.FormEn;
                        dpInfo.FormJa = info.FormJa;
                        dpInfo.FormFr = info.FormFr;
                        dpInfo.FormZhcn = info.FormZhcn;
                        dpInfo.FormZhtw = info.FormZhtw;
                        dpInfo.FormMany = info.FormMany;

                        dpInfo.UpPathID = info.UpPathID;
                        dpInfo.GroupID = info.GroupID;
                        dpInfo.LastDocID = info.LastDocID;

                        // 변경되는 값 할당.
                        if (approvalTag.Substring(0, 2) == "ZA")
                        {
                            //병렬사용자중에 결재 한 사람이 없을 때
                            if (sequence.Equals(2) && ZAApproveYN == "Y")
                            {
                                dpInfo.ApprovalState = "W";
                            }
                            else
                            {
                                dpInfo.ApprovalState = "I";
                            }
                        }
                        else if (dpInfo.AccountTag.IndexOf("ZQ") > -1)
                        {
                            dpInfo.ApprovalState = "I";
                        }
                        else
                        {
                            dpInfo.ApprovalState = (sequence.Equals(2)) ? "W" : "I";
                        }

                        dpInfo.ContentHTML = info.ContentHTML;
                        dpInfo.ContentTEXT = info.ContentTEXT;
                        dpInfo.Etc1 = dpInfo.Etc1;
                        dpInfo.Etc2 = dpInfo.Etc2;
                        dpInfo.Etc3 = dpInfo.Etc3;
                        dpInfo.Etc4 = dpInfo.Etc4;
                        dpInfo.Etc5 = dpInfo.Etc5;
                        dpInfo.Etc6 = dpInfo.Etc6;
                        dpInfo.Etc7 = dpInfo.Etc7;
                        dpInfo.Etc8 = dpInfo.Etc8;
                        dpInfo.Etc9 = dpInfo.Etc9;
                        dpInfo.Etc10 = dpInfo.Etc10;
                    }
                    #endregion

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(dpInfo.ContentHTML);
                    HtmlNode body = doc.DocumentNode.SelectSingleNode("//body");

                    #region Info처리
                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO0']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO0']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO1']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO1']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO2']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO2']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO3']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO3']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO4']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO4']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO5']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO5']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO6']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO6']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO7']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO7']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO8']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO8']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO9']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO9']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO10']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO10']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO11']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO11']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO12']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO12']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO13']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO13']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO14']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO14']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }

                    if (doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO15']") != null)
                    {
                        foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "_INFO15']"))
                        {
                            span.InnerHtml = "&nbsp;";
                        }
                    }
                    #endregion

                    // 4. 결재인장          
                    foreach (HtmlNode span in doc.DocumentNode.SelectNodes("//span[@id='" + approvalTag + "']"))
                    {
                        span.InnerHtml = CurrentUserInfo.Name;
                    }

                    dpInfo.ContentHTML = body.ParentNode.OuterHtml;
                    #endregion

                    #region 결재라인 처리
                    BE.SequenceInfo nextinfo = null;
                    DAL.Sequence seqdal = new DAL.Sequence();
                    List<BE.SequenceInfo> listseq = seqdal.GetSeqLine(CurrentUserInfo.CompanyID, documentID).Where(x => x.Sequence == sequence + 1).ToList<BE.SequenceInfo>();
                    if (listseq.Count > 0)
                    {
                        nextinfo = listseq[0];
                    }
                    #endregion

                    BE.UserInfo sendInfo = GetSendUserInfo("", "");

                    BLL.DocumentProcess bll = new BLL.DocumentProcess();
                    bool reseult = bll.ApproveSCancelRelated(dpInfo, nextinfo, sendInfo, sequence, approvalTag);

                    if (!reseult)
                        rtnval = "E";
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 이문호
        /// 작성목적 : 결재 취소
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string Test(string p)
        {
            return "test~";
        }
        #endregion



        #region DEL()

        #endregion
    }
}
