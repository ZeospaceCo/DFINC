using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.Task.Web.Home
{
    public partial class Main : Base.BasePageTask
    {
        #region 변수설정        
        public int ProcessPageNo = 0;
        public int CCPageNo = 0;
        public int DelayPageNo = 0;
        public int RecentPageNo = 0;
        public int WriteCommentPageNo = 0;
        public int ImpoPageNo = 0;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Home.Main).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-24
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {           
            if (CurrentUserInfo.IsInRole("taskall"))
            {
                Tagmenu.Visible = true;
            }
            else
            {
                Tagmenu.Visible = false;
            }                           
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                //cbtnSearch.Value = this.resMng.GetString("OrganBtnSearch");
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-23
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {            
            ProcessPageNo = GetDecryptParam("processPageNo", "0").ToNumber<int>();
            CCPageNo = GetDecryptParam("ccPageNo", "0").ToNumber<int>();
            DelayPageNo = GetDecryptParam("delayPageNo", "0").ToNumber<int>();
            RecentPageNo = GetDecryptParam("recentPageNo", "0").ToNumber<int>();
            WriteCommentPageNo = GetDecryptParam("writeCommentPageNo", "0").ToNumber<int>();            
            ImpoPageNo = GetDecryptParam("impoPageNo", "0").ToNumber<int>();
        }

        #endregion

        #region (프로퍼티)   
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}