using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Menu
{
    public partial class MoveUpdown : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string FolderList = "";
        public int MenuID;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Menu.MoveUpdown).Assembly;	//어셈블리 선언
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
                cbtnConfirm.Text = resMng.GetString("Confirm");       //"적용";
                cbtnClose.Text = resMng.GetString("Closed");         //"닫기";
                ltlTitle.Text = resMng.GetString("SortList"); //순서변경
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            StringBuilder sb = new StringBuilder();

            DAL.Menu dal = new DAL.Menu();
            DataSet ds = dal.GetMenuByPID(CurrentUserInfo.CompanyID, MenuID.ToString());            

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string name = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                        dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), PreLang);

                    sb.Append("<li id='");
                    sb.Append(dr["MenuID"].ToString());
                    sb.Append("'>");
                    sb.Append(name);
                    sb.Append("</li>");
                }
            }

            FolderList = sb.ToString();
        }

        #endregion

        #region (속성)

        #endregion
    }
}