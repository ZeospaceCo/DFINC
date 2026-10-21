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

namespace Zeospace.DeskPlus.Board.Web.Admin.Content
{
    public partial class Reply : Base.BasePageBoardAdmin
    {
        #region 변수설정

        public int ListID = 0;
        private string TemplateID = ""; //템플릿
        public string Contents = string.Empty; //내용
        private string eventArgs = String.Empty;

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Content.Reply).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

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
            LoadParam();

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CategoryID);

            GetCategory(); //카테고리 정보  

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

            SetFontColor();
            SetFontSize();
            SetCategoryHeader();

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";
            string templateContent = GetTemplate();
            tbHiddenMime_contents.Text = templateHeader + templateContent + templateFooter;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                /*
                string strScript = String.Empty;
                if (Request.Path.IndexOf("Reply.aspx") > 0)
                {
                    if (templateContent.Length == 0)
                        strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.htmlValue = form1.tbHiddenMime_contents.value;" + "<" + "/script>";
                    else
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
                string reqTime = FileHelper.GetYearMonth();
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Board/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                #endregion 에디터 본문 이미지 첨부 경로
            }
        }

        #endregion Page Process (페이지 처리)

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
                cbOptionAnonymity.Text = resMng.GetString("Anonymity");

                cbtnAlarmUser.Text = resMng.GetString("Add");
                cbtnWhole.Text = resMng.GetString("Whole");
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
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
        /// 작성목적   : 게시물저장
        /// 작성자     : daniel
        /// 최초작성일 : 2011-04-25
        /// </summary>
        /// <returns></returns>
        private int Addition()
        {
            try
            {
                BE.ListInfo info = new BE.ListInfo();
                BLL.Lists bll = new BLL.Lists();

                DAL.Lists dalList = new DAL.Lists();
                List<ListInfo> list = dalList.GetList(BoaComID, CategoryID, ListID);
                foreach (ListInfo listinfo in list)
                {
                    info.GroupID = listinfo.GroupID;
                    info.Depth = listinfo.Depth + 1;
                    info.ListOrder = listinfo.ListOrder + 1;
                }

                info.CompanyID = BoaComID;
                info.CategoryID = CategoryID;
                info.ListID = ListID;

                info.Title = ctbName.Text.Trim();
                info.Content = GetContents();
                info.MobileContent = tbHiddenText_contents.Text.Trim();

                info.OwnerDeptID = CurrentUserInfo.DeptID;
                info.OptionModify = (cbOptionModify.Checked) ? "1" : "0";
                info.OptionTop = (cbOptionTop.Checked) ? "1" : "0"; //공지 등록 여부
                info.OptionTopDay = (cbOptionTop.Checked) ? ddlDay.Value.ToNumber<int>() : 0;
                info.HeaderID = ddlCatHeader.Value.ToNumber<int>(); //말머리
                //info.DeptName = (cbOptionAnonymity.Checked) ? "" : CurrentUserInfo.DeptName;
                //info.TitleName = (cbOptionAnonymity.Checked) ? "" : CurrentUserInfo.TitleName;
                //info.DutyName = (cbOptionAnonymity.Checked) ? "" : CurrentUserInfo.DutyName;
                info.WriteUserID = CurrentUserInfo.UserID;
                info.OptionAnonymity = (cbOptionAnonymity.Checked) ? "1" : "0";
                info.WriteUserName = CurrentUserInfo.Name;

                if (cbOptionAnonymity.Checked)
                {
                    string anony = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AnonymityName", GetResource("Anonymity"));
                    info.WriteUserName = (PreLang == "ko") ? anony.Split(',')[0] : anony.Split(',')[1];
                }

                info.PassWord = string.Empty;

                //알람대상자
                string chkAlarmUser = (cbxAlarmUser.Checked) ? "1" : "0";
                string AlarmUser = hdAlarmUser.Value.Trim();

                info.AlarmUser = AlarmUser + ":" + chkAlarmUser; 

                //1.알람;
                if (AlarmUser.Length > 0 || OptionAlarm == "1" || OptionAlarm == "4")
                {
                    string alarmContent = bll.AlarmContentTemplate(lbCategory.Text, info.Title, info.WriteUserName,
                        System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString(),
                        info.DeptName, PreLang);

                    //1-1 알람 저장
                    info.AlarmContent = alarmContent;
                } //2.sms

                if (OptionAlarm == "2")
                {
                    string smsContent = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "SMSContent", "");
                    smsContent = string.Format(smsContent, info.WriteUserName, info.Title);
                    info.SMSContent = smsContent;
                    info.SMSLoginID = CurrentUserInfo.LogInID;
                    info.SMSMobilePhone = CurrentUserInfo.MobilePhone;
                }

                string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
                info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

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
                etcInfo.ListID = 0;
                etcInfo.Etc1 = "";

                string bold = (cbxBold.Checked) ? "1" : "0";
                string etc = ddlFontColor.Value + "," + ddlFontSize.Value + "," + bold;

                etcInfo.Etc2 = etc;
                etcInfo.Etc3 = "";
                etcInfo.Etc4 = "";
                etcInfo.Etc5 = "";
                etcInfo.Etc6 = "";
                etcInfo.Etc7 = "";
                etcInfo.Etc8 = 0;
                etcInfo.Etc9 = 0;

                return bll.AddLinkUrl(info, etcInfo, arrLinkURLInfo);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return 0;
            }
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
            info.OptionTop = (cbOptionTop.Checked) ? "1" : "0"; //공지 등록 여부
            info.ReadCount = 0;
            info.WriteUserID = CurrentUserInfo.UserID;
            info.WriteUserName = CurrentUserInfo.Name;

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            info.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            //screen shot
            info.ScreenShot = (HasScreenShot()) ? tbScreenShot.Text.Trim() : "";

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

            return bll.Add(info, etcInfo);
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
        /// 메서드명   :
        /// 작성목적   : 카테고리 정보
        /// 작성자     : daniel
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void GetCategory()
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

                    OptionAnonymity = dr["OptionAnonymity"].ToString();
                    OptionUseDept = dr["OptionUseDept"].ToString(); //부서별 게시판 여부
                    OptionAlarm = dr["optionAlarm"].ToString();
                    OptionListType = dr["optionListType"].ToString();
                    TemplateID = dr["templateID"].ToString();

                    OptionLinkURL = dr["OptionLinkURL"].ToString();
                }

                cbOptionAnonymity.Visible = (OptionAnonymity == "1") ? true : false;
            }
        }

        /// <summary>
        /// 템플릿 내용 가져오기
        /// </summary>
        /// <returns></returns>
        private string GetTemplate()
        {
            if (TemplateID.Length > 0)
            {
                DAL.Templates dal = new DAL.Templates();
                List<BE.TemplatesInfo> al = dal.GetTemplateLists(BoaComID, TemplateID.ToNumber<int>());

                foreach (BE.TemplatesInfo info in al) return info.Content;
            }

            return "";
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
        /// 저장
        /// </summary>
        private void Register()
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            int listID = Addition();

            if (listID > 0)
            {
                //스크린샷 이미지 업로드
                if (HasScreenShot())
                {
                    string reqTime = FileHelper.GetYearMonth();
                    string screenShotPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Board/" + reqTime + "/" + listID;
                    FileHelper.CreateSubFolder(screenShotPath);
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
                paraDic.Add("menuCatID", MenuCatID.ToString());
                paraDic.Add("catID", CatID.ToString());
                paraDic.Add("categoryID", CategoryID.ToString());
                paraDic.Add("listID", listID.ToString());
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("searchTerm", SearchTerm);
                paraDic.Add("searchTermValue", SearchTermValue);
                paraDic.Add("searchGroup", SearchGroup);
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("order", Order);
                paraDic.Add("types", Types);

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/Board/Admin/Content/View.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 임시저장
        /// </summary>
        private void RegisterTemp()
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            int listID = AdditionTemp();

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
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoToList"));
                paraDic.Add("goURL", "/DeskplusEIP/Board/Temp/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
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

                string args = eventArgs;

                switch (args)
                {
                    case "Temp":  //임시저장(문제 소지 있어서 비활성화)
                        RegisterTemp();
                        break;

                    case "Register":  //추가
                        Register();
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

        #endregion Event Handler (이벤트 핸들러)

        #region Property (프로퍼티)

        /// <summary>
        /// //게시판 리스트 형식
        /// </summary>
        public string OptionListType
        {
            get { return GetViewStateString("OptionListType", ""); }
            set { ViewState["OptionListType"] = value; }
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
        /// 익명게시판 여부
        /// </summary>
        /// <value>
        /// The option anonymity.
        /// </value>
        public string OptionAnonymity
        {
            get { return GetViewStateString("OptionAnonymity", ""); }
            set { ViewState["OptionAnonymity"] = value; }
        }

        /// <summary>
        /// 부서별 게시판
        /// </summary>
        public string OptionUseDept
        {
            get { return GetViewStateString("OptionUseDept", ""); }
            set { ViewState["OptionUseDept"] = value; }
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
        /// UrlLink 사용여부
        /// </summary>
        public string OptionLinkURL
        {
            get { return GetViewStateString("OptionLinkURL", ""); }
            set { ViewState["OptionLinkURL"] = value; }
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