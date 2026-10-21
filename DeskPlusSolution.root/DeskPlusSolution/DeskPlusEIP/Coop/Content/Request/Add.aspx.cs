using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.IO;

namespace Zeospace.DeskPlus.Coop.Web.Content.Request
{
    public partial class Add : Base.BasePageCoop
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정        
        public string TplItemSubID;
        public string BaseDocIDs;  //근거문서들 추가 순서대로의 documentID ex) |234;|454;|875; -> |ListBaseID;
        #endregion

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
            AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Content.Request.Add).Assembly;	//어셈블리 선언
            #endregion
            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();

                SwfUploadGUID = System.Guid.NewGuid().ToString();

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
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.Item dal = new DAL.Item();
            var list = dal.GetItem(CurrentUserInfo.CompanyID, true);
            
            foreach (var info in list)
            {
                ddlItem.Items.Add(new ListItem(this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr,
                    info.NameZhcn, info.NameZhtw, PreLang), info.ItemID.ToString()));
            }

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
                cbtnList.Text = resMng.GetString("BtnCancel");
                cbtnAdd.Text = resMng.GetString("Btn_Save");
                cbtnReceive.Text = resMng.GetString("AddReceive");
                cbtnReference.Text = resMng.GetString("AddReference");
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
            TplItemSubID = GetDecryptParam("tplItemSubID", "0,0");//2차 - //itemsubid, templetecount
            BaseDocIDs = GetDecryptParam("baseDocIDs", "");
        }


        /// <summary>
        /// 자주 지정자 설정하기
        /// </summary>
        private void LoadFavUser()
        {
            FavorUser = "";
            DAL.ItemFavUser dal = new DAL.ItemFavUser();
            var dtr = dal.GetList(CurrentUserInfo.CompanyID, TplItemSubID.Split(',')[0].ToNumber<int>());

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string uID = "";
                    string name = "";
                    
                    while (dtr.Read())
                    {
                        uID += dtr["UserID"].ToString() + ",";

                        name += this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), PreLang) + ",";
                    }

                    //var response = userID + "‡" + userName + "‡" + types;
                    FavorUser = uID + "‡" + name + "‡" + "M";
                }
            }

            
        }

        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : 추가, 수정, 답변시 에디터 설정하기
        /// 작성자   : 심재운
        /// </summary>
        private void LoadCUMode()
        {
            #region 템플릿 바인딩

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";

            //itemsubid, templetecount
            if (TplItemSubID != "0,0")
            {
                tbHiddenMime_contents.Text = GetTemplate();
                LoadFavUser(); //자주지정자 가져오기
            }
            else
                tbHiddenMime_contents.Text = templateHeader + templateFooter;


            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
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

            #endregion
        }


        #endregion

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
        /// 작성목적 : 추가, 답변 담기
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private int WriteMode()
        {
            string content = GetContents();

            //1. 게시판 리스트            
            BE.ListInfo be = new BE.ListInfo();

            string itemSubID = tbSelItemSubID.Text.Trim();
            if (itemSubID.Length > 0)
            {
                //itemusbid, tmeplatecount
                itemSubID = itemSubID.Split(',')[0];
            }

            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ListID = 0;            
            be.GroupID = 0;
            be.ListOrder = 0;
            be.Depth = 0;
            be.Title = tbxTitle.Text;
            be.Contents = content;
            be.MobileContent = tbHiddenText_contents.Text.Trim();
            be.Status = AppSettingHelper.GetDescription(BE.EnumStatus.STANDBY);// "1";//대기
            be.IsAlarm = "0";            
            be.WriteUserID = CurrentUserInfo.UserID;            

            #region 첨부파일

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            be.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            #endregion

            be.ToUserList = hdReceiveID.Value.Trim();
            be.CCUserList = hdReferenceID.Value.Trim();           
            
            // 전자결재 근거문서
            // |고유id^문서번호;|고유id^문서번호;|고유id^문서번호;
            string[] arrDocList = tbBaseDocIDs.Text.Trim(';').Length > 0 ? tbBaseDocIDs.Text.Trim(';').Split(';') : null;

            // 업무협업 근거문서
            // |고유id;|고유id;|고유id;
            string[] arrCoopList = tbBaseCoopIDs.Text.Trim(';').Length > 0 ? tbBaseCoopIDs.Text.Trim(';').Split(';') : null;

            #region 옵션 (질문글 및 답글에 대한 최종 상태값)
                      
            BE.ListOptionInfo loInfo = new BE.ListOptionInfo();
            loInfo.CompanyID = be.CompanyID;
            loInfo.ItemID = ddlItem.SelectedValue.ToString().ToNumber<int>();
            loInfo.ItemSubID = itemSubID.ToNumber<int>();
            loInfo.RequestDate = DateHelper.GetCultureDate(System.DateTime.Now.ToShortDateString(), "yyyyMMdd", PreLangReal);
            loInfo.CompleteRequestDate = DateHelper.GetCultureDate(Request[this.tbxCompleteRequestDate.ClientID].ToString(), "yyyyMMdd", PreLangReal);
            loInfo.IsClose = (cbIsClose.Checked) ? "2" : "1"; //1:공개;2:비공개            

            loInfo.StartDate = "";
            loInfo.EndDate = "";
            loInfo.Difficultys = "";
            loInfo.Status = AppSettingHelper.GetDescription(BE.EnumStatus.STANDBY);// "1";            
            loInfo.StartDate = "";
            loInfo.EndDate = "";
            loInfo.CompleteDate = "";
            loInfo.Difficultys = "";//중요도            
            loInfo.WriteUserID = CurrentUserInfo.UserID;            

            #endregion
            
            BLL.List dal = new BLL.List();
            int listID = dal.Add(be, loInfo, arrDocList, arrCoopList);

            #region 알람용 : 요청
            BLL.Alarm alarmBiz = new BLL.Alarm();            
            string toContent =  "[" + GetResource("Receive") + "] " + be.Title; //수신
            string ccContent = "[" + GetResource("Reference") + "] " + be.Title; //참조
            alarmBiz.AlarmRequest(CurrentUserInfo.CompanyID, listID, CurrentUserInfo.UserID, CurrentUserInfo.LogInID, CurrentUserInfo.Name, toContent, ccContent);
            #endregion

            return listID;
        }              

        #endregion

        #region (속성)

        /// <summary>
        /// FavorUser
        /// </summary>
        public string FavorUser
        {
            get { return GetViewStateString("FavorUser", ""); }
            set { ViewState["FavorUser"] = value; }
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
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            LoadParam();

            int listID = WriteMode();

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("currentPage", "1");
            paraDic.Add("searchStatus", SearchStatus);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("searchDateValue", SearchDateValue);
            paraDic.Add("order", Order);
            paraDic.Add("listCount", ListCount);
            paraDic.Add("listID", listID.ToString());
            paraDic.Add("groupID", listID.ToString());
            paraDic.Add("itemID", ItemID.ToString());
            paraDic.Add("itemSubID", ItemSubID.ToString());

            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("GoList"));

            if (Request.Url.AbsolutePath.IndexOf("Add.aspx") != -1)
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Content/Request/View.aspx");
            else
                paraDic.Add("goURL", "/DeskplusEIP/Coop/Content/Request/ViewN.aspx");


            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);
        }

        #endregion
    }
}