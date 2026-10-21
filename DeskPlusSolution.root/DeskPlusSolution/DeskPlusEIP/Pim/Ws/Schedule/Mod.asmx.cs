using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Schedule
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSPim
    {
        #region Get
        #endregion

        #region Set

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-02-05
        /// 작성목적 : 기본 알람으로 설정
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AlarmSetting(string p)
        {
            try
            {
                SetDecryptString(p);
                string alarmKind = GetDecryptParam("AlarmKind", "");
                string alarmTime = GetDecryptParam("AlarmTime", "0");

                BE.UserAlarmInfo info = new BE.UserAlarmInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.UserID = CurrentUserInfo.UserID;
                info.Kind = alarmKind;

                if (alarmKind == "M" || alarmKind == "B")
                {
                    info.Time = alarmTime.ToNumber<int>();
                }
                else
                {
                    info.Time = 0;
                }

                DAL.Alarm dal = new DAL.Alarm();
                dal.Add(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        #endregion
    }
}
