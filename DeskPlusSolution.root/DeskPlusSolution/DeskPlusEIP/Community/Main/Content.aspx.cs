using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Main
{
    public partial class Content : Base.BasePageCommunity
    {
        #region 변수설정
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수   
        public string Order = String.Empty; //sort            
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Main.Content).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2012-05-07
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (AccessAuth)
            {
                cbtnAdmin.Visible = true;
            }
            else
            {
                cbtnAdmin.Visible = false;
            }


            #region PageSize 설정하기

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.COMMUNITY, "ListCount", "10");//페이지리스트수

            DAL.Setting dal = new DAL.Setting();
            List<BE.SettingInfo> al = dal.GetList(CurrentUserInfo.CompanyID, 0, 0, CurrentUserInfo.UserID, "ML");

            foreach (BE.SettingInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = RecordPerPage;

            #endregion
            

            GetListCount();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdmin.Text = resMng.GetString("Admin");
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "WriteDate desc" : Order;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-07
        /// 작성목적 : 커뮤니티 총 갯수
        /// </summary>
        private void GetListCount()
        {   
            DAL.Com dal = new DAL.Com();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID).ToString();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}