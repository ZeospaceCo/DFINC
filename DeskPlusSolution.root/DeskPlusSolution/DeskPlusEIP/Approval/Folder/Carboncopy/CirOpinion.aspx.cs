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

namespace Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy
{
    public partial class CirOpinion : Base.BasePageApproval
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정

        public int CirDocID = 0;

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
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy.CirOpinion).Assembly;	//어셈블리 선언
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
            CirDocID = GetDecryptParam("cirDocID", "0").ToNumber<int>();
        }

        /// <summary>
        /// Sets the language.
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();


            Zeospace.DeskPlus.Approval.DAL.CirculationDocument dal = new Approval.DAL.CirculationDocument();
            var docList = dal.GetDocList(CurrentUserInfo.CompanyID, CirDocID);

            foreach (BE.CirculationDocumentInfo info in docList)
            {
                textarea.Value= info.Memo.ToNull().ToReverseXSS();
            }
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