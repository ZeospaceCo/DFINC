Add = {}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
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

});

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 목록 페이지 이동
***************************************************/
Add.goBefore = function () {

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
작성일자 : 2015-04-02
작성목적 : 취소 버튼 클릭시
***************************************************/
Add.viewList = function () {

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
작성일자 : 2015-04-02
작성목적 : 답변하기 등록 (웹서비스 호출)
***************************************************/
Add.add = function () {

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
    var url = "/DeskPlusMobile/Coop/Ws/SetCoop.asmx/AddReply";

    //파라미터값 배열에 담기
    ParamArray = {
        'BPage': BPage,
        'ListID': ListID,
        'Status':Status,
        'Content': encodeURIComponent(tmpContent)
    };

    var obj = eval("Add.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2015-04-02
작성목적 : 답변하기 등록 (웹서비스 값 받기)
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