using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.WebHard.Web.Content
{
    public partial class Home : Base.BasePageWebHard
    {
        #region 변수설정
        public int PublicPageNo = 0;
        public int SendPageNo = 0;
        public int SharePageNo = 0;
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
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Content.Home).Assembly;	//어셈블리 선언
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
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-04
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
         
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
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-04
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            PublicPageNo = GetDecryptParam("publicPageNo", "0").ToNumber<int>();
            SendPageNo = GetDecryptParam("sendPageNo", "0").ToNumber<int>();
            SharePageNo = GetDecryptParam("sharePageNo", "0").ToNumber<int>();
        }

        #endregion

        #region (프로퍼티)     
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}