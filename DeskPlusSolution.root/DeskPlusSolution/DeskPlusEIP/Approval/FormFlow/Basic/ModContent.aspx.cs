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

namespace Zeospace.DeskPlus.Approval.Web.FormFlow.Basic
{
    public partial class ModContent : Base.BasePageApproval
    {
        #region 변수설정
        #region User
        public int CompanyID = 0;
        public int RootDeptID = 0;
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
        public int ZACount = 0;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Basic.ModContent).Assembly;	//어셈블리 선언
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
            // 1. 기본정보 (기본정보)
            GetUserInfo();

            // 2. 문서정보/첨부파일/근거문서
            GetDocInfo(CurrentUserInfo.CompanyID, DocumentID);
            GetAttachFile(CurrentUserInfo.CompanyID, DocumentID);
            GetBaseDocInfo(CurrentUserInfo.CompanyID, DocumentID);

            // 2-1. 버튼 정의 
            // EDM 버튼
            if (IsLinkedEtc != "")
            {
                liEDMS.Visible = true; // IsLinkedEtc 값이 있는 경우에 열어준다.                  
            }

            // 3. 양식의 결재망정보 (관리자가 지정한 / 사용자가 지정한)

            // 4. 이전 결재에 ZA 존재여부 체크
            GetZACount(CurrentUserInfo.CompanyID, DocumentID, Sequence);

            // 5. 첨부파일 경로 (에디터의 이미지는 에디터페이지에서 처리 - DPUpload이용)
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
                cbtnPreview.Text = this.GetResource("Preview");
                cbtnModSeq.Text = this.GetResource("ChangeOrder");
                cbtnSubmit.Text = this.GetResource("EditDoc");
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
            DocumentID = GetDecryptParam("docID", "0").ToNumber<int>();
            GroupID = GetDecryptParam("groupID", "0").ToNumber<int>();
            Sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
            ApprovalTag = GetDecryptParam("approvalTag", "");
            AccountTag = GetDecryptParam("accountTag", "");
            TargetUserID = GetDecryptParam("targetUserID", "0").ToNumber<int>();
            TargetDeptID = GetDecryptParam("targetDeptID", "0").ToNumber<int>();

