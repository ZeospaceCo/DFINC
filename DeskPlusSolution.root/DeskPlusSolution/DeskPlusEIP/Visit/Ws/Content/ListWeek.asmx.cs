using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Data;
using System.IO;

namespace Zeospace.DeskPlus.Visit.Web.Ws.Content
{
    /// <summary>
    /// ListDay의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListWeek : Base.BaseWSVisit
    {

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Ws.Content.ListWeek).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                string tmpdate = GetDecryptParam("tmpdate", "0");             
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                string preLang = CurrentUserInfo.PreferredLanguage;        

                DAL.Reserve dal = new DAL.Reserve();

                DataSet ds = dal.GetWeekList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tmpdate, searchField, searchValue, order);

                List<object> listobj = new List<object>();

                for (int i = 0; i < 7; i++)
                {
                    DateTime weekDate = Convert.ToDateTime(tmpdate).AddDays(i);
                    string tmpWeekName = Convert.ToDateTime(weekDate).Day.ToString().PadLeft(2, '0') + WeekDayName(i);
                    int WeekCount = 0;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DateTime sDate = Convert.ToDateTime(dr["StartDate"]);
                        DateTime eDate = Convert.ToDateTime(dr["EndDate"]);

                        if (CheckTime(weekDate, sDate, eDate))
                        {
                            WeekCount = 1;

                            BE.ReserveInfo info = new BE.ReserveInfo();
                            info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                            info.ReserveID = dr["ReserveID"].ToString().ToNumber<int>();
                            info.ReserveGroupID = dr["ReserveGroupID"].ToString().ToNumber<int>();
                            info.Subject = dr["Subject"].ToString();
                            info.VisitType = dr["VisitType"].ToString();
                            info.Auth = dr["Auth"].ToString();

                            if (info.VisitType == "0")
                            {
                                info.VisitTypeStr = resMng.GetString("Type0");
                            }
                            else if (info.VisitType == "1")
                            {
                                info.VisitTypeStr = resMng.GetString("Type1");
                            }
                            else if (info.VisitType == "2")
                            {
                                info.VisitTypeStr = resMng.GetString("Type2");
                            }

                            info.WriteUserID = dr["WriteUserID"].ToString().ToNumber<int>();

                            info.WriteUserName = this.GetLangName(
                                       dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                                       dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                                       dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                                       dr["NameZhtw"].ToString().ToNull(), PreLang);

                            DateTime dt;

                            /*
                            if (DateTime.TryParse(dr["StartDate"].ToString(), out dt))
                                info.StartDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                info.StartDateStr = string.Empty;

                            if (DateTime.TryParse(dr["EndDate"].ToString(), out dt))
                                info.EndDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                info.EndDateStr = string.Empty;
                            */

                            info.StartDateStr = Convert.ToDateTime(dr["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
                            info.EndDateStr = Convert.ToDateTime(dr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");

                            if (DateTime.TryParse(dr["WriteDate"].ToString(), out dt))
                                info.WriteDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                            else
                                info.WriteDateStr = string.Empty;

                            info.WeekName = tmpWeekName;

                            listobj.Add(info);
                        }
                    }

                    if (WeekCount == 0)
                    {
                        BE.ReserveInfo info = new BE.ReserveInfo();
                        info.ReserveID = 0;
                        info.Subject = resMng.GetString("NoContent");
                        info.VisitTypeStr = "";
                        info.Auth = "N";
                        info.WriteUserID = 0;
                        info.WriteUserName = "";
                        info.StartDateStr = "";
                        info.EndDateStr = "";
                        info.WriteDateStr = "";
                        info.WeekName = tmpWeekName;

                        listobj.Add(info);
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-19
        /// 작성목적 : 요일 정보 가져오기
        /// </summary>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        private string WeekDayName(int _gubun)
        {
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Ws.Content.ListWeek).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            string rtnVal = "";
            switch (_gubun)
            {
                case 0: rtnVal = "(" + resMng.GetString("Sun") + ")"; break;
                case 1: rtnVal = "(" + resMng.GetString("Mon") + ")"; break;
                case 2: rtnVal = "(" + resMng.GetString("Tue") + ")"; break;
                case 3: rtnVal = "(" + resMng.GetString("Wed") + ")"; break;
                case 4: rtnVal = "(" + resMng.GetString("Thu") + ")"; break;
                case 5: rtnVal = "(" + resMng.GetString("Fri") + ")"; break;
                case 6: rtnVal = "(" + resMng.GetString("Sat") + ")"; break;
            };

            return rtnVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_nowDate"></param>
        /// <param name="_sDate"></param>
        /// <param name="_eDate"></param>
        /// <returns></returns>
        private bool CheckTime(DateTime _tmpDate, DateTime _sDate, DateTime _eDate)
        {
            bool rtn = false;

            string tmpDate = _tmpDate.Year + _tmpDate.Month.ToString().PadLeft(2, '0') + _tmpDate.Day.ToString().PadLeft(2, '0');
            string sDate = _sDate.Year + _sDate.Month.ToString().PadLeft(2, '0') + _sDate.Day.ToString().PadLeft(2, '0');
            string eDate = _eDate.Year + _eDate.Month.ToString().PadLeft(2, '0') + _eDate.Day.ToString().PadLeft(2, '0');

            if (sDate.ToNumber<int>() <= tmpDate.ToNumber<int>())
            {
                if (eDate.ToNumber<int>() >= tmpDate.ToNumber<int>())
                {
                    rtn = true;
                }
            }

            return rtn;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string tmpdate = GetDecryptParam("tmpdate", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int icnt = 0;

                DAL.Reserve dal = new DAL.Reserve();
                icnt = dal.GetWeekPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tmpdate, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion GET()

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 승인 / 취소 처리
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModVisitType(string p)
        {
            SetDecryptString(p);
            string visitType = GetDecryptParam("visitType", "0");
            string preLang = GetDecryptParam("preLang", "ko");
            int reserveID = GetDecryptParam("reserveID", "0").ToNumber<int>();

            DAL.Reserve dal = new DAL.Reserve();
            if (dal.Mod(CurrentUserInfo.CompanyID, reserveID, visitType))
            {
                // 알람처리
                BLL.Reserve bll = new BLL.Reserve();
                bll.AlarmVisitType(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, reserveID, visitType, preLang);

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion


    }
}
