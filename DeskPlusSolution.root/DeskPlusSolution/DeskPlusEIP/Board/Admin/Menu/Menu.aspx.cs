using System;
using System.Collections.Generic;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Admin.Menu
{
    public partial class Menu : Zeospace.DeskPlus.Board.Web.Base.BasePageBoardAdmin
    {
        #region 변수설정

        public string OptionUseDept = ""; //부서별 게시판 카테고리
        public string IsTotBoard = "0"; //통합게시판 여부
        public string IsTotAccount = "0";//통합관리자 인지 확인
        private string eventArgs = String.Empty;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Menu.Menu).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            
            //통합관리자
            IsTotAccount = (CurrentUserInfo.IsInRole("TotalAccount")) ? "1" : "0";
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            OptionUseDept = GetDecryptParam("optionUseDept", "0");
            IsTotBoard = GetDecryptParam("isTotBoard", "0");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSimpleAdd.Text = resMng.GetString("Cancel");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 폴더 삭제
        /// </summary>
        private void Del()
        {
            string selCategory = tbHdnValue.Text.Trim();
            string isTotBoard = tbHdnTotValue.Text.Trim();
            int CompanyID = 0;

            if (selCategory.Length > 0)
            {
                if (isTotBoard == "0") CompanyID = CurrentUserInfo.CompanyID;

                //해당 카테고리의 리스트 카운트 가져오기
                DAL.Lists bizList = new DAL.Lists();
                int countList = bizList.GetCountList(CompanyID, selCategory.ToNumber<int>());

                //작성한 글이 없을 경우, 해당 카테고리 삭제 가능하다.
                if (countList == 0)
                {
                    DAL.Category biz = new DAL.Category();
                    biz.Del(CompanyID, tbHdnValue.Text.Trim().ToNumber<int>());

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("menuCatID", this.MenuCatID.ToString());
                    paraDic.Add("catID", CatID.ToString());
                    paraDic.Add("isTotBoard", isTotBoard);

                    string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                    string url = "Menu.aspx?p=" + encParams;

                    Response.Redirect(url, false);
                }
            }
        }

        #endregion Method (함수)

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                string args = eventArgs;
                switch (args)
                {
                    case "Del"://폴더 삭제
                        Del();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion Event Handler (이벤트 핸들러)
    }
}