using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.Security;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Xml;

namespace Zeospace.DeskPlusMobile.Web.Portal.Ws
{
    /// <summary>
    /// GetPortal의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetDefault : System.Web.Services.WebService
    {
        string mailID = String.Empty;
        private string decryString = string.Empty;//복고화 값

        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 로그인 처리
        /// 작성일자 : 2010-06-14
        /// </summary>
        /// <returns>페이지 정보</returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetLoginCheck(string _loginID, string _password, string _remember, string _reCompany, string _companyId, string _logType)
        {
            return LoginCheck(_loginID, _password, _remember, _reCompany, _companyId, _logType, true);
        }

        private string LoginCheck(string _loginID, string _password, string _remember, string _reCompany, string _companyId, string _logType, bool _isHash = true)
        {
            if (String.IsNullOrEmpty(_loginID) || String.IsNullOrEmpty(_password) || String.IsNullOrEmpty(_companyId))
            {
                return "!NULL";
            }

            string userEnv = String.Empty; //환경 정보
            string userUUID = String.Empty;

            try
            {
                Helper.AppException.WriteAssert(AppSettingType.SQLPortal, Context.Request.ServerVariables.ToString());

                int companyID = _companyId.ToNumber<int>();

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_SignIn");

                string password = _password;

                if (_isHash)
                {
                    password = HashManager.GetHash(_password);
                }

                sqlQuery = String.Format(sqlQuery, companyID, _loginID.ToXSS(), password);

                string userID = Convert.ToString(SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, null));

                if (String.IsNullOrEmpty(userID))
                {
                    //로그인 실패 로그
                    SetPwdErrorLog(companyID, _loginID.ToXSS());

                    return "!NULL";
                }
                else
                {
                    // 1. 사용자 환경설정 체크
                    string activeMobile = String.Empty;
                    string accessRight = String.Empty;
                    int userCheck = 0;
                    string PasswordFaultCount = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "PasswordFaultCount");
                    string UserCnt = "0";
                    
                    string sqlQuery1 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_SignCheck");
                    sqlQuery1 = String.Format(sqlQuery1, companyID, _loginID.ToXSS(), password);

                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery1, "GetSignCheck", null);

                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string uuid = ds.Tables[0].Rows[0]["UUID"].ToString().Trim();
                            if (!String.IsNullOrEmpty(uuid))
                            {
                                userCheck = 1;
                                accessRight = ds.Tables[0].Rows[0]["AccessRight"].ToString();
                            }
                        }

                        // 0 - 모바일앱에서만 사용가능 , 1- 모바일앱 + 모바일웹 둘다 사용가능, 2 - 모바일 사용불가
                        if (accessRight == "0")
                        {
                            return "ACCESSRIGHT";
                        }
                        else if (accessRight == "2")
                        {
                            return "AUTHFAIL";
                        }

