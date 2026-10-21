using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Task.Web.Home
{
    public partial class View : Base.BasePageTask
    {
        #region 변수설정

        public string BaseDocIDs;  //근거문서들 추가 순서대로의 documentID ex) |234;|454;|875; -> |ListBaseID;
        private string eventArgs = String.Empty;
        public string IsBase = "";
        public string IsPage = "";
        public string IsRETOCC = "";
        public int RecentPageNo = 0;
        public int ProcessPageNo = 0;
        public int DelayPageNo = 0;
        public int ComPageNo = 0;
        public int CCPageNo = 0;
        public int WriteCommentPageNo = 0;
        public int ImpoPageNo = 0;
        public string Year = "";

        #endregion 변수설정

        #region Page Process (페이지 처리)

        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드

            ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Home.View).Assembly;	//어셈블리 선언
            GetResourceManager();

            #endregion 다국어 리소스 로드

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();

                SwfUploadGUID = System.Guid.NewGuid().ToString();

                #region 에디터 본문 이미지 첨부 경로
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                System.Guid guid = System.Guid.NewGuid();
                string reqTime = System.DateTime.Now.ToString("yyyyMM");
                UploadPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/Task/" + reqTime + "/editor/" + guid.ToString();
                if (NonActiveX == "T") Session["XFEUpdatePath"] = UploadPath; //xfe 첨부파일 업로드 경로
                #endregion
            }
        }

        #endregion Page Process (페이지 처리)

        #region Data Load (컨트롤에 데이터를 바인딩 하는 메소드)

        private void LoadData()
        {
            //요청자, 수신자, 참조구분 가져오기
            LoadType();

            LoadTemplate();

            //데이터 가져오기
            LoadDataContent();

            #region 조회 추가

            BE.ListViewerInfo info = new BE.ListViewerInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ListID = GroupID;
            info.UserID = CurrentUserInfo.UserID;
            info.IsType = IsType;

            DAL.ListViewer dal = new DAL.ListViewer();
            dal.Add(info);

            BE.ForwardInfo finfo = new BE.ForwardInfo();
            finfo.CompanyID = CurrentUserInfo.CompanyID;
            finfo.ListID = GroupID;
            finfo.UserID = CurrentUserInfo.UserID;

            DAL.Forward fdal = new DAL.Forward();
            fdal.Mod(finfo);

            #endregion 조회 추가
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
                cbtnfileBackUp.Text = resMng.GetString("FileBackUp");
                cbtnReadList.Text = resMng.GetString("ReadUser");
                cbtnForwardList.Text = resMng.GetString("ForwardUser"); 
                cbtnPrint.Text = resMng.GetString("Print");
                cbtnOpen.Text = resMng.GetString("Reply");
                cbtnCompCancel.Text = resMng.GetString("CompCancel");
                cbtnComplete.Text = resMng.GetString("Completes");
                cbtnForward.Text = resMng.GetString("Forward");
                cbtnIngDetail.Text = resMng.GetString("ChangeSave");
                cbtnAccept.Text = resMng.GetString("MoveAccept");
                cbtnReCall.Text = resMng.GetString("MoveRequest");
                cbtnMod.Text = resMng.GetString("Mod");
                cbtnReject.Text = resMng.GetString("RejectBtn");
                cbtnDel.Text = resMng.GetString("Del");                
                cbtnList.Text = resMng.GetString("List");

                cbtnfileBackUp_F.Text = resMng.GetString("FileBackUp");
                cbtnReadList_F.Text = resMng.GetString("ReadUser");
                cbtnForwardList_F.Text = resMng.GetString("ForwardUser");  
                cbtnPrint_F.Text = resMng.GetString("Print");
                cbtnOpen_F.Text = resMng.GetString("Reply");
                cbtnCompCancel_F.Text = resMng.GetString("CompCancel");
                cbtnComplete_F.Text = resMng.GetString("Completes");
                cbtnForward_F.Text = resMng.GetString("Forward");
                cbtnIngDetail_F.Text = resMng.GetString("ChangeSave");
                cbtnAccept_F.Text = resMng.GetString("MoveAccept");
                cbtnReCall_F.Text = resMng.GetString("MoveRequest");
                cbtnMod_F.Text = resMng.GetString("Mod");
                cbtnReject_F.Text = resMng.GetString("RejectBtn");                                
                cbtnDel_F.Text = resMng.GetString("Del");
                cbtnList_F.Text = resMng.GetString("List");

                cbtnReceive.Text = resMng.GetString("AddReceive");
                cbtnReference.Text = resMng.GetString("AddReference");   

                cbtnClose.Text = resMng.GetString("Close");
                cbtnReply.Text = resMng.GetString("Btn_Save");
                //cbtnSwfFileAdd.Text = resMng.GetString("AddFile");

                //cbtnAddApp.Text = resMng.GetString("AddDoc");
                cbtnAddTask.Text = resMng.GetString("AddDoc");                                

                //cbtnDetailOpen.Text = "▼" + resMng.GetString("DetailOpen");
                //cbtnDetailClose.Text = "▲" + resMng.GetString("DetailClose");
                //cbtnDown.Text = "▼" + resMng.GetString("DownView");
            }
        }

        /// <summary>
        /// 파라미터 정의
        /// </summary>
        private void LoadParam()
        {
            BaseDocIDs = GetDecryptParam("baseDocIDs", "");
            IsPage = GetDecryptParam("isPage", "Main");
            RecentPageNo = GetDecryptParam("recentPageNo", "0").ToNumber<int>();
            ProcessPageNo = GetDecryptParam("processPageNo", "0").ToNumber<int>();
            DelayPageNo = GetDecryptParam("delayPageNo", "0").ToNumber<int>();            
            ComPageNo = GetDecryptParam("comPageNo", "0").ToNumber<int>();
            CCPageNo = GetDecryptParam("ccPageNo", "0").ToNumber<int>();
            WriteCommentPageNo = GetDecryptParam("writeCommentPageNo", "0").ToNumber<int>();
            ImpoPageNo = GetDecryptParam("impoPageNo", "0").ToNumber<int>();
            Year = GetDecryptParam("year", "");
        }

        /// <summary>
        /// 요청자, 수신자, 참조구분 가져오기
        /// </summary>
        private void LoadType()
        {
            IsType = "AL";
            tbType.Text = "AL";

            DAL.List dalList = new DAL.List();
            var dtr = dalList.GetTypeCnt(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, ListID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        if (dtr["CCCnt"].ToString().ToNumber<int>() > 0)
                        {
                            IsType = "CC";
                            tbType.Text = "CC";
                            IsRETOCC = "CC";
                        }

                        if (dtr["TOCnt"].ToString().ToNumber<int>() > 0)
                        {
                            IsType = "TO";
                            tbType.Text = "TO";
                            IsRETOCC += "TO";
                        }

                        if (dtr["RECnt"].ToString().ToNumber<int>() > 0)
                        {
                            IsType = "RE";
                            tbType.Text = "RE";
                            IsRETOCC += "RE";
                        }                       
                    }
                }
            }

            if (IsType == "AL")
            {
                //cbtnOpen.Visible = false; //답변하기                
                cbtnForward.Visible = false; //전달하기

                //cbtnOpen_F.Visible = false; //답변하기
                cbtnForward_F.Visible = false; //전달하기
            }
        }

        /// <summary>
        /// 템플릿 정보 가져오기
        /// </summary>
        private void LoadTemplate()
        {
            string templateHeader = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetEditorHtmlHeader(PreLang, "");
            string templateFooter = "</body></html>";

            tbHiddenMime_contents.Text = templateHeader + templateFooter;
            
            if (!Page.ClientScript.IsClientScriptBlockRegistered("OnControlInit"))
            {
                /*
                string strScript = String.Empty;
                if (Request.Path.IndexOf("View.aspx") > 0)
                {
                    strScript = "<script language='JScript' FOR='twe' EVENT='OnControlInit()'>twe.htmlValue = form1.tbHiddenMime_contents.value;" + "<" + "/script>";

                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "OnControlInit", strScript);
                    NonActiveX = "F";
                }
                else
                {
                    //스크립트에서 처리한다
                    NonActiveX = "T";
                }
                */
                NonActiveX = "T";
            }
        }
        
        /// <summary>
        /// 데이터 가져오기
        /// </summary>
        private void LoadDataContent()
        {
            cbtnComplete.Visible = false;
            cbtnCompCancel.Visible = false;
            cbtnIngDetail.Visible = false;
            cbtnAccept.Visible = false;
            cbtnReject.Visible = false;
            cbtnReCall.Visible = false;
            cbtnDel.Visible = false;
            cbtnMod.Visible = false;
            cbtnReceive.Visible = false; //수신자
            cbtnReference.Visible = false; //참조자

            cbtnComplete_F.Visible = false;
            cbtnCompCancel_F.Visible = false;
            cbtnIngDetail_F.Visible = false;
            cbtnAccept_F.Visible = false;
            cbtnReject_F.Visible = false;
            cbtnReCall_F.Visible = false;
            cbtnDel_F.Visible = false;
            cbtnMod_F.Visible = false;
            
            ltlHeaderStartDate.Text = GetResource("StartDate");
            ltlHeaderEndDate.Text = GetResource("EndDate");

            DAL.List dalList = new DAL.List();

            //1. 내용 가져오기
            var dtr = dalList.GetList(CurrentUserInfo.CompanyID, ListID.ToString());

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        ltlSubject.Text = dtr["Title"].ToString(); //제목 

                        // 요청자
                        string tDeptName = this.GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(),
                                 dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(),
                                 dtr["DeptZhtw"].ToString(), PreLang).ToReverseXSS();

                        string tTitle = this.GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(),
                                 dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(),
                                 dtr["TitleZhtw"].ToString(), PreLang).ToReverseXSS();

                        string tName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(),
                                   dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(),
                                   dtr["NameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ltlRequester.Text = string.Format(@"<a onclick=""AccountInfo('{1}');return false;"" style='cursor:pointer'>{0}</a>", tDeptName + " " + tTitle + " " + tName, dtr["WriteUserID"].ToString().ToNumber<int>());
                        
                        //상태 - 1:대기;2:접수;3:처리중;4:완료;5:재검토;6:지연
                        Status = dtr["StatusLast"].ToString();
                        ltlStatus.Text = GetStatus(dtr["StatusLast"].ToString());

                        ltlRequestDate.Text = DateHelper.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                        ltlCompleteRequestDate.Text = DateHelper.GetCultureDateChar8(dtr["CompleteRequestDate"].ToString(), PreLangReal);
                        ltlStartDate.Text = DateHelper.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                        ltlEndDate.Text = DateHelper.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);

                        ctbStartDate.Text = DateHelper.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                        ctbEndDate.Text = DateHelper.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);

                        StartDate = DateHelper.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                        EndDate = DateHelper.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);

                        ltlItem.Text = this.GetLangName(dtr["ItemNameBase"].ToString(), dtr["ItemNameKo"].ToString(),
                                    dtr["ItemNameEn"].ToString(), dtr["ItemNameJa"].ToString(), dtr["ItemNameFr"].ToString(),
                                    dtr["ItemNameZhcn"].ToString(), dtr["ItemNameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ltlItemSub.Text = this.GetLangName(dtr["ItemSubNameBase"].ToString(), dtr["ItemSubNameKo"].ToString(),
                                    dtr["ItemSubNameEn"].ToString(), dtr["ItemSubNameJa"].ToString(), dtr["ItemNameFr"].ToString(),
                                    dtr["ItemSubNameZhcn"].ToString(), dtr["ItemSubNameZhtw"].ToString(), PreLang).ToReverseXSS();

                        ltlIsClose.Text = (dtr["IsClose"].ToString() == "1") ? GetResource("IsClose1") : GetResource("IsClose2");
                        ltlIsGubun.Text = (dtr["IsGubun"].ToString() == "1") ? GetResource("IsGubun1") : GetResource("IsGubun2");

                        if (dtr["SourceID"].ToString() != "0")
                        {
                            ltlIsSource.Text = this.GetLangName(dtr["SNameBase"].ToString(), dtr["SNameKo"].ToString(), dtr["SNameEn"].ToString(),
                                        dtr["SNameJa"].ToString(), dtr["SNameFr"].ToString(), dtr["SNameZhcn"].ToString(),
                                        dtr["SNameZhtw"].ToString(), PreLang).ToReverseXSS();
                        }


                        ltlTaskNumber.Text = dtr["TaskNumber"].ToString();
                        ltlHistroy.Text = "";
                        lbContent.Text = dtr["Contents"].ToString();
                        
                        //대기일 경우 수정 및 삭제가 가능하다.
                        if (CurrentUserInfo.UserID == dtr["WriteUserID"].ToString().ToNumber<int>() && Status == "1" && IsType == "RE")
                        {
                            cbtnMod.Visible = true; 
                            cbtnDel.Visible = true;

                            cbtnMod_F.Visible = true;
                            cbtnDel_F.Visible = true; 
                        }

                        //재요청일 경우
                        if (CurrentUserInfo.UserID == dtr["WriteUserID"].ToString().ToNumber<int>() && Status == "5" && IsType == "RE")
                        {
                            cbtnReCall.Visible = true; 
                            cbtnDel.Visible = true;

                            cbtnReCall_F.Visible = true;
                            cbtnDel_F.Visible = true;
                        }

                        //완료일 경우
                        if ((Status == "4") && (IsType == "RE" || IsType == "TO"))
                        {
                            ltlHeaderStartDate.Text = GetResource("Start");
                            ltlHeaderEndDate.Text = GetResource("End");
                            cbtnCompCancel.Visible = true;
                            cbtnCompCancel_F.Visible = true;
                        }

                        if ((Status == "2" || Status == "3" || Status == "6") && (IsType == "RE" || IsType == "TO"))
                        {
                            cbtnComplete.Visible = true;
                            cbtnComplete_F.Visible = true;
                        }

                        //접수,처리중,지연 이면, 수신자 및 참조자 정보 변경
                        if ((Status == "2" || Status == "3" || Status == "6") && (IsRETOCC.IndexOf("TO") >= 0))
                        {
                            cbtnReceive.Visible = true; //수신자
                            cbtnReference.Visible = true; //참조자
                            cbtnIngDetail.Visible = true;//변경저장
                            cbtnIngDetail_F.Visible = true;//변경저장

                            ltlStartDate.Visible = false;
                            ltlEndDate.Visible = false;
                        }

                        //대기일 경우
                        if ((Status == "1") && (IsRETOCC.IndexOf("TO") >= 0))
                        {
                            cbtnAccept.Visible = true; //접수화면
                            cbtnReject.Visible = true; //대기시에만 재검토가 가능함.

                            cbtnAccept_F.Visible = true; //접수화면
                            cbtnReject_F.Visible = true; //대기시에만 재검토가 가능함.
                        }
                    }
                }
            }

            #region 수신자
            DAL.ListTO dalListTo = new DAL.ListTO();
            var listTo = dalListTo.GetListTo(CurrentUserInfo.CompanyID, ListID);
            string touID = "";
            string todID = "";
            string toname = "";
            string toisGroup = "";
            string toList = "";
            foreach (var info in listTo)
            {
                if (info != null)
                {
                    touID += info.UserID + ",";
                    todID += info.DeptID + ",";

                    string tDept = this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn,
                                info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang).ToReverseXSS();

                    string tTitle = this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn,
                                info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang).ToReverseXSS();

                    string tName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                                info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang).ToReverseXSS();

                    toname += tDept + "/" + tTitle + "/" + tName + ",";

                    toisGroup += "M" + ",";

                    toList += string.Format(@"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>, ", info.UserID, tDept + " " + tTitle + " " + tName);
                }

                ReceiveName = toisGroup + "‡" + touID + "‡" + todID + "‡" + toname.TrimEnd(',');
                ltlReceive.Text = toList.Trim().TrimEnd(',');

                if ((Status == "2" || Status == "3" || Status == "6") && (IsRETOCC.IndexOf("TO") >= 0))
                {
                    ltlReceive.Text = "";
                }
            }
            #endregion

            #region 참조자
            DAL.ListCC dalListCC = new DAL.ListCC();
            var listCC = dalListCC.GetListCC(CurrentUserInfo.CompanyID, ListID);
            string ccuID = "";
            string ccdID = "";
            string ccname = "";
            string ccisGroup = "";
            string ccList = "";
            foreach (var info in listCC)
            {
                if (info != null)
                {
                    ccuID += info.UserID + ",";
                    ccdID += info.DeptID + ",";

                    string tName = this.GetLangName(info.NameBase, info.NameKo, info.NameEn,
                                   info.NameJa, info.NameFr, info.NameZhcn, info.NameZhtw, PreLang).ToReverseXSS();

                    if (info.IsGroup == "M")
                    {
                        string tDept = this.GetLangName(info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa, info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang);
                        string tTitle = this.GetLangName(info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa, info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang);

                        ccname += tDept + "/" + tTitle + "/" + tName + ",";

                        ccList += string.Format(@"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>, ", info.UserID, tDept + " " + tTitle + " " + tName);
                    }
                    else
                    {
                        ccname += tName + ",";
                        ccList += tName + ",";
                    }

                    ccisGroup += info.IsGroup + ",";
                }

                ReferenceName = ccisGroup + "‡" + ccuID + "‡" + ccdID + "‡" + ccname.TrimEnd(',');
                ltlReference.Text = ccList.Trim().TrimEnd(',');

                if ((Status == "2" || Status == "3" || Status == "6") && (IsRETOCC.IndexOf("TO") >= 0))
                {
                    ltlReference.Text = "";
                }
            }
            #endregion 

            #region 첨부파일
            DAL.Attach dal = new DAL.Attach();
            List<BE.AttachInfo> attachInfo = dal.GetAttachList(CurrentUserInfo.CompanyID, ListID);

            if (attachInfo.Count == 0)
            {
                ltlAttach.Text = "";
                IsBase = @"style='display:none'";
            }
            else
            {
                string attHtml = "";
                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    _attachInfo.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : _attachInfo.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", _attachInfo.FilePath + _attachInfo.FileGUID);
                    paraDic.Add("fileName", _attachInfo.FileName);
                    _attachInfo.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    _attachInfo.FileType = _attachInfo.FileType.Replace(".", "");
                    _attachInfo.FileLength = _attachInfo.FileLength;
                    _attachInfo.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    string temphtml = @"<tr><td><a onclick=""javascript:View.FileDownload('{0}','{1}','{2}')"">{1}</a></td></tr>";
                    attHtml += string.Format(temphtml, _attachInfo.FilePath, _attachInfo.FileName, _attachInfo.FileGUID);
                }

                ltlAttach.Text = string.Format(@"<table border='0' class='att_css'>{0}</table>", attHtml);
            }
            #endregion 

            #region 전자결재 근거문서
            DAL.BaseApp appDal = new DAL.BaseApp();
            var dtrApp = appDal.GetBaseApp(CurrentUserInfo.CompanyID, ListID.ToString());

            string tmpHtml = "";

            if (dtrApp != null)
            {
                if (dtrApp.HasRows)
                {
                    while (dtrApp.Read())
                    {
                        string temphtml = @"<tr><td><a onclick=""Common.View('{1}')"">{0}</a></td></tr>";
                        tmpHtml += string.Format(temphtml, dtrApp["Name"].ToString().ToNull(), dtrApp["DocumentID"].ToString());
                    }
                }
            }

            if (tmpHtml.Length > 0)
            {
                ltlBase.Text = string.Format(@"<table border='0' class='att_css'>{0}</table>", tmpHtml);
                IsBase = "";
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
                        string temphtml = @"<tr><td><a onclick=""Common.TaskViewer('{1}')"">{0}</a></td></tr>";
                        tmpHtml += string.Format(temphtml, GetResource("Base1") + dtrTask["Name"].ToString(), dtrTask["BaseID"].ToString());
                    }
                }
            }

            if (tmpHtml.Length > 0)
            {
                ltlBase.Text += string.Format(@"<table border='0' class='att_css'>{0}</table>", tmpHtml);
                IsBase = "";
            }     
            */
            #endregion            

            #region 변경이력

            DAL.HistoryTO dalHTo = new DAL.HistoryTO();
            int tmpTo = dalHTo.GetHistoryCnt(CurrentUserInfo.CompanyID, ListID);

            if (tmpTo > 0)
            {
                ltlHistroy.Text = "<a onclick=\"Common.History('TO'," + ListID + "," + GroupID + ");return false;\" style='cursor:pointer'>" + GetResource("ChangeTO") + "</a>, ";
            }

            DAL.HistoryCC dalHCC = new DAL.HistoryCC();
            int tmpCC = dalHCC.GetHistoryCnt(CurrentUserInfo.CompanyID, ListID);

            if (tmpCC > 0)
            {
                ltlHistroy.Text += "<a onclick=\"Common.History('CC'," + ListID + "," + GroupID + ");return false;\" style='cursor:pointer'>" + GetResource("ChangeCC") + "</a>, ";
            }

            DAL.HistoryOption dalHOption = new DAL.HistoryOption();
            int tmpOption = dalHOption.GetHistoryCnt(CurrentUserInfo.CompanyID, ListID);

            if (tmpOption > 0)
            {
                ltlHistroy.Text += "<a onclick=\"Common.History('DATE'," + ListID + "," + GroupID + ");return false;\" style='cursor:pointer'>" + GetResource("ChangeDate") + "</a>";
            }

            ltlHistroy.Text = ltlHistroy.Text.Trim().TrimEnd(',');

            #endregion 

            // 상세내역
            LoadDataGroup();
        }

        /// <summary>
        /// 그룹별 내용 출력하기
        /// </summary>
        private void LoadDataGroup()
        {
            DAL.List dal = new DAL.List();
            var dtr = dal.GetGroupList(CurrentUserInfo.CompanyID, ListID);
            string replyHtml = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {   
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

            if (replyHtml.Length > 0)
            {
                //replyHtml += "<div style=\"text-align:right; padding-right:10px;\"><a href=\"#\">▲" + GetResource("TopView") + "</a></div>";
                replyHtml += "<div style='text-align:right; padding-right:10px;'><a href=\"#\" class=btn ><i class=\"fas fa-chevron-up mr-1\"></i>" + GetResource("TopView") + "</a></div>";
                pnContent.Controls.Add(new LiteralControl("<h1>" + GetResource("IngDetail") + "</h1>" + replyHtml));
            }
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
            string tmpName = @"<a onclick=""AccountInfo('{0}');return false;"" style='cursor:pointer'>{1}</a>";
            _name = string.Format(tmpName, _writeUserID, _name);

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
            bodyHtml = @"<div class='cw_Abox'>
                    <table cellspacing='0' class='" + bgColors + @"'>
                        <colgroup>
                            <col width='80px' /><col /><col width='80px' /><col width='170px' /><col width='70px' /><col width='170px' />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class=center>{8}</th>
                                <td>{3}</td><th class=center>" + GetResource("DateOfWrite") + @"</th><td>{5}</td><th class=center>" + GetResource("State") + @"</th><td>{4}{6}</td>
                            </tr>
                            {0}{1}{2}{7}
                        </tbody>
                    </table>
                </div>";

            attchHtml = @"<tr><th width='80' class=center>" + GetResource("AttachName") + @"</th>
                        <td colspan='5'><table border='0' class='att_css'>{0}</table></td></tr>";

            baseAppHtml = @"<tr><th width='80' class=center>" + GetResource("Base") + @"</th>
                    <td colspan='5'><table border='0' class='att_css'>{0}</table></td></tr>";

            //baseTaskHtml = @"<tr><th width='80'>" + GetResource("BaseTask") + @"</th><td colspan='5'><table border='0' class='att_css'>{0}</table></td></tr>";

            #endregion 템플릿 양식

            #region 첨부파일
            DAL.Attach dal = new DAL.Attach();
            List<BE.AttachInfo> attachInfo = dal.GetAttachList(_companyID, _listID);

            if (attachInfo.Count == 0)
            {
                attchHtml = "";
            }
            else
            {
                string attHtml = "";

                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    _attachInfo.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : _attachInfo.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", _attachInfo.FilePath + _attachInfo.FileGUID);
                    paraDic.Add("fileName", _attachInfo.FileName);
                    _attachInfo.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    _attachInfo.FileType = _attachInfo.FileType.Replace(".", "");
                    _attachInfo.FileLength = _attachInfo.FileLength;
                    _attachInfo.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    string temphtml = @"<tr><td><a onclick=""javascript:View.FileDownload('{0}','{1}','{2}')"">{1}</a></td></tr>";
                    attHtml += string.Format(temphtml, _attachInfo.FilePath, _attachInfo.FileName, _attachInfo.FileGUID);
                }

                attchHtml = string.Format(attchHtml, attHtml);
            }

            #endregion 

            #region 전자결재 근거문서
            DAL.BaseApp appDal = new DAL.BaseApp();
            var dtr = appDal.GetBaseApp(_companyID, _listID.ToString());

            string tempAppDocHtml = "";

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        string temphtml = @"<tr><td><a onclick=""Common.View('{1}')"">{0}</a></td></tr>";
                        tempAppDocHtml += string.Format(temphtml, dtr["Name"].ToString().ToNull(), dtr["DocumentID"].ToString());
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
            DAL.BaseTask baseTaskDal = new DAL.BaseTask();
            var dtrTask = baseTaskDal.GetBaseTask(_companyID, _listID.ToString());

            string tempBaseTaskHtml = "";

            if (dtrTask != null)
            {
                if (dtrTask.HasRows)
                {
                    while (dtrTask.Read())
                    {
                        string temphtml = @"<tr><td><a onclick=""Common.TaskViewer('{1}')"">{0}</a></td></tr>";
                        tempBaseTaskHtml += string.Format(temphtml, dtrTask["Name"].ToString(), dtrTask["BaseID"].ToString());
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
            if (CurrentUserInfo.UserID == _writeUserID && (_statusLast != "4") && _isMsg != "1")
            {
                btnHtml = @"<li><a id='cbtnMod_{0}' onclick=""View.Mod('{1}','{2}')"" class='btn btn-secondary'><span>" + GetResource("Mod") + @"</span></a></li>";
                btnHtml += @"<li><a id='cbtnDel_{0}' onclick=""View.Del('{1}','{2}')"" class='btn btn-danger'><span>" + GetResource("Del") + @"</span></a></li>";
                btnHtml = string.Format(btnHtml, _listID.ToString(), _listID.ToString(), _groupID.ToString());
                btnHtml = "<ul class='btn_right'>" + btnHtml + "</ul>";
            }

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
        /// 태그프리 에디터 설정
        /// </summary>
        /// <returns></returns>
        private string GetContents()
        {
            string contents = "";
            if (NonActiveX == "F") //activex
            {
                string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                string serverName = baseHttp + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                string filePath = FileHelper.GetServerMapPath(UploadPath);
                string serverNameFolderName = serverName + UploadPath;

                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                string contentValue = tbHiddenMime_contents.Text;

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass util
                    = new DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass();

                util.ProcessDecoding(contentValue, filePath, serverNameFolderName, 0);
                contents = util.DecodedHTML;
            }
            else if (NonActiveX == "T") //x-free
            {
                contents = tbHiddenHTML_contents.Value;
            }

            return contents;
        }

        /// <summary>
        /// 답변하기
        /// </summary>
        private void Reply()
        {
            string content = GetContents();
                 
            BE.ListInfo be = new BE.ListInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ListID = ListID;            
            be.GroupID = GroupID;
            be.Title = "";
            be.Contents = content;
            be.ContentText = tbHiddenText_contents.Text.Trim();
            be.Status = Status;
            be.IsMsg = "0";
            //be.IsType = IsType;
            be.IsType = tbType.Text;
            be.WriteUserID = CurrentUserInfo.UserID;            
            be.SenderName = CurrentUserInfo.Name;
            be.PreLang = PreLang;

            // 첨부파일
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            be.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID);
                        
            // 전자결재 근거문서 |고유id^문서번호;|고유id^문서번호;|고유id^문서번호;
            string[] arrAppList = tbBaseApps.Text.Trim(';').Length > 0 ? tbBaseApps.Text.Trim(';').Split(';') : null;

            // 업무협업 근거문서 |고유id;|고유id;|고유id;
            string[] arrTaskList = tbBaseTasks.Text.Trim(';').Length > 0 ? tbBaseTasks.Text.Trim(';').Split(';') : null;

            BLL.List dal = new BLL.List();
            int tmplistID = dal.Reply(be, arrAppList, arrTaskList);

            if (tmplistID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();                          
                paraDic.Add("listID", ListID.ToString());
                paraDic.Add("groupID", GroupID.ToString());
                paraDic.Add("isPage", IsPage.ToString());
                paraDic.Add("year", Year);
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("searchValue", SearchValue);  

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));

                if (Request.Url.AbsolutePath.IndexOf("View.aspx") != -1)
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Home/View.aspx");
                else
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Home/ViewN.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 삭제하기
        /// </summary>
        private void Del()
        {
            BLL.List biz = new BLL.List();
            if (biz.DelGroup(CurrentUserInfo.CompanyID, GroupID))
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("reLoad", "1");
                paraDic.Add("year", Year);
                paraDic.Add("searchValue", SearchValue);       

                paraDic.Add("alertType", AlertMsgEnum.Alert_Delete.ToString());
                paraDic.Add("alertMsg", GetResource("GoToList"));

                paraDic.Add("goURL", "/DeskplusEIP/Task/Home/" + IsPage + ".aspx");                 

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 완료취소
        /// </summary>
        private void CompCancel()
        {
            BE.ListInfo be = new BE.ListInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ListID = ListID;
            be.GroupID = GroupID;
            be.Title = "";
            be.Contents = GetResource("MSGCancel");
            be.ContentText = GetResource("MSGCancel");

            //종료(예정)일이 현재날짜보다 작으면 지연으로 처리.
            int ymd = System.DateTime.Now.ToString("yyyyMMdd").ToNumber<int>();
            int endDate = DateHelper.GetCultureDate(ltlEndDate.Text, "yyyyMMdd", PreLangReal).ToNumber<int>();
            if (endDate >= ymd)
            {
                be.Status = "3"; //처리중
            }
            else
            {
                be.Status = "6"; //지연
            }    
  
            be.IsMsg = "1";
            //be.IsType = IsType;
            be.IsType = tbType.Text;
            be.WriteUserID = CurrentUserInfo.UserID;            
            be.SenderName = CurrentUserInfo.Name;
            be.PreLang = PreLang;

            #region 옵션

            BE.ListOptionInfo loInfo = new BE.ListOptionInfo();
            loInfo.CompanyID = CurrentUserInfo.CompanyID;
            loInfo.ListID = ListID;
            loInfo.Status = be.Status;            
            loInfo.ModifyUserID = CurrentUserInfo.UserID;

            #endregion          

            BLL.List dal = new BLL.List();
            int tmplistID = dal.CompCancel(be, loInfo);

            if (tmplistID > 0)
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();          
                paraDic.Add("listID", ListID.ToString());
                paraDic.Add("groupID", GroupID.ToString());
                paraDic.Add("isPage", IsPage.ToString());
                paraDic.Add("year", Year);
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("searchValue", SearchValue);  

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));

                if (Request.Url.AbsolutePath.IndexOf("View.aspx") != -1)
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Home/View.aspx");
                else
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Home/ViewN.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
            }
        }

        /// <summary>
        /// 재검토하기
        /// </summary>
        private void Reject()
        {
            BE.ListInfo be = new BE.ListInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ListID = ListID;
            be.GroupID = GroupID;
            be.Title = "";
            be.Contents = tbOpinion.Text.Trim();
            be.ContentText = tbOpinion.Text.Trim();
            be.MsgContents = GetResource("MSGReject");
            be.Status = "5";
            be.IsMsg = "1";
            be.IsType = "TO";
            be.WriteUserID = CurrentUserInfo.UserID;
            be.SenderName = CurrentUserInfo.Name;
            be.PreLang = PreLang;

            #region 옵션

            BE.ListOptionInfo loInfo = new BE.ListOptionInfo();
            loInfo.CompanyID = CurrentUserInfo.CompanyID;
            loInfo.ListID = ListID;
            loInfo.Status = "5";
            loInfo.ModifyUserID = CurrentUserInfo.UserID;

            #endregion

            BLL.List dal = new BLL.List();

            if (dal.Reject(be, loInfo))
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("listID", ListID.ToString());
                paraDic.Add("groupID", GroupID.ToString());
                paraDic.Add("isPage", IsPage.ToString());
                paraDic.Add("year", Year);
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("searchValue", SearchValue);  

                paraDic.Add("alertType", AlertMsgEnum.Alert_Register.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));

                if (Request.Url.AbsolutePath.IndexOf("View.aspx") != -1)
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Home/View.aspx");
                else
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Home/ViewN.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);

            }
        }

        /// <summary>
        /// 변경저장
        /// </summary>
        private void IngDetailSave()
        {
            string tmpDate = "";
            string content = "";

            int startDate = DateHelper.GetCultureDate(Request[this.ctbStartDate.ClientID].ToString(), "yyyyMMdd", PreLangReal).ToNumber<int>();
            int endDate = DateHelper.GetCultureDate(Request[this.ctbEndDate.ClientID].ToString(), "yyyyMMdd", PreLangReal).ToNumber<int>();

            if (tbTO.Text == "Y")
            {
                content += "<span style='color:red'>▶</span> [" + GetResource("Receiver") + "]<br>";
                content += "(" + GetResource("BeforeChange") + ") " + ReceiveName.Split('‡')[3].TrimEnd(',') + "<br>";
                content += "(" + GetResource("AfterChange") + ") " + hdReceiveName.Value.Trim() + " <br>";
            }

            if (tbCC.Text == "Y")
            {
                content += "<span style='color:red'>▶</span> [" + GetResource("Referencer") + "]<br>";

                string tmpName = "";
                if (ReferenceName != "")
                {
                    tmpName = ReferenceName.Split('‡')[3].TrimEnd(',');
                }
                content += "(" + GetResource("BeforeChange") + ") " + tmpName + "<br>";
                content += "(" + GetResource("AfterChange") + ") " + hdReferenceName.Value.Trim() + " <br>";
            }

            if (tbStartDate.Text == "Y")
            {
                tmpDate = "Y";
                content += "<span style='color:red'>▶</span> [" + GetResource("StartDate") + "]:(" + GetResource("BeforeChange") + ") " + StartDate + " -> (" + GetResource("AfterChange") + ") " + startDate + " <br>";
            }

            if (tbEndDate.Text == "Y")
            {
                tmpDate = "Y";
                content += "<span style='color:red'>▶</span> [" + GetResource("EndDate") + "]:(" + GetResource("BeforeChange") + ") " + EndDate + " -> (" + GetResource("AfterChange") + ") " + endDate + " <br>";
            }


            BE.ListInfo be = new BE.ListInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.ListID = ListID;
            be.GroupID = GroupID;
            be.Title = "";
            be.MsgContents = content;
            be.ContentText = content;
            be.ToUserList = hdReceiveID.Value.Trim();
            be.CCUserList = hdReferenceID.Value.Trim();

            //종료(예정)일이 현재날짜보다 작으면 지연으로 처리.
            int ymd = System.DateTime.Now.ToString("yyyyMMdd").ToNumber<int>();
            if (endDate >= ymd)
            {
                if (startDate <= ymd)
                {
                    be.Status = "3";
                }
                else
                {
                    be.Status = "2";
                }
            }
            else
            {
                be.Status = "6"; //지연
            }

            be.IsMsg = "1";
            be.IsType = "TO";
            be.WriteUserID = CurrentUserInfo.UserID;
            be.SenderName = CurrentUserInfo.Name;
            be.PreLang = PreLang;

            #region 옵션
            BE.ListOptionInfo loInfo = new BE.ListOptionInfo();
            loInfo.CompanyID = be.CompanyID;
            loInfo.ListID = ListID;
            loInfo.StartDate = startDate.ToString();
            loInfo.EndDate = endDate.ToString();
            loInfo.Status = be.Status;
            loInfo.ModifyUserID = CurrentUserInfo.UserID;
            #endregion

            BLL.List dal = new BLL.List();

            if (dal.IngDetail(be, loInfo, tbTO.Text, tbCC.Text, tmpDate))
            {
                Dictionary<string, string> paraDic = new Dictionary<string, string>();              
                paraDic.Add("listID", ListID.ToString());
                paraDic.Add("groupID", GroupID.ToString());
                paraDic.Add("isPage", IsPage.ToString());
                paraDic.Add("year", Year);
                paraDic.Add("currentPage", CurrentPage.ToString());
                paraDic.Add("searchValue", SearchValue);  

                paraDic.Add("alertType", AlertMsgEnum.Alert_Modify.ToString());
                paraDic.Add("alertMsg", GetResource("GoList"));

                if (Request.Url.AbsolutePath.IndexOf("View.aspx") != -1)
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Home/View.aspx");
                else
                    paraDic.Add("goURL", "/DeskplusEIP/Task/Home/ViewN.aspx");

                string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                Utility.Redirect(encParams);
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
        /// Status
        /// </summary>
        public string Status
        {
            get { return GetViewStateString("Status", ""); }
            set { ViewState["Status"] = value; }
        }

        /// <summary>
        /// 수신자
        /// </summary>
        /// <value>The name of the charger.</value>
        public string ReceiveName
        {
            get { return GetViewStateString("ReceiveName", ""); }

            set { ViewState["ReceiveName"] = value; }
        }

        /// <summary>
        /// 참조자
        /// </summary>
        /// <value>The name of the linked dept.</value>
        public string ReferenceName
        {
            get { return GetViewStateString("ReferenceName", ""); }

            set { ViewState["ReferenceName"] = value; }
        }

        /// <summary>
        /// 시작일 
        /// </summary>
        /// <value>The name of the charger.</value>
        public string StartDate
        {
            get { return GetViewStateString("StartDate", ""); }

            set { ViewState["StartDate"] = value; }
        }

        /// <summary>
        /// 종료일
        /// </summary>
        /// <value>The name of the linked dept.</value>
        public string EndDate
        {
            get { return GetViewStateString("EndDate", ""); }

            set { ViewState["EndDate"] = value; }
        }

        #endregion Property (프로퍼티)

        #region Event Handler (이벤트 핸들러)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LBTNProcess_Click(object sender, EventArgs e)
        {
            try
            {
                LoadParam();
                string args = eventArgs;
                switch (args)
                {
                    case "REPLY":  //답변하기
                        Reply();
                        break;

                    case "DEL":  //삭제
                        Del();
                        break;

                    case "COMPCANCEL":  //완료취소
                        CompCancel();
                        break;

                    case "REJECT":  //재검토
                        Reject();
                        break;

                    case "IngDetailSave":  //수신,참조,시작일,종료일 변경 저장하기
                        IngDetailSave();
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("LBTNProcess_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("MSG_FAIL"), "");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceControl"></param>
        /// <param name="eventArgument"></param>
        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            eventArgs = eventArgument;
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

        #endregion Event Handler (이벤트 핸들러)
    }
}