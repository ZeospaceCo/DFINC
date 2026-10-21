using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text;

namespace Zeospace.DeskPlus.Task.Web.Draft
{
    public partial class Add : Base.BasePageTask
    {
        #region 변수설정

        public string BaseDocIDs;  //근거문서들 추가 순서대로의 documentID ex) |234;|454;|875; -> |ListBaseID;
        private string eventArgs = String.Empty;
        public string Contents = string.Empty; //내용  
        public int DraftID = 0;

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
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Draft.Add).Assembly;	//어셈블리 선언

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

            // 소분류
            ddlItemSub.Items.Clear();
            ddlItemSub.Items.Add(new ListItem(resMng.GetString("SubCategory"), "0"));

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

            if (IsType == "RE")
            {
                ltlHeader.Text = resMng.GetString("Request");
                cbtnAdd.Text = resMng.GetString("Btn_Save");
                cbtnMod.Visible = false;
                cbtnDel.Visible = false;
                cbtnDraft.Visible = true;
                cbtnDraftList.Visible = true;
            }
            else
            {
                ltlHeader.Text = resMng.GetString("Draft");
                cbtnAdd.Text = resMng.GetString("Request");
                cbtnMod.Visible = true;
                cbtnDel.Visible = true;
                cbtnDraft.Visible = false;
                cbtnDraftList.Visible = false;
            }


            LoadDataContent();
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
                cbtnCancel.Text = resMng.GetString("BtnCancel");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnDel.Text = resMng.GetString("Del");
                cbtnReceive.Text = resMng.GetString("AddReceive");
                cbtnReference.Text = resMng.GetString("AddReference");
                //cbtnSwfFileAdd.Text = resMng.GetString("AddFile");
                
                //cbtnAddApp.Text = resMng.GetString("AddDoc");
                cbtnAddTask.Text = resMng.GetString("AddDoc");

                // 1.일반 / 2. 보안
                rblIsClose.Items.Clear();
                rblIsClose.Items.Add(new ListItem(resMng.GetString("IsClose1"), "1"));
                rblIsClose.Items.Add(new ListItem(resMng.GetString("IsClose2"), "2"));

