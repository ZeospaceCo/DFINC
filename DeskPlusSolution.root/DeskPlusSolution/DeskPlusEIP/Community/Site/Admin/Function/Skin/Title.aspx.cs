using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Community.Web.Site.Admin.Function.Skin
{
    public partial class Title : Base.BasePageCommunitySysop
    {
        #region 변수설정
        public int CommunityID;
        public string TitleColor = String.Empty;  
        public string TitleImage = String.Empty;
        public string MenuColor1 = String.Empty;
        public string MenuColor2 = String.Empty;
        public string MenuColor3 = String.Empty;
        public string MenuBlock1TextClass = String.Empty;
        public string MenuBlock2TextClass = String.Empty; 
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Admin.Function.Skin.Title).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-29
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.ComDetail dal = new DAL.ComDetail();
            DataTableReader dtr = dal.GetComDetail(CurrentUserInfo.CompanyID, CommunityID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        TitleColor = dtr["TitleTextColor"].ToString();
                        TitleImage = dtr["TitleImage"].ToString();
                        MenuColor1 = dtr["MenuColor1"].ToString();
                        MenuColor2 = dtr["MenuColor2"].ToString();
                        MenuColor3 = dtr["MenuColor3"].ToString();
                        MenuBlock1TextClass = dtr["MenuBlock1TextClass"].ToString();
                        MenuBlock2TextClass = dtr["MenuBlock2TextClass"].ToString();                      
                    }
                }
            } 
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famoa23
        /// 작성일자 : 2012-05-25
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Save");       //"확인";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-29
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
        }


        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}