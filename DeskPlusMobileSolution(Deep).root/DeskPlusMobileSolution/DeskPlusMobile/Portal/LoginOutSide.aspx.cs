using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Security;
using System.Reflection;

namespace Zeospace.DeskPlusMobile.Web.Portal
{
    public partial class LoginOutSide : System.Web.UI.Page
    {
        #region Declaration (컨트롤 선언)
        string mailID = String.Empty;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.LoginOutSide).Assembly;	//어셈블리 선언
            #endregion

            if (!Page.IsPostBack)
            {
                DataLoad();
            }
        }
        #endregion

        #region Method (함수)
        /// <summary>
        /// 
        /// </summary>
        private void DataLoad()
        {
            int companyID = this.GetParm("cID", "1").ToNumber<int>();
            string loginID = this.GetParm("uID", "");
            string PWD = this.GetParm("uPW", "");
            string uUID = this.GetParm("uuID", "");
            string mobileTyle = this.GetParm("mTyle", "");
            string targetURL = this.GetParm("tURL", "");
            string ran = this.GetParm("ran", "");

            targetURL = targetURL.Replace("$", "&");

            if (loginID.Length > 0)
            {
                SignInProcess(companyID, loginID, PWD, uUID, mobileTyle, ran);

                if (targetURL != "")
                {
                    Context.Response.Redirect(targetURL);
                }
            }
        }


        /// <summary>
        /// 인증처리
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_loginID"></param>
        /// <param name="_pwd"></param>
        /// <param name="_uUID"></param>
        /// <param name="_mobileTyle"></param>
        /// <param name="_ran"></param>
        private void SignInProcess(int _companyID, string _loginID, string _pwd, string _uUID, string _mobileTyle, string _ran)
        {
            int userID = 0;
            int ActiveMobile = 0;
            int AccessRight = 0;
            string RegID = "";
            string userEnv = "";

            string sqlQuery = XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "SEL_QRY_MDM_ListAppPwd");
            sqlQuery = String.Format(sqlQuery, _companyID, _pwd, _loginID, _uUID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "dsEnv", null);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userID = dr["UserID"].ToString().ToNumber<int>();
                    ActiveMobile = dr["ActiveMobile"].ToString().ToNumber<int>();
                    AccessRight = dr["AccessRight"].ToString().ToNumber<int>();
                    RegID = dr["RegID"].ToString();
                }
            }

            if (userID == 0)
            {
                // 접속명 혹은 비밀번호를 확인해 주십시오.
                if (_mobileTyle == "Android")
                {
                    Context.Response.Redirect("/DeskPlusMobile/Portal/AnLogin.aspx");
                }
                else if (_mobileTyle == "ios")
                {
                    Context.Response.Redirect("/DeskPlusMobile/Portal/IosLogin.aspx");
                }
                else
                {
                    Context.Response.Redirect("/DeskPlusMobile/Portal/Default.aspx");
                }
                return;
            }
            else
            {
                if (AccessRight == 2)
                {
                    if (_mobileTyle == "Android")
                    {
                        Context.Response.Redirect("/DeskPlusMobile/Portal/AnLogin.aspx");
                    }
                    else if (_mobileTyle == "ios")
                    {
                        Context.Response.Redirect("/DeskPlusMobile/Portal/IosLogin.aspx");
                    }
                    else
                    {
                        Context.Response.Redirect("/DeskPlusMobile/Portal/Default.aspx");
                    }
                    return;
                }

                if (ActiveMobile == 1)
                {
                    SetUserInfo(_companyID, _loginID);

                    FormsAuthentication.SetAuthCookie(_loginID, false);

                    Session["UUID"] = _uUID;
                    Context.Response.Cookies["UUID"].Value = _uUID;
                    Context.Response.Cookies["UUID"].Path = "/";

                    Session["MobileType"] = _mobileTyle;
                    Context.Response.Cookies["MobileType"].Value = _mobileTyle;
                    Context.Response.Cookies["MobileType"].Path = "/";

                    Session["RegID"] = RegID;
                    Context.Response.Cookies["RegID"].Value = RegID;
                    Context.Response.Cookies["RegID"].Path = "/";

                    String userArrStr = _loginID + ";";

                    Helper.Security.TripleDESManager tds = new Helper.Security.TripleDESManager();

                    Context.Response.Cookies["DPMAuth"].Value = tds.Encrypt(userArrStr.Trim(), System.Text.Encoding.UTF8);
                    Context.Response.Cookies["DPMAuth"].Path = "/";
                    Context.Response.Cookies["DPMAuth"].Expires = DateTime.Now.AddYears(2);

                    Session["DPMComanyAuth"] = _companyID;
                    Context.Response.Cookies["DPMComanyAuth"].Value = _companyID.ToString();
                    Context.Response.Cookies["DPMComanyAuth"].Path = "/";


                    //메일 기본 Key 저장
                    string useMail = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_USEMAIL");
                    if (useMail.ToUpper() == "TRUE")
                    {
                        sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_WebMailID");
                        sqlQuery = String.Format(sqlQuery, mailID);
                        string WebMailID = Convert.ToString(SQLHelper.ExecuteScalar(SQLHelper.CONN_MAIL, CommandType.Text, sqlQuery, null));
                        Session["WebMailID"] = WebMailID;
                        Context.Response.Cookies["WebMailID"].Value = WebMailID;
                        Context.Response.Cookies["WebMailID"].Path = "/";
                    }
                    //메일 기본 Key 저장

                    //환경 정보 가져오기
                    sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_USERENVIRONMENT");
                    sqlQuery = String.Format(sqlQuery, _companyID, userID);

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


                    ModRead(_companyID, userID, _ran);
                }
                else
                {
                    if (_mobileTyle == "Android")
                    {
                        Context.Response.Redirect("/DeskPlusMobile/Portal/AnLogin.aspx");
                    }
                    else if (_mobileTyle == "ios")
                    {
                        Context.Response.Redirect("/DeskPlusMobile/Portal/IosLogin.aspx");
                    }
                    else
                    {
                        Context.Response.Redirect("/DeskPlusMobile/Portal/Default.aspx");
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 작성목적 : 사용자 정보 저장(Session , Cookie)
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
                        Helper.AppException.WriteException("LoginOutSide[SetUserInfo] 1 : " + ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("LoginOutSide[SetUserInfo] 2 : " + ex);
            }
        }

        /// <summary>
        /// 작성목적 : 사용자 정보 가져오기
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
        /// 조회 추가
        /// </summary>
        private bool ModRead(int _companyID, int _userID, string _ran)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                            new SqlParameter("@CompanyID", SqlDbType.Int),
                                                            new SqlParameter("@UserID", SqlDbType.Int),
                                                            new SqlParameter("@Random", SqlDbType.VarChar,10)
                };

                parms[0].Value = _companyID;
                parms[1].Value = _userID;
                parms[2].Value = _ran;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DPM_MDM_NotificationBox_ReadByRandom", parms);

                return true;

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("ModRead: " + errMsg);
                throw;
            }
        }

        #endregion

        #region 파라미터 처리 (Parm/ViewState)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_parmName"></param>
        /// <param name="_errorReturn"></param>
        /// <returns></returns>
        protected string GetParm(string _parmName, string _errorReturn)
        {
            try
            {
                return ((string)(Request.Params[_parmName] ?? _errorReturn) == "") ? _errorReturn : (string)(Request.Params[_parmName] ?? _errorReturn);
            }
            catch
            {
                return (Request.Params[_parmName] != null) ? Convert.ToString(Request.Params[_parmName]) : Convert.ToString(_errorReturn);
            }
        }
        #endregion
    }
}