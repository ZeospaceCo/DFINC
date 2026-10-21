using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Common.Folder
{
    public partial class FolderDeptTreeScript : Base.BaseControlApproval
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
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Folder.FolderDeptTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion



            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) LoadData();
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 첫 페이지 초기값 설정
        /// </summary>
        private void LoadData()
        {
            #region 트리 환경 설정
            tvFolder.Nodes.Clear();
            tvFolder.CollapseNodeOnSelect = false;
            tvFolder.ExpandNodeOnSelect = false;
            tvFolder.MultipleSelectEnabled = false;
            tvFolder.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvFolder.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            tvFolder.DragAndDropEnabled = false;

            tvFolder.ClientSideOnNodeSelect = SetNodeSelect;
            tvFolder.ClientSideOnCallbackComplete = SetCallbackComplete;

            #endregion

            //catUser = new DAL.CategoryUser();
            CreateFolder();


            #region 트리 카테고리에 포커스 설정하기
            ComponentArt.Web.UI.TreeViewNode tvn = tvFolder.FindNodeById(CabinetID.ToString());
            if (tvn != null) tvFolder.SelectedNode = tvn;
            #endregion
        }



        #endregion

        #region Method (함수)

        #region 트리 노드 생성

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 1depth 폴더 생성하기
        /// </summary>
        private void CreateFolder()
        {
            Zeospace.DeskPlus.Approval.DAL.Cabinet dal = new DAL.Cabinet();
            DataSet ds = dal.GetCabinet(CurrentUserInfo.CompanyID);
            ds.Relations.Add("CabinetRelation", ds.Tables[0].Columns["CabinetID"], ds.Tables[0].Columns["ParentCabinetID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                bool expendNode = true;

                //부서별 문서함
                if (dbRow["CabinetID"].ToString() == "Dept")
                {
                    //treeview 에 노드 상위메뉴 생성(newNode)
                    ComponentArt.Web.UI.TreeViewNode newNode = CreateRootNode(dbRow, expendNode);

                    tvFolder.Nodes.Add(newNode);

                    //하위트리 만들기(datarow, treeviw)
                    PopulateSubNode(dbRow, newNode, "CabinetRelation");
                }
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

            #region 다국어 이름 설정
            node.Text = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            node.ID = _dr["CabinetID"].ToString();					//ID
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["FormID"] = "0";
            node.Attributes["TargetPage"] = _dr["TargetPage"].ToString();
            node.Attributes["YYYY"] = ""; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = _dr["CabinetID"].ToString(); //구분자
            node.Attributes["Types"] = _dr["Types"].ToString(); //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = _dr["GroupID"].ToString(); //그룹별

            if (UseCheckBox == true) node.ShowCheckBox = (_dr["ParentCodeID"] == System.DBNull.Value) ? false : true;

            node.ImageUrl = (_dr["Types"].ToString().ToNull() == "S") ? "groups.gif" : "folder.gif";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("useCheckBox", (UseCheckBox) ? "1" : "0");
            paraDic.Add("formID", "0"); //양식id
            paraDic.Add("yyyy", ""); //년도별
            paraDic.Add("yyyymm", ""); //년월별
            paraDic.Add("types", _dr["Types"].ToString().ToNull());
            paraDic.Add("groupID", _dr["GroupID"].ToString().ToNull());
            paraDic.Add("targetPage", _dr["TargetPage"].ToString());
            paraDic.Add("targetDeptID", TargetDeptID);

            string encParams = "";

            switch (_dr["CabinetID"].ToString().ToUpper())
            {
                case "DEPT_DOC": //부서별 문서함
                    paraDic.Add("pID", "Dept_Doc");
                    encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    node.ContentCallbackUrl = "/DeskPlusEIP/Approval/Common/Folder/GetFolderDeptTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
                    break;                

            }

            return node;
        }

        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        /// <param name="_relation">The _relation.</param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node, string _relation)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow childRow in _dbRow.GetChildRows(_relation))
            {
                //수신완료문서함은 제외한다.
                if (childRow["CabinetID"].ToString() != "Dept_Complete")
                {
                    ComponentArt.Web.UI.TreeViewNode childhNode = CreateRootNode(childRow, false);
                    _node.Nodes.Add(childhNode);

                    PopulateSubNode(childRow, childhNode, _relation);
                }                
            }
        }

        #endregion

        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// CabinetID
        /// </summary>
        /// <value>
        /// The cabinet ID.
        /// </value>
        public string CabinetID
        {
            get { return GetViewStateString("CabinetID", ""); }
            set { ViewState["CabinetID"] = value; }
        }


        /// <summary>
        /// 해당 부서 아이디로, 부서별문서함 구성하기
        /// </summary>
        /// <value>
        /// The target dept ID.
        /// </value>
        public string TargetDeptID
        {
            get { return GetViewStateString("TargetDeptID", ""); }
            set { ViewState["TargetDeptID"] = value; }
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




        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}