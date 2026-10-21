using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Code
{
    public partial class List : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CurrentPage = 0;
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = "5"; //게시판 리스트 개수        
        public string SearchField = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어        
        public string Order = string.Empty; //sort
        public string ParentCodeID;
        public int SysCodeID; //syscodeid값
        public string SysCodeName;
        
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Code.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {                
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetListCount();
            LoadSysCode();

            ddlSearchField.Items.Clear();
            //ddlSearchField.Items.Add(new ListItem("----", ""));
            ddlSearchField.Items.Add(new ListItem(resMng.GetString("CodeKey"), "CodeID"));
            ddlSearchField.Items.Add(new ListItem(resMng.GetString("CodeName"), "ManyName"));
            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;

            ddlListCount.SelectedValue = RecordPerPage;

            if (ParentCodeID == "0")
            {
                cbtnDel.Visible = false;
                cbtnActive.Visible = false;
            }
        }



        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {            
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "5");
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "codeorder asc" : Order;            

            ParentCodeID = GetDecryptParam("parentCodeID", "");
            SysCodeID = GetDecryptParam("sysCodeID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnDel.Text = resMng.GetString("Del");
                cbtnMove.Text = resMng.GetString("Orders");
                cbtnActive.Text = resMng.GetString("ActiveUnActive");
                cbtnSearch.Text = resMng.GetString("Search");
                cbtnCatAdd.Text = resMng.GetString("ModCategory");
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 타이틀 명 가져오기
        /// </summary>
        private void LoadSysCode()
        {
            if (SysCodeID > 0)
            {
                Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
                List<Zeospace.DeskPlus.Common.BE.SYSCodeInfo> al = dal.GetSysCode(CurrentUserInfo.CompanyID, SysCodeID);

                foreach (Zeospace.DeskPlus.Common.BE.SYSCodeInfo info in al)
                {
                    SysCodeName = GetLangName(
                        info.NameBase, info.NameKo, info.NameEn, info.NameJa, 
                        info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                }
            }

            if (SysCodeName == "") SysCodeName = resMng.GetString("SysCodeManage");
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 카운트 가져오기
        /// </summary>
        private void GetListCount()
        {
            Zeospace.DeskPlus.Common.DAL.SYSCode dal = new DeskPlus.Common.DAL.SYSCode();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, ParentCodeID, SearchField, SearchValue).ToString();            
        }

        #endregion

        #region Event Handler (이벤트 핸들러)       
        

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}