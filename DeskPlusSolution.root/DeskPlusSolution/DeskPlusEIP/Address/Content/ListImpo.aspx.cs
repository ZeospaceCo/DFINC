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
    public partial class ListImpo : Base.BasePageAddress
    {
        #region 변수설정        
        public int CurrentPage = 0;
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = "10"; //게시판 리스트 개수        
        public string SearchKey = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchFilter = string.Empty; //ㄱㄴㄷㄹ
        public string Order = string.Empty; //sort
        public string MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Content.ListImpo).Assembly;	//어셈블리 선언
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
            #region PageSize 설정하기
            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.ADDRESS, "ListCount", "10");//페이지리스트수    
            ctbSearch.Text = SearchValue; //검색어

            DAL.CategoryUser dal = new DAL.CategoryUser();
            BE.CategoryUserInfo info = new BE.CategoryUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CategoryID = 0;
            info.UserID = CurrentUserInfo.UserID;
            info.Types = "IG";

            List<BE.CategoryUserInfo> al = dal.GetCategoryUserLists(info);

            foreach (BE.CategoryUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();                
            }

            ddlListCount.SelectedValue = RecordPerPage;
            #endregion

            GetListCount();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {            
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
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
                cbtnDel.Text = resMng.GetString("Clear");                  // "해제";                
                cbtnMail.Text = resMng.GetString("Mail");                   // "편지쓰기";
                cbtnPrint.Text = resMng.GetString("Print");                 // "인쇄";
                cbtnSearch.Text = resMng.GetString("Search");               // "검색"

                ddlSearchField.Items.Add(new ListItem(resMng.GetString("FirstName"), "ManyName"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("Email"), "Mail"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("DeptName"), "DeptManyName"));
                ddlSearchField.Items.Add(new ListItem(resMng.GetString("CellPhone"), "MobilePhone"));

                ddlSearchField.SelectedValue = SearchKey; //검색필드
            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 총 카운트
        /// </summary>
        private void GetListCount()
        {
            Address.DAL.ImpoUser dal = new DAL.ImpoUser();
            TotalCount = dal.GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SearchKey, SearchValue, FilterString()).ToString();
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
            string fieldName = "ManyName";

            string filterStr = "";
            if (SearchFilter.Length > 0)
            {
                switch (SearchFilter)
                {
                    case "ㄱ":
                        filterStr = fieldName + " >='ㄱ' and " + fieldName + " < 'ㄴ'";
                        break;
                    case "ㄴ":
                        filterStr = fieldName + " >='ㄴ' and " + fieldName + " < 'ㄷ'";
                        break;
                    case "ㄷ":
                        filterStr = fieldName + " >='ㄷ' and " + fieldName + " < 'ㄹ'";
                        break;
                    case "ㄹ":
                        filterStr = fieldName + " >='ㄹ' and " + fieldName + " < 'ㅁ'";
                        break;
                    case "ㅁ":
                        filterStr = fieldName + " >='ㅁ' and " + fieldName + " < 'ㅂ'";
                        break;
                    case "ㅂ":
                        filterStr = fieldName + " >='ㅂ' and  " + fieldName + " < 'ㅅ'";
                        break;
                    case "ㅅ":
                        filterStr = fieldName + " >='ㅅ' and " + fieldName + " < 'ㅇ'";
                        break;
                    case "ㅇ":
                        filterStr = fieldName + " >='ㅇ' and " + fieldName + " < 'ㅈ'";
                        break;
                    case "ㅈ":
                        filterStr = fieldName + " >='ㅈ' and " + fieldName + " < 'ㅊ'";
                        break;
                    case "ㅊ":
                        filterStr = fieldName + " >='ㅊ' and " + fieldName + " < 'ㅋ'";
                        break;
                    case "ㅋ":
                        filterStr = fieldName + " >='ㅋ' and " + fieldName + " < 'ㅌ'";
                        break;
                    case "ㅌ":
                        filterStr = fieldName + " >='ㅌ' and " + fieldName + " < 'ㅍ'";
                        break;
                    case "ㅍ":
                        filterStr = fieldName + " >='ㅍ' and " + fieldName + " < 'ㅎ'";
                        break;
                    case "ㅎ":
                        filterStr = fieldName + " >='ㅎ' and " + fieldName + " < 'A'";
                        break;
                    case "A":
                        filterStr = fieldName + " >='A' and " + fieldName + " <= 'Z'";
                        break;
                    case "1":
                        filterStr = fieldName + " >='0' and " + fieldName + " <= '9'";
                        break;
                }
            }

            return filterStr;
        }

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}