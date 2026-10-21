using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text;
using ComponentArt.Web.UI;

namespace Zeospace.DeskPlus.Approval.Web.Common.FormFlow
{
    public partial class GetFormTreeScript : Base.BasePageApproval
    {
        int CompanyID;
        int ParentID;
        string Section;
        string FolderAuth;
        bool UseActiveForm = true;
        bool UseCheckBox = false;
        bool UseAdmin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";					//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.FormFlow.GetFormTreeScript).Assembly;						//어셈블리 선언
            this.GetResourceManager();
            #endregion

            ComponentArt.Web.UI.TreeView tv = new ComponentArt.Web.UI.TreeView();

            CompanyID = this.GetDecryptParam("cID", "0").ToNumber<int>();
            ParentID = this.GetDecryptParam("pID", "0").ToNumber<int>();
            Section = this.GetDecryptParam("section", "");
            FolderAuth = this.GetDecryptParam("folderauth", "");
            UseActiveForm = Convert.ToBoolean(this.GetDecryptParam("useActiveForm", "true"));
            UseCheckBox = Convert.ToBoolean(this.GetDecryptParam("useCheckBox", "false"));
            UseAdmin = Convert.ToBoolean(this.GetDecryptParam("useAdmin", "false"));

            if (CompanyID > 0 && ParentID > 0 && !Section.Equals(""))
            {
                DAL.Folder fDal = new DAL.Folder();
                DAL.Form foDal = new DAL.Form();

                #region 하위폴더

                var isactive = (UseAdmin) ? false : true;   //관리자페이지에서는 비활성화 폴더도 나옴.(ex.결재문서)

                DataSet dsFolder = fDal.GetSubFolderList("dsList", CompanyID, ParentID, isactive);
                string tmpauth = "";
                bool isauth = true;
                foreach (DataRow dr in dsFolder.Tables[0].Rows)
                {
                    if (Section.ToUpper().Equals("D"))
                    {
                        tmpauth = "," + dr["FolderID"].ToString() + ",";
                        isauth = (FolderAuth.IndexOf(tmpauth) < 0) ? false : true;
                    }

                    ComponentArt.Web.UI.TreeViewNode tmpNode = CreateNode(
                        dr["FolderID"].ToString(), dr["ParentID"].ToString()
                        , Section, "G"
                        , GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang)
                        , false, isauth, dr["ChildCnt"].ToString().ToNull().ToNumber<int>());
                    tv.Nodes.Add(tmpNode);
                }
                #endregion

                #region 해당양식
                isauth = true;
                if (Section.ToUpper().Equals("D"))
                {
                    isauth = (FolderAuth.IndexOf("," + ParentID.ToString() + ",") < 0) ? false : true;
                }
                if (isauth)
                {
                    DataSet dsCForm = foDal.GetForm("dsFormList", CurrentUserInfo.CompanyID, ParentID, UseActiveForm);
                    foreach (DataRow dr in dsCForm.Tables[0].Rows)
                    {
                        ComponentArt.Web.UI.TreeViewNode tmpNode = CreateNode(
                            dr["FormID"].ToString(), dr["FolderID"].ToString()
                            , Section, "M"
                            , GetLangName(dr["FormBase"].ToString(), dr["FormKo"].ToString(), dr["FormEn"].ToString(), dr["FormJa"].ToString(), dr["FormFr"].ToString(), dr["FormZhcn"].ToString(), dr["FormZhtw"].ToString(), PreLang)
                            , false, isauth, 1);
                        tv.Nodes.Add(tmpNode);
                    }
                }
                #endregion

                Response.Write(tv.GetXml());
            }
        }

        /// <summary>
        /// 노드생성
        /// </summary>
        /// <param name="_id">아이디</param>
        /// <param name="_pid">부모아이디</param>
        /// <param name="_section">그룹핑노드정보(공통-C, 자사-D, 임시저장-T)</param>
        /// <param name="_type">폴더여부(폴더-G, 노드-M)</param>
        /// <param name="_name">노드명</param>
        /// <param name="_expanded">확장여부</param>
        /// <param name="_isAuth">권한여부</param>
        /// <param name="_childCnt">The _child CNT.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(string _id, string _pid, string _section, string _type, string _name, bool _expanded, bool _isAuth, int _childCnt)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _name.ToReverseXSS();
            node.ID = _section + "-" + _type + "-" + _id;              // 루트노드정보(Section) + '-' + 폴더/양식여부 + '-'  + FolderID/FormID

            node.Attributes["ID"] = _id;
            node.Attributes["PID"] = _pid;
            node.Attributes["Section"] = _section;
            node.Attributes["Type"] = _type;

            node.Expanded = _expanded;
            node.ShowCheckBox = (_type.Equals("M") && UseCheckBox) ? true : false;

            #region 권한에 따른 아이콘 처리
            if (_isAuth)
            {
                if (_pid.Equals("0"))
                {
                    node.ImageUrl = "groups.gif";
                }
                else
                {
                    if (_type.ToUpper().Equals("G"))
                    {
                        node.ImageUrl = "folders.gif";
                        node.ExpandedImageUrl = "folders.gif";
                    }
                    else
                    {
                        node.ImageUrl = "form.gif";
                    }
                }
            }
            else
            {
                node.Selectable = false;
                node.ImageUrl = "folder-disable.gif";
                node.ExpandedImageUrl = "folder_open-disable.gif";
            }
            #endregion

            if (_type.ToUpper().Equals("G") && _childCnt > 0)
            {
                #region CallBack처리
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("guid", System.Guid.NewGuid().ToString());
                paraDic.Add("cID", CurrentUserInfo.CompanyID.ToString());
                paraDic.Add("pID", _id);
                paraDic.Add("section", _section);
                paraDic.Add("folderauth", FolderAuth);
                paraDic.Add("useActiveForm", (UseActiveForm) ? "true" : "false");
                paraDic.Add("useCheckBox", (UseCheckBox) ? "true" : "false");

                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                node.ContentCallbackUrl = "/DeskplusEIP/Approval/Common/FormFlow/GetFormTreeScript.aspx?p=" + encParams;
                #endregion
            }

            return node;
        }

    }
}