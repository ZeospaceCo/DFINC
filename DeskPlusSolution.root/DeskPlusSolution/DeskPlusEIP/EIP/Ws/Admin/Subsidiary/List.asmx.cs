using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Interop.ZeoLicEp;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Subsidiary
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSEIP
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 회사 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCompanyPageList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string activeCompany = GetDecryptParam("activeCompany", "0");
            string totalCount = GetDecryptParam("totalCount", "0");
            string pageSize = GetDecryptParam("pageSize", "0");
            string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
            string order = GetDecryptParam("order", "WriteDate asc");

            string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
            string[] setPageingStr = rtnSetPaging.Split(',');

            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = new List<Zeospace.DeskPlus.Common.BE.CompanyInfo>();
            Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();

            DataTableReader dtr = dal.GetCompanyPageList(companyID,
                    (activeCompany == "1") ? true : false, setPageingStr[0], setPageingStr[1], order);

            try
            {
                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Common.BE.CompanyInfo info = new Zeospace.DeskPlus.Common.BE.CompanyInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();

                            info.CompanyName = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            info.EstablishDate = dtr["EstablishDate"].ToString();
                            info.InstitutorName = dtr["InstitutorName"].ToString();
                            info.InstitutorEnglishName = dtr["InstitutorEnglishName"].ToString();
                            info.Phone = dtr["Phone"].ToString();
                            info.Fax = dtr["Fax"].ToString();
                            info.ActiveCompany = dtr["ActiveCompany"].ToString();
                            info.UserCnt = dtr["UserCnt"].ToString().ToNumber<int>();

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetCompanyList : " + ex);
                return null;
            }
        }
    }
}
