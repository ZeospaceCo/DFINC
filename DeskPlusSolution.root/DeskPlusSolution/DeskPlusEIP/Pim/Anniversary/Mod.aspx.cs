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

namespace Zeospace.DeskPlus.Pim.Web.Anniversary
{
    public partial class Mod : Base.BasePagePim
    {

        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;

        public int ScheduleID = 0;
        public string Searchkey = string.Empty;
        public string SearchValue = string.Empty;
        public int CurrentPage = 1;
        public string Starttime = string.Empty;
        public string Endtime = string.Empty;
        public string Order = string.Empty;
        public string Contents = string.Empty; //내용        
        private string eventArgs = String.Empty;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Anniversary.Mod).Assembly;	//어셈블리 선언
            #endregion
            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
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
            ListItem lt1 = new ListItem(GetResource("MESSENGER"), "M");
            ListItem lt2 = new ListItem(GetResource("MAIL"), "E");
            ListItem lt3 = new ListItem(GetResource("SMS"), "S");
            ddlnotikind.Items.Add(lt1);
            ddlnotikind.Items.Add(lt2);
            ddlnotikind.Items.Add(lt3);
            */

            //알람 시간
            for (int i = 1; i < 12; i++)
            {
                ListItem ltime1 = new ListItem((i * 5).ToString() + GetResource("Minute"), (i * 5).ToString());
                ddltime.Items.Add(ltime1);
            }
            ListItem ltime2 = new ListItem("1" + GetResource("TIMEINFO"), "60");
            ListItem ltime3 = new ListItem("2" + GetResource("TIMEINFO"), "120");
            ListItem ltime4 = new ListItem("3" + GetResource("TIMEINFO"), "180");
            ListItem ltime5 = new ListItem("4" + GetResource("TIMEINFO"), "240");
            ListItem ltime6 = new ListItem("5" + GetResource("TIMEINFO"), "300");
            ListItem ltime7 = new ListItem("6" + GetResource("TIMEINFO"), "360");

            ListItem ltime8 = new ListItem("1" + GetResource("kind4"), "2400");
            ListItem ltime9 = new ListItem("2" + GetResource("kind4"), "4800");
            ListItem ltime10 = new ListItem("3" + GetResource("kind4"), "7200");

            ddltime.Items.Add(ltime2);
            ddltime.Items.Add(ltime3);
            ddltime.Items.Add(ltime4);
            ddltime.Items.Add(ltime5);
            ddltime.Items.Add(ltime6);
            ddltime.Items.Add(ltime7);
            ddltime.Items.Add(ltime8);
            ddltime.Items.Add(ltime9);
            ddltime.Items.Add(ltime10);


            ListItem ltcate1 = new ListItem(GetResource("schedule2"), "0");

            ListItem ltcate4 = new ListItem(GetResource("AllAnniversary"), "2");


            ddlCategory.Items.Add(ltcate1);

