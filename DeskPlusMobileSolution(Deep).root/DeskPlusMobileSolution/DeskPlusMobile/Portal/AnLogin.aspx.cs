using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Portal.Ws.BE;
using System.Data;
using System.Reflection;

namespace Zeospace.DeskPlusMobile.Web.Portal
{
    public partial class AnLogin : System.Web.UI.Page
    {
        #region Declaration (컨트롤 선언)
        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public string LoginImage = "";
        public string BGLoginImage = "";
        public Assembly AssemblyName { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Portal.AnLogin).Assembly;	//어셈블리 선언
            #endregion

            if (!Page.IsPostBack)
            {
                GetMultiCompany();

                string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "BaseFileDirectory");
                string ciLoginImage = baseFileDirectory + ddlCompany.SelectedValue + "/lgn_logo.png";
                string ciBGLoginImage = baseFileDirectory + ddlCompany.SelectedValue + "/bg.jpg";
                //회사별 이미지가 있다면?
                if (System.IO.File.Exists(Server.MapPath(ciLoginImage)))
                {
                    LoginImage = ciLoginImage;
                }
                else
                {
                    //회사별 이미지가 없다면, 기본 이미지 
                    LoginImage = baseFileDirectory + ddlCompany.SelectedValue + "/Login/lgn_logo.png";
                }

                if (System.IO.File.Exists(Server.MapPath(ciBGLoginImage)))
                {
                    BGLoginImage = ciBGLoginImage;
                }
                else
                {
                    //회사별 이미지가 없다면, 기본 이미지 
                    BGLoginImage = baseFileDirectory + ddlCompany.SelectedValue + "/DStyle/bg1.jpg";
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