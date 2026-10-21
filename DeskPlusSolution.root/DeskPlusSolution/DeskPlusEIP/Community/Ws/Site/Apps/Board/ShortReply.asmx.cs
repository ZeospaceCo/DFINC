using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Community.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Apps.Board
{
    /// <summary>
    /// ShortReply의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReply : Base.BaseWSCommunity
    {

        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-25
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
    }
}
