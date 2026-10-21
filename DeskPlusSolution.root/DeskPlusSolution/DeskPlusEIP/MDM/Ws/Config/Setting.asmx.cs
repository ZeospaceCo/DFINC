using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.MDM.Web.Ws.Config
{
    /// <summary>
    /// Default의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Setting : Base.BaseWSMDM
    {

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-04-25
        /// 작성목적 : 알람설정 변경
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModAlarm(string p)
        {
            try
            {
                SetDecryptString(p);
                string mailAlarm = GetDecryptParam("mailAlarm", "N");
                string etcAlarm = GetDecryptParam("etcAlarm", "");

                BE.ListInfo info = new BE.ListInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.MailAlarm = mailAlarm;
                info.EtcAlarm = etcAlarm;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.ModifyUserName = CurrentUserInfo.Name;

                DAL.List dal = new DAL.List();
                dal.ModAlarm(info);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-04-25
        /// 작성목적 : 로그인 요청 정보 변경
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModLogin(string p)
        {
            try
            {
                SetDecryptString(p);
                string loginType = GetDecryptParam("loginType", "0");
 
                BE.ListInfo info = new BE.ListInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.LoginType = loginType;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.ModifyUserName = CurrentUserInfo.Name;

                DAL.List dal = new DAL.List();
                dal.ModLogin(info);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-04-25
        /// 작성목적 : 모바일 접근 권한 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModAuth(string p)
        {
            try
            {
                SetDecryptString(p);
                string auth = GetDecryptParam("auth", "0");

                BE.ListInfo info = new BE.ListInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.AccessRight = auth;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.ModifyUserName = CurrentUserInfo.Name;

                DAL.List dal = new DAL.List();
                dal.ModAuth(info);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-06
        /// 작성목적 : 환경설정 저장 - 미사용
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Confirm(string p)
        {
            try
            {
                SetDecryptString(p);
                string mailAlarm = GetDecryptParam("mailAlarm", "N");
                string etcAlarm = GetDecryptParam("etcAlarm", "");
                string loginType = GetDecryptParam("loginType", "0");
                string auth = GetDecryptParam("auth", "0");

                BE.ListInfo info = new BE.ListInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.MailAlarm = mailAlarm;
                info.EtcAlarm = etcAlarm;
                info.LoginType = loginType;
                info.AccessRight = auth;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.ModifyUserName = CurrentUserInfo.Name;

                DAL.List dal = new DAL.List();
                dal.ModAll(info);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion

    }
}
