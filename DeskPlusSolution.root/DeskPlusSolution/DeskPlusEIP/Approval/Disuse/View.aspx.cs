using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Disuse
{
    public partial class View : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        public int DocumentID;
        public int FormID;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Disuse.View).Assembly;	//어셈블리 선언
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
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-25
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();            

            // 문서정보 가져오기
            GetDocInfo();
            IsImgExist();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-25
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnShowImage.Text = resMng.GetString("ShowImage");   // 이미지 보기               
                cbtnList.Text = resMng.GetString("List");            //"목록";               
                cbtnPrint.Text = resMng.GetString("Print");          //인쇄              
                cbtnRestore.Text = resMng.GetString("DocRestore");     // 문서복원  
                cbtnZIP.Text = resMng.GetString("SelectDownLoadZIP");    // ZIP 선택다운로드
                cbtnDel.Text = resMng.GetString("DelCompletely"); //"영구삭제";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-25
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();
            DocumentID = GetDecryptParam("documentID", "").ToNumber<int>();                        
            FormID = GetDecryptParam("formID", "0").ToNumber<int>(); //양식id
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-25
        /// 작성목적 : 문서정보 가져오기
        /// </summary>
        public void GetDocInfo()
        {
            DAL.Document dpDal = new DAL.Document();
            int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();
            List<BE.DocumentInfo> list = dpDal.GetDocList(tmpCompanyID, DocumentID); 
            foreach (BE.DocumentInfo info in list)
            {
                ChildDocID = info.LastDocID;
                GroupID = info.GroupID;
                AccountTag = info.AccountTag;
                htaFormHTML.Value = info.ContentHTML;
                htaFormTEXT.Value = info.ContentTEXT;
            }
        }

        /// <summary>
        /// 첨부파일에 이미지가 없으면 이미지보기 버튼 감춤
        /// </summary>
        private void IsImgExist()
        {
            try
            {
                if (CurrentUserInfo.CompanyID.Equals(0) || ChildDocID.Equals(0))
                {
                    return;
                }

                // 부모 문서번호들 가져오기
                DAL.Document dalDoc = new DAL.Document();
                int tmpCompanyID = (CurrentUserInfo.CompanyID.ToString() + "000").ToNumber<int>();
                string upPathID = dalDoc.GetUpPathID(tmpCompanyID, ChildDocID); 
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
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("companyID:" + CurrentUserInfo.CompanyID.ToString() + ",documentID:" + ChildDocID.ToString() + "- 삭제된 문서입니다." + ex.Message);
            }
        }

        #endregion

        #region (프로퍼티)
        public int ChildDocID
        {
            get { return GetViewStateInt("ChildDocID", "0"); }
            set { ViewState["ChildDocID"] = value; }
        }
        public int GroupID
        {
            get { return GetViewStateInt("GroupID", "0"); }
            set { ViewState["GroupID"] = value; }
        }
        public string AccountTag
        {
            get { return GetViewStateString("AccountTag", ""); }
            set { ViewState["AccountTag"] = value; }
        }
        #endregion

        #region (이벤트핸들러)    
        #endregion
    }
}