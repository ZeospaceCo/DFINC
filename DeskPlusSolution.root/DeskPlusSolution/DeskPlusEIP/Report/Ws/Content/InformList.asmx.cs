using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class InformList : Base.BaseWSReport
    {
        #region GET()

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
        /// 작성자 : shine
        /// 작업일지 리스트 가져오기
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

                string years = GetDecryptParam("years", "");
                string months = GetDecryptParam("months", "");
                string yearmonth = months.Length == 1 ? years + "0" + months : years + months;

                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string targetUserID = GetDecryptParam("userID", CurrentUserInfo.UserID.ToString());
                string targetDeptID = GetDecryptParam("deptID", CurrentUserInfo.DeptID.ToString());
                string order = GetDecryptParam("order", "");

                int week = GetDecryptParam("week", "0").ToNumber<int>();
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.List biz = new DAL.List();
                ObjList listobj = new ObjList();

                #region 보고 체계 폴더 정보 가져오기

                DAL.Folder fDal = new DAL.Folder();
                var dtrFolder = fDal.GetFolderByInform(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);
                List<BE.FolderInfo> folderList = new List<BE.FolderInfo>();
                if (dtrFolder != null)
                {
                    if (dtrFolder.HasRows)
                    {
                        while (dtrFolder.Read())
                        {
                            folderList.Add(new BE.FolderInfo()
                            {
                                NameBase = this.GetLangName(
                                            dtrFolder["NameBase"].ToString().ToNull(), dtrFolder["NameKo"].ToString().ToNull(),
                                            dtrFolder["NameEn"].ToString().ToNull(), dtrFolder["NameJa"].ToString().ToNull(),
                                            dtrFolder["NameFr"].ToString().ToNull(), dtrFolder["NameZhcn"].ToString().ToNull(),
                                            dtrFolder["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS(),
                                FolderID = dtrFolder["FolderID"].ToString().ToNumber<int>()
                            });
                        }
                    }
                }

                #endregion 보고 체계 폴더 정보 가져오기

                var getWeekList = DateHelpers.GetMonFriByYearMonth(years.ToNumber<int>(), months.ToNumber<int>(), week);
                string weekStartDate = "";

                var sortedDict = (from entry in getWeekList orderby entry.Key descending select entry)
                            .ToDictionary(pair => pair.Key, pair => pair.Value);

                if (folderList.Count > 0)
                {
                    if (week != 0)
                    {
                        foreach (KeyValuePair<DateTime, DateTime> dic in getWeekList)
                        {
                            weekStartDate = dic.Key.ToString("yyyyMMdd");
                        }
                    }

                    DataSet ds = biz.GetInformList(CurrentUserInfo.CompanyID, yearmonth, order,
                        searchField, searchValue, CurrentUserInfo.UserID);

                    DataView dv = null;
                    if (ds != null) dv = ds.Tables[0].DefaultView;

                    bool holChk = true;

                    if (searchValue == "")
                    {
                        foreach (KeyValuePair<DateTime, DateTime> dic in sortedDict)
                        {
                            holChk = !holChk;

                            //보고체계 개수만큼 for 문 출력한다.
                            foreach (var folderInfo in folderList)
                            {
                                string startDate = dic.Key.ToString("yyyyMMdd");
                                string endDate = dic.Value.ToString("yyyyMMdd");

                                var info = new BE.ListInfo();
                                info.StartDate = DateHelpers.GetCultureDateChar8(startDate, PreLangReal);
                                info.EndDate = DateHelpers.GetCultureDateChar8(endDate, PreLangReal);
                                info.StartDateYMD = startDate;
                                info.EndDateYMD = endDate;
                                info.ListID = 0;
                                info.FileCount = 0;
                                info.ReplyCount = 0;
                                info.Flag = "0"; //미작성
                                info.IsClose = "0";
                                info.IsLinked = "0";//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                                info.WriteUserID = CurrentUserInfo.UserID;
                                info.DeptID = CurrentUserInfo.DeptID;
                                info.HolChk = (holChk == true) ? "1" : "0";
                                info.ViewCount = 0;

                                info.FolderID = folderInfo.FolderID; //보고체계 아이디                                
                                info.NameBase = folderInfo.NameBase; //보고체계 이름

                                info.TitleName = "";

                                if (dv != null)
                                {
                                    //IsLinked : 0 은 내가 작성한 것(관리자가 이관처리함 : 0: 없음; 이관:1;복사:2) / FolderID 를 조건 건다.
                                    dv.RowFilter = "StartDate = '" + startDate + "' AND FolderID = " + info.FolderID + " AND IsLinked = '0'";

                                    foreach (DataRowView dr in dv)
                                    {
                                        info.ListID = dr["ListID"].ToString().ToNumber<int>();
                                        info.FileCount = dr["FileCount"].ToString().ToNumber<int>() > 0 ? 1 : 0;
                                        info.ReplyCount = dr["ReplyCount"].ToString().ToNumber<int>();
                                        info.Flag = dr["Flag"].ToString();
                                        info.IsClose = dr["IsClose"].ToString();
                                        info.IsLinked = dr["IsLinked"].ToString();//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                                        info.ViewCount = dr["ViewCount"].ToString().ToNumber<int>();

                                        #region 날짜 비교(New Img)

                                        if (dr["ReplyDate"].ToString().ToNull() != "")
                                        {
                                            DateTime writeDate = Convert.ToDateTime(dr["ReplyDate"].ToString());
                                            DateTime todayDate = System.DateTime.Now;
                                            TimeSpan gap = todayDate.Subtract(writeDate);
                                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                                        }
                                        else
                                            info.ShowNew = "N";

                                        #endregion 날짜 비교(New Img)
                                    }
                                }
                                listobj.Add(info);
                            }
                        }                        
                    }
                    else
                    {
                        #region 검색 기능 제공

                        if (ds != null)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                holChk = !holChk;

                                var info = new BE.ListInfo();
                                info.StartDate = DateHelpers.GetCultureDateChar8(dr["StartDate"].ToString(), PreLangReal);
                                info.EndDate = DateHelpers.GetCultureDateChar8(dr["EndDate"].ToString(), PreLangReal);
                                info.StartDateYMD = dr["StartDate"].ToString();
                                info.EndDateYMD = dr["EndDate"].ToString();
                                info.ListID = dr["ListID"].ToString().ToNumber<int>();
                                info.FileCount = dr["FileCount"].ToString().ToNumber<int>() > 0 ? 1 : 0;
                                info.ReplyCount = dr["ReplyCount"].ToString().ToNumber<int>();
                                info.Flag = dr["Flag"].ToString();
                                info.WriteUserID = dr["WriteUserID"].ToString().ToNumber<int>();
                                info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                                info.IsClose = dr["IsClose"].ToString();
                                info.IsLinked = dr["IsLinked"].ToString();//0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                                info.ViewCount = dr["ViewCount"].ToString().ToNumber<int>();
                                info.FolderID = dr["FolderID"].ToString().ToNumber<int>();
                                info.HolChk = (holChk == true) ? "1" : "0";
                                
                                info.NameBase = this.GetLangName(
                                        dr["FolderNameBase"].ToString().ToNull(), dr["FolderNameKo"].ToString().ToNull(),
                                        dr["FolderNameEn"].ToString().ToNull(), dr["FolderNameJa"].ToString().ToNull(),
                                        dr["FolderNameFr"].ToString().ToNull(), dr["FolderNameZhcn"].ToString().ToNull(),
                                        dr["FolderNameZhtw"].ToString().ToNull(), PreLang);

                                info.TitleName = "";

                                #region 날짜 비교(New Img)

                                if (dr["ReplyDate"].ToString().ToNull() != "")
                                {
                                    DateTime writeDate = Convert.ToDateTime(dr["ReplyDate"].ToString());
                                    DateTime todayDate = System.DateTime.Now;
                                    TimeSpan gap = todayDate.Subtract(writeDate);
                                    int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                                    info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                                }
                                else
                                    info.ShowNew = "N";

                                #endregion 날짜 비교(New Img)

                                listobj.Add(info);
                            }
                        }

                        #endregion 검색 기능 제공
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

        #endregion GET()

        #region 첨부파일

        /// <summary>
        /// 작성자 : shine
        /// 작업일지 첨부파일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFileListMenu(string p)
        {
            try
            {
                int listID = 0;
                SetDecryptString(p);
                listID = GetDecryptParam("listID", "0").ToNumber<Int32>();

                string response = "0";

                DAL.Attach dal = new DAL.Attach();
                List<BE.AttachInfo> attachInfo = dal.GetAttachLists(CurrentUserInfo.CompanyID, listID);

                string totalFileInfo = "";
                string encFilePath = "";

                foreach (BE.AttachInfo _attachInfo in attachInfo)
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

        #endregion 첨부파일
    }
}