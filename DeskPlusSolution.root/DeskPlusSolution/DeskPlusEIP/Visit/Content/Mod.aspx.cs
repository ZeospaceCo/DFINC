
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.IO;
using System.Collections;

namespace Zeospace.DeskPlus.Visit.Web.Content
{
    public partial class Mod : Zeospace.DeskPlus.Visit.Web.Base.BasePageVisit
    {
        #region 변수처리
        public int CompanyID = 0;
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어   
        public string TmpDate = String.Empty;
        public string TagGubun = String.Empty;
        public int ReserveID;
        protected DateTime dtTargetDate;
        public string Contents = string.Empty; //내용        
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Content.Mod).Assembly;	//어셈블리 선언
            #endregion


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

        #endregion

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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            CompanyID = CurrentUserInfo.CompanyID;

            SetStartEndTime();
            GetData();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
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
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 예약 정보 가져오기
        /// </summary>
        private void GetData()
        {
            DAL.Reserve dal = new DAL.Reserve();

            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, ReserveID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ctbName.Text = dtr["Subject"].ToString().ToReverseXSS();

                        DateTime dtStart = Convert.ToDateTime(dtr["StartDate"].ToString());
                        DateTime dtEnd = Convert.ToDateTime(dtr["EndDate"].ToString());

                        startDate.Value = SetDate(dtr["StartDate"].ToString());
                        BeforeStartDate = dtStart.Year.ToString() + dtStart.Month.ToString().PadLeft(2, '0') + dtStart.Day.ToString().PadLeft(2, '0');
                        endDate.Value = SetDate(dtr["EndDate"].ToString());

                        ddlstartTime.SelectedValue = dtStart.ToShortTimeString();
                        ddlendTime.SelectedValue = dtEnd.ToShortTimeString();

                        string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");

                        Contents = templateHeader + dtr["Content"].ToString().ToEditorXSS();
                        tbHiddenHTML_contents.Value = dtr["Content"].ToString().ToEditorXSS();


                        if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                        {
                            /*
                            string strScript = String.Empty;
                            if (Request.Path.IndexOf("Mod.aspx") > 0)
                            {
                                strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.bodyValue = form1.contents.value;" + "<" + "/script>";

                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnControlInit", strScript);
                                NonActiveX = "F";
                            }
                            else
                            {   //스크립트에서 처리한다
                                NonActiveX = "T";
                            }
                            */
                            NonActiveX = "T";
                        }

                        #region 에디터 본문 이미지 첨부 경로

                        string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                        System.Guid guid = System.Guid.NewGuid();
                        string reqTime = FileHelper.GetYearMonth();
                        UploadPath = baseFileDirectory + CompanyID + "/Visit/" + reqTime + "/editor/" + guid.ToString();
                        if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                        #endregion 에디터 본문 이미지 첨부 경로

                        DateTime dtWriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                        WriteYM = dtWriteDate.Year.ToString() + dtWriteDate.Month.ToString().PadLeft(2, '0');

                        ReserveGroupID = dtr["ReserveGroupID"].ToString().ToNumber<int>();
                        WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                        WriteUserName = dtr["WriteUserName"].ToString();
                        WriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());

