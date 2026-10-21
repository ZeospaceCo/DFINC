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
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.FormGeneration
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSApproval
    {
        #region GET
        /// <summary>
        /// 양식리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            if (IsAdmin == false) return null;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Admin.FormGeneration.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            int RecordPerPage = GetDecryptParam("recordPerPage", "0").ToNumber<int>();
            int CurrentPage = GetDecryptParam("currentPage", "0").ToNumber<int>();
            string SearchField = GetDecryptParam("searchField", "");
            string SearchValue = GetDecryptParam("searchValue", "");
            string SearchTerm = GetDecryptParam("searchTerm", "");
            string SearchTermValue = GetDecryptParam("searchTermValue", "");
            string Order = GetDecryptParam("order", "");

            string startDate = "", endDate = "", getDate = "";
            if (SearchTerm.ToUpper().Equals("WEEK") || SearchTerm.ToUpper().Equals("MONTH"))
            {
                getDate = (SearchTerm.ToUpper().Equals("WEEK")) ? DateHelper.GetCultureStartEndDate("week", PreLangReal) : DateHelper.GetCultureStartEndDate("month", PreLangReal);
                startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (SearchTerm.ToUpper().Equals("PERIOD"))
            {
                startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            DAL.Form fDal = new DAL.Form();
            DataTableReader dtr = fDal.GetList(CurrentUserInfo.CompanyID, RecordPerPage, CurrentPage, SearchField, SearchValue, startDate, endDate, Order);
            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.FormInfo fInfo = new BE.FormInfo();
                        fInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        fInfo.FormID = dtr["FormID"].ToString().ToNumber<int>();
                        fInfo.FolderID = dtr["FolderID"].ToString().ToNumber<int>();
                        fInfo.FormBase = this.GetLangName(
                                        dtr["FormBase"].ToString().ToNull(), dtr["FormKo"].ToString().ToNull(), dtr["FormEn"].ToString().ToNull()
                                        , dtr["FormJa"].ToString().ToNull(), dtr["FormFr"].ToString().ToNull(), dtr["FormZhcn"].ToString().ToNull()
                                        , dtr["FormZhtw"].ToString().ToNull(), PreLang);
                        fInfo.NameBase = this.GetLangName(
                                        dtr["FolderBase"].ToString().ToNull(), dtr["FolderKo"].ToString().ToNull(), dtr["FolderEn"].ToString().ToNull()
                                        , dtr["FolderJa"].ToString().ToNull(), dtr["FolderFr"].ToString().ToNull(), dtr["FolderZhcn"].ToString().ToNull()
                                        , dtr["FolderZhtw"].ToString().ToNull(), PreLang);
                        fInfo.IsLegacy = dtr["IsLegacy"].ToString().Equals("1") ? "Y" : "N";
                        fInfo.IsLinked = dtr["IsLinked"].ToString().Equals("1") ? "Y" : "N";
                        fInfo.IsActive = dtr["IsActive"].ToString().Equals("1") ? "Y" : "N";
                        fInfo.IsActiveStr = dtr["IsActive"].ToString().Equals("1") ? this.GetResource("IsActiveTitY") : this.GetResource("IsActiveTitN");

                        DateTime dt;
                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                            fInfo.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            fInfo.WriteDateStr = string.Empty;

                        fInfo.DraftCnt = dtr["DraftCnt"].ToString();

                        uobj.Add(fInfo);
                    }
                }
            }
            return uobj;
        }

        /// <summary>
        /// 양식리스트 갯수 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCnt(string p)
        {
            if (IsAdmin == false) return 0;

            SetDecryptString(p);

            string SearchField = GetDecryptParam("searchField", "");
            string SearchValue = GetDecryptParam("searchValue", "");
            string SearchTerm = GetDecryptParam("searchTerm", "");
            string SearchTermValue = GetDecryptParam("searchTermValue", "");

            string startDate = "", endDate = "", getDate = "";
            if (SearchTerm.ToUpper().Equals("WEEK") || SearchTerm.ToUpper().Equals("MONTH"))
            {
                getDate = (SearchTerm.ToUpper().Equals("WEEK")) ? DateHelper.GetCultureStartEndDate("week", PreLangReal) : DateHelper.GetCultureStartEndDate("month", PreLangReal);
                startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (SearchTerm.ToUpper().Equals("PERIOD"))
            {
                startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            DAL.Form fDal = new DAL.Form();
            int listCnt = fDal.GetList(CurrentUserInfo.CompanyID, SearchField, SearchValue, startDate, endDate);

            return listCnt;
        }
        #endregion

        #region SET

        /// <summary>
        /// 삭제(다중)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelList(string p)
        {
            if (IsAdmin == false) return false;

            SetDecryptString(p);
            string[] arrID = GetDecryptParam("id", "").Trim(',').Split(',');

            BLL.Forms fBll = new BLL.Forms();
            foreach (string id in arrID)
            {
                if (!id.Equals(""))
                {
                    fBll.Del(CurrentUserInfo.CompanyID, id.ToNumber<int>());
                }
            }
            return true;
        }

        /// <summary>
        /// 활성화/비활성화
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool UpdActive(string p)
        {
            SetDecryptString(p);
            string[] arrID = GetDecryptParam("id", "").Trim(',').Split(',');

            BLL.Forms fBll = new BLL.Forms();
            foreach (string id in arrID)
            {
                if (id.Split('-').Length.Equals(2))
                {
                    fBll.UpdActive(CurrentUserInfo.CompanyID, id.Split('-')[0].ToNumber<int>(), id.Split('-')[1]);
                }
            }
            return true;
        }

        #endregion
    }
}