                        if (Convert.ToInt32(PasswordFaultCount) > 0) //패스워드 틀림횟수가 지정되었다면
                        {
                            UserCnt = ds.Tables[0].Rows[0]["PwdCount"].ToString(); //사용자의 틀린정보를 가져오고
                            if (Convert.ToInt32(UserCnt) >= Convert.ToInt32(PasswordFaultCount)) //틀린정보가 xml값보다 크거나 같으면
                            {
                                return "!NULL";
                            }
                        }
                    }

                    // 2. 사용자 환경설정이 없을 경우 환경설정 기본값 정보 가져오기
                    if (userCheck == 0)
                    {
                        string sqlQuery2 = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_MDM_Env");
                        sqlQuery2 = String.Format(sqlQuery2, companyID);

                        DataSet ds2 = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery2, "GetEnv", null);

                        if (ds2 != null)
                        {
                            foreach (DataRow dr in ds2.Tables[0].Rows)
                            {
                                accessRight = dr["AccessRight"].ToString();

                                // 모바일 접근권한 체크
                                if (dr["AccessRight"].ToString() == "0")
                                {
                                    return "ACCESSRIGHT";
                                }
                            }
                        }
                    }

                    SetUserInfo(companyID, _loginID);

                    FormsAuthentication.SetAuthCookie(_loginID, false);

                    //로그인 접속 정보 로그
                    SetAccessLog(companyID, userID.ToNumber<int>(), _logType);

                    //환경 정보 가져오기
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_USERENVIRONMENT");
                    sqlQuery = String.Format(sqlQuery, companyID, userID);

                    using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, null))
                    {
                        if (sdr.Read())
                        {
                            userEnv = (sdr["MobilePims"] == DBNull.Value) ? "0|0|0|0|" : sdr["MobilePims"].ToString();

                            Session["DeskPlusMobileUserEnv"] = userEnv; //환경 정보 저장

                            Context.Response.Cookies["DeskPlusMobileUserEnv"].Value = userEnv;
                            Context.Response.Cookies["DeskPlusMobileUserEnv"].Path = "/";

                            // 스킨 기능 추가
                            Session["SkinType"] = (sdr["MobileSkin"] == DBNull.Value) ? Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "DefaultSkinType") : sdr["MobileSkin"].ToString();

                            // 결재 비밀번호 사용유무
                            Session["IsApprovePwd"] = (sdr["MobileIsApprovePwd"] == DBNull.Value) ? "N" : sdr["MobileIsApprovePwd"].ToString();

                            if (_remember.ToUpper() == "1")
                            {
                                String userArrStr = _loginID + ";";

                                Helper.Security.TripleDESManager tds = new Helper.Security.TripleDESManager();

                                Context.Response.Cookies["DPMAuth"].Value = tds.Encrypt(userArrStr.Trim(), System.Text.Encoding.UTF8);
                                Context.Response.Cookies["DPMAuth"].Path = "/";
                                Context.Response.Cookies["DPMAuth"].Expires = DateTime.Now.AddYears(2);
                            }
                            else
                            {
                                if (Context.Request.Cookies["DPMAuth"] != null)
                                {
                                    Context.Response.Cookies["DPMAuth"].Value = "";
                                    Context.Response.Cookies["DPMAuth"].Expires = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
                                }
                            }

                            if (_reCompany.ToUpper() == "1")
                            {
                                Context.Response.Cookies["DPMComanyAuth"].Value = _companyId;
                                Context.Response.Cookies["DPMComanyAuth"].Path = "/";
                                Context.Response.Cookies["DPMComanyAuth"].Expires = DateTime.Now.AddYears(2);
                            }
                            else
                            {
                                if (Context.Request.Cookies["DPMComanyAuth"] != null)
                                {
                                    Context.Response.Cookies["DPMComanyAuth"].Value = "";
                                    Context.Response.Cookies["DPMComanyAuth"].Expires = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetLogin[SetUserInfo] 2 : " + ex);
            }


            return "OK";
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 외부연동시 로그인 처리
        /// 작성일자 : 2012-12-12
        /// </summary>
        /// <returns></returns>        
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public void GetLoginOutSide(string _uID, string _uPW, string _uETC, string _uCID)
        {
            string userCheck = GetLoginCheck(_uID, _uPW, "false", "false", _uCID, "2");

            if (userCheck != "!NULL")
            {
                Context.Response.Redirect("/DeskPlusMobile/Portal/Portal.aspx", false);
            }
            else
            {
                Helper.AppException.WriteException("GetLoginOutSide : 인증 실패 uID=" + _uID + " , uPW=" + _uPW + ", uETC=" + _uETC + ", uCID=" + _uCID);
                Context.Response.Redirect("/DeskPlusMobile/Portal/Default.aspx", false);
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public void GetLoginOutSideENC(string p)
        {
            SetDecryptString(p);

            string uID = GetDecryptParam("uID", "");
            string uPW = GetDecryptParam("uPW", "");
            string uCID = GetDecryptParam("uCID", "");

            string userCheck = LoginCheck(uID, uPW, "false", "false", uCID, "3", false);

            if (userCheck != "!NULL")
            {
                Context.Response.Redirect("/DeskPlusMobile/Portal/Portal.aspx", false);
            }
            else
            {
                Helper.AppException.WriteException("GetLoginOutSide : 인증 실패 uID=" + uID + " , uPW=" + uPW + ", uCID=" + uCID);
                Context.Response.Redirect("/DeskPlusMobile/Portal/Default.aspx", false);
            }
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 로그 아웃 처리
        /// 작성일자 : 2010-06-14
        /// </summary>
        /// <returns></returns>        
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public void SetLogOut()
        {
            Context.Request.Cookies.Clear();
            Session.Clear();
            Session.Abandon();

            FormsAuthentication.SignOut();
            //FormsAuthentication.RedirectToLoginPage();
            Context.Response.Redirect("../LogOut.htm", false);
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 사용자 정보 저장(Session , Cookie)
        /// 작성일자 : 2010-06-14
        /// </summary>
        /// <returns></returns>
        private void SetUserInfo(int comapnyID, string _loginID)
        {
            //------------- Cookie 생성
            try
            {
                // 개인정보 가져오기
                //    0               1               2                    3                           4              5              6                         7                        8               9     
                // userID : loginID : companyID : companyName : companyCode : name : password : employeeNumber : mobilePhone : deptID 
                //         10                11                12                13               14            15		   16                       17                     18                19			
                // deptName : codeTitleID : TitleName :  codeDutyID : DutyName : mailID : mailDomain : mailPassword : privateQuota : companyQuota : 
                //	       20                                   21                 22                        23             
                // userDept(겸직 부서 ID) : rols(Role 정보) :  EnglishName : SecurityLogin

                String[] userInfoArr = GetUserInfo(comapnyID, _loginID);		//User.Identity.Name


                // 스트링 형식으로 변환
                String userArrStr = "";
                foreach (String userInfo in userInfoArr)
                {
                    userArrStr += userInfo;
                    userArrStr += "┐";
                }

                if (userArrStr != "")
                {
                    // 인증티켓 발급
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,                              // 버전 번호
                        _loginID,     // 티켓과 관련된 사용자 이름
                        DateTime.Now,                   // 쿠키가 발행된 시간
                        DateTime.Now.AddHours(24),       // 쿠키의 만료 날짜
                        true,                          // 영구 쿠키이면 true
                        userArrStr                      // 개인정보
                        );

                    // 인증 티켓 암호화
                    String cookieStr = FormsAuthentication.Encrypt(ticket);

                    string useSession = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_USESESSION");
                    if (useSession.ToUpper() == "TRUE")
                    {
                        Context.Session.Add("DeskPlusMobile_" + _loginID, cookieStr);
                        //Context.Session["DeskPlusMobile_" + _loginID] = cookieStr;
                    }

                    // 클라이언트에 쿠키 넣기
                    Context.Response.Cookies["DeskPlusMobile_" + _loginID].Value = cookieStr;
                    Context.Response.Cookies["DeskPlusMobile_" + _loginID].Path = "/";

                    string cookieExpiresHour = "0";
                    string cookieDomain = "";

                    try
                    {
                        cookieExpiresHour = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_COOKIEEXPIRESHOUR");
                        if (cookieExpiresHour != "")
                        {
                            Context.Response.Cookies["DeskPlusMobile_" + _loginID].Expires = DateTime.Now.AddHours(Convert.ToInt32(cookieExpiresHour));
                        }

                        cookieDomain = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_COOKIEDOMAIN");
                        if (cookieDomain != "")
                        {
                            Context.Response.Cookies["DeskPlusMobile_" + _loginID].Domain = cookieDomain;
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.AppException.WriteException("GetLogin[SetUserInfo] 1 : " + ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetLogin[SetUserInfo] 2 : " + ex);
            }
        }

        /// <summary>
        /// 작성자   : freeman
        /// 작성목적 : 사용자 정보 가져오기
        /// 작성일자 : 2010-06-14
        /// </summary>
        /// <returns></returns>
        public String[] GetUserInfo(int companyID, string _loginID)
        {
            ArrayList userInfoArr = new ArrayList();

            SqlParameter[] parms = new SqlParameter[] {
                                new SqlParameter("@CompanyID", SqlDbType.Int),
							    new SqlParameter("@LoginID", SqlDbType.NVarChar, 50)
							};
            parms[0].Value = companyID;
            parms[1].Value = _loginID.ToXSS();

            int userID = -1;
            string securityLogin = "";

            int deptID = -1;

            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_PROC_USERINFO"), parms))
            {
                // 개인정보 가져오기
                if (sdr.Read())
                {
                    userInfoArr.Add((sdr["UserID"] == DBNull.Value) ? "-1" : sdr["UserID"].ToString());
                    userID = (sdr["UserID"] == DBNull.Value) ? -1 : Convert.ToInt32(sdr["UserID"]);
                    userInfoArr.Add((sdr["LoginID"] == DBNull.Value) ? "" : sdr["LoginID"].ToString());
                    userInfoArr.Add((sdr["CompanyID"] == DBNull.Value) ? "-1" : sdr["CompanyID"].ToString());

                    string tmpLang = sdr["PreferredLanguage"].ToString();
                    if (String.IsNullOrEmpty(tmpLang))
                    {
                        tmpLang = "ko";
                    }

                    string tmpComName = StringHelper.GetLangName(sdr["ComBase"].ToString(), sdr["ComKo"].ToString(), sdr["ComEn"].ToString(), sdr["ComJa"].ToString(),
                        sdr["ComFr"].ToString(), sdr["ComZhcn"].ToString(), sdr["ComZhtw"].ToString(), tmpLang);

                    userInfoArr.Add((sdr["ComBase"] == DBNull.Value) ? "" : tmpComName);
                    //userInfoArr.Add((sdr["ComBase"] == DBNull.Value) ? "" : sdr["ComBase"].ToString());
                    userInfoArr.Add((sdr["CompanyCode"] == DBNull.Value) ? "" : sdr["CompanyCode"].ToString());
                    userInfoArr.Add((sdr["NameBase"] == DBNull.Value) ? "" : sdr["NameBase"].ToString());
                    userInfoArr.Add((sdr["NameKo"] == DBNull.Value) ? "" : sdr["NameKo"].ToString());
                    userInfoArr.Add((sdr["NameEn"] == DBNull.Value) ? "" : sdr["NameEn"].ToString());
                    userInfoArr.Add((sdr["NameJa"] == DBNull.Value) ? "" : sdr["NameJa"].ToString());
                    userInfoArr.Add((sdr["NameFr"] == DBNull.Value) ? "" : sdr["NameFr"].ToString());
                    userInfoArr.Add((sdr["NameZhCn"] == DBNull.Value) ? "" : sdr["NameZhCn"].ToString());
                    userInfoArr.Add((sdr["NameZhTw"] == DBNull.Value) ? "" : sdr["NameZhTw"].ToString());
                    userInfoArr.Add((sdr["Password"] == DBNull.Value) ? "" : sdr["Password"].ToString());
                    userInfoArr.Add((sdr["EmployeeNumber"] == DBNull.Value) ? "" : sdr["EmployeeNumber"].ToString());
                    userInfoArr.Add((sdr["MobilePhone"] == DBNull.Value) ? "" : sdr["MobilePhone"].ToString());
                    userInfoArr.Add((sdr["DeptID"] == DBNull.Value) ? "-1" : sdr["DeptID"].ToString());
                    deptID = (sdr["DeptID"] == DBNull.Value) ? -1 : Convert.ToInt32(sdr["DeptID"]);
                    userInfoArr.Add((sdr["DeptBase"] == DBNull.Value) ? "" : sdr["DeptBase"].ToString());
                    userInfoArr.Add((sdr["DeptKo"] == DBNull.Value) ? "" : sdr["DeptKo"].ToString());
                    userInfoArr.Add((sdr["DeptEn"] == DBNull.Value) ? "" : sdr["DeptEn"].ToString());
                    userInfoArr.Add((sdr["DeptJa"] == DBNull.Value) ? "" : sdr["DeptJa"].ToString());
                    userInfoArr.Add((sdr["DeptFr"] == DBNull.Value) ? "" : sdr["DeptFr"].ToString());
                    userInfoArr.Add((sdr["DeptZhCn"] == DBNull.Value) ? "" : sdr["DeptZhCn"].ToString());
                    userInfoArr.Add((sdr["DeptZhTw"] == DBNull.Value) ? "" : sdr["DeptZhTw"].ToString());
                    userInfoArr.Add((sdr["CodeTitleID"] == DBNull.Value) ? "-1" : sdr["CodeTitleID"].ToString());
                    userInfoArr.Add((sdr["TitleBase"] == DBNull.Value) ? "" : sdr["TitleBase"].ToString());
                    userInfoArr.Add((sdr["TitleKo"] == DBNull.Value) ? "" : sdr["TitleKo"].ToString());
                    userInfoArr.Add((sdr["TitleEn"] == DBNull.Value) ? "" : sdr["TitleEn"].ToString());
                    userInfoArr.Add((sdr["TitleJa"] == DBNull.Value) ? "" : sdr["TitleJa"].ToString());
                    userInfoArr.Add((sdr["TitleFr"] == DBNull.Value) ? "" : sdr["TitleFr"].ToString());
                    userInfoArr.Add((sdr["TitleZhCn"] == DBNull.Value) ? "" : sdr["TitleZhCn"].ToString());
                    userInfoArr.Add((sdr["TitleZhTw"] == DBNull.Value) ? "" : sdr["TitleZhTw"].ToString());
                    userInfoArr.Add((sdr["CodeDutyID"] == DBNull.Value) ? "-1" : sdr["CodeDutyID"].ToString());
                    userInfoArr.Add((sdr["DutyBase"] == DBNull.Value) ? "" : sdr["DutyBase"].ToString());
                    userInfoArr.Add((sdr["DutyKo"] == DBNull.Value) ? "" : sdr["DutyKo"].ToString());
                    userInfoArr.Add((sdr["DutyEn"] == DBNull.Value) ? "" : sdr["DutyEn"].ToString());
                    userInfoArr.Add((sdr["DutyJa"] == DBNull.Value) ? "" : sdr["DutyJa"].ToString());
                    userInfoArr.Add((sdr["DutyFr"] == DBNull.Value) ? "" : sdr["DutyFr"].ToString());
                    userInfoArr.Add((sdr["DutyZhCn"] == DBNull.Value) ? "" : sdr["DutyZhCn"].ToString());
                    userInfoArr.Add((sdr["DutyZhTw"] == DBNull.Value) ? "" : sdr["DutyZhTw"].ToString());
                    userInfoArr.Add((sdr["MailID"] == DBNull.Value) ? "" : sdr["MailID"].ToString());
                    userInfoArr.Add((sdr["MailDomain"] == DBNull.Value) ? "" : sdr["MailDomain"].ToString());
                    userInfoArr.Add((sdr["MailPassword"] == DBNull.Value) ? "" : sdr["MailPassword"].ToString());
                    userInfoArr.Add((sdr["PrivateQuota"] == DBNull.Value) ? "0" : sdr["PrivateQuota"].ToString());
                    userInfoArr.Add((sdr["CompanyQuota"] == DBNull.Value) ? "0" : sdr["CompanyQuota"].ToString());
                    securityLogin = (sdr["SecurityLogin"] == DBNull.Value) ? "" : sdr["SecurityLogin"].ToString();
                    userInfoArr.Add((sdr["PreferredLanguage"] == DBNull.Value) ? "Ko" : sdr["PreferredLanguage"].ToString());

                    mailID = (sdr["MailID"] == DBNull.Value) ? "" : sdr["MailID"].ToString() + "@" + sdr["MailDomain"].ToString();

                }
            }

            //부서 추가
            if (userID > -1 && companyID > -1)
            {
                #region 부서정보
                string userDept = "";
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_USERDEPTS");
                sql = string.Format(sql, companyID, userID);
                using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, null))
                {
                    while (sdr.Read())
                    {
                        if (userDept.Length > 0) userDept += "|";
                        userDept += sdr["DeptID"].ToString();
                    }
                }
                userInfoArr.Add(userDept);
                #endregion

                #region Role 추가
                string userRole = "";

                SqlParameter[] parm = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@UserID", SqlDbType.Int),
                                               new SqlParameter("@DeptID", SqlDbType.Int)
										   };
                parm[0].Value = companyID;
                parm[1].Value = userID;
                parm[2].Value = deptID;

                using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_PROC_USERROLES"), parm))
                {
                    while (sdr.Read())
                    {
                        if (userRole.Length > 0) userRole += "|";
                        userRole += sdr["RoleID"].ToString();

                        /*
                        for (int i = 0; i < mobileRoleID.Length; i++)
                        {
                            if (mobileRoleID[i] == sdr["RoleID"].ToString())
                            {
                                authCheck = true;
                            }
                        }
                        */
                    }
                }
                userInfoArr.Add(userRole);
                #endregion

                #region 모바일 메뉴권한

                string menuRole = "";

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_USERROLE");
                sqlQuery = String.Format(sqlQuery, companyID, userID);

                using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, null))
                {
                    while (sdr.Read())
                    {
                        if (menuRole.Length > 0) menuRole += "|";
                        menuRole += sdr["GubunID"].ToString();
                    }
                }

                userInfoArr.Add(menuRole);
                #endregion

                //SecurityLogin
                userInfoArr.Add(securityLogin);
            }

            return (String[])userInfoArr.ToArray(typeof(String));
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 로그인 접속 정보 로그
        /// 작성일자 : 2013-05-24
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        private void SetAccessLog(int _companyID, int _userID, string _logType)
        {
            if (_companyID > 0 && _userID > 0)
            {
                //string[] remoteAddrStr = Context.Request.ServerVariables["REMOTE_ADDR"].Split('.');             
                string[] remoteAddrStr = Context.Request.UserHostAddress.Split('.');
                string remoteAddr = GetIPAddress(remoteAddrStr);

                AddLogin(_companyID, _userID, remoteAddr, _logType);
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 아이피 정보를 문자열로 반환
        /// 작성일자 : 2013-05-24
        /// </summary>
        /// <param name="_remoteAddrStr"></param>
        /// <returns></returns>
        public static string GetIPAddress(string[] _remoteAddrStr)
        {
            string iPAddress1 = String.Empty;
            string iPAddress2 = String.Empty;
            string iPAddress3 = String.Empty;
            string iPAddress4 = String.Empty;
            string remoteAddr = String.Empty;

            try
            {
                iPAddress1 = Utility.SetFrontDigit(3, _remoteAddrStr[0].ToString(), '0');
                iPAddress2 = Utility.SetFrontDigit(3, _remoteAddrStr[1].ToString(), '0');
                iPAddress3 = Utility.SetFrontDigit(3, _remoteAddrStr[2].ToString(), '0');
                iPAddress4 = Utility.SetFrontDigit(3, _remoteAddrStr[3].ToString(), '0');
                remoteAddr = iPAddress1 + iPAddress2 + iPAddress3 + iPAddress4;
            }
            catch
            {
                remoteAddr = "000000000000";
            }

            return remoteAddr;
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 로그인 접속 로그
        /// 작성일자 : 2013-05-24
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_remoteAddr"></param>
        /// <param name="_logType"></param>
        /// <returns></returns>
        public bool AddLogin(int _companyID, int _userID, string _remoteAddr, string _logType)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@IPAddress", SqlDbType.Char,12)
                                               ,new SqlParameter("@LogType", SqlDbType.Char,1)
                };

                parms[0].Value = _companyID;
                parms[1].Value = _userID;
                parms[2].Value = _remoteAddr;
                parms[3].Value = _logType;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "INS_PROC_ACCESSLOG");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsAddLogin", parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddLogin(" + _companyID + "," + _userID + "," + _remoteAddr + ")", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 로그인 실패 정보 로그
        /// 작성일자 : 2013-05-24
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_loginID"></param>
        private void SetPwdErrorLog(int _companyID, string _loginID)
        {
            if (_companyID > 0 && !String.IsNullOrEmpty(_loginID))
            {
                string[] remoteAddrStr = Context.Request.UserHostAddress.Split('.');
                string remoteAddr = GetIPAddress(remoteAddrStr);

                AddPwdErr(_companyID, _loginID, remoteAddr);
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 로그인 실패 정보 로그
        /// 작성일자 : 2013-05-24
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_loginID"></param>
        /// <param name="_remoteAddr"></param>
        /// <returns></returns>
        public bool AddPwdErr(int _companyID, string _loginID, string _remoteAddr)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@LogInID", SqlDbType.NVarChar,50)                                                 
                                               ,new SqlParameter("@IPAddress", SqlDbType.Char,12)
                };

                parms[0].Value = _companyID;
                parms[1].Value = _loginID;
                parms[2].Value = _remoteAddr;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "INS_PROC_PWDERRORLOG");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsAddPwdErr", parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddPwdErr(" + _companyID + "," + _loginID + "," + _remoteAddr + ")", ex);
                return false;
            }
        }


        /// <summary>
        /// 암호화
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        [WebMethod]
        public string AjaxGetEncrypt(string _data)
        {
            if (String.IsNullOrEmpty(_data)) return "!NULL";

            try
            {
                Helper.Security.TripleDESManager tds = new Helper.Security.TripleDESManager();
                return tds.Encrypt(_data.Trim(), System.Text.Encoding.UTF8);
            }
            catch
            {
                return "!NULL";
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : uuid 저장
        /// 작성일자 : 2013-01-03
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_userName"></param>
        /// <param name="_uuID"></param>
        /// <param name="_regID"></param>
        /// <param name="_phoneNum"></param>
        /// <param name="_mobileType"></param>
        /// <param name="_deviceName"></param>
        /// <returns></returns>
        private string AddUUID(int _companyID, string _userID, string _userName, string _uuID, string _regID, string _phoneNum, string _mobileType, string _deviceName)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@UUID", SqlDbType.NVarChar,200)                                               
                                               ,new SqlParameter("@RegID", SqlDbType.NVarChar,300)  
                                               ,new SqlParameter("@MobileNumber", SqlDbType.Char,15)   
                                               ,new SqlParameter("@MobileType", SqlDbType.NVarChar,100)   
                                               ,new SqlParameter("@DeviceName", SqlDbType.NVarChar,200)  
                                               
                };

                parms[0].Value = _companyID;
                parms[1].Value = _userID;
                parms[2].Value = _uuID;
                parms[3].Value = _regID;
                parms[4].Value = _phoneNum;
                parms[5].Value = _mobileType;
                parms[6].Value = _deviceName;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "INS_PROC_MobileList");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsAddUUID", parms);

                string rtn = ds.Tables["dsAddUUID"].Rows[0][0].ToString();

                if (rtn == "0")
                {
                    // 알람 처리 (mdm 모바일관리관리자 정보 가져오기)
                    string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_UserRole");
                    sqlQuery = String.Format(sqlQuery, _companyID);
                    DataSet dsRole = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "mdmInfo", null);

                    if (dsRole != null)
                    {
                        foreach (DataRow dr in dsRole.Tables[0].Rows)
                        {
                            SendAlarm(_companyID, _userID.ToNumber<int>(), _userName, dr["UserID"].ToString().ToNumber<int>(), _uuID);
                        }
                    }
                }

                return rtn;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddUUID(" + _companyID.ToString() + "," + _userID + "," + _uuID + "," + _regID + ")", ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 승인불가 처리 정보 가져오기
        /// 작성일자 : 2015-10-21
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        private int GetAuth(int _companyID, string _userID)
        {
            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_Auth");
                sqlQuery = String.Format(sqlQuery, _companyID, _userID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "authInfo", null);
                          
                return ds.Tables[0].Rows.Count;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("GetAuth(" + _companyID.ToString() + "," + _userID + ")", ex);
                return 1;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 로그인 처리
        /// 작성일자 : 2013-01-07
        /// </summary>
        /// <returns>페이지 정보</returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCheckConfirm(string p)
        {
            SetDecryptString(p);
            string _loginID = GetDecryptParam("loginID", "");
            string _password = GetDecryptParam("password", "");
            string _companyId = GetDecryptParam("companyID", "");
            string _uuID = GetDecryptParam("uuID", "");
            string _phoneNum = GetDecryptParam("phoneNum", "");
            string _mobileType = GetDecryptParam("mobileType", "");
            string _deviceName = GetDecryptParam("deviceName", "");
            string _regID = GetDecryptParam("regID", "");

            if (String.IsNullOrEmpty(_loginID) || String.IsNullOrEmpty(_password) || String.IsNullOrEmpty(_companyId) || String.IsNullOrEmpty(_uuID))
            {
                return "!NULL";
            }

            string userEnv = String.Empty; //환경 정보            
            string dbUUID = String.Empty;
            string userID = String.Empty;
            string userName = String.Empty;
            string activeMobile = String.Empty;
            string activeCheck = String.Empty;
            string accessRight = String.Empty;

            try
            {
                int companyID = _companyId.ToNumber<int>();

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_SignCheck");

                // & 기호 ~zeo~변환해서 호출하기때문에 재변환 (파라미터구분으로 인식해서 처리)
                string password = _password.Replace("~zeo~", "&");
                password = HashManager.GetHash(password);
                sqlQuery = String.Format(sqlQuery, companyID, _loginID.ToXSS(), password);
                string PasswordFaultCount = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "PasswordFaultCount");
                string UserCnt = "0";

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetUser", null);


                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        userID = dr["UserID"].ToString();
                        userName = dr["NameBase"].ToString();
                        accessRight = dr["AccessRight"].ToString();
                        dbUUID += dr["UUID"].ToString() + "^";
                        activeMobile += dr["ActiveMobile"].ToString() + "^";
                        UserCnt = dr["PwdCount"].ToString(); //사용자 패스워드 틀린횟수 가져오기
                    }

                    if (Convert.ToInt32(PasswordFaultCount) > 0) //패스워드 틀림횟수가 지정되었다면
                    {                        
                        if (Convert.ToInt32(UserCnt) >= Convert.ToInt32(PasswordFaultCount)) //틀린정보가 xml값보다 크거나 같으면
                        {
                            return "!NULL";
                        }
                    }
                }


                if (String.IsNullOrEmpty(userID))
                {
                    //로그인 실패 로그
                    SetPwdErrorLog(companyID, _loginID.ToXSS());

                    return "!NULL";
                }
                else
                {
                    //사용자 모바일 접속 불가 처리
                    if (accessRight == "2")
                    {
                        return "ACCESSRIGHT";
                    }

                    if (String.IsNullOrEmpty(dbUUID.Replace("^", ""))) //저장한 사용자 기기가 없을 때
                    {
                        /*
                        if (String.IsNullOrEmpty(_regID) && (_mobileType.ToUpper() != "ANDROID")) // 아이폰에서 알람을 설정하지 않으면 regID값을 받아 올수 없음.
                        {
                            return "NOREGID";
                        }
                        */

                        if (GetAuth(companyID, userID) < 1) // 승인 불가 처리한 사용자 인지 확인
                        {
                            string tmpApprove = AddUUID(companyID, userID, userName, _uuID, _regID, _phoneNum, _mobileType, _deviceName);

                            if (tmpApprove == "0") //관리자 승인이 필요
                            {
                                return "WAIT";
                            }
                            else if (tmpApprove == "1") //자동승인
                            {
                                return "OK";
                            }
                            else
                            {
                                return "!NULL";
                            }
                        }
                        else
                        {
                            return "NOAUTH";
                        }
                    }
                    else
                    {
                        if (dbUUID.Replace("^", "") == "0") //기기 없이 환경설정을 저장한 경우
                        {
                            /*
                            if (String.IsNullOrEmpty(_regID) && (_mobileType.ToUpper() != "ANDROID"))
                            {
                                return "NOREGID";
                            }
                            */

                            if (GetAuth(companyID, userID) < 1)
                            {
                                string tmpApprove = AddUUID(companyID, userID, userName, _uuID, _regID, _phoneNum, _mobileType, _deviceName);

                                if (tmpApprove == "0")
                                {
                                    return "WAIT";
                                }
                                else if (tmpApprove == "1")
                                {
                                    return "OK";
                                }
                                else
                                {
                                    return "!NULL";
                                }
                            }
                            else
                            {
                                return "NOAUTH";
                            }
                        }
                        else
                        {
                            // 이미 승인 받은 기기인지 확인.
                            string[] arrdbUUID = dbUUID.Split('^');
                            string[] arrActiveMobile = activeMobile.Split('^');
                            for (int i = 0; i < arrdbUUID.Length; i++)
                            {
                                if (_uuID == arrdbUUID[i])
                                {
                                    activeCheck = arrActiveMobile[i];
                                    break;
                                }
                            }
                        }
                    }

                    if (String.IsNullOrEmpty(activeCheck)) // 이미 승인 받은 기기에 포함되어 있지 않은 경우
                    {
                        /*
                        if (String.IsNullOrEmpty(_regID) && (_mobileType.ToUpper() != "ANDROID"))
                        {
                            return "NOREGID";
                        }
                        */

                        if (GetAuth(companyID, userID) < 1)
                        {
                            string tmpApprove = AddUUID(companyID, userID, userName, _uuID, _regID, _phoneNum, _mobileType, _deviceName);

                            if (tmpApprove == "0")
                            {
                                return "WAIT";
                            }
                            else if (tmpApprove == "1")
                            {
                                return "OK";
                            }
                            else
                            {
                                return "!NULL";
                            }
                        }
                        else
                        {
                            return "NOAUTH";
                        }

                    }
                    else if (activeCheck == "1") // 로그인 가능
                    {
                        return "OK";

                    }
                    else if (activeCheck == "2") // 관리자가 승인한 이후에 접속 불가 처리
                    {
                        return "ACCESSRIGHT";

                    }
                    else if (activeCheck == "3") // 관리자가 승인 불가 처리
                    {
                        return "NOAUTH";

                    }
                    else
                    {
                        return "WAIT";
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetLogin[SetUserInfo] 2 : " + ex);
                return "!NULL";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 회사 정보 가져오기
        /// 작성일자 : 2013-01-07
        /// </summary>
        /// <returns></returns>        
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCompany(string p)
        {
            try
            {
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_Company");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "CompanyInfo", null);

                return StringHelper.GetJSONString(ds.Tables[0]).ToReverseXSS();
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetCompany()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 외부연동시 로그인 처리
        /// 작성일자 : 2013-01-25
        /// </summary>
        /// <returns></returns>        
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public void GetLoginOutSideApp(string p)
        {
            SetDecryptString(p);
            string uID = GetDecryptParam("uID", "");
            string uCID = GetDecryptParam("uCID", "");
            string uuID = GetDecryptParam("uuID", "");
            string regID = GetDecryptParam("regID", "");

            string userCheck = GetLoginCheckApp(uID, uCID, uuID, regID);

            if (userCheck != "!NULL")
            {
                Context.Response.Redirect("/DeskPlusMobile/Portal/Portal.aspx", false);
            }
            else
            {
                Helper.AppException.WriteException("GetLoginOutSide : 인증 실패 uID=" + uID + " ,uCID=" + uCID);
                Context.Response.Redirect("/DeskPlusMobile/Portal/LogOut.aspx", false);
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 로그인 처리
        /// 작성일자 : 2013-01-25
        /// </summary>
        /// <returns>페이지 정보</returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetLoginCheckApp(string _loginID, string _companyId, string _uuID, string _regID)
        {
            if (String.IsNullOrEmpty(_loginID) || String.IsNullOrEmpty(_companyId))
            {
                return "!NULL";
            }

            string userEnv = String.Empty; //환경 정보
            string userUUID = String.Empty;

            try
            {
                Helper.AppException.WriteAssert(AppSettingType.SQLPortal, Context.Request.ServerVariables.ToString());

                int companyID = _companyId.ToNumber<int>();
                string userID = String.Empty;
                string mobileType = String.Empty;
                string dbRegID = String.Empty;
                string logOutYN = String.Empty;

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_SignInApp");
                sqlQuery = String.Format(sqlQuery, companyID, _loginID.ToXSS(), _uuID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetSign", null);

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        userID = ds.Tables[0].Rows[0]["UserID"].ToString().Trim();
                        mobileType = ds.Tables[0].Rows[0]["MobileType"].ToString().Trim();
                        dbRegID = ds.Tables[0].Rows[0]["RegID"].ToString().Trim();
                        logOutYN = ds.Tables[0].Rows[0]["LogOutYN"].ToString().Trim();
                    }
                }

                if (String.IsNullOrEmpty(userID))
                {
                    //로그인 실패 로그
                    SetPwdErrorLog(companyID, _loginID.ToXSS());
                    return "!NULL";
                }
                else
                {
                    if (_regID != "")
                    {
                        if ((dbRegID == "") || (dbRegID != _regID))
                        {
                            UpdRegID(companyID, userID, _uuID, _regID);
                        }
                    }

                    if (logOutYN == "Y")
                    {
                        UpdLogOutYN(companyID, userID, _uuID);
                    }

                    SetUserInfo(companyID, _loginID);

                    FormsAuthentication.SetAuthCookie(_loginID, false);

                    //로그인 접속 정보 로그
                    SetAccessLog(companyID, userID.ToNumber<int>(), "1");

                    Session["UUID"] = _uuID;
                    Context.Response.Cookies["UUID"].Value = _uuID;
                    Context.Response.Cookies["UUID"].Path = "/";

                    Session["MobileType"] = mobileType;
                    Context.Response.Cookies["MobileType"].Value = mobileType;
                    Context.Response.Cookies["MobileType"].Path = "/";

                    Session["RegID"] = _regID;
                    Context.Response.Cookies["RegID"].Value = _regID;
                    Context.Response.Cookies["RegID"].Path = "/";

                    String userArrStr = _loginID + ";";

                    Helper.Security.TripleDESManager tds = new Helper.Security.TripleDESManager();

                    Context.Response.Cookies["DPMAuth"].Value = tds.Encrypt(userArrStr.Trim(), System.Text.Encoding.UTF8);
                    Context.Response.Cookies["DPMAuth"].Path = "/";
                    Context.Response.Cookies["DPMAuth"].Expires = DateTime.Now.AddYears(2);

                    Session["DPMComanyAuth"] = companyID;
                    Context.Response.Cookies["DPMComanyAuth"].Value = companyID.ToString();
                    Context.Response.Cookies["DPMComanyAuth"].Path = "/";

                    //환경 정보 가져오기
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_USERENVIRONMENT");
                    sqlQuery = String.Format(sqlQuery, companyID, userID);

                    using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, null))
                    {
                        if (sdr.Read())
                        {
                            userEnv = (sdr["MobilePims"] == DBNull.Value) ? "0|0|0|0|" : sdr["MobilePims"].ToString();

                            Session["DeskPlusMobileUserEnv"] = userEnv; //환경 정보 저장

                            Context.Response.Cookies["DeskPlusMobileUserEnv"].Value = userEnv;
                            Context.Response.Cookies["DeskPlusMobileUserEnv"].Path = "/";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetLogin[SetUserInfo] 2 : " + ex);
            }


            return "OK";
        }


        /// <summary>
        /// 작성자   : freeman 
        /// 작성목적 : Badge 정보 가져오기
        /// 작성일자 : 2017-02-09
        /// </summary>
        /// <returns></returns> 
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetBadgeNum(string p)
        {
            SetDecryptString(p);
            string cID = GetDecryptParam("cID", "");
            string lID = GetDecryptParam("lID", "");
            string uuID = GetDecryptParam("uuID", "");

            if (String.IsNullOrEmpty(cID) || String.IsNullOrEmpty(lID) || String.IsNullOrEmpty(uuID))
            {
                return 0;
            }

            long number1 = 0;
            bool canConvert = long.TryParse(cID, out number1);

            if (!canConvert)
            {
                return 0;
            }

            try
            {
                string rtnVal = String.Empty;

                #region 결재 건수 (결재대상문서,대결결재대상문서 건수)
                int appCount = 0;
                string sqlAppQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_PROC_BADGECNT");
                SqlParameter[] parm = new SqlParameter[]{
                                            new SqlParameter("@CompanyID", SqlDbType.Int)
                                        ,new SqlParameter("@LoginID", SqlDbType.VarChar, 50)
                };

                parm[0].Value = Convert.ToInt32(cID);
                parm[1].Value = lID.ToXSS();


                DataSet dsApp = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlAppQuery, "dsList", parm);

                DataRow[] dr = dsApp.Tables["dsList"].Select("CAT = 'AL'");
                appCount = dr.Length;

                dr = dsApp.Tables["dsList"].Select("CAT = 'FL'");
                appCount += dr.Length;
                #endregion


                return appCount;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetBadgeNum()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : freeman 
        /// 작성목적 : 주소록 백업 정보 가져오기
        /// 작성일자 : 2017-02-09
        /// </summary>
        /// <returns></returns> 
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SyncContact(string p)
        {
            SetDecryptString(p);
            string cID = GetDecryptParam("cID", "");
            string lID = GetDecryptParam("lID", "");
            string list = GetDecryptParam("list", "");

            if (String.IsNullOrEmpty(cID) || String.IsNullOrEmpty(lID) || String.IsNullOrEmpty(list))
            {
                return "";
            }

            long number1 = 0;
            bool canConvert = long.TryParse(cID, out number1);

            if (!canConvert)
            {
                return "";
            }

            try
            {
                string rtnVal = String.Empty;



                return "";
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("SyncContact()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 환경정보 가져오기
        /// 작성일자 : 2013-02-12
        /// </summary>
        /// <returns></returns> 
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetEnv(string p)
        {
            SetDecryptString(p);
            string cID = GetDecryptParam("cID", "");
            string lID = GetDecryptParam("lID", "");
            string uuID = GetDecryptParam("uuID", "");

            if (String.IsNullOrEmpty(cID) || String.IsNullOrEmpty(lID) || String.IsNullOrEmpty(uuID))
            {
                return "!NULL";
            }

            long number1 = 0;
            bool canConvert = long.TryParse(cID, out number1);

            if (!canConvert)
            {
                return "!NULL";
            }

            try
            {
                string rtnVal = String.Empty;

                string sqlQuery = XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_MDM_ListApp");
                sqlQuery = String.Format(sqlQuery, cID, lID, uuID);

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "dsEnv", null);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {                        
                        rtnVal = dr["ActiveMobile"].ToString() + "@" + dr["LoginType"].ToString() + "@" + dr["AccessRight"].ToString();
                    }
                }

                return rtnVal;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetEnv()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : regID 저장
        /// 작성일자 : 2015-11-20
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_uuID"></param>
        /// <param name="_regID"></param>
        /// <returns></returns>
        private bool UpdRegID(int _companyID, string _userID, string _uuID, string _regID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@UUID", SqlDbType.NVarChar,200)                                               
                                               ,new SqlParameter("@RegID", SqlDbType.NVarChar,300)  
                                               
                };

                parms[0].Value = _companyID;
                parms[1].Value = _userID;
                parms[2].Value = _uuID;
                parms[3].Value = _regID;
  
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "UPD_PROC_MDM_ListRegID");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "UpdRegID", parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("UpdRegID(" + _companyID.ToString() + "," + _userID + "," + _uuID + "," + _regID + ")", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 로그인 했다는 처리 (알람받는 거랑 연관)
        /// 작성일자 : 2019-04-12
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_uuID"></param>
        /// <returns></returns>
        private bool UpdLogOutYN(int _companyID, string _userID, string _uuID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)                                               
                                               ,new SqlParameter("@UserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@UUID", SqlDbType.NVarChar,200)
                                               ,new SqlParameter("@LogOutYN", SqlDbType.Char,1)
                };

                parms[0].Value = _companyID;
                parms[1].Value = _userID;
                parms[2].Value = _uuID;
                parms[3].Value = "N";

                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "UPD_PROC_LogOutYN");
                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("UpdLogOutYN(" + _uuID + "," + _userID + ")", ex);
                return false;
            }
        }


        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 회사 로고 이미지 가져오기
        /// 작성일자 : 2013-03-21
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetLogoImage(int _companyId)
        {
            string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");

            //기본 이미지 
            string logoImage = "/DeskPlusMobile/Content/Ko/Base/Img/sm_logo.png";
            string bgLoginImage = "/DeskPlusMobile/Content/Ko/Base/Img/bg.png";

            //회사별 이미지
            string ciLoginImage = baseFileDirectory + "/" + _companyId + "/lgn_logo.png";
            string ciBGLoginImage = baseFileDirectory + "/" + _companyId + "/bg.jpg";

            #region 로고 이미지
            if (System.IO.File.Exists(Server.MapPath(ciLoginImage)))
            {
                logoImage = ciLoginImage;
            }
            #endregion

            #region 배경 이미지
            if (System.IO.File.Exists(Server.MapPath(ciBGLoginImage)))
            {
                bgLoginImage = ciBGLoginImage;
            }
            #endregion

            return logoImage + "^" + bgLoginImage;
        }

        private void SetDecryptString(string p)
        {
            string param = p;
            if (!string.IsNullOrEmpty(param) && param != "!NULL")
            {
                param = Server.UrlDecode(param);//디코딩
                decryString = DecryptString(param);
                decryString = Server.UrlDecode(decryString);//디코딩
                decryString = decryString.Replace(" ", "+");                      
            }
        }

        /// <summary>
        /// 작성목적 : code behind 단에서 복고화 하여 파라미터값 가져오기
        /// </summary>
        /// <param name="_keyName">Name of the _key.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetDecryptParam(string _keyName, string _errorReturn)
        {
            if (!string.IsNullOrEmpty(decryString))
            {
                return GetParmforParameter(decryString, _keyName, _errorReturn);
            }

            return _errorReturn;

        }

        /// <summary>
        /// 작성목적 : 파리미터값으로 키의 값 가져오기
        /// </summary>
        /// <param name="_querystring"></param>
        /// <param name="_parmName"></param>
        /// <param name="_errorReturn"></param>
        /// <returns></returns>
        protected string GetParmforParameter(string _querystring, string _parmName, string _errorReturn)
        {
            if (string.IsNullOrEmpty(_querystring) || _querystring == "!NULL") return _errorReturn;
            string response = String.Empty;

            _querystring = _querystring.Replace("&amp;", "~@~dp~@~");  //ByFreeman 20140516 URL 파라미터에 &값이 있을경우 오류

            _querystring = _querystring.ToReverseXSS();

            string[] pairs = _querystring.Split('&');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string tmpPair = String.Empty;
            foreach (string pair in pairs)
            {
                if (String.IsNullOrEmpty(pair) || pair.ToNull() == "") continue;

                tmpPair = pair.Replace("~@~dp~@~", "&");  //ByFreeman 20140516 URL 파라미터에 &값이 있을경우 오류

                string[] paramvalue = tmpPair.Split('=');

                if (dic.ContainsKey(paramvalue[0]) == false && paramvalue[0].ToNull() != "")
                {
                    if (paramvalue.Length > 2) paramvalue[1] = tmpPair.Substring(tmpPair.IndexOf('=') + 1).Trim();    //특수문자 이후 사라지는 문제 ByFreeman 20131031
                    dic.Add(paramvalue[0], paramvalue[1]);
                }
            }

            if (dic.ContainsKey(_parmName)) response = dic[_parmName];

            return (!string.IsNullOrEmpty(response)) ? response : _errorReturn;

        }


        /// <summary>
        /// 작성목적 : TripleDES 복호화
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public static string DecryptString(string _data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(_data)) return "!NULL";

            try
            {
                string UrlEnc = "N";

                if (UrlEnc == "Y")
                {
                    // 파라미터 암호화에 TripleDES 사용 (보안 강화) 
                    TripleDESManager tds = new TripleDESManager();
                    string data = tds.Decrypt(_data.Trim(), System.Text.Encoding.UTF8);
                    return data.TrimEnd('\0');
                }
                else
                {
                    _data = _data.Replace("~plus~", "+");
                    return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(_data));
                }
            }
            catch
            {
                return "!NULL";
            }
        }

        #region 알람

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-10-02
        /// 작성목적 : 메신저 알람
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_userName"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_uuID"></param>
        /// <returns></returns>
        public bool SendAlarm(int _companyID, int _userID, string _userName, int _receiverID, string _uuID)
        {
            Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo alarmInfo = new Zeospace.DeskPlusMobile.Web.Messenger.BE.AlarmInfo();
            alarmInfo.CompanyID = _companyID;
            alarmInfo.SenderID = _userID;
            alarmInfo.SenderName = _userName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            string[] arrCont = AlarmMessengerTemplate(_uuID, _userName);
            alarmInfo.Subject = arrCont[0];
            alarmInfo.Content = arrCont[1];

            alarmInfo.TargetUrl = "";
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "MDM";
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm alarm = new Zeospace.DeskPlusMobile.Web.Messenger.BLL.Alarm();
            return alarm.Add(alarmInfo);
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2015-10-02
        /// 작성목적 : 알람 내용 템플릿
        /// </summary>
        /// <param name="_uuID"></param>
        /// <param name="_userName"></param>
        /// <returns></returns>
        private string[] AlarmMessengerTemplate(string _uuID, string _userName)
        {
            string contHtml = "";

            try
            {
                XmlDocument confirmXml = null;
                string xmlPath = Server.MapPath("/DeskPlusMobile/XmlData/MDM/ko.xml");
                confirmXml = new XmlDocument();
                confirmXml.Load(xmlPath);
                XmlNode tmpXmlNode = confirmXml.DocumentElement;

                contHtml = System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ContentInfo/ContentHTML").InnerText);
                contHtml = contHtml.Replace("$UUID$", _uuID);              //고유키값
                contHtml = contHtml.Replace("$NAME$", _userName);          //요청자

                string[] arrRtnval = new string[2];

                arrRtnval[0] = System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ContentInfo/SUBJ").InnerText);
                arrRtnval[1] = contHtml;

                return arrRtnval;   // [0] : Subj / [1] : Content

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("AlarmMessengerTemplate()", ex);
                return null;
            }
        }

        #endregion 

        #region 버전
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 버전 정보 가져오기
        /// 작성일자 : 2016-03-29
        /// </summary>
        /// <returns>페이지 정보</returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetVersion(string _type)
        {
            string versionFile;

            if (_type.ToUpper() == "IOS") // IOS
            {
                versionFile = "/DeskPlusMobile/Update/IOS/version.xml";
            }
            else // Android
            {
                versionFile = "/DeskPlusMobile/Update/Android/version.xml";
            }

            string filePath = HttpContext.Current.Server.MapPath(versionFile);
            DataSet ds = new DataSet();
            ds.ReadXml(filePath);

            DataTable dt = ds.Tables[0];
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (DataRow dr in dt.Rows)
            {
                //object[] arr = new object[dt.Rows.Count + 1];

                //for (int i = 0; i <= dt.Rows.Count - 1; i++)
                //{
                //    arr[i] = dt.Rows[i].ItemArray;
                //}

                foreach (DataColumn col in dt.Columns)
                {
                    dict.Add(col.ColumnName, dr[col]);
                }
            }

            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(dict);
        }
        #endregion
    }
}
