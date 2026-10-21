using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Address.Web.Folder
{
    public partial class Mod : Base.BasePageAddress
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Folder.Mod).Assembly;	//어셈블리 선언
            #endregion


            if (!IsPostBack)
            {
                #region 리소스 처리
                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {
                    CbtnSubmit.Text = resMng.GetString("Confirm");  // 확인
                    CbtnClose.Text = resMng.GetString("Close");     // 닫기
                    CbtnAdd.Text = resMng.GetString("Add");         // 추가
                }
                #endregion

                #region 이미지 처리
                CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
                #endregion

                #region 초기화

                #endregion
            }

        }
    }
}