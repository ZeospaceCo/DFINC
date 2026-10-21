using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections;


namespace Zeospace.DeskPlus.MDM.Web.Send
{
    public partial class GetTargetURL : Base.BasePageMDM
    {
        #region 변수처리
        public int BoaComID = 1;
        public int CatID = 0;
        private string MyAuthCatID = "";
        public string TotalCount = String.Empty;  //총카운트
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.MDM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.MDM.Web.Send.GetTargetURL).Assembly;	//어셈블리 선언
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
        /// 작성자 : famo23
        /// 작성일자 : 2020-06-11
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            #region 트리 환경 설정
            tvBoardTreeScript.Nodes.Clear();
            tvBoardTreeScript.CollapseNodeOnSelect = false;
            tvBoardTreeScript.ExpandNodeOnSelect = false;
            tvBoardTreeScript.MultipleSelectEnabled = false;
            tvBoardTreeScript.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvBoardTreeScript.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            tvBoardTreeScript.DragAndDropEnabled = false;

            #endregion

            Zeospace.DeskPlus.Board.DAL.CategoryUser catUser = new Zeospace.DeskPlus.Board.DAL.CategoryUser();
            CreateFolder();

            #region 트리 카테고리에 포커스 설정하기
            //페이지 시작시 노드 선택
            if (this.tvBoardTreeScript.Nodes.Count > 0)
            {
                this.tvBoardTreeScript.SelectedNode = this.tvBoardTreeScript.Nodes[0];
                CatID = this.tvBoardTreeScript.SelectedNode.ID.ToNumber<int>();
            }

            #endregion

