using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{
    public partial class ViewShortReplyList : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int ScheduleID = 0;
        public int DisplayID = 0;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.ViewShortReplyList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region 함수

        /// <summary>
        /// 다국어설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnCancel.Text = this.resMng.GetString("Close");
                cbtnShortReply.Text = resMng.GetString("View_AddShortReply");               
            }
        }

        private void LoadParam()
        {
            ScheduleID = GetDecryptParam("scheduleID", "0").ToNumber<int>();
            DisplayID = GetDecryptParam("displayID", "0").ToNumber<int>();
        }
      

        #endregion
    }
}