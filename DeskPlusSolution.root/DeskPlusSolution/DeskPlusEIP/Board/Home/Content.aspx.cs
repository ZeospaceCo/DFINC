using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Board.Web.Home
{
    public partial class Content : Base.BasePageBoard
    {
        #region Page Process (페이지 처리)

        //금년도 최다 작성자
        public List<MaxWriterInfo> GetMaxWriter { get; set; }

        //금년도 최다 댓글자
        public List<MaxShortReplyerInfo> GetMaxShortReplyer { get; set; }

        //최신글
        public List<BE.ListViewModel> GetRecentWrite { get; set; }

        //등록한 게시플
        public List<BE.ListViewModel> GetMyWrite { get; set; }

        //메뉴별 최신 글 여부
        public NewMenuEntity GetNewMenu { get; set; }

        //막대그래프
        public MyBarChartEntity GetMyBarChart { get; set; }

        //최다 작성자의 이름
        public string GetMaxWriterName { get; set; }

        //최다 작성자의 개수
        public string GetMaxWriterCnt { get; set; }

        public int StartTab = 0;


        //막대그래프 표기시, 최대값 알아보기
        public int MyMaxVal { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Home.Content).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 심재운
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            NewMenu();

            MaxWriter();
            MaxShortReplyer();
            RecentWrite();
            //MyWrite();
            StartUserTab();
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                //cbtnAdmin.Text = this.resMng.GetString("Administrator");
                //cbtnAdd.Text = this.resMng.GetString("Search");
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 메뉴별 new 여부
        /// </summary>
        private void NewMenu()
        {
            GetNewMenu = new NewMenuEntity();
            GetMyBarChart = new MyBarChartEntity();

            BLL.HomeList biz = new BLL.HomeList();
            var alList = biz.GetNewTab(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            GetNewMenu.RecentWrite = (alList[0] > 0) ? true : false;

            //통계는 new 개념이 이상함... 최신글이 올라오는것도 아니고 ㅡㅡ;
            GetNewMenu.MaxShort = false;
            GetNewMenu.MaxRead = false;
            GetNewMenu.MaxRate = false;

            GetNewMenu.MyWrite = (alList[1] > 0) ? true : false;
            GetNewMenu.MyListByShort = (alList[2] > 0) ? true : false;
            GetNewMenu.MyShort = (alList[3] > 0) ? true : false;
            GetNewMenu.MyImpo = (alList[4] > 0) ? true : false;

            #region 막대그래프용

            //막대그래프용
            GetMyBarChart.MyWriteCnt = alList[5]; //작성글 개수
            GetMyBarChart.MyListByShortCnt = alList[6]; //댓글단 게시판 개수
            GetMyBarChart.MyShortCnt = alList[7]; //댓글 개수

            var items = new List<int>()
            {
                GetMyBarChart.MyWriteCnt,
                GetMyBarChart.MyListByShortCnt,
                GetMyBarChart.MyShortCnt
            };

            GetMyBarChart.ChartMaxValue = items.Max();

            if (GetMyBarChart.ChartMaxValue <= 100)
            {
                GetMyBarChart.ChartMaxValue = 100;
                GetMyBarChart.ChartIncre = 20;
            }
            else
            {
                //10 단위 올림
                GetMyBarChart.ChartMaxValue = ((int)Math.Ceiling(GetMyBarChart.ChartMaxValue / 10.0)) * 10;
                GetMyBarChart.ChartIncre = GetMyBarChart.ChartMaxValue / 5;
            }

            #endregion 막대그래프용
        }

        /// <summary>
        /// 작성자  : 심재운
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            //FavoriteID = GetDecryptParam("favoriteID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 금년도 최다작성자
        /// </summary>
        /// <returns></returns>
        private List<MaxWriterInfo> MaxWriter()
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            string imgPath = "";
            string encParams = "";
            Dictionary<string, string> paraDic = null;

            BLL.HomeList biz = new BLL.HomeList();
            GetMaxWriter = biz.GetMaxWriter(CurrentUserInfo.CompanyID, 3);
            int i = 0;
            if (GetMaxWriter.Count > 0)
            {
                foreach (var info in GetMaxWriter)
                {
                    GetMaxWriter[i].Name = this.GetLangName(
                        info.NameBase.ToReverseXSS(),
                        info.NameKo.ToReverseXSS(), info.NameEn.ToReverseXSS(), info.NameJa.ToReverseXSS(),
                        info.NameFr.ToReverseXSS(), info.NameZhcn.ToReverseXSS(), info.NameZhtw.ToReverseXSS(), PreLang);

                    GetMaxWriter[i].DeptName = this.GetLangName(
                        info.DeptBase.ToReverseXSS(),
                        info.DeptKo.ToReverseXSS(), info.DeptEn.ToReverseXSS(), info.DeptJa.ToReverseXSS(),
                        info.DeptFr.ToReverseXSS(), info.DeptZhcn.ToReverseXSS(), info.DeptZhtw.ToReverseXSS(), PreLang);

                    GetMaxWriter[i].TitleName = this.GetLangName(
                        info.TitleBase.ToReverseXSS(),
                        info.TitleKo.ToReverseXSS(), info.TitleEn.ToReverseXSS(), info.TitleJa.ToReverseXSS(),
                        info.TitleFr.ToReverseXSS(), info.TitleZhcn.ToReverseXSS(), info.TitleZhtw.ToReverseXSS(), PreLang);

                    GetMaxWriter[i].DutyName = this.GetLangName(
                        info.DutyBase.ToReverseXSS(),
                        info.DutyKo.ToReverseXSS(), info.DutyEn.ToReverseXSS(), info.DutyJa.ToReverseXSS(),
                        info.DutyFr.ToReverseXSS(), info.DutyZhcn.ToReverseXSS(), info.DutyZhtw.ToReverseXSS(), PreLang);

                    GetMaxWriterName += "'" + GetMaxWriter[i].Name + "',";
                    GetMaxWriterCnt += GetMaxWriter[i].Cnt.ToString() + ",";

                    imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + info.PhotoImage;
                    imgPath = string.Format(imgPath, CurrentUserInfo.CompanyID, info.UserID);

                    if (System.IO.File.Exists(Server.MapPath(imgPath)) == false)
                        imgPath = this.BaseImgPath + "/Icon/ico_noimg.gif";

                    if (System.IO.File.Exists(Server.MapPath(imgPath)))
                    {
                        #region 스크린샷

                        paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", imgPath);
                        encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                        imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                        #endregion 스크린샷

                        imgPath = string.Format("<img src='{0}' width='49' height='49' />", imgPath);
                        GetMaxWriter[i].PhotoImage = imgPath;
                    }

                    i += 1;
                }

                GetMaxWriterName = GetMaxWriterName.TrimEnd(',');
                GetMaxWriterCnt = GetMaxWriterCnt.TrimEnd(','); 
            }            

            return GetMaxWriter;
        }

        /// <summary>
        /// 금년도 최다 댓글자
        /// </summary>
        /// <returns></returns>
        private List<MaxShortReplyerInfo> MaxShortReplyer()
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            string imgPath = "";
            string encParams = "";
            Dictionary<string, string> paraDic = null;

            BLL.HomeList biz = new BLL.HomeList();
            GetMaxShortReplyer = biz.GetMaxShorter(CurrentUserInfo.CompanyID, 5);
            int i = 0;
            foreach (var info in GetMaxShortReplyer)
            {
                GetMaxShortReplyer[i].Name = this.GetLangName(
                    info.NameBase.ToReverseXSS(),
                    info.NameKo.ToReverseXSS(), info.NameEn.ToReverseXSS(), info.NameJa.ToReverseXSS(),
                    info.NameFr.ToReverseXSS(), info.NameZhcn.ToReverseXSS(), info.NameZhtw.ToReverseXSS(), PreLang);

                GetMaxShortReplyer[i].DeptName = this.GetLangName(
                    info.DeptBase.ToReverseXSS(),
                    info.DeptKo.ToReverseXSS(), info.DeptEn.ToReverseXSS(), info.DeptJa.ToReverseXSS(),
                    info.DeptFr.ToReverseXSS(), info.DeptZhcn.ToReverseXSS(), info.DeptZhtw.ToReverseXSS(), PreLang);

                GetMaxShortReplyer[i].TitleName = this.GetLangName(
                    info.TitleBase.ToReverseXSS(),
                    info.TitleKo.ToReverseXSS(), info.TitleEn.ToReverseXSS(), info.TitleJa.ToReverseXSS(),
                    info.TitleFr.ToReverseXSS(), info.TitleZhcn.ToReverseXSS(), info.TitleZhtw.ToReverseXSS(), PreLang);

                GetMaxShortReplyer[i].DutyName = this.GetLangName(
                    info.DutyBase.ToReverseXSS(),
                    info.DutyKo.ToReverseXSS(), info.DutyEn.ToReverseXSS(), info.DutyJa.ToReverseXSS(),
                    info.DutyFr.ToReverseXSS(), info.DutyZhcn.ToReverseXSS(), info.DutyZhtw.ToReverseXSS(), PreLang);

                imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + info.PhotoImage;
                imgPath = string.Format(imgPath, CurrentUserInfo.CompanyID, info.UserID);

                if (System.IO.File.Exists(Server.MapPath(imgPath)) == false)
                    imgPath = this.BaseImgPath + "/Icon/ico_noimg.gif";

                if (System.IO.File.Exists(Server.MapPath(imgPath)))
                {
                    #region 스크린샷

                    paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", imgPath);
                    encParams = Utility.EncryptString(Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                    imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                    #endregion 스크린샷

                    imgPath = string.Format("<img src='{0}' width='49' height='49' />", imgPath);

                    GetMaxShortReplyer[i].PhotoImage = imgPath;
                }

                i += 1;
            }

            return GetMaxShortReplyer;
        }

        /// <summary>
        /// 최신글
        /// </summary>
        /// <returns></returns>
        private List<BE.ListViewModel> RecentWrite()
        {
            GetRecentWrite = new List<BE.ListViewModel>();

            BLL.HomeList biz = new BLL.HomeList();
            var alList = biz.GetRecentWrite(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 5);

            foreach (var info in alList)
            {
                GetRecentWrite.Add(TitleCustom(info));
            }

            alList = null;

            return GetRecentWrite;
        }

        /// <summary>
        /// 등록한 게시글
        /// </summary>
        /// <returns></returns>
        //private List<BE.ListViewModel> MyWrite()
        //{
        //    GetMyWrite = new List<BE.ListViewModel>();

        //    BLL.HomeList biz = new BLL.HomeList();
        //    var alList = biz.GetMyWrite(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 5);

        //    foreach (var info in alList)
        //    {
        //        GetMyWrite.Add(TitleCustom(info));
        //    }

        //    alList = null;

        //    return GetMyWrite;
        //}

        /// <summary>
        /// 제목 칼라, 색깔 설정하기
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        private BE.ListViewModel TitleCustom(BE.ListViewModel info)
        {
            string headerName = this.GetLangName(
                                    info.HeaderBase.ToNull(), info.HeaderKo.ToNull(),
                                    info.HeaderEn.ToNull(), info.HeaderJa.ToNull(),
                                    info.HeaderFr.ToNull(), info.HeaderZhcn.ToNull(),
                                    info.HeaderZhtw.ToNull(), PreLang);

            info.HeaderName = (headerName.Length > 0) ? "[" + headerName + "]" : "";

            //말머리
            info.Title = (info.HeaderName.Length > 0) ? info.HeaderName + " " + info.Title : info.Title;

            //글자 폰트 크기 및 색깔 설정하기
            if (info.Etc2.ToNull().Length > 0)
            {
                string[] etcStr = info.Etc2.Split(',');
                StringBuilder sb = new StringBuilder();
                if (etcStr[2].ToString() != "0") sb.Append("<b>");
                if (etcStr[0].ToString() != "") sb.Append("<font color='" + etcStr[0].ToString() + "'>");
                if (etcStr[1].ToString() != "") sb.Append("<font size='" + etcStr[1].ToString() + "'>");
                sb.Append(info.Title);
                if (etcStr[0].ToString() != "") sb.Append("</font>");
                if (etcStr[1].ToString() != "") sb.Append("</font>");
                if (etcStr[2].ToString() != "0") sb.Append("</b>");

                info.Title = sb.ToString();

            }

            if (info.MobileContent.Length > 55)
            {
                info.MobileContent = info.MobileContent.Substring(0, 55) + "....";
            }

            #region 이름 다국어 설정

            info.WriteUserName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);

            #endregion 이름 다국어 설정

            if (info.OptionAnonymity == "1") //익명글
            {
                string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", "익명,NoName");
                info.WriteUserName = (PreLang == "ko") ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];
                info.WriteUserID = 0;
            }

            //조회하지 않으면 굵게 처리
            if (info.ReadCount == 0) info.Title = "<b>" + info.Title + "</b>";
            DateTime dt;

            if (DateTime.TryParse(info.WriteDateDT.ToString(), out dt))
                info.WriteDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
            else
                info.WriteDate = string.Empty;

            return info;
        }

        /// <summary>
        /// 사용자 초기 탭 가져오기
        /// </summary>
        private void StartUserTab()
        {
            DAL.HomeList dal = new DAL.HomeList();
            StartTab = dal.GetUserTab(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (StartTab ==1)
            {         
                //개인 게시물
                chartUser.Attributes["style"] = "display:none";
                allBoardList.Attributes["style"] = "display:none";
                maxYearUser.Attributes["style"] = "display:none";
                maxYearShort.Attributes["style"] = "display:none";
                newMyBoardList.Attributes["style"] = "display:";                
                myPage.Attributes.Add("class", "selected");               
            }
            else
            {
                //전체 게시물
                chartUser.Attributes["style"] = "display:";
                allBoardList.Attributes["style"] = "display:";                
                maxYearUser.Attributes["style"] = "display:";
                maxYearShort.Attributes["style"] = "display:";
                newMyBoardList.Attributes["style"] = "display:none";                
                allPage.Attributes.Add("class", "selected");
            }
        }
        #endregion (함수)
    }

    /// <summary>
    /// 메뉴별 최신 new 여부
    /// </summary>
    public class NewMenuEntity
    {
        #region NEW 아이콘 사용 용도

        public bool RecentWrite { get; set; }
        public bool MaxShort { get; set; }
        public bool MaxRead { get; set; }
        public bool MaxRate { get; set; }

        public bool MyWrite { get; set; }
        public bool MyListByShort { get; set; }
        public bool MyShort { get; set; }
        public bool MyImpo { get; set; }

        #endregion NEW 아이콘 사용 용도
    }

    /// <summary>
    /// 나의 관련 게시물 현황
    /// </summary>
    public class MyBarChartEntity
    {
        #region 나의 관련 게시물 현황

        public int MyWriteCnt { get; set; }
        public int MyListByShortCnt { get; set; }
        public int MyShortCnt { get; set; }
        public int ChartMaxValue { get; set; } //최대값
        public int ChartIncre { get; set; } //증가

        #endregion 나의 관련 게시물 현황
    }
}