using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Code
{
    public partial class Tree : Base.BasePageEIPAdmin
    {
        #region 변수설정

        public int SYSCodeID;
        public string CodeID;

        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Code.Tree).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            LoadSysCode();

            UCSysCodeTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCSysCodeTreeScript1.SetCallbackComplete = "onCallbackComplete";
            UCSysCodeTreeScript1.SetNodeSelect = "SysCodeNodeSelect_onNodeSelect";
            UCSysCodeTreeScript1.UseIcon = true;
            UCSysCodeTreeScript1.SYSCodeID = SYSCodeID;
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            SYSCodeID = GetDecryptParam("sysCodeID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSimpleAdd.Text = resMng.GetString("Cancel");
            }
        }


        /// <summary>
        /// 코드 정보 가져오기
        /// </summary>
        private void LoadSysCode()
        {
            Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
            List<Zeospace.DeskPlus.Common.BE.SYSCodeInfo> al = dal.GetSysCode(CurrentUserInfo.CompanyID, SYSCodeID);

            foreach (Zeospace.DeskPlus.Common.BE.SYSCodeInfo info in al)
            {
                CodeID = info.CodeID;
            }
        }        

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}