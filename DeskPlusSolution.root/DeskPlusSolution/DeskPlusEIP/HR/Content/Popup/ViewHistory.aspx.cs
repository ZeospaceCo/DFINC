using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.HR.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;

namespace Zeospace.DeskPlus.HR.Web.Content.Popup
{
    public partial class ViewHistory : Base.BasePageHR
    {
        #region 변수설정
        public int AttID = 0;
        public string TargetUserID = string.Empty;
        public string TargetDeptID = string.Empty;
        public string DayWeek = string.Empty;
        public string WorkDate = string.Empty;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.HR.Web.Resource.GlobalString.string";			    //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.HR.Web.Content.Popup.ViewHistory).Assembly;	//어셈블리 선언
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
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            #region 권한 체크
            if (!AccessAuth && !CurrentUserInfo.IsInRole("HRAttendance"))
            {
                Utility.ShowMessageBoxNoAuth(Page, "NoAuth", "");
                return;
            }
            #endregion

            LoadParam();

            GetHeaderInfo();
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : 언어설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {
                cbtnClose.Text = this.resMng.GetString("BtnClose"); // 닫기
            }
        }

        /// <summary>
        /// 작성자   : kkzz52
        /// 작성일자 : 2017-11-24
        /// 작성목적 : Loads the param.
        /// </summary>
        private void LoadParam()
        {
            AttID = GetDecryptParam("attID", "0").ToNumber<int>();
            TargetUserID = GetDecryptParam("userID", "0");
            TargetDeptID = GetDecryptParam("deptID", "0");
            DayWeek = GetDecryptParam("dayWeek", "");
            WorkDate = GetDecryptParam("workDate", "");
        }

        private void GetHeaderInfo()
        {
            string DeptName = string.Empty;
            string TitleName = string.Empty;
            string Name = string.Empty;

            Zeospace.DeskPlus.Account.DAL.User uDal = new Zeospace.DeskPlus.Account.DAL.User();
            List<Zeospace.DeskPlus.Account.BE.UserInfo> ulist = uDal.GetUserInfo(CurrentUserInfo.CompanyID, TargetUserID.ToNumber<int>(), TargetDeptID.ToNumber<int>());
            foreach (Zeospace.DeskPlus.Account.BE.UserInfo info in ulist)
            {
                DeptName = GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhCn, info.DeptZhTw, PreLang);
                TitleName = GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhCn, info.TitleZhTw, PreLang);
                Name = GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhCn, info.NameZhTw, PreLang);
            }

            ltlHeader.Text = this.resMng.GetString("History") + " (" + WorkDate + " " + DayWeek + " - " + DeptName + " " + Name + " " + TitleName + ")";
        }

        #endregion

        #region (프로퍼티)

        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}