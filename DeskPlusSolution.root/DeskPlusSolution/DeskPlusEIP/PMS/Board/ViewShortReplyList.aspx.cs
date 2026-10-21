using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Board
{
    public partial class ViewShortReplyList : Zeospace.DeskPlus.PMS.Web.Base.BasePagePMS
    {
        #region 변수설정
        public int BoardID = 0;
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

            this.ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			        //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Board.ViewShortReplyList).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = resMng.GetString("Close");
                cbtnShortReply.Text = resMng.GetString("AddShortReply");
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            BoardID = GetDecryptParam("boardID", "").ToNumber<int>();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)
        #endregion (함수)

        #region (프로퍼티)
        #endregion (프로퍼티)

        #region (이벤트핸들러)
        #endregion (이벤트핸들러)
    }
}