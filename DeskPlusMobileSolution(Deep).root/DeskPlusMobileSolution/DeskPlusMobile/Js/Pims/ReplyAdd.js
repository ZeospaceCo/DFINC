ReplyAdd = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btCancel').click(function (e) {
        if (ScheduleID != 0) {
            ReplyAdd.viewSchedule();
        }
        else {
            ReplyAdd.viewToDo();
        }
    });

    $('#btAdd').click(function (e) {
        if (ScheduleID != 0) {
            ReplyAdd.addSchedule();
        }
        else {
            ReplyAdd.addToDo();
        }
    });

    $('#before').click(function (e) {
        ReplyAdd.goBefore();
    });

    $('#listTitle').click(function (e) {
        if (ScheduleID != 0) {
            ReplyAdd.viewSchedule();
        }
        else {
            ReplyAdd.viewToDo();
        }
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //입력값 체크하기

    ReplyAdd.MaxLang();
});

/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 글자 제한
***************************************************/
ReplyAdd.MaxLang = function () {

    var onEditCallback = function (remaining) {

        $('.charsRemaining').text(remaining);

        if (remaining > 0) {
            $(this).css('border', '1px solid #d2d2d2');
        }
    }

    var onLimitCallback = function () {
        $(this).css('border', '1px solid #f00');
    }

    $('textarea[maxlength]').limitMaxlength({
        onEdit: onEditCallback,
        onLimit: onLimitCallback
    });
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 목록 페이지 이동
***************************************************/
ReplyAdd.goBefore = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'OpenID': OpenID,
            'OpenName': OpenName,
            'OpenDate': OpenDate
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 일정 보기
***************************************************/
ReplyAdd.viewSchedule = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'ScheduleID': ScheduleID,
            'DisplayID': DisplayID,
            'ToDoID': ToDoID,
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "AlarmView.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'ScheduleID': ScheduleID,
            'OpenID': OpenID,
            'OpenName': OpenName,
            'OpenDate': OpenDate,
            'RealUserID': RealUserID,
            'DisplayID': DisplayID

        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 일정 보기
***************************************************/
ReplyAdd.viewToDo = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'ScheduleID': ScheduleID,
            'DisplayID': DisplayID,
            'ToDoID': ToDoID,
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "AlarmView.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'ToDoID': ToDoID,
            'OpenID': OpenID,
            'OpenName': OpenName,
            'OpenDate': OpenDate,
            'RealUserID': RealUserID
        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 일정 댓글 등록 (웹서비스 호출)
***************************************************/
ReplyAdd.addSchedule = function () {

    var tmpContent = $("#memo").val();

    if (tmpContent == "") {
        NoticeShow("!", R_InputComment);
        return;
    }

    var IsSecurity = "1";
    if ($("#cbOpen").is(':checked')) {
        IsSecurity = "0";
    }

    LoadingShow(true);

    var url = "/DeskPlusMobile/Pims/Ws/SetPims.asmx/AddScheduleReply";

    //파라미터값 배열에 담기
    ParamArray = {
        'ScheduleID': ScheduleID,
        'DisplayID': DisplayID,
        'Content': encodeURIComponent(tmpContent),
        'IsSecurity': IsSecurity
    };

    var obj = eval("ReplyAdd.succeededSchedule");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 일정 댓글 등록 (웹서비스 값 받기)
***************************************************/
ReplyAdd.succeededSchedule = function (data, status) {

    if (data.d) {
        ReplyAdd.viewSchedule();

        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 할일 댓글 등록 (웹서비스 호출)
***************************************************/
ReplyAdd.addToDo = function () {

    var tmpContent = $("#memo").val();

    if (tmpContent == "") {
        NoticeShow("!", R_InputComment);
        return;
    }

    var IsSecurity = "1";
    if ($("#cbOpen").is(':checked')) {
        IsSecurity = "0";
    }

    LoadingShow(true);


    var url = "/DeskPlusMobile/Pims/Ws/SetPims.asmx/AddToDoReply";

    //파라미터값 배열에 담기
    ParamArray = {
        'ToDoID': ToDoID,
        'Content': encodeURIComponent(tmpContent),
        'IsSecurity': IsSecurity
    };

    var obj = eval("ReplyAdd.succeededToDo");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-07
작성목적 : 할일 댓글 등록 (웹서비스 값 받기)
***************************************************/
ReplyAdd.succeededToDo = function (data, status) {

    if (data.d) {
        ReplyAdd.viewToDo();

        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}