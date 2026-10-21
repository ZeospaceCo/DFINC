using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.Web.Board
{
    public partial class ShortReplyMod : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID { get; set; }                
        public int BoardID { get; set; }
        public int BoardShortReplyID { get; set; }

        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Board.ShortReplyMod).Assembly;	//어셈블리 선언            
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

            DAL.BoardShortReply dal = new DAL.BoardShortReply();
            CompanyID = CurrentUserInfo.CompanyID;
            
            List<BE.BoardShortReplyInfo> alList = dal.GetBoardShortReply(CompanyID, ProjectID, MenuID, BoardID, BoardShortReplyID);
            string contents = "";            

            foreach (var info in alList)
            {
                contents = info.Contents.ToReverseXSS();         

                //권한 체크 (작성자 : 심재운)
                if (CurrentUserInfo.UserID != info.WriteUserID)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBoxNoAuth(Page, "noauth", GetResource("NoAuth"));
                }
                else
                {
                    textarea.Value = contents.ToReverseXSS();
                }
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {            
            BoardID = GetDecryptParam("boardID", "0").ToNumber<int>();
            BoardShortReplyID = GetDecryptParam("boardShortReplyID", "0").ToNumber<int>();
        }


        #endregion

        #region (함수)



        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)



        #endregion
    }
}