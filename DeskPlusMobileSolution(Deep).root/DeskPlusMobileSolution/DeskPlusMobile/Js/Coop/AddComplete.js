AddComplete = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#pvMonth').click(function (e) {
        AddComplete.pvMonth();
    });

    $('#nxMonth').click(function (e) {
        AddComplete.nxMonth();
    });

    $('#tbStartDate').click(function (e) {
        CalCheck = "1";
        AddComplete.getCalendar();
    });

    $('#tbEndDate').click(function (e) {
        CalCheck = "2";
        AddComplete.getCalendar();
    });

    $('#btCancel').click(function (e) {
        AddComplete.viewList();
    });

    $('#btAdd').click(function (e) {
        AddComplete.add();
    });

    $('#before').click(function (e) {
        AddComplete.goBefore();
    });

    $('#listTitle').click(function (e) {
        AddComplete.viewList();
    });

});

/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 목록 페이지 이동
***************************************************/
AddComplete.goBefore = function () {

    ParamArray = {
        'BPage': BPage,
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'Sv': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 취소 버튼 클릭시
***************************************************/
AddComplete.viewList = function () {

    ParamArray = {
        'BPage': BPage,
        'MoreSize': MoreSize,
        'PageNo': PageNo,
        'ListID': ListID,
        'GroupID': GroupID,
        'Status': Status,
        'Sv': SearchValue
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 완료하기 등록 (웹서비스 호출)
***************************************************/
AddComplete.add = function () {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait);
        return;
    }

    LoadingShow(true);

    var startDate = $("#tbStartDate").val();
    var endDate = $("#tbEndDate").val();

    var dtResponse = CompareDT(startDate, endDate, "lessequal", PreLang);

    if (dtResponse == false) {
        NoticeShow("!", R_StartDateEndDate);
        LoadingShow(false);
        return;
    }

    var tmpContent = $("#memo").val();


    BtnClickCheck = "Y";
    var url = "/DeskPlusMobile/Coop/Ws/SetCoop.asmx/AddComplete";

    //파라미터값 배열에 담기
    ParamArray = {
        'ListID': ListID,
        'StartDate': startDate,
        'EndDate': endDate,
        'Difficulty': $("#ddlDifficulty").val(),
        'Content': encodeURIComponent(tmpContent)
    };

    var obj = eval("AddComplete.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 완료하기 등록 (웹서비스 값 받기)
***************************************************/
AddComplete.succeeded = function (data, status) {

    if (data.d) {
        //NoticeShow("*", R_Register);
        Status = "4";
        AddComplete.viewList();

        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 달력 만들기 (웹서비스 호출)
***************************************************/
AddComplete.getCalendar = function () {
    jQuery.each($("#coop_calendar"), function () { this.style.display = ""; });

    var arrDate = "";
    var tmpTimeList = "";

    if (CalCheck == "1") {
        arrDate = StartCalMonth.split('-');
        tmpTimeList = StartCalMonth + "-01";
    }
    else {
        arrDate = EndCalMonth.split('-');
        tmpTimeList = EndCalMonth + "-01";
    }

    tmpTimeList = DateYM(tmpTimeList, PreLang);
    $("#timeList").empty();
    $("#timeList").html(tmpTimeList);

    var url = "/DeskPlusMobile/Coop/Ws/GetCoop.asmx/GetBasicCalendar";

    //파라미터값 배열에 담기
    ParamArray = {
        'Year': arrDate[0],
        'Month': arrDate[1]
    };

    var obj = eval("AddComplete.succeededCalendar");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 달력 만들기 (웹서비스 값 받기)
***************************************************/
AddComplete.succeededCalendar = function (data, status) {

    $('#phCal').setTemplateURL('/DeskPlusMobile/Coop/Jt/JTCalendar.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phCal').processTemplate(data.d);

    setTimeout(function () {
        AddComplete.CalendarResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 다국어 처리
**********************************************************************************************/
AddComplete.CalendarResource = function () {

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
작성일자 : 2015-04-03
작성목적 : 날짜 이동
***************************************************/
AddComplete.pvMonth = function () {

    if (CalCheck == "1") {
        addMonth(StartCalMonth, -1);
    }
    else {
        addMonth(EndCalMonth, -1);
    }
   
}

AddComplete.nxMonth = function () {

    if (CalCheck == "1") {
        addMonth(StartCalMonth, 1);
    }
    else {
        addMonth(EndCalMonth, 1);
    }
}


function addMonth(calMonth, v_month) {
    var arrDate = calMonth.split('-');
    var yyyy = arrDate[0];
    var mm = eval(arrDate[1] + "-1");

    var addDate = new Date(yyyy, eval(mm + '+' + v_month), 1);
    yyyy = addDate.getFullYear();
    mm = (addDate.getMonth() + 1) < 10 ? "0" + (addDate.getMonth() + 1) : (addDate.getMonth() + 1);

    var tmpTimeList = yyyy + "-" + mm + "-01";
    tmpTimeList = DateYM(tmpTimeList, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpTimeList);

    if (CalCheck == "1") {
        StartCalMonth = yyyy + "-" + mm;
    }
    else {
        EndCalMonth = yyyy + "-" + mm;
    }   

    AddComplete.getCalendar();
}


/**************************************************
작성자   : famo23
작성일자 : 2015-04-03
작성목적 : 달력 이동이벤트
***************************************************/
function setList(calDate) {

    var tmpCalDate = DateYMD(calDate, PreLang);
    var tmpTimeList = DateYM(calDate, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpTimeList);

    if (CalCheck == "1") {
        $("#tbStartDate").val(tmpCalDate);
    }
    else {
        $("#tbEndDate").val(tmpCalDate);
    }

    jQuery.each($("#coop_calendar"), function () { this.style.display = "none"; });
}
