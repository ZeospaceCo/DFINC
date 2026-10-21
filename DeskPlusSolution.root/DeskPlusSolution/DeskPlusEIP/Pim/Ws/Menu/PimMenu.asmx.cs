using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;
using System.Collections;
using System.Globalization;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Menu
{
    /// <summary>
    /// GetPim의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class PimMenu : Base.BaseWSPim
    {
        #region Get
        /// <summary>
        /// 전체일정
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetScheduleListMenu(string p)
        {
            List<object> listobj = new List<object>();
            try
            {
                int companyID = 0;

                string StartTime = string.Empty;

                SetDecryptString(p);
                companyID = CurrentUserInfo.CompanyID;
                StartTime = GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd"));

                BE.ScheduleInfo info = new ScheduleInfo();
                info.CompanyID = companyID;
                info.StartTime = DateTime.Parse(StartTime);
                info.UserID = CurrentUserInfo.UserID;
                info.DeptID = CurrentUserInfo.DeptID;


                DAL.Schedule bll = new DAL.Schedule();

                DataTable dt = new DataTable();
                DataTableReader dtr = new DataTableReader(dt);

                dtr = bll.GetSheduleMon(info);


                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ScheduleInfo data = new ScheduleInfo();
                            data.CompanyID = companyID;
                            data.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                            data.ToDoID = dtr["ToDoID"].ToString().ToNumber<int>();
                            data.Type = dtr["Type"].ToString();



                            data.Title = dtr["Title"].ToString();

                            data.Content = dtr["Content"].ToString();



                            data.stringStartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToString("yyyy-MM-dd HH:mm");
                            data.stringENDTime = DateTime.Parse(dtr["EndTime"].ToString()).ToString("yyyy-MM-dd HH:mm");
                            data.Repeat = dtr["Repeat"].ToString();
                            data.Alert = dtr["Alert"].ToString();

                            data.AlertType = dtr["AlertType"].ToString();
                            data.AlertTime = dtr["AlertTime"].ToString().ToNumber<int>(); ;
                            data.AlertTargetAddress = dtr["AlertTargetAddress"].ToString();
                            data.IsOpen = dtr["IsOpen"].ToString();

                            data.UserID = dtr["UserID"].ToString().ToNumber<int>(); ;
                            data.DeptID = dtr["DeptID"].ToString().ToNumber<int>(); ;
                            data.WriteDate = DateTime.Parse(dtr["WriteDate"].ToString());


                            data.OpenType = dtr["OpenType"].ToString();
                            switch (data.OpenType)
                            {
                                case "-1"://나의일정
                                    data.Color = "lemonchiffon";
                                    break;
                                case "0"://개인일정
                                    data.Color = "lightgreen";
                                    break;
                                case "3"://부서일정
                                    data.Color = "yellow";
                                    break;
                                case "1"://공유일정
                                    data.Color = "orange";
                                    break;
                                case "2"://전체일정
                                    data.Color = "purple";
                                    break;
                            }

                            data.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>(); ;
                            data.Lunar = dtr["Lunar"].ToString();
                            listobj.Add(data);
                        }
                    }
                }
                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetScheduleListMenu", ex);
                return listobj;
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetScheduleStatic(string p)
        {
            List<object> listobj = new List<object>();
            int _UserID = 0;
            int _CompanyID = 0;
            int _DeptID = CurrentUserInfo.DeptID;
            DateTime _StartDate = DateTime.Now;
            DateTime _EndDate = DateTime.Now;
            SetDecryptString(p);
            _UserID = GetDecryptParam("UserID", "0").ToNumber<int>();
            _CompanyID = GetDecryptParam("CompanyID", "0").ToNumber<int>();

            _StartDate = DateTime.Parse(GetDecryptParam("StartDate", DateTime.Now.ToString("yyyy-MM-dd")));
            _EndDate = _StartDate.AddDays(1);

            DAL.Schedule biz = new DAL.Schedule();
            DataTable dt = new DataTable();
            DataTableReader dtr = new DataTableReader(dt);



            dtr = biz.GetScheduleStatic(_CompanyID, _UserID, _DeptID, _StartDate);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ScheduleInfo data = new ScheduleInfo();
                        data.OpenType = dtr["OpenType"].ToString();
                        data.CompanyID = dtr["CNT"].ToString().ToNumber<int>();

                        listobj.Add(data);
                    }
                }
            }


            return listobj;
        }

        #endregion

        #region Set
        #endregion
    }
}
