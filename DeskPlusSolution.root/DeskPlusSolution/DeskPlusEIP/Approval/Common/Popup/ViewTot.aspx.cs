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
    public partial class ViewTot : Base.BasePageApproval
    {
        #region 변수설정
        public int ChildDocID;
        public int DocumentID;
        public string SRH = "";
        public bool IsAuth = true;
        public string ApprovalState = "";
        public string FolderStatusID = "";
        public string CabinetID = "";
        public string Number2 = "";
        public string AccountTag = "";
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Popup.ViewTot).Assembly;	//어셈블리 선언
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

            // 권한 체크
            IsAuth = GetIsAuthDoc(DocumentID);
            if (IsAuth)
            {
                // 문서정보 가져오기
                GetDocInfo(DocumentID);

                // 참조문서 날짜 업데이트
                UpdConfirmDateCA();

                // 전달문서함이면 문서확인
                if (CabinetID.IndexOf("Forward") > -1)
                {
                    UpdConfirmDate();
                }

                IsImgExist();
            }
        }

        #endregion

        #region (함수)
        /// <summary>
        /// 권한체크
        /// </summary>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        private bool GetIsAuthDoc(int _documentID)
        {
            bool rtnval = true;
            if (!SRH.Equals(""))
            {
                DAL.Document dal = new DAL.Document();
                rtnval = dal.GetIsAuthDoc(CurrentUserInfo.CompanyID, _documentID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, CurrentUserInfo.UserID);
            }
            return rtnval;
        }

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

                cbtnPrint.Text = resMng.GetString("Print"); //"인쇄";
                cbtnPDF.Text = resMng.GetString("BatchPDF");
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
            DocumentID = GetDecryptParam("documentID", "").ToNumber<int>();
            SRH = GetDecryptParam("srh", "");   //검색페이지에서 호출
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            CabinetID = GetDecryptParam("cabinetID", "");//ID
        }

        /// <summary>
        /// 문서정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        public void GetDocInfo(int _documentID)
        {
            #region 문서내용
            DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
            List<BE.DocumentProcessInfo> list = dpDal.GetDocumentUnion(CurrentUserInfo.CompanyID, _documentID);
            foreach (BE.DocumentProcessInfo info in list)
            {
                ChildDocID = info.LastDocID;
                htaFormHTML.Value = info.ContentHTML;
                ApprovalState = info.ApprovalState;
                Number2 = info.Number2;
                AccountTag = info.AccountTag;
            }
            #endregion

            #region 근거문서/첨부파일 건수
            if (!htaFormHTML.Value.Equals(""))
            {
                DAL.AttachFile dal = new DAL.AttachFile();
                headerAttachCnt.InnerText = dal.GetAttachFileCntByPath(CurrentUserInfo.CompanyID, ChildDocID).ToString();

                DAL.BaseDocument dalBase = new DAL.BaseDocument();
                headerBaseDocCnt.InnerText = dalBase.GetBaseDocCntByPath(CurrentUserInfo.CompanyID, ChildDocID).ToString();
            }
            else
            {
                headerAttachCnt.InnerText = "0";
                headerBaseDocCnt.InnerText = "0";
            }
            #endregion

            if (AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBatchPDF", "0").Equals("1") && !headerAttachCnt.InnerText.Equals("0"))
            {
                cbtnPDF.Visible = true;
            }
            else
            {
                cbtnPDF.Visible = false;
            }
        }

        /// <summary>
        /// 첨부파일에 이미지가 없으면 이미지보기 버튼 감춤
        /// </summary>
        private void IsImgExist()
        {
            if (!htaFormHTML.Value.Equals(""))
            {
                // 부모 문서번호들 가져오기
                string upPathID = "";
                if (ApprovalState.ToUpper().Equals("C") || ApprovalState.ToUpper().Equals("R"))
                {
                    if (ApprovalState.ToUpper().Equals("R") && AccountTag.IndexOf("ZP").Equals(0))
                    {
                        DAL.DocumentProcess proc = new DAL.DocumentProcess();
                        upPathID = proc.GetUpPathID(CurrentUserInfo.CompanyID, ChildDocID);
                    }
                    else
                    {
                        DAL.Document dalDoc = new DAL.Document();
                        upPathID = dalDoc.GetUpPathID(CurrentUserInfo.CompanyID, ChildDocID);
                    }
                }
                else
                {
                    DAL.DocumentProcess proc = new DAL.DocumentProcess();
                    upPathID = proc.GetUpPathID(CurrentUserInfo.CompanyID, ChildDocID);
                }
                upPathID = upPathID.TrimStart(',');
                string[] arrUpPathID = upPathID.Split(',');

                int imgCnt = 0;
                // 부모문서 갯수만큼 반복    
                for (int i = 0; i < arrUpPathID.Length; i++)
                {
                    DAL.AttachFile dal = new DAL.AttachFile();
                    List<BE.AttachFileInfo> al = dal.GetAttachImageLists(CurrentUserInfo.CompanyID, arrUpPathID[i].ToNumber<int>());

                    imgCnt += al.Count;
                }

                if (imgCnt == 0) cbtnShowImage.Visible = false;
            }
            else
            {
                cbtnShowImage.Visible = false;
            }
        }

        /// <summary>
        /// 전달문서 읽은날짜 업데이트
        /// </summary>
        public void UpdConfirmDate()
        {
            DAL.ForwardUser dpDal = new DAL.ForwardUser();
            bool confirm = dpDal.ConfirmDoc(CurrentUserInfo.CompanyID, DocumentID, CurrentUserInfo.UserID);
        }

        /// <summary>
        /// 참조문서 읽은날짜 업데이트
        /// </summary>
        public void UpdConfirmDateCA()
        {
            DAL.CarbonCopyUser dpDal = new DAL.CarbonCopyUser();
            bool confirm = dpDal.UpdViewingDate(CurrentUserInfo.CompanyID, DocumentID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}