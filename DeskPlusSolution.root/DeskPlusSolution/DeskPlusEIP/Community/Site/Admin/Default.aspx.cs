using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Community.Web.Site.Admin
{
    public partial class Default : Base.BasePageCommunitySysop
    {
        #region 변수처리
        public int CommunityID;
        public string AdminCheck = "N";
        public string LeftURL = "";
        public string ContentURL = ""; 
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)        

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetLeftURL();
            GetContentURL();
        }
       
        #endregion

        #region (함수)

        private void GetLeftURL()
        {
            // Left URL
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("communityID", CommunityID.ToString());
            paraDic.Add("adminCheck", AdminCheck.ToString());            

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            LeftURL = "./Menu/Menu.aspx?p=" + encParams;
        }

        private void GetContentURL()
        {
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("communityID", CommunityID.ToString());
            paraDic.Add("adminCheck", AdminCheck.ToString());

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            ContentURL = "./Member/ListAdd.aspx?p=" + encParams;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-11
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            AdminCheck = GetDecryptParam("adminCheck", "N");
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}