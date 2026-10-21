using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Portlet.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Globalization;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class BirthDay : Base.BaseWSPortlet
    {

        public BirthDay()
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.BirthDay).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion
        }
        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 생일,결혼기념일 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBirthDayList(string p)
        {
            try
            {
                SetDecryptString(p);
                
                int listCount = GetDecryptParam("listCount", "0").ToNumber<int>();
                string anniversaryStartMonth = GetDecryptParam("anniversaryStartMonth", "").ToString();
                string anniversaryStartDay = GetDecryptParam("anniversaryStartDay", "").ToString();
                string anniversaryEndMonth = GetDecryptParam("anniversaryEndMonth", "").ToString();
                string anniversaryEndDay = GetDecryptParam("anniversaryEndDay", "").ToString();

                string anniversaryStartDate = string.Empty;
                string anniversaryEndDate = string.Empty;
                string lunarStartDate = string.Empty;
                string lunarEndDate = string.Empty;

                // 선택한 월의 마지막 날짜가 29,30,31 체크하기(음력때문에..)
                // 검색 시작 월
                //DateTime dt = new DateTime(System.DateTime.Now.Year, anniversaryStartMonth.ToNumber<int>() + 1, 1);
                DateTime dt = anniversaryStartMonth == "12" ?
                    new DateTime(System.DateTime.Now.Year + 1, 1, 1) : new DateTime(System.DateTime.Now.Year, anniversaryStartMonth.ToNumber<int>() + 1, 1);

                if (dt.AddDays(-1).Day.ToString().ToNumber<int>() < anniversaryStartDay.ToNumber<int>())
                    anniversaryStartDay = dt.AddDays(-1).Day.ToString();

                // 검색 종료 월
                //dt = new DateTime(System.DateTime.Now.Year, anniversaryEndMonth.ToNumber<int>() + 1, 1);
                dt = anniversaryEndMonth == "12" ?
                    new DateTime(System.DateTime.Now.Year + 1, 1, 1) : new DateTime(System.DateTime.Now.Year, anniversaryEndMonth.ToNumber<int>() + 1, 1);

                if (dt.AddDays(-1).Day.ToString().ToNumber<int>() < anniversaryEndDay.ToNumber<int>())
                    anniversaryEndDay = dt.AddDays(-1).Day.ToString();

                // 년-월-일 날짜 형식으로 만들어주기
                anniversaryStartDate = System.DateTime.Now.Year.ToString() + "-" + anniversaryStartMonth.PadLeft(2, '0') + "-" + anniversaryStartDay.PadLeft(2, '0');
                anniversaryEndDate = System.DateTime.Now.Year.ToString() + "-" + anniversaryEndMonth.PadLeft(2, '0') + "-" + anniversaryEndDay.PadLeft(2, '0');

                // 검색 시작의 음력날짜
                dt = Convert.ToDateTime(anniversaryStartDate);
                lunarStartDate = GetSolartoLunar(dt);

                // 검색 종료의 음력날짜
                dt = Convert.ToDateTime(anniversaryEndDate);
                lunarEndDate = GetSolartoLunar(dt);

                if (anniversaryStartDate.Trim().Length > 0)
                {
                    anniversaryStartDate = DateHelpers.GetCultureDate(anniversaryStartDate, "yyyyMMdd", PreLangReal);
                }

                if (anniversaryEndDate.Trim().Length > 0)
                {
                    anniversaryEndDate = DateHelpers.GetCultureDate(anniversaryEndDate, "yyyyMMdd", PreLangReal);
                }

                if (lunarStartDate.Length > 0)
                {
                    lunarStartDate = DateHelpers.GetCultureDate(lunarStartDate, "yyyyMMdd", PreLangReal);
                }

                if (lunarEndDate.Length > 0)
                {
                    lunarEndDate = DateHelpers.GetCultureDate(lunarEndDate, "yyyyMMdd", PreLangReal);
                }
                
                
                string searchValue = GetDecryptParam("searchValue", "").ToString();

                if (searchValue.Trim().Length > 0)
                {
                    searchValue = " AND ManyName like '%" + searchValue + "%' ";
                }

                DAL.BirthDay biz = new DAL.BirthDay();

                DataTableReader dtr = biz.GetBirthDayList(listCount, CurrentUserInfo.CompanyID, 1, 1, 1,
                                            anniversaryStartDate, anniversaryEndDate, searchValue, lunarStartDate, lunarEndDate);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.BirthDayInfo info = new BirthDayInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.SpecialDay = dtr["SpecialDay"].ToString();

                            if (info.SpecialDay.Trim().Length > 0)
                            {
                                info.SpecialDay = DateHelpers.GetCultureMonthDay(info.SpecialDay, PreLangReal);
                            }
                            else
                            {
                                info.SpecialDay = "";
                            }

                            if (dtr["Gubun"].ToString() == "B") //생일
                            {
                                info.Gubun = "[" + resMng.GetString("Birthday") + "]";
                                info.Lunar = dtr["Lunar"].ToString() == "L" ? "(" + GetResource("Lunar") + ")" : "(" + GetResource("Solar") + ")";
                            }
                            else //결혼
                            {
                                info.Gubun = "[" + resMng.GetString("Marriage") + "]";
                                info.Lunar = "";
                            }

                            #region 이름 다국어 설정
                            info.UserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

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

                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return new List<object>();
                //throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 양력을 음력으로 변환
        /// </summary>
        /// <param name="_dt">The _DT.</param>
        /// <returns></returns>
        private string GetSolartoLunar(DateTime _dt)
        {
            KoreanLunisolarCalendar klc = new KoreanLunisolarCalendar();

            bool leapMonth = false;
            int ileapMonth = 0;

            int lunarYear = klc.GetYear(_dt).ToString().ToNumber<int>();
            int lunarMonth = klc.GetMonth(_dt).ToString().ToNumber<int>();
            int lunarDay = klc.GetDayOfMonth(_dt).ToString().ToNumber<int>();

            if (klc.GetMonthsInYear(lunarYear) > 12)                    // 1년이 12이상이면 윤달
            {
                leapMonth = klc.IsLeapMonth(lunarYear, lunarMonth);     // 윤월인지
                ileapMonth = klc.GetLeapMonth(lunarYear);               // 년도의 윤달이 몇월인지

                if (lunarYear >= ileapMonth)
                    lunarMonth--;
            }

            return lunarYear.ToString() + "-" + (leapMonth ? "*" : "") + lunarMonth.ToString().PadLeft(2, '0') + "-" + lunarDay.ToString().PadLeft(2,'0');
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-08
        /// 작성목적 : ◀▶ 클릭 시 날짜 바인딩
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDateInfo(string p)
        {
            try
            {
                SetDecryptString(p);

                string anniversaryDate = GetDecryptParam("anniversaryDate", "").ToString();
                string scope = GetDecryptParam("scope", "").ToString();

                DateTime sdt = anniversaryDate == "" ? System.DateTime.Now :DateTime.Parse(anniversaryDate);

                if (scope == "M")
                    sdt = sdt.AddDays(-1);
                if (scope == "P")
                    sdt = sdt.AddDays(1);

                if (sdt.ToString().Trim().Length > 0)
                {
                    anniversaryDate = DateHelpers.GetCultureDate(sdt, PreLangReal);
                    anniversaryDate = anniversaryDate.Split(' ')[0];
                }   

                return anniversaryDate;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);                
                throw;
            }
        }
                
    }
}
