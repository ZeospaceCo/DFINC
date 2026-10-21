using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Coop.Web.Admin.Category
{
    public partial class Default : Base.BasePageCoopAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Admin.Category.Default).Assembly;	//어셈블리 선언
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
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 
        /// </summary>
        private void LoadData()
        {

        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnDel.Text = resMng.GetString("Del");
                CbtnMove.Text = resMng.GetString("ChangeOrder");
                
                cbtnModSub.Text = resMng.GetString("Mod");
                cbtnAddSub.Text = resMng.GetString("Add");
                cbtnDelSub.Text = resMng.GetString("Del");
                CbtnSubMove.Text = resMng.GetString("ChangeOrder");              
            }
        }

        #endregion

        #region (함수)

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)


        #endregion
    }
}