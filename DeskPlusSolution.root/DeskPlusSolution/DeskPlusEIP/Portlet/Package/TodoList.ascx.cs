using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class TodoList : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region 변수설정
        public int TodoListTotalCount;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.TodoList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                #region 이미지 처리
                #endregion

                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        public void LoadData()
        {
            LoadParam();
            GetListCount();
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {

        }

        /// <summary>
        /// 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            Zeospace.DeskPlus.Portlet.DAL.TodoList biz = new DAL.TodoList();
            DateTime start = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00"));
            TodoListTotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, start);
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}