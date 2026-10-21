using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Visit.Web.Content
{
    public partial class AddRepeat : Zeospace.DeskPlus.Visit.Web.Base.BasePageVisit
    {
        #region 변수처리
        public string StartDate;
        protected DateTime dtTargetDate;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Content.AddRepeat).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2014-02-07
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            txtcalendar.Value = StartDate;
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");       //"확인";
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
  
                ddlorder.Items.Clear();
                ddlorder.Items.Add(new ListItem(this.resMng.GetString("Repeat9"), "1"));
                ddlorder.Items.Add(new ListItem(this.resMng.GetString("Repeat10"), "2"));
                ddlorder.Items.Add(new ListItem(this.resMng.GetString("Repeat11"), "3"));
                ddlorder.Items.Add(new ListItem(this.resMng.GetString("Repeat12"), "4"));
                ddlorder.Items.Add(new ListItem(this.resMng.GetString("Repeat13"), "5"));

                ddlkind.Items.Clear();
                ddlkind.Items.Add(new ListItem(this.resMng.GetString("Repeat14"), "1"));
                ddlkind.Items.Add(new ListItem(this.resMng.GetString("Repeat15"), "2"));
                ddlkind.Items.Add(new ListItem(this.resMng.GetString("Repeat16"), "3"));
                ddlkind.Items.Add(new ListItem(this.resMng.GetString("Sun"), "4"));
                ddlkind.Items.Add(new ListItem(this.resMng.GetString("Mon"), "5"));
                ddlkind.Items.Add(new ListItem(this.resMng.GetString("Tue"), "6"));
                ddlkind.Items.Add(new ListItem(this.resMng.GetString("Wed"), "7"));
                ddlkind.Items.Add(new ListItem(this.resMng.GetString("Thu"), "8"));
                ddlkind.Items.Add(new ListItem(this.resMng.GetString("Fri"), "9"));
                ddlkind.Items.Add(new ListItem(this.resMng.GetString("Sat"), "10"));
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-07
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            StartDate = GetDecryptParam("startDate", "");
        }

        #endregion


        #region (프로퍼티)
      
        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}