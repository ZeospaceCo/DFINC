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

namespace Zeospace.DeskPlus.Pim.Web.ToDo
{
    public partial class Add : Base.BasePagePim
    {
        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        public int TodoID = 0;
        public string bPage = string.Empty;
        
        

        public string AttachLimitSize = string.Empty; //첨부파일 제한용량
        private string eventArgs = String.Empty;

        public string Contents = string.Empty; //내용

        //검색어
        public string PageUrl = string.Empty;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.ToDo.Add).Assembly;	//어셈블리 선언
                                       
            #endregion            

            if (!IsPostBack)
            {
                #region 초기화
                SetLanguage();
                SettingControl();
                #endregion

                #region 파라미터 처리부분                
                #endregion 

                #region 시간 표시
                GetDate();                
                #endregion

                #region  수정여부 판단
                LoadParam();

                if (TodoID > 0)
                {
                    LoadData();
                }
                else
                {
                    #region 에디터 설정

                    string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
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


            }
        }
        #endregion 

        #region Method (함수)
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                btnContinue.Text = GetResource("SaveContinue");
                btnConfirm.Text = GetResource("Save");
                btnCancel.Text = GetResource("Cancel");
                 
            }
        }


        private void LoadParam()
        {
            TodoID = GetDecryptParam("TodoID", "0").ToNumber<int>();
            PageUrl = GetDecryptParam("Page", "");
            Date = GetDecryptParam("Date", "");
            
            
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

        private void RegisterContent(string arg)
        {
            BE.ToDoInfo info = new BE.ToDoInfo();
            info.CompanyID = CompanyID;
            info.Type = "I";
            //진척도
            info.Status = rbtnlStatus.SelectedValue.ToString();
            //중요도
            info.Priority = ddlPriority.SelectedValue.ToString();
            //공유여부
            info.OpenType = ddlIsOpen.SelectedValue.ToString().ToNumber<int>();

            //제목
            info.Title = tbTitle.Text.Trim();
            //일정에반영
            if (ddlSchedule.SelectedValue.Equals("1"))
                info.ScheduleYN = "N";
            else
                info.ScheduleYN = "Y";
            //상세내용
            info.Content = GetContents();

            string strS = string.Empty;
              
            string strE = string.Empty; 
            

         
             strS = startDate.Value.ToString() + " " + ddlstartTime.SelectedValue.ToString();
             strE = endDate.Value.ToString() + " " + ddlendTime.SelectedValue.ToString();
            //}
            info.StartDate = DateTime.Parse(strS);
            //info.EndDate = info.StartDate.AddHours(1);
            info.EndDate = DateTime.Parse(strE);
            info.UserID = UserID;
            info.DeptID = CurrentUserInfo.DeptID;
            info.ReservationID = 0;
            info.IsOpen ="Y";

            BLL.ToDo biz = new BLL.ToDo();
            int iTodo = biz.AddTodo(info);

            if (ddlSchedule.SelectedValue.ToString().Equals("2"))
            {            
                BE.ScheduleInfo data = new BE.ScheduleInfo();
                data.CompanyID = info.CompanyID;
                data.ToDoID = iTodo;
                data.Type = "I";
                //제목 
                data.Title = info.Title;
                //
                data.StartTime = info.StartDate;
                data.EndTime = info.EndDate;
                data.Content = info.Content;
                data.Repeat = "N";
                data.Alert = "N";
                data.AlertType = "";
                data.AlertTime = 0;
                data.AlertTargetAddress = "";
                data.IsOpen = "Y";
                data.UserID = info.UserID;
                data.DeptID = info.DeptID;
                data.OpenType = ddlIsOpen.SelectedValue.ToString();
                data.Lunar = "N";
                AddSchedule(data);
            }

            LoadParam();
           
            if (PageUrl.Length == 0)
                PageUrl = "ScheduleMain.aspx";

            if (arg.Equals("SAVE"))
            {
               
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("PageScope", PageScope);
                paraDic.Add("Date", Date);
                paraDic.Add("Opentype", Opentype);   
                         
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/ScheduleMain.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
            else if (arg.Equals("CONTINUE"))
            {
                 
                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                paraDic.Add("PageScope", PageScope);
                paraDic.Add("Date", Date);
                paraDic.Add("Opentype", Opentype);   
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("CONTINUEADD"));
                paraDic.Add("goURL", "/DeskPlusEIP/Pim/Todo/AddN.aspx");

                /*
                if (NonActiveX == "T") //activex
                {
                    paraDic.Add("goURL", "/DeskPlusEIP/Pim/Todo/AddN.aspx");
                }
                else
                {
                    paraDic.Add("goURL", "/DeskPlusEIP/Pim/Todo/Add.aspx");
                }
                */


                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
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


        private void GetDate()
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
                string strFormat = string.Empty;

                if (i < 10)
                {
                    strFormat = "0" + i.ToString();
                }
                else
                {
                    strFormat = i.ToString();
                }

                strKey = strFormat + ":00";
                ListItem lvs1 = new ListItem(strTemp, strKey);
                ListItem lve1 = new ListItem(strTemp, strKey);

                ddlstartTime.Items.Add(lvs1);
                ddlendTime.Items.Add(lve1);

                strTemp = strVal + ":30";
                strKey = strFormat + ":30";
                ListItem lvs2 = new ListItem(strTemp, strKey);
                ListItem lve2 = new ListItem(strTemp, strKey);

                ddlstartTime.Items.Add(lvs2);
                ddlendTime.Items.Add(lve2);
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

        private void LoadData()
        {
            LoadParam();

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";

            

            DAL.ToDo dal = new DAL.ToDo();
            DataTableReader dtr = dal.GetTodoID(CompanyID, TodoID);


            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //권한
                        if (dtr["UserID"].ToString() != CurrentUserInfo.UserID.ToString())
                        {
                            return;
                        }
                        //진척도
                        rbtnlStatus.SelectedValue = dtr["Status"].ToString();
                        //중요도
                        ddlPriority.SelectedValue = dtr["Priority"].ToString();
                        //공유여부
                        
                        //제목
                        tbTitle.Text = dtr["Title"].ToString().ToReverseXSS();
                        //공유여부
                        //ddlIsOpen.SelectedValue = dtr["opentype"].ToString();
                        //내용
                        tbHiddenHTML_contents.Value = dtr["Content"].ToString();

                        //startDate.Value = DateTime.Parse(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd");
                        startDate.Value = SetDate(dtr["StartDate"].ToString());
                        ddlstartTime.SelectedValue = DateTime.Parse(dtr["StartDate"].ToString()).ToString("HH:mm");

                        //endDate.Value = DateTime.Parse(dtr["EndDate"].ToString()).ToString("yyyy-MM-dd");
                        endDate.Value = SetDate(dtr["EndDate"].ToString());
                        ddlendTime.SelectedValue = DateTime.Parse(dtr["EndDate"].ToString()).ToString("HH:mm");

                        if (!dtr["ScheduleYN"].ToString().Equals("Y"))
                        {
                            if (DateTime.Now.Hour < 10)
                            {
                                ddlstartTime.SelectedValue = DateTime.Now.ToString("H:00");
                            }
                            else
                            {
                                ddlstartTime.SelectedValue = DateTime.Now.ToString("HH:00");
                            }
                        }                        

                        tbHiddenMime_contents.Text = templateHeader + dtr["Content"].ToString() + templateFooter;

                        Contents += templateHeader + dtr["Content"].ToString() + templateFooter;


                        if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                        {
                            //ByFreeman 20120106 X-Free 에디터 추가 
                            /*
                            string strScript = String.Empty;
                            if (Request.Path.IndexOf("Add.aspx") > 0)
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

                        //일정에반영

                        //if (dtr["ScheduleYN"].ToString().Equals("Y"))
                        //{
                        //    ddlSchedule.SelectedValue = "2";
                        //}
                        //else
                        //{
                        //    ddlSchedule.SelectedValue = "1";
                        //}
                         

                        //상세내용
                    }
                }
            }


            

                
           
        }
        /*
        private void ModContent(string arg)
        {
            LoadParam();

            BE.ToDoInfo info = new BE.ToDoInfo();
            DAL.ToDo dal = new DAL.ToDo();
            DataTableReader dtr = dal.GetTodoID(CompanyID, TodoID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        info.CompanyID = CompanyID;
                        info.Type = dtr["TYPE"].ToString();
                        info.Status = dtr["Status"].ToString();
                        info.Priority = dtr["Priority"].ToString();
                        info.Content = dtr["Content"].ToString();
                        info.StartDate = DateTime.Parse(dtr["StartDate"].ToString());
                        info.EndDate = DateTime.Parse(dtr["EndDate"].ToString());
                        //info.ScheduleYN = dtr["ScheduleYN"].ToString();
                        //일정에반영
                        if (ddlSchedule.SelectedValue.Equals("1"))
                            info.ScheduleYN = "N";
                        else
                            info.ScheduleYN = "Y";

                        info.IsOpen = dtr["IsOpen"].ToString();
                        info.UserID = CurrentUserInfo.UserID;
                        info.ReservationID = int.Parse(dtr["ReservationID"].ToString());
                        info.ToDoID = TodoID;
                        info.OpenType = ddlIsOpen.SelectedValue.ToNumber<int>();

                        info.Title = tbTitle.Text.Trim();
                        info.Status = rbtnlStatus.SelectedValue.ToString();
                        info.Priority = ddlPriority.SelectedValue.ToString();
                        if (ddlSchedule.SelectedValue.Equals("1"))
                            info.ScheduleYN = "N";
                        else
                            info.ScheduleYN = "Y";

                        info.Content = GetContents();
                        string strS = string.Empty;

                        string strE = string.Empty;


                        if (ddlSchedule.SelectedValue.ToString().Equals("2"))
                        {
                            strS = startDate.Value.ToString() + " " + ddlstartTime.SelectedValue.ToString();
                            strE = endDate.Value.ToString() + " " + ddlendTime.SelectedValue.ToString();
                            info.StartDate = DateTime.Parse(strS);
                            info.EndDate = DateTime.Parse(strE);
                        }
                        info.UserID = UserID;

                        info.ReservationID = 0;
                        //info.IsOpen = "Y";
                         
                        
                        dal.ModTodo(info);
                        if (ddlSchedule.SelectedValue.ToString().Equals("2"))
                        {
                            BE.ScheduleInfo data = new BE.ScheduleInfo();
                            data.CompanyID = info.CompanyID;
                            data.ToDoID = TodoID;
                            data.Type = "I";
                            //제목 
                            data.Title = info.Title;
                            //
                            data.StartTime = info.StartDate;
                            data.EndTime = info.EndDate;
                            data.Content = info.Content;
                            data.Repeat = "N";
                            data.Alert = "N";
                            data.AlertType = "";
                            data.AlertTime = 0;
                            data.AlertTargetAddress = "";
                            data.IsOpen = "Y";
                            data.UserID = info.UserID;
                            data.DeptID = info.DeptID;
                            data.OpenType = ddlIsOpen.SelectedValue.ToString();
                            data.Lunar = "N";
                            AddSchedule(data);
                        }

                    }
                }
            }
             
            if (PageUrl.Length == 0)
                PageUrl = "ScheduleMain.aspx";

            if (arg.Equals("SAVE"))
            {

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("Date", Date);
                paraDic.Add("Page", PageUrl);
                paraDic.Add("PageScope", PageScope);
                //paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));

                paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/ScheduleMain.aspx");
                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);


            }
            else if (arg.Equals("CONTINUE"))
            {


                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                paraDic.Add("Date", Date);
                paraDic.Add("Page", PageUrl);
                paraDic.Add("PageScope", PageScope);
                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/Pim/Todo/AddN.aspx");

               
               // if (NonActiveX == "T") //activex
               // {
               //     paraDic.Add("goURL", "/DeskPlusEIP/Pim/Todo/AddN.aspx");
               // }
               // else
               // {
               //     paraDic.Add("goURL", "/DeskPlusEIP/Pim/Todo/Add.aspx");
               // }
               

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }
        */

        private void ModContent(string arg)
        {
            LoadParam();

            BE.ToDoInfo info = new BE.ToDoInfo();
            DAL.ToDo dal = new DAL.ToDo();
            DataTableReader dtr = dal.GetTodoID(CompanyID, TodoID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        info.CompanyID = CompanyID;
                        info.Type = dtr["TYPE"].ToString();
                        info.ToDoID = TodoID;

                        // 원 작성자 기준 유지
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                        // 수정 시에도 부서값 세팅
                        info.DeptID = CurrentUserInfo.DeptID;

                        info.IsOpen = dtr["IsOpen"].ToString();
                        info.ReservationID = int.Parse(dtr["ReservationID"].ToString());
                        info.OpenType = ddlIsOpen.SelectedValue.ToNumber<int>();

                        info.Title = tbTitle.Text.Trim();
                        info.Status = rbtnlStatus.SelectedValue.ToString();
                        info.Priority = ddlPriority.SelectedValue.ToString();
                        info.Content = GetContents();

                        if (ddlSchedule.SelectedValue.Equals("1"))
                            info.ScheduleYN = "N";
                        else
                            info.ScheduleYN = "Y";

                        info.StartDate = DateTime.Parse(dtr["StartDate"].ToString());
                        info.EndDate = DateTime.Parse(dtr["EndDate"].ToString());

                        if (ddlSchedule.SelectedValue.Equals("2"))
                        {
                            string strS = startDate.Value.ToString() + " " + ddlstartTime.SelectedValue.ToString();
                            string strE = endDate.Value.ToString() + " " + ddlendTime.SelectedValue.ToString();
                            info.StartDate = DateTime.Parse(strS);
                            info.EndDate = DateTime.Parse(strE);
                        }

                        dal.ModTodo(info);

                        if (ddlSchedule.SelectedValue.Equals("2"))
                        {
                            BE.ScheduleInfo data = new BE.ScheduleInfo();
                            data.CompanyID = info.CompanyID;
                            data.ToDoID = TodoID;
                            data.Type = "I";
                            data.Title = info.Title;
                            data.StartTime = info.StartDate;
                            data.EndTime = info.EndDate;
                            data.Content = info.Content;
                            data.Repeat = "N";
                            data.Alert = "N";
                            data.AlertType = "";
                            data.AlertTime = 0;
                            data.AlertTargetAddress = "";
                            data.IsOpen = "Y";
                            data.UserID = info.UserID;
                            data.DeptID = info.DeptID;
                            data.OpenType = ddlIsOpen.SelectedValue.ToString();
                            data.Lunar = "N";
                            AddSchedule(data);
                        }
                    }
                }
            }

            if (arg.Equals("SAVE"))
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("Date", Date);
                paraDic.Add("Page", PageUrl);
                paraDic.Add("PageScope", PageScope);
                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/Pim/Schedule/ScheduleMain.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
            else if (arg.Equals("CONTINUE"))
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("Date", Date);
                paraDic.Add("Page", PageUrl);
                paraDic.Add("PageScope", PageScope);
                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/Pim/Todo/AddN.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }


        private void SettingControl()
        {
            startDate.Value = DateTime.Now.ToShortDateString();
            endDate.Value = DateTime.Now.ToShortDateString();

            ListItem ltcate1 = new ListItem(GetResource("TodoShare1"), "0");
            ListItem ltcate2 = new ListItem(GetResource("TodoShare2"), "3");
            ListItem ltcate3 = new ListItem(GetResource("TodoShare3"), "1");
            ListItem ltcate4 = new ListItem(GetResource("TodoShare4"), "2");

            ddlIsOpen.Items.Add(ltcate1);
            ddlIsOpen.Items.Add(ltcate2);
            ddlIsOpen.Items.Add(ltcate3);
            ddlIsOpen.Items.Add(ltcate4);
            //ddlIsOpen.SelectedIndex = 2;

            //기본게시범위설정
            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "PimViewType");
            string codeID = "2";
            for (int i = 0; i < dv.Count; i++)
            {
                codeID = dv[i]["CodeID"].ToString();
            }
            ddlIsOpen.SelectedValue = codeID;


            AttachLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.PIM, "AttachLimitSize", "1");//용량
           
            

            

            #region  정리1
            ListItem lv = new ListItem();
            lv.Value = "W";
            lv.Text = "<img alt='' src='"+this.BaseImgPath+"/Pims/Status_NotBegin.gif'/>" + GetResource("priority1");
            ListItem lv1 = new ListItem();
            lv1.Value = "I";
            lv1.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_Going.gif'/>" + GetResource("priority2");
            ListItem lv2 = new ListItem();
            lv2.Value = "E";
            lv2.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_Done.gif'/>" + GetResource("priority3");
            ListItem lv3 = new ListItem();
            lv3.Value = "C";
            lv3.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_Cancel.gif'/>" + GetResource("priority4");
            ListItem lv4 = new ListItem();
            lv4.Value = "T";
            lv4.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_Pass.gif'/>" + GetResource("priority5");
            ListItem lv5 = new ListItem();
            lv5.Value = "D";
            lv5.Text = "<img alt='' src='" + this.BaseImgPath + "/Pims/Status_Postpone.gif'/>" + GetResource("priority6");

            rbtnlStatus.Items.Add(lv);
            rbtnlStatus.Items.Add(lv1);
            rbtnlStatus.Items.Add(lv2);
            rbtnlStatus.Items.Add(lv3);
            rbtnlStatus.Items.Add(lv4);
            rbtnlStatus.Items.Add(lv5);

            rbtnlStatus.SelectedValue = "W";


            ListItem c1 = new ListItem(GetResource("TOP"), "1");
            ListItem c2 = new ListItem(GetResource("MIDDLE"), "2");
            ListItem c3 = new ListItem(GetResource("BOTTOM"), "3");
            ddlPriority.Items.Add(c1);
            ddlPriority.Items.Add(c2);
            ddlPriority.Items.Add(c3);

            ListItem d1 = new ListItem(GetResource("UnPimEbem"), "1");
            ListItem d2 = new ListItem(GetResource("Ebem"), "2");
            ddlSchedule.Items.Add(d1);
            ddlSchedule.Items.Add(d2);
            #endregion

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
                LoadParam();

                
                string args = eventArgs;
                if (TodoID == 0)
                {
                    RegisterContent(args);
                 
                }
                else
                {
                    ModContent(args);
                }

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
            }
        }


        private void AddSchedule(BE.ScheduleInfo info)
        {
            Pim.DAL.Schedule bll = new DAL.Schedule();
            int iSch = bll.Add(info);
            BE.DisplayInfo DisInfo = new BE.DisplayInfo();
            DisInfo.CompanyID = info.CompanyID;
            DisInfo.ScheduleID = iSch;
            DisInfo.UserID = info.UserID;
            DisInfo.StartDate = info.StartTime;
            DisInfo.EndDate = info.EndTime;
            
            if (DisInfo.StartDate == DisInfo.EndDate)
            {
                DisInfo.EndDate = DisInfo.EndDate.AddHours(1);
            }
            DAL.Display bizDis = new DAL.Display();
            bizDis.AddDisplay(DisInfo);
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
