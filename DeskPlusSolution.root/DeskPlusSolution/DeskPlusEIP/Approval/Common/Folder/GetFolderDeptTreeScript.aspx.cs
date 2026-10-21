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
    public partial class GetFolderDeptTreeScript : Base.BasePageApproval
    {
        #region Declaration (컨트롤 선언)
        private string PID;
        private int FormID;
        private string YYYY;
        private string YYYYMM;
        private string UseCheckBox;
        private string Types;
        private string GroupID;
        private int TargetDeptID;
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
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Folder.GetFolderDeptTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            LoadParam();

            if (CurrentUserInfo.CompanyID > -1 && PID.Length > 0)
            {
                switch (PID.ToUpper())
                {
                    case "DEPT_DOC": //부서문서함 - 양식별
                        LoadDeptDocForm();
                        break;
                    case "DEPT_DOC_FORM_YY": //부서문서함 - 양식별 - 년도
                        LoadDeptDocFormYY();
                        break;
                    case "DEPT_DOC_FORM_YYMM": //부서문서함 - 양식별 - 년도 - 년월도
                        LoadDeptDocFormYYMM();
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
            TargetDeptID = GetDecryptParam("targetDeptID", "0").ToNumber<int>();
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
            DataTableReader dtr = dal.GetTreeDept(CurrentUserInfo.CompanyID, TargetDeptID, CurrentUserInfo.CodeTitleID, 0, "", "1");

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
            paraDic.Add("targetDeptID", TargetDeptID.ToString());
            encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            node.ContentCallbackUrl = "/DeskPlusEIP/Approval/Common/Folder/GetFolderDeptTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
            

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
            DataTableReader dtr = dal.GetTreeDept(CurrentUserInfo.CompanyID, TargetDeptID, CurrentUserInfo.CodeTitleID, FormID, "", "2");

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
            paraDic.Add("targetDeptID", TargetDeptID.ToString());
            encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            node.ContentCallbackUrl = "/DeskPlusEIP/Approval/Common/Folder/GetFolderDeptTreeScript.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;


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
            DataTableReader dtr = dal.GetTreeDept(CurrentUserInfo.CompanyID, TargetDeptID, CurrentUserInfo.CodeTitleID, FormID, YYYY, "3");

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

        #endregion

        #region Property (프로퍼티)


        #endregion
    }
}