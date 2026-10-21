using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Approval.Web.Process.Confirm
{
    public partial class ApprovePopup : Base.BasePageApproval
    {
        #region 변수설정
        public string ListID;
        public string FolderStatusID;
        public string EventArgs = string.Empty;

        #region User
        public int CompanyID = 0;
        public string CompanyName = "";
        public int RootDeptID = 0;
        public string LoginID = "";
        public string DeptName = "";
        public string TitleName = "";
        public int DutyID = 0;
        public string DutyName = "";
        public string EmployeeNumber = "";
        public string UserMail = "";
        public string MobilePhone = "";
        public string UserName = "";
        #endregion

        public int ChildDocID = 0;  //최종 자식문서정보
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Process.Confirm.ApprovePopup).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();

                // 결재순서 체크
                GetMyTurn(CurrentUserInfo.CompanyID, DocumentID, Sequence, ApprovalTag, CurrentUserInfo.UserID);

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
            GetUserInfo();  // 사용자정보 가져오기(+인장포함)

            GetDocSeqInfo(CurrentUserInfo.CompanyID, DocumentID, Sequence, ApprovalTag);  //문서정보 가져오기

            IsImgExist();
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
                cbtnPrint.Text = resMng.GetString("Print");     //인쇄
                cbtnHistory.Text = resMng.GetString("ModHistory"); //수정내역
                cbtnDocInfo.Text = resMng.GetString("AppInfo"); // 결재정보
                cbtnConfirm.Text = resMng.GetString("DocConfirm");  //문서확인
                cbtnShowImage.Text = resMng.GetString("ShowImage");   // 이미지 보기
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
            ListID = GetDecryptParam("listID", "");
            FolderStatusID = GetDecryptParam("folderStatusID", "");

            DocumentID = GetDecryptParam("documentID", "0").ToNumber<int>();
            Sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
            ApprovalTag = GetDecryptParam("approvalTag", "");
            AccountTag = GetDecryptParam("accountTag", "");
            ApprovalState = GetDecryptParam("approvalState", "");

            if (ApprovalState.Equals(""))
            {
                DAL.Document dal = new DAL.Document();
                DataTableReader dtr = dal.GetDocTot(CurrentUserInfo.CompanyID, DocumentID);
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ApprovalState = dtr["ApprovalState"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 사용자정보 가져오기 (인장포함)
        /// </summary>
        private void GetUserInfo()
        {
            CompanyID = CurrentUserInfo.CompanyID;
            RootDeptID = CurrentUserInfo.RootDeptID;
            UserID = CurrentUserInfo.UserID;
            LoginID = CurrentUserInfo.LogInID;
            UserName = CurrentUserInfo.Name;
            EmployeeNumber = CurrentUserInfo.EmployeeNumber;
            UserMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
            MobilePhone = CurrentUserInfo.MobilePhone;
            TitleName = CurrentUserInfo.TitleName;
            DeptName = CurrentUserInfo.DeptName;

            Zeospace.DeskPlus.Account.DAL.User uDAL = new Account.DAL.User();
            List<Account.BE.UserInfo> userlist = uDAL.GetUserInfo(CompanyID, UserID, true, true);
            foreach (Account.BE.UserInfo info in userlist)
            {
                CompanyName = GetLangName(info.ComBase, info.ComKo, info.ComEn, info.ComJa, info.ComFr, info.ComZhcn, info.ComZhtw, PreLang);

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string path = baseFileDirectory + CompanyID.ToString() + "/Users/" + UserID.ToString() + "/Picture/" + info.SealImage;
                if (File.Exists(Server.MapPath(path)))
                {
                    SealPath = FullDomainPath + path;
                }
            }
        }

        /// <summary>
        /// 문서정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        private void GetDocSeqInfo(int _companyID, int _documentID, int _sequence, string _approvalTag)
        {
            if (_companyID < 0 || _documentID < 0) return;

            // ps. 문서 viewing여부및 viewing check

            // 문서정보 & GroupID
            if (ApprovalState.Equals("C"))
            {
                #region 문서정보(완료)
                DAL.Document dDal = new DAL.Document();
                List<BE.DocumentInfo> list = dDal.GetDocList(_companyID, _documentID);
                foreach (BE.DocumentInfo info in list)
                {
                    FormID = info.FormID;
                    GroupID = info.GroupID;
                    AccountTag = info.AccountTag;
                    ApprovalState = info.ApprovalState;
                    htaFormHTML.Value = info.ContentHTML;           //.Replace("Func_ApprovalRegistForm.css", "Func_ApprovalEdit.css");
                    htaFormTEXT.Value = info.ContentTEXT;
                    ChildDocID = info.LastDocID;

                    tbEtc1.Text = info.Etc1;
                    tbEtc2.Text = info.Etc2;
                    tbEtc3.Text = info.Etc3;
                    tbEtc4.Text = info.Etc4;
                    tbEtc5.Text = info.Etc5;
                    tbEtc6.Text = info.Etc6;
                    tbEtc7.Text = info.Etc7;
                    tbEtc8.Text = info.Etc8;
                    tbEtc9.Text = info.Etc9;
                    tbEtc10.Text = info.Etc10;
                }
                #endregion
            }
            else
            {
                #region 문서정보(진행중)

                DAL.DocumentProcess dpDal = new DAL.DocumentProcess();

                List<BE.DocumentProcessInfo> list = dpDal.GetDocument(_companyID, _documentID);
                foreach (BE.DocumentProcessInfo info in list)
                {
                    FormID = info.FormID;
                    GroupID = info.GroupID;
                    AccountTag = info.AccountTag;
                    ApprovalState = info.ApprovalState;
                    htaFormHTML.Value = info.ContentHTML;           //.Replace("Func_ApprovalRegistForm.css", "Func_ApprovalEdit.css");
                    htaFormTEXT.Value = info.ContentTEXT;
                    ChildDocID = info.LastDocID;

                    tbEtc1.Text = info.Etc1;
                    tbEtc2.Text = info.Etc2;
                    tbEtc3.Text = info.Etc3;
                    tbEtc4.Text = info.Etc4;
                    tbEtc5.Text = info.Etc5;
                    tbEtc6.Text = info.Etc6;
                    tbEtc7.Text = info.Etc7;
                    tbEtc8.Text = info.Etc8;
                    tbEtc9.Text = info.Etc9;
                    tbEtc10.Text = info.Etc10;
                }

                #endregion
            }
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

                string upPathID = string.Empty;

                if (ApprovalState.Equals("C"))
                {
                    DAL.Document doc = new DAL.Document();
                    upPathID = doc.GetUpPathID(CurrentUserInfo.CompanyID, ChildDocID);
                }
                else
                {// 부모 문서번호들 가져오기
                    DAL.DocumentProcess dalDoc = new DAL.DocumentProcess();
                    upPathID = dalDoc.GetUpPathID(CurrentUserInfo.CompanyID, ChildDocID);
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
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("companyID:" + CurrentUserInfo.CompanyID.ToString() + ",documentID:" + ChildDocID.ToString() + "- 삭제된 문서입니다." + ex.Message);
            }
        }

        /// <summary>
        /// 현재 결재여부 확인
        /// </summary>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        private void GetMyTurn(int _companyID, int _documentID, int _sequence, string _approvalTag, int _userID)
        {
            DAL.Sequence seqDal = new DAL.Sequence();
            if (seqDal.GetIsApprove(_companyID, _documentID, _sequence, _approvalTag, _userID).Equals("N"))
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("companyID", _companyID.ToString());
                paraDic.Add("documentID", _documentID.ToString());

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                string goPage = "/DeskplusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + encParams;
                Response.Redirect(goPage, false);
            }
        }
        #endregion

        /// <summary>
        /// 결재자정보 생성
        /// </summary>
        /// <param name="_approvalType"></param>
        /// <returns></returns>
        private BE.SequenceInfo GetSeqInfo(string _approvalType)
        {
            if (_approvalType.Equals("")) return null;

            BE.SequenceInfo seqInfo = new BE.SequenceInfo();

            seqInfo.CompanyID = CurrentUserInfo.CompanyID;
            seqInfo.DocumentID = DocumentID;
            seqInfo.Sequence = Sequence;
            seqInfo.ApprovalTag = ApprovalTag;
            seqInfo.UserID = CurrentUserInfo.UserID;
            seqInfo.DeptID = CurrentUserInfo.DeptID;
            seqInfo.ApprovalType = _approvalType;
            seqInfo.Opinion = tbOpinion.Text;
            seqInfo.Proxy = "N";
            seqInfo.ProxyUserID = 0;

            return seqInfo;
        }

        /// <summary>
        /// 문서정보 생성
        /// </summary>
        /// <returns></returns>
        private BE.DocumentProcessInfo GetDocProcessInfo(int _companyID, int _documentID)
        {
            if (_companyID < 0 || _documentID < 0) return null;

            BE.DocumentProcessInfo dpInfo = null;

            //문서정보가져오기
            DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
            List<BE.DocumentProcessInfo> list = dpDal.GetDocument(_companyID, _documentID);

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

                // 변경되는 값 할당.
                dpInfo.ApprovalState = "I";
                dpInfo.ContentHTML = htaFormHTML.Value;
                dpInfo.ContentTEXT = htaFormTEXT.Value;
                dpInfo.Etc1 = tbEtc1.Text;
                dpInfo.Etc2 = tbEtc2.Text;
                dpInfo.Etc3 = tbEtc3.Text;
                dpInfo.Etc4 = tbEtc4.Text;
                dpInfo.Etc5 = tbEtc5.Text;
                dpInfo.Etc6 = tbEtc6.Text;
                dpInfo.Etc7 = tbEtc7.Text;
                dpInfo.Etc8 = tbEtc8.Text;
                dpInfo.Etc9 = tbEtc9.Text;
                dpInfo.Etc10 = tbEtc10.Text;
            }

            return dpInfo;
        }
        /// <summary>
        /// 문서정보 생성
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private BE.DocumentInfo GetDocInfo(int _companyID, int _documentID)
        {
            if (_companyID < 0 || _documentID < 0) return null;

            BE.DocumentInfo dpInfo = null;

            //문서정보가져오기
            DAL.Document dpDal = new DAL.Document();
            List<BE.DocumentInfo> list = dpDal.GetDocList(_companyID, _documentID);

            foreach (BE.DocumentInfo info in list)
            {
                dpInfo = new BE.DocumentInfo();
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

                // 변경되는 값 할당.
                dpInfo.ApprovalState = info.ApprovalState;
                dpInfo.ContentHTML = htaFormHTML.Value;
                dpInfo.ContentTEXT = htaFormTEXT.Value;
                dpInfo.Etc1 = tbEtc1.Text;
                dpInfo.Etc2 = tbEtc2.Text;
                dpInfo.Etc3 = tbEtc3.Text;
                dpInfo.Etc4 = tbEtc4.Text;
                dpInfo.Etc5 = tbEtc5.Text;
                dpInfo.Etc6 = tbEtc6.Text;
                dpInfo.Etc7 = tbEtc7.Text;
                dpInfo.Etc8 = tbEtc8.Text;
                dpInfo.Etc9 = tbEtc9.Text;
                dpInfo.Etc10 = tbEtc10.Text;
            }

            return dpInfo;
        }

        /// <summary>
        /// 문서확인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private bool DocConfirm(int _companyID, int _documentID)
        {
            BE.DocumentInfo dpInfo = null;
            BE.DocumentProcessInfo procinfo = null;

            if (ApprovalState.Equals("C"))
            {
                dpInfo = GetDocInfo(_companyID, _documentID);
            }
            else
            {
                procinfo = GetDocProcessInfo(_companyID, _documentID);
            }

            BE.SequenceInfo seqInfo = GetSeqInfo("C");
            if (seqInfo == null) return false;

            BE.UserInfo sendInfo = GetSendUserInfo("C", tbOpinion.Text);

            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            return dpBll.ApproveConfirm(procinfo, dpInfo, seqInfo, sendInfo, ApprovalState);
        }

        #region (프로퍼티)

        #region User
        /// <summary>
        /// 사용자아이디
        /// </summary>
        public int UserID
        {
            get { return GetViewStateInt("UserID", "0"); }
            set { ViewState["UserID"] = value; }
        }
        /// <summary>
        /// 직급아이디
        /// </summary>
        public int TitleID
        {
            get { return GetViewStateInt("TitleID", "0"); }
            set { ViewState["TitleID"] = value; }
        }
        /// <summary>
        /// 부서아이디
        /// </summary>
        public int DeptID
        {
            get { return GetViewStateInt("DeptID", "0"); }
            set { ViewState["DeptID"] = value; }
        }
        /// <summary>
        /// 인장경로
        /// </summary>
        public string SealPath
        {
            get { return GetViewStateString("SealPath", ""); }
            set { ViewState["SealPath"] = value; }
        }
        #endregion

        #region DocSeqInfo

        public int DocumentID
        {
            get { return GetViewStateInt("DocumentID", "0"); }
            set { ViewState["DocumentID"] = value; }
        }
        public int GroupID
        {
            get { return GetViewStateInt("GroupID", "0"); }
            set { ViewState["GroupID"] = value; }
        }
        public int Sequence
        {
            get { return GetViewStateInt("Sequence", "0"); }
            set { ViewState["Sequence"] = value; }
        }
        public string ApprovalTag
        {
            get { return GetViewStateString("ApprovalTag", ""); }
            set { ViewState["ApprovalTag"] = value; }
        }
        public string AccountTag
        {
            get { return GetViewStateString("AccountTag", ""); }
            set { ViewState["AccountTag"] = value; }
        }
        public string ApprovalState
        {
            get { return GetViewStateString("ApprovalState", ""); }
            set { ViewState["ApprovalState"] = value; }
        }
        public int FormID
        {
            get { return GetViewStateInt("FormID", "0"); }
            set { ViewState["FormID"] = value; }
        }

        #endregion

        #endregion

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
                bool rtnval = DocConfirm(CurrentUserInfo.CompanyID, DocumentID);

                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_SUCCESS"), "Approve.List();");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_FAIL"), "Approve.List();");
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
        #endregion
    }
}