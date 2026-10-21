using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using ICSharpCode.SharpZipLib.Zip;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Task.Web.Common
{
    public partial class CreateZipFile : Base.BasePageTask
    {
        #region 변수설정

        private string Contents;
        private string BaseFileDirectory;

        //중국어 찾기
        private Regex RegexChinese = new Regex(@"\p{IsCJKUnifiedIdeographs}");
        private string FileSpool = "/DeskPlusEIP/FileDataSpool/";

        #endregion 변수설정

        #region Page Process (페이지 처리)

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보

            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Common.CreateZipFile).Assembly;	//어셈블리 선언

            #endregion 다국어 리소스 로드/Title 정보

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }

        #endregion Page Process (페이지 처리)

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadParam();

            if (!System.IO.Directory.Exists(Server.MapPath(FileSpool))) Directory.CreateDirectory(Server.MapPath(FileSpool));

            CreateContent();
        }

        #endregion DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #region (함수)

        /// <summary>
        /// 중국어 정규화
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool IsChinese(char s)
        {
            return RegexChinese.IsMatch(s.ToString());
        }

        /// <summary>
        /// 작성자 :
        /// 작성일자 :
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
        /// 작성자 :
        /// 작성일자 :
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            ListID = GetDecryptParam("listID", "").ToNumber<int>();
        }

        /// <summary>
        /// 현재 년월 가져오기 [200601]
        /// </summary>
        /// <returns></returns>
        public static string GetYearMonth()
        {
            string nowYearMon = "";
            if (DateTime.Now.Month.ToString().Length == 1)
            {
                nowYearMon = DateTime.Now.Year.ToString() + "0" + DateTime.Now.Month.ToString();
            }
            else
            {
                nowYearMon = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString();
            }
            return nowYearMon;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateContent()
        {
            #region 수신자
            DAL.ListTO dalListTo = new DAL.ListTO();
            var listTo = dalListTo.GetListTo(CurrentUserInfo.CompanyID, ListID);
            string toname = "";
            foreach (var info in listTo)
            {
                if (info != null)
                {
                    string tmpName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                    string tmpDept = this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                    string tmpTitle = this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang);
                    toname += tmpDept + "/" + tmpTitle + "/" + tmpName + ",";                    
                }
            }
            #endregion

            #region 참조자
            DAL.ListCC dalListCC = new DAL.ListCC();
            var listCC = dalListCC.GetListCC(CurrentUserInfo.CompanyID, ListID);
            string ccname = "";
            foreach (var info in listCC)
            {
                if (info != null)
                {
                    if (info.IsGroup == "G")
                    {                        
                        string tmpName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                        ccname += tmpName + ",";
                    }
                    else
                    {
                        string tmpName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn, info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang);
                        string tmpDept = this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                        string tmpTitle = this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang);
                        ccname += tmpDept + "/" + tmpTitle + "/" + tmpName + ",";
                    }
                }
            }
            #endregion 

            #region 첨부파일
            string attach = "";
            string IsBase = "";
            DAL.Attach attdal = new DAL.Attach();
            List<BE.AttachInfo> attachInfo = attdal.GetAttachList(CurrentUserInfo.CompanyID, ListID);

            if (attachInfo.Count == 0)
            {
                attach = "";
            }
            else
            {
                string attHtml = "";
                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    string temphtml = @"<tr><td>{0}</td></tr>";
                    attHtml += string.Format(temphtml, _attachInfo.FileName);
                }

                attach= string.Format(@"<table border='0' class='att_css'>{0}</table>", attHtml);
                IsBase = "Y";
            }
            #endregion 

            #region 전자결재 근거문서
            string baseAppTask = "";
            DAL.BaseApp appDal = new DAL.BaseApp();
            var dtrApp = appDal.GetBaseApp(CurrentUserInfo.CompanyID, ListID.ToString());

            string tmpHtml = "";

            if (dtrApp != null)
            {
                if (dtrApp.HasRows)
                {
                    while (dtrApp.Read())
                    {
                        string temphtml = @"<tr><td>{0}</td></tr>";
                        tmpHtml += string.Format(temphtml, dtrApp["Name"].ToString().ToNull());
                    }
                }
            }

            if (tmpHtml.Length > 0)
            {
                baseAppTask = string.Format(@"<table border='0' class='att_css'>{0}</table>", tmpHtml);
                IsBase = "Y";
            }     
            #endregion 

            #region 업무협업 근거문서
            /*
            tmpHtml = "";

            DAL.BaseTask taskDal = new DAL.BaseTask();
            var dtrTask = taskDal.GetBaseTask(CurrentUserInfo.CompanyID, ListID.ToString());

            if (dtrTask != null)
            {
                if (dtrTask.HasRows)
                {
                    while (dtrTask.Read())
                    {
                        string temphtml = @"<tr><td>{0}</td></tr>";
                        tmpHtml += string.Format(temphtml, GetResource("Base1") + dtrTask["Name"].ToString());
                    }
                }
            }

            if (tmpHtml.Length > 0)
            {
                baseAppTask += string.Format(@"<table border='0' class='att_css'>{0}</table>", tmpHtml);
                IsBase = "Y";
            }     
            */
            #endregion            

            #region 변경이력 
            string histroy = "";
            DAL.HistoryTO dalHTo = new DAL.HistoryTO();
            int tmpTo = dalHTo.GetHistoryCnt(CurrentUserInfo.CompanyID, ListID);

            if (tmpTo > 0)
            {
                histroy = GetResource("ChangeTO") + ", ";
            }

            DAL.HistoryCC dalHCC = new DAL.HistoryCC();
            int tmpCC = dalHCC.GetHistoryCnt(CurrentUserInfo.CompanyID, ListID);

            if (tmpCC > 0)
            {
                histroy += GetResource("ChangeCC") + ", ";
            }

            DAL.HistoryOption dalHOption = new DAL.HistoryOption();
            int tmpOption = dalHOption.GetHistoryCnt(CurrentUserInfo.CompanyID, ListID);

            if (tmpOption > 0)
            {
                histroy += GetResource("ChangeDate") + ", ";
            }

            histroy = histroy.Trim().TrimEnd(',');

            #endregion 

            #region 본문 내용

            BaseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

            DAL.List dal = new DAL.List();
            var dtr = dal.GetGroupList(CurrentUserInfo.CompanyID, ListID);
            string mainHtml = "";
            string replyHtml = "";
            string taskNumber = "";
            string title = "";

            string formCssPath = FullDomainPath + "/DeskPlusEIP/Design/C/Css/Func_Task.css";
            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, formCssPath);
            string templateFooter = "</body></html>";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if (dtr["ListOrder"].ToString().ToNumber<int>() == 0)
                        {
                            taskNumber = dtr["TaskNumber"].ToString();      
                            title = dtr["Title"].ToString(); //제목 

                            string tDeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(),
                                 dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(),
                                 dtr["DeptZhtw"].ToString(), PreLang).ToReverseXSS();

                            string tTitle = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(),
                                     dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(),
                                     dtr["TitleZhtw"].ToString(), PreLang).ToReverseXSS();

                            string tName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(),
                                       dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(),
                                       dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                            string status = GetStatus(dtr["StatusLast"].ToString());

                            string item = this.GetLangName(dtr["ItemNameBase"].ToString(), dtr["ItemNameKo"].ToString(),
                                    dtr["ItemNameEn"].ToString(), dtr["ItemNameJa"].ToString(), dtr["ItemNameFr"].ToString(),
                                    dtr["ItemNameZhcn"].ToString(), dtr["ItemNameZhtw"].ToString(), PreLang).ToReverseXSS();

                            string itemSub = this.GetLangName(dtr["ItemSubNameBase"].ToString(), dtr["ItemSubNameKo"].ToString(),
                                    dtr["ItemSubNameEn"].ToString(), dtr["ItemSubNameJa"].ToString(), dtr["ItemNameFr"].ToString(),
                                    dtr["ItemSubNameZhcn"].ToString(), dtr["ItemSubNameZhtw"].ToString(), PreLang).ToReverseXSS();

                            string IsClose = (dtr["IsClose"].ToString() == "1") ? GetResource("IsClose1") : GetResource("IsClose2");
                            string IsGubun = (dtr["IsGubun"].ToString() == "1") ? GetResource("IsGubun1") : GetResource("IsGubun2");

                            string source = "";
                            if (dtr["SourceID"].ToString() != "0")
                            {
                                source = this.GetLangName(dtr["SNameBase"].ToString(), dtr["SNameKo"].ToString(), dtr["SNameEn"].ToString(),
                                            dtr["SNameJa"].ToString(), dtr["SNameFr"].ToString(), dtr["SNameZhcn"].ToString(),
                                            dtr["SNameZhtw"].ToString(), PreLang).ToReverseXSS();
                            }


                            mainHtml = @"<table cellspacing='0' class='tab_typeBA'><colgroup><col width='80px' />
                            <col /><col width='80px' /><col /></colgroup>
                            <tbody>
                            <tr>
                                <th>" + GetResource("Title") + @"</th>
                                <td colspan='3''><span class='brd_tb'>" + title + @"</span></td>
                            </tr>    
                            <tr>
                                <th>" + GetResource("Requester") + @"</th>
                                <td>" + tDeptName + " " + tTitle + " " + tName + @"</td>
                                <th>" + GetResource("State") + @"</th>                                
                                <td>" + status + @"</td>
                            </tr>   
                            <tr>
                                <th>" + GetResource("Receiver") + @"</th>                                
                                <td>" + toname.Trim().TrimEnd(',') + @"</td>
                                <th>" + GetResource("Referencer") + @"</th>
                                <td>" + ccname.Trim().TrimEnd(',') + @"</td>
                            </tr>     
                            <tr>                                
                                <th>" + GetResource("RequestDate") + @"</th>
                                <td>" + DateHelper.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal) + @"</td>
                                <th>" + GetResource("CompleteDate") + @"</th>                                
                                <td>" + DateHelper.GetCultureDateChar8(dtr["CompleteRequestDate"].ToString(), PreLangReal) + @"</td>
                            </tr>     
                            <tr>
                                <th>" + GetResource("StartDate") + @"</th>
                                <td>" + DateHelper.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal) + @"</td>
                                <th>" + GetResource("EndDate") + @"</th>
                                <td>" + DateHelper.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal) + @"</td>
                            </tr>         
                            <tr>
                                <th>" + GetResource("Group") + @"</th>
                                <td>" + item + @"->" + itemSub + @"</td>
                                <th>" + GetResource("IsClose") + @"</th>
                                <td>" + IsClose + @"</td>
                            </tr>     
                            <tr>
                                <th>" + GetResource("IsGubun") + @"</th>
                                <td>" + IsGubun + @"</td>
                                <th>" + GetResource("IsSource") + @"</th>
                                <td>" + source + @"</td>                                
                            </tr>             
                            <tr>
                                <th>" + GetResource("DocNumber") + @"</th>
                                <td>" + taskNumber + @"</td>
                                <th>" + GetResource("History") + @"</th>
                                <td>" + histroy + @"</td>
                            </tr>";

                            if(IsBase == "Y")
                            {
                               mainHtml += @"<tr>
                                <th>" + GetResource("AttachName") + @"</th>
                                <td>" + attach + @"</td>
                                <th>" + GetResource("Base") + @"</th>
                                <td>" + baseAppTask + @"</td>
                                </tr>";   
                            }
                            
                            mainHtml += @"<tr>
                                <td colspan='4' valign='top'><div style='overflow:auto;'>" + dtr["Contents"].ToString() + @"</div></td>
                            </tr>                 
                            </tbody>
                            </table>";
                        }


                        //하위글 그려주기
                        if (dtr["ListOrder"].ToString().ToNumber<int>() > 0)
                        {
                            // 글쓴이
                            string tDeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(),
                               dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(),
                               dtr["DeptZhtw"].ToString(), PreLang).ToReverseXSS();

                            string tTitle = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(),
                                     dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(),
                                     dtr["TitleZhtw"].ToString(), PreLang).ToReverseXSS();

                            string tName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(),
                                       dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(),
                                       dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                            string name = tDeptName + " " + tTitle + " " + tName;

                            replyHtml += CreateHtmlGroup(dtr["CompanyID"].ToString().ToNumber<int>(),
                                        dtr["ListID"].ToString().ToNumber<int>(),
                                        dtr["GroupID"].ToString().ToNumber<int>(),
                                        dtr["Title"].ToString(), dtr["Contents"].ToString(), dtr["ContentText"].ToString(),
                                        dtr["WriteUserID"].ToString().ToNumber<int>(),
                                        name,
                                        dtr["Status"].ToString().ToNull(),
                                        dtr["StatusLast"].ToString().ToNull(),
                                        Convert.ToDateTime(dtr["WriteDate"].ToString()),
                                        dtr["IsMsg"].ToString(),
                                        dtr["IsType"].ToString());
                        }
                    }
                }
            }

            Contents = templateHeader + mainHtml + replyHtml + templateFooter;

            #endregion 

            SendMail sendMail = new SendMail();
            MailDataInfo mailDataInfo = new MailDataInfo();

            string mailServerDomain = AppSettingHelper.GetAppSetting(AppSettingType.MAIL, "MailServerDomain", "");
            string mailAddress = "Task_" + ListID + "@" + mailServerDomain;
            mailDataInfo.From = mailAddress;
            mailDataInfo.To.Add(mailAddress, mailAddress);
            mailDataInfo.Contents = Contents;

            // 윈도우 파일명에는 \/:*?<>|" 특수문자를 넣을 수 없다. shimpark
            mailDataInfo.Subject = Regex.Replace(title, @"[\\/:*?<>|""]{1}", "");

            taskNumber = Regex.Replace(taskNumber, @"[\\/:*?<>|""]{1}", "");
            string fileName = taskNumber + ".zip";

            string tmpFileName = Guid.NewGuid().ToString();
            string zipFolderPath = BaseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
            zipFolderPath = string.Format(zipFolderPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tmpFileName, taskNumber);


            if (System.IO.Directory.Exists(Server.MapPath(zipFolderPath))) System.IO.Directory.Delete(Server.MapPath(zipFolderPath), true); // 가비지 파일이 있을수 있음
            FileHelper.CreateSubFolder(zipFolderPath);

            string docType = AppSettingHelper.GetAppSetting(AppSettingType.TASK, "IsBackupFile", "MHT");
            string sourceFileName = String.Empty;
            string destFileName = string.Empty;
            switch (docType.ToUpper())
            {
                case "IMAGE":
                    sourceFileName = Server.MapPath(FileSpool) + tmpFileName + ".jpg";
                    destFileName = Server.MapPath(zipFolderPath + "/") + taskNumber + ".jpg";

                    HtmlToImage(mailDataInfo.Contents, Server.MapPath(FileSpool), tmpFileName);

                    if (File.Exists(sourceFileName))
                    {
                        File.Move(sourceFileName, destFileName);
                    }
                    break;

                case "PDF":
                    sourceFileName = Server.MapPath(FileSpool) + tmpFileName + ".pdf";
                    destFileName = Server.MapPath(zipFolderPath + "/") + taskNumber + ".pdf";

                    HtmlToPDF(mailDataInfo.Contents, Server.MapPath(FileSpool), tmpFileName);

                    if (File.Exists(sourceFileName))
                    {
                        File.Move(sourceFileName, destFileName);
                    }
                    break;

                default:
                    sendMail.SaveMht(mailDataInfo, Server.MapPath(zipFolderPath + "/" + mailDataInfo.Subject + ".mht"), false, "", false);
                    break;
            }

            CreateZip(taskNumber, zipFolderPath);

        }

        /// <summary>
        /// 하위 글 그려주기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_listID"></param>
        /// <param name="_groupID"></param>
        /// <param name="_title"></param>
        /// <param name="_contents"></param>
        /// <param name="_contentText"></param>
        /// <param name="_writeUserID"></param>
        /// <param name="_name"></param>
        /// <param name="_status"></param>
        /// <param name="_statusLast"></param>
        /// <param name="_writeDate"></param>
        /// <param name="_isMsg"></param>
        /// <param name="_isType"></param>
        /// <returns></returns>
        private string CreateHtmlGroup(int _companyID, int _listID, int _groupID, string _title,
            string _contents, string _contentText, int _writeUserID, string _name, string _status, string _statusLast, DateTime _writeDate, string _isMsg, string _isType)
        {
            string bodyHtml = "";
            string attchHtml = "";
            string baseAppHtml = "";
            string baseTaskHtml = "";
            string bgColors = "";
            string writeGubun = "";

            switch (_status)
            {
                case "1":
                    bgColors = "tab_typeCR";//대기
                    break;
                case "2":
                    bgColors = "tab_typeCI";//접수(진행색깔)
                    break;
                case "3":
                    bgColors = "tab_typeCI";//처리중(진행색깔)
                    break;
                case "4":
                    bgColors = "tab_typeCE";//완료(완료색깔)
                    break;
                case "5":
                    bgColors = "tab_typeCE";//재검토(완료색깔)
                    break;
                case "6":
                    bgColors = "tab_typeCRE";//지연
                    break;
            }

            if (_isMsg == "1")
            {
                if (_status == "4")
                {
                    bgColors = "tab_typeCEND";//시스템 알람용
                }
                else
                {
                    bgColors = "tab_typeCN";//시스템 알람용
                }
            }

            #region 템플릿 양식
            bodyHtml = @"
                    <table cellspacing='0' class='" + bgColors + @"'>
                        <colgroup>
                            <col width='80px' /><col /><col width='80px' /><col width='140px' /><col width='70px' /><col width='170px' />
                        </colgroup>
                        <tbody>
                            <tr><th>{8}</th>
                                <td>{3}</td><th>" + GetResource("DateOfWrite") + @"</th><td>{5}</td><th>" + GetResource("State") + @"</th><td>{4}{6}</td>
                            </tr>
                            {0}{1}{2}{7}
                        </tbody>
                    </table>";

            attchHtml = @"<tr><th width='80'>" + GetResource("AttachName") + @"</th>
                        <td colspan='5'><table border='0' class='att_css'>{0}</table></td></tr>";

            baseAppHtml = @"<tr><th width='80'>" + GetResource("Base") + @"</th>
                    <td colspan='5'><table border='0' class='att_css'>{0}</table></td></tr>";

            //baseTaskHtml = @"<tr><th width='80'>" + GetResource("BaseTask") + @"</th><td colspan='5'><table border='0' class='att_css'>{0}</table></td></tr>";

            #endregion 템플릿 양식

            #region 첨부파일
            Zeospace.DeskPlus.Task.DAL.Attach dal = new Zeospace.DeskPlus.Task.DAL.Attach();
            List<Zeospace.DeskPlus.Task.BE.AttachInfo> attachInfo = dal.GetAttachList(_companyID, _listID);

            if (attachInfo.Count == 0)
            {
                attchHtml = "";
            }
            else
            {
                string attHtml = "";

                foreach (Zeospace.DeskPlus.Task.BE.AttachInfo _attachInfo in attachInfo)
                {
                    string temphtml = @"<tr><td>{0}</td></tr>";
                    attHtml += string.Format(temphtml, _attachInfo.FileName);
                }

                attchHtml = string.Format(attchHtml, attHtml);
            }

            #endregion

            #region 전자결재 근거문서
            Zeospace.DeskPlus.Task.DAL.BaseApp appDal = new Zeospace.DeskPlus.Task.DAL.BaseApp();
            var dtr = appDal.GetBaseApp(_companyID, _listID.ToString());

            string tempAppDocHtml = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        string temphtml = @"<tr><td>{0}</td></tr>";
                        tempAppDocHtml += string.Format(temphtml, dtr["Name"].ToString().ToNull());
                    }
                }
            }

            if (tempAppDocHtml == "")
            {
                baseAppHtml = "";
            }
            else
            {
                baseAppHtml = string.Format(baseAppHtml, tempAppDocHtml);
            }
            #endregion

            #region 업무협업 근거문서
            /*
            Zeospace.DeskPlus.Task.DAL.BaseTask baseTaskDal = new Zeospace.DeskPlus.Task.DAL.BaseTask();
            var dtrTask = baseTaskDal.GetBaseTask(_companyID, _listID.ToString());

            string tempBaseTaskHtml = "";

            if (dtrTask != null)
            {
                if (dtrTask.HasRows)
                {
                    while (dtrTask.Read())
                    {
                        string temphtml = @"<tr><td>{0}</td></tr>";
                        tempBaseTaskHtml += string.Format(temphtml, dtrTask["Name"].ToString());
                    }
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
            */
            #endregion

            string btnHtml = "";

            if (_contentText.Length > 0 || _contents.IndexOf("<IMG") != -1)
            {
                /*
                string tmpContent = @"
                <tr>
                    <th>" + GetResource("ReplyContent") + @"</th>
                    <td colspan='5' valign='top'><div style='overflow:auto;'>{0}</div></td>
                </tr>";
                */

                string tmpContent = @"
                <tr>                    
                    <td colspan='6' valign='top'><div style='overflow:auto;'>{0}</div></td>
                </tr>";

                _contents = string.Format(tmpContent, _contents);
            }
            else
            {
                _contents = "";
            }

            string statusStr = GetStatus(_status);

            if (_isType == "RE")
            {
                writeGubun = GetResource("Requester");
            }
            else if (_isType == "TO")
            {
                writeGubun = GetResource("Receiver");
            }
            else if (_isType == "CC")
            {
                writeGubun = GetResource("Referencer");
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
                statusStr,
                _writeDate.ToShortDateString() + " " + _writeDate.ToShortTimeString(),
                btnHtml,
                _contents,
                writeGubun);

            return bodyHtml;
        }

        /// <summary>
        /// Zip파일 만들기
        /// </summary>
        private void CreateZip(string _taskNumber, string _zipFolderPath)
        {
            string fileName = _taskNumber + ".zip";

            DAL.Attach dal = new DAL.Attach();
            List<BE.AttachInfo> attachInfo = dal.GetAttachGroupList(CurrentUserInfo.CompanyID, ListID);

            string oriFilePath = "";
            string oriFileName = "";
            string oriFileGUID = "";
            string realFileName = "";

            foreach (var _attachInfo in attachInfo)
            {
                oriFilePath = _attachInfo.FilePath;
                oriFileGUID = _attachInfo.FileGUID;   //암호화된 파일명
                oriFileName = _attachInfo.FileName;   //원본 파일명
                realFileName = oriFileName;

                foreach (char c in oriFileName)
                {
                    if (IsChinese(c) == true)
                    {
                        realFileName = oriFileGUID;
                        break;
                    }
                }

                System.IO.File.Copy(Server.MapPath(oriFilePath + oriFileGUID), Server.MapPath(_zipFolderPath + "/" + realFileName));
            }


            //5.zip 파일 생성하기
            ZipFile.ZipCompress(
                Server.MapPath(_zipFolderPath),
                Server.MapPath(_zipFolderPath + "/" + fileName),
                "");

            //잔여 첨부파일 삭제하기 (용량을 위하~~~)
            foreach (var attInfo in attachInfo)
            {
                oriFilePath = attInfo.FilePath;
                oriFileGUID = attInfo.FileGUID;
                oriFileName = attInfo.FileName;

                realFileName = oriFileName;

                foreach (char c in oriFileName)
                {
                    if (IsChinese(c) == true)
                    {
                        realFileName = oriFileGUID;
                        break;
                    }
                }

                System.IO.File.Delete(Server.MapPath(_zipFolderPath + "/" + realFileName));
            }

            String header = Request.Headers["User-Agent"];

            if (header.IndexOf("MSIE") > -1 || header.IndexOf("rv") > -1)
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName).Replace("+", "%20") + "");
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName + "");
            }

            Response.ContentType = "multipart/form-data";
            Response.TransmitFile(Server.MapPath(_zipFolderPath + "/" + fileName));
            Response.Flush();
        }


        #region zip 모듈

        private void Compress(string source, string destination)
        {
            using (System.IO.FileStream sourceStream = System.IO.File.OpenRead(source))
            {
                using (System.IO.FileStream destinationStream = System.IO.File.OpenWrite(destination))
                {
                    using (System.IO.Compression.GZipStream compressedStream = new System.IO.Compression.GZipStream(destinationStream, System.IO.Compression.CompressionMode.Compress))
                    {
                        byte[] data = new byte[sourceStream.Length];

                        sourceStream.Read(data, 0, data.Length);
                        compressedStream.Write(data, 0, data.Length);
                    }
                }
            }
        }

        public class ZipFile
        {
            public ZipFile()
            {
                //
                // TODO: 여기에 생성자 논리를 추가합니다.
                //
                //string startFolder = @"C:\inetpub\wwwroot\conversion";
                //string outputZipPath = @"C:\inetpub\wwwroot";
                //string password = "1111";
                //Zeospace.DeskPlus.Component.WebComponent.ZipFile.ZipIt(startFolder,outputZipPath + "\\" + DateTime.Now.ToString("yyyyddmmhhmm")	+ ".exe", password);
            }

            private static Hashtable excludeDirs = new Hashtable();

            //압축하기
            public static void ZipCompress(string Path, string outPathAndZipFile, string password)
            {
                string OutPath = outPathAndZipFile;
                ArrayList ar = GenerateFileList(Path); // generate file list
                // find number of chars to remove from orginal file path
                int TrimLength = (Directory.GetParent(Path)).ToString().Length;
                TrimLength += 1; //remove '\'

                byte[] obuffer;

                using (ZipOutputStream oZipStream = new ZipOutputStream(File.Create(OutPath)))
                {
                    if (password != String.Empty) oZipStream.Password = password;
                    oZipStream.SetLevel(7); // 9 = maximum compression level;7level 안전
                    ZipEntry oZipEntry;

                    foreach (string Fil in ar) // for each file, generate a zipentry
                    {
                        if (Fil.ToUpper().IndexOf("THUMBS.DB") == -1)
                        {
                            oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));

                            oZipStream.PutNextEntry(oZipEntry);

                            if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                            {
                                using (FileStream ostream = File.OpenRead(Fil))
                                {
                                    obuffer = new byte[ostream.Length];
                                    ostream.Read(obuffer, 0, obuffer.Length);
                                    oZipStream.Write(obuffer, 0, obuffer.Length);
                                }
                            }
                        }
                    }
                }
            }

            private static ArrayList GenerateFileList(string Dir)
            {
                ArrayList mid = new ArrayList();
                bool Empty = true;
                foreach (string file in Directory.GetFiles(Dir)) // add each file in directory
                {
                    mid.Add(file);
                    Empty = false;
                }

                if (Empty)
                {
                    if (Directory.GetDirectories(Dir).Length == 0) // if directory is completely empty, add it
                    {
                        mid.Add(Dir + @"/");
                    }
                }

                foreach (string dirs in Directory.GetDirectories(Dir)) // do this recursively
                {
                    // set up the excludeDir test
                    string testDir = dirs.Substring(dirs.LastIndexOf(@"\") + 1).ToUpper();
                    if (ZipFile.excludeDirs.Contains(testDir))
                        continue;
                    foreach (object obj in GenerateFileList(dirs))
                    {
                        mid.Add(obj);
                    }
                }
                return mid; // return file list
            }

            /// <summary>
            /// 압축풀기
            /// </summary>
            /// <param name="file"></param>
            /// <param name="dir"></param>
            /// <returns></returns>
            public static bool ZipUnpack(string file, string dir)
            {
                try
                {
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    ZipInputStream s = new ZipInputStream(File.OpenRead(file));
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name);

                        if (directoryName != String.Empty)
                            Directory.CreateDirectory(dir + "\\" + directoryName);

                        if (fileName != String.Empty)
                        {
                            FileStream streamWriter = File.Create(dir + "\\" + theEntry.Name);
                            int size = 2048;
                            byte[] data = new byte[2048];

                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            streamWriter.Close();
                        }
                    }

                    s.Close();

                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion zip 모듈

        #region HtmlToImage

        protected void HtmlToImage(string _content, string _filePath, string _fileName)
        {
            // 이벤트 생성
            AutoResetEvent resultEvent = new AutoResetEvent(false);

            Zeospace.DeskPlus.Component.Converter.HtmlToImage browser = new Component.Converter.HtmlToImage(_content, _filePath, _fileName, resultEvent);

            // 이미지 생성 쓰레드에서 완료 이벤트가 올때까지 대기함
            EventWaitHandle.WaitAll(new AutoResetEvent[] { resultEvent });

            // 이미지 생성시까지 대기
            while (browser == null || browser.HtmlResult == null) Thread.Sleep(5);

            string result = browser.HtmlResult; // 완료 이벤트

            browser.Dispose(); // 이미지 생성 쓰레드 Kill
        }

        #endregion HtmlToImage

        #region HtmlToPDF

        private void HtmlToPDF(string _content, string _filePath, string _fileName)
        {
            // 이벤트 생성
            AutoResetEvent resultEvent = new AutoResetEvent(false);

            Zeospace.DeskPlus.Component.Converter.HtmlToPDF browser = new Component.Converter.HtmlToPDF(_content, _filePath, _fileName, "V", resultEvent);

            // 이미지 생성 쓰레드에서 완료 이벤트가 올때까지 대기함
            EventWaitHandle.WaitAll(new AutoResetEvent[] { resultEvent });

            // 이미지 생성시까지 대기
            while (browser == null || browser.HtmlResult == null) Thread.Sleep(5);

            string result = browser.HtmlResult; // 완료 이벤트

            browser.Dispose(); // 이미지 생성 쓰레드 Kill
        }

        #endregion HtmlToPDF

        #endregion (함수)
    }
}