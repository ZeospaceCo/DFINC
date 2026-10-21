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
    public class Menu : Base.BaseWSPim
    {
        #region Get
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetOpenUserComboList(string p)
        {
            List<object> listobj = new List<object>();
            try
            {
                SetDecryptString(p);

                DAL.OpenUser biz = new OpenUser();
                DataTableReader dtr = biz.GetOpenUserCombo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);


                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.OpenUserInfo _be = new OpenUserInfo();
                            _be.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            _be.Name = dtr["Name"].ToString();

                            listobj.Add(_be);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetTodoList", ex);
                return listobj;
            }
            return listobj;
        }

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
                StartTime = GetDecryptParam("StartDate", "");



                BE.ScheduleInfo info = new ScheduleInfo();
                info.CompanyID = companyID;
                info.StartTime = DateTime.Now;//DateTime.Parse(StartTime);
                info.UserID = CurrentUserInfo.UserID;
                info.DeptID = CurrentUserInfo.DeptID;


                DAL.Schedule bll = new DAL.Schedule();

                DataTable dt = new DataTable();
                DataTableReader dtr = new DataTableReader(dt);



                dtr = bll.GetSheduleMenu(info);


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
                            /*
                            switch (data.Type)
                            {
                                case "I"://나의일정
                                    data.Type = "개인일정";
                                    break;
                                case "J"://개인일정
                                    data.Type = "개인기념일";
                                    break;
                                case "G"://부서일정
                                    data.Type = "전체공유기념일";
                                    break;
                                case "M"://공유일정
                                    data.Type = "전체공유절기";
                                    break;
                                case "K"://전체일정
                                    data.Type = "전체공유국가공휴일";
                                    break;
                                case "N"://전체일정
                                    data.Type = "전체공유국가공휴일";
                                    break;
                                case "A"://전체일정
                                    data.Color = "purple";
                                    break;
                            }
                            */
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

        #endregion

        #region Set
        #endregion
    }
}
