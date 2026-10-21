using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Zeospace.DeskPlus.Account.Web.Admin.User
{
    public partial class UserUpDown : Base.BasePageAccountAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string FolderList = "";
        public string DeptID = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.UserUpDown).Assembly;	//어셈블리 선언
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
                cbtnConfirm.Text = resMng.GetString("BtnConfirm");       //"적용";
                cbtnClose.Text = resMng.GetString("BtnClose");         //"닫기";
                ltlTitle.Text = resMng.GetString("ChangeOrders"); //순서변경
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            DeptID = GetDecryptParam("deptID", "0");
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            StringBuilder sb = new StringBuilder();

            DAL.User dal = new DAL.User();
            var userList = dal.GetUserInfoByDept(CurrentUserInfo.CompanyID, DeptID.ToString(), false);
            string name = "";

            foreach (var info in userList)
            {
                #region 다국어 이름 설정
                name = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr,
                    info.NameZhCn, info.NameZhTw, PreLang);
                #endregion

                sb.Append("<li id='");
                sb.Append(info.UserID + "|" + info.DeptID);
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