using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;

namespace Zeospace.DeskPlus.Approval.Web.Preference.Signature
{
    public partial class List : Base.BasePageApproval
    {
        #region 변수설정

        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Preference.Signature.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            SwfUploadGUID = System.Guid.NewGuid().ToString();

            Zeospace.DeskPlus.Account.DAL.User uDAL = new Account.DAL.User();
            List<Account.BE.UserInfo> userlist = uDAL.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);
            foreach (Account.BE.UserInfo info in userlist)
            {
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string path = baseFileDirectory + CurrentUserInfo.CompanyID.ToString() + "/Users/" + CurrentUserInfo.UserID.ToString() + "/Picture/" + info.SealImage;
                if (File.Exists(Server.MapPath(path)))
                {
                    SignImage = GetEncryptParam(path);
                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSend.Text = resMng.GetString("Send");           //전달;

            }
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            this.BaseParam();
        }

        #endregion

        #region (프로퍼티)
        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        /// 사인이미지
        /// </summary>
        public string SignImage
        {
            get { return GetViewStateString("SignImage", ""); }
            set { ViewState["SignImage"] = value; }
        }
        #endregion

        #region (이벤트핸들러)

        #endregion
    }
}