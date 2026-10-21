using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.Web.Common
{
    public partial class AddComplete : Base.BasePageTask
    {
        #region 변수설정

        public string BaseDocIDs;  //근거문서들 추가 순서대로의 documentID ex) |234;|454;|875; -> |ListBaseID;
        private string eventArgs = String.Empty;
        public string Contents = string.Empty; //내용  
        public string IsPage = "";
        public string Year = "";
        public int RecentPageNo = 0;
        public int ProcessPageNo = 0;
        public int DelayPageNo = 0;
        public int ComPageNo = 0;
        public int CCPageNo = 0;        
        public int WriteCommentPageNo = 0;
        public int ImpoPageNo = 0;

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

            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Common.AddComplete).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();

                SwfUploadGUID = System.Guid.NewGuid().ToString();

                #region 에디터 본문 이미지 첨부 경로
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = System.DateTime.Now.ToString("yyyyMM");
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Task/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로
                #endregion
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 :
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            LoadContent();
        }

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
                cbtnCancel.Text = resMng.GetString("BtnCancel");
                cbtnAdd.Text = resMng.GetString("Btn_Save");
                //cbtnSwfFileAdd.Text = resMng.GetString("AddFile");

                //cbtnAddApp.Text = resMng.GetString("AddDoc");
                cbtnAddTask.Text = resMng.GetString("AddDoc");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            BaseDocIDs = GetDecryptParam("baseDocIDs", "");
            IsPage = GetDecryptParam("isPage", "");

            Year = GetDecryptParam("year", "");
            RecentPageNo = GetDecryptParam("recentPageNo", "0").ToNumber<int>();
            ProcessPageNo = GetDecryptParam("processPageNo", "0").ToNumber<int>();
            DelayPageNo = GetDecryptParam("delayPageNo", "0").ToNumber<int>();
            ComPageNo = GetDecryptParam("comPageNo", "0").ToNumber<int>();
            CCPageNo = GetDecryptParam("ccPageNo", "0").ToNumber<int>();
            WriteCommentPageNo = GetDecryptParam("writeCommentPageNo", "0").ToNumber<int>();
            ImpoPageNo = GetDecryptParam("impoPageNo", "0").ToNumber<int>();
        }              

        /// <summary>
        /// 
        /// </summary>
        private void LoadContent()
        {
            DAL.List dalList = new DAL.List();

            //1. 내용 가져오기
            var dtrList = dalList.GetList(CurrentUserInfo.CompanyID, ListID.ToString());

            if (dtrList != null)
            {
                if (dtrList.HasRows)
                {
                    while (dtrList.Read())
                    {
                        #region 템플릿 바인딩

                        string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
                        string templateFooter = "</body></html>";
                        tbHiddenMime_contents.Text = templateHeader + templateFooter;

                        ctbStartDate.Text = DateHelper.GetCultureDateChar8(dtrList["StartDate"].ToString(), PreLangReal);
                        ctbEndDate.Text = DateHelper.GetCultureDateChar8(dtrList["EndDate"].ToString(), PreLangReal);

                        if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                        {
                            /*
                            string strScript = String.Empty;
                            if (Request.Path.IndexOf("AddComplete.aspx") > 0)
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

                        #endregion 템플릿 바인딩
                    }
                }
            }
        }
     
        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

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
        /// 접수완료하기
        /// </summary>
        private bool Addition()
        {
            string content = GetContents();

            BE.ListInfo be = new BE.ListInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ListID = ListID;
            be.GroupID = GroupID;
            be.Title = "";
            be.Contents = content;
            be.ContentText = tbHiddenText_contents.Text.Trim();
            be.MsgContents = GetResource("MSGComplete");
            be.Status = "4";
            be.IsMsg = "0";
            be.IsType = IsType;
            be.WriteUserID = CurrentUserInfo.UserID;
            be.ToUserList = "";
            be.CCUserList = "";            
            be.SenderName = CurrentUserInfo.Name;
            be.PreLang = PreLang;

            // 첨부파일
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            be.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            // 전자결재 근거문서 |고유id^문서번호;|고유id^문서번호;|고유id^문서번호;
            string[] arrAppList = tbBaseApps.Text.Trim(';').Length > 0 ? tbBaseApps.Text.Trim(';').Split(';') : null;

            // 업무협업 근거문서 |고유id;|고유id;|고유id;
            string[] arrTaskList = tbBaseTasks.Text.Trim(';').Length > 0 ? tbBaseTasks.Text.Trim(';').Split(';') : null;

            #region 옵션
            BE.ListOptionInfo loInfo = new BE.ListOptionInfo();
            loInfo.CompanyID = be.CompanyID;
            loInfo.ListID = ListID;
            loInfo.StartDate = DateHelper.GetCultureDate(Request[this.ctbStartDate.ClientID].ToString(), "yyyyMMdd", PreLangReal);
            loInfo.EndDate = DateHelper.GetCultureDate(Request[this.ctbEndDate.ClientID].ToString(), "yyyyMMdd", PreLangReal);
            loInfo.CompleteDate = DateHelper.GetCultureDate(System.DateTime.Now.ToShortDateString(), "yyyyMMdd", PreLangReal);
            loInfo.Status = "4";   
            loInfo.ModifyUserID = CurrentUserInfo.UserID;

            #endregion        

            BLL.List dal = new BLL.List();
            return dal.Complete(be, loInfo, arrAppList, arrTaskList);           
        }      
       
        #endregion (함수)

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

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }
       

        #endregion (속성)

        #region (이벤트핸들러)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            LoadParam();

            if (Addition())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("currentPage", "1");
                paraDic.Add("searchStatus", SearchStatus);
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("searchDateValue", SearchDateValue);                
                paraDic.Add("listID", ListID.ToString());
                paraDic.Add("groupID", GroupID.ToString());
                paraDic.Add("itemID", ItemID.ToString());
                paraDic.Add("itemSubID", ItemSubID.ToString());
                paraDic.Add("isType", IsType.ToString());
                paraDic.Add("impoUserYN", ImpoUserYN.ToString());
                paraDic.Add("isGubun", IsGubun.ToString());
                paraDic.Add("isPage", IsPage.ToString());     

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));

                if (Request.Url.AbsolutePath.IndexOf("AddComplete.aspx") != -1)
                {
                    if (IsPage == "")
                    {
                        if (IsType == "RE")
                        {
                            paraDic.Add("goURL", "/DeskplusEIP/Task/Content/Request/View.aspx");
                        }
                        else if (IsType == "TO")
                        {
                            paraDic.Add("goURL", "/DeskplusEIP/Task/Content/Receive/View.aspx");
                        }
                    }
                    else
                    {
                        paraDic.Add("goURL", "/DeskplusEIP/Task/Home/View.aspx");
                    }
                }
                else
                {
                    if (IsPage == "")
                    {
                        if (IsType == "RE")
                        {
                            paraDic.Add("goURL", "/DeskplusEIP/Task/Content/Request/ViewN.aspx");
                        }
                        else if (IsType == "TO")
                        {
                            paraDic.Add("goURL", "/DeskplusEIP/Task/Content/Receive/ViewN.aspx");
                        }
                    }
                    else
                    {
                        paraDic.Add("goURL", "/DeskplusEIP/Task/Home/ViewN.aspx");
                    }
                }

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
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

        #endregion (이벤트핸들러)
    }
}