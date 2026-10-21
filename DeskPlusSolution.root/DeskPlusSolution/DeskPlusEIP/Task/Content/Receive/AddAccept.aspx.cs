using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.Web.Content.Receive
{
    public partial class AddAccept : Base.BasePageTask
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
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Content.Receive.AddAccept).Assembly;	//어셈블리 선언

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

            LoadEditor();

            LoadUser();
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
                cbtnAdd.Text = resMng.GetString("Received");
                cbtnReceive.Text = resMng.GetString("AddReceive");
                cbtnReference.Text = resMng.GetString("AddReference");
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
        private void LoadEditor()
        {
            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";
            tbHiddenMime_contents.Text = templateHeader + templateFooter;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                /*
                string strScript = String.Empty;
                if (Request.Path.IndexOf("AddAccept.aspx") > 0)
                {
                    strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.bodyValue = form1.contents.value;" + "<" + "/script>";

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnControlInit", strScript);
                    NonActiveX = "F";
                }
                else
                {   //스크립트에서 처리한다
                    NonActiveX = "T";
                }
                */
                NonActiveX = "T";
            }         
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadUser()
        {            
            //수신자 
            DAL.ListTO dalListTo = new DAL.ListTO();
            var listTo = dalListTo.GetListToMod(CurrentUserInfo.CompanyID, ListID);
            string touID = "";
            string todID = "";
            string toname = "";
            string toisGroup = "";

            foreach (var info in listTo)
            {
                if (info != null)
                {
                    touID += info.UserID + ",";
                    todID += info.DeptID + ",";

                    string tmpName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                    string tmpDept = this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                    string tmpTitle = this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang);
                    toname += tmpDept + "/" + tmpTitle + "/" + tmpName + ",";

                    toisGroup += "M" + ",";
                }

                ReceiveName = toisGroup + "‡" + touID + "‡" + todID + "‡" + toname.TrimEnd(',');
            }


            //참조자 
            DAL.ListCC dalListCC = new DAL.ListCC();
            var listCC = dalListCC.GetListCCMod(CurrentUserInfo.CompanyID, ListID);
            string ccuID = "";
            string ccdID = "";
            string ccname = "";
            string ccisGroup = "";

            foreach (var info in listCC)
            {
                if (info != null)
                {
                    ccuID += info.UserID + ",";
                    ccdID += info.DeptID + ",";

                    if (info.IsGroup == "G")
                    {
                        string tmpName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                        ccname += tmpName + ",";
                    }
                    else
                    {                        
                        string tmpName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                        string tmpDept = this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                        string tmpTitle = this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang);
                        ccname += tmpDept + "/" + tmpTitle + "/" + tmpName + ",";
                    }

                    ccisGroup += info.IsGroup + ",";
                }

                ReferenceName = ccisGroup + "‡" + ccuID + "‡" + ccdID + "‡" + ccname.TrimEnd(',');
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
            be.MsgContents = GetResource("MSGReceive");

            int ymd = System.DateTime.Now.ToString("yyyyMMdd").ToNumber<int>();
            int startDate = DateHelper.GetCultureDate(Request[this.ctbStartDate.ClientID].ToString(), "yyyyMMdd", PreLangReal).ToNumber<int>();
            int endDate = DateHelper.GetCultureDate(Request[this.ctbEndDate.ClientID].ToString(), "yyyyMMdd", PreLangReal).ToNumber<int>();
            if (endDate >= ymd)
            {
                if (startDate <= ymd)
                {
                    be.Status = "3";
                }
                else
                {
                    be.Status = "2";
                }
            }
            else
            {
                be.Status = "6"; //지연
            }    

            be.IsMsg = "0";
            be.IsType = "TO"; //요청 받은 협업
            be.WriteUserID = CurrentUserInfo.UserID;
            be.ToUserList = hdReceiveID.Value.Trim();
            be.CCUserList = hdReferenceID.Value.Trim();            
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
            loInfo.StartDate = startDate.ToString();
            loInfo.EndDate = endDate.ToString();
            loInfo.Status = be.Status;

            loInfo.ModifyUserID = CurrentUserInfo.UserID;

            #endregion        

            BLL.List dal = new BLL.List();
            return dal.Accept(be, loInfo, arrAppList, arrTaskList);           
        }      
       
        #endregion (함수)

        #region (속성)

        /// <summary>
        /// 수신자
        /// </summary>
        /// <value>The name of the charger.</value>
        public string ReceiveName
        {
            get { return GetViewStateString("ReceiveName", ""); }

            set { ViewState["ReceiveName"] = value; }
        }

        /// <summary>
        /// 참조자
        /// </summary>
        /// <value>The name of the linked dept.</value>
        public string ReferenceName
        {
            get { return GetViewStateString("ReferenceName", ""); }

            set { ViewState["ReferenceName"] = value; }
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

                if (Request.Url.AbsolutePath.IndexOf("AddAccept.aspx") != -1)
                {
                    if (IsPage == "")
                    {
                        paraDic.Add("goURL", "/DeskplusEIP/Task/Content/Receive/View.aspx");
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
                        paraDic.Add("goURL", "/DeskplusEIP/Task/Content/Receive/ViewN.aspx");
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