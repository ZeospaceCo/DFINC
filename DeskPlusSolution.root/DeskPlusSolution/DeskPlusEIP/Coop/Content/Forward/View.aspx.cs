using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Coop.Web.Content.Forward
{
    public partial class View : Base.BasePageCoop
    {
        #region 변수설정

        public int UnitID;

        /// <summary>
        /// 1:대기;2:접수;3:처리중;4:완료;5:반려
        /// </summary>
        public string Status = string.Empty;

        public int ForwardListID = 0;
        public string IsAttach = "";
        public string IsBaseDoc = "";
        public string IsBaseCoop = "";
        private Hashtable ProcessTime = new Hashtable();

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드

            ResourceName = "Zeospace.DeskPlus.Coop.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Coop.Web.Content.Forward.List).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스 로드

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();

                SwfUploadGUID = System.Guid.NewGuid().ToString();

                #region 에디터 본문 이미지 첨부 경로

                System.Guid guid = System.Guid.NewGuid();
                string reqTime = System.DateTime.Now.ToString("yyyyMM");
                UploadPath = "/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID + "/Coop/" + reqTime + "/editor/" + guid.ToString();

                #endregion 에디터 본문 이미지 첨부 경로
            }
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            //자료 뿌려주기
            LoadDataContent();

            //조회 카운트해오기
            DAL.ForwardListViewer dal = new DAL.ForwardListViewer();
            dal.Add(CurrentUserInfo.CompanyID, ForwardListID, CurrentUserInfo.UserID);
        }

        #endregion Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            if (!this.resMng.Equals(null))
            {
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnList.Text = resMng.GetString("List");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            ForwardListID = GetDecryptParam("forwardListID", "0").ToNumber<int>();
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : 템플릿 적용
        /// 작성자   : 심재운
        /// </summary>
        /// <returns></returns>
        private string GetTemplate(int _itemSubID)
        {
            DAL.Template bll = new DAL.Template();
            var alList = bll.GetTemplateLists(CurrentUserInfo.CompanyID, _itemSubID);

            foreach (var be in alList) return be.Contents;

            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";
            return templateHeader + templateFooter;
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 자료 가져오기
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-03-27
        /// </summary>
        private void LoadDataContent()
        {
            ltlHeaderStartDate.Text = GetResource("StartDate");
            ltlHeaderEndDate.Text = GetResource("EndDate");

            DAL.ForwardList dalList = new DAL.ForwardList();

            //1. 내용 가져오기
            var dtrList = dalList.GetList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, ForwardListID.ToString());

            if (dtrList != null)
            {
                if (dtrList.HasRows)
                {
                    while (dtrList.Read())
                    {
                        ltlItem.Text = this.GetLangName(
                                    dtrList["ItemNameBase"].ToString(),
                                    dtrList["ItemNameKo"].ToString(),
                                    dtrList["ItemNameEn"].ToString(),
                                    dtrList["ItemNameJa"].ToString(),
                                    dtrList["ItemNameFr"].ToString(),
                                    dtrList["ItemNameZhcn"].ToString(),
                                    dtrList["ItemNameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ltlItemSub.Text = this.GetLangName(
                                    dtrList["ItemSubNameBase"].ToString(),
                                    dtrList["ItemSubNameKo"].ToString(),
                                    dtrList["ItemSubNameEn"].ToString(),
                                    dtrList["ItemSubNameJa"].ToString(),
                                    dtrList["ItemSubNameFr"].ToString(),
                                    dtrList["ItemSubNameZhcn"].ToString(),
                                    dtrList["ItemSubNameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ltlRequestDate.Text = DateHelper.GetCultureDateChar8(dtrList["RequestDate"].ToString(), PreLangReal);
                        ltlCompleteRequestDate.Text = DateHelper.GetCultureDateChar8(dtrList["CompleteRequestDate"].ToString(), PreLangReal);
                        ltlIsClose.Text = (dtrList["IsClose"].ToString() == "2") ? GetResource("Private") : GetResource("Public");
                        ltlSubject.Text = dtrList["Title"].ToString();
                        ltlCoopNumber.Text = dtrList["CoopNumber"].ToString();
                        ltlRequester.Text = string.Format(@"<a onclick=""AccountInfo('{1}');return false;"" style='cursor:pointer'>{0}</a>", this.GetLangName(
                                    dtrList["NameBase"].ToString(),
                                    dtrList["NameKo"].ToString(),
                                    dtrList["NameEn"].ToString(),
                                    dtrList["NameJa"].ToString(),
                                    dtrList["NameFr"].ToString(),
                                    dtrList["NameZhcn"].ToString(),
                                    dtrList["NameZhtw"].ToString(), PreLang).ToReverseXSS(), dtrList["WriteUserID"].ToString().ToNumber<int>());

                        //1:대기;2:접수;3:처리중;4:완료;5:반려
                        Status = dtrList["StatusLast"].ToString();

                        ltlStartDate.Text = DateHelper.GetCultureDateChar8(dtrList["StartDate"].ToString(), PreLangReal);
                        ltlEndDate.Text = DateHelper.GetCultureDateChar8(dtrList["EndDate"].ToString(), PreLangReal);

                        if (Status == "4")
                        {
                            ltlHeaderStartDate.Text = GetResource("Start");
                            ltlHeaderEndDate.Text = GetResource("End");
                        }

                        //난이도(상:1;중:2;하:3)
                        ltlDifficulty.Text = GetDifficulty(dtrList["Difficultys"].ToString());
                        //상태
                        ltlStatus.Text = GetStatus(dtrList["StatusLast"].ToString());

                        //본문내용
                        lbContent.Text = dtrList["Contents"].ToString();
                    }
                }
            }

            #region 첨부파일

            //첨부파일
            DAL.ForwardAttach dal = new DAL.ForwardAttach();
            List<BE.ForwardAttachInfo> attachInfo = dal.GetForwardAttachFileLists(CurrentUserInfo.CompanyID, ForwardListID);

            if (attachInfo.Count == 0)
            {
                ltlAttach.Text = "";
                IsAttach = @"style='display:none'";
            }
            else
            {
                string attHtml = "";

                foreach (BE.ForwardAttachInfo _attachInfo in attachInfo)
                {
                    _attachInfo.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : _attachInfo.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", _attachInfo.FilePath + _attachInfo.FileGUID);
                    paraDic.Add("fileName", _attachInfo.FileName);
                    _attachInfo.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    _attachInfo.FileType = _attachInfo.FileType.Replace(".", "");
                    _attachInfo.FileLength = _attachInfo.FileLength;
                    _attachInfo.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    string temphtml = @"
                        <tr>
                            <td>
                                <a onclick=""javascript:View.FileDownload('{0}','{1}','{2}')"">{1}</a>
                            </td>
                        </tr>";

                    attHtml += string.Format(temphtml, _attachInfo.FilePath, _attachInfo.FileName, _attachInfo.FileGUID);
                }

                ltlAttach.Text = string.Format(@"
                    <table border='0' class='att_css'>
                        {0}
                    </table>", attHtml);
            }

            #endregion 첨부파일

            #region 전자결재 근거문서

            DAL.ForwardBaseDoc forwardDal = new DAL.ForwardBaseDoc();
            var dtr = forwardDal.GetForwardBaseDoc(CurrentUserInfo.CompanyID, ForwardListID);

            string tmpHtml = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ForwardBaseDocInfo Info = new BE.ForwardBaseDocInfo();

                        Info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        Info.ForwardListID = dtr["ForwardListID"].ToString().ToNumber<int>();
                        Info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        Info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();

                        Info.ItemName = this.GetLangName(
                                dtr["ItemNameBase"].ToString().ToNull(), dtr["ItemNameKo"].ToString().ToNull(),
                                dtr["ItemNameEn"].ToString().ToNull(), dtr["ItemNameJa"].ToString().ToNull(),
                                dtr["ItemNameFr"].ToString().ToNull(), dtr["ItemNameZhcn"].ToString().ToNull(),
                                dtr["ItemNameZhtw"].ToString().ToNull(), PreLang);

                        Info.ItemSubName = this.GetLangName(
                                dtr["ItemSubNameBase"].ToString().ToNull(), dtr["ItemSubNameKo"].ToString().ToNull(),
                                dtr["ItemSubNameEn"].ToString().ToNull(), dtr["ItemSubNameJa"].ToString().ToNull(),
                                dtr["ItemSubNameFr"].ToString().ToNull(), dtr["ItemSubNameZhcn"].ToString().ToNull(),
                                dtr["ItemSubNameZhtw"].ToString().ToNull(), PreLang);

                        Info.RequestDate = DateHelper.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                        Info.StartDate = DateHelper.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                        Info.EndDate = DateHelper.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                        Info.Title = dtr["Title"].ToString();

                        Info.NameBase = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

                        // 근거문서 문서제목
                        Info.Name = dtr["Name"].ToString().ToNull();

                        string temphtml = @"<tr>
                                <td>
                                    <a onclick=""Common.View('{1}')"">{0}</a>
                                </td>
                            </tr>";

                        tmpHtml += string.Format(temphtml, Info.Name, Info.DocumentID);
                    }
                }
            }

            if (tmpHtml.Length > 0)
            {
                ltlBasedoc.Text = string.Format(@"
                    <table border='0' class='att_css'>
                        {0}
                    </table>", tmpHtml);
            }
            else
            {
                IsBaseDoc = @"style='display:none'";
            }

            #endregion 전자결재 근거문서

            #region 업무협업 근거문서

            tmpHtml = "";

            DAL.ForwardBaseCoop coopFWDal = new DAL.ForwardBaseCoop();
            var dtrCoop = coopFWDal.GetForwardBaseCoop(CurrentUserInfo.CompanyID, ForwardListID);

            if (dtrCoop != null)
            {
                if (dtrCoop.HasRows)
                {
                    while (dtrCoop.Read())
                    {
                        BE.BaseCoopInfo info = new BE.BaseCoopInfo();

                        info.CompanyID = dtrCoop["CompanyID"].ToString().ToNumber<int>();
                        info.ListID = dtrCoop["ListID"].ToString().ToNumber<int>();
                        info.GroupID = dtrCoop["GroupID"].ToString().ToNumber<int>();
                        info.BaseCoopID = dtrCoop["BaseCoopID"].ToString().ToNumber<int>();
                        info.BaseOrder = dtrCoop["BaseOrder"].ToString().ToNumber<int>();
                        info.Subject = dtrCoop["Subject"].ToString();

                        string temphtml = @"<tr>
                                <td>
                                    <a onclick=""Common.CoopViewer('{1}')"">{0}</a>
                                </td>
                            </tr>";

                        tmpHtml += string.Format(temphtml, info.Subject, info.BaseCoopID);
                    }
                }
            }

            if (tmpHtml.Length > 0)
            {
                ltlBaseCoop.Text = string.Format(@"
                    <table border='0' class='att_css'>
                        {0}
                    </table>", tmpHtml);
            }
            else
            {
                IsBaseCoop = @"style='display:none'";
            }

            #endregion 업무협업 근거문서

            LoadDataGroup();

            GetWorkProcess(Status);

            DAL.ForwardListTO dalListTo = new DAL.ForwardListTO();
            var listTo = dalListTo.GetForwardListTo(CurrentUserInfo.CompanyID, ForwardListID);
            string toList = "";
            foreach (var listInfo in listTo)
            {
                if (listInfo != null)
                {
                    toList += string.Format(@"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>, ", listInfo.UserID, this.GetLangName(
                                listInfo.NameBase,
                                listInfo.NameKo, listInfo.NameEn, listInfo.NameJa,
                                listInfo.NameFr, listInfo.NameZhcn, listInfo.NameZhtw, PreLang).ToReverseXSS());
                }

                ltlReceive.Text = toList.Trim().TrimEnd(',');
            }

            DAL.ForwardListCC dalListCC = new DAL.ForwardListCC();
            var listCC = dalListCC.GetForwardListCC(CurrentUserInfo.CompanyID, ForwardListID);
            string ccList = "";
            foreach (var listInfo in listCC)
            {
                if (listInfo != null)
                {
                    ccList += string.Format(@"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>, ", listInfo.UserID, this.GetLangName(
                                listInfo.NameBase,
                                listInfo.NameKo, listInfo.NameEn, listInfo.NameJa,
                                listInfo.NameFr, listInfo.NameZhcn, listInfo.NameZhtw, PreLang).ToReverseXSS());
                }

                this.ltlReference.Text = ccList.Trim().TrimEnd(',');
            }
        }

        /// <summary>
        /// 그룹별 내용 출력하기
        /// </summary>
        private void LoadDataGroup()
        {
            DAL.ForwardList dal = new DAL.ForwardList();

            var dtrList = dal.GetListByGroup(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, ForwardListID);
            string replyHtml = "";
            DateTime dtStatus1 = new DateTime(); // 대기
            DateTime dtStatus2 = new DateTime(); // 접수
            DateTime dtStatus3 = new DateTime(); // 진행
            DateTime dtStatus4 = new DateTime(); // 완료
            DateTime dtStatus5 = new DateTime(); // 반려

            if (dtrList != null)
            {
                if (dtrList.HasRows)
                {
                    while (dtrList.Read())
                    {
                        /*
                        //진행 상태별 최종 시간을 hashtable에 저장
                        if (!ProcessTime.ContainsKey(dtrList["Status"]))
                        {
                            ProcessTime.Add(dtrList["Status"].ToString(), Convert.ToDateTime(dtrList["WriteDate"].ToString()));
                        }else{
                            DateTime lastDate;
                            if (String.IsNullOrEmpty(dtrList["ModifyDate"].ToString()))
                            {
                                lastDate = Convert.ToDateTime(dtrList["WriteDate"].ToString().ToNull());
                            }
                            else
                            {
                                lastDate = (Convert.ToDateTime(dtrList["WriteDate"].ToString().ToNull()) > Convert.ToDateTime(dtrList["ModifyDate"].ToString().ToNull())) ? Convert.ToDateTime(dtrList["WriteDate"].ToString().ToNull()) : Convert.ToDateTime(dtrList["ModifyDate"].ToString().ToNull());
                            }
                            if (Convert.ToDateTime(ProcessTime[dtrList["Status"].ToString()]) < lastDate) ProcessTime[dtrList["Status"].ToString()] = lastDate;
                        }
                        */

                        switch (dtrList["Status"].ToString())
                        {
                            case "1":  // 대기  : 최초 입력 시간을 넣는다
                                if (dtStatus1.Ticks == 0)
                                {
                                    dtStatus1 = CompareDate(dtrList["WriteDate"].ToString(), dtrList["ModifyDate"].ToString());
                                }
                                break;

                            case "2":  // 접수 : IsAlarm 이 1인 값을 넣는다
                                if (dtrList["IsAlarm"].ToString() == "1")
                                {
                                    dtStatus2 = CompareDate(dtrList["WriteDate"].ToString(), dtrList["ModifyDate"].ToString());
                                }
                                break;

                            case "3":  // 처리중 : 마지막 Row 를 넣는다
                                if (dtrList["IsAlarm"].ToString() == "1")
                                {
                                    // 접수처리시 날짜가 지나면 처리중으로 변경이 됨
                                    if (dtStatus2.Ticks == 0) dtStatus2 = CompareDate(dtrList["WriteDate"].ToString(), dtrList["ModifyDate"].ToString());
                                }
                                else
                                {
                                    dtStatus3 = CompareDate(dtrList["WriteDate"].ToString(), dtrList["ModifyDate"].ToString());
                                }
                                break;

                            case "4":  // 완료  : IsAlarm 이 1인 값을 넣는다
                                if (dtrList["IsAlarm"].ToString() == "1")
                                {
                                    // 접수처리시 날짜가 지나면 처리중으로 변경이 됨
                                    dtStatus4 = CompareDate(dtrList["WriteDate"].ToString(), dtrList["ModifyDate"].ToString());
                                }
                                break;

                            case "5":  // 반려
                                if (dtrList["IsAlarm"].ToString() == "1")
                                {
                                    // 접수처리시 날짜가 지나면 처리중으로 변경이 됨
                                    dtStatus5 = CompareDate(dtrList["WriteDate"].ToString(), dtrList["ModifyDate"].ToString());
                                }
                                break;
                        }

                        //답변글만 출력한다.
                        if (dtrList["ListOrder"].ToString().ToNumber<int>() > 0)
                        {
                            #region 이름 다국어 설정

                            string name = this.GetLangName(
                                    dtrList["NameBase"].ToString().ToNull(), dtrList["NameKo"].ToString().ToNull(),
                                    dtrList["NameEn"].ToString().ToNull(), dtrList["NameJa"].ToString().ToNull(),
                                    dtrList["NameFr"].ToString().ToNull(), dtrList["NameZhcn"].ToString().ToNull(),
                                    dtrList["NameZhtw"].ToString().ToNull(), PreLang);

                            #endregion 이름 다국어 설정

                            replyHtml += CreateHtmlGroup(dtrList["CompanyID"].ToString().ToNumber<int>(),
                                        dtrList["ForwardListID"].ToString().ToNumber<int>(),
                                        dtrList["ForwardGroupID"].ToString().ToNumber<int>(),
                                        dtrList["ListOrder"].ToString().ToNumber<int>(),
                                        dtrList["Title"].ToString(), dtrList["Contents"].ToString(), dtrList["MobileContent"].ToString(),
                                        dtrList["WriteUserID"].ToString().ToNumber<int>(),
                                        name,
                                        dtrList["Status"].ToString().ToNull(),
                                        dtrList["StatusLast"].ToString().ToNull(),
                                        Convert.ToDateTime(dtrList["WriteDate"].ToString()),
                                        dtrList["IsAlarm"].ToString()
                                        );
                        }
                    }

                    ProcessTime.Add("1", dtStatus1); //대기
                    ProcessTime.Add("2", dtStatus2); //접수
                    ProcessTime.Add("3", dtStatus3); //진행
                    ProcessTime.Add("4", dtStatus4); //완료
                    ProcessTime.Add("5", dtStatus5); //반송
                }
            }

            if (replyHtml.Length > 0) pnContent.Controls.Add(new LiteralControl("<h1>" + GetResource("IngDetail") + "</h1>" + replyHtml));
        }

        /// <summary>
        /// 그룹별 내용 출력하기 - html
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_forwardListID">The _forward list ID.</param>
        /// <param name="_forwardGroupID">The _forward group ID.</param>
        /// <param name="_listOrder">The _list order.</param>
        /// <param name="_title">The _title.</param>
        /// <param name="_contents">The _contents.</param>
        /// <param name="_writeUserID">The _write user ID.</param>
        /// <param name="_name">The _name.</param>
        /// <param name="_status">The _status.</param>
        /// <param name="_statusLast">The _status last.</param>
        /// <param name="_writeDate">The _write date.</param>
        /// <param name="_isAlarm">The _is alarm.</param>
        /// <returns></returns>
        private string CreateHtmlGroup(int _companyID, int _forwardListID, int _forwardGroupID, int _listOrder, string _title,
            string _contents, string _mobileContent, int _writeUserID, string _name, string _status, string _statusLast, DateTime _writeDate, string _isAlarm)
        {
            string tmpName = @"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>";
            _name = string.Format(tmpName, _writeUserID, _name);

            string bodyHtml = "";
            string bodyLogHtml = "";
            string attchHtml = "";
            string baseDocHtml = "";
            string baseCoopHtml = "";

            string bgColors = "";
            switch (_status)
            {
                case "1":
                    bgColors = "tab_typeCR";//대기
                    break;

                case "2":
                    bgColors = "tab_typeCI";//진행
                    break;

                case "3":
                    bgColors = "tab_typeCI";//진행
                    break;

                case "4":
                    bgColors = "tab_typeCE";//완료
                    break;

                case "5":
                    bgColors = "tab_typeCRE";//반려
                    break;
            }

            if (_isAlarm == "1")
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
                <div class='cw_Abox'>
                    <table cellspacing='0' class='" + bgColors + @"'>
                        <colgroup>
                            <col width='80px' />
                            <col />
                            <col width='80px' />
                            <col />
                            <col width='80px' />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>" + GetResource("WriteName") + @"</th>
                                <td>{3}</td><th>" + GetResource("State") + @"</th><td>{4}</td><th>" + GetResource("DateOfWrite") + @"</th><td>{5}{6}</td>
                            </tr>
                            {0}{1}{2}
                            {7}
                        </tbody>
                    </table>
                </div>";

            bodyLogHtml = @"
                <div class='cw_Abox'>
                    <table cellspacing='0' class='" + bgColors + @"'>
                        <colgroup>
                            <col width='80px' />
                            <col />
                            <col width='80px' />
                            <col />
                            <col width='80px' />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>" + GetResource("WriteName") + @"</th>
                                <td>{3}</td>
                                <th></th>
                                <td>{4}</td>
                                <th>" + GetResource("DateOfWrite") + @"</th>
                                <td>{5}{6}</td>
                            </tr>
                            {0}{1}{2}
                            {7}
                        </tbody>
                    </table>
                </div>";

            attchHtml = @"<tr>
                        <th width='80'>" + GetResource("AttachName") + @"</th>
                        <td colspan='5' width='1010'>
                            <table border='0' class='att_css'>
                                {0}
                            </table>
                        </td>
                    </tr>";

            baseDocHtml = @"<tr>
                    <th width='80'>" + GetResource("BaseDoc") + @"</th>
                    <td colspan='5'>
                        <table border='0' class='att_css'>
                            {0}
                        </table>
                    </td>
                </tr>";

            baseCoopHtml = @"<tr>
                    <th width='80'>업무협업 근거문서</th>
                    <td colspan='5'>
                        <table border='0' class='att_css'>
                            {0}
                        </table>
                    </td>
                </tr>";

            #endregion 템플릿 양식

            #region 첨부파일

            //첨부파일
            DAL.ForwardAttach dal = new DAL.ForwardAttach();
            List<BE.ForwardAttachInfo> attachInfo = dal.GetForwardAttachFileLists(_companyID, _forwardListID);

            if (attachInfo.Count == 0)
            {
                attchHtml = "";
            }
            else
            {
                string attHtml = "";

                foreach (BE.ForwardAttachInfo _attachInfo in attachInfo)
                {
                    _attachInfo.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : _attachInfo.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", _attachInfo.FilePath + _attachInfo.FileGUID);
                    paraDic.Add("fileName", _attachInfo.FileName);
                    _attachInfo.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    _attachInfo.FileType = _attachInfo.FileType.Replace(".", "");
                    _attachInfo.FileLength = _attachInfo.FileLength;
                    _attachInfo.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    string temphtml = @"
                        <tr>
                            <td>
                                <a onclick=""javascript:View.FileDownload('{0}','{1}','{2}')"">{1}</a>
                            </td>
                        </tr>";

                    attHtml += string.Format(temphtml, _attachInfo.FilePath, _attachInfo.FileName, _attachInfo.FileGUID);
                }

                attchHtml = string.Format(attchHtml, attHtml);
            }

            #endregion 첨부파일

            #region 전자결재 근거문서

            DAL.ForwardBaseDoc forwardDal = new DAL.ForwardBaseDoc();
            var dtr = forwardDal.GetForwardBaseDoc(_companyID, _forwardListID);

            BE.ForwardBaseDocInfo Info = null;
            string tempAppDocHtml = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Info = new BE.ForwardBaseDocInfo();

                        Info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        Info.ForwardListID = dtr["ForwardListID"].ToString().ToNumber<int>();
                        Info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();

                        Info.ItemName = this.GetLangName(
                                dtr["ItemNameBase"].ToString().ToNull(), dtr["ItemNameKo"].ToString().ToNull(),
                                dtr["ItemNameEn"].ToString().ToNull(), dtr["ItemNameJa"].ToString().ToNull(),
                                dtr["ItemNameFr"].ToString().ToNull(), dtr["ItemNameZhcn"].ToString().ToNull(),
                                dtr["ItemNameZhtw"].ToString().ToNull(), PreLang);

                        Info.ItemSubName = this.GetLangName(
                                dtr["ItemSubNameBase"].ToString().ToNull(), dtr["ItemSubNameKo"].ToString().ToNull(),
                                dtr["ItemSubNameEn"].ToString().ToNull(), dtr["ItemSubNameJa"].ToString().ToNull(),
                                dtr["ItemSubNameFr"].ToString().ToNull(), dtr["ItemSubNameZhcn"].ToString().ToNull(),
                                dtr["ItemSubNameZhtw"].ToString().ToNull(), PreLang);

                        Info.RequestDate = DateHelper.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                        Info.StartDate = DateHelper.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                        Info.EndDate = DateHelper.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                        Info.Title = dtr["Title"].ToString();

                        Info.NameBase = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

                        // 근거문서 문서제목
                        Info.Name = dtr["Name"].ToString().ToNull();

                        string temphtml = @"<tr>
                                <td>
                                    <a onclick=""Common.View('{1}')"">{0}</a>
                                </td>
                            </tr>";

                        tempAppDocHtml += string.Format(temphtml, Info.Name, Info.DocumentID);
                    }
                }
            }

            if (Info == null)
            {
                baseDocHtml = "";
            }
            else
            {
                baseDocHtml = string.Format(baseDocHtml, tempAppDocHtml);
            }

            #endregion 전자결재 근거문서

            #region 업무협업 근거문서

            DAL.ForwardBaseCoop baseCoopDal = new DAL.ForwardBaseCoop();
            var dtrCoop = baseCoopDal.GetForwardBaseCoop(_companyID, _forwardListID);

            string tempBaseCoopHtml = "";

            if (dtrCoop != null)
            {
                if (dtrCoop.HasRows)
                {
                    while (dtrCoop.Read())
                    {
                        BE.ForwardBaseCoopInfo info = new BE.ForwardBaseCoopInfo();

                        info.CompanyID = dtrCoop["CompanyID"].ToString().ToNumber<int>();
                        info.ForwardListID = dtrCoop["ForwardListID"].ToString().ToNumber<int>();
                        info.ForwardBaseCoopID = dtrCoop["ForwardBaseCoopID"].ToString().ToNumber<int>();
                        info.ListID = dtrCoop["ListID"].ToString().ToNumber<int>();
                        info.GroupID = dtrCoop["GroupID"].ToString().ToNumber<int>();
                        info.BaseCoopID = dtrCoop["BaseCoopID"].ToString().ToNumber<int>();
                        info.BaseOrder = dtrCoop["BaseOrder"].ToString().ToNumber<int>();
                        info.Subject = dtrCoop["Subject"].ToString();

                        string temphtml = @"<tr>
                                <td>
                                    <a onclick=""Common.CoopViewer('{1}')"">{0}</a>
                                </td>
                            </tr>";

                        tempBaseCoopHtml += string.Format(temphtml, info.Subject, info.BaseCoopID);
                    }
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

            //_statusLast 은 1:대기;2:접수;3:처리중;4:완료;5:반려
            //if (CurrentUserInfo.UserID == _writeUserID && _listOrder > 0 && (_statusLast == "1" || _statusLast == "2" || _statusLast == "3"))
            //{
            //    btnHtml = @"<li class='btn_blue'><a id='cbtnMod_{0}' onclick=""View.Mod('{1}','{2}')""><span>" + GetResource("ReplyMod") + @"</span></a></li>";
            //    btnHtml += @"<li class='btn_blue'><a id='cbtnDel_{0}' onclick=""View.Del('{1}','{2}')""><span>" + GetResource("ReplyDel") + @"</span></a></li>";
            //    btnHtml = string.Format(btnHtml, _listID.ToString(), _listID.ToString(), _groupID.ToString());
            //}

            if (_mobileContent.Length > 0 || _contents.IndexOf("<IMG") != -1)
            {
                string tmpContent = @"
                <tr>
                    <th>" + GetResource("Contents") + @"</th>
                    <td colspan='5' valign='top'><div style='overflow:auto;'>{0}</div></td>
                </tr>";

                _contents = string.Format(tmpContent, _contents);
            }
            else
            {
                _contents = "";
            }

            string statusStr = GetStatus(_status);

            if (_isAlarm == "1") { bodyHtml = bodyLogHtml; statusStr = ""; }

            bodyHtml = string.Format(
                bodyHtml,
                attchHtml,
                baseDocHtml,
                baseCoopHtml,
                _name,
                statusStr,
                _writeDate.ToShortDateString() + " " + _writeDate.ToShortTimeString(),
                btnHtml,
                _contents);

            return bodyHtml;
        }

        /// <summary>
        /// 작성자 : 이현수
        /// 프로세스 표시
        /// </summary>
        /// <param name="_status"></param>
        private void GetWorkProcess(string _status)
        {
            string htmlProcessStatus = "";
            switch (_status)
            {
                case "1":
                    htmlProcessStatus = "<div class=\"cp_flowr\">";
                    htmlProcessStatus += "<div class=\"flwetext\">";
                    htmlProcessStatus += "<ul>";
                    htmlProcessStatus += "<li>" + GetResource("StandBy") + GetResource("Step") + "</li>";
                    htmlProcessStatus += "<li>" + GetResource("Receipt") + GetResource("Step") + "</li>";
                    htmlProcessStatus += "<li>" + GetResource("Progress") + GetResource("Step") + "</li>";
                    htmlProcessStatus += "<li class=\"flwf\">" + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "</ul>";
                    htmlProcessStatus += "</div>";
                    htmlProcessStatus += "<div class=\"flwe\">";
                    htmlProcessStatus += "<ul>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["1"])) + "</li>";
                    htmlProcessStatus += "</ul>";
                    htmlProcessStatus += "</div>";
                    htmlProcessStatus += "</div>";
                    break;

                case "2":
                    htmlProcessStatus = "<div class=\"cp_flowi\">";
                    htmlProcessStatus += "<div class=\"flwetext\">";
                    htmlProcessStatus += "<ul>";
                    htmlProcessStatus += "<li>" + GetResource("StandBy") + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "<li>" + GetResource("Receipt") + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "<li>" + GetResource("Progress") + GetResource("Step") + "</li>";
                    htmlProcessStatus += "<li class=\"flwf\">" + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "</ul>";
                    htmlProcessStatus += "</div>";
                    htmlProcessStatus += "<div class=\"flwe\">";
                    htmlProcessStatus += "<ul>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["1"])) + "</li>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["2"])) + "</li>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["3"])) + "</li>";
                    htmlProcessStatus += "<li class=\"flwie\">" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["4"])) + "</li>";
                    htmlProcessStatus += "</ul>";
                    htmlProcessStatus += "</div>";
                    htmlProcessStatus += "</div>";
                    break;

                case "3":
                    htmlProcessStatus = "<div class=\"cp_flowi\">";
                    htmlProcessStatus += "<div class=\"flwetext\">";
                    htmlProcessStatus += "<ul>";
                    htmlProcessStatus += "<li>" + GetResource("StandBy") + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "<li>" + GetResource("Receipt") + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "<li>" + GetResource("Progress") + GetResource("Step") + "</li>";
                    htmlProcessStatus += "<li class=\"flwf\">" + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "</ul>";
                    htmlProcessStatus += "</div>";
                    htmlProcessStatus += "<div class=\"flwe\">";
                    htmlProcessStatus += "<ul>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["1"])) + "</li>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["2"])) + "</li>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["3"])) + "</li>";
                    htmlProcessStatus += "<li class=\"flwie\">" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["4"])) + "</li>";
                    htmlProcessStatus += "</ul>";
                    htmlProcessStatus += "</div>";
                    htmlProcessStatus += "</div>";
                    break;

                case "4":
                    htmlProcessStatus = "<div class=\"cp_flowe\">";
                    htmlProcessStatus += "<div class=\"flwetext\">";
                    htmlProcessStatus += "<ul>";
                    htmlProcessStatus += "<li>" + GetResource("StandBy") + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "<li>" + GetResource("Receipt") + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "<li>" + GetResource("Progress") + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "<li class=\"flwf\">" + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "</ul>";
                    htmlProcessStatus += "</div>";
                    htmlProcessStatus += "<div class=\"flwe\">";
                    htmlProcessStatus += "<ul>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["1"])) + "</li>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["2"])) + "</li>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["3"])) + "</li>";
                    htmlProcessStatus += "<li class=\"flwf\">" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["4"])) + "</li>";
                    htmlProcessStatus += "</ul>";
                    htmlProcessStatus += "</div>";
                    htmlProcessStatus += "</div>";
                    break;

                case "5":
                    htmlProcessStatus = "<div class=\"cp_flowre\">";
                    htmlProcessStatus += "<div class=\"flwetext\">";
                    htmlProcessStatus += "<ul>";
                    htmlProcessStatus += "<li>" + GetResource("StandBy") + GetResource("Complete") + "</li>";
                    htmlProcessStatus += "<li class=\"flwre\">" + GetResource("Reject") + "</li>";
                    htmlProcessStatus += "</ul>";
                    htmlProcessStatus += "</div>";
                    htmlProcessStatus += "<div class=\"flwe\">";
                    htmlProcessStatus += "<ul>";
                    htmlProcessStatus += "<li>" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["1"])) + "</li>";
                    htmlProcessStatus += "<li class=\"flwre\">" + DisplayProcessDate(Convert.ToDateTime(ProcessTime["5"])) + "</li>";
                    htmlProcessStatus += "</ul>";
                    htmlProcessStatus += "</div>";
                    htmlProcessStatus += "</div>";
                    break;
            }

            WorkProcess = htmlProcessStatus;
        }

        /// <summary>
        /// 작성자 : 이현수
        /// 진행 날짜 표시
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>
        private string DisplayProcessDate(DateTime _dt)
        {
            return (_dt.Year == 1) ? "&nbsp;" : String.Format(@"{0:MM}.{0:dd} {0:tt hh:mm}", _dt);
        }

        /// <summary>
        /// 작성자 : 이현수
        /// 작성/수정 날짜 비교
        /// </summary>
        /// <param name="_writeDate"></param>
        /// <param name="_modifyDate"></param>
        /// <returns></returns>
        private DateTime CompareDate(string _writeDate, string _modifyDate)
        {
            if (_modifyDate.ToNull() == "")
            {
                return Convert.ToDateTime(_writeDate);
            }
            else
            {
                return (Convert.ToDateTime(_writeDate) > Convert.ToDateTime(_modifyDate)) ? Convert.ToDateTime(_writeDate) : Convert.ToDateTime(_modifyDate);
            }
        }

        #endregion Method (함수)

        #region Property (프로퍼티)

        /// <summary>
        /// 업로드할 대상 경로
        /// </summary>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }

        /// <summary>
        /// ActiveX 유무
        /// </summary>
        public string NonActiveX
        {
            get { return GetViewStateString("NonActiveX", ""); }
            set { ViewState["NonActiveX"] = value; }
        }

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        /// WorkProcess
        /// </summary>
        public string WorkProcess
        {
            get { return GetViewStateString("WorkProcess", ""); }
            set { ViewState["WorkProcess"] = value; }
        }

        #endregion Property (프로퍼티)
    }
}