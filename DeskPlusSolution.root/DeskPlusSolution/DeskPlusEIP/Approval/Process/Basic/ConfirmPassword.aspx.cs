using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Process.Basic
{
    public partial class ConfirmPassword : Base.BasePageApproval
    {
        #region 변수설정
        public string IsApprovePwd = "N";
        public string IsSign = "N";        
        public int CompanyID = 0;        
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Process.Basic.ConfirmPassword).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                this.tbOpinion.Attributes.Add("maxlength", "500");

                GetUserSignInfo(); 

                LoadParam();
                SetLanguage();
                LoadData(); // 설정정보 가져오기
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        private void LoadData()
        {
            // 1. 패스워드사용유무 정보
            Zeospace.DeskPlus.Account.DAL.UserEnvironment uDAL = new Account.DAL.UserEnvironment();
            List<Account.BE.UserEnvironmentInfo> list = uDAL.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            if (list != null)
            {
                foreach (Account.BE.UserEnvironmentInfo info in list)
                {
                    IsApprovePwd = (info.IsApprovePwd.Equals("")) ? "N" : info.IsApprovePwd;
                }
            }
        }
        #endregion

        #region (함수)
        /// <summary>
        /// 파라미터처리
        /// </summary>
        private void LoadParam()
        {
            ReturnType = GetDecryptParam("type", "");
        }
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
                cbtnSign.Text = resMng.GetString("Signature"); // "서명";
                cbtnConfirm.Text = resMng.GetString("Confirm"); // "확인";
                cbtnClose.Text = resMng.GetString("Close"); //"닫기";
            }
        }

        /// <summary>
        /// 사용자정보 가져오기(인장여부체크)
        /// </summary>
        private void GetUserSignInfo()
        {
            CompanyID = CurrentUserInfo.CompanyID;            
            UserID = CurrentUserInfo.UserID;      

            Zeospace.DeskPlus.Account.DAL.User uDAL = new Account.DAL.User();
            List<Account.BE.UserInfo> userlist = uDAL.GetUserInfo(CompanyID, UserID, true, true);
            foreach (Account.BE.UserInfo info in userlist)
            {
                if (info.SealImage == "")
                {
                    IsSign = "N";
                }
                else
                {
                    IsSign = "Y";
                }                
            }
        }

        #endregion

        #region (프로퍼티)
        //완료 처리시 넘겨줄 값
        public string ReturnType
        {
            get { return GetViewStateString("ReturnType", ""); }
            set { ViewState["ReturnType"] = value; }
        }
        /// <summary>
        /// 사용자아이디
        /// </summary>
        public int UserID
        {
            get { return GetViewStateInt("UserID", "0"); }
            set { ViewState["UserID"] = value; }
        }
        
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}