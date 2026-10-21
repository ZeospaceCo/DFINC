using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data.SqlClient;
using System.Data;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.RM
{
    public partial class Folder : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.RM.Folder).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadAuth();

            }
        }

        #region 함수

        /// <summary>
        /// 메뉴 권한체크
        /// </summary>
        private void LoadAuth()
        {
            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLRM, "SEL_QRY_ListUser_UserID");
            sql = string.Format(sql, CompanyID.ToString(), UserID.ToString());
            object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, null);

            if (Convert.ToInt32(obj) > 0)
            {
                MenuAdmin.Visible = true;
            }
        }

        /// <summary>
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        #endregion


    }
}