using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Board.Web.Home
{
    public partial class Tree : Base.BasePageBoard
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Home.Content).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 심재운
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                //cbtnAdmin.Text = this.resMng.GetString("Administrator");
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자  : 심재운
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            //FavoriteID = GetDecryptParam("favoriteID", "0").ToNumber<int>();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)



        #endregion
    }
}