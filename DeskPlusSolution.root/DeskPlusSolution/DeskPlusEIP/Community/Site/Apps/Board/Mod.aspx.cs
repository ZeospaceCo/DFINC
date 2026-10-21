using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Community.BE;

using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Site.Apps.Board
{
    public partial class Mod : Base.BasePageCommunity
    {
        #region 변수설정

        public int CommunityID = 0;
        public int MenuID = 0;
        public int BoardID = 0;
        public int CurrentPage = 0;

        private string eventArgs = String.Empty;
        public string Order = String.Empty; //sort
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종료값
        public string RecordPerPage = String.Empty; //리스트 개수

        public string Contents = string.Empty; //내용
        public string AttachLimitSize = string.Empty; //첨부파일 제한용량

        public string ListType = String.Empty;
        public int Grade = 0;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Board.Mod).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-01
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            AttachLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMUNITY, "AttachLimitSize", "1");//용량

            #region 권한 체크

            if (Grade != 2)
            {
                DAL.MenuAuth dal = new DAL.MenuAuth();
                DataTableReader dtr = dal.GetMenuAuth(CurrentUserInfo.CompanyID, CommunityID, MenuID);

                int ReadAuth = 9;
                int WriteAuth = 9;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            ReadAuth = dtr["ReadAuth"].ToString().ToNumber<int>();
                            WriteAuth = dtr["WriteAuth"].ToString().ToNumber<int>();
                        }
                    }
                }

                if (Grade <= ReadAuth)
                {
                    if (Grade > WriteAuth)
                    {
                        Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                        return;
                    }
                }
                else
                {
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
                }
            }

            #endregion 권한 체크

            LoadList();
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save");
                cbtnCancel.Text = resMng.GetString("Cancel");
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            BoardID = GetDecryptParam("boardID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            ListType = GetDecryptParam("listType", "");
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "10");
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 내용 가져오기
        /// </summary>
        private void LoadList()
        {
            DAL.Boa dal = new DAL.Boa();
            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, CommunityID, MenuID, BoardID);

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        tbTitle.Text = dtr["Title"].ToString().ToReverseXSS();
                        tbHiddenHTML_contents.Value = dtr["Content"].ToString().ToEditorXSS();

                        string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
                        string templateFooter = "</BODY></HTML>";

                        tbHiddenMime_contents.Text = templateHeader + dtr["Content"].ToString().ToEditorXSS() + templateFooter;

                        Contents += dtr["Content"].ToString().ToEditorXSS();

                        if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                        {
                            //ByFreeman 20120106 X-Free 에디터 추가
                            /*
                            string strScript = String.Empty;
                            if (Request.Path.IndexOf("Mod.aspx") > 0)
                            {
                                strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.bodyValue = form1.contents.value;" + "<" + "/script>";

                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnControlInit", strScript);
                                NonActiveX = "F";
                            }
                            else
                            {   //스크립트에서 처리한다
                                NonActiveX = "T";
                            }
                            */
                            NonActiveX = "T";

                            #region 에디터 본문 이미지 첨부 경로

                            System.Guid guid = System.Guid.NewGuid();
                            string reqTime = Convert.ToDateTime(dtr["WriteDate"].ToString()).ToString("yyyyMM");
                            UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Community/" + CommunityID + "/Board/Editor/" + reqTime + "/" + guid.ToString();
                            if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                            #endregion 에디터 본문 이미지 첨부 경로

                            ScreenShot = dtr["ScreenShot"].ToString().ToNull();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적 : 스크린샷 이미지
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

                //## 기존 첨부파일 경우 (기존경로+파일명^썸네일경로+신규파일명)
                if (fileScreenShot.IndexOf('^') != -1)
                {
                    string oriFileFullPath = "";//원본파일 full 경로
                    string thumFileFullPath = "";//섬네일 저장할 파일 full 경로

                    DAL.BoaAttach dal = new DAL.BoaAttach();
                    List<BE.BoaAttachInfo> alAttach = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, CommunityID, MenuID, BoardID,
                        fileScreenShot.Split('^')[0].ToNumber<int>());

                    foreach (BE.BoaAttachInfo info in alAttach)
                    {
                        oriFileFullPath = info.FilePath + info.FileGUID;
                        thumFileFullPath = info.FilePath + fileScreenShot.Split('^')[1];
                    }

                    fileExtension = System.IO.Path.GetExtension(Server.MapPath(oriFileFullPath)).ToLower(); //확장자명
                    screenStr[1] = "0"; //구분자(기존)
                    screenStr[2] = thumFileFullPath; //파일경로 + 섬네일 파일명(Thum_)
                    fileScreenShot = oriFileFullPath; //파일경로 + 원본 파일명
                }
                else
                {
                    //## 신규 첨부파일 경우 (경로 없고 , fileScreenShot : 파일명만 온다.)
                    filePath = filePath + "{0}/Users/{1}/Spool/{2}/{3}";
                    string screenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, fileScreenShot);
                    fileExtension = System.IO.Path.GetExtension(Server.MapPath(screenSpoolPath)).ToLower();

                    screenStr[1] = "1"; //구분자 (신규)
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
        /// 작성자 : shine
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 게시물 수정
        /// </summary>
        /// <returns></returns>
        private bool Modify()
        {
            BE.BoaInfo info = new BoaInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CommunityID = CommunityID;
            info.MenuID = MenuID;
            info.BoardID = BoardID;

            info.Title = tbTitle.Text;
            info.Content = GetContents();

            info.ModifyUserID = CurrentUserInfo.UserID;
            info.ModifyUserName = CurrentUserInfo.Name;
            info.WriteDate = FileHelper.GetYearMonth();

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

            //0 : 기존; 1: 신규
            string screenGubun = getScreen[1];

            //스크린샷을 전혀 사용하지 않을 경우
            if (string.IsNullOrEmpty(screenGubun))
            {
                screenGubun = "0";//신규처리
                info.ScreenShot = "";
            }

            BLL.Boa bll = new BLL.Boa();
            bll.Mod(info, screenGubun);

            //스크린샷 이미지 업로드
            if (info.ScreenShot.Length > 0)
            {
                if (getScreen[1] == "0")
                {
                    //기존파일 스냅샷 만들기
                    System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(getScreen[0]));
                    System.Drawing.Image thumb = image.GetThumbnailImage(150, 150,
                        delegate() { return false; }, (IntPtr)0);

                    thumb.Save(Server.MapPath(info.ScreenShot));
                }
            }

            #region DOC_STREAM
            var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "DocStreamYN", "N");

            if (docStreamYN == "Y")
            {
                DAL.BoaAttach dalFile = new DAL.BoaAttach();
                DocStream ds = new DocStream();
                try
                {
                    var newDSList = dalFile.GetAttachFileLists(info.CompanyID, info.CommunityID, info.MenuID, info.BoardID);
                    for (int i = 0; i < newDSList.Count; i++)
                    {
                        ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachFileID, "CMN_ATTACH");
                        ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachFileID, "CMN_ATTACH");
                    }
                }
                catch (Exception ex)
                {
                    var DSList = dalFile.GetAttachFileLists(info.CompanyID, info.CommunityID, info.MenuID, info.BoardID);
                    for (int i = 0; i < DSList.Count; i++)
                    {
                        ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachFileID, "CMN_ATTACH");
                    }
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CMN_Addition_DS : " + ex.Message);
                    return false;
                }

            }
            #endregion

            return true;
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 본문 내용 및 본문 이미지(업로드) MIME 하여 본문 내용가져오기
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

        #endregion (프로퍼티)

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-04
        /// 작성목적 : 수정
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

                    paraDic.Add("communityID", CommunityID.ToString());
                    paraDic.Add("menuID", MenuID.ToString());
                    paraDic.Add("boardID", BoardID.ToString());
                    paraDic.Add("currentPage", CurrentPage.ToString());
                    paraDic.Add("searchTerm", SearchTerm);
                    paraDic.Add("searchTermValue", SearchTermValue);
                    paraDic.Add("searchField", SearchField);
                    paraDic.Add("searchValue", SearchValue);
                    paraDic.Add("order", Order);
                    paraDic.Add("listType", ListType);
                    paraDic.Add("grade", Grade.ToString());
                    paraDic.Add("recordPerPage", RecordPerPage);

                    paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                    paraDic.Add("alertMsg", GetResource("GoView"));
                    paraDic.Add("goURL", "/DeskPlusEIP/Community/Site/Apps/Board/View.aspx");

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

        #endregion (이벤트핸들러)
    }
}