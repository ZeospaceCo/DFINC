using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Account.Web.Admin.Dept
{
    public partial class MoveUpdown : Base.BasePageAccountAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정        
        public int DeptID;
        public string FolderList = "";        
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.Dept.MoveUpdown).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("BtnConfirm");                          //"적용";
                cbtnClose.Text = resMng.GetString("Admin_UserRegister_Close");         //"닫기";
                ltlTitle.Text = resMng.GetString("ChangeOrders");                    //순서변경
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {            
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            StringBuilder sb = new StringBuilder();


            Zeospace.DeskPlus.Account.DAL.Dept dal = new DAL.Dept();
            List<BE.DeptInfo> al = null;

            if (DeptID > 0)
            {
                al = dal.GetParentDeptLists(CurrentUserInfo.CompanyID, DeptID, false);
            }
            else if (DeptID == 0)
            {
                al = dal.GetTopDeptLists(CurrentUserInfo.CompanyID, false);
            }

            foreach (Zeospace.DeskPlus.Account.BE.DeptInfo info in al)
            {
                #region 다국어 이름 설정

                string name = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, 
                    info.NameZhcn, info.NameZhtw, PreLang);
                #endregion

                sb.Append("<li id='");
                sb.Append(info.DeptID.ToString());
                sb.Append("'>");
                sb.Append(name);
                sb.Append("</li>");
            }

            FolderList = sb.ToString();
        }

        #endregion

        #region (속성)

        #endregion
    }
}