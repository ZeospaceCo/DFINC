using System;
using System.Collections.Specialized;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.Web.Content.Reject
{
    public partial class AddReject : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수설정
        public string ReservationID = "";
        public string CallBackEvent = "";
        #endregion 변수설정

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.Reject.AddReject).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadParam()
        {
            ReservationID = GetDecryptParam("reservationID", "");
            CallBackEvent = GetDecryptParam("callBackEvent", "");
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Confirm");
                CbtnClose.Text = resMng.GetString("Close");
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
    }
}