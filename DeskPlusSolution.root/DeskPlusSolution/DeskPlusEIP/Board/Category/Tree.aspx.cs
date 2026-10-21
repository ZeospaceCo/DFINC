using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Category
{
    public partial class Tree : Base.BasePageBoard
    {
        #region 변수설정

        private string eventArgs = String.Empty;
        public string AccActive = "0"; //accordianActive
        public string MyAuthority = "";
        public string OptionUseDept = "";
        public string OptionListType = "";
        public string OptionUseView = "";

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Category.Tree).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            //통합게시판 회사아이디
            BoaComID = BLL.Category.GetCompanyCategoryInfo(CatID);

            if (CatID == -2) BoaComID = CurrentUserInfo.CompanyID;

            if (BoaComID == 0) UCBoardTreeScript1.UseTotBoard = true;
            
            //GetCategory();  트리에서 초기 컨텐츠를 loading하지 않고 Default 에서 loading  ByFreeman 20160615
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            AccActive = GetDecryptParam("accActive", "0");
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
            }
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        private void GetCategory()
        {
            //통합게시판 회사 아이디
            if (BoaComID == 0) UCBoardTreeScript1.UseTotBoard = true;

            DAL.Category dal = new DAL.Category();
            BLL.Category bizCat = new BLL.Category();
            DataSet ds = dal.GetCategory(BoaComID, CatID);

            if (ds != null)
            {
                Account.DAL.Dept dalDept = new Account.DAL.Dept();
                DataSet deptList = dalDept.GetDept(BoaComID, "deptList", true);
                DataView dvDeptList = deptList.Tables[0].DefaultView;

                BLL.Authority bizAuth = new BLL.Authority();

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    OptionUseDept = dbRow["OptionUseDept"].ToString();
                    OptionListType = dbRow["OptionListType"].ToString();
                    OptionUseView = dbRow["OptionUseView"].ToString();

                    // 작성목적   : 게시판 권한테이블에서 회사,부서,개인 아이디값 가져오기
                    // 권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
                    NameValueCollection nvcAuth = new NameValueCollection();

                    //읽기 권한 존재하는지 체크
                    nvcAuth = bizCat.GetMyAuthCategory(BoaComID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Read, true, false);

                    if (nvcAuth.Count > 0)
                        MyAuthority = "true/3";//3 : 읽기+꼬리말
                    else
                        MyAuthority = "false/"; //권한없음.

                    //MyAuthority = bizAuth.GetAutority(BoaComID, CatID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, dvDeptList);
                }
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 즐겨찾기 폴더 삭제하기
        /// </summary>
        private void DelFavorite()
        {
            int favoriteID = tbCID.Text.Trim().ToNumber<int>();

            if (favoriteID > 0)
            {
                DAL.Favorite dal = new DAL.Favorite();
                dal.DelFavorite(BoaComID, CurrentUserInfo.UserID, favoriteID);

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("dcparm", Dcparm.ToString());
                paraDic.Add("menuCatID", MenuCatID.ToString());
                paraDic.Add("catID", CatID.ToString());
                paraDic.Add("accActive", "2");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                string goPage = "Tree.aspx?p=" + encParams;
                Response.Redirect(goPage, false);

                //Response.Redirect(Request.Url.ToString(), false);
            }
        }

        #endregion Method (함수)

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            LoadParam();

            DelFavorite();
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion Event Handler (이벤트 핸들러)
    }
}