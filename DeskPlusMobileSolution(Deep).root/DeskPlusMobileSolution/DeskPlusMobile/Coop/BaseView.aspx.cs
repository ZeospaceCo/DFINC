using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Data;
using HtmlAgilityPack;
using Zeospace.DeskPlusMobile.Web.Coop.Ws.BE;

namespace Zeospace.DeskPlusMobile.Web.Coop
{
    public partial class BaseView : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string bPage = "";
        public int pageNo = 0, moreSize = 0;
        public int listID = 0, groupID = 0;
        public string searchValue = "";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Coop.BaseView).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                Today = DateHelpers.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Sunday: Today += "(" + resMng.GetString("Sun") + ")"; break;
                    case DayOfWeek.Monday: Today += "(" + resMng.GetString("Mon") + ")"; break;
                    case DayOfWeek.Tuesday: Today += "(" + resMng.GetString("Tue") + ")"; break;
                    case DayOfWeek.Wednesday: Today += "(" + resMng.GetString("Wed") + ")"; break;
                    case DayOfWeek.Thursday: Today += "(" + resMng.GetString("Thu") + ")"; break;
                    case DayOfWeek.Friday: Today += "(" + resMng.GetString("Fri") + ")"; break;
                    case DayOfWeek.Saturday: Today += "(" + resMng.GetString("Sat") + ")"; break;
                };

                LoadParam();

                LoadData();
            }
        }


        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            bPage = GetDecryptParam("BPage", "");
            moreSize = GetDecryptParam("MoreSize", "10").ToNumber<int>();
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            listID = GetDecryptParam("ListID", "0").ToNumber<int>();
            groupID = GetDecryptParam("GroupID", "0").ToNumber<int>();          
            searchValue = GetDecryptParam("Sv", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-03-30
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadData()
        {
            string replyHtml = "";
            // 진행 내역 가져오기
            Zeospace.DeskPlusMobile.Web.Coop.Ws.Data.List dal = new Zeospace.DeskPlusMobile.Web.Coop.Ws.Data.List();

            DataSet ds = dal.GetGroupList(CompanyID, groupID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {             
                    string formContent = dr["Contents"].ToString().ToEditorXSS();
                    string detailHtml = "";

                    //By Freeman 20110721  본문의 Body 안의 내용만 추가 함
                    HtmlAgilityPack.HtmlDocument hdoc = new HtmlDocument();
                    hdoc.LoadHtml(formContent);
                    var bodyNode = hdoc.DocumentNode.SelectSingleNode("//body"); //Body 안의 TEXT 값 추출

                    if (bodyNode != null)
                    {
                        detailHtml = bodyNode.InnerHtml;
                    }
                    else
                    {
                        detailHtml = formContent;    
                    }

                    #region 이름 다국어 설정
                    string name = this.GetLangName(
                            dr["NameBase"].ToString().ToNull(), dr["NameKo"].ToString().ToNull(),
                            dr["NameEn"].ToString().ToNull(), dr["NameJa"].ToString().ToNull(),
                            dr["NameFr"].ToString().ToNull(), dr["NameZhcn"].ToString().ToNull(),
                            dr["NameZhtw"].ToString().ToNull(), PreLang);
                    #endregion 이름 다국어 설정

                    replyHtml += CreateHtmlGroup(dr["CompanyID"].ToString().ToNumber<int>(),
                                    dr["ListID"].ToString().ToNumber<int>(),
                                    dr["GroupID"].ToString().ToNumber<int>(),
                                    dr["ListOrder"].ToString().ToNumber<int>(),
                                    dr["Title"].ToString(), detailHtml, 
                                    dr["WriteUserID"].ToString().ToNumber<int>(),
                                    name,
                                    dr["StatusLast"].ToString().ToNull(),
                                    Convert.ToDateTime(dr["WriteDate"].ToString()),
                                    dr["IsAlarm"].ToString()
                                    );
                }

                if (replyHtml.Length > 0) pnContent.Controls.Add(new LiteralControl(replyHtml));
            }                
        }


        /// <summary>
        /// 그룹별 내용 출력하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_listOrder"></param>
        /// <param name="_title"></param>
        /// <param name="_contents"></param>
        /// <param name="_writeUserID"></param>
        /// <param name="_name"></param>
        /// <param name="_statusLast"></param>
        /// <param name="_writeDate"></param>
        /// <param name="_isAlarm"></param>
        /// <returns></returns>
        private string CreateHtmlGroup(int _companyID, int _listID, int _groupID, int _listOrder, string _title,
            string _contents, int _writeUserID, string _name, string _statusLast, DateTime _writeDate, string _isAlarm)
        {
            string bodyContHtml = "";
            string bodyHtml = "";
            string bodyLogHtml = "";
            string attchHtml = "";
            string baseDocHtml = "";
            string baseCoopHtml = "";

            #region 템플릿 양식

            bodyContHtml = @"
                    <table cellspacing='0' class='tab_typeCR'>
                        <colgroup>
                            <col width='60' /><col />
                        </colgroup>
                        <tbody>
                            {6}
                            {0}{1}{2}
                        </tbody>
                    </table>";

            bodyHtml = @"
                    <table cellspacing='0' class='tab_typeCR'>
                        <colgroup>
                            <col width='60' /><col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>" + resMng.GetString("WriteName") + @"</th><td>{3} ({4}) {5}</td>
                            </tr>
                            {0}{1}{2}
                            {6}
                        </tbody>
                    </table>";

            bodyLogHtml = @"
                    <table cellspacing='0' class='tab_typeCR'>
                        <colgroup>
                            <col width='60' /><col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>" + resMng.GetString("WriteName") + @"</th><td>{3} ({4}) {5}</td>
                            </tr>
                            {0}{1}{2}
                            {6}
                        </tbody>
                    </table>";

            attchHtml = @"<tr>
                        <th width='60'>" + resMng.GetString("Attach") + @"</th>
                        <td colspan='2'>
                            <table border='0' class='att_css'>
                                {0}
                            </table>
                        </td>
                    </tr>";

            baseDocHtml = @"<tr>
                    <th width='60'>" + resMng.GetString("Approval") + @"</th>
                    <td colspan='2'>
                        <table border='0' class='att_css'>
                            {0}
                        </table>
                    </td>
                </tr>";

            baseCoopHtml = @"<tr>
                    <th width='60'>" + resMng.GetString("BaseDoc") + @"</th>
                    <td colspan='2'>
                        <table border='0' class='att_css'>
                            {0}
                        </table>
                    </td>
                </tr>";

            #endregion 템플릿 양식

            #region 첨부파일

            //첨부파일
            Zeospace.DeskPlusMobile.Web.Coop.Ws.Data.Attach attDal = new Ws.Data.Attach();
            List<AttachInfo> attachInfo = attDal.GetAttach(_companyID, _listID);

            if (attachInfo.Count == 0)
            {
                attchHtml = "";
            }
            else
            {
                string attHtml = "";

                foreach (AttachInfo _attachInfo in attachInfo)
                {
                    _attachInfo.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : _attachInfo.FileName;
                    _attachInfo.FilePath = _attachInfo.FilePath.Replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/") +_attachInfo.FileGUID;                    
 
                    string temphtml = @"
                        <tr>
                            <td>
                                <a onclick=""javascript:View.goAttach('{0}','{1}')"">{1}</a>
                            </td>
                        </tr>";

                    attHtml += string.Format(temphtml, _attachInfo.FilePath, _attachInfo.FileName);
                }

                attchHtml = string.Format(attchHtml, attHtml);
            }

            #endregion 첨부파일

            #region 전자결재 근거문서
            string tempAppDocHtml = "";

            Zeospace.DeskPlusMobile.Web.Coop.Ws.Data.APPBaseDoc appDal = new Zeospace.DeskPlusMobile.Web.Coop.Ws.Data.APPBaseDoc();
            DataSet ds = appDal.GetAppBaseDoc(_companyID, _listID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string temphtml = @"<tr>
                                <td>
                                    <a onclick=""View.appDocView('{1}')"">{0}</a>
                                </td>
                            </tr>";

                    tempAppDocHtml += string.Format(temphtml, dr["Name"].ToString(), dr["DocumentID"].ToString());
                }
            }

            if (tempAppDocHtml == "")
            {
                baseDocHtml = "";
            }
            else
            {
                baseDocHtml = string.Format(baseDocHtml, tempAppDocHtml);
            }

            #endregion 전자결재 근거문서

            #region 업무협업 근거문서
            string tempBaseCoopHtml = "";

            Zeospace.DeskPlusMobile.Web.Coop.Ws.Data.BaseCoop baseCoopDal = new Zeospace.DeskPlusMobile.Web.Coop.Ws.Data.BaseCoop();
            DataSet dsCoop = baseCoopDal.GetBaseCoop(_companyID, _listID, _groupID);

            if (dsCoop != null)
            {
                foreach (DataRow dr in dsCoop.Tables[0].Rows)
                {
                    string temphtml = @"<tr>
                                <td>
                                    <a onclick=""View.CoopViewer('{1}')"">{0}</a>
                                </td>
                            </tr>";

                    tempBaseCoopHtml += string.Format(temphtml, dr["Subject"].ToString(), dr["BaseCoopID"].ToString());
                }
            }        

            if (tempBaseCoopHtml == "")
            {
                baseCoopHtml = "";
            }
            else
            {
                baseCoopHtml = string.Format(baseCoopHtml, tempBaseCoopHtml);
            }

            #endregion 업무협업 근거문서

            string btnHtml = "";

            if (_contents.Length > 0)
            {
                string tmpContent = @"
                <tr>
                    <td colspan='2' valign='top'>{0}</td>
                </tr>";

                _contents = string.Format(tmpContent, _contents);
            }
            else
            {
                _contents = "";
            }

            if (_listID == _groupID) { bodyHtml = bodyContHtml; }
            if (_isAlarm == "1") { bodyHtml = bodyLogHtml;}

            bodyHtml = string.Format(
                bodyHtml,
                attchHtml,
                baseDocHtml,
                baseCoopHtml,
                _name,
                _writeDate.ToShortDateString() + " " + _writeDate.ToShortTimeString(),
                btnHtml,
                _contents);

            return bodyHtml;
        }

        public string Today { get; set; }

    }
}