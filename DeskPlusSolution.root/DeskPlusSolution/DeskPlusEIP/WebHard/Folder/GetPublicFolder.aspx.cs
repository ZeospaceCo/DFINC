using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections;

namespace Zeospace.DeskPlus.WebHard.Web.Folder
{
    public partial class GetPublicFolder : Base.BasePageWebHard
    {
        #region 변수처리
        public int FolderID;
        public string FileList;
        private int CompanyID;
        public int UserID;
        private string[] DeptID;
        public string Gubun;
        public string ListType;
        public string Order = String.Empty; //sort   

        public int PAdmin = 0;
        public string Types = string.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Folder.GetPublicFolder).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();

                LoadData();

                #region 이미지 처리
                tvFolderTree.ImagesBaseUrl = this.BaseImgPath + "/Component/tree/";
                tvFolderTree.LineImagesFolderUrl = BaseImgPath + "/Component/tree/lines";
                #endregion

                #region 초기화
                InitTree();
                #endregion

                
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-11-07
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();
        }


        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-11
        /// 작성목적 : 웹하드 트리
        /// </summary>
        public void InitTree()
        {
            tvFolderTree.CollapseNodeOnSelect = false;
            tvFolderTree.ExpandNodeOnSelect = false;
            tvFolderTree.MultipleSelectEnabled = false;

            // 회사 정보 가져오기            
            CompanyID = CurrentUserInfo.CompanyID;	//회사고유값
            UserID = CurrentUserInfo.UserID;		//사용자아이디   
            DeptID = CurrentUserInfo.UserDepts;        //부서아이디

            Zeospace.DeskPlus.WebHard.BLL.Folder dal = new BLL.Folder();
            DataSet ds = new DataSet();

            ds = dal.GetTreeU(CompanyID, 0);
            ds.Relations.Add("FolderRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentFolderID"], false);

            foreach (DataRow dbRow in ds.Tables["GetWebHardTree"].Rows)
            {
                if (dbRow.IsNull("ParentFolderID"))
                {
                    //공용 폴더
                    ComponentArt.Web.UI.TreeViewNode node = CreateRootNode(dbRow["FolderID"].ToString(), "U");
                    tvFolderTree.Nodes.Add(node);

                    //하위트리 
                    PopulateSubNode(dbRow, node);
                }
            }

            DataSet ds2 = new DataSet();

            ds2 = dal.GetTree(CompanyID, UserID);
            ds2.Relations.Add("FolderRelation", ds2.Tables[0].Columns["FolderID"], ds2.Tables[0].Columns["ParentFolderID"], false);

            foreach (DataRow dbRow2 in ds2.Tables["GetWebHardTree"].Rows)
            {
                if (dbRow2.IsNull("ParentFolderID"))
                {
                    //개인 폴더
                    ComponentArt.Web.UI.TreeViewNode nodeU = CreateRootNodeU(dbRow2["FolderID"].ToString(), "P");
                    tvFolderTree.Nodes.Add(nodeU);

                    //하위트리 
                    PopulateSubNodeU(dbRow2, nodeU);
                }
            }

            //페이지 시작시 노드 선택
            if (this.tvFolderTree.Nodes.Count > 0)
            {
                this.tvFolderTree.SelectedNode = this.tvFolderTree.Nodes[0];
            }

            ComponentArt.Web.UI.TreeViewNode node1 = CreateRootNodeU("-1", "S");
            tvFolderTree.Nodes.Add(node1);

            //공유한 사용자 정보 가져오기
            ShareSubNode(node1);
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-11
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Confirm");       //"확인";
                CbtnClose.Text = resMng.GetString("Close");           //"닫기";
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-11
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            FileList = GetDecryptParam("fileList", "0");
            Gubun = GetDecryptParam("gubun", "0");
            ListType = GetDecryptParam("listType", "0");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "WriteDate desc" : Order;

            PAdmin = GetDecryptParam("padmin", "0").ToNumber<int>();
            Types = GetDecryptParam("types", "");
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-11
        /// 작성목적 : 최상위 루트 트리 만들기
        /// </summary>
        /// <param name="_folderID"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(string _folderID, string _type)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                //최상위 노드 다국어 표현하기
                node.Text = GetResource("PublicFolder");  //"공용폴더"; 
                node.ID = _folderID; //목록 고유값
                node.Expanded = true;	//확장여부
                node.ImageUrl = "folders.gif";
                node.Attributes["ParentFolderID"] = "-1";
                node.Attributes["Type"] = _type;
                node.Attributes["UserID"] = UserID.ToString();

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD-CreateRootNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-11
        /// 작성목적 : 최상위 루트 트리 만들기
        /// </summary>
        /// <param name="_folderID"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNodeU(string _folderID, string _type)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                //최상위 노드 다국어 표현하기
                if (_type == "P")
                {
                    node.Text = GetResource("PrivateFolderName");  //"개인폴더";   
                }
                else if (_type == "S")
                {
                    node.Text = GetResource("ShareFolder");  //"공유받은 폴더";
                    node.CssClass = "NoActiveTreeNode";
                    node.HoverCssClass = "NoActiveTreeNode";
                    node.SelectedCssClass = "NoActiveTreeNode";
                    node.Selectable = false;
                }  

                node.ID = _folderID; //목록 고유값
                node.Expanded = true;	//확장여부
                node.ImageUrl = "folders.gif";
                node.Attributes["ParentFolderID"] = "-1";
                node.Attributes["Type"] = _type;
                node.Attributes["UserID"] = UserID.ToString();

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD-CreateRootNodeU", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-11
        /// 작성목적 : 자식 노드 가져오기 (공용폴더)
        /// </summary>
        /// <param name="_dr"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(),
                    _dr["NameEn"].ToString(), _dr["NameJa"].ToString(), _dr["NameFr"].ToString(),
                    _dr["NameZhcn"].ToString(), _dr["NameZhtw"].ToString(), PreLang);

                node.Text = name;
                node.ID = _dr["FolderID"].ToString(); //목록 고유값
                node.Expanded = false;	//확장여부
                node.ImageUrl = "folder.gif";


                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                //자식 노드가 있는 경우
                if (_dr.GetChildRows("FolderRelation").Length > 0)
                {
                    paraDic.Add("PUID", _dr["FolderID"].ToString());

                    if (PAdmin == 1)
                    {
                        paraDic.Add("padmin", "1");
                    }
                    else
                    {
                        paraDic.Add("padmin", "0");
                    }

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                    node.ContentCallbackUrl = "/DeskPlusEIP/WebHard/Folder/GetFolderTree.aspx?p=" + encParams;
                }

                node.Attributes["ParentFolderID"] = _dr["ParentFolderID"].ToString();
                node.Attributes["FavoriteFolderID"] = "0";
                node.Attributes["Type"] = "U";
                node.Attributes["UserID"] = UserID.ToString();

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD-CreateNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-11
        /// 작성목적 : 하위 트리 만들기 (공용폴더)
        /// </summary>
        /// <param name="_dbRow"></param>
        /// <param name="_node"></param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            foreach (DataRow childRow in _dbRow.GetChildRows("FolderRelation"))
            {
                string folderID = childRow[2].ToString();

                if (PAdmin == 1)
                {
                    ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);
                    _node.Nodes.Add(childhNode);
                }
                else
                {
                    BLL.PublicFolder bll = new BLL.PublicFolder();
                    DataSet ds = new DataSet();

                    ds = bll.GetPublicAuthority(CompanyID, UserID, DeptID, folderID.ToNumber<int>());

                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            // 개인 권한 우선 
                            DataRow[] dr = ds.Tables[0].Select("TargetType = 'M'");
                            if (dr.Length > 0)
                            {
                                if (dr[0]["Authority"].ToString() == "3")
                                {
                                    ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);
                                    _node.Nodes.Add(childhNode);
                                }
                                else if (dr[0]["Authority"].ToString() == "2")
                                {
                                    ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);
                                    _node.Nodes.Add(childhNode);
                                }
                                else if (dr[0]["Authority"].ToString() == "1")
                                {
                                    ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);

                                    childhNode.CssClass = "GrayedTreeNode";
                                    childhNode.HoverCssClass = "GrayedTreeNode";
                                    childhNode.SelectedCssClass = "GrayedTreeNode";
                                    childhNode.Selectable = false;

                                    _node.Nodes.Add(childhNode);
                                }
                                else
                                {
                                    ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);

                                    childhNode.Text = GetResource("NoPermission");
                                    childhNode.ImageUrl = "folder-disable.gif";
                                    childhNode.CssClass = "NoActiveTreeNode";
                                    childhNode.HoverCssClass = "NoActiveTreeNode";
                                    childhNode.SelectedCssClass = "NoActiveTreeNode";
                                    childhNode.Selectable = false;

                                    _node.Nodes.Add(childhNode);
                                }
                            }
                            else
                            {
                                foreach (DataRow dbRow in ds.Tables[0].Rows)
                                {
                                    if (dbRow["Authority"].ToString() == "3")
                                    {
                                        ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);
                                        _node.Nodes.Add(childhNode);
                                    }
                                    else if (dbRow["Authority"].ToString() == "2")
                                    {
                                        ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);
                                        _node.Nodes.Add(childhNode);
                                    }
                                    else if (dbRow["Authority"].ToString() == "1")
                                    {
                                        ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);

                                        childhNode.CssClass = "GrayedTreeNode";
                                        childhNode.HoverCssClass = "GrayedTreeNode";
                                        childhNode.SelectedCssClass = "GrayedTreeNode";
                                        childhNode.Selectable = false;

                                        _node.Nodes.Add(childhNode);
                                    }
                                    else
                                    {
                                        ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);

                                        childhNode.Text = GetResource("NoPermission");
                                        childhNode.ImageUrl = "folder-disable.gif";
                                        childhNode.CssClass = "NoActiveTreeNode";
                                        childhNode.HoverCssClass = "NoActiveTreeNode";
                                        childhNode.SelectedCssClass = "NoActiveTreeNode";
                                        childhNode.Selectable = false;

                                        _node.Nodes.Add(childhNode);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);

                            childhNode.Text = GetResource("NoPermission");
                            childhNode.ImageUrl = "folder-disable.gif";
                            childhNode.CssClass = "NoActiveTreeNode";
                            childhNode.HoverCssClass = "NoActiveTreeNode";
                            childhNode.SelectedCssClass = "NoActiveTreeNode";
                            childhNode.Selectable = false;

                            _node.Nodes.Add(childhNode);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-11
        /// 작성목적 : 하위 트리 만들기 (내폴더)
        /// </summary>
        /// <param name="_dbRow"></param>
        /// <param name="_node"></param>
        private void PopulateSubNodeU(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            foreach (DataRow childRow in _dbRow.GetChildRows("FolderRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateNodeU(childRow);
                _node.Nodes.Add(childhNode);
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-11
        /// 작성목적 : 자식 노드 가져오기 (내폴더)
        /// </summary>
        /// <param name="_dr"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNodeU(DataRow _dr)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(),
                    _dr["NameEn"].ToString(), _dr["NameJa"].ToString(), _dr["NameFr"].ToString(),
                    _dr["NameZhcn"].ToString(), _dr["NameZhtw"].ToString(), PreLang);

                node.Text = name;
                node.ID = _dr["FolderID"].ToString(); //목록 고유값
                node.Expanded = false;	//확장여부
                node.ImageUrl = "folder.gif";


                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                //자식 노드가 있는 경우
                //foreach (DataRow childRow in _dr.GetChildRows("FolderRelation"))
                if (_dr.GetChildRows("FolderRelation").Length > 0)
                {
                    paraDic.Add("PID", _dr["FolderID"].ToString());
                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                    node.ContentCallbackUrl = "/DeskPlusEIP/WebHard/Folder/GetFolderTree.aspx?p=" + encParams;
                }


                node.Attributes["ParentFolderID"] = _dr["ParentFolderID"].ToString();
                node.Attributes["Type"] = "P";
                node.Attributes["UserID"] = UserID.ToString();

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD-CreateNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-12
        /// 작성목적 : 공유한 사용자 정보 가져오기
        /// </summary>
        /// <param name="_node"></param>
        private void ShareSubNode(ComponentArt.Web.UI.TreeViewNode _node)
        {
            BLL.ShareFolder bll = new BLL.ShareFolder();
            DataSet ds = new DataSet();

            ds = bll.GetShareUser(CompanyID, UserID, DeptID);

            ArrayList userIDArr = new ArrayList();

            //공유된 사용자 정보를 골라 온다
            foreach (DataRow dr in ds.Tables["GetShareUser"].Rows)
            {
                if (!userIDArr.Contains(dr["UserID"].ToString()))
                {
                    if (dr["UserID"].ToString() == UserID.ToString()) continue;
                    userIDArr.Add(dr["UserID"].ToString());
                }
            }

            for (int i = 0; i < userIDArr.Count; i++)
            {
                DataRow[] drs = ds.Tables[0].Select("UserID = " + userIDArr[i]);

                string name = this.GetLangName(drs[0]["NameBase"].ToString(), drs[0]["NameKo"].ToString(), drs[0]["NameEn"].ToString(), drs[0]["NameJa"].ToString(), drs[0]["NameFr"].ToString(), drs[0]["NameZhcn"].ToString(), drs[0]["NameZhtw"].ToString(), PreLang);
                if (name == "") continue;

                ComponentArt.Web.UI.TreeViewNode childhNode = CreateShareNode(drs[0]);
                _node.Nodes.Add(childhNode);
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-07-12
        /// 작성목적 : 공유한 사용자 트리 만들기
        /// </summary>
        /// <param name="_dr"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateShareNode(DataRow _dr)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(), _dr["NameFr"].ToString(), _dr["NameZhcn"].ToString(), _dr["NameZhtw"].ToString(), PreLang);

                node.Text = name;
                node.ID = _dr["UserID"].ToString(); //목록 고유값
                node.Expanded = false;	//확장여부
                node.ImageUrl = "shareUser.gif";


                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                paraDic.Add("UID", _dr["UserID"].ToString());
                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                node.ContentCallbackUrl = "/DeskPlusEIP/WebHard/Folder/GetFolderTree.aspx?p=" + encParams;

                node.Attributes["ParentFolderID"] = "-1";
                node.Attributes["Type"] = "S";
                node.Attributes["UserID"] = _dr["UserID"].ToString();
                node.CssClass = "NoActiveTreeNode";
                node.HoverCssClass = "NoActiveTreeNode";
                node.SelectedCssClass = "NoActiveTreeNode";
                node.Selectable = false;

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD-CreateShareNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}