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
    public partial class ViewPopup : Base.BasePageCommunity
    {
        #region 변수처리
        public int CommunityID = 0;
        public int MenuID = 0;
        public int ScheduleID;
        public int Grade = 10;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Apps.Schedule.ViewPopup).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2012-07-06
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            #region 권한 체크

            DAL.MenuAuth dal = new DAL.MenuAuth();
            DataTableReader dtr = dal.GetMenuAuth(CurrentUserInfo.CompanyID, CommunityID, MenuID);

            int WriteAuth = 9;
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

            DAL.User userdal = new DAL.User();
            DataTableReader dtrUser = userdal.GetUserDetailInfo(CurrentUserInfo.CompanyID, CommunityID, CurrentUserInfo.UserID);

            if (dtrUser != null)
            {
                if (dtrUser.HasRows)
                {
                    while (dtrUser.Read())
                    {
                        Grade = dtrUser["Grade"].ToString().ToNumber<int>();
                    }
                }
            }

            if (Grade > ReadAuth)
            {                
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }

            #endregion 
            GetData();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-06
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
                        DateTime dtWriteDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                        writeUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                    }
                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-06
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnClose.Text = resMng.GetString("Close");
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-07-06
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            MenuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            ScheduleID = GetDecryptParam("scheduleID", "").ToNumber<int>();

            CommunityID = 1;
            MenuID = 10;
            ScheduleID = 15;
        }

        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}