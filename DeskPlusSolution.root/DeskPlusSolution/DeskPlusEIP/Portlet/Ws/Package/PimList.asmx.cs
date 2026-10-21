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
    public class PimList : Base.BaseWSPortlet
    {     

        /// <summary>
        /// 일정
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetScheduleList(string p)
        {
            try
            {
                SetDecryptString(p);

                string tmpDate = GetDecryptParam("tmpDate", "");

                List<object> listobj = new List<object>();
                DAL.MyPim dal = new DAL.MyPim();
                DataTableReader dtr = dal.GetScheduleList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, tmpDate);

                string tmpPGubun = "N";

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.PimInfo info = new BE.PimInfo();
                            info.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                            info.DisplayID = dtr["DisplayID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                            info.EndDate = Convert.ToDateTime(dtr["EndDate"].ToString());
                            info.StartTime = info.StartDate.Hour.ToString().PadLeft(2, '0') + ":" + info.StartDate.Minute.ToString().PadLeft(2, '0');
                            info.EndTime = info.EndDate.Hour.ToString().PadLeft(2, '0') + ":" + info.EndDate.Minute.ToString().PadLeft(2, '0'); 

                            //여러날짜로 할때 이렇게 보여야 할 것 같구만...
                            //info.StartTime = info.StartDate.Month.ToString().PadLeft(2, '0') + "." + info.StartDate.Day.ToString().PadLeft(2, '0') + " " + info.StartDate.Hour.ToString().PadLeft(2, '0') + ":" + info.StartDate.Minute.ToString().PadLeft(2, '0');
                            //info.EndTime = info.EndDate.Month.ToString().PadLeft(2, '0') + "." + info.EndDate.Day.ToString().PadLeft(2, '0') + " " + info.EndDate.Hour.ToString().PadLeft(2, '0') + ":" + info.EndDate.Minute.ToString().PadLeft(2, '0');                            
                            
                            info.OpenType = dtr["OpenType"].ToString().ToNumber<int>();

                            if ((info.StartDate > DateTime.Now) && (tmpPGubun == "N"))
                            {
                                info.PGubun = "Y";
                                tmpPGubun = "Y";
                            }
                            else
                            {
                                info.PGubun = "N";                                
                            }

                            listobj.Add(info);
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

        /// <summary>
        /// 힐일
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetTodoList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.PimList).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                string tmpDate = GetDecryptParam("tmpDate", "");

                List<object> listobj = new List<object>();
                DAL.MyPim dal = new DAL.MyPim();
                DataTableReader dtr = dal.GetTodoList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tmpDate);
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.PimInfo info = new BE.PimInfo();
                            info.ScheduleID = dtr["ToDoID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();

                            if (dtr["Priority"].ToString() == "1")
                            {
                                info.Priority = resMng.GetString("Top");
                            }
                            else if (dtr["Priority"].ToString() == "2")
                            {
                                info.Priority = resMng.GetString("Middle");
                            }
                            else
                            {
                                info.Priority = resMng.GetString("Bottom");
                            }

                            listobj.Add(info);
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

