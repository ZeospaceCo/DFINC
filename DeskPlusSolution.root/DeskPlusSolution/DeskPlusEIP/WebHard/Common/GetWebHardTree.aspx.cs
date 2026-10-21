using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Collections;

namespace Zeospace.DeskPlus.WebHard.Web.Common
{
    public partial class GetWebHardTree : Base.BasePageWebHard
    {
        #region Declaration (컨트롤 선언)
        private int CompanyID;
        private int UserID;
        private string[] DeptID;
        private int PID;
        private int PUID;
        private int PAdmin;
        private int PList;
        private int UID;
        private int FaID;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Common.GetWebHardTree).Assembly;	//어셈블리 선언
            #endregion

            #region 바인딩

            LoadParam();

            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            if (PID != 0)
            {
                try
                {
                    CompanyID = CurrentUserInfo.CompanyID;	//회사고유값
                    UserID = CurrentUserInfo.UserID;		//사용자아이디                    

                    DataSet ds = new DataSet();

                    DAL.Folder dal = new DAL.Folder();
                    ds = dal.GetChildTree(CompanyID, PID);                    

                    if (ds.Tables["GetChildTree"] != null)
                    {
                        foreach (DataRow dbRow in ds.Tables["GetChildTree"].Rows)
                        {
                            ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow);
                            TreeView1.Nodes.Add(node);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetWebHardTree.aspx", ex);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                    throw;
                }

                Response.Clear();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                Response.Write(TreeView1.GetXml());
                Response.End();
            }

            if (PUID != 0)
            {
                try
                {
                    CompanyID = CurrentUserInfo.CompanyID;	//회사고유값
                    UserID = CurrentUserInfo.UserID;		//사용자아이디                    
                    DeptID = CurrentUserInfo.UserDepts;

                    DataSet ds = new DataSet();

                    DAL.Folder dal = new DAL.Folder();
                    ds = dal.GetChildTree(CompanyID, PUID);

                    if (ds.Tables["GetChildTree"] != null)
                    {
                        foreach (DataRow dbRow in ds.Tables["GetChildTree"].Rows)
                        {
                            BLL.PublicFolder bll = new BLL.PublicFolder();
                            DataSet ds2 = new DataSet();

                            ds2 = bll.GetPublicAuthority(CompanyID, UserID, DeptID, dbRow["FolderID"].ToString().ToNumber<int>());

                            if (ds2 != null)
                            {
                                if (ds2.Tables[0].Rows.Count > 0)
                                {
                                    ComponentArt.Web.UI.TreeViewNode node = CreatePublicNode(dbRow);
                                    TreeView1.Nodes.Add(node);
                                }
                                else
                                {
                                    ComponentArt.Web.UI.TreeViewNode node = CreatePublicNode(dbRow);

                                    if (PAdmin != 1)
                                    {
                                        node.Text = GetResource("NoPermission");
                                        node.ImageUrl = "folder-disable.gif";
                                        node.CssClass = "NoActiveTreeNode";
                                        node.HoverCssClass = "NoActiveTreeNode";
                                        node.SelectedCssClass = "NoActiveTreeNode";
                                        node.Selectable = false;
                                    }

                                    TreeView1.Nodes.Add(node);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetWebHardTree.aspx", ex);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                    throw;
                }

                Response.Clear();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                Response.Write(TreeView1.GetXml());
                Response.End();
            }

            if (UID != 0)
            {
                try
                {
                    CompanyID = CurrentUserInfo.CompanyID;	//회사고유값
                    UserID = CurrentUserInfo.UserID;		//사용자아이디    
                    DeptID = CurrentUserInfo.UserDepts;

                    DataSet ds = new DataSet();

                    BLL.ShareFolder bll = new BLL.ShareFolder();
                    ds = bll.GetShareUser(CompanyID, UserID, DeptID, UID);

                    if (ds.Tables["GetShareUser"] != null)
                    {
                        ArrayList existFolderCheck = new ArrayList(); // 동일한 폴더 존재 유무 체크
                        existFolderCheck.Clear();
                        foreach (DataRow dbRow in ds.Tables["GetShareUser"].Rows)
                        {
                            if (!existFolderCheck.Contains(dbRow["FolderID"].ToString()))
                            {
                                ComponentArt.Web.UI.TreeViewNode node = CreateShareNode(dbRow);
                                TreeView1.Nodes.Add(node);
                                existFolderCheck.Add(dbRow["FolderID"].ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetWebHardTree.aspx", ex);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                    throw;
                }

                Response.Clear();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                Response.Write(TreeView1.GetXml());
                Response.End();
            }

            if (FaID != 0)
            {
                try
                {
                    CompanyID = CurrentUserInfo.CompanyID;	//회사고유값
                    UserID = CurrentUserInfo.UserID;		//사용자아이디    
                    DeptID = CurrentUserInfo.UserDepts;

                    DataSet ds = new DataSet();

                    DAL.Favorite dal = new DAL.Favorite();
                    ds = dal.GetFavoriteTree(CompanyID, UserID, FaID);

                    if (ds.Tables["GetFavoriteTree"] != null)
                    {
                        foreach (DataRow dbRow in ds.Tables["GetFavoriteTree"].Rows)
                        {
                            ComponentArt.Web.UI.TreeViewNode node = CreateFavoriteNode(dbRow);
                            TreeView1.Nodes.Add(node);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetWebHardTree.aspx", ex);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                    throw;
                }

                Response.Clear();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                Response.Write(TreeView1.GetXml());
                Response.End();
            }

            #endregion
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            PID = GetDecryptParam("PID", "0").ToNumber<int>();
            PUID = GetDecryptParam("PUID", "0").ToNumber<int>();
            UID = GetDecryptParam("UID", "0").ToNumber<int>();
            FaID = GetDecryptParam("FaID", "0").ToNumber<int>();
            PAdmin = GetDecryptParam("padmin", "0").ToNumber<int>();
            PList = GetDecryptParam("plist", "0").ToNumber<int>();    
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-26
        /// 작성목적 : 웹하드 트리
        /// </summary>
        /// <param name="_dr"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(), _dr["NameFr"].ToString(), _dr["NameZhcn"].ToString(), _dr["NameZhtw"].ToString(), PreLang);

                node.Text = name;
                node.ID = _dr["FolderID"].ToString(); //목록 고유값
                node.Expanded = false;	//확장여부
                node.ImageUrl = "folder.gif";


                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                //자식 노드가 있는 경우
                int childCnt = _dr["childCnt"].ToString().ToNumber<int>();
                if (childCnt > 0)
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
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-19
        /// 작성목적 : 공용폴더 웹하드 트리
        /// </summary>
        /// <param name="_dr"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreatePublicNode(DataRow _dr)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(), _dr["NameFr"].ToString(), _dr["NameZhcn"].ToString(), _dr["NameZhtw"].ToString(), PreLang);

                node.Text = name;
                node.ID = _dr["FolderID"].ToString(); //목록 고유값
                node.Expanded = false;	//확장여부
                node.ImageUrl = "folder.gif";


                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                //자식 노드가 있는 경우
                int childCnt = _dr["childCnt"].ToString().ToNumber<int>();
                if (childCnt > 0)
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

                    if (PList == 1)
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

                if (PAdmin == 1)
                {
                    if (PList == 1)
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
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-26
        /// 작성목적 : 공유한 사용자의 공유 폴더 정보 가져오기
        /// </summary>
        /// <param name="_dr"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateShareNode(DataRow _dr)
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
                node.Attributes["ParentFolderID"] = _dr["ParentFolderID"].ToString();
                node.Attributes["FavoriteFolderID"] = "0";
                node.Attributes["Type"] = "S";
                node.Attributes["UserID"] = UID.ToString();                

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

                node.Attributes["ParentFavoriteID"] = _dr["ParentFavoriteID"].ToString();
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


        #endregion

        #region Property (프로퍼티)
        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}