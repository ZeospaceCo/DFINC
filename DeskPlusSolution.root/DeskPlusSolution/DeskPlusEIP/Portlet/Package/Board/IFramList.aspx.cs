using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Board.BE;
using Zeospace.DeskPlus.Board.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.Portlet.Web.Package.Board
{
    public partial class IFramList : Base.BasePagePortlet
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정

        public int CatID = 0;
        public string TotalCount = string.Empty; //총카운트
        public string RecordPerPage = string.Empty; //게시판 리스트 개수        
        public string OptionUseDept = "0";//부서별게시판(1)
        public string OptionUseView = "0"; //조회용(1)
        public string OptionListType = "0";                
        public string Order = "OptionTop desc, GroupID DESC, ListOrder ASC, WriteDate desc";

        private string MyAuthCatID = "";//내가 볼 수 있는 카테고리   
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.Board.IFramList).Assembly;	//어셈블리 선언
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
        /// 작성자 : 심재운
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();            

            GetCategory();
            GetListCount();         
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 카테고리 옵션 적용
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-05-09
        /// </summary>
        private void GetCategory()
        {
            Zeospace.DeskPlus.Board.DAL.Category dal = new Category();
            DataSet ds = dal.GetCategory(CurrentUserInfo.CompanyID, CatID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OptionUseView = dr["OptionUseView"].ToString();
                    OptionUseDept = dr["OptionUseDept"].ToString();
                    OptionListType = dr["OptionListType"].ToString();                    
                }
            }

            #region 내가 볼 수 있는 하위 카테고리

            Zeospace.DeskPlus.Board.BLL.Category biz = new DeskPlus.Board.BLL.Category();
            bool isChild = (OptionUseView == "1") ? true : false;
       
            NameValueCollection nvc = new NameValueCollection();

            if (CatID > 0)
            {
                //보기용은 하위카테고리까지 가져옴
                if (OptionUseView == "1")
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, CatID, OptionUseDept, true, BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) MyAuthCatID += s + ",";
                }
                else
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, CatID, OptionUseDept, isChild, BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) MyAuthCatID += s + ",";
                }

                MyAuthCatID = MyAuthCatID.TrimEnd(',');
            }
            else
            {
                MyAuthCatID = "-1"; //카테고리 값이 0 넘어오면 -1 로 해서 리스트 안나오게 함
            }

            #endregion
            
        }
        

        /// <summary>
        /// 작성자  : 심재운
        /// 언어 설정하기
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                
            }
        }

        /// <summary>
        /// 작성자  : 심재운
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            CatID = GetDecryptParam("catID", "0").ToNumber<int>();
            RecordPerPage = GetDecryptParam("recordPerPage", "0");
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 해당 카테고리 총 카운트
        /// </summary>
        private void GetListCount()
        {
            string startDate = "";
            string endDate = "";

            Zeospace.DeskPlus.Board.DAL.Lists list = new Lists();
            TotalCount = list.GetPageListCount(CurrentUserInfo.CompanyID, MyAuthCatID,
                startDate, endDate,
                "", "",
                OptionUseDept, CurrentUserInfo.DeptID).ToString();
        }


        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)





        #endregion
    }
}