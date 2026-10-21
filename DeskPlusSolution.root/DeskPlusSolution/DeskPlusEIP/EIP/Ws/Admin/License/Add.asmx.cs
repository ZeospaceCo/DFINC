using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Interop.ZeoLicEp;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.License
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSEIP
    {

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 추가
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddLicense(string p)
        {
            int rtnVal = -1;

            if (CurrentUserInfo.IsSysAdmin)
            {

                //파라미터 가져오기
                SetDecryptString(p);

                string licenseKey = GetDecryptParam("licenseKey", "");

                Interop.ZeoLicEp.LicenseManager zeoLicMng = null;

                try
                {
                    zeoLicMng = new Interop.ZeoLicEp.LicenseManagerClass();
                    rtnVal = zeoLicMng.Add(licenseKey.Trim());
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddLicense[] : " + ex.ToString());
                }
            }

            return rtnVal;
        }
    }
}
