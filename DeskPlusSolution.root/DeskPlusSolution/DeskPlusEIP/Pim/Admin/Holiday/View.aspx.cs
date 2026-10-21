using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Xml;
using System.Collections.Specialized;

namespace Zeospace.DeskPlus.Pim.Web.Admin.Holiday
{
    public partial class View : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        public int ScheduleID = 0;
        public int DisplayID = 0;        
        public string SearchField = "";
        public string Searchkey = "";      
        private string eventArgs = string.Empty;        
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Admin.Holiday.View).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion


        #region Method (함수)
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnEdit.Text = GetResource("Edit");
                cbtnDel.Text = GetResource("Del");
                CbtnCancel.Text = GetResource("Catalog");

            }

        }

        private void LoadData()
        {
            LoadParam();
 

            #region 권한체크
            #endregion

            #region 내용가져오기

            BE.ScheduleInfo info = new BE.ScheduleInfo();
            if (DisplayID > 0)
            {
                DAL.Display daldis = new DAL.Display();
                BE.DisplayInfo disinfo = new BE.DisplayInfo();
                disinfo.CompanyID = CompanyID;
                disinfo.DisplayID = DisplayID;
                DataTableReader ddtr = daldis.GetDisplayInfo(disinfo);
                if (ddtr != null)
                {
                    if (ddtr.HasRows)
                    {
                        while (ddtr.Read())
                        {
                            ScheduleID = ddtr["ScheduleID"].ToString().ToNumber<int>();

                        }

                    }
                }

            }


            info.CompanyID = this.CompanyID;
            info.ScheduleID = this.ScheduleID;

            DAL.Schedule dal = new DAL.Schedule();

            DataTableReader dtr = dal.GetSheduleSingleID(info);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lblTitle.Text = dtr["Title"].ToString();
                        //lblDate.Text = DateTime.Parse(dtr["StartTime"].ToString()).ToDateTimeLongString() + "~" + DateTime.Parse(dtr["EndTime"].ToString()).ToDateTimeLongString();                        
                        lblDateStr.Text = DateTime.Parse(dtr["StartTime"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["StartTime"].ToString()).ToShortTimeString() + " ~ " +
                             DateTime.Parse(dtr["EndTime"].ToString()).ToShortDateString() + " " + DateTime.Parse(dtr["EndTime"].ToString()).ToShortTimeString();
                        lbContent.Text = dtr["Content"].ToString();
                        lblWriter.Text = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                            , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                        //if(dtr["UserID"].ToString().ToNumber<int>() != CurrentUserInfo.UserID)
                        //{
                            //CbtnEdit.Visible = false;
                            //cbtnDel.Visible = false;
                        //}
                    }
                }
            }

            if (PageScope == "ListDay.aspx" || PageScope == "ListMonth.aspx" || PageScope == "ListWeek.aspx")
            {
                CbtnEdit.Visible = false;
                cbtnDel.Visible = false;
            }

            #endregion


        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();
            DisplayID = GetDecryptParam("DisplayID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("SearchField", "");
            Searchkey = GetDecryptParam("Searchkey", "");       
        }


        #endregion


        #region Event Handler (이벤트 핸들러)
        #endregion 



    }
}