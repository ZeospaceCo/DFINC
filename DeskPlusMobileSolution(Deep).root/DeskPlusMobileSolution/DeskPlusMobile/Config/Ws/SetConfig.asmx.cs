using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;


namespace Zeospace.DeskPlusMobile.Web.Config.Ws
{
    /// <summary>
    /// SetConfig의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SetConfig : Helper.MobileBaseService
    {

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 환경설정 정보 저장하기
        /// 작성일자 : 2011-06-20
        /// </summary>
        /// <returns></returns>        
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetEnvironment(string p)
        {
            if (CompanyID <= 0 || UserID <= 0) return "!NULL";

            SetDecryptString(p);
            string pimsMenu = GetDecryptParam("pimsMenu", "0");
            string alarm = GetDecryptParam("alarm", "");
            string mailAlarm = GetDecryptParam("mailAlarm", "N");
            string loginType = GetDecryptParam("loginType", "0");
            string skinType = GetDecryptParam("skinType", Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "DefaultSkinType"));
            string ispwd = GetDecryptParam("ispwd", "N");
            string alarmTimeUse = GetDecryptParam("alarmTimeUse", "0");
            string alarmStart = GetDecryptParam("alarmStart", "09:00");
            string alarmEnd = GetDecryptParam("alarmEnd", "18:00");
            string alarmList = GetDecryptParam("alarmList", "");

            ispwd = (ispwd.Equals("")) ? "N" : ispwd;

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "INS_PROC_USERENVIRONMENT");

            SqlParameter[] parm = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@UserID", SqlDbType.Int),
                                               new SqlParameter("@MobilePims", SqlDbType.VarChar, 100),
                                               new SqlParameter("@MobileSkin", SqlDbType.VarChar, 10),
                                               new SqlParameter("@MobileIsApprovePwd", SqlDbType.Char, 1)
										   };
            parm[0].Value = CompanyID;
            parm[1].Value = UserID;
            parm[2].Value = pimsMenu;
            parm[3].Value = (String.IsNullOrEmpty(UUID)) ? skinType : "";
            parm[4].Value = ispwd;

            string result = Convert.ToString(SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sqlQuery, parm));

            if (String.IsNullOrEmpty(result))
            {
                return "!NULL";
            }
            else
            {
                //알림함 설정 저장
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "INS_PROC_USERENV");

                SqlParameter[] parm1 = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@UserID", SqlDbType.Int),
                                               new SqlParameter("@AlarmMail", SqlDbType.Char, 1),
                                               new SqlParameter("@AlarmApproval", SqlDbType.Char, 1),
                                               new SqlParameter("@AlarmBoard", SqlDbType.Char, 1),
                                               new SqlParameter("@AlarmPims", SqlDbType.Char, 1),
                                               new SqlParameter("@AlarmTask", SqlDbType.Char, 1),
                                               new SqlParameter("@AlarmWork", SqlDbType.Char, 1),
                                               new SqlParameter("@AlarmCommunity", SqlDbType.Char, 1),
                                               new SqlParameter("@AlarmRM", SqlDbType.Char, 1),
                                               new SqlParameter("@AlarmSNS", SqlDbType.Char, 1)
										   };
                parm1[0].Value = CompanyID;
                parm1[1].Value = UserID;
                parm1[2].Value = (alarmList.IndexOf("Mail") > -1) ? "Y" : "N";
                parm1[3].Value = (alarmList.IndexOf("Approval") > -1) ? "Y" : "N";
                parm1[4].Value = (alarmList.IndexOf("Board") > -1) ? "Y" : "N";
                parm1[5].Value = (alarmList.IndexOf("Pim") > -1) ? "Y" : "N";
                parm1[6].Value = (alarmList.IndexOf("TASK") > -1) ? "Y" : "N";
                parm1[7].Value = (alarmList.IndexOf("Report") > -1) ? "Y" : "N";
                parm1[8].Value = (alarmList.IndexOf("Community") > -1) ? "Y" : "N";
                parm1[9].Value = (alarmList.IndexOf("ResourceManager") > -1) ? "Y" : "N";
                parm1[10].Value = (alarmList.IndexOf("SNS") > -1) ? "Y" : "N";

                string result1 = Convert.ToString(SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parm1));

                // 기기별 환경설정 저장
                if (!String.IsNullOrEmpty(UUID))
                {
                    BE.ListInfo info = new BE.ListInfo();
                    info.CompanyID = CompanyID;
                    info.UserID = UserID;
                    info.UUID = UUID;
                    info.MailAlarm = mailAlarm;
                    info.EtcAlarm = alarm;
                    info.LoginType = loginType;
                    info.SkinType = skinType;
                    info.ModifyUserID = UserID;
                    info.ModifyUserName = UserInfo.Name;
                    info.AlarmTimeUse = alarmTimeUse;
                    info.AlarmStart = alarmStart;
                    info.AlarmEnd = alarmEnd;

                    if (!AddAlarm(info))
                    {
                        return "!NULL";
                    }
                }

                string userEnv = pimsMenu;

                Context.Session["DeskPlusMobileUserEnv"] = userEnv; //환경 정보 저장                
                Context.Response.Cookies["DeskPlusMobileUserEnv"].Value = userEnv;
                Context.Response.Cookies["DeskPlusMobileUserEnv"].Path = "/";
                Context.Session["SkinType"] = skinType;
                Context.Session["IsApprovePwd"] = ispwd;

                return "OK";
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 모바일 환경정보 저장
        /// 작성일자 : 2013-02-08
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool AddAlarm(BE.ListInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                                new SqlParameter("@CompanyID", SqlDbType.Int)
                                               ,new SqlParameter("@UserID", SqlDbType.Int)
                                               ,new SqlParameter("@UUID", SqlDbType.NVarChar,200)
                                               ,new SqlParameter("@MailAlarm", SqlDbType.Char,1)
                                               ,new SqlParameter("@EtcAlarm", SqlDbType.VarChar,300)
                                               ,new SqlParameter("@LoginType", SqlDbType.Char,1)
                                               ,new SqlParameter("@SkinType", SqlDbType.VarChar,20)
                                               ,new SqlParameter("@ModifyUserID", SqlDbType.Int)                                                 
                                               ,new SqlParameter("@ModifyUserName", SqlDbType.NVarChar, 50)                                             
                                               ,new SqlParameter("@AlarmTimeUse", SqlDbType.Char, 1)
                                               ,new SqlParameter("@AlarmStart", SqlDbType.NVarChar, 5)
                                               ,new SqlParameter("@AlarmEnd", SqlDbType.NVarChar, 5)
                };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.UserID;
                parms[2].Value = _info.UUID;
                parms[3].Value = _info.MailAlarm;
                parms[4].Value = _info.EtcAlarm;
                parms[5].Value = _info.LoginType;
                parms[6].Value = _info.SkinType;
                parms[7].Value = _info.ModifyUserID;
                parms[8].Value = _info.ModifyUserName;
                parms[9].Value = _info.AlarmTimeUse;
                parms[10].Value = _info.AlarmStart;
                parms[11].Value = _info.AlarmEnd;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLMDM, "UPD_PROC_MDM_List");
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsAlarm", parms);

                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlusMobile.Web.Helper.AppException.WriteException("AddAlarm()", ex);
                return false;
            }
        }
    }
}
