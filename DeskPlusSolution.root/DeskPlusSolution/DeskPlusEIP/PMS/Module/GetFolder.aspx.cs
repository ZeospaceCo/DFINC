using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ComponentArt.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Module
{
    public partial class GetFolder : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)        
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) LoadData();
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 초기화
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-07-03
        /// </summary>
        private void LoadData()
        {
            #region 사용자정보가져오기            
            #endregion

            if (GetDecryptParam("pID", "").Length > 0) LoadTree();
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : xml 생성
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-07-03
        /// </summary>
        private void LoadTree()
        {
            ComponentArt.Web.UI.TreeView tv = new ComponentArt.Web.UI.TreeView();

            DataSet ds = new DataSet();
            DAL.Folder bll = new Zeospace.DeskPlus.PMS.DAL.Folder();
            ds = bll.GetFolder(CurrentUserInfo.CompanyID, GetDecryptParam("pID", "0").ToNumber<int>());

            foreach (DataRow dbRow in ds.Tables[0].Rows)
            {
                ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow, false);
                tv.Nodes.Add(node);
            }

            Response.Write(tv.GetXml());
        }


        /// <summary>
        /// 트리노드 생성 : Creates the node.
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_expanded">_expanded.</param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {            
            string pFolderID = _dr["ParentFolderID"].ToString();
            pFolderID = (pFolderID.Length > 0) ? pFolderID : "0";

            string nodeText = this.GetLangName(
                                    _dr["FolderBase"].ToString(),
                                    _dr["FolderKo"].ToString(), _dr["FolderEn"].ToString(), _dr["FolderJa"].ToString(),
                                    _dr["FolderFr"].ToString(), _dr["FolderZhcn"].ToString(), _dr["FolderZhtw"].ToString(), PreLang).ToReverseXSS();

            //1. 카테고리값,  2. 타이틀           3. 하위메뉴값,             4. order            5. 첨부이미지본문표시유무 
            //6. 답변사용유무 7. 짧은답글사용유무 8. 조회자통계정보사용유무, 9. 부서게시판여부, 10. 권한여부(true/false,1:읽기/2:쓰기), 11, custom 페이지경로

            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
            node.Text = (!String.IsNullOrEmpty(nodeText)) ? nodeText : "No Title";
            node.ID = _dr["FolderID"].ToString();			//목록 고유값
            //node.Value							= nodeValue;
            node.Expanded = _expanded;

            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            node.ImageUrl = "folder.gif";    // BaseImgPath + "/Component/TreeView/Group.gif";
            paraDic.Add("UseIcon", "1");

            node.Style.Add("font-style", "bold");

            if (_dr["childCnt"].ToString().ToNumber<int>() > 0)
            {

                paraDic.Add("pID", _dr["FolderID"].ToString());
                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                node.ContentCallbackUrl = "/DeskPlusEIP/PMS/Module/GetFolder.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
            }


            StringBuilder sb = new StringBuilder();
            sb.Append(_dr["FolderID"].ToString());
            sb.Append("|");
            sb.Append(nodeText);
            sb.Append("|");
            sb.Append(pFolderID); //하위메뉴값
            sb.Append("|");
            sb.Append(_dr["Orders"].ToString());
            sb.Append("|");
            sb.Append(GetMuiltiLang(
                        _dr["FolderKo"].ToString(),
                        _dr["FolderEn"].ToString(),
                        _dr["FolderJa"].ToString(),
                        _dr["FolderFr"].ToString(),
                        _dr["FolderZhcn"].ToString(),
                        _dr["FolderZhtw"].ToString()).ToReverseXSS());
            sb.Append("|");
            sb.Append(_dr["FolderBase"].ToString().ToReverseXSS());

            node.Value = sb.ToString();

            return node;
        }



        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #endregion

        #region Method (함수)
        #endregion

        #region Property (프로퍼티)

        

        #endregion


        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}
