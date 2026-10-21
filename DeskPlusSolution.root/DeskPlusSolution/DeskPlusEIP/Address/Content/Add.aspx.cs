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
    public partial class Add : Base.BasePageAddress
    {
        #region 변수설정
        public int CategoryID;
        public int CatID;
        public string Types;
        public string RTypes;
        public int KeyID;
        private string eventArgs = String.Empty;
        public int CurrentPage = 0;
        public string SearchOption = string.Empty; //검색옵션
        public string SearchKey = string.Empty; //검색키워드
        public string SearchValue = string.Empty; //검색어
        public string SearchFilter = string.Empty; //ㄱㄴㄷㄹ
        public string Order = string.Empty; //sort 
        public string PhotoName = "";        
        private string MultiName = String.Empty;
        public string GoPage = "";
        public string PageUrl = ""; //중요표시에서 넘어올때만 사용
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Address.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Address.Web.Content.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
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
                LoadPrivate();
            }
        }

        /// <summary>
        /// 수정 모드
        /// </summary>
        private void LoadPrivate()
        {
            DAL.Private dal = new DAL.Private();
            List<BE.PrivateInfo> al = dal.GetPrivateLists(CurrentUserInfo.CompanyID, 
                CategoryID, KeyID, CurrentUserInfo.UserID);

            if (al != null)
            {
                foreach (BE.PrivateInfo info in al)
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

                    tbPhotoOLD.Text = info.PhotoName; //사진이미지

                    PhotoName = "";
                    if(info.PhotoName.Length > 0)
                    {
                        string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                        string thumImgPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Address/" + info.PhotoName.Split('.')[0] + "_S." + info.PhotoName.Split('.')[1];
                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", thumImgPath);
                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                        PhotoName = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                    }

                    ctbPhone.Text = info.HomePhone;
                    ctbMobilePhone.Text = info.MobilePhone;
                    ctbFax.Text = info.HomeFax;
                    ctbHomeZipCode.Text = info.HomeZipCode;
                    ctbHomeCountry.Text = info.HomeCountry.ToReverseXSS();
                    ctbHomeCity.Text = info.HomeCity.ToReverseXSS();
                    ctbHomeGu.Text = info.HomeGu.ToReverseXSS();
                    ctbHomeAddress.Text = info.HomeAddress.ToReverseXSS();

                    ctbCompany.Text = info.CompanyBase.ToReverseXSS();
                    ctbCompanyMulti.Text = GetMuiltiLang(info.CompanyKo.ToReverseXSS(),
                        info.CompanyEn.ToReverseXSS(), 
                        info.CompanyJA.ToReverseXSS(), 
                        info.CompanyFr.ToReverseXSS(), 
                        info.CompanyZhcn.ToReverseXSS(), 
                        info.CompanyZhtw.ToReverseXSS());


                    ctbOfficeAddress.Text = info.OfficeAddress.ToReverseXSS();
                    ctbCompanyHomePage.Text = info.CompanyHomePage;

                    ctbDepartment.Text = info.DeptBase.ToReverseXSS();
                    ctbDepartmentMulti.Text = GetMuiltiLang(info.DeptKo.ToReverseXSS(),
                        info.DeptEn.ToReverseXSS(), 
                        info.DeptJa.ToReverseXSS(), 
                        info.DeptFr.ToReverseXSS(), 
                        info.DeptZhcn.ToReverseXSS(), 
                        info.DeptZhtw.ToReverseXSS());

                    ctbTitle.Text = info.TitleBase.ToReverseXSS();
                    ctbTitleMulti.Text = GetMuiltiLang(info.TitleKo.ToReverseXSS(),
                        info.TitleEn.ToReverseXSS(),
                        info.TitleJa.ToReverseXSS(), 
                        info.TitleFr.ToReverseXSS(), 
                        info.TitleZhcn.ToReverseXSS(), 
                        info.TitleZhtw.ToReverseXSS());

                    ctbCompanyPhone.Text = info.CompanyPhone;
                    ctbCompanyFax.Text = info.CompanyFax;
                    ctbIpPhone.Text = info.IpPhone;
                    ctbPager.Text = info.Pager;

                    ctbCompanyZipCode.Text = info.CompanyZipCode;
                    ctbCompanyCountry.Text = info.CompanyCountry.ToReverseXSS();
                    ctbCompanyCity.Text = info.CompanyCity.ToReverseXSS();
                    ctbCompanyGu.Text = info.CompanyGu.ToReverseXSS();
                    ctbCompanyAddress.Text = info.CompanyAddress.ToReverseXSS();
                    
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
            
            SearchOption = GetDecryptParam("searchOption", "");
            SearchKey = GetDecryptParam("searchKey", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchFilter = GetDecryptParam("searchFilter", "");
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
                cbtnConfirm.Text = resMng.GetString("BtnSave");         // "확인";
                cbtnCancel.Text = resMng.GetString("BtnCancel");             // "목록";
                cbtnSwfFileAdd.Text = resMng.GetString("UploadPicture");      //"사진등록(52x52)";

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
        /// 등록하기
        /// </summary>
        private void Register()
        {
            #region 툴팁 제거

            if (ctbPhone.Text == "02-0000-0000") ctbPhone.Text = "";
            if (ctbMobilePhone.Text == "010-0000-0000") ctbMobilePhone.Text = "";
            if (ctbFax.Text == "02-0000-0000") ctbFax.Text = "";
            if (ctbHomeZipCode.Text == "000-000") ctbHomeZipCode.Text = "";
            if (ctbCompanyHomePage.Text == "http://www.example.com") ctbCompanyHomePage.Text = "";
            if (ctbCompanyPhone.Text == "02-0000-0000") ctbCompanyPhone.Text = "";
            if (ctbCompanyFax.Text == "02-0000-0000") ctbCompanyFax.Text = "";
            if (ctbCompanyZipCode.Text == "000-000") ctbCompanyZipCode.Text = "";

            #endregion

            BE.PrivateInfo info = new BE.PrivateInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.UserID = CurrentUserInfo.UserID;
            info.LoginID = CurrentUserInfo.LogInID;
            info.CategoryID = CategoryID;            
            info.PrivateID = 0;
            info.FamilyName = ctbFamilyName.Text.Trim();
            info.LastName = ctbLastName.Text.Trim();
            info.MiddleName = ctbMiddleName.Text.Trim();
            info.WholeName = info.FamilyName.ToNull() +
                info.MiddleName.ToNull() + info.LastName.ToNull();
            info.NickName = ctbNickName.Text.Trim();

            //기본언어
            info.NameBase = info.FamilyName.ToNull() +
                info.MiddleName.ToNull() + info.LastName.ToNull();

            MultiName = ctbEnglishName.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                info.NameKo = MultiName.Split(',')[0];
                info.NameEn = MultiName.Split(',')[1];
                info.NameJa = MultiName.Split(',')[2];
                info.NameFr = MultiName.Split(',')[3];
                info.NameZhcn = MultiName.Split(',')[4];
                info.NameZhtw = MultiName.Split(',')[5];
            }
            
            info.Mail = ctbMail.Text.Trim();
            info.Lunar = ddlLunar.SelectedValue;
            info.Birthday = ctbBirthday.Text.Trim();
            info.Sex = ddlSex.SelectedValue;
            info.Memo = ctbMemo.Text.Trim();
            info.PrivateHomePage = ctbHomepage.Text.Trim();
            info.HomePhone = ctbPhone.Text.Trim();
            info.MobilePhone = ctbMobilePhone.Text.Trim();
            info.HomeFax = ctbFax.Text.Trim();
            info.HomeZipCode = ctbHomeZipCode.Text.Trim();
            info.HomeCountry = ctbHomeCountry.Text.Trim();
            info.HomeCity = ctbHomeCity.Text.Trim();
            info.HomeGu = ctbHomeGu.Text.Trim();
            info.HomeAddress = ctbHomeAddress.Text.Trim();
            info.CompanyBase = ctbCompany.Text;

            MultiName = ctbCompanyMulti.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                info.CompanyKo = MultiName.Split(',')[0];
                info.CompanyEn = MultiName.Split(',')[1];
                info.CompanyJA = MultiName.Split(',')[2];
                info.CompanyFr = MultiName.Split(',')[3];
                info.CompanyZhcn = MultiName.Split(',')[4];
                info.CompanyZhtw = MultiName.Split(',')[5];
            }

            info.OfficeAddress = ctbOfficeAddress.Text.Trim();
            info.CompanyHomePage = ctbCompanyHomePage.Text.Trim();
            info.DeptBase = ctbDepartment.Text.Trim();

            MultiName = ctbDepartmentMulti.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                info.DeptKo = MultiName.Split(',')[0];
                info.DeptEn = MultiName.Split(',')[1];
                info.DeptJa = MultiName.Split(',')[2];
                info.DeptFr = MultiName.Split(',')[3];
                info.DeptZhcn = MultiName.Split(',')[4];
                info.DeptZhtw = MultiName.Split(',')[5];
            }

            info.TitleBase = ctbTitle.Text.Trim();
            MultiName = ctbTitleMulti.Text;
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                info.TitleKo = MultiName.Split(',')[0];
                info.TitleEn = MultiName.Split(',')[1];
                info.TitleJa = MultiName.Split(',')[2];
                info.TitleFr = MultiName.Split(',')[3];
                info.TitleZhcn = MultiName.Split(',')[4];
                info.TitleZhtw = MultiName.Split(',')[5];
            }

            info.CompanyPhone = ctbCompanyPhone.Text.Trim();
            info.CompanyFax = ctbCompanyFax.Text.Trim();

            info.IpPhone = ctbIpPhone.Text.Trim();
            info.Pager = ctbPager.Text.Trim();            
                
            info.CompanyZipCode = ctbCompanyZipCode.Text.Trim();
            info.CompanyCountry = ctbCompanyCountry.Text.Trim();
            info.CompanyCity = ctbCompanyCity.Text.Trim();
            info.CompanyGu = ctbCompanyGu.Text.Trim();
            info.CompanyAddress = ctbCompanyAddress.Text.Trim();
            info.DefaultPhone = "";
            info.IsReceive = "0";
            info.SendUserID = 0;

            info.PhotoName = tbPhotoOLD.Text;

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            if (tbPhotoNEW.Text.Length > 0)
            {
                FileHelper.CreateSubFolder(baseFileDirectory + CurrentUserInfo.CompanyID + "/Address");

                #region SPOOL 파일을 ADDRESS 그룹에 복사하기
                string souceFile = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
                souceFile = string.Format(souceFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, tbPhotoNEW.Text);

                string destFile = baseFileDirectory + "{0}/Address/{1}";
                string[] fileStr = tbPhotoNEW.Text.Split('.');
                destFile = string.Format(destFile, CurrentUserInfo.CompanyID, SwfUploadGUID + "." + fileStr[1]);

                System.IO.File.Copy(Server.MapPath(souceFile), Server.MapPath(destFile));
                info.PhotoName = SwfUploadGUID + "." + fileStr[1];
                #endregion

                #region 섬네일 이미지
                System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(souceFile));
                //Size thumbnailSize = UtilHelper.GetThumbnailSize(image, 52);
                Size thumbnailSize = new Size(52, 52);

                System.Drawing.Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                destFile = baseFileDirectory + "{0}/Address/{1}";
                destFile = string.Format(destFile, CurrentUserInfo.CompanyID, SwfUploadGUID + "_S." + fileStr[1]);
                thumbnail.Save(Server.MapPath(destFile));
                #endregion
            }

            DAL.Private dal = new DAL.Private();

            if (KeyID > 0)
            {
                info.PrivateID = KeyID;
                dal.Mod(info);
            }
            else
            {
                dal.Add(info);
            }


            string[] getCatUserInfo = GetCategoryUser().Split(':');
            string goPageList = "/DeskPlusEIP/Address/Content/";
            if (RTypes.Equals("IG")) //중요표시
            {
                goPageList += (getCatUserInfo[0] == "1") ? "ListImpo.aspx" : "ListBlogImpo.aspx";
            }
            else
            {
                goPageList += (getCatUserInfo[0] == "1") ? "List.aspx" : "ListBlog.aspx";
            }

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("categoryID", CatID.ToString());
            paraDic.Add("types", Types);
            paraDic.Add("rTypes", RTypes); 
            paraDic.Add("currentPage", CurrentPage.ToString());            
            paraDic.Add("order", Order);

            paraDic.Add("searchKey", SearchKey);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("searchFilter", SearchFilter);
            paraDic.Add("searchOption", SearchOption);

            paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
            paraDic.Add("alertMsg", GetResource("GoList"));  //"목록으로 이동합니다."
            paraDic.Add("goURL", goPageList);

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            Utility.Redirect(encParams);

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
            if (RTypes.ToUpper() == "IG") //중요표시에서 넘어왔으면
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

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        } 

        #endregion

        #region (이벤트핸들러)

        /// <summary>
        /// Handles the Click event of the lbtnRegister control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                string args = eventArgs;

                switch (args)
                {                    
                    case "Register":  //추가 및 수정
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