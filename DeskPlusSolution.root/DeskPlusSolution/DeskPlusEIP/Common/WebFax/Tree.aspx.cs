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

namespace Zeospace.DeskPlus.Common.Web.WebFax
{
    public partial class Tree : Base.BasePageCommon
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.WebFax.Tree).Assembly;	//어셈블리 선언
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
            tvFaxTree.CollapseNodeOnSelect = false;
            tvFaxTree.ExpandNodeOnSelect = false;
            tvFaxTree.MultipleSelectEnabled = false;

            string nodeName = GetResource("FaxSendFax");
            string pageUrl = "Register.aspx";

            ComponentArt.Web.UI.TreeViewNode rootNode = new ComponentArt.Web.UI.TreeViewNode();
            ComponentArt.Web.UI.TreeViewNode topNode = null;

            topNode = CreateRootNode(nodeName, "0", "0", true, pageUrl);
            this.tvFaxTree.Nodes.Add(topNode);

            nodeName = GetResource("FaxRecvCabinet");
            pageUrl = "RecvList.aspx";

            topNode = CreateRootNode(nodeName, "0", "0", false, pageUrl);
            this.tvFaxTree.Nodes.Add(topNode);
            CreateFaxGroupNodeTree(pageUrl, topNode);

            nodeName = GetResource("FaxSendCabinet");
            pageUrl = "SendList.aspx";

            topNode = CreateRootNode(nodeName, "0", "0", false, pageUrl);
            this.tvFaxTree.Nodes.Add(topNode);
            CreateDeptLvlNodeTree(pageUrl, topNode);

        }




        #endregion

        #region Method (함수)

        


        /// <summary>
        /// fax 그룹 가져오기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_gubun"></param>
        /// <param name="_parentNode"></param>
        private void CreateFaxGroupNodeTree(string _pageUrl, ComponentArt.Web.UI.TreeViewNode _parentNode)
        {
            DAL.FaxManager dal = new DAL.FaxManager();
            var dtr = dal.GetPageList(CurrentUserInfo.CompanyID, "100", "1", "", "", true, "2", "");

            bool isAdmin = CurrentUserInfo.IsInRole("webfax");//webfax admin

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if (isAdmin) //관리자
                        {
                            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
                            node.ID = dtr["FaxID"].ToString(); //목록 고유값
                            node.Text = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                            node.Attributes["BranchID"] = dtr["BranchID"].ToString();
                            node.Attributes["PageUrl"] = _pageUrl;

                            _parentNode.Nodes.Add(node);
                        }
                        else
                        {
                            if (CurrentUserInfo.DeptID.ToString() == dtr["BranchID"].ToString()) //개인별 지점
                            {
                                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
                                node.ID = dtr["FaxID"].ToString(); //목록 고유값
                                node.Text = this.GetLangName(
                                    dtr["NameBase"].ToString(),
                                    dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                    dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                                node.Attributes["BranchID"] = dtr["BranchID"].ToString();
                                node.Attributes["PageUrl"] = _pageUrl;

                                _parentNode.Nodes.Add(node);
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 부서 level 1 가져오기
        /// </summary>
        /// <param name="_pageUrl"></param>
        /// <param name="_parentNode"></param>
        private void CreateDeptLvlNodeTree(string _pageUrl, ComponentArt.Web.UI.TreeViewNode _parentNode)
        {
            //Zeospace.DeskPlus.Account.DAL.Dept dal = new Account.DAL.Dept();
            //var alList = dal.GetDeptLvl(CurrentUserInfo.CompanyID, 1);

            //foreach (var info in alList)
            //{
            //    ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            //    node.ID = info.DeptID.ToString();
            //    node.Text = this.GetLangName(
            //        info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);

            //    node.Attributes["DeptID"] = info.DeptID.ToString();
            //    node.Attributes["PageUrl"] = _pageUrl;

            //    _parentNode.Nodes.Add(node);
            //}

            bool isAdmin = CurrentUserInfo.IsInRole("webfax");//webfax admin

            Zeospace.DeskPlus.Common.DAL.Code codeDal = new DeskPlus.Common.DAL.Code();
            DataSet jDs = codeDal.GetCodeByScope(CurrentUserInfo.CompanyID, "J", true);

            foreach (DataRow _dr in jDs.Tables[0].Rows)  {

                if (isAdmin) //관리자
                {
                    ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
                    node.ID = _dr["CodeID"].ToString();

                    node.Text = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                            _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

                    node.Attributes["BranchID"] = _dr["CodeID"].ToString();
                    node.Attributes["PageUrl"] = _pageUrl;

                    _parentNode.Nodes.Add(node);
                }
                else
                {
                    if (CurrentUserInfo.DeptID.ToString() == _dr["CodeID"].ToString()) //개인별 지점
                    {
                        ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
                        node.ID = _dr["CodeID"].ToString();

                        node.Text = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

                        node.Attributes["BranchID"] = _dr["CodeID"].ToString();
                        node.Attributes["PageUrl"] = _pageUrl;

                        _parentNode.Nodes.Add(node);
                    }
                }
            }

        }



        /// <summary>
        /// 최상위루트 트리 만들기
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_id"></param>
        /// <param name="_value"></param>
        /// <param name="_expanded"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateRootNode(string _text, string _id, string _value,
            bool _expanded, string _pageUrl)
        {
            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = _text.ToReverseXSS();					//목록명
            node.ID = _id;						//목록 고유값
            node.Expanded = _expanded;			//확장여부
            node.Value = _id;
            node.Style.Add("font-style", "bold");
            node.ShowCheckBox = false;

            node.Attributes["BranchID"] = "0";
            node.Attributes["PageUrl"] = _pageUrl;

            node.ImageUrl = "folder.gif";     // ImagePath + "/Component/TreeView/Root.gif";

            return node;
        }

        

        #endregion

        #region Property (프로퍼티)


        #endregion

        #region Event Handler (이벤트 핸들러)



        #endregion
    }
}