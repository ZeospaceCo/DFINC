using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Approval.BLL;
using System.Data;
using System.IO;

namespace Zeospace.DeskPlus.Approval.Web.Status
{
    public partial class ListApproveSlip : Base.BasePageApproval
    {
        #region 변수설정
        public string FolderStatusID;
        public int TotalItemCount;
        public int TotalProcessCount;
        public int TotalCompleteCount;
        public int TotalRelatedCount;
        public int TotalDelayCount;
        public int TotalReceiveCount;
        public int TotalReceiveDeptCount;
        public int TotalReceiveRejectCount;
        public int TotalAfterwardCount;
        public int TotalApproveCount;
        public int TotalDelaySlipCount;
        public int TotalProcessSlipCount;
        public int TotalCompleteSlipCount;
        public int TotalRelatedSlipCount;
        public int ProxyUserID;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Status.ListApproveSlip).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
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
            SetLanguage();

            GetProxyList();

            GetUserInfo();

            #region admin 관리자 체크
            cbtnAdmin.Visible = (AccessAuth) ? true : false;
            #endregion

            ddlSearchTerm.SelectedValue = SearchTerm;
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = this.RecordPerPage.ToString();

            // 페이지설정
            #region PageSize 설정하기

            DAL.FolderPageSize dalPageSize = new DAL.FolderPageSize();
            List<BE.FolderPageSizeInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                FolderStatusID, CurrentUserInfo.UserID, "Process");

            foreach (BE.FolderPageSizeInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }

            #endregion

            GetListCount();
            PageSize();

            if (FolderStatusID.ToNull().Length > 0)
            {
                DAL.FolderStatus dal = new DAL.FolderStatus();

                var alList = dal.GetFolderStatusName(CurrentUserInfo.CompanyID, FolderStatusID, true);
                foreach (var listinfo in alList)
                {
                    #region 전체경로

                    string name = this.GetLangName(
                            listinfo.NameBase,
                            listinfo.NameKo,
                            listinfo.NameEn, listinfo.NameJa,
                            listinfo.NameFr, listinfo.NameZhcn,
                            listinfo.NameZhtw, PreLang);

                    ltlHeader.Text = name;

                    #endregion
                }
            }

            TargetPage = Request.Url.AbsolutePath;
        }

        /// <summary>
        /// 사용자정보 가져오기 (인장포함)
        /// </summary>
        private void GetUserInfo()
        {

            Zeospace.DeskPlus.Account.DAL.User uDAL = new Account.DAL.User();
            List<Account.BE.UserInfo> userlist = uDAL.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);
            foreach (Account.BE.UserInfo info in userlist)
            {
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string path = baseFileDirectory + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Picture/" + info.SealImage;
                if (File.Exists(Server.MapPath(path)))
                {
                    SealPath = FullDomainPath + path;
                }
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
                cbtnApprove.Text = this.resMng.GetString("ApproveBulk"); // 일괄결재
                cbtnAdmin.Text = this.resMng.GetString("Administrator"); // 관리자
                cbtnDefault.Text = this.resMng.GetString("DefaultMenu"); // 초기메뉴로 설정
                cbtnCirculate.Text = this.resMng.GetString("Circulation"); //회람
                cbtnPost.Text = this.resMng.GetString("Post"); //게시              
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch"); // 검색

                // cbtnOtherDept.Text = "타부서문서열람";

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject")); //제목
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportUser"), "ManyName")); //상신자         
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportYear"), "Number2")); //기안년도
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FormName2"), "FormMany")); //기안양식
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("SendDeptReportUser"), "SendDeptManyName")); // 발신부서상신자

                ddlSearchTerm.Items[0].Text = this.resMng.GetString("ListType0");
                ddlSearchTerm.Items[1].Text = this.resMng.GetString("Week");
                ddlSearchTerm.Items[2].Text = this.resMng.GetString("Month");
                ddlSearchTerm.Items[3].Text = this.resMng.GetString("Period");

            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            ProxyUserID = GetDecryptParam("proxyUserID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            string startDate = "";
            string endDate = "";
            string getDate = "";

            int isBundle = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBundle", "0").ToNumber<int>();

            if (SearchTerm == "week" || SearchTerm == "month")
            {
                if (SearchTerm == "week")
                    getDate = DateHelper.GetCultureStartEndDate("week", PreLangReal);
                else if (SearchTerm == "month")
                    getDate = DateHelper.GetCultureStartEndDate("month", PreLangReal);

                startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (SearchTerm == "period")
            {
                startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            DAL.DocumentProcess dal = new DAL.DocumentProcess();
            DAL.Document dalDoc = new DAL.Document();
            // 결재후 트리메뉴 카운트를 갱신하기 위해
            if (ProxyUserID > 0)
            {
                TotalItemCount = dal.GetPageCountApproveSlip(CurrentUserInfo.CompanyID,
                    ProxyUserID, isBundle, startDate, endDate, SearchField, SearchValue);

                TotalApproveCount = dal.GetPageCountApprove(CurrentUserInfo.CompanyID, ProxyUserID, isBundle, startDate, endDate, SearchField, SearchValue);

                TotalDelayCount = dal.GetPageCountDelay(CurrentUserInfo.CompanyID,
                    ProxyUserID, startDate, endDate, SearchField, SearchValue);

                TotalDelaySlipCount = dal.GetPageCountDelaySlip(CurrentUserInfo.CompanyID,
                    ProxyUserID, startDate, endDate, SearchField, SearchValue);

                TotalReceiveCount = dal.GetPageCountReceive(CurrentUserInfo.CompanyID,
                 ProxyUserID, 0, startDate, endDate, SearchField, SearchValue);

                TotalReceiveDeptCount = dal.GetPageCountReceiveDept(CurrentUserInfo.CompanyID,
                 ProxyUserID, 0, startDate, endDate, SearchField, SearchValue);

                TotalReceiveRejectCount = dal.GetPageCountReceiveReject(CurrentUserInfo.CompanyID,
                    ProxyUserID, startDate, endDate, SearchField, SearchValue);
            }
            else
            {
                TotalItemCount = dal.GetPageCountApproveSlip(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, isBundle, startDate, endDate, SearchField, SearchValue);

                TotalApproveCount = dal.GetPageCountApprove(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, isBundle, startDate, endDate, SearchField, SearchValue);

                TotalDelayCount = dal.GetPageCountDelay(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, startDate, endDate, SearchField, SearchValue);

                TotalDelaySlipCount = dal.GetPageCountDelaySlip(CurrentUserInfo.CompanyID,
                    CurrentUserInfo.UserID, startDate, endDate, SearchField, SearchValue);

                TotalReceiveCount = dal.GetPageCountReceive(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, CurrentUserInfo.DeptID, startDate, endDate, SearchField, SearchValue);

                TotalReceiveDeptCount = dal.GetPageCountReceiveDept(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, CurrentUserInfo.DeptID, startDate, endDate, SearchField, SearchValue);

                TotalReceiveRejectCount = dal.GetPageCountReceiveReject(CurrentUserInfo.CompanyID,
                   CurrentUserInfo.UserID, startDate, endDate, SearchField, SearchValue);
            }

            TotalProcessCount = dal.GetPageCountProcess(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, startDate, endDate,
                SearchField, SearchValue);

            TotalCompleteCount = dalDoc.GetPageCountComplete(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, startDate, endDate,
                SearchField, SearchValue);

            TotalRelatedCount = dal.GetPageCountRelated(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, CurrentUserInfo.DeptID, startDate, endDate,
                SearchField, SearchValue);

            TotalAfterwardCount = dal.GetPageCountAfterward(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, startDate, endDate,
                SearchField, SearchValue);

            TotalProcessSlipCount = dal.GetPageCountProcessSlip(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, startDate, endDate,
                SearchField, SearchValue);

            TotalCompleteSlipCount = dalDoc.GetPageCountCompleteSlip(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, startDate, endDate,
                SearchField, SearchValue);

            TotalRelatedSlipCount = dal.GetPageCountRelatedSlip(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, CurrentUserInfo.DeptID, startDate, endDate,
                SearchField, SearchValue);
        }

        /// <summary>
        /// 작성자 : 이문호
        /// 작성일자 : 2012-06-25
        /// 작성목적 : 대결의뢰자 목록 가져오기
        /// </summary>
        private void GetProxyList()
        {
            Proxy proxy = new Proxy();
            var PList = proxy.GetProxyList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, PreLang);

            if (PList.Count > 0)
            {
                ddlProxyList.Items.Add(new ListItem(this.GetResource("Msg_Resource01"), "0"));
                foreach (var info in PList) ddlProxyList.Items.Add(new ListItem(info.proxyUserInfo, info.proxyUser));
            }
            else
            {
                ddlProxyList.Items.Add(new ListItem(this.GetResource("Msg_Resource02"), "0"));
            }
        }

        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, FolderStatusID, CurrentUserInfo.UserID, "Process");

            foreach (FolderPageSizeInfo info in al)
            {
                ddlListCount.SelectedValue = info.PageSize.ToString();
            }
        }

        #endregion

        #region (프로퍼티)

        /// <summary>
        /// 인장경로
        /// </summary>
        public string SealPath
        {
            get { return GetViewStateString("SealPath", ""); }
            set { ViewState["SealPath"] = value; }
        }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}