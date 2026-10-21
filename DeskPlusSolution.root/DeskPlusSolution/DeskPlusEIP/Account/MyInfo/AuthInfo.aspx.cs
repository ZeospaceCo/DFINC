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

namespace Zeospace.DeskPlus.Account.Web.MyInfo
{
    public partial class AuthInfo : Base.BasePageAccount
    {
        public string AuthTable = "";
        public int CompanyID = 1;
        public int UserID = 0;
        public int DeptID = 0;

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.AuthInfo).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {                
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region 함수
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = resMng.GetString("BtnClose");
            }
        }

        /// <summary>
        /// 페이지로드
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.User dal = new DAL.User();
            DataTableReader dtr =  dal.GetUserAuthInfo(CompanyID, UserID, DeptID);

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
        /// 파라메터
        /// </summary>
        private void LoadParam()
        {
            CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
        }

        #endregion
    }
}