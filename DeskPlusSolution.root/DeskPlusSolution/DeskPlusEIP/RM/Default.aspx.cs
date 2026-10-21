using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.Web
{
    public partial class Default : Base.BasePageRM
    {
        #region 변수설정
        public string RMBasePage = string.Empty;        
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-16
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            RMBasePage = GetDecryptParam("rmBasePage", "");
            string tempRMBasePage = GetParm("rmBasePage", "");
            if (tempRMBasePage != "") { RMBasePage = tempRMBasePage; }
            
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-16
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            
            
        }

        
        
        #endregion

        #region (프로퍼티)

        public string WeContent { get; set; }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}