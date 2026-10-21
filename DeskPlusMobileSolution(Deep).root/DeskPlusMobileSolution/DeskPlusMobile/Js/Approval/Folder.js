Folder = {}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 이벤트 할당
***************************************************/
$(document).ready(function () {

    // 문서 카운트 가져오기
    setTimeout("Folder.getCount();", 400);

    $('#aRep').click(function (e) {
        e.preventDefault();
        Folder.goList('REP', 'List');
    });

    $('#aRel').click(function (e) {
        e.preventDefault();
        Folder.goList('REL', 'List');
    });

    $('#aApp').click(function (e) {
        e.preventDefault();
        Folder.goList('APP', 'List');
    });

    $('#aPro').click(function (e) {
        e.preventDefault();
        location.href = "Proxy.aspx";
    });

    $('#aPublic').click(function (e) {
        e.preventDefault();
        Folder.goList('Public', 'DeptList');
    });

    $('#aPrivate').click(function (e) {
        e.preventDefault();
        Folder.goList('Private', 'DocList');
    });

    $('#aApproval').click(function (e) {
        e.preventDefault();
        Folder.goList('Approval', 'DocList');
    });

    $('#aReject').click(function (e) {
        e.preventDefault();
        Folder.goList('Reject', 'DocList');
    });

    $('#aArbitrary').click(function (e) {
        e.preventDefault();
        Folder.goList('Arbitrary', 'DocList');
    });

    $('#aCarboncopy').click(function (e) {
        e.preventDefault();
        Folder.goList('Carboncopy', 'CarboncopyList');
    });

    $('#aForward').click(function (e) {
        e.preventDefault();
        Folder.goList('Forward', 'DocList');
    });

    $('#aCirRecv').click(function (e) {
        e.preventDefault();
        Folder.goList('CirRecv', 'CirDocList');
    });

    $('#aCirSend').click(function (e) {
        e.preventDefault();
        Folder.goList('CirSend', 'CirDocList');
    });

    //문서작성
    $('#aWrite').click(function (e) {
        e.preventDefault();
        location.href = "/DeskPlusMobile/Approval/WFolder.aspx";
    });

});


/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 문서 카운트 가져오기 (웹서비스 호출)
***************************************************/
Folder.getCount = function () {
    //LoadingShow(true);
    var url = "/DeskPlusMobile/Approval/Ws/GetApproval.asmx/GetCount";

    //파라미터값 배열에 담기
    ParamArray = {
        'CompanyID': CompanyID,
        'UserID': UserID
    };

    var obj = eval("Folder.succeeded");
    CallEncrypt(CallGetParam()); //배열값을 파라미터형태로 반환
    data = "{p:'" + GetEncryptValue + "'}";
    AjaxAsyncService(url, data, obj);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-24
작성목적 : 문서 카운트 가져오기 (웹서비스 값 받기)
***************************************************/
Folder.succeeded = function (data, status) {

    var tmpCount = data.d.split('^');
    $("#app").text("(" + tmpCount[0] + ")");
    $("#rep").text("(" + tmpCount[1] + ")");
    $("#rel").text("(" + tmpCount[2] + ")");
    $("#pro").text("(" + tmpCount[3] + ")");

    //LoadingShow(false);
}

/**************************************************
작성자   : famo23
작성일자 : 2011-06-27
작성목적 : 문서 리스트 보기 페이지 이동
***************************************************/
Folder.goList = function (arg, url) {

    ParamArray = {
        'BPage': arg
    };

    CallEncrypt(CallGetParam());

    location.href = url + ".aspx?p=" + GetEncryptValue;
}


