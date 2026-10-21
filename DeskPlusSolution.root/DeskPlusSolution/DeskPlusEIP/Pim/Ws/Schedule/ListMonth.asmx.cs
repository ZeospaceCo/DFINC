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
    public class ListMonth : Base.BaseWSPim
    {
        #region Get


        /// <summary>
        /// 작성목적 : 기념일 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetHolidayList(string p)
        {
            try
            {
                List<object> listobj = new List<object>();

                SetDecryptString(p);

                string StartTime = GetDecryptParam("StartDate", DateTime.Now.ToString("H_yyyyMMdd"));
                if (StartTime.Length > 2)
                {
                    if (StartTime.Substring(0, 2) == "H_")
                    {
                        StartTime = StartTime.Replace("H_", "");
                        StartTime = StartTime.Substring(0, 4) + "-" + StartTime.Substring(4, 2) + "-" + StartTime.Substring(6, 2);
                    }

                }

                BE.ScheduleInfo info = new ScheduleInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.StartTime = DateTime.Parse(StartTime);
                info.UserID = CurrentUserInfo.UserID;

                DAL.Schedule bll = new DAL.Schedule();
                DataTableReader dtr = bll.GetSheduleMon(info);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ScheduleInfo scheinfo = new ScheduleInfo();
                            scheinfo.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                            scheinfo.Type = dtr["Type"].ToString();
                            scheinfo.Title = dtr["Title"].ToString(); 

                            listobj.Add(scheinfo);
                        }
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

        #endregion

        #region Set
        /// <summary>
        /// 사용자별 가장 마지막에 선택한 일정분류 저장
        /// </summary>
        /// <param name="p"></param>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCheckMenu(string p)
        {
            bool rtn = true;

            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
                int userID = GetDecryptParam("userId", "").ToNumber<int>();
                string checkState = GetDecryptParam("checkState", "");

                Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();

                rtn = env.ModPimCheckMenu(companyID, userID, checkState);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModCheckSchedule: ", ex);
                rtn = false;
            }

            return rtn;
        }
        #endregion

        #region utill
        public string MaxTextCut(string strSource, int iMaxLength, string strTail)
        {
            string strRt = string.Empty;
            strRt = strSource.ToReverseXSS();
            if (strSource.ToReverseXSS().Length > iMaxLength)
            {
                strRt = strSource.ToReverseXSS().Substring(0, iMaxLength) + strTail;
            }

            return strRt;//.ToXSS(); 
        }


        #endregion
    }
}
