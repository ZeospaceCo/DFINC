Default = {}

/**************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //확인
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            Default.Confirm();
        });

    });
});


/**************************************************
작성자 : famo23
작성일자 : 2013-01-16
작성목적 : 환경설정 (웹서비스 호출)
***************************************************/
Default.Confirm = function () {

    var tmpMailAlarm = $("#ddlMailAlarm").val();
    var tmpEtcAlarm = "";

    if ($("#ddlApprovalAlarm").val() == "Y") {
        tmpEtcAlarm += "Approval,";
    }

    if ($("#ddlBoardAlarm").val() == "Y") {
        tmpEtcAlarm += "Board,";
    }

    if ($("#ddlPimsAlarm").val() == "Y") {
        tmpEtcAlarm += "Pim,";
    }

    if ($("#ddlTaskAlarm").val() == "Y") {
        tmpEtcAlarm += "TASK,";
    }

    if ($("#ddlWorkAlarm").val() == "Y") {
        tmpEtcAlarm += "Report,";
    }

    if ($("#ddlCommunityAlarm").val() == "Y") {
        tmpEtcAlarm += "Community,";
    }

    if ($("#ddlResourceAlarm").val() == "Y") {
        tmpEtcAlarm += "ResourceManager,";
    }

    if ($("#ddlSNSAlarm").val() == "Y") {
        tmpEtcAlarm += "SNS,";
    }

    if ($("#ddlNoticeAlarm").val() == "Y") {
        tmpEtcAlarm += "Notice,";
    }

    var tmpLoginType = $("#ddlLoginType").val();
    var tmpAuth = $('input[name="rblAuth"]:checked').val();
    var tmpApprove = $("#ddlAppApprove").val();

    ParamArray = {
        'mailAlarm': tmpMailAlarm,
        'etcAlarm': tmpEtcAlarm,
        'loginType': tmpLoginType,
        'auth': tmpAuth,
        'approve': tmpApprove
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Default.SuccessConfirm");
    var url = "/DeskPlusEIP/MDM/Ws/Config/Default.asmx/Confirm";

    AjaxService(url, data, obj);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2013-01-16
작성목적 : 환경설정 (웹서비스 값 받기)
**********************************************************************************************/
Default.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}

