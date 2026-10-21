using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Base.Web.Ws.Site.Auth
{
    /// <summary>
    /// Default의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Default : System.Web.Services.WebService
    {

        /// <summary>
        /// 패스워드 변경일자 체크 하기
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <param name="loginID">The login ID.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string CheckPasswordDate(int companyID, string loginID, string password)
        {
            int userID = 0;
            string OTPYN = "N";
            string response = "0"; //0 : false; 1: true
            DateTime dt = DateTime.MinValue;

            Account.DAL.User biz = new Account.DAL.User();
            DataTableReader dtr = biz.GetPasswordUpdateDate(companyID, loginID, password);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        userID = (dtr["userID"] == DBNull.Value) ? 0 : Convert.ToInt32(dtr["userID"]);
                        dt = (dtr["PasswordUpdateDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(dtr["PasswordUpdateDate"]);
                        OTPYN = dtr["OTPYN"].ToString();
                    }
                }
            }

            if (userID == 0) return "-1"; //해당 사용자 없음

            //제한할 패스워드 일자 제한일자
            string passwordChangeByDay = AppSettingHelper.GetSysCode(companyID, "PwdModDate", "PwdRegex", "ko");
            
            //0 값은 패스워드 변경을 하지 않음
            if (Convert.ToInt32(passwordChangeByDay) == 0)
            {
                response = "1"; //성공
            }
            else
            {
                if (dt != DateTime.MinValue)
                {
                    DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    TimeSpan gap = todayDate.Subtract(dt);

                    //날짜가 이전 날짜 값일 경우
                    if (gap.Days >= Convert.ToInt32(passwordChangeByDay))
                    {
                    }
                    else
                    {
                        response = "1"; //성공
                    }
                }
            }

            return response + "^" + OTPYN; 
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 틀린 패스워드 카운트 저장하기
        /// </summary>
        /// <param name="companyID">The company ID.</param>
        /// <param name="loginID">The login ID.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string PasswordError(int companyID, string loginID, string password)
        {
            Account.DAL.User dal = new Account.DAL.User();
            int userID = dal.SignIn(companyID, loginID, password, false);
            int faultCount = 0;

            //패스워드 틀릴 경우
            if (userID == -1)
            {
                //틀린 패스워드 카운트 여부
                string maxPwdCount = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.ACCOUNT, "PasswrodFaultCount", "0");
                if (maxPwdCount.ToNumber<int>() > 0)
                {
                    faultCount = dal.ModPwdCount(companyID, loginID, maxPwdCount.ToNumber<int>(), false);
                }

                HttpContext context = HttpContext.Current;
                string[] remoteAddrStr = context.Request.UserHostAddress.Split('.');
                string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

                Zeospace.DeskPlus.Common.DAL.PwdErrorLog dalPwd = new Common.DAL.PwdErrorLog();
                Zeospace.DeskPlus.Common.BE.PwdErrorLogInfo infoPwd = new Common.BE.PwdErrorLogInfo();
                infoPwd.CompanyID = companyID;
                infoPwd.LogInID = loginID;
                infoPwd.IPAddress = remoteAddr;
                dalPwd.Add(infoPwd);
            }
            else if (userID == -2) //부서 비활성화 경우
            {
                faultCount = -2;
            }
            else
            {
                string maxPwdCount = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.ACCOUNT, "PasswrodFaultCount", "0");
                int nowCount = 0;

                if (maxPwdCount.ToNumber<int>() > 0)
                {
                    DataTableReader dtr = dal.GetUserByLogin(companyID, loginID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                nowCount = dtr["PwdCount"].ToString().ToNumber<int>();
                            }
                        }
                    }

                    if (nowCount >= maxPwdCount.ToNumber<int>())
                    {
                        faultCount = -3; //패스워드 틀림 횟수 설정과 현재 틀린 숫자가 같으면 -3 리턴
                    }
                    else 
                    {
                        //틀린 패스워드 카운트 0 초기화 해주기
                        dal.ModPwdCount(companyID, loginID, 0, true);
                    }
                }
            }


            return faultCount.ToString();
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AjaxGetEncrypt(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                string UrlEnc = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "EncryptURLParameter", "N");

                if (UrlEnc == "Y")
                {
                    data = Server.UrlDecode(data);
                    TripleDESManager tds = new TripleDESManager();
                    return tds.Encrypt(data.Trim(), System.Text.Encoding.UTF8);
                }
                else
                {
                    data = Server.UrlDecode(data);
                    data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
                    return data.Replace("+", "~plus~");
                }
            }
            catch
            {
                return "!NULL";
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 로고 이미지 가져오기
        /// 작성자     : freeman
        /// 최초작성일 : 2012-03-20
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetLogoImage(int companyID)
        {
            string LoginImageCnt = "";
            string VLoginImageCnt = "";
            string BGloginImgCnt = "";
            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = new List<DeskPlus.Common.BE.CompanyInfo>();
            Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
            al = dal.GetCompany(companyID, false);

            if (al.Count > 0)
            {
                foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo info in al)
                {
                    LoginImageCnt = info.Lgn_logo;                   
                    VLoginImageCnt = info.Vlgn_logo;                   
                    BGloginImgCnt = info.Bg;
                }
            }

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
            string baseImagePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImgPath", "/DeskPlusEIP/Design/C/Img");
           
            //기본 이미지 
            string logoImage = baseImagePath + "/Login/lgn_logo.png";
            string ciMenuImage = baseFileDirectory + companyID + "/lgn_logo.png";

            if (System.IO.File.Exists(Server.MapPath(ciMenuImage)))
            {
                logoImage = ciMenuImage + "?v=" + LoginImageCnt;
            }

            //세로기본 이미지 
            string VLoginImage = baseImagePath + "/Login/Vlgn_logo.png";
            string ciVLoginImage = baseFileDirectory + companyID + "/Vlgn_logo.png";

            if (System.IO.File.Exists(Server.MapPath(ciVLoginImage)))
            {
                VLoginImage = ciVLoginImage + "?v=" + VLoginImageCnt;
            }

            //배경기본 이미지 
            string BGLoginImage = baseImagePath + "/DStyle/bg1.jpg";
            string ciBGLoginImage = baseFileDirectory + companyID + "/bg.jpg";

            if (System.IO.File.Exists(Server.MapPath(ciBGLoginImage)))
            {
                BGLoginImage = ciBGLoginImage + "?v=" + BGloginImgCnt;
            }

            return logoImage + "^" + VLoginImage + "^" + BGLoginImage;
        }

                /// <summary>
        /// 메서드명   :
        /// 작성목적   : 패스워드 메일발송
        /// 작성자     : maniaysw
        /// 최초작성일 : 2017-02-13
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SendPassword(int companyID, string loginID, string langType)
        {
            try
            {
                
                Zeospace.DeskPlus.Account.DAL.PasswordManager dal = new Account.DAL.PasswordManager();
                DataTableReader dtr = dal.GetManager(companyID); ///회사의 발송자가 누군지 가져옴                
                int managerUserID = 0; //발송자 UserID     
                string managerName = "";                     
                string managerEmail = ""; ///발송자 Email(고객담당자)
                                          ///
                
                int userID = 0; //고객 UserID
                string userName = ""; //고객이름 NameBase
                string authEmail = ""; //고객 개인 인증 Email

                string ranPassword = RandomPassword();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {                            
                            managerUserID = dtr["UserID"].ToString().ToNumber<int>();
                            managerName = dtr["NameBase"].ToString();
                            managerEmail = dtr["Mail"].ToString();
                        }
                    }
                }

                Zeospace.DeskPlus.Account.DAL.User userdal = new Account.DAL.User();
                DataTableReader dtr2 = userdal.GetAuthEmail(companyID, loginID); //사용자의 2차 메일계정을 가져옴

                if (dtr2 != null)
                {
                    if (dtr2.HasRows)
                    {
                        while (dtr2.Read())
                        {                            
                            userID = dtr2["UserID"].ToString().ToNumber<int>();
                            userName = dtr2["NameBase"].ToString();
                            authEmail = dtr2["AuthEmail"].ToString();
                        }
                    }
                }                

                MailDataInfo mailInfo = new MailDataInfo();
                SendMail biz = new SendMail();
                mailInfo.From = managerEmail;
                mailInfo.To.Add(authEmail,authEmail);
                if (langType.IndexOf("ko") > -1) //브라우저가 한국어 사용자면
                {
                    mailInfo.Subject = "패스워드 초기화 메일입니다.";
                    mailInfo.Contents = @"요청하신 임시 패스워드: " + ranPassword + "입니다.";
                }
                else //한국어 이외 모든 사용자는 제목, 본문 영문으로 
                {
                    mailInfo.Subject = "This is the password initialization mail.";
                    mailInfo.Contents = @"The temporary password you requested:" + ranPassword;
                }

                Regex regex = new Regex(@"^[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$"); //메일 정규식

                Match match = regex.Match(authEmail);
                if (!match.Success)
                {
                    return "NoPattern";
                }

                var saveFullFileName = biz.SaveEml(mailInfo, "", false, "");
                
                userdal.ModSendPwd(companyID, userID, ranPassword, "");

                AddHistory(companyID, managerUserID, managerName, managerEmail, userID, userName, authEmail);

                return "1";
            }
            catch (Exception ex)
            {
                return "!NULL";
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 입력한 정보가 맞는지 체크 후 패스워드 메일발송
        /// 작성자     : maniaysw
        /// 최초작성일 : 2017-02-15
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string CheckSendPassword(int companyID, string loginID, string email, string langType)
        {
            try
            {
                Zeospace.DeskPlus.Account.DAL.PasswordManager dal = new Account.DAL.PasswordManager();
                DataTableReader dtr = dal.GetManager(companyID); ///회사의 발송자가 누군지 가져옴                
                int managerUserID = 0; //발송자 UserID
                string managerName = "";
                string managerEmail = ""; ///발송자 Email(고객담당자)

                int userID = 0; //고객 UserID
                string userName = ""; //고객이름 NameBase
                string authEmail = ""; //고객 개인 인증 Email

                string ranPassword = RandomPassword();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            managerUserID = dtr["UserID"].ToString().ToNumber<int>();
                            managerName = dtr["NameBase"].ToString();
                            managerEmail = dtr["Mail"].ToString();
                        }
                    }
                }
                if (managerEmail == "") //관리자 메일주소가 없으면 발송안되야 함
                {
                    return "-1";
                }

                Zeospace.DeskPlus.Account.DAL.User userdal = new Account.DAL.User();
                DataTableReader dtr2 = userdal.GetAuthEmail(companyID, loginID); //사용자의 2차 메일계정을 가져옴

                if (dtr2 != null)
                {
                    if (dtr2.HasRows)
                    {
                        while (dtr2.Read())
                        {
                            userID = dtr2["UserID"].ToString().ToNumber<int>();
                            userName = dtr2["NameBase"].ToString();
                            authEmail = dtr2["AuthEmail"].ToString();
                        }
                    }
                }

                Regex regex = new Regex(@"^[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$"); //메일 정규식

                Match match = regex.Match(authEmail);
                if (!match.Success)
                {
                    return "NoPattern";
                }

                if (authEmail != email) //넘어온 이메일값이 나의정보 이메일값과 다르다면 발송 안되야 함
                {
                    return "0";
                }
                else
                {
                    MailDataInfo mailInfo = new MailDataInfo();
                    SendMail biz = new SendMail();
                    mailInfo.From = managerEmail;
                    mailInfo.To.Add(authEmail, authEmail);
                    if (langType.IndexOf("ko") > -1) //브라우저가 한국어 사용자면
                    {
                        mailInfo.Subject = "패스워드 초기화 메일입니다.";
                        mailInfo.Contents = @"요청하신 임시 패스워드: " + ranPassword + "입니다.";
                    }
                    else //한국어 이외 모든 사용자는 제목, 본문 영문으로 
                    {
                        mailInfo.Subject = "This is the password initialization mail.";
                        mailInfo.Contents = @"The temporary password you requested:" + ranPassword;
                    }

                    var saveFullFileName = biz.SaveEml(mailInfo, "", false, "");

                    userdal.ModSendPwd(companyID, userID, ranPassword, "");

                    AddHistory(companyID, managerUserID, managerName, managerEmail, userID, userName, authEmail);

                    return "1";
                }

            }
            catch (Exception ex)
            {
                return "!NULL";
            }
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 발송자, 수신자 외부메일 있는지 체크로직
        /// 작성자     : maniaysw
        /// 최초작성일 : 2017-02-15
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string CheckEmail(int companyID, string loginID)
        {
            try
            {
                string authEmail = "";
                string managerEmail = "";

                //로그인 시도한 사용자 2차 메일 아이디 가져오기
                Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
                DataTableReader dtr = dal.GetAuthEmail(companyID, loginID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            authEmail = dtr["AuthEmail"].ToString();
                        }
                    }
                }

                if (authEmail == "")
                {
                    return "-2"; //2차 메일없음
                }

                //발송 관리자 이메일 가져옴
                Zeospace.DeskPlus.Account.DAL.PasswordManager managerdal = new Account.DAL.PasswordManager();
                DataTableReader dtr2 = managerdal.GetManager(companyID);
                if (dtr2 != null)
                {
                    if (dtr2.HasRows)
                    {
                        while (dtr2.Read())
                        {
                            managerEmail = dtr2["mail"].ToString();
                        }
                    }
                }
                if (managerEmail == "")
                {
                    return "-1"; //발송자 메일없음
                }

                return "1"; //둘다있으면 1
            }
            catch (Exception ex)
            {
                return "!NULL";
            }
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 로그인ID로 2차메일 찾기
        /// 작성자     : maniaysw
        /// 최초작성일 : 2017-02-15
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SearchEmail(int companyID, string loginID)
        {

            try
            {
                string authEmail = "";
                
                //로그인 시도한 사용자 2차 메일 아이디 가져오기
                Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
                DataTableReader dtr = dal.GetAuthEmail(companyID, loginID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            authEmail = dtr["AuthEmail"].ToString();
                        }
                    }
                }

                if (authEmail == "")
                {
                    return "-2"; //2차 메일없음
                }

                return authEmail; //둘다있으면 1
            }
            catch (Exception ex)
            {
                return "!NULL";
            }

        }

        /// <summary>
        /// 패스워드 히스토리 추가
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="managerUserID"></param>
        /// <param name="managerName"></param>
        /// <param name="managerEmail"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="authEmail"></param>
        private void AddHistory(int _companyID, int _managerUserID, string _managerName, string _managerEmail, int _userID, string _userName, string _authEmail)
        {
            Zeospace.DeskPlus.Account.DAL.PasswordHistory dal = new Account.DAL.PasswordHistory();
            dal.Add(_companyID, _managerUserID, _managerName, _managerEmail, _userID, _userName, _authEmail);

        }

        private string RandomPassword()
        {
            string allowedChars = "";
            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowedChars += "1,2,3,4,5,6,7,8,9,0";
            char[] sep = { ',' };

            string[] arr = allowedChars.Split(sep);

            string passwordString = "";

            string temp = "";

            Random rand = new Random();

            for (int i = 0; i < Convert.ToInt32(8); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }

            return passwordString;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="loginID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetOTP(int companyID, string loginID, string password)
        {
            Account.DAL.User dal = new Account.DAL.User();
            return dal.GetOTP(companyID, loginID, password, false);
        }
    }
}
