Add = {}

/**************************************************
작성자   : famo23
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
        Add.save();
    });

    $('#rdpims_gb1').click(function (e) {
        Add.gubunPims(this);
    });

    $('#rdpims_gb2').click(function (e) {
        Add.gubunPims(this);
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

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //알람
    $('#ddlnotikind').change(function (e) {
        e.preventDefault();

        if ($('#ddlnotikind').val() == "M") //메신저
        {
            $('#divEmail').hide();
            $('#ddlAlarmTime').show();
            $('#divment').show();
        }
        if ($('#ddlnotikind').val() == "E") //EMAIL
        {
            $('#divEmail').show();
            $('#ddlAlarmTime').hide();
            $('#divment').hide();
        }
        if ($('#ddlnotikind').val() == "B") //Mobile
        {
            $('#divEmail').hide();
            $('#ddlAlarmTime').show();
            $('#divment').show();
        }
        if ($('#ddlnotikind').val() == "") //무
        {
            $('#divEmail').hide();
            $('#ddlAlarmTime').hide();
            $('#divment').hide();
        }
    });

    $('#divEmail').hide();
    $('#ddlAlarmTime').hide();
    $('#divment').hide();

    if ($('#ddlnotikind').val() == "M") {
        $('#ddlAlarmTime').show();
        $('#divment').show();
    }
    if ($('#ddlnotikind').val() == "B") {
        $('#ddlAlarmTime').show();
        $('#divment').show();
    }
    if ($('#ddlnotikind').val() == "E") {
        $('#divEmail').show();
    }

});


/**************************************************
작성자   : famo23
작성일자 : 2011-06-21
작성목적 : 구분 선택시
***************************************************/
Add.gubunPims = function (arg) {
    var tmpID = arg.id;

    if (tmpID == "rdpims_gb1") {
        jQuery.each($("#stime_set"), function () { this.style.display = ""; });
        jQuery.each($("#etime_set"), function () { this.style.display = ""; });
        jQuery.each($("#data_set"), function () { this.style.display = ""; });
        jQuery.each($("#status_set"), function () { this.style.display = "none"; });
        jQuery.each($("#priority_set"), function () { this.style.display = "none"; });
        jQuery.each($("#statusID_set"), function () { this.style.display = ""; });
    }
    else {
        jQuery.each($("#stime_set"), function () { this.style.display = "none"; });
        jQuery.each($("#etime_set"), function () { this.style.display = "none"; });
        jQuery.each($("#data_set"), function () { this.style.display = "none"; });
        jQuery.each($("#status_set"), function () { this.style.display = ""; });
        jQuery.each($("#priority_set"), function () { this.style.display = ""; });
        jQuery.each($("#statusID_set"), function () { this.style.display = "none"; });
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-21
작성목적 : 일정 추가 버튼 클릭시 
***************************************************/
Add.save = function () {
    if ($("#rdpims_gb1").is(":checked")) {
        Add.saveSchedule();
    }
    else {
        Add.saveToDo();
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 일정 추가 (웹 서비스 호출)
***************************************************/
Add.saveSchedule = function () {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait);
        return;
    }

    LoadingShow(true);

    var title = $("#tbTitle").val();
    var startDate = $("#tbStartDate").val();
    var endDate = $("#tbEndDate").val();
    var content = $("#tbcontent").val();
    var statusID = $("#ddlStatus").val();

    if (title == "") {
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

    /*
    if (ExistSpecialChar(title) == false) {
    NoticeShow("!", R_Security);
    LoadingShow(false);
    return;
    }
    */

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

    if ($('#ddlnotikind').val() == "M") //메신저
    {
        if ($("#ddlAlarmTime").val() == "") {
            action_popup.alert(R_AlarmAlert);
            return;
        }
    }
    if ($('#ddlnotikind').val() == "B") //Mobile
    {
        if ($("#ddlAlarmTime").val() == "") {
            action_popup.alert(R_AlarmAlert);
            return;
        }
    }
    if ($('#ddlnotikind').val() == "E") //메일
    {
        if ($.trim($('#idEmail').val()) != "") {
            if (!checkEmail($('#idEmail').val())) {
                action_popup.alert(R_CheckMail);
                return;
            }
        }
    }

    var openType = $(":input:radio[name=openType]:checked").val();

    BtnClickCheck = "Y";

    var url = "/DeskPlusMobile/Pims/Ws/SetPims.asmx/AddSchedule";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': CompanyID,
        'Title': encodeURIComponent(title),
        'Content': encodeURIComponent(content),
        'StartDate': startDate,
        'EndDate': endDate,
        'StartTime': $("#ddlstartTime").val(),
        'EndTime': $("#ddlendTime").val(),
        'UserID': UserID,
        'OpenType': openType,
        'DeptID': DeptID,
        'StatusID': statusID,
        'AlarmKind': $("#ddlnotikind").val(),
        'AlarmTime': $("#ddlAlarmTime").val(),
        'AlarmMail': $("#idEmail").val()
    };

    var obj = eval("Add.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 할일 추가
***************************************************/
Add.saveToDo = function () {

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait);
        return;
    }

    LoadingShow(true);

    var title = $("#tbTitle").val();
    var startDate = $("#tbStartDate").val();
    var content = $("#tbcontent").val();

    if (title == "") {
        NoticeShow("!", R_InputSubject);
        LoadingShow(false);
        return;
    }

    if (startDate == "") {
        NoticeShow("!", R_InputDate);
        LoadingShow(false);
        return;
    }

    if (content.length > 140) {
        NoticeShow("!", R_InputCount);
        LoadingShow(false);
        return;
    }

    /*
    if (ExistSpecialChar(title) == false) {
    NoticeShow("!", R_Security);
    LoadingShow(false);
    return;
    }
    */

    var status = $(":input:radio[name=status]:checked").val();
    var priority = $(":input:radio[name=priority]:checked").val();
    var openType = $(":input:radio[name=openType]:checked").val();

    BtnClickCheck = "Y";

    var url = "/DeskPlusMobile/Pims/Ws/SetPims.asmx/AddToDo";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': CompanyID,
        'Title': encodeURIComponent(title),
        'Content': encodeURIComponent(content),
        'StartDate': startDate,
        'Status': status,
        'Priority': priority,
        'UserID': UserID,
        'OpenType': openType,
        'DeptID': DeptID
    };

    var obj = eval("Add.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 일정, 할일 추가 (웹서비스 값 받기)
***************************************************/
Add.succeeded = function (data, status) {
    LoadingShow(false);
    TmpDate = CalDate;
    Add.goBefore();
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 일정 취소
***************************************************/
Add.goBefore = function () {

    ParamArray = {
        'OpenDate': TmpDate
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-15
작성목적 : 달력 만들기 (웹서비스 호출)
***************************************************/
Add.getCalendar = function () {
    jQuery.each($("#pims_calendar"), function () { this.style.display = ""; });

    //arrDate = CalDate.split('-');
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
작성자   : famo23
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
작성자   : famo23
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
작성자   : famo23
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
작성자   : famo23
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

/**************************************************
작성자   : famo23
작성일자 : 2021-03-11
작성목적 : 메일 정규식 체크 
***************************************************/
function checkEmail(mailValue) {

    var arrMail = mailValue.split(';');

    for (var i = 0; i < arrMail.length; i++) {

        if ($.trim(arrMail[i]) != "") {
            var email = arrMail[i];
            var exptext = /^[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_\.]?[0-9a-zA-Z])*\.[a-zA-Z]{2,3}$/i;
            if (exptext.test(email) == false) {
                return false;
            };
        }
    }

    return true;
};
