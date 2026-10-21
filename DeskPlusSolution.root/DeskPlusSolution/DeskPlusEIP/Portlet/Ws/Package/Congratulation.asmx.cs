using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.Package
{
    /// <summary>
    /// Album의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Congratulation : Base.BaseWSPortlet
    {
        #region 변수

        #endregion 변수

        /// <summary>
        /// 생일자, 결혼기념일자 리스트
        /// 심재운 (생일은 양음력 처리, 결혼일자는 양력만), 기간은 금일 + 7일 기준.(7일 이외라면 소스 변경 필요)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.CongratulationInfo> GetCongratulationList(string p)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.Package.Congratulation).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            #endregion 다국어 리소스 로드/Title 정보

            SetDecryptString(p);

            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator

            try
            {
                string cacheName = "BirthDay";
                HttpContext context = HttpContext.Current;

                List<BE.CongratulationInfo> listobj = (List<BE.CongratulationInfo>)context.Cache[cacheName];

                //관리자나 생일일자 객체 값이 없으면 다시 읽어온다.
                if (listobj == null || isRoleAdmin)
                {
                    listobj = new List<BE.CongratulationInfo>();

                    Zeospace.DeskPlus.Portlet.DAL.Congratulation dal = new Zeospace.DeskPlus.Portlet.DAL.Congratulation();
                    DataSet ds = dal.GetCongratulation(CurrentUserInfo.CompanyID);

                    //******** 일주일간 (지난 생일은 생략~)
                    string startDate = System.DateTime.Now.ToString("MMdd");//월일
                    string endDate = System.DateTime.Now.AddDays(7).ToString("MMdd");//월일

                    //string startDate = "1229";
                    //string endDate = "0105";

                    if (startDate.ToNumber<int>() < endDate.ToNumber<int>())
                    {
                        var congList = GetCongratulation(ds, startDate, endDate);
                        foreach (var info in congList) listobj.Add(info);
                    }
                    else
                    {
                        /* 7일 기간
                         * 12-25 ~ 01-01,
                         * 12-26 ~ 01-02,
                         * 12-27 ~ 01-03,
                         * 12-28 ~ 01-04,
                         * 12-29 ~ 01-05,
                         * 12-30 ~ 01-06,
                         * 12-31 ~ 01-07
                         */

                        //시작일자 ~ 12월 30일까지 먼저 범위체크하여 돌린다.
                        var congList = GetCongratulation(ds, startDate, "1231");
                        foreach (var info in congList) listobj.Add(info);

                        List<BE.CongratulationInfo> tmpobj = new List<BE.CongratulationInfo>();

                        //내년 날짜는 경우에 따라 구현한다. + 7일 경우
                        switch (startDate)
                        {
                            case "1225":
                                tmpobj = GetCongratulation(ds, "0101", "0101");
                                foreach (var info in tmpobj) listobj.Add(info);
                                break;

                            case "1226":
                                tmpobj = GetCongratulation(ds, "0101", "0102");
                                foreach (var info in tmpobj) listobj.Add(info);
                                break;

                            case "1227":
                                tmpobj = GetCongratulation(ds, "0101", "0103");
                                foreach (var info in tmpobj) listobj.Add(info);
                                break;

                            case "1228":
                                tmpobj = GetCongratulation(ds, "0101", "0104");
                                foreach (var info in tmpobj) listobj.Add(info);
                                break;

                            case "1229":
                                tmpobj = GetCongratulation(ds, "0101", "0105");
                                foreach (var info in tmpobj) listobj.Add(info);
                                break;

                            case "1230":
                                tmpobj = GetCongratulation(ds, "0101", "0106");
                                foreach (var info in tmpobj) listobj.Add(info);
                                break;

                            case "1231":
                                tmpobj = GetCongratulation(ds, "0101", "0107");
                                foreach (var info in tmpobj) listobj.Add(info);
                                break;
                        }
                    }

                    //캐쉬 하루 저장
                    Context.Cache.Insert(cacheName, listobj, null, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration);
                }

                //일반 사용자는 캐쉬에 저장된 생일일자를 빨리 보여준다.
                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LIst[GetCongratulationList] : " + ex.Message);
                return new List<BE.CongratulationInfo>();
                //throw;
            }
        }

        /// <summary>
        /// 범위 생일, 축하 가져오기
        /// 심재운
        /// </summary>
        /// <param name="_ds"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        private List<BE.CongratulationInfo> GetCongratulation(DataSet _ds, string _startDate, string _endDate)
        {
            List<BE.CongratulationInfo> listobj = new List<BE.CongratulationInfo>();

            #region 양력생일 구하기

            DataView dvBirthSolar = _ds.Tables[0].DefaultView;
            dvBirthSolar.RowFilter = "Num='B' and Lunar='S' and (congratulation >= '" + _startDate + "' and congratulation <= '" + _endDate + "') ";

            foreach (DataRowView dtr in dvBirthSolar)
            {
                BE.CongratulationInfo info = new BE.CongratulationInfo();

                info.GuBun = dtr["Num"].ToString().ToNull();
                info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();

                info.DeptBase = this.GetLangName(
                            dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                            dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                            dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                            dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                info.NameBase = this.GetLangName(
                        dtr["NameBase"].ToString(),
                        dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                        dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                        PreLang);

                info.TitleBase = this.GetLangName(
                            dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                            dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                            dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                            dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                info.PhotoImage = LoadPhotoSeal(dtr["PhotoImage"].ToString().ToNull(), info.UserID);

                info.BirthDay = dtr["BirthDay"].ToString();

                info.WeddingAnniversary = "";

                if (info.BirthDay.Length == 8)
                {
                    DateTime dt;

                    DateTime tmpSolor = new DateTime(
                        info.BirthDay.Substring(0, 4).ToNumber<int>(),
                        info.BirthDay.Substring(4, 2).ToNumber<int>(),
                        info.BirthDay.Substring(6, 2).ToNumber<int>()
                        );

                    if (DateTime.TryParse(tmpSolor.ToString(), out dt))
                        //info.BirthDay = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        //info.BirthDay = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(5);
                        info.BirthDay = DateMMdd(dt);
                    else
                        info.BirthDay = string.Empty;
                }

                #region 금일과 동일할 경우, todayimage 설정한다.
                string toDay = DateMMdd(System.DateTime.Now);
                if (info.BirthDay == toDay) info.TodayImage = "B"; 
                #endregion

                listobj.Add(info);
            }

            #endregion 양력생일 구하기

            #region 음력생일 구하기

            DataView dvLunar = _ds.Tables[0].DefaultView;
            dvLunar.RowFilter = "Num='B' and Lunar='L'";

            CalendarHelper calHelper = new CalendarHelper();

            string lunarY = "";
            string lunarM = "";
            string lunarD = "";
            bool response = false;

            foreach (DataRowView dtr in dvLunar)
            {
                BE.CongratulationInfo info = new BE.CongratulationInfo();

                info.GuBun = dtr["Num"].ToString().ToNull();
                info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();

                info.DeptBase = this.GetLangName(
                            dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                            dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                            dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                            dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                info.NameBase = this.GetLangName(
                        dtr["NameBase"].ToString(),
                        dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                        dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                        PreLang);

                info.TitleBase = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                info.PhotoImage = LoadPhotoSeal(dtr["PhotoImage"].ToString().ToNull(), info.UserID);

                info.BirthDay = "";
                info.WeddingAnniversary = "";
                response = false;
                DateTime dt;

                info.BirthDay = dtr["BirthDay"].ToString().ToNull();
                lunarY = info.BirthDay.Substring(0, 4);
                lunarM = info.BirthDay.Substring(4, 2);
                lunarD = info.BirthDay.Substring(6, 2);

                //년도는 금년도 기준으로 음력으로 하여 양력을 계산한다.
                DateTime tmpLunar = new DateTime(System.DateTime.Now.Year, lunarM.ToNumber<int>(), lunarD.ToNumber<int>());
                DateTime tmpSolor = DateTime.Parse(calHelper.FromLunToSol(tmpLunar)); //음력을 양력으로 변환

                //날짜 범위에 포함되면 객체 추가한다.
                if (_startDate.ToNumber<int>() <= tmpSolor.ToString("MMdd").ToNumber<int>() &&
                    _endDate.ToNumber<int>() >= tmpSolor.ToString("MMdd").ToNumber<int>() && System.DateTime.Now.Year == tmpSolor.ToString("yyyy").ToNumber<int>()) response = true;

                #region 금일과 동일할 경우, todayimage 설정한다.
                info.TodayImage = "";
                if (DateTime.TryParse(tmpSolor.ToString(), out dt))
                {
                    info.BirthDay = DateMMdd(dt) + "(" + this.GetResource("Lunar") + ")";
                    string toDay = DateMMdd(System.DateTime.Now);
                    if (DateMMdd(dt) == toDay) info.TodayImage = "B";
                }
                else info.BirthDay = string.Empty; 
                #endregion

                //날짜 범위 내 일경우, 처리하기
                if (response) listobj.Add(info);
            }

            #endregion 음력생일 구하기

            #region 결혼기념일 구하기

            DataView dvWeddSolar = _ds.Tables[0].DefaultView;
            dvWeddSolar.RowFilter = "Num = 'W' and (congratulation >= '" + _startDate + "' and congratulation <= '" + _endDate + "') ";

            foreach (DataRowView dtr in dvWeddSolar)
            {
                BE.CongratulationInfo info = new BE.CongratulationInfo();

                info.GuBun = dtr["Num"].ToString().ToNull();
                info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();

                info.DeptBase = this.GetLangName(
                            dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                            dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                            dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                            dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                info.NameBase = this.GetLangName(
                        dtr["NameBase"].ToString(),
                        dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                        dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                        PreLang);

                info.TitleBase = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                info.PhotoImage = LoadPhotoSeal(dtr["PhotoImage"].ToString().ToNull(), info.UserID);

                info.BirthDay = "";
                info.WeddingAnniversary = dtr["WeddingAnniversary"].ToString();

                if (info.WeddingAnniversary.Length == 8)
                {
                    DateTime dt;

                    DateTime tmpSolor = new DateTime(
                        info.WeddingAnniversary.Substring(0, 4).ToNumber<int>(),
                        info.WeddingAnniversary.Substring(4, 2).ToNumber<int>(),
                        info.WeddingAnniversary.Substring(6, 2).ToNumber<int>()
                        );

                    if (DateTime.TryParse(tmpSolor.ToString(), out dt))
                        info.WeddingAnniversary = DateMMdd(dt);
                    else
                        info.WeddingAnniversary = string.Empty;
                }

                #region 금일과 동일할 경우, todayimage 설정한다.
                string toDay = DateMMdd(System.DateTime.Now);
                if (info.WeddingAnniversary == toDay) info.TodayImage = "W"; 
                #endregion

                listobj.Add(info);
            }

            #endregion 결혼기념일 구하기

            return listobj;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 사진 이미지 가져오기
        /// </summary>
        /// <param name="_photo">The _photo.</param>
        /// <param name="_seal">The _seal.</param>
        public string LoadPhotoSeal(string _photo, int _userid)
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            #region 이미지 경로 암호화

            string images = this.BaseImgPath + "/Icon/ico_noimg.gif";
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", images);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

            #endregion 이미지 경로 암호화

            string photoImage = "<img src='" + images + "' width='120' height='90' alt='photo' title='photo' />";

            images = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + _userid + "/Picture/" + _photo;

            if (System.IO.File.Exists(Server.MapPath(images)))
            {
                #region 이미지 경로 암호화

                paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", images);
                encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                #endregion 이미지 경로 암호화

                photoImage = "<img src='" + images + "' width='120' height='90' alt='' title='' />";
            }

            return photoImage;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 옵션설정에 맞게 옵션 스타일 적용하기
        /// </summary>
        /// <param name="_title">The _title.</param>
        /// <param name="_etc2">The _etc2.</param>
        /// <returns></returns>
        private string SetTitleOptionEtc(string _title, string _etc2)
        {
            try
            {
                if (_etc2.ToNull().Length > 0)
                {
                    string[] etcStr = _etc2.Split(',');
                    StringBuilder sb = new StringBuilder();
                    if (etcStr[2].ToString() != "0") sb.Append("<b>");
                    if (etcStr[0].ToString() != "") sb.Append("<font color='" + etcStr[0].ToString() + "'>");
                    if (etcStr[1].ToString() != "") sb.Append("<font size='" + etcStr[1].ToString() + "'>");
                    sb.Append(_title);
                    if (etcStr[0].ToString() != "") sb.Append("</font>");
                    if (etcStr[1].ToString() != "") sb.Append("</font>");
                    if (etcStr[2].ToString() != "0") sb.Append("</b>");

                    _title = sb.ToString();

                    return _title;
                }

                return _title;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
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
        /// 날짜 : 다국어 별로 형태 보여주기(월,일)
        /// 작성일 : 심재운
        /// </summary>
        /// <param name="_dt">The _DT.</param>
        /// <returns></returns>
        private string DateMMdd(DateTime _dt)
        {
            string response = "";

            //날짜는 PreLangReal 사용해야 함.
            string east = "ko,ja,zh-cn,zh-tw"; //한국,일본,중국,대만
            string west = "en,fr,en-gb";//미국,프랑스,유럽

            if (east.IndexOf(PreLangReal.ToLower()) != -1)
            {
                response = DateHelpers.GetCultureDate(_dt, PreLangReal).Trim();//동양은 앞에 년도/(5자리)
                response = response.Substring(5, 5);
            }
            else if (west.IndexOf(PreLangReal.ToLower()) != -1)
            {
                response = DateHelpers.GetCultureDate(_dt, PreLangReal).Trim();//서양은 뒤에 /년도(5자리)
                response = response.Substring(0, response.Length - 5);//뒤에서 5자리 제거
            }

            return response;
        }
    }
}