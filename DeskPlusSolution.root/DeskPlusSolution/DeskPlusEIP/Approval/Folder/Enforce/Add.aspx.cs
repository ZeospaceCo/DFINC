using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.DAL;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Enforce
{
    public partial class Add : Base.BasePageApproval
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        public string StampImage = string.Empty;
        public string CabinetID;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Enforce.Add).Assembly;	//어셈블리 선언
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
            LoadStampInfo();    //직인리스트
            LoadFormPath();     //템플릿리스트
            LoadDocInfo(DocumentID);
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
                CbtnConfirm.Text = resMng.GetString("Btn_Save");
                CbtnCancel.Text = resMng.GetString("Cancel");     //"취소";

                //발송형태 - 메일발송(0)/등기(1)/팩스(2)
                ddlSendType.Items.Add(new ListItem(resMng.GetString("EnforceSendTypeItem0"), "0"));
                ddlSendType.Items.Add(new ListItem(resMng.GetString("EnforceSendTypeItem1"), "1"));
                ddlSendType.Items.Add(new ListItem(resMng.GetString("EnforceSendTypeItem2"), "2"));

                //문서형태 - 온라인(0)/오프라인(1)
                ddlDocType.Items.Add(new ListItem(resMng.GetString("EnforceDocTypeItem0"), "0"));
                ddlDocType.Items.Add(new ListItem(resMng.GetString("EnforceDocTypeItem1"), "1"));

                //직인형태 - 온라인직인(0)/오프라인직인(1)
                ddlStampType.Items.Add(new ListItem(resMng.GetString("EnforceStampTypeItem0"), "0"));
                ddlStampType.Items.Add(new ListItem(resMng.GetString("EnforceStampTypeItem1"), "1"));
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
            DocumentID = GetDecryptParam("documentID", "0").ToNumber<int>();
            CabinetID = GetDecryptParam("cabinetID", "");
        }

        /// <summary>
        /// 시행문직인정보
        /// </summary>
        /// <param name="_deptID"></param>
        private void LoadStampInfo()
        {
            DAL.EnforceStamp dal = new DAL.EnforceStamp();
            List<BE.EnforceStampInfo> list = dal.GetEnforceStamp(CurrentUserInfo.CompanyID);

            if (list != null)
            {
                ddlDept.Items.Clear();
                ddlDept.Items.Add(new ListItem(this.GetResource("EnforceMsg00"), ""));

                foreach (BE.EnforceStampInfo info in list)
                {
                    ListItem item = new ListItem();
                    item.Text =this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                    item.Value = info.DeptID.ToString();
                    item.Attributes.Add("class", "stampID_" + info.DeptID.ToString());
                    item.Attributes.Add("stampID", info.EnforceStampID.ToString());

                    ddlDept.Items.Add(item);
                }

                if (ddlDept.Items.FindByValue(CurrentUserInfo.RootDeptID.ToString()) != null)
                {
                    ddlDept.SelectedValue = CurrentUserInfo.RootDeptID.ToString();
                }
            }
        }
        /// <summary>
        /// 템플릿 리스트 가져오기
        /// </summary>
        private void LoadFormPath()
        {
            DAL.EnforceForm dal = new DAL.EnforceForm();
            List<BE.EnforceFormInfo> list = dal.GetEnforceForm(CurrentUserInfo.CompanyID, true);
            if (list != null)
            {
                foreach (BE.EnforceFormInfo info in list)
                {
                    ListItem item = new ListItem();
                    item.Text = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                    item.Value = info.FormPath;

                    ddlFormPath.Items.Add(item);
                }
                ddlFormPath.SelectedIndex = -1;
            }
        }
        /// <summary>
        /// 문서정보
        /// </summary>
        /// <param name="_documentID"></param>
        private void LoadDocInfo(int _documentID)
        {
            DAL.Document dal = new DAL.Document();
            List<BE.DocumentInfo> list = dal.GetDocList(CurrentUserInfo.CompanyID, _documentID);
            if (list != null)
            {
                foreach (BE.DocumentInfo info in list)
                {
                    htaFormHTML.Value = info.ContentHTML;
                    ctbDocNumber.Text = info.DocNumber;
                    ctbSubject.Text = info.Subject;
                }
            }
        }

        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        private int Addition()
        {
            BE.EnforceInfo info = new BE.EnforceInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ReqUserID = CurrentUserInfo.UserID;
            info.ReqUserMail = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;
            info.EnforceStampID = tbEnforceStampID.Text.ToNumber<int>();
            info.StampDeptID = tbEnforceDeptID.Text.ToNumber<int>();
            info.DocumentID = DocumentID;
            info.Subject = ctbSubject.Text;
            info.ContentHTML = htaEditorHTML.Value;
            info.Receive = ctbReceive.Text;
            info.Reference = ctbReference.Text;
            info.EnforceDate = ctbEnforceDate.Text;
            info.ExpFirst = tbExpFirst.Text;
            info.ExpMid = tbExpMid.Text;
            info.ExpEnd = tbExpEnd.Text;
            info.ZipCode = ctbZipCode.Text;
            info.AddressFirst = ctbAddressFirstBase.Text;
            info.AddressEnd = ctbAddressEndBase.Text;
            info.Phone = ctbPhoneBase.Text;
            info.Fax = ctbFaxBase.Text;
            info.Homepage = ctbHomepage.Text;
            info.SendType = ddlSendType.SelectedValue;
            info.StampType = ddlStampType.SelectedValue;
            info.DocType = ddlDocType.SelectedValue;
            info.Quantity = ctbQuantity.Text;
            info.Purpose = ctbPurpose.Text;
            info.TargetMail = (info.SendType.Equals("0")) ? ctbTargetMail.Text : "";
            info.TargetMailSubject = (info.SendType.Equals("0")) ? ctbTargetMailSubject.Text : "";
            info.FormPath = ddlFormPath.SelectedValue;

            info.AlarmSubject = this.GetResource("EnforceAlarmSubjectA");
            info.AlarmContent = this.GetResource("EnforceAlarmContentA");

            BLL.Enforce bll = new BLL.Enforce();
            BE.UserInfo sendInfo = GetSendUserInfo("", "");
            return bll.Add(info, sendInfo);
        }

        #endregion

        #region (프로퍼티)
        /// <summary>
        /// DocumentID
        /// </summary>
        public int DocumentID
        {
            get { return GetViewStateInt("DocumentID", "0"); }
            set { ViewState["DocumentID"] = value; }
        }
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
                bool rtnval = false;
                switch (EventArgs.ToUpper())
                {
                    case "ADD":
                        rtnval = (Addition() > 0) ? true : false;
                        break;
                }

                if (rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", this.GetResource("MSG_SUCCESS") + this.GetResource("Msg_EnforceSubmit") , "Add.Cancel();");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FAIL"), "Add.Cancel();");
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