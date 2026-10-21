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

namespace Zeospace.DeskPlus.Task.Web.Admin.Menu
{
    public partial class Template : Base.BasePageTaskAdmin
    {
        #region Declaration (컨트롤 선언)
        private string eventArgs = String.Empty;
        public string Contents = string.Empty; //내용
        #endregion

        #region 변수설정
        
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Admin.Menu.Template).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();               
                LoadData();

                #region 에디터 본문 이미지 첨부 경로
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = System.DateTime.Now.ToString("yyyyMM");
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Task/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로
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
                CbtnDel.Text = resMng.GetString("Del");
            }
        }

        private void LoadData()
        {
            DAL.ItemSub bizSub = new DAL.ItemSub();
            var ItemSubList = bizSub.GetItemSubID(CurrentUserInfo.CompanyID, ItemID ,ItemSubID, false);

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";

           

            foreach (var item in ItemSubList)
            {
                if (item.TempCnt == 0) // 템플릿 저장 카운트가 없으면
                {
                   CbtnDel.Visible = false;
                   string strScript = String.Empty;
                   tbHiddenMime_contents.Text = templateHeader + templateFooter;
                   /*
                   if (Request.Path.IndexOf("Template.aspx") > 0)
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

                }
                else
                {

                    DAL.Template temp = new DAL.Template();

                    var tempinfo = temp.GetTemplateList(CurrentUserInfo.CompanyID, ItemSubID);

                    foreach (var info in tempinfo)
                    {

                        tbHiddenHTML_contents.Value = info.Contents.ToEditorXSS();
                        tbHiddenMime_contents.Text = info.Contents.ToEditorXSS();
                        Contents = info.Contents.ToEditorXSS();

                        /*
                        string strScript = String.Empty;
                        if (Request.Path.IndexOf("Template.aspx") > 0)
                        {
                            strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.bodyValue = form1.contents.value;" + "<" + "/script>";

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
                    }
                }
            }
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-04-14
        /// 작성목적 : 저장하기
        private void Register()
        {
            BE.TemplateInfo info = new BE.TemplateInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ItemSubID = ItemSubID;
            info.Contents = GetContents();            
            info.WriteUserID = CurrentUserInfo.UserID;

            DAL.Template biz = new DAL.Template();

            biz.AddTemplate(info);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("itemID", ItemID.ToString());
            paraDic.Add("itemSubID", ItemSubID.ToString());
            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("MoveMod"));

            if (Request.Url.AbsolutePath == "/DeskPlusEIP/Task/Admin/Menu/Template.aspx")
                paraDic.Add("goURL", "/DeskPlusEIP/Task/Admin/Menu/Template.aspx");
            else
                paraDic.Add("goURL", "/DeskplusEIP/Task/Admin/Menu/TemplateN.aspx");


            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);

        }

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자: 2014-04-27
        /// 작성목적: 본문 내용 및 본문 이미지(업로드) MIME 하여 본문 내용가져오기
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

        private void Del()
        {
            DAL.Template tmp = new DAL.Template();

            if (tmp.DelTemplate(CurrentUserInfo.CompanyID, ItemSubID))
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "MSG_OK", GetResource("MSG_SUCCESS"), "");
                CbtnDel.Visible = false;
    
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "MSG_Fail", GetResource("MSG_FAIL"), "");    
            }            
        }
        #endregion

        #region (속성)

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
               // LoadParam();

                string args = eventArgs;
                switch (args)
                {
                    case "ADD":  //추가
                        Register();
                        break;
                    case "DEL": //삭제
                        Del();
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