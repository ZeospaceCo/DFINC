List = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-15
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $("span.calv").click(function () {
        $("#pims_calendar").toggle();
        if ($("#pims_calendar").is(":visible")) {
            $("span.calv").html(R_Calendar + " ▲");
        } else {
            $("span.calv").html(R_Calendar + " ▼");
        }
    });

    if (OpenID != "0") {
        UserID = OpenID;
        IsOpen = "Y"
        jQuery.each($("#shareUserInfo"), function () { this.style.display = ""; });
        $("#userList").empty();
        $("#userList").html(OpenName);
    }

    NowMonth = (NowMonth) < 10 ? "0" + NowMonth : NowMonth;
    NowDay = (NowDay) < 10 ? "0" + NowDay : NowDay;
    NowDate = NowYear + "-" + NowMonth + "-" + NowDay;

    if (TmpDate == "") {
        TmpDate = NowDate;
    }

    var tmpStartDate = DateYMD(TmpDate, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpStartDate);

    //LoadingShow(true);

    // 일정가져오기
    //List.getSchedule(TmpDate);
    // 할일가져오기
    //List.getToDo(TmpDate);
    // 일정 할일 가져오기
    List.getScheduleToDoList(TmpDate);
    // 달력그리기
    List.getCalendar(TmpDate);

    //LoadingShow(false);

    $('#add').click(function (e) {
        List.add();
    });

    $('#share').click(function (e) {
        List.goList('Share');
    });

    $('#mylist').click(function (e) {
        List.goList('List');
    });

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



});

/**************************************************
작성자   : famo23
작성일자 : 2011-06-15
작성목적 : 날짜 이동
***************************************************/
List.pvMonth = function () {
    TmpDate = addMonth(TmpDate, -1);

    //List.getSchedule(TmpDate);
    //List.getToDo(TmpDate);
    List.getScheduleToDoList(TmpDate);
}

List.pvDay = function () {
    TmpDate = addDay(TmpDate, -1);

    //List.getSchedule(TmpDate);
    //List.getToDo(TmpDate);
    List.getScheduleToDoList(TmpDate);
}

List.nxMonth = function () {
    TmpDate = addMonth(TmpDate, 1);

    //List.getSchedule(TmpDate);
    //List.getToDo(TmpDate);
    List.getScheduleToDoList(TmpDate);
}

