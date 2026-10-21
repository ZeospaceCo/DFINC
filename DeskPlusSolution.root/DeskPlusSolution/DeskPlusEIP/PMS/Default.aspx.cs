using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.Web.Module;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web
{
    public partial class Default : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
         
        #endregion

        #region 변수설정
        public string ContentURL = "";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드

            #endregion

            if (!IsPostBack)
            {
                GetContentURL();
            }

            #region 버튼 이미지
            #endregion
        }
        #endregion

        #region (함수)
        private void GetContentURL()
        {
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("status", "0");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            ContentURL = "./Project/JList.aspx?p=" + encParams;
        }
        #endregion
    }
}
