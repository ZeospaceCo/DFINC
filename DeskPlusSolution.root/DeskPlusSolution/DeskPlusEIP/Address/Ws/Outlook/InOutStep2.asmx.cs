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
using Zeospace.DeskPlus.Address.BE;

namespace Zeospace.DeskPlus.Address.Web.Ws.Outlook
{
    /// <summary>
    /// InOutStep2의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class InOutStep2 : Base.BaseWSAddress
    {

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCSVPageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string types = GetDecryptParam("types", "");

                List<object> listobj = new List<object>();
                DAL.CSV dal = new DAL.CSV();
                DataTableReader dtr = null;

                switch (types)
                {
                    case "ALL":
                        dtr = dal.GetCSV(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                        break;
                    case "EXCEPT":
                        dtr = dal.GetCSVExcept(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                        break;
                    case "OVERLAP":
                        dtr = dal.GetCSVOverlap(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                        break;
                }

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.CSVInfo info = new BE.CSVInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CSVID = dtr["CSVID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            info.Name = dtr["Name"].ToString();
                            info.Email = dtr["Email"].ToString();
                            info.MobilePhone = dtr["MobilePhone"].ToString();
                            info.HomePhone = dtr["HomePhone"].ToString();
                            info.CompanyPhone = dtr["CompanyPhone"].ToString();
                            info.Company = dtr["Company"].ToString();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetPageList", ex);
                throw;
            }
        }
    }
}
