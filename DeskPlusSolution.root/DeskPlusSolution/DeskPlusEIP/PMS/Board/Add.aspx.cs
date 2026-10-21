using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Board
{
    public partial class Add : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)

        public string content = "";

        public int BoardID;
        public int GroupID;
        public int ListOrder;
        public int Depth;

        #endregion Declaration (컨트롤 선언)

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드

            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Board.Add).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스 로드

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();

                SwfUploadGUID = System.Guid.NewGuid().ToString();
            }
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadParam()
        {
            SearchField = Server.UrlDecode(GetDecryptParam("searchField", ""));
            SearchValue = Server.UrlDecode(GetDecryptParam("searchValue", ""));
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");

            BoardID = GetDecryptParam("boardID", "0").ToNumber<int>();
            GroupID = GetDecryptParam("groupID", "0").ToNumber<int>();
            ListOrder = GetDecryptParam("listOrder", "0").ToNumber<int>();
            Depth = GetDecryptParam("depth", "0").ToNumber<int>();
        }

        private void LoadData()
        {
            LoadCUMode();
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                ltlMenuName.Text = resMng.GetString("Writing");
                ltTitle.Text = resMng.GetString("Board_List_Title");

                cbtnConfirm.Text = resMng.GetString("Save");
                cbtnCancel.Text = resMng.GetString("Cancel");
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 추가시 에디터 설정하기
        /// 작성자   : 심재운
        /// </summary>
        private void LoadCUMode()
        {
            #region 템플릿 바인딩

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";

            tbHiddenMime_contents.Text = templateHeader + templateFooter;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                /*
                string strScript = String.Empty;
                if (Request.Path.IndexOf("Add.aspx") > 0)
                {
                    strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.HtmlValue(form1.tbHiddenMime_contents.value);</script>";
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
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/PMS/Board/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                #endregion 에디터 본문 이미지 첨부 경로
            }

            #endregion 템플릿 바인딩
        }

        /// <summary>
        /// 태그프리 에디터 설정
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

        #endregion Method (함수)

        #region Property (프로퍼티)

        /// <summary>
        /// 업로드할 대상 경로
        /// </summary>
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

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        #endregion Property (프로퍼티)

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 추가 담기
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private BE.BoardInfo WriteMode()
        {
            string title = tbTitle.Text.ToString().Trim();//제목;
            string content = GetContents();
            string mobileContent = tbHiddenText_contents.Text;

            //1. 게시판 리스트
            BE.BoardInfo listinfo = new Zeospace.DeskPlus.PMS.BE.BoardInfo();

            listinfo.CompanyID = CurrentUserInfo.CompanyID;
            listinfo.ProjectID = ProjectID;
            listinfo.MenuID = MenuID;
            listinfo.BoardID = 0;
            listinfo.GroupID = 0;
            listinfo.ListOrder = 0;
            listinfo.Depth = 0;
            listinfo.Title = title;
            listinfo.Contents = content;
            listinfo.MobileContent = mobileContent;
            listinfo.ReadCount = 0;
            listinfo.WriteUserID = CurrentUserInfo.UserID;

            #region 첨부파일

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            listinfo.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            #endregion 첨부파일

            //조회수 기능 사용할 경우
            //2. 게시판 조회 내역
            BE.BoardViewerInfo listviewinfo = new Zeospace.DeskPlus.PMS.BE.BoardViewerInfo();

            listviewinfo.CompanyID = CurrentUserInfo.CompanyID;
            listviewinfo.ProjectID = ProjectID;
            listviewinfo.MenuID = MenuID;
            listviewinfo.BoardID = 0;
            listviewinfo.WriteUserID = CurrentUserInfo.UserID;

            //listinfo 배열에 담기
            ArrayList arrListViewInfo = new ArrayList();
            arrListViewInfo.Add(listviewinfo);
            listinfo.ListViewerList = arrListViewInfo;

            return listinfo;
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 추가글
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void lbtnWrite_Click(object sender, System.EventArgs e)
        {
            LoadParam();

            BLL.Board bll = new Zeospace.DeskPlus.PMS.BLL.Board();
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            int rtnListID = bll.Add(WriteMode());

            if (rtnListID > 0)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
            }

            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchField", Server.UrlEncode(SearchField));
            paraDic.Add("searchValue", Server.UrlEncode(SearchValue));

            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("rqProjectID", RqProjectID.ToString());
            paraDic.Add("boardID", rtnListID.ToString());
            paraDic.Add("recordPerPage", RecordPerPage.ToString());

            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("GoList"));
            paraDic.Add("goURL", "/DeskplusEIP/PMS/Board/View.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        #endregion Event Handler (이벤트 핸들러)
    }
}