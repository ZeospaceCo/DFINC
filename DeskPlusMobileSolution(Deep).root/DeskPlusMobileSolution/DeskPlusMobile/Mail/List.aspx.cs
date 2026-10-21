using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.Mail
{
    public partial class List : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string code = "";
        public int pageNo = 0;
        public string searchValue = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Mail.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                Today = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday: Today += "(" + resMng.GetString("Sun") + ")"; break;
                    case DayOfWeek.Monday: Today += "(" + resMng.GetString("Mon") + ")"; break;
                    case DayOfWeek.Tuesday: Today += "(" + resMng.GetString("Tue") + ")"; break;
                    case DayOfWeek.Wednesday: Today += "(" + resMng.GetString("Wed") + ")"; break;
                    case DayOfWeek.Thursday: Today += "(" + resMng.GetString("Thu") + ")"; break;
                    case DayOfWeek.Friday: Today += "(" + resMng.GetString("Fri") + ")"; break;
                    case DayOfWeek.Saturday: Today += "(" + resMng.GetString("Sat") + ")"; break;
                };

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
            code = GetDecryptParam("Code", "0");
            pageNo = GetDecryptParam("PageNo", "0").ToNumber<int>();
            searchValue = GetDecryptParam("MSv", "");
        }

        /// <summary>
        /// 
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-29
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            Ws.Data.List dal = new Ws.Data.List();
            int Total = dal.GetListCount(CompanyID, UserInfo.MailDomain, UserInfo.MailID, UserInfo.MailPassword, code, searchValue);


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
                pageNo = 0;
            }

            TotalNum.InnerText = TotalPage.ToString();
            PageNum.InnerText = (pageNo + 1).ToString();
        }

        public string Today { get; set; }
    }
}