List.nxDay = function () {
    TmpDate = addDay(TmpDate, 1);

    //List.getSchedule(TmpDate);
    //List.getToDo(TmpDate);
    List.getScheduleToDoList(TmpDate);
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
작성자   : famo23
작성일자 : 2011-06-15
작성목적 : 일정 가져오기 (웹서비스 호출)
***************************************************/
List.getSchedule = function (tmpDate) {
    //arrDate = tmpDate.split('-');
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetScheduleList";

    //파라미터값 배열에 담기
    ParamArray = {
        'TmpDate': tmpDate,
        'UserID': UserID,
        'IsOpen': IsOpen
    };

    var obj = eval("List.succeededSchedule");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-15
작성목적 : 일정 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeededSchedule = function (data, status) {
    $('#phSchedule').setTemplateURL('/DeskPlusMobile/Pims/Jt/JTSchedule.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phSchedule').processTemplate(data.d);

    setTimeout(function () {
        List.scheduleResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
List.scheduleResource = function () {
    if ($("#NoScheduleListData")[0] != undefined) {
        $("#NoScheduleListData")[0].innerHTML = R_NoSchedule;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-17
작성목적 : 일정 보기
***************************************************/
List.viewSchedule = function (ScheduleID, RealUserID, DisplayID) {

    ParamArray = {
        'ScheduleID': ScheduleID,
        'OpenID': OpenID,
        'OpenName': OpenName,
        'OpenDate': TmpDate,
        'RealUserID': RealUserID,
        'DisplayID': DisplayID

    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 할일 가져오기 (웹서비스 호출)
***************************************************/
List.getToDo = function (tmpDate) {
    arrDate = tmpDate.split('-');
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetToDoList";

    //파라미터값 배열에 담기
    ParamArray = {
        'Year': arrDate[0],
        'Month': arrDate[1],
        'Day': arrDate[2],
        'UserID': UserID,
        'IsOpen': IsOpen
    };

    var obj = eval("List.succeededToDo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 할일 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeededToDo = function (data, status) {
    $('#phToDo').setTemplateURL('/DeskPlusMobile/Pims/Jt/JTToDo.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phToDo').processTemplate(data.d);

    setTimeout(function () {
        List.toDoResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-03-30
작성목적 : 다국어 처리
**********************************************************************************************/
List.toDoResource = function () {
    if ($("#NoToDoListData")[0] != undefined) {
        $("#NoToDoListData")[0].innerHTML = R_NoToDo;
    }
}

/**************************************************
작성자   : freeman
작성일자 : 2011-06-20
작성목적 : 달력 만들기 (웹서비스 호출)
***************************************************/
List.getCalendar = function (tmpDate) {
    arrDate = tmpDate.split('-');
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetCalendar";

    //파라미터값 배열에 담기
    ParamArray = {
        'Year': arrDate[0],
        'Month': arrDate[1],
        'Day': arrDate[2],
        'UserID': UserID,
        'IsOpen': IsOpen
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
    $('#phCal').setTemplateURL('/DeskPlusMobile/Pims/Jt/JTCalendar.htm?seq=' + Guids(), null, { filter_data: false });
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
작성자   : famo23
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
        //$('#' + TmpChoice).removeClass("busy choice");
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
    List.getScheduleToDoList(TmpDate);
    //List.getSchedule(tmpDate);
    //List.getToDo(tmpDate);

    var tmpTop = $("#phSchedule li:first").offset().top;

    $('html, body').animate({ scrollTop: tmpTop }, 1000);
}


/**************************************************
작성자   : famo23
작성일자 : 2011-06-17
작성목적 : 일정 보기
***************************************************/
List.viewToDo = function (ToDoID, RealUserID) {

    ParamArray = {
        'ToDoID': ToDoID,
        'OpenID': OpenID,
        'OpenName': OpenName,
        'OpenDate': TmpDate,
        'RealUserID': RealUserID
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 일정 추가
***************************************************/
List.add = function () {

    ParamArray = {
        'OpenDate': TmpDate
    };

    CallEncrypt(CallGetParam());

    location.href = "Add.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 나의 일정가기, 공유자 가기 
***************************************************/
List.goList = function (arg) {

    location.href = arg + ".aspx";
}

/**************************************************
작성자   : 이중호
작성일자 : 2019-04-30
작성목적 : 일정 할일 가져오기 (웹서비스 호출)
***************************************************/
List.getScheduleToDoList = function (tmpDate) {
    arrDate = tmpDate.split('-');
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetScheduleToDoList";

    //파라미터값 배열에 담기
    ParamArray = {
        'UserID': UserID,
        'IsOpen': IsOpen,
        'Year': arrDate[0],
        'Month': arrDate[1],
        'Day': arrDate[2],
    };

    var obj = eval("List.succeededScheduleToDo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 이중호
작성일자 : 2019-04-30
작성목적 : 일정 할일 가져오기 (웹서비스 값 받기)
***************************************************/
List.succeededScheduleToDo = function (data, status) {
    var rtnArray = JSON.parse(data.d);

    $("#phScheduleToDo").html("");
    var ScheduleYN = false;
    var ToDoYN = false;

    for (var i = 0; i < rtnArray["rtnList"].length; i++) {
        if (rtnArray["rtnList"][i]["ScheduleID"] > 0) {
            ScheduleYN = true;
            var strHtml = "";
            strHtml += "<li class='contentblock " + (rtnArray["rtnList"][i]["UserID"] == UserID ? "mylist" : "") + "' >";
            strHtml += "<div class='Mtitle linkDiv'";
            strHtml += " onclick=\"javascript:List.viewSchedule('" + rtnArray["rtnList"][i]["ScheduleID"] + "','" + rtnArray["rtnList"][i]["UserID"] + "','" + rtnArray["rtnList"][i]["DisplayID"] + "')\">";
            switch (rtnArray["rtnList"][i]["OpenType"]) {
                case "0":
                    strHtml += "<a><div class='pim_pr'>●</div>";
                    break;
                case "1":
                    strHtml += "<a><div class='pim_dept'>●</div>";
                    break;
                case "2":
                    strHtml += "<a><div class='pim_share'>●</div>";
                    break;
                case "3":
                    strHtml += "<a><div class='pim_tot'>●</div>";
                    break;
            }
            strHtml += "<div class='conBox'><span class='LNoready'>" + rtnArray["rtnList"][i]["Title"] + " " + rtnArray["rtnList"][i]["ReplyCnt"] + " </span> <br />";
            strHtml += "<span class='sch_user'>";
            if (rtnArray["rtnList"][i]["UserInfo"] != "") {
                strHtml += rtnArray["rtnList"][i]["UserInfo"] + "<br />";
            }
            strHtml += rtnArray["rtnList"][i]["OpenTypeStr"] + "&nbsp;&nbsp;" + rtnArray["rtnList"][i]["Status"] + "<span>";
            strHtml += "&nbsp;" + rtnArray["rtnList"][i]["StartTime"] + " ~ " + rtnArray["rtnList"][i]["EndTime"] + "</span></div></a></div></li>";

            $("#phScheduleToDo").append(strHtml);
        } else if (rtnArray["rtnList"][i]["ToDoID"] > 0) {
            ToDoYN = true;
            var strHtml = "";
            strHtml += "<li class='contentblock " + (rtnArray["rtnList"][i]["UserID"] == UserID ? "mylist" : "") + "' >";
            strHtml += "<div class='Mtitle linkDiv'";
            strHtml += " onclick=\"javascript:List.viewToDo('" + rtnArray["rtnList"][i]["ToDoID"] + "','" + rtnArray["rtnList"][i]["UserID"] + "')\">";
            switch (rtnArray["rtnList"][i]["OpenType"]) {
                case "0":
                    strHtml += "<a><div class='pim_pr'>●</div>";
                    break;
                case "1":
                    strHtml += "<a><div class='pim_dept'>●</div>";
                    break;
                case "2":
                    strHtml += "<a><div class='pim_share'>●</div>";
                    break;
                case "3":
                    strHtml += "<a><div class='pim_tot'>●</div>";
                    break;
            }
            strHtml += "<div class='conBox'><span class='LNoready'>" + rtnArray["rtnList"][i]["ScheduleYN"] + rtnArray["rtnList"][i]["Title"] + " " + rtnArray["rtnList"][i]["ReplyCnt"] + " </span> <br />";
            strHtml += "<span class='sch_user'>";
            if (rtnArray["rtnList"][i]["UserInfo"] != "") {
                strHtml += rtnArray["rtnList"][i]["UserInfo"] + "<br />";
            }
            strHtml += rtnArray["rtnList"][i]["OpenTypeStr"] + "&nbsp;&nbsp;<span class='bg-success text-white'>&nbsp;" + rtnArray["rtnList"][i]["Status"] + "&nbsp;</span>";
            strHtml += "&nbsp;&nbsp;" + rtnArray["rtnList"][i]["Priority"] + "</span></div></a></div></li>";

            $("#phScheduleToDo").append(strHtml);
        }
    }

    if (!ScheduleYN) {
        var strHtml = "";
        strHtml += "<li class='contentblock'><div class='title'><a href='#' class='mct'><span id='NoScheduleListData' />" + R_NoSchedule + "</a></div></li>"
        $("#phScheduleToDo").prepend(strHtml);
    }

    if (!ToDoYN) {
        var strHtml = "";
        strHtml += "<li class='contentblock'><div class='title'><a href='#' class='mct'><span id='NoToDoListData' />" + R_NoToDo + "</a></div></li>"
        $("#phScheduleToDo").append(strHtml);
    }
}