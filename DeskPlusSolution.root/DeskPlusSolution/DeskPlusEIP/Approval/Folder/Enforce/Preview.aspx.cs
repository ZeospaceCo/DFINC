using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.IO;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Enforce
{
    public partial class Preview : Base.BasePageApproval
    {
        #region 변수설정
        public string EventArgs = string.Empty;

        public int EnforceID;    // 공문신청ID
        public string FormPath = string.Empty;    // 시행문템플릿경로
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Enforce.Preview).Assembly;	//어셈블리 선언
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
                cbtnClose.Text = this.resMng.GetString("Close");
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
                        //SendType = info.SendType;   // 발송형태 - 메일발송(0)/등기(1)/팩스(2)
                        //StampType = info.StampType; // 온라인직인(0)/오프라인직인(1)
                        //DocType = info.DocType;     // 문서형태 - 온라인(0)/오프라인(1)
                        //IsSend = info.IsSend;    // 공문발송여부
                        FormPath = info.FormPath;    // 시행문템플릿경로

                        tbReceive.Text = info.Receive;
                        tbReference.Text = info.Reference;
                        tbEnforceDate.Text = info.EnforceDate;
                        tbExpFirst.Text = info.ExpFirst;
                        tbExpMid.Text = info.ExpMid;
                        tbExpEnd.Text = info.ExpEnd;
                        tbZipCode.Text = info.ZipCode;
                        tbAddressFirst.Text = info.AddressFirst;
                        tbAddressEnd.Text = info.AddressEnd;
                        tbPhone.Text = info.Phone;
                        tbFax.Text = info.Fax;
                        tbHomepage.Text = info.Homepage;
                        tbTargetMail.Text = info.TargetMail;
                        tbTargetMailSubject.Text = info.TargetMailSubject;

                        tbSubject.Text = info.Subject;
                        htaEditorHTML.Value = info.ContentHTML;

                        tbDocNumber.Text = info.DocNumber;
                        DateTime dt;
                        if (DateTime.TryParse(info.WriteDate.ToString(), out dt))
                        {
                            tbWriteTime.Text = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        }
                        if (DateTime.TryParse(info.CompleteDate.ToString(), out dt))
                        {
                            tbCompleteDate.Text = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        }
                    }
                }
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}