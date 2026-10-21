using System;
using System.Collections.Generic;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.Web.Category
{
    public partial class Tree : Base.BasePageCoop
    {
        //우측메뉴에서 갱신 요청하면 1
        public string ReLoad = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드

            ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Category.Tree).Assembly;	//어셈블리 선언
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

            string cabinetID = hdnCabinetID.Value;
            if (GetDecryptParam("targetPage", "") != "") cabinetID = GetDecryptParam("targetPage", "");
            if (cabinetID.Length > 0) cabinetID = cabinetID.Split(',')[0];

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
                    PopulateSubNode(dbRow, newNode, cabinetID);
                }
            }

            #region 트리 카테고리에 포커스 설정하기

            if (cabinetID.Length > 0)
            {
                if (ItemID > 0) cabinetID = cabinetID + "^" + ItemID;
                //node.ID = _cabinetID + "^" + _itemID;
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
        /// <param name="_cabinetID">The _cabinet ID.</param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node, string _cabinetID)
        {
            //bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleCoop = CurrentUserInfo.IsInRole("coop");//coop admin
            bool isRoleCoopAll = CurrentUserInfo.IsInRole("coopall");//coop admin

            DAL.Item dal = new DAL.Item();
            var itemList = dal.GetItem(CurrentUserInfo.CompanyID, true);

            foreach (DataRow childRow in _dbRow.GetChildRows("CabinetRelation"))
            {
                bool expend = false;
                if (_cabinetID == childRow["CabinetID"].ToString()) expend = true;

                if (childRow["CabinetID"].ToString() == "Admin" ||
                    childRow["CabinetID"].ToString() == "AdminItem" ||
                    childRow["CabinetID"].ToString() == "AdminList") //관리자 화면
                {
                    if (isRoleCoop)
                    {
                        ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow, false);
                        _node.Nodes.Add(childhNode);
                        PopulateSubNode(childRow, childhNode, _cabinetID);
                    }
                }
                else if (childRow["CabinetID"].ToString() == "Total") //업무협업리스트조회권한
                {
                    if (isRoleCoop || isRoleCoopAll)
                    {
                        CreateItem(_node, itemList, childRow, expend);
                    }
                }
                else
                {
                    CreateItem(_node, itemList, childRow, expend);
                }
            }
        }

        /// <summary>
        /// 1차 분류 트리 생성
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_node">The _node.</param>
        /// <param name="itemList">The item list.</param>
        /// <param name="childRow">The child row.</param>
        /// <param name="_expend">if set to <c>true</c> [_expend].</param>
        private void CreateItem(ComponentArt.Web.UI.TreeViewNode _node, List<BE.ItemInfo> itemList, DataRow childRow, bool _expend)
        {
            ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow, _expend);
            _node.Nodes.Add(childhNode);

            //업무협업리스트조회권한만 하위에 양식 보여주기 (이노시티용)
            if (childRow["CabinetID"].ToString() == "Request" || childRow["CabinetID"].ToString() == "Receive" || childRow["CabinetID"].ToString() == "Total")
            {
                if (itemList != null)
                {
                    foreach (var item in itemList)
                    {
                        string name = this.GetLangName(
                            item.NameBase.ToNull(), item.NameKo.ToNull(),
                            item.NameEn.ToNull(), item.NameJa.ToNull(),
                            item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                            item.NameZhtw.ToNull(), PreLang);

                        string itemID = item.ItemID.ToString();

                        ComponentArt.Web.UI.TreeViewNode childItemNode = CreateItemNode(childRow["CabinetID"].ToString(),
                            childRow["TargetPage"].ToString(), itemID, name, _expend);

                        childhNode.Nodes.Add(childItemNode);
                    }
                }
            }
        }

        /// <summary>
        /// 대분류 노드 생성하기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_cabinetID">The _cabinet ID.</param>
        /// <param name="_targetPage">The _target page.</param>
        /// <param name="_itemID">The _item ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_expanded">if set to <c>true</c> [_expanded].</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateItemNode(string _cabinetID, string _targetPage,
            string _itemID, string _name, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.ID = _cabinetID + "^" + _itemID;
            node.Expanded = _expanded;
            node.ShowCheckBox = false;
            node.ImageUrl = "form.gif";

            node.Attributes["CabinetID"] = _cabinetID;
            node.Attributes["TargetPage"] = _targetPage;
            node.Attributes["ItemID"] = _itemID;
            node.Text = _name;

            return node;
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

            node.ID = _dr["CabinetID"].ToString();					//부서 ID
            node.Expanded = _expanded;								//확장여부
            node.Attributes["CabinetID"] = _dr["CabinetID"].ToString();
            node.Attributes["TargetPage"] = _dr["TargetPage"].ToString();
            node.Attributes["ItemID"] = "0";
            node.ShowCheckBox = false;
            node.ImageUrl = "group.gif";

            if (_dr["ParentCabinetID"] == System.DBNull.Value)
            {
                node.ImageUrl = "groups.gif";
            }

            string textValue = "";

            #region 다국어 이름 설정

            textValue = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

            if (node.ID == "Request") //요청협업
            {
                DAL.List listDal = new DAL.List();
                int tot = listDal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", "0", "", "", CurrentUserInfo.UserID, 0, 0);
                textValue += " (" + tot + ")";
            }
            else if (node.ID == "Total") //전체
            {
                DAL.List biz = new DAL.List();
                int tot = biz.GetTotPageListCount(CurrentUserInfo.CompanyID, "", "", "0", "", "", 0, 0);
                textValue += " (" + tot + ")";
            }
            else if (node.ID == "Receive") //요청받은협업
            {
                DAL.ListTO toDal = new DAL.ListTO();
                int tot = toDal.GetTotalCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                int today = toDal.GetTodayCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                int balance = tot - today;
                if (balance < 0) balance = 0;
                textValue += " (" + balance + "/" + tot + ")";
            }
            else if (node.ID == "Reference") //참조받은협업
            {
                DAL.ListCC ccDal = new DAL.ListCC();
                int tot = ccDal.GetTotalCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                int today = ccDal.GetTodayCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                int balance = tot - today;
                if (balance < 0) balance = 0;
                textValue += " (" + balance + "/" + tot + ")";
            }
            else if (node.ID == "Forward") //전달받은협업
            {
                DAL.ForwardListViewer fDal = new DAL.ForwardListViewer();
                DataTableReader fDtr = fDal.GetForwardListCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                string totVal = "0,0";
                if (fDtr != null)
                {
                    if (fDtr.HasRows)
                    {
                        while (fDtr.Read()) totVal = fDtr["TOT"].ToString();
                    }
                }

                int tot = totVal.Split(',')[0].ToNumber<int>();
                int today = totVal.Split(',')[1].ToNumber<int>();
                textValue += " (" + (tot - today) + "/" + tot + ")";
            }

            node.Text = textValue;

            #endregion 다국어 이름 설정

            return node;
        }
    }
}