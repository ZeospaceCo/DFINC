using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Helper = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Portlet.Web.Package
{
    public partial class MyInfoNoCnt : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        #region Declaration (컨트롤 선언)

        #endregion

        #region 변수설정
        public string PhotoImage = string.Empty;
        public string ApprovalStatusMenuUrl = "/DeskPlusEIP/Approval/Status/Default.aspx";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.MyInfoNoCnt).Assembly;	    //어셈블리 선언
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
            //환경설정에 추가한 내용이 있으면 해당 사항으로 정보 변경
            //타이틀, 리스트 갯수


            /*  기본적인것은 넘어오는데 추가적으러 더 필요한 사항이 있으면 사용.
            Zeospace.DeskPlus.EIP.DAL.Portlet dal = new Zeospace.DeskPlus.EIP.DAL.Portlet();
            List<Zeospace.DeskPlus.EIP.BE.PortletInfo> al = dal.GetPortlet(CurrentUserInfo.CompanyID, categoryID, portletID);


            foreach (Zeospace.DeskPlus.EIP.BE.PortletInfo info in al)
            {

            }
             * */
            LoadParam();
            SetApprovalStatusMenuUrl();

            DAL.MyInfo info = new DAL.MyInfo();

            DataTableReader dtr = info.GetMyInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.RootDeptID, 1);

            string today = System.DateTime.Now.ToShortDateString();
            string rootDeptName = string.Empty;
            string deptName = string.Empty;
            string userName = string.Empty;
            string titleName = string.Empty;
            string accessDate = "";
            string accessTime = "";

            #region 오늘 날짜 세팅
            today = today.Replace("-", ".").Replace("/", ".");
            lbToday.InnerHtml = today;
            #endregion

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        #region 루트부서 다국어 설정
                        rootDeptName = this.GetLangName(
                                    dtr["RootDeptBase"].ToString().ToNull(), dtr["RootDeptKo"].ToString().ToNull(),
                                    dtr["RootDeptEn"].ToString().ToNull(), dtr["RootDeptJa"].ToString().ToNull(),
                                    dtr["RootDeptFr"].ToString().ToNull(), dtr["RootDeptZhcn"].ToString().ToNull(),
                                    dtr["RootDeptZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);

                        #endregion

                        #region 부서 다국어 설정
                        deptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), CurrentUserInfo.PreferredLanguage);
                        #endregion

                        #region 이름 다국어 설정
                        userName = "&nbsp;" + this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 직급 다국어 설정
                        titleName = "&nbsp;" + this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        //접속시간 보여주기
                        if (dtr["AccessTime"] != null)
                        {
                            #region 최종접속일
                            accessDate = Convert.ToDateTime(dtr["AccessTime"]).ToShortDateString();
                            accessTime = Convert.ToDateTime(dtr["AccessTime"]).ToShortTimeString();
                            DateTime dt;

                            if (DateTime.TryParse(accessDate, out dt))
                                accessDate = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);

                            accessDate = accessDate.Replace("-", ".").Replace("/", ".");
                            accessTime = accessTime.Replace(resMng.GetString("AM"), "AM").Replace(resMng.GetString("PM"), "PM");
                            #endregion
                        }

                        #region 사진
                        LoadPhotoSeal(dtr["PhotoImage"].ToString());
                        #endregion

                        lblMyRootDeptInfo.InnerHtml = rootDeptName;
                        lblMyDeptInfo.InnerHtml = deptName + userName + titleName;

                        lblAccessDate.InnerHtml = GetResource("LastAccessDate") + " " + accessDate + " " + accessTime;
                    }
                }
            }

            Zeospace.DeskPlus.Account.DAL.UserSNS snsBiz = new Zeospace.DeskPlus.Account.DAL.UserSNS();
            //공통인사는 비활성화도 보이며, 겸직은 안보인다.
            List<Zeospace.DeskPlus.Account.BE.UserSNSInfo> snsAl = snsBiz.GetUserSNSInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            string snsStr = String.Empty;
            foreach (UserSNSInfo snsinfo in snsAl)
            {
                if (!String.IsNullOrEmpty(snsinfo.FaceBook)) snsStr += "&nbsp;<a href=\"#\" class=\"facebook\" title=\"facebook\" onClick=\"MyInfo_OpenSNS('facebook', \'" + snsinfo.FaceBook + "\', \'" + snsinfo.FaceBookPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_facebook.gif\" alt=\"FaceBook\" /></a>";
                if (!String.IsNullOrEmpty(snsinfo.Twitter)) snsStr += "&nbsp;<a href=\"#\" class=\"twitter\" title=\"twitter\" onClick=\"MyInfo_OpenSNS('twitter', \'" + snsinfo.Twitter + "\', \'" + snsinfo.TwitterPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_twitter.gif\" alt=\"Twitter\" /></a>";
                //if (!String.IsNullOrEmpty(snsinfo.Me2day)) snsStr += "&nbsp;<a href=\"#\" class=\"me2day\" title=\"me2day\" onClick=\"MyInfo_OpenSNS('me2day', \'" + snsinfo.Me2day + "\', \'" + snsinfo.Me2dayPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_metoday.gif\" alt=\"Me2day\" /></a>";
                if (!String.IsNullOrEmpty(snsinfo.NaverBlog)) snsStr += "&nbsp;<a href=\"#\" class=\"NaverBlog\" title=\"NaverBlog\" onClick=\"MyInfo_OpenSNS('NaverBlog', \'" + snsinfo.NaverBlog + "\', \'" + snsinfo.NaverBlogPwd + "\')\"><img src=\"" + BaseImgPath + "/Icon/sns_Naver.gif\" alt=\"NaverBlog\" /></a>";
            }

            ltSNS.Text = snsStr;

            GetMyUserDeptInfo();
        }

        /// <summary>
        /// 개인별 겸직 정보 가져오기
        /// 심재운
        /// </summary>
        private void GetMyUserDeptInfo()
        {
            Zeospace.DeskPlus.Account.DAL.UserCompany userComBiz = new Account.DAL.UserCompany();
            List<Zeospace.DeskPlus.Account.BE.UserCompanyInfo> userComList = userComBiz.GetUserCompany(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            string groupGUID = "";
            if (userComList.Count > 0) groupGUID = userComList[0].GroupGUID;

            Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();

            int cnt = 0;
            string nameText = String.Empty;
            string nameValue = String.Empty;

            ddlChangeDept.Items.Clear();
            ddlChangeDept.AppendDataBoundItems = true;
            ddlChangeDept.Items.Add(new ListItem("---------", ""));

            //타회사정보가 존재할 경우
            if (groupGUID.Length > 0)
            {
                userComList = null;
                userComList = userComBiz.GetUserCompanyByGUID(groupGUID);
                //타회사 겸직 정보가 존재할 경우
                if (userComList.Count > 0)
                {
                    foreach (var comInfo in userComList)
                    {
                        List<Zeospace.DeskPlus.Account.BE.UserInfo> alUser = dal.GetUserInfo(comInfo.CompanyID, comInfo.UserID, true, false);

                        foreach (var info in alUser)
                        {
                            nameText = string.Format(@"{0} {1} {2}",
                                this.GetLangName(info.ComBase, info.ComKo, info.ComEn, info.ComJa, info.ComFr, info.ComZhcn, info.ComZhtw, PreLang).ToReverseXSS(),
                                this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhCn, info.DeptZhTw, PreLang).ToReverseXSS(),
                                this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhCn, info.NameZhTw, PreLang).ToReverseXSS());

                            nameValue = string.Format(@"{0},{1},{2},{3},{4},{5}",
                                info.CompanyID, info.UserID, info.DeptID, info.CodeTitleID, info.CodeDutyID, info.LogInID);

                            ddlChangeDept.Items.Add(new ListItem(nameText, nameValue));
                        }

                        cnt += alUser.Count;
                    }
                }
            }
            else
            {
                //타회사 겸직 정보가 없다면, 쿠키정보의 인사정보로 처리한다.
                List<Zeospace.DeskPlus.Account.BE.UserInfo> alUser = dal.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, true, false);

                foreach (var info in alUser)
                {
                    nameText = string.Format(@"{0} {1} {2}",
                                this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhCn, info.DeptZhTw, PreLang).ToReverseXSS(),
                                this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhCn, info.TitleZhTw, PreLang).ToReverseXSS(),
                                this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhCn, info.NameZhTw, PreLang).ToReverseXSS());

                    nameValue = string.Format(@"{0},{1},{2},{3},{4},{5}",
                        info.CompanyID, info.UserID, info.DeptID, info.CodeTitleID, info.CodeDutyID, info.LogInID);

                    ddlChangeDept.Items.Add(new ListItem(nameText, nameValue));
                }

                cnt += alUser.Count;
            }

            if (cnt == 0) ddlChangeDept.Visible = false;
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 언어 설정하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : Loads the param.
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        private void LoadParam()
        {
            
        }

        private void SetApprovalStatusMenuUrl()
        {
            Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Zeospace.DeskPlus.Account.DAL.UserEnvironment();
            List<Zeospace.DeskPlus.Account.BE.UserEnvironmentInfo> list = dal.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            if (list != null && list.Count > 0 && "NEW".Equals(list[0].ApprovalStatusMenuType))
            {
                ApprovalStatusMenuUrl = "/DeskPlusEIP/Approval/Status/DefaultAll.aspx";
            }
        }

        #endregion

        #region (함수)

        /// <summary>
        /// 작성자 : 심재운
        /// 사진 및 도장 이미지 가져오기
        /// </summary>
        /// <param name="_photo">The _photo.</param>
        private void LoadPhotoSeal(string _photo)
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

            #region 이미지 경로 암호화
            string images = this.BaseImgPath + "/Icon/ico_noimg.gif";
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", images);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
            #endregion

            PhotoImage = "<img src='" + images + "' width='70' height='70' alt='' title='' />";

            if (System.IO.File.Exists(Server.MapPath(baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Picture/" + _photo)))
            {
                #region 이미지 경로 암호화
                images = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Picture/" + _photo;
                paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", images);
                encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                #endregion

                PhotoImage = "<img class='callContextMenu' src='" + images + "' width='70' height='70' alt='photo' title='photo' />";
            }
        }
        #endregion

        #region (프로퍼티)

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}
