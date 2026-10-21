using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Helper
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
            WriteLog(_message,"Exception");
        }

        /// <summary>
        /// Assert 쓰기
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_message"></param>
        public static void WriteAssert(Helper.AppSettingType _type , string _message)
        {
            bool assert = false;

            if (_type == AppSettingType.SQLAddress)
            {
                assert = Convert.ToBoolean(Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "ASSERT_ADDRESS"));
            }
            else if (_type == AppSettingType.SQLApproval)
            {
                assert = Convert.ToBoolean(Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "ASSERT_APPROVAL"));
            }
            else if (_type == AppSettingType.SQLBoard)
            {
                assert = Convert.ToBoolean(Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "ASSERT_BOARD"));
            }
            else if (_type == AppSettingType.SQLMail)
            {
                assert = Convert.ToBoolean(Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "ASSERT_MAIL"));
            }
            else if (_type == AppSettingType.SQLPim)
            {
                assert = Convert.ToBoolean(Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "ASSERT_PIM"));
            }
            else if (_type == AppSettingType.SQLPortal)
            {
                assert = Convert.ToBoolean(Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "ASSERT_PORTAL"));
            }
            else if (_type == AppSettingType.SQLSMS)
            {
                assert = Convert.ToBoolean(Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "ASSERT_SMS"));
            }

            if(assert)  WriteLog(_message,"Assert");
        }


        /// <summary>
        /// M : Exception 쓰기
        /// </summary>
        /// <param name="_message">에러메시지</param>
        /// <param name="_innerException">Exception 객체</param>
        public static void WriteException(string _message, Exception _innerException)
        {
            WriteLog(_message,"Exception");

            if (_innerException != null)
            {
                WriteLog(_innerException.Message,"Exception");
            }
        }
        #endregion



        #region Log
        /// <summary>
        /// /// M : 메시지를 쓸때 web.config에 
        /// * 로그파일 위치가 있으면 파일에 쓰고 아니면 
        /// * EventLog 에 에러메시지를 쓴다
        /// </summary>
        /// <param name="_message"></param>
        /// <param name="_gubun"></param>
        private static void WriteLog(string _message, string _gubun)
        {
            string logFileLocation = "/DeskPlusMobile/Data/Logs/" + _gubun;

            if (logFileLocation == null || logFileLocation == "")
            {
                LogEvent(_message);
            }
            else
            {
                HttpContext context = HttpContext.Current;
                //LogFile(_message, context.Server.MapPath(logFileLocation));

                string prefix = "Mobile";
                LogFile(_message, context.Server.MapPath(logFileLocation), prefix);
            }

        }

        /// <summary>
        /// OMN 로그 메소드
        /// </summary>
        /// <param name="_message">메시지</param>
        /// <param name="_logType">ConfigurationManager.AppSettings[_logType]</param>
        /// <param name="_prefix">구분자</param>
        public static void WriteLog(string _message, string _logType, string _prefix)
        {
            string logFileLocation = "/DeskPlusMobile/Data/Logs/Exception";

            if (logFileLocation == null || logFileLocation == "")
            {
                LogEvent(_message);
            }
            else
            {
                if (logFileLocation.StartsWith("/"))
                {
                    HttpContext context = HttpContext.Current;
                    logFileLocation = context.Server.MapPath(logFileLocation);
                }

                LogFile(_message, logFileLocation, _prefix);
            }
        }

        /// <summary>
        /// SSO 로그 메시지 메소드
        /// </summary>
        /// <param name="_message"></param>
        public static void WriteSSO(string _message)
        {
            string isView = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "SSOLOGVIEW");

            if (isView == "Y")
            {
                string fileLocation = "/DeskPlusMobile/Data/Logs/SSO";

                string message = "[" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "] : " + _message;
                if (fileLocation == null || fileLocation == "")
                {
                    LogEvent(message);
                }
                else
                {
                    string prefix = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "LOGPREFIX");
                    HttpContext context = HttpContext.Current;
                    LogFile(message, context.Server.MapPath(fileLocation), prefix);
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
                    HttpContext context = HttpContext.Current;
                    LogFile(ex.ToString(), context.Server.MapPath("/DeskPlusMobile/Data/Logs/"), "");
                    return;
                }
            }
            EventLog.WriteEntry(SOURCENAME, _message, EventLogEntryType.Error);
        }

        /// <summary>
        /// M : 파일에 로그/에러/경고를 쓴다
        /// </summary>
        /// <param name="_message">메시지</param>
        /// <param name="_fileLocation">로그파일 위치</param>
        /// <param name="_prefix">구분자</param>
        public static void LogFile(string _message, string _fileLocation, string _prefix)
        {
            if (!_fileLocation.EndsWith("\\"))
            {
                _fileLocation += "\\";
            }

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
                    stream = new StreamWriter(_fileLocation + _prefix + nowYear + nowMon + nowDay + nowHour + ".log", true, Encoding.Default);

                    stream.WriteLine("▶▶-------------AppExceptioin--------------");
                    stream.WriteLine("▶▶Time : " + DateTime.Now.ToString("HH") + ":" + DateTime.Now.ToString("mm") + ":" + DateTime.Now.ToString("ss"));
                    stream.WriteLine(_message);
                    stream.WriteLine("▶▶UserID : " + HttpContext.Current.User.Identity.Name);
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
