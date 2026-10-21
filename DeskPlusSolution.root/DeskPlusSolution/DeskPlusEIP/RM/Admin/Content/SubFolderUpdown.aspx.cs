using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.Web.Admin.Content
{
    public partial class SubFolderUpdown : Base.BasePageRMAdmin
    {
        #region 변수설정

        public string FolderList = "";
        public int CategoryID = 0;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Admin.Content.SubFolderUpdown).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

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
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
                ltlTitle.Text = resMng.GetString("MoveSubFolder"); //하위폴더순서
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            
            StringBuilder sb = new StringBuilder();
            DAL.Category dal = new DAL.Category();
            DataSet ds = dal.GetChildTree(CurrentUserInfo.CompanyID, CategoryID);
                       

            if (ds != null)
            {
                foreach (DataRow dtr in ds.Tables[0].Rows)
                {
                    #region 다국어 이름 설정

                    string name = this.GetLangName(
                        dtr["NameBase"].ToString(),
                        dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                        dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang);

                    #endregion 다국어 이름 설정

                    sb.Append("<li id='");
                    sb.Append(dtr["CategoryID"].ToString());
                    sb.Append("'>");                    
                    sb.Append(name);
                    sb.Append("</li>");
                }
            }

            FolderList = sb.ToString();
        }

        #endregion (함수)
        
    }
}