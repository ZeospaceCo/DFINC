using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using Zeospace.DeskPlusMobile.Web.Helper;
using BoardData = Zeospace.DeskPlusMobile.Web.Board.Ws.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Collections;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws
{
    /// <summary>
    /// GetBoard의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class GetBoard : Helper.MobileBaseService
    {
        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 게시판 카테코리 가져오기
        /// 작성일자 : 2012-04-20
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFolder()
        {
            List<object> listobj = new List<object>();

            string tmpCount = String.Empty;
            string categoryinfo = String.Empty;
            string cIDList = String.Empty;

            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_SYSCode");
            sqlQuery = String.Format(sqlQuery, CompanyID, "MobileBoard", "SubBoard");
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "UserBoard", null);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cIDList = dr["InterfaceCode"].ToString();
            }

            if (cIDList == "") return listobj;

            string[] arrCID = cIDList.Split(',');

            Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.List biz =
                   new Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.List();

            for (int i = 0; i < arrCID.Length; i++)
            {
                //통합게시판 회사아이디
                int boardComID = BLL.Category.GetCompanyCategoryInfo(Convert.ToInt32(arrCID[i]));
                categoryinfo = biz.GetNoReadList(UserInfo.CompanyID, boardComID, UserInfo.UserID, UserInfo.DeptID, Convert.ToInt32(arrCID[i]), PreLang);

                BE.CategoryInfo info = new BE.CategoryInfo();

                info.CompanyID = boardComID;
                info.CategoryID = Convert.ToInt32(arrCID[i]);
                info.ParentCategoryID = Convert.ToInt32(arrCID[i]);
                info.CateInfo = categoryinfo;

                listobj.Add(info);
            }

            return listobj;
        }

        /// <summary>
        /// 작성자   : 심재운
        /// 작성목적 : 리스트 가져오기
        /// 작성일자 : 2015-03-31
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            List<object> listobj = new List<object>();

            int titleStyle = 1;

            SetDecryptString(p);
            string pageNo = GetDecryptParam("PageNo", "0");
            string pageSize = GetDecryptParam("PageSize", "0");
            string searchValue = GetDecryptParam("SearchValue", "");
            string pCatID = GetDecryptParam("PCatID", "0");

            try
            {
                object[] rowValues = new object[10];

                //통합게시판 회사아이디
                int boardComID = BLL.Category.GetCompanyCategoryInfo(pCatID.ToNumber<int>());

                BLL.Category bll = new Zeospace.DeskPlusMobile.Web.Board.Ws.BLL.Category();

                #region 카테고리 리스트 가져오기

                if (pageNo == "1")
                {
                    BoardData.Category biz = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Category();
                    DataSet ds = biz.GetCategoryByParentCategoryID(boardComID, pCatID.ToNumber<int>());
                    DataView dv = null;

                    //내가 볼 수 있는 카테고리값
                    string authCat = "";
                    if (ds != null)
                    {
                        dv = ds.Tables[0].DefaultView;
                        string isAuthority = "";
                        foreach (DataRow dbRow in dv.Table.Rows)
                        {
                            //해당 카테고리에 모바일용인 것만 보여줄것
                            if (!string.IsNullOrEmpty(dbRow["OptionMobile"].ToString()) && dbRow["OptionMobile"].ToString() == "1")
                            {
                                isAuthority = bll.GetCategoryAuthority(boardComID, Convert.ToInt32(dbRow["CategoryID"].ToString()), UserID, UserInfo.DeptID, "1", false);

                                //해당 카테고리 게시판을 볼 수 있는지 확인
                                if (isAuthority != "")
                                {
                                    //dbRow 에 값에 카테고리 정보가 포함되어 있음, 이를 datatable 로 만들어야 할듯
                                    authCat += dbRow["CategoryID"].ToString() + ",";
                                }
                            }
                        }

                        if (authCat.Length > 0)
                        {
                            dv.RowFilter = "CategoryID in (" + authCat.TrimEnd(',') + ")";
                        }
                        else
                        {
                            //안보이도록 조치
                            dv.RowFilter = "CategoryID in (0)";
                        }
                    }

                    if (dv != null)
                    {
                        for (int i = 0; i < dv.Count; i++)
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.CompanyID = dv[i]["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dv[i]["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = 0;
                            string tmpTitle = this.GetLangName(dv[i]["NameBase"].ToString(), dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(),
                               dv[i]["NameJa"].ToString(), dv[i]["NameFr"].ToString(), dv[i]["NameZhcn"].ToString(), dv[i]["NameZhtw"].ToString(), PreLang);
                            info.Title = tmpTitle;
                            info.ReplyCount = "";
                            info.ViewCount = 0;
                            info.WriteUserInfo = "";
                            info.WriteDateAll = "";
                            info.TitlStyle = titleStyle.ToString();
                            info.OptionTop = "";

                            titleStyle += 1;
                            listobj.Add(info);
                        }
                    }
                }

                #endregion 카테고리 리스트 가져오기

                BE.CategoryInfo be = bll.GetCategoryOptionEtc(boardComID, pCatID.ToNumber<int>(), UserID, UserInfo.DeptID);

                if (be.Auth != "")
                {
                    #region 모바일 사용 카테고리 정보 가져오기

                    Data.List listBiz = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.List();

                    string isAuthority = "";
                    string targetCategory = "";

                    //보기용이라면~
                    if (be.OptionUseView.Equals("1"))
                    {
                        string childCatList = bll.GetChildCategory(boardComID, pCatID.ToNumber<int>());

                        if (childCatList.Length > 0)
                        {
                            foreach (string catID in childCatList.Split(','))
                            {
                                isAuthority = bll.GetCategoryAuthority(boardComID, catID.ToNumber<int>(), UserID, UserInfo.DeptID, "1", false);

                                //해당 카테고리 게시판을 볼 수 있는지 확인
                                if (isAuthority != "")
                                {
                                    targetCategory += catID + ",";
                                }
                            }

                            targetCategory = targetCategory.TrimEnd(',');
                        }
                    }
                    else
                    {
                        //일반 카테고리라면..
                        isAuthority = bll.GetCategoryAuthority(boardComID, pCatID.ToNumber<int>(), UserID, UserInfo.DeptID, "1", false);
                        if (isAuthority != "") targetCategory = pCatID;
                    }

                    #endregion 모바일 사용 카테고리 정보 가져오기

                    this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
                    this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Board.Ws.GetBoard).Assembly;	//어셈블리 선언
                    this.GetResourceManager();

                    #region 게시판 리스트 가져오기

                    DataSet dsList = listBiz.GetBoardList(boardComID, targetCategory, pageNo, pageSize, be.OptionUseDept, UserInfo.DeptID.ToString(), UserID, searchValue);

                    if (dsList != null)
                    {
                        foreach (DataRow dr in dsList.Tables[0].Rows)
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.CompanyID = Convert.ToInt32(dr["CompanyID"].ToString());
                            info.CategoryID = Convert.ToInt32(dr["CategoryID"].ToString());
                            info.ListID = Convert.ToInt32(dr["ListID"].ToString());
                            info.Depth = dr["Depth"].ToString().ToNumber<int>();
                            info.Title = dr["Title"].ToString();
                            info.Title = GetTitle(info, info.Title);

                            //말머리
                            string headerName = this.GetLangName(
                                    dr["HeaderBase"].ToString().ToNull(), dr["HeaderKo"].ToString().ToNull(),
                                    dr["HeaderEn"].ToString().ToNull(), dr["HeaderJa"].ToString().ToNull(),
                                    dr["HeaderFr"].ToString().ToNull(), dr["HeaderZhcn"].ToString().ToNull(),
                                    dr["HeaderZhtw"].ToString().ToNull(), PreLang);
                            info.HeaderName = (headerName.Length > 0) ? "[" + headerName + "]" : "";

                            info.Title = (info.HeaderName.Length > 0) ?
                                info.HeaderName + " " + info.Title : info.Title;

                            info.ReplyCount = (dr["ReplyCount"].ToString() == "0") ? "" : "[" + dr["ReplyCount"].ToString() + "]";
                            info.ViewCount = Convert.ToInt32(dr["ViewCount"]);

                            if (dr["OptionAnonymity"].ToString() == "1") //익명
                            {
                                string AnonymityName = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "AnonymityName");
                                info.WriteUserInfo = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
                            }
                            else
                            {
                                string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                                dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                                string titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                                                dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                                string deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                                                dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                                info.WriteUserInfo = deptName + " " + titleName + " " + name;
                            }

                            info.WriteDateAll = Convert.ToDateTime(dr["WriteDate"]).ToString();
                            info.TitlStyle = titleStyle.ToString();
                            info.OptionTop = (dr["OptionTop"].ToString() == "1") ? resMng.GetString("OptionTop") : "";

                            #region 날짜 비교(New Img)

                            DateTime writeDate = Convert.ToDateTime(dr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                            info.ShowNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";

                            #endregion 날짜 비교(New Img)

                            titleStyle += 1;

                            listobj.Add(info);
                        }
                    }

                    #endregion 게시판 리스트 가져오기
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetList", ex);
                throw;
            }
        }

        /// <summary>
        /// 게시판 제목
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_title">The _title.</param>
        /// <returns></returns>
        private string GetTitle(BE.ListInfo _info, string _title)
        {
            string title = _title;
            string arrowURl = "<img src='/DeskPlusMobile/Content/ko/Base/Img/ico_arrow.gif' title='' />";

            string space = "";
            for (int i = 0; i < _info.Depth; i++) space += "&nbsp;&nbsp;";

            //답변글 일 경우
            if (_info.Depth > 0) title = arrowURl + " " + title;
            if (space.Length > 0) title = space + title;

            return title;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 달력 가지고 오기 
        /// 작성일자 : 2015-06-26
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetBasicCalendar(string p)
        {
            SetDecryptString(p);

            int year = Convert.ToInt32(GetDecryptParam("Year", DateTime.Now.Year.ToString()));
            int month = Convert.ToInt32(GetDecryptParam("Month", DateTime.Now.Month.ToString()));
            int maxPrevDay = 0; // 이전달의 Max 날짜
            int maxDay = 0; //현재달의 Max 날짜
            int startDay = 0; //현재달의 시작 요일
            int realDay = 1; // 실제 날짜
            int nextDay = 1; // 다음날 시작 일자
            int countWeek = 0;

            try
            {
                DateTime dt = new DateTime(year, month, 1);

                startDay = GetCountOfWeek(dt.DayOfWeek); //해당 월의 시작 요일

                DateTime dtReal = new DateTime(year, month, 1); //해당 월 
                DateTime dtPrev = dtReal.AddMonths(-1);  //이전달 Max 날짜 계산
                DateTime dtNext = dtReal.AddMonths(1);  //다음달 날짜 계산

                maxPrevDay = GetDayCountOfMonth(dtPrev.Year, dtPrev.Month); //이전달의 Max 날짜
                maxDay = GetDayCountOfMonth(year, month); //해당 월의 날짜갯수

                List<object> listobj = new List<object>();

                BE.CalendarInfo info;


                for (int i = 0; i < 42; i++)
                {
                    info = new BE.CalendarInfo();

                    //요일 표시
                    info.Week = countWeek.ToString();
                    countWeek += 1;
                    if (countWeek % 7 == 0) countWeek = 0;

                    if (i < startDay) //지난달 표시
                    {
                        info.Year = dtPrev.Year.ToString();
                        info.Month = dtPrev.Month.ToString();
                        info.Day = (maxPrevDay + 1 - startDay + i).ToString();  //해당월의 날짜가 들어간다
                        info.Class = " dim";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " dim_hol";
                        }
                    }
                    else if (i >= startDay && i < maxDay + startDay) // 현재달 표시
                    {
                        info.Year = year.ToString();
                        info.Month = month.ToString();
                        info.Day = realDay.ToString();  //해당월의 날짜가 들어간다
                        realDay += 1;

                        info.Class = "";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " hol";
                        }

                    }
                    else //다음달 표시
                    {
                        info.Year = dtNext.Year.ToString();
                        info.Month = dtNext.Month.ToString();
                        info.Day = nextDay.ToString();  //빈칸들
                        nextDay += 1;

                        info.Class = " dim";

                        //일요일 Style 표시
                        if (info.Week == "0")
                        {
                            info.Class = " dim_hol";
                        }
                    }

                    info.Month = (Convert.ToInt32(info.Month) < 10) ? "0" + info.Month : info.Month;
                    info.Day = (Convert.ToInt32(info.Day) < 10) ? "0" + info.Day : info.Day;

                    string tmpDate = info.Year + "-" + info.Month + "-" + info.Day;
                    info.Func = "Write.setList('" + tmpDate + "')";
                    info.DoCount = "";
                    info.Index = tmpDate;

                    listobj.Add(info);
                }

                /*
                //공휴일 표시
                string[] arSolar = XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "Solar").Split(','); // 양력
                string[] arLunar = XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "Lunar").Split(','); // 음력  

                string targetDay = String.Empty; //공휴일
                string beforeDay = String.Empty; //공휴일 앞으로 몇일 쉴지
                string afterDay = String.Empty;  //공휴일 뒤로 몇일 쉴지
                List<string> Holidayobj = new List<string>();
                //양력 표시
                foreach (string _s in arSolar)
                {
                    if (_s.Length > 0)
                    {
                        if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                        {
                            targetDay = _s.Split('|')[0];
                            beforeDay = _s.Split('|')[1];
                            afterDay = _s.Split('|')[2];
                        }
                        else
                        {
                            targetDay = _s;
                        }

                        DateTime sDate = DateTime.Parse(year + "-" + targetDay);
                        Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));

                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }

                //음력 표시
                foreach (string _s in arLunar)
                {
                    if (_s.Length > 0)
                    {
                        if (_s.IndexOf('|') > 0) // 날짜 | 앞에쉬는날:앞에쉬는날 | 뒤에쉬는날:뒤에쉬는날  ex) 01-01|1|1|  1월 1일 앞뒤로 쉰다면
                        {
                            targetDay = _s.Split('|')[0];
                            beforeDay = _s.Split('|')[1];
                            afterDay = _s.Split('|')[2];
                        }
                        else
                        {
                            targetDay = _s;
                        }

                        DateTime sDate = DateTime.Parse(year + "-" + targetDay);
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                        Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));


                        if (beforeDay.Trim() != "")  //이전 휴일이 있을 경우 
                        {
                            string[] tmpBeforDay = beforeDay.Split(':');
                            foreach (string tmp in tmpBeforDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + tmp)).ToString("yyyy-MM-dd"));
                            }
                        }

                        if (afterDay.Trim() != "")  //이후 휴일이 있을 경우 
                        {
                            string[] tmpAfterDay = afterDay.Split(':');
                            foreach (string tmp in tmpAfterDay)
                            {
                                if (tmp == "") continue;
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(tmp)).ToString("yyyy-MM-dd"));
                            }
                        }
                    }
                }
                */

                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj2 = new List<object>();

                if (context.Cache[CacheName + CompanyID] == null)
                {
                    SetCacheHolidayList(CompanyID, DateTime.Now.ToString("yyyy"));
                }

                listobj2 = (List<object>)context.Cache[CacheName + CompanyID];

                List<string> Holidayobj = new List<string>();
                string tempDate = string.Empty;

                foreach (Pims.Ws.BE.HolidayRInfo _be in listobj2)
                {
                    string SolarLunar = _be.SolarLunar;                               //양음력구분
                    string HOLIDAY = _be.HoliDay;                                     //일자
                    string TITLE = _be.Title;                                         //제목
                    string HolidayBetween = _be.HolidayBetween;                       //연휴구분
                    int HolidayBetweenDay = _be.HolidayBetweenDay;                    //연휴일자
                    string IsReplace = _be.IsReplace;                                 //대체휴가구분
                    int IsReplaceDay = 0;                                             //대체휴가일자
                    string IsHolidayKind = _be.IsHolidayKind;                         //공휴일구분    

                    DateTime sDate = DateTime.Parse(HOLIDAY);

                    //양력음력구분
                    if (SolarLunar != "0")
                    {
                        CalendarHelper calHelper = new CalendarHelper();
                        sDate = DateTime.Parse(calHelper.FromLunToSol(sDate)); //음력을 양력으로 변환
                    }

                    //대체휴일 확인
                    if (IsReplace == "1")
                    {
                        for (int i = 0; i <= HolidayBetweenDay; i++)
                        {
                            if (HolidayBetween == "1" || HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    if (i > 0)
                                    {
                                        IsReplaceDay = IsReplaceDay + 1;
                                    }
                                }
                            }

                            if (HolidayBetween == "2" || HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }
                    }

                    //대체휴일 + 토 확인
                    if (IsReplace == "2")
                    {
                        for (int i = 0; i <= HolidayBetweenDay; i++)
                        {
                            if (HolidayBetween == "0")
                            {
                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }
                                else if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }

                            if (HolidayBetween == "3")
                            {
                                if (sDate.AddDays(Convert.ToDouble("-" + i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) > 0)
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Saturday)
                                {
                                    IsReplaceDay = IsReplaceDay + 2;
                                }

                                if (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek == DayOfWeek.Sunday || (sDate.AddDays(Convert.ToDouble(i)).DayOfWeek != DayOfWeek.Saturday && tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) > 0))
                                {
                                    IsReplaceDay = IsReplaceDay + 1;
                                }
                            }
                        }
                    }

                    //일자 저장
                    Holidayobj.Add(sDate.ToString("yyyy-MM-dd"));

                    //비교 데이터 저장
                    if (IsHolidayKind == "0")
                    {
                        tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                    }

                    if (HolidayBetween == "1")
                    {
                        //이전일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }
                    else if (HolidayBetween == "2")
                    {
                        //다음일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "0")
                            {
                                tempDate = tempDate + sDate.ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }
                    else if (HolidayBetween == "3")
                    {
                        //이전다음일 포함
                        for (int i = 1; i <= HolidayBetweenDay; i++)
                        {
                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "1")
                            {
                                tempDate = tempDate + sDate.AddDays(Convert.ToDouble("-" + i)).ToString("yyyy-MM-dd") + "|";
                            }

                            if (tempDate.IndexOf(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd")) < 1)
                            {
                                Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                            }

                            if (IsHolidayKind == "1")
                            {
                                tempDate = tempDate + sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd") + "|";
                            }
                        }
                    }

                    if (IsReplaceDay > 0)
                    {
                        int IsReplaceDayStart = HolidayBetweenDay + 1;
                        int IsReplaceDayEnd = IsReplaceDay + HolidayBetweenDay;

                        for (int i = IsReplaceDayStart; i <= IsReplaceDayEnd; i++)
                        {
                            Holidayobj.Add(sDate.AddDays(Convert.ToDouble(i)).ToString("yyyy-MM-dd"));
                        }
                    }
                }

                string Holiday = string.Join(",", Holidayobj);

                //달력에 일정 및 공휴일 표시 
                foreach (BE.CalendarInfo calInfo in listobj)
                {
                    // 공휴일 
                    string tmpMD = calInfo.Month.PadLeft(2, '0') + "-" + calInfo.Day.PadLeft(2, '0');
                    string tmpHol = String.Empty;
                    foreach (string hol in Holiday.Split(','))
                    {
                        tmpHol = hol.Replace(year.ToString() + "-", "");
                        if (tmpMD == tmpHol)
                        {
                            if (calInfo.Month.ToNumber<int>() == month)
                            {
                                calInfo.Class = " hol";
                            }
                            else
                            {
                                calInfo.Class = " dim_hol";
                            }
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetBasicCalendar()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 휴일 캐쉬에 담기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public void SetCacheHolidayList(int _companyID, string _UserSelectedYear)
        {
            try
            {
                string CacheName = "HolidayList_";

                HttpContext context = HttpContext.Current;

                List<object> listobj = new List<object>();

                if (context.Cache[CacheName + _companyID] == null)
                {
                    SqlParameter[] parm = new SqlParameter[]{
                                new SqlParameter("@CompanyID", SqlDbType.Int)
                            ,new SqlParameter("@SelectYear", SqlDbType.Int)
                    };
                    parm[0].Value = CompanyID;
                    parm[1].Value = _UserSelectedYear.ToNumber<int>();

                    string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPim, "SEL_PROC_HolidayR_SelByYear");
                    sql = string.Format(sql, _companyID, _UserSelectedYear.ToNumber<int>());
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsYear", parm);

                    foreach (DataRow dtr in ds.Tables[0].Rows)
                    {

                        Pims.Ws.BE.HolidayRInfo _data = new Pims.Ws.BE.HolidayRInfo();

                        _data.SolarLunar = dtr["SolarLunar"].ToString();                               //양음력구분
                        _data.HoliDay = dtr["HOLIDAY"].ToString();                                     //일자
                        _data.Title = dtr["TITLE"].ToString();                                         //제목
                        _data.HolidayBetween = dtr["HolidayBetween"].ToString();                       //연휴구분
                        _data.HolidayBetweenDay = dtr["HolidayBetweenDay"].ToString().ToNumber<int>(); //연휴일자
                        _data.IsReplace = dtr["IsReplace"].ToString();                                 //대체휴가구분
                        _data.IsReplaceDay = 0;                                                        //대체휴가일자
                        _data.IsHolidayKind = dtr["IsHolidayKind"].ToString();                         //공휴일구분        

                        listobj.Add(_data);
                    }

                    HttpContext ctx = HttpContext.Current;
                    ctx.Cache.Insert(CacheName + _companyID, listobj, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("SetCacheHolidayList : " + ex);
                throw;
            }
        }

        /// <summary>
        ///  요일을 숫자료 변환
        /// </summary>
        /// <param name="_dow"></param>
        /// <returns></returns>
        private int GetCountOfWeek(DayOfWeek _dow)
        {
            int rtn = 0;

            switch (_dow)
            {
                case DayOfWeek.Sunday: rtn = 0; break;
                case DayOfWeek.Monday: rtn = 1; break;
                case DayOfWeek.Tuesday: rtn = 2; break;
                case DayOfWeek.Wednesday: rtn = 3; break;
                case DayOfWeek.Thursday: rtn = 4; break;
                case DayOfWeek.Friday: rtn = 5; break;
                case DayOfWeek.Saturday: rtn = 6; break;
            }

            return rtn;
        }

        /// <summary>
        /// 월별 날짜수 계산
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        private int GetDayCountOfMonth(int year, int month)
        {
            int leapYear = 0;
            int rtn = 0;

            //윤년 체크
            if ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0)) leapYear = 1;

            switch (month)
            {
                case 1: rtn = 31; break;
                case 2: rtn = 28 + leapYear; break;
                case 3: rtn = 31; break;
                case 4: rtn = 30; break;
                case 5: rtn = 31; break;
                case 6: rtn = 30; break;
                case 7: rtn = 31; break;
                case 8: rtn = 31; break;
                case 9: rtn = 30; break;
                case 10: rtn = 31; break;
                case 11: rtn = 30; break;
                case 12: rtn = 31; break;
                default: rtn = -1; break;
            }
            return rtn;
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성목적 : 게시판 첨부파일 가져오기
        /// 작성일자 : 2015-06-29
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetSpoolList(string p)
        {
            SetDecryptString(p);
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "0");

            try
            {
                string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/" + swfUploadGUID + "/");

                List<object> listobj = new List<object>();

                DirectoryInfo DirInfo = new DirectoryInfo(uploadPath);

                var files = new DirectoryInfo(uploadPath).EnumerateFiles().OrderBy(f => f.LastWriteTime);

                string sourceFile = "";

                foreach (var item in files)
                {
                    if (item.Name != "Thumbs.db")
                    {
                        sourceFile = uploadPath + item.Name;
                        FileInfo sourceFileInfo = new FileInfo(sourceFile);

                        BE.AttachInfo info = new BE.AttachInfo();
                        info.CompanyID = CompanyID;
                        info.FileName = sourceFileInfo.Name;
                        info.FileType = sourceFileInfo.Extension;

                        if (HasScreenShot(info.FileType))
                        {
                            info.ImageType = "1";
                        }
                        else
                        {
                            info.ImageType = "0";
                        }

                        listobj.Add(info);
                    }
                }


                return listobj;
            }
            catch (SqlException ex)
            {
                Helper.AppException.WriteException("GetSpoolList", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-07-05
        /// 작성목적 : 이미지 파일인지 구분
        /// </summary>
        /// <returns></returns>
        private bool HasScreenShot(string _fileType)
        {
            bool fileOK = false;

            string[] allowedExtensions = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "ImgFileExtension").Split(',');

            for (int i = 0; i < allowedExtensions.Length; i++)
            {
                if (_fileType.ToLower() == allowedExtensions[i])
                {
                    fileOK = true;
                    break;
                }
            }

            return fileOK;
        }

        /// <summary>
        /// 작성자   : 김형석
        /// 작성목적 : 댓글 카운트 가져오기
        /// 작성일자 : 2018-10-16
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetShortReplyCount(string p)
        {
            List<object> listobj = new List<object>();

            SetDecryptString(p);
            int categoryID = GetDecryptParam("CID", "0").ToNumber<int>();
            int listID = GetDecryptParam("ID", "0").ToNumber<int>();

            try
            {
                int icnt = 0;

                //통합게시판 회사아이디
                int boardComID = BLL.Category.GetCompanyCategoryInfo(categoryID);

                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply bizShortReply =
                  new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply();

                icnt = bizShortReply.GetShortReplyCount(boardComID, categoryID, listID);

                return icnt;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetShortReplyCount", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : 김형석
        /// 작성목적 : 댓글 리스트 가져오기
        /// 작성일자 : 2018-10-16
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShortReplyList(string p)
        {
            List<object> listobj = new List<object>();

            SetDecryptString(p);
            int categoryID = GetDecryptParam("CID", "0").ToNumber<int>();
            int listID = GetDecryptParam("ID", "0").ToNumber<int>();
            string optionShortReplyGood = GetDecryptParam("optionShortReplyGood", "0");
            string optionShortReplyTop = GetDecryptParam("optionShortReplyTop", "0");

            try
            {
                //통합게시판 회사아이디
                int boardComID = BLL.Category.GetCompanyCategoryInfo(categoryID);

                Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply bizShortReply =
                  new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.ShortReply();

                DataSet dsList = bizShortReply.GetShortReply(boardComID, categoryID, listID);

                ArrayList bestList = new ArrayList();

                // 댓글 추천/비추천 사용유무
                if (optionShortReplyGood == "1")
                {
                    #region 베스트댓글

                    Hashtable ht = new Hashtable();
                    int rowCount = 0;

                    if (dsList != null)
                    {
                        foreach (DataRow dr in dsList.Tables[0].Rows)
                        {
                            int goodCount = dr["GoodCount"].ToString().ToNumber<int>() - dr["BadCount"].ToString().ToNumber<int>();
                            ht.Add(dr["ShortReplyID"].ToString().ToNumber<int>(), goodCount);

                            rowCount++;
                        }
                    }

                    // 추천갯수-비추갯수 값이 가장 높은것이 베스트댓글
                    // 동률일 경우 모두 베스트댓글, 정렬 순서는 작성시간 순
                    // 댓글의 댓글이 베스트댓글인 경우 BEST 표시만
                    int MaxValue = 0;
                    foreach (DictionaryEntry de in ht)
                    {
                        if (MaxValue <= (Int32)de.Value)
                        {
                            MaxValue = (Int32)de.Value;
                        }
                    }

                    // 추천/비추값이 아무것도 없을 경우 베스트댓글은 없음
                    // 베스트댓글은 2건 이상의 댓글이 달렸을 경우에만
                    if (MaxValue > 0 && rowCount >= 2)
                    {
                        foreach (DictionaryEntry de in ht)
                        {
                            if (MaxValue == (Int32)de.Value)
                            {
                                bestList.Add((Int32)de.Key);
                            }
                        }
                    }

                    // 추천 댓글을 최상위에 보여주기 사용유무
                    if (optionShortReplyTop == "1")
                    {
                        if (dsList != null)
                        {
                            foreach (DataRow dr in dsList.Tables[0].Rows)
                            {
                                foreach (Int32 id in bestList)
                                {
                                    if (dr["ShortReplyID"].ToString().ToNumber<int>() == id && dr["Active"].ToString() == "1" && dr["Depth"].ToString().ToNumber<int>() == 0)
                                    {
                                        BE.ShortReplyInfo info = new BE.ShortReplyInfo();

                                        info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                                        info.CategoryID = dr["CategoryID"].ToString().ToNumber<int>();
                                        info.ListID = dr["ListID"].ToString().ToNumber<int>();
                                        info.ShortReplyID = dr["ShortReplyID"].ToString().ToNumber<int>();
                                        info.Content = dr["Content"].ToString().ToReverseXSS().Replace("\n", "<br>");
                                        info.UserID = dr["UserID"].ToString().ToNumber<int>();

                                        string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                                                   dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                                        string titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                                                        dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                                        string deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                                                        dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                                        info.UserName = deptName + " " + titleName + " " + name;

                                        info.IsAuth = (info.UserID == UserID) ? "Y" : "N";

                                        info.InputDateStr = Convert.ToDateTime(dr["InputDate"]).ToString();

                                        #region 날짜 비교(New Img)
                                        DateTime writeDate = Convert.ToDateTime(dr["InputDate"].ToString());
                                        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                                        TimeSpan gap = todayDate.Subtract(writeDate);
                                        int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                                        info.ShowNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";
                                        #endregion 날짜 비교(New Img)

                                        info.GroupID = dr["GroupID"].ToString().ToNumber<int>();
                                        info.Depth = dr["Depth"].ToString().ToNumber<int>();

                                        info.DepthDivStart = GetDivStart(info);

                                        info.Active = dr["Active"].ToString();
                                        info.ParentCnt = bizShortReply.GetParentCount(boardComID, categoryID, listID, dr["GroupID"].ToString().ToNumber<int>(), dr["ShortReplyID"].ToString().ToNumber<int>());
                                        info.IsAuth = (info.UserID == UserID) ? "Y" : "N";
                                        info.GoodCount = dr["GoodCount"].ToString().ToNumber<int>();
                                        info.BadCount = dr["BadCount"].ToString().ToNumber<int>();
                                        info.IsBest = "1";
                                        info.IsTop = "1";

                                        listobj.Add(info);
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                }

                #region 댓글리스트

                if (dsList != null)
                {
                    foreach (DataRow dr in dsList.Tables[0].Rows)
                    {
                        BE.ShortReplyInfo info = new BE.ShortReplyInfo();

                        info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                        info.CategoryID = dr["CategoryID"].ToString().ToNumber<int>();
                        info.ListID = dr["ListID"].ToString().ToNumber<int>();
                        info.ShortReplyID = dr["ShortReplyID"].ToString().ToNumber<int>();
                        info.Content = dr["Content"].ToString().ToReverseXSS().Replace("\n", "<br>");
                        info.UserID = dr["UserID"].ToString().ToNumber<int>();

                        string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                                   dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);
                        string titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                                        dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(), dr["TitleZhtw"].ToString(), PreLang);
                        string deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                                        dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(), dr["DeptZhtw"].ToString(), PreLang);

                        info.UserName = deptName + " " + titleName + " " + name;
                        info.IsAuth = (info.UserID == UserID) ? "Y" : "N";

                        info.InputDateStr = Convert.ToDateTime(dr["InputDate"]).ToString();

                        #region 날짜 비교(New Img)
                        DateTime writeDate = Convert.ToDateTime(dr["InputDate"].ToString());
                        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        TimeSpan gap = todayDate.Subtract(writeDate);
                        int NewShowDay = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "NewShowDay").ToNumber<int>();

                        info.ShowNew = (gap.Days <= NewShowDay) ? "<img src='/DeskPlusMobile/Content/ko/Base/Img/i_new.gif' title='' />" : "";
                        #endregion 날짜 비교(New Img)

                        info.GroupID = dr["GroupID"].ToString().ToNumber<int>();
                        info.Depth = dr["Depth"].ToString().ToNumber<int>();

                        info.DepthDivStart = GetDivStart(info);

                        info.Active = dr["Active"].ToString();
                        info.ParentCnt = bizShortReply.GetParentCount(boardComID, categoryID, listID, dr["GroupID"].ToString().ToNumber<int>(), dr["ShortReplyID"].ToString().ToNumber<int>());
                        info.IsAuth = (info.UserID == UserID) ? "Y" : "N";
                        info.GoodCount = dr["GoodCount"].ToString().ToNumber<int>();
                        info.BadCount = dr["BadCount"].ToString().ToNumber<int>();

                        foreach (Int32 id in bestList)
                        {
                            if (dr["ShortReplyID"].ToString().ToNumber<int>() == id)
                            {
                                info.IsBest = "1";
                            }
                        }

                        listobj.Add(info);
                    }
                }

                #endregion

                return listobj;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetShortReplyList", ex);
                throw;
            }
        }

        /// <summary>
        /// 게시판 제목 스타일 입히기
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_title">The _title.</param>
        /// <param name="_etc2">The _etc2.</param>
        /// <returns></returns>
        private string GetDivStart(BE.ShortReplyInfo _info)
        {
            int gap = 8;
            for (int i = 0; i <= _info.Depth; i++)
            {
                if (i > 1)
                {
                    gap = gap + 17;
                }
            }

            string space = "<div class='title' style=\"padding-left: " + gap + "px\">";

            return space;
        }

        #region 미사용

        ///// <summary>
        ///// 메서드명   :
        ///// 작성목적   : 해당 카테고리 트리에 바인딩할 권한이 있는지 체크
        ///// 작성자     : famo23
        ///// 최초작성일 : 2011-07-12
        ///// </summary>
        ///// <param name="dbRow">The db row.</param>
        ///// <param name="isAuthority">The is authority.</param>
        ///// <returns></returns>
        //private bool CheckCategoryAuth(DataRow dbRow, string isAuthority)
        //{
        //    bool response = false;
        //    //권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
        //    //string isAuthority = IsAutority(Convert.ToInt32(dbRow["CategoryID"]));

        //    //일반 게시판일 경우
        //    if (dbRow["OptionUseDept"].ToString().Equals("0"))
        //    {
        //        //권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한
        //        string[] isAuthorityStr = isAuthority.Split('/');

        //        //권한이 있을 경우
        //        if (Convert.ToBoolean(isAuthorityStr[0].ToString()))
        //        {
        //            //읽기 권한 - 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
        //            if (Convert.ToInt32(isAuthorityStr[1].ToString()) > 0)
        //            {
        //                response = true;
        //            }
        //        }
        //    }
        //    else //부서별 게시판일 경우 : 권한 상관이 볼 수 있음
        //    {
        //        response = true;
        //    }

        //    return response;
        //}

        ///// <summary>
        ///// 작성자   : famo23
        ///// 작성일자 : 2011-07-12
        ///// 작성목적 : 게시판 권한테이블에서 회사,부서,개인 아이디값 가져오기
        ///// 수정일자 :
        ///// 수정내용 :
        ///// </summary>
        ///// <param name="_categoryID">The _category ID.</param>
        ///// <returns></returns>
        //private string GetMyAutority(int _categoryID)
        //{
        //    //통합게시판 회사아이디
        //    int boardComID = BLL.Category.GetCompanyCategoryInfo(_categoryID);

        //    authority = "";

        //    //1. 게시판 카테고리에 해당된 회사,부서,개인 아이디값 가져오기
        //    BoardData.Authority data = new Zeospace.DeskPlusMobile.Web.Board.Ws.Data.Authority();
        //    DataSet ds = new DataSet();
        //    ds = data.GetAuthority(boardComID, _categoryID);

        //    DataView dv = ds.Tables[0].DefaultView;

        //    string deptMemberIDs = "";
        //    string authValue = "";
        //    //string tempReadWrite = "";
        //    string[] tempReadWriteStr = null;
        //    bool authcheck = false;
        //    int order = 0;

        //    Zeospace.DeskPlusMobile.Web.Account.Data.Dept dalDept = new Account.Data.Dept();
        //    StringBuilder tempReadWriteSb = new StringBuilder();

        //    /*
        //     *
        //     * 1. 개인권한이 1순위
        //     * 2. 부서권한이 2순위
        //     * 3. 회사권한이 3순위
        //     */

        //    for (int i = 0; i < dv.Count; i++)
        //    {
        //        authcheck = false;

        //        //1. 개인아이디값이 권한테이블에 있는지 체크
        //        if (dv[i]["Type"].ToString().Equals("M"))
        //        {
        //            if (UserID.ToString().Equals(dv[i]["OwnerID"].ToString()))
        //            {
        //                order = 1;
        //                authcheck = true;
        //                authValue = dv[i]["Authority"].ToString();
        //            }
        //        }

        //        //2. 개인아이디가 없으면, 부서아이디로 권한을 체크한다.
        //        if (authcheck == false)
        //        {
        //            if (dv[i]["Type"].ToString() == "G" && dv[i]["OwnerID"].ToString() != "0") //부서로 권한이 할당되어 있는 경우, 재귀함수로 하위디렉토리 검색해서 개인아이디가 있는지 확인
        //            {
        //                //해당 부서의 값으로 하위 디렉토리를 재귀함수 돌릴 경우, 부서아이디와 맴버아이이가 콤마로 분류되어 있음(무지하게 많음)
        //                //부서아이디를 넣어서 맴버를 뽑아온다.

        //                #region 부서코드로 회사 정보 가져오기

        //                DataSet dsDept = dalDept.GetDept(Convert.ToInt32(dv[i]["OwnerID"]));
        //                if (dsDept != null)
        //                {
        //                    if (dsDept.Tables[0].Rows.Count > 0)
        //                    {
        //                        foreach (DataRow drDept in dsDept.Tables[0].Rows)
        //                        {
        //                            deptMemberIDs = GetAuthorityDeptMemeber(drDept["CompanyID"].ToString().ToNumber<int>(), Convert.ToInt32(dv[i]["OwnerID"])).ToString();
        //                            if (deptMemberIDs.Length > 0) break;
        //                        }
        //                    }
        //                }

        //                #endregion 부서코드로 회사 정보 가져오기

        //                if (deptMemberIDs.Length > 0)
        //                {
        //                    deptMemberIDs = "," + deptMemberIDs + ",";
        //                    if (deptMemberIDs.IndexOf("," + UserID.ToString() + ",") > 0)
        //                    {
        //                        order = 2;
        //                        //나의 아이디값이 들어 있다면
        //                        authcheck = true;
        //                        authValue = dv[i]["Authority"].ToString();
        //                    }
        //                }
        //            }
        //        }

        //        //3. 둘다 없으면 , 회사아이디로 찾는다.
        //        if (authcheck == false)
        //        {
        //            //회사 아이디는 0으로 저장되어 있음
        //            if (dv[i]["Type"].ToString() == "G" && dv[i]["OwnerID"].ToString() == "0")
        //            {
        //                order = 3;
        //                authcheck = true;
        //                authValue = dv[i]["Authority"].ToString();
        //            }
        //        }

        //        if (authcheck)
        //        {
        //            //임시 저장 = 권한값 + 권한순서
        //            tempReadWriteSb.Append(order + ":" + authValue + ",");
        //        }
        //    }

        //    tempReadWriteStr = tempReadWriteSb.ToString().TrimEnd(',').Split(',');

        //    StringBuilder sb = new StringBuilder();

        //    //권한값이 있을 경우
        //    if (tempReadWriteSb.ToString().Length > 0)
        //    {
        //        //hash table 보다 느리지만, 동일한 키값이 포함될 수 있기 때문에 사용함!!!
        //        NameValueCollection nvc = new NameValueCollection();
        //        nvc.Clear();

        //        //1:읽기, 2:쓰기, 3: 읽기+꼬리말쓰기, 4:읽기+쓰기+꼬리말쓰기
        //        for (int i = 0; i < tempReadWriteStr.Length; i++)
        //        {
        //            // 0 : 권한값, 1 : 권한순서
        //            string[] modeOrderStr = tempReadWriteStr[i].ToString().Split(':');
        //            nvc.Add(modeOrderStr[0].ToString(), modeOrderStr[1].ToString());
        //        }

        //        //1순위 (사람)
        //        if (nvc["1"] != "" && nvc["1"] != null)
        //        {
        //            if (nvc["1"].IndexOf(",") > 0) //사람이 다중일 경우 콤마로 나옴
        //            {
        //                string[] nvcStr = nvc["1"].Split(',');
        //                sb.Append("true/" + nvcStr[0].ToString());
        //            }
        //            else
        //            {
        //                sb.Append("true/" + nvc["1"]);
        //            }
        //        } //2순위 (부서)
        //        else if (nvc["2"] != "" && nvc["2"] != null)
        //        {
        //            if (nvc["2"].IndexOf(",") > 0)
        //            {
        //                string[] nvcStr = nvc["2"].Split(',');
        //                sb.Append("true/" + nvcStr[0].ToString());
        //            }
        //            else
        //            {
        //                sb.Append("true/" + nvc["2"]);
        //            }
        //        } //3순위 (회사)
        //        else if (nvc["3"] != "" && nvc["3"] != null)
        //        {
        //            if (nvc["3"].IndexOf(",") > 0)
        //            {
        //                string[] nvcStr = nvc["3"].Split(',');
        //                sb.Append("true/" + nvcStr[0].ToString());
        //            }
        //            else
        //            {
        //                sb.Append("true/" + nvc["3"]);
        //            }
        //        }
        //    }

        //    if (sb.ToString() == "")
        //    {
        //        sb.Append("false/");
        //    }

        //    return sb.ToString();
        //}

        ///// <summary>
        ///// 작성자   : famo23
        ///// 작성일자 : 2011-07-12
        ///// 작성목적 : Gets the authority dept memeber.
        ///// 수정일자 :
        ///// 수정내용 :
        ///// </summary>
        ///// <param name="_companyID">The _company identifier.</param>
        ///// <param name="_ownerID">The _owner ID.</param>
        ///// <returns></returns>
        //private string GetAuthorityDeptMemeber(int _companyID, int _ownerID)
        //{
        //    authority = ""; //하위부서 및 사람들 가져옴, 재호출시 clear 해야 함

        //    DataSet ds = new DataSet();

        //    if (dsGetDeptUserTree != null)
        //    {
        //        ds = dsGetDeptUserTree;
        //    }
        //    else
        //    {
        //        Account.Data.Dept dept = new Zeospace.DeskPlusMobile.Web.Account.Data.Dept();
        //        ds = dept.GetDeptUserTree(_companyID);
        //        ds.Relations.Add("DeptRelation", ds.Tables[0].Columns["OwnerID"], ds.Tables[0].Columns["ParentOwnerID"], false);
        //        //ds.WriteXml("C:/wmpub/love.xml");

        //        dsGetDeptUserTree = ds;
        //    }

        //    //dataset of tvData 을 foreach 함
        //    foreach (DataRow dbRow in ds.Tables[0].Rows)
        //    {
        //        //나의 부서와 맞은 하나의 부서의 하위부서들을 재귀함수로 돌려서 viewstate 에 담는다.
        //        //부서는 G다음에 아이디값, 맴버는 아이디만 표기
        //        if (dbRow["OwnerID"].ToString() == "G" + _ownerID.ToString())
        //        {
        //            authority += dbRow["OwnerID"].ToString();
        //            PopulateSubNode(dbRow);
        //        }
        //    }

        //    return authority.ToString();
        //}

        ///// <summary>
        ///// 작성자   : famo23
        ///// 작성일자 : 2011-07-12
        ///// 작성목적 : 하위 부서 재귀함수
        ///// 수정일자 :
        ///// 수정내용 :
        ///// </summary>
        ///// <param name="_dbRow">The _DB row.</param>
        //private void PopulateSubNode(DataRow _dbRow)
        //{
        //    //자식값을 가져오기
        //    foreach (DataRow childRow in _dbRow.GetChildRows("DeptRelation"))
        //    {
        //        authority += "," + childRow["OwnerID"].ToString();
        //        //자식값을 재귀함수 이용해서 추출해 오기
        //        PopulateSubNode(childRow);
        //    }
        //}

        #endregion 미사용
    }
}