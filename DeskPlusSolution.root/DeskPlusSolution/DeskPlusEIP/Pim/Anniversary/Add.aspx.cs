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
    public partial class Add : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;                
        private string eventArgs = String.Empty;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Anniversary.Add).Assembly;	//어셈블리 선언
            #endregion            
            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
                SettingControl();
            }

        }

        private void LoadData()
        {
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
            ListItem lt1=new ListItem(GetResource("MESSENGER"),"M");
            ListItem lt2=new ListItem(GetResource("MAIL"),"E");
            ListItem lt3=new ListItem(GetResource("SMS"),"S");
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
            ListItem ltime2 = new ListItem(GetResource("OneClock"), "60");
            ListItem ltime3 = new ListItem(GetResource("TwoClock"), "120");
            ListItem ltime4 = new ListItem(GetResource("ThreeClock"), "180");
            ListItem ltime5 = new ListItem(GetResource("FourClock"), "240");
            ListItem ltime6 = new ListItem(GetResource("FiveClock"), "300");
            ListItem ltime7 = new ListItem(GetResource("SixClock"), "360");
            ListItem ltime8 = new ListItem(GetResource("OneDay"), "1440");
            ListItem ltime9 = new ListItem(GetResource("TwoDay"), "2880");
            ListItem ltime10 = new ListItem(GetResource("ThreeDay"), "4320");

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
                CbtnContinue.Text = GetResource("SaveContinue");
                CbtnConfirm.Text = GetResource("Save");
                CbtnCancel.Text = GetResource("Cancel");
            }
        }



        /// <summary>
        /// 기념일 추가
        /// </summary>
        private void Anniversary(string arg)
        {
            try
            {
                Pim.BE.ScheduleInfo info = new BE.ScheduleInfo();

                info.CompanyID = CompanyID;
                info.ToDoID = 0;
                info.Type = "J";//ddlCategory.SelectedValue.ToString();

                
                info.OpenType = ddlCategory.SelectedValue;
                //제목
                info.Title = txttitle.Value.ToString().Trim();
                //날짜
                info.StartTime = DateTime.Parse(startDate.Value.ToString());
                info.EndTime = DateTime.Parse(startDate.Value.ToString());
                //내용
                info.Content = GetContents();

                info.Repeat ="N";
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
                info.AlertTargetAddress = idEmail.Value.ToString().ToXSS();
                info.IsOpen = "Y";//사용안함
                info.UserID = CurrentUserInfo.UserID;
                info.DeptID = CurrentUserInfo.DeptID;
               

                info.ReservationID = 0;
                if (rdSun.Checked)
                {
                    info.Lunar = "N";
                }
                else
                {
                    info.Lunar = "Y";
                }

                info.StatusID = 0;//기본값

                //첨부파일
                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
                info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);
                

                Pim.DAL.Schedule bll = new DAL.Schedule();
                int scheduleid = bll.Add(info);

                #region 첨부파일 추가
                 DAL.Attach attachdal = new DAL.Attach();
                 attachdal.AddList(CompanyID,scheduleid,info.HiddenSpoolPath);                     
              
                #endregion


                 BE.DisplayInfo DisInfo = new BE.DisplayInfo();
                 DAL.Display bizDis = new DAL.Display();
                 DisInfo.CompanyID = CompanyID;
                 DisInfo.UserID = CurrentUserInfo.UserID;


                 if (rdSun.Checked)
                 {
                     //DisInfo.StartDate = LunalConvert(info.StartTime);
                     DisInfo.StartDate = info.StartTime;
                 }
                 else
                 {
                     BLL.CalendarSolToLun cstl = new BLL.CalendarSolToLun(); 
                     DisInfo.StartDate =DateTime.Parse(cstl.FromLunToSol(info.StartTime));// SunConvert(info.StartTime);
                     
                 }
                 

                 DisInfo.EndDate = DisInfo.StartDate.AddHours(1);                 

                 DisInfo.ScheduleID = scheduleid;
                 //DisInfo.
                 bizDis.AddDisplay(DisInfo);
                
                
                if (arg.Equals("SAVE"))
                {
                    if (scheduleid > 0)
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("ScheduleID", scheduleid.ToString());
                        paraDic.Add("Searchkey", "");
                        paraDic.Add("searchGroup", "");
                        paraDic.Add("CurrentPage", "");
                        paraDic.Add("searchValue", "");
                        paraDic.Add("order", "");
                        paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                        paraDic.Add("alertMsg", GetResource("GoList"));

                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Anniversary/view.aspx");
                        string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                        Utility.Redirect(encParams);              
                    }
                    
                }
                else if (arg.Equals("CONTINUE"))
                {
                   

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    
                    paraDic.Add("Searchkey", "");
                    paraDic.Add("searchGroup", "");
                    paraDic.Add("CurrentPage", "");
                    paraDic.Add("searchValue", "");
                    paraDic.Add("order", "");
                    paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                    
                    paraDic.Add("alertMsg", GetResource("CONTINUEADD"));
                    paraDic.Add("goURL", "/DeskPlusEIP/Pim/Anniversary/AddN.aspx");   

                    /*
                    if (NonActiveX == "T") //activex
                    {
                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Anniversary/AddN.aspx");                        
                    }
                    else
                    {
                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Anniversary/Add.aspx");                        
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

        /// <summary>
        /// 음력변환
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DateTime LunarConvert(DateTime dt)
        {

            int nMMon;

            int nLunarYear, nLunarMonth, nLunarDay;

            bool bYundar = false;

            System.Globalization.KoreanLunisolarCalendar lunaryear =

                new System.Globalization.KoreanLunisolarCalendar();



            nLunarYear = lunaryear.GetYear(dt);

            nLunarMonth = lunaryear.GetMonth(dt);

            nLunarDay = lunaryear.GetDayOfMonth(dt);

            if (lunaryear.GetMonthsInYear(nLunarYear) > 12)             //1년이 12이상이면 윤달이 있음..
            {

                bYundar = lunaryear.IsLeapMonth(nLunarYear, nLunarMonth);     //윤월인지

                nMMon = lunaryear.GetLeapMonth(nLunarYear);             //년도의 윤달이 몇월인지?

                if (nLunarMonth >= nMMon)                           //달이 윤월보다 같거나 크면 -1을 함 즉 윤8은->9 이기때문

                    nLunarMonth--;

            }
            string strlunarDate = nLunarYear.ToString() + "-" + (bYundar ? "*" : "") + nLunarMonth.ToString() + "-" + nLunarDay.ToString();
            return DateTime.Parse(strlunarDate.Replace("*", ""));
            

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
        /// 양력->음력
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
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

                Anniversary(args);
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