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
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.ETC.Web.PopUp
{
    public partial class Mod : Base.BasePageETC
    {
        #region Declaration (컨트롤 선언)
        private int ManagerID = 0;
        public int CurrentPage = 0;
        public int RecordPerPage = 0;
        public string Order = "";
        public string Contents = string.Empty;  // 내용
        #endregion

        #region 변수설정
        private string eventArgs = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.PopUp.Mod).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
       
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save");
                cbtnDel.Text = resMng.GetString("Delete");
                cbtnCancel.Text = resMng.GetString("Cancel");
            }
        }

        private void LoadData()
        {
            LoadParam();
            LoadOptoin();

            DAL.Manager biz = new DAL.Manager();
            DataSet ds = biz.GetPopUp(CurrentUserInfo.CompanyID, ManagerID);

            string tmpContent = string.Empty;
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ctbName.Text = dr["Subject"].ToString();
                    startDate.Value = dr["StartDate"].ToString();
                    endDate.Value = dr["EndDate"].ToString();
                    rblOption.SelectedValue = dr["LogInYN"].ToString();
                    ctbWidth.Text = dr["Width"].ToString();
                    ctbHeight.Text = dr["Height"].ToString();

                    tbHiddenHTML_contents.Value = dr["Content"].ToString().ToEditorXSS();

                    string templateHeader = Helpers.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
                    string templateFooter = "</BODY></HTML>";

                    tbHiddenMime_contents.Text = templateHeader + dr["Content"].ToString().ToEditorXSS() + templateFooter;

                    Contents += dr["Content"].ToString().ToEditorXSS();

                    if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                    {
                        NonActiveX = "T";

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
                    }

                    #region 에디터 본문 이미지 첨부 경로
                    System.Guid guid = System.Guid.NewGuid();
                    string reqTime = Convert.ToDateTime(dr["WriteDate"].ToString()).ToString("yyyyMM");
                    UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Etc/PopUp/" + reqTime + "/editor/" + guid.ToString();
                    if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로
                    #endregion
                }
            }


        }

        #endregion

        #region 함수

        private void LoadParam()
        {
            ManagerID = GetDecryptParam("managerID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "20").ToNumber<int>();
            Order = GetDecryptParam("order", "");
        }

        private void LoadOptoin()
        {
            rblOption.Items.Clear();
            rblOption.Items.Add(new ListItem(this.resMng.GetString("Before"), "0"));           //로그인 전
            rblOption.Items.Add(new ListItem(this.resMng.GetString("After"), "1"));            //로그인 후
        }

        /// <summary>
        /// 수정
        /// </summary>
        private void Modify()
        {
            BE.ManagerInfo info = new ManagerInfo();
            DAL.Manager biz = new DAL.Manager();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ManagerID = ManagerID;
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;
            info.StartDate = startDate.Value;
            info.EndDate = endDate.Value;
            info.Height = ctbHeight.Text.ToNumber<int>();
            info.Width = ctbWidth.Text.ToNumber<int>();
            info.LogInYN = rblOption.SelectedValue;
            info.Subject = ctbName.Text;
            info.Content = GetContents();

            biz.Mod(info);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("recordPerPage", RecordPerPage.ToString());
            paraDic.Add("order", Order);

            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("GoToList"));
            paraDic.Add("goURL", "/DeskPlusEIP/ETC/PopUp/List.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <remarks></remarks>
        private void Del()
        {
            DAL.Manager biz = new DAL.Manager();
            biz.Del(CurrentUserInfo.CompanyID, ManagerID);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("recordPerPage", RecordPerPage.ToString());
            paraDic.Add("order", Order);

            paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
            paraDic.Add("alertMsg", GetResource("GoToList"));
            paraDic.Add("goURL", "/DeskPlusEIP/ETC/PopUp/List.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        /// <summary>
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

        #endregion

        #region 이벤트 핸들러

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시물등록
        /// 작성자     : shine
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
                    case "Modify":
                        {
                            Modify();
                            break;
                        }
                    case "Del":
                        {
                            Del();
                            break;
                        }

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
    }
}