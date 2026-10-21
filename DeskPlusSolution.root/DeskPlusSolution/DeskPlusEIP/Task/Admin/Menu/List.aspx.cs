using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeospace.DeskPlus.Task.Web.Admin.Menu
{
    public partial class List : Base.BasePageTaskAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Admin.Menu.List).Assembly;	//어셈블리 선언
            #endregion
            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        private void LoadData()
        {

        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnAdd.Text = resMng.GetString("Add");
                cbtnDel.Text = resMng.GetString("Del");
                CbtnMove.Text = resMng.GetString("ChangeOrder");

                cbtnAddUser.Text = resMng.GetString("SpecifierAdd");
                cbtnToOrder.Text = resMng.GetString("ReceiverOrder");
                cbtnCcOrder.Text = resMng.GetString("ReferencerOrder");


                cbtnSubUserAdd.Text = resMng.GetString("SpecifierAdd");
                cbtnItemSubToUserUpdown.Text = resMng.GetString("ReceiverOrder");
                cbtnItemSubCcUserUpdown.Text = resMng.GetString("ReferencerOrder");

                cbtnModSub.Text = resMng.GetString("Mod");
                cbtnAddSub.Text = resMng.GetString("Add");
                cbtnDelSub.Text = resMng.GetString("Del");
                CbtnSubMove.Text = resMng.GetString("ChangeOrder");
            }
        }
    }
}