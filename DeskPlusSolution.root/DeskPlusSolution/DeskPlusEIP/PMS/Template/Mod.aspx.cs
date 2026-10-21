using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.PMS.Web.Template
{
    public partial class Mod : Zeospace.DeskPlus.PMS.Web.Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)

        public string Contents = string.Empty; //내용
        public string PageListUrl;
        public string PageNo;
        public int TemplateID;

        #endregion Declaration (컨트롤 선언)

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드

            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Template.Mod).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스 로드

            if (!IsPostBack)
            {
                LoadParam();

                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            GetUserInfo();
            LoadCUMode();
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 사용자 정보 가져오기
        /// 작성자   : 심재운
        /// </summary>
        private void GetUserInfo()
        {
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 추가, 수정, 답변시 에디터 설정하기
        /// 작성자   : 심재운
        /// </summary>
        private void LoadCUMode()
        {
            #region 템플릿 바인딩

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                //ByFreeman 20120106 X-Free 에디터 추가
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
                string reqTime = System.DateTime.Now.ToString("yyyyMM");
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/PMS/Template/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                #endregion 에디터 본문 이미지 첨부 경로
            }

            LoadEditMode();

            #endregion 템플릿 바인딩
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 수정모드
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private void LoadEditMode()
        {
            DAL.Template list = new Zeospace.DeskPlus.PMS.DAL.Template();
            var alList = list.GetTemplate(CurrentUserInfo.CompanyID, ProjectID, TemplateID);
            foreach (var listInfo in alList)
            {
                tbTitle.Text = listInfo.Subjects;

                string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
                string templateFooter = "</BODY></HTML>";

                tbHiddenHTML_contents.Value = listInfo.Contents;
                tbHiddenMime_contents.Text = templateHeader + listInfo.Contents + templateFooter;
                Contents += listInfo.Contents;
            }
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

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     :
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save"); //확인
                cbtnCancel.Text = resMng.GetString("Cancel"); //취소

                ltTitle.Text = resMng.GetString("Template_Register_ltlTitle");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            SearchField = Server.UrlDecode(GetDecryptParam("searchField", ""));
            SearchValue = Server.UrlDecode(GetDecryptParam("searchValue", ""));
            PageListUrl = GetDecryptParam("pageListUrl", "JList.aspx");
            PageNo = GetDecryptParam("pageNo", "0");
            TemplateID = GetDecryptParam("templateID", "0").ToNumber<int>();
        }

        #endregion Method (함수)

        #region Property (프로퍼티)

        /// <summary>
        /// TagFree의 Mime 을 사용하여 이미지를 업로드 할지 (Y : Mime 을 사용 , N : 업로드 컴포넌트를 사용) - 원인은 중국서버에서 Mime의 이상작동
        /// </summary>
        public string MimeUseChk
        {
            get
            {
                try
                {
                    return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "MimeUseChk", "N");
                }
                catch
                {
                    return "Y";
                }
            }
        }

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

        #endregion Property (프로퍼티)

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 추가, 답변 담기
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private BE.TemplateInfo EditMode()
        {
            string content = GetContents();
            string title = tbTitle.Text.ToString().Trim();//제목;

            //1. 게시판 리스트
            BE.TemplateInfo listinfo = new Zeospace.DeskPlus.PMS.BE.TemplateInfo();

            listinfo.CompanyID = CurrentUserInfo.CompanyID;
            listinfo.ProjectID = ProjectID;
            listinfo.TemplateID = TemplateID;
            listinfo.Subjects = title;
            listinfo.Contents = content;
            listinfo.WriteUserID = CurrentUserInfo.UserID;

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

            DAL.Template bll = new Zeospace.DeskPlus.PMS.DAL.Template();
            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            bll.Mod(EditMode());
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("MSG_SUCCESS"), "");

            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuID", MenuID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("pageListUrl", PageListUrl.ToString());
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("recordPerPage", RecordPerPage.ToString());
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("order", Order);

            string encparm = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            string url = PageListUrl + "?p=" + encparm;

            // 자바스크립트 주소 이동하기
            string scripts = @"<script type=""text/javascript"">location = '" + url + "';</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "goPage", scripts);
        }

        #endregion Event Handler (이벤트 핸들러)
    }
}