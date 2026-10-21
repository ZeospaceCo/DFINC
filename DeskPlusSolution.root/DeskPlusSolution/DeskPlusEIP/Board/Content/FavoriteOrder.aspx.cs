using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Content
{
    public partial class FavoriteOrder : Base.BasePageBoard
    {
        #region 변수설정

        public int FavoriteID;
        public string FolderList = "";

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Content.FavoriteOrder).Assembly;	//어셈블리 선언

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
            FavoriteID = GetDecryptParam("favoriteID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            StringBuilder sb = new StringBuilder();
            DataTableReader dtr = null;

            DAL.Favorite dal = new DAL.Favorite();
            dtr = dal.GetParentFavorite(CurrentUserInfo.CompanyID, FavoriteID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    BLL.Authority bizAuth = new BLL.Authority();
                    string name = "";

                    Account.DAL.Dept dalDept = new Account.DAL.Dept();
                    DataSet deptList = dalDept.GetDept(0, "deptList", true); //전체회사의 부서 가져오기
                    DataView dvDeptList = deptList.Tables[0].DefaultView;
                    BLL.Category bizCat = new BLL.Category();

                    while (dtr.Read())
                    {
                        #region 다국어 이름 설정

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                        #endregion 다국어 이름 설정

                        if (string.Compare(dtr["Types"].ToString(), "BC", true) == 0)
                        {
                            // 작성목적   : 게시판 권한테이블에서 회사,부서,개인 아이디값 가져오기
                            // 권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
                            //getAuthority = bizAuth.GetAutority(dtr["CompanyID"].ToString().ToNumber<int>(),
                            //    Convert.ToInt32(dtr["CategoryID"]), CurrentUserInfo.UserID, CurrentUserInfo.DeptID, dvDeptList);

                            //auth = bizAuth.AccessCategory(dtr["OptionUseDept"].ToString(), dtr["OptionUseView"].ToString(),
                            //        getAuthority, false, false, true);

                            NameValueCollection nvcAuth = new NameValueCollection();

                            //읽기 권한 존재하는지 체크
                            nvcAuth = bizCat.GetMyAuthCategory(BoaComID, CurrentUserInfo.UserID,
                                CurrentUserInfo.DeptID, dtr["CategoryID"].ToString().ToNumber<int>(), dtr["OptionUseDept"].ToString(),
                                    false, BE.BoardAuth.Read, true, false);

                            if (nvcAuth.Count == 0) name = GetResource("NoAuth");
                        }

                        sb.Append("<li id='");
                        sb.Append(dtr["KeyID"].ToString());
                        sb.Append("'>");
                        sb.Append(name);
                        sb.Append("</li>");
                    }
                }
            }

            FolderList = sb.ToString();
        }

        #endregion (함수)
    }
}