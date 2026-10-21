using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using System.Text;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Task.DAL;

namespace Zeospace.DeskPlus.Task.Web.Admin.Menu
{
    public partial class MoveSubUpdown : Base.BasePageTaskAdmin
    {
        #region 변수설정
        public string ItemSubList = "";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Admin.Menu.MoveSubUpdown).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩)
        private void LoadData()
        {

           
            StringBuilder sb = new StringBuilder();

            DAL.ItemSub itemSub = new ItemSub();

            var itemlist = itemSub.GetItemSub(CurrentUserInfo.CompanyID, ItemID, false);

            foreach (var info in itemlist)
            {

                string ItemName = GetLangName(info.NameBase.ToNull(), info.NameKo.ToNull(),
                     info.NameEn.ToNull(), info.NameJa.ToNull(),
                     info.NameFr.ToNull(), info.NameZhcn.ToNull(),
                     info.NameZhtw.ToNull(), PreLang);

                sb.Append("<li id='");
                sb.Append(info.ItemSubID.ToString());
                sb.Append("'>");
                sb.Append(ItemName);
                sb.Append("</li>");
                
                ItemID = info.ItemID;
            }

            ItemSubList = sb.ToString();

        }
        #endregion

        #region 함수
        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("BtnConfirm");      
                cbtnClose.Text = resMng.GetString("Close");
                ltlTitle.Text = resMng.GetString("ManageItemSub"); 
            }
        }
        #endregion
    }
}