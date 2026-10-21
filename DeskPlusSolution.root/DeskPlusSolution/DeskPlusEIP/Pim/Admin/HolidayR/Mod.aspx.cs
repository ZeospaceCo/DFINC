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

namespace Zeospace.DeskPlus.Pim.Web.Admin.HolidayR
{
    public partial class Mod : Base.BasePagePimAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        private string eventArgs = String.Empty;

        public int HoliDayID = 0;
        public string SearchField = "";
        public string SearchValue = "";

        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Admin.HolidayR.Mod).Assembly;	//어셈블리 선언
            #endregion
            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();
                LoadData();
            }

        }

        private void LoadData()
        {
            CompanyID = CurrentUserInfo.CompanyID;
            UserID = CurrentUserInfo.UserID;

            #region 양력구분
            rblSolarLunar.Items.Clear();
            rblSolarLunar.Items.Add(new ListItem(resMng.GetString("Holiday_Solar"), "0"));
            rblSolarLunar.Items.Add(new ListItem(resMng.GetString("Holiday_Lunar"), "1"));
            rblSolarLunar.SelectedValue = "0";
            #endregion

            #region 연휴구분
            ddlHolidayBetween.Items.Clear();
            ddlHolidayBetween.Items.Add(new ListItem(this.resMng.GetString("Holiday_HolidayBetween1"), "0"));
            //ddlHolidayBetween.Items.Add(new ListItem(this.resMng.GetString("Holiday_HolidayBetween2"), "1"));
            //ddlHolidayBetween.Items.Add(new ListItem(this.resMng.GetString("Holiday_HolidayBetween3"), "2"));
            ddlHolidayBetween.Items.Add(new ListItem(this.resMng.GetString("Holiday_HolidayBetween4"), "3"));
            #endregion

            #region 대체휴무사용구분
            ddlIsReplace.Items.Clear();
            ddlIsReplace.Items.Add(new ListItem(this.resMng.GetString("No"), "0"));
            ddlIsReplace.Items.Add(new ListItem(this.resMng.GetString("Holiday_IsHolidayKind1"), "1"));
            ddlIsReplace.Items.Add(new ListItem(this.resMng.GetString("Holiday_IsHolidayKind1") + "/" + this.resMng.GetString("kind10"), "2"));
            #endregion

            #region 사용구분
            rblIsHoliday.Items.Clear();
            rblIsHoliday.Items.Add(new ListItem(resMng.GetString("Yes"), "1"));
            rblIsHoliday.Items.Add(new ListItem(resMng.GetString("No"), "0"));
            rblIsHoliday.SelectedValue = "1";
            #endregion

            //공휴일정보
            DAL.HolidayR dal = new DAL.HolidayR();

            DataTableReader dtr = dal.GetDataByHoliDayID(CurrentUserInfo.CompanyID, HoliDayID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ctbTitle.Text = dtr["Title"].ToString();

                        ctbMultiName.Text = this.GetMuiltiLang(
                        dtr["NameKo"].ToString().ToReverseXSS(),
                        dtr["NameEn"].ToString().ToReverseXSS(),
                        dtr["NameJa"].ToString().ToReverseXSS(),
                        dtr["NameFr"].ToString().ToReverseXSS(),
                        dtr["NameZhcn"].ToString().ToReverseXSS(),
                        dtr["NameZhtw"].ToString().ToReverseXSS());

                        rblSolarLunar.SelectedValue = dtr["SolarLunar"].ToString();
                        ctbHoliDay.Text = dtr["HoliDay"].ToString();
                        ddlHolidayBetween.SelectedValue = dtr["HolidayBetween"].ToString();
                        ctbHolidayBetweenDay.Text = dtr["HolidayBetweenDay"].ToString();
                        ddlIsReplace.SelectedValue = dtr["IsReplace"].ToString();
                        rblIsHoliday.SelectedValue = dtr["IsHoliday"].ToString();
                    }
                }
            }
        }

        #endregion


        #region Method (함수)
        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            HoliDayID = GetDecryptParam("holiDayID", "0").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnConfirm.Text = GetResource("Save");
                cbtnDel.Text = GetResource("Del");
                CbtnCancel.Text = GetResource("Cancel");
            }
        }

        /// <summary>
        /// 공휴일 추가
        /// </summary>
        private void AddHolyday()
        {
            //공휴일정보
            BE.HolidayRInfo info = new BE.HolidayRInfo();

            BLL.HolidayR bll = new BLL.HolidayR();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.HoliDayID = HoliDayID;
            info.Title = ctbTitle.Text;

            string multiName = ctbMultiName.Text.Trim();
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.NameKo = multiName.Split(',')[0];
                info.NameEn = multiName.Split(',')[1];
                info.NameJa = multiName.Split(',')[2];
                info.NameFr = multiName.Split(',')[3];
                info.NameZhcn = multiName.Split(',')[4];
                info.NameZhtw = multiName.Split(',')[5];
            }

            info.SolarLunar = rblSolarLunar.SelectedValue;
            info.HoliDay = ctbHoliDay.Text;
            info.HolidayBetween = ddlHolidayBetween.SelectedValue;
            info.HolidayBetweenDay = ctbHolidayBetweenDay.Text.ToNumber<int>();
            info.IsReplace = ddlIsReplace.SelectedValue;
            info.IsHoliday = rblIsHoliday.SelectedValue;
            info.ModifyUserID = CurrentUserInfo.UserID;
            info.ModifyUserName = CurrentUserInfo.Name;

            bll.Mod(info);

            bll.ClearCacheHolidayList(CurrentUserInfo.CompanyID);
            bll.SetCacheHolidayList(CurrentUserInfo.CompanyID, DateTime.Now.ToString("yyyy"));
        }
        #endregion

        #region Event Handler (이벤트 핸들러)
        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                string args = eventArgs;
                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                switch (args)
                {
                    case "Save": //저장버튼
                        AddHolyday();
                        break;

                }
                
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("alertMsg", GetResource("GoToList"));
                paraDic.Add("goURL", "/DeskPlusEIP/Pim/Admin/HolidayR/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnAdd_Click", ex);
            }
        }
        #endregion
    }
}