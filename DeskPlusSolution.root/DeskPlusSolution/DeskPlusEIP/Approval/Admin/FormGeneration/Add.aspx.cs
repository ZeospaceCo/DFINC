using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration
{
    public partial class Add : Base.BasePageApprovalAdmin
    {
        #region 변수설정

        public string EventArgs = string.Empty;
        public int CompanyID = 0;
        public int UserID = 0;
        public int FormID = 0;
        public string Contents = string.Empty; //내용

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

            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration.Add).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            NonActiveX = "T";
            /*
            //AcitveX Editor Page 인지 확인
            if (Request.Path.IndexOf("Add.aspx") > 0)
            {
                NonActiveX = "F";
            }
            else
            {
                NonActiveX = "T";
            }
            */

            //에디터 본문 이미지 첨부 경로
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            System.Guid guid = System.Guid.NewGuid();
            string reqTime = FileHelper.GetYearMonth();
            UploadPath = baseFileDirectory + CompanyID + "/Approval/Form/Editor/" + guid.ToString();
            if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

            if (FormID > 0)
            {
                this.GetFormInfo(FormID);
                ctbFormName.Enabled = false;
                ctbNameCode.Enabled = false;
            }
            else
            {
                string formCssPath = FullDomainPath + "/DeskPlusEIP/Approval/Css/Func_ApprovalRegistForm.css";
                string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, formCssPath);
                string templateFooter = "</body></html>";
                tbHiddenMime_contents.Text = templateHeader + templateFooter;
                tbHiddenHTML_contents.Value = templateHeader + templateFooter;
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

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
                // 버튼
                cbtnPrint.Text = resMng.GetString("Print");             //"인쇄";
                cbtnConfirm.Text = resMng.GetString("Confirm");         //"확인";
                cbtnDel.Text = resMng.GetString("DelCompletely");       //"영구삭제";
                cbtnClose.Text = resMng.GetString("Close");             //"닫기";

                // Items
                rblIsActive.Items.Clear();
                rblIsActive.Items.Add(new ListItem(resMng.GetString("Use"), "1"));            //사용
                rblIsActive.Items.Add(new ListItem(resMng.GetString("NotUse"), "0"));       //미사용
                rblIsActive.Items[1].Selected = true;
            }
        }

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();
            FormID = GetDecryptParam("formID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 추가
        /// </summary>
        private bool Addition()
        {
            // 양식기본정보 및 Html내용
            FormInfo fInfo = new FormInfo();
            fInfo.CompanyID = CurrentUserInfo.CompanyID;
            fInfo.FormID = FormID;
            fInfo.FormBase = ctbFormName.Text;
            fInfo.NameCode = ctbNameCode.Text;
            fInfo.ContentHTML = GetContents();
            fInfo.IsActive = rblIsActive.SelectedValue;
            fInfo.WriteUserID = CurrentUserInfo.UserID;

            //결재태그정보처리
            List<FormSeqInfo> fslist = new List<FormSeqInfo>();
            if (!tbHiddenSeq.Text.Trim().Equals(""))
            {
                string[] arrSeq = tbHiddenSeq.Text.Trim('^').Split('^');

                for (int i = 0; i < arrSeq.Length; i++)
                {
                    string[] v = arrSeq[i].Split('-');

                    FormSeqInfo fsInfo = new FormSeqInfo();
                    fsInfo.CompanyID = CurrentUserInfo.CompanyID;
                    fsInfo.ApprovalTag = v[0];
                    fsInfo.Sequence = i + 1;
                    fsInfo.ApprovalInfo = v[1];
                    fsInfo.IsFix = (fsInfo.ApprovalTag.IndexOf("FLOW").Equals(0) || fsInfo.ApprovalTag.Length.Equals(2)) ? "0" : "1";
                    fsInfo.IsFlow = (fsInfo.ApprovalTag.IndexOf("FLOW").Equals(0)) ? "1" : "0";

                    fslist.Add(fsInfo);
                }
            }

            bool rtnval = false;
            BLL.Forms fDll = new Forms();
            if (FormID > 0)
            {
                //수정
                rtnval = fDll.Mod(fInfo, fslist);
            }
            else
            {
                //추가
                rtnval = (fDll.Add(fInfo, fslist) > 0) ? true : false;
            }

            return rtnval;
        }

        /// <summary>
        /// 작성자 :
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

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass util = new DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass();

                util.ProcessDecoding(contentValue, filePath, serverNameFolderName, 0);
                contents = util.DecodedHTML.Replace("</HTML>\0", "</HTML>");
            }
            else if (NonActiveX == "T") //x-free
            {
                contents = tbHiddenHTML_contents.Value;
            }

            return Regex.Replace(contents, "</html>\0", "</html>", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 양식삭제
        /// </summary>
        /// <returns></returns>
        private bool Del()
        {
            BLL.Forms fBLL = new Forms();
            return fBLL.Del(CurrentUserInfo.CompanyID, FormID);
        }

        /// <summary>
        /// 양식정보 가져오기
        /// </summary>
        /// <param name="_formID"></param>
        private void GetFormInfo(int _formID)
        {
            if (_formID < 0) return;
            DAL.Form fDal = new DAL.Form();
            List<BE.FormInfo> fList = fDal.GetForm(CurrentUserInfo.CompanyID, _formID);

            foreach (BE.FormInfo fInfo in fList)
            {
                #region 내용필드

                tbHiddenMime_contents.Text = fInfo.ContentHTML;
                tbHiddenHTML_contents.Value = fInfo.ContentHTML;
                this.Contents += fInfo.ContentHTML;     //?
                if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
                {
                    NonActiveX = "T";
                    /*
                    //ByFreeman 20120106 X-Free 에디터 추가  //Mod.aspx 에서 호출했는지 ModN.aspx 에서 호출했는지 확인
                    string strScript = String.Empty;
                    if (Request.Path.IndexOf("Add.aspx") > 0)
                    {
                        NonActiveX = "F";
                    }
                    else
                    {   //스크립트에서 처리한다
                        NonActiveX = "T";
                    }
                    */
                }

                #endregion 내용필드

                #region 그외 기타필드

                ctbFormName.Text = GetLangName(fInfo.FormBase, fInfo.FormKo, fInfo.FormEn, fInfo.FormJa, fInfo.FormFr, fInfo.FormZhcn, fInfo.FormZhtw, PreLang);
                ctbNameCode.Text = fInfo.NameCode;
                rblIsActive.SelectedValue = fInfo.IsActive;

                #endregion 그외 기타필드

                #region 에디터 본문 이미지 첨부 경로

                //string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                //System.Guid guid = System.Guid.NewGuid();
                //string reqTime = Convert.ToDateTime(qInfo.WriteDate).ToString("yyyyMM");
                //UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Opinion/Ques/" + reqTime + "/editor/" + guid.ToString();

                #endregion 에디터 본문 이미지 첨부 경로
            }
        }

        #endregion (함수)

        #region (프로퍼티)

        /// <summary>
        /// 에디터-파일처리경로
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

        #endregion (프로퍼티)

        #region (이벤트핸들러)

        /// <summary>
        /// [확인][삭제]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool rtnval = false;
                switch (EventArgs.ToUpper())
                {
                    case "ADD":
                        rtnval = Addition();
                        break;

                    case "DEL":
                        rtnval = Del();
                        break;
                }

                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_SUCCESS"), "Add.SubmitClose();");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FAIL"), "Add.SubmitClose();");
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FAIL"), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParam(); //파라미터처리

            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion (이벤트핸들러)
    }
}