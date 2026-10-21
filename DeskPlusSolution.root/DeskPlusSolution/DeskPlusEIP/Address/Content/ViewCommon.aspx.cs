using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Address.BE;
using System.Data;

namespace Zeospace.DeskPlus.Address.Web.Content
{
    public partial class ViewCommon : Base.BasePageAddress
    {
        #region 변수설정
        public int CategoryID;
        public int CatID;
        public string Types;
        public string RTypes;
        public int KeyID;        
        public int CurrentPage = 0;
        public string SearchOption = string.Empty; //검색옵션
        public string SearchKey = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchFilter = string.Empty; //ㄱㄴㄷㄹ
        public string Order = string.Empty; //sort 
        public string PhotoName = "";
        public string GoPage = "";
        public string PageUrl = ""; //중요표시에서 넘어올때만 사용

        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Content.ViewCommon).Assembly;	//어셈블리 선언
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

            //수정 모드
            if (KeyID > 0)
            {
                LoadCommon();
            }
        }

        /// <summary>
        /// 수정 모드
        /// </summary>
        private void LoadCommon()
        {
            DAL.Common dal = new DAL.Common();
            List<BE.CommonInfo> al = dal.GetCommonLists(CurrentUserInfo.CompanyID, CategoryID, KeyID);

            if (al != null)
            {
                foreach (BE.CommonInfo info in al)
                {
                    ctbFamilyName.Text = info.FamilyName.ToReverseXSS();
                    ctbLastName.Text = info.LastName.ToReverseXSS();
                    ctbMiddleName.Text = info.MiddleName.ToReverseXSS();
                    ctbFamilyName.Text = info.FamilyName.ToReverseXSS();
                    ctbMiddleName.Text = info.MiddleName.ToReverseXSS();
                    ctbLastName.Text = info.LastName.ToReverseXSS();

                    ctbEnglishName.Text = GetMuiltiLang(info.NameKo.ToReverseXSS(),
                        info.NameEn.ToReverseXSS(), info.NameJa.ToReverseXSS(), info.NameFr.ToReverseXSS(), 
                        info.NameZhcn.ToReverseXSS(), info.NameZhtw.ToReverseXSS());

                    ctbNickName.Text = info.NickName.ToReverseXSS();
                    ctbMail.Text = info.Mail;
                    ddlLunar.SelectedValue = info.Lunar;

                    ctbBirthday.Text = info.Birthday;
                    ddlSex.SelectedValue = info.Sex;
                    ctbMemo.Text = info.Memo.ToReverseXSS();
                    ctbHomepage.Text = info.PrivateHomePage;                    

                    PhotoName = "";
                    if (info.PhotoName.Length > 0)
                    {
                        string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                        string thumImgPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Address/" + info.PhotoName.Split('.')[0] + "_S." + info.PhotoName.Split('.')[1];
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", thumImgPath);
                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                        PhotoName = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                    }

                    ctbPhone.Text = info.HomePhone.ToReverseXSS();
                    ctbMobilePhone.Text = info.MobilePhone.ToReverseXSS();
                    ctbFax.Text = info.HomeFax.ToReverseXSS();
                    ctbHomeZipCode.Text = info.HomeZipCode;
                    ctbHomeCountry.Text = info.HomeCountry.ToReverseXSS();
                    ctbHomeCity.Text = info.HomeCity.ToReverseXSS();
                    ctbHomeGu.Text = info.HomeGu.ToReverseXSS();
                    ctbHomeAddress.Text = info.HomeAddress;

                    ctbCompany.Text = info.CompanyBase.ToReverseXSS();
                    ctbCompanyMulti.Text = GetMuiltiLang(info.CompanyKo.ToReverseXSS(),
                        info.CompanyEn.ToReverseXSS(), info.CompanyJA.ToReverseXSS(), info.CompanyFr.ToReverseXSS(), info.CompanyZhcn.ToReverseXSS(), info.CompanyZhtw.ToReverseXSS());


                    ctbOfficeAddress.Text = info.OfficeAddress;
                    ctbCompanyHomePage.Text = info.CompanyHomePage;

                    ctbDepartment.Text = info.DeptBase.ToReverseXSS();
                    ctbDepartmentMulti.Text = GetMuiltiLang(info.DeptKo.ToReverseXSS(),
                        info.DeptEn.ToReverseXSS(), info.DeptJa.ToReverseXSS(), info.DeptFr.ToReverseXSS(), info.DeptZhcn.ToReverseXSS(), info.DeptZhtw.ToReverseXSS());

                    ctbTitle.Text = info.TitleBase;
                    ctbTitleMulti.Text = GetMuiltiLang(info.TitleKo,
                        info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw);

                    ctbCompanyPhone.Text = info.CompanyPhone.ToReverseXSS();
                    ctbCompanyFax.Text = info.CompanyFax.ToReverseXSS();
                    ctbIpPhone.Text = info.IpPhone.ToReverseXSS();
                    ctbPager.Text = info.Pager;

                    ctbCompanyZipCode.Text = info.CompanyZipCode;
                    ctbCompanyCountry.Text = info.CompanyCountry.ToReverseXSS();
                    ctbCompanyCity.Text = info.CompanyCity.ToReverseXSS();
                    ctbCompanyGu.Text = info.CompanyGu.ToReverseXSS();
                    ctbCompanyAddress.Text = info.CompanyAddress;

                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            CategoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            CatID = GetDecryptParam("catID", "0").ToNumber<int>();
            Types = GetDecryptParam("types", "");
            RTypes = GetDecryptParam("rTypes", "");
            KeyID = GetDecryptParam("keyID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchOption = GetDecryptParam("searchOption", "");
            SearchKey = GetDecryptParam("searchKey", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchFilter = GetDecryptParam("searchFilter", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "Orders ASC" : Order;
            PageUrl = GetDecryptParam("pageUrl", "ListImpo.aspx"); //중요표시에서 넘어올때만 사용

            string[] getCatUserInfo = GetCategoryUser().Split(':');
            GoPage = "/DeskPlusEIP/Address/Content/";
            if (RTypes.Equals("IG")) //중요표시
            {
                GoPage += (getCatUserInfo[0] == "1") ? "ListImpo.aspx" : "ListBlogImpo.aspx";
            }
            else
            {
                GoPage += (getCatUserInfo[0] == "1") ? "List.aspx" : "ListBlog.aspx";
            }  
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

                cbtnCancel.Text = resMng.GetString("BtnCancel");             // "목록"

                ddlLunar.Items.Clear();
                ddlLunar.Items.Add(new ListItem(resMng.GetString("Solar"), "S"));
                ddlLunar.Items.Add(new ListItem(resMng.GetString("Lunar"), "L"));                
            }

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "SEX");
            this.ddlSex.Items.Clear();
            ddlSex.AppendDataBoundItems = true;
            ddlSex.Items.Add(new ListItem("-------", ""));
            string name = "";
            for (int i = 0; i < dv.Count; i++)
            {
                name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlSex.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 카테고리별 사용자 설정 정보 가져오기
        /// </summary>
        /// <returns></returns>
        public string GetCategoryUser()
        {
            BE.CategoryUserInfo info = new CategoryUserInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.CategoryID = CategoryID;
            info.Types = RTypes; //트리에서 선택한 주소록(FG :즐겨찾기;IG:중요표시)

            DAL.CategoryUser dal = new DAL.CategoryUser();
            List<CategoryUserInfo> al = dal.GetCategoryUserLists(info);

            foreach (BE.CategoryUserInfo userInfo in al)
            {
                return userInfo.ListType + ":" + userInfo.PageSize;
            }

            if (RTypes.ToUpper() == "IG")  //중요표시에서 넘어왔으면
            {
                if (PageUrl.IndexOf("ListBlogImpo") > -1) //블로그타입인지 체크함
                {
                    return "2:10";
                }
            }

            return "1:10";
        }

        #endregion

        #region (속성)

        

        #endregion

        #region (이벤트핸들러)                

        #endregion
    }
}