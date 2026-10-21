using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using Newtonsoft.Json;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws.Home
{
    /// <summary>
    /// Search의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Search : Base.BaseWSBoard
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

        #region 검색리스트

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = false)]
        public List<object> GetSearchList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchValue = GetDecryptParam("searchValue", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.HomeList dal = new DAL.HomeList();

                List<object> listobj = new ObjList();

                DataTableReader dtr = dal.SearchList(CurrentUserInfo.CompanyID, searchValue, setPageingStr[0], setPageingStr[1], CurrentUserInfo.UserID);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.ROW = dtr["ROW"].ToString();
                            info.CA_Title = dtr["CateBase"].ToString();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString().ToReverseXSS();
                            info.Filecount = dtr["FileCount"].ToString().ToNumber<int>();
                            info.ReadCount = dtr["ReadCount"].ToString().ToNumber<int>();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.WriteUserName = dtr["WriteUserName"].ToString();
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDate = string.Empty;


                            DateTime WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = System.DateTime.Now;
                            TimeSpan gap = todayDate.Subtract(WriteDate);
                            info.ShowNew = (gap.Days <= 0) ? "Y" : "N"; //하루만

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

        #region 검색카운트
        /// <summary>
        /// Gets the list count.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetSearchCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchValue = GetDecryptParam("searchValue", "");

                int icnt = 0;

                DAL.HomeList biz = new DAL.HomeList();
                icnt = biz.SearchCount(CurrentUserInfo.CompanyID, searchValue, CurrentUserInfo.UserID);


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
                int categoryID = 0;
                int listID = 0;

                SetDecryptString(p);

                categoryID = GetDecryptParam("categoryID", "0").ToNumber<Int32>();
                listID = GetDecryptParam("listID", "0").ToNumber<Int32>();

                string response = "0";

                //통합게시판 회사아이디
                int boardComID = BLL.Category.GetCompanyCategoryInfo(categoryID);

                DAL.AttachFiles dal = new DAL.AttachFiles();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileLists(boardComID, categoryID, listID);

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

        #endregion 첨부파일

    }
}
