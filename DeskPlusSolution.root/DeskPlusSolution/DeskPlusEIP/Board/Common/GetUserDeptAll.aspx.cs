using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Board.Web.Common
{
    public partial class GetUserDeptAll : Base.BasePageBoard
    {
        #region 변수처리
        public string CallBackEvent = "";
        public string Gubun = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Common.GetUserDeptAll).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            bool group = false;
            bool member = false;

            if (Gubun == "1")
            {
                group = true;
            }
            else if (Gubun == "2")
            {
                member = true;
            }
            else
            {
                group = true;
                member = true;
            }

            DeptUserTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
            DeptUserTreeScript1.DeptID = CurrentUserInfo.DeptID;
            DeptUserTreeScript1.UseActiveDept = true;
            DeptUserTreeScript1.UseIcon = true;
            DeptUserTreeScript1.UseMyRootDept = false;
            DeptUserTreeScript1.UseVirtualTree = false;
            DeptUserTreeScript1.UseRootCheckBox = false;
            DeptUserTreeScript1.UseMultiCompany = true;

            DeptUserTreeScript1.UseCheckBoxGroup = group;
            DeptUserTreeScript1.UseCheckBoxMember = member;
        }


        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-02-08
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = resMng.GetString("Check");       //"확인";
                CbtnClose.Text = resMng.GetString("Close");       //"닫기";
                CbtnAdd.Text = resMng.GetString("Add");       //"추가";   
            }

            //CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
        }


        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-02-08
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CallBackEvent = GetDecryptParam("callBackEvent", "0");
            Gubun = GetDecryptParam("gubun", "");
        }

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}