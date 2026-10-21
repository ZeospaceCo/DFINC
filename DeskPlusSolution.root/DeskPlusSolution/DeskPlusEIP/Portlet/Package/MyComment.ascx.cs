using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.BLL;
using Zeospace.DeskPlus.Pim.DAL;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data.SqlClient;
using System.Configuration;

namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class MyComment : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region 변수설정
        public string Today;
        public int TotalCount;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.MyComment).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();

                PageSize = 3;   //기본 페이징 값 처리
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        public void LoadData()
        {
            Today = System.DateTime.Now.Date.ToString();
            Today = DateHelper.GetCultureDate(Today, "yyyyMMdd", PreLangReal);
        }
        #endregion

        #region (함수)
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 나의 댓글 카운트 가져오기
        /// </summary>

        #endregion

        #region (프로퍼티)

        #endregion
    }
}