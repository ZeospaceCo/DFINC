using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Address.Web.Content
{
    public partial class PrivateTree : Base.BasePageAddress
    {

        #region 변수설정
        public string ListAll = "";

        #endregion

        #region 페이지처리
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Content.PrivateTree).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
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
                cbtnConfirm.Text = resMng.GetString("Confirm");       //"적용";
                cbtnClose.Text = resMng.GetString("Close");         //"닫기";
            }
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {

            if (Request.Form["listAll"] != null) //넘어온값이 있다면
            {
                Session["UserList"] = Request.Form["listAll"];
                ListAll = Request.Form["listAll"];
            }
            else
            {
                ListAll = Page.Session["UserList"].ToString();
            }

            cbtnAdd.Text = GetResource("Add");
        }

        /// <summary>
        /// 데이터가져오기
        /// </summary>
        private void LoadData()
        {
            LoadParam();

        }

        /// <summary>
        /// 개인주소록에 저장하기
        /// </summary>
        public void Register()
        {
            string userStr = tbUserValue.Text;
            string categoryID = tbCategoryID.Text;

            DAL.Private dal = new DAL.Private();

            foreach (var item in userStr.Split('$'))
            {
                if (item != "")
                {
                    BE.PrivateInfo info = new BE.PrivateInfo();

                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.UserID = CurrentUserInfo.UserID;
                    info.CategoryID = categoryID.ToString().ToNumber<int>();
                    info.LoginID = CurrentUserInfo.LogInID;
                    info.Sex = "";
                    info.LastName = item.Split(',')[2].ToString();
                    info.FamilyName = "";
                    info.MiddleName = "";
                    info.WholeName = item.Split(',')[2].ToString();
                    info.NameBase = item.Split(',')[2].ToString();
                    info.NameKo = "";
                    info.NameEn = "";
                    info.NameJa = "";
                    info.NameFr = "";
                    info.NameZhcn = "";
                    info.NameZhtw = "";
                    info.NickName = "";
                    info.Mail = item.Split(',')[3].ToString();
                    info.HomeZipCode = "";
                    info.HomeCountry = "";
                    info.HomeCity = "";
                    info.HomeGu = "";
                    info.HomeAddress = "";
                    info.HomePhone = "";
                    info.HomeFax = "";
                    info.MobilePhone = "";
                    info.PrivateHomePage = "";
                    info.CompanyZipCode = "";
                    info.CompanyCountry = "";
                    info.CompanyCity = "";
                    info.CompanyGu = "";
                    info.CompanyAddress = "";
                    info.CompanyPhone = "";
                    info.CompanyFax = "";
                    info.CompanyHomePage = "";
                    info.Pager = "";
                    info.CompanyBase = "";
                    info.CompanyKo = "";
                    info.CompanyEn = "";
                    info.CompanyJA = "";
                    info.CompanyFr = "";
                    info.CompanyZhcn = "";
                    info.CompanyZhtw = "";
                    info.TitleBase = item.Split(',')[1].ToString();
                    info.TitleKo = "";
                    info.TitleEn = "";
                    info.TitleJa = "";
                    info.TitleFr = "";
                    info.TitleZhcn = "";
                    info.TitleZhtw = "";
                    info.DeptBase = item.Split(',')[0].ToString();
                    info.DeptKo = "";
                    info.DeptEn = "";
                    info.DeptJa = "";
                    info.DeptFr = "";
                    info.DeptZhcn = "";
                    info.DeptZhtw = "";
                    info.OfficeAddress = "";
                    info.Memo = "";
                    info.Birthday = "";
                    info.Lunar = "";
                    info.IpPhone = "";
                    info.DefaultPhone = "";
                    info.PhotoName = "";
                    info.IsReceive = "0";
                    info.SendUserID = CurrentUserInfo.UserID;

                    dal.Add(info);

                }
            }

            Helpers.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("CompleteSelectCopyAddress"), "location='" + Request.Url.ToString() + "';");
        }

        public void AddCategory()
        {
            Address.BE.PrivateCategoryInfo categoryInfo = new BE.PrivateCategoryInfo();
            categoryInfo.CompanyID = CurrentUserInfo.CompanyID;
            categoryInfo.ParentCategoryID = tbCategoryID.Text.ToNumber<int>();
            categoryInfo.UserID = CurrentUserInfo.UserID;
            categoryInfo.NameBase = tbSubName.Text;

            //MultiName = CTBEnglishName.Text;
            //if (MultiName != "" && MultiName.IndexOf(",") != -1)
            //{
            //    categoryInfo.NameKo = MultiName.Split(',')[0];
            //    categoryInfo.NameEn = MultiName.Split(',')[1];
            //    categoryInfo.NameJa = MultiName.Split(',')[2];
            //    categoryInfo.NameFr = MultiName.Split(',')[3];
            //    categoryInfo.NameZhcn = MultiName.Split(',')[4];
            //    categoryInfo.NameZhtw = MultiName.Split(',')[5];
            //}


            DAL.PrivateCategory bizCat = new DAL.PrivateCategory();

            if (bizCat.Add(categoryInfo) > 0)
            {
                //string sc = "location.reload(true);location.href=location.href;history.go(0);";
                //Utility.WriteScript(Page, "success", sc);
                Helpers.WebComponent.Utility.ShowMessageBox(Page, "addSuccess", GetResource("Complete"), "location='" + Request.Url.ToString() + "';");

            }
            else
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("PrivateAdd()");
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }
        #endregion

        #region (이벤트핸들러)
        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            Register();
        }

        /// <summary>
        /// Handles the Click event of the LBTNProcess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LBTNAddCategory_Click(object sender, EventArgs e)
        {
            AddCategory();
        }

        #endregion

        #region 프로퍼티

        #endregion
    }
}