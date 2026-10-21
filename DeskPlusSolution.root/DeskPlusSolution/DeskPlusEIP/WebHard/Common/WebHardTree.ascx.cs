using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections;

namespace Zeospace.DeskPlus.WebHard.Web.Common
{
    public partial class WebHardTree : Base.BaseControlWebHard
    {

        #region 변수설정
        private int CompanyID;
        private int UserID;
        private string[] DeptID;
        private bool AccessAuth = false;
        private bool AccessList = false;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Common.WebHardTree).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                #region 초기화
                InitTree();
                #endregion
            }
            
        }

        #endregion 

        #region DataLoad (컨트롤이 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-13
        /// 작성목적 : 웹하드 트리
        /// </summary>
        public void InitTree()
        {
            tvWebHardTree.Nodes.Clear();
            tvWebHardTree.CollapseNodeOnSelect = false;
            tvWebHardTree.ExpandNodeOnSelect = false;
            tvWebHardTree.MultipleSelectEnabled = false;
            tvWebHardTree.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvWebHardTree.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            tvWebHardTree.DragAndDropEnabled = false;            

            // 회사 정보 가져오기            
            CompanyID = CurrentUserInfo.CompanyID;	//회사고유값
            UserID = CurrentUserInfo.UserID;		//사용자아이디   
            DeptID = CurrentUserInfo.UserDepts;        //부서아이디

            string[] useCategoryStr = UseCategory.Split(',');

            for (int i = 0; i < useCategoryStr.Length; i++)
            {
                if (useCategoryStr[i].ToString() == "U" || useCategoryStr[i].ToString() == "A" || useCategoryStr[i].ToString() == "L") // 공유폴더 정보 가져오기
                {
                    if (useCategoryStr[i].ToString() == "A" || useCategoryStr[i].ToString() == "L")
                    {
                        AccessAuth = true;
                    }

                    if (useCategoryStr[i].ToString() == "L")
                    {
                        AccessList = true;
                    }

                    Zeospace.DeskPlus.WebHard.BLL.Folder dal = new BLL.Folder();
                    DataSet ds = new DataSet();

                    ds = dal.GetTreeU(CompanyID, 0);
                    ds.Relations.Add("FolderRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentFolderID"], false);

                    foreach (DataRow dbRow in ds.Tables["GetWebHardTree"].Rows)
                    {
                        if (dbRow.IsNull("ParentFolderID"))
                        {
                            if (AccessAuth == false)
                            {
                                ComponentArt.Web.UI.TreeViewNode node = CreateRootNode(dbRow["FolderID"].ToString(), "U");
                                tvWebHardTree.Nodes.Add(node);

                                //하위트리 
                                PublicSubNode(dbRow, node);

                                //휴지통
                                ComponentArt.Web.UI.TreeViewNode node2 = CreateRootNode("-6", "B");
                                tvWebHardTree.Nodes.Add(node2);
                            }
                            else
                            {
                                //공용폴더
                                if (AccessList == false)
                                {
                                    ComponentArt.Web.UI.TreeViewNode node = CreateRootNode(dbRow["FolderID"].ToString(), "A");
                                    tvWebHardTree.Nodes.Add(node);

                                    //하위트리 
                                    PublicSubNode(dbRow, node);
                                }
                                else
                                {
                                    ComponentArt.Web.UI.TreeViewNode node = CreateRootNode(dbRow["FolderID"].ToString(), "L");
                                    tvWebHardTree.Nodes.Add(node);

                                    //하위트리 
                                    PublicSubNode(dbRow, node);

                                    //휴지통
                                    ComponentArt.Web.UI.TreeViewNode node2 = CreateRootNode("-6", "B");
                                    tvWebHardTree.Nodes.Add(node2);
                                }
                            }
                        }
                    }
                }
                else if (useCategoryStr[i].ToString() == "P") // 내폴더 정보 가져오기
                {
                    Zeospace.DeskPlus.WebHard.BLL.Folder dal = new BLL.Folder();
                    DataSet ds = new DataSet();

                    ds = dal.GetTree(CompanyID, UserID);
                    ds.Relations.Add("FolderRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentFolderID"], false);

                    foreach (DataRow dbRow in ds.Tables["GetWebHardTree"].Rows)
                    {
                        if (dbRow.IsNull("ParentFolderID"))
                        {
                            //개인 폴더
                            ComponentArt.Web.UI.TreeViewNode node = CreateRootNode(dbRow["FolderID"].ToString(), "P");
                            tvWebHardTree.Nodes.Add(node);

                            //하위트리 
                            PopulateSubNode(dbRow, node);

                            //전달 받은 파일
                            ComponentArt.Web.UI.TreeViewNode node1 = CreateRootNode("-3", "J");
                            tvWebHardTree.Nodes.Add(node1);

                            //휴지통
                            ComponentArt.Web.UI.TreeViewNode node2 = CreateRootNode("-4", "T");
                            tvWebHardTree.Nodes.Add(node2);
                        }
                    }
                }                
                else if (useCategoryStr[i].ToString() == "S")  //공유받은 폴더 정보 가져오기
                {
                    ComponentArt.Web.UI.TreeViewNode node = CreateRootNode("-1", "S");
                    tvWebHardTree.Nodes.Add(node);    
        
                    //공유한 사용자 정보 가져오기
                    ShareSubNode(node);
                }
                else if (useCategoryStr[i].ToString() == "M")  //공유한 폴더 정보 가져오기
                {
                    ComponentArt.Web.UI.TreeViewNode node = CreateRootNode("-2", "M");
                    tvWebHardTree.Nodes.Add(node);

                    //공유한 폴더 정보 가져오기
                    ShareNode(node);
                }
                else if (useCategoryStr[i].ToString() == "F")  //즐겨 찾기 정보 가져오기
                {
                    ComponentArt.Web.UI.TreeViewNode node = CreateRootNode("-5", "FC");
                    tvWebHardTree.Nodes.Add(node);

                    //즐겨 찾기 폴더 정보 가져오기
                    FavoriteNode(node);
                }
            }

            //페이지 시작시 노드 선택
            if (this.tvWebHardTree.Nodes.Count > 0)
            {
                this.tvWebHardTree.SelectedNode = this.tvWebHardTree.Nodes[0];
            }            
        }       

        #endregion

        #region Method (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-17
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

                node.ID = _folderID; //목록 고유값
                node.Expanded = true;	//확장여부                
                node.Attributes["ParentFolderID"] = "-1";
                node.Attributes["FavoriteFolderID"] = "0";
                node.Attributes["Type"] = _type;
                node.Attributes["UserID"] = UserID.ToString();
                node.Attributes["FavoriteID"] = "0"; //즐겨찾기고유아이디 
                node.ImageUrl = "folders.gif";

                //최상위 노드 다국어 표현하기
                if (_type == "U" || _type == "A" | _type == "L")
                {
                    node.Text = GetResource("PublicFolder");  //"공용폴더"; 
                }
                else if (_type == "B")
                {
                    node.Text = GetResource("TrashFolder");  //"공용휴지통";   
                }
                else if (_type == "P")
                {
                    node.Text = GetResource("PrivateFolderName");  //"개인폴더"; 
                }
                else if (_type == "J")
                {
                    node.Text = GetResource("SendFile");  //"전달받은파일";   
                }
                else if (_type == "T")
                {
                    node.Text = GetResource("TrashFolder");  //"휴지통";   
                }
                else if (_type == "S")
                {
                    node.Text = GetResource("ShareFolder");  //"공유받은 폴더";
                    node.CssClass = "NoActiveTreeNode";
                    node.HoverCssClass = "NoActiveTreeNode";
                    node.SelectedCssClass = "NoActiveTreeNode";
                    node.Selectable = false;
                }
                else if (_type == "M")
                {
                    node.Text = GetResource("SharedFolder");  //"공유한 폴더";   
                    node.CssClass = "NoActiveTreeNode";
                    node.HoverCssClass = "NoActiveTreeNode";
                    node.SelectedCssClass = "NoActiveTreeNode";
                    node.Selectable = false;
                }
                else if (_type == "FC")
                {
                    node.Text = GetResource("FavoriteFolder");  //"즐겨 찾기";  
                    node.ImageUrl = "favorites.gif";
                    node.Attributes["ParentFavoriteID"] = "0";
                }
                

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
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-14
        /// 작성목적 : 자식 노드 가져오기 (내폴더)
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
                //foreach (DataRow childRow in _dr.GetChildRows("FolderRelation"))
                if(_dr.GetChildRows("FolderRelation").Length > 0)
                {   
                    paraDic.Add("PID", _dr["FolderID"].ToString());
                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                    node.ContentCallbackUrl = "/DeskPlusEIP/WebHard/Common/GetWebHardTree.aspx?p=" + encParams;
                }
                

                node.Attributes["ParentFolderID"] = _dr["ParentFolderID"].ToString();
                node.Attributes["FavoriteFolderID"] = "0";
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
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-26
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
                node.ContentCallbackUrl = "/DeskPlusEIP/WebHard/Common/GetWebHardTree.aspx?p=" + encParams;

                node.Attributes["ParentFolderID"] = "-1";
                node.Attributes["FavoriteFolderID"] = "0";
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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-26
        /// 작성목적 : 공유한 폴더 가져오기
        /// </summary>
        /// <param name="_dr"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateShareNode2(DataRow _dr)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(), _dr["NameFr"].ToString(), _dr["NameZhcn"].ToString(), _dr["NameZhtw"].ToString(), PreLang);

                name = (name == "ROOT") ? GetResource("PrivateFolderName") : name;
                node.Text = name;
                node.ID = _dr["FolderID"].ToString(); //목록 고유값
                node.Expanded = false;	//확장여부
                node.ImageUrl = "shared.gif";
                node.Attributes["ParentFolderID"] = _dr["FolderID"].ToString();
                node.Attributes["FavoriteFolderID"] = "0";
                node.Attributes["Type"] = "M";
                node.Attributes["UserID"] = _dr["UserID"].ToString();                

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD-CreateShareNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-23
        /// 작성목적 : 즐겨 찾기 폴더 가져오기
        /// </summary>
        /// <param name="_dr"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateFavoriteNode(DataRow _dr)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(), _dr["NameFr"].ToString(), _dr["NameZhcn"].ToString(), _dr["NameZhtw"].ToString(), PreLang);

                name = (name == "ROOT") ? GetResource("PrivateFolderName") : name;
                node.Text = name;
                node.ID = _dr["FavoriteID"].ToString(); //목록 고유값
                node.Expanded = false;	//확장여부

                if (_dr["Types"].ToString() == "FC")
                {
                    node.ImageUrl = "favorites.gif";                    
                }
                else
                {
                    node.ImageUrl = "folder.gif";
                }

                //자식 노드가 있는 경우
                if (Convert.ToInt32(_dr["childCnt"]) > 0)
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();

                    paraDic.Add("FaID", _dr["FavoriteID"].ToString());
                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    node.ContentCallbackUrl = "/DeskPlusEIP/WebHard/Common/GetWebHardTree.aspx?p=" + encParams;
                }

                if (String.IsNullOrEmpty(_dr["ParentFavoriteID"].ToString()))
                {
                    node.Attributes["ParentFavoriteID"] = "-5";
                }
                else
                {
                    node.Attributes["ParentFavoriteID"] = _dr["ParentFavoriteID"].ToString();
                }

                node.Attributes["FavoriteFolderID"] = _dr["FolderID"].ToString();
                node.Attributes["Type"] = _dr["Types"].ToString();
                node.Attributes["UserID"] = _dr["UserID"].ToString();
                node.Attributes["ChildCnt"] = _dr["childCnt"].ToString(); //하위폴더개수

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD-CreateFavoriteNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-17
        /// 작성목적 : 하위 트리 만들기 (내폴더)
        /// </summary>
        /// <param name="_dbRow"></param>
        /// <param name="_node"></param>
        private void PopulateSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            foreach (DataRow childRow in _dbRow.GetChildRows("FolderRelation"))
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(childRow);
                _node.Nodes.Add(childhNode);

                //재귀함수 호출
                //PopulateSubNode(childRow, childhNode);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-26
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
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-26
        /// 작성목적 : 공유한 폴더 정보 가져오기
        /// </summary>
        /// <param name="_node"></param>
        private void ShareNode(ComponentArt.Web.UI.TreeViewNode _node)
        {
            DAL.ShareFolder dal = new DAL.ShareFolder();
            DataSet ds = new DataSet();

            ds = dal.GetShareTree(CompanyID, UserID);

            ds.Relations.Add("FavoriteRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentFolderID"], false);
            foreach (DataRow dr in ds.Tables["GetShareTree"].Rows)
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateShareNode2(dr);
                _node.Nodes.Add(childhNode);
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-23
        /// 작성목적 : 즐겨찾기 폴더 정보 가져오기
        /// </summary>
        /// <param name="_node"></param>
        private void FavoriteNode(ComponentArt.Web.UI.TreeViewNode _node)
        {
            DAL.Favorite dal = new DAL.Favorite();
            DataSet ds = new DataSet();

            int favoriteID = 0;
            ds = dal.GetFavoriteTree(CompanyID, UserID, favoriteID);
            foreach (DataRow dr in ds.Tables["GetFavoriteTree"].Rows)
            {
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateFavoriteNode(dr);
                _node.Nodes.Add(childhNode);
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-16
        /// 작성목적 : 자식 노드 가져오기 (공용폴더)
        /// </summary>
        /// <param name="_dr"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreatePublicNode(DataRow _dr)
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

                    if (AccessAuth == true)
                    {
                        paraDic.Add("padmin", "1");
                    }
                    else
                    {
                        paraDic.Add("padmin", "0");
                    }

                    if (AccessList == true)
                    {
                        paraDic.Add("plist", "1");
                    }
                    else
                    {
                        paraDic.Add("plist", "0");
                    }

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                    node.ContentCallbackUrl = "/DeskPlusEIP/WebHard/Common/GetWebHardTree.aspx?p=" + encParams;
                }

                node.Attributes["ParentFolderID"] = _dr["ParentFolderID"].ToString();
                node.Attributes["FavoriteFolderID"] = "0";

                if (AccessAuth == true)
                {
                    if (AccessList == true)
                    {
                        node.Attributes["Type"] = "L";
                    }
                    else
                    {
                        node.Attributes["Type"] = "A";                        
                    }
                }
                else
                {
                    node.Attributes["Type"] = "U";
                }

                node.Attributes["UserID"] = UserID.ToString();

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD-CreatePublicNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-16
        /// 작성목적 : 공용폴더 사용자 정보 가져오기
        /// </summary>
        /// <param name="_node"></param>
        private void PublicSubNode(DataRow _dbRow, ComponentArt.Web.UI.TreeViewNode _node)
        {
            foreach (DataRow childRow in _dbRow.GetChildRows("FolderRelation"))
            {
                string folderID = childRow[2].ToString();

                BLL.PublicFolder bll = new BLL.PublicFolder();
                DataSet ds = new DataSet();

                ds = bll.GetPublicAuthority(CompanyID, UserID, DeptID, folderID.ToNumber<int>());

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(childRow);
                        _node.Nodes.Add(childhNode);
                    }
                    else
                    {
                        ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(childRow);

                        if (AccessAuth != true)
                        {
                            childhNode.Text = GetResource("NoPermission");
                            childhNode.ImageUrl = "folder-disable.gif";
                            childhNode.CssClass = "NoActiveTreeNode";
                            childhNode.HoverCssClass = "NoActiveTreeNode";
                            childhNode.SelectedCssClass = "NoActiveTreeNode";
                            childhNode.Selectable = false;
                        }

                        _node.Nodes.Add(childhNode);
                    }
                }
            }
        }
        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// 사용할 카테고리값 (내폴더,공유받은폴더,공유한폴더,전달받은파일,중요표시 /P,S,M)
        /// </summary>
        public string UseCategory
        {
            get
            {
                object o = ViewState["UseCategory"];
                if (o == null)
                    return "U,P,S,M";
                else
                    return ViewState["UseCategory"].ToString();
            }
            set
            {
                ViewState["UseCategory"] = value;
            }
        }        
        
        #endregion

        #region Event Handler (이벤트 핸들러)       
        #endregion
    }
}