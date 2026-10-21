using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.ETC.Web.Pay.Menu
{
    public partial class Tree : Base.BasePageETC
    {
        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Pay.Menu.Tree).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                GetDefaultMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            }
        }
        #endregion

        #region (함수)
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 회계문서 초기메뉴값 가져오기
        /// 작성자     : 김형석
        /// 최초작성일 : 2012-07-02
        /// </summary>
        /// <remarks></remarks>
        private void GetDefaultMenu(int _companyID, int _userID)
        {
            // 메뉴 포커스
            UCFolderStatusTreeScript1.FolderStatusID = FolderStatusID;     
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            FolderStatusID = GetDecryptParam("folderStatusID", "");          
            FormID = GetDecryptParam("formID", "0");
        }
        #endregion


        #region Property (프로퍼티)
        public string FolderStatusID { get; set; }       
        public string FormID { get; set; }
        #endregion
    }
}