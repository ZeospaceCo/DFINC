using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Admin.AccessGrade
{
    public partial class List : Base.BasePageApprovalAdmin
    {
        #region 변수설정
        public int TotalItemCount;      //총카운트
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Admin.AccessGrade.List).Assembly;	//어셈블리 선언
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

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlListCount.SelectedValue = this.RecordPerPage.ToString();

            // 페이지설정
            #region PageSize 설정하기

            DAL.FolderPageSize dalPageSize = new DAL.FolderPageSize();
            List<BE.FolderPageSizeInfo> al = dalPageSize.GetList(CurrentUserInfo.CompanyID,
                "Admin_Access", CurrentUserInfo.UserID, "Admin");

            foreach (BE.FolderPageSizeInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }
            #endregion

            GetListCount();
            PageSize();

            #region 직급체크
            Zeospace.DeskPlus.Common.DAL.Code cDal = new Zeospace.DeskPlus.Common.DAL.Code();
            DataSet ds = cDal.GetCodeByScope(CurrentUserInfo.CompanyID, "T", true);

            string NameBase = string.Empty; ;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {              
                if (dr["AccessID"].ToString() == "" || dr["AccessID"].ToString() == "0")
                {
                    NameBase = NameBase + dr["NameBase"].ToString().ToReverseXSS() + ",";
                }
            }

            spnAccessGradeDeptN.InnerText = NameBase;
            #endregion            
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
                cbtnActive.Text = this.resMng.GetString("ActiveNInactive");     //"활성/비활성화";
                cbtnSeq.Text = this.resMng.GetString("ChangeOrder3");       //정렬순서
                cbtnBase.Text = this.resMng.GetString("SetBase");                    //기본값설정

                cbtnAdd.Text = this.resMng.GetString("Add");                    //"추가";
                cbtnDel.Text = this.resMng.GetString("Del");                    //"삭제";
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch");      //"검색";

                this.ddlSearchField.Items.Clear();
                this.ddlSearchField.Items.Add(new ListItem(resMng.GetString("AccessGradeName"), "ManyName"));      //열람등급명
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
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            DAL.AccessGrade fDal = new DAL.AccessGrade();
            TotalItemCount = fDal.GetList(CurrentUserInfo.CompanyID, SearchField, SearchValue);
        }

        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "Admin_Access", CurrentUserInfo.UserID, "Admin");

            foreach (FolderPageSizeInfo info in al)
            {
                ddlListCount.SelectedValue = info.PageSize.ToString();
            }
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}