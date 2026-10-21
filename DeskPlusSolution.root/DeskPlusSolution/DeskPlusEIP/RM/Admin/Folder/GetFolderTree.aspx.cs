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

namespace Zeospace.DeskPlus.RM.Web.Admin.Folder
{
    public partial class GetFolderTree : Zeospace.DeskPlus.RM.Web.Base.BasePageRM
    {
        #region Declaration (컨트롤 선언)
        private int CompanyID;
        private int PID;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Admin.Folder.GetFolderTree).Assembly;	//어셈블리 선언
            #endregion

            #region 바인딩

            LoadParam();

            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            if (PID != 0)
            {
                try
                {
                    CompanyID = CurrentUserInfo.CompanyID;	//회사고유값                  

                    DataSet ds = new DataSet();

                    DAL.Category dal = new DAL.Category();
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
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("RM_GetFolderTree.aspx", ex);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                    throw;
                }

                //Response.Clear();
                //Response.ClearHeaders();
                //Response.ContentType = "text/xml";
                Response.Write(TreeView1.GetXml());
                //Response.End();
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
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-07
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

                string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                    _dr["NameFr"].ToString(), _dr["NameZhcn"].ToString(), _dr["NameZhtw"].ToString(), PreLang);

                node.Text = name;
                node.ID = _dr["CategoryID"].ToString(); //목록 고유값
                node.Expanded = false;	//확장여부
                node.ImageUrl = "folder.gif";
                node.Attributes["ParentCategoryID"] = _dr["ParentCategoryID"].ToString();

                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                //자식 노드가 있는 경우
                int childCnt = _dr["childCnt"].ToString().ToNumber<int>();
                if (childCnt > 0)
                {
                    paraDic.Add("PID", _dr["CategoryID"].ToString());
                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                    node.ContentCallbackUrl = "/DeskPlusEIP/RM/Admin/Folder/GetFolderTree.aspx?p=" + encParams;
                }                

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("RM-CreateNode", ex);
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