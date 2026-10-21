using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Board.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
{
    /// <summary>
    /// ShortReplyGood의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReplyGood : Base.BaseWSBoard
    {
        #region GET()

        /// <summary>
        /// 댓글 추천 / 비추천 참여 여부
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetGoodCount(string p)
        {

            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string shortReplyID = GetDecryptParam("shortReplyID", "0");

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            BE.ShortReplyGoodInfo info = new BE.ShortReplyGoodInfo();

            info.CompanyID = boaComID;
            info.CategoryID = categoryID.ToNumber<int>();
            info.ListID = listID.ToNumber<int>();
            info.ShortReplyID = shortReplyID.ToNumber<int>();
            info.UserID = CurrentUserInfo.UserID;

            DAL.ShortReplyGood dal = new DAL.ShortReplyGood();
            int result = dal.GetGoodCount(info);

            return result;
        }

        #endregion

        #region ADD()

        /// <summary>
        /// 댓글 추천 / 비추천
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddShortReplyGood(string p)
        {

            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string shortReplyID = GetDecryptParam("shortReplyID", "0");
            string good = GetDecryptParam("good", "0");
            string bad = GetDecryptParam("bad", "0");

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

            BE.ShortReplyGoodInfo info = new BE.ShortReplyGoodInfo();

            info.CompanyID = boaComID;
            info.CategoryID = categoryID.ToNumber<int>();
            info.ListID = listID.ToNumber<int>();
            info.ShortReplyID = shortReplyID.ToNumber<int>();
            info.UserID = CurrentUserInfo.UserID;
            info.Good = good.ToNumber<int>();
            info.Bad = bad.ToNumber<int>();

            DAL.ShortReplyGood dal = new DAL.ShortReplyGood();
            dal.Add(info);

            return 1;
        }

        #endregion
    }
}
