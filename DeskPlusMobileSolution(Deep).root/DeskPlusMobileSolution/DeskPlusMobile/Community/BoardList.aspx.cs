using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.Community
{
    public partial class BoardList : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int communityID = 0;
        public int menuID = 0;
        public int pageNo = 1;
        public string searchValue = "";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Community.BoardList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();               
                LoadParam();

                tbSearch.Value = searchValue;

                GetListCount();

                Zeospace.DeskPlusMobile.Web.Community.Ws.Data.Menu dal = new Zeospace.DeskPlusMobile.Web.Community.Ws.Data.Menu();
                DataSet ds = dal.GetMenu(UserInfo.CompanyID, communityID, menuID);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        listTitle.InnerHtml = "<i class='fas fa-users dropbtn '></i>" + this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                                        dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang).ToReverseXSS();
                    }
                }
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            communityID = GetDecryptParam("CommunityID", "0").ToNumber<int>();
            menuID = GetDecryptParam("MenuID", "0").ToNumber<int>();
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-09
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-27
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            Ws.Data.List dal = new Ws.Data.List();
            int Total = dal.GetListCount(CompanyID, communityID, menuID, searchValue);

            int recsInLastPage = Total % 10;
            int TotalPage = 0;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
            {
                TotalPage = (Total / 10);
            }
            else
            {
                TotalPage = (Total / 10) + 1;
            }

            if (TotalPage < pageNo)
            {
                pageNo = 1;
            }

            TotalNum.InnerText = TotalPage.ToString();
            PageNum.InnerText = pageNo.ToString();
        }

        public new string Title { get; set; }
    }
}