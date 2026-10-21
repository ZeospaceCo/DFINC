using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Admin
{
    public partial class Default : Zeospace.DeskPlus.Board.Web.Base.BasePageBoardAdmin
    {
        #region 변수설정
        public string OptionUseDept = ""; //부서별 게시판 카테고리
        public string IsTotBoard = "0"; //통합게시판 여부
        public string IsTotAccount = "0";//통합관리자 인지 확인
        public string ContentURL = ""; 
        #endregion 변수설정

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Default).Assembly;	//어셈블리 선언
            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #region Method (함수)
        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            OptionUseDept = GetDecryptParam("optionUseDept", "0");
            IsTotBoard = GetDecryptParam("isTotBoard", "0");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSimpleAdd.Text = resMng.GetString("Cancel");
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //통합관리자
            IsTotAccount = (CurrentUserInfo.IsInRole("TotalAccount")) ? "1" : "0";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("catID", CatID.ToString());
            paraDic.Add("menuCatID", MenuCatID.ToString());

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            string pageList = (IsTotBoard == "1") ? "ModTot.aspx" : "Mod.aspx";

            ContentURL = "/DeskPlusEIP/Board/Admin/Category/" + pageList + "?p=" + encParams;
        }
        #endregion

    }
}