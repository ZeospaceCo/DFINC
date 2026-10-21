AddReply = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    $('#btCancel').click(function (e) {
        AddReply.viewList();
    });

    $('#btAdd').click(function (e) {
        AddReply.add();
    });

    $('#before').click(function (e) {
        AddReply.goBefore();
    });

    $('#listTitle').click(function (e) {
        AddReply.viewList();
    });

});

/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 목록 페이지 이동
***************************************************/
AddReply.goBefore = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'PageNo': PageNo
        };

        CallEncrypt(CallGetParam());

        location.href = "/DeskPlusMobile/MDM/List.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'BPage': BPage,
            'PageNo': PageNo,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        if (BPage == "AL") {
            location.href = "ForwardList.aspx?p=" + GetEncryptValue;
        }
        else if (BPage == "DE") {
            location.href = "DelayList.aspx?p=" + GetEncryptValue;
        }
        else {
            location.href = "List.aspx?p=" + GetEncryptValue;
        }
    }
}


/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 취소 버튼 클릭시
***************************************************/
AddReply.viewList = function () {

    if (Alarm == "Y") {
        ParamArray = {
            'BPage': BPage,
            'BType': BType,
            'PageNo': PageNo,
            'listID': ListID,
            'groupID': GroupID,
            'Status': Status,
            'viewing': Viewing
        };

        CallEncrypt(CallGetParam());

        location.href = "AlarmView.aspx?p=" + GetEncryptValue;
    }
    else {
        ParamArray = {
            'BPage': BPage,
            'BType': BType,
            'PageNo': PageNo,
            'ListID': ListID,
            'GroupID': GroupID,
            'Status': Status,
            'ForwardID': ForwardID,
            'Sv': SearchValue.securityText()
        };

        CallEncrypt(CallGetParam());

        if (BPage == "AL") {
            location.href = "ForwardView.aspx?p=" + GetEncryptValue;
        }
        else if (BPage == "DE") {
            location.href = "DelayView.aspx?p=" + GetEncryptValue;
        }
        else {
            location.href = "View.aspx?p=" + GetEncryptValue;
        }
    }
}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 답변하기 등록 (웹서비스 호출)
***************************************************/
AddReply.add = function () {

    var tmpContent = $("#memo").val();

    if (tmpContent == "") {
        NoticeShow("!", R_InputCont);
        return;
    }

    if (BtnClickCheck != "") {
        NoticeShow("!", R_ProcessingWait);
        return;
    }

    LoadingShow(true);

    BtnClickCheck = "Y";
    var url = "/DeskPlusMobile/Task/Ws/SetTask.asmx/AddReply";

    //파라미터값 배열에 담기
    ParamArray = {
        'BType': BType,
        'ListID': ListID,
        'Status': Status,
        'Content': encodeURIComponent(tmpContent)
    };

    var obj = eval("AddReply.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-05-13
작성목적 : 답변하기 등록 (웹서비스 값 받기)
***************************************************/
AddReply.succeeded = function (data, status) {

    if (data.d) {
        //NoticeShow("*", R_Register);
        AddReply.viewList();

        LoadingShow(false);
    }
    else {
        NoticeShow("!", R_ProcessingError);
        LoadingShow(false);
    }
}