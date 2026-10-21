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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Module
{
    public partial class Folder : Base.BaseControlPMS
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
            AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Module.Folder).Assembly;	//어셈블리 선언
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
            //string loginID = HttpContext.Current.User.Identity.Name;
            //if (loginID == "") return;

            //UserInfoHelper userInfo = new UserInfoHelper(loginID);
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

            FolderScript1.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            FolderScript1.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";

            ComponentArt.Web.UI.TreeViewNode topTempNode = CreateRootNode(GetResource("Module_Node3"), "-1", CompanyID + "|" + UserID, true);
            FolderScript1.Nodes.Add(topTempNode);

            LoadTree(topTempNode);
        }


        /// <summary>
        /// 트리 바인딩
        /// </summary>
        private void LoadTree(ComponentArt.Web.UI.TreeViewNode topTempNode)
        {
            FolderScript1.CollapseNodeOnSelect = false;
            FolderScript1.ExpandNodeOnSelect = false;
            FolderScript1.MultipleSelectEnabled = false;
            FolderScript1.DragAndDropEnabled = false;


            DataSet ds = new DataSet();

            DAL.Folder bll = new Zeospace.DeskPlus.PMS.DAL.Folder();
            ds = bll.GetFolder(CompanyID, 0);

            if (ds != null)
            {
                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow, false);
                    topTempNode.Nodes.Add(node);

                    //CreateOneStepNode(Convert.ToInt32(dbRow["FolderID"]), node);
                }
            }
        }

        #endregion

        #region Method (함수)

        
        #region 트리 노드 생성
        /// <summary>
        /// 최상위 트리 노드 생성 : Creates the root node.
        /// </summary>
        /// <param name="_text">_text.</param>
        /// <param name="_id">_id.</param>
        /// <param name="_value">_value.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(string _text, string _id, string _value, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text.ToReverseXSS();				//목록명
            node.ID = _id;						//목록 고유값
            node.Value = _value;				//하위메뉴값						
            node.Expanded = _expanded;			//확장여부

            node.Style.Add("font-style", "bold");

            if (ViewState["UseIcon"] != null && (bool)ViewState["UseIcon"] == true)
                node.ImageUrl = "folders.gif";     // BaseImgPath + "/Component/TreeView/Root.gif";


            return node;
        }


        /// <summary>
        /// 트리노드 생성 : Creates the node.
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {
            string pFolderID = _dr["ParentFolderID"].ToString();
            pFolderID = (pFolderID.Length > 0) ? pFolderID : "0";

            string nodeText = this.GetLangName(
                                    _dr["FolderBase"].ToString(),
                                    _dr["FolderKo"].ToString(), _dr["FolderEn"].ToString(), _dr["FolderJa"].ToString(),
                                    _dr["FolderFr"].ToString(), _dr["FolderZhcn"].ToString(), _dr["FolderZhtw"].ToString(), PreLang).ToReverseXSS();


            

            //1. 카테고리값,  2. 타이틀           3. 하위메뉴값,             4. order            5. 첨부이미지본문표시유무 
            //6. 답변사용유무 7. 짧은답글사용유무 8. 조회자통계정보사용유무, 9. 부서게시판여부, 10. 권한여부(true/false,1:읽기/2:쓰기), 11, custom 페이지경로

            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = (!String.IsNullOrEmpty(nodeText)) ? nodeText : "No Title";
            node.ID = _dr["FolderID"].ToString();			//목록 고유값
            //node.Value							= nodeValue;
            node.Expanded = _expanded;

            Dictionary<string, string> paraDic = new Dictionary<string, string>(); 

            if (ViewState["UseIcon"] != null && (bool)ViewState["UseIcon"] == true)
            {
                node.ImageUrl = "folder.gif";    // BaseImgPath + "/Component/TreeView/Group.gif";
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
            sb.Append(_dr["FolderID"].ToString());
            sb.Append("|");
            sb.Append(nodeText);
            sb.Append("|");
            sb.Append(pFolderID); //하위메뉴값
            sb.Append("|");
            sb.Append(_dr["Orders"].ToString());
            sb.Append("|");
            sb.Append(GetMuiltiLang(
                        _dr["FolderKo"].ToString(),
                        _dr["FolderEn"].ToString(),
                        _dr["FolderJa"].ToString(),
                        _dr["FolderFr"].ToString(),
                        _dr["FolderZhcn"].ToString(),
                        _dr["FolderZhtw"].ToString()).ToReverseXSS());
            sb.Append("|");
            sb.Append(_dr["FolderBase"].ToString().ToReverseXSS());

            node.Value = sb.ToString();

            return node;
        }


        #endregion



        #endregion

        #region Property (프로퍼티)

        //// <summary>
        /// 회사 ID
        /// </summary>
        private int CompanyID
        {
            get { return GetViewStateInt("CompanyID", "0"); }
            set { ViewState["CompanyID"] = value; }
        }

        /// <summary>
        /// 사용자 ID
        /// </summary>
        private int UserID
        {
            get { return GetViewStateInt("UserID", "0"); }
            set { ViewState["UserID"] = value; }
        }


        /// <summary>
        /// 부서 ID
        /// </summary>
        private int DeptID
        {
            get { return GetViewStateInt("DeptID", "0"); }
            set { ViewState["DeptID"] = value; }
        }


        /// <summary>
        /// 겸직
        /// </summary>
        private string UserDepts
        {
            get { return GetViewStateString("UserDepts", ""); }
            set { ViewState["UserDepts"] = value; }
        }


        /// <summary>
        /// BoardTreeScriptView 프로퍼티
        /// </summary>
        public ComponentArt.Web.UI.TreeView CategoryTreeScriptView
        {
            get
            {
                return FolderScript1;
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