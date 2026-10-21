using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Account.BE;
using System.Drawing;
using System.IO;
using System.Data;

namespace Zeospace.DeskPlus.Community.Web.Site.Main.Search.Common
{
    public partial class GetMemberInfo : Base.BasePageCommunity
    {
        #region 변수설정
        public int CommunityID = 0;
        public int ComUserID = 0;
        private int CompanyID = 0;
        public int Grade = 0;
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Community.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Community.Web.Site.Main.Search.Common.GetMemberInfo).Assembly;	//어셈블리 선언
            #endregion

            CompanyID = CurrentUserInfo.CompanyID;

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-12
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            LoadUser();

            LoadComUser();

            if (CurrentUserInfo.UserID == ComUserID)
            {
                if (Grade == 2)
                {
                    Des1.Visible = false;
                    Des2.Visible = true;
                    cbtnMod.Visible = true;
                    cbtnDrop.Visible = false;
                }
                else
                {
                    Des1.Visible = false;
                    Des2.Visible = true;
                    cbtnMod.Visible = true;
                    cbtnDrop.Visible = true;
                }
            }
            else
            {
                Des1.Visible = true;
                Des2.Visible = false;
                cbtnMod.Visible = false;
                cbtnDrop.Visible = false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-12
        /// 작성목적 : 사용자 정보
        /// </summary>
        private void LoadUser()
        {

            this.imgPhoto.ImageUrl = BaseImgPath + "/Etc/noimg.gif";
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            if (CompanyID != 0 && ComUserID != 0)
            {
                Zeospace.DeskPlus.Account.DAL.User biz = new Account.DAL.User();
                //공통인사는 비활성화도 보이며, 겸직은 안보인다.
                List<UserInfo> al = biz.GetUserInfo(CompanyID, ComUserID, false, false);

                string dept = "";
                string name = "";
                string title = "";
                string duty = "";
                string mobile = "";
                string officePhone = "";
                string officeExtend = "";
                string mail = "";     

                foreach (UserInfo info in al)
                {
                    name = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa,
                                info.NameFr, info.NameZhCn, info.NameZhTw, PreLang).ToReverseXSS();

                    title = this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa,
                                    info.TitleFr, info.TitleZhCn, info.TitleZhTw, PreLang).ToReverseXSS();

                    duty = this.GetLangName(info.DutyBase, info.DutyKo, info.DutyEn, info.DutyJa,
                                info.DutyFr, info.DutyZhCn, info.DutyZhTw, PreLang).ToReverseXSS();

                    dept = this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa,
                                    info.DeptFr, info.DeptZhCn, info.DeptZhTw, PreLang).ToReverseXSS();

                    if (info.BaseDept == "1")
                    {
                        mail = (info.MailID != "") ? info.MailID + "@" + info.MailDomain : "";
                        officePhone = info.OfficePhone;
                        officeExtend = info.ExtensionOfficePhone;

                        mobile = info.MobilePhone;

                        ltName.Text = name;
                        ltEmail.Text = mail;
                        ltDeptName.Text = dept + "(" + title + "/" + duty + ")";

                        string photoImage = baseFileDirectory + CompanyID + "/Users/" + ComUserID + "/Picture/" + info.PhotoImage;
                        if (!File.Exists(Server.MapPath(photoImage))) photoImage = BaseImgPath + "/Etc/noimg.gif";

                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", photoImage);
                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                        this.imgPhoto.ImageUrl = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                    }
                    else
                    {
                        ltDeptNameEtc.Text += dept + "(" + title + "/" + duty + ")" + ",";
                    }
                }

                ltMobile.Text = mobile;
                ltPhone.Text = officePhone;
                if (officeExtend.Length > 0) ltPhone.Text = ltPhone.Text + "(" + officeExtend + ")";

                ltDeptNameEtc.Text = ltDeptNameEtc.Text.TrimEnd(',');

                Zeospace.DeskPlus.Account.DAL.UserSNS snsBiz = new Zeospace.DeskPlus.Account.DAL.UserSNS();
                //공통인사는 비활성화도 보이며, 겸직은 안보인다.
                List<UserSNSInfo> snsAl = snsBiz.GetUserSNSInfo(CompanyID, ComUserID);

                string snsStr = String.Empty;
                foreach (UserSNSInfo info in snsAl)
                {
                    if (!String.IsNullOrEmpty(info.FaceBook)) snsStr += "&nbsp;<a href=\"#\" class=\"facebook\" title=\"facebook\" onClick=\"GetMemberInfo.OpenSNS('facebook', \'" + info.FaceBook + "\', \'" + info.FaceBookPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_facebook.gif\" alt=\"FaceBook\" /></a>";
                    if (!String.IsNullOrEmpty(info.Twitter)) snsStr += "&nbsp;<a href=\"#\" class=\"twitter\" title=\"twitter\" onClick=\"GetMemberInfo.OpenSNS('twitter', \'" + info.Twitter + "\', \'" + info.TwitterPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_twitter.gif\" alt=\"Twitter\" /></a>";
                    if (!String.IsNullOrEmpty(info.Me2day)) snsStr += "&nbsp;<a href=\"#\" class=\"me2day\" title=\"me2day\" onClick=\"GetMemberInfo.OpenSNS('me2day', \'" + info.Me2day + "\', \'" + info.Me2dayPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_metoday.gif\" alt=\"Me2day\" /></a>";
                    if (!String.IsNullOrEmpty(info.NaverBlog)) snsStr += "&nbsp;<a href=\"#\" class=\"NaverBlog\" title=\"NaverBlog\" onClick=\"GetMemberInfo.OpenSNS('NaverBlog', \'" + info.NaverBlog + "\', \'" + info.NaverBlogPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_Naver.gif\" alt=\"NaverBlog\" /></a>";
                }

                ltSNS.Text = snsStr;         

            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-12
        /// 작성목적 : 사용자 정보
        /// </summary>
        private void LoadComUser()
        {
            DAL.User dal = new DAL.User();

            DataTableReader dtr = dal.GetUserDetailInfo(CurrentUserInfo.CompanyID, CommunityID, ComUserID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ltMemo.Text = dtr["Description"].ToString().ToReverseXSS();
                        ctbMemo.Text = dtr["Description"].ToString().ToReverseXSS();
                    }
                }
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-12
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnClose.Text = resMng.GetString("Close");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnDrop.Text = resMng.GetString("DropUser");
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-06-12
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            CommunityID = GetDecryptParam("communityID", "0").ToNumber<int>();
            ComUserID = GetDecryptParam("comUserID", "0").ToNumber<int>();
            Grade = GetDecryptParam("grade", "10").ToNumber<int>();
        }


        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}