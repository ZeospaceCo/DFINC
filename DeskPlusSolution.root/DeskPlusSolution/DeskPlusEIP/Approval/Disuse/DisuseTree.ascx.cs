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

namespace Zeospace.DeskPlus.Approval.Web.Disuse
{
    public partial class DisuseTree : Base.BaseControlApproval
    {
        #region 변수설정
        private int CompanyID;
        private int UserID;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Disuse.DisuseTree).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2013-11-25
        /// 작성목적 : 웹하드 트리
        /// </summary>
        public void InitTree()
        {
            tvDisuseTree.Nodes.Clear();
            tvDisuseTree.CollapseNodeOnSelect = false;
            tvDisuseTree.ExpandNodeOnSelect = false;
            tvDisuseTree.MultipleSelectEnabled = false;
            tvDisuseTree.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            tvDisuseTree.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            tvDisuseTree.DragAndDropEnabled = false;

            // 회사 정보 가져오기            
            CompanyID = CurrentUserInfo.CompanyID;	//회사고유값

            Zeospace.DeskPlus.Approval.DAL.Form dal = new DAL.Form();
            DataSet ds = new DataSet();

            ds = dal.GetTree(CompanyID);            

            //결재양식
            ComponentArt.Web.UI.TreeViewNode node = CreateRootNode();
            tvDisuseTree.Nodes.Add(node);

            foreach (DataRow dbRow in ds.Tables["GetDisuseTree"].Rows)
            {
                //하위트리 
                ComponentArt.Web.UI.TreeViewNode childhNode = CreateNode(dbRow);
                node.Nodes.Add(childhNode);                         
            }                

            //페이지 시작시 노드 선택
            this.tvDisuseTree.SelectedNode = this.tvDisuseTree.Nodes[0];         
        }       

        #endregion

        #region Method (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-25
        /// 작성목적 : 최상위 루트 트리 만들기
        /// </summary>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode()
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                node.ID = "0"; //목록 고유값
                node.Expanded = true;	//확장여부                
                node.ImageUrl = "folders.gif";
                node.Text = GetResource("Form2");  //"결재양식";                
               
                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Disuse-CreateRootNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-11-25
        /// 작성목적 : 자식 노드 가져오기
        /// </summary>
        /// <param name="_dr"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr)
        {
            try
            {
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

                node.ID = _dr["FormID"].ToString(); //목록 고유값
                node.Expanded = false;	//확장여부
                node.ImageUrl = "form.gif";

                string name = this.GetLangName(_dr["FormBase"].ToString(), _dr["FormKo"].ToString(),
                    _dr["FormEn"].ToString(), _dr["FormJa"].ToString(), _dr["FormFr"].ToString(),
                    _dr["FormZhcn"].ToString(), _dr["FormZhtw"].ToString(), PreLang);

                node.Text = name;                

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Disuse-CreateNode", ex);
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