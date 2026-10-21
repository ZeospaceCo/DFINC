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

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Company
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
                            info.LicenseCnt = dtr["LicenseCnt"].ToString().ToNumber<int>();

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

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2011-11-21
        /// 작성목적 : 라이센스 숫자 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetLicenseCount()
        {
            int rtnVal = 0;

            Interop.ZeoLicEp.LicenseManager zeoLicMng = null;

            try
            {
                zeoLicMng = new Interop.ZeoLicEp.LicenseManagerClass();
                rtnVal = zeoLicMng.GetUsers(4);	// 라이센스 사용자(10명 보너스)  GetUsers(4) <- 4 가 DeskPlusEP 제품번호
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetLicenseCount[] : " + ex.ToString());
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : company 활성화 처리
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModCompanyActive(string p)
        {
            SetDecryptString(p);
            string comList = GetDecryptParam("comList", "");

            if (CurrentUserInfo.IsSysAdmin)
            {

                if (comList.Length > 0)
                {
                    Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();

                    foreach (string company in comList.Split(','))
                    {
                        dal.ModActive(company.ToNumber<int>());
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 회사별 라이센스 카운트 저장
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int ModLicense(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);
            string licenseList = GetDecryptParam("licenseList", "");
            if (licenseList.Length == 0) return 0;

            var dal = new Zeospace.DeskPlus.Common.DAL.Company();
            var alList = dal.GetCompany(true);

            foreach (var info in alList)
            {
                info.CompanyCode = info.CompanyCode.ToReverseXSS();
                info.InterfaceCode = info.InterfaceCode.ToReverseXSS();
                info.NameBase = info.NameBase.ToReverseXSS();
                info.NameKo = info.NameKo.ToReverseXSS();
                info.NameEn = info.NameEn.ToReverseXSS();
                info.NameJa = info.NameJa.ToReverseXSS();
                info.NameFr = info.NameFr.ToReverseXSS();
                info.NameZhcn = info.NameZhcn.ToReverseXSS();
                info.NameZhtw = info.NameZhtw.ToReverseXSS();
                info.EstablishDate = info.EstablishDate.ToReverseXSS();
                info.InstitutorName = info.InstitutorName.ToReverseXSS();
                info.InstitutorEnglishName = info.InstitutorEnglishName.ToReverseXSS();
                info.InstitutorRegistrationNumber = info.InstitutorRegistrationNumber.ToReverseXSS();
                info.IndustryClass = info.IndustryClass.ToReverseXSS();
                info.BusinessCondition = info.BusinessCondition.ToReverseXSS();
                info.HomePage = info.HomePage.ToReverseXSS();
                info.ZipCode = info.ZipCode.ToReverseXSS();
                info.Address1 = info.Address1.ToReverseXSS();
                info.Address2 = info.Address2.ToReverseXSS();
                info.EnglishAddress = info.EnglishAddress.ToReverseXSS();
                info.Phone = info.Phone.ToReverseXSS();
                info.Fax = info.Fax.ToReverseXSS();
                info.ActiveCompany = info.ActiveCompany.ToReverseXSS();

                foreach (string li in licenseList.Split(','))
                {
                    int comID = li.Split('_')[0].ToString().ToNumber<int>();
                    int comCnt = li.Split('_')[1].ToString().ToNumber<int>();

                    if (info.CompanyID == comID)
                    {
                        info.LicenseCnt = comCnt;
                        break;
                    }
                }
            }

            

            var zeoLicMng = new Interop.ZeoLicEp.LicenseManagerClass();
            var totLicense = zeoLicMng.GetUsers(4);	// 라이센스 사용자(10명 보너스)  GetUsers(4) <- 4 가 DeskPlusEP 제품번호

            var tot = 0;
            foreach (var info in alList) tot += info.LicenseCnt;
            //라이센스 초과일 경우 -1 반환
            if (tot > totLicense) return -1;
            
            foreach (var info in alList)
            {
                dal.Mod(info);
            }

            //나머지 남은 총 라이센스 카운트
            var balance = totLicense - tot;
            return balance;
        }
    }
}
