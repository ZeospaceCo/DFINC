using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Status
{
    public partial class DefaultAll : Base.BasePageApproval
    {
        public string AccActive = "0";
        public string FolderStatusID = "";
        public int ProxyUserID = 0;
        public int TargetUserID = 0;

        public string LeftURL = "";
        public string ContentURL = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Status.DefaultAll).Assembly;

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            LoadParam();
            GetDefaultStatusMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            GetLeftURL();
            GetStatusContentURL(FolderStatusID);
        }

        private void LoadParam()
        {
            AccActive = GetDecryptParam("accActive", "");
            if (AccActive.Length == 0) AccActive = GetParm("accActive", "");

            FolderStatusID = GetDecryptParam("folderStatusID", "");
            if (FolderStatusID.Length == 0) FolderStatusID = GetParm("folderStatusID", "");

            ProxyUserID = GetDecryptParam("proxyUserID", "0").ToNumber<int>();
            if (ProxyUserID == 0) ProxyUserID = GetParm("proxyUserID", "0").ToNumber<int>();

            TargetUserID = GetDecryptParam("dTargetUserID", "0").ToNumber<int>();
            if (TargetUserID == 0) TargetUserID = GetParm("dTargetUserID", "0").ToNumber<int>();
        }

        private void GetDefaultStatusMenu(int _companyID, int _userID)
        {
            if (FolderStatusID == "")
            {
                SqlParameter[] parms = {
                    new SqlParameter("@CompanyID", SqlDbType.Int) { Value = _companyID },
                    new SqlParameter("@UserID", SqlDbType.Int) { Value = _userID }
                };

                object folderStatusID = SQLHelper.ExecuteScalar(
                    GetSolutionConnectionString(_companyID),
                    CommandType.Text,
                    "IF OBJECT_ID('dbo.DP_APP_FolderStatusAllDefault', 'U') IS NULL SELECT '' ELSE SELECT TOP 1 FolderStatusID FROM dbo.DP_APP_FolderStatusAllDefault WHERE CompanyID = @CompanyID AND UserID = @UserID",
                    parms);

                FolderStatusID = (folderStatusID == null || folderStatusID == DBNull.Value) ? "" : folderStatusID.ToString().ToNull();
            }

            if (FolderStatusID == "") FolderStatusID = "Approve";
        }

        private string GetSolutionConnectionString(int _companyID)
        {
            ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["SQLConnSolution"];
            if (conn != null && !string.IsNullOrEmpty(conn.ConnectionString))
            {
                return conn.ConnectionString;
            }

            return SQLHelper.ConnSolution(_companyID);
        }

        private void GetLeftURL()
        {
            Dictionary<string, string> paraDic = new Dictionary<string, string>();

            paraDic.Add("accActive", "0");
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);
            paraDic.Add("folderStatusID", FolderStatusID);
            paraDic.Add("proxyUserID", ProxyUserID.ToString());

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            LeftURL = "./Menu/TreeAll.aspx?p=" + encParams;
        }

        private void GetStatusContentURL(string _menuID)
        {
            string listPage = "/DeskPlusEIP/Approval/Status/ListApproveAll.aspx";

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("currentPage", CurrentPage.ToString());
            paraDic.Add("searchTerm", SearchTerm);
            paraDic.Add("searchTermValue", SearchTermValue);
            paraDic.Add("searchField", SearchField);
            paraDic.Add("searchValue", SearchValue);
            paraDic.Add("order", Order);

            switch (_menuID)
            {
                case "Report":
                case "Process":
                    listPage = "/DeskPlusEIP/Approval/Status/ListProcessAll.aspx";
                    paraDic.Add("folderStatusID", "Process");
                    break;
                case "Complete":
                    listPage = "/DeskPlusEIP/Approval/Status/ListComplete.aspx";
                    paraDic.Add("folderStatusID", "Complete");
                    break;
                case "Related":
                    listPage = "/DeskPlusEIP/Approval/Status/ListRelatedAll.aspx";
                    paraDic.Add("folderStatusID", "Related");
                    break;
                case "Target":
                    listPage = "/DeskPlusEIP/Approval/Status/ListApproveAll.aspx";
                    paraDic.Add("folderStatusID", "Approve");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    break;
                case "Approve":
                    listPage = "/DeskPlusEIP/Approval/Status/ListApproveAll.aspx";
                    paraDic.Add("folderStatusID", "Approve");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());
                    break;
                case "Cooperate":
                    listPage = "/DeskPlusEIP/Approval/Status/ListCooperateAll.aspx";
                    paraDic.Add("folderStatusID", "Cooperate");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    break;
                case "Delay":
                    listPage = "/DeskPlusEIP/Approval/Status/ListDelayAll.aspx";
                    paraDic.Add("folderStatusID", "Delay");
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());
                    break;
                case "ReportDelay":
                    listPage = "/DeskPlusEIP/Approval/Status/ListReportDelayAll.aspx";
                    paraDic.Add("folderStatusID", "ReportDelay");
                    break;
                case "Charge":
                case "Receive":
                    listPage = "/DeskPlusEIP/Approval/Status/ListReceiveAll.aspx";
                    paraDic.Add("folderStatusID", "Receive");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    break;
                case "ReceiveDept":
                    listPage = "/DeskPlusEIP/Approval/Status/ListReceiveDept.aspx";
                    paraDic.Add("folderStatusID", "ReceiveDept");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    break;
                case "Transfer":
                    listPage = "/DeskPlusEIP/Approval/Status/ListTransferAll.aspx";
                    paraDic.Add("folderStatusID", "Transfer");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());
                    break;
                case "ReceiveReject":
                    listPage = "/DeskPlusEIP/Approval/Status/ListReceiveReject.aspx";
                    paraDic.Add("folderStatusID", "ReceiveReject");
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());
                    break;
                case "Afterward":
                    listPage = "/DeskPlusEIP/Approval/Status/ListAfterwardAll.aspx";
                    paraDic.Add("folderStatusID", "Afterward");
                    break;
                case "ReceiveSendBox":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ListReceiveAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Carboncopy_Receive");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "참조");
                    paraDic.Add("gubun", "Carboncopy_Receive");
                    paraDic.Add("folderStatusID", "CarboncopyReceiveAll");
                    break;
                case "CarboncopyReceiveAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ListReceiveAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Carboncopy_Receive");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "참조");
                    paraDic.Add("gubun", "Carboncopy_Receive");
                    break;
                case "CarboncopyForwardReceiveAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ForwardListReceiveAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Carboncopy_ForwardReceive");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "전달");
                    paraDic.Add("gubun", "Carboncopy_ForwardReceive");
                    break;
                case "Proxy":
                    listPage = "/DeskPlusEIP/Approval/Status/ListProxy.aspx";
                    paraDic.Add("folderStatusID", "Proxy");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());
                    break;
                case "Confirm":
                    listPage = "/DeskPlusEIP/Approval/Status/ListConfirm.aspx";
                    paraDic.Add("folderStatusID", "Confirm");
                    paraDic.Add("proxyUserID", ProxyUserID.ToString());
                    paraDic.Add("dTargetUserID", TargetUserID.ToString());
                    break;
                case "Reject":
                    listPage = "/DeskPlusEIP/Approval/Status/ListRejectAll.aspx";
                    paraDic.Add("folderStatusID", "Reject");
                    break;
                case "ApproveAll":
                case "ApproveSlip1":
                case "ApproveSlip":
                    listPage = "/DeskPlusEIP/Approval/Status/ListApproveSlip.aspx";
                    paraDic.Add("folderStatusID", "ApproveSlip");
                    break;
                case "ReportSlip":
                case "ProcessSlip":
                    listPage = "/DeskPlusEIP/Approval/Status/ListProcessSlip.aspx";
                    paraDic.Add("folderStatusID", "ProcessSlip");
                    break;
                case "CompleteSlip":
                    listPage = "/DeskPlusEIP/Approval/Status/ListCompleteSlip.aspx";
                    paraDic.Add("folderStatusID", "CompleteSlip");
                    break;
                case "RejectSlip":
                    listPage = "/DeskPlusEIP/Approval/Status/ListRejectSlip.aspx";
                    paraDic.Add("folderStatusID", "RejectSlip");
                    break;
                case "RelatedSlip":
                    listPage = "/DeskPlusEIP/Approval/Status/ListRelatedSlip.aspx";
                    paraDic.Add("folderStatusID", "RelatedSlip");
                    break;
                case "DelaySlip":
                    listPage = "/DeskPlusEIP/Approval/Status/ListDelaySlip.aspx";
                    paraDic.Add("folderStatusID", "DelaySlip");
                    break;
                case "PrivateCompleteAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Private/ListAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Private");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "완결(상신)");
                    paraDic.Add("gubun", "Private");
                    break;
                case "ApproveCompleteAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Approve/ListAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Approval");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "완결(결재)");
                    paraDic.Add("gubun", "Approval");
                    break;
                case "ApproveRejectAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Approve/ListRejectAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Reject");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "반려(결재)");
                    paraDic.Add("gubun", "Reject");
                    break;
                case "ArbitraryAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Arbitrary/ListAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Arbitrary");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "전결");
                    paraDic.Add("gubun", "Arbitrary");
                    break;
                case "CarboncopyAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ListAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Carboncopy_Doc");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "참조");
                    paraDic.Add("gubun", "Carboncopy_Doc");
                    break;
                case "CarboncopyForwardAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ForwardListAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Carboncopy_Forward");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "전달");
                    paraDic.Add("gubun", "Carboncopy_Forward");
                    break;
                case "CarboncopyCirReceiptAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirReceiptListAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Carboncopy_CirReceipt");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "받은회람");
                    paraDic.Add("gubun", "Carboncopy_CirReceipt");
                    break;
                case "CarboncopyCirSendAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirSendListAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Carboncopy_CirSend");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "보낸회람");
                    paraDic.Add("gubun", "Carboncopy_CirSend");
                    break;
                case "DeptCompleteAll":
                    listPage = "/DeskPlusEIP/Approval/Folder/Dept/ListAll.aspx";
                    paraDic.Add("formID", "0");
                    paraDic.Add("yyyy", "");
                    paraDic.Add("yyyymm", "");
                    paraDic.Add("cabinetID", "Dept_Doc");
                    paraDic.Add("types", "S");
                    paraDic.Add("headerName", "완결");
                    paraDic.Add("gubun", "Dept_Doc");
                    break;
                default:
                    paraDic.Add("folderStatusID", "Approve");
                    break;
            }

            if (listPage.IndexOf("/Folder/", StringComparison.OrdinalIgnoreCase) >= 0 && !paraDic.ContainsKey("folderStatusID"))
            {
                paraDic.Add("folderStatusID", _menuID);
            }

            string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
            ContentURL = listPage + "?p=" + encParams;
        }

    }
}
