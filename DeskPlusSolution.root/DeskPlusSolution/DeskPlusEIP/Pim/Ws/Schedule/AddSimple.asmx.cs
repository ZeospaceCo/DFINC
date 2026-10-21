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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

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
    public class AddSimple : Base.BaseWSPim
    {
        #region Get
        #endregion

        #region Set
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddSchedule(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.Schedule.AddSimple).Assembly;	        //어셈블리 선언

            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            BE.ScheduleInfo info = new ScheduleInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ToDoID = GetDecryptParam("ToDoID", "0").ToNumber<int>();
            info.Type = GetDecryptParam("Type", "");
            info.Title = GetDecryptParam("Title", "");
            info.Content = GetDecryptParam("Content", "");
            info.StartTime = DateTime.Parse(GetDecryptParam("StartTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm")));
            info.EndTime = DateTime.Parse(GetDecryptParam("EndTime", info.StartTime.AddHours(1).ToString("yyyy-MM-dd HH:mm")));
            if (info.StartTime == info.EndTime)
            {
                info.EndTime = info.EndTime.AddMinutes(30);
            }

            info.Repeat = GetDecryptParam("Repeat", "N");

            string AlarmKind = GetDecryptParam("AlarmKind", "");
            string AlarmTime = GetDecryptParam("AlarmTime", "0");

            if (!AlarmKind.Equals(""))
            {
                info.Alert = "Y";
                info.AlertType = AlarmKind;

                if (!AlarmKind.Equals("E"))
                {
                    info.AlertTime = int.Parse(AlarmTime);
                    info.AlertTargetAddress = "";
                }
                else
                {
                    info.AlertTime = 0;
                    info.AlertTargetAddress = GetDecryptParam("AlarmMail", "");
                }
            }
            else
            {
                info.Alert = "N";
                info.AlertType = "";
                info.AlertTime = 0;
                info.AlertTargetAddress = "";
            }

            info.IsOpen = GetDecryptParam("IsOpen", "Y");
            info.UserID = CurrentUserInfo.UserID;
            info.DeptID = CurrentUserInfo.DeptID;
            info.OpenType = GetDecryptParam("OpenType", "-1");
            info.ReservationID = GetDecryptParam("ReservationID", "0").ToNumber<int>();
            info.Lunar = GetDecryptParam("Lunar", "Y");
            info.UserName = CurrentUserInfo.Name;
            info.StatusID = GetDecryptParam("StatusID", "0").ToNumber<int>();
            info.AlarmContent = "";

            DAL.Schedule biz = new DAL.Schedule();
            int scheduleid = biz.Add(info);

            BE.DisplayInfo disinfo = new DisplayInfo();
            disinfo.CompanyID = CurrentUserInfo.CompanyID;
            disinfo.StartDate = info.StartTime;
            disinfo.EndDate = info.EndTime;
            if (info.StartTime == info.EndTime)
            {
                disinfo.EndDate = disinfo.EndDate.AddMinutes(30);
            }
            disinfo.ScheduleID = scheduleid;
            disinfo.UserID = CurrentUserInfo.UserID;
            DAL.Display bizdis = new Display();
            int DisplayID = bizdis.AddDisplay(disinfo);

            #region 알람

            if ((scheduleid > 0) && (info.Alert == "Y"))
            {
                Hashtable ht = new Hashtable();

                Pim.BLL.Schedule _abill = new BLL.Schedule();
                Dictionary<string, string> paraDical = new Dictionary<string, string>();

                paraDical.Add("DisplayID", DisplayID.ToString());
                paraDical.Add("CompanyID", CurrentUserInfo.CompanyID.ToString());
                paraDical.Add("ScheduleID", scheduleid.ToString());
                info.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                info.SenderMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                if (AlarmKind == "M" || AlarmKind == "B")
                    _abill.SendAlarm(info, scheduleid, CurrentUserInfo.UserID, ht, DisplayID);

                //매일알람을 선택했을경우
                if ((AlarmKind == "E") && (info.AlertTargetAddress != ""))
                {
                    info.Title = "[" + resMng.GetString("PimAlarm") + "]" + info.StartTime + "~" + info.EndTime + " " + info.Title;
                    _abill.SendAlarm(info, 0, 0, ht, 0);
                }                                    
            }

            #endregion

            return 1;
        }

        #endregion
    }
}
