using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Company
{
    public partial class List : Base.BasePageEIPAdmin
    {
        #region 변수설정
        public int CompanyID;        
        public int CurrentPage = 0;
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = ""; //게시판 리스트 개수                
        public string Order = string.Empty; //sort  
        public string UserCnt = "0";

        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Company.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                //슈퍼 관리자
                if (!CurrentUserInfo.IsSysAdmin)
                {
                    Response.Redirect("/DeskPlusEIP/");
                    return;
                }

                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (!CurrentUserInfo.IsSysAdmin) Response.Redirect("/DeskPlusEIP/");

            GetListCount();
            GetUserCount();

            ddlListCount.SelectedValue = RecordPerPage;
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();            
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "CompanyID ASC" : Order;
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnActive.Text = resMng.GetString("ActiveUnActive");
                cbtnLic.Text = resMng.GetString("LicenseManage");
                cbtnDel.Text = resMng.GetString("Del");
                cbtnModLicense.Text = resMng.GetString("ModifyLicenseCnt");

            }
        }


        /// <summary>
        /// 작성자   : shimpark
        /// 회사 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = null;
            Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
            al = (CompanyID > 0) ? dal.GetCompany(CompanyID, false) : dal.GetCompany(false);

            TotalCount = al.Count.ToString();
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 전체 사용자 카운트 가져오기
        /// </summary>
        private void GetUserCount()
        {
            Account.DAL.User dal = new Account.DAL.User();
            UserCnt = dal.GetCountUser(0, true).ToString();
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}