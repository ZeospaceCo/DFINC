using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Task.Web.Admin.Source
{
    public partial class List : Base.BasePageTaskAdmin
    {
        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Admin.Source.List).Assembly;	//어셈블리 선언
            #endregion
            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }


        }
        #endregion

        #region Data Load (데이터를 컨트롤에 바인딩 하는 메서드)
        public void LoadData()
        {

        }
        #endregion

        #region 함수
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-04
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
               
            }
        }
        #endregion

    }
}