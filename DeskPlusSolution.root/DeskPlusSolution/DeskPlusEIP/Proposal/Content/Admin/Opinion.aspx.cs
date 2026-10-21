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
using System.Collections;
using System.Text;


namespace Zeospace.DeskPlus.Proposal.Web.Content.Admin
{
    public partial class Opinion : Base.BasePageProposalAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int ProposalNo = 0;
        public int JudgeUserID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Content.Admin.Opinion).Assembly;	//어셈블리 선언
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
            LoadList();
        }

        #endregion

        #region (함수)

        private void LoadParam()
        {
            ProposalNo = GetDecryptParam("proposalNo", "0").ToNumber<int>();
            JudgeUserID = GetDecryptParam("judegUserID", "0").ToNumber<int>();
        }

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
        }

        private void LoadList()
        {
            DAL.Exam biz = new DAL.Exam();
            var examList = biz.GetExam(CurrentUserInfo.CompanyID, ProposalNo, JudgeUserID);

            foreach (var info in examList)
            {
                textarea.InnerText = info.JudgeView;
            }
        }
        #endregion
    }
}