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

namespace Zeospace.DeskPlus.Pim.Web.Admin.Holiday
{
    public partial class Mod : Base.BasePagePimAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;

        public int ScheduleID = 0;
        public int DisplayID = 0; 
        public string SearchField = "";
        public string Searchkey = "";   
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Admin.Holiday.Mod).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }

        private void LoadData()
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            SettingControl();
            LoadContent();
            LoadAttach();
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

            ddltime.Items.Add(ltime2);
            ddltime.Items.Add(ltime3);
            ddltime.Items.Add(ltime4);
            ddltime.Items.Add(ltime5);
            ddltime.Items.Add(ltime6);
            ddltime.Items.Add(ltime7);
            ddltime.Items.Add(ltime8);
            ddltime.Items.Add(ltime9);
            ddltime.Items.Add(ltime10);


            ListItem ltcate1 = new ListItem(GetResource("Admin1"), "G");
            ListItem ltcate2 = new ListItem(GetResource("Admin2"), "M");
            ListItem ltcate3 = new ListItem(GetResource("Admin3"), "K");

            ddlCategory.Items.Add(ltcate1);
            ddlCategory.Items.Add(ltcate2);
            ddlCategory.Items.Add(ltcate3);

            MakeTime();

        }


        private void MakeTime()
        {
            /*
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
                strKey = i.ToString() + ":00";
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
            //ddlstartTime.SelectedValue = DateTime.Now.ToString("HH:00");
            //ddlendTime.SelectedValue = DateTime.Now.ToString("HH:00");
            */

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
                    strKey = "0" + i.ToString() + ":00";
                }
                else
                {
                    strKey = i.ToString() + ":00";
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
                info.Type = ddlCategory.SelectedValue.ToString();

                //제목
                info.Title = txttitle.Value.ToString().Trim();
                //날짜
                string strS = startDate.Value.ToString() + " " + ddlstartTime.SelectedValue.ToString();
                string strE = startDate.Value.ToString() + " " + ddlendTime.SelectedValue.ToString();

                info.StartTime = DateTime.Parse(strS);
                info.EndTime = DateTime.Parse(strE);


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
                info.OpenType = "100";

                info.ReservationID = 0;
                info.Lunar = "N";
                info.StatusID = 0; //기본값

                //첨부파일
                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
                info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);


                Pim.DAL.Schedule bll = new DAL.Schedule();
                bll.Mod(info);

                #region 첨부파일 추가
                if (info.HiddenSpoolPath != "" && info.HiddenSpoolPath != null)
                {
                    HttpContext context = HttpContext.Current;
                    DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(info.HiddenSpoolPath));
                    if (di.Exists)
                    {
                        DAL.Attach attachdal = new DAL.Attach();
                        attachdal.AddList(CompanyID, ScheduleID, info.HiddenSpoolPath);
                    }
                }


                #endregion

                DAL.Display bizDis = new DAL.Display();
                BE.DisplayInfo DisInfo = new BE.DisplayInfo();
                DisInfo.CompanyID = CompanyID;
                DisInfo.ScheduleID = ScheduleID;
                //DisInfo.UserID = UserID;
                bizDis.DelDisplayID(DisInfo);

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

                if (arg.Equals("MOD"))
                {
                    if (ScheduleID > 0)
                    {
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("ScheduleID", ScheduleID.ToString());
                        paraDic.Add("Searchkey", Searchkey);
                        paraDic.Add("SearchField", SearchField);
                        paraDic.Add("PageScope", PageScope.ToString());
                        paraDic.Add("Date", Date);
                        paraDic.Add("Opentype", Opentype);

                        paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                        paraDic.Add("alertMsg", GetResource("GoList"));

                        paraDic.Add("goURL", "/DeskPlusEIP/Pim/Admin/Holiday/View.aspx");
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
            ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();
            DisplayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("SearchField", "");
            Searchkey = GetDecryptParam("Searchkey", "");
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
            DAL.Schedule bll = new DAL.Schedule();

            List<BE.ScheduleInfo> list = bll.GetSheduleDataID(info);

            foreach (BE.ScheduleInfo _info in list)
            {
                //제목
                txttitle.Value = _info.Title.ToReverseXSS();
                //날짜
                startDate.Value = _info.StartTime.ToShortDateString();

                ddlstartTime.SelectedValue = _info.StartTime.ToString("HH:mm");
                ddlendTime.SelectedValue = _info.EndTime.ToString("HH:mm");

                //음력양력
                if (_info.Lunar.Equals("Y"))
                {
                    rdSun.Checked = true;
                }
                else
                {
                    rbmoon.Checked = true;
                }
                //종류
                ddlCategory.SelectedValue = _info.Type;

                //내용
                tbHiddenHTML_contents.Value = _info.Content;


                string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang,"");
                string templateFooter = "</BODY></HTML>";

                tbHiddenMime_contents.Text = templateHeader + _info.Content + templateFooter;

                Contents += templateHeader + info.Content + templateFooter;

                if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                {
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
                }

                #region 에디터 본문 이미지 첨부 경로
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = System.DateTime.Now.ToString("yyyyMM");
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Pim/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로
                #endregion
            }

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