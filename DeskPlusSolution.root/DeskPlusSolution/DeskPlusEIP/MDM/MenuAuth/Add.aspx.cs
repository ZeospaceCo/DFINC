using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.MDM.Web.MenuAuth
{
    public partial class Add : Base.BasePageMDMAdmin
    {
        #region 변수설정
        public int MenuID;
        public string SiteMap = String.Empty;//사이트맵
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
            this.ResourceName = "Zeospace.DeskPlus.MDM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.MDM.Web.MenuAuth.Add).Assembly;	//어셈블리 선언

            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();

            GetSiteMap();  //사이트맵
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {               
                cbtnADDAuth.Text = resMng.GetString("AddAuth");                
            }
        }

        /// <summary>
        /// 사이트맵
        /// </summary>
        private void GetSiteMap()
        {
            if (MenuID > 0)
            {
                DAL.Menu dal = new DAL.Menu();
                DataSet ds = dal.GetMenu(CurrentUserInfo.CompanyID);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["MenuID"].ToString() == MenuID.ToString())
                        {
                            SiteMap = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                            dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();
                        }
                    }
                }
            }

            if (SiteMap == "") SiteMap = resMng.GetString("AuthAccessControl");
        }

        #endregion

        #region Event Handler (이벤트 핸들러)    
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}