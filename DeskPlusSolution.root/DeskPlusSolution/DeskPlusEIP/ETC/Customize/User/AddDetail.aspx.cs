using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.ETC.Web.Customize.User
{
    public partial class AddDetail : Base.BasePageETC
    {
        #region 변수설정
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
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Customize.User.AddDetail).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
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
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            // ListID = GetDecryptParam("listID", "").ToString();
            // DocID = GetDecryptParam("docID", "0").ToNumber<int>();
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}