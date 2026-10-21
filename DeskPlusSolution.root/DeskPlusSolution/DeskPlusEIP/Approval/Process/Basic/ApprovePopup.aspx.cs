using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Process.Basic
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
        public string OrginUserName = "";
        public string OrginDeptName = "";
        public string OrginTitleName = "";
        public string OrginDutyName = "";
        public string OrginSealPath = "";
        #endregion

        public int ChildDocID = 0;  //최종 자식문서정보

        public string FuncCall = string.Empty;

        /// <summary>
        /// 전자결재 상태조회 신/구메뉴 개인설정
        /// </summary>
        public string ApprovalStatusMenuType
        {
            get
            {
                try
                {
                    Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
                    List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = dal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                    if (list != null && list.Count > 0 && "NEW".Equals(list[0].ApprovalStatusMenuType, StringComparison.OrdinalIgnoreCase))
                    {
                        return "NEW";
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                }
                return "OLD";
            }
        }
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Process.Basic.ApprovePopup).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();

                // 결재순서 체크
                GetMyTurn(CurrentUserInfo.CompanyID, DocumentID, Sequence, ApprovalTag, TargetUserID);

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
                cbtnApproveH.Text = resMng.GetString("ZA"); // "합의";
                cbtnApproveHR.Text = resMng.GetString("NotZA"); // "합의불가";
                cbtnApproveS.Text = resMng.GetString("Approve"); // "승인";
                cbtnApproveA.Text = resMng.GetString("ApproveAll"); // "전결";
                cbtnApproveR.Text = resMng.GetString("StatusR"); // "반려";
                cbtnApproveD.Text = resMng.GetString("StatusD"); // "보류";
                cbtnApproveDC.Text = resMng.GetString("DelayCancel"); // "보류취소";
                cbtnShowImage.Text = resMng.GetString("ShowImage");   // 이미지 보기
                cbtnApproveSC.Text = resMng.GetString("ApproveCancel"); //결재취소
                cbtnCir.Text = resMng.GetString("Circulation"); //회람
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
            TargetUserID = GetDecryptParam("targetUserID", "0").ToNumber<int>();
            TargetDeptID = GetDecryptParam("targetDeptID", "0").ToNumber<int>();
            ProxyUserID = GetDecryptParam("proxyUserID", "0").ToNumber<int>();

            FuncCall = GetDecryptParam("func", "");
        }

        /// <summary>
        /// 사용자정보 가져오기 (인장포함)
        /// </summary>
        private void GetUserInfo()
        {
            CompanyID = CurrentUserInfo.CompanyID;
            //RootDeptID = CurrentUserInfo.RootDeptID;
            UserID = CurrentUserInfo.UserID;
            LoginID = CurrentUserInfo.LogInID;
            UserName = CurrentUserInfo.Name;
            EmployeeNumber = CurrentUserInfo.EmployeeNumber;
            UserMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
            MobilePhone = CurrentUserInfo.MobilePhone;

            /*
            TitleID = CurrentUserInfo.CodeTitleID;
            TitleName = CurrentUserInfo.TitleName;
            DeptID = CurrentUserInfo.DeptID;
            DeptName = CurrentUserInfo.DeptName;
            DutyID = CurrentUserInfo.CodeDutyID;
            DutyName = CurrentUserInfo.DutyName;
            */

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

            #region 원결재자정보
            DAL.Sequence seqDal = new DAL.Sequence();
            List<BE.SequenceInfo> seqlist = seqDal.GetSeq(CompanyID, DocumentID, Sequence, ApprovalTag);
            foreach (BE.SequenceInfo info in seqlist)
            {
                #region 이름 다국어 설정
                OrginUserName = this.GetLangName(
                        info.NameBase.ToNull(), info.NameKo.ToNull(),
                        info.NameEn.ToNull(), info.NameJa.ToNull(),
                        info.NameFr.ToNull(), info.NameZhcn.ToNull(),
                        info.NameZhtw.ToNull(), PreLang);
                #endregion

                #region 부서 다국어 설정
                OrginDeptName = this.GetLangName(
                        info.DeptBase.ToNull(), info.DeptKo.ToNull(),
                        info.DeptEn.ToNull(), info.DeptJa.ToNull(),
                        info.DeptFr.ToNull(), info.DeptZhcn.ToNull(),
                        info.DeptZhtw.ToNull(), PreLang);
                #endregion

                #region 직급 다국어 설정
                OrginTitleName = this.GetLangName(
                        info.TitleBase.ToNull(), info.TitleKo.ToNull(),
                        info.TitleEn.ToNull(), info.TitleJa.ToNull(),
                        info.TitleFr.ToNull(), info.TitleZhcn.ToNull(),
                        info.TitleZhtw.ToNull(), PreLang);
                #endregion

                #region 직책 다국어 설정
                OrginDutyName = this.GetLangName(
                        info.DutyBase.ToNull(), info.DutyKo.ToNull(),
                        info.DutyEn.ToNull(), info.DutyJa.ToNull(),
                        info.DutyFr.ToNull(), info.DutyZhcn.ToNull(),
                        info.DutyZhtw.ToNull(), PreLang);
                #endregion

                List<Account.BE.UserInfo> userlist2 = uDAL.GetUserInfo(CompanyID, info.UserID, true, true);
                foreach (Account.BE.UserInfo info2 in userlist2)
                {
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string path = baseFileDirectory + CompanyID.ToString() + "/Users/" + info.UserID.ToString() + "/Picture/" + info2.SealImage;
                    if (File.Exists(Server.MapPath(path)))
                    {
                        OrginSealPath = FullDomainPath + path;
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 문서정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        private void GetDocSeqInfo(int _companyID, int _documentID, int _sequence, string _approvalTag)
        {
            if (_companyID < 0 || _documentID < 0 || _sequence < 0 || _approvalTag.Equals("")) return;

            // ps. 문서 viewing여부및 viewing check

            // 문서정보 & GroupID
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

                #region 연동정보
                // AccountTag별 로구분할 필요가 있을런지, 확인이 필요한 걸로.
                if (!info.CustomFileName.Equals("") && info.IsLegacy.Equals("1"))
                {
                    string[] fileArr = info.CustomFileName.Split('.');
                    CustomFileName = fileArr[0] + "_Complete." + fileArr[1];
                }
                else
                {
                    CustomFileName = "";
                }
                #endregion

            }

            // 결재라인정보
            DAL.Sequence seqDal = new DAL.Sequence();

            // 최종결재 여부, 반려가능수신(ZQ) 여부
            if (_approvalTag.IndexOf("ZA") < 0)
            {
                string[] arrval = seqDal.GetNextCnt(_companyID, _documentID, GroupID, _sequence, _approvalTag).Split('-');
                IsLast = arrval[0].Equals("0") ? "Y" : "N";
                IsZQ = arrval[1].Equals("0") ? "N" : "Y";
            }
            else
            {
                IsLast = "N";
                IsZQ = "N";
            }

            //최종결재자인 경우에는 전결버튼 안 보이게 처리
            if (IsLast == "Y")
            {
                cbtnApproveA.Visible = false;
            }

            if ((TargetUserID.Equals(CurrentUserInfo.UserID)) && (!TargetDeptID.Equals(CurrentUserInfo.DeptID)))
            {
                bool rtnDept = false;
                for (int i = 0; i < CurrentUserInfo.UserDepts.Length; i++)
                {
                    if (CurrentUserInfo.UserDepts[i].ToString() == TargetDeptID.ToString())
                    {
                        rtnDept = true;
                    }
                }

                if (rtnDept)
                {
                    //겸직일 경우
                    Zeospace.DeskPlus.Account.DAL.User uDal = new Zeospace.DeskPlus.Account.DAL.User();
                    List<Zeospace.DeskPlus.Account.BE.UserInfo> ulist = uDal.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, TargetDeptID);
                    foreach (Zeospace.DeskPlus.Account.BE.UserInfo info in ulist)
                    {
                        DeptID = info.DeptID;
                        DeptName = GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhCn, info.DeptZhTw, PreLang);
                        TitleID = info.CodeTitleID;
                        TitleName = GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhCn, info.TitleZhTw, PreLang);
                        DutyID = info.CodeDutyID;
                        DutyName = GetLangName(info.DutyBase, info.DutyKo, info.DutyEn, info.DutyJa, info.DutyFr, info.DutyZhCn, info.DutyZhTw, PreLang);

                        Zeospace.DeskPlus.Account.BLL.Dept deptBll = new Zeospace.DeskPlus.Account.BLL.Dept();
                        RootDeptID = deptBll.GetRootDeptID(_companyID, DeptID, true);
                    }

                }
                else
                {
                    //부서변경일 경우
                    ChangeDeptID = CurrentUserInfo.DeptID;

                    RootDeptID = CurrentUserInfo.RootDeptID;
                    DeptID = CurrentUserInfo.DeptID;
                    DeptName = CurrentUserInfo.DeptName;
                    TitleID = CurrentUserInfo.CodeTitleID;
                    TitleName = CurrentUserInfo.TitleName;
                    DutyID = CurrentUserInfo.CodeDutyID;
                    DutyName = CurrentUserInfo.DutyName;
                }
            }
            else
            {
                // 해당 결재자 일때 & 대결자 일때
                RootDeptID = CurrentUserInfo.RootDeptID;
                DeptID = CurrentUserInfo.DeptID;
                DeptName = CurrentUserInfo.DeptName;
                TitleID = CurrentUserInfo.CodeTitleID;
                TitleName = CurrentUserInfo.TitleName;
                DutyID = CurrentUserInfo.CodeDutyID;
                DutyName = CurrentUserInfo.DutyName;
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
            seqInfo.UserID = TargetUserID;
            seqInfo.DeptID = TargetDeptID;
            seqInfo.ApprovalType = _approvalType;
            seqInfo.Opinion = tbOpinion.Text;

            if (!TargetUserID.Equals(CurrentUserInfo.UserID) && !ProxyUserID.Equals(0))
            {
                seqInfo.Proxy = "Y";
                seqInfo.ProxyUserID = CurrentUserInfo.UserID;
            }
            else
            {
                seqInfo.Proxy = "N";
                seqInfo.ProxyUserID = 0;
            }

            return seqInfo;
        }

        /// <summary>
        /// 문서정보 생성
        /// </summary>
        /// <returns></returns>
        private BE.DocumentProcessInfo GetDocInfo(int _companyID, int _documentID)
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

                Zeospace.DeskPlus.Common.DAL.DeptCode dalDept = new DeskPlus.Common.DAL.DeptCode();
                List<Zeospace.DeskPlus.Common.BE.DeptCodeInfo> alDeptInfo = dalDept.GetDepCodeLists(info.CompanyID, info.DeptCodeID);
                foreach (var deptInfo in alDeptInfo)
                {
                    dpInfo.DeptID = deptInfo.DeptID;
                }

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
                dpInfo.ContentHTML = (htaFormHTML.Value.EndsWith("<ZEOEND>")) ? htaFormHTML.Value.Substring(0, htaFormHTML.Value.Length - 8) : htaFormHTML.Value;          //<ZEOEND>
                //dpInfo.ContentHTML = htaFormHTML.Value;
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
                DAL.DocumentProcess dalDoc = new DAL.DocumentProcess();
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
            if (seqDal.GetIsMyTurn(_companyID, _documentID, _sequence, _approvalTag, _userID).Equals("N"))
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("companyID", _companyID.ToString());
                paraDic.Add("documentID", _documentID.ToString());

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                string goPage = "/DeskplusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + encParams;
                Response.Redirect(goPage, false);
            }
        }

        /// <summary>
        /// 승인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private bool ApproveS(int _companyID, int _documentID)
        {
            tbSealPath.Text = (tbSealPath.Text.Equals("")) ? SealPath : tbSealPath.Text;

            BE.DocumentProcessInfo dpInfo = GetDocInfo(_companyID, _documentID);
            if (dpInfo == null) return false;

            BE.SequenceInfo seqInfo = GetSeqInfo("S");
            if (seqInfo == null) return false;

            BE.UserInfo sendInfo = GetSendUserInfo("S", tbOpinion.Text);
            sendInfo.DeptID = TargetDeptID;

            //추가 알람대상자
            List<BE.UserInfo> list = new List<BE.UserInfo>();

            Account.DAL.UserEnvironment dalEnv = new Account.DAL.UserEnvironment();

            if (tbAddAlarmInfo.Text != "")
            {
                string[] addAlarmInfo = tbAddAlarmInfo.Text.Split('^');
                for (int i = 0; i < addAlarmInfo.Length; i++)
                {
                    if (addAlarmInfo[i] != "")
                    {
                        string[] addInfo = addAlarmInfo[i].Split(',');

                        BE.UserInfo info = new BE.UserInfo();
                        info.AlarmType = "REJECT";
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = addInfo[0].ToNumber<int>();
                        info.Name = addInfo[1];
                        info.Mail = addInfo[4];
                        info.MobilePhone = addInfo[5];

                        List<Account.BE.UserEnvironmentInfo> alEnv = dalEnv.GetUserEnv(info.CompanyID, info.UserID);
                        foreach (Account.BE.UserEnvironmentInfo infoEnv in alEnv)
                        {
                            info.PreferredLanguage = infoEnv.PreferredLanguage;
                            info.PreLang = infoEnv.PreferredLanguage;
                            info.AlarmApproval = infoEnv.AlarmApproval;
                            if (info.AlarmApproval.IndexOf("1-").Equals(0))
                            {
                                info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                info.AlarmMessenger = "0";
                                info.AlarmMail = "0";
                                info.AlarmSMS = "0";
                            }
                        }

                        list.Add(info);
                    }
                }
            }
            else
            {
                list = null;
            }

            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            return dpBll.ApproveS(dpInfo, seqInfo, tbSealPath.Text, IsLast, IsZQ, sendInfo, tbAllAlarmInfo.Text, list);
        }
        /// <summary>
        /// 전결
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private bool ApproveA(int _companyID, int _documentID)
        {
            tbSealPath.Text = (tbSealPath.Text.Equals("")) ? SealPath : tbSealPath.Text;        //전결인장

            BE.DocumentProcessInfo dpInfo = GetDocInfo(_companyID, _documentID);
            if (dpInfo == null) return false;

            BE.SequenceInfo seqInfo = GetSeqInfo("A");
            if (seqInfo == null) return false;

            BE.UserInfo sendInfo = GetSendUserInfo("A", tbOpinion.Text);
            sendInfo.DeptID = TargetDeptID;

            //추가 알람대상자
            List<BE.UserInfo> list = new List<BE.UserInfo>();

            Account.DAL.UserEnvironment dalEnv = new Account.DAL.UserEnvironment();

            if (tbAddAlarmInfo.Text != "")
            {
                string[] addAlarmInfo = tbAddAlarmInfo.Text.Split('^');
                for (int i = 0; i < addAlarmInfo.Length; i++)
                {
                    if (addAlarmInfo[i] != "")
                    {
                        string[] addInfo = addAlarmInfo[i].Split(',');

                        BE.UserInfo info = new BE.UserInfo();
                        info.AlarmType = "REJECT";
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = addInfo[0].ToNumber<int>();
                        info.Name = addInfo[1];
                        info.Mail = addInfo[4];
                        info.MobilePhone = addInfo[5];

                        List<Account.BE.UserEnvironmentInfo> alEnv = dalEnv.GetUserEnv(info.CompanyID, info.UserID);
                        foreach (Account.BE.UserEnvironmentInfo infoEnv in alEnv)
                        {
                            info.PreferredLanguage = infoEnv.PreferredLanguage;
                            info.PreLang = infoEnv.PreferredLanguage;
                            info.AlarmApproval = infoEnv.AlarmApproval;
                            if (info.AlarmApproval.IndexOf("1-").Equals(0))
                            {
                                info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                info.AlarmMessenger = "0";
                                info.AlarmMail = "0";
                                info.AlarmSMS = "0";
                            }
                        }

                        list.Add(info);
                    }
                }
            }
            else
            {
                list = null;
            }

            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            return dpBll.ApproveA(dpInfo, seqInfo, tbSealPath.Text, IsZQ, sendInfo, list);
        }
        /// <summary>
        /// 반려
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private bool ApproveR(int _companyID, int _documentID)
        {
            tbSealPath.Text = (tbSealPath.Text.Equals("")) ? SealPath : tbSealPath.Text;        //전결인장

            BE.DocumentProcessInfo dpInfo = GetDocInfo(_companyID, _documentID);
            if (dpInfo == null) return false;
            dpInfo.ApprovalState = "R";

            BE.SequenceInfo seqInfo = GetSeqInfo("R");
            if (seqInfo == null) return false;

            BE.UserInfo sendInfo = GetSendUserInfo("R", tbOpinion.Text);
            sendInfo.DeptID = TargetDeptID;

            //추가 알람대상자
            List<BE.UserInfo> list = new List<BE.UserInfo>();

            Account.DAL.UserEnvironment dalEnv = new Account.DAL.UserEnvironment();

            if (tbAddAlarmInfo.Text != "")
            {
                string[] addAlarmInfo = tbAddAlarmInfo.Text.Split('^');
                for (int i = 0; i < addAlarmInfo.Length; i++)
                {
                    if (addAlarmInfo[i] != "")
                    {
                        string[] addInfo = addAlarmInfo[i].Split(',');

                        BE.UserInfo info = new BE.UserInfo();
                        info.AlarmType = "REJECT";
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = addInfo[0].ToNumber<int>();
                        info.Name = addInfo[1];
                        info.Mail = addInfo[4];
                        info.MobilePhone = addInfo[5];

                        List<Account.BE.UserEnvironmentInfo> alEnv = dalEnv.GetUserEnv(info.CompanyID, info.UserID);
                        foreach (Account.BE.UserEnvironmentInfo infoEnv in alEnv)
                        {
                            info.PreferredLanguage = infoEnv.PreferredLanguage;
                            info.PreLang = infoEnv.PreferredLanguage;
                            info.AlarmApproval = infoEnv.AlarmApproval;
                            if (info.AlarmApproval.IndexOf("1-").Equals(0))
                            {
                                info.AlarmMail = (info.AlarmApproval.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                info.AlarmMessenger = (info.AlarmApproval.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                info.AlarmSMS = (info.AlarmApproval.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                            }
                            else
                            {
                                info.AlarmMessenger = "0";
                                info.AlarmMail = "0";
                                info.AlarmSMS = "0";
                            }
                        }

                        list.Add(info);
                    }
                }
            }
            else
            {
                list = null;
            }

            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            return dpBll.ApproveR(dpInfo, seqInfo, tbSealPath.Text, tbTargetInfo.Text, sendInfo, tbAllAlarmInfo.Text, list);
        }
        /// <summary>
        /// 보류
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private bool ApproveD(int _companyID, int _documentID)
        {
            BE.DocumentProcessInfo dpInfo = GetDocInfo(_companyID, _documentID);
            if (dpInfo == null) return false;
            dpInfo.ApprovalState = "D";

            BE.SequenceInfo seqInfo = GetSeqInfo("D");      //보류일은 반영하지 않음(상태만 반영)
            if (seqInfo == null) return false;

            BE.UserInfo sendInfo = GetSendUserInfo("D", tbOpinion.Text);
            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            return dpBll.ApproveD(dpInfo, seqInfo, sendInfo);
        }
        /// <summary>
        /// 보류취소
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private bool ApproveDC(int _companyID, int _documentID)
        {
            BE.DocumentProcessInfo dpInfo = GetDocInfo(_companyID, _documentID);
            if (dpInfo == null) return false;

            BE.SequenceInfo seqInfo = GetSeqInfo("DC");
            if (seqInfo == null) return false;

            seqInfo.ApprovalType = "";
            seqInfo.Proxy = "";
            seqInfo.ProxyUserID = 0;

            dpInfo.ApprovalState = (seqInfo.Sequence < 3) ? "W" : "I";

            // 수신문서 or 이관문서일때
            if (dpInfo.DocumentID != dpInfo.GroupID)
            {
                dpInfo.ApprovalState = "I";
            }

            BE.UserInfo sendInfo = GetSendUserInfo("DC", tbOpinion.Text);
            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            return dpBll.ApproveDCancel(dpInfo, seqInfo, sendInfo);
        }
        /// <summary>
        /// 결재취소
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private bool ApproveSC(int _companyID, int _documentID)
        {
            BE.DocumentProcessInfo dpInfo = GetDocInfo(_companyID, _documentID);
            if (dpInfo == null) return false;

            BE.SequenceInfo seqInfo = GetSeqInfo("SC");
            if (seqInfo == null) return false;

            seqInfo.ApprovalType = "";
            seqInfo.Proxy = "";
            seqInfo.ProxyUserID = 0;

            dpInfo.ApprovalState = (seqInfo.Sequence < 3 && seqInfo.ApprovalTag.IndexOf("ZA") < 0) ? "W" : "I";

            BE.UserInfo sendInfo = GetSendUserInfo("SC", tbOpinion.Text);
            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            return dpBll.ApproveSCancel(dpInfo, seqInfo, sendInfo);
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

        #region DocInfo

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
        public int TargetUserID
        {
            get { return GetViewStateInt("TargetUserID", "0"); }
            set { ViewState["TargetUserID"] = value; }
        }
        public int TargetDeptID
        {
            get { return GetViewStateInt("TargetDeptID", "0"); }
            set { ViewState["TargetDeptID"] = value; }
        }
        public int ChangeDeptID
        {
            get { return GetViewStateInt("ChangeDeptID", "0"); }
            set { ViewState["ChangeDeptID"] = value; }
        }
        public int ProxyUserID
        {
            get { return GetViewStateInt("ProxyUserID", "0"); }
            set { ViewState["ProxyUserID"] = value; }
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
        /// <summary>
        /// 최종결재여부
        /// </summary>
        public string IsLast
        {
            get { return GetViewStateString("IsLast", "N"); }
            set { ViewState["IsLast"] = value; }
        }
        /// <summary>
        /// 수신(ZQ)여부
        /// </summary>
        public string IsZQ
        {
            get { return GetViewStateString("IsZQ", "N"); }
            set { ViewState["IsZQ"] = value; }
        }

        /// <summary>
        /// 연동파일경로
        /// </summary>
        public string CustomFileName
        {
            get { return GetViewStateString("CustomFileName", ""); }
            set { ViewState["CustomFileName"] = value; }
        }
        /// <summary>
        /// 연동경로정보
        /// </summary>
        public string CustomFilePath
        {
            get
            {
                //  /DeskPlusEIP/Legacy/
                string customPath = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "CustomScriptRoot", "");
                if (!customPath.Equals("") && !CustomFileName.Equals(""))
                {
                    customPath += CustomFileName;       //  /DeskPlusEIP/Legacy/aaa.aspx

                    if (!File.Exists(Server.MapPath(customPath)))
                    {
                        customPath = "";
                    }
                }
                else
                {
                    customPath = "";
                }
                return customPath;
            }
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
                //문서내용체크
                if (!htaFormHTML.Value.EndsWith("<ZEOEND>"))
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FailNetwork"), "Approve.List();");
                    return;
                }

                bool rtnval = false;

                //cbtnEdit.Visible = false;
                cbtnApproveS.Visible = false;
                cbtnApproveA.Visible = false;
                cbtnApproveR.Visible = false;
                cbtnApproveD.Visible = false;
                cbtnApproveH.Visible = false;
                cbtnApproveHR.Visible = false;
                cbtnApproveDC.Visible = false;
                cbtnApproveSC.Visible = false;

                switch (EventArgs.ToUpper())
                {
                    case "S":
                        // 승인,합의,합의불가
                        rtnval = ApproveS(CurrentUserInfo.CompanyID, DocumentID);
                        break;
                    case "A":
                        // 전결
                        rtnval = ApproveA(CurrentUserInfo.CompanyID, DocumentID);
                        break;
                    case "R":
                        // 반려                      
                        rtnval = ApproveR(CurrentUserInfo.CompanyID, DocumentID);
                        break;
                    case "D":
                        // 보류                        
                        rtnval = ApproveD(CurrentUserInfo.CompanyID, DocumentID);
                        break;
                    case "DC":
                        // 보류취소
                        rtnval = ApproveDC(CurrentUserInfo.CompanyID, DocumentID);
                        break;
                    case "SC":
                        // 결재취소
                        rtnval = ApproveSC(CurrentUserInfo.CompanyID, DocumentID);
                        break;
                }

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
