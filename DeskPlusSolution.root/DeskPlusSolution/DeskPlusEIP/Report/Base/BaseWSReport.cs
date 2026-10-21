using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.ComponentModel;


namespace Zeospace.DeskPlus.Report.Web.Base
{
    public class BaseWSReport : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseService
    {
        /// <summary>
        /// 스킨정보
        /// </summary>
        public string Skin { get; set; }

        /// <summary>
        /// 이미지 경로 프로퍼티
        /// </summary>
        public string ImgPath { get; set; }

        /// <summary>
        /// css 경로 프로퍼티
        /// </summary>
        public string CssPath { get; set; }

        #region Construct (생성자)

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : 생성자 생성
        /// </summary>
        public BaseWSReport()
        {
            SetSkin();
        }

        #endregion Construct (생성자)

        /// <summary>
        /// 스킨 설정하기
        /// </summary>
        private void SetSkin()
        {
            try
            {
                // 이미지파일 경로 설정하기
                // ex) DeskPlusEIP/Design/[LANGUAGE]/[TYPE]/images
                string skin = Account.BLL.User.GetSkin(CurrentUserInfo.CompanyID, CurrentUserInfo.LogInID);

                CssPath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseCssDirectory", "/DeskPlusEIP/Design/");
                ImgPath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                if (!CssPath.EndsWith("/")) CssPath += "/";
                if (!ImgPath.EndsWith("/")) ImgPath += "/";

                CssPath = CssPath + skin + "/Css";
                ImgPath = ImgPath + skin + "/Img";
            }
            catch
            {
                CssPath = "/DeskPlusEIP/Design/C/Css";
                ImgPath = "/DeskPlusEIP/Design/C/Img";
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 게시판 관리자 여부 확인하기
        /// </summary>
        /// <returns></returns>
        protected bool GetAccessAuth()
        {
            bool response = false;
            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            //bool isRoleBoard = CurrentUserInfo.IsInRole("board");//board admin

            if (CurrentUserInfo.IsSysAdmin || isRoleAdmin) response = true;

            return response;
        }

        
    }

    /// <summary>
    /// AppSettingType
    /// </summary>
    public enum FlagEnum
    {
        /// <summary>
        /// 전부
        /// </summary>
        [Description("'1','2','3'")]
        All = 0,

        /// <summary>
        /// 저장
        /// </summary>
        [Description("'1'")]
        Save = 1,

        /// <summary>
        /// 확인요청
        /// </summary>
        [Description("'2'")]
        ConfirmReq = 2,

        /// <summary>
        /// 확인완료
        /// </summary>
        [Description("'3'")]
        ConfirmComp = 3,

        /// <summary>
        /// 보고
        /// </summary>
        [Description("'4'")]
        Inform = 4
    };

    
}