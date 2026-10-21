using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Portlet.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Globalization;


namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class MyPim : Base.BaseWSPortlet
    {

            /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-26
        /// 작성목적 : 일정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetTodo(string p)
        {
            try
            {
                SetDecryptString(p);
                List<object> listobj = new List<object>();
                //CompanyID
                //UserID
                //start
                //end
                int CompanyID = CurrentUserInfo.CompanyID;
                int UserID = CurrentUserInfo.UserID;
                DateTime start =DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
                

                DAL.MyPim dal = new DAL.MyPim();
                DataTableReader dtr = dal.GetTodo(CompanyID, UserID, start);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.PimInfo data = new BE.PimInfo();
                            data.Title = dtr["Title"].ToString();
                            data.StartTime = DateTime.Parse(dtr["StartDate"].ToString()).ToString("hh:mm");
                            data.DisplayID = dtr["ToDoID"].ToString().ToNumber<int>();
                            data.Kind = dtr["Status"].ToString();
                            listobj.Add(data);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSchedule(string p)
        {
            try
            {
                SetDecryptString(p);
                List<object> listobj = new List<object>();
                //CompanyID
                //UserID
                //start
                //end
                int CompanyID = CurrentUserInfo.CompanyID;
                int UserID = CurrentUserInfo.UserID;
                DateTime start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
                DateTime end = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59"));

                DAL.MyPim dal = new DAL.MyPim();


                DataTableReader dtr = dal.GetSchedule(CompanyID, UserID, start, end);


                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.PimInfo data = new BE.PimInfo();
                            data.Title = dtr["Title"].ToString();
                            data.StartTime = DateTime.Parse(dtr["StartTime"].ToString()).ToString("hh:mm");
                            data.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();
                            data.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                            data.OpenType = dtr["OpenType"].ToString().ToNumber<int>();
                            listobj.Add(data);
                        }

                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }
    }
}

