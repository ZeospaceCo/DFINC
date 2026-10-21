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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Zeospace.DeskPlus.Component.GoogleAdapter;


namespace Zeospace.DeskPlus.Pim.Web.Ws.Schedule
{
    /// <summary>
    /// LinkPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class LinkPim : Base.BaseWSPim
    {

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string  GetGoogleCalendarSingle(string p)
        {
            
            //기간
            string strPeriod = string.Empty;            
            string strDate  = string.Empty;
            SetDecryptString(p);
            strPeriod = GetDecryptParam("Period", "");
            strDate = GetDecryptParam("DATE", "");

            DAL.GoogleCalendar sDal = new DAL.GoogleCalendar();
            string googleId = string.Empty;
            string googlePwd = string.Empty;
            DataTableReader dtr = sDal.GetGoogleAccount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        googleId = dtr["Google"].ToString();
                        googlePwd =AESManager.Decrypt(dtr["GooglePwd"].ToString());
                    }
                }
            }
            DateTime dt = DateTime.Now;
            switch (strPeriod)
            {
                case "0": //전체
                    dt = DateTime.Parse("2001-01-01");
                    break;
                case "WEEK": //주
                    dt = dt.AddDays(-7);
                    break;
                case "MON": //월
                    dt = dt.AddMonths(-1);
                    break;
                case "U": //기간
                    dt = DateTime.Parse(strDate);
                    break;
            }
            try
            {
                Zeospace.DeskPlus.Component.GoogleAdapter.DataSynchronizer.SyncService gservice = new Zeospace.DeskPlus.Component.GoogleAdapter.DataSynchronizer.SyncService("googlepim", googleId, googlePwd);
                IList list = gservice.SynchronizeEvents(dt);

                
                List<BE.GoogleCalendarInfo> listobj = new List<BE.GoogleCalendarInfo>();

                foreach (Zeospace.DeskPlus.Component.GoogleAdapter.GenericEntities.GenericEvent _be in list)
                {
                    //_be.Contents      내용
                    //_be.StartTime 시작
                    //_be.EndTime;  종료
                    //_be.Location 장소
                    //_be.Title; 제목
                    //OutputScheduleLog(_be.Title + "|" + _be.UID);
                    BE.GoogleCalendarInfo _info=new GoogleCalendarInfo();
                    _info.CompanyID = CurrentUserInfo.CompanyID;
                    _info.UserID = CurrentUserInfo.UserID;
                    _info.Title = _be.Title.ToXSS();    //제목
                    _info.Location = _be.Location.ToXSS();  //장소 
                    _info.StartTime = _be.StartTime; //시작시간

                    if (_be.StartTime == _be.EndTime)
                        _info.EndTime = _be.EndTime.AddHours(1);    //종료시간
                    else
                        _info.EndTime = _be.EndTime;    //종료시간
                    
                    if (_be.Contents.Length > 1000)
                        _info.Content = _be.Contents.Substring(0, 1000).ToXSS(); //내용
                    else
                        _info.Content = _be.Contents.ToXSS();

                    _info.Uid=_be.UID;
                    _info.DeptID = CurrentUserInfo.DeptID;
                    listobj.Add(_info);                    
                }

                BLL.GoogleCalendar bll = new BLL.GoogleCalendar();
                if(bll.GoogleCalendarBulk(listobj))
                {
                    return "Success";
                }
                
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetGoogleCalendarSingle", ex);
                if (ex.Message == "Invalid credentials")
                    return "AccountErr";
                else 
                    return "False";
            }
            

            return "Success";
        }

    }
}
