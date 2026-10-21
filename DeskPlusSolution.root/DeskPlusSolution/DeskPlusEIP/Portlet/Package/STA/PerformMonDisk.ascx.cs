using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Package.STA
{
    public partial class PerformMonDisk : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region 변수설정
        public string MachinName;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.STA.PerformMonDisk).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
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

            if (!this.resMng.Equals(null)){}
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        public void LoadData()
        {
            LoadParam();
            GetMachineName();
        }

        /// <summary>
        /// 작성자  : freeman
        /// 작성일자 : 2017-01-17
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : MachineName 디코딩
        /// 작성자     : freeman
        /// 최초작성일 : 2017-01-17
        /// </summary>
        private void GetMachineName()
        {
            if (ParamValue.Length > 0)
            {
                foreach (string keyValue in ParamValue.Split('^'))
                {
                    if (keyValue.IndexOf('$') != -1)
                    {
                        string pKey = keyValue.Split('$')[0];
                        string pValue = keyValue.Split('$')[1];

                        if (pKey == "machineName") MachinName = pValue.ToString();
                    }
                }
            }

            //포틀릿 관리도구 데이타 연동 machineName 값이 없을 경우 WEB Server 정보를 가져온다
            if (MachinName == String.Empty)
            {
                MachinName = Environment.MachineName;
            }
        }

        #endregion
    }
}