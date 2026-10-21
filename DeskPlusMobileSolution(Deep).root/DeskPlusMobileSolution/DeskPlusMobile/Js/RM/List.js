List = {}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-15
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    if (BPage == "MyReserve") {
        $('#listTitle').html("<i class='fas fa-calendar-minus  dropbtn '></i>" + R_MyReserve + " <span class='calv linkDiv btn btn-dark'>" + cal + " ▼ </span>");
    }
    else if (BPage == "Reserve") {
        $('#listTitle').html("<i class='fas fa-calendar-minus  dropbtn '></i>" + R_ResourcesReserve + " <span class='calv linkDiv btn btn-dark'>" + cal + " ▼ </span>");
    }

    $("span.calv").click(function (e) {
        e.stopPropagation();
        $("#rm_calendar").toggle();
        if ($("#rm_calendar").is(":visible")) {
            $("span.calv").html(R_Calendar + " ▲");
        } else {
            $("span.calv").html(R_Calendar + " ▼");
        }
    });

    $('#listTitle').click(function (e) {
        location.href = "Folder.aspx";
    });

    NowMonth = (NowMonth) < 10 ? "0" + NowMonth : NowMonth;
    NowDay = (NowDay) < 10 ? "0" + NowDay : NowDay;
    NowDate = NowYear + "-" + NowMonth + "-" + NowDay;

    if (TmpDate == "") {
        TmpDate = NowDate;
    }

    var tmpStartDate = DateYMD(TmpDate, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpStartDate);



    // 리스트 가져오기
    List.GetPageList(TmpDate);
    // 달력그리기
    List.getCalendar(TmpDate);

    $('#pvMonth').click(function (e) {
        List.pvMonth();
    });

    $('#pvDay').click(function (e) {
        List.pvDay();
    });

    $('#nxDay').click(function (e) {
        List.nxDay();
    });

    $('#nxMonth').click(function (e) {
        List.nxMonth();
    });

    $('#add').click(function (e) {

        List.add();
    });

});

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-15
작성목적 : 날짜 이동
***************************************************/
List.pvMonth = function () {
    TmpDate = addMonth(TmpDate, -1);

    List.GetPageList(TmpDate);
}

List.pvDay = function () {
    TmpDate = addDay(TmpDate, -1);

    List.GetPageList(TmpDate);
}

List.nxMonth = function () {
    TmpDate = addMonth(TmpDate, 1);

    List.GetPageList(TmpDate);
}

List.nxDay = function () {
    TmpDate = addDay(TmpDate, 1);

    List.GetPageList(TmpDate);
}

function addDay(tmpDate, v_day) {
    var arrDate = tmpDate.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");
    var dd = arrDate[2];
    var addDate = new Date(yyyy, mm, eval(dd + '+' + v_day));
    //var oldMm = mm;
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);
    dd = addDate.getDate() < 10 ? "0" + addDate.getDate() : addDate.getDate();

    var tmpStartDate = yyyy + "-" + mm + "-" + dd;
    tmpStartDate = DateYMD(tmpStartDate, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpStartDate);

    //if (oldMm != mm) List.getCalendar(yyyy + "-" + mm + "-" + dd); // ByFreeman 달력 변경
    List.getCalendar(yyyy + "-" + mm + "-" + dd); // ByFreeman 달력 변경
    return yyyy + "-" + mm + "-" + dd;
}

