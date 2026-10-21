using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Common.Status
{
    public partial class FolderStatusAllTreeScript : Base.BaseControlApproval
    {
        #region Declaration (컨트롤 선언)

        private readonly Dictionary<string, int> folderCountCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 페이지 로드
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.Status.FolderStatusAllTreeScript).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion



            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) LoadData();
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 첫 페이지 초기값 설정
        /// </summary>
        private void LoadData()
        {
            #region 트리 환경 설정
            tvFolderStatus.Nodes.Clear();
            tvFolderStatus.CollapseNodeOnSelect = false;
            tvFolderStatus.ExpandNodeOnSelect = false;
            tvFolderStatus.MultipleSelectEnabled = false;
            tvFolderStatus.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvFolderStatus.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            tvFolderStatus.DragAndDropEnabled = false;

            tvFolderStatus.ClientSideOnNodeSelect = SetNodeSelect;
            tvFolderStatus.ClientSideOnCallbackComplete = SetCallbackComplete;

            #endregion

            //catUser = new DAL.CategoryUser();
            CreateFolder();


            #region 트리 카테고리에 포커스 설정하기
            ComponentArt.Web.UI.TreeViewNode tvn = tvFolderStatus.FindNodeById(this.FolderStatusID.ToString());
            if (tvn != null) tvFolderStatus.SelectedNode = tvn;
            #endregion
        }



        #endregion

        #region Method (함수)


        #region 트리 노드 생성

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 1depth 폴더 생성하기
        /// </summary>
        private void CreateFolder()
        {
            Zeospace.DeskPlus.Approval.DAL.FolderStatusAll dal = new DAL.FolderStatusAll();
            DataSet ds = dal.GetFolderTree(CurrentUserInfo.CompanyID);
            ds.Relations.Add("FolderStatusRelation", ds.Tables[0].Columns["FolderStatusID"], ds.Tables[0].Columns["ParentFolderStatusID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow.IsNull("ParentFolderStatusID"))
                {

                    //treeview 에 노드 상위메뉴 생성(newNode)
                    ComponentArt.Web.UI.TreeViewNode newNode = CreateRootNode(dbRow, true);

                    this.tvFolderStatus.Nodes.Add(newNode);

                    //하위트리 만들기(datarow, treeviw)
                    PopulateSubNode(dbRow, newNode);

                    /*
                    bool access = false;

                    if (this.SetFolderStatusList.Length > 0)
                    {
                        foreach (string listID in SetFolderStatusList.Split(','))
                        {
                            if (listID.ToLower() == dbRow["FolderStatusID"].ToString().ToLower()) access = true;
                        }
                    }

                    if (access)
                    {
                        //treeview 에 노드 상위메뉴 생성(newNode)
                        ComponentArt.Web.UI.TreeViewNode newNode = CreateRootNode(dbRow, true);

                        this.tvFolderStatus.Nodes.Add(newNode);

                        //하위트리 만들기(datarow, treeviw)
                        PopulateSubNode(dbRow, newNode);
                    }
                     * 
                     */
                }
            }
        }

        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow childRow in _dbRow.GetChildRows("FolderStatusRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateRootNode(childRow, true);
                _node.Nodes.Add(childhNode);

                PopulateSubNode(childRow, childhNode);
            }
        }


        /// <summary>
        /// 트리 노트 생성
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();
            node.Value = name;

            #region 다국어 이름 설정
            int cnt = GetCntFolder(_dr["FolderStatusID"].ToString().ToNull());
            name = (cnt > 0) ? name + "(" + cnt + ")" : name;

            node.Text = name;
            #endregion

            node.ID = _dr["FolderStatusID"].ToString();					//ID
            node.Expanded = _expanded;								//확장여부                        
            node.Attributes["TargetPage"] = _dr["TargetPage"].ToString();

            if (UseCheckBox == true) node.ShowCheckBox = (_dr["ParentFolderStatusID"] == System.DBNull.Value) ? false : true;
            node.ImageUrl = "groups.gif";

            return node;
        }


        /// <summary>
        /// Gets the CNT folder.
        /// </summary>
        /// <param name="_folderStatusID">The _folder status ID.</param>
        /// <returns></returns>
        private int GetCntFolder(string _folderStatusID)
        {
            if (folderCountCache.ContainsKey(_folderStatusID))
            {
                return folderCountCache[_folderStatusID];
            }

            int cnt = 0;
            int isBundle = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBundle", "0").ToNumber<int>();
            DAL.DocumentProcess dalDP = new DAL.DocumentProcess();
            DAL.DocumentProcessAll dalDPAll = new DAL.DocumentProcessAll();
            DAL.Document dalD = new DAL.Document();
            Zeospace.DeskPlus.Approval.DAL.CarboncopyAllData carboncopyAllData =
                new Zeospace.DeskPlus.Approval.DAL.CarboncopyAllData();

            switch (_folderStatusID)
            {
                case "Process": //진행중인 문서
                    cnt = dalDPAll.GetPageCountProcessAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "Complete": //완결문서
                    cnt = dalD.GetPageCountComplete(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "Related": //관계있는 문서
                    cnt = dalDPAll.GetPageCountRelatedAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "", "", "", "");
                    break;
                case "Approve": //결재대상문서
                    cnt = dalDPAll.GetPageCountApproveAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, isBundle, "", "", "", "");
                    break;
                case "Cooperate": //협조문서
                    cnt = dalDPAll.GetPageCountCooperateAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, isBundle, "", "", "", "");
                    break;
                case "Proxy": //대결문서
                    cnt = dalDP.GetPageCountProxy(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, isBundle, "", "", "", "");
                    break;
                case "Delay": //보류문서
                    cnt = dalDPAll.GetPageCountDelayAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "ReportDelay": //상신 보류문서
                    cnt = dalDPAll.GetPageCountReportDelayAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "Receive": //수신문서
                    cnt = dalDPAll.GetPageCountReceiveAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "", "", "", "");
                    break;
                case "ReceiveDept": //부서 수신문서
                    cnt = dalDP.GetPageCountReceiveDept(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "", "", "", "");
                    break;
                case "Transfer": //이관문서
                    cnt = dalDP.GetPageCountTransfer(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "ReceiveReject": //수신반려문서
                    cnt = dalDP.GetPageCountReceiveReject(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "Afterward": //후열문서
                    cnt = dalDPAll.GetPageCountAfterwardAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "CarboncopyReceiveAll": //참조
                    cnt = carboncopyAllData.GetCarbonPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, 0, "", "", "", "", "Receive");
                    break;
                case "CarboncopyForwardReceiveAll": //전달
                    cnt = carboncopyAllData.GetForwardPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "", "Receive");
                    break;
                case "Confirm": // 공람문서
                    cnt = dalDP.GetPageCountConfirm(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "Reject": // 반려문서
                    cnt = dalDPAll.GetPageCountRejectAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    //.GetPageCountReject(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, startDate, endDate, SearchField, SearchValue);
                    break;
                case "ProcessSlip": //일괄결재 진행중인 문서
                    cnt = dalDP.GetPageCountProcessSlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "CompleteSlip": //일괄결재 완결문서
                    cnt = dalD.GetPageCountCompleteSlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "RejectSlip": //일괄결재 반려문서
                    cnt = dalD.GetPageCountRejectSlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "RelatedSlip": //일괄결재 관계있는 문서
                    cnt = dalDP.GetPageCountRelatedSlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "", "", "", "");
                    break;
                case "ApproveSlip": //일괄결재대상문서
                    cnt = dalDP.GetPageCountApproveSlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, isBundle, "", "", "", "");
                    break;
                case "DelaySlip": //일괄결재 보류문서
                    cnt = dalDP.GetPageCountDelaySlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "PersonalBox": //개인 문서함
                    cnt = 0;
                    break;
                case "PrivateCompleteAll": //완결(상신)
                    cnt = new DAL.PrivateDocument().GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "ApproveCompleteAll": //완결(결재)
                    cnt = new DAL.ApprovalDocument().GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "ApproveRejectAll": //반려(결재)
                    cnt = new DAL.DocumentProcessAll().GetPageCountApproveRejectAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "ArbitraryAll": //전결
                    cnt = new DAL.ArbitraryDocument().GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "CarboncopyAll": //참조
                    cnt = carboncopyAllData.GetCarbonPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, 0, "", "", "", "", "Complete");
                    break;
                case "CarboncopyForwardAll": //전달
                    cnt = carboncopyAllData.GetForwardPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "", "Complete");
                    break;
                case "CarboncopyCirReceiptAll": //받은회람
                    cnt = new DAL.CirculationDocument().GetPageCountRecv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "CarboncopyCirSendAll": //보낸회람
                    cnt = new DAL.CirculationDocument().GetPageCountSend(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "DeptBox": //부서 문서함
                    cnt = 0;
                    break;
                case "DeptCompleteAll": //완결
                    cnt = dalD.GetFormPageListCountByDeptID(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, 0, "", "", "", "", "", "");
                    break;
            }

            folderCountCache[_folderStatusID] = cnt;
            return cnt;
        }



        #endregion

        #endregion

        #region Property (프로퍼티)



        /// <summary>
        /// FolderStatusID
        /// </summary>
        /// <value>
        /// The folder status ID.
        /// </value>
        public string FolderStatusID
        {
            get { return GetViewStateString("FolderStatusID", ""); }
            set { ViewState["FolderStatusID"] = value; }
        }

        /// <summary>
        /// 체크박스 사용할것인지
        /// </summary>
        public bool UseCheckBox
        {
            get { return Convert.ToBoolean(GetViewStateString("UseCheckBox", "false")); }
            set { ViewState["UseCheckBox"] = value; }
        }


        /// <summary>
        /// 사용할 결재상황 폴더(구분자는 콤마)
        /// </summary>
        public string SetFolderStatusList
        {
            get { return GetViewStateString("SetFolderStatusList", ""); }
            set { ViewState["SetFolderStatusList"] = value; }
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
