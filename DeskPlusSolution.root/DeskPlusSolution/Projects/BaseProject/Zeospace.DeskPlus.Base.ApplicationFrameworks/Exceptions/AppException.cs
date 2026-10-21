using System;
using System.Text;
using System.IO;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Configuration;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions
{
    /// <summary>
    /// C : Exception 처리하는 클래스
    /// </summary>
    public class AppException : System.Exception
    {
        #region 변수선언
        private const string LOGNAME = "DeskPlusLog";
        private const string SOURCENAME = "DeskPlus WebApp";
        #endregion

        #region 생성자
        static AppException()
        {
        }

        #endregion

        #region Exception
        /// <summary>
        /// M : Exception 쓰기
        /// </summary>
        /// <param name="_message">에러메시지</param>
        public static void WriteException(string _message)
        {
            WriteExceptionLog(_message);
        }


        /// <summary>
        /// M : Exception 쓰기
        /// </summary>
        /// <param name="_message">에러메시지</param>
        /// <param name="_innerException">Exception 객체</param>
        public static void WriteException(string _message, Exception _innerException)
        {
            WriteExceptionLog(_message);

            if (_innerException != null)
            {
                WriteExceptionLog(_innerException.Message);
            }
        }
        #endregion

        #region Assert

        /// <summary>
        /// Assert 로그 메시지 메소드
        /// </summary>
        /// <param name="_message"></param>
        /// <param name="_type"></param>
        public static void WriteAssert(string _message, AppSettingType _type)
        {
            string assertView = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "AssertView", "N");
            string useAssert = "N";

            try
            {
                useAssert = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "AssertView_" + AppSettingHelper.GetDescription(_type), "");
            }
            catch
            {
                useAssert = "N";
            }

            if (assertView == "Y" || useAssert == "Y")
            {
                string assertFileLocation = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "AssertLog", "/DeskPlusEIP/Data/Logs/Assert");

                string message = "[" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "] : " + _message;
                if (assertFileLocation == null || assertFileLocation == "")
                {
                    LogEvent(message);
                }
                else
                {
                    LogFile(message, assertFileLocation, "assert");
                }
            }
        }

        #endregion

        #region Mail
        /// <summary>
        /// Mail 로그 메시지 메소드
        /// </summary>
        /// <param name="_message">메시지</param>
        public static void WriteMail(string _message)
        {
            try
            {
                string mailView = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "AssertMail", "N");

                if (mailView == "Y")
                {
                    string mailFileLocation = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "MailLog", "/DeskPlusEIP/Data/Logs/Mail");

                    string message = "[" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "] : " + _message;
                    if (mailFileLocation == null || mailFileLocation == "")
                    {
                        LogEvent(message);
                    }
                    else
                    {
                        LogFile(message, mailFileLocation, "assert");
                    }
                }
            }
            catch
            {
                WriteException(_message);
            }
        }

        /// <summary>
        /// M : Exception 쓰기
        /// </summary>
        /// <param name="_message">에러메시지</param>
        /// <param name="_innerException">Exception 객체</param>
        public static void WriteMail(string _message, Exception _innerException)
        {
            WriteMail(_message);

            if (_innerException != null)
            {
                WriteMail(_innerException.Message);
            }
        }
        #endregion

        #region Log
        /// <summary>
        /// M : 메시지를 쓸때 web.config에 
        /// * 로그파일 위치가 있으면 파일에 쓰고 아니면 
        /// * EventLog 에 에러메시지를 쓴다
        /// </summary>
        /// <param name="_message">메시지</param>
        private static void WriteExceptionLog(string _message)
        {
            string logFileLocation = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "ExceptionLog", "/DeskPlusEIP/Data/Logs/Exception");

            if (logFileLocation == null || logFileLocation == "")
            {
                LogEvent(_message);
            }
            else
            {
                LogFile(_message, logFileLocation, "exception");
            }

        }


        /// <summary>
        /// SSO 로그 메시지 메소드
        /// </summary>
        /// <param name="_message">_message.</param>
        private static void WriteSSO(string _message)
        {
            string isView = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "SSOLogView", "N");

            if (isView == "Y")
            {
                string fileLocation = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "SSOLog", "/DeskPlusEIP/Data/Logs/SSO");

                string message = "[" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "] : " + _message;
                if (fileLocation == null || fileLocation == "")
                {
                    LogEvent(message);
                }
                else
                {
                    LogFile(message, fileLocation, "assert");
                }
            }
        }


        #endregion

        #region LogWrite [Event/File]
        /// <summary>
        /// M : 이벤트 뷰어에 로그/에러/경고 메시지를 쓴다
        /// </summary>
        /// <param name="_message">메시지</param>
        private static void LogEvent(string _message)
        {
            if (!EventLog.SourceExists(SOURCENAME))
            {
                try
                {
                    EventLog.CreateEventSource(SOURCENAME, LOGNAME);
                }
                catch (System.Security.SecurityException ex)
                {
                    LogFile(ex.ToString(), "/DeskPlusEIP/Data/Logs", "exception");
                    return;
                }
            }
            EventLog.WriteEntry(SOURCENAME, _message, EventLogEntryType.Error);
        }

        /// <summary>
        /// 파일에 로그/에러/경고를 쓴다
        /// </summary>
        /// <param name="_message">메시지</param>
        /// <param name="_fileLocation">로그파일 위치</param>
        /// <param name="_gubun">The _gubun.</param>
        public static void LogFile(string _message, string _fileLocation, string _gubun)
        {
            if (_fileLocation.Length == 0) return;

            if (HttpContext.Current == null)
            {
                LogEvent(_message);
                return;
            }

            HttpContext context = HttpContext.Current;
            _fileLocation = context.Server.MapPath(_fileLocation);

            if (!Directory.Exists(_fileLocation)) Directory.CreateDirectory(_fileLocation);

            if (Directory.Exists(_fileLocation))
            {
                string nowYear = DateTime.Now.Year.ToString();
                string nowMon = DateTime.Now.ToString("MM");
                string nowDay = DateTime.Now.ToString("dd");
                string nowHour = DateTime.Now.ToString("HH");

                StreamWriter stream = null;
                try
                {
                    string prefix = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "LogPrefix", "");
                    stream = new StreamWriter(_fileLocation + "\\" + prefix + nowYear + nowMon + nowDay + nowHour + ".log", true, Encoding.Default);

                    if (_gubun == "exception")
                    {

                        stream.WriteLine("▶▶------------- EXCEPTION LIST --------------");
                        stream.WriteLine("▶▶ Time : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        stream.WriteLine("▶▶ Server Name : " + context.Server.MachineName);
                        if (HttpContext.Current.User != null)
                            stream.WriteLine("▶▶ UserID : " + HttpContext.Current.User.Identity.Name);
                        else stream.WriteLine("▶▶ UserID : ");
                        stream.WriteLine("▶▶ Client Address : " + context.Request.UserHostAddress);//접속자 ip 주소 가져오기
                        stream.WriteLine("▶▶ Client Name : " + context.Request.UserHostName);
                        stream.WriteLine("▶▶ Url : " + context.Request.Path);

                        if (context.Server.GetLastError() != null) //ByFreeman 20111122 null 체크 필요
                        {
                            stream.WriteLine("▶▶ Error Message : \n" + context.Server.GetLastError().Message.ToString());
                            stream.WriteLine("▶▶ Stack Trace : " + context.Server.GetLastError().ToString());
                        }

                        stream.WriteLine("▶▶ MESSAGE : " + _message);
                    }
                    else if (_gubun == "assert")
                    {
                        stream.WriteLine("▶▶------------- ASSERT LIST --------------");
                        stream.WriteLine("▶▶ Time : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        stream.WriteLine("▶▶ MESSAGE : " + _message);
                    }

                    #region 메일 발송자 메일주소
                    string senderMailError = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "SenderMailError", "");

                    if (senderMailError != "")
                    {
                        //메일 제목
                        string sendMailTitle = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "SendMailErrorTitle", "");
                        sendMailTitle = string.Format(sendMailTitle, DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm") + ":" + DateTime.Now.ToString("ss"), HttpContext.Current.User.Identity.Name);

                        MailDataInfo mailInfo = null;
                        SendMail sendMail = new SendMail();

                        string[] _to = senderMailError.Split(',');

                        for (int i = 0; i < _to.Length; i++)
                        {
                            mailInfo = new MailDataInfo();
                            mailInfo.To.Add(_to[i], _to[i]);
                            mailInfo.From = "deskplus@zeospace.com";
                            mailInfo.Subject = sendMailTitle;
                            mailInfo.Contents = _message;

                            sendMail.Send(mailInfo);
                        }
                    }
                    #endregion
                }
                catch { }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }
        }


        /// <summary>
        /// SQLs the exception log.(written by 심재운)
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string SqlExceptionLog(SqlException ex)
        {
            if (ex != null)
            {
                string errorMessage = "";


                // written by 홍석기
                foreach (SqlError err in ex.Errors)
                {
                    errorMessage = errorMessage +
                        "Message: " + err.Message + "\n" +
                        "Source: " + err.Source + "\n" +
                        "State: " + err.State + "\n" +
                        "Procedure: " + err.Procedure + "\n";
                }

                // string errorMessage = "Message: " + ex.Message + "\n" +
                //	"Source: " + ex.Source + "\n" +
                //	"State: " + ex.State + "\n" +
                //	"Procedure: " + ex.Procedure + "\n";

                return errorMessage;
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// OracleSQLs the exception log.(written by 심재운)
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string OracleExceptionLog(OracleException ex)
        {
            if (ex != null)
            {
                string errorMessage = String.Empty;

                errorMessage = errorMessage +
                    "Code: " + ex.Code + "\n" +
                    "ErroCode: " + ex.ErrorCode + "\n" +
                    "HelpLink: " + ex.HelpLink + "\n" +
                    "Source: " + ex.Source + "\n" +
                    "Message: " + ex.Message + "\n";

                return errorMessage;
            }
            else
            {
                return "";
            }

        }


        /// <summary>
        /// 방문자 ie 정보 written by shim
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        public static string VisitorLog(HttpContext _context)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Your URL : ");
            sb.Append(_context.Request.Url.ToString());
            sb.Append(",");
            sb.Append("Your IP Address:");
            sb.Append(_context.Request.UserHostAddress);
            sb.Append(",");
            sb.Append("Your Hostname:");
            sb.Append(_context.Request.UserHostName);
            sb.Append(",");
            sb.Append("Browser/Computer Properties :");
            sb.Append(_context.Request.Browser.Browser);
            sb.Append(",");
            sb.Append("Cookies Enabled : ");
            sb.Append((_context.Request.Browser.Cookies) ? "Enable" : "False");
            sb.Append(",");
            sb.Append("Time and Date : ");
            sb.Append(System.DateTime.Now);
            sb.Append(",");
            sb.Append("Platform : ");
            sb.Append(_context.Request.Browser.Platform);
            sb.Append(",");
            sb.Append("Type : ");
            sb.Append(_context.Request.Browser.Type);
            sb.Append(",");
            sb.Append("Version : ");
            sb.Append(_context.Request.Browser.Version);
            sb.Append(",");
            sb.Append(".net Runtime Version : ");
            sb.Append(_context.Request.Browser.ClrVersion.ToString());

            return sb.ToString();
        }
        #endregion
    }
}
