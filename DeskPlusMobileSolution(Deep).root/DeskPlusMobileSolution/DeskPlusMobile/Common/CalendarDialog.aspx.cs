using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using ComponentArt.Web.UI;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Zeospace.DeskPlusMobile.Web.Common
{
    public partial class CalendarDialog : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Common.CalendarDialog).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();

                GetDateForDDL();
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-08-21
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        private void GetDateForDDL()
        {
            //년 가져오기
            for (int i = DateTime.Now.Year - 50; i <= DateTime.Now.Year + 50; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString(), i.ToString());
                ddlStartYear.Items.Add(tmpItem);
            }

            ddlStartYear.SelectedValue = DateTime.Now.Year.ToString();

            //월 가져오기
            for (int i = 1; i <= 12; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString(), i.ToString());
                ddlStartMonth.Items.Add(tmpItem);
            }

            ddlStartMonth.SelectedValue = DateTime.Now.Month.ToString();

            //년 가져오기
            for (int i = DateTime.Now.Year - 50; i <= DateTime.Now.Year + 50; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString(), i.ToString());
                ddlEndYear.Items.Add(tmpItem);
            }

            ddlEndYear.SelectedValue = DateTime.Now.Year.ToString();

            //월 가져오기
            for (int i = 1; i <= 12; i++)
            {
                ListItem tmpItem = new ListItem(i.ToString(), i.ToString());
                ddlEndMonth.Items.Add(tmpItem);
            }

            ddlEndMonth.SelectedValue = DateTime.Now.Month.ToString();
        }
    }
}