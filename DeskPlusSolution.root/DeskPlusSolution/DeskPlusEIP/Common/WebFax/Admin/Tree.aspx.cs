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

namespace Zeospace.DeskPlus.Common.Web.WebFax.Admin
{
    public partial class Tree : Base.BasePageCommon
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.WebFax.Admin.Tree).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                #region 리소스 처리
                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {

                }
                #endregion

                #region 이미지 처리
                tvFaxTree.ImagesBaseUrl = this.BaseImgPath + "/Component/tree/";
                tvFaxTree.LineImagesFolderUrl = BaseImgPath + "/Component/tree/lines";
                #endregion

                #region 초기화
                InitTree();
                #endregion
            }
        }


        #region DataLoad (컨트롤이 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Inits the tree.
        /// </summary>
        public void InitTree()
        {
            bool isAdmin = CurrentUserInfo.IsInRole("webfax");//webfax admin
            if (!isAdmin)
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }

            tvFaxTree.CollapseNodeOnSelect = false;
            tvFaxTree.ExpandNodeOnSelect = false;
            tvFaxTree.MultipleSelectEnabled = false;


            string nodeName = GetResource("FaxNumber");

            ComponentArt.Web.UI.TreeViewNode rootNode = new ComponentArt.Web.UI.TreeViewNode();
            ComponentArt.Web.UI.TreeViewNode topNode = null;

            topNode = CreateRootNode(nodeName, "1", "1", true);            
            this.tvFaxTree.Nodes.Add(topNode);

            nodeName = GetResource("FaxWebFaxNumber");

            topNode = CreateRootNode(nodeName, "2", "2", true);
            this.tvFaxTree.Nodes.Add(topNode);

        }


        

        #endregion

        #region Method (함수)        

        

        #region 미사용

        /// <summary>
        /// 최상위루트 트리 만들기
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_id"></param>
        /// <param name="_value"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(string _text, string _id, string _value, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text.ToReverseXSS();					//목록명
            node.ID = _id;						//목록 고유값
            node.Expanded = _expanded;			//확장여부

            /*
             * 1. CompanyID
             * 2. UserID
             * 3. parentOwnerID
             * 4. Orders
             * 5. Name,
             * 6. Type
             * 7. TitleID
             * 8. TitleName
             * 9. DutyID
             * 10. DutyName
             * 11. LoginID
             * 12. Mail
             * 13. Phone
             * 14. basedept
             */

            node.Value = _id;
            node.Style.Add("font-style", "bold");
            node.ShowCheckBox = false;

            node.ImageUrl = "folder.gif";     // ImagePath + "/Component/TreeView/Root.gif";

            return node;
        }

        #endregion

        #endregion

        #region Property (프로퍼티)        

        #endregion

        #region Event Handler (이벤트 핸들러)



        #endregion


    }
}