using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Home
{
    public partial class Content : Base.BasePageApproval
    {
        #region 변수설정
        public int AttachCnt = 0;
        public int ReplyCnt = 0;
        public int FavAppCnt = 0;
        public int ImpoCnt = 0;
        public string DefaultTab = "";        
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Home.Default).Assembly;	//어셈블리 선언
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
            // 총건수 가져오기
            DAL.Home dal = new DAL.Home();

            // 최근 첨부파일 사용내역(완결문서기준) 총건수
            AttachCnt = dal.GetAttachCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);
            ReplyCnt = dal.GetShortReplyCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            DAL.FavoriteApproval dal1 = new DAL.FavoriteApproval();
            FavAppCnt = dal1.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            DAL.ImpoUser dal2 = new DAL.ImpoUser();
            ImpoCnt = dal2.GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");

            // 기본탭
            Account.DAL.UserEnvironment envdal = new Account.DAL.UserEnvironment();
            List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = envdal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (list != null)
            {
                foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo info in list)
                {
                    DefaultTab = (info.ApprovalHomeMenu.Equals("")) ? "ListTotal" : info.ApprovalHomeMenu;
                }
            }

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
            //ListID = GetDecryptParam("listID", "").ToString();
            //DocID = GetDecryptParam("docID", "0").ToNumber<int>();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}