using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Ws.MyInfo
{
    /// <summary>
    /// Alarm의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Alarm : Base.BaseWSAccount
    {
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetAlarm(string p)
        {
            try
            {
                SetDecryptString(p);

                string app = GetDecryptParam("app", "0-");
                string boa = GetDecryptParam("boa", "0-");
                string pims = GetDecryptParam("pims", "0-");
                string task = GetDecryptParam("task", "0-");
                string work = GetDecryptParam("work", "0-");
                string comm = GetDecryptParam("comm", "0-");
                string rm = GetDecryptParam("rm", "0-");
                string sns = GetDecryptParam("sns", "0-");

                int companyid = GetDecryptParam("cid", CurrentUserInfo.CompanyID.ToString()).ToNumber<int>();
                int userid = GetDecryptParam("uid", CurrentUserInfo.UserID.ToString()).ToNumber<int>();

                BE.UserEnvironmentInfo info = new UserEnvironmentInfo();
                info.CompanyID = companyid;
                info.UserID = userid;
                info.AlarmApproval = app;
                info.AlarmBoard = boa;
                info.AlarmPims = pims;
                info.AlarmTask = task;
                info.AlarmWork = work;
                info.AlarmCommunity = comm;
                info.AlarmRM = rm;
                info.AlarmSNS = sns;

                DAL.UserEnvironment dal = new DAL.UserEnvironment();
                return dal.ModAlarm(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetAlarm : " + ex.Message);
                throw;
            }
        }
    }
}
