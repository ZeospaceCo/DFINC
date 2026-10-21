using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Common
{
    public partial class BoardTreeScript : Base.BaseControlBoard
    {
        #region Declaration (컨트롤 선언)

        private int Param_menuCatID;//메뉴에서 호출한 최상단 트리 나올 카테고리
        private int Param_catID;//포커스 설정할 트리 카테고리
        private string ParentCatList = string.Empty; //포커스 설정할 카테고리의 상위카테고리값들.
        private DAL.CategoryUser catUser = null;
        private DataView dvDeptList = null;
        private int CompanyID = 0;

        #endregion Declaration (컨트롤 선언)

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
            AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Common.BoardTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스 로드

            Param_menuCatID = GetDecryptParam("menuCatID", "0").ToNumber<int>();
            Param_catID = GetDecryptParam("catID", "0").ToNumber<int>();

            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) LoadData();
        }

        #endregion Page Process (페이지 처리)

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

            #endregion 트리 환경 설정

            if (!UseTotBoard) CompanyID = CurrentUserInfo.CompanyID; //통합게시판 사용시 0 임.

            //부서정보 전역변수 담기
            Account.DAL.Dept dalDept = new Account.DAL.Dept();
            DataSet deptList = dalDept.GetDept(CompanyID, "deptList", true);
            dvDeptList = deptList.Tables[0].DefaultView;

            #region 카테고리 및 폴더 사용여부

            if (UseFolder)
            {
                catUser = new DAL.CategoryUser();
                CreateFolder();
            }

            #endregion 카테고리 및 폴더 사용여부

            #region 임시저장카테고리 사용여부

            if (UseMyTemplate)
            {
                ComponentArt.Web.UI.TreeViewNode topTempNode = GetRootNode(GetResource("SaveTemp"), "-1",
                    CompanyID + "|" + CurrentUserInfo.UserID, true);
                tvBoardTreeScript.Nodes.Add(topTempNode);
            }

            #endregion 임시저장카테고리 사용여부

            #region 즐겨찾기 사용여부

            if (UseMyFavorite)
            {
                ComponentArt.Web.UI.TreeViewNode topFavorNode = GetRootNode(GetResource("FavoriteFolder"), "-2",
                    CompanyID + "|" + CurrentUserInfo.UserID, true);
                tvBoardTreeScript.Nodes.Add(topFavorNode);

                DAL.Favorite dalFavor = new Zeospace.DeskPlus.Board.DAL.Favorite();
                DataSet dsFavor = dalFavor.GetFavoriteTree(CompanyID, CurrentUserInfo.UserID);

                if (dsFavor != null)
                {
                    BLL.Authority bizAuth = new BLL.Authority();
                    BLL.Category bizCat = new BLL.Category();

                    string getAuthority = "";
                    bool auth = false;
                    string types = string.Empty;

                    //관계 설정
                    dsFavor.Relations.Add("NodeRelation", dsFavor.Tables[0].Columns["OwnerID"],
                        dsFavor.Tables[0].Columns["ParentOwnerID"], false);

                    foreach (DataRow dbRow in dsFavor.Tables[0].Rows)
                    {
                        if (dbRow.IsNull("ParentOwnerID"))
                        {
                            NameValueCollection nvcAuth = new NameValueCollection();

                            types = dbRow["Types"].ToString();
                            getAuthority = "false/";
                            auth = false;

                            if (string.Compare(types, "BC", true) == 0)
                            {
                                int boaComID = BLL.Category.GetCompanyCategoryInfo(Convert.ToInt32(dbRow["CategoryID"]));

                                if (UseWriteAuthority)//쓰기권한여부
                                {
                                    //쓰기 권한 존재하는지 체크
                                    nvcAuth = bizCat.GetMyAuthCategory(boaComID, CurrentUserInfo.UserID,
                                        CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(),
                                            false, BE.BoardAuth.Write, true, UseAdmin);
                                    if (nvcAuth.Count > 0) getAuthority = "true/4";//4 : 읽기+쓰기+꼬리말쓰기
                                }
                                else if (UseReadAuthority)//읽기권한 여부
                                {
                                    //읽기 권한 존재하는지 체크
                                    nvcAuth = bizCat.GetMyAuthCategory(boaComID, CurrentUserInfo.UserID,
                                        CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(),
                                            false, BE.BoardAuth.Read, true, UseAdmin);
                                    if (nvcAuth.Count > 0) getAuthority = "true/3";//3 : 읽기+꼬리말
                                }
                            }

                            //권한자 및 슈퍼관리자, 게시판 관리자는 권한을 보여준다.
                            if (nvcAuth.Count > 0 || UseAdmin) auth = true;

                            ComponentArt.Web.UI.TreeViewNode newNode = GetFavorNode(dbRow, getAuthority, false, auth);
                            topFavorNode.Nodes.Add(newNode);
                            CreateFavorNodeTree(dbRow, newNode);
                        }
                    }
                }
            }

            #endregion 즐겨찾기 사용여부

            #region 관심대상자 사용여부

            if (UseInterested)
            {
                ComponentArt.Web.UI.TreeViewNode topFavorNode = GetRootNode(GetResource("InterestedPerson"), "-3",
                    CurrentUserInfo.CompanyID + "|" + CurrentUserInfo.UserID, true);
                tvBoardTreeScript.Nodes.Add(topFavorNode);

                DAL.InterestedUser dalIUser = new Zeospace.DeskPlus.Board.DAL.InterestedUser();
                DataSet dsIUser = dalIUser.GetInterestedUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                if (dsIUser != null)
                {
                    foreach (DataRow dbRow in dsIUser.Tables[0].Rows)
                    {
                        ComponentArt.Web.UI.TreeViewNode newNode = GetInterestedUserNode(dbRow);
                        topFavorNode.Nodes.Add(newNode);
                        CreateFavorNodeTree(dbRow, newNode);
                    }
                }
            }

            #endregion 관심대상자 사용여부

            #region 트리 카테고리에 포커스 설정하기

            ComponentArt.Web.UI.TreeViewNode tvn = tvBoardTreeScript.FindNodeById(Param_catID.ToString());

            if (tvn != null) tvBoardTreeScript.SelectedNode = tvn;

            #endregion 트리 카테고리에 포커스 설정하기
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 첫 페이지 초기값 설정
        /// </summary>
        private void CreateFolder()
        {
            BLL.Category bll = new BLL.Category();
            ParentCatList = bll.GetParentCatList(CompanyID, Param_catID, true);
            if (ParentCatList.Length > 0) ParentCatList = "," + ParentCatList + ",";

            DAL.Category dal = new Zeospace.DeskPlus.Board.DAL.Category();

            DataSet ds = new DataSet();
            ds = dal.GetCategory(CompanyID, Param_menuCatID);

            BLL.Authority bizAuth = new BLL.Authority();
            bool expended = true;
            string getAuthority = "";
            bool auth = false;
            BLL.Category bizCat = new BLL.Category();

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                auth = false;
                getAuthority = "false/";

                // 작성목적   : 게시판 권한테이블에서 회사,부서,개인 아이디값 가져오기
                // 권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
                NameValueCollection nvcAuth = new NameValueCollection();

                if (UseWriteAuthority)
                {
                    //쓰기 권한 존재하는지 체크
                    nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(),
                        false, BE.BoardAuth.Write, true, UseAdmin);
                    if (nvcAuth.Count > 0) getAuthority = "true/4";//4 : 읽기+쓰기+꼬리말쓰기
                }
                else if (UseReadAuthority)
                {
                    //읽기 권한 존재하는지 체크
                    nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(),
                        false, BE.BoardAuth.Read, true, UseAdmin);
                    if (nvcAuth.Count > 0) getAuthority = "true/3";//3 : 읽기+꼬리말
                }

                if (nvcAuth.Count > 0 || UseAdmin) //권한이 없는 카테고리는 안보이게 처리
                {
                    //권한자 및 슈퍼관리자, 게시판 관리자는 권한을 보여준다.
                    if (nvcAuth.Count > 0 || UseAdmin) auth = true;

                    ComponentArt.Web.UI.TreeViewNode node = GetNode(dbRow, getAuthority, expended, auth);
                    tvBoardTreeScript.Nodes.Add(node);

                    CreateNodeTree(dbRow["CategoryID"].ToString().ToNumber<int>(), node);
                }
            }
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

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

            //bool expand = false;
            string getAuthority = ""; //상세 권한값 내용
            bool auth = false; //접근 가능 여부
            int categoryID = 0;
            BLL.Category bizCat = new BLL.Category();

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                auth = false;
                getAuthority = "";

                categoryID = dbRow["CategoryID"].ToString().ToNumber<int>();

                // 작성목적   : 게시판 권한테이블에서 회사,부서,개인 아이디값 가져오기
                // 권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
                NameValueCollection nvcAuth = new NameValueCollection();

                if (UseWriteAuthority)
                {
                    //쓰기 권한 존재하는지 체크
                    nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Write, true, UseAdmin);
                    if (nvcAuth.Count > 0) getAuthority = "true/4";//4 : 읽기+쓰기+꼬리말쓰기
                }
                else if (UseReadAuthority)
                {
                    //읽기 권한 존재하는지 체크
                    nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Read, true, UseAdmin);
                    if (nvcAuth.Count > 0) getAuthority = "true/3";//3 : 읽기+꼬리말
                }

                if (nvcAuth.Count > 0 || UseAdmin) //권한이 없는 카테고리는 안보이게 처리
                {
                    if (nvcAuth.Count > 0 || UseAdmin) auth = true;

                    //포커스 선택한 카테고리는 알아서 펼쳐지게 해줘야 한다.
                    bool expand = (ParentCatList.IndexOf("," + categoryID + ",") != -1) ? true : false;

                    ComponentArt.Web.UI.TreeViewNode node = GetNode(dbRow, getAuthority, expand, auth);
                    _parentNode.Nodes.Add(node);

                    if (expand == true)
                    {
                        CreateNodeTree(categoryID, node);
                    }
                }
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 즐겨찾기 하위 카테고리 트리 노드 생성하기
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <param name="_dbRow">The _DB row.</param>
        /// <param name="_parentNode">The _parent node.</param>
        private void CreateFavorNodeTree(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _parentNode)
        {
            BLL.Authority bizAuth = new BLL.Authority();

            bool expand = false;
            string getAuthority = ""; //상세 권한값 내용
            bool auth = false; //접근 가능 여부
            string types = string.Empty;
            BLL.Category bizCat = new BLL.Category();

            foreach (DataRow childRow in _dbRow.GetChildRows("NodeRelation"))
            {
                types = childRow["Types"].ToString();
                getAuthority = "";
                auth = false;

                if (string.Compare(types, "BC", true) == 0)
                {
                    // 작성목적   : 게시판 권한테이블에서 회사,부서,개인 아이디값 가져오기
                    // 권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
                    NameValueCollection nvcAuth = new NameValueCollection();

                    if (UseWriteAuthority)
                    {
                        //쓰기 권한 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                            CurrentUserInfo.DeptID, childRow["CategoryID"].ToString().ToNumber<int>(), childRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Write, true, UseAdmin);
                        if (nvcAuth.Count > 0) getAuthority = "true/4";//4 : 읽기+쓰기+꼬리말쓰기
                    }
                    else if (UseReadAuthority)
                    {
                        //읽기 권한 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                            CurrentUserInfo.DeptID, childRow["CategoryID"].ToString().ToNumber<int>(), childRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Read, true, UseAdmin);
                        if (nvcAuth.Count > 0) getAuthority = "true/3";//3 : 읽기+꼬리말
                    }

                    if (nvcAuth.Count > 0 || UseAdmin) auth = true;
                }

                ComponentArt.Web.UI.TreeViewNode childNode = GetFavorNode(childRow, getAuthority, expand, auth);
                _parentNode.Nodes.Add(childNode);

                CreateFavorNodeTree(childRow, childNode);
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
            node.Attributes["InterestedUserID"] = "0";
            node.Attributes["Types"] = "BC"; //게시판 카테고리에서 가져온 데이터 구분자값(즐겨찾기용과 구분하기 위한 방편)

            if (_id == "-1") node.ImageUrl = "private.gif";

            if (_id == "-2")
            {
                node.ImageUrl = "favorites.gif";
                node.Attributes["Types"] = "FC"; //즐겨찾기 구분자
            }

            if (_id == "-3")
            {
                node.ImageUrl = "favorites.gif";
            }

            return node;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 게시판 카테고리 트리 노드 설정값 반환하기
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_isAuthority">The _is authority.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <param name="_auth">if set to <c>true</c> [_auth].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode GetNode(DataRow _dr, string _isAuthority, bool _expanded, bool _auth)
        {
            string pCategoryID = _dr["ParentCategoryID"].ToString();
            if (pCategoryID == "") pCategoryID = "0";

            #region 다국어 이름 설정

            string name = this.GetLangName(
                _dr["NameBase"].ToString(),
                _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

            #endregion 다국어 이름 설정

            string nodeText = name;

            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            if (_auth == false)
            {
                node.Text = (ShowTitleNoAuth) ? nodeText : GetResource("NoAuth");
                node.CssClass = "GrayedTreeNode";
                node.HoverCssClass = "GrayedTreeNode";
                node.SelectedCssClass = "GrayedTreeNode";
                node.Selectable = false;
            }
            else
            {
                node.Text = (!String.IsNullOrEmpty(nodeText)) ? nodeText : GetResource("NoTitle");
            }

            node.ID = _dr["CategoryID"].ToString();			//목록 고유값
            node.Expanded = _expanded;

            node.ImageUrl = (_dr["OptionUseView"].ToString().Equals("1")) ? "folders.gif" : "folder.gif";
            if (_dr["OptionUseDept"].ToString().Equals("1")) node.ImageUrl = "inbox.gif"; //부서별 문서함

            node.Style.Add("font-style", "bold");

            //1. 카테고리값,  2. 타이틀           3. 하위메뉴값,             4. order            5. 첨부이미지본문표시유무
            //6. 답변사용유무 7. 댓글사용유무 8. 조회자통계정보사용유무, 9. 부서게시판여부, 10. 권한여부(true/false,1:읽기/2:쓰기), 11, custom 페이지경로, 12 : 투표, 13:보기용

            if (Convert.ToInt32(_dr["childCnt"]) > 0)
            {
                DAL.Category bll = new Zeospace.DeskPlus.Board.DAL.Category();
                DataSet ds = new DataSet();

                ds = bll.GetCategoryByParentCategoryID(CompanyID, _dr["CategoryID"].ToString());

                int AuthCnt = 0;
                BLL.Category bizCat = new BLL.Category();

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    NameValueCollection nvcAuth = new NameValueCollection();

                    if (UseWriteAuthority)
                    {
                        //쓰기 권한 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                            CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Write, true, UseAdmin);
                    }
                    else if (UseReadAuthority)
                    {
                        //읽기 권한 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                            CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Read, true, UseAdmin);
                    }

                    if (nvcAuth.Count > 0) AuthCnt++; //권한이 없는 카테고리는 안보이게 처리
                }

                if (AuthCnt > 0 || UseAdmin)
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("pID", _dr["CategoryID"].ToString());
                    paraDic.Add("useWriteAuth", (UseWriteAuthority) ? "true" : "false");
                    paraDic.Add("useReadAuth", (UseReadAuthority) ? "true" : "false");
                    paraDic.Add("useAdmin", (UseAdmin) ? "true" : "false");
                    paraDic.Add("showTitleNoAuth", (ShowTitleNoAuth) ? "true" : "false");
                    paraDic.Add("useTotBoard", (UseTotBoard ? "1" : "0"));

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                    node.ContentCallbackUrl = "/DeskPlusEIP/Board/Common/GetBoardTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
                }
            }

            string optionListType = (_dr["OptionListType"] == null) ? "0" : _dr["OptionListType"].ToString();

            node.Attributes["CategoryID"] = _dr["CategoryID"].ToString(); //카테고리아이디
            node.Attributes["OptionUseDept"] = _dr["OptionUseDept"].ToString(); //부서별게시판여부
            node.Attributes["OptionListType"] = optionListType; //게시판형태
            node.Attributes["OptionUseView"] = _dr["OptionUseView"].ToString(); //조회카테고리 여부
            node.Attributes["MyAuthority"] = _isAuthority;//권한여부
            node.Attributes["FavoriteID"] = "0"; //즐겨찾기고유아이디
            node.Attributes["InterestedUserID"] = "0";
            node.Attributes["ChildCnt"] = _dr["childCnt"].ToString(); //하위폴더개수
            node.Attributes["Types"] = "BC"; //즐겨찾기 폴더 FC : 카테고리 BC

            return node;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 즐겨찾기 카테고리 트리 노드 설정값 반환하기
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_isAuthority">The _is authority.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <param name="_auth">if set to <c>true</c> [_auth].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode GetFavorNode(DataRow _dr, string _isAuthority, bool _expanded, bool _auth)
        {
            #region 다국어 이름 설정

            string name = this.GetLangName(
                _dr["NameBase"].ToString(),
                _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

            #endregion 다국어 이름 설정

            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = (!String.IsNullOrEmpty(name)) ? name : GetResource("NoTitle");

            if (_dr["Types"].ToString().Equals("BC"))
            {
                if (_auth == false)
                {
                    node.Text = GetResource("NoAuth");
                    node.CssClass = "GrayedTreeNode";
                    node.HoverCssClass = "GrayedTreeNode";
                    node.SelectedCssClass = "GrayedTreeNode";
                    //node.Selectable = false;
                }
            }

            node.ID = _dr["OwnerID"].ToString();			//목록 고유값
            node.Expanded = _expanded;

            #region 트리 아이콘

            if (_dr["Types"].ToString().Equals("BC"))
            {
                if (_dr["OptionUseView"].ToString().Equals("0"))
                {
                    node.ImageUrl = "folder.gif";
                }
                else if (_dr["OptionUseView"].ToString().Equals("1"))
                {
                    node.ImageUrl = "folders.gif";
                }
            }
            else
            {
                node.ImageUrl = "favorites.gif";
            }

            #endregion 트리 아이콘

            node.Style.Add("font-style", "bold");

            //1. 카테고리값,  2. 타이틀           3. 하위메뉴값,             4. order            5. 첨부이미지본문표시유무
            //6. 답변사용유무 7. 댓글사용유무 8. 조회자통계정보사용유무, 9. 부서게시판여부, 10. 권한여부(true/false,1:읽기/2:쓰기), 11, custom 페이지경로, 12 : 투표, 13:보기용
            string categoryID = (_dr["CategoryID"].ToString().ToNull().Length > 0) ? _dr["CategoryID"].ToString() : "0";
            string keyID = (_dr["KeyID"].ToString().ToNull().Length > 0) ? _dr["KeyID"].ToString() : "0";
            string optionListType = (_dr["OptionListType"].ToString().ToNull().Length == 0) ? "0" : _dr["OptionListType"].ToString();

            node.Attributes["CategoryID"] = categoryID; //카테고리아이디
            node.Attributes["OptionUseDept"] = _dr["OptionUseDept"].ToString(); ; //부서별게시판여부
            node.Attributes["OptionListType"] = optionListType; //리스트형식
            node.Attributes["OptionUseView"] = _dr["OptionUseView"].ToString(); //조회카테고리 여부
            node.Attributes["MyAuthority"] = _isAuthority;//권한여부
            node.Attributes["FavoriteID"] = keyID; //즐겨찾기고유아이디
            node.Attributes["InterestedUserID"] = "0";
            node.Attributes["ChildCnt"] = _dr["childCnt"].ToString(); //하위폴더개수
            node.Attributes["Types"] = _dr["Types"].ToString();//즐겨찾기 폴더 FC : 카테고리 BC

            return node;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 관심대상자 츠리
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_isAuthority">The _is authority.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <param name="_auth">if set to <c>true</c> [_auth].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode GetInterestedUserNode(DataRow _dr)
        {
            #region 다국어 이름 설정

            string dept = this.GetLangName(
                _dr["DeptBase"].ToString(),
                _dr["DeptKo"].ToString(), _dr["DeptEn"].ToString(), _dr["DeptJa"].ToString(),
                _dr["DeptFr"].ToString(), _dr["DeptZhCn"].ToString(), _dr["DeptZhTw"].ToString(), PreLang);

            string title = this.GetLangName(
                _dr["TitleBase"].ToString(),
                _dr["TitleKo"].ToString(), _dr["TitleEn"].ToString(), _dr["TitleJa"].ToString(),
                _dr["TitleFr"].ToString(), _dr["TitleZhCn"].ToString(), _dr["TitleZhTw"].ToString(), PreLang);

            string name = this.GetLangName(
                _dr["NameBase"].ToString(),
                _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

            #endregion 다국어 이름 설정

            string fullName = dept + "/" + title + "/" + name;

            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = (!String.IsNullOrEmpty(fullName)) ? fullName : GetResource("NoTitle");

            node.ID = _dr["InterestedUserID"].ToString();			//목록 고유값
            node.Expanded = false;

            #region 트리 아이콘

            node.ImageUrl = "favorites.gif";

            #endregion 트리 아이콘

            node.Style.Add("font-style", "bold");

            node.Attributes["CategoryID"] = "-3"; //카테고리아이디            
            node.Attributes["FavoriteID"] = "0"; //즐겨찾기고유아이디
            node.Attributes["InterestedUserID"] = _dr["InterestedUserID"].ToString();

            return node;
        }

        #endregion 트리 노드 생성

        #endregion Method (함수)

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
        /// 트리 읽기 권한 카테고리 보여주기 여부(True, False)
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use read authority]; otherwise, <c>false</c>.
        /// </value>
        public bool UseReadAuthority
        {
            get { return Convert.ToBoolean(GetViewStateString("UseReadAuthority", "false")); }
            set { ViewState["UseReadAuthority"] = value; }
        }

        /// <summary>
        /// 트리 쓰기 권한 카테고리 보여주기 여부(True, False)
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use write authority]; otherwise, <c>false</c>.
        /// </value>
        public bool UseWriteAuthority
        {
            get { return Convert.ToBoolean(GetViewStateString("UseWriteAuthority", "false")); }
            set { ViewState["UseWriteAuthority"] = value; }
        }

        /// <summary>
        /// 관리자 여부
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use admin]; otherwise, <c>false</c>.
        /// </value>
        public bool UseAdmin
        {
            get { return Convert.ToBoolean(GetViewStateString("UseAdmin", "false")); }
            set { ViewState["UseAdmin"] = value; }
        }

        /// <summary>
        /// 게시판 폴더 보여주기 여부
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use my template]; otherwise, <c>false</c>.
        /// </value>
        public bool UseFolder
        {
            get { return Convert.ToBoolean(GetViewStateString("UseFolder", "true")); }
            set { ViewState["UseFolder"] = value; }
        }

        /// <summary>
        /// 나의 임시저장 폴더 보여주기 여부
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use my template]; otherwise, <c>false</c>.
        /// </value>
        public bool UseMyTemplate
        {
            get { return Convert.ToBoolean(GetViewStateString("UseMyTemplate", "false")); }
            set { ViewState["UseMyTemplate"] = value; }
        }

        /// <summary>
        /// 나의 즐겨찾기 폴더 관리
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use my template]; otherwise, <c>false</c>.
        /// </value>
        public bool UseMyFavorite
        {
            get { return Convert.ToBoolean(GetViewStateString("UseMyFavorite", "false")); }
            set { ViewState["UseMyFavorite"] = value; }
        }

        /// <summary>
        /// 관심 대상자 설정
        /// </summary>
        public bool UseInterested
        {
            get { return Convert.ToBoolean(GetViewStateString("UseInterested", "false")); }
            set { ViewState["UseInterested"] = value; }
        }

        /// <summary>
        /// 제목 보이기 여부 (미권한시)
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use my template]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowTitleNoAuth
        {
            get { return Convert.ToBoolean(GetViewStateString("ShowTitleNoAuth", "false")); }
            set { ViewState["ShowTitleNoAuth"] = value; }
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

        #endregion Property (프로퍼티)
    }
}