using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using System.IO;

namespace Zeospace.DeskPlus.Opinion.Web.Ws.Folder
{
    /// <summary>
    /// ShortReply의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReply : Base.BaseWSOpinion
    {

        #region GET
        #endregion

        #region SET
        /// <summary>
        /// 답글 추가/수정
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddReply(string p)
        {
            SetDecryptString(p);

            // 'quesID': QuesID, 'replyID': ReplyID, 'cont':
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            int replyID = GetDecryptParam("replyID", "0").ToNumber<int>();
            string contents = GetDecryptParam("cont", "");
            //contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);

            BE.ReplyInfo rInfo = new BE.ReplyInfo();
            rInfo.CompanyID = CurrentUserInfo.CompanyID;
            rInfo.UserID = CurrentUserInfo.UserID;
            rInfo.Cont = contents;
            rInfo.QuesID = quesID;
            rInfo.ReplyID = replyID;

            BLL.Reply rBLL = new BLL.Reply();
            return rBLL.Add(rInfo);
        }

        #endregion
    }
}
