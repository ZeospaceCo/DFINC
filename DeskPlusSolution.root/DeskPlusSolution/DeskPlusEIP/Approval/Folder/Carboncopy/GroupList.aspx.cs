using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy
{
    public partial class GroupList : Base.BasePageApproval
    {
        #region 변수설정
        public int ChildDocID;
        public int GroupID;
        public int DocumentID;

        public string DocName = string.Empty;           // 문서명
        public string WriteName = string.Empty;         // 작성자
        public string WriteDate = string.Empty;         // 작성일자
        public string DocSubject = string.Empty;        // 제목   
        public string DeptName = string.Empty;          // 부서명
        public string TitleName = string.Empty;         // 직급명

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy.GroupList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadParam();
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnCancel.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            ChildDocID = GetDecryptParam("childDocID", "").ToNumber<int>();
            DocumentID = GetDecryptParam("documentID", "").ToNumber<int>();
        }

        #endregion
    }
}