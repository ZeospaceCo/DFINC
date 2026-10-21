using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.ETC.Web.Attendance
{
    public partial class UserTree : Base.BasePageETC
    {
        #region 변수설정
        public string AccessAuth = "0";
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Attendance.UserTree).Assembly;	        //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                #region 초기화
                LoadParam();
                InitTree();
                #endregion


            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            AccessAuth = "0";

            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");                       //administrator
            bool isRoleAttendanceDept = CurrentUserInfo.IsInRole("attendanceDept");     //근태부서관리자

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin || isRoleAttendanceDept)
            {
                AccessAuth = "1";
            }
        }

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 트리
        /// </summary>
        public void InitTree()
        {
            #region 트리 초기화
            tvFolderTree.Nodes.Clear();
            tvFolderTree.CollapseNodeOnSelect = false;
            tvFolderTree.ExpandNodeOnSelect = false;
            tvFolderTree.MultipleSelectEnabled = false;
            tvFolderTree.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvFolderTree.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            #endregion 트리 초기화

            LoadTree();

            #region 트리 카테고리에 포커스 설정하기

            ComponentArt.Web.UI.TreeViewNode tvn = this.tvFolderTree.FindNodeById(DeptID.ToString());

            if (tvn != null)
            {
                tvFolderTree.SelectedNode = tvn;
            }

            #endregion 트리 카테고리에 포커스 설정하기
        }
        #endregion

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 트리 로드
        /// </summary>
        private void LoadTree()
        {
            ComponentArt.Web.UI.TreeViewNode topNode = CreateRootNode(GetResource("Attendance_Department")
                , "0", "0", true);
            topNode.ImageUrl = "groups.gif";
            tvFolderTree.Nodes.Add(topNode);

            Account.DAL.Dept biz = new Account.DAL.Dept();
            DAL.AttendanceAdminRole biz2 = new DAL.AttendanceAdminRole();

            DataSet ds;

            if (AccessAuth == "1")
            {
                ds = biz.GetDept(CurrentUserInfo.CompanyID, "dsDept", true);
            }
            else
            {
                ds = biz2.GetDept(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "dsDept");
            }

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
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 하위 트리 만들기
        /// </summary>
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
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 루트 생성
        /// </summary>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(string _text, string _id, string _value, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text.ToReverseXSS();					//목록명
            node.ID = _id;						//목록 고유값
            node.Expanded = _expanded;			//확장여부
            node.Value = "0|" + _text + "|-1|1";
            node.Style.Add("font-style", "bold");
            node.ShowCheckBox = false;
            node.ImageUrl = "groups.gif"; // ImagePath + "/Component/TreeView/root.gif";

            return node;
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2018-01-09
        /// 작성목적 : 트리 노드 생성
        /// </summary>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            #region 다국어 이름 설정

            node.Text = this.GetLangName(
                _dr["NameBase"].ToString(),
                _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

            #endregion 다국어 이름 설정
            node.ID = _dr["DeptID"].ToString();					//부서 ID
            node.Expanded = _expanded;								//확장여부

            node.Value = _dr["DeptID"].ToString() + "|" + node.Text + "|" + _dr["ParentDeptID"].ToString() + "|" + _dr["DeptOrder"].ToString();
            node.Attributes["ParentDeptID"] = _dr["ParentDeptID"].ToString();
            node.Attributes["DeptOrder"] = _dr["DeptOrder"].ToString();
            node.ShowCheckBox = false;
            node.ImageUrl = "group.gif"; // ImagePath + "/Component/TreeView/group.gif";

            return node;
        }

        #endregion Method (함수)

        #region Property (프로퍼티)

        public int DeptID
        {
            get { return GetViewStateString("DeptID", "0").ToNumber<int>(); }
            set { ViewState["DeptID"] = value; }
        }

        #endregion Property (프로퍼티)
    }
}