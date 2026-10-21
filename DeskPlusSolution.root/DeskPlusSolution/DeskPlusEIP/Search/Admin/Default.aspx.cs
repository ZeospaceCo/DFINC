using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Search.Web.Admin
{
    public partial class Default : Base.BasePageSearchAdmin
    {
        #region 변수설정
        public string ContentURL = "";
        #endregion 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            GetContentURL();
        }

        private void GetContentURL()
        {
            // Left URL
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("companyID", CurrentUserInfo.CompanyID.ToString());

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            ContentURL = "./Index/Default.aspx?p=" + encParams;
        }
    }
}