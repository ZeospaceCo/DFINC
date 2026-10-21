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

namespace Zeospace.DeskPlus.Approval.Web.Folder.EnforceAdmin
{
    public partial class Approve : Base.BasePageApproval
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        public string StampImage = string.Empty;
        public string CabinetID;
        public int EnforceID;
        public int EnforceStampID;
        public int StampDeptID;
        public string State = string.Empty;
        public int ReqUserID;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.EnforceAdmin.Approve).Assembly;	//어셈블리 선언
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
            LoadEnforceInfo(EnforceID);
        }
        #endregion

        #region (함수)
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();

            EnforceID = GetDecryptParam("enforceID", "0").ToNumber<int>();
            CabinetID = GetDecryptParam("cabinetID", "");
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
                cbtnDel.Text = resMng.GetString("Btn_Del");
                cbtnPreview.Text = resMng.GetString("Preview");
                cbtnApprove.Text = resMng.GetString("ApprovalTypeS");
                cbtnReject.Text = resMng.GetString("ApprovalTypeR");
                CbtnCancel.Text = resMng.GetString("List");     //"취소";

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
                    item.Text = this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
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
        /// 공문정보
        /// </summary>
        /// <param name="_documentID"></param>
        private void LoadEnforceInfo(int _enforceID)
        {
            if (_enforceID > 0)
            {
                DAL.Enforce dal = new DAL.Enforce();
                List<BE.EnforceInfo> list = dal.GetEnforce(CurrentUserInfo.CompanyID, _enforceID);
                if (list != null)
                {
                    foreach (BE.EnforceInfo info in list)
                    {
                        ddlDept.SelectedValue = info.StampDeptID.ToString();
                        if (ddlDept.SelectedItem != null)
                        {
                            lbDept.Text = ddlDept.SelectedItem.Text;
                        }
                        lbDocNumber.Text = info.DocNumber;
                        lbSubject.Text = info.Subject;
                        lbReceive.Text = info.Receive;
                        lbReference.Text = info.Reference;
                        lbEnforceDate.Text = info.EnforceDate;
                        lbZipCode.Text = info.ZipCode;
                        lbAddressFirstBase.Text = info.AddressFirst;
                        lbAddressEndBase.Text = info.AddressEnd;
                        lbPhoneBase.Text = info.Phone;
                        lbFaxBase.Text = info.Fax;
                        lbHomepage.Text = info.Homepage;
                        ddlSendType.SelectedValue = info.SendType;
                        if (ddlSendType.SelectedItem != null)
                        {
                            lbSendType.Text = ddlSendType.SelectedItem.Text;
                        }
                        lbQuantity.Text = info.Quantity;
                        lbTargetMail.Text = info.TargetMail;
                        lbTargetMailSubject.Text = info.TargetMailSubject;
                        ddlDocType.SelectedValue = info.DocType;
                        if (ddlDocType.SelectedItem != null)
                        {
                            lbDocType.Text = ddlDocType.SelectedItem.Text;
                        }
                        ddlStampType.SelectedValue = info.StampType;
                        if (ddlStampType.SelectedItem != null)
                        {
                            lbStampType.Text = ddlStampType.SelectedItem.Text;
                        }
                        lbPurpose.Text = info.Purpose;
                        ddlFormPath.SelectedValue = info.FormPath;
                        if (ddlFormPath.SelectedItem != null)
                        {
                            lbFormPath.Text = ddlFormPath.SelectedItem.Text;
                        }
                        htaEditorHTML.Value = info.ContentHTML;

                        EnforceStampID = info.EnforceStampID;
                        StampDeptID = info.StampDeptID;
                        State = info.State;
                        ReqUserID = info.ReqUserID;
                    }
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

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}