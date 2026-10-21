using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.Account.Web.Common
{
    public partial class CodeTreeScript : Base.BaseControlAccount
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
            ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Common.CodeTreeScript).Assembly;	//어셈블리 선언
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
            CodeTreeScript1.Nodes.Clear();
            CodeTreeScript1.CollapseNodeOnSelect = false;
            CodeTreeScript1.ExpandNodeOnSelect = false;
            CodeTreeScript1.MultipleSelectEnabled = false;
            CodeTreeScript1.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            CodeTreeScript1.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            #endregion

            #region 변수 초기화
            
            #endregion

            LoadTree();


            #region 트리 카테고리에 포커스 설정하기
            ComponentArt.Web.UI.TreeViewNode tvn = this.CodeTreeScript1.FindNodeById(CodeID.ToString());

            if (tvn != null)
            {
                CodeTreeScript1.SelectedNode = tvn;
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
            //string scope = (UseTitleDuty.ToUpper().Equals("TITLE")) ? "T" : "D";           

            Zeospace.DeskPlus.Common.DAL.Code biz = new DeskPlus.Common.DAL.Code();
            //DataSet ds = biz.GetCode(CompanyID, scope, UseActiveCode);
            DataSet ds = biz.GetCode(CompanyID, UseActiveCode);
            ds.Relations.Add("CodeRelation", ds.Tables[0].Columns["CodeID"], ds.Tables[0].Columns["ParentCodeID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow.IsNull("ParentCodeID"))
                {
                    //treeview 에 노드 상위메뉴 생성(newNode)
                    ComponentArt.Web.UI.TreeViewNode newNode = CreateNode(dbRow, true);

                    CodeTreeScript1.Nodes.Add(newNode);

                    //하위트리 만들기(datarow, treeviw)
                    PopulateSubNode(dbRow, newNode);
                }
            }
        }

        #endregion

        #region Method (함수)


        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow childRow in _dbRow.GetChildRows("CodeRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow, false);
                _node.Nodes.Add(childhNode);

                PopulateSubNode(childRow, childhNode);
            }
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

            //node.Text	= _dr["Name"].ToString();					//부서명
            node.ID = _dr["CodeID"].ToString();					//부서 ID
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["ParentCodeID"] = (_dr["ParentCodeID"].ToString() == "") ? "0" : _dr["ParentCodeID"].ToString();
            node.Attributes["CodeOrder"] = _dr["CodeOrder"].ToString();
            node.Attributes["Scope"] = _dr["Scope"].ToString();


            if (ViewState["UseCheckBox"] != null && (bool)ViewState["UseCheckBox"] == true)
            {
                node.ShowCheckBox = (_dr["ParentCodeID"] == System.DBNull.Value) ? false : true;                
            }

            if (ViewState["UseIcon"] != null && (bool)ViewState["UseIcon"] == true)
            {
                node.ImageUrl = "group.gif";
            }

            if (_dr["ParentCodeID"] == System.DBNull.Value)
            {
                node.ImageUrl = "groups.gif";
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
        /// 코드 아이디
        /// </summary>
        /// <value>
        /// The CodeID
        /// </value>
        public int CodeID
        {
            get { return GetViewStateString("CodeID", "0").ToNumber<int>(); }
            set { ViewState["CodeID"] = value; }
        }


        /// <summary>
        /// 체크박스 사용할것인지
        /// </summary>
        public bool UseCheckBox
        {
            get { return Convert.ToBoolean(GetViewStateString("UseCheckBox", "false")); }
            set { ViewState["UseCheckBox"] = value; }
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
        /// 활성화 여부
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [use active dept]; otherwise, <c>false</c>.
        /// </value>
        public bool UseActiveCode
        {
            get { return Convert.ToBoolean(GetViewStateString("UseActiveCode", "true")); }
            set { ViewState["UseActiveCode"] = value; }
        }       


        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}