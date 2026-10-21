Approve = {}

/**********************************************************************************************
작성자   : 
작성일자 : 
작성목적 : 데이터 초기화 // 데이터베이스로부터 출력할 데이터를 가져와서 출력하는 메서드
**********************************************************************************************/
$(function () {

    $(document).ready(function () {

        // 미리보기
        $('#cbtnPreview').click(function (e) { e.preventDefault(); Approve.Preview(); });
        // 승인
        $('#cbtnApprove').click(function (e) { e.preventDefault(); Approve.Confirm('S'); });
        // 반려
        $('#cbtnReject').click(function (e) { e.preventDefault(); Approve.Confirm('R'); });
        // 삭제
        $('#cbtnDel').click(function (e) { e.preventDefault(); Approve.Del(); });
        // 취소
        $('#CbtnCancel').click(function (e) { e.preventDefault(); Approve.Cancel(); });

        Approve.Init();

    });
});

Approve.Init = function () {
    if (State == "R") {
        $("#ulApprove").attr("style", "display:none");        //반려상태의 공문은 폐기만 가능.
    }
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 미리보기
***************************************************/
Approve.Preview = function () {

    ParamArray = { 'enforceID': EnforceID };
    CallEncrypt(CallGetParam());
    var url = "/DeskPlusEIP/Approval/Folder/Enforce/Preview.aspx?p=" + GetEncryptValue;
    PopupWin(800, 700, url, "-500", "", true);
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 승인(S) / 반려(R)
***************************************************/
var ApprovalType = "";
Approve.Confirm = function (_type) {
    ApprovalType = _type;
    ParamArray = { 'p': 'p' };
    CallEncrypt(CallGetParam());

    var url = "ApproveConfirm.aspx?p=" + GetEncryptValue;
    PopupWin(400, 350, url, "", "", true);
}
Approve.ConfirmFunc = function () {
    var url = "/DeskPlusEIP/Approval/Ws/Folder/EnforceAdmin/Approve.asmx/Confirm";
    ParamArray = {
        'enforceID': EnforceID,
        'enforceStampID': EnforceStampID,
        'stampDeptID': StampDeptID,
        'state': ApprovalType,
        'reqUserID' : ReqUserID,
        'confirmOpinion': $("#tbOpinion").val()
    };

    var obj = eval("Approve.SuccessConfirm");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}
Approve.SuccessConfirm = function (data, statue) {
    var msg = (data.d == true) ? R_MSG_SUCCESS : R_MSG_FAIL;
    alert(msg);
    Approve.Cancel();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 삭제
***************************************************/
Approve.Del = function () {
    if (confirm(R_Msg_Del)) {

        var url = "/DeskPlusEIP/Approval/Ws/Folder/EnforceAdmin/Approve.asmx/Del";
        ParamArray = { 'enforceID': EnforceID };

        var obj = eval("Approve.SuccessDel");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxService(url, data, obj);
    }
}
Approve.SuccessDel = function (data, status) {
    var msg = (data.d == true) ? R_MSG_SUCCESS : R_MSG_FAIL;
    alert(msg);
    Approve.Cancel();
}

/**************************************************
작성자   : 
작성일자 : 
작성목적 : 취소
***************************************************/
Approve.Cancel = function () {
    ParamArray = {
        'currentPage': CurrentPage
        , 'recordPerPage': RecordPerPage
        , 'searchField': SearchField
        , 'searchValue': SearchValue
        , 'searchTerm': SearchTerm
        , 'searchTermValue': SearchTermValue
        , 'order': Order
        , 'targetPage': TargetPage
        , 'cabinetID': CabinetID
    };
    CallEncrypt(CallGetParam());
    location.href = TargetPage + "?p=" + GetEncryptValue;
}
