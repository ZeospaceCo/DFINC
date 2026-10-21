using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Coop.Web.Content.Receive
{
    public partial class AddComplete : Base.BasePageCoop
    {
        #region 변수설정

        public string BaseDocIDs;  //근거문서들 추가 순서대로의 documentID ex) |234;|454;|875; -> |ListBaseID;

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
            AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Content.Receive.AddComplete).Assembly;	//어셈블리 선언

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

            //난이도(상:1;중:2;하:3)
            ddlDifficulty.Items.Add(new ListItem("----", "0"));
            ddlDifficulty.Items.Add(new ListItem(GetResource("Top"), "1"));
            ddlDifficulty.Items.Add(new ListItem(GetResource("Medium"), "2"));
            ddlDifficulty.Items.Add(new ListItem(GetResource("Bad"), "3"));

            LoadCUMode();
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
                cbtnList.Text = resMng.GetString("List");
                cbtnComplete.Text = resMng.GetString("Completes");
                cbtnSwfFileAdd.Text = resMng.GetString("AddFile");

                cbtnDocAdd.Text = resMng.GetString("AddDoc");
                cbtnCoopAdd.Text = resMng.GetString("AddDoc");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            BaseDocIDs = GetDecryptParam("baseDocIDs", "");
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 추가, 수정, 답변시 에디터 설정하기
        /// 작성자   : 심재운
        /// </summary>
        private void LoadCUMode()
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

                        //int itemSubID = dtrList["itemSubID"].ToString().ToNumber<int>();

                        //if (itemSubID > 0)
                        //    tbHiddenMime_contents.Text = GetTemplate(itemSubID);
                        //else
                        //    tbHiddenMime_contents.Text = templateHeader + templateFooter;

                        tbHiddenMime_contents.Text = templateHeader + templateFooter;

                        ltlStatus.Text = GetStatus(dtrList["StatusLast"].ToString());
                        ctbStartDate.Text = DateHelper.GetCultureDateChar8(dtrList["StartDate"].ToString(), PreLangReal);
                        ctbEndDate.Text = DateHelper.GetCultureDateChar8(dtrList["EndDate"].ToString(), PreLangReal);
                        ddlDifficulty.SelectedValue = dtrList["Difficultys"].ToString();

                        if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                        {
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
                        }

                        #endregion 템플릿 바인딩
                    }
                }
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
        private string GetTemplate(int _itemSubID)
        {
            if (_itemSubID == 0) return "";

            DAL.Template bll = new DAL.Template();
            var alList = bll.GetTemplateLists(CurrentUserInfo.CompanyID, _itemSubID);

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
        /// 작성목적 : 완료하기
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private void Complete()
        {
            string originWriteUserID = "";
            string originTitle = "";
            string statusLast = "";
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
                        originTitle = reader["Title"].ToString().ToNull();
                        be.IsClose = reader["IsClose"].ToString().ToNull();

                        originWriteUserID = reader["WriteUserID"].ToString();
                        //statusLast = this.GetStatus(reader["StatusLast"].ToString().ToNull());
                        statusLast = reader["StatusLast"].ToString().ToNull();
                    }
                }
            }

            string content = GetContents();

            be.ListID = ListID; //댓글시, 필요
            be.Depth = be.Depth + 1;
            be.ListOrder = be.ListOrder + 1;
            be.Status = AppSettingHelper.GetDescription(BE.EnumStatus.COMPLETE);// "4"; //완료
            be.IsAlarm = "0";
            be.Title = "";
            be.Contents = content;

            be.MobileContent = tbHiddenText_contents.Text.Trim();
            be.WriteUserID = CurrentUserInfo.UserID;

            //새로운 수신 및 참조자 할당
            be.ToUserList = "";
            be.CCUserList = "";

            #region 첨부파일

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            be.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            #endregion 첨부파일

            

            #region 옵션

            DAL.ListOption loDal = new DAL.ListOption();
            var loList = loDal.GetListOption(be.CompanyID, ListID);

            BE.ListOptionInfo loInfo = new BE.ListOptionInfo();

            foreach (var info in loList)
            {
                loInfo.CompanyID = info.CompanyID;
                loInfo.ListID = info.ListID;
                loInfo.IsClose = info.IsClose;
            }

            loInfo.StartDate = DateHelper.GetCultureDate(ctbStartDate.Text, "yyyyMMdd", PreLangReal);
            loInfo.EndDate = DateHelper.GetCultureDate(ctbEndDate.Text, "yyyyMMdd", PreLangReal);
            loInfo.Difficultys = ddlDifficulty.SelectedValue;
            loInfo.Status = AppSettingHelper.GetDescription(BE.EnumStatus.COMPLETE);// "4"; //완료
            loInfo.CompleteDate = DateHelper.GetCultureDate(System.DateTime.Now.ToShortDateString(), "yyyyMMdd", PreLangReal);
            loInfo.WriteUserID = CurrentUserInfo.UserID;

            CompleteLog(AppSettingHelper.GetDescription(BE.EnumStatus.COMPLETE)); // 시스템 로그를 먼저 표시

            loDal.ModStatus(loInfo);

            #endregion 옵션

            BLL.List dal = new BLL.List();
            //내용을 기재하지 않으면 저장하지 않는다.
            if (be.MobileContent != "")
            {
                // 전자결재 근거문서
                // |고유id^문서번호;|고유id^문서번호;|고유id^문서번호;
                string[] arrDocList = tbBaseDocIDs.Text.Trim(';').Length > 0 ? tbBaseDocIDs.Text.Trim(';').Split(';') : null;

                // 업무협업 근거문서
                // |고유id;|고유id;|고유id;
                string[] arrCoopList = tbBaseCoopIDs.Text.Trim(';').Length > 0 ? tbBaseCoopIDs.Text.Trim(';').Split(';') : null;

                dal.Reply(be, arrDocList, arrCoopList);
            }

            //CompleteLog(statusLast);
            //CompleteLog(AppSettingHelper.GetDescription(BE.EnumStatus.COMPLETE));

            #region 알람용 : 완료

            BLL.Alarm alarmBiz = new BLL.Alarm();
            string alarmContent = "[" + GetResource("Complete") + "] " + originTitle;
            alarmBiz.AlarmComplete(CurrentUserInfo.CompanyID, GroupID, originWriteUserID.ToNumber<int>(), CurrentUserInfo.UserID, CurrentUserInfo.Name, alarmContent);

            #endregion 알람용 : 완료
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 로그 기재하기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_statusLast">The _status last.</param>
        private void CompleteLog(string _statusLast)
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

            string msg = GetResource("Title") + " : '" + be.Title + "' " + CurrentUserInfo.Name + " " + GetResource("MSGComplete");

            string content = @"<HTML>
