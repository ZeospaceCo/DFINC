using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.PMS.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.PMS.Web.Ws.Board
{
    /// <summary>
    /// ShortReplyList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyList : Base.BaseWSPMS
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
        /// 작성자 : 심재운
        /// 용도 : 투표 페이징 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            string projectID = string.Empty;
            string menuID = string.Empty;
            string boardID = string.Empty;
            string totalCount = string.Empty;
            string pageSize = string.Empty;
            string pageSizePageNo = string.Empty;

            SetDecryptString(p);
            projectID = GetDecryptParam("projectID", "0");
            menuID = GetDecryptParam("menuID", "0");
            boardID = GetDecryptParam("boardID", "0");
            totalCount = GetDecryptParam("totalCount", "0");
            pageSize = GetDecryptParam("pageSize", "0");
            pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            ObjList listobj = new ObjList();

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');
            
            DAL.BoardShortReply biz = new DAL.BoardShortReply();
            DataTableReader dtr = biz.GetBoardShortReply(CurrentUserInfo.CompanyID, projectID.ToNumber<int>(), menuID.ToNumber<int>(),
                boardID.ToNumber<int>(), setPageingStr[0], setPageingStr[1]);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.BoardShortReplyInfo info = new BoardShortReplyInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ProjectID = dtr["ProjectID"].ToString().ToNumber<int>();
                        info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                        info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();
                        info.BoardShortReplyID = dtr["BoardShortReplyID"].ToString().ToNumber<int>();

                        info.Contents = dtr["Contents"].ToString().Replace("\n", "<br>");
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                        info.NameBase = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);

                        info.IsAuth = (info.WriteUserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        DateTime dt;

                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                            info.WriteDateString = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.WriteDateString = string.Empty;

                        #region 날짜 비교(New Img)
                        DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
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

        
        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 투표 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCount(string p)
        {
            SetDecryptString(p);

            int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();

            DAL.BoardShortReply biz = new DAL.BoardShortReply();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, projectID, menuID, boardID);
        }

        #endregion

        #region DEL()

        /// <summary>
        /// 짧은 댓글 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShortReply(string p)
        {
            try
            {
                SetDecryptString(p);

                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
                int boardShortReplyID = GetDecryptParam("boardShortReplyID", "0").ToNumber<int>();
                int writeUserID = GetDecryptParam("writeUserID", "0").ToNumber<int>();

                DAL.BoardShortReply dal = new DAL.BoardShortReply();
                BE.BoardShortReplyInfo info = new BoardShortReplyInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.ProjectID = projectID;
                info.MenuID = menuID;
                info.BoardID = boardID;
                info.BoardShortReplyID = boardShortReplyID;
                info.WriteUserID = writeUserID;

                if (writeUserID == CurrentUserInfo.UserID)
                {
                    return dal.Del(info);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 댓글 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddShortReply(string p)
        {
            try
            {
                SetDecryptString(p);

                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
                string contents = GetDecryptParam("contents", "");

                BE.BoardShortReplyInfo info = new BoardShortReplyInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.ProjectID = projectID;
                info.MenuID = menuID;
                info.BoardID = boardID;
                info.WriteUserID = CurrentUserInfo.UserID;
                info.Contents = Server.UrlDecode(contents).ToReverseXSS();

                DAL.BoardShortReply dal = new DAL.BoardShortReply();
                dal.Add(info);
                return true;
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
