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

namespace Zeospace.DeskPlus.WebHard.Web.Upload
{
    public partial class WebHardAttach : Base.BasePageWebHard
    {
        #region 변수처리
        public string GUID = "";
        public string CallBackEvent = "";
        public string ModuleName = "";//모듈(APPORVAL, BOARD, PIM)
        public string FileType = ""; //필터 파일확장자
        public string FileTypeDesc = ""; // 필터 설명
        public int CompanyID;
        public int UserID;
        private string[] DeptID;
        public string TotalCount = String.Empty;  //총카운트
        public int FolderID;
        public string Order = String.Empty; //sort     
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Upload.WebHardAttach).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();

                #region 이미지 처리
                tvFolderTree.ImagesBaseUrl = this.BaseImgPath + "/Component/tree/";
                tvFolderTree.LineImagesFolderUrl = BaseImgPath + "/Component/tree/lines";
                #endregion

                #region 초기화
                InitTree();
                #endregion

                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-26
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            GetListCount();
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-26
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
            DeptID = CurrentUserInfo.UserDepts;       //부서아이디

            Zeospace.DeskPlus.WebHard.BLL.Folder dal = new BLL.Folder();
            DataSet ds = new DataSet();

            ds = dal.GetTree(CompanyID, UserID);
            ds.Relations.Add("FolderRelation", ds.Tables[0].Columns["FolderID"], ds.Tables[0].Columns["ParentFolderID"], false);

            foreach (DataRow dbRow in ds.Tables["GetWebHardTree"].Rows)
            {
                if (dbRow.IsNull("ParentFolderID"))
                {
                    //개인 폴더
                    ComponentArt.Web.UI.TreeViewNode node = CreateRootNode(dbRow["FolderID"].ToString(), "P", dbRow);
                    tvFolderTree.Nodes.Add(node);

                    //하위트리 
                    PopulateSubNode(dbRow, node);

                    //전달 받은 파일
                    ComponentArt.Web.UI.TreeViewNode node0 = CreateRootNode("-3", "J", null);
                    tvFolderTree.Nodes.Add(node0);

                }
            }

            ComponentArt.Web.UI.TreeViewNode node1 = CreateRootNode("-1", "S", null);
            tvFolderTree.Nodes.Add(node1);

            //공유한 사용자 정보 가져오기
            ShareSubNode(node1);

            //페이지 시작시 노드 선택
            if (this.tvFolderTree.Nodes.Count > 0)
            {
                this.tvFolderTree.SelectedNode = this.tvFolderTree.Nodes[0];
            }
        }
        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-26
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-26
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            GUID = GetDecryptParam("swfUploadGUID", "");
            FileType = GetDecryptParam("fileType", "");
            FileTypeDesc = GetDecryptParam("fileTypeDesc", "");
            CallBackEvent = GetDecryptParam("callBackEvent", "");
            ModuleName = GetDecryptParam("moduleName", "");
            Order = "WriteDate desc";
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-26
        /// 작성목적 : 최상위 루트 트리 만들기
        /// </summary>
        /// <param name="_folderID"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(string _folderID, string _type, DataRow _dbRow)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                //최상위 노드 다국어 표현하기
                if (_type == "P")
                {
                    node.Text = GetResource("PrivateFolderName");  //"개인폴더";   
                }
                else if (_type == "J")
                {
                    node.Text = GetResource("SendFile");  //"전달받은파일";   
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD-CreateRootNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-26
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
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-26
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
                paraDic.Add("Attach", "Y");
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


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-01-26
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
        /// 작성일자 : 2012-01-26
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
        /// 작성일자 : 2012-01-26
        /// 작성목적 : 자신의 개인폴더의 게시물 갯수
        /// </summary>
        private void GetListCount()
        {
            DAL.File dal = new DAL.File();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID).ToString();
        }

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}