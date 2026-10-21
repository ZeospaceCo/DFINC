using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections.Specialized;
using helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Common.Admin
{
    public partial class BoardTreeScript : Base.BaseControlBoard
    {
        #region Declaration (컨트롤 선언)
        private int Param_menuCatID;//메뉴에서 호출한 최상단 트리 나올 카테고리
        private int Param_catID;//포커스 설정할 트리 카테고리        
        private DAL.CategoryUser catUser = null;
        private int CompanyID = 0;
        //private string authority;        

        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 페이지 로드
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Common.Admin.BoardTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            Param_menuCatID = GetDecryptParam("menuCatID", "0").ToNumber<int>();
            Param_catID = GetDecryptParam("catID", "0").ToNumber<int>();

            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) LoadData();
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 첫 페이지 초기값 설정
        /// </summary>
        private void LoadData()
        {
            #region 트리 환경 설정
            tvBoardTreeScript.Nodes.Clear();
            tvBoardTreeScript.CollapseNodeOnSelect = false;
            tvBoardTreeScript.ExpandNodeOnSelect = false;
            tvBoardTreeScript.MultipleSelectEnabled = false;
            tvBoardTreeScript.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvBoardTreeScript.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            tvBoardTreeScript.DragAndDropEnabled = false;

            tvBoardTreeScript.ClientSideOnNodeSelect = SetNodeSelect;
            tvBoardTreeScript.ClientSideOnCallbackComplete = SetCallbackComplete;

            #endregion

            if (!UseTotBoard) CompanyID = CurrentUserInfo.CompanyID; //통합게시판 사용시 0 임.

            catUser = new DAL.CategoryUser();
            CreateFolder();           
            

            #region 트리 카테고리에 포커스 설정하기
            ComponentArt.Web.UI.TreeViewNode tvn = tvBoardTreeScript.FindNodeById(Param_catID.ToString());

            if (tvn != null) tvBoardTreeScript.SelectedNode = tvn;
            #endregion
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 첫 페이지 초기값 설정
        /// </summary>
        private void CreateFolder()
        {
            DAL.Category dal = new Zeospace.DeskPlus.Board.DAL.Category();

            DataSet ds = new DataSet();
            ds = dal.GetCategory(CompanyID, Param_menuCatID);
            
            bool expended = true;
            
            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                ComponentArt.Web.UI.TreeViewNode node = GetNode(dbRow, expended);
                tvBoardTreeScript.Nodes.Add(node);

                CreateNodeTree(dbRow["CategoryID"].ToString().ToNumber<int>(), node);
            }
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시판 카테고리 트리 노드 생성하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <param name="_parentCategoryID">The _parent category ID.</param>
        /// <param name="_parentNode">The _parent node.</param>
        private void CreateNodeTree(int _parentCategoryID, ComponentArt.Web.UI.TreeViewNode _parentNode)
        {
            BLL.Authority bizAuth = new BLL.Authority();
            DAL.Category bll = new Zeospace.DeskPlus.Board.DAL.Category();
            DataSet ds = new DataSet();

            ds = bll.GetCategoryByParentCategoryID(CompanyID, _parentCategoryID.ToString());
            
            int categoryID = 0;
            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                categoryID = dbRow["CategoryID"].ToString().ToNumber<int>();                
                
                ComponentArt.Web.UI.TreeViewNode node = GetNode(dbRow, false);
                _parentNode.Nodes.Add(node);

                //CreateNodeTree(categoryID, node);
            }
        }        

        #region 트리 노드 생성

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 게시판 카테고리 트리 최상위 노드 설정값 반환하기
        /// </summary>
        /// <param name="_text">_text.</param>
        /// <param name="_id">_id.</param>
        /// <param name="_value">_value.</param>
        /// <param name="_expanded">_expanded.</param>
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
        /// 작성자 : 심재운
        /// 목적 : 게시판 카테고리 트리 노드 설정값 반환하기
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_expanded">_expanded.</param>
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
                paraDic.Add("useTotBoard", (UseTotBoard ? "1" : "0"));
                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                node.ContentCallbackUrl = "/DeskPlusEIP/Board/Common/Admin/GetBoardTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
            }
                        
            node.Attributes["OptionUseDept"] = _dr["OptionUseDept"].ToString(); //부서별게시판여부            

            return node;
        }

        #endregion

        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// BoardTreeScriptView 프로퍼티
        /// </summary>
        public ComponentArt.Web.UI.TreeView BoardTreeScriptView
        {
            get
            {
                return tvBoardTreeScript;
            }
        }

        /// <summary>
        /// 통합게시판 사용여부
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use my template]; otherwise, <c>false</c>.
        /// </value>
        public bool UseTotBoard
        {
            get { return Convert.ToBoolean(GetViewStateString("UseTotBoard", "false")); }
            set { ViewState["UseTotBoard"] = value; }
        }

        /// <summary>
        /// 트리 노드 선택시 호출할 스크립트 함수
        /// </summary>
        public string SetNodeSelect
        {
            get { return GetViewStateString("SetNodeSelect", ""); }
            set { ViewState["SetNodeSelect"] = value; }
        }

        /// <summary>
        /// callback 호출 완료 후 호출할 스크립트 함수
        /// </summary>
        public string SetCallbackComplete
        {
            get { return GetViewStateString("SetCallbackComplete", ""); }
            set { ViewState["SetCallbackComplete"] = value; }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}