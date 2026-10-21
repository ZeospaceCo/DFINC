using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.EIP.Web.Common
{
    public partial class PortletTreeScript : Base.BaseControlEIP
    {
        #region Declaration (컨트롤 선언)
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
        /// <remarks></remarks>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스
            ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Common.PortletTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) InitTree();
        }

        #endregion

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
            PortletTreeScript1.Nodes.Clear();
            PortletTreeScript1.CollapseNodeOnSelect = false;
            PortletTreeScript1.ExpandNodeOnSelect = false;
            PortletTreeScript1.MultipleSelectEnabled = false;
            PortletTreeScript1.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            PortletTreeScript1.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            PortletTreeScript1.ClientSideOnNodeSelect = SetNodeSelect;
            PortletTreeScript1.ClientSideOnCallbackComplete = SetCallbackComplete;
            #endregion

            #region 변수 초기화
            
            #endregion

            LoadTree();


            #region 트리 카테고리에 포커스 설정하기
            ComponentArt.Web.UI.TreeViewNode tvn = this.PortletTreeScript1.FindNodeById(CategoryID.ToString());

            if (tvn != null)
            {
                PortletTreeScript1.SelectedNode = tvn;
            }
            #endregion
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 트리 생성
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        private void LoadTree()
        {
            if (Gubun == "COMMON")
            {
                LoadCommonTree();
            }
            else
            {
                LoadPrivateTree();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 공용포틀릿 트리
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        private void LoadCommonTree()
        {

            ComponentArt.Web.UI.TreeViewNode topNode = CreateRootNode(this.GetResource("PortletManagement"), "0", "0", true);
            topNode.ImageUrl = "groups.gif";
            PortletTreeScript1.Nodes.Add(topNode);

            Zeospace.DeskPlus.EIP.DAL.PortletCategory dal = new DAL.PortletCategory();
            DataSet ds = dal.GetPortletCategory(CompanyID, CurrentUserInfo.UserID, true, UseActive);
            ds.Relations.Add("CategoryRelation", ds.Tables[0].Columns["CategoryID"], ds.Tables[0].Columns["ParentCategoryID"], false);

            if (ds != null)
            {
                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    if (dbRow.IsNull("ParentCategoryID"))
                    {
                        //treeview 에 노드 상위메뉴 생성(newNode)
                        ComponentArt.Web.UI.TreeViewNode newNode = CreateNode(dbRow, false);
                        topNode.Nodes.Add(newNode);
                        PopulateSubNode(dbRow, newNode);
                    }
                }
            }
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 개인용 포틀릿 트리
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        private void LoadPrivateTree()
        {
            ComponentArt.Web.UI.TreeViewNode topNode = CreateRootNode(this.GetResource("PortletManagement"), "0", "0", true);
            topNode.ImageUrl = "groups.gif";
            PortletTreeScript1.Nodes.Add(topNode);

            Zeospace.DeskPlus.EIP.DAL.PortletCategory dal = new DAL.PortletCategory();
            DataSet ds = dal.GetPortletCategory(CompanyID, CurrentUserInfo.UserID, false, UseActive);
            ds.Relations.Add("CategoryRelation", ds.Tables[0].Columns["CategoryID"], ds.Tables[0].Columns["ParentCategoryID"], false);

            if (ds != null)
            {
                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    if (dbRow.IsNull("ParentCategoryID"))
                    {
                        //treeview 에 노드 상위메뉴 생성(newNode)
                        ComponentArt.Web.UI.TreeViewNode newNode = CreateNode(dbRow, false);
                        topNode.Nodes.Add(newNode);
                        PopulateSubNode(dbRow, newNode);
                    }
                }
            }
        }



        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow childRow in _dbRow.GetChildRows("CategoryRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow, false);
                _node.Nodes.Add(childhNode);

                PopulateSubNode(childRow, childhNode);
            }
        }

        #endregion

        #region Method (함수)


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

            node.Attributes["CategoryID"] = "0";
            node.Attributes["ParentCategoryID"] = "0";
            node.Attributes["CategoryOrder"] = "0";
            node.Attributes["Levels"] = "0";

            node.Style.Add("font-style", "bold");            

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
            #endregion
            
            node.ID = _dr["CategoryID"].ToString();
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["CategoryID"] = _dr["CategoryID"].ToString();
            node.Attributes["ParentCategoryID"] = (_dr["ParentCategoryID"].ToString() == "") ? "0" : _dr["ParentCategoryID"].ToString();
            node.Attributes["CategoryOrder"] = _dr["CategoryOrder"].ToString();
            node.Attributes["Levels"] = _dr["Levels"].ToString();

            if (ViewState["UseIcon"] != null && (bool)ViewState["UseIcon"] == true)
            {
                node.ImageUrl = "group.gif";
            }

            return node;
        }

        #endregion

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
        /// CategoryID 아이디
        /// </summary>
        /// <value>
        /// CategoryID
        /// </value>
        public int CategoryID
        {
            get { return GetViewStateString("CategoryID", "0").ToNumber<int>(); }
            set { ViewState["CategoryID"] = value; }
        }


        /// <summary>
        /// UseIcon
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
        /// Gets or sets a value indicating whether [use active].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use active]; otherwise, <c>false</c>.
        /// </value>
        public bool UseActive
        {
            get { return Convert.ToBoolean(GetViewStateString("UseActive", "false")); }
            set { ViewState["UseActive"] = value; }
        }

        /// <summary>
        /// callback 호출 완료 후 호출할 스크립트 함수
        /// </summary>
        /// <value>
        /// The set callback complete.
        /// </value>
        public string SetCallbackComplete
        {
            get { return GetViewStateString("SetCallbackComplete", ""); }
            set { ViewState["SetCallbackComplete"] = value; }
        }

        /// <summary>
        /// 트리 노드 선택시 호출할 스크립트 함수
        /// </summary>
        /// <value>
        /// The set node select.
        /// </value>
        public string SetNodeSelect
        {
            get { return GetViewStateString("SetNodeSelect", ""); }
            set { ViewState["SetNodeSelect"] = value; }
        }

        /// <summary>
        /// 공용인지 개인용인지...
        /// </summary>
        /// <value>
        /// The set node select.
        /// </value>
        public string Gubun
        {
            get { return GetViewStateString("Gubun", "COMMON"); }
            set { ViewState["Gubun"] = value; }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}