using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Approval.Web.Admin.EnforceSeal
{
    public partial class RegisterLangAddress : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public string FirstBase = "";
        public string FirstKo = "";
        public string FirstEn = "";
        public string FirstJa = "";
        public string FirstFr = "";
        public string FirstZhcn = "";
        public string FirstZhtw = "";

        public string EndBase = "";
        public string EndKo = "";
        public string EndEn = "";
        public string EndJa = "";
        public string EndFr = "";
        public string EndZhcn = "";
        public string EndZhtw = "";

        public string UseName = "";
        public string CallBackPage = "";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.EnforceSeal.RegisterLangAddress).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                CallBackPage = GetDecryptParam("callBackPage", "");
                UseName = GetDecryptParam("useName", "");

                FirstBase = GetDecryptParam("firstBase", "");
                FirstKo = GetDecryptParam("firstKo", "");
                FirstEn = GetDecryptParam("firstEn", "");
                FirstJa = GetDecryptParam("firstJa", "");
                FirstFr = GetDecryptParam("firstFr", "");
                FirstZhcn = GetDecryptParam("firstZhcn", "");
                FirstZhtw = GetDecryptParam("firstZhtw", "");

                EndBase = GetDecryptParam("endBase", "");
                EndKo = GetDecryptParam("endKo", "");
                EndEn = GetDecryptParam("endEn", "");
                EndJa = GetDecryptParam("endJa", "");
                EndFr = GetDecryptParam("endFr", "");
                EndZhcn = GetDecryptParam("endZhcn", "");
                EndZhtw = GetDecryptParam("endZhtw", "");

                FirstBase = Regex.Replace(FirstBase, "</script>", "", RegexOptions.IgnoreCase);
                FirstKo = Regex.Replace(FirstKo, "</script>", "", RegexOptions.IgnoreCase);
                FirstEn = Regex.Replace(FirstEn, "</script>", "", RegexOptions.IgnoreCase);
                FirstJa = Regex.Replace(FirstJa, "</script>", "", RegexOptions.IgnoreCase);
                FirstFr = Regex.Replace(FirstFr, "</script>", "", RegexOptions.IgnoreCase);
                FirstZhcn = Regex.Replace(FirstZhcn, "</script>", "", RegexOptions.IgnoreCase);
                FirstZhtw = Regex.Replace(FirstZhtw, "</script>", "", RegexOptions.IgnoreCase);

                EndBase = Regex.Replace(EndBase, "</script>", "", RegexOptions.IgnoreCase);
                EndKo = Regex.Replace(EndKo, "</script>", "", RegexOptions.IgnoreCase);
                EndEn = Regex.Replace(EndEn, "</script>", "", RegexOptions.IgnoreCase);
                EndJa = Regex.Replace(EndJa, "</script>", "", RegexOptions.IgnoreCase);
                EndFr = Regex.Replace(EndFr, "</script>", "", RegexOptions.IgnoreCase);
                EndZhcn = Regex.Replace(EndZhcn, "</script>", "", RegexOptions.IgnoreCase);
                EndZhtw = Regex.Replace(EndZhtw, "</script>", "", RegexOptions.IgnoreCase);

                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
        }

        #endregion

        #region (함수)

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
                CbtnConfirm.Text = resMng.GetString("Confirm");
                CbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}