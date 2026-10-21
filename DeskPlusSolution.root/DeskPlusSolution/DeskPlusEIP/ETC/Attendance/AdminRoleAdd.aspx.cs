using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.ETC.Web.Attendance
{
    public partial class AdminRoleAdd : Base.BasePageETC
    {
        #region 변수설정
        public int CatID = 0;                   //최상위 트리 나올 카테고리
        public int CategoryID = 0;              //포커스 설정할 트리 카테고리
        public int pCategoryID = 0;             //상위부서 카테고리
        public string AuthArrayList = "";
        public string AuthTable = "";
        public int CompanyID = 0;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.ETC.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.ETC.Web.Attendance.AdminRoleAdd).Assembly;	//어셈블리 선언
            #endregion

            CompanyID = CurrentUserInfo.CompanyID;

            if (!IsPostBack)
            {
                #region 이미지 처리
                CbtnAdd.ImagePath = BaseImgPath + "/Lang/" + this.PreLang + "/ico_folderset_add.gif";
                #endregion

                #region 초기화
                SetLanguage();

                DeptTreeScript1.CompanyID = CurrentUserInfo.CompanyID;
                DeptTreeScript1.DeptID = CurrentUserInfo.DeptID;
                DeptTreeScript1.UseActiveDept = true;
                DeptTreeScript1.UseIcon = true;
                DeptTreeScript1.UseCheckBox = true;
                #endregion
            }
        }

        #endregion

        #region Method (함수)

        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2018-01-18
        /// 작성목적 : 다국어
        /// </summary>
        /// <param name="_date"></param>
        /// <returns></returns>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = GetResource("Confirm");
                CbtnClose.Text = GetResource("Close");                
            }
        }
        #endregion

        #region Event Handler (이벤트 핸들러)
        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}