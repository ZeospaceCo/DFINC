using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.Transfer
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
        #region GET

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

        #region GET - 부서별 문서함

        /// <summary>
        /// 작성자 : 
        /// 부서별문서함
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListDept(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                int deptID = GetDecryptParam("srhID", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.Document biz = new DAL.Document();
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

                DataTableReader dtr = biz.GetFormPageListByDeptIDAdmin(CurrentUserInfo.CompanyID, deptID, setPageingStr[0], setPageingStr[1], startDate, endDate, searchField, searchValue, order);

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

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["CompleteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

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
        /// Gets the list count.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCountDept(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                int deptID = GetDecryptParam("srhID", "0").ToNumber<int>();

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

                DAL.Document biz = new DAL.Document();
                icnt = biz.GetFormPageListCountByDeptIDAdmin(CurrentUserInfo.CompanyID, deptID, startDate, endDate, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region GET - 참조 문서함(부서)

        /// <summary>
        /// 작성자 : 
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListCarbonDept(string p)
        {

            //수정필요/

            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Folder.Carboncopy.List).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                int deptID = GetDecryptParam("srhID", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.CarbonCopyUser biz = new DAL.CarbonCopyUser();
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

                DataTableReader dtr = biz.GetPageListDept(CurrentUserInfo.CompanyID, deptID, 0, setPageingStr[0], setPageingStr[1], startDate, endDate, searchField, searchValue, order);

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

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = "";


                            if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                                info.CompleteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.CompleteDateStr = "";

                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.Filecount = dalAttach.GetAttachFileCntByPath(info.CompanyID, info.ChildDocID);
                            info.Subject = dtr["Subject"].ToString();//제목   

                            //확인일자
                            if (DateTime.TryParse(dtr["ViewingDate"].ToString(), out dt))
                                info.ViewingDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.ViewingDateStr = "";

                            info.IsGroup = dtr["IsGroup"].ToString();
                            info.IsGroupStr = (dtr["IsGroup"].ToString().Equals("G")) ? this.GetResource("TitGroup") : this.GetResource("TitMember");

                            //완료일 기준으로 
                            if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                            {
                                #region 날짜 비교(New Img)
                                DateTime writeDate = Convert.ToDateTime(dtr["CompleteDate"].ToString());
                                DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                TimeSpan gap = todayDate.Subtract(writeDate);
                                int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                                info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            }
                            else
                                info.ShowNew = "N";

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
        /// Gets the list count.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCountCarbonDept(string p)
        {

            //수정필ㅇ쇼


            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int deptID = GetDecryptParam("srhID", "0").ToNumber<int>();
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

                DAL.CarbonCopyUser biz = new DAL.CarbonCopyUser();
                icnt = biz.GetPageCountDept(CurrentUserInfo.CompanyID, deptID, 0, startDate, endDate, searchField, searchValue);
                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region GET - 개인별 문서함

        /// <summary>
        /// 작성자 : 
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListPrivate(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.PrivateDocument biz = new DAL.PrivateDocument();
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

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, userID, 0, setPageingStr[0], setPageingStr[1], startDate, endDate, searchField, searchValue, order);
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
                            info.FolderID = 0; //상신문서함은 폴더개념이 없다. 기본값 0
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

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["CompleteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

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
        /// Gets the list count.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCountPrivate(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

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

                DAL.PrivateDocument biz = new DAL.PrivateDocument();
                icnt = biz.GetPageCount(CurrentUserInfo.CompanyID, userID, 0, startDate, endDate, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region GET - 결재 문서함

        /// <summary>
        /// 작성자 : 
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListApprove(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.ApprovalDocument biz = new DAL.ApprovalDocument();
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

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, userID, 0, setPageingStr[0], setPageingStr[1], startDate, endDate, searchField, searchValue, order);

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
                            info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            info.FolderID = 0;
                            info.ApprovalDocumentID = dtr["ApprovalDocumentID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
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

                            info.Proxy = dtr["Proxy"].ToString();//대결 여부

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["CompleteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

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
        /// Gets the list count.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCountApprove(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

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

                DAL.ApprovalDocument biz = new DAL.ApprovalDocument();
                icnt = biz.GetPageCount(CurrentUserInfo.CompanyID, userID, 0, startDate, endDate, searchField, searchValue);
                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region GET - 전결 문서함

        /// <summary>
        /// 작성자 : 
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListArbitrary(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.ArbitraryDocument biz = new DAL.ArbitraryDocument();
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

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, userID, 0, setPageingStr[0], setPageingStr[1], startDate, endDate, searchField, searchValue, order);

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
                            info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            info.FolderID = 0;
                            info.ArbitraryDocumentID = dtr["ArbitraryDocumentID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
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

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["CompleteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

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
        /// Gets the list count.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCountArbitrary(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

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

                DAL.ArbitraryDocument biz = new DAL.ArbitraryDocument();
                icnt = biz.GetPageCount(CurrentUserInfo.CompanyID, userID, 0, startDate, endDate, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region GET - 참조 문서함(개인)

        /// <summary>
        /// 작성자 : 
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListCarbon(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Folder.Carboncopy.List).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.CarbonCopyUser biz = new DAL.CarbonCopyUser();
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

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, userID, 0,setPageingStr[0], setPageingStr[1],startDate, endDate, searchField, searchValue, order);

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

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = "";


                            if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                                info.CompleteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.CompleteDateStr = "";

                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.Filecount = dalAttach.GetAttachFileCntByPath(info.CompanyID, info.ChildDocID);
                            info.Subject = dtr["Subject"].ToString();//제목   

                            //확인일자
                            if (DateTime.TryParse(dtr["ViewingDate"].ToString(), out dt))
                                info.ViewingDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.ViewingDateStr = "";

                            info.IsGroup = dtr["IsGroup"].ToString();
                            info.IsGroupStr = (dtr["IsGroup"].ToString().Equals("G")) ? this.GetResource("TitGroup") : this.GetResource("TitMember");

                            //완료일 기준으로 
                            if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                            {
                                #region 날짜 비교(New Img)
                                DateTime writeDate = Convert.ToDateTime(dtr["CompleteDate"].ToString());
                                DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                TimeSpan gap = todayDate.Subtract(writeDate);
                                int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                                info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            }
                            else
                                info.ShowNew = "N";

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
        /// Gets the list count.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCountCarbon(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();
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

                DAL.CarbonCopyUser biz = new DAL.CarbonCopyUser();
                icnt = biz.GetPageCount(CurrentUserInfo.CompanyID, userID, 0,startDate, endDate, searchField, searchValue);
                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region GET - 전달 문서함

        /// <summary>
        /// 작성자 : 
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListForward(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.ForwardUser biz = new DAL.ForwardUser();
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

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, userID, 0,setPageingStr[0], setPageingStr[1],startDate, endDate, searchField, searchValue, order);

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
                                info.ShowNew = "N";

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
        public int GetListCountForward(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

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

                DAL.ForwardUser biz = new DAL.ForwardUser();
                icnt = biz.GetPageCount(CurrentUserInfo.CompanyID, userID, 0,startDate, endDate, searchField, searchValue);
                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region GET - 받은회람 문서함

        /// <summary>
        /// 작성자 : 
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListCirReceive(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.CirculationDocument biz = new DAL.CirculationDocument();

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

                DataTableReader dtr = biz.GetPageListRecv(CurrentUserInfo.CompanyID, userID, 0,setPageingStr[0], setPageingStr[1],startDate, endDate, searchField, searchValue, order);

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                ObjList listobj = new ObjList();
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.CirculationDocumentInfo info = new BE.CirculationDocumentInfo();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.CirculationDocumentID = dtr["CirculationDocumentID"].ToString().ToNumber<int>();
                            info.CirculationUserID = dtr["CirculationUserID"].ToString().ToNumber<int>();

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

                            //전송자
                            info.SendNameBase = this.GetLangName(
                                    dtr["SendNameBase"].ToString().ToNull(), dtr["SendNameKo"].ToString().ToNull(),
                                    dtr["SendNameEn"].ToString().ToNull(), dtr["SendNameJa"].ToString().ToNull(),
                                    dtr["SendNameFr"].ToString().ToNull(), dtr["SendNameZhcn"].ToString().ToNull(),
                                    dtr["SendNameZhtw"].ToString().ToNull(), PreLang);

                            //양식명                            
                            info.FormBase = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(), dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(), dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(), dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(), PreLang);

                            DateTime dt;

                            //상신일
                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;

                            //결재완료일
                            if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                                info.CompleteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.CompleteDateStr = string.Empty;

                            //확인일
                            if (DateTime.TryParse(dtr["ConFirmDate"].ToString(), out dt))
                                info.ConfirmDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.ConfirmDateStr = string.Empty;

                            //회람일자
                            if (DateTime.TryParse(dtr["CirculationDate"].ToString(), out dt))
                                info.CirculationDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.CirculationDateStr = string.Empty;

                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.Filecount = dtr["Filecount"].ToString().ToNumber<int>();

                            info.Subject = dtr["Subject"].ToString();//제목  
                            info.Memo = dtr["Memo"].ToString();//메모  

                            if (DateTime.TryParse(dtr["CirculationDate"].ToString(), out dt))
                            {
                                #region 날짜 비교(New Img)
                                DateTime writeDate = Convert.ToDateTime(dtr["CirculationDate"].ToString());
                                DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                TimeSpan gap = todayDate.Subtract(writeDate);
                                int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                                info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                                #endregion
                            }
                            else
                                info.ShowNew = "N";

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
        public int GetListCountCirReceive(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

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

                DAL.CirculationDocument biz = new DAL.CirculationDocument();
                icnt = biz.GetPageCountRecv(CurrentUserInfo.CompanyID, userID, 0,startDate, endDate, searchField, searchValue);
                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region GET - 보낸회람 문서함

        /// <summary>
        /// 작성자 : 
        /// 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListCirSend(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.CirculationDocument biz = new DAL.CirculationDocument();

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

                DataTableReader dtr = biz.GetPageListSend(CurrentUserInfo.CompanyID, userID, 0,setPageingStr[0], setPageingStr[1],startDate, endDate, searchField, searchValue, order);

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                ObjList listobj = new ObjList();
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.CirculationDocumentInfo info = new BE.CirculationDocumentInfo();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.CirculationDocumentID = dtr["CirculationDocumentID"].ToString().ToNumber<int>();
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

                            //양식명                            
                            info.FormBase = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(), dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(), dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(), dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(), PreLang);

                            DateTime dt;
                            //상신일
                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;

                            //결재완료일
                            if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                                info.CompleteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.CompleteDateStr = string.Empty;

                            //회람일자
                            if (DateTime.TryParse(dtr["CirculationDate"].ToString(), out dt))
                                info.CirculationDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.CirculationDateStr = string.Empty;

                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.Filecount = dtr["Filecount"].ToString().ToNumber<int>();

                            info.Subject = dtr["Subject"].ToString();//제목

                            if (DateTime.TryParse(dtr["CirculationDate"].ToString(), out dt))
                            {
                                #region 날짜 비교(New Img)
                                DateTime writeDate = Convert.ToDateTime(dtr["CirculationDate"].ToString());
                                DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                TimeSpan gap = todayDate.Subtract(writeDate);
                                int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                                info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                                #endregion
                            }
                            else
                                info.ShowNew = "N";

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
        public int GetListCountCirSend(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchTerm = GetDecryptParam("searchTerm", "");              //빈값
                string searchTermValue = GetDecryptParam("searchTermValue", "");    //빈값
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int userID = GetDecryptParam("srhID", "0").ToNumber<int>();

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

                DAL.CirculationDocument biz = new DAL.CirculationDocument();
                icnt = biz.GetPageCountSend(CurrentUserInfo.CompanyID,userID, 0,startDate, endDate,searchField, searchValue);
                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region 부서별 문서함 - 삭제부서 (GarbageData)

        /// <summary>
        /// 작성자 : 
        /// 부서별문서함
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetListDeptGarbage(string p)
        {
            try
            {
                SetDecryptString(p);

                DAL.PublicDocument dal = new DAL.PublicDocument();
                DataTableReader dtr = dal.GetGarbage(CurrentUserInfo.CompanyID);

                ObjList listobj = new ObjList();
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            
                            Zeospace.DeskPlus.Approval.BE.DocumentInfo info = new BE.DocumentInfo();
                            info.CompanyID = CurrentUserInfo.CompanyID;
                            info.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                            info.DeptBase = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            info.ChildCount = dtr["DocCnt"].ToString().ToNumber<int>();

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

        #endregion

        #endregion

        #region SET

        #region SET - 부서별 문서함

        /// <summary>
        /// 부서별 문서함 이관
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetTransferDept(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);

            string ids = GetDecryptParam("ids", "").Trim(',');
            string isCopy = GetDecryptParam("isCopy", "0");     // 1:복사, 0:이관
            string isAll = GetDecryptParam("isAll", "0");
            int sendDeptID = GetDecryptParam("sendID", "0").ToNumber<int>();
            int receiveDeptID = GetDecryptParam("receiveID", "0").ToNumber<int>();

            if (isCopy.Equals("") || isAll.Equals("") || sendDeptID.Equals(0) || receiveDeptID.Equals(0))
            {
                return false;
            }
            if (isAll.Equals("0") && ids.Equals(""))        //선택이관인데, 대상문서값이 없는경우, 에러.
            {
                return false;
            }

            bool rtnval = false;
            DAL.PublicDocument dal = new DAL.PublicDocument();
            if (isCopy.Equals("1"))
            {
                rtnval = dal.TransferCopy(CurrentUserInfo.CompanyID, sendDeptID, receiveDeptID, isAll, ids);
            }
            else
            {
                rtnval = dal.Transfer(CurrentUserInfo.CompanyID, sendDeptID, receiveDeptID, isAll, ids);
            }

            return rtnval;
        }

        #endregion

        #region SET - 참조 문서함 (부서)

        /// <summary>
        /// 참조 문서함 이관
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetTransferCarbonDept(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);

            string ids = GetDecryptParam("ids", "").Trim(',');
            string isCopy = GetDecryptParam("isCopy", "0");     // 1:복사, 0:이관
            string isAll = GetDecryptParam("isAll", "0");
            int sendDeptID = GetDecryptParam("sendID", "0").ToNumber<int>();
            int receiveDeptID = GetDecryptParam("receiveID", "0").ToNumber<int>();

            if (isCopy.Equals("") || isAll.Equals("") || sendDeptID.Equals(0) || receiveDeptID.Equals(0))
            {
                return false;
            }
            if (isAll.Equals("0") && ids.Equals(""))        //선택이관인데, 대상문서값이 없는경우, 에러.
            {
                return false;
            }

            bool rtnval = false;
            DAL.CarbonCopyUser dal = new DAL.CarbonCopyUser();
            if (isCopy.Equals("1"))
            {
                rtnval = dal.TransferCopyDept(CurrentUserInfo.CompanyID, sendDeptID, receiveDeptID, isAll, ids);
            }
            else
            {
                rtnval = dal.TransferDept(CurrentUserInfo.CompanyID, sendDeptID, receiveDeptID, isAll, ids);
            }

            return rtnval;
        }

        #endregion

        #region SET - 개인별 문서함

        /// <summary>
        /// 개인별 문서함 이관
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetTransferPrivate(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);

            string ids = GetDecryptParam("ids", "").Trim(',');
            string isCopy = GetDecryptParam("isCopy", "0");     // 1:복사, 0:이관
            string isAll = GetDecryptParam("isAll", "0");
            int sendUserID = GetDecryptParam("sendID", "0").ToNumber<int>();
            int receiveUserID = GetDecryptParam("receiveID", "0").ToNumber<int>();

            if (isCopy.Equals("") || isAll.Equals("") || sendUserID.Equals(0) || receiveUserID.Equals(0))
            {
                return false;
            }
            if (isAll.Equals("0") && ids.Equals(""))        //선택이관인데, 대상문서값이 없는경우, 에러.
            {
                return false;
            }

            bool rtnval = false;
            DAL.PrivateDocument dal = new DAL.PrivateDocument();
            if (isCopy.Equals("1"))
            {
                rtnval = dal.TransferCopy(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }
            else
            {
                rtnval = dal.Transfer(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }

            return rtnval;
        }

        #endregion

        #region SET - 결재 문서함

        /// <summary>
        /// 결재 문서함 이관
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetTransferApprove(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);

            string ids = GetDecryptParam("ids", "").Trim(',');
            string isCopy = GetDecryptParam("isCopy", "0");     // 1:복사, 0:이관
            string isAll = GetDecryptParam("isAll", "0");
            int sendUserID = GetDecryptParam("sendID", "0").ToNumber<int>();
            int receiveUserID = GetDecryptParam("receiveID", "0").ToNumber<int>();

            if (isCopy.Equals("") || isAll.Equals("") || sendUserID.Equals(0) || receiveUserID.Equals(0))
            {
                return false;
            }
            if (isAll.Equals("0") && ids.Equals(""))        //선택이관인데, 대상문서값이 없는경우, 에러.
            {
                return false;
            }

            bool rtnval = false;
            DAL.ApprovalDocument dal = new DAL.ApprovalDocument();
            if (isCopy.Equals("1"))
            {
                rtnval = dal.TransferCopy(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }
            else
            {
                rtnval = dal.Transfer(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }

            return rtnval;
        }

        #endregion

        #region SET - 전결 문서함

        /// <summary>
        /// 전결 문서함 이관
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetTransferArbitrary(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);

            string ids = GetDecryptParam("ids", "").Trim(',');
            string isCopy = GetDecryptParam("isCopy", "0");     // 1:복사, 0:이관
            string isAll = GetDecryptParam("isAll", "0");
            int sendUserID = GetDecryptParam("sendID", "0").ToNumber<int>();
            int receiveUserID = GetDecryptParam("receiveID", "0").ToNumber<int>();

            if (isCopy.Equals("") || isAll.Equals("") || sendUserID.Equals(0) || receiveUserID.Equals(0))
            {
                return false;
            }
            if (isAll.Equals("0") && ids.Equals(""))        //선택이관인데, 대상문서값이 없는경우, 에러.
            {
                return false;
            }

            bool rtnval = false;
            DAL.ArbitraryDocument dal = new DAL.ArbitraryDocument();
            if (isCopy.Equals("1"))
            {
                rtnval = dal.TransferCopy(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }
            else
            {
                rtnval = dal.Transfer(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }

            return rtnval;
        }

        #endregion

        #region SET - 참조 문서함 (개인)

        /// <summary>
        /// 참조 문서함 이관
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetTransferCarbon(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);

            string ids = GetDecryptParam("ids", "").Trim(',');
            string isCopy = GetDecryptParam("isCopy", "0");     // 1:복사, 0:이관
            string isAll = GetDecryptParam("isAll", "0");
            int sendUserID = GetDecryptParam("sendID", "0").ToNumber<int>();
            int receiveUserID = GetDecryptParam("receiveID", "0").ToNumber<int>();

            if (isCopy.Equals("") || isAll.Equals("") || sendUserID.Equals(0) || receiveUserID.Equals(0))
            {
                return false;
            }
            if (isAll.Equals("0") && ids.Equals(""))        //선택이관인데, 대상문서값이 없는경우, 에러.
            {
                return false;
            }

            bool rtnval = false;
            DAL.CarbonCopyUser dal = new DAL.CarbonCopyUser();
            if (isCopy.Equals("1"))
            {
                rtnval = dal.TransferCopy(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }
            else
            {
                rtnval = dal.Transfer(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }

            return rtnval;
        }

        #endregion

        #region SET - 전달 문서함

        /// <summary>
        /// 전달 문서함 이관
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetTransferForward(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);

            string ids = GetDecryptParam("ids", "").Trim(',');
            string isCopy = GetDecryptParam("isCopy", "0");     // 1:복사, 0:이관
            string isAll = GetDecryptParam("isAll", "0");
            int sendUserID = GetDecryptParam("sendID", "0").ToNumber<int>();
            int receiveUserID = GetDecryptParam("receiveID", "0").ToNumber<int>();

            if (isCopy.Equals("") || isAll.Equals("") || sendUserID.Equals(0) || receiveUserID.Equals(0))
            {
                return false;
            }
            if (isAll.Equals("0") && ids.Equals(""))        //선택이관인데, 대상문서값이 없는경우, 에러.
            {
                return false;
            }

            bool rtnval = false;
            DAL.ForwardDocument dal = new DAL.ForwardDocument();
            if (isCopy.Equals("1"))
            {
                rtnval = dal.TransferCopy(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }
            else
            {
                rtnval = dal.Transfer(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }

            return rtnval;
        }

        #endregion

        #region SET - 받은회람 문서함

        /// <summary>
        /// 받은회람 문서함 이관
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetTransferCirReceive(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);

            string ids = GetDecryptParam("ids", "").Trim(',');
            string isCopy = GetDecryptParam("isCopy", "0");     // 1:복사, 0:이관
            string isAll = GetDecryptParam("isAll", "0");
            int sendUserID = GetDecryptParam("sendID", "0").ToNumber<int>();
            int receiveUserID = GetDecryptParam("receiveID", "0").ToNumber<int>();

            if (isCopy.Equals("") || isAll.Equals("") || sendUserID.Equals(0) || receiveUserID.Equals(0))
            {
                return false;
            }
            if (isAll.Equals("0") && ids.Equals(""))        //선택이관인데, 대상문서값이 없는경우, 에러.
            {
                return false;
            }

            bool rtnval = false;
            DAL.CirculationUser dal = new DAL.CirculationUser();
            if (isCopy.Equals("1"))
            {
                rtnval = dal.TransferCopy(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }
            else
            {
                rtnval = dal.Transfer(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }

            return rtnval;
        }

        #endregion

        #region SET - 보낸회람 문서함

        /// <summary>
        /// 보낸회람 문서함 이관
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetTransferCirSend(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);

            string ids = GetDecryptParam("ids", "").Trim(',');
            string isCopy = GetDecryptParam("isCopy", "0");     // 1:복사, 0:이관
            string isAll = GetDecryptParam("isAll", "0");
            int sendUserID = GetDecryptParam("sendID", "0").ToNumber<int>();
            int receiveUserID = GetDecryptParam("receiveID", "0").ToNumber<int>();

            if (isCopy.Equals("") || isAll.Equals("") || sendUserID.Equals(0) || receiveUserID.Equals(0))
            {
                return false;
            }
            if (isAll.Equals("0") && ids.Equals(""))        //선택이관인데, 대상문서값이 없는경우, 에러.
            {
                return false;
            }

            bool rtnval = false;
            DAL.CirculationDocument dal = new DAL.CirculationDocument();
            if (isCopy.Equals("1"))
            {
                rtnval = dal.TransferCopy(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }
            else
            {
                rtnval = dal.Transfer(CurrentUserInfo.CompanyID, sendUserID, receiveUserID, isAll, ids);
            }

            return rtnval;
        }

        #endregion

        #endregion
    }
}
