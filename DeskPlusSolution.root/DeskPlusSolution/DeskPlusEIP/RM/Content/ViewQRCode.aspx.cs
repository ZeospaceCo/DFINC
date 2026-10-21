using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Threading;
using System.Globalization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;


namespace Zeospace.DeskPlus.RM.Web.Content
{
    public partial class ViewQRCode : System.Web.UI.Page
    {
        #region 변수설정
        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }
        private string decryString = string.Empty;//복고화 값
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UserLanguages != null)
            {
                PreLangs = Request.UserLanguages[0];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(PreLangs);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(PreLangs);
            }

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Content.ViewQRCode).Assembly;	//어셈블리 선언
            #endregion

            GetResourceManager();

            if (!IsPostBack)
            {
                SetLanguage();
                SetDecryptString();
                LoadData();
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            LoadResource();
            LoadListUser();
            LoadFile();
        }

        #endregion

        #region Method (함수)

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
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            /*
            ViewState["CompanyID"] = GetDecryptParam("companyID", "0").ToNumber<int>();
            ViewState["CategoryID"] = GetDecryptParam("categoryID", "");
            ViewState["ListID"] = GetDecryptParam("listID", "");
             */
            ViewState["CompanyID"] = 1;
            ViewState["CategoryID"] = 10;
            ViewState["ListID"] = 45;
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
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
        /// 작성자 : 심재운
        /// 다국어언어 간추려서 하나의 언어 반환하기
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_ko">The _ko.</param>
        /// <param name="_en">The _en.</param>
        /// <param name="_ja">The _ja.</param>
        /// <param name="_fr">The _FR.</param>
        /// <param name="_zhcn">The _ZHCN.</param>
        /// <param name="_zhtw">The _ZHTW.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        protected string GetLangName(string _name, string _ko, string _en, string _ja, string _fr,
            string _zhcn, string _zhtw, string _preLang)
        {
            return UtilHelper.GetLangName(_name, _ko, _en, _ja, _fr, _zhcn, _zhtw, _preLang);
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-08
        /// 작성목적 : 자원 정보 가져오기
        /// </summary>
        private void LoadResource()
        {
            DAL.List dal = new DAL.List();

            DataTableReader dtr = dal.GetListInfo(GetViewStateString("CompanyID", "0").ToNumber<int>(), GetViewStateString("ListID", "0").ToNumber<int>());

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    isExist.Visible = true;
                    noExist.Visible = false;

                    while (dtr.Read())
                    {
                        ltName.Text = this.GetLangName(dtr["CNameBase"].ToString(), dtr["CNameKo"].ToString(),
                                            dtr["CNameEn"].ToString(), dtr["CNameJa"].ToString(), dtr["CNameFr"].ToString(),
                                            dtr["CNameZhcn"].ToString(), dtr["CNameZhtw"].ToString(), PreLangs);

                        string title = dtr["Title"].ToString();
                        ltName.Text = title;
                        //ltCount.Text = dtr["Qty"].ToString();
                        ltCode.Text = dtr["Code"].ToString();
                        ltReCount.Text = dtr["UsableQty"].ToString();
                        ltPlace.Text = dtr["Place"].ToString();
                        ltDesc.Text = dtr["Content"].ToString();

                        if (dtr["Authority"].ToString() == "0")
                        {
                            ltAuth.Text = resMng.GetString("No");
                        }
                        else
                        {
                            ltAuth.Text = resMng.GetString("Yes");
                        }
                    }
                }
                else
                {
                    isExist.Visible = false;
                    noExist.Visible = true;
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-08
        /// 작성목적 : 자원 이미지 파일 가져오기
        /// </summary>
        private void LoadFile()
        {
            DAL.Attach dal = new DAL.Attach();

            DataSet ds = dal.GetQRCodeList(GetViewStateString("CompanyID", "0").ToNumber<int>(), GetViewStateString("ListID", "0").ToNumber<int>(), "1");

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 1)
                {
                    imgLeft.Visible = true;
                    imgRight.Visible = true;
                }
                else
                {
                    imgLeft.Visible = false;
                    imgRight.Visible = false;
                }

                string tmpImage = String.Empty;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    tmpImage += "<li><img src='" + dr["FilePath"].ToString() + dr["FileGUID"].ToString() + "' alt='img' style='height:160px;'/></li>";
                }

                phImage.InnerHtml = "<ul>" + tmpImage + "</ul>";
            }
            else
            {
                imgLeft.Visible = false;
                imgRight.Visible = false;

                phImage.InnerHtml = "<ul><li><img src='/DeskPlusEIP/Design/C/Img/Icon/ico_noimg2.gif' alt='img' style='height:160px;'/></li></ul>";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-09
        /// 작성목적 : 자원 담당자 정보 가져오기
        /// </summary>
        private void LoadListUser()
        {
            DAL.ListUser dal = new DAL.ListUser();
            DataTableReader dtr = dal.GetList(GetViewStateString("CompanyID", "0").ToNumber<int>(), GetViewStateString("ListID", "0").ToNumber<int>());

            string tmpText = "";
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        string managerName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                                            dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                            dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLangs);

                        string deptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(),
                                                            dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(),
                                                            dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLangs);

                        if (dtr["Gubun"].ToString() == "1")
                        {
                            tmpText += "<li><span>(" + resMng.GetString("ManagerGubun1") + ") </span>";
                        }
                        else
                        {
                            tmpText += "<li><span>(" + resMng.GetString("ManagerGubun2") + ") </span>";
                        }

                        tmpText += "<span>" + managerName + " </span>";
                        tmpText += "<span>" + deptName + "</span></li>";

                        /*     
                        UserContent += "<tr><td class='center'>" + managerName + "</td>";    
                        UserContent += "<td class='center'>" + deptName + "</td>";

                        if (dtr["Gubun"].ToString() == "1")
                        {
                            UserContent += "<td class='center'>" + resMng.GetString("ManagerGubun1") + "</td></tr>";
                        }
                        else
                        {
                            UserContent += "<td class='center'>" + resMng.GetString("ManagerGubun2") + "</td></tr>";
                        }
                        */
                    }
                }
            }

            spTitle.InnerHtml = tmpText;
        }


        #endregion

        #region Event Handler (이벤트 핸들러)


        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// 메서드명 : 
        /// 작성목적 : VIEWSTATE STRING 형 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_viewStateName"></param>
        /// <param name="_errorReturn"></param>
        /// <returns></returns>
        protected string GetViewStateString(string _viewStateName, string _errorReturn)
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
        /// 다국어
        /// </summary>
        /// <value>
        /// The pre langs.
        /// </value>
        private string PreLangs
        {
            get { return ViewState["PreLangs"].ToString(); }
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

                ViewState["PreLangs"] = lang;
                //ViewState["PreLangs"] = value; 
            }
        }

        /// <summary>
        /// UserContent
        /// </summary>
        public string UserContent
        {
            get { return GetViewStateString("UserContent", ""); }
            set { ViewState["UserContent"] = value; }
        }
        #endregion
    }
}