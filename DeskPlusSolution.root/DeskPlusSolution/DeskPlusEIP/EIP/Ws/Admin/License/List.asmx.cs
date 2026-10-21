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
using System.Net;
using System.Text;
using System.IO;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.License
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSEIP
    {

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 정보 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetLicense()
        {
            List<object> listobj = new List<object>();

            Interop.ZeoLicEp.LicenseManager zeoLicMng = null;
            Interop.ZeoLicEp.License zeoLic = null;

            try
            {
                zeoLicMng = new Interop.ZeoLicEp.LicenseManagerClass();
                zeoLic = new Interop.ZeoLicEp.LicenseClass();

                string strLicense = zeoLicMng.GetAllLicenses();

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("strLicense : " + strLicense, AppSettingType.EIP);

                if (String.IsNullOrEmpty(strLicense)) return listobj;

                string[] arrLicense = strLicense.Split(';');

                if (arrLicense.Length > 0)
                {
                    for (int i = 0; i < arrLicense.Length; i++)
                    {
                        Zeospace.DeskPlus.EIP.BE.LicenseInfo info = new Zeospace.DeskPlus.EIP.BE.LicenseInfo();

                        zeoLic.LicenseKey = arrLicense[i].Trim();

                        int isValid = zeoLic.LicenseIsValid();
                        if (isValid > 0)
                        {

                            info.LicenseNumber = arrLicense[i];
                            info.Description = GetProductCode(zeoLic.ProductCode.ToString());
                            info.Count = zeoLic.Users.ToString();
                            info.Status = GetStatus(zeoLicMng.GetStatus(arrLicense[i].Trim()).ToString());
                            info.Expire = GetExpire(zeoLic.Expire);

                            listobj.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetLicense[] : ", ex);
            }

            return listobj;
        }


        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 상태 가져오기
        /// </summary>
        /// <param name="_status"></param>
        /// <returns></returns>
        public string GetStatus(string _status)
        {
            string message = "";

            switch (_status)
            {
                case "0":
                    message = "비활성";
                    break;
                case "1":
                    message = "활성";
                    break;
                case "2":
                    message = "기간만료";
                    break;
                default:
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("License GetStatus[] : " + _status);
                    message = "오류";
                    break;
            }

            return message;
        }

        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 만료정보 가져오기
        public string GetExpire(string _expire)
        {
            string message = "";

            switch (_expire)
            {
                case "never":
                    message = "제한 없음";
                    break;
                default:
                    message = _expire;
                    break;
            }

            return message;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 정보
        /// </summary>
        /// <returns></returns>
        public string GetProductCode(string _code)
        {
            string message = "";

            switch (_code)
            {
                case "1":
                    message = "데스크플러스 웹";
                    break;
                case "2":
                    message = "데스크플러스 메신저";
                    break;
                case "3":
                    message = "데스크플러스 모바일";
                    break;
                case "4":
                    message = "DeskPlusEP";
                    break;
                default:
                    message = "";
                    break;
            }

            return message;
        }


        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 삭제
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelLicense(string p)
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
                    rtnVal = zeoLicMng.Remove(licenseKey);
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddLicense[] : " + ex.ToString());
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 활성화 (1차 인증 단계로 라이센스 인증서버의 라이센스를 활성화 한다)
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ActActiveLicense(string p)
        {
            string rtnVal = string.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            string licenseKey = GetDecryptParam("licenseKey", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin)
                {
                    string actServer = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "ActiveServer", "act.deskplus.co.kr");
                    string ipWeb = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "WebServerIP", "127.0.0.1");
                    string ipMail = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "MailServerIP", "127.0.0.1");
                    string domain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "www.deskplus.co.kr");

                    string url = String.Format("http://{0}/las/activation.asp", actServer);
                    string param = String.Format("lickey={0}&IpWeb={1}&IpMail={2}&Domain={3}", licenseKey.Trim(), ipWeb.Trim(), ipMail.Trim(), domain.Trim());

                    rtnVal = UsePost(url, param);


                    if (rtnVal.IndexOf("NOT_FOUND_KEY") >= 0)
                    {
                        rtnVal = "Not Exist License Key";
                    }
                    else if (rtnVal.IndexOf("DUPLICATE_KEY") >= 0)
                    {
                        rtnVal = "Exist License Key";
                    }
                    else if (rtnVal.IndexOf("VALID_KEY") >= 0)
                    {
                        rtnVal = "Activated";
                    }
                    else
                    {
                        rtnVal = "Unknown Error";
                    }

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("ActActiveLicense " + url + param, AppSettingType.CONFIG);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("ActActiveLicense rtnVal " + rtnVal, AppSettingType.CONFIG);
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ActivelLicense[] : " + ex.ToString());
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 서버 활성화 (2차 인증 단계로 EP 설치 서버의 라이센스를 활성화 한다)
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string ActivelLicense(string p)
        {
            int rtnVal = -1;
            string rtnMsg = string.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            string licenseKey = GetDecryptParam("licenseKey", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin)
                {

                    string actServer = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "ActiveServer", "act.deskplus.co.kr");
                    string ipWeb = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "WebServerIP", "127.0.0.1");
                    string ipMail = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "MailServerIP", "127.0.0.1");
                    string domain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "www.deskplus.co.kr");

                    Interop.ZeoLicEp.Activation zeoLicMng = new Interop.ZeoLicEp.Activation();
                    rtnVal = zeoLicMng.Activate(actServer.Trim(), licenseKey.Trim(), ipWeb.Trim(), ipMail.Trim(), domain.Trim());

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("ActivelLicense " + licenseKey + " : " + ipWeb + " : " + ipMail + " : " + domain, AppSettingType.CONFIG);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("ActivelLicense rtnVal " + rtnVal, AppSettingType.CONFIG);

                    switch (rtnVal)
                    {
                        case 0:
                            rtnMsg = "라이센스가 활성화 되었습니다.";
                            break;
                        case 1:
                            rtnMsg = "접속오류가 발생하였습니다.";
                            break;
                        case 2:
                            rtnMsg = "License Key가 존재하지 않습니다.";
                            break;
                        case 3:
                            rtnMsg = "중복된 License Key가 존재합니다.";
                            break;
                        case 5:
                            rtnMsg = "내부 오류가 발생하였습니다.";
                            break;
                        default:
                            rtnMsg = "오류발생 : 관리자에게 문의하세요";
                            break;
                    }
                }

                return rtnMsg;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ActivelLicense[] : " + ex.ToString());
            }

            return rtnMsg;
        }


        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : POST로 전송
        /// </summary>
        /// <param name="_url">The _url.</param>
        /// <param name="_params">The _params.</param>
        /// <returns></returns>
        ////[AutoComplete]
        public string UsePost(string _url, string _params)
        {
            string rtnVal = "";

            WebResponse result = null;

            try
            {
                WebRequest req = WebRequest.Create(_url);
                req.Method = "POST";

                req.ContentType = "application/x-www-form-urlencoded";
                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;

                if (!String.IsNullOrEmpty(_params))
                {
                    int i = 0, j;
                    while (i < _params.Length)
                    {
                        j = _params.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //UrlEncoded.Append(HttpUtility.UrlEncode(_params.Substring(i, _params.Length - i)));
                            UrlEncoded.Append(_params.Substring(i, _params.Length - i));
                            break;
                        }
                        //UrlEncoded.Append(HttpUtility.UrlEncode(_params.Substring(i, j - i)));
                        UrlEncoded.Append(_params.Substring(i, j - i));
                        UrlEncoded.Append(_params.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.UTF8.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }

                result = req.GetResponse();
                Stream ReceiveStream = result.GetResponseStream();
                Encoding encode = System.Text.Encoding.UTF8;
                StreamReader sr = new StreamReader(ReceiveStream, encode);

                rtnVal = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(_url + "?" + _params + " UsePost [" + ex.Message + "]");
            }
            finally
            {
                if (result != null)
                {
                    result.Close();
                }
            }
            return rtnVal;
        }
    }
}
