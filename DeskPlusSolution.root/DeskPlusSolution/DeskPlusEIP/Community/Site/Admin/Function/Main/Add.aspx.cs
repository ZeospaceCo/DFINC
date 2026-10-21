using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Site.Admin.Function.Main
{
    public partial class Add : Base.BasePageCommunitySysop
    {
        #region 변수설정

        public int CommunityID;
        public string Contents = string.Empty; //내용

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Admin.Function.Main.Add).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-23
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            LoadContent();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-23
        /// 작성목적 : 대문 내용 가져오기
        /// </summary>
        private void LoadContent()
        {
            string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</BODY></HTML>";
            string adminDesc = "";
            bool isMod = false;

            DAL.ComDetail dal = new DAL.ComDetail();
            DataTableReader dtr = dal.GetComDetail(CurrentUserInfo.CompanyID, CommunityID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        isMod = true;

                        tbHiddenHTML_contents.Value = dtr["AdminDescription"].ToString().ToEditorXSS();
                        adminDesc = dtr["AdminDescription"].ToString().ToEditorXSS();

                        tbHiddenMime_contents.Text = templateHeader + adminDesc + templateFooter;
                        Contents += dtr["AdminDescription"].ToString().ToEditorXSS();
                    }
                }
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                //ByFreeman 20120106 X-Free 에디터 추가
                /*
                string strScript = String.Empty;
                if (Request.Path.IndexOf("Add.aspx") > 0)
                {
                    if (isMod == false || adminDesc.Length == 0)
                    {
                        tbHiddenMime_contents.Text = templateHeader + templateFooter;
                        strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.htmlValue = form1.tbHiddenMime_contents.value;" + "<" + "/script>";
                    }
                    else
                    {
                        strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.bodyValue = form1.contents.value;" + "<" + "/script>";
                    }

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
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Community/" + CommunityID + "/Screen/Editor/" + reqTime + "/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

                #endregion 에디터 본문 이미지 첨부 경로
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-23
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save");       //"확인";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-23
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 저장
        /// </summary>
        /// <returns></returns>
        private bool Mod()
        {
            DAL.ComDetail dal = new DAL.ComDetail();

            return dal.Mod(CurrentUserInfo.CompanyID, CommunityID, GetContents());
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 저장
        /// </summary>
        private void Save()
        {
            if (Mod())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("communityID", CommunityID.ToString());
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoSiteMain"));

                if (NonActiveX == "T")
                    paraDic.Add("goURL", "/DeskPlusEIP/Community/Site/Admin/Function/Main/AddN.aspx");
                else paraDic.Add("goURL", "/DeskPlusEIP/Community/Site/Admin/Function/Main/Add.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        #endregion (함수)

        #region (프로퍼티)

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

        #endregion (프로퍼티)

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-24
        /// 작성목적 : 확인버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            Save();
        }

        #endregion (이벤트핸들러)
    }
}