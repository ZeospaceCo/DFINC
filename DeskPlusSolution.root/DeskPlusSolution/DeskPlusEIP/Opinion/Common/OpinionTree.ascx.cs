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

namespace Zeospace.DeskPlus.Opinion.Web.Common
{
    public partial class OpinionTree : Base.BaseControlOpinion
    {
        #region 변수설정
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Common.OpinionTree).Assembly;	//어셈블리 선언
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
                tvOpinionTree.ImagesBaseUrl = this.BaseImgPath + "/Component/tree/";
                tvOpinionTree.LineImagesFolderUrl = BaseImgPath + "/Component/tree/lines";
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
            tvOpinionTree.CollapseNodeOnSelect = false;
            tvOpinionTree.ExpandNodeOnSelect = false;
            tvOpinionTree.MultipleSelectEnabled = false;

            // 회사 정보 가져오기            
            //CompanyID = CurrentUserInfo.CompanyID;	//회사고유값
            //UserID = CurrentUserInfo.UserID;		//사용자아이디

            this.tvOpinionTree.Nodes.Add(this.CreateNode("2010", "2010", "2010", true));
            this.tvOpinionTree.Nodes.Add(this.CreateNode("2011", "2011", "2011", true));
            this.tvOpinionTree.Nodes.Add(this.CreateNode("2012", "2012", "2012", true));
            this.tvOpinionTree.Nodes.Add(this.CreateNode("2013", "2013", "2013", true));

            //페이지 시작시 노드 선택
            this.tvOpinionTree.SelectedNode = this.tvOpinionTree.Nodes[0];
        }

        #endregion

        #region Method (함수)
        /// <summary>
        /// 최상위루트 트리 만들기
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_id"></param>
        /// <param name="_value"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(string _text, string _id, string _value, bool _expanded)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text;					//목록명
            node.ID = _id;						//목록 고유값
            node.Expanded = _expanded;			//확장여부
            node.Value = _value;

            node.Style.Add("font-style", "bold");
            node.ShowCheckBox = false;
            
            node.ImageUrl = "group.gif";     // ImagePath + "/Component/TreeView/Root.gif";
            return node;
        }
        #endregion

        #region Property (프로퍼티)
        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}