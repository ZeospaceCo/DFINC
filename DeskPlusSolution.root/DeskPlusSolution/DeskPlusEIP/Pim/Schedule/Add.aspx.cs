using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;
using System.Collections.Specialized;
using System.Collections;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    public partial class Add : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int DisplayID = 0;
        public int CompanyID = 0;
        public int UserID = 0;
        private string eventArgs = string.Empty;
        public string strStartdate = string.Empty;
        public int ScheduleID = 0;
        public string Searchkey = string.Empty;
        public string SearchValue = string.Empty;
        public int CurrentPage = 1;
        public string Starttime = string.Empty;
        public string Endtime = string.Empty;
        public string Order = string.Empty;
                
        public string PageUrl = string.Empty;

        public ArrayList arrDisplay = new ArrayList();

        //검색어
        public string s_Distance = string.Empty;
        public string s_inquryPerson = string.Empty;
        public string s_DatePeriod = string.Empty;
        public string s_SearchKind = string.Empty;
        public string s_SerachValue = string.Empty;
        public string s_StartDate = string.Empty;
        public string s_EndDate = string.Empty;
        public string s_UserID = string.Empty;

        public string s_PageSize = string.Empty;
        public string s_CurrentPage = string.Empty;


        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.Add).Assembly;	//어셈블리 선언
            #endregion            

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
                SettingControl();
                LoadStatus();
                SetAlarm();
            }

        }

        private void LoadData()
        {
            LoadParam();

            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 쓰기 권한 체크
            #endregion

            

            #region 에디터 설정
            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang,"");
            string templateFooter = "</body></html>";

            tbHiddenMime_contents.Text = templateHeader + templateFooter;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                //ByFreeman 20120106 X-Free 에디터 추가 
                /*
                string strScript = String.Empty;
                if (Request.Path.IndexOf("Add.aspx") > 0)
                {                    
                    strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.htmlValue = form1.tbHiddenMime_contents.value;" + "<" + "/script>";

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
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Pim/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로
                #endregion
            }
            #endregion

        }

        #endregion 


        #region Method (함수)

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            

            ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();
            Searchkey = GetDecryptParam("Searchkey", "");
            SearchValue = GetDecryptParam("SearchValue", "");
            CurrentPage = GetDecryptParam("CurrentPage", "1").ToNumber<int>(); ;
            Starttime = GetDecryptParam("Starttime", "");
            Endtime = GetDecryptParam("Endtime", "");
            Order = GetDecryptParam("order", "");
            PageUrl = GetDecryptParam("Page", "");

            s_Distance = GetDecryptParam("s_Distance", "");
            s_inquryPerson = GetDecryptParam("s_inquryPerson", "");
            s_DatePeriod = GetDecryptParam("s_DatePeriod", "");
            s_SearchKind = GetDecryptParam("s_SearchKind", "");
            s_SerachValue = GetDecryptParam("s_SerachValue", "");
            s_StartDate = GetDecryptParam("s_StartDate", "");
            s_EndDate = GetDecryptParam("s_EndDate", "");
            s_UserID = GetDecryptParam("s_UserID", "");

            s_PageSize = GetDecryptParam("s_PageSize", "");
            s_CurrentPage = GetDecryptParam("s_CurrentPage", "");
        }

        /// <summary>
        /// 분류 리스트 가져오기
        /// </summary>
        private void LoadStatus()
        {
            DAL.Status dal = new DAL.Status();
            DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, "100", "0", "", true);

            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(new ListItem("--------", "0"));
            ddlStatus.AppendDataBoundItems = true;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Pim.BE.StatusInfo info = new BE.StatusInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.StatusID = dtr["StatusID"].ToString().ToNumber<int>();
                        info.Orders = dtr["Orders"].ToString().ToNumber<int>();
                        info.NameBase = dtr["NameBase"].ToString();
                        info.NameKo = dtr["NameKo"].ToString();
                        info.NameEn = dtr["NameEn"].ToString();
                        info.NameJa = dtr["NameJa"].ToString();
                        info.NameFr = dtr["NameFr"].ToString();
                        info.NameZhcn = dtr["NameZhcn"].ToString();
                        info.NameZhtw = dtr["NameZhtw"].ToString();
                        info.Colors = dtr["Colors"].ToString();
                        info.UseActive = dtr["UseActive"].ToString();
                        info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();

                        info.NameBase = this.GetLangName(
                                    info.NameBase.ToNull(), info.NameKo.ToNull(),
                                    info.NameEn.ToNull(), info.NameJa.ToNull(),
                                    info.NameFr.ToNull(), info.NameZhcn.ToNull(),
                                    info.NameZhtw.ToNull(), PreLang);

                        ListItem tmpItem = new ListItem(info.NameBase, info.StatusID.ToString());
                        tmpItem.Attributes.Add("style", "color:" + info.Colors);
                        ddlStatus.Items.Add(tmpItem);

                    }
                }
            }
        }

        /// <summary>
        /// Settings the control.
        /// </summary>
        private void SettingControl()
        {
            startDate.Value = DateTime.Now.ToShortDateString();
            endDate.Value = DateTime.Now.ToShortDateString();
            strStartdate = startDate.Value;

            //알람 - 코드관리로 처리
            DataView dvPim = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AlarmPims");
            ddlnotikind.Items.Clear();
            ddlnotikind.AppendDataBoundItems = true;
            string name = "", vals = "";

            ddlnotikind.Items.Add(new ListItem(name, vals));
            foreach (DataRowView dr in dvPim)
            {
                name = this.GetLangName(
                    dr["NameBase"].ToString(),
                    dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(),
                    dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                // 코드값이 특정 약어로 처리되는 경우, interfacecode를 이용
                vals = (dr["InterfaceCode"].ToString().ToNull().Trim().Equals("")) ? dr["CodeID"].ToString() : dr["InterfaceCode"].ToString().ToNull().Trim();
                ddlnotikind.Items.Add(new ListItem(name, vals));
            }

            //알람 시간
            for (int i = 1; i < 12; i++)
            {
                ListItem ltime1 = new ListItem((i * 5).ToString() + GetResource("Minute"), (i * 5).ToString());
                ddltime.Items.Add(ltime1);
            }

            ListItem ltime2 = new ListItem(GetResource("OneClock"), "60");
            ListItem ltime3 = new ListItem(GetResource("TwoClock"), "120");
            ListItem ltime4 = new ListItem(GetResource("ThreeClock"), "180");
            ListItem ltime5 = new ListItem(GetResource("FourClock"), "240");
            ListItem ltime6 = new ListItem(GetResource("FiveClock"), "300");
            ListItem ltime7 = new ListItem(GetResource("SixClock"), "360");

            ListItem ltime8 = new ListItem(GetResource("OneDay"), "1440");
            ListItem ltime9 = new ListItem(GetResource("TwoDay"), "2880");
            ListItem ltime10 = new ListItem(GetResource("ThreeDay"), "4320");
            ListItem ltime11 = new ListItem(GetResource("FourDay"), "5760");
            ListItem ltime12 = new ListItem(GetResource("FiveDay"), "7200");
            ListItem ltime13 = new ListItem(GetResource("SixDay"), "8640");
            ListItem ltime14 = new ListItem(GetResource("SevenDay"), "10080");


            ddltime.Items.Add(ltime2);
            ddltime.Items.Add(ltime3);
            ddltime.Items.Add(ltime4);
            ddltime.Items.Add(ltime5);
            ddltime.Items.Add(ltime6);
            ddltime.Items.Add(ltime7);
            ddltime.Items.Add(ltime8);
            ddltime.Items.Add(ltime9);
            ddltime.Items.Add(ltime10);
            ddltime.Items.Add(ltime11);
            ddltime.Items.Add(ltime12);
            ddltime.Items.Add(ltime13);
            ddltime.Items.Add(ltime14);


            ListItem ltcate1 = new ListItem(GetResource("pim1"), "0");
            ListItem ltcate2 = new ListItem(GetResource("pim2"), "3");
            ListItem ltcate3 = new ListItem(GetResource("pim3"), "1");
            ListItem ltcate4 = new ListItem(GetResource("pim4"), "2");
            

            ddlCategory.Items.Add(ltcate1);
            ddlCategory.Items.Add(ltcate2);
            ddlCategory.Items.Add(ltcate3);
            ddlCategory.Items.Add(ltcate4);
            MakeTime();

            //기본게시범위설정
            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "PimViewType");
            string codeID = "2";
            for (int i = 0; i < dv.Count; i++)
            {
                codeID = dv[i]["CodeID"].ToString();
            }
            ddlCategory.SelectedValue = codeID;

        }


        private void MakeTime()
        {
            string strVal = string.Empty;
            string strKey = string.Empty;

            for (int i = 0; i < 24; i++)
            {
                if (i < 12)
                {
                   
                    strVal = GetResource("AM")+" ";

                    if (i == 0)
                    {
                        strVal = strVal + " 12";
                    }
                    else
                    {
                        strVal = strVal +" "+ i.ToString();
                    }
                }
                else
                {
                    strVal = GetResource("PM") + " ";
                    if (i == 12)
                    {
                        strVal = strVal + " 12";
                    }
                    else
                    {
                        strVal = strVal + " " + (i - 12).ToString();
                    }
                }

                string strTemp = string.Empty;
                strTemp =strVal + ":00";
                strKey =i.ToString()+":00";
                ListItem lv1 = new ListItem(strTemp, strKey);
                ddlstartTime.Items.Add(lv1);
                ddlendTime.Items.Add(lv1);

                strTemp = strVal + ":30";
                strKey = i.ToString() + ":30";
                ListItem lv2 = new ListItem(strTemp, strKey);
                ddlstartTime.Items.Add(lv2);
                ddlendTime.Items.Add(lv2);
            }
            if (DateTime.Now.Hour < 10)
            {
                ddlstartTime.SelectedValue = DateTime.Now.ToString("H:00");
            }
            else
            {
                ddlstartTime.SelectedValue = DateTime.Now.ToString("HH:00");
            }            
            
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnContinue.Text = GetResource("SaveContinue");
                CbtnConfirm.Text = GetResource("Save");
                CbtnCancel.Text = GetResource("Cancel");                
                cbtnRepeat.Text = GetResource("Setting");
                cbtnRepCancel.Text = GetResource("Cancel");
                CbtnSetting.Text = GetResource("AlarmSetting");
                     
            }
        }

        /// <summary>
        /// 기본 알림 설정 가져오기 
        /// </summary>
        private void SetAlarm()
        {
            DAL.Alarm bll = new DAL.Alarm();
            DataTableReader dtr = bll.GetSetting(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ddlnotikind.SelectedValue = dtr["Kind"].ToString();

                        if (dtr["Kind"].ToString() == "M" || dtr["Kind"].ToString() == "B")
                        {
                            ddltime.SelectedValue = dtr["Time"].ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 일정 추가
        /// </summary>
        private void AddSchedule(string arg)
        {
            try
            {
                Pim.BE.ScheduleInfo info = new BE.ScheduleInfo();
                info.CompanyID = CompanyID;
                info.ToDoID = 0;
                info.Type = "I";//ddlCategory.SelectedValue.ToString();                
                //제목
                info.Title = txttitle.Value.ToString().Trim();
                //날짜
                string strS = startDate.Value.ToString() + " " + ddlstartTime.SelectedValue.ToString();
                string strE = endDate.Value.ToString() + " " + ddlendTime.SelectedValue.ToString();

                info.StartTime = DateTime.Parse(strS);
                info.EndTime = DateTime.Parse(strE);
                //내용
                info.Content = GetContents();
                if (hdRepeat.Text.Trim().Length > 0)
                    info.Repeat = "Y";
                else
                    info.Repeat ="N";

                if (!ddlnotikind.SelectedValue.ToString().Equals(""))
                {
                    info.Alert = "Y";
                    info.AlertType = ddlnotikind.SelectedValue.ToString();

                    if (!ddlnotikind.SelectedValue.ToString().Equals("E"))
                    {
                        info.AlertTime = int.Parse(ddltime.SelectedValue.ToString());
                        info.AlertTargetAddress = "";
                    }
                    else
                    {
                        info.AlertTime = 0;
                        info.AlertTargetAddress = idEmail.Value.ToString();
                    }
                }
                else
                {
                    info.Alert = "N";
                    info.AlertType = "";
                    info.AlertTime = 0;
                    info.AlertTargetAddress = "";
                }
                
                info.IsOpen = "Y";//사용안함
                info.UserID = CurrentUserInfo.UserID;
                info.DeptID = CurrentUserInfo.DeptID;
                info.OpenType = ddlCategory.SelectedValue.ToString();
                info.ReservationID = 0;
                info.Lunar = "N";  //양력,음력                
                info.UserName = CurrentUserInfo.Name;
                info.StatusID = ddlStatus.SelectedValue.ToNumber<int>();

                info.AlarmContent = tbHiddenText_contents.Text.Trim();
                //tbHiddenText_contents.
                //첨부파일
                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
                info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);
                
                #region 반복설정

                 int scheduleid = 0;
                 if (hdRepeat.Text.Trim().Length > 0)
                 {
                     scheduleid = AddRepeat(info);
                 }
                 else
                 {
                     Pim.DAL.Schedule bll = new DAL.Schedule();
                     scheduleid = bll.Add(info);

                     BE.DisplayInfo DisInfo = new BE.DisplayInfo();
                     DAL.Display bizDis = new DAL.Display();
                     DisInfo.CompanyID = CompanyID;
                     DisInfo.UserID = CurrentUserInfo.UserID;
                     DisInfo.StartDate = info.StartTime;
                     DisInfo.EndDate = info.EndTime;
                     if (DisInfo.StartDate == DisInfo.EndDate)
                     {
                         DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
                     }

                     DisInfo.ScheduleID = scheduleid;
                     DisplayID = bizDis.AddDisplay(DisInfo);

                     arrDisplay.Add(DisplayID + "^" + DisInfo.StartDate + "^" + DisInfo.EndDate);
                 }
                 #endregion

                #region 첨부파일 추가
                 Hashtable ht = new Hashtable();
                 if (info.HiddenSpoolPath != "" && info.HiddenSpoolPath != null)
                 {
                     HttpContext context = HttpContext.Current;
                     DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(info.HiddenSpoolPath));
                     if (di.Exists)
                     {
                         ht = AddListHash(CompanyID, scheduleid, info.HiddenSpoolPath);
                     }
                 }

                 #endregion

                #region DOC_STREAM
                 var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "DocStreamYN", "N");

                 if (docStreamYN == "Y")
                 {
                     DAL.Attach dalFile = new DAL.Attach();
                     DocStream ds = new DocStream();

                     BE.AttachInfo fileInfo = new BE.AttachInfo();
                     fileInfo.CompanyID = CompanyID;
                     fileInfo.ScheduleID = scheduleid;
                     
                     try
                     {
                         var newDSList = dalFile.GetAttachInfo(fileInfo);
                         for (int i = 0; i < newDSList.Count; i++)
                         {
                             ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachID, "PIM_ATTACH");
                             ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachID, "PIM_ATTACH");
                         }
                     }
                     catch (Exception ex)
                     {                         
                         var DSList = dalFile.GetAttachInfo(fileInfo);
                         for (int i = 0; i < DSList.Count; i++)
                         {
                             ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachID, "PIM_ATTACH");
                         }
                         Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PIM_Attach_Add_DS : " + ex.Message);
                         throw;
                     }

                 }
                 #endregion

                #region 공유설정

                 BE.ScheShareListInfo data = new BE.ScheShareListInfo();

                 data.CompanyID = CurrentUserInfo.CompanyID;
                 data.ScheduleID = scheduleid;
                 data.UserID = CurrentUserInfo.UserID;
                 data.UserMail = string.Empty;
                 data.DeptID = 0;
                 data.UserName = CurrentUserInfo.Name;


                 DAL.ScheShareList biz = new DAL.ScheShareList();
                 //biz.AddScheShareLis(data);
                 data.UserID = 0;
                 data.DeptID = 0;
                 data.UserName = string.Empty;



                 string strTemp = hdShareList.Text.Trim();
                 if (strTemp.Length > 0)
                 {
                     string [] arVal=strTemp.Split(',');
                     foreach(string _be  in arVal)
                     {
                             

                        string [] arSource =    _be.Split('$');
                        if (arSource.Length > 1)
                        {
                            if (arSource[0].Equals("G"))
                            {
                                //그룹
                                data.UserID = 0;
                                data.DeptID = int.Parse(arSource[1]);
                                data.UserName = arSource[2];
                            }
                            else if (arSource[0].Equals("M"))
                            {
                                //유저공유
                                data.UserID = int.Parse(arSource[1]);
                                data.DeptID = 0;
                                data.UserName = arSource[2];
                            }
                            

                            biz.AddScheShareLis(data);
                        }
                     }
                 }


                 #endregion

                #region 알람 

                 if (scheduleid > 0)
                 {
                     Pim.BLL.Schedule _abill = new BLL.Schedule();

                     if (ddlnotikind.SelectedValue.ToString().Length > 0)
                     {                         
                         if (ddlnotikind.SelectedValue == "M" || ddlnotikind.SelectedValue == "B")
                         {
                             for (int i = 0; i < arrDisplay.Count; i++)
                             {
                                 string[] ArrTmp = arrDisplay[i].ToString().Split('^');

                                 Dictionary<string, string> paraDical = new Dictionary<string, string>();
                                 paraDical.Add("DisplayID", ArrTmp[0].ToString());
                                 paraDical.Add("CompanyID", CurrentUserInfo.CompanyID.ToString());
                                 paraDical.Add("ScheduleID", scheduleid.ToString());
                                 info.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                                 info.SenderMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                                 info.StartTime = DateTime.Parse(ArrTmp[1]);
                                 info.EndTime = DateTime.Parse(ArrTmp[2]);

                                 _abill.SendAlarm(info, scheduleid, CurrentUserInfo.UserID, ht, ArrTmp[0].ToNumber<int>());
                             }
                         }
                     }

                     if (strTemp.Length > 0)
                     {
                         string[] arVal = strTemp.Split(',');
                         if (info.AlertTargetAddress.Trim() != "")
                         {
                             info.AlertTargetAddress += ";";
                         }

                         foreach (string _be in arVal)
                         {
                             string[] arSource = _be.Split('$');
                             if (arSource.Length > 1)
                             {
                                 if (arSource[0].Equals("G"))
                                 {
                                     //그룹                                 
                                     data.DeptID = int.Parse(arSource[1]);
                                     DataTableReader shdt = biz.GetShareListInfoDept(CompanyID, data.DeptID);
                                     if (shdt != null)
                                     {
                                         if (shdt.HasRows)
                                         {
                                             while (shdt.Read())
                                             {
                                                 info.RecvName = this.GetLangName(shdt["NameBase"].ToString(), shdt["NameKo"].ToString(),
                                                         shdt["NameEn"].ToString(), shdt["NameJa"].ToString(), shdt["NameFr"].ToString(),
                                                         shdt["NameZhcn"].ToString(), shdt["NameZhtw"].ToString(), PreLang
                                                         );

                                                 // 사용자가 메일 알림을 받겠다고 설정한 사람한테만 발송
                                                 if (shdt["AlarmPims"].ToString().ToUpper().IndexOf("MAIL") > 0)
                                                 {
                                                     if ((shdt["MailID"].ToString() != "") && (shdt["MailDomain"].ToString() != ""))
                                                     {
                                                         info.AlertTargetAddress += shdt["MailID"].ToString() + "@" + shdt["MailDomain"].ToString() + ";";
                                                     }
                                                 }

                                                 if (ddlnotikind.SelectedValue == "M" || ddlnotikind.SelectedValue == "B")
                                                 {
                                                     for (int i = 0; i < arrDisplay.Count; i++)
                                                     {
                                                         string[] ArrTmp = arrDisplay[i].ToString().Split('^');

                                                         Dictionary<string, string> paraDical = new Dictionary<string, string>();
                                                         paraDical.Add("DisplayID", ArrTmp[0].ToString());
                                                         paraDical.Add("CompanyID", CurrentUserInfo.CompanyID.ToString());
                                                         paraDical.Add("ScheduleID", scheduleid.ToString());
                                                         info.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                                                         info.SenderMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                                                         info.StartTime = DateTime.Parse(ArrTmp[1]);
                                                         info.EndTime = DateTime.Parse(ArrTmp[2]);

                                                         _abill.SendAlarm(info, scheduleid, shdt["userid"].ToString().ToNumber<int>(), ht, ArrTmp[0].ToNumber<int>());
                                                     }
                                                 }
                                             }
                                         }
                                     }

                                 }
                                 else if (arSource[0].Equals("M"))
                                 {
                                     //유저공유
                                     data.UserID = int.Parse(arSource[1]);
                                     DataTableReader shdt = biz.GetShareListInfoUser(CompanyID, data.UserID);
                                     if (shdt != null)
                                     {
                                         if (shdt.HasRows)
                                         {
                                             while (shdt.Read())
                                             {

                                                 info.RecvName = this.GetLangName(shdt["NameBase"].ToString(), shdt["NameKo"].ToString(),
                                                         shdt["NameEn"].ToString(), shdt["NameJa"].ToString(), shdt["NameFr"].ToString(),
                                                         shdt["NameZhcn"].ToString(), shdt["NameZhtw"].ToString(), PreLang
                                                         );

                                                 // 사용자가 메일 알림을 받겠다고 설정한 사람한테만 발송
                                                 if (shdt["AlarmPims"].ToString().ToUpper().IndexOf("MAIL") > 0)
                                                 {
                                                     if ((shdt["MailID"].ToString() != "") && (shdt["MailDomain"].ToString() != ""))
                                                     {
                                                         info.AlertTargetAddress += shdt["MailID"].ToString() + "@" + shdt["MailDomain"].ToString() + ";";
                                                     }
                                                 }

                                                 if (ddlnotikind.SelectedValue == "M" || ddlnotikind.SelectedValue == "B")
                                                 {
                                                     for (int i = 0; i < arrDisplay.Count; i++)
                                                     {
                                                         string[] ArrTmp = arrDisplay[i].ToString().Split('^');

                                                         Dictionary<string, string> paraDical = new Dictionary<string, string>();
                                                         paraDical.Add("DisplayID", ArrTmp[0].ToString());
                                                         paraDical.Add("CompanyID", CurrentUserInfo.CompanyID.ToString());
                                                         paraDical.Add("ScheduleID", scheduleid.ToString());
                                                         info.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                                                         info.SenderMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                                                         info.StartTime = DateTime.Parse(ArrTmp[1]);
                                                         info.EndTime = DateTime.Parse(ArrTmp[2]);

                                                         _abill.SendAlarm(info, scheduleid, shdt["userid"].ToString().ToNumber<int>(), ht, ArrTmp[0].ToNumber<int>());
                                                     }
                                                 }

                                             }
                                         }
                                     }
                                 }
                             }
                         }                        
                     }

                     //매일알람을 선택했을경우
                     if ((ddlnotikind.SelectedValue == "E") && (info.AlertTargetAddress != ""))
                     {
                         info.Title = "[" + GetResource("PimAlarm") + "]" + info.StartTime + "~" + info.EndTime + " " + info.Title;
                         _abill.SendAlarm(info, 0, 0, ht,0);
                     }
                 }

               
                #endregion

                if (arg.Equals("SAVE"))
                {
                    if (scheduleid > 0)
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("DisplayID", DisplayID.ToString());
                        paraDic.Add("PageScope", PageScope);
                        paraDic.Add("Date", Date);
                        paraDic.Add("Opentype", Opentype);
                        paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                        paraDic.Add("alertMsg", GetResource("GoList"));

                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/View.aspx");
                        string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                        Utility.Redirect(encParams);
                    }
                    else
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();

                        paraDic.Add("PageScope", PageScope);
                        paraDic.Add("Date", Date);
                        paraDic.Add("Opentype", Opentype);                        
                        paraDic.Add("alertMsg", GetResource("NoRepeatAdd"));
                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/AddN.aspx");

                        /*
                        if (NonActiveX == "T") //activex
                        {
                            paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/AddN.aspx");
                        }
                        else
                        {
                            paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/Add.aspx");
                        }
                        */

                        string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                        Utility.Redirect(encParams);        
                    }
                    
                }
                else if (arg.Equals("CONTINUE"))
                {
                   

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();

                    paraDic.Add("PageScope", PageScope);
                    paraDic.Add("Date", Date);
                    paraDic.Add("Opentype", Opentype);                    
                    paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                    paraDic.Add("alertMsg", GetResource("CONTINUEADD"));
                    paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/AddN.aspx");

                    /*
                    if (NonActiveX == "T") //activex
                    {
                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/AddN.aspx");
                    }
                    else
                    {
                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/Add.aspx");
                    }
                    */

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    Utility.Redirect(encParams);              
                }
                

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Anniversary", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        private int AddRepeat(Pim.BE.ScheduleInfo _info)
        {
            int scheduleid = 0;

            BE.DisplayInfo DisInfo = new BE.DisplayInfo();
            BE.RepeatScheduleInfo RepInfo = new BE.RepeatScheduleInfo();
            
            DAL.Display bizDis = new DAL.Display();
            DAL.RepeatSchedule bizRep = new DAL.RepeatSchedule();

            BE.RepeatScheduleInfo info = new BE.RepeatScheduleInfo();

            string strDecord =  Server.UrlDecode(hdRepeat.Text);
            string strdescrypt = Utility.DecryptString(strDecord);


            info.CompanyID = CurrentUserInfo.CompanyID;
            info.RepeatType = UtilHelper.GetParmforParameter(strdescrypt, "RepeatType", "");
            info.SelectType = UtilHelper.GetParmforParameter(strdescrypt, "SelectType", "");
            info.IntervalCnt = UtilHelper.GetParmforParameter(strdescrypt, "IntervalCnt", "0").ToNumber<int>();
            info.MaxReptCnt = UtilHelper.GetParmforParameter(strdescrypt, "MaxReptCnt", "0").ToNumber<int>();
            info.Option1 = UtilHelper.GetParmforParameter(strdescrypt, "Option1", "0");
            info.Option2 = UtilHelper.GetParmforParameter(strdescrypt, "Option2", "0");
            info.Option3 = UtilHelper.GetParmforParameter(strdescrypt, "Option3", "0");
            info.Option4 = UtilHelper.GetParmforParameter(strdescrypt, "Option4", "0");
            info.Option5 = UtilHelper.GetParmforParameter(strdescrypt, "Option5", "0");
            info.Option6 = UtilHelper.GetParmforParameter(strdescrypt, "Option6", "0");
            info.Option7 = UtilHelper.GetParmforParameter(strdescrypt, "Option7", "0");

            //info.EndDate =DateTime.Parse(GetParmforParameter(strdescrypt, "EndDate", null));

            string strS = startDate.Value.ToString() + " " + ddlstartTime.SelectedValue.ToString();
            string strE = UtilHelper.GetParmforParameter(strdescrypt, "EndDate", "");
            string strE1 = endDate.Value.ToString() + " " + ddlendTime.SelectedValue.ToString();

            if (strE.Length > 0)
            {
                info.EndDate = DateTime.Parse(strE);
                info.EndDate = DateTime.Parse(info.EndDate.ToString("yyyy-MM-dd 23:59") );
            }
            else
            {
                strE = "2999-01-01";
                info.EndDate = DateTime.Parse(strE);                
            }

            info.StartDate = DateTime.Parse(strS);

            Biz.RepeatScheduleProc bizRepeatBiz = new Biz.RepeatScheduleProc(info);

          
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

           List< BE.DisplayInfo> listobj = bizRepeatBiz.GetDateInfo();

           int iIndex = 0;

           if (listobj.Count > 0)
           {
               Pim.DAL.Schedule bll = new DAL.Schedule();
               scheduleid = bll.Add(_info);

               info.ScheduleID = scheduleid;

               foreach (BE.DisplayInfo _be in listobj)
               {
                   _be.CompanyID = CurrentUserInfo.CompanyID;
                   _be.ScheduleID = scheduleid;
                   _be.UserID = UserID;
                   _be.EndDate = _be.EndDate;

                   string _strS = startDate.Value.ToString() + " " + ddlstartTime.SelectedValue.ToString();
                   string _strE = endDate.Value.ToString() + " " + ddlendTime.SelectedValue.ToString();
                   DateTime _st = DateTime.Parse(_strS);
                   DateTime _et = DateTime.Parse(_strE);

                   _be.StartDate = DateTime.Parse(_be.StartDate.ToString("yyyy-MM-dd ") + _st.ToString("HH:mm"));

                   if ((_st.ToString("yyyyMMdd").ToNumber<int>() - _et.ToString("yyyyMMdd").ToNumber<int>()) == 0)
                   {
                       _be.EndDate = DateTime.Parse(_be.EndDate.ToString("yyyy-MM-dd ") + _et.ToString("HH:mm"));
                   }
                   else
                   {
                       //if (_be.StartDate == _be.EndDate)
                       if ((_be.StartDate.ToString("yyyyMMdd").ToNumber<int>() - _be.StartDate.ToString("yyyyMMdd").ToNumber<int>()) == 0)
                       {
                           _be.EndDate = _be.StartDate.AddHours(1);
                       }
                   }

                   if (iIndex == 0)
                   {
                       DisplayID = bizDis.AddDisplay(_be);
                       arrDisplay.Add(DisplayID + "^" + _be.StartDate + "^" + _be.EndDate);
                   }
                   else
                   {
                       int tmpDisplayID = bizDis.AddDisplay(_be);
                       arrDisplay.Add(tmpDisplayID + "^" + _be.StartDate + "^" + _be.EndDate);
                   }

                   iIndex = iIndex + 1;
               }

               bizRep.AddRepeatSchedule(info);
           }

           return scheduleid;

        }

        /// <summary>
        /// 작성자 : 심재운
        /// 본문 내용 및 본문 이미지(업로드) MIME 하여 본문 내용가져오기
        /// </summary>
        /// <returns></returns>
        private string GetContents()
        {
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
        /// 첨부파일
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_scheduleid"></param>
        /// <param name="_spoolPath"></param>
        /// <returns></returns>
        public Hashtable AddListHash(int _companyID, int _scheduleid, string _spoolPath)
        {
            Hashtable ht = new Hashtable();
            ArrayList fileList = FileHelper.GetUploadFile(_spoolPath, "", FolderType.Pims, _scheduleid, true, "");
            DAL.Attach adal = new DAL.Attach();
            if (fileList != null)
            {
                fileList.Sort(new FileHelper.FileSortComparer(FileHelper.SortDirection.Asc));
                for (int i = 0; i < fileList.Count; i++)
                {
                    BE.AttachInfo fileInfo = new BE.AttachInfo();
                    fileInfo.CompanyID = _companyID;

                    fileInfo.ScheduleID = _scheduleid;
                    fileInfo.FilePath = ((UploadFileInfo)fileList[i]).filePath;
                    fileInfo.FileName = ((UploadFileInfo)fileList[i]).fileName;
                    fileInfo.FileType = ((UploadFileInfo)fileList[i]).fileType;
                    fileInfo.FileLength = ((UploadFileInfo)fileList[i]).fileLength;
                    fileInfo.FileGUID = ((UploadFileInfo)fileList[i]).fileGUID;

                    adal.Add(fileInfo);

                    ht.Add(
                        Server.MapPath(fileInfo.FilePath +fileInfo.FileName ),
                        Server.MapPath(fileInfo.FilePath + fileInfo.FileGUID)                       
                    );
                }
            }

            return ht;
        }



        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        protected void lbtnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                string args = eventArgs;

                AddSchedule(args);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
            }
        }
        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        /// Gets or sets the upload path.
        /// </summary>
        /// <value>
        /// The upload path.
        /// </value>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }

        /// <summary>
        /// ActiveX 유무
        /// </summary>
        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }    
        #endregion
    }
}