using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Report.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// AdminList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class AdminList : Base.BaseWSReport
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
        /// 작성자 : shine
        /// 작업일지 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.AdminPageList> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string targetUserID = GetDecryptParam("targetUserID", "");
                string targetDeptID = GetDecryptParam("targetDeptID", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.List biz = new DAL.List();
                List<Ws.Content.AdminPageList> alList = new List<AdminPageList>();

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, "", "", targetDeptID.ToNumber<int>(),
                    setPageingStr[0], setPageingStr[1], searchField, searchValue, targetUserID.ToNumber<int>(), CurrentUserInfo.UserID);

                bool holChk = true;
                string tmpUserID = "";
                int i = 0;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            if (i == 0)
                            {
                                holChk = !holChk;
                                tmpUserID = dtr["StartDate"].ToString();
                            }

                            if (tmpUserID != dtr["StartDate"].ToString())
                            {
                                tmpUserID = dtr["StartDate"].ToString();
                                holChk = !holChk;
                            }

                            Ws.Content.AdminPageList info = new AdminPageList();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.StartDate = DateHelpers.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                            info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                            info.StartDateYMD = dtr["StartDate"].ToString();
                            info.EndDateYMD = dtr["EndDate"].ToString();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.FileCount = dtr["FileCount"].ToString().ToNumber<int>() > 0 ? 1 : 0;
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();
                            info.Flag = dtr["Flag"].ToString();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.HolChk = (holChk == true) ? "1" : "0";
                            info.IsClose = dtr["IsClose"].ToString();
                            info.IsLinked = dtr["IsLinked"].ToString();
                            info.ViewCount = dtr["ViewCount"].ToString().ToNumber<int>();

                            #region 이름 다국어 설정

                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            #endregion 이름 다국어 설정

                            #region 직급 다국어 설정

                            info.TitleName = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                            #endregion 직급 다국어 설정

                            #region 날짜 비교(New Img)

                            if (dtr["ReplyDate"].ToString().ToNull() != "")
                            {
                                DateTime writeDate = Convert.ToDateTime(dtr["ReplyDate"].ToString());
                                DateTime todayDate = System.DateTime.Now;
                                TimeSpan gap = todayDate.Subtract(writeDate);
                                int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                                info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                            }
                            else
                                info.ShowNew = "N";

                            #endregion 날짜 비교(New Img)

                            alList.Add(info);

                            i++;
                        }
                    }
                }

                return alList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-05-22
        /// 작성목적 : 관리자 검색 카운트 가져오기
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

                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string targetUserID = GetDecryptParam("targetUserID", "");
                string targetDeptID = GetDecryptParam("targetDeptID", "");

                int icnt = 0;

                DAL.List dal = new DAL.List();
                icnt = dal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", targetDeptID.ToNumber<int>(), searchField, searchValue, targetUserID.ToNumber<int>());

                return icnt;
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

        #region 이관 처리

        /// <summary>
        /// 작성자 : shimpark
        /// 선택이관(사용자)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveUserSelList(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("listID", "");
            int afterUserID = GetDecryptParam("afterUserID", "0").ToNumber<int>();

            if (listID.Length == 0 || afterUserID == 0) return false;

            BLL.List biz = new BLL.List();
            bool res = biz.MoveUserSelList(CurrentUserInfo.CompanyID, listID, afterUserID);

            return res;
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 선택이관(부서)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveDeptSelList(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("listID", "");
            int afterDeptID = GetDecryptParam("afterDeptID", "0").ToNumber<int>();

            if (listID.Length == 0 || afterDeptID == 0) return false;

            BLL.List biz = new BLL.List();
            bool res = biz.MoveDeptSelList(CurrentUserInfo.CompanyID, listID, afterDeptID);

            return res;
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 선택복사(사용자)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool CopyUserSelList(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("listID", "");
            int afterUserID = GetDecryptParam("afterUserID", "0").ToNumber<int>();

            if (listID.Length == 0 || afterUserID == 0) return false;

            BLL.List biz = new BLL.List();
            bool res = biz.CopyUserSelList(CurrentUserInfo.CompanyID, listID, afterUserID);

            return res;
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 선택복사(부서)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool CopyDeptSelList(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("listID", "");
            int afterDeptID = GetDecryptParam("afterDeptID", "0").ToNumber<int>();

            if (listID.Length == 0 || afterDeptID == 0) return false;

            BLL.List biz = new BLL.List();
            bool res = biz.CopyDeptSelList(CurrentUserInfo.CompanyID, listID, afterDeptID);

            return res;
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 전체이관(사용자)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveUserAllList(string p)
        {
            SetDecryptString(p);

            int beforeUserID = GetDecryptParam("beforeUserID", "0").ToNumber<int>();
            int afterUserID = GetDecryptParam("afterUserID", "0").ToNumber<int>();

            if (beforeUserID == 0 || afterUserID == 0) return false;

            BLL.List biz = new BLL.List();
            bool res = biz.MoveUserAllList(CurrentUserInfo.CompanyID, beforeUserID, afterUserID);

            return res;
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 전체복사(사용자)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool CopyUserAllList(string p)
        {
            SetDecryptString(p);

            int beforeUserID = GetDecryptParam("beforeUserID", "0").ToNumber<int>();
            int afterUserID = GetDecryptParam("afterUserID", "0").ToNumber<int>();

            if (beforeUserID == 0 || afterUserID == 0) return false;

            BLL.List biz = new BLL.List();
            bool res = biz.CopyUserAllList(CurrentUserInfo.CompanyID, beforeUserID, afterUserID);

            return res;
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 전체이관(부서)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool MoveDeptAllList(string p)
        {
            SetDecryptString(p);

            int beforeDeptID = GetDecryptParam("beforeDeptID", "0").ToNumber<int>();
            int afterDeptID = GetDecryptParam("afterDeptID", "0").ToNumber<int>();

            if (beforeDeptID == 0 || afterDeptID == 0) return false;

            BLL.List biz = new BLL.List();
            bool res = biz.MoveDeptAllList(CurrentUserInfo.CompanyID, beforeDeptID, afterDeptID);

            return res;
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 전체복사(부서)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool CopyDeptAllList(string p)
        {
            SetDecryptString(p);

            int beforeDeptID = GetDecryptParam("beforeDeptID", "0").ToNumber<int>();
            int afterDeptID = GetDecryptParam("afterDeptID", "0").ToNumber<int>();

            if (beforeDeptID == 0 || afterDeptID == 0) return false;

            BLL.List biz = new BLL.List();
            bool res = biz.CopyDeptAllList(CurrentUserInfo.CompanyID, beforeDeptID, afterDeptID);

            return res;
        }

        #endregion 이관 처리

        /// <summary>
        /// 작성자 : shimpark
        /// 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("listID", "");

            if (listID.Length == 0) return false;

            foreach (string id in listID.Split(','))
            {
                ListInfo info = new ListInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.ListID = id.ToNumber<int>();

                BLL.List biz = new BLL.List();
                biz.Del(info);
            }

            return true;
        }
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class AdminPageList : Zeospace.DeskPlus.Common.BE.DeptCodeInfo
    {
        public AdminPageList()
        {
        }

        public int CompanyID { get; set; }

        public int ListID { get; set; }

        public int FileCount { get; set; }

        public int ReplyCount { get; set; }

        public int WriteUserID { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string StartDateYMD { get; set; }

        public string EndDateYMD { get; set; }

        public string Flag { get; set; }

        public string IsClose { get; set; }

        /// <summary>
        /// 0: 없음; 사용자(1:이관;2:복사), 부서(3:이관;4:복사)
        /// </summary>
        public string IsLinked { get; set; }

        public string HolChk { get; set; }

        public string ShowNew { get; set; }

        public int ViewCount { get; set; }
    }
}