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
namespace Zeospace.DeskPlus.Task.Web.Menu
{
    public partial class Setting : Base.BasePageTask
    {
        #region 변수
        public string UserItemList = "";
       

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Menu.Setting).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Data Load (데이터를 컨트롤에 바인딩 하는 메서드)
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2015-05-08
        /// 작성목적 : 로그인 한 사용자 대분류 감춰야될 항목 로드
        /// </summary>
        public void LoadData()
        {
            IsType = "RE"; //처음 로딩 값은 요청한 협업
            DAL.Menu dal = new DAL.Menu();
            DataTableReader dtr = dal.GetMenuSetting(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, IsType);           

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        UserItemList += dtr["ItemID"].ToString()+',';                        
                    }
                }
            }

        }
        #endregion

        #region 함수

        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
               
                ddlSetting.Items.Clear();
                ddlSetting.Items.Add(new ListItem(resMng.GetString("RequestCoWork"), "RE"));
                ddlSetting.Items.Add(new ListItem(resMng.GetString("RequestedCoWork"), "TO"));

                cbtnOk.Text = resMng.GetString("Apply");
                cbtnAll.Text = resMng.GetString("MenuSelectAll");
                cbtnUnAll.Text = resMng.GetString("MenuUnSelectAll");
            }
        }
        #endregion


    }
}