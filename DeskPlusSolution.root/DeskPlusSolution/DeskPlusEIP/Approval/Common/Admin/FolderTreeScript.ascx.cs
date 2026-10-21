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

namespace Zeospace.DeskPlus.Approval.Web.Common.Admin
{
    public partial class FolderTreeScript : Base.BaseControlApproval
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Admin.FolderTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) LoadData();
        }
        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
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

            CreateFolder();

            #region 트리 카테고리에 포커스 설정하기
            ComponentArt.Web.UI.TreeViewNode tvn = tvFolder.FindNodeById(FolderID.ToString());
            if (tvn != null) tvFolder.SelectedNode = tvn;
            #endregion
        }

        #endregion

        #region Method (함수)


        #region 트리 노드 생성

        /// <summary>
        /// 작성자 : 
        /// 목적 : 1depth 폴더 생성하기
        /// </summary>
        private void CreateFolder()
        {
            List<string> list = new List<string>();
            if (TypeInfo.Equals(""))
            {
                list.Add("1");
                list.Add("2");
            }
            else
            {
                list.Add(TypeInfo);
            }

            DAL.Folder fDal = new DAL.Folder();
            DataSet ds = fDal.GetFolderList(CurrentUserInfo.CompanyID, "dsList", list, false);
            ds.Relations.Add("CabinetRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentID"], false);

            foreach (DataRow _dr in ds.Tables[0].Rows)
            {
                if (_dr.IsNull("ParentID"))
                {
                    ComponentArt.Web.UI.TreeViewNode newNode = CreateNode(_dr, true);
                    tvFolder.Nodes.Add(newNode);

                    //하위트리 만들기(datarow, treeviw)
                    PopulateSubNode(_dr, newNode);
                }
            }
        }

        /// <summary>
        /// 노드생성
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

            node.ID = _dr["FolderID"].ToString();					//ID

            node.Attributes["FolderID"] = _dr["FolderID"].ToString();
            node.Attributes["ParentID"] = (_dr["ParentID"] == System.DBNull.Value) ? "0" : _dr["ParentID"].ToString();
            node.Attributes["PathNameBase"] = this.GetLangName(_dr["PathNameBase"].ToString(), _dr["PathNameKo"].ToString(), _dr["PathNameEn"].ToString(), _dr["PathNameJa"].ToString(),
                _dr["PathNameFr"].ToString(), _dr["PathNameZhCn"].ToString(), _dr["PathNameZhTw"].ToString(), PreLang);
            node.Attributes["IsSys"] = _dr["IsSys"].ToString();
            node.Attributes["TypeInfo"] = _dr["TypeInfo"].ToString();       //1(공통),2(자사),3(연동),4(결재망)

            node.Expanded = _expanded;								//확장여부

            if (UseCheckBox == true) node.ShowCheckBox = (_dr["ParentID"] == System.DBNull.Value) ? false : true;

            node.ImageUrl = (_dr["IsSys"].ToString().ToNull().Equals("1")) ? "groups.gif" : "folder.gif";

            //Dictionary<string, string> paraDic = new Dictionary<string, string>();
            //paraDic.Add("pID", _dr["FolderID"].ToString());
            //string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            //node.ContentCallbackUrl = "/DeskPlusEIP/Approval/Common/Form/GetFormTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;

            return node;
        }

        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow"></param>
        /// <param name="_node"></param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow childRow in _dbRow.GetChildRows("CabinetRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow, false);
                _node.Nodes.Add(childhNode);

                PopulateSubNode(childRow, childhNode);
            }
        }

        #endregion

        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// CabinetID
        /// </summary>
        public string FolderID
        {
            get { return GetViewStateString("FolderID", ""); }
            set { ViewState["FolderID"] = value; }
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

        /// <summary>
        /// 구분 (1.공통/2.자사/3.연동/4.결재망)
        /// </summary>
        public string TypeInfo
        {
            get { return GetViewStateString("TypeInfo", ""); }
            set { ViewState["TypeInfo"] = value; }
        }
        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}