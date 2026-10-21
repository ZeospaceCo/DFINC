using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Process.Opinion
{
    public partial class ConfirmOpinion : Base.BasePageApproval
    {
        #region 변수설정
        public string UserName = "";
        public string DeptName = "";
        public string TitleName = "";
        public string DutyName = "";
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
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Process.Opinion.ConfirmOpinion).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                UserName = CurrentUserInfo.Name;
                DeptName = CurrentUserInfo.DeptName;
                TitleName = CurrentUserInfo.TitleName;
                DutyName = CurrentUserInfo.DutyName;

                this.tbOpinion.Attributes.Add("maxlength", "500");

                LoadParam();
                SetLanguage();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParam()
        {
        }
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm"); // "확인";
                cbtnClose.Text = resMng.GetString("Close"); //"닫기";
            }
        }
        #endregion

        #region (프로퍼티)
        

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}