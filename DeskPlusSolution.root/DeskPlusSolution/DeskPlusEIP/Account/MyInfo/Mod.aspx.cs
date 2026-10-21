using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.MyInfo
{
    public partial class Mod : Base.BasePageAccount
    {
        #region 변수설정

        public string PhotoImage;
        public string SealImage;

        private string eventArgs = String.Empty;
        private string MultiName = String.Empty;

        //public string AlarmMethod = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "AlarmMethod", "MAIL,MESSENGER").TrimEnd(',');

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.MyInfo.Mod).Assembly;	//어셈블리 선언

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
            LoadUser();
        }

        /// <summary>
        /// 용도 : 사용자 정보 가져오기
        /// 작성자 : 심재운
        /// </summary>
        private void LoadUser()
        {
            string name = "";

            DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "SEX");
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

            //Time Zone 추가
            ddlBasicTimeZone.Items.Clear();
            foreach (var tz in TimeZoneInfo.GetSystemTimeZones())
            {
                ddlBasicTimeZone.Items.Add(new ListItem(tz.DisplayName, tz.Id));
            }

            string mailID = "";
            string mailDomain = "";

            Zeospace.DeskPlus.Account.DAL.User biz = new DAL.User();
            //관리 인사에는 비활성화 한 사용자도 보여야 한다.
            List<UserInfo> al = biz.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, false, false);
            ddlDeptInfo.Items.Clear();

            foreach (UserInfo be in al)
            {
                ctbLoginID.Text = be.LogInID;
                ctbName.Text = be.NameBase.ToReverseXSS();
                ctbMuliLang.Text = GetMuiltiLang(
                        be.NameKo.ToReverseXSS(), be.NameEn.ToReverseXSS(), be.NameJa.ToReverseXSS(), be.NameFr.ToReverseXSS(), be.NameZhCn.ToReverseXSS(), be.NameZhTw.ToReverseXSS());

                //ctbPassword.Attributes.Add("value", be.Password);
                //ctbMailPassword.Attributes.Add("value", be.MailPassword);
                ctbSysCode.Text = be.UserCode.ToNull();
                ctbEmployeeNumber.Text = be.EmployeeNumber;
                ctbResNumber.Text = be.RegistrationNumber;
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
                ctbSecurityLogIn.Text = be.SecurityLogIn;
                //ctbMailID.Text = be.MailID;
                //ddlMailDomain.SelectedValue = be.MailDomain;
                cbxActiveUser.Checked = (be.ActiveUser == "1") ? true : false;

                ddlBasicLanguage.SelectedValue = be.PreferredLanguage;

                OldNameBase = ctbName.Text;
                MailDesign = be.MailDesign;
                MailPassword = (be.MailPassword.Equals("")) ? be.Password : be.MailPassword;

                LoadPhotoSeal(be.PhotoImage, be.SealImage);

                mailID = be.MailID; //메일 아이디
                mailDomain = be.MailDomain; //메일 도메인
                ctbAuthEmail.Text = be.AuthEmail;
                cbxOTPYN.Checked = (be.OTPYN == "Y") ? true : false;

                ddlDeptInfo.Items.Add(new ListItem(GetLangName(be.DeptBase, be.DeptKo, be.DeptEn, be.DeptJa, be.DeptFr, be.DeptZhCn, be.DeptZhTw, PreLang).ToReverseXSS() + " " + GetLangName(be.TitleBase, be.TitleKo, be.TitleEn, be.TitleJa, be.TitleFr, be.TitleZhCn, be.TitleZhTw, PreLang).ToReverseXSS(), Convert.ToString(be.DeptID) + "|" + Convert.ToString(be.CodeTitleID) + "|" + Convert.ToString(be.CodeDutyID)));
                if (be.BaseDept == "1")
                {
                    ddlDeptInfo.SelectedValue = Convert.ToString(be.DeptID) + "|" + Convert.ToString(be.CodeTitleID) + "|" + Convert.ToString(be.CodeDutyID);
                }

                string tz;
                if (String.IsNullOrEmpty(be.BaseTimeZone))
                {
                    tz = TimeZoneInfo.Local.Id;
                }
                else
                {
                    tz = be.BaseTimeZone;
                }
                ddlBasicTimeZone.SelectedValue = tz;
            }

            Zeospace.DeskPlus.Account.DAL.UserEnvironment dalEnv = new DAL.UserEnvironment();
            List<BE.UserEnvironmentInfo> alEnv = dalEnv.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            foreach (UserEnvironmentInfo info in alEnv)
            {
                rbTodo.SelectedValue = (info.IsTodo.Equals("")) ? "N" : info.IsTodo;

                //cbxAlarmMail.Checked = (info.AlarmMail == "1") ? true : false;
                //cbxAlarmMessenger.Checked = (info.AlarmMessenger == "1") ? true : false;
                //cbxAlarmSMS.Checked = (info.AlarmSMS == "1") ? true : false;
            }

            Zeospace.DeskPlus.Account.DAL.UserSNS dalSNS = new DAL.UserSNS();
            List<BE.UserSNSInfo> alSNS = dalSNS.GetUserSNSInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            foreach (UserSNSInfo info in alSNS)
            {
                //패스워드는 보여주지 않는다.
                tbFacebook.Text = info.FaceBook;
                tbTwitter.Text = info.Twitter;
                tbMe2day.Text = info.Me2day;
                tbGoogle.Text = info.Google;
                tbNaverBlog.Text = info.NaverBlog;
            }

            #region 메일 정보 가져오기

            cbtnMailPassword.Visible = false;
            if (mailID.Length > 0 && mailDomain.Length > 0)
            {
                //서브 메일(멀티 메일)
                Zeospace.DeskPlus.Account.DAL.UserMailDomain dalMailDomain = new DAL.UserMailDomain();
                var mailList = dalMailDomain.GetUserMailDomain(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, false);
                this.ddlMail.Items.Clear();
                ddlMail.AppendDataBoundItems = true;
                ddlMail.Items.Add(new ListItem("-------", ""));

                foreach (var mailInfo in mailList)
                {
                    ddlMail.Items.Add(new ListItem(mailInfo.MailID + "@" + mailInfo.MailDomain, mailInfo.MailID + "@" + mailInfo.MailDomain));
                }

                ddlMail.SelectedValue = mailID + "@" + mailDomain;
                Mail = mailID + "@" + mailDomain;
                cbtnMailPassword.Visible = true;

            }

            MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "").ToUpper();
            if (MailProduct == "") cbtnMailPassword.Visible = false;

            #endregion 메일 정보 가져오기
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

            images = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Picture/" + _photo;

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

            images = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Picture/" + _seal;

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
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnList.Text = resMng.GetString("BtnClose");
                cbtnUser.Text = resMng.GetString("Admin_UserManager_tabs0");                    // "기본정보";
                cbtnExtend.Text = resMng.GetString("AddBaseInfo");                              // "기본추가정보";
                //cbtnCard.Text = resMng.GetString("Card");                                       //"카드";
                cbtnSave.Text = resMng.GetString("BtnConfirm");                    //"저장";
                //cbtnSwfPhotoAdd.Text = resMng.GetString("Register");                            //"등록";
                //cbtnSwfSealAdd.Text = resMng.GetString("Register");                             //"등록";
                //cbtnLoginCheck.Text = resMng.GetString("Admin_UserRegister_DoubleCheck");       //"중복체크";
                //cbtnMailCheck.Text = resMng.GetString("Admin_UserRegister_DoubleCheck");
                //cbtnBirthPost.Text = resMng.GetString("SearchAddress");                         //"주소검색";
                //cbtnHomePost.Text = resMng.GetString("SearchAddress");                          //"주소검색";
                //cbtnPassword.Text = resMng.GetString("Admin_UserRegister_PasswordChange");       //"패스워드변경";
                cbtnPassword.Text = resMng.GetString("PasswordChange");
                cbtnMailPassword.Text = resMng.GetString("Admin_UserRegister_MailPasswordChange");       //"메일패스워드변경";
                cbtnMobile.Text = resMng.GetString("MobileInfo");       //모바일정보
                cbtnAlarm.Text = resMng.GetString("AlarmSetting");  //알람설정
                cbtnSummary.Text = resMng.GetString("EtcInfo");      //기타정보

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
            be.UserID = CurrentUserInfo.UserID;
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
            be.AuthEmail = ctbAuthEmail.Text.Trim();

            string mail = ddlMail.SelectedValue;
            if (mail.ToNull().IndexOf('@') != -1)
            {
                be.MailID = mail.Split('@')[0];
                be.MailDomain = mail.Split('@')[1];
            }

            //be.MailPassword = Request[ctbMailPassword.UniqueID].ToString();
            be.ActiveUser = "1";
            be.WriteUserID = CurrentUserInfo.UserID;
            be.PreferredLanguage = ddlBasicLanguage.SelectedValue;
            be.BaseTimeZone = ddlBasicTimeZone.SelectedValue;
            be.OTPYN = (cbxOTPYN.Checked) ? "Y" : "N";
            #region alarm & etc

            UserEnvironmentInfo envInfo = new UserEnvironmentInfo();
            envInfo.CompanyID = be.CompanyID;
            envInfo.UserID = be.UserID;
            envInfo.IsTodo = rbTodo.SelectedValue;
            //envInfo.AlarmMail = (cbxAlarmMail.Checked == true) ? "1" : "0";
            //envInfo.AlarmMessenger = (cbxAlarmMessenger.Checked == true) ? "1" : "0";
            //envInfo.AlarmSMS = (cbxAlarmSMS.Checked == true) ? "1" : "0";

            #endregion alarm

            #region sns

            UserSNSInfo snsInfo = new UserSNSInfo();
            snsInfo.CompanyID = CurrentUserInfo.CompanyID;
            snsInfo.UserID = CurrentUserInfo.UserID;
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
            FileHelper.AddUserFolder(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

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

                //true :계정부재 , false :계정존재
                if (!bizMail.IsNonExistUser(CurrentUserInfo.CompanyID, be.MailDomain, be.MailID))
                {
                    // 계정이 존재할 경우, 수정한다.
                    if (!be.NameBase.Equals(OldNameBase))
                    {
                        rtnval = bizMail.ModUser(CurrentUserInfo.CompanyID, be.MailDomain, be.MailID, MailPassword, "", be.NameBase, "", "", MailDesign, true);
                        if (!rtnval)
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Account/MyInfo/Mod.cs(이름변경 실패) : mailID(" + be.MailID + "), mailDomain(" + be.MailDomain + ")");
                        }

                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Account/MyInfo/Mod.cs(Mod) : (" + be.MailID + "), mailDomain(" + be.MailDomain + "), mailpw(" + MailPassword + "), rtnval(" + rtnval.ToString() + ")", AppSettingType.MAIL);
                    }
                }
            }

            #endregion 메일정보 변경 (이름)

            DAL.UserEnvironment dal = new DAL.UserEnvironment();
            dal.ModMyinfo(envInfo); //알람설정, 개인팝업설정

            BLL.User biz = new BLL.User();
            biz.Mod(be, snsInfo);

            RegisterUserDept(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, ddlDeptInfo.SelectedValue);
        }

        /// <summary>        
        /// 작성목적   : 기본부서 변경하기
        /// 작성자     : maniaysw
        /// 최초작성일 : 2017-04-24
        /// </summary>
        private void RegisterUserDept(int _companyID, int _userID, string _ddlValue)
        {
            try
            {
                DAL.UserDept dalUserDept = new DAL.UserDept();
                UserDeptInfo info = new UserDeptInfo();

                info.CompanyID = _companyID;
                info.UserID = _userID;
                info.DeptID = Convert.ToInt32(_ddlValue.Split('|')[0]);
                info.CodeTitleID = Convert.ToInt32(_ddlValue.Split('|')[1]);
                info.CodeDutyID = Convert.ToInt32(_ddlValue.Split('|')[2]);
                dalUserDept.ModBaseDept(info);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("사용자 부서변경 실패: RegisterUserDept", ex);
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
                switch (args)
                {
                    case "register":  //저장
                        Register();
                        break;
                }

                string scripts = @"<script type=""text/javascript"">Mod.CallSLO();" + "<" + "/script>";
                scripts = string.Format(scripts, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.LogInID);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SLO", scripts);
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

        public string MailDesign
        {
            get { return GetViewStateString("MailDesign", ""); }
            set { ViewState["MailDesign"] = value; }
        }

        public string MailPassword
        {
            get { return GetViewStateString("MailPassword", ""); }
            set { ViewState["MailPassword"] = value; }
        }

        public string Mail
        {
            get { return GetViewStateString("Mail", ""); }
            set { ViewState["Mail"] = value; }
        }


        public string MailProduct
        {
            get { return GetViewStateString("MailProduct", ""); }
            set { ViewState["MailProduct"] = value; }
        }

        #endregion 사용자정보 (For.메일)

        #endregion Property (프로퍼티)
    }
}