using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.Web.Base;

namespace Zeospace.DeskPlus.Account.Web.Admin.Position
{
    public partial class Tree : BasePageAccountAdmin
    {
        #region 변수설정

        public int CodeID;
        public int PCodeID;
        public string Scope;

        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.Position.Tree).Assembly;	//어셈블리 선언
            #endregion

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

            if (CodeID == 0)
            {
                Zeospace.DeskPlus.Common.DAL.Code dal = new DeskPlus.Common.DAL.Code();
                CodeID = dal.GetRootCode(CurrentUserInfo.CompanyID, "T", true);
            }

            if (CodeID > 0)
            {
                Zeospace.DeskPlus.Common.DAL.Code dal = new DeskPlus.Common.DAL.Code();
                List<Zeospace.DeskPlus.Common.BE.CodeInfo> al = dal.GetCodeByID(CurrentUserInfo.CompanyID, CodeID);
                foreach (Zeospace.DeskPlus.Common.BE.CodeInfo info in al)
                {
                    Scope = info.Scope;
                    PCodeID = info.ParentCodeID;
                }
            }


            UCCodeTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            UCCodeTreeScript1.CodeID = CodeID;
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CodeID = GetDecryptParam("codeID", "0").ToNumber<int>();
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


        

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)
        #endregion
    }
}