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
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSApproval
    {
        /// <summary>
        /// 시행문 직인리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetEnforceInfo(string p)
        {
            try
            {
                SetDecryptString(p);
                int stampID = GetDecryptParam("stampID", "0").ToNumber<int>();

                DAL.EnforceStamp dal = new DAL.EnforceStamp();
                List<BE.EnforceStampInfo> list = dal.GetEnforceStamp(CurrentUserInfo.CompanyID, stampID);

                ObjList listobj = new ObjList();
                foreach (BE.EnforceStampInfo info in list)
                {
                    info.ExpFirstBase = GetLangName(info.ExpFirstBase, info.ExpFirstKo, info.ExpFirstEn, info.ExpFirstJa, info.ExpFirstFr, info.ExpFirstZhcn, info.ExpFirstZhtw, PreLang);
                    info.ExpMidBase = GetLangName(info.ExpMidBase, info.ExpMidKo, info.ExpMidEn, info.ExpMidJa, info.ExpMidFr, info.ExpMidZhcn, info.ExpMidZhtw, PreLang);
                    info.ExpEndBase = GetLangName(info.ExpEndBase, info.ExpEndKo, info.ExpEndEn, info.ExpEndJa, info.ExpEndFr, info.ExpEndZhcn, info.ExpEndZhtw, PreLang);

                    info.AddressFirstBase = GetLangName(info.AddressFirstBase, info.AddressFirstKo, info.AddressFirstEn, info.AddressFirstJa, info.AddressFirstFr, info.AddressFirstZhcn, info.AddressFirstZhtw, PreLang);
                    info.AddressEndBase = GetLangName(info.AddressEndBase, info.AddressEndKo, info.AddressEndEn, info.AddressEndJa, info.AddressEndFr, info.AddressEndZhcn, info.AddressEndZhtw, PreLang);
                    info.PhoneBase = GetLangName(info.PhoneBase, info.PhoneKo, info.PhoneEn, info.PhoneJa, info.PhoneFr, info.PhoneZhcn, info.PhoneZhtw, PreLang);
                    info.FaxBase = GetLangName(info.FaxBase, info.FaxKo, info.FaxEn, info.FaxJa, info.FaxFr, info.FaxZhcn, info.FaxZhtw, PreLang);

                    listobj.Add(info);
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
