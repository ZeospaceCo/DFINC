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

namespace Zeospace.DeskPlus.Pim.Web.Anniversary
{
    public partial class view : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        public int ScheduleID = 0;
        public int CurrentPage = 0;        
        public string Searchkey = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string Order = string.Empty; //sort 
        public string Starttime = string.Empty; //검색시작일
        public string Endtime = string.Empty; //검색시작일
         

        private string eventArgs = String.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            cbtnDel.Visible = false;
            CbtnEdit.Visible = false;

            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Anniversary.view).Assembly;	//어셈블리 선언
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
            CbtnEdit.Text = GetResource("Edit");
            cbtnDel.Text = GetResource("Del");
            CbtnPrint.Text = GetResource("PRINT");
            CbtnCancel.Text = GetResource("Catalog");
        }

        private void LoadData()
        {
            LoadParam();

            if (ScheduleID == 0)
                return;

            #region 권한체크
            #endregion

            #region 내용가져오기

            BE.ScheduleInfo info = new BE.ScheduleInfo();
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
                       if (dtr["UserID"].ToString().ToNumber<int>() == CurrentUserInfo.UserID)
                       {
                           cbtnDel.Visible = true;
                           CbtnEdit.Visible = true;
                       }
                       else
                       {
                           cbtnDel.Visible = false;
                           CbtnEdit.Visible = false;
                       }
                       lblTitle.Text = dtr["Title"].ToString();

                       lblDateStr.Text = DateTime.Parse(dtr["ScStartTime"].ToString()).ToShortDateString();

                       if (dtr["lunar"].ToString().Equals("N"))
                       {
                           lblDateStr.Text += "(" + GetResource("sunday") + ")";
                       }
                       else
                       {
                           lblDateStr.Text += "(" + GetResource("lunalday") + ")";
                       }

                       lblContent.Text = dtr["Content"].ToString();
                       lblWriter.Text = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString()
                            , dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                   }
                 }
            }
             
            #endregion


        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            ScheduleID = GetDecryptParam("ScheduleID", "0").ToNumber<int>();
            Searchkey = GetDecryptParam("Searchkey", "");
            SearchValue = GetDecryptParam("SearchValue", "");
            CurrentPage = GetDecryptParam("CurrentPage", "1").ToNumber<int>(); ;
            Starttime = GetDecryptParam("Starttime", "");
            Endtime = GetDecryptParam("Endtime", "");
            Order = GetDecryptParam("order", "");
        }


        #endregion 


        #region Event Handler (이벤트 핸들러)
        #endregion 



    }
}