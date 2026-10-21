using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.MDM.Web.Send
{
    public partial class ReceiveList : Base.BasePageMDM
    {
        #region 변수설정
        public int SendID = 0;
        public int TotalCount = 0; //총카운트
        public string RecordPerPage; 
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
            this.ResourceName = "Zeospace.DeskPlus.MDM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.MDM.Web.Send.ReceiveList).Assembly;	//어셈블리 선언
            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                GetListCount();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     :
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
        /// 작성자 : famo23
        /// 작성일자 : 2020-06-02
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            SendID = GetDecryptParam("sendID", "0").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            if (RecordPerPage.Length == 0 || RecordPerPage == "0") RecordPerPage = "5";

            ddlListCount.SelectedValue = RecordPerPage;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-06-02
        /// 총 카운트
        /// </summary>
        private void GetListCount()
        {
            DAL.Receive dal = new DAL.Receive();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, SendID);
        }

        #endregion (함수)
    }
}