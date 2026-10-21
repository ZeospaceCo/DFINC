using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Pim.Web.ShortReply
{
    public partial class ShortReply : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string NowDate = string.Empty;     // 쪽지 받을 사람 유저아이디
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.ShortReply.ShortReply).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            NowDate = GetDecryptParam("nowDate", "").Trim();
        }

        /// <summary>
        /// Sets the language.
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

        private void LoadData()
        {
            LoadParam();
        }

        #endregion
    }
}