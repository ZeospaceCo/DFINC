using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.HR.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.HR.Web.Ws.Admin.Content
{
    /// <summary>
    /// AddIP의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddIP : Base.BaseWSHR
    {
        /// <summary>
        /// ip 추가하기
        /// -2 : 오류발생; -1:접근 문제; 0:숫자비교 문제;1:추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string Add(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string startIP1 = GetDecryptParam("startIP1", "0").ToNumber<int>().ToString();
            string startIP2 = GetDecryptParam("startIP2", "0").ToNumber<int>().ToString();
            string startIP3 = GetDecryptParam("startIP3", "0").ToNumber<int>().ToString();
            string startIP4 = GetDecryptParam("startIP4", "0").ToNumber<int>().ToString();
            string endIP1 = GetDecryptParam("endIP1", "0").ToNumber<int>().ToString();
            string endIP2 = GetDecryptParam("endIP2", "0").ToNumber<int>().ToString();
            string endIP3 = GetDecryptParam("endIP3", "0").ToNumber<int>().ToString();
            string endIP4 = GetDecryptParam("endIP4", "0").ToNumber<int>().ToString();

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin") || CurrentUserInfo.IsInRole("HR"))
                {
                    startIP1 = Utility.SetFrontDigit(3, startIP1.ToString(), '0');
                    startIP2 = Utility.SetFrontDigit(3, startIP2.ToString(), '0');
                    startIP3 = Utility.SetFrontDigit(3, startIP3.ToString(), '0');
                    startIP4 = Utility.SetFrontDigit(3, startIP4.ToString(), '0');

                    endIP1 = Utility.SetFrontDigit(3, endIP1.ToString(), '0');
                    endIP2 = Utility.SetFrontDigit(3, endIP2.ToString(), '0');
                    endIP3 = Utility.SetFrontDigit(3, endIP3.ToString(), '0');
                    endIP4 = Utility.SetFrontDigit(3, endIP4.ToString(), '0');

                    string startCompare = startIP1 + startIP2 + startIP3 + startIP4;
                    string endCompare = endIP1 + endIP2 + endIP3 + endIP4;

                    if (startCompare.ToDouble<double>() > endCompare.ToDouble<double>()) return "0";

                    BE.IPInfo info = new BE.IPInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.StartIP = startCompare.Trim();
                    info.EndIP = endCompare.Trim();

                    DAL.IP dal = new DAL.IP();
                    dal.Add(info);

                    return "1";
                }

                return "-1";

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                return "-2";
            }
        }
    }
}
