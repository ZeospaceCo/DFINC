using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.SO.Web
{
    public partial class Default : Base.BasePageSO
    {
        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Search.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.SO.Web.Default).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            SetLanguage();
        }

        /// <summary>
        /// 작성목적   : 파리미터
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        private void LoadParam()
        {

        }

        /// <summary>
        /// 작성목적   : 다국어
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //cbtnHomePost.Text = resMng.GetString("SearchAddress");
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}