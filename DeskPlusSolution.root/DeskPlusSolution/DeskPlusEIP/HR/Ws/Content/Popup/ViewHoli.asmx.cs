using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.HR.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.HR.Web.Ws.Content.Popup
{
    /// <summary>
    /// ViewHoli의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ViewHoli : Base.BaseWSHR
    {
        #region 리스트

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-10
        /// 작성목적 : 페이징
        /// </summary>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-20
        /// 작성목적 : 출퇴근내역관리 리스트 카운트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetHoliListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string pathID = GetDecryptParam("pathID", "");                  //부서경로
                string deptBase = GetDecryptParam("deptBase", "");              //부서명
                string startDate = GetDecryptParam("startDate", "");            //시작일자
                string endDate = GetDecryptParam("endDate", "");                //종료일자
                string minclosingID = GetDecryptParam("minClosingID", "");
                string maxclosingID = GetDecryptParam("maxClosingID", "");

                startDate = DateHelper.GetCultureDate(startDate, "yyyy-MM-dd", PreLangReal);
                endDate = DateHelper.GetCultureDate(endDate, "yyyy-MM-dd", PreLangReal);

                int icnt = 0;

                DAL.Holiday biz = new DAL.Holiday();
                icnt = biz.GetHoliListCount(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), deptID.ToNumber<int>(), pathID, startDate, endDate, minclosingID.ToNumber<int>(), maxclosingID.ToNumber<int>());

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2018-11-20
        /// 작성목적 : 출퇴근내역관리 리스트
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetHoliList(string p)
        {
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			        //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Ws.Content.Popup.ViewHoli).Assembly;	    //어셈블리 선언
                this.GetResourceManager();

                SetDecryptString(p);
                string userID = GetDecryptParam("userID", "");                  //아이디
                string deptID = GetDecryptParam("deptID", "");                  //부서
                string pathID = GetDecryptParam("pathID", "");                  //부서경로
                string deptBase = GetDecryptParam("deptBase", "");              //부서명
                string startDate = GetDecryptParam("startDate", "");            //시작일자
                string endDate = GetDecryptParam("endDate", "");                //종료일자
                string minclosingID = GetDecryptParam("minClosingID", "");
                string maxclosingID = GetDecryptParam("maxClosingID", "");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                startDate = DateHelper.GetCultureDate(startDate, "yyyy-MM-dd", PreLangReal);
                endDate = DateHelper.GetCultureDate(endDate, "yyyy-MM-dd", PreLangReal);

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.Holiday biz = new DAL.Holiday();

                DataTableReader dtr = biz.GetHoliList(CurrentUserInfo.CompanyID,
                    userID.ToNumber<int>(), deptID.ToNumber<int>(), pathID, startDate, endDate, setPageingStr[0], setPageingStr[1], minclosingID.ToNumber<int>(), maxclosingID.ToNumber<int>());
                ObjList listobj = new ObjList();
                
                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.HoliUsedInfo info = new ETC.BE.HoliUsedInfo();

                            DAL.Closing dalclo = new DAL.Closing();

                            info.ROW = dtr["ROW"].ToString();

                            info.DeptID = dtr["DeptID"].ToString().ToNumber<int>();

                            info.DeptName = deptBase;

                            DataSet ds = dalclo.GetClosingUserInfoMax(CurrentUserInfo.CompanyID, dtr["UserID"].ToString().ToNumber<int>(), info.DeptID, dtr["PathID"].ToString(), info.DeptName);
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                info.TitleName = dr["TitleBase"].ToString();
                                info.NameBase = dr["NameBase"].ToString();
                            }

                            /*
                            // 겸직자면 (겸) 표시
                            int basedept = dalclo.GetClosingBaseDeptCount(CurrentUserInfo.CompanyID, dtr["UserID"].ToString().ToNumber<int>(), info.DeptID, dtr["PathID"].ToString(), info.DeptName);
                            if (basedept > 0)
                            {
                                info.NameBase = info.NameBase + "(" + this.resMng.GetString("BaseDept") + ")";
                            }
                            */                             

                            info.DocumentID = dtr["DocumentID"].ToString();                            

                            DateTime dtS;
                            if (DateTime.TryParse(dtr["StartDate"].ToString(), out dtS))
                            {
                                info.StartDate = DateHelpers.GetCultureDate(dtS, PreLangReal).Substring(0, 10);

                                DateTime sDate = DateTime.Parse(info.StartDate);

                                if (sDate.DayOfWeek == DayOfWeek.Monday) { info.StartDate = info.StartDate + " (" + resMng.GetString("Day_Monday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Tuesday) { info.StartDate = info.StartDate + " (" + resMng.GetString("Day_Tuesday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Wednesday) { info.StartDate = info.StartDate + " (" + resMng.GetString("Day_Wednesday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Thursday) { info.StartDate = info.StartDate + " (" + resMng.GetString("Day_Thursday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Friday) { info.StartDate = info.StartDate + " (" + resMng.GetString("Day_Friday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Saturday) { info.StartDate = info.StartDate + " (" + resMng.GetString("Day_Saturday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Sunday) { info.StartDate = info.StartDate + " (" + resMng.GetString("Day_Sunday") + ")"; }
                            }
                            else
                            {
                                info.StartDate = string.Empty;
                            }

                            DateTime dtE;
                            if (DateTime.TryParse(dtr["EndDate"].ToString(), out dtE))
                            {
                                info.EndDate = DateHelpers.GetCultureDate(dtE, PreLangReal).Substring(0, 10);

                                DateTime sDate = DateTime.Parse(info.EndDate);

                                if (sDate.DayOfWeek == DayOfWeek.Monday) { info.EndDate = info.EndDate + " (" + resMng.GetString("Day_Monday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Tuesday) { info.EndDate = info.EndDate + " (" + resMng.GetString("Day_Tuesday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Wednesday) { info.EndDate = info.EndDate + " (" + resMng.GetString("Day_Wednesday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Thursday) { info.EndDate = info.EndDate + " (" + resMng.GetString("Day_Thursday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Friday) { info.EndDate = info.EndDate + " (" + resMng.GetString("Day_Friday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Saturday) { info.EndDate = info.EndDate + " (" + resMng.GetString("Day_Saturday") + ")"; }
                                if (sDate.DayOfWeek == DayOfWeek.Sunday) { info.EndDate = info.EndDate + " (" + resMng.GetString("Day_Sunday") + ")"; }
                            }
                            else
                            {
                                info.EndDate = string.Empty;
                            }

                            info.Duration = dtr["Duration"].ToString();
                            info.StartHalf = dtr["StartHalf"].ToString();
                            info.EndHalf = dtr["EndHalf"].ToString();
                            info.Gubun = dtr["Gubun"].ToString();

                            listobj.Add(info);
                        }
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
        /// 날짜 변환
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private string SetDate(string _date)
        {
            string tmpVal = String.Empty;
            DateTime dt = Convert.ToDateTime(_date);
            switch (CurrentUserInfo.PreferredLanguage)
            {
                case "ko":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "en":
                    tmpVal = dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "en-gb":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "ja":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "fr":
                    tmpVal = dt.Day.ToString().PadLeft(2, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Year.ToString();
                    break;

                case "zh-cn":
                    tmpVal = dt.Year.ToString() + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
                    break;

                case "zh-tw":
                    tmpVal = dt.Year.ToString() + "-" + dt.Month.ToString().PadLeft(2, '0') + "-" + dt.Day.ToString().PadLeft(2, '0');
                    break;
            }

            return tmpVal;
        }
        #endregion
    }
}
