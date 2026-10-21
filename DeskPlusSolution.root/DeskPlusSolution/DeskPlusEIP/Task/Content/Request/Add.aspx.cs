using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.IO;

namespace Zeospace.DeskPlus.Task.Web.Content.Request
{
    public partial class Add : Base.BasePageTask
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정        
        public string TplItemSubID;
        public int TplltemID;
        private string eventArgs = String.Empty;
        public string BaseDocIDs;  //근거문서들 추가 순서대로의 documentID ex) |234;|454;|875; -> |ListBaseID;
        public string BeforeUrl;
        #endregion

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
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Content.Request.Add).Assembly;	//어셈블리 선언
            #endregion
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

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            // 대분류
            DAL.Item dal = new DAL.Item();
            var list = dal.GetItem(CurrentUserInfo.CompanyID, true);

            ddlItem.Items.Clear();
            ddlItem.Items.Add(new ListItem(resMng.GetString("Category"), "0"));
            
            if (list != null)
            {
                foreach (var item in list)
                {
                    string tmpName = this.GetLangName(
                                   item.NameBase, item.NameKo,
                                   item.NameEn, item.NameJa,
                                   item.NameFr, item.NameZhcn,
                                   item.NameZhtw, PreLang);
                    ddlItem.Items.Add(new ListItem(tmpName, item.ItemID.ToString()));
                }                
            }

            ddlItem.SelectedValue = ItemID.ToString();

            if (TplltemID > 0)
            {
                ddlItem.SelectedValue = TplltemID.ToString();
            }

            // 소분류
            ddlItemSub.Items.Clear();
            ddlItemSub.Items.Add(new ListItem(resMng.GetString("SubCategory"), "0,0"));

            // 출처 값 가져오기
            DAL.Source sDal = new DAL.Source();
            var sourceList = sDal.GetSource(CurrentUserInfo.CompanyID, true);

            ddlSource.Items.Clear();
            ddlSource.Items.Add(new ListItem("-----", "0"));

            if (sourceList != null)
            {
                foreach (var source in sourceList)
                {
                    string tmpName = this.GetLangName(
                                  source.NameBase, source.NameKo,
                                  source.NameEn, source.NameJa,
                                  source.NameFr, source.NameZhcn,
                                  source.NameZhtw, PreLang);
                    ddlSource.Items.Add(new ListItem(tmpName, source.SourceID.ToString()));                    
                }
            }

            LoadTemplate();
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
                cbtnDraftList.Text = resMng.GetString("GetDraft");
                cbtnDraft.Text = resMng.GetString("Draft");
                cbtnList.Text = resMng.GetString("BtnCancel");
                cbtnAdd.Text = resMng.GetString("Btn_Save");
                cbtnReceive.Text = resMng.GetString("AddReceive");
                cbtnReference.Text = resMng.GetString("AddReference");
                //cbtnSwfFileAdd.Text = resMng.GetString("AddFile");
                //cbtnAddApp.Text = resMng.GetString("AddDoc");
                cbtnAddTask.Text = resMng.GetString("AddDoc");
                cbtnSpecifierAdd.Text = resMng.GetString("SpecifierSave");

                // 1.일반 / 2. 보안
                rblIsClose.Items.Clear();
                rblIsClose.Items.Add(new ListItem(resMng.GetString("IsClose1"), "1"));
                rblIsClose.Items.Add(new ListItem(resMng.GetString("IsClose2"), "2"));
                rblIsClose.SelectedValue = "1";

                // 1.협조업무 / 2. 지시업무
                rblIsGubun.Items.Clear();
                rblIsGubun.Items.Add(new ListItem(resMng.GetString("IsGubun1"), "1"));
                rblIsGubun.Items.Add(new ListItem(resMng.GetString("IsGubun2"), "2"));
                rblIsGubun.SelectedValue = "1";
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            TplItemSubID = GetDecryptParam("tplItemSubID", "0,0");//소분류 - //itemsubid, templetecount
            TplltemID = GetDecryptParam("tplltemID", "0").ToNumber<int>();
            BaseDocIDs = GetDecryptParam("baseDocIDs", "");
        }


        /// <summary>
        /// 소분류에 지정된 사용자 정보 가져오기
        /// </summary>
        private void LoadItemSubUser()
        {
            ItemSubUser = "";
           
            DAL.Specifier dalSpecifier = new DAL.Specifier();
            var dtrSpecifier = dalSpecifier.GetSpecifier(CurrentUserInfo.CompanyID, TplItemSubID.Split(',')[0].ToNumber<int>(), CurrentUserInfo.UserID);

            if (dtrSpecifier != null)
            {
                if (dtrSpecifier.HasRows)
                {
                    string userID = "";
                    string deptID = "";
                    string name = "";
                    string isGroup = "";
                    string isType = "";

                    while (dtrSpecifier.Read())
                    {
                        userID += dtrSpecifier["UserID"].ToString() + ",";
                        deptID += dtrSpecifier["RDeptID"].ToString() + ",";
                        isGroup += dtrSpecifier["IsGroup"].ToString() + ",";
                        isType += dtrSpecifier["IsType"].ToString() + ",";

                        if (dtrSpecifier["IsGroup"].ToString() == "G")
                        {
                            name += this.GetLangName(
                                    dtrSpecifier["NameBase"].ToString().ToNull(), dtrSpecifier["NameKo"].ToString().ToNull(),
                                    dtrSpecifier["NameEn"].ToString().ToNull(), dtrSpecifier["NameJa"].ToString().ToNull(),
                                    dtrSpecifier["NameFr"].ToString().ToNull(), dtrSpecifier["NameZhcn"].ToString().ToNull(),
                                    dtrSpecifier["NameZhtw"].ToString().ToNull(), PreLang) + ",";
                        }
                        else
                        {
                            string tmpName = this.GetLangName(
                                    dtrSpecifier["NameBase"].ToString().ToNull(), dtrSpecifier["NameKo"].ToString().ToNull(),
                                    dtrSpecifier["NameEn"].ToString().ToNull(), dtrSpecifier["NameJa"].ToString().ToNull(),
                                    dtrSpecifier["NameFr"].ToString().ToNull(), dtrSpecifier["NameZhcn"].ToString().ToNull(),
                                    dtrSpecifier["NameZhtw"].ToString().ToNull(), PreLang);

                            string tmpDept = this.GetLangName(
                                    dtrSpecifier["DeptBase"].ToString().ToNull(), dtrSpecifier["DeptKo"].ToString().ToNull(),
                                    dtrSpecifier["DeptEn"].ToString().ToNull(), dtrSpecifier["DeptJa"].ToString().ToNull(),
                                    dtrSpecifier["DeptFr"].ToString().ToNull(), dtrSpecifier["DeptZhcn"].ToString().ToNull(),
                                    dtrSpecifier["DeptZhtw"].ToString().ToNull(), PreLang);

                            string tmpTitle = this.GetLangName(
                                    dtrSpecifier["TitleBase"].ToString().ToNull(), dtrSpecifier["TitleKo"].ToString().ToNull(),
                                    dtrSpecifier["TitleEn"].ToString().ToNull(), dtrSpecifier["TitleJa"].ToString().ToNull(),
                                    dtrSpecifier["TitleFr"].ToString().ToNull(), dtrSpecifier["TitleZhcn"].ToString().ToNull(),
                                    dtrSpecifier["TitleZhtw"].ToString().ToNull(), PreLang);

                            name += tmpDept + "/" + tmpTitle + "/" + tmpName + ",";
                        }
                    }

                    ItemSubUser = isGroup + "‡" + userID + "‡" + deptID + "‡" + name + "‡" + isType;
                }
            }




            if (ItemSubUser == "")
            {
                DAL.ItemSubUser dal = new DAL.ItemSubUser();
                var dtr = dal.GetList(CurrentUserInfo.CompanyID, TplItemSubID.Split(',')[0].ToNumber<int>());

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        string userID = "";
                        string deptID = "";
                        string name = "";
                        string isGroup = "";
                        string isType = "";

                        while (dtr.Read())
                        {
                            userID += dtr["UserID"].ToString() + ",";
                            deptID += dtr["RDeptID"].ToString() + ",";
                            isGroup += dtr["IsGroup"].ToString() + ",";
                            isType += dtr["IsType"].ToString() + ",";

                            if (dtr["IsGroup"].ToString() == "G")
                            {
                                name += this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang) + ",";
                            }
                            else
                            {
                                string tmpName = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);

                                string tmpDept = this.GetLangName(
                                        dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                        dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                        dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                        dtr["DeptZhtw"].ToString().ToNull(), PreLang);

                                string tmpTitle = this.GetLangName(
                                        dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                        dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                        dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                        dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                                name += tmpDept + "/" + tmpTitle + "/" + tmpName + ",";
                            }
                        }

                        ItemSubUser = isGroup + "‡" + userID + "‡" + deptID + "‡" + name + "‡" + isType;
                    }
                } 
            }


        }

        /// <summary>
        /// 템플릿 정보 가져오기
        /// </summary>
        private void LoadTemplate()
        {
            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";

            //itemsubid, templetecount
            if (TplItemSubID != "0,0")
            {
                tbHiddenMime_contents.Text = GetTemplate();
                LoadItemSubUser();
            }
            else
                tbHiddenMime_contents.Text = templateHeader + templateFooter;


            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                /*
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
                 */
                NonActiveX = "T";                  
                 
            }
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 템플릿 가져오기
        /// </summary>
        /// <returns></returns>
        private string GetTemplate()
        {            
            DAL.Template bll = new DAL.Template();
            var alList = bll.GetTemplateList(CurrentUserInfo.CompanyID, TplItemSubID.Split(',')[0].ToNumber<int>());

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
        /// 요청하기
        /// </summary>
        /// <returns></returns>
        private void Addition()
        {
            string content = GetContents();
                
            BE.ListInfo be = new BE.ListInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ListID = 0;            
            be.GroupID = 0;
            be.Title = tbxTitle.Text;
            be.Contents = content;
            be.ContentText = tbHiddenText_contents.Text.Trim();
            be.Status = "1";
            be.IsMsg = "0";
            be.IsType = "RE"; //요청한 협업
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
            loInfo.ItemID = ddlItem.SelectedValue.ToString().ToNumber<int>();

            string itemSubID = tbSelItemSubID.Text.Trim();
            if (itemSubID.Length > 0)
            {
                itemSubID = itemSubID.Split(',')[0];
            }

            loInfo.ItemSubID = itemSubID.ToNumber<int>();
            loInfo.Status = "1";
            loInfo.StartDate = "";
            loInfo.EndDate = "";
            loInfo.RequestDate = DateHelper.GetCultureDate(System.DateTime.Now.ToShortDateString(), "yyyyMMdd", PreLangReal);
            loInfo.CompleteRequestDate = DateHelper.GetCultureDate(Request[this.tbxCompleteRequestDate.ClientID].ToString(), "yyyyMMdd", PreLangReal);
            loInfo.CompleteDate = "";
            loInfo.IsClose = rblIsClose.SelectedValue; //1: 일반 , 2:보안
            loInfo.IsGubun = rblIsGubun.SelectedValue; //1 : 협조업무 , 2: 지시업부
            loInfo.SourceID = ddlSource.SelectedValue.ToNumber<int>(); //출처         
            loInfo.WriteUserID = CurrentUserInfo.UserID;            

            #endregion            

            BLL.List dal = new BLL.List();
            int listID = dal.Add(be, loInfo, arrAppList, arrTaskList);            

            if (listID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("currentPage", "1");
                paraDic.Add("searchStatus", SearchStatus);
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("searchDateValue", SearchDateValue);                
                paraDic.Add("listID", listID.ToString());
                paraDic.Add("groupID", listID.ToString());
                paraDic.Add("itemID", ItemID.ToString());
                paraDic.Add("itemSubID", ItemSubID.ToString());
                paraDic.Add("isType", "RE");
                paraDic.Add("impoUserYN", "N");
                paraDic.Add("reLoad", "1");
                paraDic.Add("isGubun", IsGubun.ToString());

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));

                if (Request.Url.AbsolutePath.IndexOf("Add.aspx") != -1)
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Content/Request/View.aspx");
                else
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Content/Request/ViewN.aspx");


                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            } 
        }

        /// <summary>
        /// 임시저장 
        /// </summary>
        /// <returns></returns>
        private void Draft()
        {
            string content = GetContents();

            BE.DraftInfo be = new BE.DraftInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;        
            be.Title = tbxTitle.Text;
            be.Contents = content;
            be.ContentText = tbHiddenText_contents.Text.Trim();          
            be.WriteUserID = CurrentUserInfo.UserID;
            be.ToUserList = hdReceiveID.Value.Trim();
            be.CCUserList = hdReferenceID.Value.Trim();

            // 첨부파일
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            be.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            // 전자결재 근거문서 |고유id^문서번호;|고유id^문서번호;|고유id^문서번호;
            string[] arrAppList = tbBaseApps.Text.Trim(';').Length > 0 ? tbBaseApps.Text.Trim(';').Split(';') : null;

            // 업무협업 근거문서 |고유id;|고유id;|고유id;
            string[] arrTaskList = tbBaseTasks.Text.Trim(';').Length > 0 ? tbBaseTasks.Text.Trim(';').Split(';') : null;

            #region 옵션

            BE.DraftOptionInfo loInfo = new BE.DraftOptionInfo();
            loInfo.CompanyID = be.CompanyID;
            loInfo.ItemID = ddlItem.SelectedValue.ToString().ToNumber<int>();

            string itemSubID = tbSelItemSubID.Text.Trim();
            if (itemSubID.Length > 0)
            {
                itemSubID = itemSubID.Split(',')[0];
            }

            loInfo.ItemSubID = itemSubID.ToNumber<int>();
            loInfo.CompleteRequestDate = DateHelper.GetCultureDate(Request[this.tbxCompleteRequestDate.ClientID].ToString(), "yyyyMMdd", PreLangReal);
            loInfo.IsClose = rblIsClose.SelectedValue; //1: 일반 , 2:보안
            loInfo.IsGubun = rblIsGubun.SelectedValue; //1 : 협조업무 , 2: 지시업부
            loInfo.SourceID = ddlSource.SelectedValue.ToNumber<int>(); //출처         
            loInfo.WriteUserID = CurrentUserInfo.UserID;

            #endregion

            BLL.Draft dal = new BLL.Draft();
            int draftID = dal.Add(be, loInfo, arrAppList, arrTaskList);

            if (draftID > 0)
            {
                #region DOC_STREAM
                var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.TASK, "DocStreamYN", "N");

                if (docStreamYN == "Y")
                {
                    DAL.DraftAttach dalFile = new DAL.DraftAttach();
                    DocStream ds = new DocStream();
                    try
                    {
                        var newDSList = dalFile.GetAttachList(be.CompanyID, draftID);
                        for (int i = 0; i < newDSList.Count; i++)
                        {
                            ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachID, "TAS_DRAFT");
                            ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachID, "TAS_DRAFT");
                        }
                    }
                    catch (Exception ex)
                    {
                        var DSList = dalFile.GetAttachList(be.CompanyID, draftID);
                        for (int i = 0; i < DSList.Count; i++)
                        {
                            ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachID, "TAS_DRAFT");
                        }
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("TAS_DRAFT_Addition_DS : " + ex.Message);
                    }

                }
                #endregion
            }

            if (draftID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("currentPage", "1");
                paraDic.Add("searchStatus", SearchStatus);
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("searchDateValue", SearchDateValue);                
                paraDic.Add("draftID", draftID.ToString());
                paraDic.Add("itemID", ItemID.ToString());
                paraDic.Add("itemSubID", ItemSubID.ToString());
                paraDic.Add("isType", "RE");
                paraDic.Add("isGubun", IsGubun.ToString());

                paraDic.Add("alertType", AlertMsgEnum.Alert_Success.ToString());
                paraDic.Add("alertMsg", GetResource("GoDraft"));

                if (Request.Url.AbsolutePath.IndexOf("Add.aspx") != -1)
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Draft/Add.aspx");
                else
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Draft/AddN.aspx");


                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }              

        #endregion

        #region (속성)

        /// <summary>
        /// ItemSubUser
        /// </summary>
        public string ItemSubUser
        {
            get { return GetViewStateString("ItemSubUser", ""); }
            set { ViewState["ItemSubUser"] = value; }
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

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {            
            try
            {
                LoadParam();

                string args = eventArgs;
                switch (args)
                {
                    case "ADD":  //요청하기
                        Addition();
                        break;

                    case "Draft":  //임시저장
                        Draft();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnAdd_Click", ex);
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
        #endregion
    }
}