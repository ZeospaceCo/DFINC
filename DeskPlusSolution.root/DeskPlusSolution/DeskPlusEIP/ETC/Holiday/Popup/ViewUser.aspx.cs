using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.ETC.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.ETC.Web.Holiday.Popup
{
    public partial class ViewUser : Base.BasePageETC
    {
        #region 변수설정
        public string UserID = string.Empty;
        public string Name = string.Empty;        
        public string HoliAll1 = string.Empty;
        public string HoliJan1 = string.Empty;
        public string TitleDate = string.Empty;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			    //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Holiday.Popup.ViewUser).Assembly;	    //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 김형석
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            ltHoliAll1.Text = HoliAll1;
            ltHoliJan1.Text = HoliJan1;
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자  : 김형석
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            UserID = GetDecryptParam("userID", "");
            Name = GetDecryptParam("name", "");           
            HoliAll1 = GetDecryptParam("all1", "");
            HoliJan1 = GetDecryptParam("jan1", "");
            TitleDate = GetDecryptParam("titleDate", "");
        }

        /// <summary>
        /// 작성자  : 김형석
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                ltlHeader.Text = Name + " " + this.resMng.GetString("AdminViewTitle") + " ( " + TitleDate + " )";
                CbtnClose.Text = resMng.GetString("Closed");
            }
        }

        #endregion
    }
}