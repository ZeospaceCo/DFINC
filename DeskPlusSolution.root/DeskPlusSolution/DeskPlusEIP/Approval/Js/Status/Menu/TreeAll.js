var TreeAll = {};

TreeAll.Control = null;
TreeAll.InitNodeID = 0;
TreeAll.ContentFrame = null;
TreeAll.SelectedStatusNodeID = "";
TreeAll.SkipContentLoadRefresh = false;
TreeAll.IsCountRefreshing = false;

$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            active: AccordionActive
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        TreeAll.Init();
    });
});

TreeAll.Init = function () {
    TreeAll.ContentFrame = parent.frmContent;
    TreeAll.SelectedStatusNodeID = FolderStatusID;
}

function onStatusNodeSelect(node) {
    TreeAll.OnStatusNodeSelect(node);
}

function onStatusCallbackComplete(node) {
}

TreeAll.OnStatusNodeSelect = function (node) {
    var id = node.ID;
    var targetPage = "";
    var nodeTargetPage = TreeAll.GetNodeTargetPage(node);

    TreeAll.SelectedStatusNodeID = id;
    FolderStatusID = id;
    ParamArray = { 'folderStatusID': id };

    switch (id) {
        case "Report":
            targetPage = "/DeskPlusEIP/Approval/Status/ListProcessAll.aspx";
            ParamArray = { 'folderStatusID': "Process" };
            break;

        case "Process":
            targetPage = "/DeskPlusEIP/Approval/Status/ListProcessAll.aspx";
            ParamArray = { 'folderStatusID': "Process" };
            break;

        case "Complete":
            targetPage = "/DeskPlusEIP/Approval/Status/ListComplete.aspx";
            ParamArray = { 'folderStatusID': "Complete" };
            break;

        case "Related":
            targetPage = "/DeskPlusEIP/Approval/Status/ListRelatedAll.aspx";
            ParamArray = { 'folderStatusID': "Related" };
            break;

        case "Target":
            targetPage = "/DeskPlusEIP/Approval/Status/ListApproveAll.aspx";
            ParamArray = { 'folderStatusID': "Approve" };
            break;

        case "Approve":
            targetPage = "/DeskPlusEIP/Approval/Status/ListApproveAll.aspx";
            ParamArray = { 'folderStatusID': "Approve", 'dTargetUserID': targetUserID };
            break;

        case "Cooperate":
            targetPage = "/DeskPlusEIP/Approval/Status/ListCooperateAll.aspx";
            ParamArray = { 'folderStatusID': "Cooperate" };
            break;

        case "Delay":
            targetPage = "/DeskPlusEIP/Approval/Status/ListDelayAll.aspx";
            ParamArray = { 'folderStatusID': "Delay", 'dTargetUserID': targetUserID };
            break;

        case "ReportDelay":
            targetPage = "/DeskPlusEIP/Approval/Status/ListReportDelayAll.aspx";
            ParamArray = { 'folderStatusID': "ReportDelay" };
            break;

        case "Charge":
            targetPage = "/DeskPlusEIP/Approval/Status/ListReceiveAll.aspx";
            ParamArray = { 'folderStatusID': "Receive" };
            break;

        case "Receive":
            targetPage = "/DeskPlusEIP/Approval/Status/ListReceiveAll.aspx";
            ParamArray = { 'folderStatusID': "Receive" };
            break;

        case "ReceiveDept":
            targetPage = "/DeskPlusEIP/Approval/Status/ListReceiveDept.aspx";
            ParamArray = { 'folderStatusID': "ReceiveDept" };
            break;

        case "Transfer":
            targetPage = "/DeskPlusEIP/Approval/Status/ListTransferAll.aspx";
            ParamArray = { 'folderStatusID': "Transfer", 'dTargetUserID': targetUserID };
            break;

        case "ReceiveReject":
            targetPage = "/DeskPlusEIP/Approval/Status/ListReceiveReject.aspx";
            ParamArray = { 'folderStatusID': "ReceiveReject", 'dTargetUserID': targetUserID };
            break;

        case "Afterward":
            targetPage = "/DeskPlusEIP/Approval/Status/ListAfterwardAll.aspx";
            ParamArray = { 'folderStatusID': "Afterward" };
            break;

        case "CarboncopyReceiveAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ListReceiveAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Carboncopy_Receive", 'types': "S", 'headerName': "참조", 'gubun': "Carboncopy_Receive" };
            break;

        case "CarboncopyForwardReceiveAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ForwardListReceiveAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Carboncopy_ForwardReceive", 'types': "S", 'headerName': "전달", 'gubun': "Carboncopy_ForwardReceive" };
            break;

        case "Proxy":
            targetPage = "/DeskPlusEIP/Approval/Status/ListProxy.aspx";
            ParamArray = { 'folderStatusID': "Proxy", 'dTargetUserID': targetUserID };
            break;

        case "Confirm":
            targetPage = "/DeskPlusEIP/Approval/Status/ListConfirm.aspx";
            ParamArray = { 'folderStatusID': "Confirm" };
            break;

        case "Reject":
            targetPage = "/DeskPlusEIP/Approval/Status/ListRejectAll.aspx";
            ParamArray = { 'folderStatusID': "Reject" };
            break;

        case "ApproveAll":
            targetPage = "/DeskPlusEIP/Approval/Status/ListApproveSlip.aspx";
            ParamArray = { 'folderStatusID': "ApproveSlip" };
            break;

        case "ApproveSlip1":
            targetPage = "/DeskPlusEIP/Approval/Status/ListApproveSlip.aspx";
            ParamArray = { 'folderStatusID': "ApproveSlip" };
            break;

        case "ReportSlip":
            targetPage = "/DeskPlusEIP/Approval/Status/ListProcessSlip.aspx";
            ParamArray = { 'folderStatusID': "ProcessSlip" };
            break;

        case "ProcessSlip":
            targetPage = "/DeskPlusEIP/Approval/Status/ListProcessSlip.aspx";
            ParamArray = { 'folderStatusID': "ProcessSlip" };
            break;

        case "CompleteSlip":
            targetPage = "/DeskPlusEIP/Approval/Status/ListCompleteSlip.aspx";
            ParamArray = { 'folderStatusID': "CompleteSlip" };
            break;

        case "RejectSlip":
            targetPage = "/DeskPlusEIP/Approval/Status/ListRejectSlip.aspx";
            ParamArray = { 'folderStatusID': "RejectSlip" };
            break;

        case "RelatedSlip":
            targetPage = "/DeskPlusEIP/Approval/Status/ListRelatedSlip.aspx";
            ParamArray = { 'folderStatusID': "RelatedSlip" };
            break;

        case "ApproveSlip":
            targetPage = "/DeskPlusEIP/Approval/Status/ListApproveSlip.aspx";
            ParamArray = { 'folderStatusID': "ApproveSlip" };
            break;

        case "DelaySlip":
            targetPage = "/DeskPlusEIP/Approval/Status/ListDelaySlip.aspx";
            ParamArray = { 'folderStatusID': "DelaySlip" };
            break;

        case "ApprovalBox":
            targetPage = "/DeskPlusEIP/Approval/Status/ListApproveAll.aspx";
            ParamArray = { 'folderStatusID': "Approve" };
            break;

        case "DraftBox":
            targetPage = "/DeskPlusEIP/Approval/Status/ListProcessAll.aspx";
            ParamArray = { 'folderStatusID': "Process" };
            break;

        case "ReceiveSendBox":
            targetPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ListReceiveAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Carboncopy_Receive", 'types': "S", 'headerName': "참조", 'gubun': "Carboncopy_Receive", 'folderStatusID': "CarboncopyReceiveAll" };
            break;

        case "PersonalBox":
            targetPage = "/DeskPlusEIP/Approval/Folder/Private/ListAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Private", 'types': "S", 'headerName': "완결(상신)", 'gubun': "Private", 'folderStatusID': "PrivateCompleteAll" };
            break;

        case "DeptBox":
            targetPage = "/DeskPlusEIP/Approval/Folder/Dept/ListAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Dept_Doc", 'types': "S", 'headerName': "완결", 'gubun': "Dept_Doc", 'folderStatusID': "DeptCompleteAll" };
            break;

        case "PrivateCompleteAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Private/ListAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Private", 'types': "S", 'headerName': "완결(상신)", 'gubun': "Private" };
            break;

        case "ApproveCompleteAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Approve/ListAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Approval", 'types': "S", 'headerName': "완결(결재)", 'gubun': "Approval" };
            break;

        case "ApproveRejectAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Approve/ListRejectAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Reject", 'types': "S", 'headerName': "반려(결재)", 'gubun': "Reject" };
            break;

        case "ArbitraryAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Arbitrary/ListAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Arbitrary", 'types': "S", 'headerName': "전결", 'gubun': "Arbitrary" };
            break;

        case "CarboncopyAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ListAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Carboncopy_Doc", 'types': "S", 'headerName': "참조", 'gubun': "Carboncopy_Doc" };
            break;

        case "CarboncopyForwardAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/ForwardListAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Carboncopy_Forward", 'types': "S", 'headerName': "전달", 'gubun': "Carboncopy_Forward" };
            break;

        case "CarboncopyCirReceiptAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirReceiptListAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Carboncopy_CirReceipt", 'types': "S", 'headerName': "받은회람", 'gubun': "Carboncopy_CirReceipt" };
            break;

        case "CarboncopyCirSendAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Carboncopy/CirSendListAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Carboncopy_CirSend", 'types': "S", 'headerName': "보낸회람", 'gubun': "Carboncopy_CirSend" };
            break;

        case "DeptCompleteAll":
            targetPage = "/DeskPlusEIP/Approval/Folder/Dept/ListAll.aspx";
            ParamArray = { 'formID': "0", 'yyyy': "", 'yyyymm': "", 'cabinetID': "Dept_Doc", 'types': "S", 'headerName': "완결", 'gubun': "Dept_Doc" };
            break;

        default:
            break;
    }

    if (nodeTargetPage != "") {
        targetPage = nodeTargetPage;
    }

    if (targetPage != "" && ParamArray && ParamArray.folderStatusID == undefined) {
        ParamArray.folderStatusID = id;
    }

    if (targetPage != "" && TreeAll.ContentFrame) {
        if (TreeAll.IsCurrentContentProcessPage()) {
            TreeAll.RefreshAllCount();
        }

        CallEncrypt(CallGetParam());
        TreeAll.SkipContentLoadRefresh = true;
        TreeAll.ContentFrame.location.href = targetPage + "?p=" + GetEncryptValue;
    }
}

