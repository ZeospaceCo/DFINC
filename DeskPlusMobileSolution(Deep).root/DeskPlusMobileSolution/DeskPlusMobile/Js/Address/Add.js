Add = {}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
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
작성자   : famo23
작성일자 : 2012-11-06
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
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 목록 페이지 이동
***************************************************/
Add.goBefore = function () {

    ParamArray = {
        'gubun': Gubun,
        'PageNo': PageNo,
        'FastPath': FastPath,
        'FirstChar': FirstChar,
        'sv': SearchValue.securityText()
    };

    CallEncrypt(CallGetParam());

    location.href = "List.aspx?p=" + GetEncryptValue;
}


/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 취소 버튼 클릭시
***************************************************/
Add.viewList = function () {

    ParamArray = {
        'gubun': Gubun,
        'ownerID': OwnerID,
        'PageNo': PageNo,
        'FastPath': FastPath,
        'FirstChar': FirstChar,
        'sv': SearchValue.securityText()

    };

    CallEncrypt(CallGetParam());

    location.href = "View.aspx?p=" + GetEncryptValue;
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 댓글 등록 (웹서비스 호출)
***************************************************/
Add.add = function () {

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

    var url = "/DeskPlusMobile/MyTalk/Ws/SetMyTalk.asmx/AddMyTalk";

    //파라미터값 배열에 담기
    ParamArray = {
        'RUserID': RUserID,
        'Content': encodeURIComponent(tmpContent),
        'IsSecurity': IsSecurity
    };

    var obj = eval("Add.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2012-11-06
작성목적 : 댓글 등록 (웹서비스 값 받기)
***************************************************/
Add.succeeded = function (data, status) {

    if (data.d) {
        Add.goBefore();

        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}