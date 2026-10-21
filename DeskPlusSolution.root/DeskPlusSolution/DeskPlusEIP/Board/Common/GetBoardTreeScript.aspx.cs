using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Common
{
    public partial class GetBoardTreeScript : Base.BasePageBoard
    {
        #region Declaration (컨트롤 선언)

        private int p_ParentCategoryID;
        private bool p_UseWriteAuth = false;//쓰기권한자만
        private bool p_UseReadAuth = false;//읽기권한자만
        private bool p_UseAdmin = false;//관리자만
        private bool p_ShowTitleNoAuth = false;//제목 (미권한시 보이기 여부)
        private int p_UseTotBoard;
        private int CompanyID = 0;

        #endregion Declaration (컨트롤 선언)



        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드

            ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Common.GetBoardTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스 로드

            p_ParentCategoryID = GetDecryptParam("pID", "0").ToNumber<int>();
            p_UseReadAuth = (GetDecryptParam("useReadAuth", "false").ToUpper() == "FALSE") ? false : true;
            p_UseWriteAuth = (GetDecryptParam("useWriteAuth", "false").ToUpper() == "FALSE") ? false : true;
            p_UseAdmin = (GetDecryptParam("useAdmin", "false").ToUpper() == "FALSE") ? false : true;
            p_ShowTitleNoAuth = (GetDecryptParam("showTitleNoAuth", "false").ToUpper() == "FALSE") ? false : true;
            p_UseTotBoard = GetDecryptParam("useTotBoard", "0").ToNumber<int>();

            if (p_UseTotBoard == 0) CompanyID = CurrentUserInfo.CompanyID; //통합게시판 사용시 0 임.

            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            if (CompanyID > -1 && p_ParentCategoryID > -1)
            {
                //부서정보 전역변수 담기
                Account.DAL.Dept dalDept = new Account.DAL.Dept();
                DataSet deptList = dalDept.GetDept(CompanyID, "deptList", true);
                DataView dvDeptList = deptList.Tables[0].DefaultView;

                BLL.Authority bizAuth = new BLL.Authority();
                DAL.Category bll = new Zeospace.DeskPlus.Board.DAL.Category();
                DataSet ds = new DataSet();

                ds = bll.GetCategoryByParentCategoryID(CompanyID, p_ParentCategoryID.ToString());

                string getAuthority = "";
                bool auth = false;
                BLL.Category bizCat = new BLL.Category();

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    auth = false;
                    getAuthority = "";

                    // 작성목적   : 게시판 권한테이블에서 회사,부서,개인 아이디값 가져오기
                    // 권한여부 1 : true, 2: false / 1 : 읽기권한, 2 : 쓰기권한, 3 : 읽기+꼬리말, 4 : 읽기+쓰기+꼬리말쓰기
                    NameValueCollection nvcAuth = new NameValueCollection();

                    if (p_UseWriteAuth)
                    {
                        //쓰기 권한 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                            CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Write, true, p_UseAdmin);
                        if (nvcAuth.Count > 0) getAuthority = "true/4";//4 : 읽기+쓰기+꼬리말쓰기
                    }
                    else if (p_UseReadAuth)
                    {
                        //읽기 권한 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                            CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Read, true, p_UseAdmin);
                        if (nvcAuth.Count > 0) getAuthority = "true/3";//3 : 읽기+꼬리말
                    }

                    if (nvcAuth.Count > 0 || p_UseAdmin) //권한이 없는 카테고리는 안보이게 처리
                    {
                        if (nvcAuth.Count > 0 || p_UseAdmin) auth = true;

                        ComponentArt.Web.UI.TreeViewNode node = GetNode(dbRow, getAuthority, false, auth);
                        TreeView1.Nodes.Add(node);
                    }
                }

                Response.Write(TreeView1.GetXml());
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

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
                node.Text = (p_ShowTitleNoAuth) ? nodeText : GetResource("NoAuth");
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

                    if (p_UseWriteAuth)
                    {
                        //쓰기 권한 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                            CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Write, true, p_UseAdmin);
                    }
                    else if (p_UseReadAuth)
                    {
                        //읽기 권한 존재하는지 체크
                        nvcAuth = bizCat.GetMyAuthCategory(CompanyID, CurrentUserInfo.UserID,
                            CurrentUserInfo.DeptID, dbRow["CategoryID"].ToString().ToNumber<int>(), dbRow["OptionUseDept"].ToString(), false, BE.BoardAuth.Read, true, p_UseAdmin);
                    }

                    if (nvcAuth.Count > 0) AuthCnt++; //권한이 없는 카테고리는 안보이게 처리
                }

                if (AuthCnt > 0 || p_UseAdmin)
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("pID", _dr["CategoryID"].ToString());
                    paraDic.Add("useWriteAuth", (p_UseWriteAuth) ? "true" : "false");
                    paraDic.Add("useReadAuth", (p_UseReadAuth) ? "true" : "false");
                    paraDic.Add("useAdmin", (p_UseAdmin) ? "true" : "false");
                    paraDic.Add("showTitleNoAuth", (p_ShowTitleNoAuth) ? "true" : "false");
                    paraDic.Add("useTotBoard", p_UseTotBoard.ToString());

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
            node.Attributes["ChildCnt"] = _dr["childCnt"].ToString(); //하위폴더개수
            node.Attributes["Types"] = "BC"; //즐겨찾기 폴더 FC : 카테고리 BC

            return node;
        }

        #endregion Method (함수)
    }
}