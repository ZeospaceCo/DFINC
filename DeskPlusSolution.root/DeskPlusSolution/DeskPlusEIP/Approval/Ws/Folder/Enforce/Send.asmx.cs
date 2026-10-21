using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Enforce
{
    /// <summary>
    /// Send의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Send : Base.BaseWSApproval
    {

        /// <summary>
        /// 발송내역처리
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddHistory(string p)
        {
            try
            {
                SetDecryptString(p);

                int enforceID = GetDecryptParam("enforceID", "0").ToNumber<int>();
                string sendType = GetDecryptParam("sendType", "");
                string stampType = GetDecryptParam("stampType", "");
                string docType = GetDecryptParam("docType", "");

                if (enforceID.Equals(0) || sendType.Equals("") || stampType.Equals("") || docType.Equals(""))
                {
                    return false;
                }

                BE.EnforceHistoryInfo info = new EnforceHistoryInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.EnforceID = enforceID;
                info.UserID = CurrentUserInfo.UserID;
                info.SendType = sendType;
                info.StampType = stampType;
                info.DocType = docType;

                DAL.EnforceHistory dal = new DAL.EnforceHistory();
                return dal.Add(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

    }
}
