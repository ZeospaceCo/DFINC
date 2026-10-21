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
using System.Text;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Approval.Web.FormFlow.Temporary
{
    public partial class Add : Base.BasePageApproval
    {
        #region 변수설정
        #region User
        public int CompanyID = 0;
        public string CompanyName = "";
        public int RootDeptID = 0;
        public string RootDeptName = "";
        public int UserID = 0;
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
        #endregion

        public string FolderStatusID = string.Empty;
        public string EventArgs = string.Empty;
        public string BaseDocIDs; //근거문서들 추가 순서대로의 documentID ex)345;553;222;567;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Temporary.Add).Assembly;	//어셈블리 선언
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

            // 2. 임시저장 문서정보/첨부파일/근거문서

            // 2. 문서정보/첨부파일 (cf. 근거문서/결재선 - WS)
            GetDraftInfo(CompanyID, DraftID);
            GetAttachFile(CompanyID, DraftID);

            // 2-1. 버튼 정의 
            // EDM 버튼
            if (IsLinked == "1")
            {
                if (IsLinkedType == "2" || IsLinkedType == "3")
                {
                    liEDMS.Visible = true; // edms 이관 설정이 사용이며 사용자필수나 사용자선택의 경우만 버튼을 열어준다.                    
                }
            }

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
                cbtnSaveDraft.Text = this.GetResource("PrivateDraft");  //임시저장
                cbtnPreview.Text = this.GetResource("Preview");
                cbtnModSeq.Text = this.GetResource("ChangeOrder");
                cbtnSubmit.Text = this.GetResource("DocAdd");
                cbtnEDM.Text = this.GetResource("SendEDM");     // EDMS 이관
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            DraftID = GetDecryptParam("draftID", "0").ToNumber<int>();
            NonActiveX = GetDecryptParam("nonActiveX", "T");        // activeX(F) , nonActiveX(T)

            // 겸직으로 인해, 부서(직급,직책)정보는 파라미터 처리.
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            DeptName = GetDecryptParam("deptName", "").Replace("zeo$", "&");
            TitleID = GetDecryptParam("titleID", "0").ToNumber<int>();
            TitleName = GetDecryptParam("titleName", "");
            DutyID = GetDecryptParam("dutyID", "0").ToNumber<int>();
            DutyName = GetDecryptParam("dutyName", "");
            RootDeptID = GetDecryptParam("rootDeptID", "0").ToNumber<int>();
            RootDeptName = GetDecryptParam("rootDeptName", "").Replace("zeo$", "&");

            BaseDocIDs = GetDecryptParam("baseDocIDs", "");
            FolderStatusID = GetDecryptParam("folderStatusID", "");
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

            /*
            Zeospace.DeskPlus.Account.DAL.Dept dDal = new Account.DAL.Dept();
            List<Account.BE.DeptInfo> deptlist = dDal.GetDeptLists(CompanyID, RootDeptID, true);
            foreach (Account.BE.DeptInfo Info in deptlist)
            {
                RootDeptName = GetLangName(Info.NameBase, Info.NameKo, Info.NameEn, Info.NameJa, Info.NameFr, Info.NameZhcn, Info.NameZhtw, PreLang);
            }
            */
        }

        /// <summary>
        /// 양식정보 가져오기
        /// </summary>
        private void GetDraftInfo(int _companyID, int _draftID)
        {
            if (_companyID.Equals(0) || _draftID.Equals(0)) return;

            DAL.Draft dDal = new DAL.Draft();
            List<BE.DraftInfo> list = dDal.GetDraft(_companyID, _draftID);
            foreach (BE.DraftInfo info in list)
            {
                tbIsGuard.Text = info.IsGuard;
                tbAccessTitleID.Text = info.AccessTitleID.ToString();
                tbIsLinkedEtc.Text = info.IsLinkedEtc;
                tbIsUseFlow.Text = info.IsUseFlow.TrimEnd(',');
                tbCarbonView.Text = info.CarbonView;

                htaFormHTML.Value = info.ContentHTML.Replace("Func_ApprovalView.css", "Func_ApprovalEdit.css").Replace("contenteditable=\"false\"", "");
                htaFormHTML.Value = htaFormHTML.Value.Replace("xfereadonly=\"true\"", "").Replace("unselectable=\"on\"", "");
                htaFormTEXT.Value = info.ContentTEXT;

                FormID = info.FormID;
                IsLegacy = info.IsLegacy;
                CustomFileName = info.CustomFileName;
                IsAfterSubmitCall = info.IsAfterSubmitCall;
                FormBase = info.FormBase;
                FormKo = info.FormKo;
                FormEn = info.FormEn;
                FormJa = info.FormJa;
                FormFr = info.FormFr;
                FormZhcn = info.FormZhcn;
                FormZhtw = info.FormZhtw;
                Subject = info.Subject.ToReverseXSS();
                WriteTimeYMD = info.WriteTimeYMD;
                IsLinked = info.IsLinked;
                IsLinkedType = info.IsLinkedType;
            }
        }
        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        private void GetAttachFile(int _companyID, int _draftID)
        {
            if (_companyID.Equals(0) || _draftID.Equals(0)) return;

            DAL.DraftFile fDal = new DAL.DraftFile();
            List<BE.DraftFileInfo> list = fDal.GetAttachFileList(_companyID, _draftID);

            string saveFileList = "";
            if (list.Count > 0)
            {
                foreach (BE.DraftFileInfo info in list)
                {
                    saveFileList += "S>" + info.AttachFileID + ">" + info.FilePath + ">" + info.FileName + ">" + info.FileGUID + "|";
                }
            }
            saveFileList = saveFileList.TrimEnd('|');
            tbHiddenFileList.Text = saveFileList;       //첨부파일 정보를 저장하며 리스트박스와 동기화
            tbHiddenSaveFileList.Text = saveFileList;	//서버 파일 삭제시 비교로 사용 될 원본 파일 리스트(FileID 포함)
        }

        #region (SUBMIT 함수)

        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        private int Addition()
        {
            string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');
            string isLast = "N", isNextZQ = "N";

            BE.DocumentProcessInfo dpInfo = new BE.DocumentProcessInfo();

            #region DP_APP_DocProcess (문서정보)

            dpInfo.CompanyID = CurrentUserInfo.CompanyID;
            dpInfo.ParentDocumentID = 0;

            //dpInfo.Number1 = CurrentUserInfo.DeptName;  //상신부서
            dpInfo.Number1 = tbDeptName.Text;
            dpInfo.Number2 = yearMonth;                 //상신년월
            dpInfo.Number3 = GetLangName(FormBase, FormKo, FormEn, FormJa, FormFr, FormZhcn, FormZhtw, PreLang);    //양식명

            dpInfo.DeptID = tbDeptID.Text.ToNumber<int>();
            dpInfo.EmployeeNumber = CurrentUserInfo.EmployeeNumber;
            dpInfo.UserID = CurrentUserInfo.UserID;
            //dpInfo.NameBase = CurrentUserInfo.Name;
            //dpInfo.DeptBase = CurrentUserInfo.DeptName;
            //dpInfo.TitleBase = CurrentUserInfo.TitleName;
            //dpInfo.DutyBase = CurrentUserInfo.DutyName;

            dpInfo.LifeTime = tbLifeTime.Text;
            dpInfo.AccessTitleID = tbAccessTitleID.Text.ToNumber<int>();
            dpInfo.ApprovalState = "W";
            dpInfo.AccountTag = "ZD";
            dpInfo.Subject = (tbSubject.Text.Equals("")) ? dpInfo.Number3 : tbSubject.Text;     // 제목필드가 없으면 양식명으로 대체

            string contHTML = GetContents();
            dpInfo.ContentHTML = htaFormHTML.Value.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css").Replace("<ZEOEDITOR></ZEOEDITOR>", contHTML);
            dpInfo.ContentHTML = dpInfo.ContentHTML.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css").Replace("<zeoeditor></zeoeditor>", contHTML);
            dpInfo.ContentTEXT = htaFormTEXT.Value;

            dpInfo.IsLegacy = IsLegacy;
            dpInfo.CustomFileName = CustomFileName;
            dpInfo.IsAfterSubmitCall = IsAfterSubmitCall;
            dpInfo.IsDRM = IsDRM;
            dpInfo.IsGuard = tbIsGuard.Text;
            dpInfo.IsLinkedEtc = tbIsLinkedEtc.Text;
            dpInfo.IsUseFlow = tbIsUseFlow.Text.TrimEnd(',');
            dpInfo.CarbonView = tbCarbonView.Text;

            dpInfo.FormID = FormID;
            dpInfo.GroupID = 0;

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

            #region DP_APP_Seq / DP_APP_CounterPartUser / DP_APP_CarbonCopyUser (결재라인)

            List<BE.SequenceInfo> seqlist = new List<BE.SequenceInfo>();
            List<BE.SequenceInfo> seqlistZA = new List<BE.SequenceInfo>();
            List<BE.CounterPartUserInfo> cplist = new List<BE.CounterPartUserInfo>();
            List<BE.CarbonCopyUserInfo> carbonlist = new List<BE.CarbonCopyUserInfo>();

            #region 발신결재 - DP_APP_Seq (결재라인)

            XmlDocument seqNode = new XmlDocument();
            seqNode.LoadXml(Request["htaXmlSeq"].ToString());

            int ZACnt = 0, appCnt = 0; ;
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

            #region 수신결재 (ZQ/ZP) - DP_APP_CounterPartUser (수신)

            XmlDocument seqNodeZQ = new XmlDocument();
            seqNodeZQ.LoadXml(Request["htaXmlSeqZQ"].ToString());

            nodeList = seqNodeZQ.SelectNodes("ITEMS/ITEM");

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["CLASS"] != null)
                {
                    cplist.Add(GetCounterPartUserInfo(node, cplist.Count + 1));

                    isNextZQ = (node.SelectSingleNode("APPROVALTAG").InnerText.ToUpper().IndexOf("ZQ").Equals(0)) ? "Y" : isNextZQ;
                }
            }
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
            dpInfo.CounterPartUserList = cplist;
            dpInfo.CarbonCopyUserList = carbonlist;

            #endregion

            #region DP_APP_BaseDoc (근거문서)

            List<BE.BaseDocumentInfo> baselist = new List<BE.BaseDocumentInfo>();
            int baseOrder = 1;

            #region 이전 근거문서생성
            DAL.DraftBaseDoc baseDal = new DAL.DraftBaseDoc();
            List<BE.DraftBaseDocInfo> beforeBaseList = baseDal.GetBaseDocLists(CurrentUserInfo.CompanyID, DraftID);
            foreach (BE.DraftBaseDocInfo info in beforeBaseList)
            {
                BE.BaseDocumentInfo baseInfo = new BE.BaseDocumentInfo();
                baseInfo.CompanyID = CurrentUserInfo.CompanyID;
                baseInfo.ParentDocumentID = info.ParentDocumentID;
                baseInfo.Name = info.Name;
                baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                baseInfo.Path = "";
                baseInfo.BaseOrder = baseOrder;
                baseOrder++;

                baselist.Add(baseInfo);
            }
            #endregion

            #region 신규 근거문서생성
            if (!tbBaseDocIDs.Text.Trim().Equals(""))
            {
                string[] arrlist = tbBaseDocIDs.Text.Trim(';').Split(';');

                foreach (string s in arrlist)
                {
                    BE.BaseDocumentInfo baseInfo = new BE.BaseDocumentInfo();
                    baseInfo.CompanyID = CurrentUserInfo.CompanyID;
                    baseInfo.ParentDocumentID = s.Replace("|", "").Split('^')[0].ToNumber<int>();
                    baseInfo.Name = s.Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                    baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                    baseInfo.Path = "";
                    baseInfo.BaseOrder = baseOrder;
                    baseOrder++;

                    baselist.Add(baseInfo);
                }
            }
            #endregion

            dpInfo.BaseDocumentList = baselist;

            #endregion

            #region DP_APP_Attach (첨부파일)

            #region 이전 파일생성
            DAL.DraftFile fDal = new DAL.DraftFile();
            List<BE.DraftFileInfo> beforeAttachList = fDal.GetAttachFileList(CurrentUserInfo.CompanyID, DraftID);
            if (beforeAttachList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (BE.DraftFileInfo info in beforeAttachList)
                {
                    sb.Append("A>");
                    sb.Append(info.AttachFileID);
                    sb.Append(">");
                    sb.Append(info.FilePath);
                    sb.Append(">");
                    sb.Append(info.FileName);
                    sb.Append(">");
                    sb.Append(info.FileGUID);
                    sb.Append("|");
                }

                dpInfo.BeforeHiddenPath = sb.ToString();
            }
            #endregion

            #region 신규 파일생성
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            dpInfo.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);
            #endregion

            #endregion

            BE.UserInfo sendInfo = GetSendUserInfo("S", tbOpinion.Text);
            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            var rtnval = dpBll.Add(dpInfo, SealPath, sendInfo, isLast, isNextZQ);

            #region DOC_STREAM
            var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "DocStreamYN", "N");

            if (docStreamYN == "Y")
            {
                try
                {
                    DAL.AttachFile dalFile = new DAL.AttachFile();
                    DocStream ds = new DocStream();
                    var newDSList = dalFile.GetAttachFileLists(dpInfo.CompanyID, dpInfo.DocumentID);
                    for (int i = 0; i < newDSList.Count; i++)
                    {
                        ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachFileID, "APP_ATTACH");
                        ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachFileID, "APP_ATTACH");
                    }
                }
                catch (Exception ex)
                {
                    DAL.AttachFile dalFile = new DAL.AttachFile();
                    DocStream ds = new DocStream();
                    var DSList = dalFile.GetAttachFileLists(dpInfo.CompanyID, dpInfo.DocumentID);
                    for (int i = 0; i < DSList.Count; i++)
                    {
                        ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachFileID, "APP_ATTACH");
                    }
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("APPROVAL_TemporaryADD_DS : " + ex.Message);
                    rtnval = -1;
                }

            }
            #endregion

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
            info.UserID = _node.SelectSingleNode("USERID").InnerText.ToNumber<int>();
            info.DeptID = _node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();

            info.MyTurn = "N";
            info.ApprovalType = (_sequence.Equals(1)) ? "S" : "";
            info.Opinion = (_sequence.Equals(1)) ? this.tbOpinion.Text : "";
            info.AutoReadLaterON = "N";

            return info;
        }
        /// <summary>
        /// 결재관련 객체처리 (ZQ/ZP)
        /// </summary>
        /// <param name="_node"></param>
        /// <returns></returns>
        private BE.CounterPartUserInfo GetCounterPartUserInfo(XmlNode _node, int _sequence)
        {
            BE.CounterPartUserInfo info = new BE.CounterPartUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Sequence = _sequence;
            info.ApprovalTag = _node.SelectSingleNode("APPROVALTAG").InnerText;
            info.UserID = _node.SelectSingleNode("USERID").InnerText.ToNumber<int>();
            info.DeptID = _node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();
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

        #endregion

        #endregion

        #region (프로퍼티)

        #region 문서정보
        public int DraftID
        {
            get { return GetViewStateInt("DraftID", "0").ToString().ToNumber<int>(); }
            set { ViewState["DraftID"] = value; }
        }
        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }
        public int FormID
        {
            get { return GetViewStateInt("FormID", "0").ToString().ToNumber<int>(); }
            set { ViewState["FormID"] = value; }
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
        /// <summary>
        /// 연동파일-상신후호출 여부
        /// </summary>
        public string IsAfterSubmitCall
        {
            get { return GetViewStateString("IsAfterSubmitCall", ""); }
            set { ViewState["IsAfterSubmitCall"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormBase
        {
            get { return GetViewStateString("FormBase", ""); }
            set { ViewState["FormBase"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormKo
        {
            get { return GetViewStateString("FormKo", ""); }
            set { ViewState["FormKo"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormEn
        {
            get { return GetViewStateString("FormEn", ""); }
            set { ViewState["FormEn"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormJa
        {
            get { return GetViewStateString("FormJa", ""); }
            set { ViewState["FormJa"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormFr
        {
            get { return GetViewStateString("FormFr", ""); }
            set { ViewState["FormFr"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormZhcn
        {
            get { return GetViewStateString("FormZhcn", ""); }
            set { ViewState["FormZhcn"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormZhtw
        {
            get { return GetViewStateString("FormZhcn", ""); }
            set { ViewState["FormZhcn"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Subject
        {
            get { return GetViewStateString("Subject", ""); }
            set { ViewState["Subject"] = value; }
        }
        public string WriteTimeYMD
        {
            get { return GetViewStateString("WriteTimeYMD", ""); }
            set { ViewState["WriteTimeYMD"] = value; }
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
        /// 이관사용여부
        /// </summary>
        public string IsLinked
        {
            get { return GetViewStateString("IsLinked", ""); }
            set { ViewState["IsLinked"] = value; }
        }

        /// <summary>
        /// 이관타입
        /// IsLinkedType 1:관리자지정(변경필요) 2:사용자(필수) 3 : 사용자(선택)
        /// </summary>
        public string IsLinkedType
        {
            get { return GetViewStateString("IsLinkedType", ""); }
            set { ViewState["IsLinkedType"] = value; }
        }

        /// <summary>
        /// 이관경로
        /// </summary>
        public string IsLinkedEtc
        {
            get { return GetViewStateString("IsLinkedEtc", ""); }
            set { ViewState["IsLinkedEtc"] = value; }
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
                int docID = 0;
                switch (EventArgs.ToUpper())
                {
                    case "ADD":
                        docID = Addition();
                        rtnval = (docID > 0) ? true : false;
                        break;
                }

                if (rtnval)
                {
                    string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');
                    //PDF생성
                    BLL.CreatePDF bll = new BLL.CreatePDF();
                    bll.ProcessPDF(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, docID, yearMonth, "1");

                    if (IsAfterSubmitCall.Equals("1"))
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "success", "Add.SubmitAfterCall('" + this.CustomFilePath + "','" + docID.ToString() + "','" + this.GetResource("MSG_SUCCESS") + "');");
                    }
                    else
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "success", "Add.SubmitClose('" + this.GetResource("MSG_SUCCESS") + "');");
                    }
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