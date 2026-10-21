using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using Zeospace.DeskPlusMobile.Web.Helper.Security;
using System.Xml;
using System.Text;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;

namespace Zeospace.DeskPlusMobile.Web.Approval
{
    public partial class Add : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        public int folderID = 0;
        public int formID = 0;
        public int RootDeptID = 0;
        public string RootDeptName = "";
        public string LoginID = "";
        public string UserName = "";
        public int DeptID = 0;
        public string DeptName = "";
        public int TitleID = 0;
        public string TitleName = "";
        public int DutyID = 0;
        public string DutyName = "";
        public string EmployeeNumber = "";
        public string UserMail = "";
        public string MobilePhone = "";
        public StringBuilder AccessTitleGrade = new StringBuilder("");
        public string blockExt;
        public string upFileSize;
        public string uPwd = "";
        #endregion

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.Add).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                Today = DateHelper.GetCultureDate(DateTime.Now, UserInfo.PreferredLanguage).Substring(0, 10);

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

        #endregion

        #region (함수)

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            folderID = GetDecryptParam("FolderID", "0").ToNumber<int>();
            formID = GetDecryptParam("FormID", "0").ToNumber<int>();

            uPwd = UserInfo.Password;
        }


        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-07-11
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
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            // 1. 기본정보 (인장/기본정보/최상위계열사)
            GetUserInfo();

            // 2. 양식로드 (신규양식-기본,자사,연동 / 임시저장양식)
            GetFormInfo();

            // 3. EDM 연동
            btEDM.Visible = false;
            btEDMV.Visible = false;
            if (IsLinked == "1")
            {
                if (IsLinkedType == "2" || IsLinkedType == "3")
                {
                    btEDM.Visible = true;
                    btEDMV.Visible = true;
                }
            }

            // 4. 첨부파일 
            SwfUploadGUID = System.Guid.NewGuid().ToString();
            string uploadPath = Server.MapPath("/DeskPlusMobile/FileData/" + CompanyID + "/Users/" + UserID + "/Mobile/");
            if (Directory.Exists(uploadPath)) Directory.Delete(uploadPath, true);

            blockExt = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "BlockExt");
            upFileSize = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "UpFileSize");

            // 5. 에디터 본문 이미지 첨부 경로
            System.Guid guid = System.Guid.NewGuid();
            string reqTime = FileHelper.GetYearMonth();
            string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");
            UploadPath = baseFileDirectory + "/" + CompanyID + "/Approval/Editor/" + reqTime + "/" + guid.ToString();
            Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로

            // 6. 설정정보 체크 (겸직/문서종류/열람권한)
            GetSettingInfo();

        }

        /// <summary>
        /// 사용자정보 가져오기 (인장포함)
        /// </summary>
        private void GetUserInfo()
        {
            LoginID = UserInfo.LoginID;
            UserName = UserInfo.Name;
            DeptID = UserInfo.DeptID;
            DeptName = UserInfo.DeptBase;
            TitleID = UserInfo.CodeTitleID;
            TitleName = UserInfo.TitleBase;
            DutyID = UserInfo.CodeDutyID;
            DutyName = UserInfo.DutyBase;
            EmployeeNumber = UserInfo.EmployeeNumber;
            UserMail = UserInfo.MailID + "@" + UserInfo.MailDomain;
            MobilePhone = UserInfo.MobilePhone;

            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_USER");
            proc = string.Format(proc, CompanyID, UserID);

            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetUserList", null);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string filePath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");
                    if (!String.IsNullOrEmpty(dr["SealImage"].ToString()))
                    {
                        string path = filePath + "/" + CompanyID.ToString() + "/Users/" + UserID.ToString() + "/Picture/" + dr["SealImage"].ToString();
                        if (File.Exists(Server.MapPath(path)))
                        {
                            SealPath = path;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 양식정보 가져오기
        /// </summary>
        private void GetFormInfo()
        {
            Ws.Data.Form dal = new Ws.Data.Form();

            DataSet ds = dal.GetForm(CompanyID.ToNumber<int>(), formID);
            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    IsLegacy = dr["IsLegacy"].ToString();
                    CustomFileName = dr["CustomFileName"].ToString();
                    IsAfterSubmitCall = dr["IsAfterSubmitCall"].ToString();

                    FormBase = dr["FormBase"].ToString();
                    FormKo = dr["FormKo"].ToString();
                    FormEn = dr["FormEn"].ToString();
                    FormJa = dr["FormJa"].ToString();
                    FormFr = dr["FormFr"].ToString();
                    FormZhcn = dr["FormZhcn"].ToString();
                    FormZhtw = dr["FormZhtw"].ToString();

                    IsLinked = dr["IsLinked"].ToString();
                    IsLinkedType = dr["IsLinkedType"].ToString();
                    IsLinkedEtc = dr["IsLinkedEtc"].ToString();

                    htaFormHTML.Value = dr["ContentHTML"].ToString().Replace("Func_ApprovalRegistForm.css", "Func_ApprovalEdit.css"); ;
                }
            }
        }


        /// <summary>
        /// 설정정보 체크 (겸직/문서종류/참조열람/열람권한)
        /// </summary>
        private void GetSettingInfo()
        {
            #region 겸직정보

            string sqlQueryUserInfo = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLAccount, "SEL_QRY_ACCOUNTDEPT");
            sqlQueryUserInfo = String.Format(sqlQueryUserInfo, CompanyID, UserID);

            DataSet dsUserInfo = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQueryUserInfo, "UserUserInfo", null);

            if (dsUserInfo.Tables[0].Rows.Count == 1)
            {
                divAddtional.Visible = false;

                // 최상위 부서 정보 가져오기
                Account.BLL.Dept dept = new Account.BLL.Dept();
                RootDeptID = dept.GetRootDeptID(CompanyID.ToNumber<int>(), UserInfo.DeptID, false);

                Account.Data.Dept dDal = new Account.Data.Dept();
                DataSet dsdept = dDal.GetDept(CompanyID.ToNumber<int>(), RootDeptID);
                foreach (DataRow drDept in dsdept.Tables[0].Rows)
                {
                    RootDeptName = this.GetLangName(
                    drDept["NameBase"].ToString().ToNull(), drDept["NameKo"].ToString().ToNull(),
                    drDept["NameEn"].ToString().ToNull(), drDept["NameJa"].ToString().ToNull(),
                    drDept["NameFr"].ToString().ToNull(), drDept["NameZhcn"].ToString().ToNull(),
                    drDept["NameZhtw"].ToString().ToNull(), PreLang);
                }
            }
            else
            {
                ddlAdditionDept.Items.Clear();

                int selIdx = 0;
                int i = 0;
                string deptname = "", titlename = "", dutyname = "";

                foreach (DataRow dr in dsUserInfo.Tables[0].Rows)
                {
                    deptname = this.GetLangName(
                        dr["DeptBase"].ToString().ToNull(), dr["DeptKo"].ToString().ToNull(),
                        dr["DeptEn"].ToString().ToNull(), dr["DeptJa"].ToString().ToNull(),
                        dr["DeptFr"].ToString().ToNull(), dr["DeptZhcn"].ToString().ToNull(),
                        dr["DeptZhtw"].ToString().ToNull(), PreLang);

                    titlename = this.GetLangName(
                        dr["TitleBase"].ToString().ToNull(), dr["TitleKo"].ToString().ToNull(),
                        dr["TitleEn"].ToString().ToNull(), dr["TitleJa"].ToString().ToNull(),
                        dr["TitleFr"].ToString().ToNull(), dr["TitleZhcn"].ToString().ToNull(),
                        dr["TitleZhtw"].ToString().ToNull(), PreLang);

                    dutyname = this.GetLangName(
                        dr["DutyBase"].ToString().ToNull(), dr["DutyKo"].ToString().ToNull(),
                        dr["DutyEn"].ToString().ToNull(), dr["DutyJa"].ToString().ToNull(),
                        dr["DutyFr"].ToString().ToNull(), dr["DutyZhcn"].ToString().ToNull(),
                        dr["DutyZhtw"].ToString().ToNull(), PreLang);

                    // 최상위 부서 정보 가져오기
                    Account.BLL.Dept dept = new Account.BLL.Dept();
                    int rootDeptID = dept.GetRootDeptID(CompanyID.ToNumber<int>(), dr["DeptID"].ToString().ToNumber<int>(), false);

                    string rootDeptName = "";
                    Account.Data.Dept dDal = new Account.Data.Dept();
                    DataSet dsdept = dDal.GetDept(CompanyID.ToNumber<int>(), rootDeptID);
                    foreach (DataRow drDept in dsdept.Tables[0].Rows)
                    {
                        rootDeptName = this.GetLangName(
                        drDept["NameBase"].ToString().ToNull(), drDept["NameKo"].ToString().ToNull(),
                        drDept["NameEn"].ToString().ToNull(), drDept["NameJa"].ToString().ToNull(),
                        drDept["NameFr"].ToString().ToNull(), drDept["NameZhcn"].ToString().ToNull(),
                        drDept["NameZhtw"].ToString().ToNull(), PreLang);
                    }

                    ListItem item = new ListItem(deptname + "|" + titlename
                       , dr["DeptID"].ToString() + "|" + deptname + "|" + dr["CodeTitleID"].ToString() + "|" + titlename + "|" +
                       dr["CodeDutyID"].ToString() + "|" + dutyname + "|" + rootDeptID + "|" + rootDeptName);

                    item.Attributes.Add("deptID", dr["DeptID"].ToString());

                    ddlAdditionDept.Items.Add(item);

                    if (dr["DeptID"].ToString() == UserInfo.DeptID.ToString())
                    {
                        selIdx = i;
                        RootDeptID = rootDeptID;
                        RootDeptName = rootDeptName;
                    }
                    i++;
                }

                selIdx = (ddlAdditionDept.Items[selIdx] != null) ? selIdx : 0;
                ddlAdditionDept.Items[selIdx].Selected = true;
            }

            #endregion

            #region 문서종류
            //보안문서여부 : 일반(0)/보안(1)/긴급(2)
            ddlGuard.Items.Clear();

            ddlGuard.Items.Add(new ListItem(this.resMng.GetString("Normal"), "0"));
            ddlGuard.Items.Add(new ListItem(this.resMng.GetString("Security"), "1"));
            ddlGuard.Items.Add(new ListItem(this.resMng.GetString("Emergency"), "2"));

            ddlGuard.SelectedIndex = 0;
            #endregion

            #region 참조열람
            ddlCarbonView.Items.Clear();
            ddlCarbonView.Items.Add(new ListItem(this.resMng.GetString("IsActiveTitY"), "1"));
            ddlCarbonView.Items.Add(new ListItem(this.resMng.GetString("IsActiveTitN"), "0"));
            ddlCarbonView.SelectedIndex = ddlCarbonView.Items.Count - 1;

            string proc = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLPortal, "SEL_QRY_ComEnv");
            proc = string.Format(proc, CompanyID, "APPROVAL", "ReportUserCarbonViewOption");

            DataSet dsCarbon = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, proc, "GetomEnv", null);

            if (dsCarbon != null)
            {
                foreach (DataRow dr in dsCarbon.Tables[0].Rows)
                {
                    if (dr["KeyValue"].ToString() == "1")
                        trCarbonView.Visible = true;
                    else
                        trCarbonView.Visible = false;
                }
            }

            #endregion

            #region 열람권한

            string accessBase = "";
            ddlAccessTitle.Items.Clear();

            Ws.Data.AccessGrade cDal = new Ws.Data.AccessGrade();

            DataSet ds = cDal.GetListIsAccess(CompanyID.ToNumber<int>(), "1");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ddlAccessTitle.Items.Add(new ListItem(GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                                                                  dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang) + resMng.GetString("AuthMore")
                                                      , dr["AccessID"].ToString()));

                AccessTitleGrade.Append("<tr id='grd_" + dr["AccessID"].ToString() + "'>");
                AccessTitleGrade.Append("<th>");
                AccessTitleGrade.Append(GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang));
                AccessTitleGrade.Append("</th>");
                AccessTitleGrade.Append("<td>");
                AccessTitleGrade.Append(GetLangName(dr["AccessGradeUser"].ToString().ToReverseXSS(), dr["AccessGradeUserKo"].ToString().ToReverseXSS(), dr["AccessGradeUserEn"].ToString().ToReverseXSS(), dr["AccessGradeUserJa"].ToString().ToReverseXSS(), dr["AccessGradeUserFr"].ToString().ToReverseXSS(), dr["AccessGradeUserZhcn"].ToString().ToReverseXSS(), dr["AccessGradeUserZhtw"].ToString().ToReverseXSS(), PreLang));
                AccessTitleGrade.Append("</td>");
                AccessTitleGrade.Append("</tr>");

                accessBase = (dr["IsBase"].ToString().Equals("1") && accessBase.Equals("")) ? dr["AccessID"].ToString() : accessBase;
            }

            // 기본값처리
            if (ddlAccessTitle.Items.FindByValue(accessBase) != null)
            {
                ddlAccessTitle.SelectedValue = accessBase;
            }
            else
            {
                ddlAccessTitle.SelectedIndex = ddlAccessTitle.Items.Count - 1;
            }

            #endregion
        }


        /// <summary>
        /// 추가
        /// </summary>
        /// <returns></returns>
        private int Addition()
        {
            string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');
            string isLast = "N", isNextZQ = "N";

            Ws.BE.DocumentProcessInfo dpInfo = new Ws.BE.DocumentProcessInfo();

            #region DP_APP_DocProcess (문서정보)

            dpInfo.CompanyID = CompanyID.ToNumber<int>();
            dpInfo.ParentDocumentID = 0;

            dpInfo.Number1 = tbDeptName.Text;
            dpInfo.Number2 = yearMonth;                 //상신년월
            dpInfo.Number3 = GetLangName(FormBase, FormKo, FormEn, FormJa, FormFr, FormZhcn, FormZhtw, PreLang);    //양식명

            dpInfo.DeptID = tbDeptID.Text.ToNumber<int>();
            dpInfo.EmployeeNumber = UserInfo.EmployeeNumber;
            dpInfo.UserID = UserInfo.UserID;

            dpInfo.LifeTime = tbLifeTime.Text;
            dpInfo.AccessTitleID = ddlAccessTitle.SelectedValue.ToNumber<int>();
            dpInfo.ApprovalState = "W";
            dpInfo.AccountTag = "ZD";
            dpInfo.Subject = (tbSubject.Text.Equals("")) ? dpInfo.Number3 : tbSubject.Text;     // 제목필드가 없으면 양식명으로 대체

            string contHTML = GetContents();
            dpInfo.ContentHTML = htaFormHTML.Value.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css").Replace("<ZEOEDITOR></ZEOEDITOR>", contHTML);
            dpInfo.ContentHTML = dpInfo.ContentHTML.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css").Replace("<zeoeditor></zeoeditor>", contHTML);
            dpInfo.ContentTEXT = htaFormTEXT.Value;

            dpInfo.IsLegacy = IsLegacy;
            dpInfo.CustomFileName = CustomFileName;
            dpInfo.IsAfterSubmitCall = IsAfterSubmitCall;
            dpInfo.IsDRM = IsDRM;
            dpInfo.IsGuard = ddlGuard.SelectedValue;
            dpInfo.IsLinkedEtc = tbIsLinkedEtc.Text;
            dpInfo.IsUseFlow = tbIsUseFlow.Text.TrimEnd(',');
            dpInfo.CarbonView = ddlCarbonView.SelectedValue;

            dpInfo.FormID = formID;
            dpInfo.GroupID = 0;

            dpInfo.Etc1 = tbEtc1.Text.Trim();
            dpInfo.Etc2 = tbEtc2.Text.Trim();
            dpInfo.Etc3 = tbEtc3.Text.Trim();
            dpInfo.Etc4 = tbEtc4.Text.Trim();
            dpInfo.Etc5 = tbEtc5.Text.Trim();
            dpInfo.Etc6 = tbEtc6.Text.Trim();
            dpInfo.Etc7 = tbEtc7.Text.Trim();
            dpInfo.Etc8 = tbEtc8.Text.Trim();
            dpInfo.Etc9 = tbEtc9.Text.Trim();
            dpInfo.Etc10 = tbEtc10.Text.Trim();
            dpInfo.IsMobile = "1";

            #endregion

            #region DP_APP_Seq / DP_APP_CounterPartUser / DP_APP_CarbonCopyUser (결재라인)

            List<Ws.BE.SequenceInfo> seqlist = new List<Ws.BE.SequenceInfo>();
            List<Ws.BE.SequenceInfo> seqlistZA = new List<Ws.BE.SequenceInfo>();
            List<Ws.BE.CounterPartUserInfo> cplist = new List<Ws.BE.CounterPartUserInfo>();
            List<Ws.BE.CarbonCopyUserInfo> carbonlist = new List<Ws.BE.CarbonCopyUserInfo>();

            #region 발신결재 - DP_APP_Seq (결재라인)

            XmlDocument seqNode = new XmlDocument();
            seqNode.LoadXml(Request["htaXmlSeq"].ToString());

            int ZACnt = 0, appCnt = 0;
            XmlNodeList nodeList = seqNode.SelectNodes("ITEMS/ITEM");

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["CLASS"] != null)
                {
                    if (node.Attributes["CLASS"].Value.ToUpper().Equals("ZA"))
                    {
                        if (ZACnt.Equals(0))
                        {
                            ZACnt = seqlist.Count + 1;
                            seqlist.Add(GetSequenceInfo(node, ZACnt));              //첫번째 ZA 
                        }
                        else
                        {
                            seqlistZA.Add(GetSequenceInfo(node, ZACnt));            //그외 ZA 
                        }
                    }
                    else
                    {
                        seqlist.Add(GetSequenceInfo(node, seqlist.Count + 1));
                    }
                    // For, 상신전결
                    if (node.Attributes["CLASS"].Value.IndexOf("ZR") < 0)
                    {
                        appCnt++;
                    }
                }
            }

            if (seqlistZA.Count > 0)
            {
                seqlist.AddRange(seqlistZA);
            }

            isLast = (appCnt.Equals(1)) ? "Y" : "N";

            #endregion

            #region 수신결재 (ZQ/ZP) - DP_APP_CounterPartUser (수신)
            XmlDocument seqNodeZQ = new XmlDocument();
            seqNodeZQ.LoadXml(Request["htaXmlSeqZQ"].ToString());

            nodeList = seqNodeZQ.SelectNodes("ITEMS/ITEM");

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["CLASS"] != null)
                {
                    cplist.Add(GetCounterPartUserInfo(node, cplist.Count + 1));

                    isNextZQ = (node.SelectSingleNode("APPROVALTAG").InnerText.ToUpper().IndexOf("ZQ").Equals(0)) ? "Y" : isNextZQ;
                }
            }
            #endregion

            #region 참조 (ZC) - DP_APP_CarbonCopyUser (참조)

            XmlDocument seqNodeZC = new XmlDocument();
            seqNodeZC.LoadXml(Request["htaXmlSeqZC"].ToString());

            nodeList = seqNodeZC.SelectNodes("ITEMS/ITEM");

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["CLASS"] != null)
                {
                    if (node.SelectSingleNode("SUBTREE").InnerText.Equals("1") && node.SelectSingleNode("ISGROUP").InnerText.ToUpper().Equals("G"))
                    {
                        // 부서지정 이하
                        int deptID = node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();
                        carbonlist.Add(GetCarbonCopyUserInfo(node.SelectSingleNode("APPROVALTAG").InnerText, 0, deptID, "G", "Y", "1"));
                    }
                    else
                    {
                        // 사용자, 부서지정
                        carbonlist.Add(GetCarbonCopyUserInfo(node));
                    }
                }
            }

            #endregion

            dpInfo.SequenceList = seqlist;
            dpInfo.CounterPartUserList = cplist;
            dpInfo.CarbonCopyUserList = carbonlist;

            #endregion

            #region DP_APP_BaseDoc (근거문서)
            if (!tbBaseDocIDs.Text.Trim().Equals(""))
            {
                List<Ws.BE.BaseDocumentInfo> baselist = new List<Ws.BE.BaseDocumentInfo>();
                string[] arrlist = tbBaseDocIDs.Text.Trim(';').Split(';');

                for (int i = 0; i < arrlist.Length; i++)
                {
                    Ws.BE.BaseDocumentInfo baseInfo = new Ws.BE.BaseDocumentInfo();
                    baseInfo.CompanyID = CompanyID.ToNumber<int>();
                    baseInfo.ParentDocumentID = arrlist[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                    baseInfo.Name = arrlist[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                    baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                    baseInfo.Path = "";
                    baseInfo.BaseOrder = i + 1;

                    baselist.Add(baseInfo);
                }
                dpInfo.BaseDocumentList = baselist;
            }
            #endregion

            #region DP_APP_Attach (첨부파일)

            string filePath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");
            filePath = filePath + "/{0}/Users/{1}/Mobile/{2}/";
            dpInfo.HiddenSpoolPath = string.Format(filePath, CompanyID, UserID, SwfUploadGUID);
            #endregion

            Ws.BE.AccountInfo sendInfo = GetSendUserInfo("S", memo.Value);
            Ws.BLL.DocumentProcess dpBll = new Ws.BLL.DocumentProcess();

            var rtnval = dpBll.Add(dpInfo, SealPath, sendInfo, isLast, isNextZQ);

            #region DOC_STREAM
            string docStreamYN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "DocStreamYN");

            if (docStreamYN == "Y")
            {
                try
                {
                    Ws.Data.AttachFile dalFile = new Ws.Data.AttachFile();
                    DocStream ds = new DocStream();
                    var newDSList = dalFile.GetAttachFileLists(dpInfo.CompanyID, dpInfo.DocumentID);
                    for (int i = 0; i < newDSList.Count; i++)
                    {
                        ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachFileID, "APP_ATTACH");
                        ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachFileID, "APP_ATTACH");
                    }
                }
                catch (Exception ex)
                {
                    Ws.Data.AttachFile dalFile = new Ws.Data.AttachFile();
                    DocStream ds = new DocStream();
                    var DSList = dalFile.GetAttachFileLists(dpInfo.CompanyID, dpInfo.DocumentID);
                    for (int i = 0; i < DSList.Count; i++)
                    {
                        ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachFileID, "APP_ATTACH");
                    }
                    AppException.WriteException("APPROVAL_ADD_DS : " + ex.Message);
                    rtnval = -1;
                }

            }
            #endregion

            return rtnval;
        }

        #region 결재관련 객체처리
        /// <summary>
        /// 결재관련 객체처리 (ZD/ZH/ZA)
        /// </summary>
        /// <param name="_node"></param>
        /// <returns></returns>
        private Ws.BE.SequenceInfo GetSequenceInfo(XmlNode _node, int _sequence)
        {
            Ws.BE.SequenceInfo info = new Ws.BE.SequenceInfo();

            info.CompanyID = CompanyID.ToNumber<int>();
            info.ApprovalTag = _node.SelectSingleNode("APPROVALTAG").InnerText;
            info.Sequence = (info.ApprovalTag.IndexOf("ZR").Equals(0)) ? 0 : _sequence;     //공람의 경우, Seq는 '0'이 됨
            info.UserID = _node.SelectSingleNode("USERID").InnerText.ToNumber<int>();
            info.DeptID = _node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();

            info.MyTurn = "N";
            info.ApprovalType = (_sequence.Equals(1)) ? "S" : "";
            info.Opinion = (_sequence.Equals(1)) ? this.memo.Value : "";
            info.AutoReadLaterON = "N";

            if (_sequence.Equals(1))
            {
                info.Proxy = "N";
                info.ProxyUserID = 0;
            }

            return info;
        }
        /// <summary>
        /// 결재관련 객체처리 (ZQ/ZP)
        /// </summary>
        /// <param name="_node"></param>
        /// <returns></returns>
        private Ws.BE.CounterPartUserInfo GetCounterPartUserInfo(XmlNode _node, int _sequence)
        {
            Ws.BE.CounterPartUserInfo info = new Ws.BE.CounterPartUserInfo();
            info.CompanyID = CompanyID.ToNumber<int>();
            info.Sequence = _sequence;
            info.ApprovalTag = _node.SelectSingleNode("APPROVALTAG").InnerText;
            info.UserID = _node.SelectSingleNode("USERID").InnerText.ToNumber<int>();
            info.DeptID = _node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();
            return info;
        }
        /// <summary>
        /// 결재관련 객체처리 (ZC)
        /// </summary>
        /// <param name="_node"></param>
        /// <returns></returns>
        private Ws.BE.CarbonCopyUserInfo GetCarbonCopyUserInfo(XmlNode _node)
        {
            Ws.BE.CarbonCopyUserInfo info = new Ws.BE.CarbonCopyUserInfo();
            info.CompanyID = CompanyID.ToNumber<int>();
            info.ApprovalTag = _node.SelectSingleNode("APPROVALTAG").InnerText;
            info.UserID = _node.SelectSingleNode("USERID").InnerText.ToNumber<int>();
            info.DeptID = _node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();
            info.IsGroup = _node.SelectSingleNode("ISGROUP").InnerText;
            info.IsSet = "Y";
            info.IsSubTree = "0";
            return info;
        }
        /// <summary>
        /// 결재관련 객체처리 (ZC)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_userID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_isGroup"></param>
        /// <returns></returns>
        private Ws.BE.CarbonCopyUserInfo GetCarbonCopyUserInfo(string _approvalTag, int _userID, int _deptID, string _isGroup, string _isSet, string _isSubTree)
        {
            Ws.BE.CarbonCopyUserInfo info = new Ws.BE.CarbonCopyUserInfo();
            info.CompanyID = CompanyID.ToNumber<int>();
            info.ApprovalTag = _approvalTag;
            info.UserID = _userID;
            info.DeptID = _deptID;
            info.IsGroup = _isGroup;
            info.IsSet = _isSet;
            info.IsSubTree = _isSubTree;
            return info;
        }
        #endregion

        /// <summary>
        /// 작성자 : 
        /// 본문 내용 및 본문 이미지(업로드) MIME 하여 본문 내용가져오기
        /// </summary>
        /// <returns></returns>
        private string GetContents()
        {
            string contents = htaEditorHTML.Value;
            return Regex.Replace(contents, "</html>\0", "</html>", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 사용자(알람용도:발송자정보,첨언포함)
        /// </summary>
        /// <param name="_approvalType"></param>
        /// <param name="_opinion"></param>
        /// <returns></returns>
        public Ws.BE.AccountInfo GetSendUserInfo(string _approvalType, string _opinion)
        {
            Ws.BE.AccountInfo info = new Ws.BE.AccountInfo();
            info.CompanyID = CompanyID.ToNumber<int>();
            info.LogInID = UserInfo.LoginID;
            info.UserID = UserID.ToNumber<int>();
            info.Name = UserInfo.Name;
            info.DeptID = UserInfo.DeptID;
            info.DeptName = UserInfo.DeptName;
            info.TitleName = UserInfo.TitleName;
            info.DutyName = UserInfo.DutyName;
            info.Mail = UserInfo.MailID + "@" + UserInfo.MailDomain;
            info.MobilePhone = UserInfo.MobilePhone;

            info.Opinion = _opinion;
            info.ApprovalType = _approvalType;
            info.PreLang = PreLang;

            return info;
        }

        #endregion

        #region (프로퍼티)

        public string Today { get; set; }

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        }

        /// <summary>
        /// 인장경로
        /// </summary>
        public string SealPath
        {
            get { return GetViewStateString("SealPath", ""); }
            set { ViewState["SealPath"] = value; }
        }

        /// <summary>
        /// 연동유무
        /// </summary>
        public string IsLegacy
        {
            get { return GetViewStateString("IsLegacy", ""); }
            set { ViewState["IsLegacy"] = value; }
        }
        /// <summary>
        /// 연동파일경로
        /// </summary>
        public string CustomFileName
        {
            get { return GetViewStateString("CustomFileName", ""); }
            set { ViewState["CustomFileName"] = value; }
        }
        /// <summary>
        /// 연동경로정보
        /// </summary>
        public string CustomFilePath
        {
            get
            {
                string customPath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "CustomScriptRoot");
                if (!customPath.Equals("") && !CustomFileName.Equals("") && IsLegacy.Equals("1"))
                {
                    customPath += CustomFileName;       //  /DeskPlusMobile/Legacy/aaa.aspx

                    if (!File.Exists(Server.MapPath(customPath)))
                    {
                        customPath = "";
                    }
                }
                else
                {
                    customPath = "";
                }
                return customPath;
            }
        }

        /// <summary>
        /// 연동파일-상신후호출 여부
        /// </summary>
        public string IsAfterSubmitCall
        {
            get { return GetViewStateString("IsAfterSubmitCall", ""); }
            set { ViewState["IsAfterSubmitCall"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FormBase
        {
            get { return GetViewStateString("FormBase", ""); }
            set { ViewState["FormBase"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormKo
        {
            get { return GetViewStateString("FormKo", ""); }
            set { ViewState["FormKo"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormEn
        {
            get { return GetViewStateString("FormEn", ""); }
            set { ViewState["FormEn"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormJa
        {
            get { return GetViewStateString("FormJa", ""); }
            set { ViewState["FormJa"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormFr
        {
            get { return GetViewStateString("FormFr", ""); }
            set { ViewState["FormFr"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormZhcn
        {
            get { return GetViewStateString("FormZhcn", ""); }
            set { ViewState["FormZhcn"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormZhtw
        {
            get { return GetViewStateString("FormZhtw", ""); }
            set { ViewState["FormZhtw"] = value; }
        }

        /// <summary>
        /// 에디터-파일처리경로
        /// </summary>
        public string UploadPath
        {
            get { return GetViewStateString("UploadPath", ""); }
            set { ViewState["UploadPath"] = value; }
        }

        /// <summary>
        /// DRM연동여부
        /// </summary>
        public string IsDRM
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "IsDRMUse");
            }
        }

        /// <summary>
        /// 이관사용여부
        /// </summary>
        public string IsLinked
        {
            get { return GetViewStateString("IsLinked", ""); }
            set { ViewState["IsLinked"] = value; }
        }

        /// <summary>
        /// 이관타입
        /// IsLinkedType 1:관리자지정(변경필요) 2:사용자(필수) 3 : 사용자(선택)
        /// </summary>
        public string IsLinkedType
        {
            get { return GetViewStateString("IsLinkedType", ""); }
            set { ViewState["IsLinkedType"] = value; }
        }

        /// <summary>
        /// 이관경로
        /// </summary>
        public string IsLinkedEtc
        {
            get { return GetViewStateString("IsLinkedEtc", ""); }
            set { ViewState["IsLinkedEtc"] = value; }
        }


        #endregion

        #region (이벤트핸들러)
        /// <summary>
        /// [확인][삭제]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool rtnval = false;
                int docID = 0;

                switch (EventArgs.ToUpper())
                {
                    case "ADD":
                        docID = Addition();
                        rtnval = (docID > 0) ? true : false;
                        break;
                }

                if (rtnval)
                {
                    string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');
                    //PDF생성
                    Ws.BLL.CreatePDF bll = new Ws.BLL.CreatePDF();
                    bll.ProcessPDF(CompanyID.ToNumber<int>(), UserID.ToNumber<int>(), docID, yearMonth, "1");

                    if (IsAfterSubmitCall.Equals("1"))
                    {
                        Helper.WebComponent.Utility.WriteScript(Page, "success", "SubmitAfterCall('" + this.CustomFilePath + "','" + docID.ToString() + "','" + this.GetResource("Complete") + "');");
                    }
                    else
                    {
                        Helper.WebComponent.Utility.WriteScript(Page, "success", "SubmitClose('" + this.GetResource("Complete") + "');");
                    }
                }
                else
                {
                    Helper.WebComponent.Utility.WriteScript(Page, "fail", "SubmitClose('" + this.GetResource("ProcessingError") + "');");
                }
            }
            catch (Exception ex)
            {
                AppException.WriteException("lbtnSubmit_Click", ex);
                Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("Error"), "");
            }
        }
        /// <summary>
        /// 들어오는 포스트백 이벤트를 처리하도록 포스트백을 발생시킨 서버 컨트롤에 알립니다.
        /// </summary>
        /// <param name="sourceControl">포스트백을 발생시킨 ASP.NET 서버 컨트롤입니다.이 컨트롤은 <see cref="T:System.Web.UI.IPostBackEventHandler"/> 인터페이스를 구현해야 합니다.</param>
        /// <param name="eventArgument">포스트백 인수입니다.</param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            LoadParam(); //파라미터처리

            EventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }
        #endregion
    }
}