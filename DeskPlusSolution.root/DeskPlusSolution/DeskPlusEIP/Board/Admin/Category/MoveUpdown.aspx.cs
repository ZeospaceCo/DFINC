using System;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Board.Web.Admin.Category
{
    public partial class MoveUpdown : Base.BasePageBoardAdmin
    {
        #region 변수설정

        public string FolderList = "";

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Admin.Category.MoveUpdown).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");       //"적용";
                cbtnClose.Text = resMng.GetString("Closed");         //"닫기";
                ltlTitle.Text = resMng.GetString("FolderManage"); //폴더관리
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            IsTotBoard = GetDecryptParam("isTotBoard", "0");
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //통합게시판 회사
            int companyID = (IsTotBoard == "1") ? 0 : CurrentUserInfo.CompanyID;

            StringBuilder sb = new StringBuilder();
            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetCategoryByParentCategoryID(companyID, CatID.ToString());

            if (ds != null)
            {
                foreach (DataRow dtr in ds.Tables[0].Rows)
                {
                    #region 다국어 이름 설정

                    string name = this.GetLangName(
                        dtr["NameBase"].ToString(),
                        dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                        dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                    #endregion 다국어 이름 설정

                    sb.Append("<li id='");
                    sb.Append(dtr["CategoryID"].ToString());
                    sb.Append("'>");
                    sb.Append(name);
                    sb.Append("</li>");
                }
            }

            FolderList = sb.ToString();
        }

        #endregion (함수)

        public string IsTotBoard { get; set; }
    }
}