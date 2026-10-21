using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.RM.Web.Ws.Content.Reject
{
    /// <summary>
    /// AddReject의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddReject : Base.BaseWSRM
    {

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-02
        /// 작성목적 : 거부 사유 쓰기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddContent(string p)
        {
            SetDecryptString(p);
            string reservationID = GetDecryptParam("reservationID", "");
            string contents = GetDecryptParam("contents", "");

            reservationID = reservationID.TrimEnd(',');
            string[] ArrValue = reservationID.Split(',');

            for (int i = 0; i < ArrValue.Length; i++)
            {
                BE.RejectInfo info = new BE.RejectInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.ReservationID = ArrValue[i].ToNumber<int>();
                info.Content = Server.UrlDecode(contents).ToReverseXSS();
                info.UserID = CurrentUserInfo.UserID;
                info.UserName = CurrentUserInfo.Name;

                DAL.Reject dal = new DAL.Reject();                
                dal.Add(info);
            }

            return true;
        }

        #endregion  ADD()
    }
}
