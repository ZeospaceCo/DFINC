Add = {}

/**************************************************
작성자   : kkzz52
작성일자 : 2011-07-13
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btCancel').click(function (e) {
        Add.viewList();
    });

    $('#btAdd').click(function (e) {
        Add.add();
    });

    $('#before').click(function (e) {
        Add.goBefore();
    });

    $('#listTitle').click(function (e) {
        Add.viewList();
    });

    //경고창 닫기
    $('.modal_close').click(function (e) {
        e.preventDefault();
        action_popup.close(this);
    });

    //입력값 체크하기

    Add.MaxLang();
});

/**************************************************
작성자   : kkzz52
작성일자 : 2012-07-12
작성목적 : 글자 제한
***************************************************/
Add.MaxLang = function () {

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
작성자   : kkzz52
작성일자 : 2011-07-13
작성목적 : 목록 페이지 이동
***************************************************/
Add.goBefore = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'PCatID': PCatID,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "List.aspx?p=" + GetEncryptValue;
    }
}


/**************************************************
작성자   : kkzz52
작성일자 : 2011-07-13
작성목적 : 취소 버튼 클릭시
***************************************************/
Add.viewList = function () {
    if (Alarm == "Y") {
        ParamArray = {
            'categoryID': CID,
            'listID': ID,
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "AlarmView.aspx?p=" + GetEncryptValue + "&PageNo=" + PageNo;
    }
    else {
        ParamArray = {
            'PCatID': PCatID,
            'PageNo': PageNo,
            'CID': CID,
            'ID': ID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        location.href = "View.aspx?p=" + GetEncryptValue;
    }
}

/**************************************************
작성자   : kkzz52
작성일자 : 2011-07-13
작성목적 : 댓글 등록 (웹서비스 호출)
***************************************************/
Add.add = function () {

    var tmpContent = $("#memo").val();
    tmpContent = tmpContent.split("\n").join("<br>");

    if (tmpContent == "") {
        NoticeShow("!", R_InputComment);
        return;
    }

    LoadingShow(true);

    /*
    if (ExistSpecialChar(tmpContent) == false) {
    NoticeShow("!", R_Security);
    LoadingShow(false);
    return;
    }
    */

    var url = "/DeskPlusMobile/Board/Ws/SetBoard.asmx/AddShortReplyReplys";

    //파라미터값 배열에 담기
    ParamArray = {
        'CID': CID,
        'ID': ID,
        'Content': encodeURIComponent(tmpContent),
        'companyID': companyID,
        'shortReplyID': ShortReplyID
    };

    var obj = eval("Add.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : kkzz52
작성일자 : 2011-07-13
작성목적 : 댓글 등록 (웹서비스 값 받기)
***************************************************/
Add.succeeded = function (data, status) {

    if (data.d) {
        //NoticeShow("*", R_Register);
        Add.viewList();

        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}