            NonActiveX = GetDecryptParam("nonActiveX", "T");        // activeX(F) , nonActiveX(T)
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            ProxyUserID = GetDecryptParam("proxyUserID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 사용자정보 가져오기
        /// </summary>
        private void GetUserInfo()
        {
            CompanyID = CurrentUserInfo.CompanyID;
            RootDeptID = CurrentUserInfo.RootDeptID;
            UserID = CurrentUserInfo.UserID;
            LoginID = CurrentUserInfo.LogInID;
            UserName = CurrentUserInfo.Name;
            DeptID = CurrentUserInfo.DeptID;
            DeptName = CurrentUserInfo.DeptName;
            TitleID = CurrentUserInfo.CodeTitleID;
            TitleName = CurrentUserInfo.TitleName;
            DutyID = CurrentUserInfo.CodeDutyID;
            DutyName = CurrentUserInfo.DutyName;
            EmployeeNumber = CurrentUserInfo.EmployeeNumber;
            UserMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
            MobilePhone = CurrentUserInfo.MobilePhone;
        }

        /// <summary>
        /// 이전 결재에 ZA 존재여부 체크
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        private void GetZACount(int _companyID, int _documentID, int _sequence)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0) || _sequence.Equals(0)) return;

            DAL.Sequence dal = new DAL.Sequence(); //진행문서
            ZACount = dal.GetZACount(_companyID, _documentID, _sequence);
        }


        /// <summary>
        /// 양식정보 가져오기
        /// </summary>
        private void GetDocInfo(int _companyID, int _documentID)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return;

            DAL.DocumentProcess dpDal = new DAL.DocumentProcess(); //진행문서
            List<BE.DocumentProcessInfo> list = dpDal.GetDocument(_companyID, _documentID);

            foreach (BE.DocumentProcessInfo info in list)
            {
                tbIsGuard.Text = info.IsGuard;
                tbAccessTitleID.Text = info.AccessTitleID.ToString();
                tbLifeTime.Text = info.LifeTime;
                tbSubject.Text = info.Subject;
                tbIsLinkedEtc.Text = info.IsLinkedEtc;
                tbIsUseFlow.Text = info.IsUseFlow;
                tbCarbonView.Text = info.CarbonView;

                FormID = info.FormID;

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

                htaFormHTML.Value = info.ContentHTML.Replace("Func_ApprovalView.css", "Func_ApprovalEdit.css").Replace("contenteditable=\"false\"", "");
                htaFormHTML.Value = htaFormHTML.Value.Replace("xfereadonly=\"true\"", "").Replace("unselectable=\"on\"", "");
                htaFormTEXT.Value = info.ContentTEXT;

                IsLinkedEtc = info.IsLinkedEtc.ToString();

                FormBase = info.FormBase;
                Number2 = info.Number2;
                ApprovalState = info.ApprovalState;
                IsLegacy = info.IsLegacy;
                CustomFileName = (!info.CustomFileName.Equals("") && info.IsLegacy.Equals("1")) ? info.CustomFileName : "";
            }
        }
        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        private void GetAttachFile(int _companyID, int _documentID)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return;

            DAL.AttachFile attDal = new DAL.AttachFile();
            List<BE.AttachFileInfo> list = attDal.GetAttachFileLists(_companyID, _documentID);

            string saveFileList = "", nameList = "";
            if (list.Count > 0)
            {
                foreach (BE.AttachFileInfo info in list)
                {
                    saveFileList += "S>" + info.AttachFileID + ">" + info.FilePath + ">" + info.FileName + ">" + info.FileGUID + "|";
                    nameList += info.FileName + ",";
                }

                AttachFileInfo = nameList.TrimEnd(',');  //이전파일내역
            }
            saveFileList = saveFileList.TrimEnd('|');
            tbHiddenFileList.Text = saveFileList;       //첨부파일 정보를 저장하며 리스트박스와 동기화
            tbHiddenSaveFileList.Text = saveFileList;	//서버 파일 삭제시 비교로 사용 될 원본 파일 리스트(FileID 포함)
        }
        /// <summary>
        /// 근거문서가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        private void GetBaseDocInfo(int _companyID, int _documentID)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return;

            DAL.BaseDocument baseDal = new DAL.BaseDocument();
            List<BE.BaseDocumentInfo> list = baseDal.GetBaseDocLists(_companyID, _documentID);

            string nameList = "";
            if (list.Count > 0)
            {
                foreach (BE.BaseDocumentInfo info in list)
                {
                    nameList += info.Name + ",";
                }
                BaseDocInfo = nameList.TrimEnd(',');  //이전근거문서내역
            }
        }

        #region (SUBMIT 함수)

        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        private bool Addition()
        {
            string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');

            BE.DocumentProcessInfo dpInfo = new BE.DocumentProcessInfo();

            #region DP_APP_DocProcess (문서정보)

            dpInfo.CompanyID = CurrentUserInfo.CompanyID;
            dpInfo.DocumentID = DocumentID;
            dpInfo.GroupID = GroupID;
            dpInfo.AccountTag = AccountTag;
            dpInfo.Number2 = Number2;

            dpInfo.LifeTime = tbLifeTime.Text;
            dpInfo.AccessTitleID = tbAccessTitleID.Text.ToNumber<int>();
            dpInfo.Subject = tbSubject.Text;

            string contHTML = GetContents();
            dpInfo.ContentHTML = htaFormHTML.Value.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css").Replace("<ZEOEDITOR></ZEOEDITOR>", contHTML);
            dpInfo.ContentHTML = dpInfo.ContentHTML.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css").Replace("<zeoeditor></zeoeditor>", contHTML);
            dpInfo.ContentTEXT = htaFormTEXT.Value;

            dpInfo.IsGuard = tbIsGuard.Text;
            dpInfo.IsLinkedEtc = tbIsLinkedEtc.Text;
            dpInfo.IsUseFlow = tbIsUseFlow.Text;
            dpInfo.CarbonView = tbCarbonView.Text;

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

            int listCnt = Sequence;
            int ZACnt = 0;
            XmlNodeList nodeList = seqNode.SelectNodes("ITEMS/ITEM");

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["CLASS"] != null)
                {
                    if (node.Attributes["CLASS"].Value.ToUpper().Equals("ZA"))
                    {
                        if (ZACnt.Equals(0))
                        {
                            ZACnt = listCnt;
                            seqlist.Add(GetSequenceInfo(node, ZACnt));              //첫번째 ZA 
                            listCnt++;
                        }
                        else
                        {
                            seqlistZA.Add(GetSequenceInfo(node, ZACnt));            //그외 ZA 
                        }
                    }
                    else
                    {
                        seqlist.Add(GetSequenceInfo(node, listCnt));
                        listCnt++;
                    }
                }
            }
            if (seqlistZA.Count > 0)
            {
                seqlist.AddRange(seqlistZA);
            }

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

            #region DP_APP_BaseDoc (근거문서-신규)
            if (!tbBaseDocIDs.Text.Trim().Equals(""))
            {
                List<BE.BaseDocumentInfo> baselist = new List<BE.BaseDocumentInfo>();
                string[] arrlist = tbBaseDocIDs.Text.Trim(';').Split(';');

                for (int i = 0; i < arrlist.Length; i++)
                {
                    BE.BaseDocumentInfo baseInfo = new BE.BaseDocumentInfo();
                    baseInfo.CompanyID = CurrentUserInfo.CompanyID;
                    baseInfo.DocumentID = dpInfo.DocumentID;
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

            #region DP_APP_Attach (첨부파일-신규)
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            dpInfo.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);
            #endregion

            #region DP_APP_History (변경내역)
            BE.HistoryInfo hInfo = new BE.HistoryInfo();
            hInfo.CompanyID = dpInfo.CompanyID;
            hInfo.DocumentID = dpInfo.DocumentID;
            hInfo.AttachFileInfo = AttachFileInfo;
            hInfo.BaseDocInfo = BaseDocInfo;
            hInfo.Opinion = tbOpinion.Text;
            hInfo.ModifyUserID = CurrentUserInfo.UserID;
            hInfo.ModifyDeptID = CurrentUserInfo.DeptID;
            #endregion

            // 변경 이전정보
            // 변경 이후정보
            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            var rtnval = dpBll.Mod(dpInfo, hInfo);

            #region DOC_STREAM
            var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "DocStreamYN", "N");

            if (docStreamYN == "Y")
            {
                try
                {
                    DAL.AttachFile dalFile = new DAL.AttachFile();
                    DocStream ds = new DocStream();
                    var newDSList = dalFile.GetAttachFileLists(dpInfo.CompanyID, dpInfo.DocumentID);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(DocumentID.ToString());
                    for (int i = 0; i < newDSList.Count; i++)
                    {
                        ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachFileID, "APP_ATTACH");
                        ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachFileID, "APP_ATTACH");
                    }

                    //var pastDSList = dalFile.GetAttachFileLists(dpInfo.CompanyID, DocumentID);
                    //for (int i = 0; i < pastDSList.Count; i++)
                    //{
                    //    ds.DelDocStream(pastDSList[i].CompanyID, pastDSList[i].AttachFileID, "APP_ATTACH");
                    //}
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
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("APPROVAL_MODCONTENT_DS : " + ex.Message);
                    rtnval = false;
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
            info.DocumentID = DocumentID;
            info.ApprovalTag = _node.SelectSingleNode("APPROVALTAG").InnerText;
            //info.Sequence = _sequence;
            info.Sequence = (info.ApprovalTag.IndexOf("ZR").Equals(0)) ? 0 : _sequence;     //공람의 경우, Seq는 '0'이 됨
            info.UserID = _node.SelectSingleNode("USERID").InnerText.ToNumber<int>();
            info.DeptID = _node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();

            //info.MyTurn = (info.ApprovalTag.Equals(ApprovalTag)) ? "Y" : "N";
            info.MyTurn = (_sequence.Equals(Sequence)) ? "Y" : "N";             //해당결재자 이전에 결재자가 추가된경우도 수정대상에 포함됨.
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
            info.DocumentID = DocumentID;
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
            info.DocumentID = DocumentID;
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
            info.DocumentID = DocumentID;
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
        public int DocumentID
        {
            get { return GetViewStateInt("DocumentID", "0").ToString().ToNumber<int>(); }
            set { ViewState["DocumentID"] = value; }
        }
        public int GroupID
        {
            get { return GetViewStateInt("GroupID", "0").ToString().ToNumber<int>(); }
            set { ViewState["GroupID"] = value; }
        }
        public int Sequence
        {
            get { return GetViewStateInt("Sequence", "0").ToString().ToNumber<int>(); }
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
        public int TargetUserID
        {
            get { return GetViewStateInt("TargetUserID", "0").ToString().ToNumber<int>(); }
            set { ViewState["TargetUserID"] = value; }
        }
        public int TargetDeptID
        {
            get { return GetViewStateInt("TargetDeptID", "0").ToString().ToNumber<int>(); }
            set { ViewState["TargetDeptID"] = value; }
        }
        public int FormID
        {
            get { return GetViewStateInt("FormID", "0").ToString().ToNumber<int>(); }
            set { ViewState["FormID"] = value; }
        }

        public string AttachFileInfo
        {
            get { return GetViewStateString("AttachFileInfo", ""); }
            set { ViewState["AttachFileInfo"] = value; }
        }
        public string BaseDocInfo
        {
            get { return GetViewStateString("BaseDocInfo", ""); }
            set { ViewState["BaseDocInfo"] = value; }
        }
        public string FormBase
        {
            get { return GetViewStateString("FormBase", ""); }
            set { ViewState["FormBase"] = value; }
        }
        public string Number2
        {
            get { return GetViewStateString("Number2", ""); }
            set { ViewState["Number2"] = value; }
        }

        public string IsLegacy
        {
            get { return GetViewStateString("IsLegacy", "0"); }
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

        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }

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
        public int ProxyUserID
        {
            get { return GetViewStateInt("ProxyUserID", "0"); }
            set { ViewState["ProxyUserID"] = value; }
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
                }

                if (rtnval)
                {
                    //PDF생성
                    BLL.CreatePDF bll = new BLL.CreatePDF();
                    bll.ProcessPDF(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, DocumentID, Number2, "1");

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "success", "ModContent.SubmitClose('" + this.GetResource("MSG_SUCCESS") + "');");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "fail", "ModContent.SubmitClose('" + this.GetResource("MSG_FAIL") + "');");
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