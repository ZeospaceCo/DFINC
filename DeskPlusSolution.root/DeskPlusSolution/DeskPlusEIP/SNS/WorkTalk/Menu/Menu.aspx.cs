using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.SNS.Web.WorkTalk.Menu
{
    public partial class Menu : Base.BasePageWorkTalk
    {
        #region 변수설정        
        //우측메뉴에서 갱신 요청하면 1
        public string ReLoad = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            ResourceName = "Zeospace.DeskPlus.SNS.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.SNS.Web.WorkTalk.Menu.Menu).Assembly;	//어셈블리 선언
            GetResourceManager();
            #endregion

            if (!IsPostBack)
            {
                ReLoad = GetDecryptParam("reLoad", "");
                LoadData();
            }
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2013-05-10
        /// 작성목적 : 업무톡 메뉴
        /// </summary>
        private void LoadData()
        {
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

            #endregion
            ComponentArt.Web.UI.TreeViewNode newNode = new ComponentArt.Web.UI.TreeViewNode();
            newNode.ID = "1";
            newNode.Text = resMng.GetString("WorkTalk");
            newNode.Expanded = true;
            newNode.ImageUrl = "groups.gif";
            tvTree.Nodes.Add(newNode);

            ComponentArt.Web.UI.TreeViewNode childNode = new ComponentArt.Web.UI.TreeViewNode();
            var workDal = new Zeospace.DeskPlus.SNS.DAL.WorkTalk();
            int cntRec = workDal.GetRecvPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
            childNode.ID = "2";
            childNode.Text = resMng.GetString("ReceiveTalk") + " (" + cntRec + ")";
            childNode.ImageUrl = "group.gif";
            newNode.Nodes.Add(childNode);

            int cntSend = workDal.GetSendPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
            childNode = new ComponentArt.Web.UI.TreeViewNode();
            childNode.ID = "3";
            childNode.Text = resMng.GetString("SendTalk") + " (" + cntSend + ")";
            childNode.ImageUrl = "group.gif";
            newNode.Nodes.Add(childNode);

            int cntApp = workDal.GetAppPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
            childNode = new ComponentArt.Web.UI.TreeViewNode();
            childNode.ID = "4";
            childNode.Text = resMng.GetString("ApprovalTalk") + " (" + cntApp + ")";
            childNode.ImageUrl = "group.gif";
            newNode.Nodes.Add(childNode);

            this.tvTree.SelectedNode = this.tvTree.Nodes[0].Nodes[0];
        }
    }
}