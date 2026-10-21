Default = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-10-07
작성목적 : 초기화
***************************************************/
$(function () {
    $(document).ready(function () {

        //알람설정
        $('#cbtnAlarm').click(function (e) {
            e.preventDefault();
            Default.ModAlarm();
        });

        //로그인 요청정보
        $('#cbtnLogin').click(function (e) {
            e.preventDefault();
            Default.ModLogin();
        });

        //모바일 접근권한
        $('#cbtnAuth').click(function (e) {
            e.preventDefault();
            Default.ModAuth();
        });

    });
});



/**************************************************
작성자 : famo23
작성일자 : 2017-04-25
작성목적 : 알람설정값변경 (웹서비스 호출)
***************************************************/
Default.ModAlarm = function () {

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

    ParamArray = {
        'mailAlarm': tmpMailAlarm,
        'etcAlarm': tmpEtcAlarm
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Default.SuccessConfirm");
    var url = "/DeskPlusEIP/MDM/Ws/Config/Setting.asmx/ModAlarm";

    AjaxService(url, data, obj);
}

/**************************************************
작성자 : famo23
작성일자 : 2017-04-25
작성목적 : 로그인 요청정보 변경 (웹서비스 호출)
***************************************************/
Default.ModLogin = function () {   

    var tmpLoginType = $("#ddlLoginType").val();

    ParamArray = {
        'loginType': tmpLoginType
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Default.SuccessConfirm");
    var url = "/DeskPlusEIP/MDM/Ws/Config/Setting.asmx/ModLogin";

    AjaxService(url, data, obj);
}

/**************************************************
작성자 : famo23
작성일자 : 2017-04-25
작성목적 : 모바일 접근권한 변경 (웹서비스 호출)
***************************************************/
Default.ModAuth = function () {

    var tmpAuth = $('input[name="rblAuth"]:checked').val();

    ParamArray = {
        'auth': tmpAuth
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Default.SuccessConfirm");
    var url = "/DeskPlusEIP/MDM/Ws/Config/Setting.asmx/ModAuth";

    AjaxService(url, data, obj);
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-10-07
작성목적 : 설정값변경 (웹서비스 값 받기)
**********************************************************************************************/
Default.SuccessConfirm = function (data, status) {
    if (data.d) {
        alert(R_CompleteConfirm);
    }
    else {
        alert(R_ProcessingError);
    }
}



/**********************************************************
작성자 : famo23
작성일자 : 2015-10-07
작성목적 : 설정값변경 (웹서비스 호출) - 전체수정은 미사용
***********************************************************/
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

    var tmpLoginType = $("#ddlLoginType").val();
    var tmpAuth = $('input[name="rblAuth"]:checked').val();

    ParamArray = {
        'mailAlarm': tmpMailAlarm,
        'etcAlarm': tmpEtcAlarm,
        'loginType': tmpLoginType,
        'auth': tmpAuth
    };

    CallEncrypt(CallGetParam());

    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("Default.SuccessConfirm");
    var url = "/DeskPlusEIP/MDM/Ws/Config/Setting.asmx/Confirm";

    AjaxService(url, data, obj);
}

