using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlusMobile.Web.Community
{
    public partial class BoardFolder : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int communityID = 0;
        public int grade = 10;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Community.BoardFolder).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();                
                LoadParam();

                Zeospace.DeskPlusMobile.Web.Community.Ws.Data.Com dal = new Zeospace.DeskPlusMobile.Web.Community.Ws.Data.Com();
                DataSet ds = dal.GetComInfo(UserInfo.CompanyID, communityID, UserInfo.UserID);

                if (ds != null)
                { 
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string tmpTitle = dr["Name"].ToString().ToReverseXSS();

                        if(tmpTitle.Length > 15)
                        {
                            tmpTitle = tmpTitle.Substring(0,15) + "..";
                        }
                        listTitle.InnerHtml = "<i class='fas fa-users dropbtn '></i>" + tmpTitle;
                        grade = dr["Grade"].ToString().ToNumber<int>();
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-09
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            communityID = GetDecryptParam("CommunityID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-09
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }
    }
}