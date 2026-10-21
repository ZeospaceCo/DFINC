using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Common.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;

namespace Zeospace.DeskPlus.Common.Web.Ws.PDF
{
    /// <summary>
    /// PDF의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class PDF : Base.BaseWSCommon
    {

        /// <summary>
        /// 작성자 : 심재운
        /// pdf생성유무 체크
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetPDFCreateCheck(string p)
        {
            try
            {
                SetDecryptString(p);

                string module = GetDecryptParam("module", "");
                int listID = GetDecryptParam("listID", "0").ToNumber<int>();

                DAL.CreatePDF dal = new DAL.CreatePDF();
                DataSet ds = dal.GetPDFLog(CurrentUserInfo.CompanyID, module, listID);

                string rtnVal = string.Empty;

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        rtnVal = ds.Tables[0].Rows[0]["Status"].ToString();
                    }
                }

                return rtnVal;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        
    }
}
