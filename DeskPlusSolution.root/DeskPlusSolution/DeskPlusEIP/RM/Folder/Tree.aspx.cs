using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.RM.Web.Folder
{
    public partial class Tree : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region 변수설정
        private int CompanyID;
        public string p_AccActive = "0"; //accordianActive
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Folder.Tree).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                #region 이미지 처리
                tvFolderTree.ImagesBaseUrl = this.BaseImgPath + "/Component/tree/";
                tvFolderTree.LineImagesFolderUrl = BaseImgPath + "/Component/tree/lines";
                #endregion

                #region 초기화
                LoadParam();
                InitTree();
                LoadAuth();
                #endregion

               
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-18
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            p_AccActive = GetDecryptParam("accActive", "0");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 트리
        /// </summary>
        public void InitTree()
        {
            tvFolderTree.CollapseNodeOnSelect = false;
            tvFolderTree.ExpandNodeOnSelect = false;
            tvFolderTree.MultipleSelectEnabled = false;

            // 회사 정보 가져오기            
            CompanyID = CurrentUserInfo.CompanyID;	//회사고유값

            ComponentArt.Web.UI.TreeViewNode rootnode = CreateRMNode();
            tvFolderTree.Nodes.Add(rootnode);

            DataSet ds = new DataSet();
            DAL.Category dal = new DAL.Category();

            ds = dal.GetTree(CompanyID);
            ds.Relations.Add("FolderRelation", ds.Tables[0].Columns["CategoryID"], ds.Tables[0].Columns["ParentCategoryID"], false);

            foreach (DataRow dbRow in ds.Tables["GetRMTree"].Rows)
            {
                if (dbRow.IsNull("ParentCategoryID"))
                {
                    //ComponentArt.Web.UI.TreeViewNode node = CreateRootNode(dbRow["CategoryID"].ToString(), dbRow);
                    //tvFolderTree.Nodes.Add(node);
                    SubFolder(dbRow["CategoryID"].ToString(), dbRow, rootnode);
                }
            }

            this.tvFolderTree.SelectedNode = this.tvFolderTree.Nodes[0];

            /*
            //페이지 시작시 노드 선택
            if (this.tvFolderTree.Nodes.Count > 0)
            {
                if (this.tvFolderTree.Nodes[0].Nodes.Count > 0)
                {
                    this.tvFolderTree.SelectedNode = this.tvFolderTree.Nodes[0].Nodes[0];
                }
            }
            */
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-12-18
        /// 작성목적 : 시스템관리자, 자원관리 관리자, 자원 주 담당자
        /// </summary>
        public void LoadAuth()
        {
            /*
            if (AccessAuth)
            {
                MenuAdmin.Visible = true;
                MenuSubAdmin.Visible = true;
            }
            else
            {
            */
                DAL.List dal = new DAL.List();
                int tmpCnt = dal.GetListUser(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                if (tmpCnt > 0)
                {
                    MenuAdmin.Visible = true;
                    MenuSubAdmin.Visible = true;
                }
                else
                {
                    MenuAdmin.Visible = false;
                    MenuSubAdmin.Visible = false;
                }
            //}

            // 자원 주 담당자이면 대기자원 카운트 메뉴에 보여주기
            if (MenuAdmin.Visible)
            {
                DAL.Reverse redal = new DAL.Reverse();
                int totalCnt = redal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "N", "0", "", "", "", "");

                if (totalCnt < 1)
                {
                    mCnt.InnerText = "";
                    mSubCnt.InnerText = "";
                }
                else
                {
                    mCnt.InnerText = "(" + totalCnt + ")";
                    mSubCnt.InnerText = "(" + totalCnt + ")";
                }
            }
        }
        #endregion

        #region (함수)


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-28
        /// 작성목적 : 하위 트리 만들기 (내폴더)
        /// </summary>
        /// <param name="_dbRow"></param>
        /// <param name="_node"></param>
        private void SubFolder(string _categoryID, DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            //foreach (DataRow childRow in _dbRow.GetChildRows("FolderRelation"))
            //{
            ComponentArt.Web.UI.TreeViewNode childhNode = CreateRootNode(_categoryID, _dbRow);
            _node.Nodes.Add(childhNode);
            //}
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-28
        /// 작성목적 : 최상위 루트 트리 만들기
        /// </summary>
        /// <param name="_folderID"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRMNode()
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                node.ID = "0";
                node.Expanded = true;	//확장여부                
                node.Attributes["ParentCategoryID"] = "-2";
                node.ImageUrl = "folders.gif";
                node.Text = GetResource("FolderRoot");      

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CreateRMNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-21
        /// 작성목적 : 최상위 루트 트리 만들기
        /// </summary>
        /// <param name="_categoryID"></param>
        /// <param name="_dbRow"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(string _categoryID, DataRow _dr)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                    _dr["NameFr"].ToString(), _dr["NameZhcn"].ToString(), _dr["NameZhtw"].ToString(), PreLang);

                node.Text = name;
                node.ID = _categoryID; //목록 고유값
                node.Expanded = false;	//확장여부
                node.ImageUrl = "folder.gif";
                node.Attributes["ParentCategoryID"] = "-1";

                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                //자식 노드가 있는 경우
                if (_dr.GetChildRows("FolderRelation").Length > 0)
                {
                    paraDic.Add("PID", _dr["CategoryID"].ToString());
                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                    node.ContentCallbackUrl = "/DeskPlusEIP/RM/Folder/GetFolderTree.aspx?p=" + encParams;
                }

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("RM-CreateRootNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }



        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}