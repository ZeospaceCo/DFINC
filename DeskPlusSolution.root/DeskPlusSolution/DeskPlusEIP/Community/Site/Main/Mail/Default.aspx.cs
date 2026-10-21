using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;

namespace Zeospace.DeskPlus.Community.Web.Site.Main.Mail
{
    public partial class Default : Base.BasePageCommunity
    {
        #region 변수설정
        public int CommunityID = 0;
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string Order = String.Empty; //sort     
        public string MailOption = String.Empty;
        public string MailUserID = String.Empty; 
        public string SearchGroup = string.Empty; //분류
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchTerm = string.Empty; //검색기간구분자
        public string SearchTermValue = string.Empty; //검색기간 시작,종료값 
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
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Main.Mail.Default).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-05
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();           
            

            CTBSendName.Text = CurrentUserInfo.Name + " <" + CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain + ">";

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";

            tbHiddenMime_contents.Text = templateHeader + templateFooter;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                /*
                string strScript = String.Empty;
                if (Request.Path.IndexOf("Default.aspx") > 0)
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

                #region 에디터 본문 이미지 첨부 경로
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = FileHelper.GetYearMonth();
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Community/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로
                #endregion
            }

            if (MailOption == "2")
            {
                // 커뮤니티 회원메일 주소 다 가져오기
                GetTotalMail();
            }
            else
            {
                // 선택한 회원 메일 주소 가져오기
                if (MailUserID != "")
                {
                    GetMail();
                }
            }

        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-05
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnSend.Text = resMng.GetString("Send");             //"보내기";
                cbtnCancel.Text = resMng.GetString("Cancel");         //"취소";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-05
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MailOption = GetDecryptParam("mailOption", "1");
            MailUserID = GetDecryptParam("mailUserID", "");
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            RecordPerPage = GetDecryptParam("recordPerPage", "10");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 커뮤니티 회원메일 주소 다 가져오기
        /// </summary>
        private void GetTotalMail()
        {
            string tmpVal = String.Empty;
            DAL.User dal = new DAL.User();

            DataTableReader dtr = dal.GetUserInfo(CurrentUserInfo.CompanyID, CommunityID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if ((dtr["MailID"].ToString() != "") && (dtr["MailDomain"].ToString() != ""))
                        {
                            string name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                                        dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                            tmpVal += name + "<" + dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString() + ">;";
                        }
                    }
                }
            }

            ctbTo.Text = tmpVal;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-08
        /// 작성목적 : 선택한 회원메일 주소 가져오기
        /// </summary>
        private void GetMail()
        {
            string tmpVal = String.Empty;
            DAL.User dal = new DAL.User();

            MailUserID = MailUserID.TrimEnd(';');

            DataTableReader dtr = dal.GetUserInfo(CurrentUserInfo.CompanyID, CommunityID, MailUserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if ((dtr["MailID"].ToString() != "") && (dtr["MailDomain"].ToString() != ""))
                        {
                            string name = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                                        dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                            tmpVal += name + "<" + dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString() + ">;";
                        }
                    }
                }
            }

            ctbTo.Text = tmpVal;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-05
        /// 작성목적 : 메일 보내기
        /// </summary>
        /// <returns></returns>
        private bool Addition()
        {
            bool rtnVal = false;

            #region MailTo
            Hashtable htMailTo = new Hashtable();
            #endregion

            #region MailCC
            Hashtable htMailCC = new Hashtable();
            #endregion

            #region MailBCC
            Hashtable htMailBCC = new Hashtable();
            #endregion

            string[] arrMailTo = tbTo.Text.TrimEnd(';').Split(';');

            foreach (string mailTo in arrMailTo)
            {
                if (IsValidEmail(mailTo.Trim()))
                {
                    htMailTo.Add(mailTo.Trim(), mailTo.Trim());
                }
            }

            string[] arrMailCc = tbCc.Text.TrimEnd(';').Split(';');

            foreach (string mailCc in arrMailCc)
            {
                if (IsValidEmail(mailCc.Trim()))
                {
                    htMailCC.Add(mailCc.Trim(), mailCc.Trim());
                }
            }

            string[] arrMailBcc = tbBcc.Text.TrimEnd(';').Split(';');

            foreach (string mailBcc in arrMailBcc)
            {
                if (IsValidEmail(mailBcc.Trim()))
                {
                    htMailBCC.Add(mailBcc.Trim(), mailBcc.Trim());
                }
            }

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);

            MailDataInfo mailInfo = new MailDataInfo();
            mailInfo.To = htMailTo;
            mailInfo.Cc = htMailCC;
            mailInfo.Bcc = htMailBCC;
            string mail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
            mailInfo.From = CurrentUserInfo.Name + "<" + mail + ">";
            mailInfo.Subject = ctbTitle.Text;
            mailInfo.Contents = GetContents();
            mailInfo.Attach = GetAttachFiles();

            SendMail sendMail = new SendMail();
            rtnVal = sendMail.Send(mailInfo);
            
            return rtnVal;
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-05
        /// 작성목적 : 메일 보내기
        /// </summary>
        private void MailSend()
        {
            if (Addition())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("communityID", CommunityID.ToString());
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("searchTerm", SearchTerm);
                paraDic.Add("searchTermValue", SearchTermValue);
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("order", Order);
                paraDic.Add("recordPerPage", RecordPerPage);
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/Community/Site/Main/Search/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-05
        /// 작성목적 : 올바른 메일 형식인지 확인
        /// </summary>
        /// <param name="_email"></param>
        /// <returns></returns>
        public bool IsValidEmail(string _email)
        {
            return Regex.IsMatch(_email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-05
        /// 작성목적 : 본문 내용 및 본문 이미지(업로드) MIME 하여 본문 내용가져오기
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-05
        /// 작성목적 : 첨부파일 리스트
        /// </summary>
        /// <returns></returns>
        private Hashtable GetAttachFiles()
        {
            try
            {
                Hashtable attachList = new Hashtable();

                if (HiddenSpoolPath != "")
                {
                    // 첨부파일 리스트 가져오기
                    string attachFileUploadPath = Server.MapPath(HiddenSpoolPath);
                    if (Directory.Exists(attachFileUploadPath))
                    {
                        string attachFilePath = String.Empty;
                        string[] fileArr = Directory.GetFiles(attachFileUploadPath);

                        for (int i = 0; i < fileArr.Length; i++)
                        {
                            attachFilePath = fileArr[i];
                            attachList.Add(attachFilePath, attachFilePath);
                        }
                    }
                }

                return attachList;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region (프로퍼티)

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
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

        /// <summary>
        /// 
        /// </summary>
        public string HiddenSpoolPath
        {
            get { return GetViewStateString("HiddenSpoolPath", ""); }
            set { ViewState["HiddenSpoolPath"] = value; }
        }
                
        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-05
        /// 작성목적 : 메일 보내기 버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            MailSend();
        }


        #endregion
    }
}