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
    public partial class SetViewing : Base.BasePageApproval
    {
        #region 변수설정

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
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			    //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Process.Basic.SetViewing).Assembly;	//어셈블리 선언
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
            DAL.Sequence seqDal = new DAL.Sequence();
            seqDal.ModViewing(CurrentUserInfo.CompanyID, DocumentID, Sequence, ApprovalTag, Viewing).ToString();
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
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            DocumentID = GetDecryptParam("documentID", "0").ToNumber<int>();
            Sequence = GetDecryptParam("sequence", "0").ToNumber<int>();
            ApprovalTag = GetDecryptParam("approvalTag", "");
            Viewing = GetDecryptParam("viewing", "");
        }

        #endregion



        #region (프로퍼티)

        public int DocumentID
        {
            get { return GetViewStateInt("DocumentID", "0"); }
            set { ViewState["DocumentID"] = value; }
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
        public string Viewing
        {
            get { return GetViewStateString("Viewing", ""); }
            set { ViewState["Viewing"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}