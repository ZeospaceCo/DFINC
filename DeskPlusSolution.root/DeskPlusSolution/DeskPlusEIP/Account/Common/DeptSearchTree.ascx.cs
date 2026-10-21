using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
namespace Zeospace.DeskPlus.Account.Web.Common
{
    public partial class DeptSearchTree : Base.BaseControlAccount
    {
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
            AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Common.DeptSearchTree).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스

            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack)
            {
                CbtnDeptUserSearch.Text = GetResource("Admin_DeptManager_btnSearch");
                CbtnTreeView.Text = GetResource("Cancel");

                InitTree();
            }

            this.lbDeptUser.Attributes.Add("onChange", "LbxDeptUser_OnChange()");

            this.CallBack1.Callback += new ComponentArt.Web.UI.CallBack.CallbackEventHandler(this.CallBack1_Callback);
        }

        #endregion Page Process (페이지 처리)

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

            SearchDeptTree.Nodes.Clear();
            SearchDeptTree.CollapseNodeOnSelect = false;
            SearchDeptTree.ExpandNodeOnSelect = false;
            SearchDeptTree.MultipleSelectEnabled = false;
            SearchDeptTree.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            SearchDeptTree.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";

            #endregion 트리 초기화

            //CompanyID = GetDecryptParam("companyID", "0").ToNumber<int>();
            //DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();

            LoadTree();

            #region 트리 카테고리에 포커스 설정하기

            ComponentArt.Web.UI.TreeViewNode tvn = this.SearchDeptTree.FindNodeById(DeptID.ToString());

            if (tvn != null)
            {
                SearchDeptTree.SelectedNode = tvn;
            }

            #endregion 트리 카테고리에 포커스 설정하기
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 트리 생성
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        private void LoadTree()
        {
            ComponentArt.Web.UI.TreeViewNode topNode = CreateRootNode(GetResource("Admin_UserManager_Literal2")
                , "0", "0", true);
            topNode.ImageUrl = "groups.gif";
            SearchDeptTree.Nodes.Add(topNode);

            Account.DAL.Dept biz = new DAL.Dept();
            DataSet ds = biz.GetDept(CompanyID, "dsDept", UseActiveDept);
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
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
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

            // deptID | deptName | parentDeptID | deptOrder
            node.Value = "0|" + _text + "|-1|1";

            node.Style.Add("font-style", "bold");

            if (UseCheckBox)
                node.ShowCheckBox = false;

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

            #endregion 다국어 이름 설정

            //node.Text	= _dr["Name"].ToString();					//부서명
            node.ID = _dr["DeptID"].ToString();					//부서 ID
            node.Expanded = _expanded;								//확장여부

            //node.Value = node.Text;
            node.Value = _dr["DeptID"].ToString() + "|" + node.Text + "|" + _dr["ParentDeptID"].ToString() + "|" + _dr["DeptOrder"].ToString();
            node.Attributes["ParentDeptID"] = _dr["ParentDeptID"].ToString();
            node.Attributes["DeptOrder"] = _dr["DeptOrder"].ToString();

            if (ViewState["UseCheckBox"] != null && (bool)ViewState["UseCheckBox"] == true)
                node.ShowCheckBox = true;

            if (ViewState["UseIcon"] != null && (bool)ViewState["UseIcon"] == true)
            {
                node.ImageUrl = "group.gif"; // ImagePath + "/Component/TreeView/group.gif";

                /*
                if (_dr["ParentDeptID"] == System.DBNull.Value)
                {
                    node.ImageUrl = "groups.gif"; // ImagePath + "/Component/TreeView/root.gif";
                }
                else
                {
                    node.ImageUrl = "group.gif"; // ImagePath + "/Component/TreeView/group.gif";
                }
                 */
            }
            return node;
        }


        /// <summary>
        /// Calls the back1_ callback.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void CallBack1_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            // Thread.Sleep simulates the experience of accessing a remote server
            // Remove before going to production

            if (e.Parameter.Length > 0)
            {
                //SortedList sl = new SortedList();
                //검색어^구분자^회사아이디^사용자아이디
                string[] paramStr = e.Parameter.Split('');

                string searchName = paramStr[0];
                string gubun = paramStr[1];
                int companyID = paramStr[2].ToNumber<int>();

                //1 이면 검색, 2이면 트리 다시 보이기
                if (gubun == "1")
                {
                    this.lbDeptUser.Visible = true;
                    this.SearchDeptTree.Visible = false;

                    DataSet ds = LoadDeptUserSearch(companyID, searchName);

                    lbDeptUser.DataSource = ds;
                    lbDeptUser.DataTextField = "Name";
                    lbDeptUser.DataValueField = "ETC";
                    lbDeptUser.DataBind();

                    PlaceHolder1.RenderControl(e.Output);
                }
                else if (gubun == "2")
                {
                    LoadTreeView();
                    PlaceHolder1.RenderControl(e.Output);
                }
            }
        }

        /// <summary>
        /// 작성자  : 심재운
        /// 조직도 검색
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_searchName">_dept user.</param>
        /// <returns></returns>
        private DataSet LoadDeptUserSearch(int _companyID, string _searchName)
        {
            //SortedList sl = new SortedList();
            DataSet mdDs = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("ETC", typeof(string)));
            mdDs.Tables.Add(dt);
            object[] rowValues = new object[2];

            lbDeptUser.Items.Clear();

            if (_searchName.Length > 0)
            {
                Zeospace.DeskPlus.Account.BLL.Dept dept = new Zeospace.DeskPlus.Account.BLL.Dept();
                Account.DAL.UserDept biz = new DAL.UserDept();

                //통합회사전체 이름 검색한다.(atech 별도개발)
                DataSet ds = biz.GetDeptUserByName(_searchName, true);

                string dsText = "";
                string baseDept = "(" + this.GetResource("Module_DeptUserTreeSearch_AdditionalJob") + ")";

                StringBuilder sb = new StringBuilder();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (sb.ToString().Length > 0)
                    {
                        sb.Remove(0, sb.ToString().Length);
                    }

                    if (dr["Type"].ToString() == "M" || dr["CompanyID"].ToString() != CurrentUserInfo.CompanyID.ToString() )
                    {
                        continue;
                    }

                    sb.Append(dr["ID"].ToString());
                    sb.Append("^");
                    sb.Append(dr["Type"].ToString());
                    sb.Append("|");

                    string comName = this.GetLangName(dr["ComBase"].ToString(), dr["ComKo"].ToString(), dr["ComEn"].ToString(), dr["ComJa"].ToString(),
                        dr["ComFr"].ToString(), dr["ComZhCn"].ToString(), dr["ComZhTw"].ToString(), PreLang).ToReverseXSS();

                    string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

                    string titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(),
                        dr["TitleFr"].ToString(), dr["TitleZhCn"].ToString(), dr["TitleZhTw"].ToString(), PreLang).ToReverseXSS();

                    string dutyName = this.GetLangName(dr["DutyBase"].ToString(), dr["DutyKo"].ToString(), dr["DutyEn"].ToString(), dr["DutyJa"].ToString(),
                        dr["DutyFr"].ToString(), dr["DutyZhCn"].ToString(), dr["DutyZhTw"].ToString(), PreLang).ToReverseXSS();

                    string deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(),
                        dr["DeptFr"].ToString(), dr["DeptZhCn"].ToString(), dr["DeptZhTw"].ToString(), PreLang).ToReverseXSS();

                    //겸직,사용자일 경우 -> basedept : 0 겸직, 1은 지정
                    if (dr["BaseDept"].ToString() == "0" && dr["Type"].ToString() != "G")
                    {
                        sb.Append(dr["OwnerID"].ToString());
                        dsText = name + baseDept + "/" + titleName + "/" + deptName + "/" + comName;
                    }//부서일 경우
                    else
                    {
                        sb.Append(dr["OwnerID"].ToString().Replace("G", ""));
                        dsText = deptName;
                    }

                    sb.Append("|");
                    sb.Append(dsText);
                    sb.Append("|");
                    sb.Append(dr["ParentOwnerID"].ToString().Replace("G", ""));
                    sb.Append("|");
                    sb.Append(dr["TitleID"].ToString());
                    sb.Append("|");
                    sb.Append(titleName);
                    sb.Append("|");
                    sb.Append(dr["LoginID"].ToString());
                    sb.Append("|");
                    sb.Append(dr["Mail"].ToString());
                    sb.Append("|");
                    sb.Append(dr["Phone"].ToString());
                    sb.Append("|");
                    sb.Append(dr["OfficePhone"].ToString());
                    sb.Append("|");
                    sb.Append(dutyName);
                    sb.Append("|");
                    sb.Append(deptName);
                    sb.Append("|");
                    sb.Append(dr["DeptID"]);
                    sb.Append("|");
                    sb.Append(dr["EmployeeNumber"]);
                    sb.Append("|");
                    sb.Append(dr["CompanyID"]);

                    //this.lbDeptUser.Items.Add(new ListItem(dsText,sb.ToString()));
                    //sl.Add(dsText,sb.ToString());

                    // type(G) | deptID | deptName | parentDeptID | titleID | titleName | loginID | mail | phone | officePhone | dutyName | deptName | deptID | EmployeeNumber | CompanyID
                    // type(M) | UserID | userName | deptID | titleID | titleName | loginID | mail | phone | officePhone | dutyName | deptName | deptID | EmployeeNumber | CompanyID

                    rowValues[0] = dsText;
                    rowValues[1] = sb.ToString();
                    dt.Rows.Add(rowValues);
                }
            }

            return mdDs;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 트리 처음 로딩시 트리보이고, listbox 감추기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-02-12
        /// </summary>
        private void LoadTreeView()
        {
            this.SearchDeptTree.Visible = true;
            this.lbDeptUser.Visible = false;
            //InitTree();
        }

        #endregion Method (함수)

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
        /// 부서 아이디
        /// </summary>
        /// <value>
        /// The dept ID.
        /// </value>
        public int DeptID
        {
            get { return GetViewStateString("DeptID", "0").ToNumber<int>(); }
            set { ViewState["DeptID"] = value; }
        }

        /// <summary>
        /// 체크박스 사용할것인지
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use check box]; otherwise, <c>false</c>.
        /// </value>
        public bool UseCheckBox
        {
            get { return (bool)(ViewState["UseCheckBox"]); }
            set { ViewState["UseCheckBox"] = value; }
        }

        /// <summary>
        /// 트리에 아이콘 그림을 사용할것인지
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use icon]; otherwise, <c>false</c>.
        /// </value>
        public bool UseIcon
        {
            get { return (bool)(ViewState["UseIcon"]); }
            set { ViewState["UseIcon"] = value; }
        }

        /// <summary>
        /// 활성화 여부
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use active dept]; otherwise, <c>false</c>.
        /// </value>
        public bool UseActiveDept
        {
            get { return (bool)(ViewState["UseActiveDept"]); }
            set { ViewState["UseActiveDept"] = value; }
        }

        #endregion Property (프로퍼티)
    }
}