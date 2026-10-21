using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.Document
{
    public partial class ViewDoc : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public int ChildDocID;
        public int DocumentID;
        public int GroupID;
        public string AccountTag = String.Empty;
        public string Number2 = string.Empty;

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.Document.ViewDoc).Assembly;	//어셈블리 선언
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

            // 문서정보 가져오기
            GetDocInfo();

            // 2-1. 버튼 정의 
            // EDM 버튼
            string IsFolderToEDMS = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsFolderToEDMS", "0");


            if (IsFolderToEDMS != "1")
                cbtnEDM.Visible = false;

            IsImgExist();

            cbtnPDF.Visible = (AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBatchPDF", "0").Equals("1")) ? true : false;
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
                cbtnDel.Text = resMng.GetString("DelCompletely"); //"영구삭제";
                cbtnShowImage.Text = resMng.GetString("ShowImage");   // 이미지 보기                
                cbtnMail.Text = resMng.GetString("SendMail"); //"메일발송";
                cbtnClose.Text = this.GetResource("Close");

                cbtnPrint.Text = resMng.GetString("Print"); //"인쇄";
                cbtnZipFile.Text = resMng.GetString("FileBackup"); //"파일백업";
                cbtnHistory.Text = resMng.GetString("ModHistory"); //"수정내역";
                cbtnDocInfo.Text = resMng.GetString("AppInfo"); //"결재정보";
                cbtnEDM.Text = this.GetResource("SendEDM");     // EDMS 이관
                cbtnDisuse.Text = this.GetResource("DocDisuse");     // 문서폐기

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
        }

        /// <summary>
        /// 문서정보 가져오기
        /// </summary>
        public void GetDocInfo()
        {
            DAL.Document dpDal = new DAL.Document();
            List<BE.DocumentInfo> list = dpDal.GetDocList(CurrentUserInfo.CompanyID, DocumentID);
            foreach (BE.DocumentInfo info in list)
            {
                AccountTag = info.AccountTag;
                GroupID = info.GroupID;

                ChildDocID = info.LastDocID;
                htaFormHTML.Value = info.ContentHTML;
                htaFormTEXT.Value = info.ContentTEXT;

                Number2 = info.Number2;

                AccessTitleName = GetAccessTitleInfo(info.CompanyID, info.AccessTitleID);
            }
        }

        // 열람권한
        public string GetAccessTitleInfo(int _companyID, int _titleID)
        {
            if (_companyID.Equals(0) || _titleID.Equals(0)) return "";
            try
            {
                string rtnval = "";

                if (_titleID >= 10000)
                {
                    DAL.AccessGrade biz = new DAL.AccessGrade();

                    DataTableReader dtr = biz.GetDataByAccessID(CurrentUserInfo.CompanyID, _titleID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                rtnval = this.GetLangName(
                                        dtr["NameBase"].ToString(),
                                        dtr["NameKo"].ToString(),
                                        dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                        dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(),
                                        dtr["NameZhTw"].ToString(), PreLang);

                            }
                        }
                    }
                }
                else
                {
                    Zeospace.DeskPlus.Common.DAL.Code dal = new DeskPlus.Common.DAL.Code();
                    List<Zeospace.DeskPlus.Common.BE.CodeInfo> list = dal.GetCodeByID(_companyID, _titleID);


                    if (list != null)
                    {
                        foreach (Zeospace.DeskPlus.Common.BE.CodeInfo info in list)
                        {
                            rtnval = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                        }
                    }
                }
                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetAccessTitleInfo(" + _companyID.ToString() + "," + _titleID.ToString() + ")", ex);
                return "";
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
                string upPathID = dalDoc.GetUpPathID(CurrentUserInfo.CompanyID, ChildDocID);
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
        public string AccessTitleName
        {
            get { return GetViewStateString("AccessTitleName", ""); }
            set { ViewState["AccessTitleName"] = value; }
        }
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}