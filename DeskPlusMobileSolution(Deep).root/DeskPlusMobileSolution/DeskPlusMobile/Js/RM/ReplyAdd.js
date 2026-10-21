ReplyAdd = {}

/**************************************************
작성자   : 방영환
작성일자 : 2012-11-07
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btCancel').click(function (e) {
        ReplyAdd.view();
    });

    $('#btAdd').click(function (e) {
        ReplyAdd.add();
    });

    $('#before').click(function (e) {
        ReplyAdd.goBefore();
    });

    $('#listTitle').click(function (e) {
        ReplyAdd.view();
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
작성자   : 방영환
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
작성자   : 방영환
작성일자 : 2012-11-07
작성목적 : 리스트 페이지 이동
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
        var url = "List.aspx";
        if (BPage == "Usable0") {
            url = "UsableList.aspx";
        }


        ParamArray = {
            'PageNo': PageNo,
            'Sv': SearchValue,
            'OpenDate': TmpDate,
            'BPage': BPage
        };

        CallEncrypt(CallGetParam());

        location.href = url + "?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : 방영환
작성일자 : 2012-11-07
작성목적 : 자원예약 보기
***************************************************/
ReplyAdd.view = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'reservationID': ReservationID,
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "AlarmView.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'ReservationID': ReservationID,
            'OpenDate': TmpDate,
            'PageNo': PageNo,
            'Sv': SearchValue,
            'BPage': BPage
        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : 방영환
작성일자 : 2012-11-07
작성목적 : 자원예약 댓글 등록 (웹서비스 호출)
***************************************************/
ReplyAdd.add = function () {

    var tmpContent = $("#memo").val();

    if (tmpContent == "") {
        NoticeShow("!", R_InputComment);
        return;
    }

    LoadingShow(true);

    var url = "/DeskPlusMobile/RM/Ws/SetRM.asmx/AddReply";

    //파라미터값 배열에 담기
    ParamArray = {
        'ReservationID': ReservationID,
        'ParentReservationID': ParentReservationID,
        'Content': encodeURIComponent(tmpContent)
    };

    var obj = eval("ReplyAdd.succeededAdd");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : 방영환
작성일자 : 2012-11-07
작성목적 : 자원예약 댓글 등록 (웹서비스 값 받기)
***************************************************/
ReplyAdd.succeededAdd = function (data, status) {

    if (data.d) {
        ReplyAdd.view();
        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}

