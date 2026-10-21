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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.DAL;

namespace Zeospace.DeskPlus.Approval.Web.FormFlow.Menu
{
    public partial class TreeModReceive : Base.BasePageApproval
    {
        #region 변수설정
        public int DocumentID = 0;
        public int GroupID = 0;
        public int Sequence = 0;
        public string ApprovalTag = "";
        public string AccountTag = "";
        public int TargetUserID = 0;
        public int TargetDeptID = 0;
        public string FolderStatusID = "";
        public int ProxyUserID = 0;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Menu.TreeModReceive).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                this.tbOpinion.Attributes.Add("maxlength", "500");

                LoadParam();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2011-10-18
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
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            ProxyUserID = GetDecryptParam("proxyUserID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            // 0. 초기화 
            initData();
        }

        private void initData()
        {
            #region 조직도 초기화
            UCDeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCDeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;

            UCDeptUserTreeScript1.UseActiveDept = true;
            UCDeptUserTreeScript1.UseIcon = true;
            UCDeptUserTreeScript1.UseMyRootDept = false;
            UCDeptUserTreeScript1.UseVirtualTree = false;

            UCDeptUserTreeScript1.UseRootCheckBox = false;
            UCDeptUserTreeScript1.UseCheckBoxGroup = false;
            UCDeptUserTreeScript1.UseCheckBoxMember = false;
            #endregion
        }

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}