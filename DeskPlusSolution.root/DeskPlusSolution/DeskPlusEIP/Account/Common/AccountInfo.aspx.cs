using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.BE;
using System.Drawing;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Common.BLL;



namespace Zeospace.DeskPlus.Account.Web.Common
{
    public partial class AccountInfo : Base.BasePageAccount
    {
        #region Declaration (컨트롤 선언)
        private int CompanyID = 0;
        private int UserID = 0;

        public string UserEmpNO;
        #endregion



        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";		//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Common.AccountInfo).Assembly;	//어셈블리 선언
            #endregion

            #region HEADER 정보 로드            
            UserID = GetDecryptParam("userID", "0").ToNumber<int>();
            CompanyID = CurrentUserInfo.CompanyID;
            #endregion

            if (!IsPostBack)
            {                

                #region 리소스 처리
                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {
                    CbtnDetail.Text = resMng.GetString("DetailInfo");
                    CbtnClose.Text = resMng.GetString("Admin_UserRegister_Close");
                }
                #endregion

                #region 이미지 처리

                #endregion

                #region 초기화
                //문서 리스트 정보
                LoadData();
                #endregion
            }
        }
        #endregion

        #region Method (함수)
        private void LoadData()
        {
            bool isRoleAccount = CurrentUserInfo.IsInRole("account");
            if (isRoleAccount)
            {
                CbtnDetail.Visible = true;
            }

            //this.imgPhoto.ImageUrl = BaseImgPath + "/Etc/noimg.gif";
            this.imgPhoto.ImageUrl = BaseImgPath + "/Icon/ico_noimg.gif";
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            if (CompanyID != 0 && UserID != 0)
            {
                string nowDate = string.Empty;
                nowDate = System.DateTime.Now.Date.ToString();
                nowDate = DateHelper.GetCultureDate(nowDate, "yyyyMMdd", PreLangReal);
                Zeospace.DeskPlus.Account.DAL.User biz = new DAL.User();

                DataTableReader dtr = biz.GetUserShortListInfo(CompanyID, UserID, false, false, nowDate);

                string dept = "";
                string name = "";
                string title = "";
                string duty = "";
                string mobile = "";
                string officePhone = "";
                string officeExtend = "";

                string mailAddress = "";

                string mail = "";

                int todayCnt = 0;
                int shortCnt = 0;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            name = this.GetLangName(dtr["NameBase"].ToString(),
                                                    dtr["NameKo"].ToString(), 
                                                    dtr["NameEn"].ToString(), 
                                                    dtr["NameJa"].ToString(),
                                                    dtr["NameFr"].ToString(), 
                                                    dtr["NameZhCn"].ToString(), 
                                                    dtr["NameZhTw"].ToString(), PreLang);

                            title = this.GetLangName(dtr["TitleBase"].ToString(), 
                                                     dtr["TitleKo"].ToString(), 
                                                     dtr["TitleEn"].ToString(), 
                                                     dtr["TitleJa"].ToString(),
                                                     dtr["TitleFr"].ToString(), 
                                                     dtr["TitleZhCn"].ToString(), 
                                                     dtr["TitleZhTw"].ToString(), PreLang);

                            duty = this.GetLangName(dtr["DutyBase"].ToString(), 
                                                    dtr["DutyKo"].ToString(), 
                                                    dtr["DutyEn"].ToString(), 
                                                    dtr["DutyJa"].ToString(),
                                                    dtr["DutyFr"].ToString(), 
                                                    dtr["DutyZhCn"].ToString(), 
                                                    dtr["DutyZhTw"].ToString(), PreLang);

                            dept = this.GetLangName(dtr["DeptBase"].ToString(), 
                                                    dtr["DeptKo"].ToString(), 
                                                    dtr["DeptEn"].ToString(),
                                                    dtr["DeptJa"].ToString(),
                                                    dtr["DeptFr"].ToString(), 
                                                    dtr["DeptZhCn"].ToString(), 
                                                    dtr["DeptZhTw"].ToString(), PreLang);

                            if (dtr["BaseDept"].ToString() == "1")
                            {
                                mail = (dtr["MailID"].ToString() != "") ? "<a href=\"#\" title=\"WriteMail\" onClick=\"AccountInfo.SendMail()\">" + dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString() + "</a>" : "";
                                mailAddress = dtr["MailID"].ToString() + "@" + dtr["MailDomain"].ToString();
                                officePhone = dtr["OfficePhone"].ToString();
                                officeExtend = dtr["ExtensionOfficePhone"].ToString();

                                mobile = dtr["MobilePhone"].ToString();

                                ltName.Text = name;
                                ltEmail.Text = mail;
                                ltDeptName.Text = dept + "(" + title + "/" + duty + ")";

                                string photoImage = baseFileDirectory + CompanyID + "/Users/" + UserID + "/Picture/" + dtr["PhotoImage"].ToString();
                                //if (!File.Exists(Server.MapPath(photoImage))) photoImage = BaseImgPath + "/Etc/noimg.gif";
                                if (!File.Exists(Server.MapPath(photoImage))) photoImage = BaseImgPath + "/Icon/ico_noimg.gif";                                

                                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", photoImage);
                                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                                this.imgPhoto.ImageUrl = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            }
                            else
                            {
                                ltDeptNameEtc.Text += dept + "(" + title + "/" + duty + ")" + ",";
                            }

                            ltMemo.Text = dtr["Description"].ToString().ToNull();

                            todayCnt = dtr["todayCnt"].ToString().ToNumber<int>();
                            shortCnt = dtr["shortCnt"].ToString().ToNumber<int>();

                            UserEmpNO = dtr["EmployeeNumber"].ToString();
                        }

                        ltMobile.Text = mobile;
                        ltPhone.Text = officePhone;
                        if (officeExtend.Length > 0) ltPhone.Text = ltPhone.Text + "(" + officeExtend + ")";

                        ltDeptNameEtc.Text = ltDeptNameEtc.Text.TrimEnd(',');

                        Zeospace.DeskPlus.Account.DAL.UserSNS snsBiz = new DAL.UserSNS();
                        //공통인사는 비활성화도 보이며, 겸직은 안보인다.
                        List<UserSNSInfo> snsAl = snsBiz.GetUserSNSInfo(CompanyID, UserID);

                        string snsStr = String.Empty;
                        foreach (UserSNSInfo info in snsAl)
                        {
                            if (!String.IsNullOrEmpty(info.FaceBook)) snsStr += "&nbsp;<a href=\"#\" class=\"facebook\" title=\"facebook\" onClick=\"AccountInfo.OpenSNS('facebook', \'" + info.FaceBook + "\', \'" + info.FaceBookPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_facebook.gif\" alt=\"FaceBook\" /></a>";
                            if (!String.IsNullOrEmpty(info.Twitter)) snsStr += "&nbsp;<a href=\"#\" class=\"twitter\" title=\"twitter\" onClick=\"AccountInfo.OpenSNS('twitter', \'" + info.Twitter + "\', \'" + info.TwitterPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_twitter.gif\" alt=\"Twitter\" /></a>";
                            if (!String.IsNullOrEmpty(info.Me2day)) snsStr += "&nbsp;<a href=\"#\" class=\"me2day\" title=\"me2day\" onClick=\"AccountInfo.OpenSNS('me2day', \'" + info.Me2day + "\', \'" + info.Me2dayPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_metoday.gif\" alt=\"Me2day\" /></a>";
                            if (!String.IsNullOrEmpty(info.NaverBlog)) snsStr += "&nbsp;<a href=\"#\" class=\"NaverBlog\" title=\"NaverBlog\" onClick=\"AccountInfo.OpenSNS('NaverBlog', \'" + info.NaverBlog + "\', \'" + info.NaverBlogPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_Naver.gif\" alt=\"NaverBlog\" /></a>";
                        }

                        ltSNS.Text = snsStr;

                        string imgStr = String.Empty;
                        imgStr += "&nbsp;<a onClick=\"AccountInfo.PopupShortReplyList('" + UserID + "')\"><img src='" + BaseImgPath + "/Icon/icon_comment.gif' /></a>";
                        if (shortCnt > 0)
                            imgStr += "&nbsp;<a onClick=\"AccountInfo.PopupShortReplyList('" + UserID + "')\">[" + shortCnt + "]</a>";
                        if (todayCnt > 0)
                            imgStr += "&nbsp;<img src='" + BaseImgPath + "/btn/i_new.gif' />";
                        ltImg.Text = imgStr;
                    }
                }

                string MailServerIP = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "http://deep.deskplus.co.kr:8080");
                //URL_GateUrl = MailServerIP + AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "DEEP_GateUrl", "/zeospace.ds");
                URL_GateUrl = MailServerIP + "/zeospace.ds";

                string MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
                if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
                {
                    Zeospace.DeskPlus.Account.DAL.User bizMail = new Account.DAL.User();
                    List<UserInfo> al = bizMail.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, true);

                    string MailPassword = "";
                    foreach (UserInfo info in al)
                    {
                        MailPassword = (info.MailPassword.Equals("")) ? info.Password : info.MailPassword;
                    }

                    MailDeepFactory bll = new MailDeepFactory();
                    if (bll.AddUser(CurrentUserInfo.CompanyID, CurrentUserInfo.MailDomain, CurrentUserInfo.MailID, MailPassword, CurrentUserInfo.Name, "", ""))
                    {
                        MailDeep55 deep = new MailDeep55();

                        //act=sso&domain={0}&id={1}&pw={2}&language={3}&lp={4}&mbox={5}&msgkey={6}&to={7}&content={8}&hideTopMenu={9}

                        subject.Value = "";
                        act.Value = "sso";
                        domain.Value = CurrentUserInfo.MailDomain;
                        id.Value = CurrentUserInfo.MailID;
                        pw.Value = MailPassword;
                        language.Value = AppSettingHelper.GetDescription(deep.GetLang(PreLang));
                        lp.Value = AppSettingHelper.GetDescription(MailDeep55.UserLP.MailWrite);
                        mbox.Value = string.Empty;
                        msgkey.Value = string.Empty;
                        to.Value = mailAddress;
                        content.Value = "";
                        hideTopMenu.Value = "1";

                    }
                }
            }
        }
        
        #endregion

        #region Event Handler (이벤트 핸들러)

        #endregion


        #region (속성)
        public string URL_GateUrl
        {
            get { return GetViewStateString("URL_GateUrl", ""); }
            set { ViewState["URL_GateUrl"] = value; }
        }

        #endregion

        #region Property (프로퍼티)
        
        #endregion
    }
}