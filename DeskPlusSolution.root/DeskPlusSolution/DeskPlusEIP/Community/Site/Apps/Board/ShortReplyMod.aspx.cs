using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Community.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.Community.Web.Site.Apps.Board
{
    public partial class ShortReplyMod : Base.BasePageCommunity
    {
        #region 변수설정
        public int ShortReplyID = 0;
        public int CommunityID = 0;
        public int MenuID = 0;
        public int BoardID = 0;
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
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Board.ShortReplyMod).Assembly;	//어셈블리 선언
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
        /// 작성자 : shine
        /// 작성일자 : 2012-05-25
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.BoaShortReply dal = new DAL.BoaShortReply();
            DataTableReader dtr = dal.GetShortReplyList(CurrentUserInfo.CompanyID, CommunityID, MenuID, BoardID, ShortReplyID);

            int userID = 0;
            string contents = string.Empty;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        userID = dtr["UserID"].ToString().ToNumber<int>();
                        contents = dtr["Content"].ToString();
                    }
                }
            }

            // 권한체크
            if (userID != CurrentUserInfo.UserID)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBoxNoAuth(Page, "noauth", GetResource("Noauth"));
            }
            else
            {
                textarea.Value = contents.ToReverseXSS();
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
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

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            ShortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            BoardID = GetDecryptParam("boardID", "0").ToNumber<int>();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion

    }
}