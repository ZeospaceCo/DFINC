using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Main
{
    /// <summary>
    /// Menu의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Menu : Base.BaseWSCommunity
    {

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-07-28
        /// 작성목적 : 커뮤니티 방문 횟수 & 포인트 넣기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddVisitPoint(string p)
        {
            try
            {
                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int type = GetDecryptParam("type", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();

                // 방문 횟수 추가
                DAL.UserVisit dal = new DAL.UserVisit();
                if (!dal.Add(CurrentUserInfo.CompanyID, communityID, CurrentUserInfo.UserID))
                {
                    return false;
                }

                // 포인트 추가 ( 1.글쓰기 2.조회 3.추천 4.짧은답글 5.답변 6.방문 )
                BE.PointInfo pInfo = new BE.PointInfo();
                pInfo.CompanyID = CurrentUserInfo.CompanyID;
                pInfo.CommunityID = communityID;
                pInfo.UserID = CurrentUserInfo.UserID;
                pInfo.Type = type;
                pInfo.Point = 0;

                DAL.Point dalPoint = new DAL.Point();
                if (!dalPoint.Add(pInfo, menuID))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion 
    }
}
