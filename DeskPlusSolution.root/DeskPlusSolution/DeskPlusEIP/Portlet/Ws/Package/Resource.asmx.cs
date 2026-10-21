using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// Opinion의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Resource : Base.BaseWSPortlet
    {
        /// <summary>
        /// 현재 년월일 가져오기 [2012-01-01]
        /// </summary>
        /// <returns></returns>
        public string GetYearMonthDay()
        {
            string nowYearMonDay = "";
            int TargetYear = System.DateTime.Now.Year;
            int TargetMonth = System.DateTime.Now.Month;
            int TargetDay = System.DateTime.Now.Day;

            if (TargetMonth.ToString().Length == 1)
            {
                nowYearMonDay = TargetYear.ToString() + "-" + "0" + TargetMonth.ToString();
            }
            else
            {
                nowYearMonDay = TargetYear.ToString() + "-" + TargetMonth.ToString();
            }

            if (TargetDay.ToString().Length == 1)
            {
                nowYearMonDay = nowYearMonDay + "-" + "0" + TargetDay.ToString();
            }
            else
            {
                nowYearMonDay = nowYearMonDay + "-" + TargetDay.ToString();
            }
            return nowYearMonDay;
        }

        /// <summary>
        /// 자원 관리 리스트 
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRMList()
        {
            try
            {
                Zeospace.DeskPlus.RM.DAL.Reverse dal = new Zeospace.DeskPlus.RM.DAL.Reverse();
                //string tmpDate = System.DateTime.Now.ToShortDateString();
                string tmpDate = GetYearMonthDay();

                DataSet ds = dal.GetPortletList(CurrentUserInfo.CompanyID, tmpDate);

                List<object> listobj = new List<object>();

                int row = 0;
                string tmpListID = "";
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Zeospace.DeskPlus.RM.BE.ReserveInfo info = new Zeospace.DeskPlus.RM.BE.ReserveInfo();

                        info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                        info.ReservationID = dr["ReservationID"].ToString().ToNumber<int>();
                        info.ParentReservationID = dr["ParentReservationID"].ToString().ToNumber<int>();
                        info.Subject = dr["Subject"].ToString();
                        info.StartTime = Convert.ToDateTime(dr["StartDate"].ToString()).ToShortTimeString();
                        info.EndTime = Convert.ToDateTime(dr["EndDate"].ToString()).ToShortTimeString();
                        info.WriteUserID = dr["WriteUserID"].ToString().ToNumber<int>();

                        //예약자 이름
                        info.WriteUserName = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(),
                                                            dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                                                            dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                        int tmpTitleCount = dr["Title"].ToString().Length;
                        string tmpTitle = dr["Title"].ToString();
                        if (tmpTitleCount > 25)
                        {
                            tmpTitle = tmpTitle.Substring(0, 25) + "..";
                        }

                        info.Title = tmpTitle;

                        if (tmpListID != dr["ListID"].ToString())
                        {
                            info.Row = 1;
                            row = 1;
                            tmpListID = dr["ListID"].ToString();
                        }
                        else
                        {
                            row += row + 1;
                            info.Row = row;
                        }

                        listobj.Add(info);

                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }        
    }
}
