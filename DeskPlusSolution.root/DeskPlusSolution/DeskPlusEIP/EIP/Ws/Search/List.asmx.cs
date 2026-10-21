using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlus.Board.BE;
using Zeospace.DeskPlus.Board.DAL;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Search
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSEIP
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
        /// 메서드명   :
        /// 작성목적   : 작성자 이름설정하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <param name="_deptName">Name of the _dept.</param>
        /// <param name="_titleName">Name of the _title.</param>
        /// <param name="_dutyName">Name of the _duty.</param>
        /// <returns></returns>
        private string GetWriteUserName(string _writeUserName, string _deptName,
            string _titleName, string _dutyName)
        {
            string nameFieldFormat = _writeUserName;
            string applyCategory = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "VWNameFieldCategory", "0");
            nameFieldFormat = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "VWNameField", "WriteName");
            nameFieldFormat = (nameFieldFormat.IndexOf("WriteName") != -1) ? nameFieldFormat.Replace("WriteName", _writeUserName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("DeptName") != -1) ? nameFieldFormat.Replace("DeptName", _deptName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("TitleName") != -1) ? nameFieldFormat.Replace("TitleName", _titleName) : nameFieldFormat;
            nameFieldFormat = (nameFieldFormat.IndexOf("DutyName") != -1) ? nameFieldFormat.Replace("DutyName", _dutyName) : nameFieldFormat;

            //이름 보이기 (이름:WriteName, 부서:Dept, 직급:Title, 직책:Duty)
            return nameFieldFormat;

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 게시판 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Search.List).Assembly;	//어셈블리 선언

                SetDecryptString(p);

                string moduleName = GetDecryptParam("moduleName", "");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");

                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                //string searchGroup = GetDecryptParam("searchGroup", "");
                string order = GetDecryptParam("order", "");                

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                List<object> listobj = new List<object>();
                if (searchValue.Length == 0) return listobj;
                               

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

                Zeospace.DeskPlus.EIP.DAL.Search dal = new DAL.Search();
                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, moduleName,
                    setPageingStr[0], setPageingStr[1],
                    startDate, endDate,
                    searchField, searchValue, order, CurrentUserInfo.DeptID);
                
                

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.EIP.BE.SearchInfo info = new BE.SearchInfo();


                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.SearchID = dtr["SearchID"].ToString().ToNumber<int>();
                            info.ModuleName = dtr["ModuleName"].ToString();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            //info.Subjects = dtr["Subjects"].ToString();
                            info.AttachFileID = dtr["AttachFileID"].ToString().ToNumber<int>();
                            info.FileName = dtr["FileName"].ToString();
                            info.HeaderBase = dtr["HeaderBase"].ToString();
                            info.HeaderKo = dtr["HeaderKo"].ToString();
                            info.HeaderEn = dtr["HeaderEn"].ToString();
                            info.HeaderJa = dtr["HeaderJa"].ToString();
                            info.HeaderFr = dtr["HeaderFr"].ToString();
                            info.HeaderZhcn = dtr["HeaderZhcn"].ToString();
                            info.HeaderZhtw = dtr["HeaderZhtw"].ToString();
                            info.HeaderManyName = dtr["HeaderManyName"].ToString();
                            info.NameBase = dtr["NameBase"].ToString();
                            info.NameKo = dtr["NameKo"].ToString();
                            info.NameEn = dtr["NameEn"].ToString();
                            info.NameJa = dtr["NameJa"].ToString();
                            info.NameFr = dtr["NameFr"].ToString();
                            info.NameZhcn = dtr["NameZhcn"].ToString();
                            info.NameZhtw = dtr["NameZhtw"].ToString();
                            info.ManyName = dtr["ManyName"].ToString();
                            info.DeptBase = dtr["DeptBase"].ToString();
                            info.DeptKo = dtr["DeptKo"].ToString();
                            info.DeptEn = dtr["DeptEn"].ToString();
                            info.DeptJa = dtr["DeptJa"].ToString();
                            info.DeptFr = dtr["DeptFr"].ToString();
                            info.DeptZhcn = dtr["DeptZhcn"].ToString();
                            info.DeptZhtw = dtr["DeptZhtw"].ToString();
                            info.DeptManyName = dtr["DeptManyName"].ToString();
                            info.TitleBase = dtr["TitleBase"].ToString();
                            info.TitleKo = dtr["TitleKo"].ToString();
                            info.TitleEn = dtr["TitleEn"].ToString();
                            info.TitleJa = dtr["TitleJa"].ToString();
                            info.TitleFr = dtr["TitleFr"].ToString();
                            info.TitleZhcn = dtr["TitleZhcn"].ToString();
                            info.TitleZhtw = dtr["TitleZhtw"].ToString();
                            info.TitleManyName = dtr["TitleManyName"].ToString();
                            info.DutyBase = dtr["DutyBase"].ToString();
                            info.DutyKo = dtr["DutyKo"].ToString();
                            info.DutyEn = dtr["DutyEn"].ToString();
                            info.DutyJa = dtr["DutyJa"].ToString();
                            info.DutyFr = dtr["DutyFr"].ToString();
                            info.DutyZhcn = dtr["DutyZhcn"].ToString();
                            info.DutyZhtw = dtr["DutyZhtw"].ToString();
                            info.DutyManyName = dtr["DutyManyName"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.WriteDate = Convert.ToDateTime(dtr["WriteDate"]);
                            info.WriteDateYMD = dtr["WriteDateYMD"].ToString();

                            info.OptionAnonymity = dtr["OptionAnonymity"].ToString();//익명글여부
                            info.OptionUseDept = dtr["OptionUseDept"].ToString();//부서별게시판 여부
                            info.OwnerDeptID = dtr["OwnerDeptID"].ToString().ToNumber<int>();//부서별 게시판의 부서아이디값
                            info.Title = dtr["Title"].ToString(); //제목

                            if (info.ModuleName == "REPORT")
                            {
                                if (info.Title.Length >= 17)
                                {
                                    string iStartDate = DateHelpers.GetCultureDateChar8(info.Title.Substring(0, 17).Split(',')[0], PreLangReal);
                                    string iEndDate = DateHelpers.GetCultureDateChar8(info.Title.Substring(0, 17).Split(',')[1], PreLangReal);

                                    info.Title = iStartDate + "~" + iEndDate;
                                }
                            }
                            
                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateYMD = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDateYMD = string.Empty;

                            
                            #region 부서 다국어 설정
                            info.DeptBase = this.GetLangName(
                                info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleBase = this.GetLangName(
                                    info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang);
                            #endregion

                            #region 직책 다국어 설정
                            info.DutyBase = this.GetLangName(
                                    info.DutyBase, info.DutyKo, info.DutyEn, info.DutyJa, info.DutyFr, info.DutyZhcn, info.DutyZhtw, PreLang);
                            #endregion

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                            #endregion                            

                            #region 말머리
                            info.HeaderBase = this.GetLangName(
                                    info.HeaderBase, info.HeaderKo, info.HeaderEn, info.HeaderJa, info.HeaderFr, info.HeaderZhcn, info.HeaderZhtw, PreLang);

                            info.HeaderBase = (info.HeaderBase.Length > 0) ? "[" + info.HeaderBase + "]" : "";
                            #endregion

                            #region 번호
                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;
                            info.ROW = listNum;
                            #endregion

                            if (info.OptionAnonymity == "1") //익명글
                            {
                                string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", "익명,NoName");
                                info.NameBase = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
                                info.WriteUserID = 0;
                            }                            

                            switch (info.ModuleName.ToUpper())
                            {
                                case "BOARD" :
                                    info.Module = GetResource("SearchFieldBoard");
                                    //40 버전 게시판 창 띄우기
                                    info.RijndaelURL = "/DeskPlusEIP/Board/Content/ViewPopup.aspx?enc=" + QueryStringModule.Encrypt("catID=" + info.CategoryID + "&categoryID=" + info.CategoryID + "&listID=" + info.ListID, true);
                                    break;
                                case "APPROVAL":
                                    info.Module = GetResource("SearchFieldApproval");
                                    //33버전 결재 창 띄우기
                                    info.RijndaelURL = "/DeskPlusAPP/Approval/View/ViewDocumentByMail.aspx?enc=" + QueryStringModule.Encrypt("noDec=1&cID="+ CurrentUserInfo.CompanyID +" &dID=" + info.ListID, true);
                                    break;
                                case "PIMS" :
                                    info.Module = GetResource("SearchFieldMail");
                                    break;
                                case "RM":
                                    info.Module = GetResource("SearchFieldRM");
                                    break;
                                case "REPORT":
                                    info.Module = GetResource("SearchFieldREPORT");
                                    break;
                                default :
                                    info.Module = "";
                                    break;
                            }


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
        /// 검색 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string moduleName = GetDecryptParam("moduleName", "");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");

                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                //string searchGroup = GetDecryptParam("searchGroup", "");

                if (searchValue.Length == 0) return 0;

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

                Zeospace.DeskPlus.EIP.DAL.Search dal = new DAL.Search();
                return dal.GetPageListCount(CurrentUserInfo.CompanyID, moduleName,                    
                    startDate, endDate,
                    searchField, searchValue, CurrentUserInfo.DeptID);

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
