using System;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Category
{
    public partial class Tree : Base.BasePageReport
    {
        //우측메뉴에서 갱신 요청하면 1
        public string ReLoad = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드

            ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Category.Tree).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스 로드

            if (!IsPostBack)
            {
                ReLoad = GetDecryptParam("reLoad", "");
                LoadData();
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 첫 페이지 초기값 설정
        /// </summary>
        private void LoadData()
        {
            #region 트리 환경 설정

            tvTree.Nodes.Clear();
            tvTree.CollapseNodeOnSelect = false;
            tvTree.ExpandNodeOnSelect = false;
            tvTree.MultipleSelectEnabled = false;
            tvTree.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvTree.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            tvTree.DragAndDropEnabled = false;

            tvTree.ClientSideOnNodeSelect = "onNodeSelect";
            tvTree.ClientSideOnCallbackComplete = "onCallbackComplete";

            #endregion 트리 환경 설정

            DAL.Cabinet dal = new DAL.Cabinet();
            DataSet ds = dal.GetCabinet(CurrentUserInfo.CompanyID);
            ds.Relations.Add("CabinetRelation", ds.Tables[0].Columns["CabinetID"], ds.Tables[0].Columns["ParentCabinetID"], false);

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                if (dbRow.IsNull("ParentCabinetID"))
                {
                    //treeview 에 노드 상위메뉴 생성(newNode)
                    ComponentArt.Web.UI.TreeViewNode newNode = CreateNode(dbRow, true);

                    this.tvTree.Nodes.Add(newNode);

                    //하위트리 만들기(datarow, treeviw)
                    PopulateSubNode(dbRow, newNode);
                }
            }

            #region 트리 카테고리에 포커스 설정하기

            //Request,Content/Request/List.aspx

            string cabinetID = hdnCabinetID.Value;
            if (GetDecryptParam("targetPage", "") != "") cabinetID = GetDecryptParam("targetPage", "");

            if (cabinetID.Length > 0)
            {
                cabinetID = cabinetID.Split(',')[0];
                ComponentArt.Web.UI.TreeViewNode tvn = this.tvTree.FindNodeById(cabinetID);
                if (tvn != null) this.tvTree.SelectedNode = tvn;
            }

            #endregion 트리 카테고리에 포커스 설정하기
        }

        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow">테이블의 열</param>
        /// <param name="_node">컴포넌트 노드</param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            //MenuRelation의 관계에 datarow 의 자식행을 가져오자
            foreach (DataRow childRow in _dbRow.GetChildRows("CabinetRelation"))
            {
                if (childRow["CabinetID"].ToString() == "AdminFolder")
                {
                    bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
                    bool isRoleReport = CurrentUserInfo.IsInRole("report");//report admin

                    if (isRoleAdmin || isRoleReport)
                    {
                        ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow, false);
                        _node.Nodes.Add(childhNode);
                        PopulateSubNode(childRow, childhNode);
                    }
                }
                else
                {
                    ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow, false);
                    _node.Nodes.Add(childhNode);

                    PopulateSubNode(childRow, childhNode);
                }
            }
        }

        /// <summary>
        /// 트리 노트 생성
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            var textValue = "";

            #region 다국어 이름 설정

            textValue = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

            #endregion 다국어 이름 설정

            DAL.List listDal = null;
            DAL.Cabinet cabinetDal = null;
            DataTableReader dtr = null;
            var tot = 0;
            var read = 0;
            var noread = 0;
            var i = 0;
            switch (_dr["CabinetID"].ToString())
            {
                case "PrivateList": //개인함
                    listDal = new DAL.List();
                    tot = listDal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", 0, "flag", "1", CurrentUserInfo.UserID);
                    tot += listDal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", 0, "flag", "2", CurrentUserInfo.UserID);
                    tot += listDal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", 0, "flag", "3", CurrentUserInfo.UserID);
                    textValue += "(" + tot + ")";
                    break;

                case "InformList": //보고함
                    listDal = new DAL.List();
                    DAL.Folder fDal = new DAL.Folder();
                    string folderList = "";
                    var dtrFolder = fDal.GetFolderByInform(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);
                    if (dtrFolder != null) { if (dtrFolder.HasRows) { while (dtrFolder.Read()) { folderList += dtrFolder["FolderID"].ToString() + ","; } } }
                    folderList = folderList.TrimEnd(',');
                    tot = listDal.GetListByFolder(CurrentUserInfo.CompanyID, folderList);
                    textValue += "(" + tot + ")";
                    break;

                case "DeptList": //부서함
                    cabinetDal = new DAL.Cabinet();
                    dtr = cabinetDal.GetCabinetCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "DeptList");
                    i = 0;
                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                if (i == 0) tot = dtr["CNT"].ToString().ToNumber<int>();
                                if (i == 1) read = dtr["CNT"].ToString().ToNumber<int>();

                                i++;
                            }
                        }
                    }

                    noread = tot - read;
                    textValue += "(" + noread.ToString() + "/" + tot.ToString() + ")";
                    break;

                case "ConfirmList": //확인요청함
                    cabinetDal = new DAL.Cabinet();
                    dtr = cabinetDal.GetCabinetCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "ConfirmList");
                    i = 0;
                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                if (i == 0) tot = dtr["CNT"].ToString().ToNumber<int>();
                                if (i == 1) read = dtr["CNT"].ToString().ToNumber<int>();

                                i++;
                            }
                        }
                    }

                    noread = tot - read;
                    textValue += "(" + noread.ToString() + "/" + tot.ToString() + ")";
                    break;

                case "ShareList": //공유함
                    cabinetDal = new DAL.Cabinet();
                    dtr = cabinetDal.GetCabinetCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "ShareList");
                    i = 0;
                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                if (i == 0) tot = dtr["CNT"].ToString().ToNumber<int>();
                                if (i == 1) read = dtr["CNT"].ToString().ToNumber<int>();

                                i++;
                            }
                        }
                    }

                    noread = tot - read;
                    textValue += "(" + noread.ToString() + "/" + tot.ToString() + ")";
                    break;
            }

            node.Text = textValue;

            node.ID = _dr["CabinetID"].ToString();					//부서 ID
            node.Expanded = _expanded;								//확장여부
            node.Attributes["TargetPage"] = _dr["TargetPage"].ToString();
            node.ShowCheckBox = false;
            node.ImageUrl = "group.gif";

            if (_dr["ParentCabinetID"] == System.DBNull.Value)
            {
                node.ImageUrl = "groups.gif";
            }
            return node;
        }
    }
}