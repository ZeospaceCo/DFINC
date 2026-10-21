using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Net;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.Common
{
    public partial class AttachDSView : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string fileID = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Common.AttachView).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadAttachDS();
            }
        }

        /// <summary>
        ///작성자   : 이중호
        ///작성일자 : 2018-04-30
        ///작성목적 : DS 보여주기
        /// </summary>
        private void LoadAttachDS()
        {
            fileID = GetDecryptParam("FileID", "0");
        }

    }
}