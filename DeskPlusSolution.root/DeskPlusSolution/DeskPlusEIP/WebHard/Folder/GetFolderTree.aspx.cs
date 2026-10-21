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

namespace Zeospace.DeskPlus.WebHard.Web.Folder
{
    public partial class GetFolderTree : Base.BasePageWebHard
    {
        #region Declaration (컨트롤 선언)
        private int CompanyID;
        private int UserID;
        private string[] DeptID;
        private int PID;
        private int UID;
        private int PUID;
        private int PAdmin;
        private string Attach;

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Folder.GetFolderTree).Assembly;	//어셈블리 선언
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
                            if (PAdmin == 1)
                            {
                                ComponentArt.Web.UI.TreeViewNode node = CreatePublicNode(dbRow);
                                TreeView1.Nodes.Add(node);
                            }
                            else
                            {

                                BLL.PublicFolder bll = new BLL.PublicFolder();
                                DataSet ds2 = new DataSet();

                                ds2 = bll.GetPublicAuthority(CompanyID, UserID, DeptID, dbRow["FolderID"].ToString().ToNumber<int>());

                                if (ds2 != null)
                                {
                                    if (ds2.Tables[0].Rows.Count > 0)
                                    {
                                        // 개인 권한 우선 
                                        DataRow[] dr = ds2.Tables[0].Select("TargetType = 'M'");
                                        if (dr.Length > 0)
                                        {
                                            if (dr[0]["Authority"].ToString() == "3")
                                            {
                                                ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(dbRow);
                                                TreeView1.Nodes.Add(childhNode);
                                            }
                                            else if (dr[0]["Authority"].ToString() == "2")
                                            {
                                                ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(dbRow);
                                                TreeView1.Nodes.Add(childhNode);
                                            }
                                            else if (dr[0]["Authority"].ToString() == "1")
                                            {
                                                ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(dbRow);

                                                childhNode.CssClass = "GrayedTreeNode";
                                                childhNode.HoverCssClass = "GrayedTreeNode";
                                                childhNode.SelectedCssClass = "GrayedTreeNode";
                                                childhNode.Selectable = false;

                                                TreeView1.Nodes.Add(childhNode);
                                            }
                                            else
                                            {
                                                ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(dbRow);

                                                childhNode.Text = GetResource("NoPermission");
                                                childhNode.ImageUrl = "folder-disable.gif";
                                                childhNode.CssClass = "NoActiveTreeNode";
                                                childhNode.HoverCssClass = "NoActiveTreeNode";
                                                childhNode.SelectedCssClass = "NoActiveTreeNode";
                                                childhNode.Selectable = false;

                                                TreeView1.Nodes.Add(childhNode);
                                            }
                                        }
                                        else
                                        {
                                            foreach (DataRow dbRow2 in ds2.Tables[0].Rows)
                                            {
                                                if (dbRow2["Authority"].ToString() == "3")
                                                {
                                                    ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(dbRow);
                                                    TreeView1.Nodes.Add(childhNode);
                                                }
                                                else if (dbRow2["Authority"].ToString() == "2")
                                                {
                                                    ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(dbRow);
                                                    TreeView1.Nodes.Add(childhNode);
                                                }
                                                else if (dbRow2["Authority"].ToString() == "1")
                                                {
                                                    ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(dbRow);

                                                    childhNode.CssClass = "GrayedTreeNode";
                                                    childhNode.HoverCssClass = "GrayedTreeNode";
                                                    childhNode.SelectedCssClass = "GrayedTreeNode";
                                                    childhNode.Selectable = false;

                                                    TreeView1.Nodes.Add(childhNode);
                                                }
                                                else
                                                {
                                                    ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(dbRow);

                                                    childhNode.Text = GetResource("NoPermission");
                                                    childhNode.ImageUrl = "folder-disable.gif";
                                                    childhNode.CssClass = "GrayedTreeNode";
                                                    childhNode.HoverCssClass = "GrayedTreeNode";
                                                    childhNode.SelectedCssClass = "GrayedTreeNode";
                                                    childhNode.Selectable = false;

                                                    TreeView1.Nodes.Add(childhNode);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ComponentArt.Web.UI.TreeViewNode childhNode = CreatePublicNode(dbRow);

                                        childhNode.Text = GetResource("NoPermission");
                                        childhNode.ImageUrl = "folder-disable.gif";
                                        childhNode.CssClass = "NoActiveTreeNode";
                                        childhNode.HoverCssClass = "NoActiveTreeNode";
                                        childhNode.SelectedCssClass = "NoActiveTreeNode";
                                        childhNode.Selectable = false;

                                        TreeView1.Nodes.Add(childhNode);
                                    }
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
            UID = GetDecryptParam("UID", "0").ToNumber<int>();
            PUID = GetDecryptParam("PUID", "0").ToNumber<int>();
            PAdmin = GetDecryptParam("padmin", "0").ToNumber<int>();
            Attach = GetDecryptParam("Attach", "N");
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-11-07
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
        /// 작성일자 : 2011-11-07
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

                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                    _dr["NameFr"].ToString(), _dr["NameZhcn"].ToString(), _dr["NameZhtw"].ToString(), PreLang);

                name = (name == "ROOT") ? GetResource("PrivateFolderName") : name;
                node.Text = name;
                node.ID = _dr["FolderID"].ToString(); //목록 고유값
                node.Expanded = false;	//확장여부
                node.ImageUrl = "shared.gif";
                node.Attributes["ParentFolderID"] = _dr["ParentFolderID"].ToString();
                node.Attributes["Type"] = "S";
                node.Attributes["UserID"] = UID.ToString();                

                if (Attach == "Y")
                {
                    if ((_dr["Authority"].ToString() == "0") || (_dr["Authority"].ToString() == "2"))
                    {
                        node.CssClass = "GrayedTreeNode";
                        node.HoverCssClass = "GrayedTreeNode";
                        node.SelectedCssClass = "GrayedTreeNode";
                        node.Selectable = false;
                    }
                }
                else
                {
                    if ((_dr["Authority"].ToString() == "0") || (_dr["Authority"].ToString() == "1"))
                    {
                        node.CssClass = "GrayedTreeNode";
                        node.HoverCssClass = "GrayedTreeNode";
                        node.SelectedCssClass = "GrayedTreeNode";
                        node.Selectable = false;
                    }
                }
                

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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WHD-CreatePublicNode", ex);
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