using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.Task.Web.Common
{
    public partial class ForwardList : Base.BasePageTask
    {
        #region 변수설정
                
        public int TotalCount = 0; //총카운트
        public string RecordPerPage; //선택 리스트 개수

        #endregion 변수설정

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Common.ForwardList).Assembly;	//어셈블리 선언

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
            ddlListCount.SelectedValue = RecordPerPage;
        }
        #endregion

        #region 함수

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     :
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
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 파라메터 가져오기
        /// </summary>
        private void LoadParam()
        {            
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            if (RecordPerPage.Length == 0 || RecordPerPage == "0") RecordPerPage = "5";
        }

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 사용자 리스트 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            DAL.ListViewer list = new DAL.ListViewer();
            TotalCount = list.GetForwardListCount(CurrentUserInfo.CompanyID, ListID);
        }

        #endregion 함수
    }
}