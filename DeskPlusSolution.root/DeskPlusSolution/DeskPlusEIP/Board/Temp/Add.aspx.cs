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
using Zeospace.DeskPlus.Board.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Temp
{
    public partial class Add : Board.Web.Base.BasePageBoard
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        private string eventArgs = String.Empty;
        public string Contents = string.Empty; //내용
        #endregion

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Temp.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// </summary>
        private void LoadData()
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            LoadParam();
            SetFontColor();
            SetFontSize();

            LoadRegister();
        }

        /// <summary>
        /// 추가시 초기화
        /// </summary>
        private void LoadRegister()
        {
            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang,"");
            string templateFooter = "</body></html>";

            tbHiddenMime_contents.Text = templateHeader + templateFooter;
            
            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
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
                UploadPath = baseFileDirectory + CompanyID + "/Board/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                #endregion 에디터 본문 이미지 첨부 경로

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
                cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnConfirm.Text = resMng.GetString("Confirm");                
            }
        }


        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            
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
        /// 임시저장
        /// </summary>
        /// <returns></returns>
        private int AdditionTemp()
        {

            BE.MyTemplateInfo info = new BE.MyTemplateInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = 0;
            info.MyTemplateID = 0;

            info.Title = ctbName.Text.Trim();
            info.Content = GetContents();
            info.MobileContent = tbHiddenText_contents.Text.Trim();

            info.OwnerDeptID = CurrentUserInfo.DeptID;
            info.OptionModify = (cbOptionModify.Checked) ? "1" : "0";
            info.OptionTop = "0"; //공지 등록 여부
            info.ReadCount = 0;
            info.WriteUserID = CurrentUserInfo.UserID;
            info.WriteUserName = CurrentUserInfo.Name;
            info.WriteDate = System.DateTime.Now;

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            //screen shot
            /*
            * [0] 스크린샷 이미지(파일경로 + 파일명)
            * [1] 0:기존파일;1:신규파일
            * [2] 신규경로 + 신규파일명 (기존파일용)
            */

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

            //기타옵션 저장하기
            Board.BE.MyTemplateEtcInfo etcInfo = new MyTemplateEtcInfo();
            etcInfo.CompanyID = CurrentUserInfo.CompanyID;
            etcInfo.CategoryID = 0;
            etcInfo.ListID = 0;
            etcInfo.Etc1 = "";

            string bold = (cbxBold.Checked) ? "1" : "0";
            string etc = ddlFontColor.Value + "," + ddlFontSize.Value + "," + bold;

            etcInfo.Etc2 = etc;
            etcInfo.Etc3 = "";
            etcInfo.Etc4 = "";
            etcInfo.Etc5 = "";

            BLL.MyTemplate bll = new BLL.MyTemplate();

            var rtnval = bll.Add(info, etcInfo);

            #region DOC_STREAM
            var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "DocStreamYN", "N");

            if (docStreamYN == "Y")
            {
                try
                {
                    DAL.MyAttachFiles dalFile = new DAL.MyAttachFiles();
                    DocStream ds = new DocStream();
                    var newDSList = dalFile.GetAttachFileLists(info.CompanyID, info.CategoryID, rtnval);
                    
                    for (int i = 0; i < newDSList.Count; i++)
                    {
                        ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachFileID, "BOA_MYATTACH");
                        ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachFileID, "BOA_MYATTACH");
                    }
                }
                catch (Exception ex)
                {
                    DAL.MyAttachFiles dalFile = new DAL.MyAttachFiles();
                    DocStream ds = new DocStream();
                    var DSList = dalFile.GetAttachFileLists(info.CompanyID, info.CategoryID, rtnval);
                    for (int i = 0; i < DSList.Count; i++)
                    {
                        ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachFileID, "BOA_MYATTACH");
                    }
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("BOA_MyAttach_Add_DS : " + ex.Message);
                    rtnval = -1;
                }

            }
            #endregion

            return rtnval;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 스크린샷 이미지
        /// </summary>
        /// <returns></returns>
        private string[] GetScreenShot()
        {
            bool fileOK = false;

            /*
             * [0] 스크린샷 이미지(파일경로 + 파일명)
             * [1] 0:기존파일;1:신규파일
             * [2] 신규경로 + 신규파일명 (기존파일용)
             */

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
                    fileExtension = System.IO.Path.GetExtension(Server.MapPath(fileScreenShot.Split('^')[0])).ToLower(); //확장자명
                    screenStr[1] = "0"; //구분자
                    screenStr[2] = fileScreenShot.Split('^')[1]; //신규경로 + 신규파일명
                    fileScreenShot = fileScreenShot.Split('^')[0]; //파일경로 + 파일명
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


                //이미지 파일 존재여부
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                        break;
                    }
                }

                //이미지 경우, 섬네일 이름 저장
                fileScreenShot = (fileOK) ? fileScreenShot : "";
            }

            screenStr[0] = fileScreenShot;

            return screenStr;
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
        /// 용도 : 스크린샷 이미지 확인 여부
        /// </summary>
        /// <returns></returns>
        private bool HasScreenShot()
        {
            bool fileOK = false;

            //스크린샷 이미지 업로드
            if (tbScreenShot.Text.Trim().Length > 0)
            {

                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/{2}/{3}";
                string screenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, tbScreenShot.Text.Trim());

                string fileExtension = System.IO.Path.GetExtension(Server.MapPath(screenSpoolPath)).ToLower();
                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                        break;
                    }
                }
            }

            return fileOK;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 임시저장 및 수정
        /// </summary>
        private void RegisterTemp()
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            int listID = AdditionTemp(); //추가

            if (listID > 0)
            {
                //스크린샷 이미지 업로드
                if (HasScreenShot())
                {
                    string reqTime = FileHelper.GetYearMonth();
                    string screenShotPath = baseFileDirectory + "{0}/Users/{1}/Board/{2}/{3}";
                    screenShotPath = string.Format(screenShotPath, CurrentUserInfo.CompanyID.ToString(),
                        CurrentUserInfo.UserID.ToString(), reqTime, listID.ToString());
                    screenShotPath = screenShotPath + "/" + tbScreenShot.Text.Trim();


                    string filePath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
                    string screenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, tbScreenShot.Text.Trim());

                    if (System.IO.File.Exists(Server.MapPath(screenSpoolPath)))
                    {
                        string imgSize = UtilHelper.GetThumnailSize(Server.MapPath(screenSpoolPath), 160, 160);

                        System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(screenSpoolPath));
                        System.Drawing.Image thumb = image.GetThumbnailImage(imgSize.Split(',')[0].ToNumber<int>(), imgSize.Split(',')[1].ToNumber<int>(),
                            delegate() { return false; }, (IntPtr)0);

                        thumb.Save(Server.MapPath(screenShotPath));
                    }
                }


                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("categoryID", CategoryID.ToString());
                paraDic.Add("listID", listID.ToString());
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("searchTerm", SearchTerm);
                paraDic.Add("searchTermValue", SearchTermValue);
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("order", Order);
                paraDic.Add("types", Types);

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskplusEIP/Board/Temp/View.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        #endregion

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

                string args = eventArgs;

                switch (args)
                {
                    case "Temp":  //임시저장
                        RegisterTemp();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
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
        #endregion

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
        #endregion
    }
}