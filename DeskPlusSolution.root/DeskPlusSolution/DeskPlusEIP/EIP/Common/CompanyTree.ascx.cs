using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Zeospace.DeskPlus.EIP.Web.Common
{
    public partial class CompanyTree : Base.BaseControlEIP
    {
        #region Declaration (컨트롤 선언)                

        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 페이지 로드
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스
            ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Common.CompanyTree).Assembly;	//어셈블리 선언
            GetResourceManager();  
            #endregion

            // 여기에 사용자 코드를 배치하여 페이지를 초기화합니다.
            if (!IsPostBack) InitTree();
        }

        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 트리 초기화
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <remarks></remarks>
        private void InitTree()
        {
            #region 트리 초기화
            CompanyTreeScript.Nodes.Clear();
            CompanyTreeScript.CollapseNodeOnSelect = false;
            CompanyTreeScript.ExpandNodeOnSelect = false;
            CompanyTreeScript.MultipleSelectEnabled = false;
            CompanyTreeScript.ImagesBaseUrl = BaseImgPath + "/Component/tree/";
            CompanyTreeScript.LineImagesFolderUrl = this.BaseImgPath + "/Component/tree/lines";
            #endregion

            #region 변수 초기화            

            #endregion

            LoadTree();
        }


        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 1단계 트리 생성
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-23
        /// </summary>
        /// <param name="_catID">The _cat ID.</param>
        /// <param name="_parentCategoryID">The _parent category ID.</param>
        /// <remarks></remarks>
        private void LoadTree()
        {
            string uriString = "/DeskPlusEIP/EIP/App_Data/Admin/Menu/" + this.PreLang + ".xml";

            //슈퍼관리자
            if (CurrentUserInfo.IsSysAdmin)
            {
                uriString = "/DeskPlusEIP/EIP/App_Data/Admin/Menu/Admin_" + this.PreLang.ToLower() + ".xml";
            }

            uriString = Server.MapPath(uriString);
            DataSet ds = new DataSet();
            ds.ReadXml(uriString);

            CompanyTreeScript.DataSource = ds;
            CompanyTreeScript.DataBind(); 
        }

        #endregion

        #region Method (함수)

        #endregion

        #region Property (프로퍼티)        

        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}