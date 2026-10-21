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
using System.Text;


namespace Zeospace.DeskPlus.Portlet.Web.Package.UserSearch
{
    public partial class AccountInfo : Base.BasePagePortlet
    {
        #region Declaration (컨트롤 선언)

        private string UserName = "";   //포틀릿에서 검색한 이름
        public int DeptID = 0;  //선택한 부서id
        public string DeptUserInfo = "";  //출력테이블
        public int Gubun = 0; //내부검색인지 체크
        public string SelectDeptName = "";

        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.UserSearch.AccountInfo).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();

                // 상세정보 보기
                LoadData(DeptID, UserName, Gubun);
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 상세정보 보기
        /// </summary>
        /// <param name="_deptID"></param>
        /// <param name="_userName"></param>
        /// <param name="_gubun"></param>
        private void LoadData(int _deptID, string _userName, int _gubun)
        {
            // 검색어 입력
            ctbName.Attributes.Add("placeholder", resMng.GetString("PhName"));

            if (_userName != "")
            {
                ctbName.Text = _userName;
            }

            // 부서 검색시
            if (SelectDeptName != "")
            {
                SelectDeptName = "* [" + SelectDeptName + "] " + resMng.GetString("SearchIn"); 
            }
           
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();
            DataTableReader list = dal.GetUserSearchList(CurrentUserInfo.CompanyID, Convert.ToString(_deptID), _userName); 

            string dept = "";
            string name = "";
            string title = "";
            string duty = "";

            StringBuilder sb = new StringBuilder();

            if (_gubun == 1) //검색에 검색이면
            {
                if (_deptID.ToString() != "") //부서 선택했을때
                {

                    #region 부서검색을 했었다면 부서에서 2차검색                 

                    if (list != null)
                    {
                        if (list.HasRows)
                        {
                            while (list.Read())
                            {

                                name = this.GetLangName(list["NameBase"].ToString(),
                                                                   list["NameKo"].ToString(),
                                                                   list["NameEn"].ToString(),
                                                                   list["NameJa"].ToString(),
                                                                   list["NameFr"].ToString(),
                                                                   list["NameZhCn"].ToString(),
                                                                   list["NameZhTw"].ToString(), PreLang);


                                title = this.GetLangName(list["TitleBase"].ToString(),
                                                                         list["TitleKo"].ToString(),
                                                                         list["TitleEn"].ToString(),
                                                                         list["TitleJa"].ToString(),
                                                                         list["TitleFr"].ToString(),
                                                                         list["TitleZhCn"].ToString(),
                                                                         list["TitleZhTw"].ToString(), PreLang);

                                    duty = this.GetLangName(list["DutyBase"].ToString(),
                                                            list["DutyKo"].ToString(),
                                                            list["DutyEn"].ToString(),
                                                            list["DutyJa"].ToString(),
                                                            list["DutyFr"].ToString(),
                                                            list["DutyZhCn"].ToString(),
                                                            list["DutyZhTw"].ToString(), PreLang);

                                    dept = this.GetLangName(list["DeptBase"].ToString(),
                                                            list["DeptKo"].ToString(),
                                                            list["DeptEn"].ToString(),
                                                            list["DeptJa"].ToString(),
                                                            list["DeptFr"].ToString(),
                                                            list["DeptZhCn"].ToString(),
                                                            list["DeptZhTw"].ToString(), PreLang);


                                    string photoImage = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + list["UserID"].ToString() + "/Picture/" + list["PhotoImage"].ToString();
                                    if (!File.Exists(Server.MapPath(photoImage)))
                                    {
                                        photoImage = (list["Sex"].ToString().Equals("2")) ? BaseImgPath + "/Etc/noimg_woman.gif" : BaseImgPath + "/Etc/noimg_man.gif";
                                    }
                                    else
                                    {
                                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                        paraDic.Add("filePath", photoImage);
                                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                                        photoImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                    }

                                    sb.Append("<table class='table_typeB'>");
                                    sb.Append("<caption>");
                                    sb.Append("</caption>");
                                    sb.Append("<colgroup>");
                                    sb.Append("<col width='80px'/>");
                                    sb.Append("<col/>");
                                    sb.Append("<col width='110px'/>");
                                    sb.Append("</colgroup>");
                                    sb.Append("<tbody>");
                                    sb.Append("<tr>");
                                    sb.Append("<th>" + resMng.GetString("UserName") + "</th>");
                                    sb.Append("<td>&nbsp;" + name + "</td>");
                                    sb.Append("<td rowspan='5' class='last-child2' style='padding-top:9px;'><img src='" + photoImage + "' width='94px' height='124px'></img></td>");
                                    sb.Append("</tr>");
                                    sb.Append("<tr>");
                                    sb.Append("<th>" + resMng.GetString("DeptName") + "</th>");
                                    sb.Append("<td>&nbsp;" + dept + "</td>");
                                    sb.Append("</tr>");
                                    sb.Append("<th>" + resMng.GetString("TitleName") + "</th>");
                                    sb.Append("<td>&nbsp;" + title + "</td>");
                                    sb.Append("</tr>");
                                    sb.Append("<tr>");
                                    sb.Append("<th>" + resMng.GetString("CellerPhone") + "</th>");
                                    sb.Append("<td>&nbsp;" + list["MobilePhone"].ToString() + "</td>");
                                    sb.Append("</tr>");
                                    sb.Append("<tr>");
                                    sb.Append("<th>" + resMng.GetString("CompanyTelephone") + "</th>");
                                    sb.Append("<td>&nbsp;" + list["OfficePhone"].ToString() + "</td>");
                                    sb.Append("</tr>");
                                    sb.Append("<tr>");
                                    sb.Append("<th>" + resMng.GetString("Email") + "</th>");
                                    if (list["Mail"].ToString() != "")
                                    {
                                        sb.Append("<td>&nbsp;<a href=\"#\" title=\"WriteMail\" onClick=\"AccountInfo.SendMail('" + list["Mail"].ToString() + "')\">" + list["Mail"].ToString() + "</a></td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>&nbsp;</td>");
                                    }
                                    sb.Append("<td></td>");
                                    sb.Append("</tr>");
                                    sb.Append("<tr>");
                                    sb.Append("<th class='last-child'>" + resMng.GetString("Business") + "</th>");
                                    sb.Append("<td class='last-child' colspan='2'>");
                                    sb.Append("<div class='layout_scroll'  style='min-height:40px;height:40px;'>");
                                    sb.Append("" + list["Description"].ToString() + "");
                                    sb.Append("</div>");
                                    sb.Append("</td>");
                                    sb.Append("</tr>");
                                    sb.Append("</tbody>");
                                    sb.Append("</table>");
                                    sb.Append("<div class='dotLine'></div>");
                                
                            }
                        }
                    }
                    
                    #endregion
                    
                }
                else //부서선택 안하고 이름 검색시 
                {
                    #region 포틀릿에서 이름값이 있을시
                    if (UserName.ToString() != "") //부서 선택안하고 이름만 검색시
                    {
                        if (list != null)
                        {
                            if (list.HasRows)
                            {
                                while (list.Read())
                                {
                                    name = this.GetLangName(list["NameBase"].ToString(),
                                                                       list["NameKo"].ToString(),
                                                                       list["NameEn"].ToString(),
                                                                       list["NameJa"].ToString(),
                                                                       list["NameFr"].ToString(),
                                                                       list["NameZhCn"].ToString(),
                                                                       list["NameZhTw"].ToString(), PreLang);


                                    title = this.GetLangName(list["TitleBase"].ToString(),
                                                                             list["TitleKo"].ToString(),
                                                                             list["TitleEn"].ToString(),
                                                                             list["TitleJa"].ToString(),
                                                                             list["TitleFr"].ToString(),
                                                                             list["TitleZhCn"].ToString(),
                                                                             list["TitleZhTw"].ToString(), PreLang);

                                    duty = this.GetLangName(list["DutyBase"].ToString(),
                                                            list["DutyKo"].ToString(),
                                                            list["DutyEn"].ToString(),
                                                            list["DutyJa"].ToString(),
                                                            list["DutyFr"].ToString(),
                                                            list["DutyZhCn"].ToString(),
                                                            list["DutyZhTw"].ToString(), PreLang);

                                    dept = this.GetLangName(list["DeptBase"].ToString(),
                                                            list["DeptKo"].ToString(),
                                                            list["DeptEn"].ToString(),
                                                            list["DeptJa"].ToString(),
                                                            list["DeptFr"].ToString(),
                                                            list["DeptZhCn"].ToString(),
                                                            list["DeptZhTw"].ToString(), PreLang);



                                    string photoImage = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + list["UserID"].ToString() + "/Picture/" + list["PhotoImage"].ToString();
                                    if (!File.Exists(Server.MapPath(photoImage)))
                                    {
                                        photoImage = (list["Sex"].ToString().Equals("2")) ? BaseImgPath + "/Etc/noimg_woman.gif" : BaseImgPath + "/Etc/noimg_man.gif";
                                    }
                                    else
                                    {
                                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                        paraDic.Add("filePath", photoImage);
                                        string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                                        photoImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                    }

                                    sb.Append("<table class='table_typeB'>");
                                    sb.Append("<caption>");
                                    sb.Append("</caption>");
                                    sb.Append("<colgroup>");
                                    sb.Append("<col width='80px'/>");
                                    sb.Append("<col/>");
                                    sb.Append("<col width='110px'/>");
                                    sb.Append("</colgroup>");
                                    sb.Append("<tbody>");
                                    sb.Append("<tr>");
                                    sb.Append("<th>" + resMng.GetString("UserName") + "</th>");
                                    sb.Append("<td>&nbsp;" + name + "</td>");
                                    sb.Append("<td rowspan='5' class='last-child2' style='padding-top:9px;'><img src='" + photoImage + "' width='94px' height='124px'></img></td>");
                                    sb.Append("</tr>");
                                    sb.Append("<tr>");
                                    sb.Append("<th>" + resMng.GetString("DeptName") + "</th>");
                                    sb.Append("<td>&nbsp;" + dept + "</td>");
                                    sb.Append("</tr>");
                                    sb.Append("<th>" + resMng.GetString("TitleName") + "</th>");
                                    sb.Append("<td>&nbsp;" + title + "</td>");
                                    sb.Append("</tr>");
                                    sb.Append("<tr>");
                                    sb.Append("<th>" + resMng.GetString("CellerPhone") + "</th>");
                                    sb.Append("<td>&nbsp;" + list["MobilePhone"].ToString() + "</td>");
                                    sb.Append("</tr>");
                                    sb.Append("<tr>");
                                    sb.Append("<th>" + resMng.GetString("CompanyTelephone") + "</th>");
                                    sb.Append("<td>&nbsp;" + list["OfficePhone"].ToString() + "</td>");
                                    sb.Append("</tr>");
                                    sb.Append("<tr>");
                                    sb.Append("<th>" + resMng.GetString("Email") + "</th>");
                                    if (list["Mail"].ToString() != "")
                                    {
                                        sb.Append("<td>&nbsp;<a href=\"#\" title=\"WriteMail\" onClick=\"AccountInfo.SendMail('" + list["Mail"].ToString() + "')\">" + list["Mail"].ToString() + "</a></td>");
                                    }
                                    else
                                    {
                                        sb.Append("<td>&nbsp;</td>");
                                    }
                                    sb.Append("<td></td>");
                                    sb.Append("</tr>");
                                    sb.Append("<tr>");
                                    sb.Append("<th class='last-child'>" + resMng.GetString("Business") + "</th>");
                                    sb.Append("<td class='last-child' colspan='2'>");
                                    sb.Append("<div class='layout_scroll'  style='min-height:40px;height:40px;'>");
                                    sb.Append("" + list["Description"].ToString() + "");
                                    sb.Append("</div>");
                                    sb.Append("</td>");
                                    sb.Append("</tr>");
                                    sb.Append("</tbody>");
                                    sb.Append("</table>");
                                    sb.Append("<div class='dotLine'></div>");

                                }
                            }
                        }
                    }
                    #endregion
                }

            }
            else //최초 포틀릿에서 검색시
            {
                if (list != null)
                {
                    if (list.HasRows)
                    {
                        while (list.Read())
                        {

                            name = this.GetLangName(list["NameBase"].ToString(),
                                                                       list["NameKo"].ToString(),
                                                                       list["NameEn"].ToString(),
                                                                       list["NameJa"].ToString(),
                                                                       list["NameFr"].ToString(),
                                                                       list["NameZhCn"].ToString(),
                                                                       list["NameZhTw"].ToString(), PreLang);


                            title = this.GetLangName(list["TitleBase"].ToString(),
                                                                     list["TitleKo"].ToString(),
                                                                     list["TitleEn"].ToString(),
                                                                     list["TitleJa"].ToString(),
                                                                     list["TitleFr"].ToString(),
                                                                     list["TitleZhCn"].ToString(),
                                                                     list["TitleZhTw"].ToString(), PreLang);

                            duty = this.GetLangName(list["DutyBase"].ToString(),
                                                    list["DutyKo"].ToString(),
                                                    list["DutyEn"].ToString(),
                                                    list["DutyJa"].ToString(),
                                                    list["DutyFr"].ToString(),
                                                    list["DutyZhCn"].ToString(),
                                                    list["DutyZhTw"].ToString(), PreLang);

                            dept = this.GetLangName(list["DeptBase"].ToString(),
                                                    list["DeptKo"].ToString(),
                                                    list["DeptEn"].ToString(),
                                                    list["DeptJa"].ToString(),
                                                    list["DeptFr"].ToString(),
                                                    list["DeptZhCn"].ToString(),
                                                    list["DeptZhTw"].ToString(), PreLang);


                            string photoImage = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + list["UserID"].ToString() + "/Picture/" + list["PhotoImage"].ToString();
                            if (!File.Exists(Server.MapPath(photoImage)))
                            {
                                photoImage = (list["Sex"].ToString().Equals("2")) ? BaseImgPath + "/Etc/noimg_woman.gif" : BaseImgPath + "/Etc/noimg_man.gif";
                            }
                            else
                            {
                                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", photoImage);
                                string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                                photoImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            }

                            sb.Append("<table class='table_typeB'>");
                            sb.Append("<caption>");
                            sb.Append("</caption>");
                            sb.Append("<colgroup>");
                            sb.Append("<col width='80px'/>");
                            sb.Append("<col/>");
                            sb.Append("<col width='110px'/>");
                            sb.Append("</colgroup>");
                            sb.Append("<tbody>");
                            sb.Append("<tr>");
                            sb.Append("<th>" + resMng.GetString("UserName") + "</th>");
                            sb.Append("<td>&nbsp;" + name + "</td>");
                            sb.Append("<td rowspan='5' class='last-child2' style='padding-top:9px;'><img src='" + photoImage + "' width='94px' height='124px'></img></td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<th>" + resMng.GetString("DeptName") + "</th>");
                            sb.Append("<td>&nbsp;" + dept + "</td>");
                            sb.Append("</tr>");
                            sb.Append("<th>" + resMng.GetString("TitleName") + "</th>");
                            sb.Append("<td>&nbsp;" + title + "</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<th>" + resMng.GetString("CellerPhone") + "</th>");
                            sb.Append("<td>&nbsp;" + list["MobilePhone"].ToString() + "</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<th>" + resMng.GetString("CompanyTelephone") + "</th>");
                            sb.Append("<td>&nbsp;" + list["OfficePhone"].ToString() + "</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<th>" + resMng.GetString("Email") + "</th>");
                            if (list["Mail"].ToString() != "")
                            {
                                sb.Append("<td>&nbsp;<a href=\"#\" title=\"WriteMail\" onClick=\"AccountInfo.SendMail('" + list["Mail"].ToString() + "')\">" + list["Mail"].ToString() + "</a></td>");
                            }
                            else
                            {
                                sb.Append("<td>&nbsp;</td>");
                            }
                            sb.Append("<td></td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<th class='last-child'>" + resMng.GetString("Business") + "</th>");
                            sb.Append("<td class='last-child' colspan='2'>");
                            sb.Append("<div class='layout_scroll'  style='min-height:40px;height:40px;'>");
                            sb.Append("" + list["Description"].ToString() + "");
                            sb.Append("</div>");
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("</tbody>");
                            sb.Append("</table>");
                            sb.Append("<div class='dotLine'></div>");

                        }
                    }
                }

            }

            if (sb.ToString() != "")
            {
                DeptUserInfo = sb.ToString();
            }
            else
            {
                DeptUserInfo = resMng.GetString("NoPeople");
            }                

            // 메일쓰기
            SetMail();
        }

