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

namespace Zeospace.DeskPlus.Pim.Web.Admin.HolidayD
{
    public partial class Add : Base.BasePagePimAdmin
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public int CompanyID = 0;
        public int UserID = 0;
        private string eventArgs = String.Empty;

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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Admin.HolidayD.Add).Assembly;	//어셈블리 선언
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

            #region 반복휴일
            rblHoliDayKind.Items.Clear();
            rblHoliDayKind.Items.Add(new ListItem("Yes", "0"));
            rblHoliDayKind.Items.Add(new ListItem("No", "1"));
            rblHoliDayKind.SelectedValue = "0";
            #endregion

            #region 양력구분
            rblSolarLunar.Items.Clear();
            rblSolarLunar.Items.Add(new ListItem(resMng.GetString("Holiday_Solar"), "0"));
            rblSolarLunar.Items.Add(new ListItem(resMng.GetString("Holiday_Lunar"), "1"));
            rblSolarLunar.SelectedValue = "0";
            #endregion

            #region 사용구분
            rblIsHoliday.Items.Clear();
            rblIsHoliday.Items.Add(new ListItem(resMng.GetString("Yes"), "1"));
            rblIsHoliday.Items.Add(new ListItem(resMng.GetString("No"), "0"));
            rblIsHoliday.SelectedValue = "1";
            #endregion

        }

        #endregion


        #region Method (함수)
        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
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
                CbtnCancel.Text = GetResource("Cancel");
            }
        }

        /// <summary>
        /// 공휴일 추가
        /// </summary>
        private void AddHolyday()
        {
            //결재라인정보
            BE.HolidayDInfo info = new BE.HolidayDInfo();

            BLL.HolidayD bll = new BLL.HolidayD();

            info.CompanyID = CurrentUserInfo.CompanyID;
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

            info.HoliDayKind = rblHoliDayKind.SelectedValue;
            info.SolarLunar = rblSolarLunar.SelectedValue;

            if (rblHoliDayKind.SelectedValue == "0")
            {
                info.HoliDay = ctbHoliDayY.Text;
            }
            else
            {
                info.HoliDay = ctbHoliDayN.Text;
            }

            info.IsHoliday = rblIsHoliday.SelectedValue;
            info.WriteUserID = CurrentUserInfo.UserID;
            info.WriteUserName = CurrentUserInfo.Name;

            bll.Add(info);

            BLL.HolidayR bll2 = new BLL.HolidayR();

            bll2.ClearCacheHolidayList(CurrentUserInfo.CompanyID);
            bll2.SetCacheHolidayList(CurrentUserInfo.CompanyID, DateTime.Now.ToString("yyyy"));
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
                paraDic.Add("goURL", "/DeskPlusEIP/Pim/Admin/HolidayD/List.aspx");

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