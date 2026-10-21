using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Task.Web.Common
{
    public partial class StatusInfo : Base.BasePageTask
    {
        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Common.StatusInfo).Assembly;	//어셈블리 선언
            #endregion            

            if (!IsPostBack)
            {
                SetLanguage();
            }

        }

        #endregion 

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-12
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = resMng.GetString("Close");       //"닫기";
            }
        }

        #endregion

    }
}