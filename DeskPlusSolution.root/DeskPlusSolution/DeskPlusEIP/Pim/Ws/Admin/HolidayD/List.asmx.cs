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

namespace Zeospace.DeskPlus.Pim.Web.Ws.Admin.HolidayD
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSPim
    {
        #region Get
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            SetDecryptString(p);
            string searchField = GetDecryptParam("searchField", "");
            string searchValue = GetDecryptParam("searchValue", "");

            List<object> listobj = new List<object>();

            DAL.HolidayD biz = new DAL.HolidayD();

            DataTableReader dtr = biz.GetList(CurrentUserInfo.CompanyID, searchField, searchValue);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.HolidayDInfo info = new HolidayDInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.HoliDayID = dtr["HoliDayID"].ToString().ToNumber<int>();
                        info.HoliDayKind = dtr["HoliDayKind"].ToString();
                        info.Title = dtr["Title"].ToString();
                        info.SolarLunar = dtr["SolarLunar"].ToString();
                        info.HoliDay = dtr["HoliDay"].ToString();
                        info.HolidayBetween = dtr["HolidayBetween"].ToString();
                        info.HolidayBetweenDay = dtr["HolidayBetweenDay"].ToString().ToNumber<int>();
                        info.IsReplace = dtr["IsReplace"].ToString();
                        info.IsHolidayKind = dtr["IsHolidayKind"].ToString();
                        info.IsHoliday = dtr["IsHoliday"].ToString();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        info.WriteUserName = dtr["WriteUserName"].ToString();
                        info.WriteTime = (dtr["WriteTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dtr["WriteTime"];
                        info.WriteYMD = dtr["WriteYMD"].ToString();
                        info.ModifyUserID = dtr["ModifyUserID"].ToString().ToNumber<int>();
                        info.ModifyUserName = dtr["ModifyUserName"].ToString();
                        info.ModifyTime = (dtr["ModifyTime"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dtr["ModifyTime"];
                        info.ModifyYMD = dtr["ModifyYMD"].ToString();

                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }
        #endregion
    }
}
