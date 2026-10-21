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


namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.DocumentEtc
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSApproval
    {
        #region 삭제

        /// <summary>
        /// 작성자 : 심재운
        /// 완결 문서 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDoc(string p)
        {
            try
            {
                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                if (docID.Length > 0)
                {
                    BLL.Document bizDoc = new BLL.Document();

                    foreach (string doc in docID.Split(','))
                    {
                        bizDoc.DelDocList(CurrentUserInfo.CompanyID, doc.ToNumber<int>(), true);
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 진행 문서 삭제하기 (현숙과장이 검토하기로 했음, 2012-07-02)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDocProcess(string p)
        {
            try
            {
                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                if (docID.Length > 0)
                {
                    BLL.DocumentProcess biz = new BLL.DocumentProcess();
                    DAL.DocumentProcess dal = new DAL.DocumentProcess();

                    foreach (string doc in docID.Split(','))
                    {
                        List<BE.DocumentProcessInfo> list = dal.GetDocument(CurrentUserInfo.CompanyID, doc.ToNumber<int>());

                        if (list.Count > 0)
                        {
                            if (list[0].AccountTag.IndexOf("ZP") == -1)
                            {
                                if ((list[0].AccountTag.IndexOf("ZQ") == -1) && (list[0].DocumentID != list[0].GroupID))
                                {
                                    //이관 받은 문서                             
                                    biz.DelDocProcess(list, true);
                                }
                                else
                                {
                                    //ZD , ZQ 문서
                                    //groupid 로 문서 가져오기
                                    List<BE.DocumentProcessInfo> listdoc = dal.GetDocProcessByGroupID(CurrentUserInfo.CompanyID, list[0].GroupID);
                                    biz.DelDocProcess(listdoc, true);
                                }
                            }
                            else
                            {
                                //ZP 문서 
                                biz.DelDocProcess(list, true);
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        #endregion

        #region 문서폐기

        /// <summary>
        /// 작성자 : kkzz52
        /// 작성일자 : 2022-08-17
        /// 작성목적 : 문서 폐기하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DisuseDoc(string p)
        {
            try
            {
                SetDecryptString(p);
                string docID = GetDecryptParam("docID", "");

                if (docID.Length > 0)
                {
                    DAL.Document dalDoc = new DAL.Document();
                    BLL.Document bizDoc = new BLL.Document();

                    foreach (string doc in docID.Split(','))
                    {
                        List<BE.DocumentInfo> list = dalDoc.GetDocList(CurrentUserInfo.CompanyID, doc.ToNumber<int>());

                        if (list.Count > 0)
                        {   
                            /*
                            // 일반문서는 폐기, 연동문서는 삭제
                            if (list[0].Etc2 == "")
                            {
                                int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();
                                dalDoc.DisuseDoc(CurrentUserInfo.CompanyID, tmpCompanyID, doc.ToNumber<int>());
                            }
                            else
                            {
                                bizDoc.DelDocList(CurrentUserInfo.CompanyID, doc.ToNumber<int>(), true);
                            }
                            */

                            /*
                            int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();
                            dalDoc.DisuseDoc(CurrentUserInfo.CompanyID, tmpCompanyID, doc.ToNumber<int>());
                            */

                            bizDoc.DisuseDocList(CurrentUserInfo.CompanyID, doc.ToNumber<int>(), CurrentUserInfo.UserID);
                        }
                        
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region 가져오기

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
        /// 양식별 문서 리스트 가져오기 (완결 및 진행)
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
                string gubun = GetDecryptParam("gubun", "");//CompleteDoc : 완결; ProcessDoc : 진행

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
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

                DataTableReader dtr = null;

                if (gubun.ToLower() == "completedoc") //완결
                {
                    order = "CompleteDate desc";
                    DAL.Document dalDoc = new DAL.Document();

                    dtr = dalDoc.GetPageListForm(CurrentUserInfo.CompanyID, formID, setPageingStr[0], setPageingStr[1],
                        startDate, endDate, searchField, searchValue, order);
                }
                else if (gubun.ToLower() == "processdoc") //진행
                {
                    order = "WriteTime desc";
                    DAL.DocumentProcess dalProc = new DAL.DocumentProcess();

                    dtr = dalProc.GetPageListForm(CurrentUserInfo.CompanyID, formID, setPageingStr[0], setPageingStr[1],
                        startDate, endDate, searchField, searchValue, order);
                }
                DAL.AttachFile dalAttach = new DAL.AttachFile();

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");
                string customPath = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "CustomScriptRoot", "");

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.DocumentInfo info = new BE.DocumentInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            info.DocNumber = dtr["DocNumber"].ToString();
                            info.Number1 = dtr["Number1"].ToString();
                            info.Number2 = dtr["Number2"].ToString();
                            info.Number3 = dtr["Number3"].ToString();
                            info.Number4 = dtr["Number4"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

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

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;


                            if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                                info.CompleteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.CompleteDateStr = string.Empty;

                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.Filecount = dalAttach.GetAttachFileCntByPath(info.CompanyID, info.ChildDocID);

                            info.Subject = dtr["Subject"].ToString();//제목      

                            info.IsGuard = dtr["IsGuard"].ToString();

                            #region 날짜 비교(New Img)
                            if (gubun.ToLower() == "completedoc") //완결)
                            {
                                if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                                {
                                    DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                    TimeSpan gap = todayDate.Subtract(dt);
                                    int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                                    info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                                }
                            }
                            else if (gubun.ToLower() == "processdoc") //진행
                            {
                                DateTime writeDate = Convert.ToDateTime(dtr["WriteTime"].ToString());
                                DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                TimeSpan gap = todayDate.Subtract(writeDate);
                                int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();

                                info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            }


                            #endregion

                            #region 연동문서체크
                            info.IsLegacy = dtr["IsLegacy"].ToString();             //커스텀연동정보
                            info.CustomFileName = dtr["CustomFileName"].ToString(); //커스텀연동정보

                            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(info.IsLegacy);
                            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(info.CustomFileName);

                            if (info.IsLegacy.Equals("1") && !info.CustomFileName.Equals("") && info.CustomFileName.Split('.').Length > 1 && !customPath.Equals(""))
                            {
                                if (!File.Exists(Server.MapPath(customPath + info.CustomFileName.Split('.')[0] + "_Cancel." + info.CustomFileName.Split('.')[1])))
                                {
                                    info.IsLegacy = "0";
                                    info.CustomFileName = "";
                                }
                            }

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
        /// 작성자 : 심재운
        /// 양식별 문서 카운트 가져오기 (완결 및 진행)
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
                string gubun = GetDecryptParam("gubun", "");//CompleteDoc : 완결; ProcessDoc : 진행

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

                if (gubun.ToLower() == "completedoc") //완결
                {
                    DAL.Document dalDoc = new DAL.Document();
                    icnt = dalDoc.GetPageCountForm(CurrentUserInfo.CompanyID, formID,
                        startDate, endDate, searchField, searchValue);
                }
                else if (gubun.ToLower() == "processdoc") //진행
                {
                    DAL.DocumentProcess dalProc = new DAL.DocumentProcess();
                    icnt = dalProc.GetPageCountForm(CurrentUserInfo.CompanyID, formID,
                        startDate, endDate, searchField, searchValue);
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

        #endregion
    }
}
