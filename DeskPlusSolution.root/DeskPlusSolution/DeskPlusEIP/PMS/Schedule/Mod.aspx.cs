using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.PMS.Web.Schedule
{
    public partial class Mod : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)

        protected DateTime dtTargetDate;

        #endregion Declaration (컨트롤 선언)

        #region 변수선언

        private int DisplayID = 0;
        public string Contents = string.Empty; //내용
        public int ScheduleID = 0;

        #endregion 변수선언

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드

            this.ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Schedule.Add).Assembly;		//어셈블리 선언\
            GetResourceManager();

            #endregion 다국어 리소스 로드

            #region 날짜 세팅

            dtTargetDate = new DateTime(TargetYear, TargetMonth, TargetDay);

            #endregion 날짜 세팅

            #region 초기 데이터 로드

            if (!IsPostBack)
            {
                SetLanguage();
                SetStartEndTime();
                LoadData();
            }

            #endregion 초기 데이터 로드
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadParam();

            #region 개인정보 및 viewstate 기술

            UserName = CurrentUserInfo.Name;
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            //tbFullYMD1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //tbFullYMD2.Text = DateTime.Now.ToString("yyyy-MM-dd");

            #endregion 개인정보 및 viewstate 기술

            DAL.Display daldis = new DAL.Display();
            DataTableReader dtr = daldis.GetDisplayInfo(CompanyID, DisplayID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        tbTitleName.Text = dtr["Subjects"].ToString().ToReverseXSS();
                        //lbContent.Text = dtr["Contents"].ToString();
                        tbFullYMD1.Text = DateTime.Parse(dtr["StartDate"].ToString()).ToShortDateString();
                        tbFullYMD2.Text = DateTime.Parse(dtr["EndDate"].ToString()).ToShortDateString();
                        tbHiddenHTML_contents.Value = dtr["Contents"].ToString();//_info.Content;
                        //ddlStartTime
                        string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
                        string templateFooter = "</BODY></HTML>";

                        tbHiddenMime_contents.Text = templateHeader + dtr["Contents"].ToString() + templateFooter;
                        Contents = string.Empty;
                        Contents += templateHeader + dtr["Contents"].ToString() + templateFooter;

                        ddlStartTime.SelectedValue = DateTime.Parse(dtr["StartDate"].ToString()).ToString("HH:mm");
                        ddlEndTime.SelectedValue = DateTime.Parse(dtr["EndDate"].ToString()).ToString("HH:mm");

                        if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                        {
                            //ByFreeman 20120106 X-Free 에디터 추가
                            /*
                            string strScript = String.Empty;
                            if (Request.Path.IndexOf("Mod.aspx") > 0)
                            {
                                strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.HtmlValue(form1.tbHiddenMime_contents.value);</script>";
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
                            string reqTime = System.DateTime.Now.ToString("yyyyMM");
                            UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/PMS/Schedule/" + reqTime + "/editor/" + guid.ToString();
                            if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                            #endregion 에디터 본문 이미지 첨부 경로
                        }
                        //ScheduleID =dtr["ScheduleID"].ToString().ToNumber<int>()

                        GetRepeat(dtr["scheduleID"].ToString().ToNumber<int>(), dtr["CompanyID"].ToString().ToNumber<int>());
                    }
                }
            }
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        private void LoadParam()
        {
            ScheduleID = GetDecryptParam("scheduleID", "0").ToNumber<int>();
            DisplayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();
        }

        private void GetRepeat(int schedulid, int companyid)
        {
            DAL.Repeat biz = new DAL.Repeat();

            List<BE.RepeatScheduleInfo> data = biz.GetRepeatSchedule(companyid, schedulid);
            BE.RepeatScheduleInfo info = new BE.RepeatScheduleInfo();

            foreach (BE.RepeatScheduleInfo _be in data)
            {
                info.RepeatType = _be.RepeatType;
                info.SelectType = _be.SelectType;

                info.IntervalCnt = _be.IntervalCnt;
                info.MaxReptCnt = _be.MaxReptCnt;
                info.ScheduleID = _be.ScheduleID;
                info.Option1 = _be.Option1;
                info.Option2 = _be.Option2;
                info.Option3 = _be.Option3;
                info.Option4 = _be.Option4;
                info.Option5 = _be.Option5;
                info.Option6 = _be.Option6;
                info.Option7 = _be.Option7;
                info.StartDate = _be.StartDate;

                if (_be.EndDate.ToString("yyyyMMdd") == "29990101")
                    info.EndDate = DateTime.Parse("0001-01-01");
                else
                    info.EndDate = _be.EndDate;

                BLL.RepeatScheduleProc bizRepeatBiz = new BLL.RepeatScheduleProc(info);
                //BLL.RepeatScheduleProc bizRepeatBiz = new BLL.RepeatScheduleProc(info);

                string strRt = string.Empty;

                switch (info.RepeatType)
                {
                    case "D":
                        strRt = bizRepeatBiz.MakeDay();
                        break;

                    case "W":
                        strRt = bizRepeatBiz.MakeWeek();
                        break;

                    case "M":
                        strRt = bizRepeatBiz.MakeMonth();
                        break;
                }

                txtrepeat.Value = strRt;
                hdRepeatChk.Text = "I";
            }

            if (info.RepeatType != null)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("RepeatType", info.RepeatType.ToString());
                paraDic.Add("SelectType", info.SelectType.ToString());
                paraDic.Add("IntervalCnt", info.IntervalCnt.ToString());
                paraDic.Add("MaxReptCnt", info.MaxReptCnt.ToString());
                paraDic.Add("Option1", info.Option1.ToString());
                paraDic.Add("Option2", info.Option2.ToString());
                paraDic.Add("Option3", info.Option3.ToString());
                paraDic.Add("Option4", info.Option4.ToString());
                paraDic.Add("Option5", info.Option5.ToString());
                paraDic.Add("Option6", info.Option6.ToString());
                paraDic.Add("Option7", info.Option7.ToString());

                paraDic.Add("StartDate", "");// info.StartDate.ToString("yyyy-MM-dd HH:mm"));
                paraDic.Add("EndDate", "");//info.EndDate.ToString("yyyy-MM-dd HH:mm"));

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                hdRepeat.Text = encParams;
                hdRepeatChk.Text = "I";
            }
        }

        /// <summary>
        /// 일시의 시작일자와 종료일자 DropDownList 바인딩
        /// </summary>
        private void SetStartEndTime()
        {
            ddlStartTime.Items.Clear();
            ddlEndTime.Items.Clear();
            ddlStartTimeRepeat.Items.Clear();
            ddlEndTimeRepeat.Items.Clear();

            try
            {
                DateTime dtFromTime = dtTargetDate;
                DateTime dtToTime = dtTargetDate.AddDays(1);

                while (dtFromTime < dtToTime)
                {
                    //직접 바인딩
                    ListItem tmpItemFrom = new ListItem(dtFromTime.ToShortTimeString(), dtFromTime.ToString("HH:mm"));
                    ddlStartTime.Items.Add(tmpItemFrom);
                    ddlStartTimeRepeat.Items.Add(tmpItemFrom);

                    ListItem tmpItemTo = new ListItem(dtFromTime.AddMinutes(30).ToShortTimeString(), dtFromTime.AddMinutes(30).ToString("HH:mm"));
                    ddlEndTime.Items.Add(tmpItemTo);
                    ddlEndTimeRepeat.Items.Add(tmpItemTo);

                    dtFromTime = dtFromTime.AddMinutes(30);
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// 년월일 비교
        /// </summary>
        /// <param name="_startDate">_start date.</param>
        /// <param name="_endDate">_end date.</param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string AjaxCompareDateTime(string _startDate, string _endDate)
        {
            DateTime dtStrat = Convert.ToDateTime(_startDate);
            DateTime dtEnd = Convert.ToDateTime(_endDate);
            TimeSpan gap = dtEnd.Subtract(dtStrat);

            //종료일>시작일
            if ((int)gap.TotalMinutes > 0)
            {
                return "1";
            }//종료일 == 시작일
            else if ((int)gap.TotalMinutes == 0)
            {
                return "0";
            } //종료일<시작일
            else
            {
                return "-1";
            }
        }

        /// <summary>
        /// 시,분 비교
        /// </summary>
        /// <param name="_startTime">_start time.</param>
        /// <param name="_endTime">_end time.</param>
        /// <returns></returns>
        [ComponentArtCallbackMethod]
        public string AjaxCompareTime(string _startTime, string _endTime)
        {
            DateTime dtStartTime = Convert.ToDateTime(_startTime);
            DateTime dtEndTime = Convert.ToDateTime(_endTime);

            if (dtStartTime >= dtEndTime)
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }

        /// <summary>
        /// Saves the schedule.
        /// </summary>
        /// <returns></returns>
        private void ModSchedule()
        {
            LoadParam();

            DateTime dtStrat = Convert.ToDateTime(hdStart.Text);
            DateTime dtEnd = Convert.ToDateTime(hdEnd.Text);

            BE.ScheduleInfo be = new Zeospace.DeskPlus.PMS.BE.ScheduleInfo();
            be.CompanyID = CompanyID;
            be.ProjectID = ProjectID;
            //be.Title = GetTextBoxValidation(tbTitleName.Text.Trim());
            be.Subjects = tbTitleName.Text.Trim();
            be.Contents = SetEditorFileFolder();
            be.StartDate = new DateTime(dtStrat.Year, dtStrat.Month, dtStrat.Day, (Convert.ToDateTime(ddlStartTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlStartTime.SelectedValue)).Minute, 0);
            be.EndDate = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, (Convert.ToDateTime(ddlEndTime.SelectedValue)).Hour, (Convert.ToDateTime(ddlEndTime.SelectedValue)).Minute, 0);
            if (be.StartDate == be.EndDate)
            {
                be.EndDate = be.StartDate.AddHours(1);
            }

            //TimeSpan gap = be.EndDate.Subtract(be.StartDate);
            //be.DifferMinite = (int)gap.TotalMinutes;
            be.WriteUserID = UserID;
            be.AddPims = cbAddPims.Checked;
            be.ScheduleID = ScheduleID;
            DAL.Schedule bll = new Zeospace.DeskPlus.PMS.DAL.Schedule();

            switch (hdRepeatSel.Text)
            {
                case "1": //이번일정만
                    ScheduleID = bll.Add(be);
                    break;

                case "2":
                    ScheduleID = bll.Add(be);
                    break;

                case "3":
                    if (hdRepeatChk.Text == "N") //반복설정을 수정했을떄 수정이되면 안된다.
                    {
                        DataTableReader dtr = bll.GetSheduleID(be);
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    tbFullYMD1.Text = DateTime.Parse(dtr["StartTime"].ToString()).ToString("yyyy-MM-dd"); // _info.StartTime.ToString("yyyy-MM-dd");
                                    ddlStartTimeRepeat.SelectedValue = DateTime.Parse(dtr["StartTime"].ToString()).ToString("HH:mm");// _info.StartTime.ToString("HH:mm");
                                    tbFullYMD2.Text = DateTime.Parse(dtr["EndTime"].ToString()).ToString("yyyy-MM-dd");///_info.EndTime.ToString("yyyy-MM-dd");
                                    ddlEndTimeRepeat.SelectedValue = DateTime.Parse(dtr["EndTime"].ToString()).ToString("HH:mm");//_info.EndTime.ToString("HH:mm");
                                    be.StartDate = DateTime.Parse(dtr["StartTime"].ToString());
                                    be.EndDate = DateTime.Parse(dtr["StartTime"].ToString());
                                }
                            }
                        }
                        bll.Mod(be);
                    }
                    else
                    {
                        bll.Mod(be);
                    }

                    break;

                case "": //일반수정
                    bll.Mod(be);
                    break;
            }

            #region 일정에 추가

            if (cbAddPims.Checked)
            {
                Pim.BE.ScheduleInfo pinfo = new Pim.BE.ScheduleInfo();
                pinfo.CompanyID = CompanyID;
                pinfo.ToDoID = 0;
                pinfo.Type = "I";
                pinfo.Title = be.Subjects;
                pinfo.Content = be.Contents;
                pinfo.StartTime = be.StartDate;
                pinfo.EndTime = be.EndDate;
                pinfo.Repeat = "N";
                pinfo.Alert = "N";
                pinfo.AlertType = "";
                pinfo.AlertTime = 0;
                pinfo.AlertTargetAddress = "";
                pinfo.IsOpen = "Y";
                pinfo.UserID = CurrentUserInfo.UserID;
                pinfo.DeptID = CurrentUserInfo.DeptID;
                pinfo.OpenType = "0";
                pinfo.Lunar = "N";
                pinfo.StatusID = 0;//기본값

                Pim.DAL.Schedule pimsch = new Pim.DAL.Schedule();

                int pSchedule = pimsch.Add(pinfo);

                Pim.BE.DisplayInfo dinfo = new Pim.BE.DisplayInfo();

                dinfo.CompanyID = pinfo.CompanyID;
                dinfo.UserID = pinfo.UserID;
                dinfo.StartDate = pinfo.StartTime;
                dinfo.EndDate = pinfo.EndTime;
                dinfo.ScheduleID = pSchedule;
                Pim.DAL.Display bizdis = new Pim.DAL.Display();
                bizdis.AddDisplay(dinfo);
            }

            #endregion 일정에 추가

            #region 반복설정

            DAL.Repeat bizre = new DAL.Repeat();
            BE.RepeatScheduleInfo infore = new BE.RepeatScheduleInfo();
            DAL.Display bizDis = new DAL.Display();
            BE.DisplayInfo DisInfo = new BE.DisplayInfo();

            infore.CompanyID = CompanyID;
            infore.ScheduleID = ScheduleID;
            DisInfo.CompanyID = CompanyID;
            DisInfo.ScheduleID = ScheduleID;
            DisInfo.UserID = UserID;
            DisInfo.DisplayID = DisplayID;

            if (hdRepeat.Text.Trim().Length > 0)
            {
                DisInfo.ScheduleID = ScheduleID;
                if (hdRepeatChk.Text == "I") //반복설정에 변경사항이 없을때
                {
                    switch (hdRepeatSel.Text)
                    {
                        case "1":
                            bizDis.DelDisplayIDSolo(DisInfo);
                            DisInfo.CompanyID = CompanyID;
                            DisInfo.UserID = CurrentUserInfo.UserID;
                            DisInfo.StartDate = be.StartDate;
                            DisInfo.EndDate = be.EndDate;
                            if (DisInfo.StartDate == DisInfo.EndDate)
                            {
                                DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
                            }
                            DisInfo.ScheduleID = ScheduleID;
                            DisplayID = bizDis.AddDisplay(DisInfo);
                            break;

                        case "2":
                            DisInfo.ScheduleID = GetDecryptParam("scheduleID", "0").ToNumber<int>();
                            DisInfo.StartDate = DateTime.Parse(be.StartDate.ToString("yyyy-MM-dd"));
                            DisInfo.NewScheduleID = ScheduleID;
                            bizDis.UpdDisplayDate(DisInfo);
                            break;

                        case "3":
                            DisInfo.ScheduleID = GetDecryptParam("scheduleID", "0").ToNumber<int>();
                            DisInfo.NewScheduleID = ScheduleID;
                            bizDis.UpdDisplayAll(DisInfo);
                            break;
                    }
                }

                if (hdRepeatChk.Text == "N") //반복설정을 수정했을떄
                {
                    switch (hdRepeatSel.Text)
                    {
                        case "1":
                            bizDis.DelDisplayIDSolo(DisInfo);
                            break;

                        case "2":
                            ///bizDis.ModDisplay
                            DisInfo.ScheduleID = GetDecryptParam("scheduleID", "0").ToNumber<int>();
                            DisInfo.StartDate = DateTime.Parse(be.StartDate.ToString("yyyy-MM-dd"));
                            bizDis.DelDisplayDate(DisInfo);
                            break;

                        case "3":
                            DisInfo.ScheduleID = GetDecryptParam("scheduleID", "0").ToNumber<int>();
                            bizDis.DelDisplay(DisInfo); //전체삭제
                            break;
                    }

                    bizre.DelRepeatSchedule(infore);
                    AddRepeat(ScheduleID);
                }

                //기존에 없는데 생겼을떄
                if (hdRepeatChk.Text == "")
                {
                    bizDis.DelDisplayIDSolo(DisInfo);
                    AddRepeat(ScheduleID);
                }
            }
            else if (hdRepeatChk.Text == "N")
            {
                bizDis.DelDisplayIDSolo(DisInfo);
                DisInfo.CompanyID = CompanyID;
                DisInfo.UserID = CurrentUserInfo.UserID;
                DisInfo.StartDate = be.StartDate;
                DisInfo.EndDate = be.EndDate;
                if (DisInfo.StartDate == DisInfo.EndDate)
                {
                    DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
                }
                DisInfo.ScheduleID = ScheduleID;
                DisplayID = bizDis.AddDisplay(DisInfo);
            }
            else
            {
                if (hdRepeatChk.Text == "") //원래 반복설정이 없을때
                {
                    bizDis.DelDisplay(DisInfo);
                    DisInfo.CompanyID = CompanyID;
                    DisInfo.UserID = CurrentUserInfo.UserID;
                    DisInfo.StartDate = be.StartDate;
                    DisInfo.EndDate = be.EndDate;
                    if (DisInfo.StartDate == DisInfo.EndDate)
                    {
                        DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
                    }
                    DisInfo.ScheduleID = ScheduleID;
                    DisplayID = bizDis.AddDisplay(DisInfo);
                }
                else //반복설정이 있었는데 삭제 됬을때
                {
                    bizre.DelRepeatSchedule(infore); //반복설정 삭제
                    //현시점 이후 삭제
                    DisInfo.ScheduleID = GetDecryptParam("scheduleID", "0").ToNumber<int>();
                    DisInfo.StartDate = DateTime.Parse(be.StartDate.ToString("yyyy-MM-dd"));
                    bizDis.DelDisplayDate(DisInfo);

                    //bizDis.DelDisplay(DisInfo);
                    DisInfo.CompanyID = CompanyID;
                    DisInfo.UserID = CurrentUserInfo.UserID;
                    DisInfo.StartDate = be.StartDate;
                    DisInfo.EndDate = be.EndDate;
                    if (DisInfo.StartDate == DisInfo.EndDate)
                    {
                        DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
                    }
                    DisInfo.ScheduleID = ScheduleID;
                    DisplayID = bizDis.AddDisplay(DisInfo);
                }
            }

            #endregion 반복설정

            if (ScheduleID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                paraDic.Add("projectID", ProjectID.ToString());
                paraDic.Add("menuType", MenuType.ToString());
                paraDic.Add("rqProjectID", RqProjectID.ToString());
                paraDic.Add("status", Status.ToString());
                paraDic.Add("scheduleID", ScheduleID.ToString());
                paraDic.Add("DisplayID", DisplayID.ToString());

                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/PMS/Schedule/View.aspx");
                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// Dels the schedule repeat.
        /// </summary>
        /// <returns></returns>
        private bool DelScheduleRepeat()
        {
            DAL.Schedule bll = new Zeospace.DeskPlus.PMS.DAL.Schedule();

            //자신의 값일 경우
            return bll.Del(CompanyID, ProjectID, ScheduleID);
        }

        /// <summary>
        /// 내용 가져오기 //20070830 ByFreeman FCK 에디터 추가
        /// </summary>
        /// <returns></returns>
        private string SetEditorFileFolder()
        {
            //if (EditorName == "TAGFREE")
            //{
            //                return tbContentHTML.Text;
            //}
            //else
            //{
            //    return FCKEditor.GetFCKeditorValue();
            //}

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

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     :
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save"); //확인
                cbtnCancel.Text = resMng.GetString("Cancel"); //취소
                cbtnSetRepeat.Text = resMng.GetString("SetRepeat"); //반복설정
                cbtnRepCancel.Text = resMng.GetString("Cancel"); //반복취소

                ltTitleName.Text = resMng.GetString("Schedule_Header1");
                ltTime.Text = resMng.GetString("Schedule_Header2");
                ltScheduleRepeatInfo.Text = resMng.GetString("Schedule_Header4");
                ltAlarm.Text = resMng.GetString("Schedule_Header5");
            }
        }

        private void AddRepeat(int scheduleid)
        {
            BE.DisplayInfo DisInfo = new BE.DisplayInfo();
            BE.RepeatScheduleInfo RepInfo = new BE.RepeatScheduleInfo();

            DAL.Display bizDis = new DAL.Display();
            DAL.Repeat bizRep = new DAL.Repeat();

            BE.RepeatScheduleInfo info = new BE.RepeatScheduleInfo();

            string strDecord = Server.UrlDecode(hdRepeat.Text);
            string strdescrypt = Utility.DecryptString(strDecord);

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.RepeatType = UtilHelper.GetParmforParameter(strdescrypt, "RepeatType", "");
            info.SelectType = UtilHelper.GetParmforParameter(strdescrypt, "SelectType", "");
            info.IntervalCnt = UtilHelper.GetParmforParameter(strdescrypt, "IntervalCnt", "0").ToNumber<int>();
            info.MaxReptCnt = UtilHelper.GetParmforParameter(strdescrypt, "MaxReptCnt", "0").ToNumber<int>();
            info.ScheduleID = scheduleid;
            info.Option1 = UtilHelper.GetParmforParameter(strdescrypt, "Option1", "0");
            info.Option2 = UtilHelper.GetParmforParameter(strdescrypt, "Option2", "0");
            info.Option3 = UtilHelper.GetParmforParameter(strdescrypt, "Option3", "0");
            info.Option4 = UtilHelper.GetParmforParameter(strdescrypt, "Option4", "0");
            info.Option5 = UtilHelper.GetParmforParameter(strdescrypt, "Option5", "0");
            info.Option6 = UtilHelper.GetParmforParameter(strdescrypt, "Option6", "0");
            info.Option7 = UtilHelper.GetParmforParameter(strdescrypt, "Option7", "0");

            //info.EndDate =DateTime.Parse(UtilHelper.GetParmforParameter(strdescrypt, "EndDate", null));

            string strS = hdStart.Text.ToString() + " " + ddlStartTimeRepeat.SelectedValue.ToString();
            string strE = UtilHelper.GetParmforParameter(strdescrypt, "EndDate", "");
            string strE1 = hdEnd.Text.ToString() + " " + ddlEndTimeRepeat.SelectedValue.ToString();

            if (strE.Length > 0)
            {
                info.EndDate = DateTime.Parse(strE);
                info.EndDate = DateTime.Parse(info.EndDate.ToString("yyyy-MM-dd 23:59"));
            }
            else
            {
                strE = "2999-01-01";
                info.EndDate = DateTime.Parse(strE);
            }

            info.StartDate = DateTime.Parse(strS);

            BLL.RepeatScheduleProc bizRepeatBiz = new BLL.RepeatScheduleProc(info);

            //BLL.RepeatScheduleProc bizRepeatBiz = new BLL.RepeatScheduleProc(info);

            switch (info.RepeatType)
            {
                case "D":
                    bizRepeatBiz.RepeatDate();
                    break;

                case "W":
                    bizRepeatBiz.CalcWeekDayCount();
                    break;

                case "M":

                    // this.Option2 = 0~5   날, 평일,주말
                    //this.Option1 =1~5   첫째 둘째
                    if (info.MaxReptCnt > 0)
                    {
                        /// 매달 특정일에 회수 제한
                        if (info.SelectType == "1")
                        {
                            bizRepeatBiz.calcMonth1RepetitionByCount();
                        }
                        else
                        {
                            //매월 반복
                            bizRepeatBiz.calcMonth2RepetitionByCount();
                        }
                    }
                    else
                    {
                        if (info.SelectType == "1")
                        {
                            /// 매월 반복 - 특정일까지
                            bizRepeatBiz.calcMonth1RepetitionByTillDate();
                        }
                        else
                        {
                            //매월 반복 - 특정일까지
                            bizRepeatBiz.calcMonth2RepetitionByTillDate();
                        }
                    }
                    break;
            }

            List<BE.DisplayInfo> listobj = bizRepeatBiz.GetDateInfo();

            int iIndex = 0;

            foreach (BE.DisplayInfo _be in listobj)
            {
                _be.CompanyID = CurrentUserInfo.CompanyID;
                _be.ScheduleID = scheduleid;
                _be.UserID = UserID;
                _be.EndDate = _be.EndDate;

                string _strS = hdStart.Text.ToString() + " " + ddlStartTimeRepeat.SelectedValue.ToString();
                string _strE = hdStart.Text.ToString() + " " + ddlEndTimeRepeat.SelectedValue.ToString();
                DateTime _st = DateTime.Parse(_strS);
                DateTime _et = DateTime.Parse(_strE);

                if ((_st.ToString("yyyyMMdd").ToNumber<int>() - _et.ToString("yyyyMMdd").ToNumber<int>()) == 0)
                {
                    _be.EndDate = DateTime.Parse(_be.EndDate.ToString("yyyy-MM-dd ") + _et.ToString("HH:mm"));
                }
                else
                {
                    if (_be.StartDate == _be.EndDate)
                    {
                        _be.EndDate = _be.EndDate.AddHours(1);
                    }
                }

                if (iIndex == 0)
                {
                    DisplayID = bizDis.AddDisplay(_be);
                }
                else
                {
                    bizDis.AddDisplay(_be);
                }

                iIndex = iIndex + 1;
            }

            bizRep.AddRepeatSchedule(info);
        }

        private void PimAddRepeat(int scheduleid)
        {
            Pim.BE.DisplayInfo DisInfo = new Pim.BE.DisplayInfo();
            Pim.BE.RepeatScheduleInfo RepInfo = new Pim.BE.RepeatScheduleInfo();

            Pim.DAL.Display bizDis = new Pim.DAL.Display();
            Pim.DAL.RepeatSchedule bizRep = new Pim.DAL.RepeatSchedule();

            Pim.BE.RepeatScheduleInfo info = new Pim.BE.RepeatScheduleInfo();

            string strDecord = Server.UrlDecode(hdRepeat.Text);
            string strdescrypt = Utility.DecryptString(strDecord);

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.RepeatType = UtilHelper.GetParmforParameter(strdescrypt, "RepeatType", "");
            info.SelectType = UtilHelper.GetParmforParameter(strdescrypt, "SelectType", "");
            info.IntervalCnt = UtilHelper.GetParmforParameter(strdescrypt, "IntervalCnt", "0").ToNumber<int>();
            info.MaxReptCnt = UtilHelper.GetParmforParameter(strdescrypt, "MaxReptCnt", "0").ToNumber<int>();
            info.ScheduleID = scheduleid;
            info.Option1 = UtilHelper.GetParmforParameter(strdescrypt, "Option1", "0");
            info.Option2 = UtilHelper.GetParmforParameter(strdescrypt, "Option2", "0");
            info.Option3 = UtilHelper.GetParmforParameter(strdescrypt, "Option3", "0");
            info.Option4 = UtilHelper.GetParmforParameter(strdescrypt, "Option4", "0");
            info.Option5 = UtilHelper.GetParmforParameter(strdescrypt, "Option5", "0");
            info.Option6 = UtilHelper.GetParmforParameter(strdescrypt, "Option6", "0");
            info.Option7 = UtilHelper.GetParmforParameter(strdescrypt, "Option7", "0");

            //info.EndDate =DateTime.Parse(UtilHelper.GetParmforParameter(strdescrypt, "EndDate", null));

            string strS = hdStart.Text.ToString() + " " + ddlStartTimeRepeat.SelectedValue.ToString();
            string strE = UtilHelper.GetParmforParameter(strdescrypt, "EndDate", "");
            string strE1 = hdEnd.Text.ToString() + " " + ddlEndTimeRepeat.SelectedValue.ToString();

            if (strE.Length > 0)
            {
                info.EndDate = DateTime.Parse(strE);
                info.EndDate = DateTime.Parse(info.EndDate.ToString("yyyy-MM-dd 23:59"));
            }
            else
            {
                strE = "2999-01-01";
                info.EndDate = DateTime.Parse(strE);
            }

            info.StartDate = DateTime.Parse(strS);

            Pim.BLL.RepeatScheduleProc bizRepeatBiz = new Pim.BLL.RepeatScheduleProc(info);

            switch (info.RepeatType)
            {
                case "D":
                    bizRepeatBiz.RepeatDate();
                    break;

                case "W":
                    bizRepeatBiz.CalcWeekDayCount();
                    break;

                case "M":

                    // this.Option2 = 0~5   날, 평일,주말
                    //this.Option1 =1~5   첫째 둘째
                    if (info.MaxReptCnt > 0)
                    {
                        /// 매달 특정일에 회수 제한
                        if (info.SelectType == "1")
                        {
                            bizRepeatBiz.calcMonth1RepetitionByCount();
                        }
                        else
                        {
                            //매월 반복
                            bizRepeatBiz.calcMonth2RepetitionByCount();
                        }
                    }
                    else
                    {
                        if (info.SelectType == "1")
                        {
                            /// 매월 반복 - 특정일까지
                            bizRepeatBiz.calcMonth1RepetitionByTillDate();
                        }
                        else
                        {
                            //매월 반복 - 특정일까지
                            bizRepeatBiz.calcMonth2RepetitionByTillDate();
                        }
                    }
                    break;
            }

            List<Pim.BE.DisplayInfo> listobj = bizRepeatBiz.GetDateInfo();

            int iIndex = 0;

            foreach (Pim.BE.DisplayInfo _be in listobj)
            {
                _be.CompanyID = CurrentUserInfo.CompanyID;
                _be.ScheduleID = scheduleid;
                _be.UserID = UserID;
                _be.EndDate = _be.EndDate;

                string _strS = hdStart.Text.ToString() + " " + ddlStartTimeRepeat.SelectedValue.ToString();
                string _strE = hdStart.Text.ToString() + " " + ddlEndTimeRepeat.SelectedValue.ToString();
                DateTime _st = DateTime.Parse(_strS);
                DateTime _et = DateTime.Parse(_strE);

                if ((_st.ToString("yyyyMMdd").ToNumber<int>() - _et.ToString("yyyyMMdd").ToNumber<int>()) == 0)
                {
                    _be.EndDate = DateTime.Parse(_be.EndDate.ToString("yyyy-MM-dd ") + _et.ToString("HH:mm"));
                }
                else
                {
                    if (_be.StartDate == _be.EndDate)
                    {
                        _be.EndDate = _be.EndDate.AddHours(1);
                    }
                }

                if (iIndex == 0)
                {
                    bizDis.AddDisplay(_be);
                }
                else
                {
                    bizDis.AddDisplay(_be);
                }

                iIndex = iIndex + 1;
            }

            bizRep.AddRepeatSchedule(info);
        }

        #endregion Method (함수)

        #region Property (프로퍼티)

        /// <summary>
        /// Gets or sets the target year.
        /// </summary>
        /// <value>The target year.</value>
        public int TargetYear
        {
            get
            {
                return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : ViewState["targetYear"].ToString().ToNumber<int>();
            }
            set
            {
                ViewState["targetYear"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the target month.
        /// </summary>
        /// <value>The target month.</value>
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : ViewState["targetMonth"].ToString().ToNumber<int>(); }
            set { ViewState["targetMonth"] = value; }
        }

        /// <summary>
        /// TargetDay
        /// </summary>
        /// <value></value>
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : ViewState["targetDay"].ToString().ToNumber<int>(); }
            set { ViewState["targetDay"] = value; }
        }

        /// <summary>
        /// 태그프리 경로
        /// </summary>
        /// <value>The activex tagfree.</value>
        //public string ActivexTagfree
        //{
        //    get
        //    {
        //        return (EditorName == "TAGFREE") ? "<script src=\"/DeskPlusEIP/Html/Js/IEActiveX/activexTagfreeNoImage.js\"></script>" : "";
        //    }
        //}

        /// <summary>
        /// Gets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get { return GetViewStateInt("CompanyID", "1"); }
            set { ViewState["CompanyID"] = value; }
        }

        /// <summary>
        /// Get the UserID
        /// </summary>
        public int UserID
        {
            get { return GetViewStateInt("UserID", "0"); }
            set { ViewState["UserID"] = value; }
        }

        /// <summary>
        /// Get the UserName
        /// </summary>
        public string UserName
        {
            get { return GetViewStateString("UserName", "0"); }
            set { ViewState["UserName"] = value; }
        }

        /// <summary>
        /// Gets the user grade.
        /// </summary>
        /// <value>The user grade.</value>
        public int UserGrade
        {
            get { return Request.Cookies["PMS"]["UserGrade"].ToNumber<int>(); }
        }

        /// <summary>
        /// TagFree의 Mime 을 사용하여 이미지를 업로드 할지 (Y : Mime 을 사용 , N : 업로드 컴포넌트를 사용) - 원인은 중국서버에서 Mime의 이상작동
        /// </summary>
        public string MimeUseChk
        {
            get
            {
                try
                {
                    return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "MimeUseChk", "N");
                }
                catch
                {
                    return "Y";
                }
            }
        }

        /// <summary>
        /// 업로드할 대상 경로
        /// </summary>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }

        /// <summary>
        /// 반복 : 1, 일반 : 0
        /// </summary>
        /// <value>The is repeat.</value>
        public string IsRepeat
        {
            get { return GetViewStateString("IsRepeat", ""); }
            set { ViewState["IsRepeat"] = value; }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get { return GetDecryptParam("status", ""); }
        }

        /// <summary>
        /// ActiveX 유무
        /// </summary>
        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }

        #endregion Property (프로퍼티)

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 저장버튼
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void lbtnSubmit_Click(object sender, System.EventArgs e)
        {
            //추가일 경우
            ModSchedule();
        }

        #endregion Event Handler (이벤트 핸들러)
    }
}