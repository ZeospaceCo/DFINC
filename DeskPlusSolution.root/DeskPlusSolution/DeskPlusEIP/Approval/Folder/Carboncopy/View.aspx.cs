using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy
{
    public partial class View : Base.BasePageApproval
    {
        #region 변수설정
        public int CirDocumentID;
        public int ChildDocID;
        public int LastDocID;
        public string FolderStatusID;

        public string CabinetID;
        public string FormID;
        public string YYYY;
        public string YYYYMM;
        public string Types;//S:기본폴더; U : 수발자 관리자 추가 폴더
        public string HeaderName;


        public int AttachFileCount;
        public int BaseDocCount;
        public int DocumentID;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy.View).Assembly;	//어셈블리 선언
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

            DAL.Document dpDal = new DAL.Document();
            List<BE.DocumentInfo> list = dpDal.GetDocList(CurrentUserInfo.CompanyID, ChildDocID);
            foreach (BE.DocumentInfo info in list)
            {
                LastDocID = info.LastDocID;
            }

            List<BE.CirculationDocumentInfo> al = dal.GetDocList(CurrentUserInfo.CompanyID, CirDocumentID);

            foreach (var cirDocInfo in al)
            {
                DocumentID = cirDocInfo.DocumentID;
            }

            IsImgExist();
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
                cbtnMail.Text = resMng.GetString("SendMail"); //메일발송
                cbtnList.Text = resMng.GetString("List"); //"목록";
                cbtnPrint.Text = resMng.GetString("Print");     //인쇄
                cbtnZipFile.Text = resMng.GetString("FileBackup"); //파일백업             
                cbtnDocInfo.Text = resMng.GetString("AppInfo"); // 결재정보
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
            ChildDocID = GetDecryptParam("childDocID", "").ToNumber<int>();
            FolderStatusID = GetDecryptParam("folderStatusID", "");

            CabinetID = GetDecryptParam("cabinetID", "");//ID
            Types = GetDecryptParam("types", ""); //S(SYSTEM, U:USER)
            FormID = GetDecryptParam("formID", ""); //양식id
            YYYY = GetDecryptParam("yyyy", ""); //년도
            YYYYMM = GetDecryptParam("yyyymm", ""); //년월 
            HeaderName = GetDecryptParam("headerName", "");//리스트 해더 경로(동적은 없으므로 파리미터 날림)
        }

        /// <summary>
        /// 첨부파일에 이미지가 없으면 이미지보기 버튼 감춤
        /// </summary>
        private void IsImgExist()
        {
            try
            {
                if (CurrentUserInfo.CompanyID.Equals(0) || CirDocumentID.Equals(0))
                {
                    return;
                }

                int imgCnt = 0;

                DAL.CirculationAttachFile dal = new DAL.CirculationAttachFile();
                List<BE.CirculationAttachFileInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, CirDocumentID);

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (var _attachInfo in attachInfo)
                {
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (_attachInfo.FileType.ToLower() == allowedExtensions[i])
                        {
                            imgCnt++;
                            break;
                        }
                    }
                }

                if (imgCnt == 0) cbtnShowImage.Visible = false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("companyID:" + CurrentUserInfo.CompanyID.ToString() + ",CirDocumentID:" + CirDocumentID.ToString() + "- 삭제된 문서입니다." + ex.Message);
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}