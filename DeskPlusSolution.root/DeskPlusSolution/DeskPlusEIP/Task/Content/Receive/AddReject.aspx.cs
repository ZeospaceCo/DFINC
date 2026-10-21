using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.Web.Content.Receive
{
    public partial class AddReject : Base.BasePageTask
    {
        #region 변수설정
        public string CallBackEvent = "";
        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Content.Receive.AddReject).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

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
                CbtnConfirm.Text = resMng.GetString("BtnConfirm");
                CbtnClose.Text = resMng.GetString("Close");                                
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-11
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CallBackEvent = GetDecryptParam("callBackEvent", "");
        }
       
        #endregion (함수)

        #region (속성)
        #endregion (속성)

        #region (이벤트핸들러)
        #endregion (이벤트핸들러)
    }
}