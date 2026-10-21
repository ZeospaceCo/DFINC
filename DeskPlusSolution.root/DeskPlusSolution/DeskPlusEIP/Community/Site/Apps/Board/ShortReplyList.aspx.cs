using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.Community.Web.Site.Apps.Board
{
    public partial class ShortReplyList : Base.BasePageCommunity
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정

        public int CommunityID = 0;
        public int MenuID = 0;
        public int BoardID = 0;
        public int TotalCount = 0; //총카운트
        public string RecordPerPage; //게시판 리스트 개수    
        public int Grade = 0;
        public string CheckAuth = "";
        public string ListType = String.Empty;
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
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Board.ShortReplyList).Assembly;	//어셈블리 선언
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-15
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            #region 권한 체크

            if (Grade != 2)
            {
                DAL.MenuAuth dal = new DAL.MenuAuth();
                DataTableReader dtr = dal.GetMenuAuth(CurrentUserInfo.CompanyID, CommunityID, MenuID);

                int WriteAuth = 9;
                int ReadAuth = 9;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            WriteAuth = dtr["WriteAuth"].ToString().ToNumber<int>();
                            ReadAuth = dtr["ReadAuth"].ToString().ToNumber<int>();
                        }
                    }
                }

                if (Grade <= ReadAuth)
                {
                    if (Grade <= WriteAuth)
                    {
                        cbtnShortReply.Visible = true;
                        textarea.Disabled = false;
                    }
                    else
                    {
                        CheckAuth = "Y";
                        cbtnShortReply.Visible = false;
                        textarea.Disabled = true;
                    }
                }
                else
                {
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
                }
            }

            #endregion 
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적   : 다국어 설정        
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = resMng.GetString("Close");
                cbtnShortReply.Text = resMng.GetString("AddShortReply");
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-11
        /// 작성목적   : 파라미터
        /// </summary>
        private void LoadParam()
        {            
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            if (RecordPerPage.Length == 0 || RecordPerPage == "0") RecordPerPage = "5";

            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            BoardID = GetDecryptParam("boardID", "").ToNumber<int>();
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();
            ListType = GetDecryptParam("listType", "");
        }

        #endregion

        #region (속성)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}