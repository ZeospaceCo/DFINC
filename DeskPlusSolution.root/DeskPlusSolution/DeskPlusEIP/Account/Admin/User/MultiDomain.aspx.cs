using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;

namespace Zeospace.DeskPlus.Account.Web.Admin.User
{
    public partial class MultiDomain : Base.BasePageAccountAdmin
    {
        #region 변수설정
        public int UserID = 0;
        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.MultiDomain).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {                
                SetLanguage();
                LoadData();
            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 내용가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2011-04-25
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            LoadDomain();
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();       
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdd.Text = resMng.GetString("Admin_CompanyManager_btnAdd");
                cbtnClose.Text = resMng.GetString("Admin_UserRegister_Close"); //닫기
                cbtnMailCheck.Text = resMng.GetString("Admin_UserRegister_DoubleCheck"); //중복체크
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 회사별 메일 도메인 설정하기
        /// </summary>
        private void LoadDomain()
        {
            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "MAILDOMAIN");
            ddlMailDomain.Items.Clear();
            ddlMailDomain.AppendDataBoundItems = true;
            ddlMailDomain.Items.Add(new ListItem("-------", ""));

            for (int i = 0; i < dv.Count; i++)
            {
                ddlMailDomain.Items.Add(new ListItem(dv[i]["CodeID"].ToString(), dv[i]["CodeID"].ToString()));
            }
        }
        

        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion

        #region Property (프로퍼티)

        #endregion
    }
}