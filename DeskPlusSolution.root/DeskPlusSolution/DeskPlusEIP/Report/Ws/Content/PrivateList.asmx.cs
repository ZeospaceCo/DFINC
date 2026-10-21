using System;
using System.Collections.Generic;
using System.Data;
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
    public class PrivateList : Base.BaseWSReport
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

                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string years = GetDecryptParam("years", "");
                string months = GetDecryptParam("months", "");
                string yearmonth = months.Length == 1 ? years + "0" + months : years + months;

                string targetUserID = GetDecryptParam("userID", CurrentUserInfo.UserID.ToString());
                string order = GetDecryptParam("order", "");

                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.List biz = new DAL.List();
                ObjList listobj = new ObjList();

                var getWeekDic = DateHelpers.GetMonFriByYearMonth(years.ToNumber<int>(), months.ToNumber<int>(), 0);//이번달

                DataSet ds = biz.GetPrivateList(CurrentUserInfo.CompanyID, yearmonth, order, searchField, searchValue, targetUserID.ToNumber<int>());

                DataView dv = null;
                DataView dvLinked = null;

                if (ds != null)
                {
                    dv = ds.Tables[0].DefaultView;
                    dvLinked = ds.Tables[0].DefaultView;
                }

                if (searchValue == "")
                {
                    foreach (KeyValuePair<DateTime, DateTime> dic in getWeekDic)
                    {
                        string startDate = dic.Key.ToString("yyyyMMdd");
                        string endDate = dic.Value.ToString("yyyyMMdd");

                        var info = new BE.ListStatusInfo();
                        info.StartDate = DateHelpers.GetCultureDateChar8(startDate, PreLangReal);
                        info.EndDate = DateHelpers.GetCultureDateChar8(endDate, PreLangReal);
                        info.StartDateYMD = startDate;
                        info.EndDateYMD = endDate;
                        info.ListID = 0;
                        info.AttachCount = "0";
                        info.ShortCount = "0";
                        info.Flag = "0"; //미작성

                        if (dv != null)
                        {
                            //0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                            dv.RowFilter = "StartDate = '" + startDate + "' AND IsLinked IN('0') ";

                            foreach (DataRowView dr in dv)
                            {
                                info.ListID = dr["ListID"].ToString().ToNumber<int>();
                                info.AttachCount = dr["FileCount"].ToString().ToNumber<int>() > 0 ? "1" : "0";
                                info.ShortCount = dr["ReplyCount"].ToString();
                                info.Flag = dr["Flag"].ToString();

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

                    //이관 및 복사 경우 출력
                    if (dvLinked != null)
                    {
                        //0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
                        string qry = " IsLinked IN('1','2','3','4') ";
                        dvLinked.RowFilter = qry;
                        dvLinked.Sort = "StartDate DESC";

                        foreach (DataRowView dr in dvLinked)
                        {
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

                            info.HolChk = "1";

                            #region 이름 다국어 설정

                            info.NameBase = this.GetLangName(
                                    dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                    dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                    dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                    dr["NameZhtw"].ToString().ToNull(), PreLang);

                            #endregion 이름 다국어 설정

                            #region 직급 다국어 설정

                            info.TitleName = this.GetLangName(
                                    dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                                    dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                                    dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                                    dr["TitleZhtw"].ToString().ToNull(), PreLang);

                            #endregion 직급 다국어 설정

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
                }
                else
                {
                    if (dv != null)
                    {
                        foreach (DataRowView dr in dv)
                        {
                            var info = new BE.ListStatusInfo();
                            info.StartDate = DateHelpers.GetCultureDateChar8(dr["StartDate"].ToString(), PreLangReal);
                            info.EndDate = DateHelpers.GetCultureDateChar8(dr["EndDate"].ToString(), PreLangReal);
                            info.StartDateYMD = dr["StartDate"].ToString();
                            info.EndDateYMD = dr["EndDate"].ToString();
                            info.ListID = dr["ListID"].ToString().ToNumber<int>();
                            info.AttachCount = dr["FileCount"].ToString().ToNumber<int>() > 0 ? "1" : "0";
                            info.ShortCount = dr["ReplyCount"].ToString();
                            info.Flag = dr["Flag"].ToString();

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