using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;

namespace Zeospace.DeskPlus.Pim.Web.Admin.Holiday
{
    public partial class List : Base.BasePagePimAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
                
        public string SearchField = "";
        public string Searchkey = "";     
        
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;
            
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Admin.Holiday.List).Assembly;	//어셈블리 선언
            #endregion     

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
                SettingControl();                
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = GetResource("ADD");
                CbtnMySchedule.Text = GetResource("MYSCHEDULE");
            }
        }


        private void SettingControl()
        {
             
            //divCategory
            //일정
            //할일
            ListItem ltholy1 = new ListItem(GetResource("holy2"), "G,M,K,N,A");
            ListItem ltholy2 = new ListItem(GetResource("holy1"), "G,M");            
            ListItem ltholy3 = new ListItem(GetResource("holy3"), "G");
            ListItem ltholy4 = new ListItem(GetResource("holy4"), "M");
            ListItem ltholy5 = new ListItem(GetResource("holy5"), "K");
            //ListItem ltholy6 = new ListItem(GetResource("holy6"), "N");
            //ListItem ltholy7 = new ListItem(GetResource("holy7"), "A");


            ddlCategory.Items.Add(ltholy1);
            ddlCategory.Items.Add(ltholy2);
            ddlCategory.Items.Add(ltholy3);
            ddlCategory.Items.Add(ltholy4);
            ddlCategory.Items.Add(ltholy5);
            //ddlCategory.Items.Add(ltholy6);
            //ddlCategory.Items.Add(ltholy7);

            /*
            int iYear=DateTime.Now.AddYears(3).Year;

            for (int i = 0; i < 10; i++)
            {
                ListItem ltYear = new ListItem(iYear.ToString(), iYear.ToString());
                iYear--;
                ddlSearchYear.Items.Add(ltYear);
            }
            */

            DAL.Schedule dal = new DAL.Schedule();
            DataTableReader dtr = dal.GetYear(CurrentUserInfo.CompanyID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ddlSearchYear.Items.Add(new ListItem(dtr["StartYear"].ToString(), dtr["StartYear"].ToString()));                                            
                    }
                }
            }

            if (ddlSearchYear.Items.Count == 0)
            {
                ddlSearchYear.Items.Add(new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));                                            
            }            

            if (SearchField != "")
            {
                ddlSearchYear.SelectedValue = SearchField;
            }
            else
            {
                ddlSearchYear.SelectedValue = DateTime.Now.Year.ToString();
            }

            if (Searchkey != "")
            {
                ddlCategory.SelectedValue = Searchkey;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadParam()
        {
            SearchField = GetDecryptParam("SearchField", "");
            Searchkey = GetDecryptParam("Searchkey", "");
        }

        #endregion

        #region (함수)





        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)





        #endregion
    }
}