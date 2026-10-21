var Tree = {};

Tree.Control = null;
Tree.InitNodeID = 0;
Tree.ContentFrame = null;

/**************************************************
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {
        $(".accordion").accordion({
            fillSpace: true,
            animated: 'slide',
            active: AccordionActive
            //event: "mouseover",
        });

        $("#accordionResizer").resizable({
            minHeight: 0,
            resize: function () { $("#accordion").accordion("resize"); }
        });

        Tree.Init();
    });
});

/**************************************************************
초기화
**************************************************************/
Tree.Init = function () {

    Tree.ContentFrame = parent.frmContent;
    Tree.SelectedNodeID = FolderStatusID;
}

/**************************************************
onNodeSelect 트리 노드 선택
***************************************************/
function onNodeSelect(node) {

    var id = node.ID;
    var targetPage = "";
    Tree.SelectedNodeID = node.ID;

    switch (id) {
        case "Report": //상신문서
            targetPage = "../ListProcess.aspx";
            ParamArray = { 'folderStatusID': "Process" };
            break;

        case "Process": //진행중인문서
            targetPage = "../ListProcess.aspx";
            ParamArray = { 'folderStatusID': "Process" };
            break;

        case "Complete": //완결문서
            targetPage = "../ListComplete.aspx";
            ParamArray = { 'folderStatusID': "Complete" };
            break;

        case "Related": //관계 문서
            targetPage = "../ListRelated.aspx";
            ParamArray = { 'folderStatusID': "Related" };
            break;

        case "Target": //결재 진행
            targetPage = "../ListApprove.aspx";
            ParamArray = { 'folderStatusID': "Approve" };
            break;

        case "Approve": //결재 대상
            targetPage = "../ListApprove.aspx";
            ParamArray = { 'folderStatusID': "Approve", 'dTargetUserID': targetUserID };
            break;

        case "Delay":   //보류 문서
            targetPage = "../ListDelay.aspx";
            ParamArray = { 'folderStatusID': "Delay", 'dTargetUserID': targetUserID };
            break;

        case "Charge": //담당자 결재 문서
            targetPage = "../ListReceive.aspx";
            ParamArray = { 'folderStatusID': "Receive" };

            break;
        case "Receive": //수신 문서
            targetPage = "../ListReceive.aspx";
            ParamArray = { 'folderStatusID': "Receive" };
            break;

        case "ReceiveDept": //부서 수신 문서
            targetPage = "../ListReceiveDept.aspx";
            ParamArray = { 'folderStatusID': "ReceiveDept" };
            break;

        case "Transfer": //이관
            targetPage = "../ListTransfer.aspx";
            ParamArray = { 'folderStatusID': "Transfer", 'dTargetUserID': targetUserID };
            break;

        case "ReceiveReject":   //수신반려
            targetPage = "../ListReceiveReject.aspx";
            ParamArray = { 'folderStatusID': "ReceiveReject", 'dTargetUserID': targetUserID };
            break;

        case "Afterward":   //후열 문서
            targetPage = "../ListAfterward.aspx";
            ParamArray = { 'folderStatusID': "Afterward" };
            break;

        case "Batch":   //일괄 - 미개발 사항
            //targetPage = "../Batch/List.aspx";
            //ParamArray = { 'folderStatusID': menuID, 'dTargetUserID': targetUserID };
            break;

        case "Proxy": //대결 대상
            targetPage = "../ListProxy.aspx";
            ParamArray = { 'folderStatusID': "Proxy", 'dTargetUserID': targetUserID };
            break;

        case "Confirm":   //공람문서
            targetPage = "../ListConfirm.aspx";
            ParamArray = { 'folderStatusID': "Confirm" };
            break;

        case "Reject":   //반려문서
            targetPage = "../ListReject.aspx";
            ParamArray = { 'folderStatusID': "Reject" };
            break;

        case "ApproveAll": //일괄결재대상문서
            targetPage = "../ListApproveSlip.aspx";
            ParamArray = { 'folderStatusID': "ApproveSlip" };
            break;

        case "ApproveSlip1": //일괄결재 ERP문서
            targetPage = "../ListApproveSlip.aspx";
            ParamArray = { 'folderStatusID': "ApproveSlip" };
            break;

        case "ReportSlip": //일괄결재 상신문서
            targetPage = "../ListProcessSlip.aspx";
            ParamArray = { 'folderStatusID': "ProcessSlip" };
            break;

        case "ProcessSlip": //일괄결재 진행중인문서
            targetPage = "../ListProcessSlip.aspx";
            ParamArray = { 'folderStatusID': "ProcessSlip" };
            break;

        case "CompleteSlip": //일괄결재 완결문서
            targetPage = "../ListCompleteSlip.aspx";
            ParamArray = { 'folderStatusID': "CompleteSlip" };
            break;

        case "RejectSlip":   //일괄결재 반려문서
            targetPage = "../ListRejectSlip.aspx";
            ParamArray = { 'folderStatusID': "RejectSlip" };
            break;

        case "RelatedSlip": //일괄결재 관계 문서
            targetPage = "../ListRelatedSlip.aspx";
            ParamArray = { 'folderStatusID': "RelatedSlip" };
            break;

        case "ApproveSlip": //일괄결재 결재대상문서
            targetPage = "../ListApproveSlip.aspx";
            ParamArray = { 'folderStatusID': "ApproveSlip" };
            break;

        case "DelaySlip":   //일괄결재 보류 문서
            targetPage = "../ListDelaySlip.aspx";
            ParamArray = { 'folderStatusID': "DelaySlip" };
            break;

        default:
            break;
    }
    if (targetPage != "" && Tree.ContentFrame) {
        CallEncrypt(CallGetParam());
        Tree.ContentFrame.location.href = targetPage + "?p=" + GetEncryptValue;
    }
}


/**************************************************************
onCallbackComplete 
**************************************************************/
function onCallbackComplete(node) {

}
