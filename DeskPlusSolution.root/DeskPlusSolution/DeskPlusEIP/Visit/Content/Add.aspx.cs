using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Visit.Web.Content
{
    public partial class Add : Zeospace.DeskPlus.Visit.Web.Base.BasePageVisit
    {
        #region 변수처리

        public int CompanyID = 0;
        protected DateTime dtTargetDate;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어
        public string TmpDate = String.Empty;
        public string TmpTime = String.Empty;
        public string TagGubun = String.Empty;

        #endregion 변수처리

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Content.Add).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            #region 날짜 세팅

            dtTargetDate = new DateTime(TargetYear, TargetMonth, TargetDay);

            #endregion 날짜 세팅

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetStartEndTime();

            CompanyID = CurrentUserInfo.CompanyID;

            startDate.Value = SetDate(TmpDate);
            endDate.Value = SetDate(TmpDate);

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");

            string templateFooter = "</body></html>";
            string templateContent = LoadTemplate();

            tbHiddenMime_contents.Text = templateHeader + templateContent + templateFooter;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                /*
                string strScript = String.Empty;
                if (Request.Path.IndexOf("Add.aspx") > 0)
                {
                    if (templateContent.Length == 0)
                        strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.htmlValue = form1.tbHiddenMime_contents.value;" + "<" + "/script>";
                    else
                        strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.bodyValue = form1.tbHiddenMime_contents.value;" + "<" + "/script>";

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnControlInit", strScript);
                    NonActiveX = "F";
                }
                else
                {
                    //스크립트에서 처리한다
                    NonActiveX = "T";
                }
                */
                NonActiveX = "T";

                #region 에디터 본문 이미지 첨부 경로

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = FileHelper.GetYearMonth();
                UploadPath = baseFileDirectory + CompanyID + "/Visit/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                #endregion 에디터 본문 이미지 첨부 경로
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : Loads the data.
        /// </summary>
        private string LoadTemplate()
        {
            string templatePath = "/DeskPlusEIP/Visit/Jt/Template/JTVisit.htm";

            string file = Server.MapPath(templatePath);
            StreamReader sr;

            FileInfo fi = new FileInfo(file);
            string templateHTML = "";
            if (File.Exists(file))
            {
                sr = File.OpenText(file);
                templateHTML += sr.ReadToEnd();
                sr.Close();
            }

            return templateHTML;
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save");
                cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnRepeat.Text = resMng.GetString("Setting");
                cbtnRepCancel.Text = resMng.GetString("Cancel");        //"취소";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
            TmpTime = GetDecryptParam("tmpTime", "");
            TagGubun = GetDecryptParam("tagGubun", "day");
        }

        /// <summary>
        /// 현재 년월일 가져오기 [2012-01-01]
        /// </summary>
        /// <returns></returns>
        public string GetYearMonthDay()
        {
            string nowYearMonDay = "";
            if (TargetMonth.ToString().Length == 1)
            {
                nowYearMonDay = TargetYear.ToString() + "-" + "0" + TargetMonth.ToString();
            }
            else
            {
                nowYearMonDay = TargetYear.ToString() + "-" + TargetMonth.ToString();
            }

            if (TargetDay.ToString().Length == 1)
            {
                nowYearMonDay = nowYearMonDay + "-" + "0" + TargetDay.ToString();
            }
            else
            {
                nowYearMonDay = nowYearMonDay + "-" + TargetDay.ToString();
            }
            return nowYearMonDay;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : 일시 시간
        /// </summary>
        private void SetStartEndTime()
        {
            ddlstartTime.Items.Clear();
            ddlendTime.Items.Clear();

            DateTime dtFromTime = dtTargetDate;
            DateTime dtToTime = dtTargetDate.AddDays(1);

            while (dtFromTime < dtToTime)
            {
                //직접 바인딩
                ListItem tmpItemFrom = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlstartTime.Items.Add(tmpItemFrom);

                ListItem tmpItemTo = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToShortTimeString());
                ddlendTime.Items.Add(tmpItemTo);

                dtFromTime = dtFromTime.AddMinutes(30);
            }

            if (TmpTime != "")
            {
                ddlstartTime.SelectedIndex = TmpTime.ToNumber<int>() * 2;
                ddlendTime.SelectedIndex = ddlstartTime.SelectedIndex + 1;
            }
            else
            {
                ddlstartTime.SelectedIndex = 18;
                ddlendTime.SelectedIndex = ddlstartTime.SelectedIndex + 1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : 방문예약 등록
        /// </summary>
        /// <returns></returns>
        private int Addition()
        {
            int rtnVal = 0;
            BE.ReserveInfo info = new BE.ReserveInfo();
            RM.BE.ReserveInfo rmInfo = new RM.BE.ReserveInfo();
            BLL.Reserve bll = new BLL.Reserve();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Subject = ctbName.Text.Trim();
            info.Content = GetContents();
            info.VisitType = "0";

            info.WriteUserID = CurrentUserInfo.UserID;
            info.WriteUserName = CurrentUserInfo.Name;
            info.WriteDateYM = FileHelper.GetYearMonth();
            info.WriteDate = System.DateTime.Now;

            info.ModifyUserID = CurrentUserInfo.UserID;
            info.ModifyUserName = CurrentUserInfo.Name;
            info.ModifyDate = System.DateTime.Now;

            info.PreLang = PreLang;
            info.SMSLoginID = CurrentUserInfo.LogInID;
            info.Mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

            // 첨부파일
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            // 반복설정
            string[] arrRepeat = tbRepeat.Text.Trim().TrimEnd('$').Split('$');
            BE.RepeatInfo repInfo = new BE.RepeatInfo();
            RM.BE.RepeatInfo rmrepInfo = new RM.BE.RepeatInfo();

            if (hdRepeatType.Text != "")
            {
                repInfo.CompanyID = CurrentUserInfo.CompanyID;
                repInfo.RepeatType = hdRepeatType.Text;
                repInfo.SelectType = hdSelectType.Text;
                repInfo.IntervalCnt = hdIntervalCnt.Text.ToNumber<int>();
                repInfo.Option1 = (hdOption1.Text == "") ? "0" : hdOption1.Text;
                repInfo.Option2 = (hdOption2.Text == "") ? "0" : hdOption2.Text;
                repInfo.Option3 = (hdOption3.Text == "") ? "0" : hdOption3.Text;
                repInfo.Option4 = (hdOption4.Text == "") ? "0" : hdOption4.Text;
                repInfo.Option5 = (hdOption5.Text == "") ? "0" : hdOption5.Text;
                repInfo.Option6 = (hdOption6.Text == "") ? "0" : hdOption6.Text;
                repInfo.Option7 = (hdOption7.Text == "") ? "0" : hdOption7.Text;
                repInfo.MaxReptCnt = hdMaxReptCnt.Text.ToNumber<int>();
                repInfo.EndDate = (hdEndDate.Text != "") ? Convert.ToDateTime(hdEndDate.Text) : Convert.ToDateTime("2999-01-01");

                rmrepInfo.CompanyID = CurrentUserInfo.CompanyID;
                rmrepInfo.RepeatType = hdRepeatType.Text;
                rmrepInfo.SelectType = hdSelectType.Text;
                rmrepInfo.IntervalCnt = hdIntervalCnt.Text.ToNumber<int>();
                rmrepInfo.Option1 = (hdOption1.Text == "") ? "0" : hdOption1.Text;
                rmrepInfo.Option2 = (hdOption2.Text == "") ? "0" : hdOption2.Text;
                rmrepInfo.Option3 = (hdOption3.Text == "") ? "0" : hdOption3.Text;
                rmrepInfo.Option4 = (hdOption4.Text == "") ? "0" : hdOption4.Text;
                rmrepInfo.Option5 = (hdOption5.Text == "") ? "0" : hdOption5.Text;
                rmrepInfo.Option6 = (hdOption6.Text == "") ? "0" : hdOption6.Text;
                rmrepInfo.Option7 = (hdOption7.Text == "") ? "0" : hdOption7.Text;
                rmrepInfo.MaxReptCnt = hdMaxReptCnt.Text.ToNumber<int>();
                rmrepInfo.EndDate = (hdEndDate.Text != "") ? Convert.ToDateTime(hdEndDate.Text) : Convert.ToDateTime("2999-01-01");
            }

            // 공유자 설정
            List<BE.OpenListInfo> alOpen = new List<BE.OpenListInfo>();

            string[] arrHiddenShare = tbHiddenShare.Text.Trim().TrimEnd('|').Split('|');

            if (tbHiddenShare.Text.Trim() != "")
            {
                for (int i = 0; i < arrHiddenShare.Length; i++)
                {
                    BE.OpenListInfo openInfo = new BE.OpenListInfo();
                    openInfo.CompanyID = CurrentUserInfo.CompanyID;
                    openInfo.Type = arrHiddenShare[i].Substring(0, 1);
                    openInfo.OwnerID = arrHiddenShare[i].Replace("M$", "").Replace("G$", "").ToNumber<int>();
                    openInfo.WriteUserID = CurrentUserInfo.UserID;
                    openInfo.WriteUserName = CurrentUserInfo.Name;

                    alOpen.Add(openInfo);
                }
            }

            // 자원 리스트
            string[] arrHiddenResource = tbHiddenResource.Text.Trim().TrimEnd('|').Split('|');

            if (tbHiddenResource.Text.Trim() != "")
            {
                rmInfo.CompanyID = CurrentUserInfo.CompanyID;
                rmInfo.Subject = ctbName.Text.Trim();
                rmInfo.UserImg = "FF0000"; // 하드코딩
                rmInfo.Content = GetContents();
                rmInfo.IsGuard = "0"; //보안설정:일반

                rmInfo.WriteUserID = CurrentUserInfo.UserID;
                rmInfo.WriteUserName = CurrentUserInfo.Name;
                rmInfo.WriteDateYM = FileHelper.GetYearMonth();
                rmInfo.WriteDate = System.DateTime.Now;

                rmInfo.ModifyUserID = CurrentUserInfo.UserID;
                rmInfo.ModifyUserName = CurrentUserInfo.Name;
                rmInfo.ModifyDate = System.DateTime.Now;

                rmInfo.PreLang = PreLang;
                rmInfo.SMSLoginID = CurrentUserInfo.LogInID;
                rmInfo.Mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                for (int i = 0; i < arrHiddenResource.Length; i++)
                {
                    if (arrHiddenResource[i].ToString() != "")
                    {
                        string[] arrResource = arrHiddenResource[i].Split('$');
                        rmInfo.ArrListID += arrResource[0].ToString() + ",";
                        rmInfo.ArrQty += arrResource[1].ToString() + ",";

                        // Usable - 0 : 미승인, 1 : 승인 , 2 : 거부
                        if (arrResource[2].ToString() == "1")
                        {
                            // 1. 자원의 담당자 승인 필요 O
                            rmInfo.ArrUsable += "0,";
                            Zeospace.DeskPlus.RM.DAL.ListUser dal = new Zeospace.DeskPlus.RM.DAL.ListUser();

                            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, arrResource[0].ToString().ToNumber<int>());

                            if (dtr != null)
                            {
                                if (dtr.HasRows)
                                {
                                    while (dtr.Read())
                                    {
                                        if (CurrentUserInfo.UserID == dtr["ManagerUserID"].ToString().ToNumber<int>())
                                        {
                                            rmInfo.ArrUsable += "1,";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // 2. 자원의 담당자 승인 필요 X
                            rmInfo.ArrUsable += "1,";
                        }
                    }
                }
            }

            // 날짜 리스트
            List<BE.ReserveEtcInfo> al = new List<BE.ReserveEtcInfo>();
            List<Zeospace.DeskPlus.RM.BE.ReserveEtcInfo> rmal = new List<RM.BE.ReserveEtcInfo>();

            if (tbRepeat.Text.Trim() != "")
            {
                // 반복 일정 O
                for (int j = 0; j < arrRepeat.Length; j++)
                {
                    if (arrRepeat[j].ToString() != "")
                    {
                        BE.ReserveEtcInfo etcInfo = new BE.ReserveEtcInfo();
                        DateTime dtRepeat = Convert.ToDateTime(arrRepeat[j].ToString());

                        etcInfo.StartDate = new DateTime(dtRepeat.Year, dtRepeat.Month, dtRepeat.Day, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Minute, 0);
                        etcInfo.EndDate = new DateTime(dtRepeat.Year, dtRepeat.Month, dtRepeat.Day, (Convert.ToDateTime(ddlendTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlendTime.SelectedValue)).Minute, 0);

                        RM.BE.ReserveEtcInfo rmetcInfo = new RM.BE.ReserveEtcInfo();
                        rmetcInfo.StartDate = etcInfo.StartDate;
                        rmetcInfo.EndDate = etcInfo.EndDate;
                        TimeSpan gap = etcInfo.EndDate.Subtract(etcInfo.StartDate);
                        rmetcInfo.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();

                        al.Add(etcInfo);
                        rmal.Add(rmetcInfo);
                    }
                }

                rtnVal = bll.Add(info, rmInfo, alOpen, al, rmal, repInfo, rmrepInfo);
            }
            else
            {
                //반복 일정 X
                DateTime dtStart = Convert.ToDateTime(startDate.Value);
                DateTime dtEnd = Convert.ToDateTime(endDate.Value);
                info.StartDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Minute, 0);
                info.EndDate = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, (Convert.ToDateTime(ddlendTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlendTime.SelectedValue)).Minute, 0);

                rmInfo.StartDate = info.StartDate;
                rmInfo.EndDate = info.EndDate;
                TimeSpan gap = info.EndDate.Subtract(info.StartDate);
                rmInfo.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();

                rmInfo.ParentReservationID = 0;

                info.ReserveGroupID = 0;

                rtnVal = bll.Add(info, rmInfo, alOpen);
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : 저장
        /// </summary>
        private void Save()
        {
            int reserveID = Addition();

            if (reserveID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("reserveID", reserveID.ToString());
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("tmpDate", TmpDate);
                paraDic.Add("tagGubun", TagGubun);
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoView"));
                paraDic.Add("goURL", "/DeskPlusEIP/Visit/Content/View.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : 본문 내용 및 본문 이미지(업로드) MIME 하여 본문 내용가져오기
        /// </summary>
        /// <returns></returns>
        private string GetContents()
        {
            //string contents = "";
            //contents = tbHiddenHTML_contents.Value;

            //return contents;


            string contents = "";

            if (NonActiveX == "F") //activex
            {
                string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                string serverName = baseHttp + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                string filePath = FileHelper.GetServerMapPath(UploadPath);
                string serverNameFolderName = serverName + UploadPath;

                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                string contentValue = tbHiddenMime_contents.Text;

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass util
                    = new DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass();

                util.ProcessDecoding(contentValue, filePath, serverNameFolderName, 0);
                contents = util.DecodedHTML;
            }
            else if (NonActiveX == "T") //x-free
            {
                contents = tbHiddenHTML_contents.Value;
            }

            return contents;
        }

        #endregion (함수)

        #region (프로퍼티)

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int TargetYear
        {
            get { return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : Convert.ToInt32(ViewState["targetYear"].ToString()); }
            set { ViewState["targetYear"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : Convert.ToInt32(ViewState["targetMonth"].ToString()); }
            set { ViewState["targetMonth"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : Convert.ToInt32(ViewState["targetDay"].ToString()); }
            set { ViewState["targetDay"] = value; }
        }

        /// <summary>
        /// ActiveX 유무
        /// </summary>
        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }

        /// <summary>
        /// ListTitle
        /// </summary>
        public string ListTitle
        {
            get { return GetViewStateString("ListTitle", ""); }
            set { ViewState["ListTitle"] = value; }
        }

        /// <summary>
        /// ListQty
        /// </summary>
        public int ListQty
        {
            get { return GetViewStateInt("ListQty", "0"); }
            set { ViewState["ListQty"] = value; }
        }

        /// <summary>
        /// ListUsableQty
        /// </summary>
        public int ListUsableQty
        {
            get { return GetViewStateInt("ListUsableQty", "0"); }
            set { ViewState["ListUsableQty"] = value; }
        }

        /// <summary>
        /// ListAuthority
        /// </summary>
        public string ListAuthority
        {
            get { return GetViewStateString("ListAuthority", ""); }
            set { ViewState["ListAuthority"] = value; }
        }

        #endregion (프로퍼티)

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-15
        /// 작성목적 : 확인버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            Save();
        }

        #endregion (이벤트핸들러)
    }
}