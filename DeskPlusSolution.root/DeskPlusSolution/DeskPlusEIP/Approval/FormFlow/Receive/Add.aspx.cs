using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Approval.Web.FormFlow.Receive
{
    public partial class Add : Base.BasePageApproval
    {
        #region 변수설정
        public string EventArgs = string.Empty;

        #region User
        public int CompanyID = 0;
        public string CompanyName = "";
        public int UserID = 0;
        public int RootDeptID = 0;
        public string RootDeptName = "";
        public string LoginID = "";
        public int DeptID = 0;
        public string DeptName = "";
        public int TitleID = 0;
        public string TitleName = "";
        public int DutyID = 0;
        public string DutyName = "";
        public string EmployeeNumber = "";
        public string UserMail = "";
        public string MobilePhone = "";
        public string UserName = "";
        public string FolderStatusID = "";
        public int ProxyUserID = 0;
        #endregion

        #region Etc
        public string NonActiveX;
        public string BaseDocIDs;
        #endregion

        #region Form
        public int FormID = 0;      //수신결재망
        #endregion
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Receive.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
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
            // 1. 기본정보 (인장/기본정보/최상위계열사)
            GetUserInfo();

            GetDocInfo(CompanyID, ParentDocumentID, AccountTag);

            // 2. 양식로드 (신규양식-기본,자사,연동 / 임시저장양식)
            GetFormInfo(FormID);

            // 2-1. 버튼 정의 
            // EDM 버튼
            if (tbIsLinkedEtc.Text != "")
            {
                liEDMS.Visible = true; // IsLinkedEtc 값이 있는 경우에 열어준다.                  
            }

            // 3. 양식의 결재망정보 (관리자가 지정한 / 사용자가 지정한)

            // 5. 문서보존년한

            // 6. 첨부파일 경로 (에디터의 이미지는 에디터페이지에서 처리 - DPUpload이용)
            SwfUploadGUID = System.Guid.NewGuid().ToString();

            //에디터 본문 이미지 첨부 경로
            System.Guid guid = System.Guid.NewGuid();
            string reqTime = FileHelper.GetYearMonth();
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Approval/Editor/" + reqTime + "/" + guid.ToString();
            if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로
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
                cbtnFlowAdd.Text = this.GetResource("Tit_FlowAdd");         //결재선추가
                cbtnFlowDel.Text = this.GetResource("Tit_FlowDel");         //결재선삭제
                cbtnPreview.Text = this.GetResource("Print");
                cbtnSubmit.Text = this.GetResource("DocAdd");       //문서상신
                cbtnModify.Text = this.GetResource("ChangeRcver");
                cbtnReject.Text = this.GetResource("StatusR");      //"반려";
                cbtnComplete.Text = this.GetResource("RcvComplete");
                cbtnModSeq.Text = this.GetResource("ChangeOrder");  //결재순서변경
                cbtnSetLine.Text = this.GetResource("SetAppLine");  //결재자지정 
                cbtnEDM.Text = this.GetResource("SendEDM");     // EDMS 이관
                cbtnEdit.Text = resMng.GetString("EditDoc"); // "결재선/본문수정";
                cbtnOpinionInfo.Text = resMng.GetString("Opinion"); //결재의견
                cbtnPrivateNet.Text = this.GetResource("PrivateAppNet");     //개인결재망
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            FormID = GetDecryptParam("formid", "0").ToNumber<int>();    //수신결재망 정보

            // 겸직으로 인해, 부서(직급,직책)정보는 파라미터 처리.
            NonActiveX = GetDecryptParam("nonActiveX", "T");        // activeX(F) , nonActiveX(T)
            BaseDocIDs = GetDecryptParam("baseDocIDs", "");

            ParentDocumentID = GetDecryptParam("docID", "0").ToNumber<int>();
            AccountTag = GetDecryptParam("approvalTag", "");
            TargetUserID = GetDecryptParam("targetUserID", "0").ToNumber<int>();
            TargetDeptID = GetDecryptParam("targetDeptID", "0").ToNumber<int>();
            GroupID = GetDecryptParam("groupID", "0").ToNumber<int>();
            Sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
            PassedDocID = GetDecryptParam("passedDocID", "0").ToNumber<int>();
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            ProxyUserID = GetDecryptParam("proxyUserID", "0").ToNumber<int>();

            // ParentDocumentID (항상, 발신문서)
            if (Sequence > 1 && AccountTag.IndexOf("ZQ").Equals(0))
            {
                // 발신문서가 아닌 이전문서정보 가져오기
                DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
                List<BE.CounterPartUserInfo> cplist = cpDal.GetCounterPartUsers(CurrentUserInfo.CompanyID, ParentDocumentID);
                List<BE.CounterPartUserInfo> beforelist = cplist.Where(x => x.Sequence < Sequence).OrderByDescending(x => x.Sequence).ToList<BE.CounterPartUserInfo>();

                if (beforelist.Count > 0)
                {
                    ParentDocumentID = beforelist[0].ApprovedDocumentID;
                }
            }

            Zeospace.DeskPlus.Account.DAL.Dept dDal = new Account.DAL.Dept();
            List<Account.BE.DeptInfo> deptlist = dDal.GetDeptLists(CurrentUserInfo.CompanyID, TargetDeptID, true);
            foreach (Account.BE.DeptInfo Info in deptlist)
            {
                TargetDeptName = GetLangName(Info.NameBase, Info.NameKo, Info.NameEn, Info.NameJa, Info.NameFr, Info.NameZhcn, Info.NameZhtw, PreLang);
            }
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
            DeptID = CurrentUserInfo.DeptID;
            DeptName = CurrentUserInfo.DeptName;
            TitleID = CurrentUserInfo.CodeTitleID;
            TitleName = CurrentUserInfo.TitleName;
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
        }

        /// <summary>
        /// 이전 문서정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        private void GetDocInfo(int _companyID, int _documentID, string _approvalTag)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return;

            if (_approvalTag.IndexOf("ZQ") < 0)
            {
                DAL.Document dDal = new DAL.Document(); //완료문서
                List<BE.DocumentInfo> list = dDal.GetDocList(_companyID, _documentID);

                foreach (BE.DocumentInfo info in list)
                {
                    tbIsGuard.Text = info.IsGuard;
                    tbAccessTitleID.Text = info.AccessTitleID.ToString();
                    tbLifeTime.Text = info.LifeTime;
                    tbSubject.Text = info.Subject;
                    tbIsLinkedEtc.Text = info.IsLinkedEtc;
                    tbCarbonView.Text = info.CarbonView;

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

                    htaFormHTML.Value = info.ContentHTML;
                    htaFormTEXT.Value = info.ContentTEXT;

                    ParentFormID = info.FormID;
                    ParentDocumentID = info.DocumentID;

                    Number3 = info.Number3;

                    IsLegacy = info.IsLegacy;
                    CustomFileName = info.CustomFileName;
                }
            }
            else
            {
                DAL.DocumentProcess dpDal = new DAL.DocumentProcess(); //진행문서
                List<BE.DocumentProcessInfo> list = dpDal.GetDocument(_companyID, _documentID);

                foreach (BE.DocumentProcessInfo info in list)
                {
                    tbIsGuard.Text = info.IsGuard;
                    tbAccessTitleID.Text = info.AccessTitleID.ToString();
                    tbCarbonView.Text = info.CarbonView;
                    tbLifeTime.Text = info.LifeTime;
                    tbSubject.Text = info.Subject;
                    tbIsLinkedEtc.Text = info.IsLinkedEtc;

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

                    htaFormHTML.Value = info.ContentHTML;
                    htaFormTEXT.Value = info.ContentTEXT;

                    ParentFormID = info.FormID;
                    ParentDocumentID = info.DocumentID;

                    Number1 = info.Number1;
                    Number2 = info.Number2;
                    Number3 = info.Number3;

                    IsLegacy = info.IsLegacy;
                    CustomFileName = info.CustomFileName;

                    // 다음 수신여부 정보 (GroupID,ApprovalTag)
                    DAL.Sequence seqDal = new DAL.Sequence();
                    string[] arrval = seqDal.GetNextCnt(_companyID, 0, info.GroupID, 0, "").Split('-');
                    if (arrval.Length > 1)
                    {
                        IsNextZQ = (arrval[1].ToNumber<int>() < 2) ? "N" : "Y"; //본인포함 why) 상신 전, 이미 approvaltype은 업데이트 안되어 있는 상태. 
                    }
                }
            }


            if (TargetUserID.Equals(CurrentUserInfo.UserID))
            {
                TargetEmployeeNumber = EmployeeNumber;

                if (TargetDeptID.Equals(CurrentUserInfo.DeptID))
                {
                    RootDeptID = CurrentUserInfo.RootDeptID;
                    DeptID = CurrentUserInfo.DeptID;
                    DeptName = CurrentUserInfo.DeptName;
                    TitleID = CurrentUserInfo.CodeTitleID;
                    TitleName = CurrentUserInfo.TitleName;
                    DutyID = CurrentUserInfo.CodeDutyID;
                    DutyName = CurrentUserInfo.DutyName;

                    Zeospace.DeskPlus.Account.DAL.Dept dDal = new Account.DAL.Dept();
                    List<Account.BE.DeptInfo> deptlist = dDal.GetDeptLists(CompanyID, RootDeptID, true);
                    foreach (Account.BE.DeptInfo Info in deptlist)
                    {
                        RootDeptName = GetLangName(Info.NameBase, Info.NameKo, Info.NameEn, Info.NameJa, Info.NameFr, Info.NameZhcn, Info.NameZhtw, PreLang);
                    }
                }
                else
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

                            Zeospace.DeskPlus.Account.DAL.Dept dDal = new Account.DAL.Dept();
                            List<Account.BE.DeptInfo> deptlist = dDal.GetDeptLists(CompanyID, RootDeptID, true);
                            foreach (Account.BE.DeptInfo Info in deptlist)
                            {
                                RootDeptName = GetLangName(Info.NameBase, Info.NameKo, Info.NameEn, Info.NameJa, Info.NameFr, Info.NameZhcn, Info.NameZhtw, PreLang);
                            }
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

                        Zeospace.DeskPlus.Account.DAL.Dept dDal = new Account.DAL.Dept();
                        List<Account.BE.DeptInfo> deptlist = dDal.GetDeptLists(CompanyID, RootDeptID, true);
                        foreach (Account.BE.DeptInfo Info in deptlist)
                        {
                            RootDeptName = GetLangName(Info.NameBase, Info.NameKo, Info.NameEn, Info.NameJa, Info.NameFr, Info.NameZhcn, Info.NameZhtw, PreLang);
                        }
                    }
                }
            }
            else
            {
                // 대결자 
                RootDeptID = CurrentUserInfo.RootDeptID;
                DeptID = CurrentUserInfo.DeptID;
                DeptName = CurrentUserInfo.DeptName;
                TitleID = CurrentUserInfo.CodeTitleID;
                TitleName = CurrentUserInfo.TitleName;
                DutyID = CurrentUserInfo.CodeDutyID;
                DutyName = CurrentUserInfo.DutyName;

                Zeospace.DeskPlus.Account.DAL.Dept dDal = new Account.DAL.Dept();
                List<Account.BE.DeptInfo> deptlist = dDal.GetDeptLists(CompanyID, RootDeptID, true);
                foreach (Account.BE.DeptInfo Info in deptlist)
                {
                    RootDeptName = GetLangName(Info.NameBase, Info.NameKo, Info.NameEn, Info.NameJa, Info.NameFr, Info.NameZhcn, Info.NameZhtw, PreLang);
                }

                //TargetUserID 정보에 맞는 사원번호 가져오기
                Zeospace.DeskPlus.Account.DAL.User uDal = new Account.DAL.User();
                List<Account.BE.UserInfo> userlist = uDal.GetUserInfo(CompanyID, TargetUserID, true, true);
                foreach (Account.BE.UserInfo Info in userlist)
                {
                    TargetEmployeeNumber = Info.EmployeeNumber;
                }
            }
        }

        /// <summary>
        /// 수신 결재망정보 가져오기
        /// </summary>
        private void GetFormInfo(int _formID)
        {
            if (_formID > 0)
            {
                DAL.Form fDal = new DAL.Form();
                List<BE.FormInfo> list = fDal.GetForm(CurrentUserInfo.CompanyID, _formID);
                foreach (BE.FormInfo info in list)
                {
                    htaFormNet.Value = info.ContentHTML.ToUpper().Replace("FUNC_APPROVALREGISTFORM.CSS", "FUNC_APPROVALEDIT.CSS").Replace("</HTML>", "<DIV ID='PARENTDOCUMENT_" + AccountTag + "'></DIV></HTML>");
                }
            }
        }

        #region (SUBMIT 함수)

        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        private bool Addition()
        {
            #region 수신문서 2개 이상 생기는 오류 방지

            DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
            List<BE.CounterPartUserInfo> cplist = cpDal.GetCounterPartUsers(CurrentUserInfo.CompanyID, GroupID);
            List<BE.CounterPartUserInfo> cplistInfo = cplist.Where(x => x.Sequence == Sequence && x.ApprovalTag == AccountTag).ToList<BE.CounterPartUserInfo>();

            int ApprovedDocumentID = -1;
            if (cplistInfo.Count > 0)
            {
                ApprovedDocumentID = cplistInfo[0].ApprovedDocumentID;
            }

            if (ApprovedDocumentID > 0)
            {
                return false;
            }

            #endregion

            string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');
            string isLast = "N";

            BE.DocumentProcessInfo dpInfo = new BE.DocumentProcessInfo();

            #region DP_APP_DocProcess (문서정보)

            dpInfo.CompanyID = CurrentUserInfo.CompanyID;
            dpInfo.ParentDocumentID = ParentDocumentID;

            if (ChangeDeptID > 0)
            {
                TargetDeptName = CurrentUserInfo.DeptName;
            }

            dpInfo.Number1 = (this.AccountTag.IndexOf("ZQ").Equals(0)) ? Number1 : TargetDeptName;   //상신부서
            dpInfo.Number2 = (this.AccountTag.IndexOf("ZQ").Equals(0)) ? Number2 : yearMonth;  //상신년월
            dpInfo.Number3 = Number3;       //양식명

            dpInfo.DeptID = (ChangeDeptID == 0) ? TargetDeptID : ChangeDeptID;
            dpInfo.UserID = this.TargetUserID;
            if (!TargetUserID.Equals(CurrentUserInfo.UserID) && ProxyUserID.Equals(0))
            {
                dpInfo.UserID = CurrentUserInfo.UserID; //부서수신 결재 때문에 결재자 정보를 변경한다.
            }
            dpInfo.EmployeeNumber = TargetEmployeeNumber;

            //dpInfo.NameBase = CurrentUserInfo.Name;
            //dpInfo.DeptBase = CurrentUserInfo.Name;
            //dpInfo.TitleBase = CurrentUserInfo.TitleName;
            //dpInfo.DutyBase = CurrentUserInfo.DutyName;

            dpInfo.LifeTime = tbLifeTime.Text;
            dpInfo.AccessTitleID = tbAccessTitleID.Text.ToNumber<int>();

            dpInfo.ApprovalState = "I";
            dpInfo.AccountTag = (this.AccountTag.IndexOf("PS").Equals(0)) ? "ZD" : this.AccountTag;   // PS. ZQ01,ZQ02, ZP01,ZP02
            dpInfo.Subject = (tbSubject.Text.Equals("")) ? dpInfo.Number3 : tbSubject.Text.ToReverseXSS();     // 제목필드가 없으면 양식명으로 대체

            string contHTML = GetContents();
            dpInfo.ContentHTML = htaFormHTML.Value.Replace("FUNC_APPROVALEDIT.CSS", "Func_ApprovalView.css").Replace("<ZEOEDITOR></ZEOEDITOR>", contHTML);
            dpInfo.ContentHTML = dpInfo.ContentHTML.Replace("<zeoeditor></zeoeditor>", contHTML);
            dpInfo.ContentTEXT = htaFormTEXT.Value;

            dpInfo.IsLegacy = IsLegacy;
            dpInfo.CustomFileName = CustomFileName;
            dpInfo.IsAfterSubmitCall = "0";
            dpInfo.IsDRM = IsDRM;
            dpInfo.IsGuard = tbIsGuard.Text;
            dpInfo.IsLinkedEtc = tbIsLinkedEtc.Text;
            dpInfo.IsUseFlow = tbIsUseFlow.Text.TrimEnd(',');
            dpInfo.CarbonView = tbCarbonView.Text;

            dpInfo.FormID = ParentFormID;
            dpInfo.GroupID = GroupID;

            dpInfo.Etc1 = tbEtc1.Text.Trim();
            dpInfo.Etc2 = tbEtc2.Text.Trim();
            dpInfo.Etc3 = tbEtc3.Text.Trim();
            dpInfo.Etc4 = tbEtc4.Text.Trim();
            dpInfo.Etc5 = tbEtc5.Text.Trim();
            dpInfo.Etc6 = tbEtc6.Text.Trim();
            dpInfo.Etc7 = tbEtc7.Text.Trim();
            dpInfo.Etc8 = tbEtc8.Text.Trim();
            dpInfo.Etc9 = tbEtc9.Text.Trim();
            dpInfo.Etc10 = tbEtc10.Text.Trim();
            dpInfo.IsMobile = "0";

            #endregion

            #region DP_APP_Seq / DP_APP_CarbonCopyUser (결재라인)

            List<BE.SequenceInfo> seqlist = new List<BE.SequenceInfo>();
            List<BE.SequenceInfo> seqlistZA = new List<BE.SequenceInfo>();
            List<BE.CarbonCopyUserInfo> carbonlist = new List<BE.CarbonCopyUserInfo>();

            #region 발신결재 - DP_APP_Seq (결재라인)

            XmlDocument seqNode = new XmlDocument();
            seqNode.LoadXml(Request["htaXmlSeq"].ToString());

            int ZACnt = 0, appCnt = 0;
            XmlNodeList nodeList = seqNode.SelectNodes("ITEMS/ITEM");

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["CLASS"] != null)
                {
                    if (node.Attributes["CLASS"].Value.ToUpper().Equals("ZA"))
                    {
                        if (ZACnt.Equals(0))
                        {
                            ZACnt = seqlist.Count + 1;
                            seqlist.Add(GetSequenceInfo(node, ZACnt));              //첫번째 ZA 
                        }
                        else
                        {
                            seqlistZA.Add(GetSequenceInfo(node, ZACnt));            //그외 ZA 
                        }
                    }
                    else
                    {
                        seqlist.Add(GetSequenceInfo(node, seqlist.Count + 1));
                    }
                    // For, 상신전결
                    if (node.Attributes["CLASS"].Value.IndexOf("ZR") < 0)
                    {
                        appCnt++;
                    }
                }
            }

            if (seqlistZA.Count > 0)
            {
                seqlist.AddRange(seqlistZA);
            }

            //isLast = (seqlist.Count.Equals(1)) ? "Y" : "N";
            isLast = (appCnt.Equals(1)) ? "Y" : "N";

            #endregion

            #region 참조 (ZC) - DP_APP_CarbonCopyUser (참조)

            XmlDocument seqNodeZC = new XmlDocument();
            seqNodeZC.LoadXml(Request["htaXmlSeqZC"].ToString());

            nodeList = seqNodeZC.SelectNodes("ITEMS/ITEM");

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["CLASS"] != null)
                {
                    if (node.SelectSingleNode("SUBTREE").InnerText.Equals("1") && node.SelectSingleNode("ISGROUP").InnerText.ToUpper().Equals("G"))
                    {
                        int deptID = node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();

                        carbonlist.Add(GetCarbonCopyUserInfo(node.SelectSingleNode("APPROVALTAG").InnerText, 0, deptID, "G", "Y", "1"));

                        //Zeospace.DeskPlus.Account.BLL.Dept dBll = new Account.BLL.Dept();
                        //string subDepts = "," + dBll.GetChildDeptList(CurrentUserInfo.CompanyID, deptID, true).Trim(',') + ",";   //해당부서포함된 하위부서리스트 (1,51,30,31,32,33,35,36,37,34,38,52,53,54,55,56)
                        //subDepts = subDepts.Replace("," + deptID.ToString() + ",", "");
                        //string[] arrDepts = subDepts.Trim(',').Split(',');

                        //foreach (string s in arrDepts)
                        //{
                        //    carbonlist.Add(GetCarbonCopyUserInfo(node.SelectSingleNode("APPROVALTAG").InnerText, 0, s.ToNumber<int>(), "G", "N", "0"));
                        //}
                    }
                    else
                    {
                        carbonlist.Add(GetCarbonCopyUserInfo(node));
                    }
                }
            }

            #endregion

            dpInfo.SequenceList = seqlist;
            dpInfo.CarbonCopyUserList = carbonlist;

            #endregion

            #region DP_APP_BaseDoc (근거문서)
            if (!tbBaseDocIDs.Text.Trim().Equals(""))
            {
                List<BE.BaseDocumentInfo> baselist = new List<BE.BaseDocumentInfo>();
                string[] arrlist = tbBaseDocIDs.Text.Trim(';').Split(';');

                for (int i = 0; i < arrlist.Length; i++)
                {
                    BE.BaseDocumentInfo baseInfo = new BE.BaseDocumentInfo();
                    baseInfo.CompanyID = CurrentUserInfo.CompanyID;
                    baseInfo.ParentDocumentID = arrlist[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                    baseInfo.Name = arrlist[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                    baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                    baseInfo.Path = "";
                    baseInfo.BaseOrder = i + 1;

                    baselist.Add(baseInfo);
                }
                dpInfo.BaseDocumentList = baselist;
            }
            #endregion

            #region DP_APP_Attach (첨부파일)
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            dpInfo.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);
            #endregion

            bool rtnval = false;

            BE.UserInfo sendInfo = GetSendUserInfo("S", tbOpinion.Text);
            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();

            // 이관일때와 수신일때 분기.
            if (this.AccountTag.IndexOf("PS") < 0)
            {
                //수신 (ZP,ZQ)
                #region 수신자 정보
                BE.CounterPartUserInfo cpInfo = new BE.CounterPartUserInfo();
                cpInfo.CompanyID = dpInfo.CompanyID;
                cpInfo.GroupID = dpInfo.GroupID;
                cpInfo.Sequence = Sequence;
                cpInfo.ApprovalTag = AccountTag;
                cpInfo.UserID = TargetUserID;
                if (!TargetUserID.Equals(CurrentUserInfo.UserID) && ProxyUserID.Equals(0))
                {
                    cpInfo.UserID = CurrentUserInfo.UserID; //부서수신 결재 때문에 결재자 정보를 변경한다.
                }
                cpInfo.DeptID = (ChangeDeptID == 0) ? TargetDeptID : ChangeDeptID; //TargetDeptID;
                cpInfo.ApprovalType = "S";
                cpInfo.Opinion = tbOpinion.Text;
                cpInfo.Proxy = (!TargetUserID.Equals(CurrentUserInfo.UserID) && !ProxyUserID.Equals(0)) ? "Y" : "N";
                cpInfo.ProxyUserID = (!TargetUserID.Equals(CurrentUserInfo.UserID) && !ProxyUserID.Equals(0)) ? CurrentUserInfo.UserID : 0;
                #endregion

                rtnval = dpBll.AddZQ(dpInfo, cpInfo, SealPath, sendInfo, isLast, IsNextZQ);
            }
            else
            {
                // 이관일때는 연동문서 적용안함
                dpInfo.CustomFileName = "";

                //이관 (PS)
                #region 이관자 정보
                BE.PassedDocumentInfo pInfo = new BE.PassedDocumentInfo();
                pInfo.CompanyID = dpInfo.CompanyID;
                pInfo.PassedDocID = this.PassedDocID;
                pInfo.DocumentID = this.ParentDocumentID;
                pInfo.ReceiveUserID = TargetUserID;
                pInfo.ReceiveDeptID = (ChangeDeptID == 0) ? TargetDeptID : ChangeDeptID; //TargetDeptID;
                //cpInfo.Proxy = (!TargetUserID.Equals(CurrentUserInfo.UserID)) ? "Y" : "N";
                //cpInfo.ProxyUserID = (!TargetUserID.Equals(CurrentUserInfo.UserID)) ? TargetUserID : 0;
                #endregion

                rtnval = dpBll.AddPS(dpInfo, pInfo, SealPath, sendInfo, isLast, IsNextZQ);
            }

            return rtnval;
        }

        #region 결재관련 객체처리
        /// <summary>
        /// 결재관련 객체처리 (ZD/ZH/ZA)
        /// </summary>
        /// <param name="_node"></param>
        /// <returns></returns>
        private BE.SequenceInfo GetSequenceInfo(XmlNode _node, int _sequence)
        {
            BE.SequenceInfo info = new BE.SequenceInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ApprovalTag = _node.SelectSingleNode("APPROVALTAG").InnerText;
            //info.Sequence = _sequence;
            info.Sequence = (info.ApprovalTag.IndexOf("ZR").Equals(0)) ? 0 : _sequence;     //공람의 경우, Seq는 '0'이 됨
            info.MyTurn = "N";
            info.ApprovalType = (_sequence.Equals(1)) ? "S" : "";
            info.Opinion = (_sequence.Equals(1)) ? this.tbOpinion.Text : "";
            info.AutoReadLaterON = "N";

            if ((!TargetUserID.Equals(CurrentUserInfo.UserID)) && (_sequence.Equals(1)) && !ProxyUserID.Equals(0))
            {
                info.UserID = this.TargetUserID;
                info.DeptID = this.TargetDeptID;
                info.Proxy = "Y";
                info.ProxyUserID = CurrentUserInfo.UserID;

            }
            else
            {
                if ((!TargetUserID.Equals(CurrentUserInfo.UserID)) && (_sequence.Equals(1)))
                {
                    //부서수신 결재 때문에 결재자 정보를 변경한다.
                    info.UserID = CurrentUserInfo.UserID;
                    info.DeptID = CurrentUserInfo.DeptID;
                }
                else
                {
                    info.UserID = _node.SelectSingleNode("USERID").InnerText.ToNumber<int>();
                    info.DeptID = _node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();
                }

                info.Proxy = "N";
                info.ProxyUserID = 0;
            }

            return info;
        }
        /// <summary>
        /// 결재관련 객체처리 (ZC)
        /// </summary>
        /// <param name="_node"></param>
        /// <returns></returns>
        private BE.CarbonCopyUserInfo GetCarbonCopyUserInfo(XmlNode _node)
        {
            BE.CarbonCopyUserInfo info = new BE.CarbonCopyUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ApprovalTag = _node.SelectSingleNode("APPROVALTAG").InnerText;
            info.UserID = _node.SelectSingleNode("USERID").InnerText.ToNumber<int>();
            info.DeptID = _node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();
            info.IsGroup = _node.SelectSingleNode("ISGROUP").InnerText;
            info.IsSet = "Y";
            info.IsSubTree = "0";
            return info;
        }
        /// <summary>
        /// 결재관련 객체처리 (ZC)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_isGroup"></param>
        /// <returns></returns>
        private BE.CarbonCopyUserInfo GetCarbonCopyUserInfo(string _approvalTag, int _userID, int _deptID, string _isGroup, string _isSet, string _isSubTree)
        {
            BE.CarbonCopyUserInfo info = new BE.CarbonCopyUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ApprovalTag = _approvalTag;
            info.UserID = _userID;
            info.DeptID = _deptID;
            info.IsGroup = _isGroup;
            info.IsSet = _isSet;
            info.IsSubTree = _isSubTree;
            return info;
        }
        #endregion

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

                string contentValue = htaEditorMime.Text;

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass util
                    = new DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass();

                util.ProcessDecoding(contentValue, filePath, serverNameFolderName, 0);
                contents = util.DecodedHTML;

                string[] arrFile = Directory.GetFiles(filePath);
                if (arrFile.Length.Equals(0))
                {
                    Directory.Delete(filePath);
                }
            }
            else if (NonActiveX == "T") //x-free
            {
                contents = htaEditorHTML.Value;
            }

            return Regex.Replace(contents, "</html>\0", "</html>", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 수신자 반려
        /// </summary>
        /// <returns></returns>
        private bool Reject()
        {
            BE.DocumentProcessInfo dpInfo = new BE.DocumentProcessInfo();

            DAL.DocumentProcess dpDal = new DAL.DocumentProcess(); //진행문서
            List<BE.DocumentProcessInfo> list = dpDal.GetDocument(CurrentUserInfo.CompanyID, ParentDocumentID);

            foreach (BE.DocumentProcessInfo info in list)
            {
                dpInfo.UserID = info.UserID;
                dpInfo.Subject = info.Subject;
                dpInfo.Number3 = info.Number3;
                dpInfo.DeptID = info.DeptID;

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
                dpInfo.NameBase = info.NameBase;
                dpInfo.NameKo = info.NameKo;
                dpInfo.NameEn = info.NameEn;
                dpInfo.NameJa = info.NameJa;
                dpInfo.NameFr = info.NameFr;
                dpInfo.NameZhcn = info.NameZhcn;
                dpInfo.NameZhtw = info.NameZhtw;
                dpInfo.ManyName = info.ManyName;
                #endregion
            }

            dpInfo.CompanyID = CurrentUserInfo.CompanyID;
            dpInfo.DocumentID = ParentDocumentID;
            dpInfo.GroupID = GroupID;
            dpInfo.AccountTag = AccountTag;
            dpInfo.ApprovalState = "R";
            dpInfo.ContentHTML = htaFormHTML.Value.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css");
            dpInfo.ContentTEXT = htaFormTEXT.Value;
            dpInfo.Subject = tbSubject.Text.ToReverseXSS();

            #region 수신자 정보
            BE.CounterPartUserInfo cpInfo = new BE.CounterPartUserInfo();
            cpInfo.CompanyID = dpInfo.CompanyID;
            cpInfo.GroupID = dpInfo.GroupID;
            cpInfo.Sequence = Sequence;
            cpInfo.ApprovalTag = AccountTag;
            cpInfo.UserID = TargetUserID;
            if (!TargetUserID.Equals(CurrentUserInfo.UserID) && ProxyUserID.Equals(0))
            {
                cpInfo.UserID = CurrentUserInfo.UserID; //부서수신 결재 때문에 결재자 정보를 변경한다.
            }
            cpInfo.DeptID = (ChangeDeptID == 0) ? TargetDeptID : ChangeDeptID; //TargetDeptID;
            cpInfo.ApprovalType = "R";
            cpInfo.Opinion = tbOpinion.Text;
            cpInfo.Proxy = (!TargetUserID.Equals(CurrentUserInfo.UserID) && !ProxyUserID.Equals(0)) ? "Y" : "N";
            cpInfo.ProxyUserID = (!TargetUserID.Equals(CurrentUserInfo.UserID) && !ProxyUserID.Equals(0)) ? CurrentUserInfo.UserID : 0;
            #endregion

            //tbTargetInfo : [0]DocumentID-[1]Sequence-[2]ApprovalTag-[3]UserID

            BE.UserInfo sendInfo = GetSendUserInfo("R", tbOpinion.Text);
            sendInfo.DeptID = TargetDeptID;

            //추가 알람대상자
            List<BE.UserInfo> addlist = new List<BE.UserInfo>();

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

                        addlist.Add(info);
                    }
                }
            }
            else
            {
                addlist = null;
            }

            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            return dpBll.ApproveRReceive(dpInfo, cpInfo, tbTargetInfo.Text, sendInfo, tbAllAlarmInfo.Text, addlist);
        }

        /// <summary>
        /// 담당자 변경
        /// </summary>
        /// <returns></returns>
        private bool Modify()
        {
            //tbTargetInfo : [0]UserID-[1]DeptID
            string[] targetInfo = tbTargetInfo.Text.Split('-');

            BE.CounterPartUserInfo cpInfo = new BE.CounterPartUserInfo();
            cpInfo.CompanyID = CurrentUserInfo.CompanyID;
            cpInfo.GroupID = GroupID;
            cpInfo.Sequence = Sequence;
            cpInfo.ApprovalTag = AccountTag;
            cpInfo.UserID = targetInfo[0].TrimEnd(',').ToNumber<int>();
            cpInfo.DeptID = targetInfo[1].TrimEnd(',').ToNumber<int>();

            // 반려가능한 수신일때만 변경. (SP에서 처리)
            string contentHTML = GetContents();
            contentHTML = htaFormHTML.Value.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css").Replace("<ZEOEDITOR></ZEOEDITOR>", contentHTML);
            contentHTML = contentHTML.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css").Replace("<zeoeditor></zeoeditor>", contentHTML);

            BE.DocumentProcessInfo dpInfo = new BE.DocumentProcessInfo();

            DAL.DocumentProcess dpDal = new DAL.DocumentProcess(); //진행문서
            List<BE.DocumentProcessInfo> list = dpDal.GetDocument(CurrentUserInfo.CompanyID, ParentDocumentID);

            foreach (BE.DocumentProcessInfo info in list)
            {
                dpInfo.CompanyID = info.CompanyID;
                dpInfo.DocumentID = info.DocumentID;

                dpInfo.UserID = info.UserID;
                dpInfo.Subject = info.Subject;
                dpInfo.Number3 = info.Number3;
                dpInfo.DeptID = info.DeptID;

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
                dpInfo.NameBase = info.NameBase;
                dpInfo.NameKo = info.NameKo;
                dpInfo.NameEn = info.NameEn;
                dpInfo.NameJa = info.NameJa;
                dpInfo.NameFr = info.NameFr;
                dpInfo.NameZhcn = info.NameZhcn;
                dpInfo.NameZhtw = info.NameZhtw;
                dpInfo.ManyName = info.ManyName;
                #endregion
            }

            dpInfo.ContentHTML = contentHTML;
            dpInfo.ContentTEXT = htaFormTEXT.Value;

            BE.UserInfo sendInfo = GetSendUserInfo("S", "");

            BLL.DocumentProcess bll = new BLL.DocumentProcess();
            return bll.ModCounterpartuser(dpInfo, sendInfo, cpInfo);

            //DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
            //return cpDal.ModReplace(cpInfo, contentHTML, contentText);
        }

        /// <summary>
        /// 수신자 완료
        /// </summary>
        /// <returns></returns>
        private bool Complete()
        {
            BE.CounterPartUserInfo cpInfo = new BE.CounterPartUserInfo();
            cpInfo.CompanyID = CurrentUserInfo.CompanyID;
            cpInfo.GroupID = GroupID;
            cpInfo.Sequence = Sequence;
            cpInfo.ApprovalTag = AccountTag;
            cpInfo.UserID = TargetUserID;
            if (!TargetUserID.Equals(CurrentUserInfo.UserID) && ProxyUserID.Equals(0))
            {
                cpInfo.UserID = CurrentUserInfo.UserID; //부서수신 결재 때문에 결재자 정보를 변경한다.
            }
            cpInfo.DeptID = (ChangeDeptID == 0) ? TargetDeptID : ChangeDeptID; //TargetDeptID;
            cpInfo.ApprovalType = "C";
            cpInfo.Opinion = tbOpinion.Text;
            cpInfo.Proxy = (!TargetUserID.Equals(CurrentUserInfo.UserID) && !ProxyUserID.Equals(0)) ? "Y" : "N";
            cpInfo.ProxyUserID = (!TargetUserID.Equals(CurrentUserInfo.UserID) && !ProxyUserID.Equals(0)) ? CurrentUserInfo.UserID : 0;

            DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
            return cpDal.ModComplete(cpInfo);
        }
        #endregion

        #endregion

        #region (프로퍼티)

        #region 문서정보
        public int ParentDocumentID
        {
            get { return GetViewStateInt("ParentDocumentID", "0"); }
            set { ViewState["ParentDocumentID"] = value; }
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
        public int TargetUserID
        {
            get { return GetViewStateInt("TargetUserID", "0"); }
            set { ViewState["TargetUserID"] = value; }
        }
        public string TargetEmployeeNumber
        {
            get { return GetViewStateString("TargetEmployeeNumber", ""); }
            set { ViewState["TargetEmployeeNumber"] = value; }
        }
        public string TargetDeptName
        {
            get { return GetViewStateString("TargetDeptName", ""); }
            set { ViewState["TargetDeptName"] = value; }
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
        public int Sequence
        {
            get { return GetViewStateInt("Sequence", "0"); }
            set { ViewState["Sequence"] = value; }
        }
        /// <summary>
        /// 이관정보
        /// </summary>
        public int PassedDocID
        {
            get { return GetViewStateInt("PassedDocID", "0"); }
            set { ViewState["PassedDocID"] = value; }
        }
        #endregion

        #region 발신문서정보
        /// <summary>
        /// 상신부서명
        /// </summary>
        public string Number1
        {
            get { return GetViewStateString("Number1", ""); }
            set { ViewState["Number1"] = value; }
        }
        /// <summary>
        /// 상신년월
        /// </summary>
        public string Number2
        {
            get { return GetViewStateString("Number2", ""); }
            set { ViewState["Number2"] = value; }
        }
        /// <summary>
        /// 문서명
        /// </summary>
        public string Number3
        {
            get { return GetViewStateString("Number3", ""); }
            set { ViewState["Number3"] = value; }
        }
        /// <summary>
        /// 원본양식ID
        /// </summary>
        public int ParentFormID
        {
            get { return GetViewStateInt("ParentFormID", "0"); }
            set { ViewState["ParentFormID"] = value; }
        }

        /// <summary>
        /// 연동유무
        /// </summary>
        public string IsLegacy
        {
            get { return GetViewStateString("IsLegacy", ""); }
            set { ViewState["IsLegacy"] = value; }
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
                if (!customPath.Equals("") && !CustomFileName.Equals("") && IsLegacy.Equals("1"))
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

        /// <summary>
        /// 에디터-파일처리경로
        /// </summary>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }

        /// <summary>
        /// 파일업로드
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        /// DRM연동여부
        /// </summary>
        public string IsDRM
        {
            get
            {
                return AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsDRMUse", "0");
            }
        }

        /// <summary>
        /// 인장경로
        /// </summary>
        public string SealPath
        {
            get { return GetViewStateString("SealPath", ""); }
            set { ViewState["SealPath"] = value; }
        }

        /// <summary>
        /// 수신(ZQ)여부
        /// </summary>
        public string IsNextZQ
        {
            get { return GetViewStateString("IsNextZQ", "N"); }
            set { ViewState["IsNextZQ"] = value; }
        }

        #endregion

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
                    System.Collections.Generic.List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = dal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
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
                    case "REJECT":
                        rtnval = Reject();
                        break;
                    case "COMPLETE":
                        rtnval = Complete();
                        break;
                    case "MODIFY":
                        rtnval = Modify();
                        break;
                }

                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "success", "Add.SubmitClose('" + this.GetResource("MSG_SUCCESS") + "');");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "fail", "Add.SubmitClose('" + this.GetResource("MSG_FAIL") + "');");
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
