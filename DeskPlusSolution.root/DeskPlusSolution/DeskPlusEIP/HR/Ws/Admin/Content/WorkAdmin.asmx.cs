using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.HR.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.HR.Web.Ws.Admin.Content
{
    /// <summary>
    /// WorkAdmin의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class WorkAdmin : Base.BaseWSHR
    {
        #region 리스트

        /// <summary>
        /// 유연근무 대상 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetWorkEtcList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			    //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Ws.Admin.Content.WorkAdmin).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);

            DAL.WorkEtc aDal = new DAL.WorkEtc();
            DataTableReader dtr = aDal.GetWorkEtcList(CurrentUserInfo.CompanyID);

            List<object> obj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.WorkEtcInfo info = new BE.WorkEtcInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.WorkEtcID = dtr["WorkEtcID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                        #region 부서 다국어 설정
                        info.DeptName = this.GetLangName(
                                dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 직급 다국어 설정
                        info.TitleName = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 이름 다국어 설정
                        info.UserName = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        info.WSTimeStr = Convert.ToDateTime(dtr["WSTime"].ToString()).ToShortTimeString();
                        info.WCTimeStr = Convert.ToDateTime(dtr["WCTime"].ToString()).ToShortTimeString();

                        //요일
                        if (dtr["WeekDays"].ToString() != "")
                        {
                            string tmpWeek = string.Empty;
                            string yoil = string.Empty;
                            foreach (string s in dtr["WeekDays"].ToString().TrimEnd(',').Split(','))
                            {
                                yoil = string.Empty;
                                if (s == "2") { yoil = "월"; }
                                if (s == "3") { yoil = "화"; }
                                if (s == "4") { yoil = "수"; }
                                if (s == "5") { yoil = "목"; }
                                if (s == "6") { yoil = "금"; }

                                tmpWeek += yoil + ", ";
                            }

                            info.WeekDays = tmpWeek.TrimEnd(' ').TrimEnd(',');
                        }

                        info.Types = (dtr["Types"].ToString() == "GM") ? resMng.GetString("User") : resMng.GetString("Dept");

                        obj.Add(info);
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// 작성자 : kkzz52
        /// 목적 : IP 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<IPInfo> GetIPList(string p)
        {
            string rtnVal = String.Empty;

            //파라미터 가져오기
            SetDecryptString(p);

            int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();

            try
            {
                DAL.IP dal = new DAL.IP();
                List<BE.IPInfo> al = dal.GetIP(companyID);

                foreach (IPInfo info in al)
                {
                    if (info.StartIP.Trim().Length > 0)
                    {
                        info.StartIP1 = info.StartIP.Substring(0, 3).ToNumber<int>().ToString();
                        info.StartIP2 = info.StartIP.Substring(3, 3).ToNumber<int>().ToString();
                        info.StartIP3 = info.StartIP.Substring(6, 3).ToNumber<int>().ToString();
                        info.StartIP4 = info.StartIP.Substring(9, 3).ToNumber<int>().ToString();
                    }

                    if (info.EndIP.Trim().Length > 0)
                    {
                        info.EndIP1 = info.EndIP.Substring(0, 3).ToNumber<int>().ToString();
                        info.EndIP2 = info.EndIP.Substring(3, 3).ToNumber<int>().ToString();
                        info.EndIP3 = info.EndIP.Substring(6, 3).ToNumber<int>().ToString();
                        info.EndIP4 = info.EndIP.Substring(9, 3).ToNumber<int>().ToString();
                    }
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetIPList : " + ex);
                return null;
            }
        }

        #endregion

        #region Set

        /// <summary>
        /// 유연근무 대상 추가
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool InsWorkEtc(string p)
        {
            SetDecryptString(p);
            string arrUnitID = GetDecryptParam("arrID", "").Trim(',');
            string wsTime = GetDecryptParam("wsTime", "").Trim(',');
            string wctime = GetDecryptParam("wctime", "").Trim(',');
            string searchWeek = GetDecryptParam("searchWeek", "");

            DateTime tmpWSTime = new DateTime(2000, 1, 1, (Convert.ToDateTime(wsTime)).Hour, (Convert.ToDateTime(wsTime)).Minute, 0);
            DateTime tmpWCTime = new DateTime(2000, 1, 1, (Convert.ToDateTime(wctime)).Hour, (Convert.ToDateTime(wctime)).Minute, 0);

            string tmpWeek = string.Empty;
            if (searchWeek.Length > 0)
            {
                foreach (string s in searchWeek.TrimEnd(',').Split(','))
                {
                    tmpWeek += s + ",";
                }
                tmpWeek = tmpWeek.TrimEnd(',');
            }

            BLL.WorkEtc aBLL = new BLL.WorkEtc();
            return aBLL.Add(CurrentUserInfo.CompanyID, arrUnitID, tmpWSTime, tmpWCTime, tmpWeek);
        }
        /// <summary>
        /// 유연근무 대상 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelWorkEtc(string p)
        {
            SetDecryptString(p);
            int workEtcID = GetDecryptParam("workEtcID", "0").ToNumber<int>();

            BLL.WorkEtc aBLL = new BLL.WorkEtc();
            return aBLL.Del(CurrentUserInfo.CompanyID, workEtcID);
        }

        /// <summary>
        /// ip 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelIP(string p)
        {

            try
            {
                //파라미터 가져오기
                SetDecryptString(p);

                int companyID = GetDecryptParam("companyID", "0").ToNumber<int>();
                int IPID = GetDecryptParam("IPID", "0").ToNumber<int>();

                if (CurrentUserInfo.IsSysAdmin || CurrentUserInfo.IsInRole("admin") || CurrentUserInfo.IsInRole("HR"))
                {
                    DAL.IP dal = new DAL.IP();
                    dal.Del(companyID, IPID);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelIP : " + ex);
                return false;
            }
        }

        #endregion
    }
}
