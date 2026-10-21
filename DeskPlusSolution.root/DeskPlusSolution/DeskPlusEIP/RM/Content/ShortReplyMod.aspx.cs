using System;
using System.Collections.Specialized;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.Web.Content
{
    public partial class ShortReplyMod : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수설정
        public int ReservationID { get; set; }
        public int ParentReservationID { get; set; }
        public int ShortReplyID { get; set; }
        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.ShortReplyMod).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.ShortReply dal = new DAL.ShortReply();
            DataTableReader dtr = dal.GetShortReplyListID(CurrentUserInfo.CompanyID, ReservationID, ShortReplyID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //권한 체크 
                        if (CurrentUserInfo.UserID != dtr["UserID"].ToString().ToNumber<int>())
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBoxNoAuth(Page, "noauth", "해당 사용자는 접근하실 수 없습니다.");
                        }
                        else
                        {
                            textarea.Value = dtr["Content"].ToString().ToReverseXSS();
                        }
                    }
                }
            }            
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ReservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();
            ParentReservationID = GetDecryptParam("parentReservationID", "0").ToNumber<int>();
            ShortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
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