using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.Web.Common
{
    public partial class GetAddressMailTree : System.Web.UI.Page
    {
        #region Declaration (컨트롤 선언)

        protected System.Resources.ResourceManager resMng = null;

        public string ResourceName { get; set; }

        public Assembly AssemblyName { get; set; }

        private int CompanyID;
        private int UserID;
        private string PID;
        private string UseActiveDept;
        private string UseCheckBoxGroup;
        private string UseCheckBoxMember;
        private string UseIcon;

        private string decryString;

        #endregion Declaration (컨트롤 선언)

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, System.EventArgs e)
        {
            LoadParam();

            if (Request.UserLanguages != null)
            {
                PreLang = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLang);
            }

            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Common.GetAddressTree).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            #region 바인딩

            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            if (PID != "0")
            {
                try
                {
                    DataSet ds = new DataSet();

                    Zeospace.DeskPlus.Address.DAL.Private bll = new DAL.Private();
                    ds = bll.GetTree(CompanyID, PID.ToString(), UserID, PreLang);

                    if (ds.Tables["GetAddressTree"] != null)
                    {
                        foreach (DataRow dbRow in ds.Tables["GetAddressTree"].Rows)
                        {
                            if (UseActiveDept == "1")//사용자 및 부서의 활성화
                            {
                                if (dbRow["ActiveDept"].ToString() == "1")
                                {
                                    ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow, false);
                                    if (node != null) TreeView1.Nodes.Add(node);
                                }
                            }
                            else
                            {
                                ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow, false);
                                if (node != null) TreeView1.Nodes.Add(node);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                    throw;
                }

                Response.Clear();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                Response.Write(TreeView1.GetXml());
                Response.End();
            }

            #endregion 바인딩
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            SetDecryptString();

            CompanyID = GetDecryptParam("cID", "0").ToNumber<int>();
            UserID = GetDecryptParam("uID", "0").ToNumber<int>();
            PreLang = GetDecryptParam("prelang", "");
            PID = GetDecryptParam("PID", "");
            UseActiveDept = GetDecryptParam("UseActiveDept", "");
            UseCheckBoxGroup = GetDecryptParam("UseCheckBoxGroup", "");
            UseCheckBoxMember = GetDecryptParam("UseCheckBoxMember", "");
            UseIcon = GetDecryptParam("UseIcon", "");
        }

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
            string baseDeptText = GetResource("Both");   // (겸)

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

            if (titleName.Length > 0 || comName.Length > 0)
            {
                if (titleName.Length > 0 && comName.Length > 0)
                {
                    etcName = "(" + titleName + "/" + comName + ")";
                }
                else
                {
                    etcName = (titleName.Length > 0) ? "(" + titleName + ")" : "";
                    etcName = (comName.Length > 0) ? "(" + comName + ")" : etcName;
                }
            }

            //부서일 경우 / 사람일 경우 겸직 표시함
            if (baseDept == "0" && type.IndexOf("M") != -1)
            {
                dsText = name + baseDeptText + etcName;	//목록명
            }
            else
            {
                dsText = (type.IndexOf("M") != -1) ? name + etcName : name;
            }

            return dsText;
        }

        /// <summary>
        /// 하위트리 만들기
        /// </summary>
        /// <param name="_dr"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {
            try
            {
                //'GM','TM','DM' 회사,직급,직책그룹 사용자중 deskplus 는 감춘다.
                //2013-03-21 : deskplus 계정은 조직도에서 보이지 않음 ( shimpark)
                if (_dr["UserID"].ToString() == "1" && (_dr["Type"].ToString() == "GM" || _dr["Type"].ToString() == "TM" || _dr["Type"].ToString() == "DM")) return null;

                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
                node.Text = NodeName(_dr).ToReverseXSS();

                //node.Text	= _dr["Name"].ToString();				//목록명
                node.ID = _dr["OwnerID"].ToString();			//목록 고유값
                node.Expanded = _expanded;							//확장여부

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("cID", CompanyID.ToString());
                paraDic.Add("uID", UserID.ToString());
                paraDic.Add("prelang", PreLang);
                paraDic.Add("UseActiveDept", UseActiveDept);
                paraDic.Add("UseIcon", UseIcon);
                paraDic.Add("UseCheckBoxMember", UseCheckBoxMember);
                paraDic.Add("UseCheckBoxGroup", UseCheckBoxGroup);

                // 그룹일경우
                if (_dr["Type"].ToString().IndexOf("M") == -1)
                {
                    node.ShowCheckBox = false;

                    if (UseCheckBoxGroup == "1")
                    {
                        // 그룹에 대한 체크박스가 허용이 되어도 직급/직책에 대해서는 체크박스를 막는다.
                        node.ShowCheckBox = (_dr["OwnerID"].ToString() == "TG1" || _dr["OwnerID"].ToString() == "TG2") ? false : true;
                    }
                }
                else
                {
                    node.ShowCheckBox = (UseCheckBoxMember == "1") ? true : false;
                }

                if (UseIcon != "" && UseIcon == "1")
                {
                    if (_dr["ETC"].ToString().IndexOf("FG") != -1)
                    {
                        node.ImageUrl = BaseImgPath + "/Component/tree/favorites.gif";
                    }
                    else
                    {
                        node.ImageUrl = (_dr["Type"].ToString().IndexOf("M") == -1) ? BaseImgPath + "/Component/tree/folder.gif" : BaseImgPath + "/Component/tree/Member.gif";
                    }
                }
                else
                {
                    if (_dr["Type"].ToString().IndexOf("G") != -1)
                    {
                        node.Text = node.Text;
                        node.Style.Add("font-style", "bold");
                    }
                }

                //부서명의 노드 일경우
                if (_dr["Type"].ToString().IndexOf("M") == -1)
                {
                    int childCnt = _dr["childCnt"].ToString().ToNumber<int>();
                    if (childCnt > 0)
                    {
                        string pId = (_dr["Type"].ToString() == "R") ? _dr["OwnerID"].ToString().Replace(_dr["Type"].ToString(), "") : _dr["OwnerID"].ToString();

                        paraDic.Add("PID", pId);
                        string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                        node.ContentCallbackUrl = "/DeskPlusEIP/Address/Common/GetAddressTree.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CreateNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        #endregion Method (함수)

        /// <summary>
        /// 작성자 : shimpark
        /// 파리미터 암호화값 전역변수에 할당하기
        /// </summary>
        private void SetDecryptString()
        {
            string param = GetParm("p", "!NULL");
            if (!string.IsNullOrEmpty(param) && param != "!NULL")
            {
                param = Server.UrlDecode(param);//디코딩
                decryString = Utility.DecryptString(param);
                decryString = Server.UrlDecode(decryString);//디코딩
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : Request 파라미터 STRING 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected string GetParm(string _parmName, string _errorReturn)
        {
            try
            {
                return ((string)(Request.Params[_parmName] ?? _errorReturn) == "") ? _errorReturn : (string)(Request.Params[_parmName] ?? _errorReturn);
            }
            catch
            {
                return (Request.Params[_parmName] != null) ? Convert.ToString(Request.Params[_parmName]) : Convert.ToString(_errorReturn);
            }
        }

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
        /// 작성자   : shimpark
        /// 작성목적 : code behind 단에서 복고화 하여 파라미터값 가져오기
        /// </summary>
        /// <param name="_keyName">Name of the _key.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetDecryptParam(string _keyName, string _errorReturn)
        {
            if (!string.IsNullOrEmpty(decryString))
            {
                return UtilHelper.GetParmforParameter(decryString, _keyName, _errorReturn);
            }

            return _errorReturn;
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

        /// <summary>
        /// 다국어
        /// </summary>
        /// <value>
        /// The pre langs.
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
    }
}