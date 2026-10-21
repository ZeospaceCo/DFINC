using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BirthDay : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region 변수설정
        public string NowDate = string.Empty;
        public int ListCount;
        public string ScriptJS = "";
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.BirthDay).Assembly;	//어셈블리 선언
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
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        public void LoadData()
        {
            LoadParam();            

            //NowDate = System.DateTime.Now.ToShortDateString();

            //DateTime dt;

            //if (DateTime.TryParse(NowDate, out dt))
            //    NowDate = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);

            //BirthDayStartDate.Value = NowDate;
            //BirthDayEndDate.Value = NowDate;

            this.ddlBirthDayStartMonth.Items.Clear();
            this.ddlBirthDayEndMonth.Items.Clear();
            this.ddlBirthDayStartDay.Items.Clear();
            this.ddlBirthDayEndDay.Items.Clear();

            for (int month = 1; month < 13; month++)
            {
                ddlBirthDayStartMonth.Items.Add(new ListItem(month.ToString(), month.ToString()));
                ddlBirthDayEndMonth.Items.Add(new ListItem(month.ToString(), month.ToString()));
            }

            for (int day = 1; day < 32; day++)
            {
                ddlBirthDayStartDay.Items.Add(new ListItem(day.ToString(), day.ToString()));
                ddlBirthDayEndDay.Items.Add(new ListItem(day.ToString(), day.ToString()));
            }

            ddlBirthDayStartMonth.SelectedValue = DateTime.Now.Month.ToString();
            ddlBirthDayEndMonth.SelectedValue = DateTime.Now.Month.ToString();

            ddlBirthDayStartDay.SelectedValue = DateTime.Now.Day.ToString();
            ddlBirthDayEndDay.SelectedValue = DateTime.Now.Day.ToString();

        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {
            ListCount = (ListCount == 0) ? 4 : ListCount;
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        
        #endregion
    }
}