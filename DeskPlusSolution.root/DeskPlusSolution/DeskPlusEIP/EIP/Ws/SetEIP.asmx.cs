using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using System.Web.Caching;
using System.Collections;

namespace Zeospace.DeskPlus.EIP.Web.Ws
{
    /// <summary>
    /// GetEIP의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetEIP : Base.BaseWSEIP
    {
        #region ADD

        #region COMMON 관련

        /// <summary>
        /// 회사 관리자 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddSubsideUser(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string userID = GetDecryptParam("userID", "");
            string roleCode = GetDecryptParam("roleCode", "");            

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    if (userID.Length > 0)
                    {
                        Zeospace.DeskPlus.EIP.DAL.UserRole dal = new DAL.UserRole();

                        if (userID.IndexOf(',') != -1)
                        {
                            foreach (string user in userID.Split(','))
                            {
                                BE.UserRoleInfo info = new BE.UserRoleInfo();
                                info.CompanyID = companyID;
                                info.RoleCode = roleCode;
                                info.UserID = user.ToNumber<int>();
                                info.Types = "GM"; //사용자
                                dal.Add(info);
                            }
                        }
                        else
                        {
                            BE.UserRoleInfo info = new BE.UserRoleInfo();
                            info.CompanyID = companyID;
                            info.RoleCode = roleCode;
                            info.UserID = userID.ToNumber<int>();
                            info.Types = "GM"; //사용자
                            dal.Add(info);
                        }
                    }

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddSubsideUser : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 회사 관리자 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSubsideUser(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string userID = GetDecryptParam("userID", "");
            string roleCode = GetDecryptParam("roleCode", "");                  

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    if (userID.Length > 0)
                    {
                        Zeospace.DeskPlus.EIP.DAL.UserRole dal = new DAL.UserRole();

                        if (userID.IndexOf(',') != -1)
                        {
                            foreach (string user in userID.Split(','))
                            {
                                BE.UserRoleInfo info = new BE.UserRoleInfo();
                                info.CompanyID = companyID;
                                info.RoleCode = roleCode;
                                info.UserID = user.ToNumber<int>();
                                dal.Del(info);
                            }
                        }
                        else
                        {
                            BE.UserRoleInfo info = new BE.UserRoleInfo();
                            info.CompanyID = companyID;
                            info.RoleCode = roleCode;
                            info.UserID = userID.ToNumber<int>();
                            dal.Del(info);
                        }
                    }

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelSubsideUser : " + ex);
                return false;
            }
        }

        #endregion

        #region 라이센스
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
                    rtnVal = zeoLicMng.Add(licenseKey);
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
                    string param = String.Format("lickey={0}&IpWeb={1}&IpMail={2}&Domain={3}", licenseKey, ipWeb, ipMail, domain);

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
                    rtnVal = zeoLicMng.Activate(actServer, licenseKey, ipWeb, ipMail, domain);

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

