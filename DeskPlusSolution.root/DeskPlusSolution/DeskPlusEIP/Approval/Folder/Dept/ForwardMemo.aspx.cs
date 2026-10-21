using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;
using System.IO;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Dept
{
    public partial class ForwardMemo : Base.BasePageApproval
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정

        public string DocID;

        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Dept.ForwardMemo).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                GetParam();
                SetLanguage();
                LoadData();
            }
        }



        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseCheckBoxGroup = false;
            DeptUserTreeScript1.UseCheckBoxMember = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = this.resMng.GetString("Add");
                cbtnShortReply.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void GetParam()
        {
            DocID = GetDecryptParam("docID", "");
        }

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
            seqInfo.DocumentID = 0;
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
                dpInfo.CirUserList = userList.Value;
            }

            return dpInfo;
        }

        /// <summary>
        /// 전달알람
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private bool ApproveFor(int _companyID, int _documentID)
        {
            BE.DocumentProcessInfo dpInfo = GetDocInfo(_companyID, _documentID);
            if (dpInfo == null) return false;

            BE.SequenceInfo seqInfo = GetSeqInfo("S");
            if (seqInfo == null) return false;

            BE.UserInfo sendInfo = GetSendUserInfo("S", tbOpinion.Text);
            BLL.DocumentProcess dpBll = new BLL.DocumentProcess();
            return dpBll.ApproveFor(dpInfo, seqInfo, IsLast, IsZQ, sendInfo);
        }
        #endregion

        #region (속성)

        #endregion

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
        private void CompletePopup()
        {
            string message = GetResource("MSG_SUCCESS").Replace("\\", "\\\\").Replace("'", "\\'");
            string script = "alert('" + message + "'); if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }";
            ClientScript.RegisterStartupScript(this.GetType(), "CompletePopup", script, true);
        }

        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int DocID = documentId.Value.ToString().ToNumber<int>();
                bool rtnval = false;
                rtnval = ApproveFor(CurrentUserInfo.CompanyID, DocID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
            }
            finally
            {
                CompletePopup();
            }
        }
        #endregion
    }
}
