using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Threading;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Common
{
    public partial class TransferAppoval : Board.Web.Base.BasePageBoard
    {
        #region 변수설정

        /// <summary>
        /// Gets or sets the name of the cat.
        /// </summary>
        /// <value>
        /// The name of the cat.
        /// </value>
        private string CatName { get; set; }

        /// <summary>
        /// Gets or sets the contents.
        /// </summary>
        /// <value>
        /// The contents.
        /// </value>
        private string Contents { get; set; }

        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        private int DocumentID { get; set; }

        /// <summary>
        /// Gets or sets the option alarm.
        /// </summary>
        /// <value>
        /// The option alarm.
        /// </value>
        private string OptionAlarm { get; set; }

        /// <summary>
        /// Gets or sets the option use dept.
        /// </summary>
        /// <value>
        /// The option use dept.
        /// </value>
        private string OptionUseDept { get; set; }

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Common.TransferAppoval).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시물저장
        /// 작성자     : shimpark
        /// 최초작성일 : 2011-04-25
        /// </summary>
        /// <returns></returns>
        private int TransDocToBoard()
        {
            BE.ListInfo info = new BE.ListInfo();
            BLL.Lists bll = new BLL.Lists();

            Zeospace.DeskPlus.Approval.DAL.Document doc = new Approval.DAL.Document();
            List<Zeospace.DeskPlus.Approval.BE.DocumentInfo> docList = doc.GetDocList(CurrentUserInfo.CompanyID, DocumentID);
            if (docList.Count == 0) return -1;

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = CategoryID;
            info.ListID = 0;
            info.GroupID = 0;
            info.ListOrder = 0;
            info.Depth = 0;

            info.Title = docList[0].Subject;
            info.Content = templateHeader + templateFooter;
            info.MobileContent = docList[0].ContentTEXT;

            info.OwnerDeptID = CurrentUserInfo.DeptID;
            info.OptionModify = "0";
            info.OptionTop = "0"; //공지 등록 여부
            info.OptionTopDay = 0;
            info.HeaderID = 0; //말머리
            info.WriteUserID = CurrentUserInfo.UserID;
            info.WriteDate = FileHelper.GetYearMonth();

            info.OptionAnonymity = "0";
            info.WriteUserName = CurrentUserInfo.Name;

            info.PassWord = string.Empty;

            //1.알람;
            if (OptionAlarm == "1" || OptionAlarm == "4")
            {
                string alarmContent = bll.AlarmContentTemplate(CatName, info.Title, info.WriteUserName,
                    System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString(),
                    info.DeptName, PreLang);

                //1-1 알람 저장
                info.AlarmContent = alarmContent;
            } //2.sms
            else if (OptionAlarm == "2")
            {
                string smsContent = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "SMSContent", "");
                smsContent = string.Format(smsContent, info.WriteUserName, info.Title);
                info.SMSContent = smsContent;
                info.SMSLoginID = CurrentUserInfo.LogInID;
                info.SMSMobilePhone = CurrentUserInfo.MobilePhone;
            }

            info.HiddenSpoolPath = "";
            info.ScreenShot = "";

            //기타옵션 저장하기
            Board.BE.ListEtcInfo etcInfo = new BE.ListEtcInfo();
            etcInfo.CompanyID = CurrentUserInfo.CompanyID;
            etcInfo.CategoryID = CategoryID;
            etcInfo.ListID = 0;
            etcInfo.Etc1 = "";

            string etc = ",,";

            etcInfo.Etc2 = etc;
            etcInfo.Etc3 = "";
            etcInfo.Etc4 = "";
            etcInfo.Etc5 = "";
            etcInfo.Etc6 = "";
            etcInfo.Etc7 = "";
            etcInfo.Etc8 = 0;
            etcInfo.Etc9 = 0;

            List<BE.LinkURLInfo> arrLinkURLInfo = new List<BE.LinkURLInfo>();

            int boaListID = bll.AddLinkUrl(info, etcInfo, arrLinkURLInfo);            

            #region 전자결재 첨부파일을 게시판 첨부파일로 이동

            DAL.AttachFiles attBoa = new DAL.AttachFiles();
            Zeospace.DeskPlus.Approval.DAL.AttachFile attApp = new Approval.DAL.AttachFile();
            List<Zeospace.DeskPlus.Approval.BE.AttachFileInfo> attAppList = attApp.GetAttachFileLists(CurrentUserInfo.CompanyID, DocumentID);

            //게시판 첨부파일 폴더 생성하기
            string boaFolder = string.Format("/DeskPlusEIP/FileData/{0}/Board/{1}/{2}/", CurrentUserInfo.CompanyID, FileHelper.GetYearMonth(), boaListID);
            if (!Directory.Exists(Server.MapPath(boaFolder))) Directory.CreateDirectory(Server.MapPath(boaFolder));

            foreach (var attAppInfo in attAppList)
            {
                BE.AttachFileInfo attBoaInfo = new BE.AttachFileInfo();
                attBoaInfo.CompanyID = attAppInfo.CompanyID;
                attBoaInfo.CategoryID = CategoryID;
                attBoaInfo.ListID = boaListID;
                attBoaInfo.FilePath = boaFolder;
                attBoaInfo.FileName = attAppInfo.FileName;
                attBoaInfo.FileType = attAppInfo.FileType;
                attBoaInfo.FileLength = attAppInfo.FileLength;
                attBoaInfo.FileGUID = attAppInfo.FileGUID;

                attBoa.Add(attBoaInfo);

                System.IO.File.Copy(
                    Server.MapPath(attAppInfo.FilePath) + attAppInfo.FileGUID,
                    Server.MapPath(boaFolder) + attAppInfo.FileGUID, true);
            }

            #endregion 전자결재 첨부파일을 게시판 첨부파일로 이동

            #region 결재 본문 jpg 파일 첨부파일로 추가하기

            //네트웍 타 서버에 첨부파일 복사가 안되어 로컬에 저장하고, move 처리함.
            string spoolFolder = "/DeskPlusEIP/FileDataSpool/";
            if (System.IO.Directory.Exists(Server.MapPath(spoolFolder)) == false) System.IO.Directory.CreateDirectory(Server.MapPath(spoolFolder));

            string tmpFileName = Guid.NewGuid().ToString();
            HtmlToImage(Contents, Server.MapPath(spoolFolder), tmpFileName);

            string sppolFile = spoolFolder + tmpFileName + ".jpg";
            if (System.IO.File.Exists(Server.MapPath(sppolFile)))
                System.IO.File.Move(Server.MapPath(sppolFile), Server.MapPath(boaFolder) + tmpFileName + ".jpg");

            BE.AttachFileInfo attBoaImgInfo = new BE.AttachFileInfo();
            attBoaImgInfo.CompanyID = CurrentUserInfo.CompanyID;
            attBoaImgInfo.CategoryID = CategoryID;
            attBoaImgInfo.ListID = boaListID;
            attBoaImgInfo.FilePath = boaFolder;
            attBoaImgInfo.FileName = docList[0].DocNumber + ".jpg";
            attBoaImgInfo.FileType = ".jpg";
            attBoaImgInfo.FileLength = 63867;
            attBoaImgInfo.FileGUID = tmpFileName + ".jpg";

            attBoa.Add(attBoaImgInfo);

            #region DOC_STREAM
            var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "DocStreamYN", "N");

            if (docStreamYN == "Y")
            {
                try
                {
                    DAL.AttachFiles dalFile = new DAL.AttachFiles();
                    DocStream ds = new DocStream();
                    var newDSList = dalFile.GetAttachFileLists(info.CompanyID, info.CategoryID, boaListID);
                    for (int i = 0; i < newDSList.Count; i++)
                    {
                        ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachFileID, "BOA_ATTACH");
                        ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachFileID, "BOA_ATTACH");
                    }
                }
                catch (Exception ex)
                {
                    DAL.AttachFiles dalFile = new DAL.AttachFiles();
                    DocStream ds = new DocStream();
                    var DSList = dalFile.GetAttachFileLists(info.CompanyID, info.CategoryID, boaListID);
                    for (int i = 0; i < DSList.Count; i++)
                    {
                        ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachFileID, "BOA_ATTACH");
                    }
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("BOA_Attach_Add_DS : " + ex.Message);
                    boaListID = -1;
                }

            }
            #endregion

            #endregion 결재 본문 jpg 파일 첨부파일로 추가하기

            return boaListID;
        }

        /// <summary>
        /// 게시판 카테고리 정보
        /// </summary>
        private void GetCategory()
        {
            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(CurrentUserInfo.CompanyID, CategoryID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CatName = this.GetLangName(
                        dr["NameBase"].ToString(),
                        dr["NameKo"].ToString(),
                        dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(),
                        dr["NameZhTw"].ToString(), PreLang);

                    OptionUseDept = dr["OptionUseDept"].ToString(); //부서별 게시판 여부
                    OptionAlarm = dr["optionAlarm"].ToString();
                }
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            #region 결재를 게시판으로 이관처리

            if (CategoryID > 0 && DocumentID > 0)
            {
                GetCategory(); //게시판 폴더 정보 가져오기

                #region 쓰기 권한 체크

                BLL.Category bizCat = new BLL.Category();
                NameValueCollection nvcAuth = new NameValueCollection();

                //쓰기 권한 존재하는지 체크
                nvcAuth = bizCat.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                    CurrentUserInfo.DeptID, CategoryID, OptionUseDept, false, BE.BoardAuth.Write, true, false);

                if (nvcAuth.AllKeys.Length == 0) return;

                #endregion 쓰기 권한 체크
                int rtn = 0;

                rtn = TransDocToBoard();

                if (rtn > 0)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("Processing"), "");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("ProcessingError"), "");
                }
            }

            #endregion 결재를 게시판으로 이관처리
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            string module = GetParm("module", "");
            string listID = GetParm("listID", "");
            Contents = GetParm("contents", "");

            if (module == "Approval")
            {
                //catID=1&docID=1
                if (listID.Length > 0)
                {
                    string tmpCategoryID = listID.Split('&')[0];
                    string tmpDocumentID = listID.Split('&')[1];
                    if (tmpCategoryID.Split('=')[0] == "catID") CategoryID = tmpCategoryID.Split('=')[1].ToNumber<int>();
                    if (tmpDocumentID.Split('=')[0] == "docID") DocumentID = tmpDocumentID.Split('=')[1].ToNumber<int>();
                }
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
                //cbtnCancel.Text = resMng.GetString("Cancel");
            }
        }

        #endregion Method (함수)

        #region Event Handler (이벤트 핸들러)

        #endregion Event Handler (이벤트 핸들러)

        #region Property (프로퍼티)

        #endregion Property (프로퍼티)

        /// <summary>
        /// HTMLs to image.
        /// 비동기 방식이라, 파일이 네트웍 다른 서버에 파일 복사가 되지 않음.~
        /// </summary>
        /// <param name="_content">The _content.</param>
        /// <param name="_filePath">The _file path.</param>
        /// <param name="_fileName">Name of the _file.</param>
        protected void HtmlToImage(string _content, string _filePath, string _fileName)
        {
            // 이벤트 생성
            AutoResetEvent resultEvent = new AutoResetEvent(false);

            //파일 경로 : d:\deskpluseip\filedata\
            //파일 명 : ImageName(확장자 제외 jpg 로 생성함)
            Zeospace.DeskPlus.Component.Converter.HtmlToImage browser = new Component.Converter.HtmlToImage(_content, _filePath, _fileName, resultEvent);

            // 이미지 생성 쓰레드에서 완료 이벤트가 올때까지 대기함
            EventWaitHandle.WaitAll(new AutoResetEvent[] { resultEvent });

            // 이미지 생성시까지 대기
            while (browser == null || browser.HtmlResult == null) Thread.Sleep(5);

            string result = browser.HtmlResult; // 완료 이벤트

            browser.Dispose(); // 이미지 생성 쓰레드 Kill
        }
    }
}