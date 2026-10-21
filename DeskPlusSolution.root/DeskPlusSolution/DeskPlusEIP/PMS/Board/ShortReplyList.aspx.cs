using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.PMS.Web.Board
{
    public partial class ShortReplyList : Base.BasePagePMS
    {
        #region 변수설정
        public int BoardID = 0;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Board.ShortReplyList).Assembly;	//어셈블리 선언
            GetResourceManager();
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

        private void LoadData()
        { 
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-11
        /// 작성목적   : 다국어 설정        
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
        /// 작성자 : shine
        /// 작성일자 : 2012-06-11
        /// 작성목적   : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BoardID = GetDecryptParam("boardID", "").ToNumber<int>();
        }
        #endregion

        #region (속성)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}