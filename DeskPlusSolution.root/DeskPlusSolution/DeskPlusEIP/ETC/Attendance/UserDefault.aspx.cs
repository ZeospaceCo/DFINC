using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.ETC.Web.Attendance
{
    public partial class UserDefault : Base.BasePageETC
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Attendance.UserDefault).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 이지원
        /// 최초작성일 : 2018-01-09
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {

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