using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ComponentArt.Web.UI;
using System.IO;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.PMS.Web.Schedule
{
    public partial class View : Base.BasePagePMS
    {
        #region Declaration (컨트롤 선언)
        protected DateTime dtTargetDate;
        #endregion

        #region 변수선언
        
      
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Schedule.View).Assembly;		//어셈블리 선언
            GetResourceManager();
            #endregion

            #region 날짜 세팅
            dtTargetDate = new DateTime(TargetYear, TargetMonth, TargetDay);
            #endregion 날짜 세팅

            
            #region 초기 데이터 로드
            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
            #endregion

            #region 버튼 이미지

            #endregion

            #region 관련프로젝트 처리
            if (Authority == 1) cbtnMod.Visible = false;
            if (Authority == 1) cbtnDel.Visible = false;
            #endregion
        }
        #endregion

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)
        private void LoadData()
        {
            int getValueAuth = 2;
            if (getValueAuth == 2)
            {
                this.cbtnMod.Visible = false;
                this.cbtnDel.Visible = false;
            }

            DAL.Display daldis = new DAL.Display();
            DataTableReader dtr = daldis.GetDisplayInfo(CurrentUserInfo.CompanyID, DisplayID);
            
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lbTitleName.Text = dtr["Subjects"].ToString();
                        lbContent.Text = dtr["Contents"].ToString();
                        lbFullYMD1.Text = DateTime.Parse(dtr["StartDate"].ToString()).ToShortDateString();
                        lbFullYMD2.Text = DateTime.Parse(dtr["EndDate"].ToString()).ToShortDateString();
                            
                            

                        if (CurrentUserInfo.UserID == dtr["WriteUserID"].ToString().ToNumber<int>())
                        {
                            this.cbtnMod.Visible = true;
                            this.cbtnDel.Visible = true;
                        }

                        GetRepeat(dtr["scheduleID"].ToString().ToNumber<int>(), dtr["CompanyID"].ToString().ToNumber<int>());
                    }
                }
            }
        }

        #endregion

        #region Method (함수)
        private void GetRepeat(int schedulid, int companyid)
        {
            DAL.Repeat biz = new DAL.Repeat();

            List<BE.RepeatScheduleInfo> data = biz.GetRepeatSchedule(companyid, schedulid);
            BE.RepeatScheduleInfo info = new BE.RepeatScheduleInfo();

            foreach (BE.RepeatScheduleInfo _be in data)
            {
                hdRepeatChk.Text = "I";
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
           
        }


        /// <summary>
        /// 작성목적   : 다국어 설정
        /// 작성자     : 
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnMod.Text = resMng.GetString("Mod"); //수정
                cbtnDel.Text = resMng.GetString("Unit_Register_Delete"); //삭제
                cbtnCancel.Text = resMng.GetString("List"); //목록

                ltTitleName.Text = resMng.GetString("Schedule_Header1");
                ltTime.Text = resMng.GetString("Schedule_Header2");
            }
        }
        #endregion

        #region Property (프로퍼티)

        /// <summary>
        /// 반복 : 1, 일반 : 0
        /// </summary>
        /// <value>The is repeat.</value>
        public int DisplayID
        {
            get { return GetDecryptParam("DisplayID", "0").ToNumber<int>(); }
        }

        
        /// <summary>
        /// 반복 : 1, 일반 : 0
        /// </summary>
        /// <value>The is repeat.</value>
        public int ScheduleID
        {
            get { return GetDecryptParam("scheduleID", "0").ToNumber<int>(); }
        }
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get { return GetDecryptParam("status", ""); }
        }

        /// <summary>
        /// 반복 : 1, 일반 : 0
        /// </summary>
        /// <value>The is repeat.</value>
        public string IsRepeat
        {
            get { return GetViewStateString("IsRepeat", ""); }
            set { ViewState["IsRepeat"] = value; }
        }
        
        /// <summary>
        /// Gets or sets the target year.
        /// </summary>
        /// <value>The target year.</value>
        public int TargetYear
        {
            get { return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : ViewState["targetYear"].ToString().ToNumber<int>(); }
            set { ViewState["targetYear"] = value; }
        }
        
        /// <summary>
        /// Gets or sets the target month.
        /// </summary>
        /// <value>The target month.</value>
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : ViewState["targetMonth"].ToString().ToNumber<int>(); }
            set { ViewState["targetMonth"] = value; }
        }
        
        /// <summary>
        /// TargetDay
        /// </summary>
        /// <value></value>
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : ViewState["targetDay"].ToString().ToNumber<int>(); }
            set { ViewState["targetDay"] = value; }
        }

        /// <summary>
        /// 태그프리 경로
        /// </summary>
        /// <value>The activex tagfree.</value>
        //public string ActivexTagfree
        //{
        //    get { return (EditorName == "TAGFREE") ? "<script src=\"/DeskPlusEIP/Html/Js/IEActiveX/activexTagfreeNoImage.js\"></script>" : ""; }
        //}
        
        /// <summary>
        /// Gets the company ID.
        /// </summary>
        /// <value>The company ID.</value>
        public int CompanyID
        {
            get { return GetViewStateInt("CompanyID", "1"); }
            set { ViewState["CompanyID"] = value; }
        }

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        /// <value>The user ID.</value>
        public int UserID
        {
            get { return GetViewStateInt("UserID", "1"); }
            set { ViewState["UserID"] = value; }
        }
        #endregion


        #region Event Handler (이벤트 핸들러)
        /// <summary>
        /// Handles the Click event of the cbtnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void cbtnDelete_Click(object sender, EventArgs e)
        {
            if (DisplayID > 0)
            {
                BE.DisplayInfo bedis = new BE.DisplayInfo();
                bedis.CompanyID = CurrentUserInfo.CompanyID;
                bedis.UserID = CurrentUserInfo.UserID;
                bedis.DisplayID = DisplayID;
                DAL.Display blldis = new DAL.Display();
                //blldis.DelDisplayIDSolo(bedis);

                switch (hdRepeatSel.Text)
                {
                    case "1":
                        blldis.DelDisplayIDSolo(bedis);
                        break;
                    case "2":  //향후
                        blldis.DelDisplayDate2(bedis);
                        break;
                    case "3":  //모두
                        blldis.DelDisplayAll(bedis);
                         break;
                    default:
                         blldis.DelDisplayIDSolo(bedis);
                         break;
                }


            }

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            
            paraDic.Add("projectID", ProjectID.ToString());
            paraDic.Add("menuType", MenuType.ToString());
            paraDic.Add("rqProjectID", RqProjectID.ToString());
            
            paraDic.Add("status", Status.ToString());

            paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
            paraDic.Add("alertMsg", GetResource("GoToList"));

            paraDic.Add("goURL", "/DeskPlusEIP/PMS/Schedule/List.aspx");
            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);  

            
            
        }

        #endregion
    }
}
