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
    public partial class SysCodeTreeScript : Base.BaseControlEIP
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
            AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Common.SysCodeTreeScript).Assembly;	//어셈블리 선언
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
            SysCodeTreeScript1.Nodes.Clear();
            SysCodeTreeScript1.CollapseNodeOnSelect = false;
            SysCodeTreeScript1.ExpandNodeOnSelect = false;
            SysCodeTreeScript1.MultipleSelectEnabled = false;
            SysCodeTreeScript1.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            SysCodeTreeScript1.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            SysCodeTreeScript1.ClientSideOnNodeSelect = SetNodeSelect;
            SysCodeTreeScript1.ClientSideOnCallbackComplete = SetCallbackComplete;
            #endregion

            #region 변수 초기화
            
            #endregion

            LoadTree();


            #region 트리 카테고리에 포커스 설정하기
            ComponentArt.Web.UI.TreeViewNode tvn = this.SysCodeTreeScript1.FindNodeById(SYSCodeID.ToString());

            if (tvn != null)
            {
                SysCodeTreeScript1.SelectedNode = tvn;
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
            ComponentArt.Web.UI.TreeViewNode topNode = CreateRootNode(resMng.GetString("SystemCode"), "0", "0", true);
            topNode.ImageUrl = "groups.gif";
            SysCodeTreeScript1.Nodes.Add(topNode);  

            Zeospace.DeskPlus.Common.DAL.SYSCode biz = new DeskPlus.Common.DAL.SYSCode();            
            DataSet ds = biz.GetSysCodeByPID(CompanyID, "0");

            if (ds != null)
            {
                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    //treeview 에 노드 상위메뉴 생성(newNode)
                    ComponentArt.Web.UI.TreeViewNode newNode = CreateNode(dbRow, true);

                    topNode.Nodes.Add(newNode);
                }
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
            node.Text = _text;					//목록명
            node.ID = _id;						//목록 고유값

            node.Expanded = _expanded;			//확장여부

            node.Attributes["CodeID"] = "0";
            node.Attributes["ParentCodeID"] = "0";
            node.Attributes["CodeOrder"] = "0";

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
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);
            #endregion
            
            node.ID = _dr["SysCodeID"].ToString();
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["CodeID"] = _dr["CodeID"].ToString();
            node.Attributes["ParentCodeID"] = (_dr["ParentCodeID"].ToString() == "") ? "0" : _dr["ParentCodeID"].ToString();
            node.Attributes["CodeOrder"] = _dr["CodeOrder"].ToString();

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
        /// SYSCodeID 아이디
        /// </summary>
        /// <value>
        /// The SYS code ID.
        /// </value>
        public int SYSCodeID
        {
            get { return GetViewStateString("SYSCodeID", "0").ToNumber<int>(); }
            set { ViewState["SYSCodeID"] = value; }
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

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}