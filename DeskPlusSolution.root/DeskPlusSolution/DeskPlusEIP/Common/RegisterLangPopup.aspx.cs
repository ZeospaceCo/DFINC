using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Common.Web
{
    public partial class RegisterLangPopup : Base.BasePageCommon
    {
        #region Declaration (컨트롤 선언)
        public string CallBackPage = "";
        public string NameBase = "";
        public string NameKo = "";
        public string NameEn = "";
        public string NameJa = "";
        public string NameFr = "";
        public string NameZhcn = "";
        public string NameZhtw = "";
        public string UseName = "";
        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.RegisterLangPopup).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                CallBackPage = GetDecryptParam("callBackPage", "");
                UseName = GetDecryptParam("useName", "");
                NameBase = GetDecryptParam("nameBase", "");
                NameKo = GetDecryptParam("nameKo", "");
                NameEn = GetDecryptParam("nameEn", "");
                NameJa = GetDecryptParam("nameJa", "");
                NameFr = GetDecryptParam("nameFr", "");
                NameZhcn = GetDecryptParam("nameZhcn", "");
                NameZhtw = GetDecryptParam("nameZhtw", "");


                NameBase = Regex.Replace(NameBase, "</script>", "", RegexOptions.IgnoreCase);
                NameKo = Regex.Replace(NameKo, "</script>", "", RegexOptions.IgnoreCase);
                NameEn = Regex.Replace(NameEn, "</script>", "", RegexOptions.IgnoreCase);
                NameJa = Regex.Replace(NameJa, "</script>", "", RegexOptions.IgnoreCase);
                NameFr = Regex.Replace(NameFr, "</script>", "", RegexOptions.IgnoreCase);
                NameZhcn = Regex.Replace(NameZhcn, "</script>", "", RegexOptions.IgnoreCase);
                NameZhtw = Regex.Replace(NameZhtw, "</script>", "", RegexOptions.IgnoreCase);

                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Alert_Ok");
                CbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {

        }

        #endregion

        #region (속성)

        #endregion
    }
}