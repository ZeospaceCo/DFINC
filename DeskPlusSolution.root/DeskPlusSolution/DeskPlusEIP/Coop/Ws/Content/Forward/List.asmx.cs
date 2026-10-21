using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Coop.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Coop.Web.Ws.Content.Forward
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSCoop
    {

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
        public List<Ws.Content.Forward.PageForwardList> GetPageList(string p)
        {
            SetDecryptString(p);

            string totalCount = GetDecryptParam("totalCount", "0");
            string pageSize = GetDecryptParam("pageSize", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
            string searchStatus = GetDecryptParam("searchStatus", "");
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");
            string searchDateValue = GetDecryptParam("searchDateValue", "");
            string order = GetDecryptParam("order", "");
            string itemID = GetDecryptParam("itemID", "0");
            string itemSubID = GetDecryptParam("itemSubID", "0");

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            #region 날짜

            string startDate = "";
            string endDate = "";

            if (searchField == "RequestDate" || searchField == "StartDate" || searchField == "EndDate")
            {
                startDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            #endregion

            DAL.ForwardList biz = new DAL.ForwardList();

            DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, setPageingStr[0], setPageingStr[1],
                startDate, endDate, searchStatus, searchField, searchValue, order, CurrentUserInfo.UserID, itemID.ToNumber<int>(), itemSubID.ToNumber<int>());

            List<Ws.Content.Forward.PageForwardList> listobj = new List<Ws.Content.Forward.PageForwardList>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        #region  할당하기

                        Ws.Content.Forward.PageForwardList info = new PageForwardList();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ForwardListID = dtr["ForwardListID"].ToString().ToNumber<int>();
                        info.ForwardGroupID = dtr["ForwardGroupID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();

                        #region 이름 다국어 설정
                        info.NameBase = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 직급 다국어 설정
                        info.TitleBase = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        info.FileCount = dtr["FileCount"].ToString().ToNumber<int>();

                        info.ItemName = this.GetLangName(
                                dtr["ItemNameBase"].ToString().ToNull(), dtr["ItemNameKo"].ToString().ToNull(),
                                dtr["ItemNameEn"].ToString().ToNull(), dtr["ItemNameJa"].ToString().ToNull(),
                                dtr["ItemNameFr"].ToString().ToNull(), dtr["ItemNameZhcn"].ToString().ToNull(),
                                dtr["ItemNameZhtw"].ToString().ToNull(), PreLang);

                        info.ItemSubName = this.GetLangName(
                                dtr["ItemSubNameBase"].ToString().ToNull(), dtr["ItemSubNameKo"].ToString().ToNull(),
                                dtr["ItemSubNameEn"].ToString().ToNull(), dtr["ItemSubNameJa"].ToString().ToNull(),
                                dtr["ItemSubNameFr"].ToString().ToNull(), dtr["ItemSubNameZhcn"].ToString().ToNull(),
                                dtr["ItemSubNameZhtw"].ToString().ToNull(), PreLang);

                        //1:대기;2:접수;3:처리중;4:완료;5:반려
                        info.Status = GetStatus(dtr["StatusLast"].ToString());

                        info.RequestDate = DateHelpers.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                        info.StartDate = DateHelpers.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                        info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                        info.CoopNumber = dtr["CoopNumber"].ToString();

                        info.ROW = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;
                        info.Title = dtr["Title"].ToString();

                        info.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();                        
                        info.ViewerCount = dtr["ViewerCount"].ToString().ToNumber<int>();

                        info.SendUserID = dtr["SendUserID"].ToString().ToNumber<int>();
                        info.SendUserName = this.GetLangName(
                                dtr["SendNameBase"].ToString().ToNull(), dtr["SendNameKo"].ToString().ToNull(),
                                dtr["SendNameEn"].ToString().ToNull(), dtr["SendNameJa"].ToString().ToNull(),
                                dtr["SendNameFr"].ToString().ToNull(), dtr["SendNameZhcn"].ToString().ToNull(),
                                dtr["SendNameZhtw"].ToString().ToNull(), PreLang);

                        info.ForwardUserID = dtr["ForwardUserID"].ToString().ToNumber<int>();
                        listobj.Add(info);

                        #endregion
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 총 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageCount(string p)
        {
            SetDecryptString(p);

            string searchStatus = GetDecryptParam("searchStatus", "");
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");
            string searchDateValue = GetDecryptParam("searchDateValue", "");
            string itemID = GetDecryptParam("itemID", "0");
            string itemSubID = GetDecryptParam("itemSubID", "0");

            #region 날짜

            string startDate = "";
            string endDate = "";

            if (searchField == "RequestDate" || searchField == "StartDate" || searchField == "EndDate")
            {
                startDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            #endregion

            DAL.ForwardList biz = new DAL.ForwardList();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, startDate, endDate, searchStatus, searchField, searchValue, itemID.ToNumber<int>(), itemSubID.ToNumber<int>(), CurrentUserInfo.UserID);

        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 첨부파일 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetForwardAttachFileListMenu(string p)
        {  
            SetDecryptString(p);

            int forwardListID = GetDecryptParam("forwardListID", "0").ToNumber<Int32>();

            string response = "0";

            DAL.ForwardAttach dal = new DAL.ForwardAttach();
            List<BE.ForwardAttachInfo> attachInfo = dal.GetForwardAttachFileLists(CurrentUserInfo.CompanyID, forwardListID);

            string totalFileInfo = "";
            string encFilePath = "";

            foreach (var _attachInfo in attachInfo)
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
    
    }

    /// <summary>
    /// 전달받은 협업 리스트 클래스
    /// </summary>
    [Serializable]
    public class PageForwardList
    {
        public PageForwardList() { }

        public int ROW { get; set; }
        public int CompanyID { get; set; }
        public int ForwardListID { get; set; }
        public int ForwardGroupID { get; set; }
        public int ListID { get; set; }
        public int GroupID { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string ItemName { get; set; }
        public string ItemSubName { get; set; }
        public string RequestDate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CoopNumber { get; set; }
        public int FileCount { get; set; }
        public int WriteUserID { get; set; }
        public string NameBase { get; set; }
        public string TitleBase { get; set; }
        public int ViewerCount { get; set; }
        public int ReplyCnt { get; set; }
        public int SendUserID { get; set; }
        public string SendUserName { get; set; }
        public int ForwardUserID { get; set; }
    }
}
