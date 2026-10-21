using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web.Content
{
    public partial class Send : Base.BasePageAddress
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string ListID = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Content.Send).Assembly;	//어셈블리 선언
            #endregion


            if (!IsPostBack)
            {
                #region 리소스 처리
                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {
                    CbtnSubmit.Text = resMng.GetString("Confirm");      // 확인
                    CbtnClose.Text = resMng.GetString("Close");         // 닫기
                    CbtnAdd.Text = resMng.GetString("Add");             // 추가
                }
                #endregion

                #region 이미지 처리
                //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
                #endregion

                #region 초기화
                LoadData();
                #endregion
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// Gets the param.
        /// </summary>
        private void GetParam()
        {
            ListID = GetDecryptParam("listID", "");           
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            GetParam();

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseCheckBoxMember = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseCheckBoxGroup = false;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;
        }
        #endregion

        #region (함수)

        
        #endregion

        #region (프로퍼티)

        #endregion


        #region (이벤트핸들러)

        #endregion
    }
}