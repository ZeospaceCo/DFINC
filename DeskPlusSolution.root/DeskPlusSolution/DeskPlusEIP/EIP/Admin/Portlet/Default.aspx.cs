using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Portlet
{
    public partial class Default : Base.BasePageEIPAdmin
    {
        #region 변수설정
        public int CategoryID;
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
            LoadParam();
            GetContentURL();
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
        }

        private void GetContentURL()
        {
            // Left URL
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("categoryID", CategoryID.ToString());
            paraDic.Add("gubun", "add");

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            ContentURL = "AddFolder.aspx?p=" + encParams;
        }

    }
}