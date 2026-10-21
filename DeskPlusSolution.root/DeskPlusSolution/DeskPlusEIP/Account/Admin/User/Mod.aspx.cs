using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Admin.User
{
    public partial class Mod : Base.BasePageAccountAdmin
    {
        #region 변수설정

        public int UserID;
        public int DeptID;
        public string PhotoImage;
        public string SealImage;
        public string GroupGUID;

        private string eventArgs = String.Empty;
        private string MultiName = String.Empty;        

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.Mod).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region Method (함수)

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
            LoadUser();

            string useMultiDomain = AppSettingHelper.GetAppSetting(AppSettingType.ACCOUNT, "UseMultiDomain", "N");
            if (useMultiDomain == "N") cbtnMultiDomain.Visible = false;

            MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "").ToUpper();
            if (MailProduct == "")
            {
                cbtnMultiDomain.Visible = false;
                cbtnMailPassword.Visible = false;
            }

            DAL.UserCompany dal = new DAL.UserCompany();
            List<BE.UserCompanyInfo> alList = dal.GetUserCompany(CurrentUserInfo.CompanyID, UserID);
            if (alList.Count > 0) GroupGUID = alList[0].GroupGUID;

            // 근태 사용여부 가져오기
            GetHREnv();
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
                ddlMailDomain.Items.Add(new ListItem(dv[i]["CodeID"].ToString().ToLower(), dv[i]["CodeID"].ToString().ToLower()));
            }

            dv = null;
            string name = "";

            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "SEX");
            this.ddlSex.Items.Clear();
            ddlSex.AppendDataBoundItems = true;
            ddlSex.Items.Add(new ListItem("-------", ""));

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

            dv = null;

            dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "MultiLang");
            this.ddlBasicLanguage.Items.Clear();
            ddlBasicLanguage.AppendDataBoundItems = true;
            ddlBasicLanguage.Items.Add(new ListItem("-------", ""));

            for (int i = 0; i < dv.Count; i++)
            {
                name = this.GetLangName(
                    dv[i]["NameBase"].ToString(),
                    dv[i]["NameKo"].ToString(),
                    dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                    dv[i]["NameFr"].ToString(), dv[i]["NameZhCn"].ToString(),
                    dv[i]["NameZhTw"].ToString(), PreLang);

                ddlBasicLanguage.Items.Add(new ListItem(name, dv[i]["CodeID"].ToString()));
            }
        }

        /// <summary>
        /// 용도 : 사용자 정보 가져오기
        /// 작성자 : 심재운
        /// </summary>
        private void LoadUser()
        {
            Zeospace.DeskPlus.Account.DAL.User biz = new DAL.User();
            //관리 인사에는 비활성화 한 사용자도 보여야 한다.
            List<UserInfo> al = biz.GetUserInfo(CurrentUserInfo.CompanyID, UserID, false, false);

            foreach (UserInfo be in al)
            {
                ctbLoginID.Text = be.LogInID;
                ctbName.Text = be.NameBase.ToReverseXSS();
                ctbMuliLang.Text = GetMuiltiLang(
                        be.NameKo.ToReverseXSS(), be.NameEn.ToReverseXSS(), be.NameJa.ToReverseXSS(), be.NameFr.ToReverseXSS(), be.NameZhCn.ToReverseXSS(), be.NameZhTw.ToReverseXSS());

                //ctbPassword.Attributes.Add("value", be.Password);
                //ctbMailPassword.Attributes.Add("value", be.MailPassword);

                ctbSysCode.Text = be.UserCode.ToString().ToNull();
                ctbEmployeeNumber.Text = be.EmployeeNumber.ToReverseXSS();
                ctbResNumber.Text = be.RegistrationNumber.ToReverseXSS();
                ddlSex.SelectedValue = be.Sex;
                cbxLunar.Checked = (be.Lunar == "L") ? true : false;

                #region 숫자8자리를 다국어날짜형태로 반환하기

                ctbBirthDay.Text = DateHelper.GetCultureDateChar8(be.BirthDay, PreLangReal);
                ctbJoinDate.Text = DateHelper.GetCultureDateChar8(be.JoinDate, PreLangReal);
                ctbRetireDate.Text = DateHelper.GetCultureDateChar8(be.RetireDate, PreLangReal);
                ctbWedAnn.Text = DateHelper.GetCultureDateChar8(be.WeddingAnniversary, PreLangReal);

                #endregion 숫자8자리를 다국어날짜형태로 반환하기

                ctbDescription.Text = be.Description.ToReverseXSS();
                ctbHomeZipCode.Text = be.HomeZipCode;
                ctbHomeAddress1.Text = be.HomeAddress1.ToReverseXSS();
                ctbHomeAddress2.Text = be.HomeAddress2.ToReverseXSS();
                ctbBirthZipCode.Text = be.BirthZipCode;
                ctbBirthAddress1.Text = be.BirthAddress1.ToReverseXSS();
                ctbBirthAddress2.Text = be.BirthAddress2.ToReverseXSS();
                ctbMobilePhone.Text = be.MobilePhone.ToReverseXSS();
                ctbHomePhone.Text = be.HomePhone.ToReverseXSS();
                ctbOfficePhone.Text = be.OfficePhone.ToReverseXSS();
                ctbExtOfficePhone.Text = be.ExtensionOfficePhone.ToReverseXSS();
                ctbFax.Text = be.Fax.ToReverseXSS();
                //ctbMailQuota.Text = be.MailQuota.ToString();
                ctbPrivateQuota.Text = be.PrivateQuota.ToString();
                ctbSecurityLogIn.Text = be.SecurityLogIn.ToReverseXSS();
                //ctbMailID.Text = be.MailID;
                //ddlMailDomain.SelectedValue = be.MailDomain;
                cbxActiveUser.Checked = (be.ActiveUser == "1") ? true : false;

                ctbMailID.Text = be.MailID;
                ddlMailDomain.SelectedValue = be.MailDomain.ToLower();
                ddlBasicLanguage.SelectedValue = be.PreferredLanguage;

                MailID = be.MailID;
                MailDomain = be.MailDomain;
                MailPassword = (be.MailPassword.Equals("")) ? be.Password : be.MailPassword;
                OldNameBase = ctbName.Text;
                MailDesign = be.MailDesign;
                ctbAuthEmail.Text = be.AuthEmail.ToString();

                LoadPhotoSeal(be.PhotoImage, be.SealImage);

                if (String.IsNullOrEmpty(be.OTP))
                {
                    cbtnOTP.Visible = false;
                }
                else
                {
                    cbtnOTP.Visible = true;
                }

                cbxOTPYN.Checked = (be.OTPYN == "Y") ? true : false;
            }

            Zeospace.DeskPlus.Account.DAL.UserEnvironment dalEnv = new DAL.UserEnvironment();
            List<BE.UserEnvironmentInfo> alEnv = dalEnv.GetUserEnv(CurrentUserInfo.CompanyID, UserID);
            foreach (UserEnvironmentInfo info in alEnv)
            {
                rbTodo.SelectedValue = (info.IsTodo.Equals("")) ? "N" : info.IsTodo;

                //cbxAlarmMail.Checked = (info.AlarmMail == "1") ? true : false;
                //cbxAlarmMessenger.Checked = (info.AlarmMessenger == "1") ? true : false;
                //cbxAlarmSMS.Checked = (info.AlarmSMS == "1") ? true : false;
            }

            Zeospace.DeskPlus.Account.DAL.UserSNS dalSNS = new DAL.UserSNS();
            List<BE.UserSNSInfo> alSNS = dalSNS.GetUserSNSInfo(CurrentUserInfo.CompanyID, UserID);

            foreach (UserSNSInfo info in alSNS)
            {
                //패스워드는 보여주지 않는다.
                tbFacebook.Text = info.FaceBook;
                tbTwitter.Text = info.Twitter;
                tbMe2day.Text = info.Me2day;
                tbGoogle.Text = info.Google;
                tbNaverBlog.Text = info.NaverBlog;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 사진 및 도장 이미지 가져오기
        /// </summary>
        /// <param name="_photo">The _photo.</param>
        /// <param name="_seal">The _seal.</param>
        private void LoadPhotoSeal(string _photo, string _seal)
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            #region 이미지 경로 암호화

            string images = this.BaseImgPath + "/Icon/ico_noimg.gif";
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", images);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

            string images2 = this.BaseImgPath + "/Icon/ico_noimg2.gif";
            Dictionary<string, string> paraDic2 = new Dictionary<string, string>();
            paraDic2.Add("filePath", images2);
            string encParams2 = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic2));
            images2 = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams2;

            #endregion 이미지 경로 암호화

            PhotoImage = "<img src='" + images + "' alt='photo' title='photo' />";
            SealImage = "<img src='" + images2 + "' alt='photo' title='photo' />";

            images = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + UserID + "/Picture/" + _photo;

            if (System.IO.File.Exists(Server.MapPath(images)))
            {
                #region 이미지 경로 암호화

                paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", images);
                encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                #endregion 이미지 경로 암호화

                PhotoImage = "<img src='" + images + "' width='94' height='124' alt='' title='' />";
            }

            images = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + UserID + "/Picture/" + _seal;

            if (System.IO.File.Exists(Server.MapPath(images)))
            {
                #region 이미지 경로 암호화

                paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", images);
                encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

                #endregion 이미지 경로 암호화

                SealImage = "<img src='" + images + "' width='94' height='124' alt='' title='' />";
            }
        }

        /// <summary>
        /// 근태 사용여부 가져오기
        /// </summary>
        private void GetHREnv()
        {
            DAL.User dal = new DAL.User();

            DataTableReader dtr = dal.GetHREnv(CurrentUserInfo.CompanyID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        WorkYN = dtr["WorkYN"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            this.GetPagingParam();

            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
            DeptID = GetDecryptParam("deptID", "0").ToNumber<int>();
            SearchGubun = (DeptID == 0) ? "Whole" : "Internal";
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                //cbtnCancel.Text = resMng.GetString("Cancel");
                cbtnUser.Text = resMng.GetString("Admin_UserManager_tabs0");                    // "기본정보";
                cbtnExtend.Text = resMng.GetString("AddBaseInfo");                              // "기본추가정보";
                cbtnDept.Text = resMng.GetString("DeptInfo");                                   //"부서정보";
                //cbtnCard.Text = resMng.GetString("Card");                                       //"카드";
                cbtnSave.Text = resMng.GetString("BtnSave");                    //"저장";
                cbtnDel.Text = resMng.GetString("Admin_DeptManager_btnDel");                    //"삭제";
                cbtnList.Text = resMng.GetString("Cancel");                                       //"목록";
                //cbtnSwfPhotoAdd.Text = resMng.GetString("Register");                            //"등록";
                //cbtnSwfSealAdd.Text = resMng.GetString("Register");                             //"등록";
                cbtnLoginCheck.Text = resMng.GetString("Admin_UserRegister_DoubleCheck");       //"중복체크";
                cbtnMailCheck.Text = resMng.GetString("Admin_UserRegister_DoubleCheck");
                //cbtnBirthPost.Text = resMng.GetString("SearchAddress");                         //"주소검색";
                //cbtnHomePost.Text = resMng.GetString("SearchAddress");                          //"주소검색";
                cbtnPassword.Text = resMng.GetString("Admin_UserRegister_PasswordChange");       //"패스워드변경";
                cbtnMailPassword.Text = resMng.GetString("Admin_UserRegister_MailPasswordChange");       //"메일패스워드변경";
                cbtnMultiDomain.Text = resMng.GetString("MultiDomain");
                cbtnAuth.Text = resMng.GetString("Auth");  //할당된 권한
                cbtnInfo.Text = resMng.GetString("LinkageInfo");
                cbtnMultiCompany.Text = resMng.GetString("MultiComUserManage");
                cbtnAlarm.Text = resMng.GetString("AlarmSetting");  //알람설정
                cbtnSummary.Text = resMng.GetString("EtcInfo");      //기타정보
                cbtnOTP.Text = "OTP초기화";

                rbTodo.Items[0].Text = resMng.GetString("TodoItemN");
                rbTodo.Items[1].Text = resMng.GetString("TodoItemY");
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 인사 등록하기
        /// </summary>
        private void Register()
        {
            Account.BE.UserInfo be = new UserInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.UserID = UserID;
            be.UserCode = ctbSysCode.Text.Trim();
            be.LogInID = ctbLoginID.Text.Trim();
            be.NameBase = ctbName.Text.Trim();

            MultiName = ctbMuliLang.Text.Trim();
            if (MultiName != "" && MultiName.IndexOf(",") != -1)
            {
                be.NameKo = MultiName.Split(',')[0];
                be.NameEn = MultiName.Split(',')[1];
                be.NameJa = MultiName.Split(',')[2];
                be.NameFr = MultiName.Split(',')[3];
                be.NameZhCn = MultiName.Split(',')[4];
                be.NameZhTw = MultiName.Split(',')[5];
            }

            //be.Password = Request[ctbPassword.UniqueID].ToString();
            be.EmployeeNumber = ctbEmployeeNumber.Text.Trim();
            be.RegistrationNumber = ctbResNumber.Text.Trim();
            be.Sex = ddlSex.SelectedValue;
            be.Lunar = (cbxLunar.Checked) ? "L" : "S";

            #region 다국어날짜형태를 숫자 8자리로 만들기

            be.BirthDay = DateHelper.GetCultureDate(ctbBirthDay.Text.Trim(), "yyyyMMdd", PreLangReal);
            be.JoinDate = DateHelper.GetCultureDate(ctbJoinDate.Text.Trim(), "yyyyMMdd", PreLangReal);
            be.RetireDate = DateHelper.GetCultureDate(ctbRetireDate.Text.Trim(), "yyyyMMdd", PreLangReal);
            be.WeddingAnniversary = DateHelper.GetCultureDate(ctbWedAnn.Text.Trim(), "yyyyMMdd", PreLangReal);

            #endregion 다국어날짜형태를 숫자 8자리로 만들기

            be.Description = ctbDescription.Text.Trim();
            be.HomeZipCode = ctbHomeZipCode.Text.Trim();
            be.HomeAddress1 = ctbHomeAddress1.Text.Trim();
            be.HomeAddress2 = ctbHomeAddress2.Text.Trim();
            be.BirthZipCode = ctbBirthZipCode.Text.Trim();
            be.BirthAddress1 = ctbBirthAddress1.Text.Trim();
            be.BirthAddress2 = ctbBirthAddress2.Text.Trim();
            be.MobilePhone = ctbMobilePhone.Text.Trim();
            be.HomePhone = ctbHomePhone.Text.Trim();
            be.OfficePhone = ctbOfficePhone.Text.Trim();
            be.ExtensionOfficePhone = ctbExtOfficePhone.Text.Trim();
            be.Fax = ctbFax.Text.Trim();
            //be.MailQuota = ctbMailQuota.Text.Trim().ToNumber<int>();
            be.PrivateQuota = ctbPrivateQuota.Text.Trim().ToNumber<int>();
            be.SecurityApproval = "";
            be.SecurityLogIn = ctbSecurityLogIn.Text.Trim();
            be.MailID = ctbMailID.Text.Trim();
            be.MailDomain = ddlMailDomain.SelectedValue;
            //be.MailPassword = Request[ctbMailPassword.UniqueID].ToString();
            be.ActiveUser = (cbxActiveUser.Checked) ? "1" : "0";
            be.WriteUserID = CurrentUserInfo.UserID;
            be.PreferredLanguage = ddlBasicLanguage.SelectedValue;
            be.AuthEmail = ctbAuthEmail.Text.Trim();
            be.OTPYN = (cbxOTPYN.Checked) ? "Y" : "N";

            #region alarm & etc

            UserEnvironmentInfo envInfo = new UserEnvironmentInfo();
            envInfo.CompanyID = be.CompanyID;
            envInfo.UserID = be.UserID;
            //envInfo.AlarmMail = (cbxAlarmMail.Checked == true) ? "1" : "0";
            //envInfo.AlarmMessenger = (cbxAlarmMessenger.Checked == true) ? "1" : "0";
            //envInfo.AlarmSMS = (cbxAlarmSMS.Checked == true) ? "1" : "0";
            envInfo.IsTodo = rbTodo.SelectedValue;

            #endregion alarm

            #region sns

            UserSNSInfo snsInfo = new UserSNSInfo();
            snsInfo.CompanyID = CurrentUserInfo.CompanyID;
            snsInfo.UserID = UserID;
            snsInfo.FaceBook = tbFacebook.Text.Trim();
            snsInfo.FaceBookPwd = tbFacebookPwd.Text.Trim();
            snsInfo.Twitter = tbTwitter.Text.Trim();
            snsInfo.TwitterPwd = tbTwitterPwd.Text.Trim();
            snsInfo.Me2day = tbMe2day.Text.Trim();
            snsInfo.Me2dayPwd = tbMe2dayPwd.Text.Trim();
            snsInfo.Google = this.tbGoogle.Text.Trim();
            snsInfo.GooglePwd = this.tbGooglePwd.Text.Trim();
            snsInfo.NaverBlog = this.tbNaverBlog.Text.Trim();
            snsInfo.NaverBlogPwd = this.tbNaverBlogPwd.Text.Trim();

            #endregion sns

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
            FileHelper.AddUserFolder(CurrentUserInfo.CompanyID, UserID);

            #region 사진 및 도장 추가

            //사진 저장
            if (this.tbPhotoNEW.Text.Length > 0)
            {
                FileHelper.CreateSubFolder(baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Spool/" + SwfUploadGUID);
                //FileHelper.CreateSubFolder(baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Picture/" + SwfUploadGUID);

                #region 나의 SPOOL 파일을 추가한 사용자의 USERS 폴더에 복사하기

                string souceFile = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
                souceFile = string.Format(souceFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, tbPhotoNEW.Text);

                string destFile = baseFileDirectory + "{0}/Users/{1}/Picture/{2}";
                string[] fileStr = tbPhotoNEW.Text.Split('.');
                destFile = string.Format(destFile, CurrentUserInfo.CompanyID, be.UserID, SwfUploadGUID + "." + fileStr[1]);

                System.IO.File.Copy(Server.MapPath(souceFile), Server.MapPath(destFile), true);

                #endregion 나의 SPOOL 파일을 추가한 사용자의 USERS 폴더에 복사하기

                #region 섬네일 이미지

                System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(souceFile));
                //Size thumbnailSize = UtilHelper.GetThumbnailSize(image, 52);
                Size thumbnailSize = new Size(94, 124);

                System.Drawing.Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                destFile = baseFileDirectory + "{0}/Users/{1}/Picture/{2}";
                destFile = string.Format(destFile, CurrentUserInfo.CompanyID, be.UserID, "Thum_Photo_" + SwfUploadGUID + "." + fileStr[1]);

                // UC에서안해줌. UC에서 메신저사진은 무조건 JPG 여야한다 함. ...2016-03-25
                switch (fileStr[1].ToUpper())
                {
                    case "GIF":
                        thumbnail.Save(Server.MapPath(destFile), System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "JPG":
                        thumbnail.Save(Server.MapPath(destFile), System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "PNG":
                        thumbnail.Save(Server.MapPath(destFile), System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        thumbnail.Save(Server.MapPath(destFile));
                        break;
                } 

                #endregion 섬네일 이미지

                be.PhotoImage = "Thum_Photo_" + SwfUploadGUID + "." + fileStr[1];
            }

            //도장
            if (tbSealNEW.Text.Length > 0)
            {
                FileHelper.CreateSubFolder(baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Spool/" + SwfUploadGUID);
                //FileHelper.CreateSubFolder(baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Picture/" + SwfUploadGUID);

                #region SPOOL 파일을 USERS 폴더에 복사하기

                string souceFile = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
                souceFile = string.Format(souceFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, tbSealNEW.Text);

                string destFile = baseFileDirectory + "{0}/Users/{1}/Picture/{2}";
                string[] fileStr = tbSealNEW.Text.Split('.');
                destFile = string.Format(destFile, CurrentUserInfo.CompanyID, be.UserID, SwfUploadGUID + "." + fileStr[1]);

                System.IO.File.Copy(Server.MapPath(souceFile), Server.MapPath(destFile), true);

                #endregion SPOOL 파일을 USERS 폴더에 복사하기

                #region 섬네일 이미지

                System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(souceFile));
                //Size thumbnailSize = UtilHelper.GetThumbnailSize(image, 52);
                Size thumbnailSize = new Size(94, 124);

                System.Drawing.Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                destFile = baseFileDirectory + "{0}/Users/{1}/Picture/{2}";
                destFile = string.Format(destFile, CurrentUserInfo.CompanyID, be.UserID, "Thum_Seal_" + SwfUploadGUID + "." + fileStr[1]);
                thumbnail.Save(Server.MapPath(destFile));

                #endregion 섬네일 이미지

                //33버전 결재를 위해 seal.jpg 를 하나 복사한다.(2012-03-14)
                string sealDestFile = string.Format(baseFileDirectory + "{0}/Users/{1}/Picture/{2}", CurrentUserInfo.CompanyID, be.UserID, "seal.jpg");
                System.IO.File.Copy(Server.MapPath(destFile), Server.MapPath(sealDestFile), true);

                be.SealImage = "Thum_Seal_" + SwfUploadGUID + "." + fileStr[1];
            }

            #endregion 사진 및 도장 추가

            #region 메일정보 변경 (이름)

           
            string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                Zeospace.DeskPlus.Common.BLL.MailDeepFactory bizMail = new DeskPlus.Common.BLL.MailDeepFactory();
                bool rtnval = false;

                if (!bizMail.IsNonExistUser(CurrentUserInfo.CompanyID, be.MailDomain, be.MailID))
                {
                    bool isActive = (be.ActiveUser.Equals("1")) ? true : false;
                    rtnval = bizMail.ModUser(CurrentUserInfo.CompanyID, be.MailDomain, be.MailID, MailPassword, "", be.NameBase, "", "", MailDesign, isActive);
                    if (!rtnval)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Account/Admin/User/Mod.cs(이름변경 실패) : mailID(" + be.MailID + "), mailDomain(" + be.MailDomain + ")");
                    }

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Account/Admin/User/Mod.cs(Mod) : (" + be.MailID + "), mailDomain(" + be.MailDomain + "), mailpw(" + MailPassword + "), rtnval(" + rtnval.ToString() + ")", AppSettingType.MAIL);
                }
                else
                {
                    // 추가
                    rtnval = bizMail.AddUser(CurrentUserInfo.CompanyID, be.MailDomain, be.MailID, MailPassword, be.NameBase, "", "");
                    if (!rtnval)
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Account/Admin/User/Mod.cs(사용자추가 실패) : mailID(" + be.MailID + "), mailDomain(" + be.MailDomain + ")");
                    }

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Account/Admin/User/Mod.cs(Add) : (" + be.MailID + "), mailDomain(" + be.MailDomain + "), mailpw(" + MailPassword + "), rtnval(" + rtnval.ToString() + ")", AppSettingType.MAIL);
                }
            }

            #endregion 메일정보 변경 (이름)

            if (mailProduct.ToUpper() == "ZETTA65")
            {
                Zeospace.DeskPlus.Common.BLL.MailZetta65 zmail65 = new DeskPlus.Common.BLL.MailZetta65();

                if (be.ActiveUser == "1")
                {
                    zmail65.ApprovalUser(be.MailID + "@" + be.MailDomain);
                }
                else
                {
                    zmail65.SuspendUser(be.MailID + "@" + be.MailDomain);
                }
            }

            DAL.UserEnvironment dal = new DAL.UserEnvironment();
            dal.ModMyinfo(envInfo); //알람설정, 개인팝업설정

            BLL.User biz = new BLL.User();
            biz.Mod(be, snsInfo);
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 인사 삭제하기
        /// </summary>
        private void Del()
        {
            try
            {
                DAL.User dal = new DAL.User();

                #region 근태 메뉴 사용일때 마감데이터에 입,퇴사일 적용

                if (WorkYN == "Y")
                {
                    string JoinDate = DateHelper.GetCultureDate(ctbJoinDate.Text.Trim(), "yyyyMMdd", PreLangReal);
                    string RetireDate = DateHelper.GetCultureDate(ctbRetireDate.Text.Trim(), "yyyyMMdd", PreLangReal);

                    dal.ModDateBackUp(CurrentUserInfo.CompanyID, UserID, JoinDate, RetireDate);
                }

                #endregion

                //멀티 도메인 정보 가져오기
                Zeospace.DeskPlus.Account.DAL.UserMailDomain bizMail = new DAL.UserMailDomain();
                
                var mailList = bizMail.GetUserMailDomain(CurrentUserInfo.CompanyID, UserID, false);

                string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");

                if (mailProduct.ToUpper().IndexOf("ZETTA").Equals(0))
                {
                    #region 제타 메일삭제/ GW사용자삭제

                    dal.Del(CurrentUserInfo.CompanyID, UserID); //사용자 삭제 후

                    //제타 6.5 일 경우 삭제 api 호출
                    if (mailProduct.ToUpper() == "ZETTA65")
                    {
                        Zeospace.DeskPlus.Common.BLL.MailZettaFactory zettaBiz = new DeskPlus.Common.BLL.MailZettaFactory();
                        foreach (var mailInfo in mailList) zettaBiz.DelUser(mailInfo.MailID + "@" + mailInfo.MailDomain);
                    }

                    #endregion 제타 메일삭제/ GW사용자삭제
                }
                else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                    #region DEEP 메일삭제/ GW사용자삭제

                    Zeospace.DeskPlus.Common.BLL.MailDeepFactory deepBiz = new DeskPlus.Common.BLL.MailDeepFactory();
                    foreach (var mailInfo in mailList) deepBiz.DelUser(CurrentUserInfo.CompanyID, mailInfo.MailDomain, mailInfo.MailID, mailInfo.MailPassword);

                    dal.Del(CurrentUserInfo.CompanyID, UserID); //사용자 삭제 후

                    #endregion DEEP 메일삭제/ GW사용자삭제
                }
                else
                {
                    dal.Del(CurrentUserInfo.CompanyID, UserID); //사용자 삭제 후
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
        }

        #endregion Method (함수)

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

                string args = eventArgs;
                Dictionary<string, string> paraDic = new Dictionary<string, string>();

                switch (args)
                {
                    case "register":  //저장
                        Register();
                        paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                        break;

                    case "del":  //삭제
                        Del();
                        paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
                        break;
                }

                #region 비활성 사용자를 활성으로 처리하여 다시 가져와야 함.

                Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                bizMenu.ClearCacheMenuUser(CurrentUserInfo.CompanyID);

                //사용자별 메뉴 정보 - 캐쉬설정
                //bizMenu.SetCacheMenuListUser(CurrentUserInfo.CompanyID);
                //쿼리방식변경
                bizMenu.SetCacheMenuListUserAllOne(CurrentUserInfo.CompanyID);

                #endregion 비활성 사용자를 활성으로 처리하여 다시 가져와야 함.

                paraDic.Add("deptID", DeptID.ToString());
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("recordPerPage", RecordPerPage);
                paraDic.Add("searchField", SearchField);
                paraDic.Add("searchValue", SearchValue);
                paraDic.Add("searchTerm", SearchTerm);
                paraDic.Add("searchTermValue", SearchTermValue);
                paraDic.Add("order", Order);
                paraDic.Add("listPage", ListPage);

                paraDic.Add("alertMsg", GetResource("GoList"));  //"목록으로 이동합니다."
                paraDic.Add("goURL", "/DeskPlusEIP/Account/Admin/User/" + ListPage);

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
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

        #endregion Event Handler (이벤트 핸들러)

        #region Property (프로퍼티)

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        #region 사용자정보 (For.메일)

        public string OldNameBase
        {
            get { return GetViewStateString("OldNameBase", ""); }
            set { ViewState["OldNameBase"] = value; }
        }

        public string MailID
        {
            get { return GetViewStateString("MailID", ""); }
            set { ViewState["MailID"] = value; }
        }

        public string MailDomain
        {
            get { return GetViewStateString("MailDomain", ""); }
            set { ViewState["MailDomain"] = value; }
        }

        public string MailPassword
        {
            get { return GetViewStateString("MailPassword", ""); }
            set { ViewState["MailPassword"] = value; }
        }

        public string MailDesign
        {
            get { return GetViewStateString("MailDesign", ""); }
            set { ViewState["MailDesign"] = value; }
        }

        public string MailProduct
        {
            get { return GetViewStateString("MailProduct", ""); }
            set { ViewState["MailProduct"] = value; }
        }

        #endregion 사용자정보 (For.메일)

        /// <summary>
        /// 근태 사용 정보
        /// </summary>
        public string WorkYN
        {
            get { return GetViewStateString("WorkYN", ""); }
            set { ViewState["WorkYN"] = value; }
        }

        #endregion Property (프로퍼티)
    }
}