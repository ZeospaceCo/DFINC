using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Text;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Approval.Web.FormFlow.Common
{
    public partial class SaveDraft : Base.BasePageApproval
    {
        #region 변수설정
        public string EventArgs = string.Empty;
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.FormFlow.Common.SaveDraft).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region Method (함수)
        private void LoadParam()
        {
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnConfirm.Text = this.GetResource("Confirm");
                cbtnCancel.Text = this.GetResource("Close");
            }
        }

        private void LoadData()
        {
            LoadParam();
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

            BE.DraftInfo dInfo = new BE.DraftInfo();

            #region DP_APP_Draft
            dInfo.CompanyID = CurrentUserInfo.CompanyID;
            dInfo.UserID = CurrentUserInfo.UserID;
            dInfo.FormID = tbFormID.Text.ToNumber<int>();
            dInfo.Subject = tbDraftName.Text;

            //string replaceChar = (tbNonActiveX.Text.Equals("F")) ? "<ZEOEDITOR></ZEOEDITOR>" : "<zeoeditor></zeoeditor>";
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

            List<BE.DraftSequenceInfo> seqlist = new List<BE.DraftSequenceInfo>();
            List<BE.DraftSequenceInfo> seqlistZA = new List<BE.DraftSequenceInfo>();

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
            List<BE.DraftBaseDocInfo> baselist = new List<BE.DraftBaseDocInfo>();

            #region 이전 근거문서생성
            if (BeforeDraftID > 0)
            {
                DAL.DraftBaseDoc baseDal = new DAL.DraftBaseDoc();
                List<BE.DraftBaseDocInfo> beforeBaseList = baseDal.GetBaseDocLists(CurrentUserInfo.CompanyID, BeforeDraftID);
                foreach (BE.DraftBaseDocInfo info in beforeBaseList)
                {
                    BE.DraftBaseDocInfo baseInfo = new BE.DraftBaseDocInfo();

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
                    BE.DraftBaseDocInfo baseInfo = new BE.DraftBaseDocInfo();

                    baseInfo.CompanyID = CurrentUserInfo.CompanyID;
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
                DAL.DraftFile attDal = new DAL.DraftFile();

                List<BE.DraftFileInfo> beforeAttachList = attDal.GetAttachFileList(CurrentUserInfo.CompanyID, BeforeDraftID);
                if (beforeAttachList.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (BE.DraftFileInfo info in beforeAttachList)
                    {
                        sb.Append("A>");
                        sb.Append(info.AttachFileID);
                        sb.Append(">");
                        sb.Append(info.FilePath);
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
            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            dInfo.HiddenSpoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tbSwfUploadGUID.Text);
            #endregion

            #endregion


            BLL.Draft dBll = new BLL.Draft();
            //bool rtnval = false;
            bool rtnval = (!_isMod) ? dBll.Add(dInfo) : dBll.Mod(dInfo, tbDraftID.Text.ToNumber<int>(), tbWriteTimeYMD.Text);

            #region DOC_STREAM
            var docStreamYN = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "DocStreamYN", "N");

            if (docStreamYN == "Y")
            {
                try
                {
                    DAL.DraftFile dalFile = new DAL.DraftFile();
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
                    DAL.DraftFile dalFile = new DAL.DraftFile();
                    DocStream ds = new DocStream();
                    var DSList = dalFile.GetAttachFileList(dInfo.CompanyID, dInfo.DraftID);
                    for (int i = 0; i < DSList.Count; i++)
                    {
                        ds.DelDocStream(DSList[i].CompanyID, DSList[i].AttachFileID, "APP_DRAFT");
                    }
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SaveDraft_Addition_DS : " + ex.Message);
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
        private BE.DraftSequenceInfo GetDraftSequenceInfo(XmlNode _node, int _sequence)
        {
            BE.DraftSequenceInfo info = new BE.DraftSequenceInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
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
        private BE.DraftSequenceInfo GetDraftCarbonCopyUserInfo(XmlNode _node)
        {
            BE.DraftSequenceInfo info = new BE.DraftSequenceInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
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
        private BE.DraftSequenceInfo GetDraftCarbonCopyUserInfo(string _approvalTag, int _userID, int _deptID, string _isGroup, string _isSet, string _isSubTree)
        {
            BE.DraftSequenceInfo info = new BE.DraftSequenceInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
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
            string contents = "";

            if (tbNonActiveX.Text == "F") //activex
            {
                string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                string serverName = baseHttp + AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                string filePath = FileHelper.GetServerMapPath(tbUploadPath.Text);
                string serverNameFolderName = serverName + tbUploadPath.Text;

                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                string contentValue = htaEditorMime.Text;

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass util
                    = new DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.MimeUtilClass();

                util.ProcessDecoding(contentValue, filePath, serverNameFolderName, 0);
                contents = util.DecodedHTML;

                string[] arrFile = Directory.GetFiles(filePath);
                if (arrFile.Length.Equals(0))
                {
                    Directory.Delete(filePath);
                }
            }
            else if (tbNonActiveX.Text == "T") //x-free
            {
                contents = htaEditorHTML.Value;
            }

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
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "success", scriptArg + "('" + this.GetResource("MSG_SUCCESS") + "');");
                }
                else
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.WriteScript(Page, "fail", "SaveDraft.SubmitClose('" + this.GetResource("MSG_FAIL") + "');");
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnSubmit_Click", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", this.GetResource("MSG_FAIL"), "");
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

        #region Property (프로퍼티)

        #endregion
    }
}