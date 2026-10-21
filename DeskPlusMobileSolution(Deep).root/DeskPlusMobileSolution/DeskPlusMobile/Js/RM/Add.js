Add = {}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-21
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {
    var arrCalMonth = TmpDate.split('-');
    CalMonth = arrCalMonth[0] + "-" + arrCalMonth[1];

    $('#pvMonth').click(function (e) {
        Add.pvMonth();
    });

    $('#nxMonth').click(function (e) {
        Add.nxMonth();
    });

    $('#btCancel').click(function (e) {
        Add.goBefore();
    });

    $('#btAdd').click(function (e) {
        Add.confirm();
    });

    $('#tbStartDate').click(function (e) {
        CalCheck = "1";
        Add.getCalendar();
    });

    $('#tbEndDate').click(function (e) {
        CalCheck = "2";
        Add.getCalendar();
    });

    $('#listTitle').click(function (e) {
        Add.goBefore();
    });

    $('#ddlList').change(function () {
        Add.setRequestQty();
    });

    Add.setRequestQty();

});



/**************************************************
작성자   : 방영환
작성일자 : 2011-06-21
작성목적 : 자원예약 저장 버튼 클릭시 
***************************************************/
Add.confirm = function () {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait);
        return;
    }

    LoadingShow(true);

    var subject = $("#tbSubject").val();
    var startDate = $("#tbStartDate").val();
    var endDate = $("#tbEndDate").val();
    var content = $("#tbContent").val();
    var listID = $("#ddlList").val().split('_')[0];
    var qty = $("#ddlQty").val();


    if (subject == "") {
        NoticeShow("!", R_InputSubject);
        LoadingShow(false);
        return;
    }

    if ((startDate == "") || (endDate == "")) {
        NoticeShow("!", R_InputDate);
        LoadingShow(false);
        return;
    }

    if (content.length > 140) {
        NoticeShow("!", R_InputCount);
        LoadingShow(false);
        return;
    }

    var sTime = $("#ddlstartTime option").index($("#ddlstartTime option:selected"));
    var eTime = $("#ddlendTime option").index($("#ddlendTime option:selected"));

    var dtResponse = CompareDT(startDate, endDate, "lessequal", PreLang);

    if (dtResponse == false) {
        NoticeShow("!", R_StartDateEndDate);
        LoadingShow(false);
        return;
    }

    var dtResponse1 = CompareDT(startDate, endDate, "equal", PreLang);
    if (dtResponse1 == true) {
        if (eTime <= sTime) {
            NoticeShow("!", R_StartDateEndDiffer);
            LoadingShow(false);
            return
        }
    }

    if (listID == 0) {
        NoticeShow("!", R_SelectResources);
        LoadingShow(false);
        return;
    }

    if (qty == "0") {
        NoticeShow("!", R_QtyZero);
        LoadingShow(false);
        return;
    }

    //자원을 사용할 수 있는지 확인
    Add.useResource();
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-06-24
작성목적 : 자원을 사용할 수 있는 지 확인 (웹 서비스 호출)
***************************************************/
Add.useResource = function () {
    var url = "/DeskPlusMobile/RM/Ws/GetRM.asmx/UseResource";

    var getStartDate = $("#tbStartDate").val();
    var getEndDate = $("#tbEndDate").val();
    var getSTime = $("#ddlstartTime").val();
    var getETime = $("#ddlendTime").val();

    var listID = $("#ddlList").val().split('_')[0]; //자원ID
    var usableQty = $("#ddlList").val().split('_')[1];  //예약가능수량
    var qty = $("#ddlQty").val();       //예약수량

    //파라미터값 배열에 담기
    ParamArray = {
        'listID': listID,
        'qty': qty,
        'usableQty': usableQty,
        'startDate': getStartDate,
        'endDate': getEndDate,
        'sTime': getSTime,
        'eTime': getETime
    };

    var obj = eval("Add.successUseResource");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**********************************************************************************************
작성자   : 방영환
작성일자 : 2012-02-16
작성목적 : 자원을 사용할 수 있는 지 확인 (웹서비스 값 받기)
**********************************************************************************************/
Add.successUseResource = function (data, status) {
    if (data.d) {
        Add.save();
    }
    else {
        NoticeShow("!", R_UsableResource);
        LoadingShow(false);
        return;
    }
}


/**************************************************
작성자   : 방영환
작성일자 : 2011-06-24
작성목적 : 자원예약 (웹 서비스 호출)
***************************************************/
Add.save = function () {
    var url = "/DeskPlusMobile/RM/Ws/SetRM.asmx/Add";

    var subject = $("#tbSubject").val();
    var content = $("#tbContent").val();
    var getStartDate = $("#tbStartDate").val();
    var getEndDate = $("#tbEndDate").val();
    var getSTime = $("#ddlstartTime").val();
    var getETime = $("#ddlendTime").val();
    var isGuard = $("#ddlGuard").val();
    var listID = $("#ddlList").val().split('_')[0]; //자원ID    
    var qty = $("#ddlQty").val();       //예약수량
    var authority = $("#ddlList").val().split('_')[2];

    //파라미터값 배열에 담기
    ParamArray = {
        'subject': subject,
        'content': content,
        'startDate': getStartDate,
        'endDate': getEndDate,
        'sTime': getSTime,
        'eTime': getETime,
        'isGuard': isGuard,
        'listID': listID,
        'qty': qty,
        'authority': authority
    };


    var obj = eval("Add.success");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**********************************************************************************************
작성자   : 방영환
작성일자 : 2012-02-16
작성목적 : 자원예약 (웹서비스 값 받기)
**********************************************************************************************/
Add.success = function (data, status) {

    LoadingShow(false);
    TmpDate = CalDate;
    Add.goBefore();
}


/**************************************************
작성자   : 방영환
작성일자 : 2011-06-24
작성목적 : 자원예약 취소
***************************************************/
Add.goBefore = function () {

    var url = "List.aspx";
    if (BPage == "Folder") {
        url = "Folder.aspx";
    }

    ParamArray = {
        'OpenDate': TmpDate,
        'BPage': BPage
    };

    CallEncrypt(CallGetParam());

    location.href = url + "?p=" + GetEncryptValue;
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-07-15
작성목적 : 달력 만들기 (웹서비스 호출)
***************************************************/
Add.getCalendar = function () {
    jQuery.each($("#pims_calendar"), function () { this.style.display = ""; });

    arrDate = CalMonth.split('-');
    var tmpTimeList = CalMonth + "-01";
    tmpTimeList = DateYM(tmpTimeList, PreLang);

    $("#timeList").empty();
    $("#timeList").html(tmpTimeList);

    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetBasicCalendar";

    //파라미터값 배열에 담기
    ParamArray = {
        'Year': arrDate[0],
        'Month': arrDate[1]
    };

    var obj = eval("Add.succeededCalendar");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-07-15
작성목적 : 달력 만들기 (웹서비스 값 받기)
***************************************************/
Add.succeededCalendar = function (data, status) {
    $('#phCal').setTemplateURL('/DeskPlusMobile/Pims/Jt/JTCalendar.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phCal').processTemplate(data.d);

    $('*').removeClass("busy choice");
    $('#' + CalDate).attr("class", "busy choice");

    setTimeout(function () {
        Add.CalendarResource();
    }, 100);
}

/**********************************************************************************************
작성자   : 방영환
작성일자 : 2012-04-24
작성목적 : 다국어 처리
**********************************************************************************************/
Add.CalendarResource = function () {

    $("#JSun")[0].innerHTML = R_Sun;
    $("#JMon")[0].innerHTML = R_Mon;
    $("#JTue")[0].innerHTML = R_Tue;
    $("#JWed")[0].innerHTML = R_Wed;
    $("#JThu")[0].innerHTML = R_Thu;
    $("#JFri")[0].innerHTML = R_Fri;
    $("#JSat")[0].innerHTML = R_Sat;
}

/**************************************************
작성자   : 방영환
작성일자 : 2011-07-15
작성목적 : 날짜 이동
***************************************************/
Add.pvMonth = function () {
    addMonth(CalMonth, -1);
}

Add.nxMonth = function () {
    addMonth(CalMonth, 1);
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

    CalMonth = yyyy + "-" + mm;

    Add.getCalendar();
}


/**************************************************
작성자   : 방영환
작성일자 : 2011-07-15
작성목적 : 달력 이동이벤트
***************************************************/
setList = function (calDate) {

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

    jQuery.each($("#pims_calendar"), function () { this.style.display = "none"; });
}


/**********************************************************************************************
작성자   : 방영환
작성일자 : 2012-02-15
작성목적 : 예약 수량
**********************************************************************************************/
Add.setRequestQty = function () {
    var tmpQty = 0;
    if ($("#ddlList").val().split('_').length >= 2) {
        tmpQty = parseInt($("#ddlList").val().split('_')[1]);
    }

    $('#ddlQty').empty();

    if (tmpQty == 0) {
        $('#ddlQty').append("<option value='0'>0</option>");
        return;
    }

    for (var i = 1; i < tmpQty + 1; i++) {
        $('#ddlQty').append("<option value='" + i + "' >" + i + "</option>");
    }
}
