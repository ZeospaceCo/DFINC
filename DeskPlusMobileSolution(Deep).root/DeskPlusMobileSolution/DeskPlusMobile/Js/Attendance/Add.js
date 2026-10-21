Add = {}

/**************************************************
작성자   : 이지원
작성일자 : 2018-05-04
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    $('#btCancel').click(function (e) {
        Add.goHome();
    });

    $('#GotoWork').click(function (e) {
        Add.GotoWork();
    });

    $('#OffWork').click(function (e) {
        Add.OffWork();
    });

    AttendanceInit();
});

/**************************************************
작성자   : 이지원
작성일자 : 2018-05-04
작성목적 : 초기화
***************************************************/
function AttendanceInit() {    
    var array_rtn = rtnAttendanceData.split("|");

    bWS = array_rtn[0];
    WSTime = array_rtn[1];
    bWC = array_rtn[2];
    WCTime = array_rtn[3];

    if (NowHour < 18) {
        document.getElementById("OffWork").innerText = R_Attendance_Early;
    }

    if (bWS == "1") {
        $("#StateWork").css("color", "blue");
        document.getElementById("StateWork").innerText = R_Attendance_Onduty;

        document.getElementById("GotoWork").className = "btn btn-lg btn-outline-light";
        document.getElementById("attendanceGotoWork").innerText = WSTime;
    }
    else {
        document.getElementById("OffWork").className = "btn btn-lg btn-outline-light";
    }
  
    if (bWC == "1") {
        $("#StateWork").css("color", "red");
        document.getElementById("StateWork").innerText = R_Attendance_OffWork;

        document.getElementById("attendanceOffWork").innerText = WCTime;
    }


}

/**************************************************
작성자   : 이지원
작성일자 : 2018-05-04
작성목적 : 출근
***************************************************/
Add.GotoWork = function () {

    var reason = document.getElementById("reason").value;
    var hr = document.getElementById("hr").innerText;
    var minute = document.getElementById("minute").innerText;
    var period = document.getElementById("period").innerText;

    if (bWS == "1") return;

    if ((parseInt(hr) > 8 && parseInt(minute) > 0 && period == "AM") || period == "PM") {
        if (reason == "") {
            alert(R_Attendance_Reason);
            return;
        }
    }

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait);
        return;
    }

    LoadingShow(true);

    BtnClickCheck = "Y";

    var url = "/DeskPlusMobile/Attendance/Ws/SetAttendance.asmx/SetGotoWork";
    ParamArray = { 'reason': reason }

    var obj = eval("Add.GotoWorkSucceeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-05-04
작성목적 : 출근 성공시
***************************************************/
Add.GotoWorkSucceeded = function (data, status) {

    if (data.d) {
        LoadingShow(false);
        document.location.href = "Add.aspx";
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-05-04
작성목적 : 퇴근
***************************************************/
Add.OffWork = function () {
    var reason = document.getElementById("reason").value;
    var hr = document.getElementById("hr").innerText;
    var period = document.getElementById("period").innerText;

    if (bWS != "1") return;

    if (bWC != "1") {
        if (!confirm(R_Attendance_OffWorkConfirm)) {
            return;
        }
    }
    else {
        if (!confirm(R_Attendance_OffWorkRe)) {
            return;
        }
    }

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait);
        return;
    }

    if (period == "AM" || (period == "PM" && parseInt(hr) < 6) || (period == "PM" && parseInt(hr) == 12)) {
        if (reason == "") {
            alert(R_Attendance_Reason);
            return;
        }
    }

    LoadingShow(true);

    BtnClickCheck = "Y";

    var url = "/DeskPlusMobile/Attendance/Ws/SetAttendance.asmx/SetOffWork";
    ParamArray = { 'reason': reason }

    var obj = eval("Add.OffWorkSucceeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-05-04
작성목적 : 출근 성공시
***************************************************/
Add.OffWorkSucceeded = function (data, status) {

    if (data.d) {
        LoadingShow(false);
        document.location.href = "Add.aspx";
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

/**************************************************
작성자   : 이지원
작성일자 : 2018-05-04
작성목적 : 목록 페이지 이동
***************************************************/
Add.goHome = function () {
    document.location.href = "/DeskPlusMobile/Portal/Portal.aspx";
}
