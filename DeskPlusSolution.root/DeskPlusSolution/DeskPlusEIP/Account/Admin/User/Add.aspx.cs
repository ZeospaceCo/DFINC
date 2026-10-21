using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Account.Web.Admin.User
{
    public partial class Add : Base.BasePageAccountAdmin
    {
        #region 변수설정

        public int DeptID;
        private string MultiName = String.Empty;

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Admin.User.Add).Assembly;	//어셈블리 선언

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
            GetHREnv();

            MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "").ToUpper();            
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            this.GetPagingParam();

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
                cbtnSave.Text = resMng.GetString("BtnSave");                    //"저장";
                cbtnList.Text = resMng.GetString("Cancel");                                       //"목록";
                //cbtnSwfPhotoAdd.Text = resMng.GetString("Register");                            //"등록";
                //cbtnSwfSealAdd.Text = resMng.GetString("Register");                             //"등록";
                cbtnLoginCheck.Text = resMng.GetString("Admin_UserRegister_DoubleCheck");       //"중복체크";
                cbtnMailCheck.Text = resMng.GetString("Admin_UserRegister_DoubleCheck");//"중복체크";
                //cbtnBirthPost.Text = resMng.GetString("SearchAddress");                         //"주소검색";
                //cbtnHomePost.Text = resMng.GetString("SearchAddress");                          //"주소검색";
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
        /// 작성자 : 심재운
        /// 인사 등록하기
        /// </summary>
        private int Register()
        {
            Account.BE.UserInfo be = new UserInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.UserID = 0;
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

            be.Password = Request[ctbPassword.UniqueID].ToString();
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
            be.MailPassword = ctbMailPassword.Text.Trim();
            be.ActiveUser = (cbxActiveUser.Checked) ? "1" : "0";
            be.WriteUserID = CurrentUserInfo.UserID;
            be.PreferredLanguage = ddlBasicLanguage.SelectedValue;
            be.AuthEmail = ctbAuthEmail.Text.ToString().Trim();
            be.OTPYN = (cbxOTPYN.Checked) ? "Y" : "N";

            if (tbPhotoNEW.Text.Length > 0)
            {
                string[] fileStr = tbPhotoNEW.Text.Split('.');
                be.PhotoImage = "Thum_Photo_" + SwfUploadGUID + "." + fileStr[1];
            }

            if (tbSealNEW.Text.Length > 0)
            {
                string[] fileStr = tbSealNEW.Text.Split('.');
                be.SealImage = "Thum_Seal_" + SwfUploadGUID + "." + fileStr[1];
            }

            #region sns

            UserSNSInfo snsInfo = new UserSNSInfo();
            snsInfo.CompanyID = CurrentUserInfo.CompanyID;
            snsInfo.UserID = 0;
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

            BLL.User biz = new BLL.User();
            int userID = biz.Add(be, snsInfo);

            if (userID > 0)
            {
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

                FileHelper.AddUserFolder(CurrentUserInfo.CompanyID, userID);

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
                    destFile = string.Format(destFile, CurrentUserInfo.CompanyID, userID, SwfUploadGUID + "." + fileStr[1]);

                    System.IO.File.Copy(Server.MapPath(souceFile), Server.MapPath(destFile), true);

                    #endregion 나의 SPOOL 파일을 추가한 사용자의 USERS 폴더에 복사하기

                    #region 섬네일 이미지

                    System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(souceFile));
                    //Size thumbnailSize = UtilHelper.GetThumbnailSize(image, 52);
                    Size thumbnailSize = new Size(94, 124);

                    System.Drawing.Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                    destFile = baseFileDirectory + "{0}/Users/{1}/Picture/{2}";
                    destFile = string.Format(destFile, CurrentUserInfo.CompanyID, userID, "Thum_Photo_" + SwfUploadGUID + "." + fileStr[1]);

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
                    destFile = string.Format(destFile, CurrentUserInfo.CompanyID, userID, SwfUploadGUID + "." + fileStr[1]);

                    System.IO.File.Copy(Server.MapPath(souceFile), Server.MapPath(destFile), true);

                    #endregion SPOOL 파일을 USERS 폴더에 복사하기

                    #region 섬네일 이미지

                    System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(souceFile));
                    //Size thumbnailSize = UtilHelper.GetThumbnailSize(image, 52);
                    Size thumbnailSize = new Size(94, 124);

                    System.Drawing.Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                    destFile = baseFileDirectory + "{0}/Users/{1}/Picture/{2}";
                    destFile = string.Format(destFile, CurrentUserInfo.CompanyID, userID, "Thum_Seal_" + SwfUploadGUID + "." + fileStr[1]);
                    thumbnail.Save(Server.MapPath(destFile));

                    #endregion 섬네일 이미지

                    //33버전 결재를 위해 seal.jpg 를 하나 복사한다.(2012-03-14)
                    string sealDestFile = string.Format(baseFileDirectory + "{0}/Users/{1}/Picture/{2}", CurrentUserInfo.CompanyID, userID, "seal.jpg");
                    System.IO.File.Copy(Server.MapPath(destFile), Server.MapPath(sealDestFile), true);
                }

                #endregion 사진 및 도장 추가
            }

            #region 사용자 메일계정 생성하기

            string mailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "").ToUpper();

            if (mailProduct.IndexOf("ZETTA").Equals(0))
            {
                Zeospace.DeskPlus.Common.BLL.MailZettaFactory bizMail = new DeskPlus.Common.BLL.MailZettaFactory();

                //ZETTA 메일연동
                string email = be.MailID + "@" + be.MailDomain;
                string encryptType = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "EncryptType", "");

                if (encryptType.ToUpper() == "SHA1")
                {
                    //제타메일 사용자 추가, 맨 끝 파라메터 is_encrypt 1 값
                    string mailPassword = HashManager.GetHash(be.MailPassword.ToNull().ToXSS());

                    bizMail.AddUser(be.CompanyID, email, mailPassword, Server.UrlEncode(be.NameBase), email, 1);

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("add.aspx.cs : " + email, AppSettingType.MAIL);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("add.aspx.cs : " + be.Password, AppSettingType.MAIL);
                }
            }
            else if (mailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                Zeospace.DeskPlus.Common.BLL.MailDeepFactory bizMail = new DeskPlus.Common.BLL.MailDeepFactory();

                //DEEP 메일연동
                string mailpw = HashManager.GetHash(be.MailPassword.ToNull().ToXSS());
                bool rtnval = bizMail.AddUser(be.CompanyID, be.MailDomain, be.MailID, mailpw, be.NameBase, "", "");
                if (!rtnval)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Account/Admin/User/Add.cs(메일계정추가실패) : mailID(" + be.MailID + "), mailDomain(" + be.MailDomain + ")");
                }

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("Account/Admin/User/Add.cs:mailID(" + be.MailID + "), mailDomain(" + be.MailDomain + "), mailpw(" + mailpw + "), rtnval(" + rtnval.ToString() + ")", AppSettingType.MAIL);
            }

            #endregion 사용자 메일계정 생성하기

            return userID;
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

                int userID = Register();

                if (userID > 0)
                {
                    Zeospace.DeskPlus.EIP.BLL.Menu bizMenu = new DeskPlus.EIP.BLL.Menu();
                    //사용자 접근 가능한 메뉴 정보 - 캐쉬 초기화
                    bizMenu.ClearCacheMenuUser(CurrentUserInfo.CompanyID);
                    //사용자별 메뉴 정보 - 캐쉬설정
                    bizMenu.SetCacheMenuListUser(CurrentUserInfo.CompanyID);

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("userID", userID.ToString());
                    paraDic.Add("deptID", "0");

                    paraDic.Add("currentPage", CurrentPage.ToString());
                    paraDic.Add("recordPerPage", RecordPerPage);
                    paraDic.Add("searchField", SearchField);
                    paraDic.Add("searchValue", SearchValue);
                    paraDic.Add("searchTerm", SearchTerm);
                    paraDic.Add("searchTermValue", SearchTermValue);
                    paraDic.Add("order", Order);
                    paraDic.Add("listPage", ListPage);

                    paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                    paraDic.Add("alertMsg", GetResource("GoList"));  //"목록으로 이동합니다."
                    paraDic.Add("goURL", "/DeskPlusEIP/Account/Admin/User/UserDept/List.aspx");

                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                    Utility.Redirect(encParams);
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnRegister_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
            }
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

        public string MailProduct
        {
            get { return GetViewStateString("MailProduct", ""); }
            set { ViewState["MailProduct"] = value; }
        }

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