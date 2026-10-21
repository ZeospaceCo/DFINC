using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.MDM.Web.Common
{
    public partial class MDMMenuTreeScript : Base.BaseControlMDM
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스
            ResourceName = "Zeospace.DeskPlus.MDM.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.MDM.Web.Common.MDMMenuTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) InitTree();
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-01-17
        /// 작성목적 : 트리 초기화
        /// </summary>
        private void InitTree()
        {
            #region 트리 초기화
            MDMMenuTreeScript1.Nodes.Clear();
            MDMMenuTreeScript1.CollapseNodeOnSelect = false;
            MDMMenuTreeScript1.ExpandNodeOnSelect = false;
            MDMMenuTreeScript1.MultipleSelectEnabled = false;
            MDMMenuTreeScript1.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            MDMMenuTreeScript1.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            MDMMenuTreeScript1.ClientSideOnNodeSelect = SetNodeSelect;
            MDMMenuTreeScript1.ClientSideOnCallbackComplete = SetCallbackComplete;
            #endregion

            #region 변수 초기화
            
            #endregion

            LoadTree();


            #region 트리 카테고리에 포커스 설정하기
            //페이지 시작시 노드 선택
            this.MDMMenuTreeScript1.SelectedNode = this.MDMMenuTreeScript1.Nodes[0].Nodes[0];
            #endregion
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-01-17
        /// 작성목적 : Tree 생성
        /// </summary>
        private void LoadTree()
        {
            ComponentArt.Web.UI.TreeViewNode topNode = CreateRootNode(resMng.GetString("MenuManagement"), "0", true);
            topNode.ImageUrl = "groups.gif";
            MDMMenuTreeScript1.Nodes.Add(topNode);

            Zeospace.DeskPlus.MDM.DAL.Menu dal = new DAL.Menu();
            DataSet ds = dal.GetMenu(CompanyID);

            if (ds != null)
            {
                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    ComponentArt.Web.UI.TreeViewNode newNode = CreateNode(dbRow, false);
                    topNode.Nodes.Add(newNode);
                }
            }
        }
     

        #endregion

        #region Method (함수)


        /// <summary>        
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_id"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(string _text, string _id, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text.ToReverseXSS();					//목록명
            node.ID = _id;						//목록 고유값
            node.Expanded = _expanded;			//확장여부
            node.CssClass = "GrayedTreeNode";
            node.HoverCssClass = "GrayedTreeNode";
            node.SelectedCssClass = "GrayedTreeNode";
            node.Selectable = false;
            node.Style.Add("font-style", "bold");            

            if (UseIcon)
                node.ImageUrl = "groups.gif"; 
            return node;
        }        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dr"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            #region 다국어 이름 설정
            node.Text = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion      
            
            node.ID = _dr["MenuID"].ToString();
            node.Expanded = _expanded;								      
            node.Attributes["MenuID"] = _dr["MenuID"].ToString();

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