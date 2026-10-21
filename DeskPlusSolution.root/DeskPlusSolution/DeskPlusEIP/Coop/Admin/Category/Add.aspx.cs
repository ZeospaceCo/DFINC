using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.IO;

namespace Zeospace.DeskPlus.Coop.Web.Admin.Category
{
    public partial class Add : Base.BasePageCoopAdmin
    {
        #region Declaration (컨트롤 선언)
        private string eventArgs = String.Empty;        
        #endregion

        #region 변수설정
        
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Admin.Category.Add).Assembly;	//어셈블리 선언
            #endregion

            

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
                LoadData();

                #region 에디터 본문 이미지 첨부 경로
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = System.DateTime.Now.ToString("yyyyMM");
                UploadPath = "/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID + "/Coop/" + reqTime + "/editor/" + guid.ToString();
                #endregion
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnCancel.Text = resMng.GetString("Close");
                CbtnConfirm.Text = GetResource("BtnConfirm");          // "추가"
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            ItemSubID = GetDecryptParam("itemSubID", "0").ToNumber<int>();
            ItemID = GetDecryptParam("itemID", "0").ToNumber<int>();
        }

        private void LoadData()
        {

            DAL.ItemSub bizSub = new DAL.ItemSub();
            var ItemSubList = bizSub.GetItemSub(CurrentUserInfo.CompanyID, ItemID, ItemSubID, false);
            //템플릿이 존재하면.
            foreach (var item in ItemSubList)
            {
                if (item.TempCnt > 0)
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("itemID", ItemID.ToString());
                    paraDic.Add("itemSubID", ItemSubID.ToString());
                    paraDic.Add("alertType", "");
                    paraDic.Add("alertMsg", GetResource("MoveMod"));

                    if (Request.Url.AbsolutePath.IndexOf("Add.aspx") != -1)
                        paraDic.Add("goURL", "/DeskplusEIP/Coop/Admin/Category/Mod.aspx");
                    else
                        paraDic.Add("goURL", "/DeskplusEIP/Coop/Admin/Category/ModN.aspx");


                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    Utility.Redirect(encParams);
                }
            }



            string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</BODY></HTML>";            

            tbHiddenMime_contents.Text = templateHeader + templateFooter;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                //ByFreeman 20120106 X-Free 에디터 추가 
                //Mod.aspx 에서 호출했는지 ModN.aspx 에서 호출했는지 확인
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
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 심재운
        /// 추가
        /// </summary>
        private void Register()
        {
            BE.TemplateInfo info = new BE.TemplateInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ItemSubID = ItemSubID;
            info.Title = ctbName.Text;
            info.Contents = GetContents();
            info.WriteUserID = CurrentUserInfo.UserID;

            DAL.Template biz = new DAL.Template();
            biz.Add(info);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("itemID", ItemID.ToString());
            paraDic.Add("itemSubID", ItemSubID.ToString());
            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("MoveMod"));

            if (Request.Url.AbsolutePath.IndexOf("Add.aspx") != -1)
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Admin/Category/Mod.aspx");
            else
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Admin/Category/ModN.aspx");
                       

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
        

        #endregion

        #region (속성)
        /// <summary>
        /// ItemSubID
        /// </summary>
        /// <value>
        /// The item sub ID.
        /// </value>
        public int ItemSubID
        {
            get { return GetViewStateInt("ItemSubID", "0"); }
            set { ViewState["ItemSubID"] = value; }
        }

        public int ItemID
        {
            get { return GetViewStateInt("ItemID", "0"); }
            set { ViewState["ItemID"] = value; }
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

        #endregion

        #region (이벤트핸들러)

        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                string args = eventArgs;
                switch (args)
                {
                    case "ADD":  //추가
                        Register();
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

        #endregion
    }
}