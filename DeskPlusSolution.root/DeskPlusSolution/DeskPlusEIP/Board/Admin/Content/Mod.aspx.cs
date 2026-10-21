using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;

using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Admin.Content
{
    public partial class Mod : Base.BasePageBoardAdmin
    {
        #region 변수설정

        public int ListID = 0;
        public string Contents = string.Empty; //내용
        public string AttachLimitSize = string.Empty; //첨부파일 제한용량

        public string OptionPopUp = string.Empty;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Content.Mod).Assembly;	//어셈블리 선언

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
            LoadParam();

            AttachLimitSize = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "AttachLimitSize", "1");//용량

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CategoryID);

            GetMainPopupSetting();  //메인팝업설정 정보


            #region 전사 권한 체크
            DAL.Authority auth = new DAL.Authority();
            DataTableReader dtr = auth.GetAuthority(BoaComID, CatID);

            if (dtr != null)
            {
                string type = string.Empty;
                int ownerID;

                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        type = dtr["type"].ToString();
                        ownerID = dtr["ownerID"].ToString().ToNumber<int>();

                        if (type == "G" && ownerID == 0)
                        {
                            allUser = "1";
                            break;
                        }
                    }
                }
            }
            #endregion 전사 권한 체크

            SetCategory();
            SetFontColor();
            SetFontSize();
            SetCategoryHeader();

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
                cbtnConfirm.Text = resMng.GetString("Submit");
                cbOptionAnonymity.Text = resMng.GetString("Anonymity");
                cbtnAlarmUser.Text = resMng.GetString("Add");
                cbtnWhole.Text = resMng.GetString("Whole");
            }
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 첨부파일 정보 가져오기
        /// </summary>
        private void LoadAttach()
        {
            DAL.AttachFiles biz = new DAL.AttachFiles();
            List<BE.AttachFileInfo> al = biz.GetAttachFileLists(BoaComID, CategoryID, ListID);

            string saveFileList = "";

            if (al.Count > 0)
            {
                foreach (BE.AttachFileInfo info in al)
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

            DAL.Lists dl = new DAL.Lists();
            List<ListInfo> list = dl.GetList(BoaComID, CategoryID, ListID);

            foreach (ListInfo info in list)
            {
                ListOrder = info.ListOrder;
                Depth = info.Depth;
                GroupID = info.GroupID;

                ctbName.Text = info.Title.ToReverseXSS();

                cbOptionModify.Checked = (info.OptionModify == "1") ? true : false;

                if (info.OptionTop == "1")
                {
                    cbOptionTop.Checked = true;
                    ddlDay.Value = info.OptionTopDay.ToString();
                }

                tbHiddenHTML_contents.Value = info.Content.ToEditorXSS();

                if (info.Depth > 0)
                {
                    cbOptionTop.Disabled = false;
                }

                string AnonymityName = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", GetResource("Anonymity"));
                AnonymityName = (PreLang.Equals("ko")) ? AnonymityName.Split(',')[0] : AnonymityName.Split(',')[1];

                cbOptionAnonymity.Checked = (AnonymityName == info.WriteUserName) ? true : false;

                string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
                string templateFooter = "</BODY></HTML>";

                tbHiddenMime_contents.Text = templateHeader + info.Content.ToEditorXSS() + templateFooter;

                Contents += info.Content.ToEditorXSS();

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

                    #region 에디터 본문 이미지 첨부 경로

                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    System.Guid guid = System.Guid.NewGuid();
                    string reqTime = FileHelper.GetYearMonth();
                    UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Board/" + reqTime + "/editor/" + guid.ToString();
                    if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                    #endregion 에디터 본문 이미지 첨부 경로
                }

                //접근여부
                IsAuth = (CurrentUserInfo.UserID == info.WriteUserID) ? true : false;
                //타인수정 가능시, 수정가능
                if (!IsAuth) IsAuth = (info.OptionModify == "1") ? true : false;

                WriteDate = info.WriteDate;

                ScreenShot = info.ScreenShot.ToNull();
                if (info.HeaderID > 0) ddlCatHeader.Value = info.HeaderID.ToString();               
            }

            //알람대상자 
            DAL.ListAlarm dalAlarm = new DAL.ListAlarm();
            var listAlarm = dalAlarm.GetList(BoaComID, CategoryID, ListID);
            string uID = "";
            string dID = "";
            string name = "";
            string isGroup = "";     

            foreach (var info in listAlarm)
            {
                if (info != null)
                {
                    uID += info.UserID + ",";
                    dID += info.DeptID + ",";

                    IsLowRank = info.IsLowRank;

                    if (info.IsGroup == "G")
                    {
                        string tmpName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                        name += tmpName + ",";
                    }
                    else
                    {
                        string tmpName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                        string tmpDept = this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                        string tmpTitle = this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang);
                        name += tmpDept + "/" + tmpTitle + "/" + tmpName + ",";
                    }

                    isGroup += info.IsGroup + ",";
                }

                AlarmUser = isGroup + "‡" + uID + "‡" + dID + "‡" + name.TrimEnd(',');
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 기타 정보 가져오기
        /// </summary>
        private void LoadEtc()
        {
            DAL.ListEtc biz = new DAL.ListEtc();
            List<BE.ListEtcInfo> al = biz.GetList(BoaComID, CategoryID, ListID);

            if (al.Count > 0)
            {
                foreach (BE.ListEtcInfo info in al)
                {
                    if (info.Etc2.ToNull().Length > 0)
                    {
                        string[] etcStr = info.Etc2.Split(',');
                        if (etcStr[0].ToString() != "") ddlFontColor.Value = etcStr[0].ToString().ToReverseXSS();
                        if (etcStr[1].ToString() != "") ddlFontSize.Value = etcStr[1].ToString();
                        cbxBold.Checked = (etcStr[2].ToString() == "1") ? true : false;
                    }

                    // 팝업설정
                    if (info.Etc3.ToNull().Length > 0)
                    {
                        cbPopUp.Checked = true;
                    }

                    if (info.Etc4.ToNull().Length > 0)
                    {
                        startDate.Value = DateHelper.GetCultureDateChar8(info.Etc4, PreLangReal);
                    }

                    if (info.Etc5.ToNull().Length > 0)
                    {
                        endDate.Value = DateHelper.GetCultureDateChar8(info.Etc5, PreLangReal);
                    }

                    // 메인팝업설정
                    if (info.Etc6.ToNull().Length > 0)
                    {
                        cbMainPopUp.Checked = true;
                    }

                    if (info.Etc7.Length > 0)
                    {
                        popupWidth.Value = info.Etc7.Split(',')[0];
                        popupHeight.Value = info.Etc7.Split(',')[1];
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
        /// 메서드명   :
        /// 작성목적   : 메인팝업 설정 정보가져오기
        /// 작성자     : daniel
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void GetMainPopupSetting()
        {
            Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
            DataSet ds = dal.GetSysCodeByPID(BoaComID, "MainPopupSetting");

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (dr["CodeID"].ToString())
                    {
                        //case "PopupWidth":
                        //    popupWidth.Value = dr["InterfaceCode"].ToString();
                        //    break;
                        //case "PopupHeight":
                        //    popupHeight.Value = dr["InterfaceCode"].ToString();
                        //    break;
                        case "OnlyAdmin":
                            OnlyAdmin = dr["InterfaceCode"].ToString();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 카테고리 정보
        /// 작성자     : daniel
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void SetCategory()
        {
            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(BoaComID, CategoryID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string name = this.GetLangName(
                        dr["PathNameBase"].ToString(),
                        dr["PathNameKo"].ToString(),
                        dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                        dr["PathNameFr"].ToString(), dr["PathNameZhCn"].ToString(),
                        dr["PathNameZhTw"].ToString(), PreLang);

                    switch (PreLang)
                    {
                        case "ko":
                            name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "en":
                            name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "ja":
                            name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "fr":
                            name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "zhcn":
                            name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;

                        case "zhtw":
                            name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                            break;
                    }

                    lbCategory.Text = name.Substring(1, name.Length - 1);
                    //익명게시판 활성화 여부
                    Anonymity = (dr["OptionAnonymity"].ToString() == "1") ? "1" : "0";
                    OptionUseDept = dr["OptionUseDept"].ToString(); //부서별 게시판 여부
                    OptionAlarm = dr["optionAlarm"].ToString();
                    OptionListType = dr["optionListType"].ToString();
                    cbPopUp.Visible = dr["OptionPopUp"].ToString() == "1" ? true : false;
                    lbPopUp.Visible = dr["OptionPopUp"].ToString() == "1" ? true : false;

                    if (OnlyAdmin == "Y")
                    {
                        //OnlyAdmin 이 Y일 경우 메인팝업은 administrator, 게시판관리자만 사용가능
                        if (CurrentUserInfo.IsInRole("admin") || CurrentUserInfo.IsInRole("board"))
                        {
                            cbMainPopUp.Visible = dr["OptionPopUp"].ToString() == "1" ? true : false;
                            lbMainPopUp.Visible = dr["OptionPopUp"].ToString() == "1" ? true : false;
                        }
                        else
                        {
                            cbMainPopUp.Visible = false;
                            lbMainPopUp.Visible = false;
                        }
                    }
                    else
                    {
                        cbMainPopUp.Visible = dr["OptionPopUp"].ToString() == "1" ? true : false;
                        lbMainPopUp.Visible = dr["OptionPopUp"].ToString() == "1" ? true : false;
                    }

                    OptionPopUp = dr["OptionPopUp"].ToString();
                    OptionLinkURL = dr["OptionLinkURL"].ToString();

                }

                cbOptionAnonymity.Visible = (Anonymity == "1") ? true : false;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 말머리
        /// </summary>
        private void SetCategoryHeader()
        {
            string name = "";
            ddlCatHeader.Items.Clear();
            ddlCatHeader.Items.Add(new ListItem(GetResource("SelectHeader"), "0"));
            DAL.Header dal = new DAL.Header();
            List<BE.HeaderInfo> al = dal.GetHeaderLists(BoaComID, CategoryID);

            IEnumerable<BE.HeaderInfo> ICatHeader =
                from catHeader in al
                where catHeader.UseActive == "1"
                select catHeader;

            foreach (BE.HeaderInfo be in ICatHeader)
            {
                #region 다국어 이름 설정

                name = this.GetLangName(
                    be.NameBase.ToReverseXSS(),
                    be.NameKo.ToReverseXSS(), be.NameEn.ToReverseXSS(), be.NameJa.ToReverseXSS(),
                    be.NameFr.ToReverseXSS(), be.NameZhcn.ToReverseXSS(), be.NameZhtw.ToReverseXSS(), PreLang);

                #endregion 다국어 이름 설정

                ListItem tmpItem = new ListItem(name, be.HeaderID.ToString());
                this.ddlCatHeader.Items.Add(tmpItem);
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

                //## 기존 첨부파일 경우 (기존경로+파일명^썸네일경로+신규파일명)
                if (fileScreenShot.IndexOf('^') != -1)
                {
                    string oriFileFullPath = "";//원본파일 full 경로
                    string thumFileFullPath = "";//섬네일 저장할 파일 full 경로

                    DAL.AttachFiles dal = new DAL.AttachFiles();
                    List<BE.AttachFileInfo> alAttach = dal.GetAttachFileLists(BoaComID, CategoryID, ListID,
                        fileScreenShot.Split('^')[0].ToNumber<int>());

                    foreach (BE.AttachFileInfo info in alAttach)
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
        /// 메서드명   :
        /// 작성목적   : 게시물 수정
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        /// <returns></returns>
        private bool Modify()
        {
            BE.ListInfo info = new BE.ListInfo();

            info.CompanyID = BoaComID;
            info.CategoryID = CategoryID;
            info.ListID = ListID;
            info.Title = ctbName.Text.Trim();
            info.Content = GetContents();
            info.MobileContent = tbHiddenText_contents.Text.Trim();
            info.OptionModify = (cbOptionModify.Checked) ? "1" : "0";
            info.OptionTop = (cbOptionTop.Checked) ? "1" : "0"; //공지 등록 여부
            info.OptionTopDay = (cbOptionTop.Checked) ? ddlDay.Value.ToNumber<int>() : 0;
            info.HeaderID = ddlCatHeader.Value.ToNumber<int>(); //말머리
            info.ModifyUserID = CurrentUserInfo.UserID;

            info.OptionAnonymity = (cbOptionAnonymity.Checked) ? "1" : "0";
            info.ModifyUserName = CurrentUserInfo.Name;
            info.WriteDate = FileHelper.GetYearMonth();

            //알람대상자
            string chkAlarmUser = (cbxAlarmUser.Checked) ? "1" : "0";
            string AlarmUserList = hdAlarmUser.Value.Trim();
            string UseAlarm = "0";

            if (OptionUseDept == "0" && allUser == "1")
            {
                UseAlarm = (cbxUseAlarm.Checked) ? "1" : "0";
            }
            else
            {
                if (OptionUseDept == "0" && (OptionAlarm == "1" || OptionAlarm == "4"))
                {
                    UseAlarm = (cbxUseAlarm2.Checked) ? "1" : "0";
                }
            }

            info.AlarmUser = AlarmUserList + ":" + chkAlarmUser;
            info.UseAlarm = UseAlarm;  

            if (UseAlarm == "1")
            {
                //1.알람
                if (AlarmUser.Length > 0 || OptionAlarm == "1" || OptionAlarm == "4")
                {
                    BLL.Lists bllalarmContent = new BLL.Lists();

                    string alarmContent = bllalarmContent.AlarmContentTemplate(lbCategory.Text, info.Title, info.WriteUserName,
                        System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString(),
                        info.DeptName, PreLang);

                    //1-1 알람 저장
                    info.AlarmContent = alarmContent;
                }

                /*
                //2.sms
                if (OptionAlarm == "2")
                {
                    string smsContent = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "SMSContent", "");
                    smsContent = string.Format(smsContent, info.WriteUserName, info.Title);
                    info.SMSContent = smsContent;
                    info.SMSLoginID = CurrentUserInfo.LogInID;
                    info.SMSMobilePhone = CurrentUserInfo.MobilePhone;
                }
                */
            }

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

            //링크URL 저장            
            string[] arrLinkURLName = hiddenLinkURLName.Text.Trim().TrimEnd('|').Split('|');
            string[] arrLinkURL = hiddenLinkURL.Text.Trim().TrimEnd('|').Split('|');

            List<BE.LinkURLInfo> arrLinkURLInfo = new List<BE.LinkURLInfo>();
            BE.LinkURLInfo linkURLInfo;

            if (hiddenLinkURLName.Text.Length > 0)
            {
                for (int i = 0; i < arrLinkURLName.Length; i++)
                {
                    linkURLInfo = new BE.LinkURLInfo();

                    linkURLInfo.URLName = arrLinkURLName[i];
                    linkURLInfo.URL = arrLinkURL[i];

                    arrLinkURLInfo.Add(linkURLInfo);
                }
            }

            //기타옵션 저장하기
            Board.BE.ListEtcInfo etcInfo = new ListEtcInfo();
            etcInfo.CompanyID = BoaComID;
            etcInfo.CategoryID = CategoryID;
            etcInfo.ListID = ListID;
            etcInfo.Etc1 = "";

            string bold = (cbxBold.Checked) ? "1" : "0";
            string etc = ddlFontColor.Value + "," + ddlFontSize.Value + "," + bold;

            etcInfo.Etc2 = etc;
            etcInfo.Etc3 = (cbPopUp.Checked) ? "1" : "";
            etcInfo.Etc4 = (cbPopUp.Checked || cbMainPopUp.Checked) ? DateHelper.GetCultureDate(startDate.Value.Trim(), "yyyyMMdd", PreLangReal) : "";
            etcInfo.Etc5 = (cbPopUp.Checked || cbMainPopUp.Checked) ? DateHelper.GetCultureDate(endDate.Value.Trim(), "yyyyMMdd", PreLangReal) : "";
            etcInfo.Etc6 = (cbMainPopUp.Checked) ? "1" : "";
            etcInfo.Etc7 = popupWidth.Value + "," + popupHeight.Value;
            etcInfo.Etc8 = 0;
            etcInfo.Etc9 = 0;

            //0 : 기존; 1: 신규
            string screenGubun = getScreen[1];

            //스크린샷을 전혀 사용하지 않을 경우
            if (string.IsNullOrEmpty(screenGubun))
            {
                screenGubun = "0";//신규처리
                info.ScreenShot = "";
            }

            BLL.Lists bll = new BLL.Lists();

            bll.ModLinkUrl(info, etcInfo, screenGubun, arrLinkURLInfo);

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

                    string screenShotPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Board/" + reqTime + "/" + ListID;
                    FileHelper.CreateSubFolder(screenShotPath);
                    screenShotPath = screenShotPath + "/" + info.ScreenShot;

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
                    //PDF파일생성
                    string yearMonth = FileHelper.GetYearMonth();
                    BLL.CreatePDF bll = new BLL.CreatePDF();
                    bll.ProcessPDF(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, ListID, CategoryID, yearMonth, "1");

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("menuCatID", MenuCatID.ToString());
                    paraDic.Add("catID", CatID.ToString());
                    paraDic.Add("categoryID", CategoryID.ToString());
                    paraDic.Add("listID", ListID.ToString());
                    paraDic.Add("currentPage", CurrentPage.ToString());
                    paraDic.Add("searchTerm", SearchTerm);
                    paraDic.Add("searchTermValue", SearchTermValue);
                    paraDic.Add("searchGroup", SearchGroup);
                    paraDic.Add("searchField", SearchField);
                    paraDic.Add("searchValue", SearchValue);
                    paraDic.Add("order", Order);
                    paraDic.Add("types", Types);

                    paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                    paraDic.Add("alertMsg", GetResource("GoList"));
                    paraDic.Add("goURL", "/DeskplusEIP/Board/Admin/Content/View.aspx");

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
        /// 게시판 리스트 형식
        /// </summary>
        public string OptionListType
        {
            get { return GetViewStateString("OptionListType", ""); }
            set { ViewState["OptionListType"] = value; }
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
        /// Gets or sets the list order.
        /// </summary>
        /// <value>
        /// The list order.
        /// </value>
        public int ListOrder
        {
            get { return GetViewStateInt("ListOrder", ""); }
            set { ViewState["ListOrder"] = value; }
        }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        public int Depth
        {
            get { return GetViewStateInt("Depth", ""); }
            set { ViewState["Depth"] = value; }
        }

        /// <summary>
        /// Gets or sets the group ID.
        /// </summary>
        /// <value>
        /// The group ID.
        /// </value>
        public int GroupID
        {
            get { return GetViewStateInt("GroupID", ""); }
            set { ViewState["GroupID"] = value; }
        }

        /// <summary>
        /// 익명 카테고리 확인 여부
        /// </summary>
        /// <value>
        /// The anonymity.
        /// </value>
        public string Anonymity
        {
            get { return GetViewStateString("Anonymity", "0"); }
            set { ViewState["Anonymity"] = value; }
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
        /// WriteDate
        /// </summary>
        private string WriteDate
        {
            get { return GetViewStateString("WriteDate", ""); }
            set { ViewState["WriteDate"] = value; }
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
        /// 페이지 이동
        /// </summary>
        public string GoPage
        {
            get
            {
                return "/DeskplusEIP/Board/Admin/Content/List.aspx";
            }
        }

        /// <summary>
        /// 메인팝업 관리자만 사용유무
        /// </summary>
        public string OnlyAdmin
        {
            get { return GetViewStateString("OnlyAdmin", ""); }
            set { ViewState["OnlyAdmin"] = value; }
        }

        /// <summary>
        /// 전사 권한 체크
        /// </summary>
        public string allUser
        {
            get { return GetViewStateString("allUser", "0"); }
            set { ViewState["allUser"] = value; }
        }

        /// <summary>
        /// 부서별게시판 사용여부
        /// </summary>
        /// <value>
        /// The option alarm.
        /// </value>
        public string OptionUseDept
        {
            get { return GetViewStateString("OptionUseDept", ""); }
            set { ViewState["OptionUseDept"] = value; }
        }

        /// <summary>
        /// 알람여부
        /// </summary>
        /// <value>
        /// The option alarm.
        /// </value>
        public string OptionAlarm
        {
            get { return GetViewStateString("OptionAlarm", ""); }
            set { ViewState["OptionAlarm"] = value; }
        }

        /// <summary>
        /// 알람대상자
        /// <value>The name of the linked dept.</value>
        public string AlarmUser
        {
            get { return GetViewStateString("AlarmUser", ""); }

            set { ViewState["AlarmUser"] = value; }
        }

        /// <summary>
        /// 하위분류포함여부
        /// <value>The name of the linked dept.</value>
        public string IsLowRank
        {
            get { return GetViewStateString("IsLowRank", ""); }

            set { ViewState["IsLowRank"] = value; }
        }

        /// <summary>
        /// UrlLink 사용여부
        /// </summary>
        public string OptionLinkURL
        {
            get { return GetViewStateString("OptionLinkURL", ""); }
            set { ViewState["OptionLinkURL"] = value; }
        }
        #endregion Property (프로퍼티)
    }
}