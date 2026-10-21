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

namespace Zeospace.DeskPlus.EIP.Web.Ws.Admin.Code
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
        /// sys code 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSysCodePageList(string p)
        {
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string parentCodeID = GetDecryptParam("parentCodeID", "");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                string rtnSetPaging = Utility.SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                Zeospace.DeskPlus.Common.DAL.SYSCode biz = new DeskPlus.Common.DAL.SYSCode();
                DataTableReader dtr = biz.GetPageList(companyID.ToNumber<int>(),
                    parentCodeID, setPageingStr[0], setPageingStr[1], searchField, searchValue, order);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Common.BE.SYSCodeInfo info = new DeskPlus.Common.BE.SYSCodeInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.SYSCodeID = dtr["SYSCodeID"].ToString().ToNumber<int>();
                            info.CodeID = dtr["CodeID"].ToString();
                            info.ParentCodeID = dtr["ParentCodeID"].ToString();
                            info.ActiveCode = dtr["ActiveCode"].ToString();
                            info.InterfaceCode = dtr["InterfaceCode"].ToString();

                            info.NameBase = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);


                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSysCodePageList : " + ex.Message);
                return null;
            }
        }


        /// <summary>
        /// sys code 카운트 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetSysCodeCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string companyID = GetDecryptParam("companyID", "0");
                string parentCodeID = GetDecryptParam("parentCodeID", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                Zeospace.DeskPlus.Common.DAL.SYSCode biz = new DeskPlus.Common.DAL.SYSCode();
                return biz.GetPageListCount(companyID.ToNumber<int>(), parentCodeID, searchField, searchValue);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetSysCodeCount : " + ex.Message);
                return 0;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : syscode 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelSysCode(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string sysCodeList = GetDecryptParam("sysCodeList", "");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {
                Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();

                if (dal.Del(companyID, sysCodeList))
                {
                    string dqName = "SysCode" + CurrentUserInfo.CompanyID.ToString();//applicaton name
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application[dqName] = null;
                    HttpContext.Current.Application.UnLock();
                }

                return true;
            }

            return false;

        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : syscode 활성화 처리
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ActiveSysCode(string p)
        {
            SetDecryptString(p);
            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            string sysCodeList = GetDecryptParam("sysCodeList", "");

            if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin"))
            {

                if (sysCodeList.Length > 0)
                {
                    Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();

                    foreach (string _codeID in sysCodeList.Split(','))
                    {
                        dal.ModActive(companyID, _codeID.ToNumber<int>());
                    }

                    string dqName = "SysCode" + CurrentUserInfo.CompanyID.ToString();//applicaton name
                    HttpContext.Current.Application.Lock();
                    HttpContext.Current.Application[dqName] = null;
                    HttpContext.Current.Application.UnLock();
                }

                return true;
            }

            return false;
        }
    }
}
