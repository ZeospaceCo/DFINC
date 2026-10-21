using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Address.Web.Content
{
    public partial class AddSimple : Base.BasePageAddress
    {
        #region 변수설정
        public int CategoryID;
        public string Types;
        private string eventArgs = String.Empty;
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Content.AddSimple).Assembly;	//어셈블리 선언
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
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();


        }

        #endregion

        #region (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            Types = GetDecryptParam("types", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //CbtnSimpleAdd.Text = resMng.GetString("Cancel");
                cbtnConfirm.Text = resMng.GetString("Confirm");     // "확인";
                cbtnCancel.Text = resMng.GetString("Close");        // "닫기";
            }
        }


        /// <summary>
        /// Registers this instance.
        /// </summary>
        private void Register()
        {
            BE.PrivateInfo info = new BE.PrivateInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.LoginID = CurrentUserInfo.LogInID;
            info.CategoryID = CategoryID;
            info.PrivateID = 0;
            info.FamilyName = "";
            info.LastName = ctbName.Text.Trim();
            info.MiddleName = "";
            info.WholeName = info.FamilyName + info.MiddleName + info.LastName;
            info.NickName = "";            
            info.Mail = ctbMail.Text.Trim();
            info.Lunar = "";
            info.Birthday = "";
            info.Sex = "";
            info.Memo = "";
            info.PrivateHomePage = "";
            info.HomePhone = "";
            info.MobilePhone = ctbMobile1.Text.Trim() + "-" + ctbMobile2.Text.Trim() + "-" + ctbMobile3.Text.Trim();

            info.HomeFax = "";
            info.HomeZipCode = "";
            info.HomeCountry = "";
            info.HomeCity = "";
            info.HomeGu = "";
            info.HomeAddress = "";            
            info.OfficeAddress = "";
            info.CompanyHomePage = "";
            
            info.CompanyPhone = ctbPhone1.Text.Trim() + "-" + ctbPhone2.Text.Trim() + "-" + ctbPhone3.Text.Trim();
            info.CompanyFax = ""; ;
            info.IpPhone = "";
            info.Pager = "";

            info.CompanyFax = "";
            info.CompanyZipCode = "";
            info.CompanyCountry = "";
            info.CompanyCity = "";
            info.CompanyGu = "";
            info.CompanyAddress = "";
            info.DefaultPhone = "";
            info.IsReceive = "0";
            info.SendUserID = 0;

            DAL.Private dal = new DAL.Private();
            dal.Add(info);

        }

        #endregion

        #region (속성)

        #endregion

        #region (이벤트핸들러)

        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                string args = eventArgs;

                switch (args)
                {
                    case "Register":  //추가
                        Register();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnRegister_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion
    }
}