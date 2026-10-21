using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.Web.Admin.Category
{
    public partial class MoveUpdown : Base.BasePageCoopAdmin
    {
        #region 변수설정
        public string ItemList = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Admin.Category.MoveUpdown).Assembly;	//어셈블리 선언
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
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            StringBuilder sb = new StringBuilder();

            DAL.Item dal = new DAL.Item();
            var itemList = dal.GetItem(CurrentUserInfo.CompanyID, false);

            if (itemList != null)
            {
                foreach (var item in itemList)
                {
                    string name = this.GetLangName(
                        item.NameBase.ToNull(), item.NameKo.ToNull(),
                        item.NameEn.ToNull(), item.NameJa.ToNull(),
                        item.NameFr.ToNull(), item.NameZhcn.ToNull(),
                        item.NameZhtw.ToNull(), PreLang);

                    sb.Append("<li id='");
                    sb.Append(item.ItemID.ToString());
                    sb.Append("'>");
                    sb.Append(name);
                    sb.Append("</li>");
                }
            }

            ItemList = sb.ToString();
        }

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
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
                ltlTitle.Text = resMng.GetString("ManageItem"); //폴더관리
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            
        }

        #endregion
    }
}