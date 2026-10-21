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
    public partial class MoveUpdown : Base.BasePageTaskAdmin
    {

        #region 변수설정        
        public string ItemList = "";
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Admin.Menu.MoveUpdown).Assembly;	//어셈블리 선언
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
           

            DAL.Item item = new Item();
            
            var itemlist = item.GetItem(CurrentUserInfo.CompanyID, false);            

            foreach (var info in itemlist)
            {
                
               string ItemName = GetLangName(info.NameBase.ToNull(), info.NameKo.ToNull(),
                    info.NameEn.ToNull(),info.NameJa.ToNull(),
                    info.NameFr.ToNull(), info.NameZhcn.ToNull(),
                    info.NameZhtw.ToNull(), PreLang);

                sb.Append("<li id='");
                sb.Append(info.ItemID.ToString());
                sb.Append("'>");
                sb.Append(ItemName);
                sb.Append("</li>");
            }

            ItemList = sb.ToString();
            
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
                cbtnConfirm.Text = resMng.GetString("BtnConfirm");       //"적용";
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
                ltlTitle.Text = resMng.GetString("ManageItem"); 
            }
        }
        #endregion

        #region 속성

        #endregion

    }
}