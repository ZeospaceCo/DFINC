using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web
{
    public partial class Default : Base.BasePageBoard
    {
        #region Declaration (컨트롤 선언)
        
        #endregion

        #region 변수설정
        private string OptionUseDept = ""; //부서별 게시판 카테고리
        private string OptionListType = "";
        private string OptionUseView = "";
        private string ListTypeUser = "0";
        public string LeftURL = ""; 
        public string ContentURL = ""; 
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();                
            }
        }
        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {

        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            GetLeftURL();
            GetContentURL();
        }

        private void GetLeftURL()
        {
            // Left URL
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("dcparm", Dcparm.ToString());
            paraDic.Add("catID", CatID.ToString());
            paraDic.Add("menuCatID", MenuCatID.ToString());

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            LeftURL = "./Category/Tree.aspx?p=" + encParams;
        }

        private void GetContentURL()
        {
            string listPage = "./Content/List.aspx";
            GetCategory();
            GetCategoryUser();

            if (CatID > 0) {
                //전체용 리스트 선택시
                if (OptionListType == "0") {
                    switch (ListTypeUser) {
                        case "1":
                            listPage = "./Content/List.aspx";
                            break;
                        case "2":
                            listPage = "./Content/BlogList.aspx";
                            break;
                        case "3":
                            listPage = "./Content/GalleryList.aspx";
                            break;
                        default: listPage = "./Content/List.aspx";
                            break;
                    }
                } else {
                    switch (OptionListType) {
                        case "1":
                            listPage = "./Content/List.aspx";
                            break;
                        case "2":
                            listPage = "./Content/BlogList.aspx";
                            break;
                        case "3":
                            listPage = "./Content/GalleryList.aspx";
                            break;
                        default: listPage = "./Content/List.aspx";
                            break;
                    }
                }
            }

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("dcparm", Dcparm.ToString());
            paraDic.Add("catID", CatID.ToString());
            paraDic.Add("menuCatID", MenuCatID.ToString());
            paraDic.Add("favoriteID", "0");
            paraDic.Add("types", Types.ToString());

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            ContentURL = listPage + "?p=" + encParams;
        }

        // 카테고리 정보 가져오기
        private void GetCategory()
        {
            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CatID);

            if (CatID == -2) BoaComID = CurrentUserInfo.CompanyID;

            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategory(BoaComID, CatID);

            if (ds != null)
            {
                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    OptionUseDept = dbRow["OptionUseDept"].ToString();
                    OptionListType = dbRow["OptionListType"].ToString();
                    OptionUseView = dbRow["OptionUseView"].ToString();
                }
            }
        }

        // 카테고리 사용자별 정보 가져오기 ListType
        public void GetCategoryUser()
        {
            DAL.CategoryUser dal = new DAL.CategoryUser();
            List<BE.CategoryUserInfo> al = dal.GetList(CurrentUserInfo.CompanyID, CatID,
                CurrentUserInfo.UserID, "BL");

            foreach (BE.CategoryUserInfo info in al)
            {
                ListTypeUser = info.ListType;
            }
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)



        #endregion
    }
}