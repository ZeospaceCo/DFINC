using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Opinion.Web.Admin.Content
{
    public partial class QuesAdd : Zeospace.DeskPlus.Opinion.Web.Base.BasePageOpinionAdmin
    {
        #region 변수설정

        public string PageTitle = string.Empty; //페이지제목
        public string EventArgs = string.Empty;
        public int CompanyID = 0;
        public int UserID = 0;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Admin.Content.QuesAdd).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        ///
        /// </summary>
        private void LoadData()
        {
            SwfUploadGUID = System.Guid.NewGuid().ToString();

            LoadParm();     //파라미터처리
            SetLanguage();  //다국어처리

            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";
            tbHiddenMime_contents.Text = templateHeader + templateFooter;

            /*
            string strScript = String.Empty;
            if (Request.Path.IndexOf("QuesAdd.aspx") > 0)
            {
                strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.htmlValue = form1.tbHiddenMime_contents.value;" + "<" + "/script>";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnControlInit", strScript);

                NonActiveX = "F";
            }
            else
            {
                NonActiveX = "T";
            }
            */
            NonActiveX = "T";

            #region 에디터 본문 이미지 첨부 경로

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            System.Guid guid = System.Guid.NewGuid();
            string reqTime = System.DateTime.Now.ToString("yyyyMM");
            UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Opinion/Ques/" + reqTime + "/editor/" + guid.ToString();
            if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

            #endregion 에디터 본문 이미지 첨부 경로
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 다국어처리
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //Title
                this.PageTitle = this.GetResource("TitQuesAdd");

                //Button
                this.cbtnConfirm.Text = this.GetResource("BtnSubmit");
                this.cbtnCancel.Text = this.GetResource("BtnCancel");
                this.cbtnUptSuper.Text = this.GetResource("BtnSuper");
                //this.cbtnFileAdd.Text = this.GetResource("BtnFileAdd");


                //DropDown Item
                this.ddlGroupName.Items.Clear();
                this.ddlGroupName.Items.Add(new ListItem(resMng.GetString("Header1"), "말머리1"));
                this.ddlGroupName.Items.Add(new ListItem(resMng.GetString("Header2"), "말머리2"));
                this.ddlGroupName.Items.Add(new ListItem(resMng.GetString("Header3"), "말머리3"));

                this.ddlIsActive.Items.Clear();
                this.ddlIsActive.Items.Add(new ListItem(this.GetResource("Unit_Active"), "1"));
                this.ddlIsActive.Items.Add(new ListItem(this.GetResource("Unit_ActiveNon"), "0"));

                //checkbox
                this.cbIsMain.Text = this.GetResource("TitIsMain");
                this.cbIsMain.ToolTip = this.GetResource("TitF5");
                this.cbIsReply.Text = this.GetResource("TitReply");
                this.cbIsReply.ToolTip = GetResource("TitF6");
                this.cbIsOpen.Text = this.GetResource("TitIsOpen");
                this.cbIsOpen.ToolTip = GetResource("TitF7");
                this.cbIsVoteUser.Text = this.GetResource("VoteUser");
                this.cbIsVoteUser.ToolTip = GetResource("TitF8");
                this.cbIsSecret.Text = this.GetResource("Secret");
                this.cbIsSecret.ToolTip = GetResource("TitSecret");
            }
        }

        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParm()
        {
        }

        /// <summary>
        /// 추가
        /// </summary>
        private int Addition()
        {
            BE.QuesInfo qInfo = new BE.QuesInfo();
            BLL.Ques qBll = new BLL.Ques();

            qInfo.CompanyID = CurrentUserInfo.CompanyID;
            qInfo.Title = ctbTitle.Text.Trim().ToXSS();
            qInfo.Cont = GetContents();// ?? ToXSS()
            qInfo.StartDate = DateTime.Parse(ctbSDate.Text);
            qInfo.EndDate = DateTime.Parse(ctbEDate.Text);
            qInfo.GroupName = ddlGroupName.SelectedValue;
            qInfo.IsMain = (cbIsMain.Checked) ? "1" : "0";
            qInfo.IsReply = (cbIsReply.Checked) ? "1" : "0";
            qInfo.IsOpen = (cbIsOpen.Checked) ? "1" : "0";
            qInfo.IsVoteUser = (cbIsVoteUser.Checked) ? "1" : "0";
            qInfo.IsSecret = (cbIsSecret.Checked) ? "1" : "0";
            qInfo.IsActive = ddlIsActive.SelectedValue;
            qInfo.SuperDeptID = tbDeptID.Text.ToNumber<int>();
            qInfo.WriteUserID = CurrentUserInfo.UserID;

            //첨부파일처리
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            qInfo.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            return qBll.Add(qInfo);
        }

        /// <summary>
        /// 작성자 :
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

        #endregion (함수)

        #region (프로퍼티)

        /// <summary>
        /// 에디터-파일처리경로
        /// </summary>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }

        /// <summary>
        /// 파일업로드
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        /// ActiveX 유무
        /// </summary>
        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }

        #endregion (프로퍼티)

        #region (이벤트핸들러)

        /// <summary>
        /// [확인][삭제]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                switch (EventArgs.ToUpper())
                {
                    case "ADD":

                        int quesID = Addition();
                        if (quesID > 0)
                        {
                            Dictionary<string, string> parmDic = new Dictionary<string, string>();
                            parmDic.Add("menuID", MenuID);
                            parmDic.Add("pageSize", PageSize.ToString());
                            parmDic.Add("pageNo", PageNo.ToString());
                            parmDic.Add("sKey", SKey);
                            parmDic.Add("sVal", SVal);
                            parmDic.Add("sPeriod", SPeriod);
                            parmDic.Add("sPeriodVal", SPeriodVal);
                            parmDic.Add("order", Order);
                            parmDic.Add("quesID", quesID.ToString());

                            parmDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());     //AlertMsgEnum.Alert_Success.ToString()
                            parmDic.Add("alertMsg", this.GetResource("GoToInputInfo"));

                            if (this.Request.Url.ToString().IndexOf("QuesAdd.aspx") > 0)
                            {
                                parmDic.Add("goURL", "/DeskPlusEIP/Opinion/Admin/Content/QuesMod.aspx");
                            }
                            else
                            {
                                parmDic.Add("goURL", "/DeskPlusEIP/Opinion/Admin/Content/QuesModN.aspx");
                            }

                            string encParams = GetEncryptParam(Utility.GetParameterDic(parmDic));
                            Utility.Redirect(encParams);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", AlertMsgEnum.Alert_Error.ToString(), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParm(); //파라미터처리

            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion (이벤트핸들러)
    }
}