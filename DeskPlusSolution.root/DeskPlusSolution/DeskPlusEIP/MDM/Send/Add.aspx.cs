using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.MDM.Web.Send
{
    public partial class Add : Base.BasePageMDM
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
            this.ResourceName = "Zeospace.DeskPlus.MDM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.MDM.Web.Send.Add).Assembly;	//어셈블리 선언
            #endregion

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
        /// 작성일자 : 2020-02-26
        /// 작성목적 : Data Load
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            tbxHeader.Text = AppSettingHelper.GetAppSetting(AppSettingType.MDM, "Header", "");
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-02-26
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnSend.Text = resMng.GetString("Send");
                cbtnClose.Text = resMng.GetString("Close");
                cbtnDept.Text = resMng.GetString("AccountDept");
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnDel.Text = resMng.GetString("Del");
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-02-26
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {        
        }
       

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}