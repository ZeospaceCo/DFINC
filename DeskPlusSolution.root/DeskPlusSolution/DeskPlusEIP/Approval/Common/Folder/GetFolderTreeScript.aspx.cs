using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections.Specialized;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Common.Folder
{
    public partial class GetFolderTreeScript : Base.BasePageApproval
    {
        #region Declaration (컨트롤 선언)
        private string PID;
        private int FormID;
        private string YYYY;
        private string YYYYMM;
        private string UseCheckBox;
        private string Types;
        private string GroupID;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Folder.GetFolderTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            LoadParam();

            if (CurrentUserInfo.CompanyID > -1 && PID.Length > 0)
            {
                switch (PID.ToUpper())
                {

                    case "DEPT_DOC": 
                        //부서별문서함 - 양식
                        LoadDeptDocForm();
                        break;
                    case "DEPT_DOC_FORM_YY":
                        //부서별문서함 - 양식 - 년도
                        LoadDeptDocFormYY();
                        break;
                    case "DEPT_DOC_FORM_YYMM":
                        //부서별문서함 - 양식 - 년도 - 년월
                        LoadDeptDocFormYYMM();
                        break;

                    case "DEPT_SEND":
                        //부서별문서함 - 발신문서함 - 양식
                        LoadDeptDocSendForm();
                        break;
                    case "DEPT_SEND_FORM_YY":
                        //부서별문서함 - 발신문서함 - 양식 - 년도
                        LoadDeptDocSendFormYY();
                        break;
                    case "DEPT_SEND_FORM_YYMM":
                        //부서별문서함 - 발신문서함 - 양식 - 년도 - 년월
                        LoadDeptDocSendFormYYMM();
                        break;

                    case "DEPT_RECEIPT":
                        //부서별문서함 - 수신문서함 - 양식
                        LoadDeptDocRecvForm();
                        break;
                    case "DEPT_RECEIPT_FORM_YY":
                        //부서별문서함 - 수신문서함 - 양식 - 년도
                        LoadDeptDocRecvFormYY();
                        break;
                    case "DEPT_RECEIPT_FORM_YYMM":
                        //부서별문서함 - 수신문서함 - 양식 - 년도 - 년월
                        LoadDeptDocRecvFormYYMM();
                        break;

                    case "DEPT_COMMON": 
                        //부서공용문서함
                        LoadDeptCom();
                        break;

                    case "PRIVATE_DOC": 
                        //개인별문서함 - 상신문서함
                        LoadPriDoc();
                        break;
                    case "PRIVATE_FOLDER": 
                        //개인별문서함 - 개인문서함
                        LoadPriFolder();
                        break;

                    case "APPROVAL_DOC": 
                        //완결결재문서함 - 양식
                        LoadAppDoc();
                        break;
                    case "APPROVAL_FOLDER": 
                        //완결결재문서함 - 개인문서함
                        LoadAppFolder();
                        break;

                    case "REJECT_DOC": 
                        //반려문서함 - 양식
                        LoadRejectDoc();
                        break;

                    case "REJECT_FOLDER": 
                        //반려문서함 - 개인문서함
                        LoadRejectFolder();
                        break;

                    case "ARBITRARY_DOC": 
                        //전결문서함 - 양식별
                        LoadArbDoc();
                        break;
                    case "ARBITRARY_FOLDER": 
                        //전결문서함 - 개인별문서함
                        LoadArbFolder();
                        break;

                    case "CARBONCOPY_DOC": 
                        //참조문서함 - 참조문서함
                        LoadCarbonFolder(PID);
                        break;
                    case "CARBONCOPY_FORWARD": 
                        //참조문서함 - 전달문서함
                        LoadCarbonFolder(PID);
                        break;
                    case "CARBONCOPY_CIRRECEIPT": 
                        //참조문서함 - 받은 회람함
                        LoadCarbonFolder(PID);
                        break;
                    case "CARBONCOPY_CIRSEND": 
                        //참조문서함 - 보낸 회람함
                        LoadCarbonFolder(PID);
                        break;

                }
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            PID = GetDecryptParam("pID", "");
            UseCheckBox = GetDecryptParam("useCheckBox", "0");
            FormID = GetDecryptParam("formID", "0").ToNumber<int>();
            YYYY = GetDecryptParam("yyyy", "");
            YYYYMM = GetDecryptParam("yyyymm", "");
            Types = GetDecryptParam("types", "");
            GroupID = GetDecryptParam("groupID", "");
            TargetPage = GetDecryptParam("targetPage", "");
        }

        #endregion

        #region Method (함수)

        #region 부서별 문서함 / 양식

        /// <summary>
        /// 부서별분류함 / 양식 로드하기
        /// </summary>
        private void LoadDeptDocForm()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            //부서별분류함 / 양식 로드하기
            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeDept(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, 0, "", "1");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";

                    while (dtr.Read())
                    {
                        name = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(),
                                    dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(),
                                    dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(),
                                    dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(),
                                    PreLang);

                        ComponentArt.Web.UI.TreeViewNode node = CreateDeptDocFormNode(name, dtr["FormID"].ToString(), false);
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서별분류함 / 양식 트리 생성
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateDeptDocFormNode(string _name, string _formID, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.Text = _name.ToReverseXSS();
            node.ID = "dept_" + _formID.ToString();
            node.Expanded = _expanded;

            node.ImageUrl = "folder.gif";
            node.Style.Add("font-style", "bold");

            node.Attributes["FormID"] = _formID.ToString();
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = ""; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Dept_Doc_Form"; //문서양식들
            node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = GroupID; //그룹별

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            string encParams = "";
            paraDic.Add("pID", "Dept_Doc_Form_YY");
            paraDic.Add("useCheckBox", UseCheckBox);
            paraDic.Add("formID", _formID.ToString()); //양식id
            paraDic.Add("yyyy", YYYY); //년도별
            paraDic.Add("yyyymm", YYYYMM); //년월별
            paraDic.Add("types", Types);
            paraDic.Add("groupID", GroupID);
            paraDic.Add("targetPage", TargetPage);
            encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            node.ContentCallbackUrl = "/DeskPlusEIP/Approval/Common/Folder/GetFolderTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;


            return node;
        }

        #endregion

        #region 부서별 문서함 / 양식 / 년도별

        /// <summary>
        /// 부서별분류함 / 양식 / 년도별 로드하기
        /// </summary>
        private void LoadDeptDocFormYY()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            //부서별 문서함 / 양식 / 년도별
            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeDept(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, FormID, "", "2");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Number2yy : 년도별(2012)
                        ComponentArt.Web.UI.TreeViewNode node = CreateDeptDocFormYYNode(dtr["Number2yy"].ToString(), dtr["FormID"].ToString(), false);
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서별분류함 / 양식 / 년도별
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateDeptDocFormYYNode(string _name, string _formID, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.Text = _name.ToReverseXSS(); //년도
            node.ID = _formID + "_" + _name;
            node.Expanded = _expanded;

            node.ImageUrl = "folder.gif";
            node.Style.Add("font-style", "bold");

            node.Attributes["FormID"] = _formID.ToString();
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = _name; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Dept_Doc_Form_YY"; //문서양식들
            node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = GroupID; //그룹별

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            string encParams = "";
            paraDic.Add("pID", "Dept_Doc_Form_YYMM");
            paraDic.Add("useCheckBox", UseCheckBox);
            paraDic.Add("formID", _formID.ToString()); //양식id
            paraDic.Add("yyyy", _name); //년도별
            paraDic.Add("yyyymm", ""); //년월별
            paraDic.Add("types", Types);
            paraDic.Add("groupID", GroupID);
            paraDic.Add("targetPage", TargetPage);
            encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            node.ContentCallbackUrl = "/DeskPlusEIP/Approval/Common/Folder/GetFolderTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;


            return node;
        }

        #endregion

        #region 부서별 문서함 / 양식 / 년도별 / 년월별

        /// <summary>
        /// 부서별분류함 / 양식 / 년도별 / 년월별 로드하기
        /// </summary>
        private void LoadDeptDocFormYYMM()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            //부서별분류함 / 양식 / 년도별 / 년월별 로드하기
            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeDept(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, FormID, YYYY, "3");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Number2 : 년월별(201201)
                        ComponentArt.Web.UI.TreeViewNode node = CreateDeptDocFormYYMMNode(dtr["Number2"].ToString(), dtr["FormID"].ToString(), true);
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서별분류함 / 양식 / 년도별
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateDeptDocFormYYMMNode(string _name, string _formID, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.Text = _name.ToReverseXSS(); //년월
            node.ID = _formID + "_" + _name;
            node.Expanded = _expanded;

            node.ImageUrl = "folder.gif";
            node.Style.Add("font-style", "bold");

            node.Attributes["FormID"] = _formID.ToString();
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = YYYY; //년도별
            node.Attributes["YYYYMM"] = _name; //년월별
            node.Attributes["Gubun"] = "Dept_Doc_Form_YYMM"; //문서양식들
            node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = GroupID; //그룹별


            return node;
        }

        #endregion



        #region 부서별문서함 / 발신문서 / 양식별
        /// <summary>
        /// 부서별분류함 / 양식 로드하기
        /// </summary>
        private void LoadDeptDocSendForm()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            //부서별분류함 / 양식 로드하기
            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeDeptSend(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, 0, "", "1");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";

                    while (dtr.Read())
                    {
                        name = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(),
                                    dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(),
                                    dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(),
                                    dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(),
                                    PreLang);

                        ComponentArt.Web.UI.TreeViewNode node = CreateDeptDocSendFormNode(name, dtr["FormID"].ToString(), false);
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서별분류함 / 양식 트리 생성
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateDeptDocSendFormNode(string _name, string _formID, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.Text = _name.ToReverseXSS();
            node.ID = "dept_" + _formID.ToString();
            node.Expanded = _expanded;

            node.ImageUrl = "folder.gif";
            node.Style.Add("font-style", "bold");

            node.Attributes["FormID"] = _formID.ToString();
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = ""; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Dept_Send_Form"; //문서양식들
            node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = GroupID; //그룹별

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            string encParams = "";
            paraDic.Add("pID", "Dept_Send_Form_YY");
            paraDic.Add("useCheckBox", UseCheckBox);
            paraDic.Add("formID", _formID.ToString()); //양식id
            paraDic.Add("yyyy", YYYY); //년도별
            paraDic.Add("yyyymm", YYYYMM); //년월별
            paraDic.Add("types", Types);
            paraDic.Add("groupID", GroupID);
            paraDic.Add("targetPage", TargetPage);
            encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            node.ContentCallbackUrl = "/DeskPlusEIP/Approval/Common/Folder/GetFolderTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;


            return node;
        }
        #endregion

        #region 부서별문서함 / 발신문서 / 양식별 / 년도별
        /// <summary>
        /// 부서별분류함 / 양식 / 년도별 로드하기
        /// </summary>
        private void LoadDeptDocSendFormYY()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            //부서별 문서함 / 양식 / 년도별
            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeDeptSend(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, FormID, "", "2");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Number2yy : 년도별(2012)
                        ComponentArt.Web.UI.TreeViewNode node = CreateDeptDocSendFormYYNode(dtr["Number2yy"].ToString(), dtr["FormID"].ToString(), false);
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서별분류함 / 양식 / 년도별
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateDeptDocSendFormYYNode(string _name, string _formID, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.Text = _name.ToReverseXSS(); //년도
            node.ID = _formID + "_" + _name;
            node.Expanded = _expanded;

            node.ImageUrl = "folder.gif";
            node.Style.Add("font-style", "bold");

            node.Attributes["FormID"] = _formID.ToString();
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = _name; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Dept_Send_Form_YY"; //문서양식들
            node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = GroupID; //그룹별

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            string encParams = "";
            paraDic.Add("pID", "Dept_Send_Form_YYMM");
            paraDic.Add("useCheckBox", UseCheckBox);
            paraDic.Add("formID", _formID.ToString()); //양식id
            paraDic.Add("yyyy", _name); //년도별
            paraDic.Add("yyyymm", ""); //년월별
            paraDic.Add("types", Types);
            paraDic.Add("groupID", GroupID);
            paraDic.Add("targetPage", TargetPage);
            encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            node.ContentCallbackUrl = "/DeskPlusEIP/Approval/Common/Folder/GetFolderTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;


            return node;
        }
        #endregion

        #region 부서별문서함 / 발신문서 / 양식별 / 년도별 / 년월별
        /// <summary>
        /// 부서별분류함 / 양식 / 년도별 / 년월별 로드하기
        /// </summary>
        private void LoadDeptDocSendFormYYMM()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            //부서별분류함 / 양식 / 년도별 / 년월별 로드하기
            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeDeptSend(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, FormID, YYYY, "3");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Number2 : 년월별(201201)
                        ComponentArt.Web.UI.TreeViewNode node = CreateDeptDocSendFormYYMMNode(dtr["Number2"].ToString(), dtr["FormID"].ToString(), true);
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서별분류함 / 양식 / 년도별
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateDeptDocSendFormYYMMNode(string _name, string _formID, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.Text = _name.ToReverseXSS(); //년월
            node.ID = _formID + "_" + _name;
            node.Expanded = _expanded;

            node.ImageUrl = "folder.gif";
            node.Style.Add("font-style", "bold");

            node.Attributes["FormID"] = _formID.ToString();
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = YYYY; //년도별
            node.Attributes["YYYYMM"] = _name; //년월별
            node.Attributes["Gubun"] = "Dept_Send_Form_YYMM"; //문서양식들
            node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = GroupID; //그룹별


            return node;
        }
        #endregion



        #region 부서별문서함 / 수신문서 / 양식별
        /// <summary>
        /// 부서별분류함 / 양식 로드하기
        /// </summary>
        private void LoadDeptDocRecvForm()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            //부서별분류함 / 양식 로드하기
            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeDeptRecv(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, 0, "", "1");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";

                    while (dtr.Read())
                    {
                        name = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(),
                                    dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(),
                                    dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(),
                                    dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(),
                                    PreLang);

                        ComponentArt.Web.UI.TreeViewNode node = CreateDeptDocRecvFormNode(name, dtr["FormID"].ToString(), false);
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서별분류함 / 양식 트리 생성
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateDeptDocRecvFormNode(string _name, string _formID, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.Text = _name.ToReverseXSS();
            node.ID = "dept_" + _formID.ToString();
            node.Expanded = _expanded;

            node.ImageUrl = "folder.gif";
            node.Style.Add("font-style", "bold");

            node.Attributes["FormID"] = _formID.ToString();
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = ""; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Dept_Receipt_Form"; //문서양식들
            node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = GroupID; //그룹별

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            string encParams = "";
            paraDic.Add("pID", "Dept_Receipt_Form_YY");
            paraDic.Add("useCheckBox", UseCheckBox);
            paraDic.Add("formID", _formID.ToString()); //양식id
            paraDic.Add("yyyy", YYYY); //년도별
            paraDic.Add("yyyymm", YYYYMM); //년월별
            paraDic.Add("types", Types);
            paraDic.Add("groupID", GroupID);
            paraDic.Add("targetPage", TargetPage);
            encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            node.ContentCallbackUrl = "/DeskPlusEIP/Approval/Common/Folder/GetFolderTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;


            return node;
        }
        #endregion

        #region 부서별문서함 / 수신문서 / 양식별 / 년도별
        /// <summary>
        /// 부서별분류함 / 양식 / 년도별 로드하기
        /// </summary>
        private void LoadDeptDocRecvFormYY()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            //부서별 문서함 / 양식 / 년도별
            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeDeptRecv(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, FormID, "", "2");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Number2yy : 년도별(2012)
                        ComponentArt.Web.UI.TreeViewNode node = CreateDeptDocRecvFormYYNode(dtr["Number2yy"].ToString(), dtr["FormID"].ToString(), false);
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서별분류함 / 양식 / 년도별
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateDeptDocRecvFormYYNode(string _name, string _formID, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.Text = _name.ToReverseXSS(); //년도
            node.ID = _formID + "_" + _name;
            node.Expanded = _expanded;

            node.ImageUrl = "folder.gif";
            node.Style.Add("font-style", "bold");

            node.Attributes["FormID"] = _formID.ToString();
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = _name; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Dept_Receipt_Form_YY"; //문서양식들
            node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = GroupID; //그룹별

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            string encParams = "";
            paraDic.Add("pID", "Dept_Receipt_Form_YYMM");
            paraDic.Add("useCheckBox", UseCheckBox);
            paraDic.Add("formID", _formID.ToString()); //양식id
            paraDic.Add("yyyy", _name); //년도별
            paraDic.Add("yyyymm", ""); //년월별
            paraDic.Add("types", Types);
            paraDic.Add("groupID", GroupID);
            paraDic.Add("targetPage", TargetPage);
            encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            node.ContentCallbackUrl = "/DeskPlusEIP/Approval/Common/Folder/GetFolderTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;


            return node;
        }
        #endregion

        #region 부서별문서함 / 발신문서 / 양식별 / 년도별 / 년월별
        /// <summary>
        /// 부서별분류함 / 양식 / 년도별 / 년월별 로드하기
        /// </summary>
        private void LoadDeptDocRecvFormYYMM()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            //부서별분류함 / 양식 / 년도별 / 년월별 로드하기
            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeDeptRecv(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, FormID, YYYY, "3");

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        //Number2 : 년월별(201201)
                        ComponentArt.Web.UI.TreeViewNode node = CreateDeptDocRecvFormYYMMNode(dtr["Number2"].ToString(), dtr["FormID"].ToString(), true);
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서별분류함 / 양식 / 년도별
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateDeptDocRecvFormYYMMNode(string _name, string _formID, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.Text = _name.ToReverseXSS(); //년월
            node.ID = _formID + "_" + _name;
            node.Expanded = _expanded;

            node.ImageUrl = "folder.gif";
            node.Style.Add("font-style", "bold");

            node.Attributes["FormID"] = _formID.ToString();
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = YYYY; //년도별
            node.Attributes["YYYYMM"] = _name; //년월별
            node.Attributes["Gubun"] = "Dept_Receipt_Form_YYMM"; //문서양식들
            node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = GroupID; //그룹별


            return node;
        }
        #endregion



        #region 부서별 공용 폴더

        /// <summary>
        /// 부서별 공용 폴더 로드하기
        /// </summary>
        private void LoadDeptCom()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            //부서별분류함 / 양식 로드하기
            Zeospace.DeskPlus.Approval.DAL.PublicFolder dal = new DAL.PublicFolder();
            DataSet ds = dal.GetFolder(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID);
            if (ds != null)
            {
                ds.Relations.Add("FolderRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentFolderID"], false);

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    if (dbRow.IsNull("ParentFolderID"))
                    {
                        //treeview 에 노드 상위메뉴 생성(newNode)
                        ComponentArt.Web.UI.TreeViewNode newNode = CreateDeptCom(dbRow, true);

                        TreeView1.Nodes.Add(newNode);

                        //하위트리 만들기(datarow, treeviw)
                        SubNodeDeptCom(dbRow, newNode);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }


        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        private void SubNodeDeptCom(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {

            foreach (DataRow childRow in _dbRow.GetChildRows("FolderRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateDeptCom(childRow, false);
                _node.Nodes.Add(childhNode);

                SubNodeDeptCom(childRow, childhNode);
            }
        }


        /// <summary>
        /// 트리 노트 생성
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateDeptCom(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            #region 다국어 이름 설정
            node.Text = this.GetLangName(_dr["FolderBase"].ToString(), _dr["FolderKo"].ToString(), _dr["FolderEn"].ToString(), _dr["FolderJa"].ToString(),
                _dr["FolderFr"].ToString(), _dr["FolderZhCn"].ToString(), _dr["FolderZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            node.ID = _dr["FolderID"].ToString();					//ID
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["FormID"] = "0";
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = ""; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Dept_Common"; //구분자
            node.Attributes["Types"] = _dr["Types"].ToString(); //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = _dr["GroupID"].ToString(); //그룹별

            if (UseCheckBox == "1") node.ShowCheckBox = (_dr["ParentFolderID"] == System.DBNull.Value) ? false : true;

            node.ImageUrl = (_dr["Types"].ToString().ToNull() == "S") ? "groups.gif" : "folder.gif";

            return node;
        }

        #endregion



        #region 개인별 문서함 / 상신문서함 (양식별)

        /// <summary>
        /// 부서별분류함 / 양식 로드하기
        /// </summary>
        private void LoadPriDoc()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();


            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreePrivate(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";

                    while (dtr.Read())
                    {
                        name = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(),
                                    dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(),
                                    dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(),
                                    dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(),
                                    PreLang);

                        ComponentArt.Web.UI.TreeViewNode node = CreatePriDoc(name, dtr["FormID"].ToString(), true);
                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 부서별분류함 / 양식 트리 생성
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreatePriDoc(string _name, string _formID, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.Text = _name.ToReverseXSS();
            node.ID = "PrivateDoc_" + _formID.ToString();
            node.Expanded = _expanded;

            node.ImageUrl = "folder.gif";
            node.Style.Add("font-style", "bold");

            node.Attributes["FormID"] = _formID.ToString();
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = ""; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Private_Doc_Form"; //문서양식들
            node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더     
            node.Attributes["GroupID"] = GroupID; //그룹별

            return node;
        }

        #endregion

        #region 개인별 문서함 / 개인별문서함

        /// <summary>
        /// 개인별 문서함 / 개인별문서함
        /// </summary>
        private void LoadPriFolder()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();


            Zeospace.DeskPlus.Approval.DAL.PrivateFolder dal = new DAL.PrivateFolder();
            DataSet ds = dal.GetFolder(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (ds != null)
            {
                ds.Relations.Add("PriFolderRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentFolderID"], false);

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    if (dbRow.IsNull("ParentFolderID"))
                    {
                        //treeview 에 노드 상위메뉴 생성(newNode)
                        ComponentArt.Web.UI.TreeViewNode newNode = CreatePriFolder(dbRow, true);

                        TreeView1.Nodes.Add(newNode);

                        //하위트리 만들기(datarow, treeviw)
                        SubNodePriFolder(dbRow, newNode);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }


        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        private void SubNodePriFolder(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {

            foreach (DataRow childRow in _dbRow.GetChildRows("PriFolderRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreatePriFolder(childRow, false);
                _node.Nodes.Add(childhNode);

                SubNodePriFolder(childRow, childhNode);
            }
        }


        /// <summary>
        /// 트리 노트 생성
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreatePriFolder(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            #region 다국어 이름 설정
            node.Text = this.GetLangName(_dr["FolderBase"].ToString(), _dr["FolderKo"].ToString(), _dr["FolderEn"].ToString(), _dr["FolderJa"].ToString(),
                _dr["FolderFr"].ToString(), _dr["FolderZhCn"].ToString(), _dr["FolderZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            node.ID = "PrivateFolder_" + _dr["FolderID"].ToString();					//ID
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["FormID"] = "0";
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = ""; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Private_Folder"; //구분자
            node.Attributes["Types"] = _dr["Types"].ToString(); //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = _dr["GroupID"].ToString(); //그룹별

            if (UseCheckBox == "1") node.ShowCheckBox = (_dr["ParentFolderID"] == System.DBNull.Value) ? false : true;

            node.ImageUrl = (_dr["Types"].ToString().ToNull() == "S") ? "groups.gif" : "folder.gif";

            return node;
        }

        #endregion



        #region 결재 문서함 / 완결결재문서함

        /// <summary>
        /// 결재 문서함 / 완결결재문서함 / 양식별
        /// </summary>
        private void LoadAppDoc()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeApp(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                        #region 다국어 이름 설정
                        node.Text = this.GetLangName(dtr["FormBase"].ToString(), dtr["FormKo"].ToString(), dtr["FormEn"].ToString(), dtr["FormJa"].ToString(),
                            dtr["FormFr"].ToString(), dtr["FormZhCn"].ToString(), dtr["FormZhTw"].ToString(), PreLang).ToReverseXSS();
                        #endregion

                        node.ID = "ApprovalDoc_" + dtr["FormID"].ToString();					//ID
                        node.Expanded = true;								//확장여부            
                        node.Attributes["FormID"] = dtr["FormID"].ToString();
                        node.Attributes["TargetPage"] = TargetPage;
                        node.Attributes["YYYY"] = ""; //년도별
                        node.Attributes["YYYYMM"] = ""; //년월별
                        node.Attributes["Gubun"] = "Approval_Doc"; //구분자
                        node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
                        node.Attributes["GroupID"] = GroupID; //그룹별

                        if (UseCheckBox == "1") node.ShowCheckBox = true;

                        node.ImageUrl = "folder.gif";

                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 완결결재문서함 / 개인폴더별
        /// </summary>
        private void LoadAppFolder()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();


            Zeospace.DeskPlus.Approval.DAL.ApprovalFolder dal = new DAL.ApprovalFolder();
            DataSet ds = dal.GetFolder(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (ds != null)
            {
                ds.Relations.Add("AppFolderRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentFolderID"], false);

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    if (dbRow.IsNull("ParentFolderID"))
                    {
                        //treeview 에 노드 상위메뉴 생성(newNode)
                        ComponentArt.Web.UI.TreeViewNode newNode = CreateAppFolder(dbRow, true);

                        TreeView1.Nodes.Add(newNode);

                        //하위트리 만들기(datarow, treeviw)
                        SubNodeAppFolder(dbRow, newNode);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        private void SubNodeAppFolder(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {

            foreach (DataRow childRow in _dbRow.GetChildRows("AppFolderRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateAppFolder(childRow, false);
                _node.Nodes.Add(childhNode);

                SubNodeAppFolder(childRow, childhNode);
            }
        }

        /// <summary>
        /// 완결결재문서함 / 개인폴더별 노드 생성
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateAppFolder(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            #region 다국어 이름 설정
            node.Text = this.GetLangName(_dr["FolderBase"].ToString(), _dr["FolderKo"].ToString(), _dr["FolderEn"].ToString(), _dr["FolderJa"].ToString(),
                _dr["FolderFr"].ToString(), _dr["FolderZhCn"].ToString(), _dr["FolderZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            node.ID = "ApprovalFolder_" + _dr["FolderID"].ToString();					//ID
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["FormID"] = "0";
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = ""; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Approval_Folder"; //구분자
            node.Attributes["Types"] = _dr["Types"].ToString(); //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = _dr["GroupID"].ToString(); //그룹별

            if (UseCheckBox == "1") node.ShowCheckBox = (_dr["ParentFolderID"] == System.DBNull.Value) ? false : true;

            node.ImageUrl = (_dr["Types"].ToString().ToNull() == "S") ? "groups.gif" : "folder.gif";

            return node;
        }

        #endregion

        #region 결재 문서함 / 대결문서함

        /// <summary>
        /// 결재 문서함 / 대결문서함 / 양식별
        /// </summary>
        private void LoadArbDoc()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeArb(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                        #region 다국어 이름 설정
                        node.Text = this.GetLangName(dtr["FormBase"].ToString(), dtr["FormKo"].ToString(), dtr["FormEn"].ToString(), dtr["FormJa"].ToString(),
                            dtr["FormFr"].ToString(), dtr["FormZhCn"].ToString(), dtr["FormZhTw"].ToString(), PreLang).ToReverseXSS();
                        #endregion

                        node.ID = "ArbitraryDoc_" + dtr["FormID"].ToString();					//ID
                        node.Expanded = true;								//확장여부            
                        node.Attributes["FormID"] = dtr["FormID"].ToString();
                        node.Attributes["TargetPage"] = TargetPage;
                        node.Attributes["YYYY"] = ""; //년도별
                        node.Attributes["YYYYMM"] = ""; //년월별
                        node.Attributes["Gubun"] = "Arbitrary_Doc"; //구분자
                        node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
                        node.Attributes["GroupID"] = GroupID; //그룹별

                        if (UseCheckBox == "1") node.ShowCheckBox = true;

                        node.ImageUrl = "folder.gif";

                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 결재 문서함 / 대결문서함 / 개인폴더별
        /// </summary>
        private void LoadArbFolder()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();


            Zeospace.DeskPlus.Approval.DAL.ArbitraryFolder dal = new DAL.ArbitraryFolder();
            DataSet ds = dal.GetFolder(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (ds != null)
            {
                ds.Relations.Add("ArbFolderRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentFolderID"], false);

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    if (dbRow.IsNull("ParentFolderID"))
                    {
                        //treeview 에 노드 상위메뉴 생성(newNode)
                        ComponentArt.Web.UI.TreeViewNode newNode = CreateArbFolder(dbRow, true);

                        TreeView1.Nodes.Add(newNode);

                        //하위트리 만들기(datarow, treeviw)
                        SubNodeArbFolder(dbRow, newNode);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        private void SubNodeArbFolder(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {

            foreach (DataRow childRow in _dbRow.GetChildRows("ArbFolderRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateAppFolder(childRow, false);
                _node.Nodes.Add(childhNode);

                SubNodeArbFolder(childRow, childhNode);
            }
        }

        /// <summary>
        /// 결재 문서함 / 대결문서함 / 개인폴더별 노드 생성
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateArbFolder(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            #region 다국어 이름 설정
            node.Text = this.GetLangName(_dr["FolderBase"].ToString(), _dr["FolderKo"].ToString(), _dr["FolderEn"].ToString(), _dr["FolderJa"].ToString(),
                _dr["FolderFr"].ToString(), _dr["FolderZhCn"].ToString(), _dr["FolderZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            node.ID = "ArbitraryFolder_" + _dr["FolderID"].ToString();					//ID
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["FormID"] = "0";
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = ""; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = "Arbitrary_Folder"; //구분자
            node.Attributes["Types"] = _dr["Types"].ToString(); //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = _dr["GroupID"].ToString(); //그룹별

            if (UseCheckBox == "1") node.ShowCheckBox = (_dr["ParentFolderID"] == System.DBNull.Value) ? false : true;

            node.ImageUrl = (_dr["Types"].ToString().ToNull() == "S") ? "groups.gif" : "folder.gif";

            return node;
        }

        #endregion



        #region 참조문서함 / 참조문서함,전달문서함,받은회람함,보낸회람함

        /// <summary>
        /// 참조문서함 / 참조문서함,전달문서함,받은회람함,보낸회람함 가져오기
        /// </summary>
        private void LoadCarbonFolder(string _gubun)
        {
            string name = "";
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();
            DataTableReader dtr = null;

            switch (_gubun.ToUpper())
            {
                case "CARBONCOPY_DOC": //참조
                    Zeospace.DeskPlus.Approval.DAL.Document dalCarbon = new DAL.Document();
                    dtr = dalCarbon.GetTreeCarbonCopy(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);
                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                name = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(),
                                    dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(),
                                    dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(),
                                    dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(),
                                    PreLang);


                                ComponentArt.Web.UI.TreeViewNode newNode = CreatCarbonFolder(name, dtr["FormID"].ToString(), "CarboncopyDoc", true);
                                TreeView1.Nodes.Add(newNode);
                            }


                        }
                    }
                    break;
                case "CARBONCOPY_FORWARD": //전달
                    Zeospace.DeskPlus.Approval.DAL.Document dalForward = new DAL.Document();
                    dtr = dalForward.GetTreeFoward(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                name = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(),
                                    dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(),
                                    dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(),
                                    dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(),
                                    PreLang);

                                ComponentArt.Web.UI.TreeViewNode newNode = CreatCarbonFolder(name, dtr["FormID"].ToString(), "CarboncopyForward", true);
                                TreeView1.Nodes.Add(newNode);
                            }
                        }
                    }
                    break;
                case "CARBONCOPY_CIRSEND": //보낸회람함
                    Zeospace.DeskPlus.Approval.DAL.CirculationDocument dalCirSend = new DAL.CirculationDocument();
                    dtr = dalCirSend.GetTreeSend(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                name = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(),
                                    dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(),
                                    dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(),
                                    dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(),
                                    PreLang);

                                ComponentArt.Web.UI.TreeViewNode newNode = CreatCarbonFolder(name, dtr["FormID"].ToString(), "CarboncopyCirSend", true);
                                TreeView1.Nodes.Add(newNode);
                            }
                        }
                    }
                    break;
                case "CARBONCOPY_CIRRECEIPT": //받은회람함
                    Zeospace.DeskPlus.Approval.DAL.CirculationDocument dalCirRecv = new DAL.CirculationDocument();
                    dtr = dalCirRecv.GetTreeRecv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                name = this.GetLangName(
                                    dtr["FormBase"].ToString().ToNull(),
                                    dtr["FormKo"].ToString().ToNull(),
                                    dtr["FormEn"].ToString().ToNull(),
                                    dtr["FormJa"].ToString().ToNull(),
                                    dtr["FormFr"].ToString().ToNull(),
                                    dtr["FormZhcn"].ToString().ToNull(),
                                    dtr["FormZhtw"].ToString().ToNull(),
                                    PreLang);

                                ComponentArt.Web.UI.TreeViewNode newNode = CreatCarbonFolder(name, dtr["FormID"].ToString(), "CarboncopyCirReceipt", true);
                                TreeView1.Nodes.Add(newNode);
                            }
                        }
                    }
                    break;
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 트리 노트 생성
        /// </summary>
        /// <param name="_nodeName">Name of the _node.</param>
        /// <param name="_formID">The _form ID.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreatCarbonFolder(string _nodeName, string _formID, string _gubun, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _nodeName.ToReverseXSS();

            node.ID = _gubun + "_" + _formID;					//ID
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["FormID"] = _formID;
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = ""; //년도별
            node.Attributes["YYYYMM"] = ""; //년월별
            node.Attributes["Gubun"] = _gubun; //구분자
            node.Attributes["Types"] = "S"; //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = "Carboncopy"; //그룹별

            if (UseCheckBox == "1") node.ShowCheckBox = true;

            node.ImageUrl = "folder.gif";

            return node;
        }

        #endregion


        #region 결재문서함 / 반려문서함

        /// <summary>
        /// 개인별 문서함 / 개인별문서함
        /// </summary>
        private void LoadRejectDoc()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            Zeospace.DeskPlus.Approval.DAL.Document dal = new DAL.Document();
            DataTableReader dtr = dal.GetTreeReject(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                        #region 다국어 이름 설정
                        node.Text = this.GetLangName(dtr["FormBase"].ToString(), dtr["FormKo"].ToString(), dtr["FormEn"].ToString(), dtr["FormJa"].ToString(),
                            dtr["FormFr"].ToString(), dtr["FormZhCn"].ToString(), dtr["FormZhTw"].ToString(), PreLang).ToReverseXSS();
                        #endregion

                        node.ID = "RejectDoc_" + dtr["FormID"].ToString();					//ID
                        node.Expanded = true;								//확장여부            
                        node.Attributes["FormID"] = dtr["FormID"].ToString();
                        node.Attributes["TargetPage"] = TargetPage;
                        node.Attributes["YYYY"] = ""; //년도별
                        node.Attributes["YYYYMM"] = ""; //년월별
                        node.Attributes["Gubun"] = "Approval_Doc"; //구분자
                        node.Attributes["Types"] = Types; //S : 기본폴더; U : 관리자추가폴더
                        node.Attributes["GroupID"] = GroupID; //그룹별

                        if (UseCheckBox == "1") node.ShowCheckBox = true;

                        node.ImageUrl = "folder.gif";

                        TreeView1.Nodes.Add(node);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 완결결재문서함 / 개인폴더별
        /// </summary>
        private void LoadRejectFolder()
        {
            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();


            Zeospace.DeskPlus.Approval.DAL.ApprovalFolderR dal = new DAL.ApprovalFolderR();
            DataSet ds = dal.GetFolder(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (ds != null)
            {
                ds.Relations.Add("AppFolderRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentFolderID"], false);

                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    if (dbRow.IsNull("ParentFolderID"))
                    {
                        //treeview 에 노드 상위메뉴 생성(newNode)
                        ComponentArt.Web.UI.TreeViewNode newNode = CreateRejectFolder(dbRow, true);

                        TreeView1.Nodes.Add(newNode);

                        //하위트리 만들기(datarow, treeviw)
                        SubNodeRejectFolder(dbRow, newNode);
                    }
                }
            }

            Response.Write(TreeView1.GetXml());
        }

        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        private void SubNodeRejectFolder(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {

            foreach (DataRow childRow in _dbRow.GetChildRows("AppFolderRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateAppFolder(childRow, false);
                _node.Nodes.Add(childhNode);

                SubNodeRejectFolder(childRow, childhNode);
            }
        }

        /// <summary>
        /// 완결결재문서함 / 개인폴더별 노드 생성
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRejectFolder(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            #region 다국어 이름 설정
            node.Text = this.GetLangName(_dr["FolderBase"].ToString(), _dr["FolderKo"].ToString(), _dr["FolderEn"].ToString(), _dr["FolderJa"].ToString(),
                _dr["FolderFr"].ToString(), _dr["FolderZhCn"].ToString(), _dr["FolderZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            node.ID = "RejectFolder_" + _dr["FolderID"].ToString();	//ID
            node.Expanded = _expanded;								//확장여부            
            node.Attributes["FormID"] = "0";
            node.Attributes["TargetPage"] = TargetPage;
            node.Attributes["YYYY"] = "";                           //년도별
            node.Attributes["YYYYMM"] = "";                         //년월별
            node.Attributes["Gubun"] = "Reject_Folder";           //구분자
            node.Attributes["Types"] = _dr["Types"].ToString(); //S : 기본폴더; U : 관리자추가폴더
            node.Attributes["GroupID"] = _dr["GroupID"].ToString(); //그룹별

            if (UseCheckBox == "1") node.ShowCheckBox = (_dr["ParentFolderID"] == System.DBNull.Value) ? false : true;

            node.ImageUrl = (_dr["Types"].ToString().ToNull() == "S") ? "groups.gif" : "folder.gif";

            return node;
        }
        #endregion

        

        #endregion

        #region Property (프로퍼티)


        #endregion
    }
}