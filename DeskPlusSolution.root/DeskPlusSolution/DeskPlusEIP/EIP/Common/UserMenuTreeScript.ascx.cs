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
    public partial class UserMenuTreeScript : Base.BaseControlEIP
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
            AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Common.UserMenuTreeScript).Assembly;	//어셈블리 선언
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
            MenuTreeScript1.Nodes.Clear();
            MenuTreeScript1.CollapseNodeOnSelect = false;
            MenuTreeScript1.ExpandNodeOnSelect = false;
            MenuTreeScript1.MultipleSelectEnabled = false;
            MenuTreeScript1.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            MenuTreeScript1.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            MenuTreeScript1.ClientSideOnNodeSelect = SetNodeSelect;
            MenuTreeScript1.ClientSideOnCallbackComplete = SetCallbackComplete;
            #endregion

            #region 변수 초기화
            
            #endregion

            LoadTree();


            #region 트리 카테고리에 포커스 설정하기
            ComponentArt.Web.UI.TreeViewNode tvn = this.MenuTreeScript1.FindNodeById(MenuID.ToString());

            if (tvn != null)
            {
                MenuTreeScript1.SelectedNode = tvn;
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
            ComponentArt.Web.UI.TreeViewNode topNode = CreateRootNode(resMng.GetString("MenuManagement"), "0", "0", true);
            topNode.ImageUrl = "groups.gif";
            MenuTreeScript1.Nodes.Add(topNode);

            Zeospace.DeskPlus.EIP.DAL.Menu dal = new DAL.Menu();
            DataSet ds = dal.GetMenu(CompanyID);
            ds.Relations.Add("MenuRelation", ds.Tables[0].Columns["MenuID"], ds.Tables[0].Columns["ParentMenuID"], false);

            string[] remoteAddrStr = Request.UserHostAddress.Split('.');
            string remoteAddr = UtilHelper.GetIPAddress(remoteAddrStr);

            Zeospace.DeskPlus.EIP.BLL.Menu bll = new BLL.Menu();
            string getMyMenuList = string.Empty;
            List<string> menuListUser = bll.GetMenuListUser(CompanyID, CurrentUserInfo.UserID, remoteAddr.ToDouble<double>());
            if (menuListUser.Count() > 0) getMyMenuList = String.Join(",", menuListUser.Select(x => x.ToString()).ToArray());

            if (ds != null)
            {
                DataView dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "MenuID IN (" + getMyMenuList + ") AND DirectCall <> 'home'";

                DataTable dt = dv.Table.Clone();
                    foreach (DataRowView drv in dv)
                    dt.ImportRow(drv.Row);
                
                DataSet dss = new DataSet();
                dss.Tables.Add(dt);

                dss.Relations.Add("MenuRelation", dss.Tables[0].Columns["MenuID"], dss.Tables[0].Columns["ParentMenuID"], false);

                foreach (DataRow dbRow in dss.Tables[0].Rows)
                {
                    if (dbRow.IsNull("ParentMenuID"))
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
            foreach (DataRow childRow in _dbRow.GetChildRows("MenuRelation"))
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

            node.Attributes["CodeID"] = "0";
            node.Attributes["ParentCodeID"] = "0";
            node.Attributes["MenuOrder"] = "0";
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
            node.Text = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            if (_dr["ParentMenuID"].ToString().Length > 0 && Disable2Depth)
            {                
                node.CssClass = "GrayedTreeNode";
                node.HoverCssClass = "GrayedTreeNode";
                node.SelectedCssClass = "GrayedTreeNode";
                node.Selectable = false;
            }           
            
            node.ID =  "Fa_" + _dr["MenuID"].ToString();
            node.Expanded = _expanded;								//확장여부 
            node.ShowCheckBox = true;
            node.Attributes["MenuID"] = _dr["MenuID"].ToString();
            node.Attributes["ParentMenuID"] = (_dr["ParentMenuID"].ToString() == "") ? "0" : _dr["ParentMenuID"].ToString();
            node.Attributes["MenuOrder"] = _dr["MenuOrder"].ToString();
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
        /// MenuID 아이디
        /// </summary>
        /// <value>
        /// The SYS code ID.
        /// </value>
        public int MenuID
        {
            get { return GetViewStateString("MenuID", "0").ToNumber<int>(); }
            set { ViewState["MenuID"] = value; }
        }
        

        /// <summary>
        /// 트리에 아이콘 그림을 사용할것인지
        /// </summary>
        public bool UseIcon
        {
            get { return Convert.ToBoolean(GetViewStateString("UseIcon", "false")); }
            set { ViewState["UseIcon"] = value; }
        }

        /// <summary>
        /// callback 호출 완료 후 호출할 스크립트 함수
        /// </summary>
        public string SetCallbackComplete
        {
            get { return GetViewStateString("SetCallbackComplete", ""); }
            set { ViewState["SetCallbackComplete"] = value; }
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
        /// 트리 노드 선택시 호출할 스크립트 함수
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disable2 depth]; otherwise, <c>false</c>.
        /// </value>
        public bool Disable2Depth
        {
            get { return Convert.ToBoolean(GetViewStateString("Disable2Depth", "false")); }
            set { ViewState["Disable2Depth"] = value; }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}