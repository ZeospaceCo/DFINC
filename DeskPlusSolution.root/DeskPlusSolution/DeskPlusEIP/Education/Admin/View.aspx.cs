using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Web;

namespace Zeospace.DeskPlus.Education.Web.Admin
{
    public partial class View : Base.BasePageEducationAdmin
    {
        public bool HasEducation { get; private set; }

        #region 변수설정
        private string eventArgs = String.Empty;
        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            if (!AccessAuth)
            {
                Response.StatusCode = 403;
                Response.TrySkipIisCustomErrors = true;
                form1.Visible = false;
                return;
            }

            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Education.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Education.Web.Admin.View).Assembly;	
            GetResourceManager();
            #endregion 다국어 리소스 로드

            SetLanguage();
            LoadData();
            if (HasEducation) LoadCertificates();
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            DAL.List dalList = new DAL.List();

            using (DataTableReader dtr = dalList.GetList(CurrentUserInfo.CompanyID, ListID.ToString()))
            {
                if (dtr == null || !dtr.HasRows)
                {
                    Response.StatusCode = 404;
                    Response.TrySkipIisCustomErrors = true;
                    lbTitle.Text = "조회할 수 없는 교육입니다.";
                    return;
                }

                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        HasEducation = true;
                        lbTitle.Text = HttpUtility.HtmlEncode(dtr["Title"].ToString().ToReverseXSS());
                        ltlDate.Text = DateHelper.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal) + "~" + DateHelper.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);

                        if (dtr["PopUpYN"].ToString() == "Y")
                        {
                            ltlPopUpYN.Text = "O";
                        }
                        else
                        {
                            ltlPopUpYN.Text = "X";
                        }                        

                        if (dtr["AlarmDay"].ToString() == "0")
                        {
                            ltlAlarmDay.Text = "미알람";
                        }
                        else
                        {
                            ltlAlarmDay.Text = dtr["AlarmDay"].ToString() + "일전 미완료자 재알람 발송";
                        }

                        lbContent.Text = dtr["Contents"].ToString().ToEditorXSS();         

                        DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "EducationItem");

                        for (int i = 0; i < dv.Count; i++)
                        {
                            if (dv[i]["CodeID"].ToString() == dtr["ItemID"].ToString())
                            {
                                ltlItem.Text = HttpUtility.HtmlEncode(this.GetLangName(dv[i]["NameBase"].ToString(), dv[i]["NameKo"].ToString(), dv[i]["NameEn"].ToString(), dv[i]["NameJa"].ToString(),
                                                dv[i]["NameFr"].ToString(), dv[i]["NameZhcn"].ToString(), dv[i]["NameZhtw"].ToString(), PreLang).ToReverseXSS());
                            }
                        }
                    }
                }
            }

            #region 수신자

            DAL.ListTO dalListTo = new DAL.ListTO();
            var listTo = dalListTo.GetListTo(CurrentUserInfo.CompanyID, ListID);
            bool isAllTarget = listTo.Exists(info => info != null && info.IsGroup == "A");

            if (isAllTarget)
            {
                this.ltlReceive.Text = "전체";
            }
            else
            {
                string toList = "";
                foreach (var info in listTo)
                {
                    if (info != null)
                    {
                        string tName = HttpUtility.HtmlEncode(this.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                                       info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang).ToReverseXSS());

                        if (info.IsGroup == "M")
                        {
                            string tDeptName = HttpUtility.HtmlEncode(this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn,
                                        info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang).ToReverseXSS());

                            string tTitle = HttpUtility.HtmlEncode(this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn,
                                        info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang).ToReverseXSS());

                            toList += string.Format(@"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>, ", info.UserID, tDeptName + " " + tTitle + " " + tName);
                        }
                        else
                        {
                            toList += tName + ",";
                        }
                    }

                    this.ltlReceive.Text = toList.Trim().TrimEnd(',');
                }
            }

            #endregion                  
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        private void LoadCertificates()
        {
            List<BE.TOAttachInfo> certificates = new DAL.TOAttach().GetList(CurrentUserInfo.CompanyID, ListID);
            pnlCertificates.Visible = true;
            rpCertificates.Visible = certificates.Count > 0;
            ltlCertificateEmpty.Text = certificates.Count == 0 ? "등록된 수료증이 없습니다." : "";
            rpCertificates.DataSource = certificates;
            rpCertificates.DataBind();
        }

        protected string CertificateDownloadUrl(int userID, string fileGUID)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("listID", ListID.ToString());
            parameters.Add("userID", userID.ToString());
            parameters.Add("fileGUID", fileGUID);
            return "/DeskPlusEIP/Education/Content/CertificateDownload.aspx?p="
                + HttpUtility.UrlEncode(GetEncryptParam(Utility.GetParameterDic(parameters)));
        }

        protected string CertificateUserName(BE.TOAttachInfo certificate)
        {
            string name = GetLangName(certificate.NameBase, certificate.NameKo, certificate.NameEn,
                certificate.NameJa, certificate.NameFr, certificate.NameZhcn, certificate.NameZhtw, PreLang);
            if (string.IsNullOrWhiteSpace(name)) name = certificate.UserName;
            if (string.IsNullOrWhiteSpace(name)) name = "사용자 " + certificate.UserID;
            return HttpUtility.HtmlEncode(name.ToReverseXSS());
        }

        /// <summary>
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnCancel.Text = "목록";     
            }
        }

        #endregion Method (함수)

        #region Property (프로퍼티)      

        /// <summary>
        /// Status
        /// </summary>
        public string Status
        {
            get { return GetViewStateString("Status", ""); }
            set { ViewState["Status"] = value; }
        }

        #endregion Property (프로퍼티)
      
    }
}
