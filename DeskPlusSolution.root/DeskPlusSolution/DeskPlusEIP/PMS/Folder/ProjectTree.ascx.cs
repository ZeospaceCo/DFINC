using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ComponentArt.Web.UI;
using System.Collections.Generic;
using System.Text;
using Zeospace.DeskPlus.PMS.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Folder
{
    public partial class ProjectTree : Base.BaseControlPMS
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Folder.ProjectTree).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion


            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) InitTree();
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 트리 초기화
        /// </summary>
        private void InitTree()
        {
            #region 사용자정보가져오기
            ////string loginID = HttpContext.Current.User.Identity.Name;
            ////if (loginID == "") return;

            ////UserInfoHelper userInfo = new UserInfoHelper(loginID);
            CompanyID = CurrentUserInfo.CompanyID;	//회사 id
            UserID = CurrentUserInfo.UserID;		//유저 id
            DeptID = CurrentUserInfo.DeptID;		//부서 id	
            
            string userDepts = "";
            string[] userDeptArr = CurrentUserInfo.UserDepts;	//유저의 부서 id
            //string[] auth = CurrentUserInfo.Authrity;
            for (int i = 0; i < userDeptArr.Length; i++)
            {
                if (userDepts.Length > 0) userDepts += ",";
                userDepts += userDeptArr[i];
            }
            UserDepts = userDepts;	//유저의 부서 id
            #endregion

            tvProjectTree.CollapseNodeOnSelect = false;
            tvProjectTree.ExpandNodeOnSelect = false;
            tvProjectTree.MultipleSelectEnabled = false;
            tvProjectTree.DragAndDropEnabled = false;

            tvProjectTree.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvProjectTree.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";


            //ComponentArt.Web.UI.TreeViewNode topTempNode1 = CreateBaseNode(GetResource("Module_Node1"), "-1", "-1|" + UserID, true, true);
            //tvProjectTree.Nodes.Add(topTempNode1);


            //ComponentArt.Web.UI.TreeViewNode node = CreateBaseNode(GetResource("Module_Node2"), "-2", "-2|" + UserID, false, true);
            //_topTempNode.Nodes.Add(node);

            ComponentArt.Web.UI.TreeViewNode node1 = CreateBaseNode(GetResource("Module_Node3"), "-3", "-3|" + UserID, true, true, false);
            tvProjectTree.Nodes.Add(node1);

            FolderNode(node1);

            //node = CreateBaseNode(GetResource("Module_Node4"), "-4", "-4|" + UserID, false, true);
            //_topTempNode.Nodes.Add(node);

            ComponentArt.Web.UI.TreeViewNode node2 = CreateBaseNode(GetResource("Module_Node5"), "-5", "-5|" + UserID, true, true, true);
            tvProjectTree.Nodes.Add(node2);

            MyNode(node2);



            //LoadTree(topTempNode1);            
        }


        /// <summary>
        /// 트리 바인딩
        /// </summary>
        //private void LoadTree(ComponentArt.Web.UI.TreeViewNode _topTempNode)
        //{


        //    //ComponentArt.Web.UI.TreeViewNode node = CreateBaseNode(GetResource("Module_Node2"), "-2", "-2|" + UserID, false, true);
        //    //_topTempNode.Nodes.Add(node);

        //    ComponentArt.Web.UI.TreeViewNode node1 = CreateBaseNode(GetResource("Module_Node3"), "-3", "-3|" + UserID, false, true);
        //    _topTempNode.Nodes.Add(node1);

        //    FolderNode(node1);

        //    //node = CreateBaseNode(GetResource("Module_Node4"), "-4", "-4|" + UserID, false, true);
        //    //_topTempNode.Nodes.Add(node);

        //    ComponentArt.Web.UI.TreeViewNode node2 = CreateBaseNode(GetResource("Module_Node5"), "-5", "-5|" + UserID, true, true);
        //    _topTempNode.Nodes.Add(node2);

        //    MyNode(node2);
        //}
        

        #endregion

        #region Method (함수)

        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 나의 프로젝트
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-13
        /// </summary>
        /// <param name="_parentNode">The _parent node.</param>
        private void MyNode(ComponentArt.Web.UI.TreeViewNode _parentNode)
        {
            DataSet ds = new DataSet();

            Zeospace.DeskPlus.PMS.DAL.ProjectUser bll = new DAL.ProjectUser();
            ds = bll.GetProjectUser(CompanyID, UserID);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                //프로젝트를 개설해 줄때 나의 프로젝트에 표기된다.
                if (dbRow["ProjectStatus"].ToString().Trim() != "1")
                {
                    ComponentArt.Web.UI.TreeViewNode node = CreateMyNode(dbRow, false);
                    _parentNode.Nodes.Add(node);
                }
            }
        }


        /// <summary>
        /// 메서드명   : 
		/// 작성목적   : 폴더
		/// 작성자     : 심재운
		/// 최초작성일 : 2009-01-13
        /// </summary>
        /// <param name="_parentNode">The _parent node.</param>
        private void FolderNode(ComponentArt.Web.UI.TreeViewNode _parentNode)
        {
            DataSet ds = new DataSet();

            Zeospace.DeskPlus.PMS.DAL.Folder bll = new Zeospace.DeskPlus.PMS.DAL.Folder();
            ds = bll.GetFolder(CompanyID, 0);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                ComponentArt.Web.UI.TreeViewNode node = CreateFolderNode(dbRow, false);
                _parentNode.Nodes.Add(node);
            }
        }

        #region 트리 노드 생성
        /// <summary>
        /// 최상위 트리 노드 생성 : Creates the root node.
        /// </summary>
        /// <param name="_text">_text.</param>
        /// <param name="_id">_id.</param>
        /// <param name="_value">_value.</param>
        /// <param name="_root">if set to <c>true</c> [_root].</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateBaseNode(string _text, string _id, string _value, bool _root, bool _expanded, bool _isPrivate)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text.ToReverseXSS();					//목록명
            node.ID = _id;						//목록 고유값
            node.Value = _value;				//하위메뉴값						
            node.Expanded = _expanded;			//확장여부

            node.Style.Add("font-style", "bold");

            if (ViewState["UseIcon"] != null && (bool)ViewState["UseIcon"] == true)
            {
                if (_isPrivate)
                {
                    if (_root)node.ImageUrl = "private.gif";
                }
                else
                {
                    node.ImageUrl = (_root) ? "folders.gif" : "folder.gif";
                }
            }

            return node;
        }


        /// <summary>
        /// 트리노드 생성 : Creates the node.
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateMyNode(DataRow _dr, bool _expanded)
        {            

            //1. 카테고리값,  2. 타이틀           3. 하위메뉴값,             4. order            5. 첨부이미지본문표시유무 
            //6. 답변사용유무 7. 짧은답글사용유무 8. 조회자통계정보사용유무, 9. 부서게시판여부, 10. 권한여부(true/false,1:읽기/2:쓰기), 11, custom 페이지경로

            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();


            string titleName = this.GetLangName(
                                    _dr["SubjectBase"].ToString(),
                                    _dr["SubjectKo"].ToString(), _dr["SubjectEn"].ToString(), _dr["SubjectJa"].ToString(),
                                    _dr["SubjectFr"].ToString(), _dr["SubjectZhcn"].ToString(), _dr["SubjectZhtw"].ToString(), PreLang).ToReverseXSS();

            node.Text = (titleName.Length > 0) ? titleName : "No Title";
            node.ID = _dr["ProjectID"].ToString();			//목록 고유값
            //node.Value							= nodeValue;
            node.Expanded = _expanded;


            //string ppt = "";

            //if (ViewState["UseIcon"] != null && (bool)ViewState["UseIcon"] == true)
            //{
            //    node.ImageUrl = "folders.gif";
            //    ppt += "&UseIcon=1";
            //}

            node.Style.Add("font-style", "bold");            

            StringBuilder sb = new StringBuilder();            
            sb.Append("p|");
            sb.Append(_dr["ProjectID"].ToString());
            sb.Append("|");
            sb.Append(titleName);            
            sb.Append("|");
            sb.Append(_dr["UserID"].ToString());            
            node.Value = sb.ToString();

            return node;
        }


        /// <summary>
        /// 트리노드 생성 : Creates the node.
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateFolderNode(DataRow _dr, bool _expanded)
        {

            //1. 카테고리값,  2. 타이틀           3. 하위메뉴값,             4. order            5. 첨부이미지본문표시유무 
            //6. 답변사용유무 7. 짧은답글사용유무 8. 조회자통계정보사용유무, 9. 부서게시판여부, 10. 권한여부(true/false,1:읽기/2:쓰기), 11, custom 페이지경로

            string folderName = this.GetLangName(
                                    _dr["FolderBase"].ToString(),
                                    _dr["FolderKo"].ToString(), _dr["FolderEn"].ToString(), _dr["FolderJa"].ToString(),
                                    _dr["FolderFr"].ToString(), _dr["FolderZhcn"].ToString(), _dr["FolderZhtw"].ToString(), PreLang).ToReverseXSS();

            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = (folderName.Length > 0) ? folderName : "No Title";
            node.ID = _dr["FolderID"].ToString();			//목록 고유값
            //node.Value							= nodeValue;
            node.Expanded = _expanded;


            Dictionary<string, string> paraDic = new Dictionary<string, string>(); 

            if (ViewState["UseIcon"] != null && (bool)ViewState["UseIcon"] == true)
            {
                node.ImageUrl = "folder.gif";
                paraDic.Add("UseIcon", "1");
            }

            node.Style.Add("font-style", "bold");

            if (_dr["childCnt"].ToString().ToNumber<int>() > 0)
            {
                paraDic.Add("pID", _dr["FolderID"].ToString());
                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                node.ContentCallbackUrl = "/DeskPlusEIP/PMS/Module/GetFolder.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("f|");
            sb.Append(_dr["FolderID"].ToString());
            sb.Append("|");
            sb.Append(folderName);            
            node.Value = sb.ToString();

            return node;
        }


        #endregion



        #endregion

        #region Property (프로퍼티)


        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        private int CompanyID
        {
            get { return GetViewStateInt("CompanyID", "0"); }
            set { ViewState["CompanyID"] = value; }
        }


        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        private int UserID
        {
            get { return GetViewStateInt("UserID", "0"); }
            set { ViewState["UserID"] = value; }
        }



        /// <summary>
        /// Gets or sets the dept ID.
        /// </summary>
        /// <value>The dept ID.</value>
        private int DeptID
        {
            get { return GetViewStateInt("DeptID", "0"); }
            set { ViewState["DeptID"] = value; }
        }


        /// <summary>
        /// 겸직
        /// </summary>
        /// <value>The user depts.</value>
        private string UserDepts
        {
            get { return GetViewStateString("UserDepts", ""); }
            set { ViewState["UserDepts"] = value; }
        }


        /// <summary>
        /// BoardTreeScriptView 프로퍼티
        /// </summary>
        /// <value>The category tree script view.</value>
        public ComponentArt.Web.UI.TreeView CategoryTreeScriptView
        {
            get
            {
                return tvProjectTree;
            }
        }



        /// <summary>
        /// 트리 아이콘 사용여부(true, false)
        /// </summary>
        public bool UseIcon
        {
            get { return Convert.ToBoolean(ViewState["UseIcon"]); }
            set { ViewState["UseIcon"] = value; }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}