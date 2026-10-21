using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class ShortReplyMod : Base.BasePageBoard
    {
        #region 변수설정

        public int ListID { get; set; }

        public int ShortReplyID { get; set; }

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.ShortReplyMod).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {
                    CbtnConfirm.Text = resMng.GetString("Confirm");
                    CbtnClose.Text = resMng.GetString("Closed");
                }

                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

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

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CategoryID);

            DAL.ShortReply dal = new DAL.ShortReply();

            BE.ShortReplyInfo info = dal.GetShortReplyListID(BoaComID, CategoryID, ListID, ShortReplyID);

            //권한 체크 (작성자 : 심재운)
            if (CurrentUserInfo.UserID != info.UserID)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBoxNoAuth(Page, "noauth", "해당 사용자는 접근하실 수 없습니다.");
            }
            else
            {
                textarea.Value = info.Content.ToReverseXSS();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
            ShortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
    }
}