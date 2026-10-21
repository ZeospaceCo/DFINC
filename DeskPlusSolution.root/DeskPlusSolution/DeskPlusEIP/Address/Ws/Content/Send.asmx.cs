using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Address.Web.Ws.Content
{
    /// <summary>
    /// Send의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Send : Base.BaseWSAddress
    {

        /// <summary>
        /// 작성자 : shine
        /// 전달받은 주소록으로 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddPrivateReceived(string p)
        {
            try
            {
                SetDecryptString(p);
                string UserIDs = GetDecryptParam("userID", "");    // 주소록을 받을 사람
                string ListID = GetDecryptParam("listID", "");     // 주소록
                bool response = true;

                if (UserIDs == "" || ListID == "")
                    return false;

                DAL.Private dal = new DAL.Private();

                string[] ArrUserID = UserIDs.Split(',');

                foreach (string UserID in ArrUserID)
                {
                    if (!dal.AddSend(CurrentUserInfo.CompanyID, UserID.ToNumber<int>(), CurrentUserInfo.UserID, ListID))
                    {
                        response = false;
                        break;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPrivateReceived : " + ex.Message);
                throw;
            }
        }
    }
}
