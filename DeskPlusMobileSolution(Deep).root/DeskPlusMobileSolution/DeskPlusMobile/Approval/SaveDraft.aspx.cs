using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlusMobile.Web.Helper.WebComponent.DateHelper;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlusMobile.Web.Approval
{
    public partial class SaveDraft : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.Approval.SaveDraft).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();

                LoadParam();
            }
        }

        #endregion

        #region Method (함수)
        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2019-07-25
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
        /// 추가
        /// </summary>
        /// <returns></returns>
        private bool Addition(bool _isMod)
        {
            int BeforeDraftID = tbDraftID.Text.ToNumber<int>();
            string BeforeYearMonth = tbWriteTimeYMD.Text;

            string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');

            Ws.BE.DraftInfo dInfo = new Ws.BE.DraftInfo();

            #region DP_APP_Draft
            dInfo.CompanyID = CompanyID.ToNumber<int>();
            dInfo.UserID = UserID.ToNumber<int>();
            dInfo.FormID = tbFormID.Text.ToNumber<int>();
            dInfo.Subject = tbDraftName.Value;

            string contHTML = GetContents();
            dInfo.ContentHTML = htaFormHTML.Value.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css").Replace("<ZEOEDITOR></ZEOEDITOR>", contHTML);
            dInfo.ContentHTML = dInfo.ContentHTML.Replace("Func_ApprovalEdit.css", "Func_ApprovalView.css").Replace("<zeoeditor></zeoeditor>", contHTML);
            dInfo.ContentTEXT = htaFormTEXT.Value;

            dInfo.AccessTitleID = tbAccessTitleID.Text.ToNumber<int>();
            dInfo.IsGuard = tbIsGuard.Text;
            dInfo.IsLinkedEtc = tbIsLinkedEtc.Text;
            dInfo.IsUseFlow = tbIsUseFlow.Text;
            dInfo.CarbonView = tbCarbonView.Text;
            dInfo.IsMobile = "1";
            #endregion

            #region DP_APP_DraftSequence (DP_APP_Seq / DP_APP_CounterPartUser / DP_APP_CarbonCopyUser)

            List<Ws.BE.DraftSequenceInfo> seqlist = new List<Ws.BE.DraftSequenceInfo>();
            List<Ws.BE.DraftSequenceInfo> seqlistZA = new List<Ws.BE.DraftSequenceInfo>();

            #region 발신객체 처리

            XmlDocument seqNode = new XmlDocument();
            seqNode.LoadXml(Request["htaXmlSeq"].ToString());

            int ZACnt = 0;
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
                            seqlist.Add(GetDraftSequenceInfo(node, ZACnt));              //첫번째 ZA 
                        }
                        else
                        {
                            seqlistZA.Add(GetDraftSequenceInfo(node, ZACnt));            //그외 ZA 
                        }
                    }
                    else
                    {
                        seqlist.Add(GetDraftSequenceInfo(node, seqlist.Count + 1));
                    }
                }
            }
            if (seqlistZA.Count > 0)
            {
                seqlist.AddRange(seqlistZA);
            }

            #endregion

            #region 수신객체 처리

            XmlDocument seqNodeZQ = new XmlDocument();
            seqNodeZQ.LoadXml(Request["htaXmlSeqZQ"].ToString());

            nodeList = seqNodeZQ.SelectNodes("ITEMS/ITEM");

            int zqCnt = 0;
            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["CLASS"] != null)
                {
                    seqlist.Add(GetDraftSequenceInfo(node, zqCnt++));
                }
            }

            #endregion

            #region 참조객체 처리

            XmlDocument seqNodeZC = new XmlDocument();
            seqNodeZQ.LoadXml(Request["htaXmlSeqZC"].ToString());

            nodeList = seqNodeZQ.SelectNodes("ITEMS/ITEM");

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes["CLASS"] != null)
                {
                    if (node.SelectSingleNode("SUBTREE").InnerText.Equals("1") && node.SelectSingleNode("ISGROUP").InnerText.ToUpper().Equals("G"))
                    {
                        // 부서지정 이하
                        int deptID = node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();
                        seqlist.Add(GetDraftCarbonCopyUserInfo(node.SelectSingleNode("APPROVALTAG").InnerText, 0, deptID, "G", "Y", "1"));
                    }
                    else
                    {
                        // 사용자, 부서지정
                        seqlist.Add(GetDraftCarbonCopyUserInfo(node));
                    }
                }
            }

            #endregion

            dInfo.DraftSequenceList = seqlist;

            #endregion

            #region DP_APP_DraftBaseDoc

            int baseOrder = 1;
            List<Ws.BE.DraftBaseDocInfo> baselist = new List<Ws.BE.DraftBaseDocInfo>();

            #region 이전 근거문서생성
            if (BeforeDraftID > 0)
            {
                Ws.Data.DraftBaseDoc baseDal = new Ws.Data.DraftBaseDoc();
                List<Ws.BE.DraftBaseDocInfo> beforeBaseList = baseDal.GetBaseDocLists(CompanyID.ToNumber<int>(), BeforeDraftID);
                foreach (Ws.BE.DraftBaseDocInfo info in beforeBaseList)
                {
                    Ws.BE.DraftBaseDocInfo baseInfo = new Ws.BE.DraftBaseDocInfo();

                    baseInfo.CompanyID = info.CompanyID;
                    baseInfo.ParentDocumentID = info.ParentDocumentID;
                    baseInfo.Name = info.Name;
                    baseInfo.IsLegacy = info.IsLegacy;
                    baseInfo.Path = info.Path;
                    baseInfo.BaseOrder = baseOrder;
                    baseOrder++;

                    baselist.Add(baseInfo);
                }
            }
            #endregion

            #region 근거문서생성
            if (!tbBaseDocIDs.Text.Trim().Equals(""))
            {
                string[] arrlist = tbBaseDocIDs.Text.Trim(';').Split(';');

                for (int i = 0; i < arrlist.Length; i++)
                {
                    Ws.BE.DraftBaseDocInfo baseInfo = new Ws.BE.DraftBaseDocInfo();

                    baseInfo.CompanyID = CompanyID.ToNumber<int>();
                    baseInfo.ParentDocumentID = arrlist[i].Replace("|", "").Split('^')[0].ToNumber<int>();
                    baseInfo.Name = arrlist[i].Replace("|", "").Split('^')[1];  // |docID^docNumber;|docID^docNumber;
                    baseInfo.IsLegacy = "N";    // 외부연동용도 (IsLegacy, Path)
                    baseInfo.Path = "";
                    baseInfo.BaseOrder = baseOrder;
                    baseOrder++;

                    baselist.Add(baseInfo);
                }
            }

            if (baselist.Count > 0)
            {
                dInfo.DraftBaseDocList = baselist;
            }
            #endregion

            #endregion

            #region DP_APP_DraftFile

            #region 이전 파일생성
            if (BeforeDraftID > 0)
            {
                Ws.Data.DraftFile attDal = new Ws.Data.DraftFile();

                List<Ws.BE.DraftFileInfo> beforeAttachList = attDal.GetAttachFileList(CompanyID.ToNumber<int>(), BeforeDraftID);
                if (beforeAttachList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (Ws.BE.DraftFileInfo info in beforeAttachList)
                    {
                        sb.Append("A>");
                        sb.Append(info.AttachFileID);
                        sb.Append(">");
                        sb.Append(info.FilePath.Replace("DeskPlusEIP", "DeskPlusMobile"));
                        sb.Append(">");
                        sb.Append(info.FileName);
                        sb.Append(">");
                        sb.Append(info.FileGUID);
                        sb.Append("|");
                    }

                    dInfo.BeforeHiddenPath = sb.ToString();
                }
            }
            #endregion

            #region 신규 파일생성
            string filePath = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");
            filePath = filePath + "/{0}/Users/{1}/Mobile/{2}/";
            dInfo.HiddenSpoolPath = string.Format(filePath, CompanyID, UserID, tbSwfUploadGUID.Text);
            #endregion

            #endregion


            Ws.BLL.Draft dBll = new Ws.BLL.Draft();
            bool rtnval = (!_isMod) ? dBll.Add(dInfo) : dBll.Mod(dInfo, tbDraftID.Text.ToNumber<int>(), tbWriteTimeYMD.Text);

            #region DOC_STREAM
            string docStreamYN = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Approval, "DocStreamYN");

            if (docStreamYN == "Y")
            {
                try
                {
                    Ws.Data.DraftFile dalFile = new Ws.Data.DraftFile();
                    DocStream ds = new DocStream();
                    var newDSList = dalFile.GetAttachFileList(dInfo.CompanyID, dInfo.DraftID);
                    for (int i = 0; i < newDSList.Count; i++)
                    {
                        ds.AddDocStream(newDSList[i].CompanyID, newDSList[i].AttachFileID, "APP_DRAFT");
                        ds.UpdateFileID(newDSList[i].CompanyID, newDSList[i].AttachFileID, "APP_DRAFT");
                    }
                }
                catch (Exception ex)
                {
                    Ws.Data.DraftFile dalFile = new Ws.Data.DraftFile();
                    DocStream ds = new DocStream();
                    var DSList = dalFile.GetAttachFileList(dInfo.CompanyID, dInfo.DraftID);
                    for (int i = 0; i < DSList.Count; i++)
                    {
                        ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachFileID, "APP_DRAFT");
                    }
                    AppException.WriteException("SaveDraft_Addition_DS : " + ex.Message);
                    rtnval = false;
                }

            }
            #endregion

            return rtnval;
        }

        #region 결재관련 객체처리
        /// <summary>
        /// 결재관련 객체처리 (ZD/ZH/ZA/ZQ/ZP/ZC)
        /// </summary>
        /// <param name="_node"></param>
        /// <returns></returns>
        private Ws.BE.DraftSequenceInfo GetDraftSequenceInfo(XmlNode _node, int _sequence)
        {
            Ws.BE.DraftSequenceInfo info = new Ws.BE.DraftSequenceInfo();
            info.CompanyID = CompanyID.ToNumber<int>();
            info.Sequence = _sequence;
            info.ApprovalTag = _node.SelectSingleNode("APPROVALTAG").InnerText;
            info.UserID = _node.SelectSingleNode("USERID").InnerText.ToNumber<int>();
            info.DeptID = _node.SelectSingleNode("DEPTID").InnerText.ToNumber<int>();
            info.IsGroup = "M";
            info.IsSet = "Y";
            info.IsSubTree = "0";

            return info;
        }
        /// <summary>
        /// 결재관련 객체처리 (ZC)
        /// </summary>
        /// <param name="_node"></param>
        /// <returns></returns>
        private Ws.BE.DraftSequenceInfo GetDraftCarbonCopyUserInfo(XmlNode _node)
        {
            Ws.BE.DraftSequenceInfo info = new Ws.BE.DraftSequenceInfo();
            info.CompanyID = CompanyID.ToNumber<int>();
            info.Sequence = 0;
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
        /// <param name="_node"></param>
        /// <returns></returns>
        private Ws.BE.DraftSequenceInfo GetDraftCarbonCopyUserInfo(string _approvalTag, int _userID, int _deptID, string _isGroup, string _isSet, string _isSubTree)
        {
            Ws.BE.DraftSequenceInfo info = new Ws.BE.DraftSequenceInfo();
            info.CompanyID = CompanyID.ToNumber<int>();
            info.Sequence = 0;
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

        #endregion

        #region Event Handler (이벤트 핸들러)
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
                string scriptArg = "";
                switch (EventArgs.ToUpper())
                {
                    case "ADD":
                        rtnval = Addition(false);
                        scriptArg = "SaveDraft.SubmitClose";
                        break;

                    case "MOD":
                        rtnval = Addition(true);
                        scriptArg = "SaveDraft.SubmitCloseReload";
                        break;
                }

                if (rtnval)
                {
                    Helper.WebComponent.Utility.WriteScript(Page, "success", scriptArg + "('" + this.GetResource("Complete") + "');");
                }
                else
                {
                    Helper.WebComponent.Utility.WriteScript(Page, "fail", "SaveDraft.SubmitClose('" + this.GetResource("ProcessingError") + "');");
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