TreeAll.IsCurrentContentProcessPage = function () {
    try {
        if (!TreeAll.ContentFrame || !TreeAll.ContentFrame.location) return false;
        if (typeof parent.isApprovalStatusProcessPage != "function") return false;

        return parent.isApprovalStatusProcessPage((TreeAll.ContentFrame.location.href || "").toLowerCase());
    }
    catch (e) {
        return false;
    }
}

TreeAll.RefreshAllCount = function (folderStatusID) {
    if (TreeAll.IsCountRefreshing) return;

    TreeAll.IsCountRefreshing = true;

    var url = "/DeskPlusEIP/Approval/Ws/Status/Common/SetDefaultAll.asmx/GetFolderCountList";
    var data = "{}";
    var obj = eval("TreeAll.SuccessRefreshAllCount");
    AjaxService(url, data, obj);
}

TreeAll.SuccessRefreshAllCount = function (data, status) {
    TreeAll.IsCountRefreshing = false;

    if (data == null || data.d == null) return;
    if (typeof UCFolderStatusTreeScript1_tvFolderStatus == "undefined") return;

    var tree = UCFolderStatusTreeScript1_tvFolderStatus;
    for (var i = 0; i < data.d.length; i++) {
        var item = data.d[i];
        var node = tree.FindNodeById(item.FolderStatusID);
        if (node == null) continue;

        var cnt = parseInt(item.Count, 10);
        if (isNaN(cnt)) cnt = 0;

        node.SetProperty('Text', (cnt > 0) ? node.Value + "(" + cnt + ")" : node.Value);
    }

    tree.Render();
}

TreeAll.GetNodeTargetPage = function (node) {
    if (node && typeof node.GetProperty == "function") {
        var targetPage = node.GetProperty("TargetPage");
        if (targetPage != undefined && targetPage != null) {
            return targetPage;
        }
    }

    return "";
}
