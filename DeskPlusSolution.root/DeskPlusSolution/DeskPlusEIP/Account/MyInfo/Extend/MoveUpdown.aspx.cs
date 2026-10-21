using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.MyInfo.Extend
{
    public partial class MoveUpdown : Base.BasePageAccount
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정        
        public string Gubun = "";
        public string FolderList = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.Extend.MoveUpdown).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        #endregion

        #region (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = resMng.GetString("BtnConfirm");                          //"적용";
                cbtnClose.Text = resMng.GetString("BtnClose");         //"닫기";
                ltlTitle.Text = resMng.GetString("Admin_UserManager_GridHeader5");     //폴더관리
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            Gubun = GetDecryptParam("gubun", "");
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            switch (Gubun)
            {
                case "family" :
                    LoadFamily();
                    break;
                case "education" :
                    LoadEdu();
                    break;
                case "reward" :
                    LoadReward();
                    break;
                case "career" :
                    LoadCareer();
                    break;
                case "scholar" :
                    LoadScholar();
                    break;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 가족 현황
        /// </summary>
        private void LoadFamily()
        {
            DAL.UserFamily dalFamily = new DAL.UserFamily();
            List<UserFamilyInfo> al = dalFamily.GetUserFamilyInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            StringBuilder sb = new StringBuilder();

            foreach (UserFamilyInfo info in al)
            {
                sb.Append("<li id='");
                sb.Append(info.FamilyID);
                sb.Append("'>");
                sb.Append(info.Name);
                sb.Append("</li>");
            }

            FolderList = sb.ToString();
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 가족 현황
        /// </summary>
        private void LoadEdu()
        {
            DAL.UserEducation dalFamily = new DAL.UserEducation();
            List<UserEducationInfo> al = dalFamily.GetUserEducationInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            StringBuilder sb = new StringBuilder();

            foreach (UserEducationInfo info in al)
            {
                sb.Append("<li id='");
                sb.Append(info.EducationID);
                sb.Append("'>");
                sb.Append(info.Name);
                sb.Append("</li>");
            }

            FolderList = sb.ToString();
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 상벌 현황
        /// </summary>
        private void LoadReward()
        {
            DAL.UserReward dal = new DAL.UserReward();
            List<UserRewardInfo> al = dal.GetUserRewardInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            StringBuilder sb = new StringBuilder();

            foreach (UserRewardInfo info in al)
            {
                sb.Append("<li id='");
                sb.Append(info.RewardID);
                sb.Append("'>");
                sb.Append(info.RewardType);
                sb.Append("</li>");
            }

            FolderList = sb.ToString();
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 경력사항 현황
        /// </summary>
        private void LoadCareer()
        {
            DAL.UserCareer dal = new DAL.UserCareer();
            List<UserCareerInfo> al = dal.GetUserCareerInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            StringBuilder sb = new StringBuilder();

            foreach (UserCareerInfo info in al)
            {
                sb.Append("<li id='");
                sb.Append(info.CareerID);
                sb.Append("'>");
                sb.Append(info.CompanyName);
                sb.Append("</li>");
            }

            FolderList = sb.ToString();
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 학력 현황
        /// </summary>
        private void LoadScholar()
        {
            DAL.UserScholar dal = new DAL.UserScholar();
            List<UserScholarInfo> al = dal.GetUserScholarInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            StringBuilder sb = new StringBuilder();

            foreach (UserScholarInfo info in al)
            {
                sb.Append("<li id='");
                sb.Append(info.ScholarshipID);
                sb.Append("'>");
                sb.Append(info.SchoolName);
                sb.Append("</li>");
            }

            FolderList = sb.ToString();
        }

        #endregion

        #region (속성)

        #endregion
    }
}