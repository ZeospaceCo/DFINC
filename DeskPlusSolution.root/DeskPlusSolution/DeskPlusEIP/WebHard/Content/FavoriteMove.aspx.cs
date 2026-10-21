using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.WebHard.Web.Content
{
    public partial class FavoriteMove : Base.BasePageWebHard
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int MenuCatID;
        public int CatID;
        public int FavoriteID;
        public int ParentFavoriteID;
        public int ChildCnt;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Content.FavoriteMove).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");       //"적용";
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-26
        /// 작성목적 : 데이터 가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            FavoriteID = GetDecryptParam("favoriteID", "0").ToNumber<int>();
            ParentFavoriteID = GetDecryptParam("parentFavoriteID", "0").ToNumber<int>();
            ChildCnt = GetDecryptParam("childCnt", "0").ToNumber<int>();
        }

        #endregion

        #region (속성)

        #endregion
    }
}