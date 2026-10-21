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
using CarboncopyAllData = Zeospace.DeskPlus.Approval.DAL.CarboncopyAllData;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Carboncopy
{
    /// <summary>
    /// ForwardList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ForwardListReceiveAll : Base.BaseWSApproval
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
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                int formID = GetDecryptParam("formID", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                CarboncopyAllData biz = new CarboncopyAllData();
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

                DataTableReader dtr = biz.GetForwardPageList(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, formID,
                    setPageingStr[0], setPageingStr[1],
                    startDate, endDate,
                    searchField, searchValue, order, "Receive");

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.DocumentInfo info = new BE.DocumentInfo();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ForwardDocumentID = dtr["ForwardDocumentID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            info.DocNumber = dtr["DocNumber"].ToString();
                            info.Number1 = dtr["Number1"].ToString();
                            info.Number2 = dtr["Number2"].ToString();
                            info.Number3 = dtr["Number3"].ToString();
                            info.Number4 = dtr["Number4"].ToString().ToNumber<int>();

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

                            //전달자
                            info.SenderName = this.GetLangName(
                                    dtr["S_NameBase"].ToString().ToNull(), dtr["S_NameKo"].ToString().ToNull(),
                                    dtr["S_NameEn"].ToString().ToNull(), dtr["S_NameJa"].ToString().ToNull(),
                                    dtr["S_NameFr"].ToString().ToNull(), dtr["S_NameZhcn"].ToString().ToNull(),
                                    dtr["S_NameZhtw"].ToString().ToNull(), PreLang);

                            //양식명                            
                            info.FormBase = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(), dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(), dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(), dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(), PreLang);

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;

                            //완료일
                            if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                                info.CompleteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.CompleteDateStr = string.Empty;

                            //확인일
                            if (DateTime.TryParse(dtr["ConfirmDate"].ToString(), out dt))
                                info.ConfirmDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.ConfirmDateStr = string.Empty;

                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.Filecount = dalAttach.GetAttachFileCntByPath(info.CompanyID, info.ChildDocID);

                            info.Subject = dtr["Subject"].ToString();//제목 
                            info.IsGuard = dtr["IsGuard"].ToString();

                            //완료일 기준으로 
                            if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                            {
                                //날짜 비교(New Img)
                                DateTime writeDate = Convert.ToDateTime(dtr["CompleteDate"].ToString());
                                DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                TimeSpan gap = todayDate.Subtract(writeDate);
                                int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                                info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            }
                            else
                            {
                                info.ShowNew = "N";
                            }

                            info.ImpoCnt = dtr["Impocnt"].ToString().ToNumber<int>();

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
        /// Gets the list count.
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

                int formID = GetDecryptParam("formID", "0").ToNumber<int>();

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

                CarboncopyAllData biz = new CarboncopyAllData();
                icnt = biz.GetForwardPageCount(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, formID,
                    startDate, endDate,
                    searchField, searchValue, "Receive");

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

        #endregion

        #region DEL()

        /// <summary>
        /// 전달문서 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            try
            {
                SetDecryptString(p);

                string forDocumentID = GetDecryptParam("forDocumentID", "");
                if (forDocumentID.Length > 0)
                {

                    DAL.ForwardUser dal = new DAL.ForwardUser();
                    return dal.Del(CurrentUserInfo.CompanyID, forDocumentID, CurrentUserInfo.UserID);
                }

                return false;

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
