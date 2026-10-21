using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.Address.Web.Content
{
    public partial class AddPrivate : Base.BasePageAddress
    {
        #region 변수설정
        public string TopDeptID = string.Empty;
        public string MailLogin;

        public int CompanyID;
        public int UserID;

        public int SharAddressUserID;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParam();         
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Content.AddPrivate).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }

        }

        #region 함수
        private void LoadParam()
        {
            MailLogin = CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain;            
        }

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
                
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
                cbtnSearch.Text = resMng.GetString("Search");       //"검색";
                cbtnAddPrivate.Text = resMng.GetString("AddPersonal");       //"개인주소록추가";
                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FirstName"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("EmailEx"), "Mail"));
            }
        }

        #endregion

        #region 컨트롤에 데이터를 바인딩 하는 메소드
        private void LoadData()
        {
            ViewState["PreLang"] = PreLang;
            //특정권한을 가진 사용자의  UserID
            Zeospace.DeskPlus.EIP.DAL.UserRole dal = new EIP.DAL.UserRole();
            DataTableReader dr = dal.GetUserRoleCodeList(CompanyID, "SharAddressUser");
            SharAddressUserID = 0;
            if (dr != null)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SharAddressUserID = dr["UserID"].ToString().ToNumber<int>();
                    }
                }
            }

            if (Request.UrlReferrer != null)
            {
                AppException.WriteAssert("AddressSearch referer : " + Request.UrlReferrer.ToString(), AppSettingType.MAIL);
            }

            UCAddressMailTree.PreLang = PreLang;
            UCAddressMailTree.CompanyID = 0;
            UCAddressMailTree.UserID = 0;

            if (MailLogin.Length == 0)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("AddressSearch"))
                {
                    string strScript = @"<script type=""text/javascript"">alert('{0}');self.close();" + "<" + "/script>";
                    strScript = String.Format(strScript, "please send me 'loginID' parameter value.");
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "AddressSearch", strScript);

                    
                    cbtnClose.Visible = false;

                    return;
                }
            }



            if (MailLogin.Length > 0)
            {
                if (MailLogin.IndexOf('@') != -1)
                {
                    Zeospace.DeskPlus.Account.DAL.UserMailDomain uMail = new Account.DAL.UserMailDomain();
                    var mailList = uMail.GetUserMailDomain(MailLogin.Split('@')[0], MailLogin.Split('@')[1]);

                    foreach (var info in mailList)
                    {
                        CompanyID = info.CompanyID;
                        UserID = info.UserID;

                        UCAddressMailTree.CompanyID = info.CompanyID;
                        UCAddressMailTree.UserID = info.UserID;
                        
                    }
                }
            }

            if (CompanyID == 0 || UserID == 0)
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("AddressSearch"))
                {
                    string strScript = @"<script type=""text/javascript"">alert('{0}');self.close();" + "<" + "/script>";
                    strScript = String.Format(strScript, "This user does not exist.");
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "AddressSearch", strScript);

                    
                    cbtnClose.Visible = false;

                    return;

                }
            }

            //if (Request.Cookies["ss_webmail_id"]["Mail"] != null)
            //{
            //    Mail = Request.Cookies["ss_webmail_id"]["Mail"].ToString();
            //    LoginCheck();
            //}
        }

        #endregion
    }
}