                // 1.협조업무 / 2. 지시업무
                rblIsGubun.Items.Clear();
                rblIsGubun.Items.Add(new ListItem(resMng.GetString("IsGubun1"), "1"));
                rblIsGubun.Items.Add(new ListItem(resMng.GetString("IsGubun2"), "2"));
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            BaseDocIDs = GetDecryptParam("baseDocIDs", "");
            DraftID = GetDecryptParam("draftID", "0").ToNumber<int>();       
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadDataContent()
        {
            int tmpItemID = 0;
            int tmpItemSubID = 0;
            bool isAuth = false;

            DAL.Draft dalList = new DAL.Draft();

            //1. 내용 가져오기
            var dtrList = dalList.GetList(CurrentUserInfo.CompanyID, DraftID.ToString());

            if (dtrList != null)
            {
                if (dtrList.HasRows)
                {
                    while (dtrList.Read())
                    {
                        ddlItem.SelectedValue = dtrList["ItemID"].ToString();
                        tmpItemID = dtrList["ItemID"].ToString().ToNumber<int>();
                        tmpItemSubID = dtrList["ItemSubID"].ToString().ToNumber<int>();
                        rblIsClose.SelectedValue = dtrList["IsClose"].ToString();
                        ltlRequestDate.Text = DateHelper.GetCultureDateChar8(System.DateTime.Now.ToString("yyyyMMdd"), PreLangReal);                        

                        tbxCompleteRequestDate.Text = DateHelper.GetCultureDateChar8(dtrList["CompleteRequestDate"].ToString(), PreLangReal);
                        rblIsGubun.SelectedValue = dtrList["IsGubun"].ToString();
                        ddlSource.SelectedValue = dtrList["SourceID"].ToString();

                        tbxTitle.Text = dtrList["Title"].ToString().ToReverseXSS();

                        string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");

                        Contents = templateHeader + dtrList["Contents"].ToString().ToEditorXSS();
                        tbHiddenHTML_contents.Value = dtrList["Contents"].ToString().ToEditorXSS();
                        tbHiddenMime_contents.Text = dtrList["Contents"].ToString().ToEditorXSS();

                        if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                        {
                            /*
                            string strScript = String.Empty;
                            if (Request.Path.IndexOf("Add.aspx") > 0)
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

                        //접근여부
                        isAuth = (CurrentUserInfo.UserID == dtrList["WriteUserID"].ToString().ToNumber<int>()) ? true : false;
                    }
                }
            }

            if (!isAuth)
            {
                this.tbxTitle.Text = "";
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            }

            //소분류 
            DAL.ItemSub dalItemSub = new DAL.ItemSub();
            var itemSubList = dalItemSub.GetItemSub(CurrentUserInfo.CompanyID, tmpItemID, true);
            foreach (var info in itemSubList)
            {
                ddlItemSub.Items.Add(new ListItem(this.GetLangName(
                                    info.NameBase, info.NameKo,
                                    info.NameEn, info.NameJa,
                                    info.NameFr, info.NameZhcn,
                                    info.NameZhtw, PreLang).ToReverseXSS(),
                                    info.ItemSubID.ToString()));
            }

            ddlItemSub.SelectedValue = tmpItemSubID.ToString();

            //수신자 
            DAL.DraftTO dalListTo = new DAL.DraftTO();
            var listTo = dalListTo.GetDraftTo(CurrentUserInfo.CompanyID, DraftID);
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
            DAL.DraftCC dalListCC = new DAL.DraftCC();
            var listCC = dalListCC.GetListCC(CurrentUserInfo.CompanyID, DraftID);
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

            #region 이전 파일생성
            DAL.DraftAttach fDal = new DAL.DraftAttach();
            List<BE.DraftAttachInfo> beforeAttachList = fDal.GetAttachList(CurrentUserInfo.CompanyID, DraftID);
            if (beforeAttachList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (BE.DraftAttachInfo info in beforeAttachList)
                {
                    sb.Append("T>");
                    sb.Append(info.AttachID);
                    sb.Append(">");
                    sb.Append(info.FilePath);
                    sb.Append(">");
                    sb.Append(info.FileName);
                    sb.Append(">");
                    sb.Append(info.FileGUID);
                    sb.Append("|");
                }

                be.BeforeHiddenPath = sb.ToString();
            }
            #endregion

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
            int listID = dal.Add(be, loInfo, arrAppList, arrTaskList, DraftID);

            if (listID > 0)
            {
                #region DOC_STREAM
                var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.TASK, "DocStreamYN", "N");

                if (docStreamYN == "Y")
                {
                    DAL.Attach dalFile = new DAL.Attach();
                    DocStream ds = new DocStream();
                    try
                    {
                        var newDSList = dalFile.GetAttachList(be.CompanyID, listID);
                        for (int i = 0; i < newDSList.Count; i++)
                        {
                            ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachID, "TAS_ATTACH");
                            ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachID, "TAS_ATTACH");
                        }
                    }
                    catch (Exception ex)
                    {
                        var DSList = dalFile.GetAttachList(be.CompanyID, listID);
                        for (int i = 0; i < DSList.Count; i++)
                        {
                            ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachID, "TAS_ATTACH");
                        }
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("TAS_ATTACH_Addition_DS : " + ex.Message);
                    }

                }
                #endregion
            }

            if (listID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();                
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
        /// 수정하기
        /// </summary>
        private void ModContent()
        {
            string content = GetContents();

            BE.DraftInfo be = new BE.DraftInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.DraftID = DraftID;
            be.Title = tbxTitle.Text;
            be.Contents = content;
            be.ContentText = tbHiddenText_contents.Text.Trim();            
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
            loInfo.DraftID = be.DraftID;
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

            #endregion

            BLL.Draft dal = new BLL.Draft();
            bool rtn = dal.Mod(be, loInfo, arrAppList, arrTaskList);

            if (rtn)
            {
                #region DOC_STREAM
                var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.TASK, "DocStreamYN", "N");

                if (docStreamYN == "Y")
                {
                    DAL.DraftAttach dalFile = new DAL.DraftAttach();
                    DocStream ds = new DocStream();
                    try
                    {
                        var newDSList = dalFile.GetAttachList(be.CompanyID, be.DraftID);
                        for (int i = 0; i < newDSList.Count; i++)
                        {
                            ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachID, "TAS_DRAFT");
                            ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachID, "TAS_DRAFT");
                        }
                    }
                    catch (Exception ex)
                    {
                        var DSList = dalFile.GetAttachList(be.CompanyID, be.DraftID);
                        for (int i = 0; i < DSList.Count; i++)
                        {
                            ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachID, "TAS_DRAFT");
                        }
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("TAS_DRAFT_MOD_DS : " + ex.Message);
                    }

                }
                #endregion
            }

            if (rtn)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("currentPage", "1");
                paraDic.Add("searchStatus", SearchStatus);
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("searchDateValue", SearchDateValue);
                paraDic.Add("itemID", ItemID.ToString());
                paraDic.Add("itemSubID", ItemSubID.ToString());
                paraDic.Add("isGubun", IsGubun.ToString());

                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoToList"));
                paraDic.Add("goURL", "/DeskplusEIP/Task/Draft/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 삭제하기
        /// </summary>
        private void Del()
        {
            BLL.Draft biz = new BLL.Draft();
            if (biz.Del(CurrentUserInfo.CompanyID, DraftID))
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("searchStatus", SearchStatus);
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("searchDateValue", SearchDateValue);
                paraDic.Add("itemID", ItemID.ToString());
                paraDic.Add("itemSubID", ItemSubID.ToString());
                paraDic.Add("isGubun", IsGubun.ToString());

                paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
                paraDic.Add("alertMsg", GetResource("GoToList"));
                paraDic.Add("goURL", "/DeskplusEIP/Task/Draft/List.aspx");

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
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("TAS_DRAFT_DS : " + ex.Message);
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
                paraDic.Add("isType", IsType);
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
                    case "ADD": //요청하기
                        Addition();
                        break;

                    case "MOD":  //수정
                        ModContent();
                        break;

                    case "DEL":  //삭제
                        Del();
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

        #endregion (이벤트핸들러)
    }
}