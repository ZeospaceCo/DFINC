using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Pim.BE;

namespace Zeospace.DeskPlus.Pim.Web.Schedule
{

    public partial class SortHoliDayList : Base.BasePagePim
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Schedule.SortHoliDayList).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();

            }
        }

        /// <summary>
        /// 작성자     : famo23
        /// 작성일자   : 2012-07-20
        /// 작성목적   : 기념일 데이터 설정
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            string sDtae = string.Empty;

            sDtae = Date;
            if (sDtae.Length > 2)
            {
                if (sDtae.Substring(0, 2) == "H_")
                {
                    sDtae = sDtae.Replace("H_", "");
                    sDtae = sDtae.Substring(0, 4) + "-" + sDtae.Substring(4, 2) + "-" + sDtae.Substring(6, 2);
                }
            }
 

            List<object> listobj = new List<object>();

            BE.ScheduleInfo info = new ScheduleInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.StartTime = DateTime.Parse(sDtae);
            info.UserID = CurrentUserInfo.UserID;

            DAL.Schedule dal = new DAL.Schedule();
            DataTableReader dtr = dal.GetSheduleMon(info);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ScheduleInfo scheinfo = new ScheduleInfo();
                        scheinfo.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                        scheinfo.Type = dtr["Type"].ToString();
                        scheinfo.Title = dtr["Title"].ToString();

                        listobj.Add(scheinfo);
                    }
                }
            }

            foreach (ScheduleInfo _info in listobj)
            {
                ulList.InnerHtml += "<li>&nbsp;<a href='#' onclick='parent.MoveHolidayView(\"" + _info + "\",\"" + _info.Type + "\")'>" + _info.Title + "<div class=dotLine></div></a></li>";

            }
        }



        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            Date = GetDecryptParam("StartDate", "0").ToString();
        }

        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                divPopTitle.InnerText = GetResource("HOLIDAYCADALOG");
                CbtnClose.Text = GetResource("Close");
            }
        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }


}