        /// <summary>
        ///  메일 쓰기 
        /// </summary>
        private void SetMail()
        {
            string MailProduct = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailProduct", "");
            if (MailProduct.ToUpper().IndexOf("DEEP").Equals(0))
            {
                string MailServerIP = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerIP", "http://deep.deskplus.co.kr:8080");
                URL_GateUrl = MailServerIP + "/zeospace.ds";

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

                    subject.Value = "";
                    act.Value = "sso";
                    domain.Value = CurrentUserInfo.MailDomain;
                    id.Value = CurrentUserInfo.MailID;
                    pw.Value = MailPassword;
                    language.Value = AppSettingHelper.GetDescription(deep.GetLang(PreLang));
                    lp.Value = AppSettingHelper.GetDescription(MailDeep55.UserLP.MailWrite);
                    mbox.Value = string.Empty;
                    msgkey.Value = string.Empty;
                    //to.Value = mailAddress;
                    content.Value = "";
                    hideTopMenu.Value = "1";

                }
            }
        }


        #endregion 

        #region 함수

        /// <summary>
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                CbtnClose.Text = resMng.GetString("Close");
                cbtnSearch.Text = resMng.GetString("Search");
            }
        }

        /// <summary>
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            UserName = GetDecryptParam("userName", "").ToString().Replace("zeo$", "&");
            DeptID = GetDecryptParam("deptID", "0").ToString().ToNumber<int>();
            Gubun = GetDecryptParam("gubun", "0").ToString().ToNumber<int>();
            SelectDeptName = GetDecryptParam("deptName", "").ToString().Replace("zeo$", "&");
        }

        #endregion 

        #region (프로퍼티)

        public string URL_GateUrl
        {
            get { return GetViewStateString("URL_GateUrl", ""); }
            set { ViewState["URL_GateUrl"] = value; }
        }

        #endregion

        #region (이벤트핸들러)

        //결과 내 재검색
        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.UserSearch.AccountInfo).Assembly;	//어셈블리 선언
            #endregion

            SetLanguage();
            LoadParam();

            LoadData(DeptID, ctbName.Text.Trim(), 1);
        }

        #endregion 
    }
}