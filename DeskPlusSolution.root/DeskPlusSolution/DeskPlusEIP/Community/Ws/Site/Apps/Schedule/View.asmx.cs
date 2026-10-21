using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Community.Web.Ws.Site.Apps.Schedule
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSCommunity
    {
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 반복 일정이 있는 지 체크
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetRepeat(string p)
        {
            try
            {
                bool rtnVal = false;

                SetDecryptString(p);
                int communityID = GetDecryptParam("communityID", "0").ToNumber<int>();
                int scheduleID = GetDecryptParam("scheduleID", "0").ToNumber<int>();

                DAL.Repeat dal = new DAL.Repeat();

                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, communityID, scheduleID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            rtnVal = true;
                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }
    }
}
