using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.Account.Web.MyInfo.Etc
{
    public partial class AuthInfo : Base.BasePageAccount
    {
        #region 변수설정

        public string AuthTable = "";
        public int CompanyID = 0;
        public int UserID = 0;
        public int DeptID = 0;
        public string IsAdmin = "0";

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.Etc.AuthInfo).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //사용자 or 관리자 호출
            cbtnUser.Visible = (IsAdmin.Equals("0")) ? true : false;

            DAL.User dal = new DAL.User();
            DataTableReader dtr = dal.GetUserAuthInfo(CompanyID, UserID, DeptID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if (!(dtr["RoleName"].ToString().ToUpper() == "USERS"))
                        {
                            AuthTable += "<tr><td class='center'>" + dtr["RoleName"].ToString() + "</td></tr>";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            CompanyID = (GetDecryptParam("companyID", "0").ToNumber<int>().Equals(0)) ? CurrentUserInfo.CompanyID : GetDecryptParam("companyID", "0").ToNumber<int>();
            UserID = (GetDecryptParam("userID", "0").ToNumber<int>().Equals(0)) ? CurrentUserInfo.UserID : GetDecryptParam("userID", "0").ToNumber<int>();
            DeptID = (GetDecryptParam("deptID", "0").ToNumber<int>().Equals(0)) ? CurrentUserInfo.DeptID : GetDecryptParam("deptID", "0").ToNumber<int>();
            IsAdmin = GetDecryptParam("isadmin", "0");
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnUser.Text = resMng.GetString("Admin_UserManager_tabs0");    //"기본정보";
                cbtnList.Text = resMng.GetString("Admin_UserRegister_Close");
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}