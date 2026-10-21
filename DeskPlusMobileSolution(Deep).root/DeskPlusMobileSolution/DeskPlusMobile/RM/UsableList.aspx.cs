using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlusMobile.Web.Board.Ws;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.RM
{
    public partial class UsableList : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int pageNo = 1;
        public string mainCID = "0";
        public string searchValue = "";
        public string bPage = "";
        public string pCatID = "";
        public string authority = "";
        public string OptionUseDept = "0";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.RM.UsableList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();

                LoadParam();

                tbSearch.Value = searchValue;

                GetListCount();
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            bPage = GetDecryptParam("BPage", "Usable0");
        }

        /// <summary>
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
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            Ws.Data.Reserve dal = new Ws.Data.Reserve();
            int Total = dal.GetUsalbleListCount(int.Parse(CompanyID), int.Parse(UserID), "0", searchValue);

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
    }
}