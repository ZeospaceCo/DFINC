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
    /// DeptList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class DeptList : Base.BaseWSReport
    {
        #region 리스트

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

                string targetUserID = GetDecryptParam("userID", CurrentUserInfo.UserID.ToString());
                string targetDeptID = GetDecryptParam("deptID", CurrentUserInfo.DeptID.ToString());
                string order = GetDecryptParam("order", "");

                int week = GetDecryptParam("week", "0").ToNumber<int>();
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.List biz = new DAL.List();
                ObjList listobj = new ObjList();

                Account.DAL.User userDal = new Account.DAL.User();
                Account.BLL.Dept userBiz = new Account.BLL.Dept();
                string deptList = userBiz.GetChildDeptList(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, true);
                deptList = (deptList.Length == 0) ? CurrentUserInfo.DeptID.ToString() : deptList;
                List<Account.BE.UserInfo> userList = userDal.GetUserInfoByDept(CurrentUserInfo.CompanyID, deptList, true);

                var getWeekList = DateHelpers.GetMonFriByYearMonth(years.ToNumber<int>(), months.ToNumber<int>(), week);
                string weekStartDate = "";

                var sortedDict = (from entry in getWeekList orderby entry.Key descending select entry)
                            .ToDictionary(pair => pair.Key, pair => pair.Value);

                if (userList.Count > 0)
                {
                    if (week != 0)
                    {
                        foreach (KeyValuePair<DateTime, DateTime> dic in getWeekList)
                        {
                            weekStartDate = dic.Key.ToString("yyyyMMdd");
                        }
                    }

                    DataSet ds = biz.GetDeptList(CurrentUserInfo.CompanyID, yearmonth, weekStartDate, order,
                        searchField, searchValue, targetDeptID.ToNumber<int>(), CurrentUserInfo.UserID);

                    DataView dv = null;
                    DataView dvLinked = null;
                    if (ds != null)
                    {
                        dv = ds.Tables[0].DefaultView;
                        dvLinked = ds.Tables[0].DefaultView;
                    }

                    bool holChk = true;

                    if (searchValue == "")
                    {
                        foreach (KeyValuePair<DateTime, DateTime> dic in sortedDict)
                        {
                            holChk = !holChk;
                            foreach (var userInfo in userList)
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
                                info.WriteUserID = userInfo.UserID;
                                info.DeptID = userInfo.DeptID;
                                info.HolChk = (holChk == true) ? "1" : "0";
                                info.ViewCount = 0;

                                #region 이름 다국어 설정

                                info.NameBase = this.GetLangName(userInfo.NameBase, userInfo.NameKo, userInfo.NameEn, userInfo.NameJa, userInfo.NameFr, userInfo.NameZhCn, userInfo.NameZhTw, PreLang);

                                #endregion 이름 다국어 설정

                                #region 직급 다국어 설정

                                info.TitleName = this.GetLangName(userInfo.TitleBase, userInfo.TitleKo, userInfo.TitleEn, userInfo.TitleJa, userInfo.TitleFr, userInfo.NameZhCn, userInfo.NameZhTw, PreLang);

                                #endregion 직급 다국어 설정

                                if (dv != null)
                                {
                                    //IsLinked : 0 은 내가 작성한 것(관리자가 이관처리함 : 0: 없음; 이관:1;복사:2)
                                    dv.RowFilter = "StartDate = '" + startDate + "' AND IsLinked = '0' AND WriteUserID = " + info.WriteUserID;

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
                                info.ViewCount = dr["ViewCount"].ToString().ToNumber<int>();

                                info.HolChk = (holChk == true) ? "1" : "0";

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

                                info.HolChk = (holChk == true) ? "1" : "0";

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
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 리스트

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

        #region GET()

        /// <summary>
        /// 작성자 : shine
        /// 비공개 작업일지 관리자 여부 확인(부서함)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AuthCheck(string p)
        {
            SetDecryptString(p);

            string userID = GetDecryptParam("userID", CurrentUserInfo.UserID.ToString());
            string useComplete = GetDecryptParam("useComplete", "");

            DAL.ShareUser share = new DAL.ShareUser();
            int icnt = 0;

            if (userID.ToNumber<int>() != CurrentUserInfo.UserID)
            {
                icnt = share.GetShareUserCntTargetUserID(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), CurrentUserInfo.UserID, useComplete);
                return icnt;
            }
            else
            {
                return 1;
            }
        }

        #endregion GET()
    }
}