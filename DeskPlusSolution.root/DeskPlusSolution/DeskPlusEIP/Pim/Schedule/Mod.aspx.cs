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
    public partial class Mod : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;

        public int DisplayID = 0;
        public int ScheduleID = 0;
        public string Searchkey = string.Empty;
        public string SearchValue = string.Empty;
        public int CurrentPage = 1;
        public string Starttime = string.Empty;
        public string Endtime = string.Empty;
        public string Order = string.Empty;
        public string Contents = string.Empty; //내용        
        private string eventArgs = String.Empty;
        public string PageUrl=string.Empty;

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.Mod).Assembly;	//어셈블리 선언
            #endregion
            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadStatus();
                LoadData();
                SettingControl();
                LoadContent();
                LoadAttach();
            }

        }

        private void LoadData()
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 쓰기 권한 체크
            #endregion

            

            

        }

        #endregion

        #region Method (함수)

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


        private void SettingControl()
        {
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
            /*
            ListItem lt0 = new ListItem("", "");
            ListItem lt1 = new ListItem(GetResource("MESSENGER"), "M");
            ListItem lt2 = new ListItem(GetResource("MAIL"), "E");
            ListItem lt3 = new ListItem(GetResource("SMS"), "S");

            ddlnotikind.Items.Add(lt0);

            string[] arAlarmType = AppSettingHelper.GetAppSetting(AppSettingType.PIM, "AlartType", "Messenger,SMS,EMAIL").Split(',');

            foreach (string s in arAlarmType)
            {
                switch (s)
                {
                    case "Messenger":
                        ddlnotikind.Items.Add(lt1);
                        break;
                    case "SMS":
                        ddlnotikind.Items.Add(lt3);
                        break;
                    case "EMAIL":
                        ddlnotikind.Items.Add(lt2);
                        break;
                }

            }
            */

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
        }

        private void MakeTime()
        {
            string strVal = string.Empty;
            string strKey = string.Empty;

            for (int i = 0; i < 24; i++)
            {
                if (i < 12)
                {

                    strVal = GetResource("AM") + " ";

                    if (i == 0)
                    {
                        strVal = strVal + " 12";
                    }
                    else
                    {
                        strVal = strVal + " " + i.ToString();
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
                strTemp = strVal + ":00";
                if (i < 10)
                {
                    strKey ="0"+ i.ToString() + ":00";
                }
                else
                {
                    strKey =  i.ToString() + ":00";
                }

                ListItem lv1 = new ListItem(strTemp, strKey);
                ddlstartTime.Items.Add(lv1);
                ListItem lvs1 = new ListItem(strTemp, strKey);
                ddlendTime.Items.Add(lvs1);

                strTemp = strVal + ":30";
                
                if (i < 10)
                {
                    strKey = "0" + i.ToString() + ":30";
                }
                else
                {
                    strKey = i.ToString() + ":30";
                }

                ListItem lv2 = new ListItem(strTemp, strKey);
                ddlstartTime.Items.Add(lv2);
                ListItem lve2 = new ListItem(strTemp, strKey);
                ddlendTime.Items.Add(lve2);

                
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
                CbtnConfirm.Text = GetResource("Save");
                CbtnCancel.Text = GetResource("Cancel");
                cbtnRepeat.Text = GetResource("Setting");
                cbtnRepCancel.Text = GetResource("Cancel");
                CbtnSetting.Text = GetResource("AlarmSetting");
            }
        }



        /// <summary>
        /// 일정수정
        /// </summary>
        private void ModContent(string arg)
        {
            try
            {
                LoadParam();

                Pim.BE.ScheduleInfo info = new BE.ScheduleInfo();
                info.ScheduleID = ScheduleID;
                info.CompanyID = CompanyID;
                info.ToDoID = 0;
                info.OpenType = ddlCategory.SelectedValue.ToString();
                info.Type = "I";
                //제목
                info.Title = txttitle.Value.ToString().Trim();
                
                //내용
                info.Content = GetContents();

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
                info.UserName = CurrentUserInfo.Name;
                info.DeptID = CurrentUserInfo.DeptID;
                //info.OpenType = "0";
                info.Lunar = "N";
                info.StatusID = ddlStatus.SelectedValue.ToNumber<int>();                

                info.ReservationID = 0;
                
                //첨부파일
                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
                info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);
                
                //날짜                
                info.StartTime = DateTime.Parse(startDate.Value.ToString() + " " + ddlstartTime.SelectedValue);
                info.EndTime = DateTime.Parse(endDate.Value.ToString() + " " + ddlendTime.SelectedValue);

                //info.Repeat = "N"; 
                if (hdRepeat.Text.Trim().Length > 0)
                {
                    info.EndTime = DateTime.Parse(startDate.Value.ToString() + " " + ddlendTime.SelectedValue);
                    info.Repeat = "Y";
                }
                else
                {
                    info.Repeat = "N";
                }

                if (hdRepeatChk.Text == "I") //반복설정에 변경사항이 없을때
                {
                    switch (hdRepeatSel.Text)
                    {
                        case "1":
                            info.Repeat = "N";
                            break;
                    }
                }

                Pim.DAL.Schedule bll = new DAL.Schedule();

                switch (hdRepeatSel.Text)
                {
                    case "1":
                        
                        ScheduleID = bll.Add(info);
                        break;
                    case "2":
                        ScheduleID = bll.Add(info);
                        break;
                    case "3":
                        // 3. 모든 일정일떄 
                        bll.Mod(info);
                        break;
                    case "":
                        if (hdRepeatChk.Text == "N")
                        {
                            ScheduleID = bll.Add(info);
                        }
                        else
                        {
                            if (hdRepeatChk.Text == "") //원래 반복설정이 없을때
                            {
                                bll.Mod(info);
                            }
                            else
                            {
                                ScheduleID = bll.Add(info);
                            }
                        }
                        break;
                }

                int beforeScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();

                #region 첨부파일 추가
                DAL.Attach attachdal = new DAL.Attach();
                
                if (info.HiddenSpoolPath != "" && info.HiddenSpoolPath != null)
                {
                    HttpContext context = HttpContext.Current;
                    DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(info.HiddenSpoolPath));
                    if (di.Exists)
                    {
                        if (!attachdal.AddList(CompanyID, ScheduleID, info.HiddenSpoolPath))
                        {
                            /*
                            ArrayList ar = new ArrayList();
                            DirectoryInfo dr = new DirectoryInfo(Server.MapPath("/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Pims/" + ScheduleID.ToString()));
                            dr.Create();
                            
                            switch (hdRepeatSel.Text)
                            {
                                case "1": //이번일정만                                    
                                    break;
                                case "2"://향후모든일정만
                                    if (tbHiddenFileList.Text.Length > 0)
                                    {
                                        string[] arFile = tbHiddenFileList.Text.Split('|');

                                        for (int i = 0; i < arFile.Length; i++)
                                        {
                                            string[] arSingle = arFile[i].Split('>');
                                            string[] FileName = arSingle[4].Split('.');
                                            string strGuid = Guid.NewGuid().ToString();

                                            FileInfo fi = new FileInfo(Server.MapPath(arSingle[2] + arSingle[4]));

                                            string orifile = Server.MapPath(arSingle[2] + arSingle[4]);
                                            string copyfile = Server.MapPath("/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Pims/" + ScheduleID.ToString() + "/" + strGuid) + "." + FileName[1];
                                            File.Copy(orifile, copyfile);

                                            BE.AttachInfo _befile = new BE.AttachInfo();
                                            _befile.CompanyID = CurrentUserInfo.CompanyID;
                                            _befile.ScheduleID = ScheduleID;
                                            //_befile.FilePath = Server.MapPath("/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Pims/" + ScheduleID.ToString() + "/" + strGuid);
                                            _befile.FilePath = "/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Pims/" + ScheduleID.ToString() + "/";
                                            _befile.FileName = arSingle[3];
                                            _befile.FileType = "." + FileName[1];
                                            _befile.FileLength = fi.Length;
                                            _befile.FileGUID = strGuid + "." + FileName[1];
                                            attachdal.Add(_befile);
                                        }

                                    }
                                    break;
                                case "3": // 모든일정
                                    break;
                            }
                            */
                        }
                    }
                }

                if ((hdRepeatSel.Text == "1") || (hdRepeatSel.Text == "2") || ((hdRepeat.Text.Trim().Length == 0) && (hdRepeatChk.Text != "")))
                {
                    if (tbHiddenFileList.Text.Length > 0)
                    {
                        ArrayList ar = new ArrayList();
                        DirectoryInfo dr = new DirectoryInfo(Server.MapPath("/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Pims/" + ScheduleID.ToString()));
                        dr.Create();     

                        BE.AttachInfo _attachinfo = new BE.AttachInfo();
                        _attachinfo.CompanyID = CurrentUserInfo.CompanyID;
                        _attachinfo.ScheduleID = beforeScheduleID;

                        DAL.Attach attachbiz = new DAL.Attach();

                        List<BE.AttachInfo> list = attachbiz.GetAttachInfo(_attachinfo);
                        if (list.Count > 0)
                        {
                            foreach (BE.AttachInfo attachinfo in list)
                            {
                                string strGuid = Guid.NewGuid().ToString();
    
                                string orifile = Server.MapPath(attachinfo.FilePath + attachinfo.FileGUID);
                                string copyfile = Server.MapPath("/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Pims/" + ScheduleID.ToString() + "/" + strGuid) + attachinfo.FileType;
                                File.Copy(orifile, copyfile);

                                BE.AttachInfo _befile = new BE.AttachInfo();
                                _befile.CompanyID = CurrentUserInfo.CompanyID;
                                _befile.ScheduleID = ScheduleID;                                
                                _befile.FilePath = "/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Pims/" + ScheduleID.ToString() + "/";
                                _befile.FileName = attachinfo.FileName;
                                _befile.FileType = attachinfo.FileType;
                                _befile.FileLength = attachinfo.FileLength;
                                _befile.FileGUID = strGuid + attachinfo.FileType;
                                attachdal.Add(_befile);
                            }
                        }                        
                    }
                }
          
                

                #endregion

                #region 공유설정

                BE.ScheShareListInfo data = new BE.ScheShareListInfo();

                data.CompanyID = CurrentUserInfo.CompanyID;
                data.ScheduleID = ScheduleID;
                //data.UserID = CurrentUserInfo.UserID;
                data.UserMail = string.Empty;
                //data.DeptID = 0;
                //data.UserName = CurrentUserInfo.Name;

                DAL.ScheShareList biz = new DAL.ScheShareList();
                biz.DelScheShareLis(data);

                //biz.AddScheShareLis(data);
                //data.UserID = 0;
                //data.DeptID = 0;
                //data.UserName = string.Empty;   
              
                string strTemp = hdShareList.Text.Trim();
                if (strTemp.Length > 0)
                {
                    string[] arVal = strTemp.Split(',');

                    foreach (string _be in arVal)
                    {
                        string[] arSource = _be.Split('$');
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

                #region 반복설정
                 
                DAL.RepeatSchedule bizre = new DAL.RepeatSchedule();
                BE.RepeatScheduleInfo infore=new BE.RepeatScheduleInfo();
                DAL.Display bizDis = new DAL.Display();
                BE.DisplayInfo DisInfo = new BE.DisplayInfo();

                infore.CompanyID=CompanyID;
                infore.ScheduleID =ScheduleID;
                DisInfo.CompanyID=CompanyID;
                DisInfo.ScheduleID=ScheduleID;
                DisInfo.UserID=UserID;
                DisInfo.DisplayID = DisplayID;

                DAL.Alarm dalalarm = new DAL.Alarm();

                if (hdRepeat.Text.Trim().Length > 0)
                {                   
                        DisInfo.ScheduleID = ScheduleID;                        
                        if (hdRepeatChk.Text == "I") //반복설정에 변경사항이 없을때
                        {
                            switch (hdRepeatSel.Text)
                            {
                                case "1":

                                    // 기존 알람 삭제                                    
                                    dalalarm.Del(DisInfo.CompanyID, beforeScheduleID, DisInfo.DisplayID);

                                    bizDis.DelDisplayIDSolo(DisInfo);
                                    DisInfo.CompanyID = CompanyID;
                                    DisInfo.UserID = CurrentUserInfo.UserID;
                                    DisInfo.StartDate = info.StartTime;
                                    DisInfo.EndDate = info.EndTime;
                                    if (DisInfo.StartDate == DisInfo.EndDate)
                                    {
                                        DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
                                    }
                                    DisInfo.ScheduleID = ScheduleID;
                                    DisplayID = bizDis.AddDisplay(DisInfo);

                                    arrDisplay.Add(DisplayID + "^" + DisInfo.StartDate + "^" + DisInfo.EndDate);

                                    break;
                                case "2":                                    

                                    DateTime dt = Convert.ToDateTime(startDate.Value);
                                    int tmpStartDate = (dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0')).ToNumber<int>();
                                    int tmpBeforeDate = BeforeStartDate.ToNumber<int>();

                                    tmpStartDate = (tmpStartDate > tmpBeforeDate) ? tmpBeforeDate : tmpStartDate;

                                    // 기존 알람 삭제
                                    dalalarm.Del(CompanyID, beforeScheduleID, tmpStartDate.ToString());

                                    bizDis.DelDisplayStartDate(CompanyID, beforeScheduleID, tmpStartDate.ToString());

                                    AddRepeat(ScheduleID);

                                    break;
                                case "3":
                                    DisInfo.ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();                                    

                                    // 기존 알람 삭제
                                    dalalarm.Del(CompanyID, beforeScheduleID);

                                    bizDis.DelDisplay(DisInfo); //전체삭제
                                    bizre.DelRepeatSchedule(infore);
                                    AddRepeat(ScheduleID);
                                    break;
                            }
                        }

                        if (hdRepeatChk.Text == "N") //반복설정을 수정했을떄
                        {
                            switch (hdRepeatSel.Text)
                            {
                                case "1":
                                    // 기존 알람 삭제                                    
                                    dalalarm.Del(DisInfo.CompanyID, beforeScheduleID, DisInfo.DisplayID);

                                    bizDis.DelDisplayIDSolo(DisInfo);
                                    break;
                                case "2":         
                                    DateTime dt = Convert.ToDateTime(startDate.Value);
                                    int tmpStartDate = (dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0')).ToNumber<int>();
                                    int tmpBeforeDate = BeforeStartDate.ToNumber<int>();

                                    tmpStartDate = (tmpStartDate > tmpBeforeDate) ? tmpBeforeDate : tmpStartDate;

                                    // 기존 알람 삭제
                                    dalalarm.Del(CompanyID, beforeScheduleID, tmpStartDate.ToString());

                                    bizDis.DelDisplayStartDate(CompanyID, beforeScheduleID, tmpStartDate.ToString());

                                    break;
                                case "3":                                    
                                    DisInfo.ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();

                                    // 기존 알람 삭제
                                    dalalarm.Del(CompanyID, beforeScheduleID);

                                    bizDis.DelDisplay(DisInfo); //전체삭제
                                    bizre.DelRepeatSchedule(infore);
                                    break;
                            }
                            
                            AddRepeat(ScheduleID);
                        }   

                        //기존에 없는데 생겼을떄
                        if (hdRepeatChk.Text == "")
                        {
                            // 기존 알람 삭제
                            dalalarm.Del(CompanyID, beforeScheduleID);

                            bizDis.DelDisplay(DisInfo); //전체삭제
                            AddRepeat(ScheduleID);
                        }

                 }
                else if (hdRepeatChk.Text == "N")
                {
                    // 반복일정이 있었음 -> 반복일정 새롭게 지정 - > 반복일정취소
                    // 결론 : 향후 모든 일정하고 동일 로직으로 구현 (해당일정 이후의 반복일정은 삭제하고 새로운 일정 등록)

                    DateTime dt = Convert.ToDateTime(startDate.Value);
                    int tmpStartDate = (dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0')).ToNumber<int>();
                    int tmpBeforeDate = BeforeStartDate.ToNumber<int>();

                    tmpStartDate = (tmpStartDate > tmpBeforeDate) ? tmpBeforeDate : tmpStartDate;

                    // 기존 알람 삭제
                    dalalarm.Del(CompanyID, beforeScheduleID, tmpStartDate.ToString());

                    bizDis.DelDisplayStartDate(CompanyID, beforeScheduleID, tmpStartDate.ToString());

                    //bizDis.DelDisplayIDSolo(DisInfo);          
                    DisInfo.CompanyID = CompanyID;
                    DisInfo.UserID = CurrentUserInfo.UserID;
                    DisInfo.StartDate = info.StartTime;
                    DisInfo.EndDate = info.EndTime;
                    if (DisInfo.StartDate == DisInfo.EndDate)
                    {
                        DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
                    }
                    DisInfo.ScheduleID = ScheduleID;
                    DisplayID = bizDis.AddDisplay(DisInfo);

                    arrDisplay.Add(DisplayID + "^" + DisInfo.StartDate + "^" + DisInfo.EndDate);
                }
                else
                {
                    if (hdRepeatChk.Text == "") //원래 반복설정이 없을때
                    {
                        // 기존 알람 삭제
                        dalalarm.Del(CompanyID, beforeScheduleID);

                        bizDis.DelDisplay(DisInfo);
                        DisInfo.CompanyID = CompanyID;
                        DisInfo.UserID = CurrentUserInfo.UserID;
                        DisInfo.StartDate = info.StartTime;
                        DisInfo.EndDate = info.EndTime;
                        if (DisInfo.StartDate == DisInfo.EndDate)
                        {
                            DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
                        }
                        DisInfo.ScheduleID = ScheduleID;
                        DisplayID = bizDis.AddDisplay(DisInfo);

                        arrDisplay.Add(DisplayID + "^" + DisInfo.StartDate + "^" + DisInfo.EndDate);
                    }
                    else //반복설정이 있었는데 삭제 됬을때 
                    {
                        DateTime dt = Convert.ToDateTime(startDate.Value);
                        int tmpStartDate = (dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0')).ToNumber<int>();
                        int tmpBeforeDate = BeforeStartDate.ToNumber<int>();

                        tmpStartDate = (tmpStartDate > tmpBeforeDate) ? tmpBeforeDate : tmpStartDate;

                        // 기존 알람 삭제
                        dalalarm.Del(CompanyID, beforeScheduleID, tmpStartDate.ToString());

                        bizDis.DelDisplayStartDate(CompanyID, beforeScheduleID, tmpStartDate.ToString());

                        DisInfo.CompanyID = CompanyID;
                        DisInfo.UserID = CurrentUserInfo.UserID;
                        DisInfo.StartDate = info.StartTime;
                        DisInfo.EndDate = info.EndTime;
                        if (DisInfo.StartDate == DisInfo.EndDate)
                        {
                            DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
                        }
                        DisInfo.ScheduleID = ScheduleID;
                        DisplayID = bizDis.AddDisplay(DisInfo);

                        arrDisplay.Add(DisplayID + "^" + DisInfo.StartDate + "^" + DisInfo.EndDate);
                    }
                }
                #endregion

                #region 알람

                if (DisplayID > 0)
                {
                    Pim.BLL.Schedule _abill = new BLL.Schedule();
                    if (ddlnotikind.SelectedValue.ToString().Length > 0)
                    {
                        Hashtable ht = AddListHash();

                        if (ddlnotikind.SelectedValue == "M" || ddlnotikind.SelectedValue == "B")
                        {
                            for (int i = 0; i < arrDisplay.Count; i++)
                            {
                                string[] ArrTmp = arrDisplay[i].ToString().Split('^');

                                Dictionary<string, string> paraDical = new Dictionary<string, string>();
                                paraDical.Add("DisplayID", ArrTmp[0].ToString());
                                paraDical.Add("CompanyID", CurrentUserInfo.CompanyID.ToString());
                                paraDical.Add("ScheduleID", ScheduleID.ToString());
                                info.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                                info.SenderMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                                info.StartTime = DateTime.Parse(ArrTmp[1]);
                                info.EndTime = DateTime.Parse(ArrTmp[2]);

                                _abill.SendAlarm(info, ScheduleID, CurrentUserInfo.UserID, ht, ArrTmp[0].ToNumber<int>());
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
                                                            paraDical.Add("ScheduleID", ScheduleID.ToString());
                                                            info.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                                                            info.SenderMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                                                            info.StartTime = DateTime.Parse(ArrTmp[1]);
                                                            info.EndTime = DateTime.Parse(ArrTmp[2]);

                                                            _abill.SendAlarm(info, ScheduleID, shdt["userid"].ToString().ToNumber<int>(), ht, ArrTmp[0].ToNumber<int>());
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
                                                            paraDical.Add("ScheduleID", ScheduleID.ToString());
                                                            info.Parameterstring = GetEncryptParam(Utility.GetParameterDic(paraDical));
                                                            info.SenderMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;

                                                            info.StartTime = DateTime.Parse(ArrTmp[1]);
                                                            info.EndTime = DateTime.Parse(ArrTmp[2]);

                                                            _abill.SendAlarm(info, ScheduleID, shdt["userid"].ToString().ToNumber<int>(), ht, ArrTmp[0].ToNumber<int>());
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
                }

                #endregion 

                if (arg.Equals("MOD"))
                {
                    if (DisplayID > 0)
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("PageScope", PageScope);
                        paraDic.Add("Date", Date);
                        paraDic.Add("Opentype", Opentype);
                        paraDic.Add("ScheduleID", ScheduleID.ToString());
                        paraDic.Add("DisplayID", DisplayID.ToString());
                        
                        paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                        paraDic.Add("alertMsg", GetResource("GoList"));
                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/View.aspx");
                        string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                        Utility.Redirect(encParams);
                    }
                    else
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();

                        paraDic.Add("alertMsg", GetResource("NoRepeatAdd"));
                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/ScheduleMain.aspx");
                        string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                        Utility.Redirect(encParams);
                    }
                }


            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModContent", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
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
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            DisplayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();
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
        /// 작성자  : 심재운
        /// 첨부파일 정보 가져오기
        /// </summary>
        private void LoadAttach()
        {
            BE.AttachInfo _info = new BE.AttachInfo();
            _info.CompanyID = CurrentUserInfo.CompanyID;
            _info.ScheduleID = ScheduleID;

            DAL.Attach biz = new DAL.Attach();

            List<BE.AttachInfo> list = biz.GetAttachInfo(_info);
            string saveFileList = "";
            if (list.Count > 0)
            {
                foreach (BE.AttachInfo info in list)
                {
                    saveFileList += "S>" + info.AttachID + ">" + info.FilePath + ">" + info.FileName + ">" + info.FileGUID + "|";

                }
            }
            saveFileList = saveFileList.TrimEnd('|');

            //tbHiddenFileList.Text = fileList;					//첨부파일 정보를 저장하며 리스트박스와 동기화
            tbHiddenFileList.Text = saveFileList;					//첨부파일 정보를 저장하며 리스트박스와 동기화
            tbHiddenSaveFileList.Text = saveFileList;			//서버 파일 삭제시 비교로 사용 될 원본 파일 리스트(FileID 포함)
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

        /// <summary>
        /// 작성자  : 심재운
        /// 리스트
        /// </summary>
        private void LoadContent()
        {
            LoadParam();
            
            BE.ScheduleInfo info = new BE.ScheduleInfo();
            info.CompanyID = CompanyID;
            info.ScheduleID = ScheduleID;
            info.DisplayID = this.DisplayID;
            info.UserID = CurrentUserInfo.UserID;

            DAL.Schedule bll = new DAL.Schedule();
            DataTableReader dtr = bll.GetSheduleDataViewID(info);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {

                        //제목
                        txttitle.Value = dtr["Title"].ToString().ToReverseXSS();//_info.Title;
                        //날짜
                        //startDate.Value = DateTime.Parse(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd"); // _info.StartTime.ToString("yyyy-MM-dd");
                        startDate.Value = SetDate(dtr["StartDate"].ToString());

                        DateTime dtStart = Convert.ToDateTime(dtr["StartDate"].ToString());
                        BeforeStartDate = dtStart.Year.ToString() + dtStart.Month.ToString().PadLeft(2, '0') + dtStart.Day.ToString().PadLeft(2, '0');

                        ddlstartTime.SelectedValue = DateTime.Parse(dtr["StartDate"].ToString()).ToString("HH:mm");// _info.StartTime.ToString("HH:mm");

                        //endDate.Value = DateTime.Parse(dtr["EndDate"].ToString()).ToString("yyyy-MM-dd");///_info.EndTime.ToString("yyyy-MM-dd");
                        endDate.Value = SetDate(dtr["EndDate"].ToString());
                        ddlendTime.SelectedValue = DateTime.Parse(dtr["EndDate"].ToString()).ToString("HH:mm");//_info.EndTime.ToString("HH:mm");
                        //음력양력

                        if (dtr["Alert"].ToString() == "Y")
                        {
                            if (dtr["AlertType"].ToString() == "M")
                            {
                                ddlnotikind.SelectedValue = "M";
                                if (dtr["AlertTime"].ToString() != "0")
                                {                                    
                                    ddltime.SelectedValue = dtr["AlertTime"].ToString();
                                }
                            } 
                            else if (dtr["AlertType"].ToString() == "B")
                            {
                                ddlnotikind.SelectedValue = "B";
                                if (dtr["AlertTime"].ToString() != "0")
                                {
                                    ddltime.SelectedValue = dtr["AlertTime"].ToString();
                                }
                            }
                            else if (dtr["AlertType"].ToString() == "E")
                            {
                                ddlnotikind.SelectedValue = "E";
                                idEmail.Value = dtr["AlertTargetAddress"].ToString();
                            }
                        }
                        //종류
                        ddlCategory.SelectedValue = dtr["OpenType"].ToString();//_info.OpenType;
                        //내용
                        tbHiddenHTML_contents.Value = dtr["Content"].ToString();//_info.Content;
                        //상태값
                        ddlStatus.SelectedValue = dtr["StatusID"].ToString();//_info.Content;

                        string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang,"");
                        string templateFooter = "</BODY></HTML>";

                        tbHiddenMime_contents.Text = templateHeader + dtr["Content"].ToString() + templateFooter;

                        Contents += templateHeader + dtr["Content"].ToString() + templateFooter;


                        if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                        {
                            //ByFreeman 20120106 X-Free 에디터 추가 
                            /*
                            string strScript = String.Empty;
                            if (Request.Path.IndexOf("Mod.aspx") > 0)
                            {
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
                            string reqTime = System.DateTime.Now.ToString("yyyyMM");
                            UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Pim/" + reqTime + "/editor/" + guid.ToString();
                            if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로
                            #endregion
                        }

                        #region 반복설정

                        GetRepeat(info.ScheduleID, info.CompanyID);

                        #endregion

                        #region 공유설정
                        GetShareUser(info.ScheduleID, info.CompanyID, info.UserID);
                        #endregion 

                    }
                }
            }
             

        }


        private void GetRepeat(int schedulid,int companyid)
        {
            DAL.RepeatSchedule biz = new DAL.RepeatSchedule();

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

                Biz.RepeatScheduleProc bizRepeatBiz = new Biz.RepeatScheduleProc(info);
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
                //paraDic.Add("EndDate", "");//info.EndDate.ToString("yyyy-MM-dd HH:mm"));
                paraDic.Add("EndDate", info.EndDate.ToString("yyyy-MM-dd 23:59"));


                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                hdRepeat.Text = encParams;
                hdRepeatChk.Text = "I";
            }

        }

        private void GetShareUser(int schedulid, int companyid,int userid)
        {
            DAL.ScheShareList biz = new DAL.ScheShareList();
            List<BE.ScheShareListInfo> data = biz.GetScheShareList(companyid, schedulid, userid);

            string rtHidden="";
            string rtHtml = "";

            foreach (BE.ScheShareListInfo _be in data)
            {
                if (_be.DeptID > 0)
                {
                    string tempid = "G$" + _be.DeptID + "$" + _be.UserName + ",";
                    rtHidden += "G$" + _be.DeptID + "$" + _be.UserName + ",";

                    rtHtml += "<li>";
			        rtHtml += "<span>" + _be.UserName   + "</span>";
                    rtHtml += "<a onclick='Mod.ShareUserClick(this)' value='" + tempid + "' style='display:inline-block;' title='" + GetResource("Del") + "'><i class='fas fa-times-circle'></i></a>";
                    rtHtml += "</li>";
                }
                else if (_be.UserID > 0)
                {
                    string tempid = "M$" + _be.UserID + "$" + _be.UserName + ",";
                    rtHidden += "M$" + _be.UserID + "$" + _be.UserName + ",";

                    rtHtml += "<li>";
                    rtHtml += "<span>" + _be.UserName + "</span>";
                    rtHtml += "<a onclick='Mod.ShareUserClick(this)' value='" + tempid + "' style='display:inline-block;' title='" + GetResource("Del") + "'><i class='fas fa-times-circle'></i></a>";
                    rtHtml += "</li>";
                }
            }

            if (rtHtml == "")
            {
                rtHtml = "<li></li>";
            }
          
            divShare.InnerHtml = rtHtml;
            hdShareList.Text = rtHidden;

            /*
            string rtHtml = "<table cellspacing='0' class='table_typeT'>";
            
            rtHtml += "<caption></caption>";
            rtHtml += "<colgroup><col /><col width='90px'/></colgroup>";
            rtHtml += "<tr><th>" + GetResource("Name") + "</th><th>" + GetResource("Del") + "</th></tr>";

            string strImg = "<img src='" + BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_del.gif' alt='del' />";
            
            bool bchk = false;

            foreach (BE.ScheShareListInfo _be in data)
            {
                bchk = true;    
                if (_be.DeptID > 0)
                {
                    string tempid = "G$" + _be.DeptID + "$" + _be.UserName + ",";
                    rtHidden += "G$" + _be.DeptID + "$" + _be.UserName + ",";


                    rtHtml += "<tr><tr><td class='center'>" + _be.UserName + "</td>";
                    rtHtml += "<td class='center'><a onclick='javascript:Mod.ShareUserClick(this)' value='" + tempid + "' >" + strImg + " </a></td></tr>";

                }
                else if (_be.UserID > 0)
                {
                    string tempid = "M$" + _be.UserID + "$" + _be.UserName + ",";
                    rtHidden += "M$" + _be.UserID + "$" + _be.UserName + ",";

                    rtHtml += "<tr><tr><td class='center'>" + _be.UserName + "</td>";
                    rtHtml += "<td class='center'><a onclick='javascript:Mod.ShareUserClick(this)' value='" + tempid + "' >" + strImg + "</a></td></tr>";                    
                } 
            }

            if (!bchk)
            {
                rtHtml += "<tr><td colspan='2' class='center'>" + GetResource("TextEmpty") + "</td></tr>";
            }


            rtHtml += "</table>";

            divShare.InnerHtml = rtHtml;
            hdShareList.Text = rtHidden;
            */
                
        }


        private void AddRepeat(int scheduleid)
        {


            BE.DisplayInfo DisInfo = new BE.DisplayInfo();
            BE.RepeatScheduleInfo RepInfo = new BE.RepeatScheduleInfo();

            DAL.Display bizDis = new DAL.Display();
            DAL.RepeatSchedule bizRep = new DAL.RepeatSchedule();

            BE.RepeatScheduleInfo info = new BE.RepeatScheduleInfo();

            string strDecord = Server.UrlDecode(hdRepeat.Text);
            string strdescrypt = Server.UrlDecode(Utility.DecryptString(strDecord));

            if (strdescrypt.Length > 0)
            {
                foreach (string _qry in strdescrypt.Split('&'))
                {
                    string key = _qry.Split('=')[0];
                    string keyValue = _qry.Split('=')[1];

                    switch (key)
                    {
                        case "RepeatType":
                            info.RepeatType = keyValue;
                            break;
                        case "SelectType":
                            info.SelectType = keyValue;
                            break;
                        case "IntervalCnt":
                            info.IntervalCnt = keyValue.ToNumber<int>();
                            break;
                        case "MaxReptCnt":
                            info.MaxReptCnt = keyValue.ToNumber<int>();
                            break;
                        case "Option1":
                            info.Option1 = keyValue;
                            break;
                        case "Option2":
                            info.Option2 = keyValue;
                            break;
                        case "Option3":
                            info.Option3 = keyValue;
                            break;
                        case "Option4":
                            info.Option4 = keyValue;
                            break;
                        case "Option5":
                            info.Option5 = keyValue;
                            break;
                        case "Option6":
                            info.Option6 = keyValue;
                            break;
                        case "Option7":
                            info.Option7 = keyValue;
                            break;

                    }
                }
            }

            info.ScheduleID = scheduleid;
            info.CompanyID = CurrentUserInfo.CompanyID;
            

           string strS = startDate.Value.ToString() + " " + ddlstartTime.SelectedValue.ToString();
            string strE = UtilHelper.GetParmforParameter(strdescrypt, "EndDate", "");
            string strE1 = endDate.Value.ToString() + " " + ddlendTime.SelectedValue.ToString();

            if (strE.Length > 0)
            {
                if (DateTime.Parse(strE).ToString("yyyyMMdd") == "00010101")
                    info.EndDate = DateTime.Parse("2999-01-01");
                else
                    info.EndDate = DateTime.Parse(strE);
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

            List<BE.DisplayInfo> listobj = bizRepeatBiz.GetDateInfo();

            int iIndex = 0;
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

            if (listobj.Count == 0)
            {
                DisplayID = 0;
            }
                        
            if (info.StartDate == info.EndDate)
            {
                info.EndDate = info.EndDate.AddHours(1);
            }
            bizRep.AddRepeatSchedule(info);
        }


        /// <summary>
        /// 첨부파일
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_scheduleid"></param>
        /// <param name="_spoolPath"></param>
        /// <returns></returns>
        public Hashtable AddListHash()
        {
            Hashtable ht = new Hashtable();
             
            BE.AttachInfo _info = new BE.AttachInfo();
            _info.CompanyID = CurrentUserInfo.CompanyID;
            _info.ScheduleID = ScheduleID;

            DAL.Attach biz = new DAL.Attach();
            List<BE.AttachInfo> list = biz.GetAttachInfo(_info);
            
            if (list.Count > 0)
            {
                foreach (BE.AttachInfo info in list)
                {
                    ht.Add(
                    Server.MapPath(info.FilePath + info.FileName),
                      Server.MapPath(info.FilePath + info.FileGUID)
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

                ModContent(args);
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


        /// <summary>
        /// BeforeStartDate
        /// </summary>
        public string BeforeStartDate
        {
            get { return GetViewStateString("BeforeStartDate", ""); }
            set { ViewState["BeforeStartDate"] = value; }
        }

        #endregion
    }
}