using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    public partial class ShortReplyMod : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID { get; set; }
        public int ScheduleID { get; set; }
        public int ReplyID { get; set; }
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.ShortReplyMod).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {
                    CbtnConfirm.Text = resMng.GetString("Confirm");
                    CbtnClose.Text = resMng.GetString("Close");
                }

                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 댓글 가져오기
        /// 작성자     : daniel
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.ScheduleReply dal = new DAL.ScheduleReply();
            CompanyID = CurrentUserInfo.CompanyID;

            BE.ScheduleReplyInfo info = dal.GetShortReplyID(CompanyID, ScheduleID, ReplyID);

            string contents = info.Content;
            int writeUserID = info.UserID;
            string isSecurity = info.IsSecurity;

            //권한 체크 (작성자 : 심재운)
            if (CurrentUserInfo.UserID != writeUserID)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBoxNoAuth(Page, "noauth", GetResource("NoUserAuth"));
            }
            else
            {
                textarea.Value = contents.ToReverseXSS();
                chkOpen.Checked = isSecurity == "1" ? false : true;
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ScheduleID = GetDecryptParam("scheduleID", "0").ToNumber<int>();
            ReplyID = GetDecryptParam("replyID", "0").ToNumber<int>();
        }

        #endregion
    }
}