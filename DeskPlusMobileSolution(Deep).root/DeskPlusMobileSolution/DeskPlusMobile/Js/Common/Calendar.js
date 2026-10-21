Calendar = {}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    NowMonth = (NowMonth) < 10 ? "0" + NowMonth : NowMonth;
    NowDay = (NowDay) < 10 ? "0" + NowDay : NowDay;
    NowDate = NowYear + "-" + NowMonth + "-" + NowDay;

    SetDate = NowDate;
    CalMonth = NowYear + "-" + NowMonth;

    $("#selectDate").text(SetDate);

    Calendar.getCalendar(CalMonth);

    $('#btConfirm').click(function (e) {
        e.preventDefault();
        Calendar.Confirm();
    });

    $('#btClose').click(function (e) {
        e.preventDefault();
        Calendar.Close();
    });

    $('#pvMonthS').click(function (e) {
        e.preventDefault();
        Calendar.pvMonth();
    });

    $('#nxMonthS').click(function (e) {
        e.preventDefault();
        Calendar.nxMonth();
    });

    $("#ddlYear").change(function (e) {
        e.preventDefault();
        Calendar.LoadDate();
    });

    $("#ddlMonth").change(function (e) {
        e.preventDefault();
        Calendar.LoadDate();
    });

});


/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 달력 만들기 (웹서비스 호출)
***************************************************/
Calendar.getCalendar = function (_calMonth) {
    var arrDate = _calMonth.split('-');
    var url = "/DeskPlusMobile/Common/Ws/GetCommon.asmx/GetBasicCalendar";

    //파라미터값 배열에 담기
    ParamArray = {
        'Year': arrDate[0],
        'Month': arrDate[1]
    };

    var obj = eval("Calendar.succeededCalendar");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 달력 만들기 (웹서비스 값 받기)
***************************************************/
Calendar.succeededCalendar = function (data, status) {

    $('#phCal').setTemplateURL('/DeskPlusMobile/Common/Jt/JTCalendar.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phCal').processTemplate(data.d);

    if ($('#' + SetDate).text() != "") {
        TmpClassName = $('#' + SetDate).attr("class");
        TmpChoice = SetDate;

        if (TmpClassName.indexOf("hol") > 0) {
            $('#' + SetDate).attr("class", "busy holchoice");
        }
        else {
            $('#' + SetDate).attr("class", "busy choice");
        }
    }
    else {
        TmpClassName = "";
        TmpChoice = "";
    }

    setTimeout(function () {
        Calendar.CalendarResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 다국어 처리
**********************************************************************************************/
Calendar.CalendarResource = function () {

    $("#JSun")[0].innerHTML = R_Sun;
    $("#JMon")[0].innerHTML = R_Mon;
    $("#JTue")[0].innerHTML = R_Tue;
    $("#JWed")[0].innerHTML = R_Wed;
    $("#JThu")[0].innerHTML = R_Thu;
    $("#JFri")[0].innerHTML = R_Fri;
    $("#JSat")[0].innerHTML = R_Sat;

}
/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 날짜 이동
***************************************************/
Calendar.pvMonth = function () {
    addMonth(CalMonth, -1);
}

Calendar.nxMonth = function () {
    addMonth(CalMonth, 1);
}


function addMonth(calMonth, v_month) {
    var arrDate = calMonth.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");

    var addDate = new Date(yyyy, eval(mm + '+' + v_month), 1);
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);

    CalMonth = yyyy + "-" + mm;
    $("#ddlYear").val(yyyy);
    $("#ddlMonth").val(parseInt(mm));
    Calendar.getCalendar(CalMonth);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 달력 이동이벤트
***************************************************/
Calendar.setList = function (calDate) {
    SetDate = calDate;

    $("#selectDate").text(SetDate);

    if (TmpChoice != "") {
        $('#' + TmpChoice).attr("class", TmpClassName);
    }

    TmpClassName = $('#' + SetDate).attr("class");
    TmpChoice = SetDate;

    if (TmpClassName.indexOf("hol") > 0) {
        $('#' + SetDate).attr("class", "busy holchoice");
    }
    else {
        $('#' + SetDate).attr("class", "busy choice");
    }
}


/**********************************************************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : [닫기]
**********************************************************************************************/
Calendar.Close = function () {

    if (parent.disablePopup) { parent.disablePopup(); } else { self.close(); }
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 년/월 드롭다운
***************************************************/
Calendar.LoadDate = function (arg) {

    var tmpMonth = $("#ddlMonth").val();

    tmpMonth = (tmpMonth) < 10 ? "0" + tmpMonth : tmpMonth;

    CalMonth = $("#ddlYear").val() + "-" + tmpMonth;
    Calendar.getCalendar(CalMonth);
}

/**************************************************
작성자   : famo23
작성일자 : 2019-08-21
작성목적 : 확인
***************************************************/
Calendar.Confirm = function () {

    var arrStart = $("#selectDate").text().split('-');
    parent.ifrmProcessing.GetDateUseDay("", arrStart[0], arrStart[1], arrStart[2]);

    Calendar.Close();

}

