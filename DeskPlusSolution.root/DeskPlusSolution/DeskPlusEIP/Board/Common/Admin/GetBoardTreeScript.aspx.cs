using System;
using System.Collections.Generic;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Common.Admin
{
    public partial class GetBoardTreeScript : Base.BasePageBoard
    {
        #region Declaration (컨트롤 선언)

        private int p_ParentCategoryID;
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
            p_UseTotBoard = GetDecryptParam("useTotBoard", "0").ToNumber<int>();

            if (p_UseTotBoard == 0) CompanyID = CurrentUserInfo.CompanyID; //통합게시판 사용시 0 임.

            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            if (CompanyID > -1 && p_ParentCategoryID > -1)
            {
                DAL.Category bll = new Zeospace.DeskPlus.Board.DAL.Category();
                DataSet ds = new DataSet();

                ds = bll.GetCategoryByParentCategoryID(CompanyID, p_ParentCategoryID.ToString());

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow, false);
                    TreeView1.Nodes.Add(node);
                }

                Response.Write(TreeView1.GetXml());
            }
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        /// <summary>
        /// 트리노드 생성 : Creates the node.
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
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
            node.Text = (!String.IsNullOrEmpty(nodeText)) ? nodeText : "No Title";
            node.ID = _dr["CategoryID"].ToString();			//목록 고유값
            node.Expanded = _expanded;

            node.ImageUrl = (_dr["OptionUseView"].ToString().Equals("1")) ? "folders.gif" : "folder.gif";
            if (_dr["OptionUseDept"].ToString().Equals("1")) node.ImageUrl = "inbox.gif"; //부서별 문서함

            node.Style.Add("font-style", "bold");

            //1. 카테고리값,  2. 타이틀           3. 하위메뉴값,             4. order            5. 첨부이미지본문표시유무
            //6. 답변사용유무 7. 댓글사용유무 8. 조회자통계정보사용유무, 9. 부서게시판여부, 10. 권한여부(true/false,1:읽기/2:쓰기), 11, custom 페이지경로, 12 : 투표, 13:보기용

            if (_dr["childCnt"].ToString().ToNumber<int>() > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("pID", _dr["CategoryID"].ToString());
                paraDic.Add("useTotBoard", p_UseTotBoard.ToString());
                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                node.ContentCallbackUrl = "/DeskPlusEIP/Board/Common/Admin/GetBoardTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
            }

            node.Attributes["OptionUseDept"] = _dr["OptionUseDept"].ToString(); //부서별게시판여부

            return node;
        }

        #endregion Method (함수)
    }
}