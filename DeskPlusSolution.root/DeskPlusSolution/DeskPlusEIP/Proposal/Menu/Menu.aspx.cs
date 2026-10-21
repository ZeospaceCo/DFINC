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
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;


namespace Zeospace.DeskPlus.Proposal.Web.Menu
{
    public partial class Menu : Base.BasePageProposal
    {
        #region 변수설정
        public string AccActive = "0"; //accordianActive
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Proposal.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Proposal.Web.Menu.Menu).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                LoadJudgeAuth();
            }
        }

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            AccActive = GetDecryptParam("accActive", "0");
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 2011-10-18
        /// 작성목적 : 1차 심사자, 메뉴 권한 가져오기
        /// </summary>
        private void LoadJudgeAuth()
        {
            adminTitle.Visible = false;
            adminMenu.Visible = false;

            // admin 권한 있는 사람만 관리자 모드 메뉴 보임
            if (AccessAuth)
            {
                adminTitle.Visible = true;
                adminMenu.Visible = true;
            }

            // 메뉴 admin과 1차 심사자의 권한은 별개
            DAL.Judge dal = new DAL.Judge();
            DataTableReader dtr = dal.GetJudgeList(CurrentUserInfo.CompanyID, 1);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if (CurrentUserInfo.UserID == dtr["UserID"].ToString().ToNumber<int>())
                        {
                            JudgeAuth = "TRUE";
                            break;
                        }
                    }
                }
            }
        }

        #endregion


        #region Property (프로퍼티)

        #endregion
    }
}