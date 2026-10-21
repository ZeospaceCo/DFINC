using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Status
{
    public partial class Default : Base.BasePageApproval
    {
        #region 변수설정
        public string AccActive = "0"; //accordianActive

        public string FolderStatusID = "";
        public int ProxyUserID = 0;
        public int TargetUserID = 0;

        public string LeftURL = "";
        public string ContentURL = ""; 
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Status.Default).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        #endregion

        #region (함수)

        /// <summary>
        /// 초기화
        /// </summary>
        private void LoadData()
        {
            LoadParam();
            if (IsLegacyMessengerApprovalStatusLink())
            {
                Response.Redirect("DefaultAll.aspx" + Request.Url.Query, false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            GetDefaultMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            GetLeftURL();
            GetContentURL(FolderStatusID);
        }

        private bool IsLegacyMessengerApprovalStatusLink()
        {
            string folderStatusID = Request.QueryString["folderStatusID"];
            if (string.IsNullOrEmpty(folderStatusID)) return false;

            switch (folderStatusID.ToUpper())
            {
                case "RECEIVE":
                case "REJECT":
                case "APPROVE":
                case "TRANSFER":
                    break;
                default:
                    return false;
            }

            Account.DAL.UserEnvironment bizEnv = new Account.DAL.UserEnvironment();
            var alEnv = bizEnv.GetUserEnv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            return alEnv != null && alEnv.Count > 0 &&
                string.Equals(alEnv[0].ApprovalStatusMenuType, "NEW", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 작성자 : 
        /// 작성일자 : 
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            FolderStatusID = GetDecryptParam("folderStatusID", "");
            if (FolderStatusID == "")
            {
                //다이렉트 호출 시 사용
                FolderStatusID = GetParm("folderStatusID", "");
            }
            ProxyUserID = GetDecryptParam("proxyUserID", "0").ToNumber<int>();
        }

        //결재상황조회 초기메뉴값 가져오기
        private void GetDefaultMenu(int _companyID, int _userID)
        {
            if (FolderStatusID == "")
            {
                Account.DAL.UserEnvironment bizEnv = new Account.DAL.UserEnvironment();
                var alEnv = bizEnv.GetUserEnv(_companyID, _userID);

                FolderStatusID = alEnv[0].ApprovalStatusMenu;
                if (FolderStatusID.Length == 0) FolderStatusID = "Process";
            }
        }

        //Left URL
        private void GetLeftURL()
        {
            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("accActive", AccActive);

            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);

            paraDic.Add("folderStatusID", FolderStatusID);
            paraDic.Add("proxyUserID", ProxyUserID.ToString());

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            LeftURL = "./Menu/Tree.aspx?p=" + encParams;
        }

        // Content URL
        private void GetContentURL(string _menuID)
        {
            string listPage = "./ListProcess.aspx";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);

            switch (_menuID)
            {
                case "Report":          //상신문서
                    listPage = "./ListProcess.aspx";
                    paraDic.Add("folderStatusID", "Process");

                    break;
                case "Process":         //진행중인문서
                    listPage = "./ListProcess.aspx";
                    paraDic.Add("folderStatusID", "Process");

                    break;
                case "Complete":        //완결문서
                    listPage = "./ListComplete.aspx";
                    paraDic.Add("folderStatusID", "Complete");

                    break;
                case "Related":         //관계 문서
                    listPage = "./ListRelated.aspx";
                    paraDic.Add("folderStatusID", "Related");

                    break;
                case "Target":          //결재 진행
                    listPage = "./ListApprove.aspx";
                    paraDic.Add("folderStatusID", "Approve");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());

                    break;
                case "Approve":         //결재 대상
                    listPage = "./ListApprove.aspx";
                    paraDic.Add("folderStatusID", "Approve");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());

                    break;
                case "Delay":           //보류 문서
                    listPage = "./ListDelay.aspx";
                    paraDic.Add("folderStatusID", "Delay");
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());

                    break;
                case "Charge":          //담당자 결재 문서
                    listPage = "./ListReceive.aspx";
                    paraDic.Add("folderStatusID", "Receive");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());

                    break;
                case "Receive":         //수신 문서
                    listPage = "./ListReceive.aspx";
                    paraDic.Add("folderStatusID", "Receive");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    //paraDic.Add("dTargetUserID", TargetUserID.ToString());    //???

                    break;
                case "ReceiveDept":     //부서 수신 문서
                    listPage = "./ListReceiveDept.aspx";
                    paraDic.Add("folderStatusID", "ReceiveDept");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    //paraDic.Add("dTargetUserID", TargetUserID.ToString());    //???

                    break;
                case "Transfer":        //이관
                    listPage = "./ListTransfer.aspx";
                    paraDic.Add("folderStatusID", "Transfer");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());

                    break;
                case "ReceiveReject":   //수신반려
                    listPage = "./ListReceiveReject.aspx";
                    paraDic.Add("folderStatusID", "ReceiveReject");
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());

                    break;
                case "Afterward":       //후열 문서
                    listPage = "./ListAfterward.aspx";
                    paraDic.Add("folderStatusID", "Afterward");

                    break;
                case "Batch":           //일괄
                    //listPage = "./Batch/List.aspx";
                    //paraDic.Add("folderStatusID", "Batch");
                    //paraDic.Add("dTargetUserID", TargetUserID.ToString());

                    break;
                case "Proxy":           //대결
                    listPage = "./ListProxy.aspx";
                    paraDic.Add("folderStatusID", "Proxy");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());

                    break;
                case "Confirm":         //공람문서
                    listPage = "./ListConfirm.aspx";
                    paraDic.Add("folderStatusID", "Confirm");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());

                    break;
                case "Reject":          //반려문서
                    listPage = "./ListReject.aspx";
                    paraDic.Add("folderStatusID", "Reject");

                    break;
                default:
                    paraDic.Add("folderStatusID", "Process");
                    break;
            }

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            ContentURL = listPage + "?p=" + encParams;
        }


        
        #endregion

        #region (프로퍼티)
        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}
