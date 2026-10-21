using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.HR.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.HR.Web.Ws.Content.Popup
{
    /// <summary>
    /// AccountInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AccountInfo : Base.BaseWSHR
    {
        /// <summary>
        /// 입사일, 퇴사일 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string Mod(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int userID = GetDecryptParam("userID", "0").ToNumber<int>();
            string joinDate = GetDecryptParam("joinDate", "").ToString();
            string retireDate = GetDecryptParam("retireDate", "").ToString();

            try
            {
                if (companyID != 0 && userID != 0 && joinDate != "")
                {                    
                    DAL.Closing dal = new DAL.Closing();

                    joinDate = DateHelper.GetCultureDate(joinDate.Trim(), "yyyyMMdd", PreLangReal);
                    retireDate = DateHelper.GetCultureDate(retireDate.Trim(), "yyyyMMdd", PreLangReal);

                    dal.ModUserBackUp(companyID, userID, joinDate, retireDate);

                    return "1";
                }

                return "-1";

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod : " + ex);
                return "-1";
            }
        }
    }
}
