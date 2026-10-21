using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;

namespace Zeospace.DeskPlusMobile.Web.HR.Ws
{
    /// <summary>
    /// SetAttendance의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetAttendance : Helper.MobileBaseService
    {
        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 출근
        /// 작성일자 : 2018-12-06
        /// </summary>
        /// <param name="_shortreplyInfo"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetGotoWork(string p)
        {
            SetDecryptString(p);

            string reason = GetDecryptParam("reason", "").ToString();
            string isHoli = GetDecryptParam("isHoli", "").ToString();
            string bLate = GetDecryptParam("bLate", "0").ToString();

            try
            {
                int bLateMinute = 0;
                if (bLate == "1")
                {
                    // 오늘 정규 출근시간 가져오기
                    DateTime BaseWSTime = new DateTime();

                    Zeospace.DeskPlusMobile.Web.HR.Ws.Data.HR dal = new Ws.Data.HR();
                    DataTableReader dtrT = dal.GetAttendanceTime(CompanyID, UserID, DateTime.Now.ToShortDateString(), isHoli);
                    if (dtrT != null)
                    {
                        if (dtrT.HasRows)
                        {
                            while (dtrT.Read())
                            {
                                if (dtrT["BaseWSTime"].ToString() != "") //출근시간(출근인정범위 미포함)
                                {
                                    BaseWSTime = Convert.ToDateTime(dtrT["BaseWSTime"]);
                                }
                            }
                        }
                    }

                    TimeSpan ts = (DateTime.Now - BaseWSTime);
                    bLateMinute = (ts.Hours * 60) + ts.Minutes;
                }

                string conn = GetIPConn();

                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int) 
                                               ,new SqlParameter("@DeptID", SqlDbType.Int)
                                               ,new SqlParameter("@Reason", SqlDbType.NVarChar,400)
                                               ,new SqlParameter("@WSConn", SqlDbType.VarChar,50)  
                                               ,new SqlParameter("@IsHoli", SqlDbType.Char,1)
                                               ,new SqlParameter("@WSTime", SqlDbType.DateTime)
                                               ,new SqlParameter("@bLate", SqlDbType.Char,1)
                                               ,new SqlParameter("@bLateMinute", SqlDbType.Int)
                };

                parms[0].Value = CompanyID;
                parms[1].Value = UserID;
                parms[2].Value = UserInfo.DeptID;
                parms[3].Value = reason;
                parms[4].Value = conn;
                parms[5].Value = isHoli;
                parms[6].Value = DateTime.Now;
                parms[7].Value = bLate;
                parms[8].Value = bLateMinute;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLHR, "INS_PROC_GOTOWORK");

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms);

                string hour = DateTime.Now.Hour.ToString();
                string minute = DateTime.Now.Minute.ToString();

                if (DateTime.Now.Hour < 10)
                {
                    hour = "0" + DateTime.Now.Hour.ToString();
                }

                if (DateTime.Now.Minute < 10)
                {
                    minute = "0" + DateTime.Now.Minute.ToString();
                }

                return conn + "^" + hour + ":" + minute;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("SetGotoWork", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 퇴근
        /// 작성일자 : 2018-12-06
        /// </summary>
        /// <param name="_shortreplyInfo"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetOffWork(string p)
        {
            SetDecryptString(p);

            string reason = GetDecryptParam("reason", "").ToString();
            string isHoli = GetDecryptParam("isHoli", "").ToString();
            string ddlEarly = GetDecryptParam("ddlEarly", "").ToString();
            string bEarly = GetDecryptParam("bEarly", "0").ToString();

            string conn = GetIPConn();

            DateTime WCTime;
            int bEarlyMinute = 0;
            if (bEarly == "0")
            {
                WCTime = DateTime.Now;
            }
            else
            {
                // 조퇴
                WCTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, (Convert.ToDateTime(ddlEarly)).Hour, (Convert.ToDateTime(ddlEarly)).Minute, 0);

                // 오늘 정규 퇴근시간 가져오기
                DateTime bWCTime = new DateTime();

                Zeospace.DeskPlusMobile.Web.HR.Ws.Data.HR dal = new Ws.Data.HR();
                DataTableReader dtrT = dal.GetAttendanceTime(CompanyID, UserID, DateTime.Now.ToShortDateString(), isHoli);
                if (dtrT != null)
                {
                    if (dtrT.HasRows)
                    {
                        while (dtrT.Read())
                        {
                            if (dtrT["bWCTime"].ToString() != "") //퇴근시간
                            {
                                bWCTime = Convert.ToDateTime(dtrT["bWCTime"]);
                            }
                        }
                    }
                }

                TimeSpan ts = (bWCTime - WCTime);
                bEarlyMinute = (ts.Hours * 60) + ts.Minutes;
            }

            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)  
                                               ,new SqlParameter("@DeptID", SqlDbType.Int)
                                               ,new SqlParameter("@Reason", SqlDbType.NVarChar,400)
                                               ,new SqlParameter("@WCConn", SqlDbType.VarChar,50)  
                                               ,new SqlParameter("@IsHoli", SqlDbType.Char,1)
                                               ,new SqlParameter("@WCTime", SqlDbType.DateTime)
                                               ,new SqlParameter("@bEarly", SqlDbType.Char,1)
                                               ,new SqlParameter("@bEarlyMinute", SqlDbType.Int)
                };

                parms[0].Value = CompanyID;
                parms[1].Value = UserID;
                parms[2].Value = UserInfo.DeptID;
                parms[3].Value = reason;
                parms[4].Value = conn;
                parms[5].Value = isHoli;
                parms[6].Value = WCTime;
                parms[7].Value = bEarly;
                parms[8].Value = bEarlyMinute;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLHR, "INS_PROC_OFFWORK");

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms);

                string hour = WCTime.Hour.ToString();
                string minute = WCTime.Minute.ToString();

                if (WCTime.Hour < 10)
                {
                    hour = "0" + WCTime.Hour.ToString();
                }

                if (WCTime.Minute < 10)
                {
                    minute = "0" + WCTime.Minute.ToString();
                }

                return conn + "^" + hour + ":" + minute + "^" + bEarly;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("SetOffWork", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 이전일퇴근
        /// 작성일자 : 2018-12-06
        /// </summary>
        /// <param name="_shortreplyInfo"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetPreOffWork(string p)
        {
            SetDecryptString(p);

            string reason = GetDecryptParam("reason", "").ToString();
            string isHoli = GetDecryptParam("isHoli", "").ToString();

            // 날짜를 하루 전으로 세팅
            string selDate = DateTime.Now.AddDays(-1.0).ToString("yyyy-MM-dd");

            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)  
                                               ,new SqlParameter("@DeptID", SqlDbType.Int)
                                               ,new SqlParameter("@Reason", SqlDbType.NVarChar,400)
                                               ,new SqlParameter("@WCConn", SqlDbType.VarChar,50)  
                                               ,new SqlParameter("@IsHoli", SqlDbType.Char,1)
                                               ,new SqlParameter("@PreSelDate", SqlDbType.NVarChar, 10)
                                               ,new SqlParameter("@WCTime", SqlDbType.DateTime)
                };

                string conn = GetIPConn();

                parms[0].Value = CompanyID;
                parms[1].Value = UserID;
                parms[2].Value = UserInfo.DeptID;
                parms[3].Value = reason;
                parms[4].Value = conn;
                parms[5].Value = isHoli;
                parms[6].Value = selDate;
                parms[7].Value = DateTime.Now;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLHR, "INS_PROC_PREOFFWORK");

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms);

                string hour = DateTime.Now.Hour.ToString();
                string minute = DateTime.Now.Minute.ToString();

                if (DateTime.Now.Hour < 10)
                {
                    hour = "0" + DateTime.Now.Hour.ToString();
                }

                if (DateTime.Now.Minute < 10)
                {
                    minute = "0" + DateTime.Now.Minute.ToString();
                }

                return conn + "^" + hour + ":" + minute;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("SetPreOffWork", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성목적 : 퇴근(조퇴) 정보 삭제
        /// 작성일자 : 2018-12-06
        /// </summary>
        /// <param name="_shortreplyInfo"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelOffWork(string p)
        {
            SetDecryptString(p);

            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)  
                                               ,new SqlParameter("@SelDate", SqlDbType.NVarChar, 10)
                };

                parms[0].Value = CompanyID;
                parms[1].Value = UserID;
                parms[2].Value = DateTime.Now.ToString("yyyy-MM-dd");

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLHR, "INS_PROC_OFFWORKDEL");

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("DelOffWork", ex);
                throw;
            }
        }

        /// <summary>
        /// 접속한 IP가 사내, 사외인지 여부
        /// </summary>
        /// <returns></returns>
        public string GetIPConn()
        {
            try
            {
                string WSConn = "OUT";
                double stratIP = 0;
                double endIP = 0;

                HttpContext context = HttpContext.Current;
                string[] remoteAddrStr = context.Request.UserHostAddress.Split('.');
                double ipAddress = GetIPAddress(remoteAddrStr).ToDouble<double>();

                Zeospace.DeskPlusMobile.Web.HR.Ws.Data.HR dal = new Ws.Data.HR();
                DataSet ds = dal.GetIP(CompanyID);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        stratIP = dr["StartIP"].ToString().ToDouble<double>();
                        endIP = dr["EndIP"].ToString().ToDouble<double>();

                        if (stratIP <= ipAddress && endIP >= ipAddress) WSConn = "IN";
                    }
                }

                return WSConn;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("GetIPConn", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성목적 : 아이피 정보를 문자열로 반환
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

    }
}
