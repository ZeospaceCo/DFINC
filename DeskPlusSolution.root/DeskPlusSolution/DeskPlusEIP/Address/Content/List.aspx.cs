using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Address.Web.Content
{
    public partial class List : Base.BasePageAddress
    {
        #region 변수설정
        public string CategoryID;
        public string Types;
        public string RTypes;
        public int CurrentPage = 0;
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = "10"; //게시판 리스트 개수  
        public string SearchOption = string.Empty; //옵션(하위부서 포함/제외)
        public string SearchKey = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchFilter = string.Empty; //ㄱㄴㄷㄹ
        public string Order = string.Empty; //sort
        public string SiteMap = string.Empty;//사이트맵

        public string MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Content.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }

            
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자  : 심재운
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            #region admin 관리자 체크
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleAddress = CurrentUserInfo.IsInRole("address");//address admin
            cbtnAdmin.Visible = (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleAddress) ? true : false;
            #endregion

            #region PageSize 설정하기
            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.ADDRESS, "ListCount", "10");//페이지리스트수            

            DAL.CategoryUser dal = new DAL.CategoryUser();
            BE.CategoryUserInfo info = new BE.CategoryUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = CategoryID.ToNumber<int>();
            info.UserID = CurrentUserInfo.UserID;
            info.Types = Types;

            List<BE.CategoryUserInfo> al = dal.GetCategoryUserLists(info);

            foreach (BE.CategoryUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();                
            }

            ddlListCount.SelectedValue = RecordPerPage;
            #endregion


            GetListCount();

            ctbSearch.Text = SearchValue; //검색어
            cbtnFolderManage.Visible = false; //하위그룹순서
            cbtnSimpleAdd.Visible = false; //간편추가
            cbtnAdd.Visible = false;//추가
            cbtnCopy.Visible = false;//복사
            cbtnMove.Visible = false;//이동
            cbtnDel.Visible = false;//삭제
            cbtnSend.Visible = false;//타인에게 전달

            cbtnFavorite.Visible = true; //즐겨찾기            
            cbtnOut.Visible = true;//외부주소록
            cbtnPrint.Visible = true;//인쇄

            //공용주소록, 회사주소록, 직급주소록, 직책 주소록(복사기능 추가 및 즐겨찾기 기능 제공)
            if (RTypes == "CG" || RTypes == "GG" || RTypes == "TG" || RTypes == "DG")
            {
                cbtnCopy.Visible = true;//복사

                if (RTypes == "GG")
                    cbtnCopy.Visible = false;

                //직급주소록 하단, 직책주소록 하단은 즐겨찾기 기능 제공
                if ((RTypes == "TG" && CategoryID == "1") || (RTypes == "DG" && CategoryID == "2"))
                    cbtnFavorite.Visible = false; //즐겨찾기
            }
            else if (RTypes == "PG" && CategoryID != "0") //개인주소록 하단
            {
                cbtnSimpleAdd.Visible = true;
                cbtnAdd.Visible = true;
                cbtnCopy.Visible = true;
                cbtnMove.Visible = true;
                cbtnDel.Visible = true;
                cbtnSend.Visible = true;
                cbtnFolderManage.Visible = true;
            }
            else if (RTypes == "PG" && CategoryID == "0") //개인주소록 루트
            {
                cbtnCopy.Visible = true;
                cbtnMove.Visible = true;
                cbtnDel.Visible = true;
                cbtnSend.Visible = true;
                cbtnFolderManage.Visible = true;
            }
            else if (RTypes == "RG") //전달받은 주소록
            {
                cbtnCopy.Visible = true;
                cbtnMove.Visible = true;
                cbtnDel.Visible = true;
                cbtnFavorite.Visible = false; //즐겨찾기
            }
            else if (RTypes == "FG") //즐겨찾기 주소록
            {
                cbtnFavorite.Visible = false;
                cbtnFolderManage.Visible = true;
            }

            if (CategoryID == "0") cbtnFavorite.Visible = false; //즐겨찾기 감추기

            GetSiteMap(); //사이트맵

            // 주소록별 하위그룹 검색 옵션 드롭다운 리스트 보이기
            if (AppSettingHelper.GetAppSetting(AppSettingType.ADDRESS, "SubSearchList", "N") == "N" ||
                  (RTypes == "TG" || RTypes == "DG" || RTypes == "RG" || RTypes == "FG" || CategoryID == "0"))
            {
                ddlSearchOption.Visible = false;
            }
            else
            {
                ddlSearchOption.Visible = true;
            }

        }

        #endregion

        #region (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0");
            RTypes = GetDecryptParam("rTypes", "");
            Types = GetDecryptParam("types", "");
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchOption = GetDecryptParam("searchOption", "");
            SearchKey = GetDecryptParam("searchKey", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchFilter = GetDecryptParam("searchFilter", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "Orders ASC" : Order;
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
                cbtnSimpleAdd.Text = resMng.GetString("AddSimple");         //"간편추가";
                cbtnAdd.Text = resMng.GetString("AddDetail");               //"상세추가";
                cbtnCopy.Text = resMng.GetString("Copy");                   //"복사";
                cbtnMove.Text = resMng.GetString("Move");                   //"이동";
                cbtnDel.Text = resMng.GetString("Delete");                  // "삭제";                               
                cbtnSend.Text = resMng.GetString("Send");                   // "전달";                
                cbtnMail.Text = resMng.GetString("Mail");                   // "편지쓰기";                
                cbtnPrint.Text = resMng.GetString("Print");                 // "인쇄";
                cbtnOut.Text = resMng.GetString("OutAddress");              // "외부주소";
                cbtnFavorite.Text = resMng.GetString("FolderFavorite");           // "즐겨찾기";
                cbtnSearch.Text = resMng.GetString("Search");               // "검색"
                cbtnFolderManage.Text = resMng.GetString("FolderManage"); //그룹관리
                cbtnAdmin.Text = resMng.GetString("Admin"); //관리자
                //cbtnPrivate.Text = resMng.GetString("PrivateAddress") + resMng.GetString("Add");    //개인주소록추가
                cbtnPrivate.Text = resMng.GetString("AddPersonal");    //개인주소록추가

                ddlSearchField.Items.Add(new ListItem(resMng.GetString("FirstName"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("Email"), "Mail"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("DeptName"), "DeptManyName"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("CellPhone"), "MobilePhone"));

                ddlSearchOption.Items.Add(new ListItem(resMng.GetString("PresentFolder"), "List")); //현재그룹
                ddlSearchOption.Items.Add(new ListItem(resMng.GetString("SubFolder"), "ListWithSub")); //하위그룹

                ddlSearchOption.SelectedValue = SearchOption;
                ddlSearchField.SelectedValue = SearchKey; //검색필드
            }
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 용도 : 사이트맵 보이기
        /// </summary>
        private void GetSiteMap()
        {
            BLL.List biz = new BLL.List();

            SiteMap = biz.GetSitemapName(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CategoryID.ToNumber<int>(),
                Types, PreLang);

            if (SiteMap == "")
            {
                switch (Types)
                {
                    case "GG":
                        SiteMap = GetResource("CompanyAddress");
                        break;
                    case "CG":
                        SiteMap = GetResource("CommonAddress");
                        break;
                    case "PG":
                        SiteMap = GetResource("PrivateAddress");
                        break;
                    case "RG":
                        SiteMap = GetResource("ForwardAddress");
                        break;
                    case "TG":
                        SiteMap = GetResource("TitleAddress");
                        break;
                    case "DG":
                        SiteMap = GetResource("TitleAddress");
                        break;
                    case "FG":
                        SiteMap = GetResource("Favorite");
                        break;
                }
            }

        }

        

        /// <summary>
        /// 작성자   : shimpark
        /// 해당 카테고리 총 카운트
        /// </summary>
        private void GetListCount()
        {            
            string typeMember = "";
            int userID = 0;

            switch (Types)
            {
                case "GG":
                    typeMember = "GM";
                    break;
                case "CG":
                    typeMember = "CM";
                    break;
                case "PG":
                    typeMember = "PM";
                    userID = CurrentUserInfo.UserID;
                    break;
                case "RG":
                    typeMember = "RM";
                    userID = CurrentUserInfo.UserID;
                    break;
                case "TG":
                    typeMember = "TM";                    
                    break;
                case "DG":
                    typeMember = "DM";
                    break;
                case "FG":
                    typeMember = "FM"; //가짜 (데이터 안나올거임)
                    userID = CurrentUserInfo.UserID;
                    break;
            }

            BLL.List biz = new BLL.List();
            string catSubList = biz.GetSubCatList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CategoryID, Types);
            

            Address.DAL.Private pBiz = new DAL.Private();
            TotalCount = pBiz.GetPageCountWithSub(CurrentUserInfo.CompanyID, catSubList, userID, typeMember, SearchOption, SearchKey, SearchValue, FilterString()).ToString();
        }



        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 필터링 쿼리 구문
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-02-02
        /// </summary>
        /// <returns></returns>
        private string FilterString()
        {
            return BLL.List.FilterString(SearchFilter);            
        }

        #endregion

        #region (속성)
        
        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}