                        DateTime dtGroup = Convert.ToDateTime(dtr["GroupStartDate"].ToString());
                        GroupStartDate = dtGroup.Year + "-" + dtGroup.Month.ToString().PadLeft(2, '0') + "-" + dtGroup.Day.ToString().PadLeft(2, '0');
                    }
                }
            }

            GetRepeat(ReserveGroupID);

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 반복 설정 가져오기
        /// </summary>
        private void GetRepeat(int _reserveGroupID)
        {
            DAL.Repeat dal = new DAL.Repeat();

            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, _reserveGroupID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.RepeatInfo info = new BE.RepeatInfo();
                        info.CompanyID = dtr["RepeatType"].ToString().ToNumber<int>();
                        info.ReserveGroupID = dtr["ReserveGroupID"].ToString().ToNumber<int>();
                        info.RepeatType = dtr["RepeatType"].ToString();
                        info.SelectType = dtr["SelectType"].ToString();
                        info.IntervalCnt = dtr["IntervalCnt"].ToString().ToNumber<int>();
                        info.Option1 = dtr["Option1"].ToString();
                        info.Option2 = dtr["Option2"].ToString();
                        info.Option3 = dtr["Option3"].ToString();
                        info.Option4 = dtr["Option4"].ToString();
                        info.Option5 = dtr["Option5"].ToString();
                        info.Option6 = dtr["Option6"].ToString();
                        info.Option7 = dtr["Option7"].ToString();
                        info.MaxReptCnt = dtr["MaxReptCnt"].ToString().ToNumber<int>();
                        info.EndDate = Convert.ToDateTime(dtr["EndDate"].ToString());

                        hdRepeatType.Text = dtr["RepeatType"].ToString();
                        hdSelectType.Text = dtr["SelectType"].ToString();
                        hdIntervalCnt.Text = dtr["IntervalCnt"].ToString();
                        hdOption1.Text = dtr["Option1"].ToString();
                        hdOption2.Text = dtr["Option2"].ToString();
                        hdOption3.Text = dtr["Option3"].ToString();
                        hdOption4.Text = dtr["Option4"].ToString();
                        hdOption5.Text = dtr["Option5"].ToString();
                        hdOption6.Text = dtr["Option6"].ToString();
                        hdOption7.Text = dtr["Option7"].ToString();
                        hdMaxReptCnt.Text = dtr["MaxReptCnt"].ToString();
                        hdEndDate.Text = Convert.ToDateTime(dtr["EndDate"].ToString()).ToShortDateString();

                        switch (info.RepeatType)
                        {
                            case "D":
                                GetDay(info);
                                break;
                            case "W":
                                GetWeek(info);
                                break;
                            case "M":
                                GetMonth(info);
                                break;
                        }
                    }

                    hdRepeatChk.Text = "Y";
                }
            }
        }

        /// <summary>
        /// 일별 반복
        /// </summary>
        /// <param name="_info"></param>
        private void GetDay(BE.RepeatInfo _info)
        {
            string strVal = String.Empty;
            if (_info.SelectType == "1")
            {
                strVal += _info.IntervalCnt + resMng.GetString("Repeat17");
            }
            else
            {
                strVal += resMng.GetString("Repeat3");
            }

            if (_info.MaxReptCnt > 0)
            {
                strVal += _info.MaxReptCnt + resMng.GetString("Repeat23");
            }
            else
            {
                strVal += _info.EndDate.ToShortDateString() + resMng.GetString("Repeat22");
            }

            txtrepeat.Value = strVal;
        }

        /// <summary>
        /// 주별 반복
        /// </summary>
        /// <param name="_info"></param>
        private void GetWeek(BE.RepeatInfo _info)
        {
            string strVal = String.Empty;

            strVal += _info.IntervalCnt + resMng.GetString("Repeat18") + " ";

            if (_info.Option1 == "1")
                strVal += resMng.GetString("Sun") + " ";
            if (_info.Option2 == "1")
                strVal += resMng.GetString("Mon") + " ";
            if (_info.Option3 == "1")
                strVal += resMng.GetString("Tue") + " ";
            if (_info.Option4 == "1")
                strVal += resMng.GetString("Wed") + " ";
            if (_info.Option5 == "1")
                strVal += resMng.GetString("Thu") + " ";
            if (_info.Option6 == "1")
                strVal += resMng.GetString("Fri") + " ";
            if (_info.Option7 == "1")
                strVal += resMng.GetString("Sat") + " ";

            strVal += resMng.GetString("Repeat19");

            if (_info.MaxReptCnt > 0)
            {
                strVal += _info.MaxReptCnt + resMng.GetString("Repeat23");
            }
            else
            {
                strVal += _info.EndDate.ToShortDateString() + resMng.GetString("Repeat22");
            }

            txtrepeat.Value = strVal;
        }

        /// <summary>
        /// 월별 반복
        /// </summary>
        /// <param name="_info"></param>
        private void GetMonth(BE.RepeatInfo _info)
        {
            string strVal = String.Empty;
            if (_info.SelectType == "1")
            {
                strVal += _info.IntervalCnt + resMng.GetString("Repeat5") + " " + _info.Option1 + resMng.GetString("Repeat20");
            }
            else
            {
                strVal += _info.IntervalCnt + resMng.GetString("Repeat5") + " " + _info.Option1 + _info.Option2 + resMng.GetString("Repeat21");
            }

            if (_info.MaxReptCnt > 0)
            {
                strVal += _info.MaxReptCnt + resMng.GetString("Repeat23");
            }
            else
            {
                strVal += _info.EndDate.ToShortDateString() + resMng.GetString("Repeat22");
            }

            txtrepeat.Value = strVal;
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Mod");
                cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnRepeat.Text = resMng.GetString("Setting");        //"설정";
                cbtnRepCancel.Text = resMng.GetString("Cancel");        //"취소";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
            TagGubun = GetDecryptParam("tagGubun", "");
            ReserveID = GetDecryptParam("reserveID", "").ToNumber<int>();
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
        /// 작성일자 : 2014-02-13
        /// 작성목적 : 방문 예약 수정
        /// </summary>
        /// <returns></returns>
        private bool Modify()
        {
            bool rtnVal = false;

            BE.ReserveInfo info = new BE.ReserveInfo();
            Zeospace.DeskPlus.RM.BE.ReserveInfo rmInfo = new Zeospace.DeskPlus.RM.BE.ReserveInfo();
            BLL.Reserve bll = new BLL.Reserve();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ReserveGroupID = ReserveGroupID;
            info.ReserveID = ReserveID;
            info.Subject = ctbName.Text.Trim();
            info.Content = GetContents();
            info.VisitType = "0";

            info.WriteUserID = WriteUserID;
            info.WriteUserName = WriteUserName;
            info.WriteDate = WriteDate;
            info.WriteDateYM = WriteYM;

            info.ModifyUserID = CurrentUserInfo.UserID;
            info.ModifyUserName = CurrentUserInfo.Name;
            info.ModifyDate = System.DateTime.Now;

            info.PreLang = PreLang;
            info.SMSLoginID = CurrentUserInfo.LogInID;
            info.Mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            // 반복 설정
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

                rmInfo.WriteUserID = WriteUserID;
                rmInfo.WriteUserName = WriteUserName;
                rmInfo.WriteDate = WriteDate;
                rmInfo.WriteDateYM = WriteYM;

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

            List<BE.ReserveEtcInfo> al = new List<BE.ReserveEtcInfo>();
            List<Zeospace.DeskPlus.RM.BE.ReserveEtcInfo> rmal = new List<RM.BE.ReserveEtcInfo>();

            // 날짜 리스트                     
            if (tbRepeat.Text.Trim() != "")
            {
                // 반복 일정 O                           
                if (hdRepeatSel.Text == "1") // 1. 이번 일정만 
                {
                    if (hdRepeatChk.Text == "N")
                    {
                        #region 반복 일정 수정시 (해당 일정은 반복에서 삭제 / 신규 반복일정을 생성)
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

                        ReserveID = bll.Mod(info, rmInfo, alOpen, al, rmal, repInfo, rmrepInfo, ReserveGroupID);
                        rtnVal = true;
                        #endregion
                    }
                }
                else if (hdRepeatSel.Text == "2") // 2. 향후 모든 일정
                {
                    #region 반복 일정 수정시, 미수정시 (해당 일정 이후의 일정을 삭제 / 신규 반복일정을 생성)
                    DateTime dt = Convert.ToDateTime(startDate.Value);
                    int tmpStartDate = (dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0')).ToNumber<int>();
                    int tmpBeforeDate = BeforeStartDate.ToNumber<int>();

                    tmpStartDate = (tmpStartDate > tmpBeforeDate) ? tmpBeforeDate : tmpStartDate;

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

                    ReserveID = bll.ModFuture(info, rmInfo, alOpen, al, rmal, repInfo, rmrepInfo, ReserveGroupID, tmpStartDate.ToString());
                    rtnVal = true;
                    #endregion
                }
                else if (hdRepeatSel.Text == "3") // 3. 모든 일정
                {
                    #region 반복 일정 수정시, 미수정시 (해당 일정은 반복에서 삭제 / 신규 반복일정을 생성)
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

                    ReserveID = bll.ModAll(info, rmInfo, alOpen, al, rmal, repInfo, rmrepInfo, ReserveGroupID);
                    rtnVal = true;

                    #endregion
                }
                else // 4. 반복 설정이 원래 없었지만 추가한 경우
                {
                    #region 반복 설정 새롭게 추가
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

                    ReserveID = bll.Mod(info, rmInfo, alOpen, al, rmal, repInfo, rmrepInfo);

                    rtnVal = true;

                    #endregion
                }
            }
            else
            {
                //반복 일정 X
                if (hdRepeatChk.Text != "")
                {
                    if (hdRepeatSel.Text == "1") // 1. 이번 일정만 
                    {
                        #region 반복 일정 미수정시 (해당 일정 하나만 수정 / 반복일정에서 빠진다)
                        DateTime dtStart = Convert.ToDateTime(startDate.Value);
                        info.StartDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Minute, 0);
                        info.EndDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, (Convert.ToDateTime(ddlendTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlendTime.SelectedValue)).Minute, 0);

                        rmInfo.StartDate = info.StartDate;
                        rmInfo.EndDate = info.EndDate;
                        TimeSpan gap = info.EndDate.Subtract(info.StartDate);
                        rmInfo.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();

                        info.ReserveGroupID = ReserveID;

                        rtnVal = bll.Mod(info, rmInfo, ReserveGroupID, alOpen);

                        #endregion
                    }
                    else
                    {
                        #region  반복 일정 설정이 있었을 경우
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

                        ReserveID = bll.ModFuture(info, rmInfo, ReserveGroupID, BeforeStartDate, alOpen);
                        rtnVal = true;
                        #endregion
                    }
                }
                else
                {
                    #region 반복 일정 설정이 없었을 경우
                    DateTime dtStart = Convert.ToDateTime(startDate.Value);
                    DateTime dtEnd = Convert.ToDateTime(endDate.Value);
                    info.StartDate = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlstartTime.SelectedValue)).Minute, 0);
                    info.EndDate = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, (Convert.ToDateTime(ddlendTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlendTime.SelectedValue)).Minute, 0);

                    rmInfo.StartDate = info.StartDate;
                    rmInfo.EndDate = info.EndDate;
                    TimeSpan gap = info.EndDate.Subtract(info.StartDate);
                    rmInfo.DifferMinite = gap.TotalMinutes.ToString().ToNumber<int>();

                    rmInfo.ParentReservationID = 0;

                    info.ReserveGroupID = ReserveID;

                    rtnVal = bll.Mod(info, rmInfo, alOpen);

                    #endregion
                }
            }

            return rtnVal;

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 예약 자원 수정
        /// </summary>
        private void ModList()
        {
            if (Modify())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("reserveID", ReserveID.ToString());
                paraDic.Add("tmpDate", TmpDate.ToString());
                paraDic.Add("searchField", SearchField.ToString());
                paraDic.Add("searchValue", SearchValue.ToString());
                paraDic.Add("tagGubun", TagGubun.ToString());
                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoView"));
                paraDic.Add("goURL", "/DeskPlusEIP/Visit/Content/View.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
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

        #endregion


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
        /// ListContent
        /// </summary>
        public string ListContent
        {
            get { return GetViewStateString("ListContent", ""); }
            set { ViewState["ListContent"] = value; }
        }

        /// <summary>
        /// RListID
        /// </summary>
        public string RListID
        {
            get { return GetViewStateString("RListID", ""); }
            set { ViewState["RListID"] = value; }
        }

        /// <summary>
        /// RTitle
        /// </summary>
        public string RTitle
        {
            get { return GetViewStateString("RTitle", ""); }
            set { ViewState["RTitle"] = value; }
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
        /// WriteYM
        /// </summary>
        public string WriteYM
        {
            get { return GetViewStateString("WriteYM", ""); }
            set { ViewState["WriteYM"] = value; }
        }

        /// <summary>
        /// ReserveGroupID
        /// </summary>
        public int ReserveGroupID
        {
            get { return GetViewStateInt("ReserveGroupID", "0"); }
            set { ViewState["ReserveGroupID"] = value; }
        }

        /// <summary>
        /// BeforeStartDate
        /// </summary>
        public string BeforeStartDate
        {
            get { return GetViewStateString("BeforeStartDate", ""); }
            set { ViewState["BeforeStartDate"] = value; }
        }

        /// <summary>
        /// WriteUserID
        /// </summary>
        public int WriteUserID
        {
            get { return GetViewStateInt("WriteUserID", "0"); }
            set { ViewState["WriteUserID"] = value; }
        }

        /// <summary>
        /// WriteUserName
        /// </summary>
        public string WriteUserName
        {
            get { return GetViewStateString("WriteUserName", ""); }
            set { ViewState["WriteUserName"] = value; }
        }

        /// <summary>
        /// WriteDate
        /// </summary>
        public DateTime WriteDate
        {
            get { return Convert.ToDateTime(ViewState["WriteDate"]); }
            set { ViewState["WriteDate"] = value; }
        }


        /// <summary>
        /// GroupStartDate
        /// </summary>
        public string GroupStartDate
        {
            get { return GetViewStateString("GroupStartDate", ""); }
            set { ViewState["GroupStartDate"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 확인(수정) 버튼 클릭시 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            ModList();
        }

        #endregion
    }
}