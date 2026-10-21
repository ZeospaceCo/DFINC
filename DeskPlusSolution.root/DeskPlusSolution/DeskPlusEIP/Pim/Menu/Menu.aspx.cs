using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Pim.Web.Menu
{
    public partial class Menu : Base.BasePagePim
    {

        #region 변수설정
        public int CompanyID = 0;//최상위 트리 나올 카테고리
        public int UserID = 0;//포커스 설정할 트리 카테고리
        

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Menu.Menu).Assembly;	//어셈블리 선언
            #endregion

            
            
            #region 초기 데이터 로드
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #endregion

            #region 이미지 처리
            this.CalendarSingle.ImagesBaseUrl = this.BaseImgPath + "/pims/";

            //CbtnMonth.Icon

            #endregion

        }
    }
}