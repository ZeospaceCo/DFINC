using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.Web.Admin.Subsidiary
{
    public partial class Add : Base.BasePageEIPAdmin
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region 변수설정
        public int CurrentPage;
        public string Order;
        //회사별 administrator 의 roleid 값 가져오기
        public int RoleAdmin = 0;
        public string LoginImage;
        public string MenuImage;
        public string VLoginImage;
        public string AMenuImage;
        public string BGloginImg;
        public string BGPortalImg;
        public string LoginImageCnt;
        public string MenuImageCnt;
        public string VLoginImageCnt;
        public string AMenuImageCnt;
        public string BGloginImgCnt;
        public string BGPortalImgCnt;

        #endregion

        #region Page Process (페이지 처리)
        #endregion

        #region Method (함수)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Admin.Subsidiary.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                if (!CurrentUserInfo.IsInRole("admin")) Response.Redirect("/DeskPlusEIP/");
                UploadGUID = System.Guid.NewGuid().ToString();
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

            if (CurrentUserInfo.CompanyID > 0)
            {
                //회사별 administrator 의 roleid 값 가져오기
                DAL.Role roleDal = new DAL.Role();
                List<RoleInfo> roleList =  roleDal.GetRole(CurrentUserInfo.CompanyID, "admin", "1");
                if (roleList.Count > 0) RoleAdmin = roleList[0].RoleID;

                List<Zeospace.DeskPlus.Common.BE.CompanyInfo> al = new List<DeskPlus.Common.BE.CompanyInfo>();
                Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
                al = dal.GetCompany(CurrentUserInfo.CompanyID, false);

                if (al.Count > 0)
                {
                    foreach (Zeospace.DeskPlus.Common.BE.CompanyInfo info in al)
                    {
                        ctbCompanyName.Text = info.NameBase.ToReverseXSS();
                        ctbInterfaceCode.Text = info.InterfaceCode;
                        ctbMuliLang.Text = GetMuiltiLang(
                            info.NameKo.ToReverseXSS(), info.NameEn.ToReverseXSS(),
                            info.NameJa.ToReverseXSS(), info.NameFr.ToReverseXSS(),
                            info.NameZhcn.ToReverseXSS(), info.NameZhtw.ToReverseXSS());


                        if (info.EstablishDate.Trim().Length > 0)
                        {
                            ctbEstablishDate.Text = info.EstablishDate.Trim().Substring(0, 4) + "-" + info.EstablishDate.Trim().Substring(4, 2) + "-"
                                + info.EstablishDate.Trim().Substring(6, 2);
                        }

                        ctbInstitutorName.Text = info.InstitutorName.ToReverseXSS();
                        ctbInstitutorEngName.Text = info.InstitutorEnglishName.ToReverseXSS();

                        //ctbInstitutorRegNumber.Text = info.InstitutorRegistrationNumber;
                        ctbHomePage.Text = info.HomePage;
                        ctbZipCode.Text = info.ZipCode;
                        ctbAddress1.Text = info.Address1.ToReverseXSS();
                        ctbAddress2.Text = info.Address2.ToReverseXSS();
                        ctbEnglishAddress.Text = info.EnglishAddress.ToReverseXSS();
                        ctbPhone.Text = info.Phone;
                        ctbFax.Text = info.Fax;
                        cbxActive.Checked = (info.ActiveCompany == "1") ? true : false;
                        ctbCompanyQuota.Text = (info.CompanyQuota < 0) ? "" : info.CompanyQuota.ToString();
                        ctbLicenseCnt.Text = info.LicenseCnt.ToString();
                        ctbSlogan.Text = info.Slogan.ToReverseXSS();

                        LoginImageCnt = info.Lgn_logo;
                        MenuImageCnt = info.Logo;
                        VLoginImageCnt = info.Vlgn_logo;
                        AMenuImageCnt = info.Alogo;
                        BGloginImgCnt = info.Bg;
                        BGPortalImgCnt = info.Mainback;

                        LoadImage(info.CompanyID);
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-03-15
        /// 작성목적 : Loads the Image
        /// </summary>
        /// <param name="_companyID"></param>
        private void LoadImage(int _companyID)
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");

            //기본 이미지 
            LoginImage = this.BaseImgPath + "/Login/lgn_logo.png";
            VLoginImage = this.BaseImgPath + "/Login/Vlgn_logo.png";
            MenuImage = this.BaseImgPath + "/Portal/logo.png";            
            AMenuImage = this.BaseImgPath + "/Portal/Alogo.gif";
            BGloginImg = this.BaseImgPath + "/DStyle/bg1.jpg";
            BGPortalImg = this.BaseImgPath + "/DStyle/mainback1.jpg";

            //회사별 이미지
            string ciLoginImage = baseFileDirectory + _companyID + "/lgn_logo.png";
            string ciVLoginImage = baseFileDirectory + _companyID + "/Vlgn_logo.png";
            string ciMenuImage = baseFileDirectory + _companyID + "/logo.png";            
            string ciAMenuImage = baseFileDirectory + _companyID + "/Alogo.gif";
            string ciBGloginImg = baseFileDirectory + _companyID + "/bg.jpg";
            string ciBGPortalImg = baseFileDirectory + _companyID + "/mainback.jpg";

            #region Login / Menu 이미지
            //이미지 존재 유무 확인
            if (System.IO.File.Exists(Server.MapPath(ciLoginImage)))
            {
                //LoginImage = ciLoginImage + "?t=" + System.Guid.NewGuid().ToString();
                LoginImage = ciLoginImage + "?v=" + LoginImageCnt;
            }

            /*
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", LoginImage);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            LoginImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
            */

            if (System.IO.File.Exists(Server.MapPath(ciMenuImage)))
            {
                MenuImage = ciMenuImage + "?v=" + MenuImageCnt;
            }

            if (System.IO.File.Exists(Server.MapPath(ciVLoginImage)))
            {
                VLoginImage = ciVLoginImage + "?v=" + VLoginImageCnt;
            }

            if (System.IO.File.Exists(Server.MapPath(ciAMenuImage)))
            {
                AMenuImage = ciAMenuImage + "?v=" + AMenuImageCnt;
            }

            if (System.IO.File.Exists(Server.MapPath(ciBGloginImg)))
            {
                BGloginImg = ciBGloginImg + "?v=" + BGloginImgCnt;
            }

            if (System.IO.File.Exists(Server.MapPath(ciBGPortalImg)))
            {
                BGPortalImg = ciBGPortalImg + "?v=" + BGPortalImgCnt;
            }

            #endregion
        }

        /// <summary>
        /// 작성목적   : 파라미터 설정
        /// 작성자     : 
        /// </summary>
        private void LoadParam()
        {
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
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
                cbtnMod.Text = resMng.GetString("Save");// "수정";
                cbtnComUserAdd.Text = resMng.GetString("Add");//"추가";
                cbtnComUserDel.Text = resMng.GetString("Del");//"삭제";

            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 인사 등록하기
        /// </summary>
        private bool Register()
        {
            Zeospace.DeskPlus.Common.DAL.Company dal = new DeskPlus.Common.DAL.Company();
            Zeospace.DeskPlus.Common.BE.CompanyInfo info = new DeskPlus.Common.BE.CompanyInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.CompanyCode = "";
            info.InterfaceCode = ctbInterfaceCode.Text.Trim();
            info.NameBase = ctbCompanyName.Text;

            string multiName = ctbMuliLang.Text.Trim();
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.NameKo = multiName.Split(',')[0];
                info.NameEn = multiName.Split(',')[1];
                info.NameJa = multiName.Split(',')[2];
                info.NameFr = multiName.Split(',')[3];
                info.NameZhcn = multiName.Split(',')[4];
                info.NameZhtw = multiName.Split(',')[5];
            }
            
            info.EstablishDate = ctbEstablishDate.Text.Replace("-","");
            info.InstitutorName = ctbInstitutorName.Text;
            info.InstitutorEnglishName = ctbInstitutorEngName.Text;
            //info.InstitutorRegistrationNumber = ctbInstitutorRegNumber.Text;
            info.IndustryClass = "";
            info.BusinessCondition = "";
            info.HomePage = ctbHomePage.Text;
            info.ZipCode = ctbZipCode.Text;
            info.Address1 = ctbAddress1.Text;
            info.Address2 = ctbAddress2.Text;
            info.EnglishAddress = ctbEnglishAddress.Text;
            info.Phone = ctbPhone.Text;
            info.Fax = ctbFax.Text;
            info.CompanyQuota = ctbCompanyQuota.Text.Trim().ToNumber<int>();
            info.ActiveCompany = (cbxActive.Checked) ? "1" : "0";
            info.WriteUserID = CurrentUserInfo.UserID;
            info.LicenseCnt = ctbLicenseCnt.Text.ToNumber<int>();
            info.Slogan = ctbSlogan.Text;

            return dal.Mod(info);

        }

        #endregion

        #region Event Handler (이벤트 핸들러)

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();

                if (Register())
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "success", GetResource("SUCCESS"), "");
                }
                else
                {                    
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                }

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));
                paraDic.Add("goURL", "/DeskPlusEIP/EIP/Admin/Subsidiary/Add.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);   
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnRegister_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// UploadGUID
        /// </summary>
        public string UploadGUID
        {
            get { return GetViewStateString("UploadGUID", ""); }
            set { ViewState["UploadGUID"] = value; }
        }
        #endregion
    }
}