using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Status.Common
{
    /// <summary>
    /// 통합 트리 초기메뉴 설정
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class SetDefaultAll : Base.BaseWSApproval
    {
        private readonly Dictionary<string, int> folderCountCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public void SetDefaultMenu(string p)
        {
            try
            {
                SetDecryptString(p);
                string folderStatusID = GetDecryptParam("folderStatusID", "");

                if (string.IsNullOrEmpty(folderStatusID)) return;

                SqlParameter[] parms = {
                    new SqlParameter("@CompanyID", SqlDbType.Int) { Value = CurrentUserInfo.CompanyID },
                    new SqlParameter("@UserID", SqlDbType.Int) { Value = CurrentUserInfo.UserID },
                    new SqlParameter("@FolderStatusID", SqlDbType.VarChar, 50) { Value = folderStatusID }
                };

                SQLHelper.ExecuteNonQuery(
                    GetSolutionConnectionString(CurrentUserInfo.CompanyID),
                    CommandType.Text,
                    @"MERGE dbo.DP_APP_FolderStatusAllDefault AS T
USING (SELECT @CompanyID AS CompanyID, @UserID AS UserID, @FolderStatusID AS FolderStatusID) AS S
ON T.CompanyID = S.CompanyID AND T.UserID = S.UserID
WHEN MATCHED THEN
    UPDATE SET FolderStatusID = S.FolderStatusID, ModifyDate = GETDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT (CompanyID, UserID, FolderStatusID, WriteDate)
    VALUES (S.CompanyID, S.UserID, S.FolderStatusID, GETDATE());",
                    parms);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<FolderStatusAllCountInfo> GetFolderCountList()
        {
            List<FolderStatusAllCountInfo> list = new List<FolderStatusAllCountInfo>();
            string[] folderStatusIDs = new string[] {
                "Process",
                "Complete",
                "Related",
                "Approve",
                "Cooperate",
                "Proxy",
                "Delay",
                "ReportDelay",
                "Receive",
                "ReceiveDept",
                "Transfer",
                "ReceiveReject",
                "Afterward",
                "CarboncopyReceiveAll",
                "CarboncopyForwardReceiveAll",
                "Confirm",
                "Reject",
                "ProcessSlip",
                "CompleteSlip",
                "RejectSlip",
                "RelatedSlip",
                "ApproveSlip",
                "DelaySlip",
                "PersonalBox",
                "PrivateCompleteAll",
                "ApproveCompleteAll",
                "ApproveRejectAll",
                "ArbitraryAll",
                "CarboncopyAll",
                "CarboncopyForwardAll",
                "CarboncopyCirReceiptAll",
                "CarboncopyCirSendAll",
                "DeptBox",
                "DeptCompleteAll"
            };

            foreach (string folderStatusID in folderStatusIDs)
            {
                list.Add(new FolderStatusAllCountInfo(folderStatusID, GetCntFolder(folderStatusID)));
            }

            return list;
        }

        private int GetCntFolder(string _folderStatusID)
        {
            if (folderCountCache.ContainsKey(_folderStatusID))
            {
                return folderCountCache[_folderStatusID];
            }

            int cnt = 0;
            int isBundle = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "IsBundle", "0").ToNumber<int>();
            DAL.DocumentProcess dalDP = new DAL.DocumentProcess();
            DAL.DocumentProcessAll dalDPAll = new DAL.DocumentProcessAll();
            DAL.Document dalD = new DAL.Document();
            DAL.CarboncopyAllData carboncopyAllData = new DAL.CarboncopyAllData();

            switch (_folderStatusID)
            {
                case "Process":
                    cnt = dalDPAll.GetPageCountProcessAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "Complete":
                    cnt = dalD.GetPageCountComplete(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "Related":
                    cnt = dalDPAll.GetPageCountRelatedAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "", "", "", "");
                    break;
                case "Approve":
                    cnt = dalDPAll.GetPageCountApproveAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, isBundle, "", "", "", "");
                    break;
                case "Cooperate":
                    cnt = dalDPAll.GetPageCountCooperateAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, isBundle, "", "", "", "");
                    break;
                case "Proxy":
                    cnt = dalDP.GetPageCountProxy(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, isBundle, "", "", "", "");
                    break;
                case "Delay":
                    cnt = dalDPAll.GetPageCountDelayAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "ReportDelay":
                    cnt = dalDPAll.GetPageCountReportDelayAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "Receive":
                    cnt = dalDPAll.GetPageCountReceiveAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "", "", "", "");
                    break;
                case "ReceiveDept":
                    cnt = dalDP.GetPageCountReceiveDept(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "", "", "", "");
                    break;
                case "Transfer":
                    cnt = dalDP.GetPageCountTransfer(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "ReceiveReject":
                    cnt = dalDP.GetPageCountReceiveReject(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "Afterward":
                    cnt = dalDPAll.GetPageCountAfterwardAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "CarboncopyReceiveAll":
                    cnt = carboncopyAllData.GetCarbonPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, 0, "", "", "", "", "Receive");
                    break;
                case "CarboncopyForwardReceiveAll":
                    cnt = carboncopyAllData.GetForwardPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "", "Receive");
                    break;
                case "Confirm":
                    cnt = dalDP.GetPageCountConfirm(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "Reject":
                    cnt = dalDPAll.GetPageCountRejectAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "ProcessSlip":
                    cnt = dalDP.GetPageCountProcessSlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "CompleteSlip":
                    cnt = dalD.GetPageCountCompleteSlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "RejectSlip":
                    cnt = dalD.GetPageCountRejectSlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "RelatedSlip":
                    cnt = dalDP.GetPageCountRelatedSlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, "", "", "", "");
                    break;
                case "ApproveSlip":
                    cnt = dalDP.GetPageCountApproveSlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, isBundle, "", "", "", "");
                    break;
                case "DelaySlip":
                    cnt = dalDP.GetPageCountDelaySlip(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "", "", "", "");
                    break;
                case "PersonalBox":
                    cnt = 0;
                    break;
                case "PrivateCompleteAll":
                    cnt = new DAL.PrivateDocument().GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "ApproveCompleteAll":
                    cnt = new DAL.ApprovalDocument().GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "ApproveRejectAll":
                    cnt = new DAL.ApprovalDocument().GetPageCountReject(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "ArbitraryAll":
                    cnt = new DAL.ArbitraryDocument().GetPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "CarboncopyAll":
                    cnt = carboncopyAllData.GetCarbonPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, 0, "", "", "", "", "Complete");
                    break;
                case "CarboncopyForwardAll":
                    cnt = carboncopyAllData.GetForwardPageCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "", "Complete");
                    break;
                case "CarboncopyCirReceiptAll":
                    cnt = new DAL.CirculationDocument().GetPageCountRecv(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "CarboncopyCirSendAll":
                    cnt = new DAL.CirculationDocument().GetPageCountSend(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, 0, "", "", "", "");
                    break;
                case "DeptBox":
                    cnt = 0;
                    break;
                case "DeptCompleteAll":
                    cnt = dalD.GetFormPageListCountByDeptID(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, 0, "", "", "", "", "", "");
                    break;
            }

            folderCountCache[_folderStatusID] = cnt;
            return cnt;
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
    }

    public class FolderStatusAllCountInfo
    {
        public FolderStatusAllCountInfo()
        {
        }

        public FolderStatusAllCountInfo(string _folderStatusID, int _count)
        {
            FolderStatusID = _folderStatusID;
            Count = _count;
        }

        public string FolderStatusID { get; set; }
        public int Count { get; set; }
    }
}
