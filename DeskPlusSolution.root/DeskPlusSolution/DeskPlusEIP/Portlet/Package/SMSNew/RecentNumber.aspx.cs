using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Portlet.Web.Package.SMSNew
{
    public partial class RecentNumber : Base.BasePagePortlet
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.SMSNew.RecentNumber).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
            }
        }


        /// <summary>
        /// 작성자  : 심재운
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = "닫기";
                CbtnDel.Text = "삭제";
            }
        }
    }
}