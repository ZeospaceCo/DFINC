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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Common.FormFlow
{
    public partial class PrivateTree : Base.BaseControlApproval
    {
        #region 전역변수
        
        #endregion

        #region Declaration (컨트롤 선언)

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.FormFlow.PrivateTree).Assembly;	//어셈블리 선언
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
            tvPrivate.Nodes.Clear();
            tvPrivate.CollapseNodeOnSelect = false;
            tvPrivate.ExpandNodeOnSelect = false;
            tvPrivate.MultipleSelectEnabled = false;
            tvPrivate.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvPrivate.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            tvPrivate.DragAndDropEnabled = false;

            tvPrivate.ClientSideOnNodeSelect = SetNodeSelect;
            //tvPrivate.ClientSideOnCallbackComplete = SetCallbackComplete;

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
            if (UseSetFolder.IndexOf("1") != -1)
            {
                ComponentArt.Web.UI.TreeViewNode cNode1 = CreateNode("Temporary", "0", "P", "G", this.GetResource("TempFolder"), true);
                tvPrivate.Nodes.Add(cNode1);
            }
            if (UseSetFolder.IndexOf("2") != -1)
            {
                ComponentArt.Web.UI.TreeViewNode cNode2 = CreateNode("Submit", "0", "P", "G", this.GetResource("MenuSubmitFolder"), true);
                tvPrivate.Nodes.Add(cNode2);
            }
        }

        /// <summary>
        /// 노드생성
        /// </summary>
        /// <param name="_id">아이디</param>
        /// <param name="_pid">부모아이디</param>
        /// <param name="_section">그룹핑노드정보(공통-C, 자사-D, 임시저장-T, 결재망-N)</param>
        /// <param name="_type">폴더여부(폴더-G, 노드-M)</param>
        /// <param name="_name">노드명</param>
        /// <param name="_expanded">확장여부</param>
        /// <param name="_isAuth">권한여부</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(string _id, string _pid, string _section, string _type, string _name, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _name.ToReverseXSS();
            node.ID = _section + "-" + _type + "-" + _id;              // 루트노드정보(Section) + '-' + 폴더/양식여부 + '-'  + FolderID/FormID

            node.Attributes["ID"] = _id;
            node.Attributes["PID"] = _pid;
            node.Attributes["Section"] = _section;
            node.Attributes["Type"] = _type;

            node.Expanded = _expanded;
            node.ShowCheckBox = false;

            if (_pid.Equals("0"))
            {
                node.ImageUrl = "groups.gif";
            }
            else
            {
                if (_type.ToUpper().Equals("G"))
                {
                    node.ImageUrl = "folder.gif";
                    node.ExpandedImageUrl = "folder.gif";
                }
                else
                {
                    node.ImageUrl = "form.gif";
                }
            }

            //if (_type.ToUpper().Equals("G"))
            //{
            //    #region CallBack처리
            //    Dictionary<string, string> paraDic = new Dictionary<string, string>();
            //    paraDic.Add("guid", System.Guid.NewGuid().ToString());
            //    paraDic.Add("cID", CurrentUserInfo.CompanyID.ToString());
            //    paraDic.Add("pID", _id);
            //    paraDic.Add("section", _section);

            //    string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            //    node.ContentCallbackUrl = "/DeskplusEIP/Approval/Common/FormFlow/GetFormNetTreeScript.aspx?p=" + encParams;
            //    #endregion
            //}

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
        /// 트리 노드 선택시 호출할 스크립트 함수
        /// </summary>
        public string SetNodeSelect
        {
            get { return GetViewStateString("SetNodeSelect", ""); }
            set { ViewState["SetNodeSelect"] = value; }
        }
        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}