Mod = {}

/**************************************************
작성자   : freeman
작성일자 : 2011-06-21
작성목적 : 페이지 로드 이벤트 할당
***************************************************/
$(document).ready(function () {

    LoadingShow(true);

    $('#divEmail').hide();
    $('#ddlAlarmTime').hide();
    $('#divment').hide();

    var arrCalMonth = TmpDate.split('-');
    CalMonth = arrCalMonth[0] + "-" + arrCalMonth[1];

    if (ScheduleID != 0) { //일정 수정        
        jQuery.each($("#stime_set"), function () { this.style.display = ""; });
        jQuery.each($("#etime_set"), function () { this.style.display = ""; });
        jQuery.each($("#data_set"), function () { this.style.display = ""; });
        jQuery.each($("#status_set"), function () { this.style.display = "none"; });
        jQuery.each($("#priority_set"), function () { this.style.display = "none"; });
        jQuery.each($("#statusID_set"), function () { this.style.display = ""; });

        setTimeout("Mod.getSchedule();", 400);
    }
    else { //할일 수정
        jQuery.each($("#stime_set"), function () { this.style.display = "none"; });
        jQuery.each($("#etime_set"), function () { this.style.display = "none"; });
        jQuery.each($("#data_set"), function () { this.style.display = "none"; });
        jQuery.each($("#status_set"), function () { this.style.display = ""; });
        jQuery.each($("#priority_set"), function () { this.style.display = ""; });
        jQuery.each($("#statusID_set"), function () { this.style.display = "none"; });

        setTimeout("Mod.getToDo();", 400);
    }

    if (OpenID != 0) {
        jQuery.each($("#del_set"), function () { this.style.display = "none"; });
    }
    else {
        jQuery.each($("#del_set"), function () { this.style.display = ""; });
    }

    LoadingShow(false);

    $('#pvMonth').click(function (e) {
        Mod.pvMonth();
    });

    $('#nxMonth').click(function (e) {
        Mod.nxMonth();
    });

    $('#btCancel').click(function (e) {
        Mod.goBefore();
    });

    $('#btSave').click(function (e) {
        Mod.save();
    });

    $('#tbStartDate').click(function (e) {
        CalCheck = "1";
        Mod.getCalendar();
    });

    $('#tbEndDate').click(function (e) {
        CalCheck = "2";
        Mod.getCalendar();
    });

    $('#toTitle').click(function (e) {
        Mod.goBefore();
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

    $('#tbcontent').autogrow(); //받는 사람 자동 높이 조절

});



/**************************************************
작성자   : freeman
작성일자 : 2011-06-17
작성목적 : 일정 가져오기 (웹서비스 호출)
***************************************************/
Mod.getSchedule = function () {
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetScheduleContentStripHTML";

    //파라미터값 배열에 담기
    ParamArray = {
        'ScheduleID': ScheduleID,
        'DisplayID': DisplayID
    };

    var obj = eval("Mod.succeededSchedule");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-17
작성목적 : 일정 가져오기 (웹서비스 값 받기)
***************************************************/
Mod.succeededSchedule = function (data, status) {
    var schCont = eval('(' + data.d + ')');
    $("#toTitle").val(R_PimsMod);  //Title 표시
    $("input:radio[name=openType]").filter('input:radio[value=' + schCont.GetContent[0].OpenType + ']').attr("checked", "checked"); //설정 선택
    $("#tbTitle").val(schCont.GetContent[0].Subject); //제목

    var time1 = DateDeserialize(schCont.GetContent[0].StartTime);  // 자바스크립트 날짜형으로 출력
    var tmpStartTime = new Date(time1);  // 자바스크립에서 원하는 형태로 사용가능
    var time2 = DateDeserialize(schCont.GetContent[0].EndTime);  // 자바스크립트 날짜형으로 출력
    var tmpEndTime = new Date(time2);  // 자바스크립에서 원하는 형태로 사용가능

    $("#ddlstartTime").val(schCont.GetContent[0].StartShortTime);
    $("#ddlendTime").val(schCont.GetContent[0].EndShortTime);

    var tmpStartDate = Format(tmpStartTime, 'Y') + "-" + Format(tmpStartTime, 'm') + "-" + Format(tmpStartTime, 'd');
    var tmpEndDate = Format(tmpEndTime, 'Y') + "-" + Format(tmpEndTime, 'm') + "-" + Format(tmpEndTime, 'd');

    tmpStartDate = DateYMD(tmpStartDate, PreLang);
    tmpEndDate = DateYMD(tmpEndDate, PreLang);

    $("#tbStartDate").val(tmpStartDate); //날짜표시
    $("#tbEndDate").val(tmpEndDate); //날짜표시

    $("#ddlStatus").val(schCont.GetContent[0].StatusID);
    $("#tbcontent").val(schCont.GetContent[0].bodyNoStyle);

    if (schCont.GetContent[0].Alert == "Y") {

        $("#ddlnotikind").val(schCont.GetContent[0].AlertType);
        $("#ddlAlarmTime").val(schCont.GetContent[0].AlertTime);
        $("#idEmail").val(schCont.GetContent[0].AlertTargetAddress);
    }

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

}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 할일 가져오기 (웹서비스 호출)
***************************************************/
Mod.getToDo = function () {
    var url = "/DeskPlusMobile/Pims/Ws/GetPims.asmx/GetToDoContentStripHTML";

    //파라미터값 배열에 담기
    ParamArray = {
        'ToDoID': ToDoID
    };

    var obj = eval("Mod.succeededToDo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 할일 가져오기 (웹서비스 값 받기)
***************************************************/
Mod.succeededToDo = function (data, status) {
    var toDoCont = eval('(' + data.d + ')');
    $("#toTitle").html(R_ToDoMod);

    var time1 = DateDeserialize(toDoCont.GetToDoContent[0].StartDate);  // 자바스크립트 날짜형으로 출력
    var tmpStartTime = new Date(time1);  // 자바스크립에서 원하는 형태로 사용가능
    var tmpToDoDate = Format(tmpStartTime, 'Y') + "-" + Format(tmpStartTime, 'm') + "-" + Format(tmpStartTime, 'd')
    tmpToDoDate = DateYMD(tmpToDoDate, PreLang);


    $("#tbStartDate").val(tmpToDoDate); //날짜표시    
    $("#tbTitle").val(toDoCont.GetToDoContent[0].Subject); //제목
    $("input:radio[name=status]").filter('input:radio[value=' + toDoCont.GetToDoContent[0].Status + ']').attr("checked", "checked"); //진척도 선택
    $("input:radio[name=priority]").filter('input:radio[value=' + toDoCont.GetToDoContent[0].Priority + ']').attr("checked", "checked"); //중요도 선택
    $("input:radio[name=openType]").filter('input:radio[value=' + toDoCont.GetToDoContent[0].OpenType + ']').attr("checked", "checked"); //설정 선택

    $("#tbcontent").val(toDoCont.GetToDoContent[0].bodyNoStyle);
}


/**************************************************
작성자   : famo23
작성일자 : 2011-06-21
작성목적 : 일정 수정 버튼 클릭시 
***************************************************/
Mod.save = function () {
    if (ScheduleID != 0) {
        Mod.saveSchedule();
    }
    else {
        Mod.saveToDo();
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 일정 수정 (웹 서비스 호출)
***************************************************/
Mod.saveSchedule = function () {

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

    var url = "/DeskPlusMobile/Pims/Ws/SetPims.asmx/ModSchedule";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': CompanyID,
        'ScheduleID': ScheduleID,
        'Title': encodeURIComponent(title),
        'Content': encodeURIComponent(content),
        'StartDate': startDate,
        'EndDate': endDate,
        'StartTime': $("#ddlstartTime").val(),
        'EndTime': $("#ddlendTime").val(),
        'UserID': UserID,
        'OpenType': openType,
        'DisplayID': DisplayID,
        'StatusID': statusID,
        'AlarmKind': $("#ddlnotikind").val(),
        'AlarmTime': $("#ddlAlarmTime").val(),
        'AlarmMail': $("#idEmail").val()
    };

    var obj = eval("Mod.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 할일 수정
***************************************************/
Mod.saveToDo = function () {

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

    if (content.length > 140) {
        NoticeShow("!", R_InputCount);
        LoadingShow(false);
        return;
    }

    var status = $(":input:radio[name=status]:checked").val();
    var priority = $(":input:radio[name=priority]:checked").val();
    var openType = $(":input:radio[name=openType]:checked").val();

    BtnClickCheck = "Y";

    var url = "/DeskPlusMobile/Pims/Ws/SetPims.asmx/ModToDo";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': CompanyID,
        'ToDoID': ToDoID,
        'Title': encodeURIComponent(title),
        'Content': encodeURIComponent(content),
        'StartDate': startDate,
        'Status': status,
        'Priority': priority,
        'UserID': UserID,
        'OpenType': openType
    };

    var obj = eval("Mod.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}


/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 일정, 할일 추가 (웹서비스 값 받기)
***************************************************/
Mod.succeeded = function (data, status) {
    LoadingShow(false);
    Mod.goBefore();
}


/**************************************************
작성자   : famo23
작성일자 : 2011-06-20
작성목적 : 이전 페이지 이동
***************************************************/
Mod.goBefore = function () {
    ParamArray = {
        'ScheduleID': ScheduleID,
        'ToDoID': ToDoID,
        'OpenID': OpenID,
        'OpenName': OpenName,
        'OpenDate': OpenDate,
        'RealUserID': UserID,
        'DisplayID': DisplayID
    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-15
작성목적 : 달력 만들기 (웹서비스 호출)
***************************************************/
Mod.getCalendar = function () {
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

    var obj = eval("Mod.succeededCalendar");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";

    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-07-15
작성목적 : 달력 만들기 (웹서비스 값 받기)
***************************************************/
Mod.succeededCalendar = function (data, status) {
    $('#phCal').setTemplateURL('/DeskPlusMobile/Pims/Jt/JTCalendar.htm?seq=' + Guids(), null, { filter_data: false });
    $('#phCal').processTemplate(data.d);

    $('*').removeClass("busy choice");
    $('#' + CalDate).attr("class", "busy choice");

    setTimeout(function () {
        Mod.CalendarResource();
    }, 100);
}

/**********************************************************************************************
작성자   : famo23
작성일자 : 2012-04-24
작성목적 : 다국어 처리
**********************************************************************************************/
Mod.CalendarResource = function () {

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
Mod.pvMonth = function () {
    addMonth(CalMonth, -1);
}

Mod.nxMonth = function () {
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

    Mod.getCalendar();
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
