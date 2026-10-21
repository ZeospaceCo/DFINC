using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Search.Web.Admin.Index
{
    public partial class Default : Base.BasePageSearchAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int DeptID = 0;
        public int UserID = 0;
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string Order = String.Empty; //sort 
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-01-17 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.Search.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Search.Web.Admin.Index.Default).Assembly;	//어셈블리 선언
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
        /// 작성자 : freeman
        /// 작성일자 : 2013-01-18
        /// </summary>
        private void LoadData()
        {
            LoadParam();

        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-01-17
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnDel.Text = resMng.GetString("IndexDel");
                cbtnOptimize.Text = resMng.GetString("IndexOptimize");
                cbtnAdd.Text = resMng.GetString("IndexAdd");
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-11-21
        /// 작성목적 : 다국어
        /// </summary>
        private void LoadParam()
        {

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