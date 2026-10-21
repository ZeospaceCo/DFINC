using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace Zeospace.DeskPlusMobile.Web.MDM
{
    public partial class List : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {

        public int pageNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.MDM.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();

                LoadParam();

                GetListCount();
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-23
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
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-27
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            WS.Data.Notification dal = new WS.Data.Notification();

            int Total = dal.GetTotalCnt(CompanyID, UserID.ToNumber<int>());

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