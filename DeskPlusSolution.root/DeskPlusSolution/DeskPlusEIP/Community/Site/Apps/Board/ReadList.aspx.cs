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

namespace Zeospace.DeskPlus.Community.Web.Site.Apps.Board
{
    public partial class ReadList : Base.BasePageCommunity
    {
        #region 변수설정
        public int CommunityID = 0;
        public int MenuID = 0;
        public int BoardID = 0;
        public int TotalCount = 0; //총카운트
        public int CurrentPage = 0;
        public string RecordPerPage; //게시판 리스트 개수
        
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
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Board.ReadList).Assembly;	//어셈블리 선언
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
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            ddlListCount.SelectedValue = RecordPerPage;

            GetListCount();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            BoardID = GetDecryptParam("boardID", "0").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            DAL.BoaView dal = new DAL.BoaView();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, CommunityID, MenuID, BoardID);
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion

    }
}