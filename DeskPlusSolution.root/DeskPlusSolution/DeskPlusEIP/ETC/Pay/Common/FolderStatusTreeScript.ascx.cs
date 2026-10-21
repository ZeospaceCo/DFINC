using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.ETC.Web.Pay.Common
{
    public partial class FolderStatusTreeScript : Base.BaseControlETC
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 페이지 로드
        /// 작성자     : 김형석
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			            //리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Pay.Common.FolderStatusTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) LoadData();
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// 작성자 : 김형석
        /// 목적 : 첫 페이지 초기값 설정
        /// </summary>
        private void LoadData()
        {
            #region 트리 환경 설정
            tvFolderStatus.Nodes.Clear();
            tvFolderStatus.CollapseNodeOnSelect = false;
            tvFolderStatus.ExpandNodeOnSelect = false;
            tvFolderStatus.MultipleSelectEnabled = false;
            tvFolderStatus.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvFolderStatus.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            tvFolderStatus.DragAndDropEnabled = false;

            tvFolderStatus.ClientSideOnNodeSelect = SetNodeSelect;
            tvFolderStatus.ClientSideOnCallbackComplete = SetCallbackComplete;

            #endregion
            
            CreateFolder();

            #region 트리 카테고리에 포커스 설정하기
            ComponentArt.Web.UI.TreeViewNode tvn = tvFolderStatus.FindNodeById(this.FolderStatusID.ToString());
            if (tvn != null) tvFolderStatus.SelectedNode = tvn;
            #endregion
        }
        #endregion

        #region Method (함수)

        #region 트리 노드 생성
        /// <summary>
        /// 작성자 : 김형석
        /// 목적 : 1depth 폴더 생성하기
        /// </summary>
        private void CreateFolder()
        {
            Zeospace.DeskPlus.ETC.DAL.PayFolderStatus dal = new DAL.PayFolderStatus();
            DataSet ds = dal.GetFolderTree(CurrentUserInfo.CompanyID);
            ds.Relations.Add("FolderStatusRelation", ds.Tables[0].Columns["FolderStatusID"], ds.Tables[0].Columns["ParentFolderStatusID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow.IsNull("ParentFolderStatusID"))
                {
                    //treeview 에 노드 상위메뉴 생성(newNode)
                    ComponentArt.Web.UI.TreeViewNode newNode = CreateRootNode(dbRow, true);

                    this.tvFolderStatus.Nodes.Add(newNode);

                    //하위트리 만들기(datarow, treeviw)
                    PopulateSubNode(dbRow, newNode);
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
            foreach (DataRow childRow in _dbRow.GetChildRows("FolderStatusRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateRootNode(childRow, true);
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
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();
            node.Value = name;

            #region 다국어 이름 설정
            node.Text = name;
            #endregion

            node.ID = _dr["FolderStatusID"].ToString(); //ID
            node.Expanded = _expanded; //확장여부              
            node.Attributes["FormID"] = (_dr["FormID"] == System.DBNull.Value) ? "0" : _dr["FormID"].ToString(); ; //회계양식ID
            node.Attributes["IsPay"] = _dr["IsPay"].ToString(); //지급여부
            node.Attributes["TargetPage"] = _dr["TargetPage"].ToString(); //페이지 URL            

            if (UseCheckBox == true) node.ShowCheckBox = (_dr["ParentFolderStatusID"] == System.DBNull.Value) ? false : true;
            node.ImageUrl = "groups.gif";

            return node;
        }
        #endregion

        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// 체크박스 사용할것인지
        /// </summary>
        public bool UseCheckBox
        {
            get { return Convert.ToBoolean(GetViewStateString("UseCheckBox", "false")); }
            set { ViewState["UseCheckBox"] = value; }
        }

        /// <summary>
        /// 사용할 회계문서 폴더(구분자는 콤마)
        /// </summary>
        public string SetFolderStatusList
        {
            get { return GetViewStateString("SetFolderStatusList", ""); }
            set { ViewState["SetFolderStatusList"] = value; }
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

        /// <summary>
        /// 회계문서 FolderStatusID
        /// </summary>
        /// <value>
        /// The folder status ID.
        /// </value>
        public string FolderStatusID
        {
            get { return GetViewStateString("FolderStatusID", ""); }
            set { ViewState["FolderStatusID"] = value; }
        }

        /// <summary>
        /// 회계문서 페이지 URL
        /// </summary>
        public string TargetPage
        {
            get { return GetViewStateString("TargetPage", ""); }
            set { ViewState["TargetPage"] = value; }
        }
        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}