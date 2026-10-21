using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Proposal.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Proposal.Web.Ws.Content
{
    /// <summary>
    /// Statistics의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Statistics : Base.BaseWSProposal
    {

        /// <summary>
        /// 선택한 년,월의 최대 제안자 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetMaxUserInfo(string p)
        {
            try
            {
                SetDecryptString(p);

                string maxYear = GetDecryptParam("maxYear", System.DateTime.Now.Year.ToString());
                string maxMonth = GetDecryptParam("maxMonth", System.DateTime.Now.Month.ToString().PadLeft(2, '0'));

                maxMonth = maxMonth.Length == 1 ? maxMonth.PadLeft(2, '0') : maxMonth;

                string yearMonth = maxYear + maxMonth;

                DAL.List biz = new DAL.List();
                DataTableReader dtr = biz.GetMaxProposerInfo(CurrentUserInfo.CompanyID, yearMonth);

                string name = string.Empty;
                string userID = string.Empty;
                string count = string.Empty;
                string result = string.Empty;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            // 이름
                            name = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                PreLang);
                            // 아이디
                            userID = dtr["UserID"].ToString();
                            // 건수
                            count = dtr["MaxCount"].ToString();
                        }

                        result = name + "," + userID + "," + count;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 선택한 년,월의 사업부별 제안현황 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.StaticList> GetDeptInfo(string p)
        {
            try 
            {
                SetDecryptString(p);

                string maxYear = GetDecryptParam("maxYear", System.DateTime.Now.Year.ToString());
                string maxMonth = GetDecryptParam("maxMonth", System.DateTime.Now.Month.ToString().PadLeft(2, '0'));

                maxMonth = maxMonth.Length == 1 ? maxMonth.PadLeft(2, '0') : maxMonth;

                string yearMonth = maxYear + maxMonth;

                DAL.List biz = new DAL.List();
                List<Ws.Content.StaticList> alList = new List<StaticList>();

                DataTableReader dtr = biz.GetDeptInfo(CurrentUserInfo.CompanyID, yearMonth);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        string percentage = string.Empty;
                        while (dtr.Read())
                        {
                            Ws.Content.StaticList info = new StaticList();

                            // 본부이름(경영,반도체, 시스템, 마이크로)
                            info.DeptID = dtr["Bul_Select"].ToString();
                            // 건수
                            info.Count = dtr["MaxCount"].ToString();
                            // 비율
                            percentage = FormatPercent(dtr["Per"].ToString(), 2);
                            // 100%일 경우 테이블 사이즈가 변경되서 수정함
                            info.Percentage = percentage == "100" ? "85" : percentage;

                            alList.Add(info);
                        }
                    }
                }
                return alList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 선택한 년,월의 우수제안자 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.StaticList> GetMaxScoreInfo(string p)
        {
            try
            {
                SetDecryptString(p);

                string maxYear = GetDecryptParam("maxYear", System.DateTime.Now.Year.ToString());
                string maxMonth = GetDecryptParam("maxMonth", System.DateTime.Now.Month.ToString().PadLeft(2, '0'));

                maxMonth = maxMonth.Length == 1 ? maxMonth.PadLeft(2, '0') : maxMonth;

                string yearMonth = maxYear + maxMonth;

                DAL.List biz = new DAL.List();
                List<Ws.Content.StaticList> alList = new List<StaticList>();

                DataTableReader dtr = biz.GetMaxScoreInfo(CurrentUserInfo.CompanyID, yearMonth);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.Content.StaticList info = new StaticList();

                            // 작성자 이름
                            info.UserName = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                PreLang);
                            // 작성자 UserID
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            // 제목
                            info.Title = dtr["Title"].ToString();
                            // 개선안
                            info.AfterCont = dtr["AfterCont"].ToString();

                            alList.Add(info);
                        }
                    }
                }
                return alList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 선택한 년,월의 사업부별 제안현황 정보 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.StaticList> GetYearInfo(string p)
        {
            try
            {
                SetDecryptString(p);

                string maxYear = GetDecryptParam("maxYear", System.DateTime.Now.Year.ToString());

                DAL.List biz = new DAL.List();
                List<Ws.Content.StaticList> alList = new List<StaticList>();

                DataTableReader dtr = biz.GetYearInfo(CurrentUserInfo.CompanyID, maxYear);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.Content.StaticList info = new StaticList();

                            // 월
                            info.Months = dtr["Months"].ToString();
                            // 건수
                            info.Count = dtr["MaxCount"].ToString();
                            // 비율
                            info.Percentage = FormatPercent(dtr["Per"].ToString(), 2);

                            alList.Add(info);
                        }
                    }
                }
                return alList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 자리수처리 (소숫점 2자리이하, 버림)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public string FormatPercent(string s, int len)
        {
            string[] sa = s.Split('.');
            if (sa.Length.Equals(2))
            {
                sa[1] = sa[1].Length == 2 ? sa[1] : sa[1].ToString() + "0";
                return sa[0] + "." + sa[1].Substring(0, len);
            }
            else
            {
                return s;
            }
        }
    }

    [Serializable]
    public class StaticList
    {
        public StaticList() { }

        public string DeptID { get; set; }
        public string Count { get; set; }
        public string Percentage { get; set; }
        public string Months { get; set; }

        public int UserID { get; set; }
        public string UserName {get;set;}
        public string Title {get;set;}
        public string AfterCont {get;set;}
    }
}
