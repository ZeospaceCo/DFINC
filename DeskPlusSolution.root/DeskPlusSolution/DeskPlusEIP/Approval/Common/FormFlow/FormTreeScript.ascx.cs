using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Common.FormFlow
{
    public partial class FormTreeScript : Base.BaseControlApproval
    {
        #region 전역변수
        private string FolderAuth = "";
        private int Type1 = 1;  //공통
        private int Type2 = 2;  //자사
        private int Type3 = 3;  //연동
        #endregion

        #region Declaration (컨트롤 선언)

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.FormFlow.FormTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) LoadData();
        }
        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
        /// 목적 : 첫 페이지 초기값 설정
        /// </summary>
        private void LoadData()
        {
            #region 트리 환경 설정
            tvForm.Nodes.Clear();
            tvForm.CollapseNodeOnSelect = false;
            tvForm.ExpandNodeOnSelect = false;
            tvForm.MultipleSelectEnabled = false;
            tvForm.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvForm.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            tvForm.DragAndDropEnabled = false;

            tvForm.ClientSideOnNodeSelect = SetNodeSelect;
            tvForm.ClientSideOnCallbackComplete = SetCallbackComplete;

            #endregion

            //권한정보 가져오기
            DAL.FolderAuthority faDal = new DAL.FolderAuthority();
            FolderAuth = faDal.GetFolderAuthList(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID);

            #region 폴더정보 가져오기
            if (CurrentUserInfo.CompanyID > 1)
            {
                DAL.Folder fdal = new DAL.Folder();
                List<BE.FolderInfo> list = fdal.GetFolderRoot(CurrentUserInfo.CompanyID);

                if (list.Count > 0)
                {
                    List<BE.FolderInfo> listType1 = list.Where(x => x.TypeInfo.Equals("1")).ToList<BE.FolderInfo>();
                    if (listType1.Count > 0)
                    {
                        Type1 = listType1[0].FolderID;
                    }
                    List<BE.FolderInfo> listType2 = list.Where(x => x.TypeInfo.Equals("2")).ToList<BE.FolderInfo>();
                    if (listType2.Count > 0)
                    {
                        Type2 = listType2[0].FolderID;
                    }
                    List<BE.FolderInfo> listType3 = list.Where(x => x.TypeInfo.Equals("3")).ToList<BE.FolderInfo>();
                    if (listType3.Count > 0)
                    {
                        Type3 = listType3[0].FolderID;
                    }
                }
            }
            #endregion

            CreateFolder();

            #region 트리 카테고리에 포커스 설정하기
            //ComponentArt.Web.UI.TreeViewNode tvn = tvForm.FindNodeById(FolderID.ToString());
            //if (tvn != null) tvForm.SelectedNode = tvn;
            #endregion
        }

        #endregion

        #region Method (함수)

        #region 트리 노드 생성

        /// <summary>
        /// 작성자 : 
        /// 목적 : 1depth 폴더 생성하기
        /// </summary>
        private void CreateFolder()
        {
            DAL.Folder fDal = new DAL.Folder();
            DAL.Form foDal = new DAL.Form();

            DataSet dsFolder = null;
            DataSet dsForm = null;

            bool useCheckBox = UseCheckBox;

            var isactive = (UseAdmin) ? false : true;   //관리자페이지에서는 비활성화 폴더도 나옴.(ex.결재문서)

            #region 공통 양식 (folderid:1)

            if (UseSetFolder.IndexOf("1") != -1)
            {

                ComponentArt.Web.UI.TreeViewNode cNode = CreateNode(Type1.ToString(), "0", "C", "G", this.GetResource("FolderType1"), true, true, 1, useCheckBox);
                tvForm.Nodes.Add(cNode);

                #region 하위폴더
                dsFolder = fDal.GetSubFolderList("dsList", CurrentUserInfo.CompanyID, Type1, isactive);
                foreach (DataRow dr in dsFolder.Tables[0].Rows)
                {
                    ComponentArt.Web.UI.TreeViewNode tmpNode = CreateNode(
                        dr["FolderID"].ToString(), dr["ParentID"].ToString()
                        , "C", "G"
                        , GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString()
                        , dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang)
                        , false, true, dr["ChildCnt"].ToString().ToNull().ToNumber<int>(), useCheckBox);
                    cNode.Nodes.Add(tmpNode);
                }
                #endregion

                #region 해당양식
                dsForm = foDal.GetForm("dsFormList", CurrentUserInfo.CompanyID, Type1, UseActiveForm);
                foreach (DataRow dr in dsForm.Tables[0].Rows)
                {
                    ComponentArt.Web.UI.TreeViewNode tmpNode = CreateNode(
                        dr["FormID"].ToString(), dr["FolderID"].ToString()
                        , "C", "M"
                        , GetLangName(dr["FormBase"].ToString(), dr["FormKo"].ToString(), dr["FormEn"].ToString(), dr["FormJa"].ToString()
                        , dr["FormFr"].ToString(), dr["FormZhcn"].ToString(), dr["FormZhtw"].ToString(), PreLang)
                        , false, true, 1, useCheckBox);
                    cNode.Nodes.Add(tmpNode);
                }
                #endregion
            }

            #endregion

            dsFolder = null;
            dsForm = null;

            #region 자사 양식 (folderid:2)

            if (UseSetFolder.IndexOf("2") != -1)
            {
                ComponentArt.Web.UI.TreeViewNode gNode = CreateNode(Type2.ToString(), "0", "D", "G", this.GetResource("FolderType2"), true, true, 1, useCheckBox);
                tvForm.Nodes.Add(gNode);

                #region 하위폴더
                dsFolder = fDal.GetSubFolderList("dsList", CurrentUserInfo.CompanyID, Type2, isactive);
                string tmpauth = "";
                bool isauth;
                foreach (DataRow dr in dsFolder.Tables[0].Rows)
                {
                    tmpauth = "," + dr["FolderID"].ToString() + ",";
                    isauth = (FolderAuth.IndexOf(tmpauth) < 0) ? false : true;

                    ComponentArt.Web.UI.TreeViewNode tmpNode = CreateNode(
                        dr["FolderID"].ToString(), dr["ParentID"].ToString()
                        , "D", "G"
                        , GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang)
                        , false, isauth, dr["ChildCnt"].ToString().ToNull().ToNumber<int>(), useCheckBox);
                    gNode.Nodes.Add(tmpNode);
                }
                #endregion

                #region 해당양식
                isauth = (FolderAuth.IndexOf(",2,") < 0) ? false : true;
                if (isauth)
                {
                    dsForm = foDal.GetForm("dsFormList", CurrentUserInfo.CompanyID, Type2, UseActiveForm);
                    foreach (DataRow dr in dsForm.Tables[0].Rows)
                    {
                        ComponentArt.Web.UI.TreeViewNode tmpNode = CreateNode(
                            dr["FormID"].ToString(), dr["FolderID"].ToString()
                            , "D", "M"
                            , GetLangName(dr["FormBase"].ToString(), dr["FormKo"].ToString(), dr["FormEn"].ToString(), dr["FormJa"].ToString(), dr["FormFr"].ToString(), dr["FormZhcn"].ToString(), dr["FormZhtw"].ToString(), PreLang)
                            , false, isauth, 1, useCheckBox);
                        gNode.Nodes.Add(tmpNode);
                    }
                }
                #endregion

            }

            #endregion

            dsFolder = null;
            dsForm = null;

            #region 연동 양식 (folderid:3)

            if (UseSetFolder.IndexOf("3") != -1)
            {

                ComponentArt.Web.UI.TreeViewNode cNode = CreateNode(Type3.ToString(), "0", "L", "G", this.GetResource("FolderType3"), false, true, 1, useCheckBox);
                tvForm.Nodes.Add(cNode);

                #region 해당양식
                dsForm = foDal.GetForm("dsFormList", CurrentUserInfo.CompanyID, Type3, UseActiveForm);
                foreach (DataRow dr in dsForm.Tables[0].Rows)
                {
                    ComponentArt.Web.UI.TreeViewNode tmpNode = CreateNode(
                        dr["FormID"].ToString(), dr["FolderID"].ToString()
                        , "L", "M"
                        , GetLangName(dr["FormBase"].ToString(), dr["FormKo"].ToString(), dr["FormEn"].ToString(), dr["FormJa"].ToString()
                        , dr["FormFr"].ToString(), dr["FormZhcn"].ToString(), dr["FormZhtw"].ToString(), PreLang)
                        , false, true, 1, useCheckBox);
                    cNode.Nodes.Add(tmpNode);
                }
                #endregion
            }

            #endregion
        }

        /// <summary>
        /// 노드생성
        /// </summary>
        /// <param name="_id">아이디</param>
        /// <param name="_pid">부모아이디</param>
        /// <param name="_section">그룹핑노드정보(공통-C, 자사-D, 임시저장-T)</param>
        /// <param name="_type">폴더여부(폴더-G, 노드-M)</param>
        /// <param name="_name">노드명</param>
        /// <param name="_expanded">확장여부</param>
        /// <param name="_isAuth">권한여부</param>
        /// <param name="_childCnt">The _child CNT.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(string _id, string _pid, string _section, string _type, string _name, bool _expanded, bool _isAuth, int _childCnt, bool _useCheckbox)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _name.ToReverseXSS();
            node.ID = _section + "-" + _type + "-" + _id;              // 루트노드정보(Section) + '-' + 폴더/양식여부 + '-'  + FolderID/FormID

            node.Attributes["ID"] = _id;
            node.Attributes["PID"] = _pid;
            node.Attributes["Section"] = _section;
            node.Attributes["Type"] = _type;

            node.Expanded = _expanded;
            node.ShowCheckBox = (_type.Equals("M") && _useCheckbox) ? true : false;

            #region 권한에 따른 아이콘 처리
            if (_isAuth || UseAdmin)
            {
                if (_pid.Equals("0"))
                {
                    node.ImageUrl = "groups.gif";
                }
                else
                {
                    if (_type.ToUpper().Equals("G"))
                    {
                        node.ImageUrl = "folders.gif";
                        node.ExpandedImageUrl = "folders.gif";
                    }
                    else
                    {
                        node.ImageUrl = "form.gif";
                    }
                }
            }
            else
            {
                node.Selectable = false;
                node.ImageUrl = "folder-disable.gif";
                node.ExpandedImageUrl = "folder_open-disable.gif";
            }
            #endregion

            if (_type.ToUpper().Equals("G") && _childCnt > 0)
            {
                #region CallBack처리
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("guid", System.Guid.NewGuid().ToString());
                paraDic.Add("cID", CurrentUserInfo.CompanyID.ToString());
                paraDic.Add("pID", _id);
                paraDic.Add("section", _section);
                paraDic.Add("folderauth", FolderAuth);
                paraDic.Add("useActiveForm", (UseActiveForm) ? "true" : "false");
                paraDic.Add("useCheckBox", (UseCheckBox) ? "true" : "false");
                paraDic.Add("useAdmin", (UseAdmin) ? "true" : "false");     //관리자페이지에서는 비활성화 폴더도 나옴.(ex.결재문서)

                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                node.ContentCallbackUrl = "/DeskplusEIP/Approval/Common/FormFlow/GetFormTreeScript.aspx?p=" + encParams;
                #endregion
            }

            return node;
        }

        #endregion

        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// 사용할 양식 폴더 구분
        /// </summary>
        public string UseSetFolder
        {
            get { return GetViewStateString("UseSetFolder", ""); }
            set { ViewState["UseSetFolder"] = value; }
        }

        /// <summary>
        /// 관리자 여부
        /// </summary>
        public bool UseAdmin
        {
            get { return Convert.ToBoolean(GetViewStateString("UseAdmin", "false")); }
            set { ViewState["UseAdmin"] = value; }
        }

        /// <summary>
        /// 양식 활성화 보이기, 감추기(false : 둘다보이기; true : 비활성화 감추기)
        /// </summary>
        public bool UseActiveForm
        {
            get { return Convert.ToBoolean(GetViewStateString("UseActiveForm", "true")); }
            set { ViewState["UseActiveForm"] = value; }
        }

        /// <summary>
        /// CabinetID
        /// </summary>
        public string FolderID
        {
            get { return GetViewStateString("FolderID", ""); }
            set { ViewState["FolderID"] = value; }
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
        /// 체크박스 사용여부
        /// </summary>
        public bool UseCheckBox
        {
            get { return Convert.ToBoolean(GetViewStateString("UseCheckBox", "false")); }
            set { ViewState["UseCheckBox"] = value; }
        }


        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}