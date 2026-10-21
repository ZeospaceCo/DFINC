using System;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Content
{
    public partial class ShortReplyMod : Base.BasePageReport
    {
        #region 변수선언

        public int ShortReplyID;

        #endregion 변수선언

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Content.ShortReplyMod).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 작성목적   : 초기화 데이터 설정
        /// 작성자     :
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            DAL.ShortReply dal = new DAL.ShortReply();
            BE.ShortReplyInfo info = dal.GetShortReplyListID(CurrentUserInfo.CompanyID, ListID, ShortReplyID);
            string contents = info.Content;
            int writeUserID = info.UserID;

            //권한 체크 (작성자 : 심재운)
            if (CurrentUserInfo.UserID != writeUserID)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBoxNoAuth(Page, "noauth", GetResource("NoAuth"));
            }
            else
            {
                textarea.Value = contents.ToReverseXSS();
            }
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     :
        /// </summary>
        private void LoadParam()
        {
            ShortReplyID = GetDecryptParam("shortReplyID", "0").ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "");
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
                CbtnConfirm.Text = resMng.GetString("ConfirmCheck");
                CbtnClose.Text = resMng.GetString("Close");
            }
        }

        public string Gubun { get; set; }
    }
}