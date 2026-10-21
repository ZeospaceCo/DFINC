List = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 환경설정 가져오기
    if (PimsCheck == "Y") {
        setTimeout("List.getConfig();", 400);
    }

    $('#btsave').click(function (e) {
        List.setConfig();
    });

    $('#rdPims1').click(function (e) {
        List.choicePims(this);
    });

    $('#rdPims2').click(function (e) {
        List.choicePims(this);
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    if ($("#cbAlarmTimeUse").prop("checked")) {
        $("#divAlarmTime").show();
    };

    $('#cbAlarmTimeUse').change(function () {
        var checked = $(this).prop("checked");
        if (checked) {
            $("#divAlarmTime").show();
        } else {
            $("#divAlarmTime").hide();
        }
        $("#alarmStartHour").attr("disabled", !checked);
        $("#alarmStartMin").attr("disabled", !checked);
        $("#alarmEndHour").attr("disabled", !checked);
        $("#alarmEndMin").attr("disabled", !checked);
    });

    $('#ddlMailList').change(function () {
        if (UUID != "") {
            if ($("#ddlMailList").val() == "N") {
                $("#ddlMailAlarm").val("N");
                $("#ddlMailAlarm").attr("disabled", true);
            }
            else {
                $("#ddlMailAlarm").attr("disabled", false);
            }
        }
    });

    $('#ddlApprovalList').change(function () {
        if (UUID != "") {
            if ($("#ddlApprovalList").val() == "N") {
                $("#ddlApprovalAlarm").val("N");
                $("#ddlApprovalAlarm").attr("disabled", true);
            }
            else {
                $("#ddlApprovalAlarm").attr("disabled", false);
            }
        }
    });

    $('#ddlBoardList').change(function () {
        if (UUID != "") {
            if ($("#ddlBoardList").val() == "N") {
                $("#ddlBoardAlarm").val("N");
                $("#ddlBoardAlarm").attr("disabled", true);
            }
            else {
                $("#ddlBoardAlarm").attr("disabled", false);
            }
        }
    });

    $('#ddlPimsList').change(function () {
        if (UUID != "") {
            if ($("#ddlPimsList").val() == "N") {
                $("#ddlPimsAlarm").val("N");
                $("#ddlPimsAlarm").attr("disabled", true);
            }
            else {
                $("#ddlPimsAlarm").attr("disabled", false);
            }
        }
    });

    $('#ddlTaskList').change(function () {
        if (UUID != "") {
            if ($("#ddlTaskList").val() == "N") {
                $("#ddlTaskAlarm").val("N");
                $("#ddlTaskAlarm").attr("disabled", true);
            }
            else {
                $("#ddlTaskAlarm").attr("disabled", false);
            }
        }
    });

    $('#ddlWorkList').change(function () {
        if (UUID != "") {
            if ($("#ddlWorkList").val() == "N") {
                $("#ddlWorkAlarm").val("N");
                $("#ddlWorkAlarm").attr("disabled", true);
            }
            else {
                $("#ddlWorkAlarm").attr("disabled", false);
            }
        }
    });

    $('#ddlCommunityList').change(function () {
        if (UUID != "") {
            if ($("#ddlCommunityList").val() == "N") {
                $("#ddlCommunityAlarm").val("N");
                $("#ddlCommunityAlarm").attr("disabled", true);
            }
            else {
                $("#ddlCommunityAlarm").attr("disabled", false);
            }
        }
    });

    $('#ddlResourceList').change(function () {
        if (UUID != "") {
            if ($("#ddlResourceList").val() == "N") {
                $("#ddlResourceAlarm").val("N");
                $("#ddlResourceAlarm").attr("disabled", true);
            }
            else {
                $("#ddlResourceAlarm").attr("disabled", false);
            }
        }
    });

    $('#ddlSNSList').change(function () {
        if (UUID != "") {
            if ($("#ddlSNSList").val() == "N") {
                $("#ddlSNSAlarm").val("N");
                $("#ddlSNSAlarm").attr("disabled", true);
            }
            else {
                $("#ddlSNSAlarm").attr("disabled", false);
            }
        }
    });

});

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 환경설정 가져오기 (웹서비스 호출)
***************************************************/
List.getConfig = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Config/Ws/GetConfig.asmx/GetEnvironment";

    //파라미터값 배열에 담기
    ParamArray = {
        'guid1': '1'
    };

    var obj = eval("List.succeededGet");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 환경설정 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeededGet = function (data, status) {
    var conCont = eval('(' + data.d + ')');

    var pimsMenu = conCont.UserEnv[0].MobilePims;

    if (pimsMenu == null) {
        pimsMenu = "0|0|0|0|";
    }

    if (pimsMenu.indexOf("1") > -1) {
        $("#rdPims2").attr("checked", true);
        jQuery.each($("#liCbPims2"), function () { this.style.display = ""; });

        var pimsMenuArr = pimsMenu.split("|");

        //개인일정보기 저장 유무
        if (pimsMenuArr[0] == "1") {
            $("#rdPimsUser1").attr("checked", true);
        } else {
            $("#rdPimsUser2").attr("checked", true);
        }

        //부서일정보기 저장 유무
        if (pimsMenuArr[1] == "1") {
            $("#rdPimsDept1").attr("checked", true);
        } else {
            $("#rdPimsDept2").attr("checked", true);
        }

        //공유일정보기 저장 유무
        if (pimsMenuArr[2] == "1") {
            $("#rdPimsShare1").attr("checked", true);
        } else {
            $("#rdPimsShare2").attr("checked", true);
        }

        //전체일정보기 저장 유무
        if (pimsMenuArr[3] == "1") {
            $("#rdPimsAll1").attr("checked", true);
        } else {
            $("#rdPimsAll2").attr("checked", true);
        }
    }
    else {

        $("#rdPims1").attr("checked", true);
        jQuery.each($("#liCbPims2"), function () { this.style.display = "none"; });
    }

    // 결재전용 비밀번호 사용 2017-02-24
    var ispwd = conCont.UserEnv[0].MobileIsApprovePwd;
    if (ispwd == null) {
        ispwd = "N";
    }
    if (ispwd == "N") {
        $("#rblUsePwd_0").attr("checked", true);
    } else {
        $("#rblUsePwd_1").attr("checked", true);
    }

    //LoadingShow(false);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-21
작성목적 : 일정 보기 설정 라디오버튼 클릭시
***************************************************/
List.choicePims = function (arg) {
    var tmpID = arg.id;

    if (tmpID == "rdPims1") {
        jQuery.each($("#liCbPims2"), function () { this.style.display = "none"; });

    }
    else {
        jQuery.each($("#liCbPims2"), function () { this.style.display = ""; });

        $("#rdPimsUser2").attr("checked", true);
        $("#rdPimsDept2").attr("checked", true);
        $("#rdPimsShare2").attr("checked", true);
        $("#rdPimsAll2").attr("checked", true);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2011-06-21
작성목적 : 환경설정 저장하기 (웹서비스 호출)
***************************************************/
List.setConfig = function () {

    var pimsMenu = "";

    if (PimsCheck == "Y") {
        if ($("#rdPims1").is(":checked")) {
            pimsMenu = "0|0|0|0|";
        }
        else {
            if ($("#rdPimsUser1").is(":checked")) {
                pimsMenu += "1|";
            }
            else {
                pimsMenu += "0|";
            }

            if ($("#rdPimsDept1").is(":checked")) {
                pimsMenu += "1|";
            }
            else {
                pimsMenu += "0|";
            }

            if ($("#rdPimsShare1").is(":checked")) {
                pimsMenu += "1|";
            }
            else {
                pimsMenu += "0|";
            }

            if ($("#rdPimsAll1").is(":checked")) {
                pimsMenu += "1|";
            }
            else {
                pimsMenu += "0|";
            }
        }
    }
    else {
        pimsMenu = "0|0|0|0|";
    }

    var tmpAlarmList = "";

    if ($("#ddlMailList").val() == "Y") {
        tmpAlarmList += "Mail,";
    }

    if ($("#ddlApprovalList").val() == "Y") {
        tmpAlarmList += "Approval,";
    }

    if ($("#ddlBoardList").val() == "Y") {
        tmpAlarmList += "Board,";
    }

    if ($("#ddlPimsList").val() == "Y") {
        tmpAlarmList += "Pim,";
    }

    if ($("#ddlTaskList").val() == "Y") {
        tmpAlarmList += "TASK,";
    }

    if ($("#ddlWorkList").val() == "Y") {
        tmpAlarmList += "Report,";
    }

    if ($("#ddlCommunityList").val() == "Y") {
        tmpAlarmList += "Community,";
    }

    if ($("#ddlResourceList").val() == "Y") {
        tmpAlarmList += "ResourceManager,";
    }

    if ($("#ddlSNSList").val() == "Y") {
        tmpAlarmList += "SNS,";
    }

    var tmpAlarm = "";

    if (UUID != "") {
        if ($("#ddlApprovalAlarm").val() == "Y") {
            tmpAlarm += "Approval,";
        }

        if ($("#ddlBoardAlarm").val() == "Y") {
            tmpAlarm += "Board,";
        }

        if ($("#ddlPimsAlarm").val() == "Y") {
            tmpAlarm += "Pim,";
        }

        if ($("#ddlTaskAlarm").val() == "Y") {
            tmpAlarm += "TASK,";
        }

        if ($("#ddlWorkAlarm").val() == "Y") {
            tmpAlarm += "Report,";
        }

        if ($("#ddlCommunityAlarm").val() == "Y") {
            tmpAlarm += "Community,";
        }

        if ($("#ddlResourceAlarm").val() == "Y") {
            tmpAlarm += "ResourceManager,";
        }

        if ($("#ddlSNSAlarm").val() == "Y") {
            tmpAlarm += "SNS,";
        }

        if ($("#ddlNoticeAlarm").val() == "Y") {
            tmpAlarm += "Notice,";
        }
    }

    var ispwd = ($("#rblUsePwd_0").is(":checked")) ? "N" : "Y";

    var tmpMail = $("#ddlMailAlarm").val();
    var tmpLoginType = $("#ddlLoginType").val();

    var alarmTimeUse = $("#cbAlarmTimeUse").prop("checked") ? "1" : "0";
    var alarmStart = $("#alarmStartHour").val() + ":" + $("#alarmStartMin").val();
    var alarmEnd = $("#alarmEndHour").val() + ":" + $("#alarmEndMin").val();

    if (alarmStart == alarmEnd) {
        action_popup.alert(R_AlarmTimeCheck);
        return;
    }

    LoadingShow(true);
    var url = "/DeskPlusMobile/Config/Ws/SetConfig.asmx/SetEnvironment";

    //파라미터값 배열에 담기
    ParamArray = {
        'pimsMenu': pimsMenu,
        'alarm': tmpAlarm,
        'mailAlarm': tmpMail,
        'loginType': tmpLoginType,
        'skinType': SkinType,
        'ispwd': ispwd,
        'alarmTimeUse': alarmTimeUse,
        'alarmStart': alarmStart,
        'alarmEnd': alarmEnd,
        'alarmList': tmpAlarmList
    };

    var obj = eval("List.succeededSet");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-21
작성목적 : 환경설정 저장하기 (웹서비스 값 받기)
***************************************************/
List.succeededSet = function (data, status) {
    NoticeShow("*", R_Save1);
    LoadingShow(false);
    document.location.reload();
}


/**************************************************
작성자:
작성일:2016-07-12
작성목적 : 
***************************************************/
List.SetSkin = function (color) {
    SkinType = color;
    $("#ColorSample").attr("src", "/DeskPlusMobile/Content/ko/Base/Img/ColorSample/" + color + ".png");
}

List.AlarmTimeUse = function (alarmTimeUse) {
    var AlarmTimeUse = alarmTimeUse == "1" ? true : false;
    $("#cbAlarmTimeUse").prop("checked", AlarmTimeUse);
    if (AlarmTimeUse) {
        $("#divAlarmTime").show();
    } else {
        $("#divAlarmTime").hide();
    }
    $("#alarmStartHour").attr("disabled", !AlarmTimeUse);
    $("#alarmStartMin").attr("disabled", !AlarmTimeUse);
    $("#alarmEndHour").attr("disabled", !AlarmTimeUse);
    $("#alarmEndMin").attr("disabled", !AlarmTimeUse);
}

