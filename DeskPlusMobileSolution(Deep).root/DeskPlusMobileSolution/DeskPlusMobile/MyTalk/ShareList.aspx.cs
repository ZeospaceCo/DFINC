using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.MyTalk
{
    public partial class ShareList : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int pageSize = 0;
        public int moreSize = 0;
        public int pageNo = 0;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.MyTalk.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
            }

        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            pageSize = GetDecryptParam("PageSize", "10").ToNumber<int>();
            moreSize = GetDecryptParam("MoreSize", "10").ToNumber<int>();
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();         
        }

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-11-05
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        #endregion
    }
}