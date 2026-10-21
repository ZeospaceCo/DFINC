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
using Zeospace.DeskPlusMobile.Web.Task.Ws.BE;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlusMobile.Web.Task
{
    public partial class BaseView : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string bPage = "";
        public int pageNo = 1;
        public int listID = 0, groupID = 0;
        public string status = "";
        public string searchValue = "";
        public string alarm;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Task.BaseView).Assembly;	//어셈블리 선언
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
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            listID = GetDecryptParam("ListID", "0").ToNumber<int>();
            groupID = GetDecryptParam("GroupID", "0").ToNumber<int>();
            searchValue = GetDecryptParam("Sv", "");
            alarm = GetDecryptParam("Alarm", "");
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-05-13
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
            Zeospace.DeskPlusMobile.Web.Task.Ws.Data.List dal = new Zeospace.DeskPlusMobile.Web.Task.Ws.Data.List();
            DataSet ds = dal.GetGroupList(CompanyID, groupID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    status = dr["StatusLast"].ToString();

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

                    detailHtml = Regex.Replace(detailHtml, "<()*style([^>])*?>", "<style>", RegexOptions.IgnoreCase);
                    detailHtml = Regex.Replace(detailHtml, "(<()*(/)()*?style()*>)", "</style>", RegexOptions.IgnoreCase);
                    detailHtml = Regex.Replace(detailHtml, @"(<style>)[\s\S]*?(</style>)", "", RegexOptions.IgnoreCase);

                    // 글쓴이
                    string tDeptName = this.GetLangName(dr["DeptBase"].ToString(), dr["DeptKo"].ToString(), dr["DeptEn"].ToString(),
                       dr["DeptJa"].ToString(), dr["DeptFr"].ToString(), dr["DeptZhcn"].ToString(),
                       dr["DeptZhtw"].ToString(), PreLang).ToReverseXSS();

                    string tTitle = this.GetLangName(dr["TitleBase"].ToString(), dr["TitleKo"].ToString(), dr["TitleEn"].ToString(),
                             dr["TitleJa"].ToString(), dr["TitleFr"].ToString(), dr["TitleZhcn"].ToString(),
                             dr["TitleZhtw"].ToString(), PreLang).ToReverseXSS();

                    string tName = this.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                               dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(),
                               dr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                    string name = tDeptName + " " + tTitle + " " + tName;

                    replyHtml += CreateHtmlGroup(dr["CompanyID"].ToString().ToNumber<int>(),
                                    dr["ListID"].ToString().ToNumber<int>(),
                                    dr["GroupID"].ToString().ToNumber<int>(),
                                    dr["ListOrder"].ToString().ToNumber<int>(),
                                    dr["Title"].ToString(), detailHtml,
                                    dr["WriteUserID"].ToString().ToNumber<int>(),
                                    name,
                                    dr["StatusLast"].ToString().ToNull(),
                                    Convert.ToDateTime(dr["WriteDate"].ToString()),
                                    dr["IsMsg"].ToString(),
                                    dr["IsType"].ToString()
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
        /// <param name="_isMsg"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        private string CreateHtmlGroup(int _companyID, int _listID, int _groupID, int _listOrder, string _title,
            string _contents, int _writeUserID, string _name, string _statusLast, DateTime _writeDate, string _isMsg, string _isType)
        {
            string bodyContHtml = "";
            string bodyHtml = "";
            string bodyLogHtml = "";
            string attchHtml = "";
            string baseAppHtml = "";
            string baseTaskHtml = "";
            string writeGubun = "";

            #region 템플릿 양식

            bodyContHtml = @"
                    <table cellspacing='0' class='tab_typeCR'>
                        <colgroup>
                            <col width='60' /><col />
                        </colgroup>
                        <tbody>                            
                            {0}{1}{2}
                            {5}
                        </tbody>
                    </table>";

            bodyHtml = @"
                    <table cellspacing='0' class='tab_typeCR'>
                        <colgroup>
                            <col width='60' /><col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>{6}</th><td>{3} ({4})</td>
                            </tr>
                            {0}{1}{2}
                            {5}
                        </tbody>
                    </table>";

            bodyLogHtml = @"
                    <table cellspacing='0' class='tab_typeCR'>
                        <colgroup>
                            <col width='60' /><col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>{6}</th><td>{3} ({4})</td>
                            </tr>
                            {0}{1}{2}
                            {5}
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

            baseAppHtml = @"<tr>
                    <th width='60'>" + resMng.GetString("BaseApp") + @"</th>
                    <td colspan='2'>
                        <table border='0' class='att_css'>
                            {0}
                        </table>
                    </td>
                </tr>";

            baseTaskHtml = @"<tr>
                    <th width='60'>" + resMng.GetString("BaseTask") + @"</th>
                    <td colspan='2'>
                        <table border='0' class='att_css'>
                            {0}
                        </table>
                    </td>
                </tr>";

            #endregion 템플릿 양식

            #region 첨부파일

            //첨부파일
            Zeospace.DeskPlusMobile.Web.Task.Ws.Data.Attach attDal = new Ws.Data.Attach();
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
                    _attachInfo.FilePath = _attachInfo.FilePath.Replace("/DeskPlusEIP/FileData/", "/DeskPlusMobile/FileData/") + _attachInfo.FileGUID;

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
            string tempBaseAppHtml = "";

            Zeospace.DeskPlusMobile.Web.Task.Ws.Data.BaseApp appDal = new Zeospace.DeskPlusMobile.Web.Task.Ws.Data.BaseApp();
            DataSet ds = appDal.GetBaseApp(_companyID, _listID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string temphtml = @"<tr>
                                <td>
                                    <a onclick=""View.AppView('{1}')"">{0}</a>
                                </td>
                            </tr>";

                    tempBaseAppHtml += string.Format(temphtml, dr["Name"].ToString(), dr["DocumentID"].ToString());
                }
            }

            if (tempBaseAppHtml == "")
            {
                baseAppHtml = "";
            }
            else
            {
                baseAppHtml = string.Format(baseAppHtml, tempBaseAppHtml);
            }

            #endregion 전자결재 근거문서

            #region 업무협업 근거문서
            string tempBaseTaskHtml = "";

            Zeospace.DeskPlusMobile.Web.Task.Ws.Data.BaseTask baseTaskDal = new Zeospace.DeskPlusMobile.Web.Task.Ws.Data.BaseTask();
            DataSet dsTask = baseTaskDal.GetBaseTask(_companyID, _listID);

            if (dsTask != null)
            {
                foreach (DataRow dr in dsTask.Tables[0].Rows)
                {
                    string temphtml = @"<tr>
                                <td>
                                    <a onclick=""View.TaskView('{1}')"">{0}</a>
                                </td>
                            </tr>";

                    tempBaseTaskHtml += string.Format(temphtml, dr["Name"].ToString(), dr["BaseID"].ToString());
                }
            }

            if (tempBaseTaskHtml == "")
            {
                baseTaskHtml = "";
            }
            else
            {
                baseTaskHtml = string.Format(baseTaskHtml, tempBaseTaskHtml);
            }

            #endregion 업무협업 근거문서

            if (_contents.Length > 0)
            {
                string tmpContent = @"
                <tr>
                    <td colspan='2' valign='top'><div>{0}</div></td>
                </tr>";

                _contents = string.Format(tmpContent, _contents);
            }
            else
            {
                _contents = "";
            }

            if (_listID == _groupID) { bodyHtml = bodyContHtml; }
            if (_isMsg == "1") { bodyHtml = bodyLogHtml; }

            if (_isType == "RE")
            {
                writeGubun = GetResource("Requester");
            }
            else if (_isType == "TO")
            {
                writeGubun = GetResource("To");
            }
            else if (_isType == "CC")
            {
                writeGubun = GetResource("Cc");
            }
            else
            {
                writeGubun = GetResource("ETCUser");
            }

            bodyHtml = string.Format(
                bodyHtml,
                attchHtml,
                baseAppHtml,
                baseTaskHtml,
                _name,
                _writeDate.ToShortDateString() + " " + _writeDate.ToShortTimeString(),
                _contents,
                writeGubun);

            return bodyHtml;
        }


        public string Today { get; set; }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
        /// 작성목적 : SSL 사용 유무      
        /// </summary>
        public string httpType
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
        /// 작성목적 : 도메인 정보        
        /// </summary>
        public string MobileDOMAIN
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "MobileDOMAIN");
            }
        }
    }
}