<HEAD>
<META content='text/html; charset=utf-8' http-equiv=Content-Type>
<STYLE>p {margin-top:2px;margin-bottom:2px;}</STYLE>
<META name=GENERATOR content='TAGFREE Active Designer v2.5'>
</HEAD>
<BODY>" + msg + "</BODY></HTML>";

            be.Contents = content;
            be.MobileContent = msg;
            be.Title = "";

            be.ListID = ListID; //댓글시, 필요
            be.Depth = be.Depth + 1;
            be.ListOrder = be.ListOrder + 1;

            be.WriteUserID = CurrentUserInfo.UserID;
            be.Status = _statusLast;
            be.IsAlarm = "1";

            #region 알람용

            be.LoginID = CurrentUserInfo.LogInID;
            be.NameBase = CurrentUserInfo.Name;

            #endregion 알람용

            be.AlarmContent = ""; //alarm 내용
            be.SMSContent = ""; //sms  내용

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

            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("GoList"));

            if (Request.Url.AbsolutePath.IndexOf("AddComplete.aspx") != -1)
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Content/Receive/ViewComplete.aspx");
            else
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Content/Receive/ViewCompleteN.aspx");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
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
        /// 1:대기;2:접수;3:처리중;4:완료;5:반려
        /// </summary>
        /// <value>
        /// The depth.
        /// </value>
        public string Status
        {
            get { return GetViewStateString("Status", ""); }
            set { ViewState["Status"] = value; }
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
            Complete();
        }

        #endregion (이벤트핸들러)
    }
}