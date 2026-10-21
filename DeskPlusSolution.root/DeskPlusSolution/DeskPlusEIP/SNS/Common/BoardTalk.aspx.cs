using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.SNS.Web.Common
{
    public partial class BoardTalk : Base.BasePageWorkTalk
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CategoryID = 0;
        public int ListID = 0;
        public int TotalCount = 0; //총카운트
        public string RecordPerPage; //게시판 리스트 개수         
        public string Order = string.Empty; //sort
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.SNS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.SNS.Web.Common.BoardTalk).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }



        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            ddlListCount.SelectedValue = RecordPerPage;

            Zeospace.DeskPlus.Board.DAL.Category dal = new Zeospace.DeskPlus.Board.DAL.Category();
            DataSet ds = dal.GetCategory(CurrentUserInfo.CompanyID, CategoryID);

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

            //조회 활성화 여부
            if (optionUseShortReply == "1")
            {
                Zeospace.DeskPlus.Board.BLL.Category bizCat = new Zeospace.DeskPlus.Board.BLL.Category();
                NameValueCollection nvcAuth = new NameValueCollection();

                //읽기 이상 존재하는지 체크
                nvcAuth = bizCat.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                    CurrentUserInfo.DeptID, CategoryID, optionUseDept, false, Board.BE.BoardAuth.Read, true, false);

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

        #endregion

        #region (함수)

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnReload.Text = resMng.GetString("Reload");
                CbtnClose.Text = resMng.GetString("Close");
                cbtnShortReply.Text = resMng.GetString("AddShortReply");
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            Order = (Order.Length == 0) ? "Lists.CompanyID, Lists.CategoryID, Lists.ListID, Lists.CRUDDate" : Order;
            ListID = GetDecryptParam("listID", "0").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            if (RecordPerPage.Length == 0 || RecordPerPage == "0") RecordPerPage = "5";
        }

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}