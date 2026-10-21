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
using Zeospace.DeskPlus.Proposal.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Proposal.Web.Content.Judge
{
    public partial class JudgeList : Base.BasePageProposal
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정

        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Content.Judge.JudgeList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            LoadParam();

            if (JudgeAuth != "TRUE")
            {
                Utility.ShowMessageBoxNoAuth(Page, "권한이 없습니다.", "");
            }

            ddlListCount.SelectedValue = ListCount;//페이지 카운트

        }

        #endregion

        #region (함수)

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
        }

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}