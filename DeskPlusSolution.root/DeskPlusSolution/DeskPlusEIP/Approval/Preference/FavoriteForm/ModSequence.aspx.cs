using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;

namespace Zeospace.DeskPlus.Approval.Web.Preference.FavoriteForm
{
    public partial class ModSequence : Base.BasePageApproval
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public string FolderList = "";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";   //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.FavoriteForm.ModSequence).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParm();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParm()
        {
        }
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("Confirm");
                cbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            StringBuilder sb = new StringBuilder();

            DAL.FavoriteForm dal = new DAL.FavoriteForm();
            DataTableReader dtr = dal.GetFavoriteFormList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    string name = "";
                    int cnt = 0;
                    while (dtr.Read())
                    {
                        cnt++;
                        name = GetLangName(dtr["FormBase"].ToString(), dtr["FormKo"].ToString(), dtr["FormEn"].ToString(), dtr["FormJa"].ToString(), dtr["FormFr"].ToString(), dtr["FormZhcn"].ToString(), dtr["FormZhtw"].ToString(), PreLang);
                        sb.Append("<li id='");
                        sb.Append(dtr["FavoriteFormID"].ToString());
                        sb.Append("'>");
                        sb.Append(cnt.ToString());
                        sb.Append(". ");
                        sb.Append(name);
                        sb.Append("</li>");
                    }
                }
            }

            FolderList = sb.ToString();
        }
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion  
    }
}