            ddlCategory.Items.Add(ltcate4);

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
            }
        }

        /// <summary>
        /// 기념일 추가
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
                info.Type = "J";// ddlCategory.SelectedValue.ToString();

                //제목
                info.Title = txttitle.Value.ToString().Trim();
                //날짜
                info.StartTime = DateTime.Parse(startDate.Value.ToString());
                info.EndTime = DateTime.Parse(startDate.Value.ToString());
                //내용
                info.Content = GetContents();

                info.Repeat = "N";
                if (!ddltime.SelectedValue.ToString().Equals(""))
                {
                    info.Alert = "N";
                    info.AlertType = ddlnotikind.SelectedValue.ToString();
                    info.AlertTime = int.Parse(ddltime.SelectedValue.ToString());
                }
                else
                {
                    info.Alert = "N";
                    info.AlertType = "";
                    info.AlertTime = 0;
                }
                info.AlertTargetAddress = idEmail.Value.ToString();
                info.IsOpen = "Y";//사용안함
                info.UserID = CurrentUserInfo.UserID;
                info.DeptID = CurrentUserInfo.DeptID;
                info.OpenType = ddlCategory.SelectedValue;
                info.StatusID = 0; //기본값

                info.ReservationID = 0;
                if (rdSun.Checked)
                {
                    info.Lunar = "N";
                }
                else
                {
                    info.Lunar = "Y";
                }

                //첨부파일
                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
                info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);


                Pim.DAL.Schedule bll = new DAL.Schedule();
                bll.Mod(info);

                #region 첨부파일 추가
                DAL.Attach attachdal = new DAL.Attach();
                attachdal.AddList(CompanyID, ScheduleID, info.HiddenSpoolPath);

                #endregion

                BE.DisplayInfo DisInfo = new BE.DisplayInfo();
                DAL.Display bizDis = new DAL.Display();
                DisInfo.CompanyID = CompanyID;
                DisInfo.UserID = CurrentUserInfo.UserID;
                DisInfo.StartDate = info.StartTime;
                DisInfo.EndDate = info.StartTime;


                if (rdSun.Checked)
                {
                    //DisInfo.Start Date = LunalConvert(info.StartTime);
                    DisInfo.StartDate = info.StartTime;
                }
                else
                {
                    BLL.CalendarSolToLun cstl = new BLL.CalendarSolToLun();
                    DisInfo.StartDate = DateTime.Parse(cstl.FromLunToSol(info.StartTime));// SunConvert(info.StartTime);

                }
                
                DisInfo.EndDate = DisInfo.StartDate.AddHours(1);

                DisInfo.ScheduleID = ScheduleID;
                bizDis.DelDisplay(DisInfo);
                bizDis.AddDisplay(DisInfo);



                if (arg.Equals("MOD"))
                {
                    if (ScheduleID > 0)
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("ScheduleID", ScheduleID.ToString());
                        paraDic.Add("Searchkey", Searchkey);
                        paraDic.Add("SearchValue", SearchValue);
                        paraDic.Add("CurrentPage", CurrentPage.ToString());
                        paraDic.Add("Starttime", Starttime);
                        paraDic.Add("Endtime", Endtime);
                        paraDic.Add("order", Order);
                        paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                        paraDic.Add("alertMsg", GetResource("GoList"));


                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Anniversary/view.aspx");
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

        public DateTime LunalConvert(DateTime dt)
        {
            int nMmonth; //윤월
            int nLyear, nLmonth, nLday;//음력년,음력월,음력일
            bool bMmon = false;  //윤달
            System.Globalization.KoreanLunisolarCalendar LunalCon =
            new System.Globalization.KoreanLunisolarCalendar();


            nLyear = LunalCon.GetYear(dt);
            nLmonth = LunalCon.GetMonth(dt);
            nLday = LunalCon.GetDayOfMonth(dt);
            if (LunalCon.GetMonthsInYear(nLyear) > 12)             //1년이 12이상이면 윤달이 있음..
            {
                bMmon = LunalCon.IsLeapMonth(nLyear, nLmonth);     //윤월인지
                nMmonth = LunalCon.GetLeapMonth(nLyear);             //년도의 윤달이 몇월인지?
                if (nLmonth >= nMmonth)                           //달이 윤월보다 같거나 크면 -1을 함 즉 윤8은->9 이기때문
                    nLmonth--;
            }
            string rtDate = nLyear.ToString() + "-" + (bMmon ? "*" : "") + nLmonth.ToString() + "-" + nLday.ToString();
            return DateTime.Parse(rtDate.Replace("*",""));
            //return nLyear.ToString() + "-" + (bMmon ? "*" : "") + nLmonth.ToString() + "-" + nLday.ToString();
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
            ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();
            Searchkey = GetDecryptParam("Searchkey", "");
            SearchValue = GetDecryptParam("SearchValue", "");
            CurrentPage = GetDecryptParam("CurrentPage", "1").ToNumber<int>(); ;
            Starttime = GetDecryptParam("Starttime", "");
            Endtime = GetDecryptParam("Endtime", "");
            Order = GetDecryptParam("order", "");
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

            DAL.Attach biz=new DAL.Attach();

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
        /// 작성자  : 심재운
        /// 리스트
        /// </summary>
        private void LoadContent()
        {
            LoadParam();
            //bool IsAuth = false;
            BE.ScheduleInfo info = new BE.ScheduleInfo();
            info.CompanyID = CompanyID;
            info.ScheduleID = ScheduleID;
            DAL.Schedule bll=new DAL.Schedule();

            List<BE.ScheduleInfo> list = bll.GetSheduleDataID(info);

            foreach (BE.ScheduleInfo _info in list)
            {
                if (_info.UserID != CurrentUserInfo.UserID)
                {
                    return;
                }

                //제목
                txttitle.Value = _info.Title.ToReverseXSS();
                //날짜
                startDate.Value = _info.StartTime.ToShortDateString();
                //음력양력
                if (_info.Lunar.Equals("Y"))
                {
                    rbmoon.Checked = true;
                    
                }
                else
                {
                    rdSun.Checked = true;
                    //rbmoon.Checked = true;
                }
                //종류
                ddlCategory.SelectedValue = _info.OpenType;
                    
                //내용
                tbHiddenHTML_contents.Value = _info.Content;
                

                string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang,"");
                string templateFooter = "</BODY></HTML>";

                tbHiddenMime_contents.Text = templateHeader + _info.Content + templateFooter;

                Contents += templateHeader + info.Content + templateFooter;

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
            }
             
        }

        /// <summary>
        /// 양력변환
        /// </summary>
        /// <param name="_st"></param>
        /// <returns></returns>
        private DateTime SunConvert(DateTime _st)
        {
            int nlunarYear = _st.Year;
            int nlunarmon = _st.Month;
            int nlunarDay = _st.Day;
            bool bMon = true;
            int nMimon;
            System.Globalization.KoreanLunisolarCalendar lunar =
            new System.Globalization.KoreanLunisolarCalendar();

            if (lunar.GetMonthsInYear(nlunarYear) > 12)
            {
                nMimon = lunar.GetLeapMonth(nlunarYear);
                if (bMon)
                    nlunarmon++;

                if (nlunarmon > nMimon)
                    nlunarmon++;
            }
            return lunar.ToDateTime(nlunarYear, nlunarmon, nlunarDay, 0, 0, 0, 0);
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
        #endregion
    }
}