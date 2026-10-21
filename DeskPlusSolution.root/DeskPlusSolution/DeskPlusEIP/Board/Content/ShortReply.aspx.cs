using System;
using System.Collections.Specialized;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class ShortReply : Base.BasePageBoard
    {
        #region 변수설정
        public int ListID = 0;
        public int ShortReplyID = 0;
        #endregion 변수설정

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.ShortReply).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
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

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Confirm");
                CbtnClose.Text = resMng.GetString("Closed");
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CategoryID);

            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(BoaComID, CategoryID);

            string optionUseDept = "";
            string optionUseShortReply = "";

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    optionUseDept = dr["OptionUseDept"].ToString();
                    optionUseShortReply = dr["OptionUseShortReply"].ToString();
                }
            }

            //댓글 활성화 여부
            if (optionUseShortReply == "1")
            {
                BLL.Category bizCat = new BLL.Category();
                NameValueCollection nvcAuth = new NameValueCollection();

                //댓글 쓰기(3,4) 존재하는지 체크
                nvcAuth = bizCat.GetMyAuthCategory(BoaComID, CurrentUserInfo.UserID,
                    CurrentUserInfo.DeptID, CategoryID, optionUseDept, false, BE.BoardAuth.ShortWrite, true, false);

                if (nvcAuth.AllKeys.Length == 0)
                {
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
                }
            }
            else
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
    }
}