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

namespace Zeospace.DeskPlus.Approval.Web.Ws.Preference.FavoriteForm
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
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Preference.FavoriteForm.List).Assembly;	//어셈블리 선언
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

            DAL.FavoriteForm fDal = new DAL.FavoriteForm();
            DataTableReader dtr = fDal.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, RecordPerPage, CurrentPage, SearchField, SearchValue, startDate, endDate, Order);
            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.FavoriteFormInfo info = new BE.FavoriteFormInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.FavoriteFormID = dtr["FavoriteFormID"].ToString().ToNumber<int>();
                        info.FormID = dtr["FormID"].ToString().ToNumber<int>();
                        info.FolderID = dtr["FolderID"].ToString().ToNumber<int>();
                        info.FormBase = this.GetLangName(
                                        dtr["FormBase"].ToString().ToNull(), dtr["FormKo"].ToString().ToNull(), dtr["FormEn"].ToString().ToNull()
                                        , dtr["FormJa"].ToString().ToNull(), dtr["FormFr"].ToString().ToNull(), dtr["FormZhcn"].ToString().ToNull()
                                        , dtr["FormZhtw"].ToString().ToNull(), PreLang);
                        info.NameBase = this.GetLangName(
                                        dtr["FolderBase"].ToString().ToNull(), dtr["FolderKo"].ToString().ToNull(), dtr["FolderEn"].ToString().ToNull()
                                        , dtr["FolderJa"].ToString().ToNull(), dtr["FolderFr"].ToString().ToNull(), dtr["FolderZhcn"].ToString().ToNull()
                                        , dtr["FolderZhtw"].ToString().ToNull(), PreLang);
                        info.PathNameBase = this.GetLangName(
                                        dtr["PathBase"].ToString().ToNull(), dtr["PathKo"].ToString().ToNull(), dtr["PathEn"].ToString().ToNull()
                                        , dtr["PathJa"].ToString().ToNull(), dtr["PathFr"].ToString().ToNull(), dtr["PathZhcn"].ToString().ToNull()
                                        , dtr["PathZhtw"].ToString().ToNull(), PreLang);

                        string tmpPath = info.PathNameBase.Replace("/", "");

                        if (String.IsNullOrEmpty(tmpPath))
                        {
                            info.PathNameBase = dtr["PathBase"].ToString().ToNull();
                        }

                        DateTime dt;
                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                            info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.WriteDateStr = string.Empty;

                        uobj.Add(info);
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

            DAL.FavoriteForm fDal = new DAL.FavoriteForm();
            int listCnt = fDal.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchField, SearchValue, startDate, endDate);

            return listCnt;
        }
        #endregion

        #region SET
        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddList(string p)
        {
            SetDecryptString(p);
            string[] arrID = GetDecryptParam("id", "").Trim('|').Replace("F", "").Split('|');

            DAL.FavoriteForm dal = new DAL.FavoriteForm();
            foreach (string id in arrID)
            {
                if (!id.Equals(""))
                {
                    BE.FavoriteFormInfo info = new BE.FavoriteFormInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.UserID = CurrentUserInfo.UserID;
                    info.FormID = id.ToNumber<int>();

                    dal.Add(info);
                }
            }
            return true;
        }

        /// <summary>
        /// 삭제(다중)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelList(string p)
        {
            SetDecryptString(p);
            string[] arrID = GetDecryptParam("id", "").Trim(',').Split(',');

            DAL.FavoriteForm dal = new DAL.FavoriteForm();
            foreach (string id in arrID)
            {
                if (!id.Equals(""))
                {
                    dal.Del(CurrentUserInfo.CompanyID, id.ToNumber<int>());
                }
            }
            return true;
        }
        #endregion
    }
}
