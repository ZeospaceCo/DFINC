using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Common
{
    public partial class View : Base.BasePageApproval
    {
        #region 변수설정
        public string EventArgs = string.Empty;

        public int DocumentID;
        public int ForwardDocumentID;
        public string FolderStatusID;
        public string CabinetID;
        public string FormID;
        public string YYYY;
        public string YYYYMM;
        public string Types;//S:기본폴더; U : 수발자 관리자 추가 폴더
        public string HeaderName;

        public string IsReject = "N";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Common.View).Assembly;	//어셈블리 선언
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

            //전달리스트에서 왔을 경우 보기
            cbtnOpinion.Visible = (ForwardDocumentID == 0) ? false : true;

            #region 문서함별 버튼 컨트롤

            if (CabinetID == "Dept_Complete")
            {
                //수신완료문서함
                cbtnForward.Visible = false;//전달
                cbtnPassed.Visible = false;//이관
            }
            else if (CabinetID.IndexOf("Private") != -1
                || CabinetID.IndexOf("Approval") != -1
                || CabinetID.IndexOf("Arbitrary") != -1
                || CabinetID.IndexOf("Impo") != -1
                )
            {
                //상신문서함, 결재문서함, 전결문서함                
                cbtnPassed.Visible = false; //이관
            }
            else if (CabinetID.IndexOf("Carboncopy") != -1)
            {
                if (CabinetID == "Carboncopy" || CabinetID == "Carboncopy_Receive" ||
                    CabinetID == "Carboncopy_Doc" || CabinetID.IndexOf("CarboncopyDoc_") != -1 ||
                    CabinetID == "CarboncopyReceiveAll" ||
                    CabinetID == "Carboncopy_Forward" || CabinetID == "Carboncopy_ForwardReceive" ||
                    CabinetID.IndexOf("CarboncopyForward_") != -1 ||
                    CabinetID == "CarboncopyForwardReceiveAll")
                {
                    //참조문서함, 참조문서함양식, 전달문서함, 전달문서함양식
                    cbtnPassed.Visible = false; //이관

                    //전달문서함이면 문서확인
                    if (CabinetID == "Carboncopy_Forward" || CabinetID == "Carboncopy_ForwardReceive" ||
                        CabinetID.IndexOf("CarboncopyForward_") != -1 ||
                        CabinetID == "CarboncopyForwardReceiveAll")
                        UpdConfirmDate();

                    //참조문서함이면 문서확인
                    if (CabinetID == "Carboncopy" || CabinetID == "Carboncopy_Receive" ||
                        CabinetID == "Carboncopy_Doc" || CabinetID.IndexOf("CarboncopyDoc_") != -1 ||
                        CabinetID == "CarboncopyReceiveAll")
                        UpdConfirmDateCA();

                }
                else if (CabinetID == "Carboncopy_Forward" || CabinetID.IndexOf("CarboncopyForward_") != -1)
                {
                    //전달문서함, 전달문서함양식
                    cbtnPassed.Visible = false; //이관   
                    UpdConfirmDate();
                }
                else if (CabinetID == "Carboncopy_CirReceipt" || CabinetID.IndexOf("CarboncopyCirReceipt_") != -1)
                {
                    //받은회람함, 받은회람함양식
                    cbtnForward.Visible = false;//전달
                    cbtnPassed.Visible = false;//이관
                    cbtnCir.Visible = false;//회람
                }
                else if (CabinetID == "Carboncopy_CirSend" || CabinetID.IndexOf("CarboncopyCirSend_") != -1)
                {
                    //보낸회람함,보낸회람함양식
                    cbtnForward.Visible = false;//전달
                    cbtnPassed.Visible = false;//이관
                    cbtnCir.Visible = false;//회람
                }
            }

            #endregion

            // 2-1. 버튼 정의 
            // EDM 버튼
            string IsFolderToEDMS = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsFolderToEDMS", "0");

            if (IsFolderToEDMS != "1")
                cbtnEDM.Visible = false;

            // 문서정보 가져오기
            GetDocInfo();
            IsImgExist();

            cbtnPDF.Visible = (AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBatchPDF", "0").Equals("1")) ? true : false;
        }

        #endregion

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
                cbtnShowImage.Text = resMng.GetString("ShowImage");   // 이미지 보기
                cbtnForward.Text = resMng.GetString("Forward");      //"전달";
                cbtnPassed.Text = resMng.GetString("Transfer");      //"이관";
                cbtnCir.Text = resMng.GetString("Circulation");      //"회람";
                cbtnMail.Text = resMng.GetString("SendMail");       //"메일발송";
                cbtnForwardList.Text = resMng.GetString("ForwardUser"); //전달현황
                cbtnList.Text = resMng.GetString("List");            //"목록";
                cbtnOpinion.Text = resMng.GetString("ViewOpinion");     //"의견보기";
                cbtnPrint.Text = resMng.GetString("Print");          //인쇄
                cbtnZipFile.Text = resMng.GetString("FileBackup");  //파일백업   
                cbtnHistory.Text = resMng.GetString("ModHistory");  //"수정내역";
                cbtnDocInfo.Text = resMng.GetString("AppInfo");     // 결재정보       
                cbtnOpinionAdd.Text = resMng.GetString("AddOpinion"); //의견추가
                cbtnEDM.Text = this.GetResource("SendEDM");     // EDMS 이관
                cbtnBoard.Text = resMng.GetString("Post");     //게시
                cbtnPDF.Text = resMng.GetString("BatchPDF");    // PDF일괄다운로드
                cbtnZIP.Text = resMng.GetString("SelectDownLoadZIP");    // ZIP 선택다운로드
                cbtnOpinionInfo.Text = resMng.GetString("Opinion"); //결재의견
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            DocumentID = GetDecryptParam("documentID", "").ToNumber<int>();
            ForwardDocumentID = GetDecryptParam("forwardDocumentID", "").ToNumber<int>();
            FolderStatusID = GetDecryptParam("folderStatusID", "");

            CabinetID = GetDecryptParam("cabinetID", "");//ID
            Types = GetDecryptParam("types", ""); //S(SYSTEM, U:USER)
            FormID = GetDecryptParam("formID", ""); //양식id
            YYYY = GetDecryptParam("yyyy", ""); //년도
            YYYYMM = GetDecryptParam("yyyymm", ""); //년월 
            HeaderName = GetDecryptParam("headerName", "");//리스트 해더 경로(동적은 없으므로 파리미터 날림)

            IsReject = GetDecryptParam("isreject", "N");
        }

        /// <summary>
        /// 문서정보 가져오기
        /// </summary>
        public void GetDocInfo()
        {
            DAL.Document dpDal = new DAL.Document();
            List<BE.DocumentInfo> list = dpDal.GetDocList(CurrentUserInfo.CompanyID, DocumentID);
            foreach (BE.DocumentInfo info in list)
            {
                ChildDocID = info.LastDocID;
                GroupID = info.GroupID;
                AccountTag = info.AccountTag;
                htaFormHTML.Value = info.ContentHTML;
                htaFormTEXT.Value = info.ContentTEXT;
                Number2 = info.Number2;

                AccessTitleName = GetAccessTitleInfo(info.CompanyID, info.AccessTitleID);
                IsGuardName = GetIsGuardInfo(info.IsGuard);
                IsCarbonView = (info.CarbonView == "1") ? this.resMng.GetString("Use") : this.resMng.GetString("NotUse");
            }
        }


        // 열람권한
        public string GetAccessTitleInfo(int _companyID, int _titleID)
        {
            if (_companyID.Equals(0) || _titleID.Equals(0)) return "";
            try
            {
                string rtnval = "";

                if (_titleID >= 10000)
                {
                    DAL.AccessGrade biz = new DAL.AccessGrade();

                    DataTableReader dtr = biz.GetDataByAccessID(CurrentUserInfo.CompanyID, _titleID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                rtnval = this.GetLangName(
                                        dtr["NameBase"].ToString(),
                                        dtr["NameKo"].ToString(),
                                        dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                        dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(),
                                        dtr["NameZhTw"].ToString(), PreLang);

                            }
                        }
                    }
                }
                else
                {
                    Zeospace.DeskPlus.Common.DAL.Code dal = new DeskPlus.Common.DAL.Code();
                    List<Zeospace.DeskPlus.Common.BE.CodeInfo> list = dal.GetCodeByID(_companyID, _titleID);


                    if (list != null)
                    {
                        foreach (Zeospace.DeskPlus.Common.BE.CodeInfo info in list)
                        {
                            rtnval = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                        }
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
        /// 첨부파일에 이미지가 없으면 이미지보기 버튼 감춤
        /// </summary>
        private void IsImgExist()
        {
            try
            {
                if (CurrentUserInfo.CompanyID.Equals(0) || ChildDocID.Equals(0))
                {
                    return;
                }

                // 부모 문서번호들 가져오기
                DAL.Document dalDoc = new DAL.Document();
                string upPathID = dalDoc.GetUpPathID(CurrentUserInfo.CompanyID, ChildDocID);
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
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("companyID:" + CurrentUserInfo.CompanyID.ToString() + ",documentID:" + ChildDocID.ToString() + "- 삭제된 문서입니다." + ex.Message);
            }
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
        public string IsCarbonView
        {
            get { return GetViewStateString("IsCarbonView", ""); }
            set { ViewState["IsCarbonView"] = value; }
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