function addMonth(tmpDate, v_month) {
    var arrDate = tmpDate.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");
    var dd = arrDate[2];
    var addDate = new Date(yyyy, eval(mm + '+' + v_month), dd);
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);
    dd = addDate.getDate() < 10 ? "0" + addDate.getDate() : addDate.getDate();

    var tmpStartDate = yyyy + "-" + mm + "-" + dd;
    tmpStartDate = DateYMD(tmpStartDate, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpStartDate);

    List.getCalendar(yyyy + "-" + mm + "-" + dd); // ByFreeman 달력 변경
    return yyyy + "-" + mm + "-" + dd;
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-15
작성목적 : 예약리스트 가져오기 (웹서비스 호출)
***************************************************/
List.GetPageList = function (tmpDate) {

    var url = "";
    if (BPage == "MyReserve") {
        url = "/DeskPlusMobile/RM/Ws/GetRM.asmx/GetMyReserveList";
    }
    else {
        url = "/DeskPlusMobile/RM/Ws/GetRM.asmx/GetReserveList";
    }

    //파라미터값 배열에 담기
    ParamArray = {
        'TmpDate': tmpDate
    };

    var obj = eval("List.SuccessPageList");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-15
작성목적 : 예약리스트 가져오기 (웹서비스 값 받기)
***************************************************/
List.SuccessPageList = function (data, status) {
    var url = "";
    if (BPage == "MyReserve") {
        url = "/DeskPlusMobile/RM/Jt/JTMyReserveList.htm";
    }
    else {
        url = "/DeskPlusMobile/RM/Jt/JTReserveList.htm";
    }

    $('#phRM').setTemplateURL(url + '?seq=' + Guids(), null, { filter_data: false });
    $('#phRM').processTemplate(data.d);

    setTimeout(function () {
        List.Resource();
    }, 100);
}

/**********************************************************************************************
작성자   : 방영환
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
List.Resource = function () {
    if ($("#NoListData")[0] != undefined) {
        $("#NoListData")[0].innerHTML = R_NoContent;
    }
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-17
작성목적 : 예약자원 보기
***************************************************/
List.view = function (ReservationID) {

    ParamArray = {
        'ReservationID': ReservationID,
        'OpenDate': TmpDate,
        'BPage': BPage
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;

}



/**************************************************
작성자   : freeman
작성일자 : 2011-06-20
작성목적 : 달력 만들기 (웹서비스 호출)
***************************************************/
List.getCalendar = function (tmpDate) {
    arrDate = tmpDate.split('-');
    var url = "/DeskPlusMobile/RM/Ws/GetRM.asmx/GetCalendar";

    //파라미터값 배열에 담기
    ParamArray = {
        'Year': arrDate[0],
        'Month': arrDate[1],
        'Day': arrDate[2],
        'Gubun': BPage
    };

    var obj = eval("List.succeededCalendar");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : freeman
작성일자 : 2011-06-20
작성목적 : 달력 만들기 (웹서비스 값 받기)
***************************************************/
List.succeededCalendar = function (data, status) {
    $('#phCal').setTemplateURL('/DeskPlusMobile/RM/Jt/JTCalendar.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phCal').processTemplate(data.d);

    TmpClassName = $('#' + TmpDate).attr("class");

    if (TmpClassName.indexOf("hol") > 0) {
        $('#' + TmpDate).attr("class", "busy holchoice");
    }
    else {
        $('#' + TmpDate).attr("class", "busy choice");
    }

    TmpChoice = TmpDate;

    setTimeout(function () {
        List.CalendarResource();
    }, 100);
}

/**********************************************************************************************
작성자   : 방영환
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
List.CalendarResource = function () {

    $("#JSun")[0].innerHTML = R_Sun;
    $("#JMon")[0].innerHTML = R_Mon;
    $("#JTue")[0].innerHTML = R_Tue;
    $("#JWed")[0].innerHTML = R_Wed;
    $("#JThu")[0].innerHTML = R_Thu;
    $("#JFri")[0].innerHTML = R_Fri;
    $("#JSat")[0].innerHTML = R_Sat;
}

/**************************************************
작성자   : freeman
작성일자 : 2011-06-20
작성목적 : 달력 이동이벤트
***************************************************/
List.getList = function (tmpDate) {

    if (TmpChoice != "") {
        $('#' + TmpChoice).attr("class", TmpClassName);
    }

    TmpClassName = $('#' + tmpDate).attr("class");

    if (TmpClassName.indexOf("hol") > 0) {
        $('#' + tmpDate).attr("class", "busy holchoice");
    }
    else {
        $('#' + tmpDate).attr("class", "busy choice");
    }

    var tmpStartDate = DateYMD(tmpDate, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpStartDate);

    TmpDate = tmpDate;
    TmpChoice = tmpDate;
    List.GetPageList(tmpDate);

    var tmpTop = $("#phRM li:first").offset().top;

    $('html, body').animate({ scrollTop: tmpTop }, 1000);
}

/**************************************************
작성자   : 방영환
작성일자 : 2015-09-04
작성목적 : 자원예약
***************************************************/
List.add = function () {

    ParamArray = {
        'OpenDate': TmpDate,
        'BPage': BPage
    };

    CallEncrypt(CallGetParam());

    location.href = "Add.aspx?p=" + GetEncryptValue;
}