            //통합게시판 회사아이디
            if (CatID > 0)
            {
                BoaComID = Zeospace.DeskPlus.Board.BLL.Category.GetCompanyCategoryInfo(CatID);

                GetCategory();
                GetListCount();
            }
        }

        /// <summary>
        /// 첫 페이지 초기값 설정
        /// </summary>
        private void CreateFolder()
        {
            Zeospace.DeskPlus.Board.DAL.Category dal = new Zeospace.DeskPlus.Board.DAL.Category();

            DataSet ds = new DataSet();
            ds = dal.GetCategory(CurrentUserInfo.CompanyID, 0);

            bool expended = true;

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                ComponentArt.Web.UI.TreeViewNode node = GetNode(dbRow, expended);
                tvBoardTreeScript.Nodes.Add(node);

                CreateNodeTree(dbRow["CategoryID"].ToString().ToNumber<int>(), node);
            }
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-06-11
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Confirm");       //"확인";
                CbtnClose.Text = resMng.GetString("Close");           //"닫기";
                lblSelected.Text = resMng.GetString("Target");         
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-06-11
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
        }

        #region 트리 노드 생성

        /// <summary>
        /// 게시판 카테고리 트리 노드 생성하기
        /// </summary>
        /// <param name="_parentCategoryID"></param>
        /// <param name="_parentNode"></param>
        private void CreateNodeTree(int _parentCategoryID, ComponentArt.Web.UI.TreeViewNode _parentNode)
        {
            Zeospace.DeskPlus.Board.BLL.Authority bizAuth = new Zeospace.DeskPlus.Board.BLL.Authority();
            Zeospace.DeskPlus.Board.DAL.Category bll = new Zeospace.DeskPlus.Board.DAL.Category();
            DataSet ds = new DataSet();

            ds = bll.GetCategoryByParentCategoryID(CurrentUserInfo.CompanyID, _parentCategoryID.ToString());

            int categoryID = 0;
            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                categoryID = dbRow["CategoryID"].ToString().ToNumber<int>();

                ComponentArt.Web.UI.TreeViewNode node = GetNode(dbRow, false);
                _parentNode.Nodes.Add(node);
            }
        }

        /// <summary>
        /// 게시판 카테고리 트리 최상위 노드 설정값 반환하기
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_id"></param>
        /// <param name="_value"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode GetRootNode(string _text, string _id, string _value, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text.ToReverseXSS();					//목록명
            node.ID = _id;						//목록 고유값
            node.Value = _value;				//하위메뉴값						
            node.Expanded = _expanded;			//확장여부
            node.Style.Add("font-style", "bold");
            node.Attributes["CategoryID"] = _id; //카테고리아이디
            node.Attributes["FavoriteID"] = "0"; //즐겨찾기고유아이디
            node.Attributes["Types"] = "BC"; //게시판 카테고리에서 가져온 데이터 구분자값(즐겨찾기용과 구분하기 위한 방편)            

            if (_id == "-1") node.ImageUrl = "private.gif";

            if (_id == "-2")
            {
                node.ImageUrl = "favorites.gif";
                node.Attributes["Types"] = "FC"; //즐겨찾기 구분자                
            }

            return node;
        }

        /// <summary>
        /// 게시판 카테고리 트리 노드 설정값 반환하기
        /// </summary>
        /// <param name="_dr"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode GetNode(DataRow _dr, bool _expanded)
        {
            string pCategoryID = _dr["ParentCategoryID"].ToString();
            if (pCategoryID == "") pCategoryID = "0";


            #region 다국어 이름 설정
            string name = this.GetLangName(
                _dr["NameBase"].ToString(),
                _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            string nodeText = name;

            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = (!String.IsNullOrEmpty(nodeText)) ? nodeText : GetResource("NoTitle");

            node.ID = _dr["CategoryID"].ToString();			//목록 고유값            
            node.Expanded = _expanded;

            node.ImageUrl = (_dr["OptionUseView"].ToString().Equals("1")) ? "folders.gif" : "folder.gif";
            if (_dr["OptionUseDept"].ToString().Equals("1")) node.ImageUrl = "inbox.gif"; //부서별 문서함

            node.Style.Add("font-style", "bold");

            //1. 카테고리값,  2. 타이틀           3. 하위메뉴값,             4. order            5. 첨부이미지본문표시유무 
            //6. 답변사용유무 7. 댓글사용유무 8. 조회자통계정보사용유무, 9. 부서게시판여부, 10. 권한여부(true/false,1:읽기/2:쓰기), 11, custom 페이지경로, 12 : 투표, 13:보기용

            if (Convert.ToInt32(_dr["childCnt"]) > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("pID", _dr["CategoryID"].ToString());
                paraDic.Add("useTotBoard", "0");
                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                node.ContentCallbackUrl = "/DeskPlusEIP/Board/Common/Admin/GetBoardTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
            }

            node.Attributes["OptionUseDept"] = _dr["OptionUseDept"].ToString(); //부서별게시판여부            

            return node;
        }

        #endregion

        /// <summary>
        /// 카테고리 옵션 적용
        /// </summary>
        private void GetCategory()
        {
            Zeospace.DeskPlus.Board.DAL.Category dal = new Zeospace.DeskPlus.Board.DAL.Category();
            DataSet ds = dal.GetCategory(BoaComID, CatID);

            string OptionUseView = "";
            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OptionUseView = dr["OptionUseView"].ToString();
                }
            }

            #region 하위 카테고리
            MyAuthCatID = CatID.ToString();

            //보기용 및 하위분류 검색시, 하위카테고리까지 가져옴
            Zeospace.DeskPlus.Board.BLL.Category biz = new Zeospace.DeskPlus.Board.BLL.Category();
            if (OptionUseView == "1") MyAuthCatID = biz.GetChildCategory(BoaComID, CatID);

            #endregion 하위 카테고리
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 :  2020-06-12
        /// 작성목적 : 자신의 개인폴더의 게시물 갯수
        /// </summary>
        private void GetListCount()
        {
            Zeospace.DeskPlus.Board.DAL.Lists list = new Zeospace.DeskPlus.Board.DAL.Lists();
            TotalCount = list.GetPageListCount(BoaComID, MyAuthCatID, "", "", "", "", "0", CurrentUserInfo.DeptID).ToString();
        }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}