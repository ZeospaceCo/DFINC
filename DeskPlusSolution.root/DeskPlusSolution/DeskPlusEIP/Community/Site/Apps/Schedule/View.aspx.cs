using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Community.Web.Site.Apps.Schedule
{
    public partial class View : Base.BasePageCommunity
    {
        #region 변수처리
        public int CommunityID = 0;
        public int MenuID = 0;
        public string TmpDate = String.Empty;        
        public int ScheduleID;
        public int Grade = 0;
        public int WriteAuth = 9;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Schedule.View).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            #region 권한 체크

            if (Grade != 2)
            {
                DAL.MenuAuth dal = new DAL.MenuAuth();
                DataTableReader dtr = dal.GetMenuAuth(CurrentUserInfo.CompanyID, CommunityID, MenuID);

                int ReadAuth = 9;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            WriteAuth = dtr["WriteAuth"].ToString().ToNumber<int>();
                            ReadAuth = dtr["ReadAuth"].ToString().ToNumber<int>();
                        }
                    }
                }

                if (Grade > ReadAuth)
                {                   
                    Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                    return;
                }
            }

            #endregion 

            GetData();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 스케줄 정보 가져오기
        /// </summary>
        private void GetData()
        {
            DAL.Schedule dal = new DAL.Schedule();

            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, CommunityID, ScheduleID);

            int writeUserID = 0;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        lbTitle.Text = dtr["Title"].ToString();
                        lbDate.Text = dtr["StartDate"].ToString() + " ~ " + dtr["EndDate"].ToString();

                        // 등록자
                        string[] arrManyName = dtr["MManyName"].ToString().Split('|');
                        string tmpManagerName = "";
                        if (arrManyName.Length == 7)
                        {
                            tmpManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                                   arrManyName[2], arrManyName[3], arrManyName[4],
                                                                   arrManyName[5], arrManyName[6], PreLang).ToReverseXSS();
                        }

                        lbWriteName.Text = tmpManagerName;

                        lbContent.Text = dtr["Content"].ToString().ToEditorXSS();
                        ParentScheduleID = dtr["ParentScheduleID"].ToString().ToNumber<int>();
                        StartDate = Convert.ToDateTime(dtr["StartDate"].ToString());
                        DateTime dtWriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                        WriteYM = dtWriteDate.Year.ToString() + dtWriteDate.Month.ToString().PadLeft(2, '0');                        
                        writeUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                    }
                }
            }

            GetAuth(writeUserID);
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 삭제 버튼 권한 가져오기
        /// </summary>
        private void GetAuth(int _writeUserID)
        {
            if (_writeUserID == CurrentUserInfo.UserID)
            {
                if (Grade > WriteAuth)
                {
                    cbtnDel.Visible = false;
                    cbtnMod.Visible = false;
                }
                else
                {
                    cbtnDel.Visible = true;
                    cbtnMod.Visible = true;
                }
            }
            else
            {
                cbtnDel.Visible = false;
                cbtnMod.Visible = false;
            }

            if (Grade == 2)
            {
                cbtnDel.Visible = true;
                cbtnMod.Visible = true;
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnDel.Text = resMng.GetString("Del");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnList.Text = resMng.GetString("List");
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            TmpDate = GetDecryptParam("tmpDate", GetYearMonthDay());
            ScheduleID = GetDecryptParam("scheduleID", "").ToNumber<int>();
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();
        }

        /// <summary>
        /// 현재 년월일 가져오기 [2012-01-01]
        /// </summary>
        /// <returns></returns>
        public string GetYearMonthDay()
        {
            string nowYearMonDay = "";
            if (TargetMonth.ToString().Length == 1)
            {
                nowYearMonDay = TargetYear.ToString() + "-" + "0" + TargetMonth.ToString();
            }
            else
            {
                nowYearMonDay = TargetYear.ToString() + "-" + TargetMonth.ToString();
            }

            if (TargetDay.ToString().Length == 1)
            {
                nowYearMonDay = nowYearMonDay + "-" + "0" + TargetDay.ToString();
            }
            else
            {
                nowYearMonDay = nowYearMonDay + "-" + TargetDay.ToString();
            }
            return nowYearMonDay;
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 스케줄 삭제
        /// </summary>
        /// <returns></returns>
        private bool Del()
        {
            bool rtnVal = false;
            BLL.Schedule bll = new BLL.Schedule();

            if (hdRepeatSel.Text == "1")
            {
                rtnVal = bll.Del(CurrentUserInfo.CompanyID, CommunityID, ScheduleID, ParentScheduleID, WriteYM);
            }
            else if (hdRepeatSel.Text == "2")
            {
                string tmpStartDate = StartDate.ToShortDateString().Replace("-", "");
                rtnVal = bll.Del(CurrentUserInfo.CompanyID, CommunityID, ScheduleID, ParentScheduleID, WriteYM, tmpStartDate);
            }
            else if (hdRepeatSel.Text == "3")
            {
                rtnVal = bll.DelAll(CurrentUserInfo.CompanyID, CommunityID, ParentScheduleID);
            }
            else
            {
                rtnVal = bll.Del(CurrentUserInfo.CompanyID, CommunityID, ScheduleID);
            }

            return rtnVal;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 예약 자원 삭제
        /// </summary>
        private void DelList()
        {
            if (Del())
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("scheduleID", ScheduleID.ToString());
                paraDic.Add("communityID", CommunityID.ToString());
                paraDic.Add("menuID", MenuID.ToString());                
                paraDic.Add("tmpDate", TmpDate.ToString());
                paraDic.Add("grade", Grade.ToString());
                paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/Community/Site/Apps/Schedule/List.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }



        #endregion

        #region (프로퍼티)
        /// <summary>
        /// 
        /// </summary>
        public int TargetYear
        {
            get { return (ViewState["targetYear"] == null) ? System.DateTime.Now.Year : Convert.ToInt32(ViewState["targetYear"].ToString()); }
            set { ViewState["targetYear"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetMonth
        {
            get { return (ViewState["targetMonth"] == null) ? System.DateTime.Now.Month : Convert.ToInt32(ViewState["targetMonth"].ToString()); }
            set { ViewState["targetMonth"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TargetDay
        {
            get { return (ViewState["targetDay"] == null) ? System.DateTime.Now.Day : Convert.ToInt32(ViewState["targetDay"].ToString()); }
            set { ViewState["targetDay"] = value; }
        }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateTime StartDate
        {
            get { return Convert.ToDateTime(ViewState["StartDate"]); }
            set { ViewState["StartDate"] = value; }
        }

        /// <summary>
        /// WriteYM
        /// </summary>
        public string WriteYM
        {
            get { return GetViewStateString("WriteYM", ""); }
            set { ViewState["WriteYM"] = value; }
        }

        /// <summary>
        /// ParentScheduleID
        /// </summary>
        public int ParentScheduleID
        {
            get { return GetViewStateInt("ParentScheduleID", "0"); }
            set { ViewState["ParentScheduleID"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-01
        /// 작성목적 : 삭제 버튼 클릭시 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnConfirm_Click(object sender, EventArgs e)
        {
            LoadParam();

            DelList();
        }

        #endregion
    }
}