using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Common.Popup33
{
    public partial class ViewTot : Base.BasePageApproval
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        public int DocumentID;
        public string SRH = "";
        public bool IsAuth = true;
        public string ApprovalState = "";
        public string FolderStatusID = "";
        public string CabinetID = "";
        public int ForwardDocumentID;
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
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Popup33.ViewTot).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            // 권한 체크
            IsAuth = GetIsAuthDoc(DocumentID);
            if (IsAuth)
            {
                //전달리스트에서 왔을 경우 보기
                cbtnOpinion.Visible = (ForwardDocumentID == 0) ? false : true;

                // 문서정보 가져오기
                GetDocInfo(DocumentID);

                // 참조문서 날짜 업데이트
                UpdConfirmDateCA();

                // 전달문서함이면 문서확인
                if (CabinetID.IndexOf("Forward") > -1)
                {
                    UpdConfirmDate();
                }

                IsImgExist();
            }
        }

        #endregion

        #region (함수)
        /// <summary>
        /// 권한체크
        /// </summary>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private bool GetIsAuthDoc(int _documentID)
        {
            bool rtnval = true;
            if (!SRH.Equals(""))
            {
                DAL.Document dal = new DAL.Document();
                rtnval = dal.GetIsAuthDoc(CurrentUserInfo.CompanyID, _documentID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, CurrentUserInfo.UserID);

                // 겸직부서의 문서권한도 체크
                if (!rtnval)
                {
                    int deptID = 0;
                    Zeospace.DeskPlus.Account.DAL.User biz = new Account.DAL.User();
                    List<Zeospace.DeskPlus.Account.BE.UserInfo> al = biz.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, false);

                    foreach (Zeospace.DeskPlus.Account.BE.UserInfo be in al)
                    {
                        if (CurrentUserInfo.DeptID != be.DeptID)
                        {
                            deptID = be.DeptID;
                            rtnval = dal.GetIsAuthDoc(CurrentUserInfo.CompanyID, _documentID, deptID, be.CodeTitleID, CurrentUserInfo.UserID);

                            if (rtnval) break;
                        }
                    }
                }
            }
            return rtnval;
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
                cbtnShowImage.Text = resMng.GetString("ShowImage");   // 이미지 보기
                cbtnForward.Text = resMng.GetString("Forward");      //"전달";
                cbtnCir.Text = resMng.GetString("Circulation");      //"회람";
                cbtnMail.Text = resMng.GetString("SendMail");       //"메일발송";
                cbtnForwardList.Text = resMng.GetString("ForwardUser"); //전달현황
                cbtnPrint.Text = resMng.GetString("Print");          //인쇄
                cbtnZipFile.Text = resMng.GetString("FileBackup");  //파일백업   
                cbtnDocInfo.Text = resMng.GetString("AppInfo");     // 결재정보       
                cbtnOpinionAdd.Text = resMng.GetString("AddOpinion"); //의견추가
                cbtnBoard.Text = resMng.GetString("Post");     //게시
                cbtnPDF.Text = resMng.GetString("BatchPDF");    // PDF일괄다운로드
                cbtnZIP.Text = resMng.GetString("SelectDownLoadZIP");    // ZIP 선택다운로드
                cbtnOpinion.Text = resMng.GetString("ViewOpinion");     //"의견보기";
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            //DocumentID = GetDecryptParam("documentID", "").ToNumber<int>();
            SRH = GetDecryptParam("srh", "");   //검색페이지에서 호출
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            CabinetID = GetDecryptParam("cabinetID", "");//ID

            string strEnc = this.GetParm("enc", "1");
            if (strEnc.Equals("0"))
            {
                DocumentID = Convert.ToInt32(this.GetParm("dID", "0"));
            }
            else
            {
                DocumentID = GetDecryptParam("documentID", "0").ToNumber<int>();
            }

            ForwardDocumentID = GetDecryptParam("forwardDocumentID", "").ToNumber<int>();
        }

        /// <summary>
        /// 문서정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        public void GetDocInfo(int _documentID)
        {
            #region 문서내용
            DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
            List<BE.DocumentProcessInfo> list = dpDal.GetDocumentUnion(CurrentUserInfo.CompanyID, _documentID);
            foreach (BE.DocumentProcessInfo info in list)
            {
                ChildDocID = info.LastDocID;
                GroupID = info.GroupID;
                htaFormHTML.Value = info.ContentHTML;
                ApprovalState = info.ApprovalState;
                Number2 = info.Number2;
                AccountTag = info.AccountTag;

                AccessTitleName = GetAccessTitleInfo(info.CompanyID, info.AccessTitleID);
                IsGuardName = GetIsGuardInfo(info.IsGuard);
            }
            #endregion

            #region 근거문서/첨부파일 건수
            if (!htaFormHTML.Value.Equals(""))
            {
                DAL.AttachFile dal = new DAL.AttachFile();
                headerAttachCnt.InnerText = dal.GetAttachFileCntByPath(CurrentUserInfo.CompanyID, ChildDocID).ToString();

                DAL.BaseDocument dalBase = new DAL.BaseDocument();
                headerBaseDocCnt.InnerText = dalBase.GetBaseDocCntByPath(CurrentUserInfo.CompanyID, ChildDocID).ToString();
            }
            else
            {
                headerAttachCnt.InnerText = "0";
                headerBaseDocCnt.InnerText = "0";
            }
            #endregion

            if (AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBatchPDF", "0").Equals("1") && !headerAttachCnt.InnerText.Equals("0"))
            {
                cbtnPDF.Visible = true;
            }
            else
            {
                cbtnPDF.Visible = false;
            }
        }

        // 열람권한
        public string GetAccessTitleInfo(int _companyID, int _titleID)
        {
            if (_companyID.Equals(0) || _titleID.Equals(0)) return "";
            try
            {
                Zeospace.DeskPlus.Common.DAL.Code dal = new DeskPlus.Common.DAL.Code();
                List<Zeospace.DeskPlus.Common.BE.CodeInfo> list = dal.GetCodeByID(_companyID, _titleID);

                string rtnval = "";
                if (list != null)
                {
                    foreach (Zeospace.DeskPlus.Common.BE.CodeInfo info in list)
                    {
                        rtnval = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                    }
                }
                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAccessTitleInfo(" + _companyID.ToString() + "," + _titleID.ToString() + ")", ex);
                return "";
            }
        }

        // 문서종류
        public string GetIsGuardInfo(string _isGuard)
        {
            //ddlGuard.Items.Add(new ListItem(this.resMng.GetString("Normal"), "0"));
            //ddlGuard.Items.Add(new ListItem(this.resMng.GetString("Security"), "1"));
            //ddlGuard.Items.Add(new ListItem(this.resMng.GetString("Emergency"), "2"));

            if (_isGuard.Equals("0"))
            {
                return this.resMng.GetString("Normal");
            }
            else if (_isGuard.Equals("1"))
            {
                return this.resMng.GetString("Security");
            }
            else if (_isGuard.Equals("2"))
            {
                return this.resMng.GetString("Emergency");
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 첨부파일에 이미지가 없으면 이미지보기 버튼 감춤
        /// </summary>
        private void IsImgExist()
        {
            // 부모 문서번호들 가져오기
            string upPathID = "";
            if (ApprovalState.ToUpper().Equals("C") || ApprovalState.ToUpper().Equals("R"))
            {
                if (ApprovalState.ToUpper().Equals("R") && AccountTag.IndexOf("ZP").Equals(0))
                {
                    DAL.DocumentProcess proc = new DAL.DocumentProcess();
                    upPathID = proc.GetUpPathID(CurrentUserInfo.CompanyID, ChildDocID);
                }
                else
                {
                    DAL.Document dalDoc = new DAL.Document();
                    upPathID = dalDoc.GetUpPathID(CurrentUserInfo.CompanyID, ChildDocID);
                }
            }
            else
            {
                DAL.DocumentProcess proc = new DAL.DocumentProcess();
                upPathID = proc.GetUpPathID(CurrentUserInfo.CompanyID, ChildDocID);
            }
            upPathID = upPathID.TrimStart(',');
            string[] arrUpPathID = upPathID.Split(',');

            int imgCnt = 0;
            // 부모문서 갯수만큼 반복    
            for (int i = 0; i < arrUpPathID.Length; i++)
            {
                DAL.AttachFile dal = new DAL.AttachFile();
                List<BE.AttachFileInfo> al = dal.GetAttachImageLists(CurrentUserInfo.CompanyID, arrUpPathID[i].ToNumber<int>());

                imgCnt += al.Count;
            }

            if (imgCnt == 0) cbtnShowImage.Visible = false;
        }

        /// <summary>
        /// 전달문서 읽은날짜 업데이트
        /// </summary>
        public void UpdConfirmDate()
        {
            DAL.ForwardUser dpDal = new DAL.ForwardUser();
            bool confirm = dpDal.ConfirmDoc(CurrentUserInfo.CompanyID, DocumentID, CurrentUserInfo.UserID);
        }

        /// <summary>
        /// 참조문서 읽은날짜 업데이트
        /// </summary>
        public void UpdConfirmDateCA()
        {
            DAL.CarbonCopyUser dpDal = new DAL.CarbonCopyUser();
            bool confirm = dpDal.UpdViewingDate(CurrentUserInfo.CompanyID, DocumentID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);
        }

        /// <summary>
        /// 결재의견 추가
        /// </summary>
        /// <returns></returns>
        public bool Addition()
        {
            bool rtnval = false;

            if (DocumentID > 0 && GroupID > 0 && !AccountTag.Equals(""))
            {
                BE.DocumentInfo dpInfo = new BE.DocumentInfo();
                dpInfo.CompanyID = CurrentUserInfo.CompanyID;
                dpInfo.DocumentID = DocumentID;
                dpInfo.GroupID = GroupID;
                dpInfo.AccountTag = AccountTag;
                dpInfo.ContentHTML = htaFormHTML.Value;
                dpInfo.ContentTEXT = htaFormTEXT.Value;

                DAL.Document docDal = new DAL.Document();
                rtnval = docDal.ModContentHTML(dpInfo);

                #region 업무톡연동
                if (rtnval && !tbOpinion.Text.Equals(""))
                {
                    // 업무톡연동
                    BE.ShortReplyInfo be = new BE.ShortReplyInfo();
                    be.CompanyID = CurrentUserInfo.CompanyID;
                    be.DocumentID = DocumentID;
                    be.Section = "2";
                    be.Content = tbOpinion.Text;
                    be.UserID = CurrentUserInfo.UserID;
                    be.IsOpen = "1";

                    DAL.ShortReply dal = new DAL.ShortReply();
                    rtnval = dal.Add(be);
                }
                #endregion

                #region 알람
                //의견추가시 알람발송.
                if (rtnval && !tbOpinion.Text.Equals(""))
                {
                    BE.DocumentProcessInfo docInfo = GetDocumentInfo(CurrentUserInfo.CompanyID, DocumentID);
                    if (dpInfo == null) return false;

                    //발송자정보 가져오기
                    BE.UserInfo sendInfo = GetSendUserInfo("S", tbOpinion.Text);

                    BLL.DocumentProcess biz = new BLL.DocumentProcess();
                    rtnval = biz.ApproveOpinion(docInfo, sendInfo);

                }
                #endregion
            }

            return rtnval;
        }

        /// <summary>
        /// 문서정보 생성
        /// </summary>
        /// <returns></returns>
        private BE.DocumentProcessInfo GetDocumentInfo(int _companyID, int _documentID)
        {
            if (_companyID < 0 || _documentID < 0) return null;

            BE.DocumentProcessInfo dpInfo = null;

            //문서정보가져오기
            DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
            List<BE.DocumentProcessInfo> list = dpDal.GetDocumentUnion(_companyID, _documentID);

            foreach (BE.DocumentProcessInfo info in list)
            {
                dpInfo = new BE.DocumentProcessInfo();
                dpInfo.CompanyID = info.CompanyID;
                dpInfo.DocumentID = info.DocumentID;
                dpInfo.ParentDocumentID = info.ParentDocumentID;
                dpInfo.Number1 = info.Number1;
                dpInfo.Number2 = info.Number2;
                dpInfo.Number3 = info.Number3;
                dpInfo.WriteTime = info.WriteTime;
                dpInfo.WriteTimeYMD = info.WriteTimeYMD;
                dpInfo.EmployeeNumber = info.EmployeeNumber;
                dpInfo.DeptCodeID = info.DeptCodeID;

                #region 다국어처리
                dpInfo.DeptBase = info.DeptBase;
                dpInfo.DeptKo = info.DeptKo;
                dpInfo.DeptEn = info.DeptEn;
                dpInfo.DeptJa = info.DeptJa;
                dpInfo.DeptFr = info.DeptFr;
                dpInfo.DeptZhcn = info.DeptZhcn;
                dpInfo.DeptZhtw = info.DeptZhtw;
                dpInfo.TitleBase = info.TitleBase;
                dpInfo.TitleKo = info.TitleKo;
                dpInfo.TitleEn = info.TitleEn;
                dpInfo.TitleJa = info.TitleJa;
                dpInfo.TitleFr = info.TitleFr;
                dpInfo.TitleZhcn = info.TitleZhcn;
                dpInfo.TitleZhtw = info.TitleZhtw;
                dpInfo.DutyBase = info.DutyBase;
                dpInfo.DutyKo = info.DutyKo;
                dpInfo.DutyEn = info.DutyEn;
                dpInfo.DutyJa = info.DutyJa;
                dpInfo.DutyFr = info.DutyFr;
                dpInfo.DutyZhcn = info.DutyZhcn;
                dpInfo.DutyZhtw = info.DutyZhtw;
                #endregion

                dpInfo.UserID = info.UserID;
                dpInfo.NameBase = info.NameBase;
                dpInfo.NameKo = info.NameKo;
                dpInfo.NameEn = info.NameEn;
                dpInfo.NameJa = info.NameJa;
                dpInfo.NameFr = info.NameFr;
                dpInfo.NameZhcn = info.NameZhcn;
                dpInfo.NameZhtw = info.NameZhtw;
                dpInfo.ManyName = info.ManyName;
                dpInfo.LifeTime = info.LifeTime;
                dpInfo.AccessTitleID = info.AccessTitleID;
                dpInfo.AccountTag = info.AccountTag;
                dpInfo.Subject = info.Subject;
                dpInfo.IsConfirm = info.IsConfirm;
                dpInfo.IsLegacy = info.IsLegacy;
                dpInfo.CustomFileName = info.CustomFileName;
                dpInfo.IsAfterSubmitCall = info.IsAfterSubmitCall;
                dpInfo.IsDRM = info.IsDRM;
                dpInfo.IsGuard = info.IsGuard;
                dpInfo.IsLinkedEtc = info.IsLinkedEtc;
                dpInfo.IsUseForce = info.IsUseForce;
                dpInfo.IsUseFlow = info.IsUseFlow;
                dpInfo.FormID = info.FormID;
                dpInfo.FormBase = info.FormBase;
                dpInfo.FormKo = info.FormKo;
                dpInfo.FormEn = info.FormEn;
                dpInfo.FormJa = info.FormJa;
                dpInfo.FormFr = info.FormFr;
                dpInfo.FormZhcn = info.FormZhcn;
                dpInfo.FormZhtw = info.FormZhtw;
                dpInfo.FormMany = info.FormMany;

                dpInfo.UpPathID = info.UpPathID;
                dpInfo.GroupID = info.GroupID;
                dpInfo.LastDocID = info.LastDocID;


                dpInfo.ApprovalState = info.ApprovalState;
                dpInfo.ContentHTML = htaFormHTML.Value;
                dpInfo.ContentTEXT = htaFormTEXT.Value;
                dpInfo.Etc1 = info.Etc1;
                dpInfo.Etc2 = info.Etc2;
                dpInfo.Etc3 = info.Etc3;
                dpInfo.Etc4 = info.Etc4;
                dpInfo.Etc5 = info.Etc5;
                dpInfo.Etc6 = info.Etc6;
                dpInfo.Etc7 = info.Etc7;
                dpInfo.Etc8 = info.Etc8;
                dpInfo.Etc9 = info.Etc9;
                dpInfo.Etc10 = info.Etc10;

            }

            return dpInfo;
        }
        #endregion

        #region (프로퍼티)

        public int ChildDocID
        {
            get { return GetViewStateInt("ChildDocID", "0"); }
            set { ViewState["ChildDocID"] = value; }
        }
        public int GroupID
        {
            get { return GetViewStateInt("GroupID", "0"); }
            set { ViewState["GroupID"] = value; }
        }
        public string AccountTag
        {
            get { return GetViewStateString("AccountTag", ""); }
            set { ViewState["AccountTag"] = value; }
        }
        public string Number2
        {
            get { return GetViewStateString("Number2", ""); }
            set { ViewState["Number2"] = value; }
        }
        public string AccessTitleName
        {
            get { return GetViewStateString("AccessTitleName", ""); }
            set { ViewState["AccessTitleName"] = value; }
        }
        public string IsGuardName
        {
            get { return GetViewStateString("IsGuardName", ""); }
            set { ViewState["IsGuardName"] = value; }
        }

        #endregion

        #region (이벤트핸들러)
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
                }

                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_SUCCESS"), "");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_FAIL"), "View.List();");
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FAIL"), "");
            }
        }

        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParam(); //파라미터처리

            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}