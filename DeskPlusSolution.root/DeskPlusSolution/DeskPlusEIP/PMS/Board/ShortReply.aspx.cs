using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;
using System.Text;
using System.Xml;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.Web.Board
{
    public partial class ShortReply : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정      
        public int BoardID;

        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Board.ShortReply).Assembly;	//어셈블리 선언            
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
            BoardID = GetDecryptParam("boardID", "0").ToNumber<int>();
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

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }
            

        #endregion

        #region (함수)

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}
