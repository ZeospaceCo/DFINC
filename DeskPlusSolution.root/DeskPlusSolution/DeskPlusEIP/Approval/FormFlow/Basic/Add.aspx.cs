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
    public partial class Add : Base.BasePageApproval
    {
        #region 변수설정
        public string EventArgs = string.Empty;

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

        #region Etc
        //public int ParentDocumentID;
        //public int GroupID;
        public string NonActiveX;
        public string BaseDocIDs; // 근거문서들 추가 순서대로의 documentID ex) |234;|454;|875; -> |docID;
        public int FormID = 0;
        public string NickName = string.Empty;
        public string LegacyKey = string.Empty;
        public string ParamEtc = string.Empty;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Basic.Add).Assembly;	//어셈블리 선언
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
            // 0. 버튼처리
            if (!NickName.Equals(""))
            {
                cbtnSaveDraft.Visible = false;
            }

            // 1. 기본정보 (인장/기본정보/최상위계열사)
            GetUserInfo();

            // 2. 양식로드 (신규양식-기본,자사,연동 / 임시저장양식)
            GetFormInfo();

            // 2-1. 버튼 정의 
            // EDM 버튼
            if (IsLinked == "1")
            {
                if (IsLinkedType == "2" || IsLinkedType == "3")
                {
                    liEDMS.Visible = true; // edms 이관 설정이 사용이며 사용자필수나 사용자선택의 경우만 버튼을 열어준다.                    
                }
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
                cbtnPreview.Text = this.GetResource("Preview");         //미리보기
                cbtnSaveNet.Text = this.GetResource("NetAdd");          //결재망저장
                cbtnSaveDraft.Text = this.GetResource("PrivateDraft");  //임시저장
                cbtnSubmit.Text = this.GetResource("DocAdd");           //문서상신
                cbtnModSeq.Text = this.GetResource("ChangeOrder");      //결재순서변경
                cbtnPrivateNet.Text = this.GetResource("PrivateAppNet");     //개인결재망
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
            FormID = GetDecryptParam("formID", "0").ToNumber<int>();
            NickName = GetDecryptParam("nickname", "");
            LegacyKey = GetDecryptParam("legacykey", "");
            ParamEtc = GetDecryptParam("parametc", "");


            // 겸직으로 인해, 부서(직급,직책)정보는 파라미터 처리.
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            DeptName = GetDecryptParam("deptName", "").Replace("zeo$", "&");
            TitleID = GetDecryptParam("titleID", "0").ToNumber<int>();
            TitleName = GetDecryptParam("titleName", "");
            DutyID = GetDecryptParam("dutyID", "0").ToNumber<int>();
            DutyName = GetDecryptParam("dutyName", "");
            RootDeptID = GetDecryptParam("rootDeptID", "0").ToNumber<int>();
            RootDeptName = GetDecryptParam("rootDeptName", "").Replace("zeo$", "&");
            NonActiveX = GetDecryptParam("nonActiveX", "T");        // activeX(F) , nonActiveX(T)
            BaseDocIDs = GetDecryptParam("baseDocIDs", "");
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
            Zospace.DeskPlus.Account.DAL.Dept dDal = new Account.DAL.Dept();
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
        private void GetFormInfo()
        {
            if (FormID > 0)
            {
                DAL.Form fDal = new DAL.Form();
                List<BE.FormInfo> list = fDal.GetForm(CurrentUserInfo.CompanyID, FormID);
                foreach (BE.FormInfo info in list)
                {
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

                    IsLinked = info.IsLinked.ToString();
                    IsLinkedType = info.IsLinkedType.ToString();
                    IsLinkedEtc = info.IsLinkedEtc.ToString();

                    htaFormHTML.Value = info.ContentHTML.Replace("Func_ApprovalRegistForm.css", "Func_ApprovalEdit.css");
                }
            }
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

            //string replaceChar = (NonActiveX.Equals("F")) ? "<ZEOEDITOR></ZEOEDITOR>" : "<zeoeditor></zeoeditor>";
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
                        // 부서지정 이하
                        int deptID = node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();
                        carbonlist.Add(GetCarbonCopyUserInfo(node.SelectSingleNode("APPROVALTAG").InnerText, 0, deptID, "G", "Y", "1"));
                    }
                    else
                    {
                        // 사용자, 부서지정
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

            BE.UserInfo sendInfo = GetSendUserInfo("S", tbOpinion.Text);
            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();

            var rtnval= dpBll.Add(dpInfo, SealPath, sendInfo, isLast, isNextZQ);

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
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("APPROVAL_ADD_DS : " + ex.Message);
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

            if (_sequence.Equals(1))
            {
                info.Proxy = "N";
                info.ProxyUserID = 0;
            }

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

        #region 양식정보
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
            get { return GetViewStateString("FormZhtw", ""); }
            set { ViewState["FormZhtw"] = value; }
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
                bool rtnUserID = true;

                int docID = 0;

                if (FormBase.IndexOf("ERP문서") > -1 && NickName != "" && tbEtc1.Text.Trim() != "")
                {
                    DAL.DocumentProcess dal = new DAL.DocumentProcess();
                    int erpCnt = dal.GetDocumentUnionEtc(CurrentUserInfo.CompanyID, tbEtc1.Text.Trim(), tbEtc2.Text.Trim(), tbEtc3.Text.Trim(), tbEtc4.Text.Trim(),
                        tbEtc5.Text.Trim(), tbEtc6.Text.Trim(), tbEtc7.Text.Trim(), tbEtc8.Text.Trim(), tbEtc9.Text.Trim(), tbEtc10.Text.Trim());

                    if (erpCnt > 0)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "fail", "SubmitCloseEtc();");
                        return;
                    }
                }

                string BeforeUserID = string.Empty;
                string NowUserID = string.Empty;

                BeforeUserID = tbUserID.Text.ToString();
                NowUserID = CurrentUserInfo.UserID.ToString();

                if (BeforeUserID != NowUserID)
                {
                    rtnUserID = false;
                }

                if (rtnUserID)
                {
                    switch (EventArgs.ToUpper())
                    {
                        case "ADD":
                            docID = Addition();
                            rtnval = (docID > 0) ? true : false;
                            break;
                    }
                }
                else
                {
                    rtnval = false;
                }

                if (rtnval)
                {
                    string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');
                    //PDF생성
                    BLL.CreatePDF bll = new BLL.CreatePDF();
                    bll.ProcessPDF(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, docID, yearMonth, "1");

                    if (IsAfterSubmitCall.Equals("1"))
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "success", "SubmitAfterCall('" + this.CustomFilePath + "','" + docID.ToString() + "','" + this.GetResource("MSG_SUCCESS") + "');");
                    }
                    else
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "success", "SubmitClose('" + this.GetResource("MSG_SUCCESS") + "');");
                    }
                }
                else
                {
                    if (rtnUserID)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "fail", "SubmitClose('" + this.GetResource("MSG_FAIL") + "');");
                    }
                    else
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "fail", "SubmitClose('" + this.GetResource("Msg_ErrorUserInfo") + "');");
                    }
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