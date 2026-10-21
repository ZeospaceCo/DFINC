using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.Portlet.Web.TodoPopup
{
    public partial class TodoList : Base.BasePagePortlet
    {
        #region 변수설정
        public int UserID = 0;
        public string TodayStr = String.Empty;
        public string TodoMenu = String.Empty;
        public string PopID = String.Empty;
        public string IsMgr = "FALSE";
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.TodoPopup.TodoList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                TodayStr = DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString().PadLeft(2, '0') + "." + DateTime.Now.Day.ToString().PadLeft(2, '0');

                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     :
        /// 최초작성일 :
        /// </summary>
        private void LoadData()
        {
            UserID = CurrentUserInfo.UserID;

            LoadParam();
            SetLanguage();

            // 메뉴정보 가져오기
            Zeospace.DeskPlus.Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();
            List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = env.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (list != null)
            {
                foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo info in list)
                {
                    TodoMenu = (info.TodoMenu.Equals("")) ? "APP-S,TAS-S,PIM-T,RM-C" : info.TodoMenu;
                }
            }

            // 자원관리자여부 가져오기
            DAL.TodoPopup dal = new DAL.TodoPopup();
            IsMgr = dal.GetIsManager(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID).ToString().ToUpper();
            if (IsMgr.Equals("FALSE"))
            {
                TodoMenu = TodoMenu.Replace("RM-W", "");
            }

            TodoMenu = TodoMenu.Replace(",,", ",");
        }


        /// <summary>
        /// 파라미터 설정
        /// </summary>
        private void LoadParam()
        {
            PopID = this.GetParm("popid", "TODO-" + UserID.ToString());
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }
        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}