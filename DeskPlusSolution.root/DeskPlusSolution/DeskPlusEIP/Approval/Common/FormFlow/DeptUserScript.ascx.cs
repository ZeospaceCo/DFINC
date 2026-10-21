using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.BLL;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;

namespace Zeospace.DeskPlus.Approval.Web.Common.FormFlow
{
    public partial class DeptUserScript : Base.BaseControlApproval
    {
        #region 컨트롤 선언부

        #endregion

        #region 전역변수 선언부
        private int Access;
        #endregion

        #region 페이지 로드
        private void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";					//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.FormFlow.DeptUserScript).Assembly;						//어셈블리 선언
            this.GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();

                InitTree();
            }
                        
            this.lbDeptUser.Attributes.Add("onClick", "LbxDeptUser_OnChange()");
            this.CallBack1.Callback += new ComponentArt.Web.UI.CallBack.CallbackEventHandler(this.CallBack1_Callback);
        }
        #endregion

        #region 데이터바인딩

        /// <summary>
        /// Inits the tree.
        /// </summary>
        public void InitTree()
        {
            #region 트리 초기화
            DeptUserTree.Nodes.Clear();
            DeptUserTree.CollapseNodeOnSelect = false;
            DeptUserTree.ExpandNodeOnSelect = false;
            DeptUserTree.MultipleSelectEnabled = false;
            DeptUserTree.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            DeptUserTree.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            #endregion

            if (ClientSideOnNodeMouseDoubleClick)
            {
                DeptUserTree.ClientSideOnNodeMouseDoubleClick = "DeptUserTreeScript_onNodeDblClick";
            }

            string companyName = this.GetResource("OrganTit");
            string virtualTree = this.GetResource("OrganVirtual");

            //top 트리명(treeRootName) 지정(id 는 0 value 값은 없음)
            ComponentArt.Web.UI.TreeViewNode topNode = CreateNode(companyName, "0", "0", true, false);

            //treeview 노드에 회사명 추가
            DeptUserTree.Nodes.Add(topNode);
            DeptUserTree.SelectedNode = DeptUserTree.Nodes[0];

            BindTree(0, topNode, false);

            #region 가상조직도 사용할 경우

            //가상조직도 사용할 경우
            if (UseVirtualTree)
            {
                //top 트리명(treeRootName) 지정(id 는 0 value 값은 없음)
                ComponentArt.Web.UI.TreeViewNode virtualNode = CreateNode(virtualTree, "0", "0", true, true);

                //treeview 노드에 회사명 추가
                DeptUserTree.Nodes.Add(virtualNode);

                BindTree(0, virtualNode, true);
            }

            #endregion
        }

        /// <summary>
        /// Binds the tree.
        /// </summary>
        /// <param name="_deptID">_dept ID.</param>
        /// <param name="_parentNode">_parent node.</param>
        /// <param name="_virtual">if set to <c>true</c> [_virtual].</param>
        private void BindTree(int _deptID, ComponentArt.Web.UI.TreeViewNode _parentNode, bool _virtual)
        {
            DeptUserTree.CollapseNodeOnSelect = false;
            DeptUserTree.ExpandNodeOnSelect = false;
            DeptUserTree.MultipleSelectEnabled = false;

            //일반조직도
            if (!_virtual)
            {
                Approval.DAL.Account userDept = new DAL.Account();
                Account.BLL.Dept deptBiz = new Account.BLL.Dept();

                DataSet ds = new DataSet();

                #region 속성에 따라 트리 생성시 필요한 데이터 가져오기

                // ### 나의 회사의 [최상위부서] + [사용자] 및 [부서]의 활성화
                if (UseMyRootDept == true)
                {
                    Access = 1;
                    string getTopRootDeptID = deptBiz.GetTopRootDeptID(CompanyID, DeptID, UseActiveDept);
                    if (getTopRootDeptID != "")
                    {
                        string[] getTopRootDeptIDStr = getTopRootDeptID.Split(',');
                        if (getTopRootDeptIDStr.Length > 0)
                        {
                            ds = userDept.GetDeptUserTreeByOwnerID(CompanyID, getTopRootDeptIDStr[0].ToNumber<int>(), UseActiveDept);
                        }
                    }
                }
                else if (UseMyRootDept == false) //### 나의 회사의 [사용자] 및 [부서]
                {
                    Access = 2;
                    ds = userDept.GetDeptUserTree(CompanyID, _deptID, UseActiveDept);
                }

                #endregion

                #region 하위 트리노드 생성하기

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    //하위 부서 조직도 트리 추가하기
                    AddTreeParentNode(_parentNode, dbRow);
                }

                #endregion
            }
            else if (_virtual) //가상조직트리 생성
            {
                Access = 3;

                EIP.DAL.UserRole dal = new EIP.DAL.UserRole();
                //deptid 가 0 이면 role 에서 정보 가져옴
                DataSet ds = dal.GetUserRoleTree(this.CompanyID, _deptID);

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow, false);
                    if (node != null) _parentNode.Nodes.Add(node);
                }
            }
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 하위 부서 조직도 트리 추가하기
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <param name="_parentNode">The _parent node.</param>
        /// <param name="_dbRow">The _DB row.</param>
        private void AddTreeParentNode(ComponentArt.Web.UI.TreeViewNode _parentNode, DataRow _dbRow)
        {
            //감출 특정 최상위 부서 체크
            if (UseRootDeptOwnerID(_dbRow["OwnerID"].ToString()))
            {
                if (Access == 1)// ### 로긴 최상위부서
                {
                    if (_dbRow["Type"].ToString() == "G")
                    {
                        ComponentArt.Web.UI.TreeViewNode node = CreateNode(_dbRow, false);
                        if (node != null) _parentNode.Nodes.Add(node);
                    }
                }
                else if (Access == 2)//### 사용자 및 부서의 활성화 여부
                {
                    if (UseActiveDept) //부서 활성화
                    {
                        if (_dbRow["ActiveDept"].ToString() == "1")
                        {
                            ComponentArt.Web.UI.TreeViewNode node = CreateNode(_dbRow, false);
                            if (node != null) _parentNode.Nodes.Add(node);
                        }
                    }
                    else
                    {
                        ComponentArt.Web.UI.TreeViewNode node = CreateNode(_dbRow, false);
                        if (node != null) _parentNode.Nodes.Add(node);
                    }
                }
            }
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 감출 특정 최상위 부서 체크
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <param name="_ownerID">OwnerID</param>
        /// <returns></returns>
        private bool UseRootDeptOwnerID(string _ownerID)
        {
            bool useOwnerID = true;
            if (SetHiddenRootDept.Length > 0)
            {
                if (SetHiddenRootDept.IndexOf(',') != -1)
                {
                    foreach (string ownerID in SetHiddenRootDept.Split(','))
                    {
                        if (Helpers.UtilHelper.Compare(ownerID, _ownerID))
                        {
                            useOwnerID = false;
                            break;
                        }
                    }
                }
                else
                {
                    if (Helpers.UtilHelper.Compare(SetHiddenRootDept, _ownerID))
                    {
                        useOwnerID = false;
                    }
                }
            }

            return useOwnerID;
        }

        #endregion

        #region 트리 모양 만들기

        /// <summary>
        /// 트리의 노드 생성
        /// </summary>
        /// <param name="_text">The _text.</param>
        /// <param name="_id">The _id.</param>
        /// <param name="_value">The _value.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <param name="_virtual">if set to <c>true</c> [_virtual].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(string _text, string _id, string _value, bool _expanded, bool _virtual)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text.ToReverseXSS();					//목록명
            node.ID = _id;						//목록 고유값
            node.Expanded = _expanded;			//확장여부

            // type(G) | deptID | deptName | parentDeptID | parentDeptName | titleID | titleName | loginID | mail | officephone | fax
            node.Value = "G|0|" + _text + "|-1||||||||";
            node.Style.Add("font-style", "bold");
            node.ImageUrl = "groups.gif";

            if (UseRootCheckBox)
            {
                if (!_virtual)
                {
                    node.ShowCheckBox = (UseRootCheckBox) ? true : false;
                }
                else
                {
                    node.ShowCheckBox = false;
                }
            }

            return node;
        }

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {
            //2013-03-21 : deskplus 계정은 조직도에서 보이지 않음 ( shimpark)
            if (_dr["Type"].ToString().Equals("M") && _dr["OwnerID"].ToString() == "1") return null;

            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            //string titleName = "";

            StringBuilder nameSb = new StringBuilder();
            string baseDept = "(" + this.GetResource("OrganAddtionJob") + ")"; //겸

            #region 다국어 이름 설정
            string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            #region 다국어 직급 설정
            string titleName = this.GetLangName(_dr["TitleBase"].ToString(), _dr["TitleKo"].ToString(), _dr["TitleEn"].ToString(), _dr["TitleJa"].ToString(),
                _dr["TitleFr"].ToString(), _dr["TitleZhCn"].ToString(), _dr["TitleZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            #region 다국어 직책 설정
            string dutyName = this.GetLangName(_dr["DutyBase"].ToString(), _dr["DutyKo"].ToString(), _dr["DutyEn"].ToString(), _dr["DutyJa"].ToString(),
                _dr["DutyFr"].ToString(), _dr["DutyZhCn"].ToString(), _dr["DutyZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            #region 다국어 부서 설정
            string deptName = this.GetLangName(_dr["DeptBase"].ToString(), _dr["DeptKo"].ToString(), _dr["DeptEn"].ToString(), _dr["DeptJa"].ToString(),
                _dr["DeptFr"].ToString(), _dr["DeptZhCn"].ToString(), _dr["DeptZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            string titleNames = (titleName.Length > 0) ? "(" + titleName + ")" : "";

            //겸(0) + 사람일 경우
            if (_dr["BaseDept"].ToString() == "0" && _dr["Type"].ToString() != "G")
            {
                nameSb.Append(name + baseDept + titleNames);	//목록명					
            }
            else
            {
                //겸(x) + 사람일 경우
                if (_dr["Type"].ToString() == "M")
                {
                    nameSb.Append(name + titleNames);
                }
                else //겸(x) + 부서일 경우
                {
                    nameSb.Append(name);
                }
            }

            node.Text = nameSb.ToString();

            //node.Text	= _dr["Name"].ToString();					//목록명
            node.ID = _dr["ID"].ToString();					//목록 고유값
            node.Expanded = _expanded;							//확장여부
            node.ShowCheckBox = false;

            if (_dr["Type"].ToString().Equals("G"))
            {
                if (UseIcon) node.ImageUrl = "group.gif";
            }

            if (_dr["Type"].ToString().Equals("M"))
            {
                node.ImageUrl = "member.gif";
            }

            //G : 조직도 그룹, R : 가상조직도 그룹
            if (_dr["Type"].ToString() == "G" || _dr["Type"].ToString() == "R")
            {
                if (UseCheckBoxGroup)
                {
                    node.ShowCheckBox = true;
                }
            }
            else
            {
                if (UseCheckBoxMember)
                {
                    node.ShowCheckBox = true;
                }
            }

            if (_dr["Type"].ToString() == "G" || _dr["Type"].ToString() == "R")
            {
                node.Text = node.Text;
                node.Style.Add("font-style", "bold");
            }

            // [0]type(G) | [1]deptID | [2]deptName | [3]parentDeptID | [4]parentDeptName | [5]titleID | [6]titleName | [7]dutyID | [8]dutyName | [9]loginID | [10]mail | [11]officephone | [12]fax
            // [0]type(M) | [1]userID | [2]userName | [3]deptID       | [4]deptName       | [5]titleID | [6]titleName | [7]dutyID | [8]dutyName | [9]loginID | [10]mail | [11]phone       | [12]officePhone

            //string nodeData = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(_dr["Type"].ToString());              // [0]type(G/M)
            sb.Append("|");

            if (_dr["Type"].ToString() == "G" || _dr["Type"].ToString() == "R")
            {
                sb.Append(_dr["OwnerID"].ToString().Replace(_dr["Type"].ToString(), ""));			// [1]deptID/userID

                if (_dr["ChildCnt"].ToString().ToNumber<int>() > 0)
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("guid", System.Guid.NewGuid().ToString());
                    paraDic.Add("access", Access.ToString());
                    paraDic.Add("useCheckBoxGroup", (UseCheckBoxGroup) ? "1" : "0");
                    paraDic.Add("useCheckBoxMember", (UseCheckBoxMember) ? "1" : "0");
                    paraDic.Add("useIcon", (UseIcon) ? "1" : "0");
                    paraDic.Add("useActiveDept", (UseActiveDept) ? "1" : "0");
                    paraDic.Add("cID", CompanyID.ToString());
                    paraDic.Add("pID", _dr["OwnerID"].ToString().Replace(_dr["Type"].ToString(), ""));

                    string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                    node.ContentCallbackUrl = "/DeskplusEIP/Approval/Common/FormFlow/GetDeptUserScript.aspx?p=" + encParams;
                }
            }
            else
            {
                sb.Append(_dr["OwnerID"].ToString());       // [1]deptID/userID
            }
            sb.Append("|");
            sb.Append(name);                   // [2]deptName/userName
            sb.Append("|");
            sb.Append(_dr["ParentOwnerID"].ToString().Replace("G", "")); // [3]parentDeptID/deptID      - 하위메뉴값   
            sb.Append("|");
            sb.Append(deptName);                                         // [4]parentDeptName/deptName
            sb.Append("|");
            sb.Append(_dr["TitleID"].ToString());           // [5]titleID/titleID
            sb.Append("|");
            sb.Append(titleName);                           // [6]titleName/titleName
            sb.Append("|");
            sb.Append(_dr["DutyID"].ToString());            // [7]dutyID/dutyID
            sb.Append("|");
            sb.Append(dutyName);                            // [8]dutyName/dutyName
            sb.Append("|");
            sb.Append(_dr["LoginID"].ToString());           // [9]loginID/loginID
            sb.Append("|");
            sb.Append(_dr["Mail"].ToString());              // [10]mail/mail
            sb.Append("|");
            sb.Append(_dr["Phone"].ToString());             // [11]officephone/phone
            sb.Append("|");
            sb.Append(_dr["OfficePhone"].ToString());       // [12]fax/officePhone

            #region 수발자
            name = this.GetLangName(_dr["CPNameBase"].ToString(), _dr["CPNameKo"].ToString(), _dr["CPNameEn"].ToString(), _dr["CPNameJa"].ToString(),
                _dr["CPNameFr"].ToString(), _dr["CPNameZhCn"].ToString(), _dr["CPNameZhTw"].ToString(), PreLang);

            titleName = this.GetLangName(_dr["CPTitleBase"].ToString(), _dr["CPTitleKo"].ToString(), _dr["CPTitleEn"].ToString(), _dr["CPTitleJa"].ToString(),
                                    _dr["CPTitleFr"].ToString(), _dr["CPTitleZhCn"].ToString(), _dr["CPTitleZhTw"].ToString(), PreLang);

            dutyName = this.GetLangName(_dr["CPDutyBase"].ToString(), _dr["CPDutyKo"].ToString(), _dr["CPDutyEn"].ToString(), _dr["CPDutyJa"].ToString(),
                                    _dr["CPDutyFr"].ToString(), _dr["CPDutyZhCn"].ToString(), _dr["CPDutyZhTw"].ToString(), PreLang);

            deptName = this.GetLangName(_dr["CPDeptBase"].ToString(), _dr["CPDeptKo"].ToString(), _dr["CPDeptEn"].ToString(), _dr["CPDeptJa"].ToString(),
                _dr["CPDeptFr"].ToString(), _dr["CPDeptZhCn"].ToString(), _dr["CPDeptZhTw"].ToString(), PreLang);

            sb.Append("|");
            sb.Append(_dr["CPUserID"].ToString());          // [13]cpuserID 
            sb.Append("|");
            sb.Append(name);                                // [14]cpuserName
            sb.Append("|");
            sb.Append(_dr["CPDeptID"].ToString());          // [15]cpdeptID
            sb.Append("|");
            sb.Append(deptName);                            // [16]cpdeptName
            sb.Append("|");
            sb.Append(_dr["CPTitleID"].ToString());         // [17]cptitleID
            sb.Append("|");
            sb.Append(titleName);                           // [18]cptitleName 
            sb.Append("|");
            sb.Append(_dr["CPDutyID"].ToString());          // [19]cpdutyID
            sb.Append("|");
            sb.Append(dutyName);                            // [20]cpdutyName
            sb.Append("|");
            sb.Append(_dr["CPLoginID"].ToString());         // [21]cploginID
            #endregion

            #region 결재자
            name = this.GetLangName(_dr["AppNameBase"].ToString(), _dr["AppNameKo"].ToString(), _dr["AppNameEn"].ToString(), _dr["AppNameJa"].ToString(),
                _dr["AppNameFr"].ToString(), _dr["AppNameZhCn"].ToString(), _dr["AppNameZhTw"].ToString(), PreLang);

            titleName = this.GetLangName(_dr["AppTitleBase"].ToString(), _dr["AppTitleKo"].ToString(), _dr["AppTitleEn"].ToString(), _dr["AppTitleJa"].ToString(),
                                    _dr["AppTitleFr"].ToString(), _dr["AppTitleZhCn"].ToString(), _dr["AppTitleZhTw"].ToString(), PreLang);

            dutyName = this.GetLangName(_dr["AppDutyBase"].ToString(), _dr["AppDutyKo"].ToString(), _dr["AppDutyEn"].ToString(), _dr["AppDutyJa"].ToString(),
                                    _dr["AppDutyFr"].ToString(), _dr["AppDutyZhCn"].ToString(), _dr["AppDutyZhTw"].ToString(), PreLang);

            deptName = this.GetLangName(_dr["AppDeptBase"].ToString(), _dr["AppDeptKo"].ToString(), _dr["AppDeptEn"].ToString(), _dr["AppDeptJa"].ToString(),
                                    _dr["AppDeptFr"].ToString(), _dr["AppDeptZhCn"].ToString(), _dr["AppDeptZhTw"].ToString(), PreLang);

            sb.Append("|");
            sb.Append(_dr["AppUserID"].ToString());         // [22]appuserID
            sb.Append("|");
            sb.Append(name);                                // [23]appuserName
            sb.Append("|");
            sb.Append(_dr["AppDeptID"].ToString());         // [24]appdeptID
            sb.Append("|");
            sb.Append(deptName);                            // [25]appdeptName
            sb.Append("|");
            sb.Append(_dr["AppTitleID"].ToString());        // [26]apptitleID
            sb.Append("|");
            sb.Append(titleName);                           // [27]apptitleName
            sb.Append("|");
            sb.Append(_dr["AppDutyID"].ToString());         // [28]appdutyID
            sb.Append("|");
            sb.Append(dutyName);                            // [29]appdutyName
            sb.Append("|");
            sb.Append(_dr["AppLoginID"].ToString());        // [30]apploginID
            #endregion 

            
            node.Value = sb.ToString();

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
                    this.DeptUserTree.Visible = false;

                    DataSet ds = LoadDeptUserSearch(companyID, searchName);

                    //					foreach (DictionaryEntry key in sl) 
                    //					{
                    //						this.lbDeptUser.Items.Add(new ListItem(key.Key.ToString(),key.Value.ToString()));												
                    //					}

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
        /// 작성자  : 
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
                Account.BLL.Dept dept = new Zeospace.DeskPlus.Account.BLL.Dept();
                Approval.DAL.Account biz = new DAL.Account();
                DataSet ds = biz.GetDeptUserByName(_companyID, _searchName, true);

                string dsText = "";
                string baseDept = "(" + this.GetResource("OrganAddtionJob") + ")";

                StringBuilder sb = new StringBuilder();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (sb.ToString().Length > 0)
                    {
                        sb.Remove(0, sb.ToString().Length);
                    }

                    sb.Append(dr["Type"].ToString());                       // [1] G/M
                    sb.Append("|");

                    string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                    string titleName = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(), dr["TitleJa"].ToString(),
                        dr["TitleFr"].ToString(), dr["TitleZhCn"].ToString(), dr["TitleZhTw"].ToString(), PreLang);

                    string dutyName = this.GetLangName(dr["DutyBase"].ToString(), dr["DutyKo"].ToString(), dr["DutyEn"].ToString(), dr["DutyJa"].ToString(),
                        dr["DutyFr"].ToString(), dr["DutyZhCn"].ToString(), dr["DutyZhTw"].ToString(), PreLang);

                    string deptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(), dr["DeptJa"].ToString(),
                        dr["DeptFr"].ToString(), dr["DeptZhCn"].ToString(), dr["DeptZhTw"].ToString(), PreLang);



                    //겸직,사용자일 경우 -> basedept : 0 겸직, 1은 지정
                    if (dr["BaseDept"].ToString() == "0" && dr["Type"].ToString() != "G")
                    {
                        sb.Append(dr["OwnerID"].ToString());                                    
                        dsText = name + baseDept + "/" + titleName + "/" + deptName;
                    }//부서일 경우
                    else
                    {
                        sb.Append(dr["OwnerID"].ToString().Replace("G", ""));                   // [1]deptID/userID
                        dsText = name + "/" + titleName + "/" + deptName;
                    }

                    sb.Append("|");
                    sb.Append(name);                                                // [2]deptName/userName
                    sb.Append("|");
                    sb.Append(dr["ParentOwnerID"].ToString().Replace("G", ""));     // [3]parentDeptID/deptID      - 하위메뉴값   
                    sb.Append("|");
                    sb.Append(deptName);                                            // [4]parentDeptName/deptName
                    sb.Append("|");
                    sb.Append(dr["TitleID"].ToString());                            // [5]titleID/titleID
                    sb.Append("|");
                    sb.Append(titleName);                                           // [6]titleName/titleName
                    sb.Append("|");
                    sb.Append(dr["DutyID"].ToString());                            // [7]dutyID/dutyID
                    sb.Append("|");
                    sb.Append(dutyName);                                            // [8]dutyName/dutyName
                    sb.Append("|");
                    sb.Append(dr["LoginID"].ToString());                            // [9]loginID/loginID
                    sb.Append("|");
                    sb.Append(dr["Mail"].ToString());                               // [10]mail/mail
                    sb.Append("|");
                    sb.Append(dr["Phone"].ToString());                              // [11]officephone/phone
                    sb.Append("|");
                    sb.Append(dr["OfficePhone"].ToString());                        // [12]fax/officePhone

                    #region 수발자
                    name = this.GetLangName(dr["CPNameBase"].ToString().ToNull(), dr["CPNameKo"].ToString().ToNull(), dr["CPNameEn"].ToString().ToNull(), dr["CPNameJa"].ToString().ToNull(),
                        dr["CPNameFr"].ToString().ToNull(), dr["CPNameZhCn"].ToString().ToNull(), dr["CPNameZhTw"].ToString().ToNull(), PreLang);

                    titleName = this.GetLangName(dr["CPTitleBase"].ToString().ToNull(), dr["CPTitleKo"].ToString().ToNull(), dr["CPTitleEn"].ToString().ToNull(), dr["CPTitleJa"].ToString().ToNull(),
                                            dr["CPTitleFr"].ToString().ToNull(), dr["CPTitleZhCn"].ToString().ToNull(), dr["CPTitleZhTw"].ToString().ToNull(), PreLang);

                    dutyName = this.GetLangName(dr["CPDutyBase"].ToString().ToNull(), dr["CPDutyKo"].ToString().ToNull(), dr["CPDutyEn"].ToString().ToNull(), dr["CPDutyJa"].ToString().ToNull(),
                                            dr["CPDutyFr"].ToString().ToNull(), dr["CPDutyZhCn"].ToString().ToNull(), dr["CPDutyZhTw"].ToString().ToNull(), PreLang);

                    deptName = this.GetLangName(dr["CPDeptBase"].ToString().ToNull(), dr["CPDeptKo"].ToString().ToNull(), dr["CPDeptEn"].ToString().ToNull(), dr["CPDeptJa"].ToString().ToNull(),
                        dr["CPDeptFr"].ToString().ToNull(), dr["CPDeptZhCn"].ToString().ToNull(), dr["CPDeptZhTw"].ToString().ToNull(), PreLang);

                    sb.Append("|");
                    sb.Append(dr["CPUserID"].ToString().ToNull());
                    sb.Append("|");
                    sb.Append(name);
                    sb.Append("|");
                    sb.Append(dr["CPDeptID"].ToString().ToNull());
                    sb.Append("|");
                    sb.Append(deptName);
                    sb.Append("|");
                    sb.Append(dr["CPTitleID"].ToString().ToNull());
                    sb.Append("|");
                    sb.Append(titleName);
                    sb.Append("|");
                    sb.Append(dr["CPDutyID"].ToString().ToNull());
                    sb.Append("|");
                    sb.Append(dutyName);
                    sb.Append("|");
                    sb.Append(dr["CPLoginID"].ToString().ToNull());
                    #endregion 

                    #region 결재자
                    name = this.GetLangName(dr["AppNameBase"].ToString().ToNull(), dr["AppNameKo"].ToString().ToNull(), dr["AppNameEn"].ToString().ToNull(), dr["AppNameJa"].ToString().ToNull(),
                        dr["AppNameFr"].ToString().ToNull(), dr["AppNameZhCn"].ToString().ToNull(), dr["AppNameZhTw"].ToString().ToNull(), PreLang);

                    titleName = this.GetLangName(dr["AppTitleBase"].ToString().ToNull(), dr["AppTitleKo"].ToString().ToNull(), dr["AppTitleEn"].ToString().ToNull(), dr["AppTitleJa"].ToString().ToNull(),
                                            dr["AppTitleFr"].ToString().ToNull(), dr["AppTitleZhCn"].ToString().ToNull(), dr["AppTitleZhTw"].ToString().ToNull(), PreLang);

                    dutyName = this.GetLangName(dr["AppDutyBase"].ToString().ToNull(), dr["AppDutyKo"].ToString().ToNull(), dr["AppDutyEn"].ToString().ToNull(), dr["AppDutyJa"].ToString().ToNull(),
                                            dr["AppDutyFr"].ToString().ToNull(), dr["AppDutyZhCn"].ToString().ToNull(), dr["AppDutyZhTw"].ToString().ToNull(), PreLang);

                    deptName = this.GetLangName(dr["AppDeptBase"].ToString().ToNull(), dr["AppDeptKo"].ToString().ToNull(), dr["AppDeptEn"].ToString().ToNull(), dr["AppDeptJa"].ToString().ToNull(),
                                            dr["AppDeptFr"].ToString().ToNull(), dr["AppDeptZhCn"].ToString().ToNull(), dr["AppDeptZhTw"].ToString().ToNull(), PreLang);

                    sb.Append("|");
                    sb.Append(dr["AppUserID"].ToString().ToNull());
                    sb.Append("|");
                    sb.Append(name);
                    sb.Append("|");
                    sb.Append(dr["AppDeptID"].ToString().ToNull());
                    sb.Append("|");
                    sb.Append(deptName);
                    sb.Append("|");
                    sb.Append(dr["AppTitleID"].ToString().ToNull());
                    sb.Append("|");
                    sb.Append(titleName);
                    sb.Append("|");
                    sb.Append(dr["AppDutyID"].ToString().ToNull());
                    sb.Append("|");
                    sb.Append(dutyName);
                    sb.Append("|");
                    sb.Append(dr["AppLoginID"].ToString().ToNull());
                    #endregion 
                    //this.lbDeptUser.Items.Add(new ListItem(dsText,sb.ToString()));
                    //sl.Add(dsText,sb.ToString());

                    // [0]type(G) | [1]deptID | [2]deptName | [3]parentDeptID | [4]parentDeptName | [5]titleID | [6]titleName | [7]dutyID | [8]dutyName | [9]loginID | [10]mail | [11]officephone | [12]fax
                    // [0]type(M) | [1]userID | [2]userName | [3]deptID       | [4]deptName       | [5]titleID | [6]titleName | [7]dutyID | [8]dutyName | [9]loginID | [10]mail | [11]phone       | [12]officePhone

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
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        private void LoadTreeView()
        {
            this.DeptUserTree.Visible = true;
            this.lbDeptUser.Visible = false;
            //InitTree();
        }

        #endregion

        #region Method (함수)
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnDeptUserSearch.Text = resMng.GetString("OrganBtnSearch");
                CbtnTreeView.Text = resMng.GetString("OrganBtnCancel");
            }
        }
        #endregion

        #region 프로퍼티
        public ComponentArt.Web.UI.TreeView DeptUserTreeView
        {
            get
            {
                return DeptUserTree;
            }
        }

        public Unit Width
        {
            set
            {
                DeptUserTree.Width = value;
            }

        }

        public Unit Height
        {
            set
            {
                DeptUserTree.Height = value;
            }
        }

        public ListSelectionMode UseMultiSelectList
        {
            set
            {
                this.lbDeptUser.SelectionMode = value;
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 트리 마우스 더블클릭 사용여부
        /// 작성자     : 홍석기
        /// 최초작성일 : 
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [client side on node mouse double click]; otherwise, <c>false</c>.
        /// </value>
        public bool ClientSideOnNodeMouseDoubleClick
        {
            get { return (bool)(ViewState["ClientSideOnNodeMouseDoubleClick"] ?? false); }
            set { ViewState["ClientSideOnNodeMouseDoubleClick"] = value; }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : listbox 에서 더블클릭 가능
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [lb dept user double click]; otherwise, <c>false</c>.
        /// </value>
        public bool lbDeptUserDoubleClick
        {
            get { return (bool)(ViewState["lbDeptUserDoubleClick"] ?? false); }
            set { ViewState["lbDeptUserDoubleClick"] = value; }
        }

        /// <summary>
        /// 회사아이디
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
        /// 부서아이디
        /// </summary>
        /// <value>
        /// DeptID
        /// </value>
        public int DeptID
        {
            get { return GetViewStateString("DeptID", "0").ToNumber<int>(); }
            set { ViewState["DeptID"] = value; }
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 아이콘 사용여부
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <value><c>true</c> if [use icon]; otherwise, <c>false</c>.</value>
        public bool UseIcon
        {
            get { return (bool)(ViewState["UseIcon"] ?? false); }
            set { ViewState["UseIcon"] = value; }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 그룹 체크박스 활성화여부
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <value><c>true</c> if [use check box group]; otherwise, <c>false</c>.</value>
        public bool UseCheckBoxGroup
        {
            get { return (bool)(ViewState["UseCheckBoxGroup"] ?? false); }
            set { ViewState["UseCheckBoxGroup"] = value; }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 맴버 체크박스 활성화여부
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <value><c>true</c> if [use check box member]; otherwise, <c>false</c>.</value>
        public bool UseCheckBoxMember
        {
            get { return (bool)(ViewState["UseCheckBoxMember"] ?? false); }
            set { ViewState["UseCheckBoxMember"] = value; }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 나의 해당 최상위 부서만 보이도록 함
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <value><c>true</c> if [use my dept]; otherwise, <c>false</c>.</value>
        public bool UseMyRootDept
        {
            get { return (bool)(ViewState["UseMyRootDept"] ?? false); }
            set { ViewState["UseMyRootDept"] = value; }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 사용자및 부서의 활성화 보이기/감추기
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <value><c>true</c> if [use active dept]; otherwise, <c>false</c>.</value>
        public bool UseActiveDept
        {
            get { return (bool)(ViewState["UseActiveDept"] ?? true); }
            set { ViewState["UseActiveDept"] = value; }
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 최상위메뉴 체크박스 사용여부
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <value><c>true</c> if [use parent check box]; otherwise, <c>false</c>.</value>
        public bool UseRootCheckBox
        {
            get { return (bool)(ViewState["UseRootCheckBox"] ?? false); }
            set { ViewState["UseRootCheckBox"] = value; }
        }


        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 최상위 부서들 중 감출 부서값(구분자는 콤마 예 : 1,2,3)
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <value>The set hidden root dept.</value>
        public string SetHiddenRootDept
        {
            get { return (string)(ViewState["SetHiddenRootDept"] ?? ""); }
            set { ViewState["SetHiddenRootDept"] = value; }
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 트리 백그라운드칼라
        /// 작성자     : 
        /// 최초작성일 : 
        /// </summary>
        /// <value>The color of the back.</value>
        public string BackColor
        {
            set
            {
                DeptUserTree.BackColor = Color.FromName(value);
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [use virtual tree].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use virtual tree]; otherwise, <c>false</c>.
        /// </value>
        public bool UseVirtualTree
        {
            get { return (bool)(ViewState["UseVirtualTree"] ?? false); }
            set { ViewState["UseVirtualTree"] = value; }
        }



        #endregion        
    }
}