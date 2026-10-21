using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy
{
    public partial class List : Base.BasePageApproval
    {
        #region 변수설정

        public int TotalItemCount;

        public string CabinetID;
        public string FormID;
        public string YYYY;
        public string YYYYMM;
        public string Types;//S:기본폴더; U : 수발자 관리자 추가 폴더
        public string HeaderName;

        public string Gubun;
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Carboncopy.List).Assembly;	//어셈블리 선언
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
                "Carboncopy", CurrentUserInfo.UserID, "Complete");

            foreach (BE.FolderPageSizeInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize;
            }
            #endregion

            GetListCount();
            PageSize();

            if (Types == "S" && CabinetID.ToNull().Length > 0) //관리자가 설정한 cabinet 테이블에서 검색
            {
                DAL.Cabinet dal = new DAL.Cabinet();
                var alList = dal.GetCabinet(CurrentUserInfo.CompanyID, CabinetID, true);
                foreach (var listinfo in alList)
                {
                    #region 전체경로

                    /*
                    string name = this.GetLangName(
                            listinfo.PathNameBase,
                            listinfo.PathNameKo,
                            listinfo.PathNameEn, listinfo.PathNameJa,
                            listinfo.PathNameFr, listinfo.PathNameZhcn,
                            listinfo.PathNameZhtw, PreLang);

                    switch (PreLang)
                    {
                        case "ko":
                            name = (listinfo.NameKo == "") ? listinfo.PathNameBase : name;
                            break;
                        case "en":
                            name = (listinfo.NameEn == "") ? listinfo.PathNameBase : name;
                            break;
                        case "ja":
                            name = (listinfo.NameJa == "") ? listinfo.PathNameBase : name;
                            break;
                        case "fr":
                            name = (listinfo.NameFr == "") ? listinfo.PathNameBase : name;
                            break;
                        case "zhcn":
                            name = (listinfo.NameZhcn == "") ? listinfo.PathNameBase : name;
                            break;
                        case "zhtw":
                            name = (listinfo.NameZhtw == "") ? listinfo.PathNameBase : name;
                            break;
                    }

                    ltlHeader.Text = name.Substring(1, name.Length - 1);
                    */

                    ltlHeader.Text = this.GetLangName(
                          listinfo.NameBase,
                          listinfo.NameKo,
                          listinfo.NameEn, listinfo.NameJa,
                          listinfo.NameFr, listinfo.NameZhcn,
                          listinfo.NameZhtw, PreLang);  

                    #endregion
                }
            }


            if (ltlHeader.Text.Length == 0) ltlHeader.Text = HeaderName;

            TargetPage = Request.Url.AbsolutePath;

            // 초기메뉴설정 버튼제어
            cbtnDefault.Visible = (Gubun.Equals(CabinetID)) ? true : false;
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
                cbtnAdmin.Text = this.resMng.GetString("Administrator"); // 관리자           
                cbtnSearch.Text = this.resMng.GetString("OrganBtnSearch"); // 검색
                cbtnForward.Text = this.resMng.GetString("Forward");  //"전달";
                cbtnDel.Text = this.resMng.GetString("Btn_Del"); //삭제

                cbtnDefault.Text = this.resMng.GetString("DefaultMenu"); // 초기메뉴로 설정

                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("Subject"), "Subject"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportUser"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DocNumber"), "DocNumber"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("DeptName"), "DeptManyName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ReportYear"), "Number2"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FormName2"), "FormMany"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("ContentTEXT"), "ContentTEXT"));
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
            this.Order = "CompleteDate desc";

            CabinetID = GetDecryptParam("cabinetID", "");//ID
            Types = GetDecryptParam("types", ""); //S(SYSTEM, U:USER)
            FormID = GetDecryptParam("formID", ""); //양식id
            YYYY = GetDecryptParam("yyyy", ""); //년도
            YYYYMM = GetDecryptParam("yyyymm", ""); //년월 
            HeaderName = GetDecryptParam("headerName", "");//리스트 해더 경로(동적은 없으므로 파리미터 날림)

            Gubun = GetDecryptParam("gubun", "");
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

            int FormID = 0; //양식 id

            //(CarboncopyDoc_1)
            if (CabinetID.IndexOf("CarboncopyDoc_") != -1) FormID = CabinetID.Split('_')[1].ToNumber<int>();

            DAL.CarbonCopyUser dal = new DAL.CarbonCopyUser();
            TotalItemCount = dal.GetPageCount(CurrentUserInfo.CompanyID,
                CurrentUserInfo.UserID, CurrentUserInfo.DeptID, FormID,
                startDate, endDate,
                SearchField, SearchValue);
        }

        /// <summary>
        /// 작성자 : 이문호
        /// DDL 페이지 사이즈
        /// </summary>
        private void PageSize()
        {
            DAL.FolderPageSize dal = new DAL.FolderPageSize();

            //Process : 결재상황조회 Complete : 문서함
            List<FolderPageSizeInfo> al = dal.GetList(CurrentUserInfo.CompanyID, "Carboncopy", CurrentUserInfo.UserID, "Complete");

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