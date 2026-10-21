PrivateMod = {}

$(function () {

    $(document).ready(function () {

        //닫기
        $('#cbtnClose').click(function (e) {
            e.preventDefault();
            if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
        });

        //저장
        $('#cbtnConfirm').click(function (e) {
            e.preventDefault();
            PrivateMod.Confirm();
        });

        PrivateMod.Init();
    });
});

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-21
작성목적 : 초기화
***************************************************/
PrivateMod.Init = function () {

}

/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 출퇴근시간 수정
***************************************************/
PrivateMod.Confirm = function () {
    
    if ($("#ddlWSTimeHour").val() == "" && $("#ddlWSTimeMinute").val() == "" && $("#ddlWSConn").val() == "" && $("#ddlWCTimeHour").val() == "" && $("#ddlWCTimeMinute").val() == "" && $("#ddlWCConn").val() == "") {
        alert(R_SelTime); //시간을 선택하세요.
        return;
    }

    if ($("#ddlWSTimeHour").val() != "" || $("#ddlWSTimeMinute").val() != "" || $("#ddlWSConn").val() != "") {
        if ($("#ddlWSTimeHour").val() == "" || $("#ddlWSTimeMinute").val() == "" || $("#ddlWSConn").val() == "") {
            alert(R_SelWSTime); //출근시간을 선택하세요.
            return;
        }
    }

    if ($("#ddlWCTimeHour").val() != "" || $("#ddlWCTimeMinute").val() != "" || $("#ddlWCConn").val() != "") {
        if ($("#ddlWCTimeHour").val() == "" || $("#ddlWCTimeMinute").val() == "" || $("#ddlWCConn").val() == "") {
            alert(R_SelWCTime); //퇴근시간을 선택하세요.
            return;
        }
    }

    if ($("#ddlWCTimeHour").val() != "") {
        if ($("#ddlWSTimeHour").val() == "" || $("#ddlWSTimeMinute").val() == "" || $("#ddlWSConn").val() == "") {
            alert(R_SelWSTime); //출근시간을 선택하세요.
            return;
        }
    }

    if (jQuery.trim($("#txtModReason").val()) == "") {
        alert(R_ModReasonInput); // 수정사유를 입력하세요.
        return;
    }

    //선택한 출근시간 915
    var h = $("#ddlWSTimeHour").val();
    if (h.substr(0, 1) == "0") {
        h = h.substr(1, 1);
    }
    var mm = $("#ddlWSTimeMinute").val();
    var selWSTime = parseInt(h + mm);

    //선택한 퇴근시간 1815
    var h2 = $("#ddlWCTimeHour").val();
    if (h2.substr(0, 1) == "0") {
        h2 = h2.substr(1, 1);
    }
    var mm2 = $("#ddlWCTimeMinute").val();
    var selWCTime = parseInt(h2 + mm2);

    var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900
    var tWCTime = parseInt(bWCTimeHour + bWCTimeMinute); //정규 퇴근시간 시분 1800

    var bLate = "0";
    if ($("#ddlWSTimeHour").val() != "") {
        if (selWSTime > tWSTime) //지각
        {
            /*
            if (jQuery.trim($("#txtLateRemark").val()) == "") {
                alert(R_LateReason); // 지각사유를 입력하세요.
                return;
            }
            */

            bLate = "1";
        }
    }

    var bEarly = "0";
    if ($("#ddlWCTimeHour").val() != "") {
        if (selWCTime < tWCTime) //조퇴  
        {
            /*
            if (jQuery.trim($("#txtEarlyRemark").val()) == "") {
                alert(R_EarlyReason); // 조퇴사유를 입력하세요.
                return;
            }
            */

            bEarly = "1";
        }
    }

    var url = "/DeskPlusEIP/HR/Ws/Content/Popup/PrivateMod.asmx/SetAttendance";

    ParamArray = {
        'attID': AttID,
        'targetUserID': TargetUserID,
        'targetDeptID': TargetDeptID,
        'pathID': PathID,
        'workDate': WorkDate,
        'wsTimeHour': $("#ddlWSTimeHour").val(),
        'wsTimeMinute': $("#ddlWSTimeMinute").val(),
        'wcTimeHour': $("#ddlWCTimeHour").val(),
        'wcTimeMinute': $("#ddlWCTimeMinute").val(),
        'bLate': bLate,
        'bEarly': bEarly,
        'lateRemark': $("#txtLateRemark").val(),
        'earlyRemark': $("#txtEarlyRemark").val(),
        'wsConn': $("#ddlWSConn").val(),
        'wcConn': $("#ddlWCConn").val(),
        'isHoli': IsHoli,
        'modReason': $("#txtModReason").val(),
        'modifyUserID': UserID
    };

    CallEncrypt(CallGetParam());
    var data = "{p:'" + GetEncryptValue + "'}";
    var obj = eval("PrivateMod.SuccessConfirm");

    AjaxService(url, data, obj);
}


/**************************************************
작성자   : kkzz52
작성일자 : 2017-11-24
작성목적 : 출퇴근시간 수정 성공 시
***************************************************/
PrivateMod.SuccessConfirm = function (data, status) {
    if (data.d == true) {
        alert(R_MSG_SUCCESS);
        parent.ListPrivateMod.GoPageLoad();
        self.close();
    }
}