                return "";

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ActivelLicense[] : " + ex.ToString());
            }

            return rtnMsg;
        }
        #endregion

        #region EIP 관련

        /// <summary>
        /// ROLE 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddRole(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string roleCode = GetDecryptParam("roleCode", "");
            string roleName = GetDecryptParam("roleName", "");
            string roleDesc = GetDecryptParam("roleDesc", "");
            string gubun = GetDecryptParam("gubun", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    Zeospace.DeskPlus.EIP.BE.RoleInfo info = new BE.RoleInfo();
                    info.CompanyID = companyID;
                    info.RoleCode = roleCode;
                    info.RoleName = roleName;
                    info.RoleDesc = roleDesc;
                    info.Gubun = gubun;

                    Zeospace.DeskPlus.EIP.DAL.Role dal = new DAL.Role();
                    dal.Add(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddRole : " + ex);
                return false;
            }
        }


        /// <summary>
        /// 데이터연동 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddMenuDataLink(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            string encriptType = GetDecryptParam("encriptType", "");
            string encriptValue = GetDecryptParam("encriptValue", "");
            string linkName = GetDecryptParam("linkName", "");
            string linkValue = GetDecryptParam("linkValue", "");
            string description = GetDecryptParam("description", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {                    
                    Zeospace.DeskPlus.EIP.BE.MenuDataLinkInfo info = new BE.MenuDataLinkInfo();
                    info.CompanyID = companyID;
                    info.MenuID = menuID;
                    info.EncriptType = encriptType;
                    info.EncriptValue = encriptValue;
                    info.LinkName = linkName;
                    info.LinkValue = linkValue;
                    info.Description = description;

                    Zeospace.DeskPlus.EIP.DAL.MenuDataLink dal = new DAL.MenuDataLink();
                    dal.Add(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddMenuDataLink : " + ex);
                return false;
            }
        }       


        /// <summary>
        /// 메뉴의 권한 접근제어 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddMenuRole(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            string roleList = GetDecryptParam("roleList", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    if (roleList.Length > 0)
                    {
                        List<BE.MenuRoleInfo> al = new List<BE.MenuRoleInfo>();

                        foreach (string _authValue in roleList.Split(','))
                        {
                            BE.MenuRoleInfo authInfo = new BE.MenuRoleInfo();
                            authInfo.CompanyID = CurrentUserInfo.CompanyID;
                            authInfo.MenuID = menuID;
                            authInfo.RoleID = _authValue.ToNumber<int>();

                            al.Add(authInfo);
                        }

                        BLL.MenuRole biz = new BLL.MenuRole();
                        biz.AddTx(al);

                        return true;
                    }
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddMenuRole : " + ex);
                return false;
            }
        }

        /// <summary>
        /// ip 추가하기
        /// -2 : 오류발생; -1:접근 문제; 0:숫자비교 문제;1:추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddMenuIP(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            string startIP1 = GetDecryptParam("startIP1", "0").ToNumber<int>().ToString();
            string startIP2 = GetDecryptParam("startIP2", "0").ToNumber<int>().ToString();
            string startIP3 = GetDecryptParam("startIP3", "0").ToNumber<int>().ToString();
            string startIP4 = GetDecryptParam("startIP4", "0").ToNumber<int>().ToString();
            string endIP1 = GetDecryptParam("endIP1", "0").ToNumber<int>().ToString();
            string endIP2 = GetDecryptParam("endIP2", "0").ToNumber<int>().ToString();
            string endIP3 = GetDecryptParam("endIP3", "0").ToNumber<int>().ToString();
            string endIP4 = GetDecryptParam("endIP4", "0").ToNumber<int>().ToString();
            string ipAccess = GetDecryptParam("ipAccess", "0");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
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

                    BE.MenuIPInfo info = new BE.MenuIPInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.MenuID = menuID;
                    info.StartIP = startCompare;
                    info.EndIP = endCompare;
                    info.IPAccess = ipAccess;                    

                    DAL.MenuIP dal = new DAL.MenuIP();
                    dal.Add(info);

                    return "1";
                }
                
                return "-1";

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddMenuIP : " + ex);
                return "-2";
            }
        }

        /// <summary>
        /// 포틀릿 카테고리 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddPortletCategory(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string name = GetDecryptParam("name", "");
            string description = GetDecryptParam("description", "");
            string activeCategory = GetDecryptParam("activeCategory", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    BE.PortletCategoryInfo info = new BE.PortletCategoryInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.ParentCategoryID = categoryID;
                    info.NameBase = name;
                    info.Description = description;
                    info.ActiveCategory = activeCategory;
                    info.IsAdmin = "1";
                    info.WriteUserID = CurrentUserInfo.UserID;

                    DAL.PortletCategory dal = new DAL.PortletCategory();
                    return dal.Add(info);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPortletCategory : " + ex);
                return -1;
            }
        }

        /// <summary>
        /// Portlet 데이터연동 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddPortletDataLink(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            string encriptType = GetDecryptParam("encriptType", "");
            string encriptValue = GetDecryptParam("encriptValue", "");
            string targetType = GetDecryptParam("targetType", "");
            string targetDataType = GetDecryptParam("targetDataType", "");
            string linkName = GetDecryptParam("linkName", "");
            string linkType = GetDecryptParam("linkType", "");
            string linkValue = GetDecryptParam("linkValue", "");
            string description = GetDecryptParam("description", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    //값에 대해 직접입력(OU); LDAP (OL)
                    linkType = (linkType.Length > 0) ? "OL" : "OU";

                    Zeospace.DeskPlus.EIP.BE.PortletDataLinkInfo info = new BE.PortletDataLinkInfo();
                    info.CompanyID = companyID;
                    info.PortletID = portletID;
                    info.EncriptType = encriptType;
                    info.EncriptValue = encriptValue;
                    info.TargetType = targetType;
                    info.TargetDataType = targetDataType;
                    info.LinkName = linkName;
                    info.LinkValue = linkValue;
                    info.LinkType = linkType;
                    info.Description = description;

                    Zeospace.DeskPlus.EIP.DAL.PortletDataLink dal = new DAL.PortletDataLink();
                    dal.Add(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddMenuDataLink : " + ex);
                return false;
            }
        }


        /// <summary>
        /// Portlet 권한 접근제어 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddPortletRole(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            string roleList = GetDecryptParam("roleList", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    if (roleList.Length > 0)
                    {
                        List<BE.PortletRoleInfo> al = new List<BE.PortletRoleInfo>();

                        foreach (string _authValue in roleList.Split(','))
                        {
                            BE.PortletRoleInfo authInfo = new BE.PortletRoleInfo();
                            authInfo.CompanyID = CurrentUserInfo.CompanyID;
                            authInfo.PortletID = portletID;
                            authInfo.RoleID = _authValue.ToNumber<int>();

                            al.Add(authInfo);
                        }

                        BLL.PortletRole biz = new BLL.PortletRole();
                        biz.AddTx(al);

                        return true;
                    }
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddMenuRole : " + ex);
                return false;
            }
        }

        /// <summary>
        /// Portlet 탭 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddPortletTab(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            int targetPortletID = GetDecryptParam("targetPortletID", "0").ToNumber<int>();
            string name = GetDecryptParam("name", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    BE.PortletTabInfo info = new BE.PortletTabInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.PortletID = portletID;
                    info.TargetPortletID = targetPortletID;
                    info.NameBase = name;
                    info.WriteUserID = CurrentUserInfo.UserID;

                    DAL.PortletTab dal = new DAL.PortletTab();
                    dal.Add(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddPortletTab : " + ex);
                return false;
            }
        }

        #endregion


        #endregion

        #region MOD

        #region COMMON 관련
        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : syscode 순서 정렬 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModOrderSysCode(string p)
        {
            SetDecryptString(p);

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                string sortList = GetDecryptParam("sortList", ""); //카테고리값            
                sortList = sortList.TrimEnd(',');

                Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
                return dal.ModOrder(CurrentUserInfo.CompanyID, sortList);
            }

            return false;

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : syscode 활성화 처리
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ActiveSysCode(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string sysCodeList = GetDecryptParam("sysCodeList", "");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {

                if (sysCodeList.Length > 0)
                {
                    Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();

                    foreach (string _codeID in sysCodeList.Split(','))
                    {
                        dal.ModActive(companyID, _codeID.ToNumber<int>());
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : company 활성화 처리
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCompanyActive(string p)
        {
            SetDecryptString(p);
            string comList = GetDecryptParam("comList", "");

            if (CurrentUserInfo.IsSysAdmin)
            {

                if (comList.Length > 0)
                {
                    Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();

                    foreach (string company in comList.Split(','))
                    {
                        dal.ModActive(company.ToNumber<int>());
                    }
                }

                return true;
            }

            return false;
        }

        #endregion

        #region EIP
        

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 메뉴 및 권한, IP 들 초기화하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ResetMenuList(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int userID = GetDecryptParam("userID", "0").ToNumber<int>();

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                
                BLL.MenuIP bizMenuIP = new BLL.MenuIP();
                BLL.Menu bizMenu = new BLL.Menu();


                //메뉴별 IP 정보 - 캐쉬 초기화
                bizMenuIP.ClearCacheMenuIP(companyID);

                //메뉴 - 캐쉬 초기화
                bizMenu.ClearCacheMenu(companyID);

                //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                bizMenu.ClearCacheMenuUser(companyID);

                //메뉴별 IP 정보 - 캐쉬설정
                bizMenuIP.SetCacheMenuIP(companyID);

                //메뉴 - 캐쉬설정
                bizMenu.SetCacheMenuList(companyID);

                //사용자별 메뉴 정보 - 캐쉬설정
                bizMenu.SetCacheMenuListUser(companyID);
                
                HttpContext context = HttpContext.Current;
                string[] remoteAddrStr = context.Request.UserHostAddress.Split('.');
                string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

                //사용자 메뉴별 ROLE 및 IP 재설정                
                bizMenu.GetMenuListUser(companyID, userID, remoteAddr.ToDouble<double>());

                return true;
            }

            return false;
        }
        

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 메뉴 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModMenuOrder(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string sortList = GetDecryptParam("sortList", ""); //ROLEID 리스트들
            sortList = sortList.TrimEnd(',');

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                DAL.Menu dal = new DAL.Menu();
                return dal.ModOrder(companyID, sortList);
            }

            return false;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 상위 메뉴 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModParentMenuID(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            int parentMenuID = GetDecryptParam("parentMenuID", "0").ToNumber<int>();

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                Zeospace.DeskPlus.EIP.DAL.Menu dal = new DAL.Menu();
                return dal.ModParentMenuID(companyID, menuID, parentMenuID);
            }

            return false;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 권한 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModRoleOrder(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();            
            string sortList = GetDecryptParam("sortList", ""); //ROLEID 리스트들    
            string gubun = GetDecryptParam("gubun", ""); //구분자 (1:권한 2:등급)
            sortList = sortList.TrimEnd(',');

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {

                DAL.Role dal = new DAL.Role();
                return dal.ModRoleOrder(companyID, sortList, gubun);
            }

            return false;
        }        

        /// <summary>
        /// 포틀릿 카테고리 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModPortletCategory(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string name = GetDecryptParam("name", "");
            string multiName = GetDecryptParam("multiName", "");
            string description = GetDecryptParam("description", "");
            string activeCategory = GetDecryptParam("activeCategory", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    BE.PortletCategoryInfo info = new BE.PortletCategoryInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.CategoryID = categoryID;
                    info.NameBase = name;

                    if (multiName != "" && multiName.IndexOf(",") != -1)
                    {
                        info.NameKo = multiName.Split(',')[0];
                        info.NameEn = multiName.Split(',')[1];
                        info.NameJa = multiName.Split(',')[2];
                        info.NameFr = multiName.Split(',')[3];
                        info.NameZhcn = multiName.Split(',')[4];
                        info.NameZhtw = multiName.Split(',')[5];
                    }


                    info.Description = description;
                    info.ActiveCategory = activeCategory;

                    DAL.PortletCategory dal = new DAL.PortletCategory();
                    dal.Mod(info);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModPortletCategory : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 포틀릿 카테고리 순서 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModPortletCategoryOrder(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string sortList = GetDecryptParam("sortList", ""); //ROLEID 리스트들
            sortList = sortList.TrimEnd(',');

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                DAL.PortletCategory dal = new DAL.PortletCategory();
                dal.ModOrder(companyID, sortList);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 포틀릿 카테고리 이동하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModMovePortlet(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string portletID = GetDecryptParam("portletID", "");
            

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.Portlet dal = new DAL.Portlet();
                    dal.ModMove(companyID, categoryID, portletID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModMovePortlet : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 포틀릿 활성,비활성화 처리
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ActivePortlet(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string portletList = GetDecryptParam("portletList", "");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {

                if (portletList.Length > 0)
                {
                    DAL.Portlet dal = new DAL.Portlet();

                    foreach (string _portletID in portletList.Split(','))
                    {
                        dal.ModActive(companyID, categoryID, _portletID.ToNumber<int>());
                    }
                }

                return true;
            }

            return false;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 탭 순서변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModPortletTabOrder(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string sortList = GetDecryptParam("sortList", ""); //ROLEID 리스트들
            sortList = sortList.TrimEnd(',');

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                DAL.PortletTab dal = new DAL.PortletTab();
                dal.ModOrder(companyID, sortList);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 탭 이름 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModPortletTab(string p)
        {
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            int portletTabID = GetDecryptParam("portletTabID", "0").ToNumber<int>();
            int targetPortletID = GetDecryptParam("targetPortletID", "0").ToNumber<int>();
            string name = GetDecryptParam("name", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    BE.PortletTabInfo info = new BE.PortletTabInfo();
                    info.CompanyID = companyID;
                    info.PortletID = portletID;
                    info.PortletTabID = portletTabID;
                    info.TargetPortletID = targetPortletID;                    
                    info.NameBase = name;
                    info.WriteUserID = CurrentUserInfo.UserID;

                    DAL.PortletTab dal = new DAL.PortletTab();
                    dal.Mod(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModPortletTab : " + ex);
                return false;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 탭 이름 변경하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModPortletTabName(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
            int portletTabID = GetDecryptParam("portletTabID", "0").ToNumber<int>();
            
            string nameKo = GetDecryptParam("nameko", "");
            string nameEn = GetDecryptParam("nameEn", "");
            string nameJa = GetDecryptParam("nameJa", "");
            string nameFr = GetDecryptParam("nameFr", "");
            string nameZhcn = GetDecryptParam("nameZhcn", "");
            string nameZhtw = GetDecryptParam("nameZhtw", "");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                BE.PortletTabInfo info = new BE.PortletTabInfo();
                info.CompanyID = companyID;
                info.PortletID = portletID;
                info.PortletTabID = portletTabID;
                info.NameKo = nameKo;
                info.NameEn = nameEn;
                info.NameJa = nameJa;
                info.NameFr = nameFr;
                info.NameZhcn = nameZhcn;
                info.NameZhtw = nameZhtw;

                DAL.PortletTab dal = new DAL.PortletTab();
                dal.ModTabName(info);

                return true;
            }

            return false;
        }


        #endregion

        #endregion

        #region DEL
        #region 라이센스

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
        #endregion

        #region COMMON 관련

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : syscode 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSysCode(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string sysCodeList = GetDecryptParam("sysCodeList", "");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
                return dal.Del(companyID, sysCodeList);
            }

            return false;

        }

        #endregion

        #region EIP 관련

        
        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : role 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelRole(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int roleID = GetDecryptParam("roleID", "0").ToNumber<int>();
            string gubun = GetDecryptParam("gubun", "");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                Zeospace.DeskPlus.EIP.DAL.Role dal = new DAL.Role();
                return dal.Del(companyID, roleID, gubun);
            }

            return false;
        }

        /// <summary>
        /// role user 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelRoleUser(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int roleID = GetDecryptParam("roleID", "0").ToNumber<int>();
            int userID = GetDecryptParam("userID", "0").ToNumber<int>();
            string types = GetDecryptParam("types", "");

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    Zeospace.DeskPlus.EIP.BE.UserRoleInfo info = new BE.UserRoleInfo();
                    info.CompanyID = companyID;
                    info.RoleID = roleID;
                    info.UserID = userID;
                    info.Types = types;

                    Zeospace.DeskPlus.EIP.DAL.UserRole dal = new DAL.UserRole();
                    dal.Del(info);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelSubsideUser : " + ex);
                return false;
            }
        }
        
        /// <summary>
        /// 메뉴 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string DelMenu(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();                
                
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.Menu dal = new DAL.Menu();
                    DataSet ds = dal.GetMenuByPID(companyID, menuID.ToString());
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0) return "0";
                    }

                    dal.Del(companyID, menuID);

                    return "1";
                }

                return "-1";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelMenu : " + ex);
                return "-2";
            }
        }

        /// <summary>
        /// 데이터연동 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMenuDataLink(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int menuDataLinkID = GetDecryptParam("menuDataLinkID", "0").ToNumber<int>();

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    Zeospace.DeskPlus.EIP.DAL.MenuDataLink dal = new DAL.MenuDataLink();
                    dal.Del(companyID, menuDataLinkID);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelMenuDataLink : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 메뉴 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMenuRole(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int menuRoleID = GetDecryptParam("menuRoleID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.MenuRole dal = new DAL.MenuRole();                    
                    dal.Del(companyID, menuRoleID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelMenuRole : " + ex);
                return false;
            }
        }

        /// <summary>
        /// ip 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMenuIP(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int menuIPID = GetDecryptParam("menuIPID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.MenuIP dal = new DAL.MenuIP();
                    dal.Del(companyID, menuIPID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelMenuIP : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 포틀릿 카테고리 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortletCategory(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.PortletCategory dal = new DAL.PortletCategory();
                    dal.Del(companyID, categoryID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortletCategory : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 포틀릿 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortlet(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            string portletList = GetDecryptParam("portletList", "").TrimEnd(',');

            if (portletList.Length == 0) return false;

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    foreach (string id in portletList.Split(','))
                    {
                        DAL.Portlet dal = new DAL.Portlet();
                        dal.Del(companyID, categoryID, id.ToNumber<int>());
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortlet : " + ex);
                return false;
            }
        }


        /// <summary>
        /// 데이터연동 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortletDataLink(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            int portletDataLinkID = GetDecryptParam("portletDataLinkID", "0").ToNumber<int>();

            try
            {
                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    Zeospace.DeskPlus.EIP.DAL.PortletDataLink dal = new DAL.PortletDataLink();
                    dal.Del(companyID, portletDataLinkID);

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortletDataLink : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 메뉴 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortletRole(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int portletRoleID = GetDecryptParam("portletRoleID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.PortletRole dal = new DAL.PortletRole();
                    dal.Del(companyID, portletRoleID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortletRole : " + ex);
                return false;
            }
        }


        /// <summary>
        /// 포틀릿 탭 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortletTab(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();
                int portletTabID = GetDecryptParam("portletTabID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
                {
                    DAL.PortletTab dal = new DAL.PortletTab();
                    dal.Del(companyID, portletID, portletTabID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPortletTab : " + ex);
                return false;
            }
        }

        #endregion

        #endregion
    }
}
