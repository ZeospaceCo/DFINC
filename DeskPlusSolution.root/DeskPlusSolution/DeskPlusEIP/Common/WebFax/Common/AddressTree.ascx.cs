using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Common.Web.WebFax.Common
{
    public partial class AddressTree : Base.BasControlCommon
    {

        #region 변수설정                
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.WebFax.Common.AddressTree).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                #region 리소스 처리
                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {

                }
                #endregion

                #region 이미지 처리
                tvAddressTree.ImagesBaseUrl = this.BaseImgPath + "/Component/tree/";
                tvAddressTree.LineImagesFolderUrl = BaseImgPath + "/Component/tree/lines";
                #endregion

                #region 초기화
                InitTree();
                #endregion
            }
            
        }

        #region DataLoad (컨트롤이 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Inits the tree.
        /// </summary>
        public void InitTree()
        {
            tvAddressTree.CollapseNodeOnSelect = false;
            tvAddressTree.ExpandNodeOnSelect = false;
            tvAddressTree.MultipleSelectEnabled = false;
            tvAddressTree.ClientSideOnNodeSelect = SetNodeSelect;
            tvAddressTree.ClientSideOnCallbackComplete = SetCallbackComplete;            
            
            //해더만 나옴.
            Zeospace.DeskPlus.Address.DAL.Private bll = new Address.DAL.Private();
            DataSet ds = bll.GetTree(CurrentUserInfo.CompanyID, "0", CurrentUserInfo.UserID, PreLang);

            string[] useCategoryStr = UseCategory.Split(',');
            bool isUseCategory = false;
            ComponentArt.Web.UI.TreeViewNode newNode = null;

            if (ds != null)
            {
                foreach (DataRow dbRow in ds.Tables["GetAddressTree"].Rows)
                {
                    isUseCategory = false;

                    #region UseCategory 에 따라 검색 (작성자 심재운)

                    string ownerID = dbRow["OwnerID"].ToString();
                    for (int i = 0; i < useCategoryStr.Length; i++)
                    {
                        if (ownerID.Equals(useCategoryStr[i].ToString()))
                        {
                            isUseCategory = true;
                            break;
                        }
                    }

                    #endregion

                    if (isUseCategory)
                    {
                        newNode = CreateNode(dbRow, true);
                        tvAddressTree.Nodes.Add(newNode);

                        if (UseCategory.IndexOf("GG") != -1 && ownerID.Equals("GG-1")) CreateNodeTree("GG-1", newNode);
                        if (UseCategory.IndexOf("CG") != -1 && ownerID.Equals("CG-2")) CreateNodeTree("CG-2", newNode);
                        if (UseCategory.IndexOf("PG") != -1 && ownerID.Equals("PG-3")) CreateNodeTree("PG-3", newNode);
                        if (UseCategory.IndexOf("RG") != -1 && ownerID.Equals("RG-4")) CreateNodeTree("RG-4", newNode);
                        if (UseCategory.IndexOf("TG") != -1 && ownerID.Equals("TG-5")) CreateNodeTree("TG-5", newNode);
                        if (UseCategory.IndexOf("FG") != -1 && ownerID.Equals("FG-6")) CreateNodeTree("FG-6", newNode);
                    }
                }
            }

            

            //패이지 시작시 노드 선택
            //this.tvAddressTree.SelectedNode = this.tvAddressTree.Nodes[0];
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 1 DEPTH 더 열기
        /// </summary>
        /// <param name="_parentCategoryID">The _parent category ID.</param>
        /// <param name="_parentNode">The _parent node.</param>
        private void CreateNodeTree(string _parentCategoryID, ComponentArt.Web.UI.TreeViewNode _parentNode)
        {
            Zeospace.DeskPlus.Address.DAL.Private bll = new Address.DAL.Private();
            DataSet ds = bll.GetTree(CurrentUserInfo.CompanyID, _parentCategoryID, CurrentUserInfo.UserID, PreLang);

            if (ds.Tables["GetAddressTree"] != null)
            {
                if (ds.Tables["GetAddressTree"].Rows.Count > 0)
                {
                    foreach (DataRow dbRow in ds.Tables[0].Rows)
                    {
                        ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow, false);
                        _parentNode.Nodes.Add(node);
                    }
                }
            }
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 이름 이외의 별도 추가 이름들(직급,부서,회사명)
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-12
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <returns></returns>
        private string NodeName(DataRow _dr)
        {            
            string type = _dr["Type"].ToString();
            string baseDept = _dr["BaseDept"].ToString();            
            string etcName = "";            
            string dsText = "";
            string baseDeptText = GetResource("Both");//(겸)

            string name = this.GetLangName(
                _dr["NameBase"].ToString(), 
                _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

            string titleName = this.GetLangName(
                _dr["TitleBase"].ToString(),
                _dr["TitleKo"].ToString(), _dr["TitleEn"].ToString(), _dr["TitleJa"].ToString(),
                _dr["TitleFr"].ToString(), _dr["TitleZhCn"].ToString(), _dr["TitleZhTw"].ToString(), PreLang);

            #region 다국어 회사 설정
            string comName = this.GetLangName(
                _dr["CompanyBase"].ToString(),
                _dr["CompanyKo"].ToString(), _dr["CompanyEn"].ToString(), _dr["CompanyJa"].ToString(),
                _dr["CompanyFr"].ToString(), _dr["CompanyZhCn"].ToString(), _dr["CompanyZhTw"].ToString(), PreLang);
            #endregion            

            //직급, 회사명이 존재할 경우
            if (titleName.Length > 0 || comName.Length > 0)
            {
                if (titleName.Length > 0 && comName.Length > 0)
                {
                    //둘다 존재할경우
                    etcName = "(" + titleName + "/" + comName + ")";
                }
                else
                {
                    //어느 하나 존재할 경우
                    etcName = (titleName.Length > 0) ? "(" + titleName + ")" : "";
                    etcName = (comName.Length > 0) ? "(" + comName + ")" : etcName;
                }
            }

            //겸직 0, 사용자 일경우
            if (baseDept == "0" && type.IndexOf("M") != -1)
            {
                dsText = name + baseDeptText + etcName;
            }
            else //겸직 x, 사용자 일경우
            {
                dsText = (type.IndexOf("M") != -1) ? name + etcName : name;
            }

            return dsText;
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 하위트리 만들기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-12
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {
            try
            {                
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
                string dsText = NodeName(_dr).ToReverseXSS();

                //최상위 노드 다국어 표현하기
                if (_dr["ParentOwnerID"] == System.DBNull.Value)
                {
                    switch (_dr["NameKo"].ToString())
                    {
                        case "Company":
                            node.Text = GetResource("CompanyAddress");  //"회사주소록";
                            break;
                        case "Common":
                            node.Text = GetResource("CommonAddress");   //"공용주소록";
                            break;
                        case "Private":
                            node.Text = GetResource("PrivateAddress");  //"개인주소록";
                            break;
                        case "Receive":
                            node.Text = GetResource("ForwardAddress");  //"전달받은 주소록";
                            break;
                        case "Code":
                            node.Text = GetResource("TitleAddress");    //"직급주소록";
                            break;
                        case "Favorite":
                            node.Text = GetResource("Favorite");        //"즐겨찾기";
                            break;
                        default:
                            node.Text = dsText;
                            break;
                    }

                    if (node.Text.Trim() == "" || String.IsNullOrEmpty(node.Text.Trim()))
                    {
                        node.Text = "no title";
                    }
                }
                else
                {
                    node.Text = dsText;
                }

                node.ID = _dr["OwnerID"].ToString(); //목록 고유값
                node.Expanded = _expanded;	//확장여부
                node.ShowCheckBox = false;  //체크박스 비활성화                

                if (UseIcon)
                {
                    if (
                        _dr["OwnerID"].ToString() == "GG-1" ||
                        _dr["OwnerID"].ToString() == "CG-2" ||
                        _dr["OwnerID"].ToString() == "PG-3" ||
                        _dr["OwnerID"].ToString() == "RG-4" ||
                        _dr["OwnerID"].ToString() == "TG-5" ||                        
                        _dr["OwnerID"].ToString() == "FG-6")
                    {
                        node.ImageUrl = "folders.gif";
                    }
                    else                        
                    {
                        if (_dr["Type"].ToString().Equals("FG"))
                        {
                            node.ImageUrl = "favorites.gif";
                        }
                        else
                        {
                            node.ImageUrl = (_dr["Type"].ToString().IndexOf("M") != -1) ? "member.gif" : "folder.gif";//아이콘 사용여부
                        }
                    }
                }

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("UseActiveDept", (UseActiveDept == true) ? "1" : "");
                paraDic.Add("UseIcon", (UseIcon == true) ? "1" : "");
                paraDic.Add("UseCheckBoxMember", (UseCheckBoxMember == true) ? "1" : "");
                paraDic.Add("UseCheckBoxGroup", (UseCheckBoxGroup == true) ? "1" : "");

                //부서명의 노드 일경우
                if (_dr["Type"].ToString().IndexOf("M") == -1)
                {
                    int childCnt = _dr["childCnt"].ToString().ToNumber<int>();

                    if (childCnt > 0)
                    {
                        string pId = (_dr["Type"].ToString() == "R") ? _dr["OwnerID"].ToString().Replace(_dr["Type"].ToString(), "") : _dr["OwnerID"].ToString();
                        paraDic.Add("PID", pId);                        
                        string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                        node.ContentCallbackUrl = "/DeskPlusEIP/Portlet/Package/SMS/Common/GetAddressTree.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
                    }
                }

                node.Attributes["CategoryID"] = _dr["CategoryID"].ToString();                
                node.Attributes["ParentOwnerID"] = _dr["ParentOwnerID"].ToString().Replace(_dr["Type"].ToString(), "");//하위메뉴값
                node.Attributes["Orders"] = _dr["Orders"].ToString();
                node.Attributes["Type"] = _dr["Type"].ToString();
                node.Attributes["Etc"] = _dr["Etc"].ToString();

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("Alert_Error"), "");
                throw;
            }
        }

        #region 미사용

        /// <summary>
        /// 최상위루트 트리 만들기
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_id"></param>
        /// <param name="_value"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(string _text, string _id, string _value, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text.ToReverseXSS();					//목록명
            node.ID = _id;						//목록 고유값
            node.Expanded = _expanded;			//확장여부

            /*
             * 1. CompanyID
             * 2. UserID
             * 3. parentOwnerID
             * 4. Orders
             * 5. Name,
             * 6. Type
             * 7. TitleID
             * 8. TitleName
             * 9. DutyID
             * 10. DutyName
             * 11. LoginID
             * 12. Mail
             * 13. Phone
             * 14. basedept
             */

            node.Value = "-1|-1|-1|-1|" + _text + "|-1|-1|-1|-1|-1|-1|-1|-1|-1";
            node.Style.Add("font-style", "bold");
            node.ShowCheckBox = false;

            if (UseIcon)
            {
                node.ImageUrl = "folder.gif";     // ImagePath + "/Component/TreeView/Root.gif";
            }

            return node;
        }

        #endregion

        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// 사용자및 부서의 활성화 보이기/감추기
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use active dept]; otherwise, <c>false</c>.
        /// </value>
        public bool UseActiveDept
        {
            get { return Convert.ToBoolean(ViewState["UseActiveDept"]); }
            set { ViewState["UseActiveDept"] = value; }
        }

        public bool UseIcon
        {
            get { return Convert.ToBoolean(ViewState["UseIcon"]); }
            set { ViewState["UseIcon"] = value; }
        }     

        public bool UseCheckBoxGroup
        {
            get { return Convert.ToBoolean(ViewState["UseCheckBoxGroup"]); }
            set { ViewState["UseCheckBoxGroup"] = value; }
        }

        public bool UseCheckBoxMember
        {
            get { return Convert.ToBoolean(ViewState["UseCheckBoxMember"]); }
            set { ViewState["UseCheckBoxMember"] = value; }
        }

        /// <summary>
        /// 사용할 카테고리값 (회사주소록, 공용주소록, 개인주소록, 전달받은 주소록,직급직책:GG-1,CG-2,PG-3,RG-4,TG-5)
        /// </summary>
        /// <value>The use category.</value>
        public string UseCategory
        {
            get
            {
                object o = ViewState["UseCategory"];
                if (o == null)
                    return "GG-1,CG-2,PG-3,RG-4,TG-5,FG-6";
                else
                    return ViewState["UseCategory"].ToString();
            }
            set
            {
                ViewState["UseCategory"] = value;
            }
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