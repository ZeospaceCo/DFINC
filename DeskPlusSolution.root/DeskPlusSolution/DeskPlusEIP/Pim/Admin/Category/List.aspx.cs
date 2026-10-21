using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Pim.Web.Admin.Category
{
    public partial class List : Base.BasePagePimAdmin
    {
        #region Declaration (컨트롤 선언)
        public int pageCount;
        public int PageNo;
        public string TotalCount = String.Empty;  //총카운트
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PIM.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Admin.Category.List).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }


            #region 버튼 이미지

            #endregion
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadList();
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("ADD");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            PageNo = GetDecryptParam("pageNo", "0").ToNumber<int>();
        }


        /// <summary>
        /// 게시판 리스트 : Loads the board list.
        /// </summary>
        /// <param name="_categoryID">_category ID.</param>
        private void LoadList()
        {            
            //RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.BOARD, "ListCount", "10").ToNumber<int>();//페이지리스트수            
        }

        #endregion

        #region Property (프로퍼티)        


        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}