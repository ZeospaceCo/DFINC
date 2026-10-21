using System;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.Web.Common
{
    public partial class DeptTreeScript : Base.BaseControlAccount
    {
        #region Page Process (페이지 처리)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 페이지 로드
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스

            ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Common.DeptTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스

            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) InitTree();
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 트리 초기화
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        private void InitTree()
        {
            #region 트리 초기화

            DeptTreeScript1.Nodes.Clear();
            DeptTreeScript1.CollapseNodeOnSelect = false;
            DeptTreeScript1.ExpandNodeOnSelect = false;
            DeptTreeScript1.MultipleSelectEnabled = false;
            DeptTreeScript1.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            DeptTreeScript1.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";

            #endregion 트리 초기화

            //CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            //DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();

            LoadTree();

            #region 트리 카테고리에 포커스 설정하기

            ComponentArt.Web.UI.TreeViewNode tvn = this.DeptTreeScript1.FindNodeById(DeptID.ToString());

            if (tvn != null)
            {
                DeptTreeScript1.SelectedNode = tvn;
            }

            #endregion 트리 카테고리에 포커스 설정하기
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 트리 생성
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        private void LoadTree()
        {
            ComponentArt.Web.UI.TreeViewNode topNode = CreateRootNode(GetResource("Admin_UserManager_Literal2")
                , "0", "0", true);
            topNode.ImageUrl = "groups.gif";
            DeptTreeScript1.Nodes.Add(topNode);

            Account.DAL.Dept biz = new DAL.Dept();
            DataSet ds = biz.GetDept(CompanyID, "dsDept", UseActiveDept);
            ds.Relations.Add("DeptRelation", ds.Tables[0].Columns["DeptID"], ds.Tables[0].Columns["ParentDeptID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow.IsNull("ParentDeptID"))
                {
                    //treeview 에 노드 상위메뉴 생성(newNode)
                    ComponentArt.Web.UI.TreeViewNode newNode = CreateNode(dbRow, false);

                    topNode.Nodes.Add(newNode);

                    //하위트리 만들기(datarow, treeviw)
                    PopulateSubNode(dbRow, newNode);
                }
            }
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow childRow in _dbRow.GetChildRows("DeptRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow, false);
                _node.Nodes.Add(childhNode);

                PopulateSubNode(childRow, childhNode);
            }
        }

        /// <summary>
        /// 루트 노드 생성
        /// </summary>
        /// <param name="_text">The _text.</param>
        /// <param name="_id">The _id.</param>
        /// <param name="_value">The _value.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(string _text, string _id, string _value, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text.ToReverseXSS();					//목록명
            node.ID = _id;						//목록 고유값

            node.Expanded = _expanded;			//확장여부

            // deptID | deptName | parentDeptID | deptOrder
            node.Value = "0|" + _text + "|-1|1";

            node.Style.Add("font-style", "bold");

            if (UseCheckBox)
                node.ShowCheckBox = false;

            if (UseIcon)
                node.ImageUrl = "groups.gif"; // ImagePath + "/Component/TreeView/root.gif";
            return node;
        }

        /// <summary>
        /// 트리 노트 생성
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            #region 다국어 이름 설정

            node.Text = this.GetLangName(
                _dr["NameBase"].ToString(),
                _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

            #endregion 다국어 이름 설정

            //node.Text	= _dr["Name"].ToString();					//부서명
            node.ID = _dr["DeptID"].ToString();					//부서 ID
            node.Expanded = _expanded;								//확장여부

            //node.Value = node.Text;
            node.Value = _dr["DeptID"].ToString() + "|" + node.Text + "|" + _dr["ParentDeptID"].ToString() + "|" + _dr["DeptOrder"].ToString();
            node.Attributes["ParentDeptID"] = _dr["ParentDeptID"].ToString();
            node.Attributes["DeptOrder"] = _dr["DeptOrder"].ToString();

            if (ViewState["UseCheckBox"] != null && (bool)ViewState["UseCheckBox"] == true)
                node.ShowCheckBox = true;

            if (ViewState["UseIcon"] != null && (bool)ViewState["UseIcon"] == true)
            {
                node.ImageUrl = "group.gif"; // ImagePath + "/Component/TreeView/group.gif";

                /*
                if (_dr["ParentDeptID"] == System.DBNull.Value)
                {
                    node.ImageUrl = "groups.gif"; // ImagePath + "/Component/TreeView/root.gif";
                }
                else
                {
                    node.ImageUrl = "group.gif"; // ImagePath + "/Component/TreeView/group.gif";
                }
                 */
            }
            return node;
        }

        #endregion Method (함수)

        #region Property (프로퍼티)

        /// <summary>
        /// 회사 아이디
        /// </summary>
        /// <value>
        /// The company ID.
        /// </value>
        public int CompanyID
        {
            get { return GetViewStateString("CompanyID", "0").ToNumber<int>(); }
            set { ViewState["CompanyID"] = value; }
        }

        /// <summary>
        /// 부서 아이디
        /// </summary>
        /// <value>
        /// The dept ID.
        /// </value>
        public int DeptID
        {
            get { return GetViewStateString("DeptID", "0").ToNumber<int>(); }
            set { ViewState["DeptID"] = value; }
        }

        /// <summary>
        /// 체크박스 사용할것인지
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use check box]; otherwise, <c>false</c>.
        /// </value>
        public bool UseCheckBox
        {
            get { return Convert.ToBoolean(GetViewStateString("UseCheckBox", "false")); }
            set { ViewState["UseCheckBox"] = value; }
        }

        /// <summary>
        /// 트리에 아이콘 그림을 사용할것인지
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use icon]; otherwise, <c>false</c>.
        /// </value>
        public bool UseIcon
        {
            get { return Convert.ToBoolean(GetViewStateString("UseIcon", "false")); }
            set { ViewState["UseIcon"] = value; }
        }

        /// <summary>
        /// 활성화 여부
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use active dept]; otherwise, <c>false</c>.
        /// </value>
        public bool UseActiveDept
        {
            get { return Convert.ToBoolean(GetViewStateString("UseActiveDept", "true")); }
            set { ViewState["UseActiveDept"] = value; }
        }

        #endregion Property (프로퍼티)
    }
}