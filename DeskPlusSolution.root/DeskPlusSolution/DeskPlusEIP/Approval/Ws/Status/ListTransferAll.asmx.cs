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
    /// ListTransferAll의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListTransferAll : Base.BaseWSApproval
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
        /// 결재상황조회 - 이관받은 문서 리스트 가져오기
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
                string proxyUserId = GetDecryptParam("proxyUserID", "0");

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

                // 대결의뢰자 선택시 userID 대신 proxyUserID사용
                DataTableReader dtr;
                if (proxyUserId.ToNumber<int>() > 0)
                {
                    dtr = biz.GetPageListTransfer(CurrentUserInfo.CompanyID,
                        proxyUserId.ToNumber<int>(), setPageingStr[0], setPageingStr[1],
                        startDate, endDate, searchField, searchValue, order);
                }
                else
                {
                    dtr = biz.GetPageListTransfer(CurrentUserInfo.CompanyID,
                        CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1],
                        startDate, endDate, searchField, searchValue, order);
                }

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
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            info.Number3 = dtr["Number3"].ToString();
                            info.ApprovalState = dtr["ApprovalState"].ToString();

                            #region 이관 보낸 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["SendNameBase"].ToString().ToNull(), dtr["SendNameKo"].ToString().ToNull(),
                                    dtr["SendNameEn"].ToString().ToNull(), dtr["SendNameJa"].ToString().ToNull(),
                                    dtr["SendNameFr"].ToString().ToNull(), dtr["SendNameZhcn"].ToString().ToNull(),
                                    dtr["SendNameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이관 보낸 부서 다국어 설정
                            info.DeptBase = this.GetLangName(
                                    dtr["SendDeptBase"].ToString().ToNull(), dtr["SendDeptKo"].ToString().ToNull(),
                                    dtr["SendDeptEn"].ToString().ToNull(), dtr["SendDeptJa"].ToString().ToNull(),
                                    dtr["SendDeptFr"].ToString().ToNull(), dtr["SendDeptZhcn"].ToString().ToNull(),
                                    dtr["SendDeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이관 보낸 직급 다국어 설정
                            info.TitleBase = this.GetLangName(
                                    dtr["SendTitleBase"].ToString().ToNull(), dtr["SendTitleKo"].ToString().ToNull(),
                                    dtr["SendTitleEn"].ToString().ToNull(), dtr["SendTitleJa"].ToString().ToNull(),
                                    dtr["SendTitleFr"].ToString().ToNull(), dtr["SendTitleZhcn"].ToString().ToNull(),
                                    dtr["SendTitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이관 보낸 직책 다국어 설정
                            info.DutyBase = this.GetLangName(
                                    dtr["SendDutyBase"].ToString().ToNull(), dtr["SendDutyKo"].ToString().ToNull(),
                                    dtr["SendDutyEn"].ToString().ToNull(), dtr["SendDutyJa"].ToString().ToNull(),
                                    dtr["SendDutyFr"].ToString().ToNull(), dtr["SendDutyZhcn"].ToString().ToNull(),
                                    dtr["SendDutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 문서양식명 다국어 설정
                            info.FormBase = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(), dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(), dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(), dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.ReceiveUserID = dtr["ReceiveUserID"].ToString().ToNumber<int>();
                            info.ReceiveDeptID = dtr["ReceiveDeptID"].ToString().ToNumber<int>();
                            info.PassedDocID = dtr["PassedDocID"].ToString().ToNumber<int>();

                            DateTime dt;
                            // 이관일
                            if (DateTime.TryParse(dtr["PassDate"].ToString(), out dt))
                                info.PassDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.PassDateStr = string.Empty;
                            // 상신일
                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;


                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.Filecount = dalAttach.GetAttachFileCntByPath(info.CompanyID, info.ChildDocID);

                            info.Subject = dtr["Subject"].ToString();//제목                            

                            info.IsGuard = dtr["IsGuard"].ToString();

                            #region 날짜 비교(New Img)
                            DateTime passDate = Convert.ToDateTime(dtr["PassDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(passDate);
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
                throw;
            }
        }



        /// <summary>
        /// 결재상황조회 이관받은 문서 카운트
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
                string proxyUserId = GetDecryptParam("proxyUserID", "0");

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
                // 대결의뢰자 선택시 userID 대신 proxyUserID사용
                if (proxyUserId.ToNumber<int>() > 0)
                {
                    icnt = biz.GetPageCountTransfer(CurrentUserInfo.CompanyID,
                        proxyUserId.ToNumber<int>(), startDate, endDate, searchField, searchValue);
                }
                else
                {
                    icnt = biz.GetPageCountTransfer(CurrentUserInfo.CompanyID,
                        CurrentUserInfo.UserID, startDate, endDate, searchField, searchValue);
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

        #region 첨부파일

        /// <summary>
        /// 작성자   : shimpark
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



        #region DEL()

        #endregion
    }
}
