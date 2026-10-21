Attendance = {}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    $('#btCancel').click(function (e) {
        Attendance.goHome();
    });

    // 출근
    $('#GotoWork').click(function (e) {
        Attendance.GotoWork();
    });

    // 조퇴
    $('#HROffWork').click(function (e) {
        Attendance.OffWork();
    });

    // 조퇴취소
    $('#btnEarlyCancel').click(function (e) {
        Attendance.EarlyCancel();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    AttendanceInit();

    if (late == "1") {
        Attendance.GotoWork();
    }

    if (early == "1") {
        Attendance.OffWork();
    }

});

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 초기화
***************************************************/
function AttendanceInit() {
    var h = NowHour;
    var m = parseInt(NowMinute);
    var mm = m < 10 ? "0" + m : String(m);
    var tNowTime = parseInt(h + mm); //현재 시분 200 (새벽2시)
    var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900
    var tWCTime = parseInt(bWCTimeHour + bWCTimeMinute); //정규 퇴근시간 시분 1800

    var realWCTime = parseInt(WCTimeHour + WCTimeMinute); //실제 퇴근시간 시분 1800

    // 현재시각이 출근시간 이상이고 정규 퇴근시간 이전일때 조퇴로 표시
    if (tNowTime >= tWSTime && tNowTime < tWCTime) {
        document.getElementById("HROffWork").innerText = Attendance_Early; //조퇴
    }

    // 출근시간이 등록되어 있을 경우
    if (bWS == "1") {
        document.getElementById("attendanceGotoWork").innerText = WSTime; //출근시간
        document.getElementById("GotoWork").className = "btn btn-lg btn-outline-light";
    }
    else {
        document.getElementById("HROffWork").className = "btn btn-lg btn-outline-light";
    }

    // 퇴근시간이 등록되어 있을 경우
    if (bWC == "1") {
        // 퇴근시간이 정규 퇴근시간 이전이면 조퇴이고 조퇴라인 표시해줌
        if (realWCTime < tWCTime) {
            document.getElementById("trEarlyTime").style.display = "";
            document.getElementById("spEarlyTime").innerText = WCTime;
        }
        else {
            document.getElementById("spOffWork").style.display = "";
            document.getElementById("attendanceOffWork").innerText = WCTime;
        }
    }

    // 근무중 표시
    if (bWS == "1") {
        if (bWC == "0") {
            $("#StateWork").css("color", "blue");
            document.getElementById("StateWork").innerText = R_Attendance_Onduty; //근무중
        }
        if (bWC == "1") {
            // 현재시간이 퇴근(조퇴)시간 이전일때만 근무중 표시
            if (tNowTime < realWCTime) {
                $("#StateWork").css("color", "blue");
                document.getElementById("StateWork").innerText = R_Attendance_Onduty; //근무중
            }
            else {
                $("#StateWork").css("color", "red");

                // 퇴근시간이 정규 퇴근시간 이전이면 조퇴이고 조퇴
                if (realWCTime < tWCTime) {
                    document.getElementById("StateWork").innerText = Attendance_Early; //조퇴
                }
                else {
                    document.getElementById("StateWork").innerText = Attendance_OffWork; //퇴근
                }
            }
        }
    }

    // 주간 연장근무시간
    document.getElementById("spOverTime").innerText = OverTime;
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 출근
***************************************************/
Attendance.GotoWork = function () {

    var reason = document.getElementById("reason").value;

    //var h = NowHour;
    //var m = parseInt(NowMinute);
    var d = new Date();
    var h = d.getHours();
    var m = parseInt(d.getMinutes());
    var mm = m < 10 ? "0" + m : String(m);
    var tNowTime = parseInt(h + mm); //현재 시분 915
    var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900

    if (isNaN(tNowTime)) return;

    if (tWSTime == "0") return;

    if (tNowTime >= tWSTime) {
        if (bWS == "1") return;

        var bLate = "0";

        if (tNowTime > tWSTime) {
            if (jQuery.trim(reason) == "") {
                NoticeShow("!", Attendance_Reason); // 사유를 입력 하시기 바랍니다.
                return;
            }

            bLate = "1";
        }

        if (BtnClickCheck != "") {
            NoticeShow("!", R_ProcessingWait);
            return;
        }

        LoadingShow(true);

        BtnClickCheck = "Y";

        var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetGotoWork";
        ParamArray = { 'reason': reason, 'isHoli': isHoli, 'bLate': bLate }

        var obj = eval("Attendance.GotoWorkSucceeded");
        CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
        data = "{p:'" + GetEncryptValue + "'}";
        AjaxAsyncService(url, data, obj);

    }
    else {
        var bLate = "0";
        reason = "";

        if (bWS == "1") {
            action_popup.confirm(Attendance_GotoWorkRe, function (res) { // 출근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
                if (res) {
                    if (BtnClickCheck != "") {
                        NoticeShow("!", R_ProcessingWait);
                        return;
                    }

                    LoadingShow(true);

                    BtnClickCheck = "Y";

                    var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetGotoWork";
                    ParamArray = { 'reason': reason, 'isHoli': isHoli, 'bLate': bLate }

                    var obj = eval("Attendance.GotoWorkSucceeded");
                    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                    data = "{p:'" + GetEncryptValue + "'}";
                    AjaxAsyncService(url, data, obj);
                }
            })
        }
        else {
            if (BtnClickCheck != "") {
                NoticeShow("!", R_ProcessingWait);
                return;
            }

            LoadingShow(true);

            BtnClickCheck = "Y";

            var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetGotoWork";
            ParamArray = { 'reason': reason, 'isHoli': isHoli, 'bLate': bLate }

            var obj = eval("Attendance.GotoWorkSucceeded");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    }

}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 출근 성공시
***************************************************/
Attendance.GotoWorkSucceeded = function (data, status) {

    if (data.d != null && data.d.length > 0) {
        LoadingShow(false);

        var ip = data.d.split('^')[0];
        var time = data.d.split('^')[1];
        var location = R_LocationOut; //사외

        if (ip == "IN") {
            location = R_LocationIn; //사내
        }

        //alert(R_GotoSucceed + "\n" + R_GotoTime + ": " + time + "\n" + R_Location + ": " + location); //"출근 처리되었습니다.\n출근시간: " + time + "\n위치: " + location

        document.location.href = "Attendance.aspx";
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 퇴근
***************************************************/
Attendance.OffWork = function () {
    //var h = NowHour;
    //var m = parseInt(NowMinute);
    var d = new Date();
    var h = d.getHours();
    var m = parseInt(d.getMinutes());
    var mm = m < 10 ? "0" + m : String(m);
    var tNowTime = parseInt(h + mm); //현재 시분 200 (새벽2시)
    var tWSTime = parseInt(bWSTimeHour + bWSTimeMinute); //정규 출근시간 시분 900
    var tWCTime = parseInt(bWCTimeHour + bWCTimeMinute); //정규 퇴근시간 시분 1800

    // 현재시각이 출근시간 전이면 이전일로 처리한다. (00:00AM ~ 출근시간 사이)
    if (tNowTime < tWSTime) {
        PreAttendanceLoad();
    }
    else {

        var reason = document.getElementById("reason").value;
        var hr = document.getElementById("hr").innerText;
        var period = document.getElementById("period").innerText;

        if (bWS != "1") {
            NoticeShow("!", Attendance_NotGotoWork); // 출근 상태가 아닙니다.
            return;
        }

        var bEarly = "0"; //조퇴 상태값
        if (tNowTime < tWCTime) { //조퇴
            if (bWC == "1") {
                return;
            }

            document.getElementById("trEarly").style.display = "";
            document.getElementById("trEarlyTime").style.display = "none";

            if (jQuery.trim(reason) == "") {
                NoticeShow("!", Attendance_Reason); // 사유를 입력 하시기 바랍니다.
                return;
            }

            if (document.getElementById("ddlEarly").value == "0") {
                NoticeShow("!", Attendance_EarlyTimeMsg); // 조퇴 예정시간을 선택하세요.
                return;
            }

            action_popup.confirm(Attendance_EarlyConfirm, function (res) { // 조퇴 처리 하시겠습니까?
                if (res) {
                    Attendance.OffWorkSet(reason, "1");
                }
            })
        }
        else {
            if (bWC != "1") {
                action_popup.confirm(Attendance_OffWorkConfirm, function (res) { // 퇴근 처리 하시겠습니까?
                    if (res) {
                        Attendance.OffWorkSet("", "0");
                    }
                })
            }
            else {
                action_popup.confirm(Attendance_OffWorkRe, function (res) { // 퇴근 처리 하시겠습니까?\n처리시 현재 시간으로 다시 저장됩니다.
                    if (res) {
                        Attendance.OffWorkSet("", "0");
                    }
                })
            }
        }
    }
}
/**************************************************
작성목적 : 퇴근 호출 
***************************************************/
Attendance.OffWorkSet = function (_reason, _bEarly) {

    var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetOffWork";
    ParamArray = { 'reason': _reason, 'isHoli': isHoli, 'ddlEarly': document.getElementById("ddlEarly").value, 'bEarly': _bEarly }

    var obj = eval("Attendance.OffWorkSucceeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 출근 성공시
***************************************************/
Attendance.OffWorkSucceeded = function (data, status) {

    if (data.d != null && data.d.length > 0) {
        LoadingShow(false);

        var ip = data.d.split('^')[0];
        var time = data.d.split('^')[1];
        var early = data.d.split('^')[2];
        var location = R_LocationOut; //사외

        if (ip == "IN") {
            location = R_LocationIn; //사내
        }

        /*
        if (early == "1") {
        alert(R_EarlySucceed + "\n" + R_EarlyTime + ": " + time + "\n" + R_Location + ": " + location); //"조퇴 처리되었습니다.\n조퇴시간: " + time + "\n위치: " + location
        }
        else {
        alert(R_OffWorkSucceed + "\n" + R_OffWorkTime + ": " + time + "\n" + R_Location + ": " + location); //"퇴근 처리되었습니다.\n퇴근시간: " + time + "\n위치: " + location
        }
        */
        document.location.href = "Attendance.aspx";
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

// 이전일 퇴근시간 등록
function PreAttendanceLoad() {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait);
        return;
    }

    LoadingShow(true);

    BtnClickCheck = "Y";

    var url = "/DeskPlusMobile/HR/Ws/GetAttendance.asmx/GetPreAttendanceData";
    ParamArray = {}

    var obj = eval("PreAttendance_SuccessList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

function PreAttendance_SuccessList(data, status) {

    if (data.d) {
        LoadingShow(false);
        BtnClickCheck = "";
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
        return;
    }

    var rtn = data.d;

    var array_rtn = rtn.split("|");

    var Pre_bWS = array_rtn[0]; // 전날 출근여부
    var Pre_bWC = array_rtn[2]; // 전날 퇴근여부
    var Pre_isHoli = array_rtn[4]; // 전날 휴일여부
    var Pre_Date = array_rtn[5]; //전날 날짜

    if (Pre_bWS != "1") // 전날 출근여부
    {
        NoticeShow("!", Pre_Date + " " + Attendance_NotGotoWork); // 출근 상태가 아닙니다.
        return;
    }
    else {
        if (Pre_bWC != "1") // 전날 퇴근여부
        {
            action_popup.confirm(Pre_Date + " " + Attendance_OffWorkConfirm, function (res) {
                if (res) {
                    var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetPreOffWork";
                    ParamArray = { 'reason': "", 'isHoli': Pre_isHoli }

                    var obj = eval("PreOffWork_SuccessList");
                    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                    data = "{p:'" + GetEncryptValue + "'}";
                    AjaxAsyncService(url, data, obj);
                }
            })
        }
        else {
            action_popup.confirm(Pre_Date + " " + Attendance_OffWorkRe, function (res) {
                if (res) {

                    var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/SetPreOffWork";
                    ParamArray = { 'reason': "", 'isHoli': Pre_isHoli }

                    var obj = eval("PreOffWork_SuccessList");
                    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
                    data = "{p:'" + GetEncryptValue + "'}";
                    AjaxAsyncService(url, data, obj);
                }
            })
        }
    }
}

function PreOffWork_SuccessList(data, status) {
    if (data.d != null && data.d.length > 0) {
        LoadingShow(false);

        var ip = data.d.split('^')[0];
        var time = data.d.split('^')[1];
        var location = R_LocationOut; //사외

        if (ip == "IN") {
            location = R_LocationIn; //사내
        }

        //alert(R_OffWorkSucceed + "\n" + R_OffWorkTime + ": " + time + "\n" + R_Location + ": " + location); //"퇴근 처리되었습니다.\n퇴근시간: " + time + "\n위치: " + location

        document.location.href = "Attendance.aspx";
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

// 조퇴 취소
Attendance.EarlyCancel = function () {
    action_popup.confirm(Attendance_EarlyCancelConfirm, function (res) {
        if (res) {

            if (BtnClickCheck != "") {
                NoticeShow("!", R_ProcessingWait);
                return;
            }

            LoadingShow(true);

            BtnClickCheck = "Y";

            var url = "/DeskPlusMobile/HR/Ws/SetAttendance.asmx/DelOffWork";
            ParamArray = {}

            var obj = eval("Attendance.EarlyCancelSucceeded");
            CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
            data = "{p:'" + GetEncryptValue + "'}";
            AjaxAsyncService(url, data, obj);
        }
    })
}

Attendance.EarlyCancelSucceeded = function (data, status) {
    if (data.d) {
        LoadingShow(false);
        document.location.href = "Attendance.aspx";
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2018-12-06
작성목적 : 목록 페이지 이동
***************************************************/
Attendance.goHome = function () {
    document.location.href = "/DeskPlusMobile/Portal/Portal.aspx";
}
