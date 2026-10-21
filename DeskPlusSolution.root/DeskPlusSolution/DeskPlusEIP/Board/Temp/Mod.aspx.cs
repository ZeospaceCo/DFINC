using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;

using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Temp
{
    public partial class Mod : Board.Web.Base.BasePageBoard
    {
        #region 변수설정

        public int CompanyID = 0;
        public int UserID = 0;
        public int ListID = 0;
        public string Contents = string.Empty; //내용
        public string AttachLimitSize = string.Empty; //첨부파일 제한용량

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Temp.Mod).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;
            AttachLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "AttachLimitSize", "1");//용량

            LoadParam();

            SetFontColor();
            SetFontSize();

            LoadList();
            LoadEtc();
            LoadAttach();
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnConfirm.Text = resMng.GetString("Confirm");
            }
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 첨부파일 정보 가져오기
        /// </summary>
        private void LoadAttach()
        {
            DAL.MyAttachFiles biz = new DAL.MyAttachFiles();
            List<BE.MyAttachFileInfo> al = biz.GetAttachFileLists(CurrentUserInfo.CompanyID, 0, ListID);

            string saveFileList = "";

            if (al.Count > 0)
            {
                foreach (BE.MyAttachFileInfo info in al)
                {
                    saveFileList += "S>" + info.AttachFileID + ">" + info.FilePath + ">" + info.FileName + ">" + info.FileGUID + "|";
                }
            }

            //fileList = fileList.TrimEnd('|');
            saveFileList = saveFileList.TrimEnd('|');

            //tbHiddenFileList.Text = fileList;					//첨부파일 정보를 저장하며 리스트박스와 동기화
            tbHiddenFileList.Text = saveFileList;					//첨부파일 정보를 저장하며 리스트박스와 동기화
            tbHiddenSaveFileList.Text = saveFileList;			//서버 파일 삭제시 비교로 사용 될 원본 파일 리스트(FileID 포함)
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 리스트
        /// </summary>
        private void LoadList()
        {
            bool IsAuth = false;

            DAL.MyTemplates dl = new DAL.MyTemplates();
            List<MyTemplateInfo> list = dl.GetMyTemplateLists(CurrentUserInfo.CompanyID, CategoryID, ListID, UserID);

            foreach (MyTemplateInfo info in list)
            {
                ctbName.Text = info.Title.ToReverseXSS();

                cbOptionModify.Checked = (info.OptionModify == "1") ? true : false;

                tbHiddenHTML_contents.Value = info.Content;

                string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", GetResource("Anonymity"));
                AnonymityName = (PreLang.Equals("ko")) ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];

                string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
                string templateFooter = "</BODY></HTML>";

                tbHiddenMime_contents.Text = templateHeader + info.Content + templateFooter;
                Contents += info.Content;

                if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                {
                    /*
                    string strScript = String.Empty;
                    if (Request.Path.IndexOf("Mod.aspx") > 0)
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

                #region 에디터 본문 이미지 첨부 경로

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = FileHelper.GetYearMonth();
                UploadPath = baseFileDirectory + CompanyID + "/Board/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                #endregion 에디터 본문 이미지 첨부 경로

                //접근여부
                IsAuth = (CurrentUserInfo.UserID == info.WriteUserID) ? true : false;
                //타인수정 가능시, 수정가능
                if (!IsAuth) IsAuth = (info.OptionModify == "1") ? true : false;

                ScreenShot = info.ScreenShot.ToNull();
            }

            if (!IsAuth)
            {
                ctbName.Text = "";
                Contents = "";

                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 기타 정보 가져오기
        /// </summary>
        private void LoadEtc()
        {
            DAL.MyTemplateEtc biz = new DAL.MyTemplateEtc();
            List<BE.MyTemplateEtcInfo> al = biz.GetList(CompanyID, CategoryID, ListID);

            if (al.Count > 0)
            {
                foreach (BE.MyTemplateEtcInfo info in al)
                {
                    if (info.Etc2.ToNull().Length > 0)
                    {
                        string[] etcStr = info.Etc2.Split(',');
                        if (etcStr[0].ToString() != "") ddlFontColor.Value = etcStr[0].ToString().ToReverseXSS();
                        if (etcStr[1].ToString() != "") ddlFontSize.Value = etcStr[1].ToString();
                        cbxBold.Checked = (etcStr[2].ToString() == "1") ? true : false;
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 폰트 칼라
        /// </summary>
        private void SetFontColor()
        {
            ddlFontColor.Items.Clear();
            string xmlPath = Server.MapPath("/DeskplusEIP/Common/App_Data/" + PreLang + "/Common_FontColor.xml");
            List<XmlDataInfo> al = UtilHelper.XmlLoadData(xmlPath);

            foreach (XmlDataInfo be in al)
            {
                ListItem tmpItem = new ListItem(be.KeyValue, be.KeyID);
                tmpItem.Attributes.Add("style", "color:" + be.KeyID);
                this.ddlFontColor.Items.Add(tmpItem);
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 사이즈
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-11-13
        /// </summary>
        private void SetFontSize()
        {
            ddlFontSize.Items.Clear();
            string xmlPath = Server.MapPath("/DeskplusEIP/Common/App_Data/" + PreLang + "/Common_FontSize.xml");
            List<XmlDataInfo> al = UtilHelper.XmlLoadData(xmlPath);

            foreach (XmlDataInfo be in al)
            {
                ListItem tmpItem = new ListItem(be.KeyValue, be.KeyID);
                this.ddlFontSize.Items.Add(tmpItem);
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
        /// 작성자 : 심재운
        /// 용도 : 스크린샷 이미지 (0:파일명
        /// </summary>
        /// <returns></returns>
        private string[] GetScreenShot()
        {
            bool fileOK = false;

            string[] screenStr = new string[3];

            string fileScreenShot = tbScreenShot.Text.Trim();

            //스크린샷 이미지 업로드
            if (fileScreenShot.Length > 0)
            {
                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };
                string fileExtension = "";

                //## 기존 첨부파일 경우 (기존경로+파일명^신규경로+신규파일명)
                if (fileScreenShot.IndexOf('^') != -1)
                {
                    string oriFileFullPath = "";//원본파일 full 경로
                    string thumFileFullPath = "";//섬네일 저장할 파일 full 경로

                    DAL.MyAttachFiles dal = new DAL.MyAttachFiles();
                    List<BE.MyAttachFileInfo> alAttach = dal.GetAttachLists(CurrentUserInfo.CompanyID, ListID,
                        fileScreenShot.Split('^')[0].ToNumber<int>());

                    foreach (BE.MyAttachFileInfo info in alAttach)
                    {
                        oriFileFullPath = info.FilePath + info.FileGUID;
                        thumFileFullPath = info.FilePath + fileScreenShot.Split('^')[1];
                    }

                    fileExtension = System.IO.Path.GetExtension(Server.MapPath(oriFileFullPath)).ToLower(); //확장자명
                    screenStr[1] = "0"; //구분자
                    screenStr[2] = thumFileFullPath; //파일경로 + 섬네일 파일명(Thum_)
                    fileScreenShot = oriFileFullPath; //파일경로 + 원본 파일명
                }
                else
                {
                    //## 신규 첨부파일 경우
                    filePath = filePath + "{0}/Users/{1}/Spool/{2}/{3}";
                    string screenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, fileScreenShot);
                    fileExtension = System.IO.Path.GetExtension(Server.MapPath(screenSpoolPath)).ToLower();

                    screenStr[1] = "1"; //구분자
                    screenStr[2] = "";
                }

                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                        break;
                    }
                }

                fileScreenShot = (fileOK) ? fileScreenShot : "";
            }

            screenStr[0] = fileScreenShot;

            return screenStr;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시물 수정
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        /// <returns></returns>
        private bool Modify()
        {
            BE.MyTemplateInfo info = new BE.MyTemplateInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = CategoryID;
            info.MyTemplateID = ListID;
            info.Title = ctbName.Text.Trim();
            info.Content = GetContents();
            info.MobileContent = tbHiddenText_contents.Text.Trim();
            info.OptionModify = (cbOptionModify.Checked) ? "1" : "0";
            info.OptionTop = "0"; //공지 등록 여부
            info.WriteUserID = CurrentUserInfo.UserID;
            info.WriteDate = System.DateTime.Now;

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            string filePath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/";
            info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            #region 스크린샷

            string[] getScreen = GetScreenShot();

            info.ScreenShot = "";

            //기존 파일을 스크린샷 생성
            if (getScreen[1] == "0" && getScreen[0].Length > 0)
            {
                info.ScreenShot = getScreen[2]; //신규섬네일 저장 경로+파일
            }
            else if (getScreen[1] == "1" && getScreen[0].Length > 0)
            {
                //신규 추가파일을 스크린샷 생성
                info.ScreenShot = getScreen[0];
            }

            #endregion 스크린샷

            //기타옵션 저장하기
            Board.BE.MyTemplateEtcInfo etcInfo = new MyTemplateEtcInfo();
            etcInfo.CompanyID = CurrentUserInfo.CompanyID;
            etcInfo.CategoryID = CategoryID;
            etcInfo.ListID = ListID;
            etcInfo.Etc1 = "";

            string bold = (cbxBold.Checked) ? "1" : "0";
            string etc = ddlFontColor.Value + "," + ddlFontSize.Value + "," + bold;

            etcInfo.Etc2 = etc;
            etcInfo.Etc3 = "";
            etcInfo.Etc4 = "";
            etcInfo.Etc5 = "";

            //0 : 기존; 1: 신규
            string screenGubun = getScreen[1];

            //스크린샷을 전혀 사용하지 않을 경우
            if (string.IsNullOrEmpty(screenGubun))
            {
                screenGubun = "0";//신규처리
                info.ScreenShot = "";
            }

            BLL.MyTemplate bll = new BLL.MyTemplate();

            bll.Mod(info, etcInfo, screenGubun);

            //스크린샷 이미지 업로드
            if (info.ScreenShot.Length > 0)
            {
                string reqTime = FileHelper.GetYearMonth();

                if (getScreen[1] == "0")
                {
                    //기존파일 스냅샷 만들기

                    // /DeskPlusEIP/FileData/1/Board/201112/100642/
                    if (System.IO.File.Exists(Server.MapPath(getScreen[0])))
                    {
                        string imgSize = UtilHelper.GetThumnailSize(Server.MapPath(getScreen[0]), 160, 160);

                        System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(getScreen[0]));
                        System.Drawing.Image thumb = image.GetThumbnailImage(imgSize.Split(',')[0].ToNumber<int>(), imgSize.Split(',')[1].ToNumber<int>(),
                            delegate() { return false; }, (IntPtr)0);

                        thumb.Save(Server.MapPath(info.ScreenShot));
                    }
                }
                else if (getScreen[1] == "1")
                {
                    //신규파일 스냅샷 만들기
                    string screenShotPath = baseFileDirectory + "{0}/Users/{1}/Board/{2}/{3}";
                    screenShotPath = string.Format(screenShotPath, CurrentUserInfo.CompanyID.ToString(),
                        CurrentUserInfo.UserID.ToString(), reqTime, ListID);
                    screenShotPath = screenShotPath + "/" + tbScreenShot.Text.Trim();

                    string fileDirPath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
                    string screenSpoolPath = string.Format(fileDirPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, info.ScreenShot);

                    if (System.IO.File.Exists(Server.MapPath(screenSpoolPath)))
                    {
                        string imgSize = UtilHelper.GetThumnailSize(Server.MapPath(screenSpoolPath), 160, 160);

                        System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(screenSpoolPath));
                        System.Drawing.Image thumb = image.GetThumbnailImage(imgSize.Split(',')[0].ToNumber<int>(), imgSize.Split(',')[1].ToNumber<int>(),
                            delegate() { return false; }, (IntPtr)0);

                        thumb.Save(Server.MapPath(screenShotPath));
                    }
                }
            }

            return true;
        }

        #endregion Method (함수)

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시물버튼 클릭
        /// 작성자     : daniel
        /// 최초작성일 : 2011-04-25
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnWrite_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                if (Modify())
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("categoryID", CategoryID.ToString());
                    paraDic.Add("listID", ListID.ToString());
                    paraDic.Add("currentPage", CurrentPage.ToString());
                    paraDic.Add("searchTerm", SearchTerm);
                    paraDic.Add("searchTermValue", SearchTermValue);
                    paraDic.Add("searchField", SearchField);
                    paraDic.Add("searchValue", SearchValue);
                    paraDic.Add("order", Order);
                    paraDic.Add("types", Types);

                    paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                    paraDic.Add("alertMsg", GetResource("GoList"));
                    paraDic.Add("goURL", "/DeskplusEIP/Board/Temp/View.aspx");

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    Utility.Redirect(encParams);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        #endregion Event Handler (이벤트 핸들러)

        #region Property (프로퍼티)

        /// <summary>
        /// 페이지 이동
        /// </summary>
        public string GoPage
        {
            get
            {
                BLL.Lists biz = new BLL.Lists();
                return "/DeskplusEIP/Board/Temp/" + biz.GoListPageTemp(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            }
        }

        /// <summary>
        /// ScreenShot
        /// </summary>
        /// <value>
        /// The screen shot.
        /// </value>
        public string ScreenShot
        {
            get { return GetViewStateString("ScreenShot", ""); }
            set { ViewState["ScreenShot"] = value; }
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
        /// SwfUploadGUID
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

        #endregion Property (프로퍼티)
    }
}