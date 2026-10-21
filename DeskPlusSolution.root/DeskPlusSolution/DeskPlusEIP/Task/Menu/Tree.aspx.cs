using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Task.Web.Menu
{
    public partial class Tree : Base.BasePageTask
    {
        #region 변수설정
        public string p_AccActive = "0"; //accordianActive
        public bool isRoleTask = false;
        public string DefaultTab = "";
        public string hStatus = "";
        #endregion

        #region Page Process (페이지 처리)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Menu.Tree).Assembly;	            //어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            isRoleTask = CurrentUserInfo.IsInRole("taskall");//task admin

            if (isRoleTask)
            {
                // 기본탭
                Account.DAL.UserEnvironment envdal = new Account.DAL.UserEnvironment();
                List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = envdal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                if (list != null)
                {
                    foreach (Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo info in list)
                    {
                        DefaultTab = info.TaskHomeMenu;
                    }
                }

                if (DefaultTab == "")
                {
                    DefaultTab = "AdminMain";
                }
            }
            else
            {
                DefaultTab = "Main";
            }

            if (AccessAuth)
            {
                MenuAdmin.Visible = true;
                MenuSubAdmin.Visible = true;
            }
            else
            {
                MenuAdmin.Visible = false;
                MenuSubAdmin.Visible = false;
            }

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

            string cabinetID = "root";
            //if (!isRoleTask) cabinetID = "Request";
            if (GetDecryptParam("cabinetID", "") != "") cabinetID = GetDecryptParam("cabinetID", "");

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
                string tmpCabinetID = cabinetID;
                if (ItemID > 0) cabinetID = cabinetID + "^" + ItemID;
                ComponentArt.Web.UI.TreeViewNode tvn = this.tvTree.FindNodeById(cabinetID);
                if (tvn != null)
                {
                    this.tvTree.SelectedNode = tvn;
                }
                else
                {
                    ComponentArt.Web.UI.TreeViewNode tvn1 = this.tvTree.FindNodeById(tmpCabinetID);
                    this.tvTree.SelectedNode = tvn1;
                }
            }

            #endregion 트리 카테고리에 포커스 설정하기
        }

        /// <summary>
        /// 하위 트리 만들기
        /// </summary>
        /// <param name="_dbRow"></param>
        /// <param name="_node"></param>
        /// <param name="_cabinetID"></param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node, string _cabinetID)
        {

            DAL.Item dal = new DAL.Item();

            foreach (DataRow childRow in _dbRow.GetChildRows("CabinetRelation"))
            {
                bool expend = false;
                if (_cabinetID == childRow["CabinetID"].ToString()) expend = true;

                if (childRow["CabinetID"].ToString() == "Request")
                {
                    var itemList = dal.GetItem(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "RE");
                    CreateItem(_node, itemList, childRow, expend);
                }
                else if (childRow["CabinetID"].ToString() == "Receive")
                {
                    var itemList = dal.GetItem(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "TO");
                    CreateItem(_node, itemList, childRow, expend);
                }
                else if (childRow["CabinetID"].ToString() == "Total") 
                {
                    if (isRoleTask)
                    {
                        var itemList = dal.GetItem(CurrentUserInfo.CompanyID, true);
                        CreateItem(_node, itemList, childRow, expend);
                    }
                }                
                else
                {
                    CreateItem(_node, null, childRow, expend);
                }
            }
        }

        /// <summary>
        /// 대분류 가져오기
        /// </summary>
        /// <param name="_node"></param>
        /// <param name="itemList"></param>
        /// <param name="childRow"></param>
        /// <param name="_expend"></param>
        private void CreateItem(ComponentArt.Web.UI.TreeViewNode _node, List<BE.ItemInfo> itemList, DataRow childRow, bool _expend)
        {
            ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow, _expend);
            _node.Nodes.Add(childhNode);

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
        /// 대분류
        /// </summary>
        /// <param name="_cabinetID"></param>
        /// <param name="_targetPage"></param>
        /// <param name="_itemID"></param>
        /// <param name="_name"></param>
        /// <param name="_expanded"></param>
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
        /// <param name="_dr"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            node.ID = _dr["CabinetID"].ToString();					
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
                int tot = listDal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", "", "", "", CurrentUserInfo.UserID, 0, 0,0);
                textValue += " (" + tot + ")";                
            }
            else if (node.ID == "Receive") //요청받은협업
            {
                DAL.ListTO toDal = new DAL.ListTO();
                int tot = toDal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", "", "", "", CurrentUserInfo.UserID, 0, 0, 0);
                int totcnt = toDal.GetNoReadCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                if (totcnt < 0) totcnt = 0;
                node.Value = textValue + "," + totcnt.ToString() + "," + tot.ToString();
                textValue += " (" + totcnt + "/" + tot + ")";                
            }
            else if (node.ID == "Reference") //참조받은협업
            {
                DAL.ListCC ccDal = new DAL.ListCC();
                int tot = ccDal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", "", "", "", CurrentUserInfo.UserID, 0, 0,0);
                int totcnt = ccDal.GetNoReadCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                if (totcnt < 0) totcnt = 0;
                node.Value = textValue + "," + totcnt.ToString() + "," + tot.ToString();
                textValue += " (" + totcnt + "/" + tot + ")";                
            }
            else if (node.ID == "Forward") //전달받은협업
            {
                DAL.Forward fDal = new DAL.Forward();
                int tot = fDal.GetPageListCount(CurrentUserInfo.CompanyID, "", "", "", "", "", CurrentUserInfo.UserID, 0, 0,0);
                int totcnt = fDal.GetNoReadCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
                if (totcnt < 0) totcnt = 0;
                node.Value = textValue + "," + totcnt.ToString() + "," + tot.ToString();
                textValue += " (" + totcnt + "/" + tot + ")";
                
            }
            else if (node.ID == "Total") //전체
            {
                DAL.List biz = new DAL.List();
                int tot = biz.GetTotalPageListCount(CurrentUserInfo.CompanyID, "", "", "", "", "", CurrentUserInfo.UserID, 0, 0, "1",0);
                textValue += " (" + tot + ")";
            }
            
            node.Text = textValue;

            #endregion 다국어 이름 설정

            return node;
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-07
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            p_AccActive = GetDecryptParam("accActive", "0");
            hStatus = GetDecryptParam("hStatus", "");
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}