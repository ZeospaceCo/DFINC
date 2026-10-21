using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class IFrame : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.IFrame).Assembly;	//어셈블리 선언
            #endregion


            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        public void LoadData()
        {
            //환경설정에 추가한 내용이 있으면 해당 사항으로 정보 변경
            //타이틀, 리스트 갯수


            /*  기본적인것은 넘어오는데 추가적으러 더 필요한 사항이 있으면 사용.
            Zeospace.DeskPlus.EIP.DAL.Portlet dal = new Zeospace.DeskPlus.EIP.DAL.Portlet();
            List<Zeospace.DeskPlus.EIP.BE.PortletInfo> al = dal.GetPortlet(CurrentUserInfo.CompanyID, categoryID, portletID);


            foreach (Zeospace.DeskPlus.EIP.BE.PortletInfo info in al)
            {

            }
             * */

        }
        #endregion

        #region (함수)
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }
        #endregion

        #region (프로퍼티)
        
        #endregion
    }
}