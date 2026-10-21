using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.Approval.Web.Common.Folder
{
    public partial class ForwardList : Base.BasePageApproval
    {
        #region 변수설정

        public int DocumentID;
        public int TotalCount = 0; //총카운트
        public string RecordPage; //선택 리스트 개수

        #endregion 변수설정

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Folder.ForwardList).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        private void LoadData()
        {
            GetListCount();
            ddlListCount.SelectedValue = RecordPage;
        }
        #endregion

        #region 함수

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = GetResource("Close");
            }
        }

        /// <summary>
        /// 파라메터 가져오기
        /// </summary>
        private void LoadParam()
        {
            RecordPage = GetDecryptParam("RecordPage", "5");
            if (RecordPage.Length == 0 || RecordPage == "0") RecordPage = "5";
            DocumentID = GetDecryptParam("childDocID", "5").ToString().ToNumber<int>();

        }

        /// <summary>
        ///사용자 리스트 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            DAL.ListViewer list = new DAL.ListViewer();
            TotalCount = list.GetForwardListCount(CurrentUserInfo.CompanyID, DocumentID);
        }

        #endregion 함수

    }
}