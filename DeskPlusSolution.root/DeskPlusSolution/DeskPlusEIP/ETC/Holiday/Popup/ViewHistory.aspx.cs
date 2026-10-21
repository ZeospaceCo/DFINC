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
    public partial class ViewHistory : Base.BasePageETC
    {
        #region 변수설정
        public string UserID = string.Empty;
        public string StartDate = string.Empty;
        public string TitleDate = string.Empty;
        public string Type = string.Empty;
        public string Name = string.Empty;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Holiday.Popup.ViewHistory).Assembly;	    //어셈블리 선언
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
            StartDate = GetDecryptParam("startDate", "");
            TitleDate = GetDecryptParam("titleDate", "");
            Type = GetDecryptParam("type", "");
            Name = GetDecryptParam("name", "");
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
                ltlHeader.Text = Name + " " + this.resMng.GetString("HoliCount") + " " + this.resMng.GetString("ModHistory") + " ( " + TitleDate + " )";
                cbtnClose.Text = resMng.GetString("Closed");
            }
        }

        #endregion
    }
}