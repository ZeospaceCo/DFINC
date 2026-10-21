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
    public class Default : Base.BaseWSMDM
    {

        #region ADD()
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-01-16
        /// 작성목적 : 환경설정 저장
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
                string approve = GetDecryptParam("approve", "0");

                BE.EnvInfo info = new BE.EnvInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.MailAlarm = mailAlarm;
                info.EtcAlarm = etcAlarm;
                info.LoginType = loginType;
                info.AccessRight = auth;
                info.Approve = approve;
                info.WriteUserID = CurrentUserInfo.UserID;
                info.WriteUserName = CurrentUserInfo.Name;
                info.ModifyUserID = CurrentUserInfo.UserID;
                info.ModifyUserName = CurrentUserInfo.Name;

                DAL.Env dal = new DAL.Env();
                dal.Add(info);

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
