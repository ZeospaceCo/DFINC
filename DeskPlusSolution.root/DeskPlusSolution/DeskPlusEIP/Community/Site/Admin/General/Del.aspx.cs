using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Community.Web.Site.Admin.General
{
    public partial class Del : Base.BasePageCommunitySysop
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
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Admin.General.Del).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnDel.Text = resMng.GetString("Dissolution");           //해체;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 커뮤니티 해체
        /// </summary>
        private void DelCom()
        {
            bool rtnval = false;

            DAL.Com dal = new DAL.Com();
            rtnval = dal.Mod(CurrentUserInfo.CompanyID, CommunityID, ctbDelDes.Text, 0);

            if (rtnval)
            {   
                string sc = "Del.GoMain();";
                Utility.WriteScript(Page, "success", sc);
            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }

        }


        #endregion

        #region (프로퍼티)

        /// <summary>
        /// ParentReservationID
        /// </summary>
        public int CommunityID
        {
            get { return GetViewStateInt("CommunityID", "0"); }
            set { ViewState["CommunityID"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 커뮤니티 해체
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            DelCom();
        }


        #endregion
    }
}