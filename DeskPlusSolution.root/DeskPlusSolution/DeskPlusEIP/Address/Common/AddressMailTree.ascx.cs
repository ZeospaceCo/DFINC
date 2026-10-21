using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Threading;
using System.Globalization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.Web.Common
{
    public partial class AddressMailTree : System.Web.UI.UserControl
    {
        #region 변수설정

        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }        

        #endregion 변수설정

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["PreLang"] != null)
            {
                PreLang = ViewState["PreLang"].ToString();
            }
            else if (Request.UserLanguages != null)
            {
                PreLang = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLang);
            }

            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Common.AddressMailTree).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                #region 리소스 처리

                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {
                }

                #endregion 리소스 처리

                #region 이미지 처리

                tvAddressTree.ImagesBaseUrl = this.BaseImgPath + "/Component/tree/";
                tvAddressTree.LineImagesFolderUrl = BaseImgPath + "/Component/tree/lines";

                #endregion 이미지 처리

                #region 초기화

                InitTree();

                #endregion 초기화
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
            Zeospace.DeskPlus.Address.DAL.Private bll = new DAL.Private();
            DataSet ds = bll.GetTree(CompanyID, "0", UserID, PreLang);

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

                    #endregion UseCategory 에 따라 검색 (작성자 심재운)

                    if (isUseCategory)
                    {
                        newNode = CreateNode(dbRow, true);
                        if (newNode != null)
                        {
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
            Zeospace.DeskPlus.Address.DAL.Private bll = new DAL.Private();
            DataSet ds = bll.GetTree(CompanyID, _parentCategoryID, UserID, PreLang);

            if (ds.Tables["GetAddressTree"] != null)
            {
                if (ds.Tables["GetAddressTree"].Rows.Count > 0)
                {
                    foreach (DataRow dbRow in ds.Tables[0].Rows)
                    {
                        ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow, false);
                        if (node != null) _parentNode.Nodes.Add(node);
                    }
                }
            }
        }

        #endregion DataLoad (컨트롤이 데이터를 바인딩 하는 메소드)

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

            string name = UtilHelper.GetLangName(
                _dr["NameBase"].ToString(),
                _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

            string titleName = UtilHelper.GetLangName(
                _dr["TitleBase"].ToString(),
                _dr["TitleKo"].ToString(), _dr["TitleEn"].ToString(), _dr["TitleJa"].ToString(),
                _dr["TitleFr"].ToString(), _dr["TitleZhCn"].ToString(), _dr["TitleZhTw"].ToString(), PreLang);

            #region 다국어 회사 설정

            string comName = UtilHelper.GetLangName(
                _dr["CompanyBase"].ToString(),
                _dr["CompanyKo"].ToString(), _dr["CompanyEn"].ToString(), _dr["CompanyJa"].ToString(),
                _dr["CompanyFr"].ToString(), _dr["CompanyZhCn"].ToString(), _dr["CompanyZhTw"].ToString(), PreLang);

            #endregion 다국어 회사 설정

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
                //'GM','TM','DM' 회사,직급,직책그룹 사용자중 deskplus 는 감춘다.
                //2013-03-21 : deskplus 계정은 조직도에서 보이지 않음 ( shimpark)
                if (_dr["UserID"].ToString() == "1" && (_dr["Type"].ToString() == "GM" || _dr["Type"].ToString() == "TM" || _dr["Type"].ToString() == "DM")) return null;

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
                paraDic.Add("cID", CompanyID.ToString());
                paraDic.Add("uID", UserID.ToString());
                paraDic.Add("prelang", PreLang);
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

                        node.ContentCallbackUrl = "/DeskPlusEIP/Address/Common/GetAddressMailTree.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
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

        #endregion 미사용

        #endregion Method (함수)

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

        /// <summary>
        /// 다국어
        /// </summary>
        /// <value>
        /// The use category.
        /// </value>
        public string PreLang
        {
            get { return ViewState["PreLang"].ToString(); }
            set
            {
                string lang = "";

                if (value == "")
                {
                    lang = Request.UserLanguages[0].ToLower();
                }
                else
                {
                    lang = value.ToLower();
                }

                if (lang == "zh" || lang == "zh-cn" || lang == "zh-hans" || lang == "zh-sg")  //zh-Hans(중국어간체)
                {
                    lang = "zh-cn";
                }
                else if (lang == "zh-tw" || lang == "zh-hant" || lang == "zh-mo")  //번체
                {
                    lang = "zh-tw";
                }
                else if (lang == "ja" || lang == "ja-jp") //일본어
                {
                    lang = "ja";
                }
                else if (lang == "ko" || lang == "ko-kr") //한국어
                {
                    lang = "ko";
                }
                else //나머지는 영어
                {
                    lang = "en";
                }
                value = lang;
                ViewState["PreLang"] = lang;
                //ViewState["PreLangs"] = value;
            }
        }


        /// <summary>
        /// CompanyID
        /// </summary>
        /// <value>
        /// The company identifier.
        /// </value>
        public int CompanyID
        {
            get { return GetViewStateInt("CompanyID", "0"); }
            set { ViewState["CompanyID"] = value; }
        }


        /// <summary>
        /// UserID
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserID
        {
            get { return GetViewStateInt("UserID", "0"); }
            set { ViewState["UserID"] = value; }
        }

        #endregion Property (프로퍼티)

        /// <summary>
        /// 작성자 : shimpark
        /// 암호화 문자열 반환하기
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        protected string GetEncryptParam(string data)
        {
            //빈값처리 Ajax에서는 빈값을 보낼경우 오류 발생 하므로 INULL 이라는 값으로 null 표시함
            if (String.IsNullOrEmpty(data)) return "!NULL";

            try
            {
                return Utility.EncryptString(Server.UrlEncode(data));
            }
            catch
            {
                return "!NULL";
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : VIEWSTATE STRING 형 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_viewStateName"></param>
        /// <param name="_errorReturn"></param>
        /// <returns></returns>
        private string GetViewStateString(string _viewStateName, string _errorReturn)
        {
            try
            {
                return (string)(ViewState[_viewStateName] ?? _errorReturn);
            }
            catch
            {
                return (ViewState[_viewStateName] != null) ? Convert.ToString(ViewState[_viewStateName]) : Convert.ToString(_errorReturn);
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : VIEWSTATE INT 형 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_viewStateName">Name of the _view state.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        private int GetViewStateInt(string _viewStateName, string _errorReturn)
        {
            try
            {
                return (int)(ViewState[_viewStateName] ?? Convert.ToInt32(_errorReturn));
            }
            catch
            {
                return (ViewState[_viewStateName] != null) ? Convert.ToInt32(ViewState[_viewStateName]) : Convert.ToInt32(_errorReturn);
            }
        }


        /// <summary>
        /// 다국어 리소스 인스턴스 메소드
        /// </summary>
        /// <remarks></remarks>
        private void GetResourceManager()
        {
            resMng = new System.Resources.ResourceManager(ResourceName, AssemblyName);
        }

        /// <summary>
        /// 다국어 리소스 값 반환 메소드
        /// </summary>
        /// <param name="_resName">Name of the _res.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetResource(string _resName)
        {
            return new System.Resources.ResourceManager(ResourceName, AssemblyName).GetString(_resName);
        }

        /// <summary>
        /// 공통 CSS
        /// </summary>
        public string BaseCssPath
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssPath", "/DeskPlusEIP/Design/C/Css"); }
        }

        /// <summary>
        /// 공통 Img
        /// </summary>
        public string BaseImgPath
        {
            get { return AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImgPath", "/DeskPlusEIP/Design/C/Img"); }
        }

        
    }
}