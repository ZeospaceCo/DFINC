using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class Weather : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region 변수설정
        public string NowDate = string.Empty;
        public int ListCount;
        public string ScriptJS = "";

        public string CitySelect = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.Weather).Assembly;	//어셈블리 선언
            #endregion 


            if (!IsPostBack)
            {
                SetLanguage();
                AddItem();
                LoadData();
            }
        }

        private void AddItem()
        {
            ddlWeather.Items.Clear();
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Seoul"), "Seoul"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Incheon"), "Incheon"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Daejeon"), "Daejeon"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Daegu"), "Daegu"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Kwangju"), "Kwangju"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Busan"), "Busan"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Jeju"), "Jeju"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Kangwon"), "Kangwon"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Chungbuk"), "Chungbuk"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Chungnam"), "Chungnam"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Jeonbuk"), "Jeonbuk"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Jeonnam"), "Jeonnam"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Kyungbuk"), "Kyungbuk"));
            ddlWeather.Items.Add(new ListItem(this.resMng.GetString("Weather_Kyungnam"), "Kyungnam"));


        }

        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
               
            }
        }

        public void LoadData()
        {
            DAL.Weather dal = new DAL.Weather();
            DataTableReader dtr = dal.GetUserWeather(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ddlWeather.SelectedValue = dtr["City"].ToString();
                        CitySelect = dtr["City"].ToString();
                    }
                }
            }

            
        }


    }
}