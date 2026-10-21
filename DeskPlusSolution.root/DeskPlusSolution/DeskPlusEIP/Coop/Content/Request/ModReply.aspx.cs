using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Coop.Web.Content.Request
{
    public partial class ModReply : Base.BasePageCoop
    {
        #region 변수설정

        public string BaseDocIDs;  //근거문서들 추가 순서대로의 documentID ex) |234;|454;|875; -> |ListBaseID;
        public string Contents = string.Empty; //내용
        public string Status = ""; //상태값

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 :
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Content.Request.ModReply).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();

                SwfUploadGUID = System.Guid.NewGuid().ToString();

                #region 에디터 본문 이미지 첨부 경로

                System.Guid guid = System.Guid.NewGuid();
                string reqTime = System.DateTime.Now.ToString("yyyyMM");
                UploadPath = "/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID + "/Coop/" + reqTime + "/editor/" + guid.ToString();

                #endregion 에디터 본문 이미지 첨부 경로
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
            LoadList();
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
                cbtnList.Text = resMng.GetString("BtnCancel");
                cbtnAdd.Text = resMng.GetString("Btn_Save");
                cbtnSwfFileAdd.Text = resMng.GetString("AttachName");

                cbtnDocAdd.Text = resMng.GetString("AddDoc");
                cbtnCoopAdd.Text = resMng.GetString("AddDoc");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            //itemsubid, templetecount
            BaseDocIDs = GetDecryptParam("baseDocIDs", "");
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 리스트
        /// </summary>
        private void LoadList()
        {
            bool isAuth = false;

            DAL.List dalList = new DAL.List();

            //1. 내용 가져오기
            var dtrList = dalList.GetList(CurrentUserInfo.CompanyID, ListID.ToString());

            if (dtrList != null)
            {
                if (dtrList.HasRows)
                {
                    while (dtrList.Read())
                    {
                        ListOrder = dtrList["ListOrder"].ToString().ToNumber<int>();
                        Depth = dtrList["Depth"].ToString().ToNumber<int>();

                        Contents += dtrList["Contents"].ToString().ToEditorXSS();
                        tbHiddenHTML_contents.Value = dtrList["Contents"].ToString().ToEditorXSS();
                        tbHiddenMime_contents.Text = Contents;

                        if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                        {
                            //ByFreeman 20120106 X-Free 에디터 추가
                            //Mod.aspx 에서 호출했는지 ModN.aspx 에서 호출했는지 확인
                            string strScript = String.Empty;
                            if (Request.Path.IndexOf("ModReply.aspx") > 0)
                            {
                                strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.bodyValue = form1.contents.value;" + "<" + "/script>";

                                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnControlInit", strScript);
                                NonActiveX = "F";
                            }
                            else
                            {   //스크립트에서 처리한다
                                NonActiveX = "T";
                            }
                        }

                        //접근여부
                        isAuth = (CurrentUserInfo.UserID == dtrList["WriteUserID"].ToString().ToNumber<int>()) ? true : false;
                    }
                }
            }

            if (!isAuth)
            {
                Contents = "";
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
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
        /// 메서드명 :
        /// 작성목적 : 추가, 답변 담기
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private bool WriteMode()
        {
            string content = GetContents();

            //1. 게시판 리스트
            BE.ListInfo be = new BE.ListInfo();

            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ListID = ListID;
            be.GroupID = GroupID;
            be.Title = "";
            be.Contents = content;
            be.MobileContent = tbHiddenText_contents.Text.Trim();
            be.Status = AppSettingHelper.GetDescription(BE.EnumStatus.STANDBY);// "1"; //상신자는 대기
            be.WriteUserID = CurrentUserInfo.UserID;
            be.ModifyUserID = CurrentUserInfo.UserID;
            be.LoginID = CurrentUserInfo.LogInID;
            be.NameBase = CurrentUserInfo.Name;

            be.AlarmContent = ""; //alarm 내용
            be.SMSContent = ""; //sms  내용

            #region 첨부파일

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            be.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            #endregion 첨부파일

            be.ToUserList = "";
            be.CCUserList = "";

            #region 근거문서

            // 전자결재 근거문서
            // |고유id^문서번호;|고유id^문서번호;|고유id^문서번호;
            string[] arrDocList = tbBaseDocIDs.Text.Trim(';').Length > 0 ? tbBaseDocIDs.Text.Trim(';').Split(';') : null;

            // 업무협업 근거문서
            // |고유id;|고유id;|고유id;
            string[] arrCoopList = tbBaseCoopIDs.Text.Trim(';').Length > 0 ? tbBaseCoopIDs.Text.Trim(';').Split(';') : null;

            #endregion 근거문서

            BLL.List dal = new BLL.List();
            dal.Mod(be, null, arrDocList, arrCoopList);

            return true;
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

        #endregion (속성)

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 :
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            LoadParam();

            WriteMode();

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchStatus", SearchStatus);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("searchDateValue", SearchDateValue);
            paraDic.Add("order", Order);
            paraDic.Add("listCount", ListCount);
            paraDic.Add("listID", GroupID.ToString());
            paraDic.Add("groupID", GroupID.ToString());
            paraDic.Add("itemID", ItemID.ToString());
            paraDic.Add("itemSubID", ItemSubID.ToString());

            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("GoList"));

            if (Request.Url.AbsolutePath.IndexOf("ModReply.aspx") != -1)
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Content/Request/View.aspx");
            else
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Content/Request/ViewN.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        #endregion (이벤트핸들러)
    }
}