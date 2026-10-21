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
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSPim
    {
        #region Get
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCheckCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string solarLunar = GetDecryptParam("solarLunar", "");
                string holiDay = GetDecryptParam("holiDay", "");

                int icnt = 0;

                DAL.HolidayD biz = new DAL.HolidayD();

                icnt = biz.GetCheckCount(CurrentUserInfo.CompanyID, solarLunar, holiDay);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetCheckCountByHoliDayID(string p)
        {
            try
            {
                SetDecryptString(p);
                string holiDayID = GetDecryptParam("holiDayID", "0");
                string solarLunar = GetDecryptParam("solarLunar", "");
                string holiDay = GetDecryptParam("holiDay", "");

                int icnt = 0;

                DAL.HolidayD biz = new DAL.HolidayD();

                icnt = biz.GetCheckCountByHoliDayID(CurrentUserInfo.CompanyID, holiDayID.ToNumber<int>(), solarLunar, holiDay);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region Del
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int DelHoliDay(string p)
        {
            try
            {
                int CompanyId = 0;
                int HoliDayID = 0;
                SetDecryptString(p);

                CompanyId = CurrentUserInfo.CompanyID;
                HoliDayID = GetDecryptParam("holiDayID", "0").ToNumber<int>();

                DAL.HolidayD dal = new DAL.HolidayD();

                dal.Del(CompanyId, HoliDayID);

                BLL.HolidayR bll2 = new BLL.HolidayR();

                bll2.ClearCacheHolidayList(CurrentUserInfo.CompanyID);
                bll2.SetCacheHolidayList(CurrentUserInfo.CompanyID, DateTime.Now.ToString("yyyy"));

                return 1;
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
