using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Community.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Apps.Board
{
    /// <summary>
    /// ShortReplyList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyList : Base.BaseWSCommunity
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 짧은 답글 페이징 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            SetDecryptString(p);
            int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
            string totalCount = GetDecryptParam("totalCount", "0");
            string pageSize = GetDecryptParam("pageSize", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

            ObjList listobj = new ObjList();

            string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            DAL.BoaShortReply biz = new DAL.BoaShortReply();
            DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, communityID, menuID, boardID, setPageingStr[0], setPageingStr[1]);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";

                    while (dtr.Read())
                    {
                        BE.BoaShortReplyInfo info = new BE.BoaShortReplyInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.CommunityID = dtr["CommunityID"].ToString().ToNumber<int>();
                        info.MenuID = dtr["MenuID"].ToString().ToNumber<int>();
                        info.BoardID = dtr["BoardID"].ToString().ToNumber<int>();
                        info.ShortReplyID = dtr["ShortReplyID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

                        if (name.Trim() == "")
                        {
                            name = dtr["UserName"].ToString();
                        }

                        info.UserName = name;
                        info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        DateTime dt;

                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                            info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.WriteDate = string.Empty;

                        #region 날짜 비교(New Img)
                        DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                        DateTime todayDate = System.DateTime.Now;
                        TimeSpan gap = todayDate.Subtract(writeDate);
                        int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                        info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                        #endregion


                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 짧은 답글 갯수
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCount(string p)
        {
            SetDecryptString(p);
            int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();

            DAL.BoaShortReply biz = new DAL.BoaShortReply();
            return biz.GetPageListCount(CurrentUserInfo.CompanyID, communityID, menuID, boardID);
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

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
                int grade = GetDecryptParam("grade", "10").ToNumber<int>();
                string listType = GetDecryptParam("listType", ""); 
                string contents = GetDecryptParam("contents", "");

                BE.BoaShortReplyInfo info = new BoaShortReplyInfo();
                BE.PointInfo pInfo = new PointInfo();

                #region 댓글
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CommunityID = communityID;
                info.MenuID = menuID;
                info.BoardID = boardID;
                info.UserID = CurrentUserInfo.UserID;
                info.Content = Server.UrlDecode(contents).ToReverseXSS();
                info.UserName = CurrentUserInfo.Name;
                info.ListType = listType;

                BLL.ShortReply bll = new BLL.ShortReply();

                if (!bll.Add(info, PreLang))
                {
                    return false;
                }
                #endregion

                #region 포인트 추가
                if (grade != 10)
                {
                    // 1.글쓰기 2.조회 3.추천 4.짧은답글 5.답변 6.방문

                    pInfo.CompanyID = CurrentUserInfo.CompanyID;
                    pInfo.CommunityID = communityID;
                    pInfo.UserID = CurrentUserInfo.UserID;
                    pInfo.Type = 4;
                    pInfo.Point = 0;

                    DAL.Point dalPoint = new DAL.Point();
                    if (!dalPoint.Add(pInfo, menuID))
                    {
                        return false;
                    }
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region DEL()
        /// <summary>
        /// 작성자 : famao23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 짧은 댓글 삭제
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

                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
                int shortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
                int userID = GetDecryptParam("userID", "0").ToNumber<int>();

                DAL.BoaShortReply dal = new DAL.BoaShortReply();

                if (userID == CurrentUserInfo.UserID)
                {
                    return dal.Del(CurrentUserInfo.CompanyID, communityID, menuID, boardID, shortReplyID, CurrentUserInfo.UserID);
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
    }
}
