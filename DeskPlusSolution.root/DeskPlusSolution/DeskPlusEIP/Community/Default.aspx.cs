using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Community.Web
{
    public partial class Default : Base.BasePageCommunity
    {
        #region 변수설정
        public string CommunityGuid = String.Empty;
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
        /// 작성일자 : 2012-07-03
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (CommunityGuid != "")
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("gubun", CommunityGuid);

                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                string url = "/DeskPlusEIP/Community/Site/Default.aspx?p=" + encParams;

                Response.Redirect(url, false);
            }
        }

        #endregion

        #region (함수)


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-03
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityGuid = GetParm("gubun", "");
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}