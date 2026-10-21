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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Home
{
    /// <summary>
    /// Menu의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Menu : Base.BaseWSApproval
    {

        #region InfoBox = 대결자정보 + 결재건수(결재대상건수/참조건수/상신건수/수신건수)

        /// <summary>
        /// 대결자정보
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public BE.ProxyInfo GetProxy(string p)
        {
            try
            {
                SetDecryptString(p);

                DAL.Proxy dal = new DAL.Proxy();
                DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "100", "0");

                BE.ProxyInfo info = new ProxyInfo();
                info.IsProxy = "N";

                DataTable dt = new DataTable();
                dt.Load(dtr);
                DataRow[] drs = dt.Select("IsProxy = 'Y'");
                if (drs != null)
                {
                    foreach (DataRow dr in drs)
                    {
                        info.IsProxy = dr["IsProxy"].ToString();

                        info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                        info.UserID = dr["UserID"].ToString().ToNumber<int>();
                        info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                        info.ProxyID = dr["ProxyID"].ToString().ToNumber<int>();
                        info.ProxyUserID = dr["ProxyUserID"].ToString().ToNumber<int>();
                        info.ProxyDeptID = dr["ProxyDeptID"].ToString().ToNumber<int>();
                        info.ProxyUserName = GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                        info.ProxyDeptName = GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);
                        info.ProxyTitleName = GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                        info.ProxyDutyName = GetLangName(dr["DutyBase"].ToString(), dr["DutyKo"].ToString(), dr["DutyEn"].ToString(), dr["DutyJa"].ToString(), dr["DutyFr"].ToString(), dr["DutyZhcn"].ToString(), dr["DutyZhtw"].ToString(), PreLang);
                        info.ProxyDateStr = "<div class='in_date'>" + DateTime.Parse(dr["ProxyStartDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dr["ProxyStartDate"].ToString()).ToShortTimeString() + " ~<br>" +
                                 DateTime.Parse(dr["ProxyEndDate"].ToString()).ToShortDateString() + " " + DateTime.Parse(dr["ProxyEndDate"].ToString()).ToShortTimeString() + "</div>";
                    }
                }

                return info;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 결재건수
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public CountInfo GetTodayInfo(string p)
        {
            try
            {
                SetDecryptString(p);

                // 데이타처리 - 건수
                DAL.Home dal = new DAL.Home();
                DataTableReader dtr = dal.GetTodayCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);
                DataTable dt = new DataTable();
                dt.Load(dtr);

                CountInfo obj = new CountInfo();
                obj.CntSM = dt.Select("CAT = 'SM'").Length.ToString();     //결재대상문서 건수
                obj.CntCC = dt.Select("CAT = 'CC'").Length.ToString();     //참조받은문서 건수
                obj.CntSC = dt.Select("CAT = 'SC'").Length.ToString();     //상신한문서 건수
                obj.CntCP = dt.Select("CAT = 'CP'").Length.ToString();     //수신받은문서 건수

                return obj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion

        #region 자주사용하는양식

        /// <summary>
        /// 자주사용하는양식
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFavForm(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Home.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            DAL.FavoriteForm fDal = new DAL.FavoriteForm();
            DataTableReader dtr = fDal.GetFavoriteFormList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
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

                        uobj.Add(info);
                    }
                }
            }
            return uobj;
        }

        #endregion


        #region BE

        /// <summary>
        /// 건수 - Info (결재대상건수/참조건수/상신건수/수신건수)
        /// </summary>
        [Serializable]
        public class CountInfo
        {
            public CountInfo() { }

            public string CntSM { get; set; }
            public string CntCC { get; set; }
            public string CntSC { get; set; }
            public string CntCP { get; set; }
        }
        #endregion
    }
}
