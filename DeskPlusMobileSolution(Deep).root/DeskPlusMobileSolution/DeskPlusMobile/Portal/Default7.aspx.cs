using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Portal
{
    public partial class Default7 : System.Web.UI.Page
    {
        #region Declaration (컨트롤 선언)
        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.Default).Assembly;	//어셈블리 선언
            #endregion

            if (!Page.IsPostBack)
            {
                GetMultiCompany();

                //쿠키값이 있으면
                if (Request.Cookies["DPMAuth"] != null)
                {
                    string userStr = Request.Cookies["DPMAuth"].Value;

                    try
                    {
                        Helper.Security.TripleDESManager tds = new Helper.Security.TripleDESManager();
                        userStr = tds.Decrypt(userStr.Trim(), System.Text.Encoding.UTF8);

                        string[] userArr = userStr.Split(';');

                        if (userArr.Length > 1)
                        {
                            if (userArr[0].Trim().Length != 0)
                            {
                                tbLoginID.Text = userArr[0];
                            }

                            if (userArr[1].Trim().Length != 0)
                            {
                                tbPassword.Text = userArr[1];
                            }

                            this.chkRemember.Checked = true;
                        }
                        else
                        {
                            this.chkRemember.Checked = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Helper.AppException.WriteException("Login.aspx : ", ex);
                    }
                }

                if (Request.Cookies["DPMComanyAuth"] != null)
                {
                    try
                    {
                        ddlCompany.SelectedValue = Request.Cookies["DPMComanyAuth"].Value;
                        this.chkCompany.Checked = true;
                    }
                    catch (Exception ex)
                    {
                        this.chkCompany.Checked = false;
                        Helper.AppException.WriteException("Default.aspx : ", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-04-19
        /// 작성목적 : 회사 정보 가져오기
        /// </summary>
        private void GetMultiCompany()
        {
            string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_Company");
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "CompanyInfo", null);

            ddlCompany.Items.Clear();

            string companyName = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {

                companyName = StringHelper.GetLangName(
                    dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                    dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLangs);


                ddlCompany.Items.Add(new ListItem(companyName.ToReverseXSS(), dr["CompanyID"].ToString()));

            }
        }

        // <summary>
        /// 다국어 리소스 인스턴스 메소드
        /// </summary>
        /// <remarks></remarks>
        private void GetResourceManager()
        {
            resMng = new System.Resources.ResourceManager(ResourceName, AssemblyName);
        }

        /// <summary>
        /// 다국어 리소스 값 반환 메소드
        /// </summary>
        /// <param name="_resName">Name of the _res.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetResource(string _resName)
        {
            return new System.Resources.ResourceManager(ResourceName, AssemblyName).GetString(_resName);
        }

        /// <summary>
        /// Remember
        /// </summary>
        public string Remember { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// 다국어
        /// </summary>
        private string PreLangs
        {
            get
            {
                try
                {
                    return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "DefaultLanguage");
                }
                catch
                {
                    return "";
                }
            }
        }

    }
}