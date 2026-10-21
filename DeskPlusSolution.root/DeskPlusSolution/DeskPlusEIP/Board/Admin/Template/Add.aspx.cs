using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Admin.Template
{
    public partial class Add : Zeospace.DeskPlus.Board.Web.Base.BasePageBoardAdmin
    {
        #region Declaration (컨트롤 선언)

        private string eventArgs = String.Empty;
        public int CompanyID = 0;
        public int UserID = 0;
        public string Contents = string.Empty; //내용

        #endregion Declaration (컨트롤 선언)

        #region 변수설정

        

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Template.Add).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            TemplateID = GetDecryptParam("templateID", "0").ToNumber<int>();

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Submit");
                CbtnCancel.Text = resMng.GetString("Cancel");
                CbtnDel.Text = resMng.GetString("Del");
            }
        }

        private void LoadData()
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</BODY></HTML>";

            if (TemplateID == 0) //write
            {
                CbtnConfirm.Text = GetResource("Add");          // "추가"

                tbHiddenMime_contents.Text = templateHeader + templateFooter;

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
                string reqTime = FileHelper.GetYearMonth();
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Board/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                #endregion 에디터 본문 이미지 첨부 경로
            }
            else if (TemplateID > 0) //edit
            {
                CbtnConfirm.Text = GetResource("Modify");       //"수정"

                DAL.Templates biz = new DAL.Templates();
                List<BE.TemplatesInfo> al = biz.GetTemplateLists(CurrentUserInfo.CompanyID, TemplateID);

                if (al.Count > 0)
                {
                    foreach (BE.TemplatesInfo info in al)
                    {
                        ctbName.Text = info.Title.ToReverseXSS();
                        tbHiddenHTML_contents.Value = info.Content.ToEditorXSS();

                        tbHiddenMime_contents.Text = templateHeader + info.Content.ToEditorXSS() + templateFooter;

                        //Contents += templateHeader + info.Content + templateFooter;
                        Contents += info.Content.ToEditorXSS();

                        if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                        {
                            /*
                            string strScript = String.Empty;
                            if (Request.Path.IndexOf("Add.aspx") > 0)
                            {
                                strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.htmlValue = form1.contents.value;" + "<" + "/script>";

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
                    }
                }
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자 : 심재운
        /// 추가
        /// </summary>
        private void Register()
        {
            BE.TemplatesInfo info = new BE.TemplatesInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.TemplateID = 0;
            info.Title = ctbName.Text;
            info.Content = GetContents();
            info.WriteUserID = CurrentUserInfo.UserID;

            DAL.Templates biz = new DAL.Templates();
            biz.AddMod(info);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("GoToList"));
            paraDic.Add("goURL", "/DeskplusEIP/Board/Admin/Template/List.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
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
        /// 작성자 : 심재운
        /// 수정
        /// </summary>
        private void Modify()
        {
            BE.TemplatesInfo info = new BE.TemplatesInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.TemplateID = TemplateID;
            info.Title = ctbName.Text;
            info.Content = GetContents();
            info.WriteUserID = CurrentUserInfo.UserID;

            DAL.Templates biz = new DAL.Templates();
            biz.AddMod(info);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
            paraDic.Add("alertMsg", GetResource("GoToList"));
            paraDic.Add("goURL", "/DeskplusEIP/Board/Admin/Template/List.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        /// <summary>
        /// 템플릿 삭제
        /// </summary>
        private void Del()
        {
            DAL.Templates dal = new DAL.Templates();
            dal.Del(CurrentUserInfo.CompanyID, TemplateID);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
            paraDic.Add("alertMsg", GetResource("GoToList"));
            paraDic.Add("goURL", "/DeskplusEIP/Board/Admin/Template/List.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        #endregion (함수)

        #region (속성)

        /// <summary>
        /// TemplateID
        /// </summary>
        /// <value>
        /// The template ID.
        /// </value>
        public int TemplateID
        {
            get { return GetViewStateInt("TemplateID", "0"); }
            set { ViewState["TemplateID"] = value; }
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

        #endregion (속성)

        #region (이벤트핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            try
            {
                string args = eventArgs;
                switch (args)
                {
                    case "ADD":  //추가
                        Register();
                        break;

                    case "MOD":  //수정
                        if (TemplateID > 0)
                        {
                            Modify();
                        }
                        break;

                    case "DEL":  //삭제
                        if (TemplateID > 0)
                        {
                            Del();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

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

        #endregion (이벤트핸들러)
    }
}