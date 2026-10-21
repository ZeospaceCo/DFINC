using System;

namespace Zeospace.DeskPlus.Portlet.Web.Package.Board
{
    public partial class ListTot : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        public int BoardTotalCount { get; set; }

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.Board.ListTot).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        public void LoadData()
        {
            GetListCount();
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시판 총 카운트 가져오기
        /// 작성자     : shine
        /// 최초작성일 : 2011-05-09
        /// </summary>
        private void GetListCount()
        {
            try
            {
                Zeospace.DeskPlus.Portlet.DAL.Board dal = new DAL.Board();
                BoardTotalCount = dal.GetBoardTotCnt(CurrentUserInfo.CompanyID);
            }
            catch (Exception ex)
            {
                BoardTotalCount = 0;
                //throw;
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
            }
        }

        #endregion (함수)


    }
}