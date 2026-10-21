using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Common.Popup
{
    public partial class ViewCir : Base.BasePageApproval
    {
        #region 변수설정
        public int CirDocumentID;
        public string FolderStatusID;
        public string CabinetID;
        public int AttachFileCount;
        public int BaseDocCount;
        public int DocumentID;
        public int ChildDocID;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Popup.ViewCir).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.CirculationDocument dal = new DAL.CirculationDocument();
            htaFormHTML.Value = dal.GetContentHtml(CurrentUserInfo.CompanyID, CirDocumentID);

            DAL.CirculationUser cuDal = new DAL.CirculationUser();
            cuDal.Mod(CurrentUserInfo.CompanyID, CirDocumentID, CurrentUserInfo.UserID);

            DAL.CirculationAttachFile attDal = new DAL.CirculationAttachFile();
            AttachFileCount = attDal.GetAttachCnt(CurrentUserInfo.CompanyID, CirDocumentID);

            DAL.CirculationBaseDocument baseDal = new DAL.CirculationBaseDocument();
            BaseDocCount = baseDal.GetBaseDocCnt(CurrentUserInfo.CompanyID, CirDocumentID);

            List<BE.CirculationDocumentInfo> al = dal.GetDocList(CurrentUserInfo.CompanyID, CirDocumentID);
            foreach (var cirDocInfo in al)
            {
                DocumentID = cirDocInfo.DocumentID;
            }

            DAL.Document dpDal = new DAL.Document();
            List<BE.DocumentInfo> list = dpDal.GetDocList(CurrentUserInfo.CompanyID, DocumentID);
            foreach (BE.DocumentInfo info in list)
            {
                ChildDocID = info.LastDocID;
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnShowImage.Text = resMng.GetString("ShowImage");   // 이미지 보기                
                cbtnPrint.Text = resMng.GetString("Print");     //인쇄
                cbtnOpinion.Text = resMng.GetString("ViewOpinion"); //첨언추가
                cbtnZIP.Text = resMng.GetString("SelectDownLoadZIP");    // ZIP 선택다운로드
                cbtnOpinionInfo.Text = resMng.GetString("Opinion"); //결재의견
            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CirDocumentID = GetDecryptParam("cirDocumentID", "").ToNumber<int>();
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            CabinetID = GetDecryptParam("cabinetID", "");//ID
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}