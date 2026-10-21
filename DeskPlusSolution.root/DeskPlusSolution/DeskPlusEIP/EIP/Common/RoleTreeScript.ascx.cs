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
    public partial class RoleTreeScript : Base.BaseControlEIP
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
            AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Common.RoleTreeScript).Assembly;	//어셈블리 선언
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
            RoleTreeScript1.Nodes.Clear();
            RoleTreeScript1.CollapseNodeOnSelect = false;
            RoleTreeScript1.ExpandNodeOnSelect = false;
            RoleTreeScript1.MultipleSelectEnabled = false;
            RoleTreeScript1.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            RoleTreeScript1.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            RoleTreeScript1.ClientSideOnNodeSelect = SetNodeSelect;
            RoleTreeScript1.ClientSideOnCallbackComplete = SetCallbackComplete;
            #endregion

            #region 변수 초기화
            
            #endregion

            LoadTree();


            #region 트리 카테고리에 포커스 설정하기
            ComponentArt.Web.UI.TreeViewNode tvn = this.RoleTreeScript1.FindNodeById(RoleID.ToString());

            if (tvn != null)
            {
                RoleTreeScript1.SelectedNode = tvn;
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
            Zeospace.DeskPlus.EIP.DAL.Role dal = new DAL.Role();

            if (UseRole)
            {
                ComponentArt.Web.UI.TreeViewNode topNode1 = CreateRootNode(resMng.GetString("AuthManage"), "-1", "-1", true);
                topNode1.ImageUrl = "groups.gif";
                RoleTreeScript1.Nodes.Add(topNode1);                
                DataSet dsRole = dal.GetRole(CurrentUserInfo.CompanyID, "1");

                if (dsRole != null)
                {
                    foreach (DataRow dbRow in dsRole.Tables[0].Rows)
                    {
                        if (dbRow["RoleCode"].ToString() != "sysadmin")
                        {
                            ComponentArt.Web.UI.TreeViewNode newNode = CreateNode(dbRow, true);
                            topNode1.Nodes.Add(newNode);
                        }
                    }
                }
            }

            if (UseGrade)
            {
                ComponentArt.Web.UI.TreeViewNode topNode2 = CreateRootNode(resMng.GetString("LevelManage"), "-2", "-2", false);
                topNode2.ImageUrl = "groups.gif";
                RoleTreeScript1.Nodes.Add(topNode2);
                DataSet dsGrade =  dal.GetRole(CurrentUserInfo.CompanyID, "2");

                if (dsGrade != null)
                {
                    foreach (DataRow dbRow in dsGrade.Tables[0].Rows)
                    {
                        if (dbRow["RoleCode"].ToString() != "sysadmin")
                        {
                            ComponentArt.Web.UI.TreeViewNode newNode = CreateNode(dbRow, true);
                            topNode2.Nodes.Add(newNode);
                        }
                    }
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
            node.Value = _id + "|" + _text;

            node.Attributes["RoleID"] = _id;
            node.Attributes["RoleOrder"] = "0";
            node.Attributes["Gubun"] = (_id == "-1") ? "1" : "2";

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
            node.Text = _dr["RoleName"].ToString();
            #endregion
            
            node.ID = _dr["RoleID"].ToString();
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["RoleID"] = _dr["RoleID"].ToString();
            node.Attributes["RoleOrder"] = _dr["RoleOrder"].ToString();
            node.Attributes["Gubun"] = _dr["Gubun"].ToString();
            node.Value = _dr["RoleID"].ToString() + "|" + _dr["RoleName"].ToString();

            if (UseIcon == true) node.ImageUrl = "group.gif";
            if (UseCheckBox == true) node.ShowCheckBox = true;

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
        /// RoleID 아이디
        /// </summary>
        /// <value>
        /// The SYS code ID.
        /// </value>
        public int RoleID
        {
            get { return GetViewStateString("RoleID", "0").ToNumber<int>(); }
            set { ViewState["RoleID"] = value; }
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
        /// 체크박스 사용여부
        /// </summary>
        public bool UseCheckBox
        {
            get { return Convert.ToBoolean(GetViewStateString("UseCheckBox", "false")); }
            set { ViewState["UseCheckBox"] = value; }
        }


        /// <summary>
        /// 권한관리 사용여부
        /// </summary>
        public bool UseRole
        {
            get { return Convert.ToBoolean(GetViewStateString("UseRole", "false")); }
            set { ViewState["UseRole"] = value; }
        }

        /// <summary>
        /// 등급관리 사용여부
        /// </summary>
        public bool UseGrade
        {
            get { return Convert.ToBoolean(GetViewStateString("UseGrade", "false")); }
            set { ViewState["UseGrade"] = value; }
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