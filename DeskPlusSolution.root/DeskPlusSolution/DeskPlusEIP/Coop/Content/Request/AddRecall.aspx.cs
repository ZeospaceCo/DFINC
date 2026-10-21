using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Coop.Web.Content.Request
{
    public partial class AddRecall : Base.BasePageCoop
    {
        #region 변수설정

        public string TplItemSubID;
        public string BaseDocIDs;  //근거문서들 추가 순서대로의 documentID ex) |234;|454;|875; -> |ListBaseID;
        public string Contents = string.Empty; //내용
        public string Status = ""; //상태값
        private string eventArgs = String.Empty;

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
            AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Content.Request.AddRecall).Assembly;	//어셈블리 선언

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
                cbtnReceive.Text = resMng.GetString("AddReceive");
                cbtnReference.Text = resMng.GetString("AddReference");
                cbtnRecall.Text = resMng.GetString("Rerequest");
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
            TplItemSubID = GetDecryptParam("tplItemSubID", "0,0");//2차 - //itemsubid, templetecount
            BaseDocIDs = GetDecryptParam("baseDocIDs", "");
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 리스트
        /// </summary>
        private void LoadList()
        {
            int tmpItemID = 0;
            int tmpItemSubID = 0;
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
                        tmpItemID = dtrList["ItemID"].ToString().ToNumber<int>();
                        tmpItemSubID = dtrList["ItemSubID"].ToString().ToNumber<int>();

                        //ltlRequestDate.Text = DateHelper.GetCultureDateChar8(dtrList["RequestDate"].ToString(), PreLangReal);
                        ltlRequestDate.Text = DateHelper.GetCultureDateChar8(System.DateTime.Now.ToString("yyyyMMdd"), PreLangReal);
                        tbxCompleteRequestDate.Text = DateHelper.GetCultureDateChar8(dtrList["CompleteRequestDate"].ToString(), PreLangReal);

                        this.tbxTitle.Text = dtrList["Title"].ToString().ToReverseXSS();
                        this.cbIsClose.Checked = (dtrList["IsClose"].ToString() == "2") ? true : false; //1:공개;2:비공개

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
                            if (Request.Path.IndexOf("AddRecall.aspx") > 0)
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
                this.tbxTitle.Text = "";
                Contents = "";
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            }

            //수신자  - groupid
            DAL.ListTO dalListTo = new DAL.ListTO();
            var listTo = dalListTo.GetListTo(CurrentUserInfo.CompanyID, GroupID);
            string toID = "";
            string toName = "";
            string toGubun = "";
            foreach (var listInfo in listTo)
            {
                if (listInfo != null)
                {
                    toID += listInfo.UserID + ",";
                    toName += this.GetLangName(
                                listInfo.NameBase,
                                listInfo.NameKo, listInfo.NameEn, listInfo.NameJa,
                                listInfo.NameFr, listInfo.NameZhcn, listInfo.NameZhtw, PreLang).ToReverseXSS() + ",";
                    toGubun += "M" + ",";
                }

                ReceiveName = toID + "‡" + toName + "‡" + toGubun.TrimEnd(',');
            }

            //참조자 - groupid
            DAL.ListCC dalListCC = new DAL.ListCC();
            var listCC = dalListCC.GetListCC(CurrentUserInfo.CompanyID, GroupID);
            string ccID = "";
            string ccName = "";
            string ccGubun = "";
            foreach (var listInfo in listCC)
            {
                if (listInfo != null)
                {
                    ccID += listInfo.UserID + ",";
                    ccName += this.GetLangName(
                                listInfo.NameBase,
                                listInfo.NameKo, listInfo.NameEn, listInfo.NameJa,
                                listInfo.NameFr, listInfo.NameZhcn, listInfo.NameZhtw, PreLang).ToReverseXSS() + ",";
                    ccGubun += "M" + ",";
                }

                ReferenceName = ccID + "‡" + ccName + "‡" + ccGubun.TrimEnd(',');
            }

            //1차
            DAL.Item dalItem = new DAL.Item();
            var itemList = dalItem.GetItem(CurrentUserInfo.CompanyID, true);
            foreach (var info in itemList)
            {
                ddlItem.Items.Add(new ListItem(this.GetLangName(
                                    info.NameBase,
                                    info.NameKo,
                                    info.NameEn,
                                    info.NameJa,
                                    info.NameFr,
                                    info.NameZhcn,
                                    info.NameZhtw, PreLang).ToReverseXSS(),
                                    info.ItemID.ToString()));
            }

            if (tmpItemID > 0) this.ddlItem.SelectedValue = tmpItemID.ToString();

            //선택된 2차의 템플릿 카운트
            int selItemSubCnt = 0;
            //2차
            if (tmpItemSubID > 0)
            {
                DAL.ItemSub dalItemSub = new DAL.ItemSub();
                var itemSubList = dalItemSub.GetItemSub(CurrentUserInfo.CompanyID, tmpItemID, true);
                foreach (var info in itemSubList)
                {
                    ddlItemSub.Items.Add(new ListItem(this.GetLangName(
                                        info.NameBase,
                                        info.NameKo,
                                        info.NameEn,
                                        info.NameJa,
                                        info.NameFr,
                                        info.NameZhcn,
                                        info.NameZhtw, PreLang).ToReverseXSS(),
                                        info.ItemSubID.ToString() + "," + info.TempCnt));

                    if (info.ItemSubID == tmpItemSubID) selItemSubCnt = info.TempCnt;
                }

                ddlItemSub.SelectedValue = tmpItemSubID.ToString() + "," + selItemSubCnt;
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 템플릿 적용
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private string GetTemplate()
        {
            DAL.Template bll = new DAL.Template();
            var alList = bll.GetTemplateLists(CurrentUserInfo.CompanyID, TplItemSubID.Split(',')[0].ToNumber<int>());

            foreach (var be in alList) return be.Contents;

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";
            return templateHeader + templateFooter;
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
        /// 메서드명 :
        /// 작성목적 : 수정하기
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private void ModContent()
        {
            string content = GetContents();

            //1. 게시판 리스트
            BE.ListInfo be = new BE.ListInfo();

            string tmpItemSubID = tbSelItemSubID.Text.Trim();//2,0
            if (tmpItemSubID.Length > 0)
            {
                //itemusbid, tmeplatecount
                tmpItemSubID = tmpItemSubID.Split(',')[0];
            }

            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ListID = ListID; 
            be.GroupID = GroupID;
            be.Title = tbxTitle.Text;
            be.Contents = content;
            be.MobileContent = tbHiddenText_contents.Text.Trim();
            be.WriteUserID = CurrentUserInfo.UserID;
            be.LoginID = CurrentUserInfo.LogInID;
            be.NameBase = CurrentUserInfo.Name;
            be.Status = AppSettingHelper.GetDescription(BE.EnumStatus.STANDBY);// "1";
            be.AlarmContent = ""; //alarm 내용
            be.SMSContent = ""; //sms  내용

            #region 첨부파일

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            be.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            #endregion 첨부파일

            be.ToUserList = hdReceiveID.Value.Trim();
            be.CCUserList = hdReferenceID.Value.Trim();

            #region 근거문서

            // 전자결재 근거문서
            // |고유id^문서번호;|고유id^문서번호;|고유id^문서번호;
            string[] arrDocList = tbBaseDocIDs.Text.Trim(';').Length > 0 ? tbBaseDocIDs.Text.Trim(';').Split(';') : null;

            // 업무협업 근거문서
            // |고유id;|고유id;|고유id;
            string[] arrCoopList = tbBaseCoopIDs.Text.Trim(';').Length > 0 ? tbBaseCoopIDs.Text.Trim(';').Split(';') : null;

            #endregion 근거문서

            #region 옵션

            DAL.ListOption loDal = new DAL.ListOption();
            var loList = loDal.GetListOption(be.CompanyID, ListID);

            loList[0].ItemID = ddlItem.SelectedValue.ToString().ToNumber<int>();
            loList[0].ItemSubID = tmpItemSubID.ToNumber<int>();
            loList[0].RequestDate = DateHelper.GetCultureDate(System.DateTime.Now.ToShortDateString(), "yyyyMMdd", PreLangReal);
            loList[0].CompleteRequestDate = DateHelper.GetCultureDate(Request[this.tbxCompleteRequestDate.ClientID].ToString(), "yyyyMMdd", PreLangReal);
            loList[0].IsClose = (cbIsClose.Checked) ? "2" : "1"; //1:공개;2:비공개

            loList[0].StartDate = "";
            loList[0].EndDate = "";
            loList[0].Difficultys = "";
            loList[0].CompleteDate = "";
            loList[0].Status = AppSettingHelper.GetDescription(BE.EnumStatus.STANDBY);// 진행중
            loList[0].WriteUserID = CurrentUserInfo.UserID;

            loDal.Mod(loList[0]);

            #endregion 옵션

            RecallLog();

            BLL.List dal = new BLL.List();
            dal.Mod(be, null, arrDocList, arrCoopList);

            #region 알람용 : 요청

            BLL.Alarm alarmBiz = new BLL.Alarm();
            string toContent = "[" + GetResource("Receive") + "] " + be.Title;
            string ccContent = "[" + GetResource("Reference") + "] " + be.Title;
            alarmBiz.AlarmRequest(CurrentUserInfo.CompanyID, GroupID, CurrentUserInfo.UserID, CurrentUserInfo.LogInID, CurrentUserInfo.Name, toContent, ccContent);

            #endregion 알람용 : 요청

            //RecallLog();

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

            if (Request.Url.AbsolutePath.IndexOf("AddRecall.aspx") != -1)
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Content/Request/View.aspx");
            else
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Content/Request/ViewN.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 재요청 로그 쌓기
        /// 작성자   : 심재운
        /// </summary>
        private void RecallLog()
        {
            string originWriteName = "";

            BE.ListInfo be = new BE.ListInfo();
            DAL.List dalList = new DAL.List();

            //1. 내용 가져오기
            var reader = dalList.GetList(CurrentUserInfo.CompanyID, GroupID.ToString());

            if (reader != null)
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        be.CompanyID = reader["CompanyID"].ToString().ToNull().ToNumber<int>();
                        be.ItemID = reader["ItemID"].ToString().ToNull().ToNumber<int>();
                        be.ItemSubID = reader["ItemSubID"].ToString().ToNull().ToNumber<int>();
                        be.GroupID = reader["GroupID"].ToString().ToNull().ToNumber<int>();
                        be.ListOrder = reader["ListOrder"].ToString().ToNull().ToNumber<int>();
                        be.Depth = reader["Depth"].ToString().ToNull().ToNumber<int>();
                        be.Title = reader["Title"].ToString().ToNull();
                        be.IsClose = reader["IsClose"].ToString().ToNull();

                        originWriteName = this.GetLangName(
                                reader["NameBase"].ToString().ToNull(), reader["NameKo"].ToString().ToNull(),
                                reader["NameEn"].ToString().ToNull(), reader["NameJa"].ToString().ToNull(),
                                reader["NameFr"].ToString().ToNull(), reader["NameZhcn"].ToString().ToNull(),
                                reader["NameZhtw"].ToString().ToNull(), PreLang).ToReverseXSS();
                    }
                }
            }

            string msg = GetResource("Title") + " : '" + be.Title + "' " + CurrentUserInfo.Name + " " + GetResource("MSGRequest");

            string content = @"<HTML>
<HEAD>
<META content='text/html; charset=utf-8' http-equiv=Content-Type>
<STYLE>p {margin-top:2px;margin-bottom:2px;}</STYLE>
<META name=GENERATOR content='TAGFREE Active Designer v2.5'>
</HEAD>
<BODY>" + msg + "</BODY></HTML>";

            be.ListID = ListID; //댓글시, 필요
            be.Contents = content;
            be.MobileContent = msg;
            be.Title = "";
            be.Depth = be.Depth + 1;
            be.ListOrder = be.ListOrder + 1;

            be.WriteUserID = CurrentUserInfo.UserID;
            be.Status = "1"; //대기
            be.IsAlarm = "1";

            //새로운 수신 및 참조자 할당
            be.ToUserList = "";
            be.CCUserList = "";

            #region 첨부파일

            be.HiddenSpoolPath = "";

            #endregion 첨부파일

            BLL.List dal = new BLL.List();
            dal.Reply(be, null, null);

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchStatus", SearchStatus);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("searchDateValue", SearchDateValue);
            paraDic.Add("order", Order);
            paraDic.Add("listCount", ListCount);
            paraDic.Add("listID", ListID.ToString());
            paraDic.Add("groupID", GroupID.ToString());
            paraDic.Add("itemID", ItemID.ToString());
            paraDic.Add("itemSubID", ItemSubID.ToString());

            paraDic.Add("alertType", AlertMsgEnum.Alert_Success.ToString());
            paraDic.Add("alertMsg", GetResource("GoList"));

            if (Request.Url.AbsolutePath.IndexOf("AddRecall.aspx") != -1)
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Content/Request/View.aspx");
            else
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Content/Request/ViewN.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
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
            try
            {
                LoadParam();
                string args = eventArgs;
                switch (args)
                {
                    case "MOD":  //수정
                        ModContent();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
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