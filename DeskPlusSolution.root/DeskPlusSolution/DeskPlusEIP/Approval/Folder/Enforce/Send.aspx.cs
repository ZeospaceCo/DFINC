using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Helpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.IO;
using System.Text.RegularExpressions;

namespace Zeospace.DeskPlus.Approval.Web.Folder.Enforce
{
    public partial class Send : Base.BasePageApproval
    {
        #region 변수설정
        public string EventArgs = string.Empty;

        public string CabinetID;
        public int EnforceID;    // 공문신청ID

        public string SendType = string.Empty;  // 발송형태 - 메일발송(0)/등기(1)/팩스(2)
        public string StampType = string.Empty; // 온라인직인(0)/오프라인직인(1)
        public string DocType = string.Empty;   // 문서형태 - 온라인(0)/오프라인(1)
        public string IsSend = string.Empty;    // 공문발송여부
        public string FormPath = string.Empty;    // 시행문템플릿경로

        public int EnforceStampID; //직인ID
        public string StampPath = string.Empty; //직인경로
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Folder.Enforce.Send).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadParam();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {
            LoadEnforceInfo(EnforceID);
            LoadStampInfo(EnforceStampID);
        }
        #endregion

        #region (함수)
        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            BaseParam();

            EnforceID = GetDecryptParam("enforceID", "0").ToNumber<int>();
            CabinetID = GetDecryptParam("cabinetID", "");
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
                CbtnPreviewStamp.Text = resMng.GetString("EnforcePreviewStamp");//직인미리보기
                CbtnMail.Text = resMng.GetString("EnforceMail");//메일발송
                CbtnPrintStamp.Text = resMng.GetString("EnforcePrintStamp");//출력(직인)
                CbtnPrint.Text = resMng.GetString("EnforcePrint");//출력(직인없음)
                CbtnCancel.Text = resMng.GetString("List");     //"취소";
                CbtnHistory.Text = resMng.GetString("EnforceHistory"); //공문발송내역
            }
        }

        /// <summary>
        /// 공문정보
        /// </summary>
        /// <param name="_documentID"></param>
        private void LoadEnforceInfo(int _enforceID)
        {
            if (_enforceID > 0)
            {
                DAL.Enforce dal = new DAL.Enforce();
                List<BE.EnforceInfo> list = dal.GetEnforce(CurrentUserInfo.CompanyID, _enforceID);
                if (list != null)
                {
                    foreach (BE.EnforceInfo info in list)
                    {
                        EnforceStampID = info.EnforceStampID;

                        SendType = info.SendType;   // 발송형태 - 메일발송(0)/등기(1)/팩스(2)
                        StampType = info.StampType; // 온라인직인(0)/오프라인직인(1)
                        DocType = info.DocType;     // 문서형태 - 온라인(0)/오프라인(1)
                        IsSend = info.IsSend;    // 공문발송여부
                        FormPath = info.FormPath;    // 시행문템플릿경로

                        tbReceive.Text = info.Receive;
                        tbReference.Text = info.Reference;
                        tbEnforceDate.Text = info.EnforceDate;
                        tbExpFirst.Text = info.ExpFirst;
                        tbExpMid.Text = info.ExpMid;
                        tbExpEnd.Text = info.ExpEnd;
                        tbZipCode.Text = info.ZipCode;
                        tbAddressFirst.Text = info.AddressFirst;
                        tbAddressEnd.Text = info.AddressEnd;
                        tbPhone.Text = info.Phone;
                        tbFax.Text = info.Fax;
                        tbHomepage.Text = info.Homepage;
                        tbTargetMail.Text = info.TargetMail;
                        tbTargetMailSubject.Text = info.TargetMailSubject;
                        
                        tbSubject.Text = info.Subject;
                        htaEditorHTML.Value = info.ContentHTML;

                        tbDocNumber.Text = info.DocNumber;
                        DateTime dt;
                        if (DateTime.TryParse(info.WriteDate.ToString(), out dt))
                        {
                            tbWriteTime.Text = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        }
                        if (DateTime.TryParse(info.CompleteDate.ToString(), out dt))
                        {
                            tbCompleteDate.Text = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        }
                    }
                }
            }
        }

        private void LoadStampInfo(int _stampID)
        {
            if (_stampID.Equals(0)) return;

            DAL.EnforceStamp dal = new DAL.EnforceStamp();
            List<BE.EnforceStampInfo> list = dal.GetEnforceStamp(CurrentUserInfo.CompanyID, _stampID);

            if (list != null)
            {
                foreach (BE.EnforceStampInfo info in list)
                {
                    StampPath = info.FilePath + info.FileName;
                }
            }

            if (File.Exists(Server.MapPath(StampPath)))
            {
                StampPath = FullDomainPath + StampPath;
            }
            else
            {
                StampPath = "";
            }
        }

        public bool SendMail()
        {
            if (tbTargetMail.Text.Equals("") || tbTargetMailSubject.Text.Equals("")) return false;

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); //메일 정규식

            string to = "";

            string [] arrTo = tbTargetMail.Text.TrimEnd(';').Split(';');
            foreach (string item in arrTo)
            {
                if (item.Split('@').Length > 1)
                {
                    Match match = regex.Match(item.Split('@')[0] + "@" + item.Split('@')[1]);
                    if (match.Success) to += item.Split('@')[0] + "@" + item.Split('@')[1] + ";";
                }
            }

            to = to.TrimEnd(';');

            string subject = tbTargetMailSubject.Text;
            string contents = htaFormHTML.Value;


            Zeospace.DeskPlus.Common.BLL.MailZettaFactory biz = new DeskPlus.Common.BLL.MailZettaFactory();
            biz.CreateMHT(CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain, CurrentUserInfo.Name, to,
                "", "", subject, contents, null, "", false);

            return true;
        }
        #endregion

        #region (프로퍼티)

        #endregion

        #region (이벤트핸들러)
        /// <summary>
        /// [메일발송]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool rtnval = false;
                switch (EventArgs.ToUpper())
                {
                    case "MAIL":
                        rtnval = SendMail();

                        if (rtnval)
                        {
                            Dictionary<string, string> parmDic = new Dictionary<string, string>();

                            parmDic.Add("currentPage", CurrentPage.ToString());
                            parmDic.Add("recordPerPage", RecordPerPage.ToString());
                            parmDic.Add("searchField", SearchField);
                            parmDic.Add("searchValue", SearchValue);
                            parmDic.Add("searchTerm", SearchTerm);
                            parmDic.Add("searchTermValue", SearchTermValue);
                            parmDic.Add("order", Order);
                            parmDic.Add("targetPage", TargetPage);
                            parmDic.Add("cabinetID", CabinetID);

                            parmDic.Add("alertType", AlertMsgEnum.Alert_Success.ToString());
                            parmDic.Add("alertMsg", GetResource("GoList"));
                            parmDic.Add("goURL", TargetPage);

                            string encParams = GetEncryptParam(Utility.GetParameterDic(parmDic));
                            Utility.Redirect(encParams);
                        }

                        break;
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
    }
}