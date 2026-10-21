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
    public class ScheduleMain : Base.BaseWSPim
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
                            _be.Name = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                            , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                                
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
        #endregion

        #region Set
        /// <summary>
        /// 사용자별 가장 마지막에 선택한 탭 저장
        /// </summary>
        /// <param name="p"></param>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModTabMenu(string p)
        {
            bool rtn = true;

            try
            {
                SetDecryptString(p);
                int companyID = GetDecryptParam("companyID", "").ToNumber<int>();
                int userID = GetDecryptParam("userId", "").ToNumber<int>();
                string tabType = GetDecryptParam("tabType", "day");

                Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();

                rtn = env.ModPimTabMenu(companyID, userID, tabType);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModTabMenu: ", ex);
                rtn = false;
            }

            return rtn;
        }
        #endregion
    }
}
